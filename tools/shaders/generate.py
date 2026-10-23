#!/usr/bin/env python3
"""Generate URP-compatible ShaderLab files from extracted + decompiled game shaders.

    generate.py <work_dir> <out_dir> [--names names.json] [--report report.json] [--only <sanitized name>...]

For every <work_dir>/<shader>/shader.json with decompiled <shader>/dxbc/<n>.hlsl (see extract.py, decompile.py) this
writes <out_dir>/<sanitized>.shader (+ .meta, GUID = md5("vdshader:" + shader name)) and
<out_dir>/<sanitized>/p<pass>_<stage>_<blob>.hlsl (+ .meta): one include per compiled variant, holding the decompiled
program with cbN[i] / tN / sN_s rewritten to the original constant, texture and sampler names. The body is
asm2hlsl's bit-exact translation of <n>.asm (the disassembly) when it exists; 3Dmigoto's own body loses integer
semantics (see asm2hlsl.py) and is only the fallback, counted in the report as `body3dm`.

Scheme: ShaderLab name, Properties, SubShader/Pass tags, render state and LightMode come from the parsed form; each
pass gets one HLSLPROGRAM with cbuffer declarations (packoffset = original offsets), the union of textures/samplers,
`#pragma multi_compile` lines reconstructed from the keyword sets of the shipped variants (mutually exclusive
keywords share one line, `_` when the game shipped variants without any of them, `_local` for local keywords), and a
`#if` chain that picks the shipped variant whose keyword set is the largest subset of the active keywords.
Instancing/stereo/foveated variants are not emitted (see docs/SHADERS.md).
"""
import hashlib
import json
import os
import re
import struct
import sys

sys.path.insert(0, os.path.dirname(os.path.abspath(__file__)))
import asm2hlsl  # noqa: E402

DROP_KEYWORDS = {'INSTANCING_ON', 'DOTS_INSTANCING_ON', 'PROCEDURAL_INSTANCING_ON', 'STEREO_INSTANCING_ON',
                 'STEREO_MULTIVIEW_ON', 'STEREO_CUBEMAP_RENDER_ON', 'UNITY_SINGLE_PASS_STEREO',
                 '_FOVEATED_RENDERING_NON_UNIFORM_RASTER'}
STAGE_ENTRY = {'progVertex': 'vert', 'progFragment': 'frag', 'progGeometry': 'geom', 'progHull': 'hull', 'progDomain': 'domain'}
STAGE_SHORT = {'progVertex': 'vs', 'progFragment': 'ps', 'progGeometry': 'gs', 'progHull': 'hs', 'progDomain': 'ds'}
STAGE_PRAGMA = {'progVertex': 'vertex', 'progFragment': 'fragment', 'progGeometry': 'geometry', 'progHull': 'hull', 'progDomain': 'domain'}
BLEND = ['Zero', 'One', 'DstColor', 'SrcColor', 'OneMinusDstColor', 'SrcAlpha', 'OneMinusSrcColor', 'DstAlpha',
         'OneMinusDstAlpha', 'SrcAlphaSaturate', 'OneMinusSrcAlpha']
BLENDOP = ['Add', 'Sub', 'RevSub', 'Min', 'Max', 'LogicalClear', 'LogicalSet', 'LogicalCopy', 'LogicalCopyInverted',
           'LogicalNoop', 'LogicalInvert', 'LogicalAnd', 'LogicalNand', 'LogicalOr', 'LogicalNor', 'LogicalXor',
           'LogicalEquiv', 'LogicalAndReverse', 'LogicalAndInverted', 'LogicalOrReverse', 'LogicalOrInverted',
           'Multiply', 'Screen', 'Overlay', 'Darken', 'Lighten', 'ColorDodge', 'ColorBurn', 'HardLight', 'SoftLight',
           'Difference', 'Exclusion', 'HSLHue', 'HSLSaturation', 'HSLColor', 'HSLLuminosity']
COMPARE = ['Off', 'Never', 'Less', 'Equal', 'LEqual', 'Greater', 'NotEqual', 'GEqual', 'Always']
STENCILOP = ['Keep', 'Zero', 'Replace', 'IncrSat', 'DecrSat', 'Invert', 'IncrWrap', 'DecrWrap']
CULL = ['Off', 'Front', 'Back']
TAG_NAMES = {'LIGHTMODE': 'LightMode', 'IGNOREPROJECTOR': 'IgnoreProjector', 'QUEUE': 'Queue', 'SHADOWSUPPORT': 'ShadowSupport',
             'PREVIEWTYPE': 'PreviewType', 'DISABLEBATCHING': 'DisableBatching', 'FORCENOSHADOWCASTING': 'ForceNoShadowCasting',
             'CANUSESPRITEATLAS': 'CanUseSpriteAtlas', 'RENDERTYPE': 'RenderType'}
PROP_FLAGS = [(1, 'HideInInspector'), (2, 'PerRendererData'), (4, 'NoScaleOffset'), (8, 'Normal'), (16, 'HDR'), (32, 'Gamma'),
              (64, 'NonModifiableTextureData'), (128, 'MainTexture'), (256, 'MainColor')]
TEXDIM = {1: 'Any', 2: '2D', 3: '3D', 4: 'Cube', 5: '2DArray', 6: 'CubeArray'}
PARAM_TYPE = {0: 'float', 1: 'int', 2: 'int', 3: 'float', 4: 'int', 5: 'uint', 6: 'uint'}  # Unity ShaderParamType; bool/half -> int/float
SWZ = 'xyzw'

PRELUDE = '''// 3Dmigoto decompiler conventions used by the per-variant code
#define cmp -
#define VD_COL(M, c) (float4((M)[0][c], (M)[1][c], (M)[2][c], (M)[3][c]))
int vd_asint(float v) { return asint(v); } int2 vd_asint(float2 v) { return asint(v); } int3 vd_asint(float3 v) { return asint(v); } int4 vd_asint(float4 v) { return asint(v); }
int vd_asint(uint v) { return asint(v); } int2 vd_asint(uint2 v) { return asint(v); } int3 vd_asint(uint3 v) { return asint(v); } int4 vd_asint(uint4 v) { return asint(v); }
int vd_asint(int v) { return v; } int2 vd_asint(int2 v) { return v; } int3 vd_asint(int3 v) { return v; } int4 vd_asint(int4 v) { return v; }
uint vd_asuint(float v) { return asuint(v); } uint2 vd_asuint(float2 v) { return asuint(v); } uint3 vd_asuint(float3 v) { return asuint(v); } uint4 vd_asuint(float4 v) { return asuint(v); }
uint vd_asuint(int v) { return asuint(v); } uint2 vd_asuint(int2 v) { return asuint(v); } uint3 vd_asuint(int3 v) { return asuint(v); } uint4 vd_asuint(int4 v) { return asuint(v); }
uint vd_asuint(uint v) { return v; } uint2 vd_asuint(uint2 v) { return v; } uint3 vd_asuint(uint3 v) { return v; } uint4 vd_asuint(uint4 v) { return v; }
float vd_asfloat(int v) { return asfloat(v); } float2 vd_asfloat(int2 v) { return asfloat(v); } float3 vd_asfloat(int3 v) { return asfloat(v); } float4 vd_asfloat(int4 v) { return asfloat(v); }
float vd_asfloat(uint v) { return asfloat(v); } float2 vd_asfloat(uint2 v) { return asfloat(v); } float3 vd_asfloat(uint3 v) { return asfloat(v); } float4 vd_asfloat(uint4 v) { return asfloat(v); }
float vd_asfloat(float v) { return v; } float2 vd_asfloat(float2 v) { return v; } float3 vd_asfloat(float3 v) { return v; } float4 vd_asfloat(float4 v) { return v; }
'''


def guid_for(key):
    return hashlib.md5(('vdshader:' + key).encode('utf-8')).hexdigest()


def sanitize(name):
    return re.sub(r'[^A-Za-z0-9_.-]+', '_', name).strip('_')


def fmt_num(v):
    if isinstance(v, float) and v.is_integer():
        return str(int(v))
    return repr(v)


class GenError(Exception):
    pass


# ---------------------------------------------------------------- ShaderLab text

def prop_line(p):
    attrs = []
    for bit, nm in PROP_FLAGS:
        if p['m_Flags'] & bit:
            attrs.append('[%s]' % nm)
    attrs += ['[%s]' % a for a in p['m_Attributes']]
    t = p['m_Type']
    d = [p['m_DefValue_0_'], p['m_DefValue_1_'], p['m_DefValue_2_'], p['m_DefValue_3_']]
    if t == 0:
        body = 'Color) = (%s, %s, %s, %s)' % tuple(fmt_num(x) for x in d)
    elif t == 1:
        body = 'Vector) = (%s, %s, %s, %s)' % tuple(fmt_num(x) for x in d)
    elif t == 2:
        body = 'Float) = %s' % fmt_num(d[0])
    elif t == 3:
        body = 'Range(%s, %s)) = %s' % (fmt_num(d[1]), fmt_num(d[2]), fmt_num(d[0]))
    elif t == 4:
        dim = TEXDIM.get(p['m_DefTexture']['m_TexDim'], '2D')
        body = '%s) = "%s" {}' % (dim, p['m_DefTexture']['m_DefaultName'])
    elif t == 5:
        body = 'Integer) = %d' % int(d[0])
    else:
        raise GenError('property type %r' % t)
    desc = p['m_Description'].replace('"', "'")
    return '%s%s ("%s", %s' % (' '.join(attrs) + (' ' if attrs else ''), p['m_Name'], desc, body)


def tags_line(tags):
    if not tags:
        return ''
    return 'Tags { ' + ' '.join('"%s" = "%s"' % (TAG_NAMES.get(k, k), v) for k, v in tags) + ' }'


def sval(fv, table=None):
    """SerializedShaderFloatValue -> ShaderLab token: [_Prop] when driven by a property, else the enum name."""
    if fv['name'] not in ('<noninit>', ''):
        return '[%s]' % fv['name']
    v = int(fv['val'])
    if table is None:
        return str(v)
    return table[v] if 0 <= v < len(table) else str(v)


def named(fv):
    return fv['name'] not in ('<noninit>', '')


def state_lines(st):
    out = []
    # blend
    def blend_lines(rb, idx):
        s, d, sa, da = rb['srcBlend'], rb['destBlend'], rb['srcBlendAlpha'], rb['destBlendAlpha']
        nondefault = any(named(x) for x in (s, d, sa, da)) or (s['val'], d['val'], sa['val'], da['val']) != (1.0, 0.0, 1.0, 0.0)
        res = []
        pre = 'Blend %d ' % idx if idx is not None else 'Blend '
        if nondefault:
            if (s['val'], d['val'], s['name'], d['name']) == (sa['val'], da['val'], sa['name'], da['name']):
                res.append('%s%s %s' % (pre, sval(s, BLEND), sval(d, BLEND)))
            else:
                res.append('%s%s %s, %s %s' % (pre, sval(s, BLEND), sval(d, BLEND), sval(sa, BLEND), sval(da, BLEND)))
        op, opa = rb['blendOp'], rb['blendOpAlpha']
        if named(op) or named(opa) or op['val'] != 0 or opa['val'] != 0:
            pre = 'BlendOp %d ' % idx if idx is not None else 'BlendOp '
            if (op['val'], op['name']) == (opa['val'], opa['name']):
                res.append('%s%s' % (pre, sval(op, BLENDOP)))
            else:
                res.append('%s%s, %s' % (pre, sval(op, BLENDOP), sval(opa, BLENDOP)))
        cm = rb['colMask']
        # ShaderLab puts the render target last for ColorMask ("ColorMask RGB 1"), unlike Blend/BlendOp ("Blend 1 ...")
        rt = ' %d' % idx if idx is not None else ''
        if named(cm):
            res.append('ColorMask [%s]%s' % (cm['name'], rt))
        elif cm['val'] != 15:
            m = int(cm['val'])
            letters = ''.join(l for bit, l in ((8, 'R'), (4, 'G'), (2, 'B'), (1, 'A')) if m & bit) or '0'
            res.append('ColorMask %s%s' % (letters, rt))
        return res
    if st.get('rtSeparateBlend'):
        for i in range(8):
            rb = st.get('rtBlend%d' % i)
            if rb:
                out += blend_lines(rb, i)
    else:
        out += blend_lines(st['rtBlend0'], None)
    if named(st['alphaToMask']) or st['alphaToMask']['val'] != 0:
        out.append('AlphaToMask %s' % ('[%s]' % st['alphaToMask']['name'] if named(st['alphaToMask']) else 'On'))
    if st.get('zClip') and st['zClip']['val'] != 1:
        out.append('ZClip False')
    if named(st['zTest']) or st['zTest']['val'] != 4:
        out.append('ZTest %s' % sval(st['zTest'], COMPARE))
    if named(st['zWrite']) or st['zWrite']['val'] != 1:
        out.append('ZWrite %s' % ('[%s]' % st['zWrite']['name'] if named(st['zWrite']) else ('On' if st['zWrite']['val'] else 'Off')))
    if named(st['culling']) or st['culling']['val'] != 2:
        out.append('Cull %s' % sval(st['culling'], CULL))
    if named(st['offsetFactor']) or named(st['offsetUnits']) or st['offsetFactor']['val'] != 0 or st['offsetUnits']['val'] != 0:
        out.append('Offset %s, %s' % (sval(st['offsetFactor']), sval(st['offsetUnits'])))
    if st.get('conservative') and st['conservative']['val'] != 0:
        out.append('Conservative True')
    # stencil
    def op_default(o):
        return not any(named(o[k]) for k in ('comp', 'pass_', 'fail', 'zFail')) and o['comp']['val'] == 8 and o['pass_']['val'] == 0 and o['fail']['val'] == 0 and o['zFail']['val'] == 0
    ops = st['stencilOp']
    front, back = st['stencilOpFront'], st['stencilOpBack']
    if not (op_default(ops) and op_default(front) and op_default(back) and not named(st['stencilRef']) and st['stencilRef']['val'] == 0
            and not named(st['stencilReadMask']) and st['stencilReadMask']['val'] == 255 and not named(st['stencilWriteMask']) and st['stencilWriteMask']['val'] == 255):
        s = ['Stencil {', '  Ref %s' % sval(st['stencilRef'])]
        if named(st['stencilReadMask']) or st['stencilReadMask']['val'] != 255:
            s.append('  ReadMask %s' % sval(st['stencilReadMask']))
        if named(st['stencilWriteMask']) or st['stencilWriteMask']['val'] != 255:
            s.append('  WriteMask %s' % sval(st['stencilWriteMask']))
        def emit_ops(o, suffix):
            r = []
            if named(o['comp']) or o['comp']['val'] != 8:
                r.append('  Comp%s %s' % (suffix, sval(o['comp'], COMPARE)))
            if named(o['pass_']) or o['pass_']['val'] != 0:
                r.append('  Pass%s %s' % (suffix, sval(o['pass_'], STENCILOP)))
            if named(o['fail']) or o['fail']['val'] != 0:
                r.append('  Fail%s %s' % (suffix, sval(o['fail'], STENCILOP)))
            if named(o['zFail']) or o['zFail']['val'] != 0:
                r.append('  ZFail%s %s' % (suffix, sval(o['zFail'], STENCILOP)))
            return r
        s += emit_ops(ops, '')
        if not op_default(front) and front != ops:
            s += emit_ops(front, 'Front')
        if not op_default(back) and back != ops:
            s += emit_ops(back, 'Back')
        s.append('}')
        out += s
    return out


# ---------------------------------------------------------------- parameter layout

def param_size(p):
    """Bytes spanned in the cbuffer. Array elements have a 16-byte stride; column-major matrices take `cols` registers."""
    if p['isMatrix']:
        return 16 * max(p['cols'], 1) * max(p['arraySize'], 1)
    if p['arraySize']:
        return 16 * (p['arraySize'] - 1) + 4 * p['cols']
    return 4 * p['cols']


def hlsl_type(p):
    base = PARAM_TYPE.get(p['type'], 'float')
    if p['isMatrix']:
        return '%s%dx%d' % (base, p['rows'], p['cols'])
    return base if p['cols'] == 1 else '%s%d' % (base, p['cols'])


def decl(p, packoffset):
    t = hlsl_type(p)
    arr = '[%d]' % p['arraySize'] if p['arraySize'] else ''
    po = ''
    if packoffset:
        reg, comp = divmod(p['offset'], 16)
        po = ' : packoffset(c%d%s)' % (reg, '' if comp == 0 or p['isMatrix'] or p['arraySize'] else '.' + SWZ[comp // 4])
    return '%s %s%s%s;' % (t, p['name'], arr, po)


class Layout:
    """Union of parameters across the variants of a pass, checked for consistent offsets."""

    def __init__(self):
        self.cbuffers = {}  # name -> {param name -> param}
        self.cb_sizes = {}
        self.textures = {}  # name -> dim
        self.tex_types = {}  # name -> decompiled declaration type ("Texture2D<float4>")
        self.sampler_types = {}  # sampler name -> "SamplerState"/"SamplerComparisonState"
        self.sampler_names = set()
        self.warnings = []

    def add_params(self, params):
        for cb in params['cbuffers']:
            d = self.cbuffers.setdefault(cb['name'], {})
            self.cb_sizes[cb['name']] = max(self.cb_sizes.get(cb['name'], 0), cb['size'])
            for p in cb['params']:
                q = d.get(p['name'])
                if q is None:
                    d[p['name']] = dict(p)
                elif cb['name'] != '$Globals' and (q['offset'] != p['offset'] or q['cols'] != p['cols'] or q['arraySize'] != p['arraySize']):
                    raise GenError('cbuffer %s member %s layout differs between variants (%r vs %r)' % (cb['name'], p['name'], q, p))
                elif q['arraySize'] < p['arraySize']:
                    d[p['name']] = dict(p)
        for t in params['textures']:
            self.textures.setdefault(t['name'], t.get('dim'))


# ---------------------------------------------------------------- HLSL rewriting

CB_RE = re.compile(r'\bcb(\d+)\[([^\]]+)\](?:\.([xyzw]{1,4}))?')
TEX_RE = re.compile(r'\bt(\d+)(?=[.\s,)])')
SMP_RE = re.compile(r'\bs(\d+)_s\b')
DECL_RE = re.compile(r'^\s*((?:Texture|RWTexture|Buffer|StructuredBuffer|ByteAddressBuffer|RWStructuredBuffer|RWBuffer|RWByteAddressBuffer)[A-Za-z0-9_]*(?:<[^>]+>)?)\s+t(\d+)\s*:\s*register\(t\d+\);')
SMP_DECL_RE = re.compile(r'^\s*(SamplerState|SamplerComparisonState)\s+s(\d+)_s\s*:\s*register\(s\d+\);')


def sampler_flag_name(flags):
    # Unity inline sampler flags as measured: bits 0-1 filter (0 point, 1 linear, 2 trilinear), bits 2-3 / 4-5 / 6-7 wrap U/V/W
    # (0 repeat, 1 clamp, 2 mirror, 3 mirrorOnce), bit 8 compare, bits 9.. aniso.
    filt = ['point', 'linear', 'trilinear', 'trilinear'][flags & 3]
    wraps = [['repeat', 'clamp', 'mirror', 'mirroronce'][(flags >> s) & 3] for s in (2, 4, 6)]
    wrap = wraps[0] if wraps[0] == wraps[1] == wraps[2] else '%su_%sv_%sw' % tuple(wraps)
    name = 'sampler_vd_%s_%s' % (filt, wrap)
    if flags & 0x100:
        name += '_compare'
    aniso = flags >> 9
    if aniso:
        name += '_aniso%d' % (1 << aniso)
    return name


def rewrite_variant(hlsl_text, params, entry, layout, errors):
    """Return the decompiled program with names resolved, plus the declaration info collected."""
    cb_by_slot = {b['index']: b['name'] for b in params['cbBindings']}
    cb_params = {cb['name']: sorted(cb['params'], key=lambda p: p['offset']) for cb in params['cbuffers']}
    tex_by_slot = {t['index']: t for t in params['textures']}
    samp_by_slot = {}
    for t in params['textures']:
        if t['samplerIndex'] is not None and t['samplerIndex'] >= 0:
            samp_by_slot.setdefault(t['samplerIndex'], 'sampler' + t['name'])
    for s in params['samplers']:
        samp_by_slot.setdefault(s['index'], sampler_flag_name(s['flags']))

    lines = hlsl_text.splitlines()
    body_start = None
    for i, ln in enumerate(lines):
        m = DECL_RE.match(ln)
        if m:
            slot = int(m.group(2))
            t = tex_by_slot.get(slot)
            if t is None:
                errors.append('texture slot t%d has no name' % slot)
                continue
            prev = layout.tex_types.get(t['name'])
            if prev and prev != m.group(1):
                layout.warnings.append('texture %s declared %s and %s' % (t['name'], prev, m.group(1)))
            layout.tex_types.setdefault(t['name'], m.group(1))
            continue
        m = SMP_DECL_RE.match(ln)
        if m:
            slot = int(m.group(2))
            nm = samp_by_slot.get(slot)
            if nm is None:
                nm = 'sampler_vd_unbound_s%d' % slot
                samp_by_slot[slot] = nm
                layout.warnings.append('sampler slot s%d has no texture or inline state; declared %s' % (slot, nm))
            layout.sampler_types.setdefault(nm, m.group(1))
            layout.sampler_names.add(nm)
            continue
        if ln.startswith('void main('):
            body_start = i
            break
    if body_start is None:
        raise GenError('no main() in decompiled program')
    body = '\n'.join(lines[body_start:])
    body = body.replace('void main(', 'void %s(' % entry, 1)

    def find_param(cbname, byte):
        for p in cb_params.get(cbname, ()):
            if p['offset'] <= byte < p['offset'] + param_size(p):
                return p
        return None

    def comp_expr(cbname, byte):
        p = find_param(cbname, byte)
        if p is None:
            return None, None
        rel = byte - p['offset']
        if p['isMatrix']:
            if p['arraySize']:
                e, rel2 = divmod(rel, 16 * p['cols'])
                col, row = divmod(rel2, 16)
                return p, '%s[%d][%d][%d]' % (p['name'], e, row // 4, col)
            col, row = divmod(rel, 16)
            return p, '%s[%d][%d]' % (p['name'], row // 4, col)
        if p['arraySize']:
            e, rel2 = divmod(rel, 16)
            if p['cols'] == 1:
                return p, '%s[%d]' % (p['name'], e)
            return p, '%s[%d].%s' % (p['name'], e, SWZ[rel2 // 4])
        if p['cols'] == 1:
            return p, p['name']
        return p, '%s.%s' % (p['name'], SWZ[rel // 4])

    def cb_access(m):
        slot = int(m.group(1))
        idx = m.group(2).strip()
        swz = m.group(3) or 'xyzw'
        cbname = cb_by_slot.get(slot)
        if cbname is None:
            errors.append('cb%d not bound' % slot)
            return m.group(0)
        if re.fullmatch(r'\d+', idx):
            reg = int(idx)
            parts = []
            owners = []
            for c in swz:
                p, e = comp_expr(cbname, reg * 16 + SWZ.index(c) * 4)
                if p is None:
                    errors.append('cb%d[%d].%s (%s) maps to no parameter' % (slot, reg, c, cbname))
                    return m.group(0)
                parts.append(e)
                owners.append(p['name'])
            p = find_param(cbname, reg * 16 + SWZ.index(swz[0]) * 4)
            if len(set(owners)) == 1:
                rel0 = reg * 16 - p['offset']
                if p['isMatrix'] and not p['arraySize']:
                    col = rel0 // 16
                    return '(VD_COL(%s, %d).%s)' % (p['name'], col, swz)
                if not p['isMatrix'] and not p['arraySize'] and p['cols'] > 1:
                    base = (reg * 16 - p['offset']) // 4
                    return '%s.%s' % (p['name'], ''.join(SWZ[base + SWZ.index(c)] for c in swz))
                if not p['isMatrix'] and p['arraySize'] and p['cols'] > 1:
                    e, rel2 = divmod(reg * 16 - p['offset'], 16)
                    base = rel2 // 4
                    return '%s[%d].%s' % (p['name'], e, ''.join(SWZ[base + SWZ.index(c)] for c in swz))
                if p['cols'] == 1 and not p['isMatrix']:
                    if p['arraySize']:
                        return parts[0] if len(swz) == 1 else '(%s).%s' % (parts[0], 'x' * len(swz))
                    return p['name'] if len(swz) == 1 else '(%s).%s' % (p['name'], 'x' * len(swz))
            if len(parts) == 1:
                return '(%s)' % parts[0]
            types = {PARAM_TYPE.get(find_param(cbname, reg * 16 + SWZ.index(c) * 4)['type'], 'float') for c in swz}
            if len(types) > 1:
                # parameters of different types in one read: pack their bits, a value conversion would change them
                return 'float%d(%s)' % (len(parts), ', '.join('vd_asfloat(%s)' % e for e in parts))
            return '%s%d(%s)' % (types.pop(), len(parts), ', '.join(parts))
        # dynamic index: "rN.w+K", "K+rN.w" or an expression without constant
        mm = re.fullmatch(r'(.+?)\s*\+\s*(\d+)', idx) or re.fullmatch(r'(\d+)\s*\+\s*(.+)', idx)
        if mm:
            if mm.group(1).strip().isdigit():
                k, expr = int(mm.group(1)), mm.group(2)
            else:
                expr, k = mm.group(1), int(mm.group(2))
        else:
            expr, k = idx, 0
        p = find_param(cbname, k * 16)
        if p is None or not p['arraySize']:
            errors.append('dynamic cb%d[%s] (%s) is not inside an array parameter' % (slot, idx, cbname))
            return m.group(0)
        if p['isMatrix']:
            errors.append('dynamic cb%d[%s] indexes matrix array %s' % (slot, idx, p['name']))
            return m.group(0)
        if (k * 16 - p['offset']) % 16:
            errors.append('dynamic cb%d[%s] base is not element-aligned in %s' % (slot, idx, p['name']))
            return m.group(0)
        e0 = (k * 16 - p['offset']) // 16
        index = '(int)(%s)%s' % (expr.strip(), ' + %d' % e0 if e0 else '')
        if p['cols'] == 1:
            if any(c != 'x' for c in swz):
                errors.append('dynamic cb%d[%s].%s on scalar array %s' % (slot, idx, swz, p['name']))
            return '%s[%s]' % (p['name'], index) if len(swz) == 1 else '(%s[%s]).%s' % (p['name'], index, 'x' * len(swz))
        for c in swz:
            if SWZ.index(c) >= p['cols']:
                errors.append('dynamic cb%d[%s].%s exceeds %s cols' % (slot, idx, swz, p['name']))
        return '%s[%s].%s' % (p['name'], index, swz)

    body = CB_RE.sub(cb_access, body)

    def tex_sub(m):
        slot = int(m.group(1))
        t = tex_by_slot.get(slot)
        if t is None:
            errors.append('texture t%d unnamed' % slot)
            return m.group(0)
        return t['name']

    body = TEX_RE.sub(tex_sub, body)

    def smp_sub(m):
        slot = int(m.group(1))
        nm = samp_by_slot.get(slot)
        if nm is None:
            errors.append('sampler s%d unnamed' % slot)
            return m.group(0)
        layout.sampler_names.add(nm)
        return nm

    body = SMP_RE.sub(smp_sub, body)
    body = re.sub(r'\b(asint|asuint|asfloat)\(', r'vd_\1(', body)
    if re.search(r'\bcb\d+\[', body):
        errors.append('unresolved cb access remains')
    return body


def dxbc_target(path):
    b = open(path, 'rb').read()
    n = struct.unpack_from('<I', b, 28)[0]
    for i in range(n):
        off = struct.unpack_from('<I', b, 32 + 4 * i)[0]
        if b[off:off + 4] in (b'SHDR', b'SHEX'):
            v = struct.unpack_from('<I', b, off + 8)[0]
            return (v >> 4) & 0xF, v & 0xF
    return 5, 0


# ---------------------------------------------------------------- keyword variants

def variants_of(subs):
    """Shipped variants of a stage minus instancing/stereo ones: list of (frozenset keywords, subprogram)."""
    out = {}
    for sp in sorted(subs, key=lambda s: (len(set(s['keywords']) & DROP_KEYWORDS), s['blob'])):
        kws = frozenset(k for k in sp['keywords'] if k not in DROP_KEYWORDS)
        out.setdefault(kws, sp)
    return out


URP_GROUPS = [
    ['FOG_LINEAR', 'FOG_EXP', 'FOG_EXP2'],
    ['_MAIN_LIGHT_SHADOWS', '_MAIN_LIGHT_SHADOWS_CASCADE', '_MAIN_LIGHT_SHADOWS_SCREEN'],
    ['_ADDITIONAL_LIGHTS_VERTEX', '_ADDITIONAL_LIGHTS'],
    ['_SHADOWS_SOFT', '_SHADOWS_SOFT_LOW', '_SHADOWS_SOFT_MEDIUM', '_SHADOWS_SOFT_HIGH'],
    ['PROBE_VOLUMES_L1', 'PROBE_VOLUMES_L2'],
    ['EVALUATE_SH_MIXED', 'EVALUATE_SH_VERTEX'],
    ['_ALPHAPREMULTIPLY_ON', '_ALPHAMODULATE_ON'],
    ['_DBUFFER_MRT1', '_DBUFFER_MRT2', '_DBUFFER_MRT3'],
    ['_DETAIL_MULX2', '_DETAIL_SCALED'],
    ['DIRECTIONAL', 'DIRECTIONAL_COOKIE', 'POINT', 'POINT_COOKIE', 'SPOT'],
    ['SHADOWS_DEPTH', 'SHADOWS_CUBE', 'SHADOWS_SCREEN'],
    ['LIGHTMAP_SHADOW_MIXING', 'SHADOWS_SHADOWMASK'],
]


def keyword_groups(stage_variants, props):
    """Partition the pass's keywords into `#pragma multi_compile` lines.

    Keywords of one KeywordEnum property and known URP sets share a line (mutually exclusive); every other keyword
    gets its own line, so any combination a material or script enables stays representable and the #if chain,
    not Unity's line order, decides which shipped variant is used. '_' is added when a stage shipped variants
    without any keyword of the line."""
    combos_by_stage = {st: list(v.keys()) for st, v in stage_variants.items()}
    all_combos = [c for cs in combos_by_stage.values() for c in cs]
    universe = sorted({k for c in all_combos for k in c})
    candidates = [list(g) for g in URP_GROUPS]
    for p in props:
        for a in p['m_Attributes']:
            m = re.match(r'KeywordEnum\s*\((.*)\)', a)
            if m:
                candidates.append(['%s_%s' % (p['m_Name'].upper(), v.strip().upper()) for v in m.group(1).split(',')])
    groups = []
    placed = set()
    for cand in candidates:
        members = [k for k in cand if k in universe and k not in placed]
        if len(members) < 2:
            continue
        if any(len(c & set(members)) > 1 for c in all_combos):
            continue  # not exclusive in the shipped data; keep separate lines
        groups.append(members)
        placed.update(members)
    # Remaining keywords: those sharing their first name token (BLEND_*, GRADIENT_LERP_*) that never occur together in
    # any shipped variant came from one `multi_compile a b c` line. Without this every keyword gets its own line and
    # the variant space (what a player build compiles) grows to 2^n instead of the original product of line sizes.
    def first_token(k):
        return k.lstrip('_').split('_')[0]

    def common_prefix(a, b):
        n = 0
        while n < min(len(a), len(b)) and a[n] == b[n]:
            n += 1
        return n

    cliques = []
    for k in universe:
        if k in placed:
            continue
        best = None
        for c in cliques:
            if first_token(c[0]) != first_token(k):
                continue
            if any(k in combo and m in combo for combo in all_combos for m in c):
                continue
            score = max(common_prefix(k, m) for m in c)
            if best is None or score > best[0]:
                best = (score, c)
        if best is not None:
            best[1].append(k)
        else:
            cliques.append([k])
    for c in cliques:
        groups.append(c)
        placed.update(c)
    result = []
    for g in groups:
        gs = set(g)
        stages = [st for st, cs in combos_by_stage.items() if any(c & gs for c in cs)]
        need_blank = any(not (c & gs) for st in stages for c in combos_by_stage[st])
        result.append((g, need_blank))
    result.sort(key=lambda r: r[0])
    return result


def dispatch_chain(variants, relpath_for):
    """#if chain: variant whose keyword set is the largest subset of the active keywords wins."""
    order = sorted(variants.keys(), key=lambda c: (-len(c), sorted(c)))
    lines = []
    for i, c in enumerate(order):
        cond = ' && '.join('defined(%s)' % k for k in sorted(c)) or '1'
        lines.append(('#if ' if i == 0 else '#elif ') + cond)
        lines.append('#include "%s"' % relpath_for(variants[c]))
    if order and order[-1]:
        # no shipped variant is a subset of the active keywords: fall back to the smallest one
        lines.append('#else')
        lines.append('#include "%s"' % relpath_for(variants[order[-1]]))
    lines.append('#endif')
    return lines


# ---------------------------------------------------------------- shader

def generate(sdir, out_dir, report):
    s = json.load(open(os.path.join(sdir, 'shader.json')))
    name = s['name']
    san = sanitize(name)
    inc_dir = os.path.join(out_dir, san)
    errors = []
    warnings = []
    body3dm = []  # blobs left with 3Dmigoto's body (no disassembly or an instruction asm2hlsl does not know)
    files = {}  # relpath -> text
    L = ['Shader "%s"' % name, '{', 'Properties', '{']
    for p in s['props']:
        L.append('    ' + prop_line(p))
    L.append('}')
    targets = set()
    for si, ss in enumerate(s['subshaders']):
        if not ss['passes']:
            # The player build strips subshaders of other pipelines down to their tags; ShaderLab rejects a SubShader
            # without passes, and nothing could render with it anyway.
            L.append('// SubShader %d (%s) shipped without passes' % (si, tags_line(ss['tags'])))
            continue
        L.append('SubShader')
        L.append('{')
        if ss['lod']:
            L.append('    LOD %d' % ss['lod'])
        if ss['tags']:
            L.append('    ' + tags_line(ss['tags']))
        for pi, pa in enumerate(ss['passes']):
            if pa['type'] == 1:
                L.append('    UsePass "%s"' % pa['useName'])
                continue
            if pa['type'] == 2:
                L.append('    GrabPass { %s}' % ('"%s" ' % pa['textureName'] if pa['textureName'] else ''))
                continue
            st = pa['state']
            L.append('    Pass')
            L.append('    {')
            if st['m_Name']:
                L.append('        Name "%s"' % st['m_Name'])
            tags = list(st['m_Tags']['tags'])
            for t in pa['tags']:
                if t not in tags:
                    tags.append(t)
            if tags:
                L.append('        ' + tags_line(tags))
            for ln in state_lines(st):
                L.append('        ' + ln)
            stage_variants = {}
            for stage, subs in pa['programs'].items():
                if stage not in STAGE_ENTRY:
                    raise GenError('stage %s not supported' % stage)
                stage_variants[stage] = variants_of(subs)
            if not stage_variants:
                L.append('    }')
                continue
            layout = Layout()
            for stage, vs in stage_variants.items():
                for sp in vs.values():
                    layout.add_params(sp['params'])
            # per-variant include files
            inc_names = {}
            perr = []
            for stage, vs in stage_variants.items():
                for kws, sp in vs.items():
                    hp = os.path.join(sdir, 'dxbc', '%d.hlsl' % sp['blob'])
                    if not os.path.exists(hp):
                        perr.append('variant %s %s blob %d has no decompiled HLSL' % (st['m_Name'], stage, sp['blob']))
                        continue
                    dp = os.path.join(sdir, 'dxbc', '%d.dxbc' % sp['blob'])
                    targets.add(dxbc_target(dp))
                    verr = []
                    src = open(hp, encoding='utf-8', errors='replace').read()
                    ap = os.path.join(sdir, 'dxbc', '%d.asm' % sp['blob'])
                    try:
                        src = asm2hlsl.translate(open(ap).read(), src)
                    except (OSError, asm2hlsl.Unsupported) as e:
                        warnings.append('blob %d keeps the 3Dmigoto body: %s' % (sp['blob'], e))
                        body3dm.append(sp['blob'])
                    body = rewrite_variant(src, sp['params'], STAGE_ENTRY[stage], layout, verr)
                    if verr:
                        perr += ['%s %s blob %d: %s' % (st['m_Name'], stage, sp['blob'], e) for e in sorted(set(verr))]
                    rel = 'p%d_%s_%d.hlsl' % (pi, STAGE_SHORT[stage], sp['blob'])
                    head = '// %s | pass %d "%s" | %s | keywords: %s | blob %d\n' % (name, pi, st['m_Name'], stage, ' '.join(sorted(kws)) or '(none)', sp['blob'])
                    files[os.path.join(san, rel)] = head + body + '\n'
                    inc_names[(stage, kws)] = rel
            if perr:
                errors += perr
            warnings += layout.warnings
            # program block
            P = ['HLSLPROGRAM']
            tmax = max(targets) if targets else (5, 0)
            P.append('#pragma target %d.%d' % tmax)
            for stage in stage_variants:
                P.append('#pragma %s %s' % (STAGE_PRAGMA[stage], STAGE_ENTRY[stage]))
            kwflags = dict(zip(s['keywordNames'], s['keywordFlags'] or []))
            for group, need_blank in keyword_groups(stage_variants, s['props']):
                local = all(kwflags.get(k, 0) & 1 for k in group)
                P.append('#pragma multi_compile%s %s%s' % ('_local' if local else '', '_ ' if need_blank else '', ' '.join(group)))
            P.append('')
            P.append(PRELUDE)
            # declarations
            for cbname in sorted(layout.cbuffers):
                params = sorted(layout.cbuffers[cbname].values(), key=lambda p: p['offset'])
                if cbname == '$Globals':
                    P.append('// $Globals: bound per variable by name')
                    for p in params:
                        P.append(decl(p, False))
                else:
                    P.append('cbuffer %s' % cbname)
                    P.append('{')
                    for p in params:
                        P.append('    ' + decl(p, True))
                    P.append('}')
            P.append('')
            for tname in sorted(layout.textures):
                tt = layout.tex_types.get(tname)
                if tt is None:
                    dim = layout.textures[tname]
                    tt = {2: 'Texture2D<float4>', 3: 'Texture3D<float4>', 4: 'TextureCube<float4>', 5: 'Texture2DArray<float4>', 6: 'TextureCubeArray<float4>'}.get(dim, 'Texture2D<float4>')
                P.append('%s %s;' % (tt, tname))
            for sname in sorted(layout.sampler_names):
                P.append('%s %s;' % (layout.sampler_types.get(sname, 'SamplerState'), sname))
            P.append('')
            for stage, vs in stage_variants.items():
                P.append('// ---- %s' % stage)
                P += dispatch_chain(vs, lambda sp, stage=stage: '%s/%s' % (san, inc_names[(stage, frozenset(k for k in sp['keywords'] if k not in DROP_KEYWORDS))]))
            P.append('ENDHLSL')
            for ln in P:
                L.append('        ' + ln if ln else '')
            L.append('    }')
        L.append('}')
    if s['fallback']:
        L.append('Fallback "%s"' % s['fallback'])
    if s['customEditor']:
        L.append('CustomEditor "%s"' % s['customEditor'])
    for ce in s.get('customEditorForRenderPipelines') or []:
        L.append('CustomEditorForRenderPipeline "%s" "%s"' % (ce['customEditorName'], ce['renderPipelineType']))
    L.append('}')
    if errors:
        report[name] = {'status': 'failed', 'errors': sorted(set(errors))[:40], 'warnings': sorted(set(warnings))[:20]}
        return False
    os.makedirs(inc_dir, exist_ok=True)
    # drop stale includes from a previous run
    for f in os.listdir(inc_dir):
        if f.endswith('.hlsl') or f.endswith('.hlsl.meta'):
            if os.path.join(san, f.replace('.meta', '')) not in files:
                os.remove(os.path.join(inc_dir, f))
    for rel, text in files.items():
        p = os.path.join(out_dir, rel)
        with open(p, 'w', encoding='utf-8', newline='\n') as f:
            f.write(text)
        with open(p + '.meta', 'w', newline='\n') as f:
            f.write('fileFormatVersion: 2\nguid: %s\nShaderIncludeImporter:\n  externalObjects: {}\n  userData: \n  assetBundleName: \n  assetBundleVariant: \n' % guid_for(name + ':' + rel.replace(os.sep, '/')))
    with open(inc_dir + '.meta', 'w', newline='\n') as f:
        f.write('fileFormatVersion: 2\nguid: %s\nfolderAsset: yes\nDefaultImporter:\n  externalObjects: {}\n  userData: \n  assetBundleName: \n  assetBundleVariant: \n' % guid_for(name + ':dir'))
    sp = os.path.join(out_dir, san + '.shader')
    with open(sp, 'w', encoding='utf-8', newline='\n') as f:
        f.write('\n'.join(L) + '\n')
    with open(sp + '.meta', 'w', newline='\n') as f:
        f.write('fileFormatVersion: 2\nguid: %s\nShaderImporter:\n  externalObjects: {}\n  defaultTextures: []\n  nonModifiableTextures: []\n  preprocessorOverride: 0\n  userData: \n  assetBundleName: \n  assetBundleVariant: \n' % guid_for(name))
    report[name] = {'status': 'ok', 'file': sp, 'variants': len(files), 'body3dm': len(set(body3dm)), 'warnings': sorted(set(warnings))[:20]}
    return True


def main():
    args = sys.argv[1:]
    work = args.pop(0)
    out_dir = args.pop(0)
    only = None
    report_path = None
    names = None
    while args:
        a = args.pop(0)
        if a == '--report':
            report_path = args.pop(0)
        elif a == '--names':
            names = {sanitize(n) for n in json.load(open(args.pop(0)))}
        elif a == '--only':
            only = set(args)
            break
    os.makedirs(out_dir, exist_ok=True)
    report = {}
    ok = fail = 0
    for d in sorted(os.listdir(work)):
        if (only and d not in only) or (names is not None and d not in names):
            continue
        sdir = os.path.join(work, d)
        if not os.path.exists(os.path.join(sdir, 'shader.json')):
            continue
        try:
            if generate(sdir, out_dir, report):
                ok += 1
            else:
                fail += 1
        except GenError as e:
            name = json.load(open(os.path.join(sdir, 'shader.json')))['name']
            report[name] = {'status': 'failed', 'errors': [str(e)]}
            fail += 1
        except Exception as e:  # keep going, report
            import traceback
            name = json.load(open(os.path.join(sdir, 'shader.json')))['name']
            report[name] = {'status': 'failed', 'errors': [traceback.format_exc().splitlines()[-1]]}
            fail += 1
    print('generated %d, failed %d' % (ok, fail))
    for n, r in sorted(report.items()):
        if r['status'] != 'ok':
            print('FAILED', n, r['errors'][:5])
    if report_path:
        with open(report_path, 'w') as f:
            json.dump(report, f, indent=1)


if __name__ == '__main__':
    main()
