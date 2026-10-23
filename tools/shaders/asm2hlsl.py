#!/usr/bin/env python3
"""Translate a D3D11 shader disassembly (3Dmigoto `cmd_Decompiler -d`, <n>.asm) into an HLSL body that keeps the
bytecode's bit-exact semantics, for generate.py.

Why not 3Dmigoto's own HLSL (-D): it keeps every temp register as float4 and converts values between float and int at
each instruction, which is wrong for the integer code fxc emits (measured on URP's Forward+ light loop in
LCArt/Character/shader_DE_Character_Normal, blob 50):
  - `and r6.w, r6.w, r8.x` (a bit mask AND) comes out as `r6.w = r6.w ? r8.x : 0;`
  - `ushr r4.w, l(-1), r4.w` comes out as `-1 >> (uint)r4.w`, an arithmetic shift that stays -1
  - `utof r2.w, r2.w; ushr r2.w, r2.w, l(23)` (firstbitlow through the float exponent) shifts the converted value,
    not its bits
  - a 32-bit mask stored as a float value loses every bit below the top 24.
The light loop then never clears its mask bit and spins: on the GPU the frame is lost (the driver kills the context,
the render target keeps whatever memory it had), on llvmpipe the editor hangs.

Here every temp is a uint4 holding the raw 32-bit pattern, as in the bytecode; each instruction reinterprets its sources
in its own domain (float/int/uint) with asfloat/asint and stores asuint of the result. Only the declarations and the
function signature come from 3Dmigoto's HLSL (its patched VDREG signature, texture/sampler/cbuffer declarations);
generate.py then names cbN/tN/sN as before.

    translate(asm_text, decompiled_hlsl_text) -> HLSL text with the 3Dmigoto declarations and a new main() body
Raises Unsupported for an instruction it does not know; generate.py then keeps 3Dmigoto's body and reports it.
"""
import re
import struct


class Unsupported(Exception):
    pass


SWZ = 'xyzw'

FLOAT_OPS = {'add', 'mul', 'mad', 'div', 'min', 'max', 'frc', 'round_ni', 'round_pi', 'round_ne', 'round_z', 'sqrt',
             'rsq', 'log', 'exp', 'rcp', 'deriv_rtx', 'deriv_rty', 'deriv_rtx_coarse', 'deriv_rty_coarse',
             'deriv_rtx_fine', 'deriv_rty_fine', 'dp2', 'dp3', 'dp4'}
FLOAT_CMP = {'ge': '>=', 'lt': '<', 'eq': '==', 'ne': '!='}
INT_CMP = {'ige': '>=', 'ilt': '<', 'ieq': '==', 'ine': '!='}
UINT_CMP = {'uge': '>=', 'ult': '<'}
BINOP = {'add': '+', 'mul': '*', 'div': '/', 'iadd': '+', 'and': '&', 'or': '|', 'xor': '^'}
FUNC1 = {'frc': 'frac', 'round_ni': 'floor', 'round_pi': 'ceil', 'round_ne': 'round', 'round_z': 'trunc',
         'sqrt': 'sqrt', 'rsq': 'rsqrt', 'log': 'log2', 'exp': 'exp2', 'rcp': 'rcp',
         'deriv_rtx': 'ddx', 'deriv_rty': 'ddy', 'deriv_rtx_coarse': 'ddx_coarse', 'deriv_rty_coarse': 'ddy_coarse',
         'deriv_rtx_fine': 'ddx_fine', 'deriv_rty_fine': 'ddy_fine'}
TEX_COORDS = {'texture1d': 1, 'texture2d': 2, 'texture3d': 3, 'texturecube': 3, 'texture1darray': 2,
              'texture2darray': 3, 'texturecubearray': 4, 'texture2dms': 2}


def vec(t, n):
    return t if n == 1 else '%s%d' % (t, n)


def lit_bits(tok):
    """Bits of one literal as the disassembler prints it: 1.000000 is a float, 1 / -1 / 0x0000ffff are integers."""
    tok = tok.strip()
    if re.fullmatch(r'[-+]?0x[0-9a-fA-F]+', tok):
        return int(tok, 16) & 0xffffffff
    if re.fullmatch(r'[-+]?\d+', tok):
        return int(tok) & 0xffffffff
    low = tok.lower()
    if low in ('inf', '+inf', '1.#inf00'):
        return 0x7f800000
    if low in ('-inf', '-1.#inf00'):
        return 0xff800000
    if 'nan' in low or '#qnan' in low or '#ind' in low:
        return 0x7fc00000
    return struct.unpack('<I', struct.pack('<f', float(tok)))[0]


def bits_float_text(b):
    f = struct.unpack('<f', struct.pack('<I', b))[0]
    if f != f or f in (float('inf'), float('-inf')):
        return 'asfloat(0x%08xu)' % b
    s = '%.9g' % f
    if 'e' not in s and '.' not in s:
        s += '.0'
    # keep the exact bits when 9 significant digits do not round-trip (denormals)
    if struct.unpack('<I', struct.pack('<f', float(s)))[0] != b:
        return 'asfloat(0x%08xu)' % b
    return s


def int_text(b):
    v = b - (1 << 32) if b & 0x80000000 else b
    return str(v) if v != -2147483648 else '(-2147483647 - 1)'


class Operand:
    """One source or destination operand: kind r/v/o/x/cb/icb/l/t/s/null/oDepth, plus modifiers and swizzle."""

    def __init__(self, text):
        self.text = text = text.strip()
        self.neg = False
        self.abs = False
        if text.startswith('-'):
            self.neg = True
            text = text[1:].strip()
        if text.startswith('|') and text.endswith('|'):
            self.abs = True
            text = text[1:-1].strip()
        self.lits = None
        self.swz = ''
        self.index = None
        if text == 'null':
            self.kind = 'null'
            return
        m = re.fullmatch(r'l\((.*)\)', text)
        if m:
            self.kind = 'l'
            vals = [lit_bits(v) for v in m.group(1).split(',')]
            self.lits = vals if len(vals) == 4 else [vals[0]] * 4
            return
        m = re.fullmatch(r'(oDepth|oDepthGE|oDepthLE|oMask|vCoverage)(?:\.([xyzw]+))?', text)
        if m:
            self.kind = m.group(1)
            self.swz = m.group(2) or 'x'
            return
        m = re.fullmatch(r'(cb\d+|icb|x\d+)\[([^\]]+)\](?:\.([xyzw]+))?', text)
        if m:
            self.kind = 'cb' if m.group(1).startswith('cb') else ('icb' if m.group(1) == 'icb' else 'x')
            self.name = m.group(1)
            self.index = m.group(2).strip()
            self.swz = m.group(3) or 'xyzw'
            return
        m = re.fullmatch(r'([rvotsu])(\d+)(?:\.([xyzw]+))?', text)
        if m:
            self.kind = m.group(1)
            self.name = m.group(1) + m.group(2)
            self.reg = int(m.group(2))
            self.swz = m.group(3) or ('xyzw' if m.group(1) != 's' else '')
            return
        raise Unsupported('operand %r' % self.text)

    def comps(self, positions):
        """Source components feeding the destination components at `positions` (0..3)."""
        s = self.swz
        if len(s) == 1:
            return s * len(positions)
        if len(s) == 4:
            return ''.join(s[p] for p in positions)
        # short swizzle (rare in the disassembly): positional, last component repeated
        return ''.join(s[min(p, len(s) - 1)] for p in positions)


class Translator:
    def __init__(self, asm, header):
        self.asm = asm
        self.in_types = {}   # 'v3' -> 'float' / 'uint' / 'int' / 'bool'
        self.out_types = {}  # 'o0' -> base type, also 'oDepth'
        self.res_dim = {}    # 't0' -> 'texture2d'
        self.temps = 0
        self.xdecl = []
        self.icb = None
        self.out = []
        self.depth = 1
        for m in re.finditer(r'(out\s+)?(float|uint|int|bool|min16float|half)(\d?)\s+(v\d+|o\d+|oDepth)\s*:', header):
            (self.out_types if m.group(1) else self.in_types)[m.group(4)] = m.group(2)

    # ---- source reading
    def src(self, op, positions, dom):
        """Expression of operand `op` read for destination `positions`, in domain dom (float/int/uint)."""
        n = len(positions)
        if op.kind == 'l':
            vals = [op.lits[p] for p in positions]
            if dom == 'float':
                parts = [bits_float_text(b) for b in vals]
                e = parts[0] if n == 1 else '%s(%s)' % (vec('float', n), ', '.join(parts))
            elif dom == 'int':
                parts = [int_text(b) for b in vals]
                e = parts[0] if n == 1 else '%s(%s)' % (vec('int', n), ', '.join(parts))
            else:
                parts = ['0x%08xu' % b for b in vals]
                e = parts[0] if n == 1 else '%s(%s)' % (vec('uint', n), ', '.join(parts))
            return self.mods(op, e, dom, literal=True)
        c = op.comps(positions)
        if op.kind == 'r':
            raw = '%s.%s' % (op.name, c)
            e = {'float': 'asfloat(%s)', 'int': 'asint(%s)', 'uint': '%s'}[dom] % raw
        elif op.kind == 'x':
            raw = '%s[%s].%s' % (op.name, op.index, c)
            e = {'float': 'asfloat(%s)', 'int': 'asint(%s)', 'uint': '%s'}[dom] % raw
        elif op.kind == 'icb':
            raw = 'icb[%s].%s' % (op.index, c)
            e = {'float': 'asfloat(%s)', 'int': 'asint(%s)', 'uint': '%s'}[dom] % raw
        elif op.kind == 'cb':
            # generate.py replaces cbN[...] with the named parameter; vd_as* is a no-op on a parameter of that type
            e = 'vd_as%s(%s[%s].%s)' % (dom, op.name, op.index, c)
        elif op.kind == 'v':
            t = self.in_types.get(op.name, 'float')
            raw = '%s.%s' % (op.name, c)
            if t == 'bool':
                raw = '((%s)%s ? 0xffffffffu : 0u)' % (vec('bool', n), raw)
                e = {'float': 'asfloat(%s)', 'int': 'asint(%s)', 'uint': '%s'}[dom] % raw
            else:
                e = 'vd_as%s(%s)' % (dom, raw)
        elif op.kind == 'vCoverage':
            e = 'vd_as%s(vCoverage)' % dom
        else:
            raise Unsupported('source operand %s' % op.text)
        return self.mods(op, e, dom)

    @staticmethod
    def mods(op, e, dom, literal=False):
        if op.abs:
            if dom != 'float':
                raise Unsupported('abs modifier on %s operand' % dom)
            e = 'abs(%s)' % e
        if op.neg:
            e = '(-(%s))' % e
        return e

    # ---- destination writing
    def write(self, dst, value, dom, sat=False):
        """Assign `value` (an expression in domain dom, width = len(dst.swz)) to dst."""
        if dst.kind == 'null':
            return
        n = len(dst.swz)
        if sat:
            if dom != 'float':
                value = 'asfloat(%s)' % self.to_uint(value, dom)
                dom = 'float'
            value = 'saturate(%s)' % value
        if dst.kind in ('o', 'oDepth', 'oDepthGE', 'oDepthLE', 'oMask'):
            name = dst.name if dst.kind == 'o' else dst.kind
            t = self.out_types.get(name, 'uint' if dst.kind == 'oMask' else 'float')
            lhs = name if dst.kind != 'o' else '%s.%s' % (name, dst.swz)
            if t in ('float', 'half', 'min16float'):
                rhs = value if dom == 'float' else 'asfloat(%s)' % self.to_uint(value, dom)
            elif t == 'int':
                rhs = value if dom == 'int' else 'asint(%s)' % (value if dom == 'uint' else self.to_uint(value, dom))
            else:
                rhs = self.to_uint(value, dom)
            self.emit('%s = %s;' % (lhs, rhs))
            return
        if dst.kind == 'r':
            lhs = '%s.%s' % (dst.name, dst.swz)
        elif dst.kind == 'x':
            lhs = '%s[%s].%s' % (dst.name, dst.index, dst.swz)
        else:
            raise Unsupported('destination %s' % dst.text)
        self.emit('%s = %s;' % (lhs, self.to_uint(value, dom)))

    @staticmethod
    def to_uint(value, dom):
        return value if dom == 'uint' else 'asuint(%s)' % value

    def emit(self, s):
        self.out.append('  ' * self.depth + s)

    # ---- instructions
    def positions(self, dst):
        return [SWZ.index(c) for c in dst.swz] if dst.kind != 'null' else [0]

    def inst(self, opc, ops):
        sat = opc.endswith('_sat')
        if sat:
            opc = opc[:-4]
        base = opc.split('(')[0]
        if base in ('loop',):
            self.emit('while (true) {')
            self.depth += 1
            return
        if base in ('endloop', 'endif'):
            self.depth -= 1
            self.emit('}')
            return
        if base == 'else':
            self.depth -= 1
            self.emit('} else {')
            self.depth += 1
            return
        if base in ('if_nz', 'if_z'):
            c = self.src(ops[0], [0], 'uint')
            self.emit('if (%s %s 0u) {' % (c, '!=' if base == 'if_nz' else '=='))
            self.depth += 1
            return
        if base == 'break':
            self.emit('break;')
            return
        if base == 'continue':
            self.emit('continue;')
            return
        if base in ('breakc_nz', 'breakc_z', 'continuec_nz', 'continuec_z', 'discard_nz', 'discard_z', 'retc_nz', 'retc_z'):
            c = self.src(ops[0], [0], 'uint')
            what = {'breakc': 'break;', 'continuec': 'continue;', 'discard': 'discard;', 'retc': 'return;'}[base.rsplit('_', 1)[0]]
            self.emit('if (%s %s 0u) %s' % (c, '!=' if base.endswith('_nz') else '==', what))
            return
        if base == 'ret':
            self.emit('return;')
            return
        if base == 'nop':
            return

        if base == 'sincos':
            dsin, dcos, s = ops
            for d, f in ((dsin, 'sin'), (dcos, 'cos')):
                if d.kind != 'null':
                    p = self.positions(d)
                    self.write(d, '%s(%s)' % (f, self.src(s, p, 'float')), 'float', sat)
            return
        if base in ('imul', 'umul'):
            hi, lo, a, b = ops
            if hi.kind != 'null':
                raise Unsupported('%s high result' % base)
            dom = 'int' if base == 'imul' else 'uint'
            p = self.positions(lo)
            self.write(lo, '(%s * %s)' % (self.src(a, p, dom), self.src(b, p, dom)), dom, sat)
            return
        if base == 'udiv':
            q, r, a, b = ops
            for d, o in ((q, '/'), (r, '%')):
                if d.kind != 'null':
                    p = self.positions(d)
                    self.write(d, '(%s %s %s)' % (self.src(a, p, 'uint'), o, self.src(b, p, 'uint')), 'uint', sat)
            return

        dst = ops[0]
        srcs = ops[1:]
        p = self.positions(dst)
        n = len(p)

        if base == 'mov':
            dom = 'float' if (sat or any(s.neg or s.abs for s in srcs)) else 'uint'
            self.write(dst, self.src(srcs[0], p, dom), dom, sat)
            return
        if base == 'movc':
            c = self.src(srcs[0], p, 'uint')
            dom = 'float' if (sat or any(s.neg or s.abs for s in srcs[1:])) else 'uint'
            a, b = self.src(srcs[1], p, dom), self.src(srcs[2], p, dom)
            self.write(dst, '((%s != 0u) ? %s : %s)' % (c, a, b), dom, sat)
            return
        if base in ('add', 'mul', 'div'):
            self.write(dst, '(%s %s %s)' % (self.src(srcs[0], p, 'float'), BINOP[base], self.src(srcs[1], p, 'float')), 'float', sat)
            return
        if base == 'mad':
            a, b, c = (self.src(s, p, 'float') for s in srcs)
            self.write(dst, '(%s * %s + %s)' % (a, b, c), 'float', sat)
            return
        if base in ('min', 'max'):
            self.write(dst, '%s(%s, %s)' % (base, self.src(srcs[0], p, 'float'), self.src(srcs[1], p, 'float')), 'float', sat)
            return
        if base in FUNC1:
            self.write(dst, '%s(%s)' % (FUNC1[base], self.src(srcs[0], p, 'float')), 'float', sat)
            return
        if base in ('dp2', 'dp3', 'dp4'):
            k = int(base[2])
            q = list(range(k))
            e = 'dot(%s, %s)' % (self.src(srcs[0], q, 'float'), self.src(srcs[1], q, 'float'))
            if n > 1:
                e = '((%s)%s)' % (vec('float', n), e)
            self.write(dst, e, 'float', sat)
            return
        for table, dom in ((FLOAT_CMP, 'float'), (INT_CMP, 'int'), (UINT_CMP, 'uint')):
            if base in table:
                e = '(%s %s %s)' % (self.src(srcs[0], p, dom), table[base], self.src(srcs[1], p, dom))
                # true is all bits set, as in the bytecode
                self.write(dst, '(-(%s)%s)' % (vec('int', n), e), 'int', sat)
                return
        if base in ('iadd',):
            self.write(dst, '(%s + %s)' % (self.src(srcs[0], p, 'int'), self.src(srcs[1], p, 'int')), 'int', sat)
            return
        if base == 'imad':
            a, b, c = (self.src(s, p, 'int') for s in srcs)
            self.write(dst, '(%s * %s + %s)' % (a, b, c), 'int', sat)
            return
        if base == 'umad':
            a, b, c = (self.src(s, p, 'uint') for s in srcs)
            self.write(dst, '(%s * %s + %s)' % (a, b, c), 'uint', sat)
            return
        if base in ('imin', 'imax'):
            self.write(dst, '%s(%s, %s)' % (base[1:], self.src(srcs[0], p, 'int'), self.src(srcs[1], p, 'int')), 'int', sat)
            return
        if base in ('umin', 'umax'):
            self.write(dst, '%s(%s, %s)' % (base[1:], self.src(srcs[0], p, 'uint'), self.src(srcs[1], p, 'uint')), 'uint', sat)
            return
        if base == 'ineg':
            self.write(dst, '(-%s)' % self.src(srcs[0], p, 'int'), 'int', sat)
            return
        if base == 'not':
            self.write(dst, '(~%s)' % self.src(srcs[0], p, 'uint'), 'uint', sat)
            return
        if base in ('and', 'or', 'xor'):
            self.write(dst, '(%s %s %s)' % (self.src(srcs[0], p, 'uint'), BINOP[base], self.src(srcs[1], p, 'uint')), 'uint', sat)
            return
        if base in ('ishl', 'ushr', 'ishr'):
            # the shift count uses its low 5 bits; a signed left operand makes >> arithmetic (both sides int, so HLSL
            # does not promote the expression to uint)
            sh = srcs[1]
            if sh.kind == 'l':
                cnt = [sh.lits[i] & 31 for i in p]
                cnt_e = str(cnt[0]) if len(set(cnt)) == 1 else '%s(%s)' % (vec('int', n), ', '.join(map(str, cnt)))
            else:
                cnt_e = '(%s)(%s & 31u)' % (vec('int', n), self.src(sh, p, 'uint'))
            if base == 'ishr':
                self.write(dst, '(%s >> %s)' % (self.src(srcs[0], p, 'int'), cnt_e), 'int', sat)
            else:
                self.write(dst, '(%s %s (%s)%s)' % (self.src(srcs[0], p, 'uint'), '<<' if base == 'ishl' else '>>', vec('uint', n), cnt_e), 'uint', sat)
            return
        if base == 'ftoi':
            self.write(dst, '((%s)%s)' % (vec('int', n), self.src(srcs[0], p, 'float')), 'int', sat)
            return
        if base == 'ftou':
            self.write(dst, '((%s)%s)' % (vec('uint', n), self.src(srcs[0], p, 'float')), 'uint', sat)
            return
        if base == 'itof':
            self.write(dst, '((%s)%s)' % (vec('float', n), self.src(srcs[0], p, 'int')), 'float', sat)
            return
        if base == 'utof':
            self.write(dst, '((%s)%s)' % (vec('float', n), self.src(srcs[0], p, 'uint')), 'float', sat)
            return
        if base in ('sample', 'sample_b', 'sample_l', 'sample_d', 'sample_c', 'sample_c_lz', 'ld'):
            self.texop(base, dst, srcs, p, sat)
            return
        raise Unsupported('instruction %s' % opc)

    def texop(self, base, dst, srcs, p, sat):
        addr, res = srcs[0], srcs[1]
        if res.kind != 't':
            raise Unsupported('%s on %s' % (base, res.text))
        dim = self.res_dim.get(res.name)
        if dim not in TEX_COORDS:
            raise Unsupported('%s on %s resource %s' % (base, dim, res.name))
        k = TEX_COORDS[dim]
        q = list(range(k))
        rsw = ''.join(res.swz[i] for i in p) if len(res.swz) == 4 else res.comps(p)
        if base == 'ld':
            if dim != 'texture2d':
                raise Unsupported('ld on %s' % dim)
            a = self.src(addr, [0, 1, 3], 'int')
            e = '%s.Load(%s)' % (res.name, a)
        else:
            smp = srcs[2]
            sname = '%s_s' % smp.name
            c = self.src(addr, q, 'float')
            if base == 'sample':
                e = '%s.Sample(%s, %s)' % (res.name, sname, c)
            elif base == 'sample_b':
                e = '%s.SampleBias(%s, %s, %s)' % (res.name, sname, c, self.src(srcs[3], [0], 'float'))
            elif base == 'sample_l':
                e = '%s.SampleLevel(%s, %s, %s)' % (res.name, sname, c, self.src(srcs[3], [0], 'float'))
            elif base == 'sample_d':
                kd = {'texture2d': 2, 'texture3d': 3, 'texturecube': 3, 'texture2darray': 2}.get(dim, k)
                qd = list(range(kd))
                e = '%s.SampleGrad(%s, %s, %s, %s)' % (res.name, sname, c, self.src(srcs[3], qd, 'float'), self.src(srcs[4], qd, 'float'))
            else:
                fn = 'SampleCmp' if base == 'sample_c' else 'SampleCmpLevelZero'
                e = '%s.%s(%s, %s, %s)' % (res.name, fn, sname, c, self.src(srcs[3], [0], 'float'))
                self.write(dst, '((%s)%s)' % (vec('float', len(p)), e) if len(p) > 1 else e, 'float', sat)
                return
        self.write(dst, '%s.%s' % (e, rsw), 'float', sat)

    def run(self):
        lines = self.asm.splitlines()
        i = 0
        body = []
        while i < len(lines):
            ln = lines[i].split('//')[0].strip()
            i += 1
            if not ln:
                continue
            if re.fullmatch(r'(vs|ps|gs|hs|ds|cs)_\d_\d', ln):
                continue
            if ln.startswith('dcl_immediateConstantBuffer'):
                txt = ln
                while txt.count('{') != txt.count('}') and i < len(lines):
                    txt += lines[i].strip()
                    i += 1
                rows = re.findall(r'\{\s*([^{}]+?)\s*\}', txt)
                self.icb = [[lit_bits(v) for v in r.split(',')] for r in rows]
                continue
            if ln.startswith('dcl_temps'):
                self.temps = int(ln.split()[1])
                continue
            if ln.startswith('dcl_indexableTemp'):
                m = re.fullmatch(r'dcl_indexableTemp\s+(x\d+)\[(\d+)\],\s*(\d+)', ln)
                if not m:
                    raise Unsupported(ln)
                self.xdecl.append((m.group(1), int(m.group(2))))
                continue
            m = re.match(r'dcl_resource_(\w+)\s*(?:\([^)]*\))?\s+(t\d+)', ln)
            if m:
                self.res_dim[m.group(2)] = m.group(1)
                continue
            if ln.startswith('dcl_'):
                continue
            body.append(ln)
        for ln in body:
            m = re.match(r'(\S+(?:\([^)]*\))*)\s*(.*)$', ln)
            opc, rest = m.group(1), m.group(2)
            opc = re.sub(r'_indexable\([^)]*\)(\([^)]*\))?', '', opc)
            ops = [Operand(t) for t in split_operands(rest)] if rest else []
            self.inst(opc, ops)
        if self.depth != 1:
            raise Unsupported('unbalanced control flow')
        decl = []
        if self.temps:
            decl.append('  uint4 %s;' % ', '.join('r%d' % k for k in range(self.temps)))
            # every temp starts at 0, so a component read before its first write is defined
            decl.append('  %s' % ' '.join('r%d = 0u;' % k for k in range(self.temps)))
        for name, size in self.xdecl:
            decl.append('  uint4 %s[%d];' % (name, size))
        if self.icb is not None:
            rows = ['uint4(%s)' % ', '.join('0x%08xu' % b for b in (r + [0] * 4)[:4]) for r in self.icb]
            decl.append('  const uint4 icb[%d] = { %s };' % (len(rows), ', '.join(rows)))
        return decl + [''] + self.out


def split_operands(s):
    out, depth, cur = [], 0, ''
    for ch in s:
        if ch in '([':
            depth += 1
        elif ch in ')]':
            depth -= 1
        if ch == ',' and depth == 0:
            out.append(cur)
            cur = ''
        else:
            cur += ch
    if cur.strip():
        out.append(cur)
    return out


def translate(asm_text, hlsl_text):
    """3Dmigoto HLSL (declarations + signature) with its body replaced by the bit-exact translation of asm_text."""
    lines = hlsl_text.splitlines()
    start = next((i for i, ln in enumerate(lines) if ln.startswith('void main(')), None)
    if start is None:
        raise Unsupported('no main() in decompiled program')
    brace = next((i for i in range(start, len(lines)) if lines[i].strip() == '{'), None)
    if brace is None:
        raise Unsupported('no body brace')
    header = '\n'.join(lines[start:brace])
    t = Translator(asm_text, header)
    body = t.run()
    return '\n'.join(lines[:brace + 1] + body + ['}']) + '\n'


if __name__ == '__main__':
    import sys
    sys.stdout.write(translate(open(sys.argv[1]).read(), open(sys.argv[2]).read()))
