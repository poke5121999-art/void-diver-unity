#!/usr/bin/env python3
"""OpenGL variant of a D3D11 program whose bytecode baked in D3D's screen-space conventions.

    gl_asm, notes = uvflip.gl_variant(asm_text, params, stage)

The game ships D3D11 bytecode only, so every `#if UNITY_UV_STARTS_AT_TOP` (and UNITY_NEAR_CLIP_VALUE) in the source
was resolved for D3D before we ever see it. On OpenGL Core (the Linux editor) that code flips the image: each blit
through a rebuilt fullscreen shader turns it upside down (docs/DECODING.md trap 20). This module finds the D3D-only
instructions in the disassembly and returns a copy with the OpenGL branch of the same source in their place;
generate.py translates both and emits the differing lines under `#if UNITY_UV_STARTS_AT_TOP ... #else ... #endif`, so the
D3D build compiles exactly the original translation. `notes` names each rewrite; gl_asm is None when nothing applies.

Patterns (all found by survey over the 7,510 shipped blobs; docs/SHADERS.md "Lật UV"):
  vs-uv      vertex shader fed by SV_VertexID (URP Blit.hlsl Vert / GetFullScreenTriangleTexCoord, and the game's own
             fullscreen vertex stage): `add d, -s, l(1.0)` where s, evaluated for vertex ids 0..3, is the screen v
             0.5 * clip.y + 0.5. GL keeps s (`mov d, s`); when d only feeds a `movc` that picks between d and s (the
             game's `_FlipY < 0.5 ? 1 - v : v`), GL swaps the two picks instead, which keeps _FlipY's meaning.
  vs-nearz   same vertex stage writing clip z = 1.0 literal (UNITY_NEAR_CLIP_VALUE on reversed-Z D3D): GL -1.0.
  ps-scalebiasrt  URP TransformNormalizedScreenUV: `mad x, y, _ScaleBiasRt.x, _ScaleBiasRt.y` then `add z, -x, l(1.0)`;
             GL has no transform (`mov x, y` and `mov z, x`, so an in-place mad still yields y).
  ps-pixelpos  Shader Graph PixelPosition: `lt c, _ProjectionParams.x, l(0.0)` then `add r, -vN.y, _ScreenParams.y`
             (URP names it _ScaledScreenParams);
             the GL branch of the template tests `_ProjectionParams.x > 0` (`lt c, l(0.0), _ProjectionParams.x`).
"""
import re
import struct

from asm2hlsl import split_operands, lit_bits

SWZ = 'xyzw'
ONE = 0x3f800000


class Inst:
    def __init__(self, idx, indent, opc, ops):
        self.idx, self.indent, self.opc, self.ops = idx, indent, opc, ops

    def text(self):
        return '%s%s %s' % (self.indent, self.opc, ', '.join(self.ops))


def parse(asm_text):
    """(lines, instructions): instruction lines only (no comments, declarations or the version token)."""
    lines = asm_text.splitlines()
    insts = []
    for i, ln in enumerate(lines):
        code = ln.split('//')[0]
        s = code.strip()
        if not s or s.startswith('dcl_') or re.fullmatch(r'(vs|ps|gs|hs|ds|cs)_\d_\d', s) or s.startswith('{') or s.startswith('}'):
            continue
        m = re.match(r'(\s*)(\S+)\s*(.*)$', code)
        insts.append(Inst(i, m.group(1), m.group(2), [o.strip() for o in split_operands(m.group(3))] if m.group(3).strip() else []))
    return lines, insts


def reg(op):
    """('r0', 'xy') for a temp/output/input operand without modifiers, else None."""
    m = re.fullmatch(r'([rov]\d+)(?:\.([xyzw]+))?', op)
    return (m.group(1), m.group(2) or 'xyzw') if m else None


def comps_for(swz, mask):
    """Source components read for each destination component of `mask` (D3D: swizzle position = dest component)."""
    s = swz if len(swz) == 4 else (swz * 4 if len(swz) == 1 else (swz + swz[-1] * 4)[:4])
    return {c: s[SWZ.index(c)] for c in mask}


# ---------------------------------------------------------------- vertex stage: evaluate per vertex id
def f2b(f):
    return struct.unpack('<I', struct.pack('<f', f))[0]


def b2f(b):
    return struct.unpack('<f', struct.pack('<I', b))[0]


INT_OPS = {'ishl', 'ushr', 'ishr', 'and', 'or', 'xor', 'iadd', 'imul', 'utof', 'itof', 'ftou', 'ftoi', 'mov', 'movc'}
FLOAT_EVAL = {'add', 'mul', 'mad', 'div'}


def eval_vs(insts, vid):
    """Per-component register bits after each instruction for SV_VertexID = vid; None where a value is unknown
    (constant buffers, textures). Returns (state after the program, list of states before each instruction)."""
    st = {}
    before = []

    def src(op, comp):
        neg = op.startswith('-')
        t = op[1:].strip() if neg else op
        absm = t.startswith('|')
        if absm:
            t = t.strip('|')
        m = re.fullmatch(r'l\((.*)\)', t)
        if m:
            vals = [lit_bits(v) for v in m.group(1).split(',')]
            vals = vals if len(vals) == 4 else vals * 4
            return vals[SWZ.index(comp)], neg, absm
        r = reg(t)
        if r is None:
            return None, neg, absm
        if r[0] == 'v0':
            return vid, neg, absm
        return st.get((r[0], comp)), neg, absm

    for ins in insts:
        before.append(dict(st))
        base = ins.opc.split('_sat')[0]
        if not ins.ops:
            continue
        d = reg(ins.ops[0])
        if d is None:
            continue
        for dc in d[1]:
            vals = []
            for so in ins.ops[1:]:
                sr = reg(so.lstrip('-').strip('|'))
                swz = sr[1] if sr else 'xyzw'
                sc = comps_for(swz, d[1])[dc]
                vals.append(src(so, sc))
            out = None
            if any(v[0] is None for v in vals) and base != 'movc':
                out = None
            elif base in FLOAT_EVAL:
                fs = []
                for b, neg, absm in vals:
                    f = b2f(b)
                    f = abs(f) if absm else f
                    fs.append(-f if neg else f)
                try:
                    out = f2b({'add': lambda: fs[0] + fs[1], 'mul': lambda: fs[0] * fs[1], 'mad': lambda: fs[0] * fs[1] + fs[2],
                               'div': lambda: fs[0] / fs[1]}[base]())
                except (ZeroDivisionError, OverflowError):
                    out = None
            elif base in INT_OPS:
                iv = [(((-b) if neg else b) & 0xffffffff) if b is not None else None for b, neg, absm in vals]
                if base == 'mov':
                    b, neg, absm = vals[0]
                    out = b ^ 0x80000000 if neg else b
                elif base == 'movc':
                    out = None if iv[0] is None else (iv[1] if iv[0] else iv[2])
                elif base == 'utof':
                    out = f2b(float(iv[0]))
                elif base == 'itof':
                    out = f2b(float(iv[0] - (1 << 32) if iv[0] & 0x80000000 else iv[0]))
                elif base == 'ishl':
                    out = (iv[0] << (iv[1] & 31)) & 0xffffffff
                elif base == 'ushr':
                    out = iv[0] >> (iv[1] & 31)
                elif base == 'and':
                    out = iv[0] & iv[1]
                elif base == 'or':
                    out = iv[0] | iv[1]
                elif base == 'xor':
                    out = iv[0] ^ iv[1]
                elif base == 'iadd':
                    out = (iv[0] + iv[1]) & 0xffffffff
                elif base == 'imul':
                    out = (iv[0] * iv[1]) & 0xffffffff
            st[(d[0], dc)] = out
    return st, before


def fullscreen_vs(lines, insts, notes):
    """Rewrites for a vertex stage fed by SV_VertexID; {instruction index: new text}."""
    if any(i.opc.startswith(('if', 'loop', 'else', 'endif', 'endloop', 'switch', 'call', 'breakc', 'continuec', 'retc'))
           for i in insts):
        notes.append('vs-uv: control flow, not analysed')
        return {}
    runs = [eval_vs(insts, vid) for vid in range(4)]
    posy = [r[0].get(('o0', 'y')) for r in runs]
    if any(p is None for p in posy):
        notes.append('vs-uv: clip y does not depend on the vertex id alone, not analysed')
        return {}
    screen_v = [0.5 * b2f(p) + 0.5 for p in posy]
    edits = {}
    for k, ins in enumerate(insts):
        base = ins.opc
        if base == 'add' and len(ins.ops) == 3 and ins.ops[2] == 'l(1.000000)' and ins.ops[1].startswith('-'):
            d, s = reg(ins.ops[0]), reg(ins.ops[1][1:])
            if not d or not s or len(d[1]) != 1:
                continue
            sc = comps_for(s[1], d[1])[d[1]]
            vals = [r[1][k].get((s[0], sc)) for r in runs]
            if any(v is None for v in vals) or any(abs(b2f(v) - sv) > 1e-6 for v, sv in zip(vals, screen_v)):
                continue
            # where does d go? the first reads of d before it is written again
            readers = []
            for j in range(k + 1, len(insts)):
                o = insts[j]
                for so in o.ops[1:]:
                    r = reg(so.lstrip('-').strip('|'))
                    dr = reg(o.ops[0]) if o.ops else None
                    if r and r[0] == d[0] and d[1] in ''.join(comps_for(r[1], dr[1] if dr else 'x').values()):
                        readers.append(j)
                        break
                w = reg(o.ops[0]) if o.ops else None
                if w and w[0] == d[0] and d[1] in w[1]:
                    break
            pick = readers[0] if len(readers) == 1 and insts[readers[0]].opc == 'movc' else None
            if pick is not None:
                o = insts[pick]
                a, b = reg(o.ops[2]), reg(o.ops[3])
                od = reg(o.ops[0])
                if a and b and od and len(od[1]) == 1 and a[0] == d[0] and comps_for(a[1], od[1])[od[1]] == d[1] \
                        and b[0] == s[0] and comps_for(b[1], od[1])[od[1]] == sc:
                    edits[pick] = Inst(o.idx, o.indent, o.opc, [o.ops[0], o.ops[1], o.ops[3], o.ops[2]]).text()
                    notes.append('vs-uv(movc)')
                    continue
            edits[k] = Inst(ins.idx, ins.indent, 'mov', [ins.ops[0], '%s.%s' % (s[0], sc)]).text()
            notes.append('vs-uv')
        elif base == 'mov' and len(ins.ops) == 2:
            d = reg(ins.ops[0])
            m = re.fullmatch(r'l\((.*)\)', ins.ops[1])
            if d and d[0] == 'o0' and 'z' in d[1] and m:
                vals = [v.strip() for v in m.group(1).split(',')]
                if len(vals) == 4 and lit_bits(vals[2]) == ONE:
                    vals[2] = '-1.000000'
                    edits[k] = Inst(ins.idx, ins.indent, 'mov', [ins.ops[0], 'l(%s)' % ', '.join(vals)]).text()
                    notes.append('vs-nearz')
    return edits


# ---------------------------------------------------------------- pixel stage: named URP constants
def cb_namer(params):
    slot = {b['index']: b['name'] for b in params.get('cbBindings', [])}
    cbs = {cb['name']: cb['params'] for cb in params.get('cbuffers', [])}

    def name(op):
        m = re.fullmatch(r'cb(\d+)\[(\d+)\]\.([xyzw])', op.strip())
        if not m:
            return None
        byte = int(m.group(2)) * 16 + SWZ.index(m.group(3)) * 4
        for p in cbs.get(slot.get(int(m.group(1))), ()):
            if p['isMatrix'] or p['arraySize']:
                # only plain scalars/vectors are matched by name; a matrix or array element answers its bare name
                size = 16 * max(p['rows'], p['cols'], 1) * max(p['arraySize'], 1)
                if p['offset'] <= byte < p['offset'] + size:
                    return p['name']
            elif p['offset'] <= byte < p['offset'] + 4 * p['cols']:
                return '%s.%s' % (p['name'], SWZ[(byte - p['offset']) // 4]) if p['cols'] > 1 else p['name']
        return None
    return name


def screen_ps(insts, params, notes):
    name = cb_namer(params)
    edits = {}
    for k in range(len(insts) - 1):
        a, b = insts[k], insts[k + 1]
        if a.opc == 'mad' and len(a.ops) == 4 and name(a.ops[2]) == '_ScaleBiasRt.x' and name(a.ops[3]) == '_ScaleBiasRt.y':
            x, y = reg(a.ops[0]), reg(a.ops[1])
            if not x or not y or len(x[1]) != 1 or len(y[1]) != 1:
                notes.append('ps-scalebiasrt: unexpected operands at instruction %d' % k)
                continue
            if b.opc == 'add' and len(b.ops) == 3 and b.ops[1] == '-%s.%s' % x and b.ops[2] == 'l(1.000000)':
                # x = y, z = x: also right when the mad overwrote y in place (mad r0.y, r0.y, ...)
                edits[k] = Inst(a.idx, a.indent, 'mov', [a.ops[0], a.ops[1]]).text()
                edits[k + 1] = Inst(b.idx, b.indent, 'mov', [b.ops[0], '%s.%s' % x]).text()
                notes.append('ps-scalebiasrt')
            else:
                notes.append('ps-scalebiasrt: no 1 - v after instruction %d' % k)
        elif a.opc == 'lt' and len(a.ops) == 3 and name(a.ops[1]) == '_ProjectionParams.x' and a.ops[2] == 'l(0.000000)':
            if b.opc == 'add' and len(b.ops) == 3 and re.fullmatch(r'-v\d+\.y', b.ops[1]) \
                    and name(b.ops[2]) in ('_ScreenParams.y', '_ScaledScreenParams.y'):  # URP: #define _ScreenParams _ScaledScreenParams
                edits[k] = Inst(a.idx, a.indent, 'lt', [a.ops[0], a.ops[2], a.ops[1]]).text()
                notes.append('ps-pixelpos')
            else:
                notes.append('ps-pixelpos: _ProjectionParams.x < 0 without PixelPosition at instruction %d' % k)
    return edits


def gl_variant(asm_text, params, stage):
    """(OpenGL asm or None, notes)."""
    notes = []
    lines, insts = parse(asm_text)
    if stage == 'progVertex' and re.search(r'^dcl_input_sgv v0\.x, vertex_id', asm_text, re.M):
        edits = fullscreen_vs(lines, insts, notes)
    elif stage == 'progFragment':
        edits = screen_ps(insts, params, notes)
    else:
        return None, notes
    if not edits:
        return None, notes
    out = list(lines)
    for k, txt in edits.items():
        out[insts[k].idx] = txt
    return '\n'.join(out) + '\n', notes


def merge(d3d, gl):
    """One source holding both translations: lines that differ go under #if UNITY_UV_STARTS_AT_TOP / #else."""
    import difflib
    a, b = d3d.splitlines(), gl.splitlines()
    out = []
    for tag, i1, i2, j1, j2 in difflib.SequenceMatcher(None, a, b, autojunk=False).get_opcodes():
        if tag == 'equal':
            out += a[i1:i2]
            continue
        out.append('#if UNITY_UV_STARTS_AT_TOP')
        out += a[i1:i2]
        out.append('#else')
        out += b[j1:j2]
        out.append('#endif')
    return '\n'.join(out) + '\n'
