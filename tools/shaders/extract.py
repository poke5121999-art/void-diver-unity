#!/usr/bin/env python3
"""Extract the game's compiled shaders (parsed ShaderLab form + D3D11 DXBC blobs + constant-buffer layouts).

    extract.py <out_dir> [--names names.json] [--index-only] [--list files.txt] <container files...>

For every Shader object in the given Unity containers (Addressables .bundle, data.unity3d, resources.assets)
whose name is in names.json (or every shader when --names is omitted) this writes
    <out_dir>/<sanitized name>/shader.json      parsed form: properties, subshaders, passes, state, keywords,
                                                per-subprogram keyword sets and merged parameter layouts
    <out_dir>/<sanitized name>/dxbc/<n>.dxbc    the raw DXBC container of blob entry n (RDEF stripped by Unity)
and <out_dir>/index.json listing every shader name seen per container (for locating shaders).

Facts about the Unity 6 blob layout this relies on (measured on the game's data, see docs/SHADERS.md):
    blob = int32 count, then count entries of (int32 offset, int32 length, int32 segment) ...
    subprogram entry = int32 version(202012090), int32 gpuProgramType, 4 x int32 stats, keywords (int32 n, aligned
        strings), int32 codeLen + code (Unity header, then "DXBC"...), int32 sourceMap, int32 n + n x (source, target)
    parameter entry = int32 version, 5 x int32, then cbuffers until the binding list:
        cbuffer = string name, int32 size, int32 n + n x (string name, int32 type, rows, cols, isMatrix, arraySize,
        offset), int32 structCount + structs (name, index, arraySize, size, int32 n + n member records as above;
        only instancing variants have them)
        bindings = int32 n + n x (string name, int32 kind, int32 index, int32 extra [, int32 dim if kind == 0])
        kind 0 = texture (extra = coupled sampler slot or -1), 1 = constant buffer binding (index = slot),
        4 = inline sampler state (index = slot, extra = Unity sampler flags)
    A parameter entry only lists what is *not* in the program's m_CommonParameters; both are merged here.
Run with the UnityPy venv and -I since the inputs are untrusted game files.
"""
import hashlib
import json
import os
import re
import struct
import sys

import UnityPy
from UnityPy.helpers import CompressionHelper

PLATFORM_D3D11 = 4
PARAM_HEADER_MAGIC = 202012090


def sanitize(name):
    return re.sub(r'[^A-Za-z0-9_.-]+', '_', name).strip('_')


def walk_fields(o, depth=0):
    """Turn a UnityPy generated object tree into plain JSON data."""
    if depth > 24:
        return '...'
    if isinstance(o, (int, float, str, bool)) or o is None:
        return o
    if isinstance(o, (bytes, bytearray)):
        return '<bytes %d>' % len(o)
    if isinstance(o, (list, tuple)):
        return [walk_fields(x, depth + 1) for x in o]
    if isinstance(o, dict):
        return {k: walk_fields(v, depth + 1) for k, v in o.items()}
    ann = getattr(type(o), '__annotations__', None)
    if ann:
        return {k: walk_fields(getattr(o, k, None), depth + 1) for k in ann}
    return str(o)


class Reader:
    def __init__(self, b):
        self.b = b
        self.pos = 0

    def i32(self):
        v = struct.unpack_from('<i', self.b, self.pos)[0]
        self.pos += 4
        return v

    def string(self):
        n = self.i32()
        v = self.b[self.pos:self.pos + n].decode('utf-8', errors='replace')
        self.pos += (n + 3) & ~3
        return v

    def left(self):
        return len(self.b) - self.pos


def parse_subprogram_entry(b):
    r = Reader(b)
    version = r.i32()
    gpu_type = r.i32()
    stats = [r.i32() for _ in range(4)]
    kw = [r.string() for _ in range(r.i32())]
    code_len = r.i32()
    code = b[r.pos:r.pos + code_len]
    r.pos += (code_len + 3) & ~3
    source_map = r.i32() if r.left() >= 4 else 0
    channels = []
    if r.left() >= 4:
        for _ in range(r.i32()):
            channels.append((r.i32(), r.i32()))
    j = code.find(b'DXBC')
    dxbc = None
    if j >= 0:
        total = struct.unpack_from('<I', code, j + 24)[0]
        dxbc = code[j:j + total]
    return {'version': version, 'gpuType': gpu_type, 'stats': stats, 'keywords': kw, 'sourceMap': source_map,
            'channels': channels, 'unityHeader': code[:j].hex() if j >= 0 else code[:64].hex()}, dxbc


def parse_param_entry(b):
    r = Reader(b)
    version = r.i32()
    assert version == PARAM_HEADER_MAGIC, 'parameter entry version %d not understood' % version
    head = [r.i32() for _ in range(5)]
    cbuffers = []
    # cbuffers follow without a count; a binding list (int32 n, then records) closes the entry. Distinguish by
    # trying to read a cbuffer record: a cbuffer name is a non-empty identifier-like string and size >= 0.
    while r.left() > 0:
        save = r.pos
        n = r.i32()
        ok = 0 < n < 256 and r.left() >= n
        if ok:
            raw = r.b[r.pos:r.pos + n]
            ok = all(32 < c < 127 for c in raw) and r.left() >= ((n + 3) & ~3) + 8
        r.pos = save
        if not ok:
            break
        name = r.string()
        size = r.i32()
        params = []
        for _ in range(r.i32()):
            pname = r.string()
            ptype, rows, cols, is_matrix, array_size, offset = [r.i32() for _ in range(6)]
            params.append({'name': pname, 'type': ptype, 'rows': rows, 'cols': cols, 'isMatrix': bool(is_matrix),
                           'arraySize': array_size, 'offset': offset})
        struct_count = r.i32()
        structs = []
        for _ in range(struct_count):
            # struct = name, index, arraySize, structSize, member count, members (same 6-int record as a parameter)
            sname = r.string()
            sidx, sarr, ssize = r.i32(), r.i32(), r.i32()
            members = []
            for _ in range(r.i32()):
                mn = r.string()
                mtype, mrows, mcols, mmat, marr, moff = [r.i32() for _ in range(6)]
                members.append({'name': mn, 'type': mtype, 'rows': mrows, 'cols': mcols, 'isMatrix': bool(mmat), 'arraySize': marr, 'offset': moff})
            structs.append({'name': sname, 'index': sidx, 'arraySize': sarr, 'size': ssize, 'members': members})
        cbuffers.append({'name': name, 'size': size, 'params': params, 'structs': structs})
        # A cbuffer record is followed by another cbuffer or by the binding count. Peek: if the next int is a
        # plausible string length followed by printable bytes it is a cbuffer name, else it is the binding count.
    textures, cb_bindings, samplers, buffers, uavs, others = [], [], [], [], [], []
    if r.left() >= 4:
        for _ in range(r.i32()):
            name = r.string()
            kind, index, extra = r.i32(), r.i32(), r.i32()
            if kind == 0:
                dim = r.i32()
                # the blob stores dim << 1 (2D = 4, Cube = 8); m_CommonParameters stores TextureDimension (2D = 2)
                textures.append({'name': name, 'index': index, 'samplerIndex': extra, 'dim': dim >> 1, 'multiSampled': bool(dim & 1)})
            elif kind == 1:
                cb_bindings.append({'name': name, 'index': index, 'arraySize': extra})
            elif kind == 4:
                samplers.append({'index': index, 'flags': extra})
            elif kind == 2:
                uavs.append({'name': name, 'index': index, 'originalIndex': extra})
            elif kind == 3:
                buffers.append({'name': name, 'index': index, 'arraySize': extra})
            else:
                others.append({'name': name, 'kind': kind, 'index': index, 'extra': extra})
    return {'head': head, 'cbuffers': cbuffers, 'textures': textures, 'cbBindings': cb_bindings, 'samplers': samplers,
            'buffers': buffers, 'uavs': uavs, 'others': others, 'trailing': r.left()}


def common_params(prog, names):
    """m_CommonParameters of a SerializedProgram, in the same shape as parse_param_entry."""
    cp = prog.get('m_CommonParameters') or {}
    nm = lambda i: names[i]
    cbuffers = []
    for cb in cp.get('m_ConstantBuffers') or []:
        params = []
        for v in cb.get('m_VectorParams') or []:
            params.append({'name': nm(v['m_NameIndex']), 'type': v['m_Type'], 'rows': 1, 'cols': v['m_Dim'],
                           'isMatrix': False, 'arraySize': v['m_ArraySize'], 'offset': v['m_Index']})
        for m in cb.get('m_MatrixParams') or []:
            params.append({'name': nm(m['m_NameIndex']), 'type': m['m_Type'], 'rows': m['m_RowCount'], 'cols': 4,
                           'isMatrix': True, 'arraySize': m['m_ArraySize'], 'offset': m['m_Index']})
        structs = []
        for s in cb.get('m_StructParams') or []:
            structs.append({'name': nm(s['m_NameIndex']), 'index': s['m_Index'], 'arraySize': s['m_ArraySize'],
                            'size': s['m_StructSize'],
                            'vectors': [{'name': nm(v['m_NameIndex']), 'dim': v['m_Dim'], 'type': v['m_Type'],
                                         'offset': v['m_Index'], 'arraySize': v['m_ArraySize']} for v in s.get('m_VectorMembers') or []],
                            'matrices': [{'name': nm(v['m_NameIndex']), 'rows': v['m_RowCount'], 'type': v['m_Type'],
                                          'offset': v['m_Index'], 'arraySize': v['m_ArraySize']} for v in s.get('m_MatrixMembers') or []]})
        cbuffers.append({'name': nm(cb['m_NameIndex']), 'size': cb['m_Size'], 'params': params, 'structs': structs,
                         'partial': cb.get('m_IsPartialCB')})
    textures = [{'name': nm(t['m_NameIndex']), 'index': t['m_Index'], 'samplerIndex': t['m_SamplerIndex'],
                 'dim': t['m_Dim'], 'multiSampled': t.get('m_MultiSampled')} for t in cp.get('m_TextureParams') or []]
    cbb = [{'name': nm(c['m_NameIndex']), 'index': c['m_Index'], 'arraySize': c.get('m_ArraySize')} for c in cp.get('m_ConstantBufferBindings') or []]
    samplers = [{'index': s['bindPoint'], 'flags': s['sampler']} for s in cp.get('m_Samplers') or []]
    buffers = [{'name': nm(b['m_NameIndex']), 'index': b['m_Index'], 'arraySize': b.get('m_ArraySize')} for b in cp.get('m_BufferParams') or []]
    uavs = [{'name': nm(u['m_NameIndex']), 'index': u['m_Index'], 'originalIndex': u.get('m_OriginalIndex')} for u in cp.get('m_UAVParams') or []]
    # Top-level vector/matrix params (outside any cbuffer) are the $Globals members in some Unity versions.
    loose = []
    for v in cp.get('m_VectorParams') or []:
        loose.append({'name': nm(v['m_NameIndex']), 'type': v['m_Type'], 'rows': 1, 'cols': v['m_Dim'], 'isMatrix': False,
                      'arraySize': v['m_ArraySize'], 'offset': v['m_Index']})
    for m in cp.get('m_MatrixParams') or []:
        loose.append({'name': nm(m['m_NameIndex']), 'type': m['m_Type'], 'rows': m['m_RowCount'], 'cols': 4, 'isMatrix': True,
                      'arraySize': m['m_ArraySize'], 'offset': m['m_Index']})
    return {'cbuffers': cbuffers, 'textures': textures, 'cbBindings': cbb, 'samplers': samplers, 'buffers': buffers,
            'uavs': uavs, 'loose': loose}


def merge_params(common, specific):
    out = {'cbuffers': [], 'textures': [], 'cbBindings': [], 'samplers': [], 'buffers': [], 'uavs': []}
    cbs = {}
    for src in (common, specific or {}):
        for cb in src.get('cbuffers') or []:
            dst = cbs.setdefault(cb['name'], {'name': cb['name'], 'size': cb['size'], 'params': [], 'structs': []})
            dst['size'] = max(dst['size'], cb['size'])
            known = {p['name'] for p in dst['params']}
            dst['params'] += [p for p in cb['params'] if p['name'] not in known]
            dst['structs'] += cb.get('structs') or []
        for key in ('textures', 'cbBindings', 'buffers', 'uavs'):
            known = {t['name'] for t in out[key]}
            out[key] += [t for t in src.get(key) or [] if t['name'] not in known]
        known = {s['index'] for s in out['samplers']}
        out['samplers'] += [s for s in src.get('samplers') or [] if s['index'] not in known]
    if common.get('loose'):
        g = cbs.setdefault('$Globals', {'name': '$Globals', 'size': 0, 'params': [], 'structs': []})
        known = {p['name'] for p in g['params']}
        g['params'] += [p for p in common['loose'] if p['name'] not in known]
    for cb in cbs.values():
        cb['params'].sort(key=lambda p: p['offset'])
    out['cbuffers'] = sorted(cbs.values(), key=lambda c: c['name'])
    return out


def flatten_subprograms(prog):
    subs = prog.get('m_PlayerSubPrograms') or []
    flat = []
    for tier, lst in enumerate(subs):
        for sp in lst or []:
            flat.append((tier, sp))
    if not flat:
        for sp in prog.get('m_SubPrograms') or []:
            flat.append((0, sp))
    return flat


def extract_shader(shader_obj, out_dir, container):
    s = shader_obj
    pf = walk_fields(s.m_ParsedForm)
    name = pf['m_Name']
    sdir = os.path.join(out_dir, sanitize(name))
    os.makedirs(os.path.join(sdir, 'dxbc'), exist_ok=True)
    platforms = list(s.platforms)
    blob = bytes(s.compressedBlob)
    entries_by_platform = {}
    for pi, plat in enumerate(platforms):
        offs, cls, dls = s.offsets[pi], s.compressedLengths[pi], s.decompressedLengths[pi]
        if not isinstance(offs, list):
            offs, cls, dls = [offs], [cls], [dls]
        tiers = []
        for off, cl, dl in zip(offs, cls, dls):
            dec = CompressionHelper.decompress_lz4(blob[off:off + cl], dl)
            n = struct.unpack_from('<i', dec, 0)[0]
            ents = [struct.unpack_from('<iii', dec, 4 + i * 12) for i in range(n)]
            tiers.append([dec[o:o + l] for (o, l, seg) in ents])
        entries_by_platform[plat] = tiers
    if PLATFORM_D3D11 not in entries_by_platform:
        return {'name': name, 'error': 'no d3d11 blob, platforms %r' % platforms}
    tiers = entries_by_platform[PLATFORM_D3D11]
    entries = tiers[0]
    blob_meta = {}
    param_cache = {}

    def param_entry(idx):
        if idx not in param_cache:
            param_cache[idx] = parse_param_entry(entries[idx])
        return param_cache[idx]

    kw_names = pf.get('m_KeywordNames') or []
    out = {'name': name, 'container': container, 'pathId': s.object_reader.path_id if hasattr(s, 'object_reader') else None,
           'fallback': pf['m_FallbackName'], 'customEditor': pf['m_CustomEditorName'],
           'customEditorForRenderPipelines': pf.get('m_CustomEditorForRenderPipelines'),
           'dependencies': pf['m_Dependencies'], 'disableNoSubshadersMessage': pf['m_DisableNoSubshadersMessage'],
           'keywordNames': kw_names, 'keywordFlags': pf.get('m_KeywordFlags'), 'props': pf['m_PropInfo']['m_Props'],
           'platforms': platforms, 'tiers': len(tiers), 'subshaders': []}
    for ss in pf['m_SubShaders']:
        sso = {'lod': ss['m_LOD'], 'tags': ss['m_Tags']['tags'], 'passes': []}
        for p in ss['m_Passes']:
            names = {i: n for n, i in p['m_NameIndices']}
            po = {'type': p['m_Type'], 'name': p['m_Name'], 'useName': p['m_UseName'], 'textureName': p['m_TextureName'],
                  'tags': p['m_Tags']['tags'], 'state': p['m_State'], 'hasInstancingVariant': p['m_HasInstancingVariant'],
                  'hasProceduralInstancingVariant': p.get('m_HasProceduralInstancingVariant'), 'programs': {}}
            for stage in ('progVertex', 'progFragment', 'progGeometry', 'progHull', 'progDomain'):
                prog = p.get(stage)
                if not prog:
                    continue
                flat = flatten_subprograms(prog)
                if not flat:
                    continue
                pbi = prog.get('m_ParameterBlobIndices') or []
                pbi_flat = [x for lst in pbi for x in (lst if isinstance(lst, list) else [lst])]
                common = common_params(prog, names)
                subs = []
                for k, (tier, sp) in enumerate(flat):
                    bi = sp['m_BlobIndex']
                    meta, dxbc = parse_subprogram_entry(entries[bi])
                    if dxbc is not None:
                        with open(os.path.join(sdir, 'dxbc', '%d.dxbc' % bi), 'wb') as f:
                            f.write(dxbc)
                    specific = param_entry(pbi_flat[k]) if k < len(pbi_flat) else None
                    params = merge_params(common, specific)
                    subs.append({'blob': bi, 'tier': tier, 'gpuType': sp['m_GpuProgramType'],
                                 'keywords': sorted(kw_names[i] for i in sp['m_KeywordIndices']) if kw_names else meta['keywords'],
                                 'blobKeywords': meta['keywords'], 'requirements': sp.get('m_ShaderRequirements'),
                                 'sourceMap': meta['sourceMap'], 'channels': meta['channels'], 'unityHeader': meta['unityHeader'],
                                 'hasDxbc': dxbc is not None, 'params': params,
                                 'paramBlob': pbi_flat[k] if k < len(pbi_flat) else None})
                po['programs'][stage] = subs
            sso['passes'].append(po)
        out['subshaders'].append(sso)
    with open(os.path.join(sdir, 'shader.json'), 'w') as f:
        json.dump(out, f, indent=1)
    return {'name': name, 'dir': sdir, 'passes': sum(len(ss['passes']) for ss in out['subshaders'])}


def main():
    args = sys.argv[1:]
    out_dir = args.pop(0)
    names = None
    index_only = False
    files = []
    while args:
        a = args.pop(0)
        if a == '--names':
            names = set(json.load(open(args.pop(0))))
        elif a == '--index-only':
            index_only = True
        elif a == '--list':
            files += [ln.rstrip('\n') for ln in open(args.pop(0)) if ln.strip()]
        else:
            files.append(a)
    os.makedirs(out_dir, exist_ok=True)
    index_path = os.path.join(out_dir, 'index.json')
    index = json.load(open(index_path)) if os.path.exists(index_path) else {}
    done = []
    for path in files:
        env = UnityPy.load(path)
        found = []
        for o in env.objects:
            if o.type.name != 'Shader':
                continue
            try:
                s = o.read()
                nm = s.m_ParsedForm.m_Name
            except Exception as e:  # noqa
                print('read failed', path, o.path_id, e, file=sys.stderr)
                continue
            found.append(nm)
            if index_only or (names is not None and nm not in names):
                continue
            try:
                r = extract_shader(s, out_dir, os.path.basename(path))
                done.append(r)
                print('extracted', nm, r.get('error', ''))
            except Exception as e:
                import traceback
                traceback.print_exc()
                done.append({'name': nm, 'error': repr(e)})
        index[os.path.basename(path)] = sorted(found)
        print(path, 'shaders:', len(found))
    with open(index_path, 'w') as f:
        json.dump(index, f, indent=1)
    with open(os.path.join(out_dir, 'extract_report.json'), 'a') as f:
        for r in done:
            f.write(json.dumps(r) + '\n')


if __name__ == '__main__':
    main()
