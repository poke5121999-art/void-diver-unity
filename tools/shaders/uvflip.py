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
  ps-clipspace  URP ComputeClipSpacePosition before unity_MatrixInvP / unity_MatrixInvVP: `mul d, -s, <column 1>`, the
             `positionCS.y = -positionCS.y` of UNITY_UV_STARTS_AT_TOP; GL multiplies by s.
  ps-ndcuv   a position projected in the pixel stage by a GPU projection (glstate_matrix_projection, or the game's
             GL.GetGPUProjectionMatrix(renderIntoTexture) previous-frame matrix), divided, `mad uv.xy, ndc.xy, 0.5, 0.5`,
             then `add uv.z, -uv.y, l(1.0)`: D3D's projection into a texture is flipped, so v is flipped back; GL's is not
             (`mov uv.z, uv.y`).

Depth (`depth=True`, for a pass that depth_pass() picks): D3D11 here means reversed Z, device depth 1 at the near plane
and 0 at the far plane; OpenGL stores 0 near, 1 far, and its clip z runs -1..1. The game compares depths against baked
constants and blend ops (BlendOp Max keeps the nearest custom depth, `v.z - customDepth.z < 2.5e-5` means "in front"), so
on GL these passes run on D3D depth values instead of translating each comparison:
  ps-posz    SV_Position.z read by the pixel stage: GL uses 1 - z (window depth 0..1 maps to D3D's reversed 1..0).
  ps-depthtex  a sample of a hardware depth texture (_CameraDepthTexture, the unified blur's depth copy): 1 - d.
  vs-clipz   a vertex output that carries clip z next to the position: `mov o.zw, pos.zzzw` (ComputeScreenPos) gets
             z' = (w - z) / 2, `div o.z, pos.z, pos.w` gets (1 - z/w) / 2, both D3D's device depth after the divide.
  depth_uniforms()  in the translated GL body, _ZBufferParams, unity_MatrixInvP and unity_MatrixInvVP become their
             D3D values: _ZBufferParams (-x, x + y, -z, z + w); the inverse matrices take D3D clip z through
             (x, y, z, w) -> (x, y, w - 2z, w), GL's clip z for the same depth.
A pass runs on D3D depth when its LightMode is CustomDepth or a pixel variant reads _CustomDepthBuffer, unity_MatrixInvP
or unity_MatrixInvVP: everything that writes the custom depth buffer, reads it, or rebuilds positions from depth.
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


# ---------------------------------------------------------------- depth: run D3D's reversed-Z values on GL
DEPTH_TEXTURES = ('_CameraDepthTexture', '_UnifiedBlurDepthTexture')
INV_MATRICES = ('unity_MatrixInvP', 'unity_MatrixInvVP')


def matrix_columns(params, names):
    """{('cbN[row]'): (matrix name, column)} for the column registers of the named matrices."""
    slot = {b['name']: b['index'] for b in params.get('cbBindings', [])}
    out = {}
    for cb in params.get('cbuffers', []):
        if cb['name'] not in slot:
            continue
        for p in cb['params']:
            if p['isMatrix'] and not p['arraySize'] and p['name'] in names:
                for c in range(4):
                    out['cb%d[%d]' % (slot[cb['name']], p['offset'] // 16 + c)] = (p['name'], c)
    return out


def ndcuv_ps(insts, notes):
    """ps-ndcuv: {instruction index: new text}."""
    edits = {}
    half = re.compile(r'l\(0\.500000, 0\.500000(, [-0-9.]+, [-0-9.]+)?\)')
    for k in range(1, len(insts) - 1):
        p, a, b = insts[k - 1], insts[k], insts[k + 1]
        if a.opc != 'mad' or len(a.ops) != 4 or not half.fullmatch(a.ops[2]) or not half.fullmatch(a.ops[3]):
            continue
        u, src = reg(a.ops[0]), reg(a.ops[1])
        if not u or not src or u[1] != 'xy' or p.opc != 'div' or not reg(p.ops[0]) or reg(p.ops[0])[0] != src[0]:
            continue
        if b.opc == 'add' and len(b.ops) == 3 and b.ops[1] == '-%s.y' % u[0] and b.ops[2] == 'l(1.000000)':
            edits[k + 1] = Inst(b.idx, b.indent, 'mov', [b.ops[0], '%s.y' % u[0]]).text()
            notes.append('ps-ndcuv')
    return edits


def clipspace_ps(insts, params, notes):
    """ps-clipspace: {instruction index: new text}."""
    cols = matrix_columns(params, INV_MATRICES)
    edits = {}
    for k, ins in enumerate(insts):
        for j, op in enumerate(ins.ops[1:], 1):
            m = re.fullmatch(r'(cb\d+\[\d+\])(?:\.\w+)?', op)
            if not m or cols.get(m.group(1), (None, -1))[1] != 1:
                continue
            others = [o for i, o in enumerate(ins.ops[1:], 1) if i != j]
            if ins.opc == 'mul' and len(others) == 1 and others[0].startswith('-') and reg(others[0][1:]):
                ops = list(ins.ops)
                ops[3 - j] = others[0][1:]
                edits[k] = Inst(ins.idx, ins.indent, ins.opc, ops).text()
                notes.append('ps-clipspace')
            else:
                notes.append('ps-clipspace: %s column 1 read without the y flip at instruction %d' % (cols[m.group(1)][0], k))
    return edits


def depth_pass(tags, fragment_programs):
    """True when a pass must run on D3D depth values on GL (module docstring). fragment_programs: [(asm, params)]."""
    if any(v == 'CustomDepth' for v in tags.values()):
        return True
    for asm, params in fragment_programs:
        if any(t['name'] == '_CustomDepthBuffer' for t in params.get('textures', [])):
            return True
        if matrix_columns(params, INV_MATRICES):
            return True
    return False


def temp_count(lines):
    for i, ln in enumerate(lines):
        m = re.match(r'\s*dcl_temps (\d+)', ln)
        if m:
            return i, int(m.group(1))
    return None, 0


def depth_ps(lines, insts, params, texts, notes):
    """ps-posz and ps-depthtex on `texts` ({instruction index: text}); returns the number of temps added."""
    added = 0
    _, ntemps = temp_count(lines)
    m = re.search(r'^dcl_input_ps_siv\b.*?\b(v\d+)\.(\w+), position', '\n'.join(lines), re.M)
    if m and 'z' in m.group(2):
        pos = m.group(1)
        readz = [k for k, ins in enumerate(insts) for o in ins.ops[1:]
                 if (lambda r: r and r[0] == pos and 'z' in r[1])(reg(o.lstrip('-').strip('|')))]
        if readz:
            t = 'r%d' % (ntemps + added)
            added += 1
            for k in range(len(insts)):
                texts[k] = re.sub(r'\b%s\b' % pos, t, texts[k])
            texts[0] = '%smov %s.xyzw, %s.xyzw\n%sadd %s.z, -%s.z, l(1.000000)\n%s' % (
                insts[0].indent, t, pos, insts[0].indent, t, pos, texts[0])
            notes.append('ps-posz')
    tex = {t['index']: t['name'] for t in params.get('textures', [])}
    for k, ins in enumerate(insts):
        if not ins.opc.startswith(('sample', 'ld', 'gather4')):
            continue
        res = next((o for o in ins.ops[1:] if re.fullmatch(r't\d+(\.\w+)?', o)), None)
        if res is None or tex.get(int(re.match(r't(\d+)', res).group(1))) not in DEPTH_TEXTURES:
            continue
        d = reg(ins.ops[0])
        if not d or d[0][0] != 'r':
            notes.append('ps-depthtex: destination %s not a temp at instruction %d' % (ins.ops[0], k))
            continue
        swz = res.split('.')[1] if '.' in res else 'xyzw'
        if ins.opc.startswith('gather4'):
            src = {c: ('x' if swz[0] == 'x' else '-') for c in d[1]}
        else:
            src = comps_for(swz, d[1])
        fix = ['%sadd %s.%s, -%s.%s, l(1.000000)' % (ins.indent, d[0], c, d[0], c) for c in d[1] if src[c] == 'x']
        if fix:
            texts[k] = texts[k] + '\n' + '\n'.join(fix)
            notes.append('ps-depthtex')
    return added


def depth_vs(lines, insts, texts, notes):
    """vs-clipz on `texts`; returns the number of temps added."""
    _, ntemps = temp_count(lines)
    k_pos = pos = None
    for k, ins in enumerate(insts):
        if ins.opc == 'mov' and len(ins.ops) == 2:
            d, s = reg(ins.ops[0]), reg(ins.ops[1])
            if d and s and s[0].startswith('r') and re.search(r'^dcl_output_siv %s\.xyzw, position' % d[0], '\n'.join(lines), re.M) \
                    and d[1] == 'xyzw':
                k_pos, pos = k, s
    if k_pos is None:
        return 0
    zc = comps_for(pos[1], 'z')['z']
    wc = comps_for(pos[1], 'w')['w']
    pr = pos[0]

    def writes(ins, comp):
        d = reg(ins.ops[0]) if ins.ops else None
        return d is not None and d[0] == pr and comp in d[1]

    k_def = max((k for k in range(k_pos) if writes(insts[k], zc)), default=None)
    if k_def is None:
        return 0
    t = 'r%d' % ntemps
    used = False
    for k in range(k_def + 1, len(insts)):
        ins = insts[k]
        if k != k_pos and ins.ops:
            d = reg(ins.ops[0])
            reads = []
            for so in ins.ops[1:]:
                r = reg(so.lstrip('-').strip('|'))
                if r and r[0] == pr and d:
                    reads += [c for c, sc in comps_for(r[1], d[1]).items() if sc == zc]
            if reads and d and d[0].startswith('o'):
                s = reg(ins.ops[1]) if len(ins.ops) > 1 else None
                w_ok = not any(writes(insts[j], wc) for j in range(k_def + 1, k))
                if ins.opc == 'mov' and s and not ins.ops[1].startswith(('-', '|')) and w_ok:
                    cm = comps_for(s[1], d[1])
                    keep = ''.join(c for c in d[1] if c not in reads)
                    out = []
                    if keep:
                        out.append('%smov %s.%s, %s.%s' % (ins.indent, d[0], keep, pr, ''.join(cm[c] for c in keep)))
                    out.append('%smul %s.x, %s.%s, l(0.500000)' % (ins.indent, t, pr, wc))
                    for c in reads:
                        out.append('%smad %s.%s, %s.%s, l(-0.500000), %s.x' % (ins.indent, d[0], c, pr, zc, t))
                    texts[k] = '\n'.join(out)
                    used = True
                    notes.append('vs-clipz')
                elif ins.opc == 'div' and len(reads) == 1 and len(d[1]) == 1 and reg(ins.ops[2]) and reg(ins.ops[2])[0] == pr \
                        and comps_for(reg(ins.ops[2])[1], d[1])[d[1]] == wc and w_ok:
                    texts[k] = '%sdiv %s.x, %s, %s\n%smad %s.%s, %s.x, l(-0.500000), l(0.500000)' % (
                        ins.indent, t, ins.ops[1], ins.ops[2], ins.indent, d[0], d[1], t)
                    used = True
                    notes.append('vs-clipz')
                else:
                    notes.append('vs-clipz: output %s reads clip z through `%s`, left alone' % (ins.ops[0], ins.opc))
            elif reads:
                notes.append('vs-clipz: clip z feeds %s through `%s`, left alone' % (ins.ops[0], ins.opc))
        if writes(ins, zc) and k != k_def:
            break
    return 1 if used else 0


def depth_uniforms(body, notes):
    """The GL body with _ZBufferParams, unity_MatrixInvP and unity_MatrixInvVP replaced by their D3D values."""
    defs = []
    if re.search(r'\b_ZBufferParams\b', body):
        body = re.sub(r'\b_ZBufferParams\b', 'vd_ZBufferParamsD3D', body)
        defs.append('  float4 vd_ZBufferParamsD3D = float4(-_ZBufferParams.x, _ZBufferParams.x + _ZBufferParams.y, '
                    '-_ZBufferParams.z, _ZBufferParams.z + _ZBufferParams.w);')
        notes.append('ps-zbufferparams')
    for nm in INV_MATRICES:
        if re.search(r'\b%s\b' % nm, body):
            body = re.sub(r'\b%s\b' % nm, 'vd_%sD3D' % nm[6:], body)
            defs.append('  float4x4 vd_%sD3D = mul(%s, float4x4(1, 0, 0, 0, 0, 1, 0, 0, 0, 0, -2, 1, 0, 0, 0, 1));' % (nm[6:], nm))
            notes.append('ps-inverse-matrix')
    if not defs:
        return body
    lines = body.split('\n')
    brace = next(i for i, ln in enumerate(lines) if ln.strip() == '{')
    return '\n'.join(lines[:brace + 1] + defs + lines[brace + 1:])


def gl_variant(asm_text, params, stage, depth=False):
    """(OpenGL asm or None, notes). depth: the pass runs on D3D depth values (depth_pass)."""
    notes = []
    lines, insts = parse(asm_text)
    if not insts:
        return None, notes
    if stage == 'progVertex' and re.search(r'^dcl_input_sgv v0\.x, vertex_id', asm_text, re.M):
        edits = fullscreen_vs(lines, insts, notes)
    elif stage == 'progFragment':
        edits = screen_ps(insts, params, notes)
        edits.update(clipspace_ps(insts, params, notes))
        edits.update(ndcuv_ps(insts, notes))
    elif stage == 'progVertex':
        edits = {}
    else:
        return None, notes
    texts = {k: edits.get(k, ins.text()) for k, ins in enumerate(insts)}
    added = 0
    if depth and stage == 'progFragment':
        added = depth_ps(lines, insts, params, texts, notes)
    elif depth and stage == 'progVertex':
        added = depth_vs(lines, insts, texts, notes)
    changed = {k: t for k, t in texts.items() if t != insts[k].text()}
    if not changed:
        return None, notes
    out = list(lines)
    for k, txt in changed.items():
        out[insts[k].idx] = txt
    if added:
        i, n = temp_count(lines)
        if i is None:
            out[insts[0].idx] = 'dcl_temps %d\n%s' % (added, out[insts[0].idx])
        else:
            out[i] = 'dcl_temps %d' % (n + added)
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
