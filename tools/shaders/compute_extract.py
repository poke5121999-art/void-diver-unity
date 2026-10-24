#!/usr/bin/env python3
"""Extract the game's compiled compute shaders (kernels, bindings, cbuffer layouts, D3D11 DXBC).

    compute_extract.py <out_dir> [--list files.txt] <container files...>

For every ComputeShader object in the given Unity containers this writes
    <out_dir>/<name>/compute.json   per target renderer: kernels (name, thread group size, keywords, bindings of each
                                    unique variant: cbs / textures / inBuffers / outBuffers / builtinSamplers with
                                    bind points), constant buffers (params with type/offset/rows/cols/arraySize)
    <out_dir>/<name>/dxbc/<kernel>_<v>.dxbc   the D3D11 (targetRenderer 2) DXBC of unique variant v of each kernel
and <out_dir>/index.json: every ComputeShader name seen per container. A name seen in several containers keeps the
copy with the most unique variants. Read with the UnityPy venv and -I: the inputs are untrusted game files.
"""
import json
import os
import sys

import UnityPy

D3D11 = 2  # GfxDeviceRenderer: 2 D3D11, 17 OpenGLCore, 18 D3D12, 21 Vulkan


def plain(o):
    if isinstance(o, dict):
        return {k: plain(v) for k, v in o.items()}
    if isinstance(o, list):
        return [plain(v) for v in o]
    if isinstance(o, (bytes, bytearray)):
        return '<bytes %d>' % len(o)
    return o


def extract(t, out_dir, container):
    name = t['m_Name']
    d = os.path.join(out_dir, name)
    total = sum(len(k['uniqueVariants']) for v in t['variants'] for k in v['kernels'])
    prev = os.path.join(d, 'compute.json')
    if os.path.exists(prev) and json.load(open(prev)).get('uniqueVariants', 0) >= total:
        return {'name': name, 'skipped': 'kept copy with as many variants'}
    os.makedirs(os.path.join(d, 'dxbc'), exist_ok=True)
    out = {'name': name, 'container': container, 'uniqueVariants': total, 'variants': []}
    for v in t['variants']:
        vj = {'targetRenderer': v['targetRenderer'], 'targetLevel': v['targetLevel'],
              'constantBuffers': plain(v['constantBuffers']), 'resourcesResolved': v.get('resourcesResolved'),
              'kernels': []}
        for k in v['kernels']:
            kj = {'name': k['name'], 'variantIndices': plain(k.get('variantIndices')),
                  'globalKeywords': plain(k.get('globalKeywords')), 'localKeywords': plain(k.get('localKeywords')),
                  'dynamicKeywords': plain(k.get('dynamicKeywords')), 'uniqueVariants': []}
            for i, u in enumerate(k['uniqueVariants']):
                code = bytes(u['code'])
                uj = {kk: plain(vv) for kk, vv in u.items() if kk != 'code'}
                uj['codeLen'] = len(code)
                if v['targetRenderer'] == D3D11:
                    if code[:4] != b'DXBC':
                        raise ValueError('%s %s: D3D11 code is not DXBC' % (name, k['name']))
                    fn = '%s_%d.dxbc' % (k['name'], i)
                    open(os.path.join(d, 'dxbc', fn), 'wb').write(code)
                    uj['dxbc'] = fn
                kj['uniqueVariants'].append(uj)
            vj['kernels'].append(kj)
        out['variants'].append(vj)
    with open(prev, 'w') as f:
        json.dump(out, f, indent=1)
    return {'name': name, 'kernels': sorted({k['name'] for v in t['variants'] for k in v['kernels']}),
            'renderers': [v['targetRenderer'] for v in t['variants']], 'uniqueVariants': total}


def main():
    args = sys.argv[1:]
    out_dir = args.pop(0)
    files = []
    while args:
        a = args.pop(0)
        if a == '--list':
            files += [ln.rstrip('\n') for ln in open(args.pop(0)) if ln.strip()]
        else:
            files.append(a)
    os.makedirs(out_dir, exist_ok=True)
    index_path = os.path.join(out_dir, 'index.json')
    index = json.load(open(index_path)) if os.path.exists(index_path) else {}
    for path in files:
        env = UnityPy.load(path)
        found = []
        for o in env.objects:
            if o.type.name != 'ComputeShader':
                continue
            t = o.read_typetree()
            found.append(t['m_Name'])
            print(json.dumps(extract(t, out_dir, os.path.basename(path))))
        index[os.path.basename(path)] = sorted(found)
        print(path, 'compute shaders:', len(found), file=sys.stderr)
        with open(index_path, 'w') as f:
            json.dump(index, f, indent=1)


if __name__ == '__main__':
    main()
