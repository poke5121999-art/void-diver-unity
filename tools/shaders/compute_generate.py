#!/usr/bin/env python3
"""Generate .compute files from extracted + disassembled game compute shaders.

    compute_generate.py <cx_dir> <out_dir> --donor <donor_dir> [--report report.json]

<cx_dir> is compute_extract.py's output with each D3D11 blob disassembled next to it (<kernel>_<v>.asm, 3Dmigoto
cmd_Decompiler -d). For every compute shader that no donor package ships under the same file name this writes
<out_dir>/<name>.compute (+ .meta, GUID = md5("vdcompute:" + name)): one `#pragma kernel` per shipped kernel, the
$Globals parameters each kernel reads, textures / structured buffers / typed UAVs under their Unity binding names, and
each kernel body as asm2hlsl.translate_cs's bit-exact translation of the disassembly (no 3Dmigoto HLSL involved).

Only what the game shipped is emitted: kernels, thread group sizes and bindings come from the ComputeShader object;
a kernel with keywords or more than one D3D11 variant, or an instruction asm2hlsl does not know, fails the shader
(reported, nothing written for it) instead of being guessed.

Per-API typed UAV formats (GL_UAV_TYPES): D3D11 reads any UNORM/FLOAT render texture through a `float4` UAV, but
OpenGL's imageLoad reinterprets the bits with the image's declared layout qualifier, which Unity's HLSL->GLSL derives
from the HLSL element type (float4 -> rgba32f). A UAV the kernel *reads* therefore gets, on OpenGL only, the element
type matching the render texture the game's code binds to it (format named next to each entry). Stores are not
affected (a writeonly image needs no format).
"""
import hashlib
import json
import os
import re
import sys

sys.path.insert(0, os.path.dirname(os.path.abspath(__file__)))
import asm2hlsl  # noqa: E402
from generate import PRELUDE, PARAM_TYPE, GenError, rewrite_variant, Layout  # noqa: E402

D3D11 = 2
SHADER_DONORS = ['com.unity.render-pipelines.universal', 'com.unity.render-pipelines.core', 'com.unity.ugui',
                 'com.unity.shadergraph']
# (shader, UAV) -> OpenGL element type of a UAV the kernel loads from, matching the RenderTexture the code binds
GL_UAV_TYPES = {
}
TEX_TYPE = {'texture1d': 'Texture1D<float4>', 'texture2d': 'Texture2D<float4>', 'texture3d': 'Texture3D<float4>',
            'texture2darray': 'Texture2DArray<float4>', 'texturecube': 'TextureCube<float4>'}
UAV_TYPE = {'texture1d': 'RWTexture1D', 'texture2d': 'RWTexture2D', 'texture3d': 'RWTexture3D',
            'texture2darray': 'RWTexture2DArray'}
RET_TYPE = {'float': 'float', 'unorm': 'unorm float', 'snorm': 'snorm float', 'uint': 'uint', 'sint': 'int'}

UAV_HELPERS = '''// typed UAV loads widen to 4 components like a D3D typed load of a narrower format (missing: 0, 0, 1)
float4 vd_uav4(float4 v) { return v; } float4 vd_uav4(float3 v) { return float4(v, 1.0); }
float4 vd_uav4(float2 v) { return float4(v, 0.0, 1.0); } float4 vd_uav4(float v) { return float4(v, 0.0, 0.0, 1.0); }
'''


def guid_for(name):
    return hashlib.md5(('vdcompute:' + name).encode('utf-8')).hexdigest()


def meta_text(guid):
    return ('fileFormatVersion: 2\nguid: %s\nComputeShaderImporter:\n  externalObjects: {}\n  preprocessorOverride: 0\n'
            '  userData: \n  assetBundleName: \n  assetBundleVariant: \n' % guid)


def donor_provides(donor, name):
    for d in SHADER_DONORS:
        for dp, _, fs in os.walk(os.path.join(donor, d)):
            if name + '.compute' in fs:
                return os.path.relpath(os.path.join(dp, name + '.compute'), donor)
    return None


def gen_params(cb):
    """Unity ComputeShader cbuffer params -> generate.py's parameter records."""
    out = []
    for p in cb['params']:
        if p['rowCount'] > 1:
            raise GenError('matrix parameter %s not handled' % p['name'])
        out.append({'name': p['name'], 'type': p['type'], 'offset': p['offset'], 'rows': p['rowCount'],
                    'cols': p['colCount'], 'isMatrix': 0, 'arraySize': p['arraySize']})
    return {'name': cb['name'], 'size': cb['byteSize'], 'params': out}


def generate(sdir, out_dir, report):
    cj = json.load(open(os.path.join(sdir, 'compute.json')))
    name = cj['name']
    var = [v for v in cj['variants'] if v['targetRenderer'] == D3D11]
    if len(var) != 1:
        raise GenError('%d D3D11 variants' % len(var))
    var = var[0]
    cbs = [gen_params(cb) for cb in var['constantBuffers']]
    errors, kernels = [], []
    globals_ = {}   # name -> param record (union over kernels, checked consistent)
    textures, structured, uavs = {}, {}, {}  # name -> dim / stride / (dim, return types, loaded?)
    for k in var['kernels']:
        kw = (k.get('globalKeywords') or []) + (k.get('localKeywords') or []) + (k.get('dynamicKeywords') or [])
        if kw or len(k['uniqueVariants']) != 1:
            raise GenError('kernel %s: keywords %r, %d variants (keyword variants are not generated)'
                           % (k['name'], kw, len(k['uniqueVariants'])))
        u = k['uniqueVariants'][0]
        if u.get('builtinSamplers'):
            raise GenError('kernel %s uses inline samplers %r' % (k['name'], u['builtinSamplers']))
        asm = open(os.path.join(sdir, 'dxbc', u['dxbc'][:-5] + '.asm')).read()
        names = {}
        for b in u['textures'] + u['inBuffers']:
            names['t%d' % b['bindPoint']] = b['name']
        for b in u['outBuffers']:
            names['u%d' % b['bindPoint']] = b['name']
        try:
            info, fn = asm2hlsl.translate_cs(asm, k['name'], names)
        except asm2hlsl.Unsupported as e:
            raise GenError('kernel %s: %s' % (k['name'], e))
        if info['threadGroup'] != list(u['threadGroupSize']):
            raise GenError('kernel %s: thread group %r in DXBC, %r in Unity data' % (k['name'], info['threadGroup'], u['threadGroupSize']))
        for slot, nm in names.items():
            if slot.startswith('t'):
                if slot in info['structured']:
                    kind = ('structured', info['structured'][slot])
                    prev = structured.setdefault(nm, kind[1])
                elif slot in info['textures']:
                    kind = ('texture', info['textures'][slot])
                    prev = textures.setdefault(nm, kind[1])
                else:
                    continue  # bound by Unity but not declared by the program
                if prev != kind[1] or (nm in structured and nm in textures):
                    raise GenError('%s declared differently between kernels' % nm)
            else:
                if slot not in info['uav']:
                    continue
                dim, rt = info['uav'][slot]
                loaded = re.search(r'ld_uav_typed\S*\([^)]*\)\s+\S+,\s*\S+,\s*%s\b' % slot, asm) is not None
                prev = uavs.get(nm)
                if prev and prev[:2] != (dim, rt):
                    raise GenError('UAV %s declared differently between kernels' % nm)
                uavs[nm] = (dim, rt, loaded or (prev and prev[2]))
        cb = cbs[u['cbVariantIndices'][0]] if u['cbVariantIndices'] else None
        params = {'cbBindings': [{'index': c['bindPoint'], 'name': c['name']} for c in u['cbs']],
                  'cbuffers': [cb] if cb else [], 'textures': [], 'samplers': []}
        for c in u['cbs']:
            if c['name'] != '$Globals':
                raise GenError('cbuffer %s (only $Globals is handled)' % c['name'])
        fn_lines = fn.splitlines()
        head = fn_lines[0]  # [numthreads(...)]
        text = '\n'.join(fn_lines[1:]).replace('void %s(' % k['name'], 'void main(', 1)
        errs = []
        body = rewrite_variant(text, params, k['name'], Layout(), errs)
        if errs:
            raise GenError('kernel %s: %s' % (k['name'], '; '.join(sorted(set(errs)))[:400]))
        used = set(re.findall(r'\b[A-Za-z_]\w*\b', body))
        for p in (cb['params'] if cb else []):
            if p['name'] not in used:
                continue
            q = globals_.setdefault(p['name'], p)
            if (q['type'], q['cols'], q['arraySize']) != (p['type'], p['cols'], p['arraySize']):
                raise GenError('parameter %s differs between kernels' % p['name'])
        kernels.append((k['name'], head + '\n' + body, info))

    L = ['// %s: rebuilt from the game\'s D3D11 bytecode by tools/shaders/compute_generate.py (do not edit).' % name,
         '// Kernel bodies are asm2hlsl translations of the shipped DXBC; names and bindings from the ComputeShader object.']
    for kn, _, _ in kernels:
        L.append('#pragma kernel %s' % kn)
    L.append('')
    L.append(PRELUDE)
    L.append(UAV_HELPERS)
    L.append('// $Globals: bound per variable by name')
    for p in sorted(globals_.values(), key=lambda p: p['offset']):
        t = PARAM_TYPE.get(p['type'], 'float')
        t = t if p['cols'] == 1 else '%s%d' % (t, p['cols'])
        L.append('%s %s%s;' % (t, p['name'], '[%d]' % p['arraySize'] if p['arraySize'] else ''))
    L.append('')
    for nm in sorted(textures):
        L.append('%s %s;' % (TEX_TYPE[textures[nm]], nm))
    for stride in sorted(set(structured.values())):
        L.append('struct vd_s%d { uint v[%d]; };  // raw %d-byte element; the kernels reinterpret each dword' % (stride, stride // 4, stride))
    for nm in sorted(structured):
        L.append('StructuredBuffer<vd_s%d> %s;' % (structured[nm], nm))
    gl_used = {}
    for nm in sorted(uavs):
        dim, rt, loaded = uavs[nm]
        if len(set(rt)) != 1:
            raise GenError('UAV %s mixed return types %r' % (nm, rt))
        d3d = '%s%d' % (RET_TYPE[rt[0]], len(rt))
        gl = GL_UAV_TYPES.get((name, nm))
        if gl:
            gl_used[nm] = gl[0]
            L.append('#if defined(SHADER_API_GLCORE) || defined(SHADER_API_GLES3)')
            L.append('typedef %s vd_t_%s;  // %s' % (gl[0], nm, gl[1]))
            L.append('#else')
            L.append('typedef %s vd_t_%s;' % (d3d, nm))
            L.append('#endif')
        else:
            if loaded:
                report.setdefault('warnings', []).append('%s: UAV %s is loaded but has no OpenGL format entry' % (name, nm))
            L.append('typedef %s vd_t_%s;' % (d3d, nm))
        L.append('%s<vd_t_%s> %s;' % (UAV_TYPE[dim], nm, nm))
    L.append('')
    for kn, body, _ in kernels:
        L.append(body)
    text = '\n'.join(L).rstrip() + '\n'
    open(os.path.join(out_dir, name + '.compute'), 'w').write(text)
    open(os.path.join(out_dir, name + '.compute.meta'), 'w').write(meta_text(guid_for(name)))
    return {'status': 'ok', 'guid': guid_for(name),
            'kernels': {kn: {'threadGroup': info['threadGroup']} for kn, _, info in kernels},
            'globals': sorted(globals_), 'textures': sorted(textures), 'structured': structured,
            'uav': {nm: {'d3d': '%s%d' % (RET_TYPE[v[1][0]], len(v[1])), 'gl': gl_used.get(nm), 'loaded': bool(v[2])}
                    for nm, v in uavs.items()}}


def main():
    args = sys.argv[1:]
    cx, out_dir = args.pop(0), args.pop(0)
    donor, report_path = None, None
    while args:
        a = args.pop(0)
        if a == '--donor':
            donor = args.pop(0)
        elif a == '--report':
            report_path = args.pop(0)
    os.makedirs(out_dir, exist_ok=True)
    report = {'shaders': {}}
    for d in sorted(os.listdir(cx)):
        if not os.path.exists(os.path.join(cx, d, 'compute.json')):
            continue
        name = json.load(open(os.path.join(cx, d, 'compute.json')))['name']
        hit = donor and donor_provides(donor, name)
        if hit:
            report['shaders'][name] = {'status': 'package', 'path': hit}
            continue
        try:
            report['shaders'][name] = generate(os.path.join(cx, d), out_dir, report)
        except GenError as e:
            report['shaders'][name] = {'status': 'failed', 'errors': [str(e)]}
    for n, r in sorted(report['shaders'].items()):
        print(n, r['status'], ' '.join(r.get('kernels', {})) or r.get('path', '') or r.get('errors'))
    for w in report.get('warnings', []):
        print('WARNING', w)
    if report_path:
        with open(report_path, 'w') as f:
            json.dump(report, f, indent=1)
    return 1 if any(r['status'] == 'failed' for r in report['shaders'].values()) else 0


if __name__ == '__main__':
    sys.exit(main())
