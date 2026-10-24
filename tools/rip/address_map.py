#!/usr/bin/env python3
"""Catalog address -> project asset path (rerunnable).

Usage:
  python3 address_map.py [--rescan]       build tools/rip/address_map.json (runs the UnityPy scan through
                                          ~/handoff/heavy.sh when its cache is missing) and print coverage + checks
  python3 address_map.py --no-check       skip the independent verification step
  ~/vd-rip/venv/bin/python -I address_map.py _scan <bundle_dir> <out.json> [bundle ...]    (internal, needs UnityPy)
  ~/vd-rip/venv/bin/python -I address_map.py _deep <jobs.json> <out.json>                  (internal: pixel/mesh hashes)

How it works
  1. scan:   every original bundle -> its full container (key = original GUID, all entries, not just the last one)
             plus a content fingerprint of each container object, computed from the UnityPy type tree.
  2. index:  every exported project file (bundle dir, class, m_Name, YAML-derived fingerprint on demand).
  3. match:  catalog location (address,type) -> original object (bundle, guid, class) -> project file.
             unique-name  : exactly one project file with that (bundle, class, name)
             fingerprint  : several candidates, exactly one agrees on every comparable fingerprint field
             identical    : several candidates, all with identical content (interchangeable)
             subasset     : catalog type is a sub-object of the GUID's main asset (e.g. Bloom inside a VolumeProfile)
             Anything else is reported in `unmatched` with a reason, never guessed.
Map key: the address when it has one catalog location, else "address|ShortType" for every location of that address.
Map value: path relative to Assets/AssetBundles/.
"""
import collections, hashlib, json, os, random, re, struct, subprocess, sys, time, zlib

# A catalog key that is an asset GUID, not a label (package assets carry their GUID as an extra key).
GUID_KEY = re.compile('[0-9a-f]{32}')

HOME = os.path.expanduser('~')
PROJ = os.path.join(HOME, 'void-diver-unity')
ABROOT = os.path.join(PROJ, 'Assets', 'AssetBundles')
CATALOG = os.path.join(PROJ, 'captures', 'catalog.json')
OUT = os.path.join(PROJ, 'tools', 'rip', 'address_map.json')
WORK = os.path.join(HOME, 'vd-rip', 'addrmap')
BUNDLES = os.path.join(HOME, 'vd-src', 'VOID DIVER Escape from the Abyss Demo', 'VOID DIVER_Data',
                       'StreamingAssets', 'aa', 'StandaloneWindows64')
VENV_PY = os.path.join(HOME, 'vd-rip', 'venv', 'bin', 'python')
HEAVY = os.path.join(HOME, 'handoff', 'heavy.sh')
SELF = os.path.abspath(__file__)

# ---------------------------------------------------------------- fingerprints (shared by both sides)

def num(v):
    if isinstance(v, bool): return int(v)
    if isinstance(v, (int, float)): return v
    if isinstance(v, str) and re.fullmatch(r'[-+]?(\d+\.?\d*|\.\d+)([eE][-+]?\d+)?', v): return float(v)
    return v

def f32(v):
    try: return struct.unpack('<f', struct.pack('<f', v))[0]
    except (OverflowError, struct.error): return v

def fl(v):
    v = num(v)
    return float('%.6g' % f32(v)) if isinstance(v, float) else v

def pairs(x):
    """Unity map fields: typetree gives [(k, v)], YAML gives [{k: v}] (or [{first:, second:}])."""
    out = {}
    for e in x or []:
        if isinstance(e, (tuple, list)) and len(e) == 2: out[str(e[0])] = e[1]
        elif isinstance(e, dict) and set(e) == {'first', 'second'}: out[str(e['first'])] = e['second']
        elif isinstance(e, dict) and len(e) == 1: (k, v), = e.items(); out[str(k)] = v
    return out

def g(t, *path):
    for p in path:
        if isinstance(t, dict) and p in t: t = t[p]
        else: return None
    return t

def md5b(b): return hashlib.md5(b).hexdigest()[:16]

def blob(v):
    if v is None: return None
    if isinstance(v, str): return bytes.fromhex(v) if re.fullmatch(r'([0-9a-f]{2})*', v) else None
    if isinstance(v, (bytes, bytearray)): return bytes(v)
    if isinstance(v, (list, tuple)):
        try: return bytes(v)
        except Exception: return None
    return None

def is_null_ptr(p):
    if not isinstance(p, dict): return True
    return not (p.get('m_PathID') or p.get('fileID'))

def vec(t, *ks):
    return [fl(t.get(k, 0)) for k in ks] if isinstance(t, dict) else None

def fp_mesh(t):
    subs = t.get('m_SubMeshes') or []
    if isinstance(subs, dict): subs = [subs]
    vd = t.get('m_VertexData') or {}
    f = {'nsub': len(subs), 'ni': sum(int(s.get('indexCount', 0)) for s in subs),
         'nvs': sum(int(s.get('vertexCount', 0)) for s in subs), 'nv': vd.get('m_VertexCount')}
    c, e = g(t, 'm_LocalAABB', 'm_Center'), g(t, 'm_LocalAABB', 'm_Extent')
    if c and e: f['aabb'] = vec(c, 'x', 'y', 'z') + vec(e, 'x', 'y', 'z')
    ib = blob(t.get('m_IndexBuffer'))
    if ib: f['ib'] = md5b(ib)
    vb = blob(vd.get('_typelessdata') or vd.get('m_DataSize'))
    if vb: f['vb'] = md5b(vb)
    return f

def fp_sprite(t):
    # the exporter writes the atlas-packed rect/offset/pivot, so compare against the type tree's m_RD.textureRect
    f = {}
    rd = t.get('m_RD') or {}
    r = rd.get('textureRect') or t.get('m_Rect')
    if r: f['wh'] = vec(r, 'width', 'height')
    f['ppu'] = fl(t.get('m_PixelsToUnits'))
    subs = rd.get('m_SubMeshes') or []
    f['rdni'] = sum(int(s.get('indexCount', 0)) for s in subs)
    f['rdnv'] = g(rd, 'm_VertexData', 'm_VertexCount')
    ib = blob(rd.get('m_IndexBuffer'))
    if ib: f['rdib'] = md5b(ib)
    vb = blob(g(rd, 'm_VertexData', '_typelessdata') or g(rd, 'm_VertexData', 'm_DataSize'))
    if vb: f['rdvb'] = md5b(vb)
    rk = t.get('m_RenderDataKey')       # the exporter keeps the sprite's original GUID as the first key
    if isinstance(rk, dict) and rk: f['rdkey'] = str(next(iter(rk)))
    return f

def fp_material(t):
    sp = t.get('m_SavedProperties') or {}
    f = {'floats': {k: fl(v) for k, v in pairs(sp.get('m_Floats')).items()},
         'colors': {k: vec(v, 'r', 'g', 'b', 'a') for k, v in pairs(sp.get('m_Colors')).items()},
         'tex': sorted(k for k, v in pairs(sp.get('m_TexEnvs')).items() if not is_null_ptr((v or {}).get('m_Texture'))),
         'texall': sorted(pairs(sp.get('m_TexEnvs'))),
         'queue': t.get('m_CustomRenderQueue'), 'lmf': t.get('m_LightmapFlags')}
    ts = {}
    for k, v in pairs(sp.get('m_TexEnvs')).items():
        v = v or {}
        ts[k] = vec(v.get('m_Scale'), 'x', 'y') + vec(v.get('m_Offset'), 'x', 'y')
    f['texst'] = ts
    return f

def fp_anim(t):
    f = {'rate': fl(t.get('m_SampleRate')), 'legacy': int(bool(t.get('m_Legacy')))}
    st = g(t, 'm_MuscleClip', 'm_StartTime'); sp = g(t, 'm_MuscleClip', 'm_StopTime')
    if st is None: st = g(t, 'm_AnimationClipSettings', 'm_StartTime'); sp = g(t, 'm_AnimationClipSettings', 'm_StopTime')
    f['start'], f['stop'] = fl(st), fl(sp)
    f['nEvents'] = len(t.get('m_Events') or [])
    # the exporter rebuilds curves from the clip's binding table; both sides share the path CRCs
    crcs, ncurve = set(), 0
    for k in ('m_RotationCurves', 'm_PositionCurves', 'm_ScaleCurves', 'm_FloatCurves', 'm_PPtrCurves', 'm_EulerCurves'):
        for c in t.get(k) or []:
            if isinstance(c, dict):
                ncurve += 1; crcs.add(zlib.crc32(str(c.get('path', '')).encode()) & 0xffffffff)
    gbs = {int(gb['path']) for gb in g(t, 'm_ClipBindingConstant', 'genericBindings') or [] if isinstance(gb, dict) and 'path' in gb}
    crcs = gbs or crcs
    f['bpaths'] = md5b(','.join(map(str, sorted(crcs))).encode()) if crcs else None
    f['ncurve'] = ncurve or None
    return f

def flatten(x, path, out, cap=500):
    if len(out) >= cap: return
    x = num(x)
    if isinstance(x, dict):
        if set(x) >= {'m_FileID', 'm_PathID'} or set(x) >= {'fileID'}:
            out.append(path + '=ptr' + ('0' if is_null_ptr(x) else '1')); return
        for k, v in x.items():
            if k in ('m_Script', 'm_GameObject', 'm_CorrespondingSourceObject', 'm_PrefabInstance', 'm_PrefabAsset',
                     'm_ObjectHideFlags', 'serializedVersion', 'm_Enabled', 'm_EditorHideFlags',
                     'm_EditorClassIdentifier'): continue
            flatten(v, path + '.' + str(k), out, cap)
    elif isinstance(x, (list, tuple)):
        if len(x) == 2 and isinstance(x[0], str) and not isinstance(x[1], str):  # typetree pair
            flatten(x[1], path + '{' + x[0] + '}', out, cap); return
        out.append(path + '#' + str(len(x)))
        for i, v in enumerate(x[:24]): flatten(v, path + '[]', out, cap)
    elif isinstance(x, float): out.append('%s=%.4g' % (path, f32(x)))
    elif isinstance(x, (bytes, bytearray)): out.append(path + '=blob%d' % len(x))
    elif isinstance(x, str) and len(x) > 80: out.append(path + '=str%d' % len(x))
    else: out.append('%s=%s' % (path, x))

def collect_refs(t, side, cls):
    """non-null object references of an object: field path -> original path id (orig) or exported guid (project)."""
    out = {}
    def ptr(x):
        if side == 'o':
            return x.get('m_PathID') if isinstance(x, dict) and 'm_PathID' in x and x.get('m_PathID') else None
        return x.get('guid') if isinstance(x, dict) and 'fileID' in x and x.get('guid') else None
    def walk(x, path, depth=0):
        if len(out) > 60 or depth > 6: return
        p = ptr(x)
        if p: out[path] = p; return
        if isinstance(x, dict):
            if 'm_PathID' in x or 'fileID' in x: return
            for k, v in x.items():
                if k not in ('m_Script', 'm_GameObject', 'm_CorrespondingSourceObject', 'm_PrefabInstance', 'm_PrefabAsset'):
                    walk(v, path + '.' + str(k), depth + 1)
        elif isinstance(x, (list, tuple)):
            for v in x[:24]: walk(v, path + '[]', depth + 1)
    if cls == 'Material':
        for k, v in pairs(g(t, 'm_SavedProperties', 'm_TexEnvs')).items():
            q = ptr((v or {}).get('m_Texture'))
            if q: out['tex:' + k] = q
    elif cls == 'MonoBehaviour':
        walk(t, '')
    return out

def fp_leaves(t):
    out = []
    flatten(t, '', out)
    return {'leaves': sorted(set(md5b(s.encode())[:10] for s in out)), 'nleaf': len(out)}

def fp_generic(t):
    return {}

FP = {'Mesh': fp_mesh, 'Sprite': fp_sprite, 'Material': fp_material, 'AnimationClip': fp_anim,
      'MonoBehaviour': fp_leaves}

def feq(a, b):
    if isinstance(a, bool): a = int(a)
    if isinstance(b, bool): b = int(b)
    if isinstance(a, (int, float)) and isinstance(b, (int, float)):
        return abs(a - b) <= 1e-4 + 1e-3 * max(abs(a), abs(b))
    if isinstance(a, dict) and isinstance(b, dict):
        return set(a) == set(b) and all(feq(a[k], b[k]) for k in a)
    if isinstance(a, (list, tuple)) and isinstance(b, (list, tuple)):
        return len(a) == len(b) and all(feq(x, y) for x, y in zip(a, b))
    return a == b

def compare(fa, fb):
    """(agree, comparable): fields present (not None) on both sides."""
    ok = tot = 0
    for k in fa:
        if k in ('leaves', 'nleaf') or k not in fb or fa[k] is None or fb[k] is None: continue
        tot += 1; ok += feq(fa[k], fb[k])
    if 'leaves' in fa and 'leaves' in fb:
        A, B = set(fa['leaves']), set(fb['leaves'])
        tot += 1; ok += (A == B)
    return ok, tot

# ---------------------------------------------------------------- original side (UnityPy)

def scan(bundle_dir, out_path, only=()):
    import UnityPy
    res = {}
    files = sorted(f for f in os.listdir(bundle_dir) if f.endswith('.bundle'))
    for fn in files:
        bname = fn.rsplit('_', 1)[0]
        if only and bname not in only: continue
        t0 = time.time()
        env = UnityPy.load(os.path.join(bundle_dir, fn))
        objs = {o.path_id: o for o in env.objects}
        ab = next((o for o in objs.values() if o.type.name == 'AssetBundle'), None)
        entries = []
        if ab is not None:
            for key, info in ab.read().m_Container:
                pid = info.asset.m_PathID
                o = objs.get(pid)
                if o is None: entries.append({'g': key, 'p': pid, 'c': None}); continue
                entries.append(describe(key, o, objs))
        res[bname] = entries
        print('scan %-48s %5d entries %5.1fs' % (bname, len(entries), time.time() - t0), flush=True)
    json.dump(res, open(out_path, 'w'), separators=(',', ':'))

def describe(key, o, objs):
    cls = o.type.name
    e = {'g': key, 'p': o.path_id, 'c': cls}
    try:
        if cls == 'GameObject':
            t = o.read_typetree(); e['n'] = t['m_Name']; e['f'] = fp_go_orig(o, objs)
        else:
            t = o.read_typetree(); e['n'] = t.get('m_Name')
            if cls in FP: e['f'] = FP[cls](t)
            if cls == 'Sprite': e['f']['rdkey'] = key
            elif cls == 'Texture2D':
                e['f'] = {'w': t.get('m_Width'), 'h': t.get('m_Height')}
            elif cls == 'AudioClip':
                e['f'] = {'len': fl(t.get('m_Length')), 'ch': t.get('m_Channels'), 'hz': t.get('m_Frequency')}
            if cls == 'MonoBehaviour':
                e['f']['script'] = int(t.get('m_Script', {}).get('m_PathID') or 0)
            if cls in ('MonoBehaviour', 'Material'):
                rf = collect_refs(t, 'o', cls)
                if rf: e['refs'] = rf
            if cls == 'Shader' and not e['n']: e['n'] = key       # nameless shaders are exported under their guid
    except Exception as ex:
        e['err'] = repr(ex)[:120]
    return e

def fp_go_orig(root, objs):
    names, cls, tfs = [], collections.Counter(), []
    stack = [root]; seen = set()
    while stack:
        go = stack.pop()
        if go.path_id in seen: continue
        seen.add(go.path_id)
        t = go.read_typetree(); names.append(t['m_Name']); cls[1] += 1
        tr = None
        for c in t['m_Component']:
            ptr = c['component']; o = objs.get(ptr['m_PathID'])
            if o is None: cls[-1] += 1; continue
            cls[int(o.type.value)] += 1
            if o.type.name in ('Transform', 'RectTransform'): tr = o
        if tr is not None:
            tt = tr.read_typetree()
            p, r, sc = tt['m_LocalPosition'], tt['m_LocalRotation'], tt['m_LocalScale']
            tfs.append([p['x'], p['y'], p['z'], r['x'], r['y'], r['z'], r['w'], sc['x'], sc['y'], sc['z']])
            for ch in tt['m_Children']:
                co = objs.get(ch['m_PathID'])
                if co is None: continue
                gp = co.read_typetree()['m_GameObject']['m_PathID']
                if gp in objs: stack.append(objs[gp])
    return go_fp(names, cls, tfs)

def go_fp(names, cls, tfs):
    return {'ngo': len(names), 'cls': {str(k): v for k, v in sorted(cls.items())},
            'names': md5b('\n'.join(sorted(names)).encode()),
            'tf': md5b('\n'.join(sorted('%.4g,' * 10 % tuple(f32(x) for x in t) for t in tfs)).encode())}

def deep(jobs_path, out_path):
    """jobs: [[bundle_name, pathId, kind]] -> hashes of pixels (Texture2D) / vertex positions (Mesh)."""
    import UnityPy
    jobs = json.load(open(jobs_path)); by = collections.defaultdict(list)
    for b, pid, kind in jobs: by[b].append((pid, kind))
    res = {}
    for fn in sorted(os.listdir(BUNDLES)):
        b = fn.rsplit('_', 1)[0]
        if b not in by: continue
        env = UnityPy.load(os.path.join(BUNDLES, fn)); objs = {o.path_id: o for o in env.objects}
        for pid, kind in by[b]:
            try:
                o = objs[pid]
                if kind == 'Texture2D':
                    im = o.read().image.convert('RGBA')
                    res['%s|%d' % (b, pid)] = {'px': md5b(im.tobytes()), 'wh': list(im.size)}
                elif kind == 'Mesh':
                    vd = o.read_typetree()['m_VertexData']
                    res['%s|%d' % (b, pid)] = {'pos': mesh_positions(vd, blob(vd['m_DataSize'])), 'nv': vd['m_VertexCount']}
            except Exception as ex:
                res['%s|%d' % (b, pid)] = {'err': repr(ex)[:100]}
        print('deep', b, flush=True)
    json.dump(res, open(out_path, 'w'))


# ---------------------------------------------------------------- project side

EXT_CLASS = {'.prefab': 'GameObject', '.mat': 'Material', '.anim': 'AnimationClip', '.controller': 'AnimatorController',
             '.png': 'Texture2D', '.jpg': 'Texture2D', '.tga': 'Texture2D', '.exr': 'Texture2D', '.hdr': 'Texture2D',
             '.ogg': 'AudioClip', '.wav': 'AudioClip', '.mp3': 'AudioClip',
             '.bytes': 'TextAsset', '.txt': 'TextAsset', '.json': 'TextAsset', '.xml': 'TextAsset', '.csv': 'TextAsset',
             '.shader': 'Shader', '.ttf': 'Font', '.otf': 'Font', '.mask': 'AvatarMask', '.webm': 'VideoClip',
             '.mp4': 'VideoClip', '.renderTexture': 'RenderTexture', '.mixer': 'AudioMixer',
             '.playable': 'MonoBehaviour', '.shadervariants': 'ShaderVariantCollection', '.compute': 'ComputeShader'}
YAML_EXT = {'.prefab', '.mat', '.anim', '.controller', '.asset', '.playable', '.mask', '.renderTexture', '.mixer',
            '.shadervariants'}
BAD = re.compile(r'[\\/:*?"<>|]')

def unq(raw):
    raw = raw.strip()
    if raw[:1] in ("'", '"'):
        try:
            import yaml
            return str(yaml.safe_load(raw))
        except Exception: return raw
    return raw

def base_stems(stem):
    out = {stem}
    for _ in range(2):
        for x in list(out):
            m = re.fullmatch(r'(.*?)(_\d+|_case\d+)', x)
            if m: out.add(m.group(1))
    return out

class Proj:
    def __init__(self):
        self.files = []              # dicts
        self.by_guid = {}
        self.by_bc = collections.defaultdict(list)     # (bundle, cls) -> files
        self.stems = collections.defaultdict(list)     # (bundle, cls, base stem) -> files
        self.names = collections.defaultdict(list)     # (bundle, cls, m_Name) -> files
        self.cache = {}
        self.load()

    def load(self):
        for b in sorted(os.listdir(ABROOT)):
            bd = os.path.join(ABROOT, b)
            if not os.path.isdir(bd): continue
            for dp, dn, fns in os.walk(bd):
                for fn in fns:
                    if fn.endswith('.meta'): continue
                    full = os.path.join(dp, fn); rel = os.path.relpath(full, ABROOT)
                    ext = os.path.splitext(fn)[1]
                    if fn.endswith('.shadervariants'): ext = '.shadervariants'
                    stem = fn[:-len(ext)] if ext else fn
                    rec = {'b': b, 'rel': rel, 'ext': ext, 'stem': stem, 'cls': EXT_CLASS.get(ext), 'nm': None,
                           'guid': None, 'main': None}
                    try:
                        for ln in open(full + '.meta', errors='replace'):
                            if ln.startswith('guid:'): rec['guid'] = ln.split()[1]
                            elif 'mainObjectFileID:' in ln: rec['main'] = int(ln.split(':')[1])
                    except OSError: pass
                    if ext in YAML_EXT:
                        data = open(full, 'rb').read(65536).decode('utf-8', 'replace')
                        m = self.main_header(data, rec['main'])
                        if m is None and len(data) >= 60000:
                            data = open(full, 'rb').read().decode('utf-8', 'replace')
                            m = self.main_header(data, rec['main'])
                        if m:
                            if ext == '.asset' or rec['cls'] is None: rec['cls'] = m.group(3)
                            rec['ydoc'] = m.group(3)
                            nm = re.search(r'^  m_Name: ?(.*)$', data[m.end():m.end() + 6000], re.M)
                            if nm and ext != '.prefab': rec['nm'] = unq(nm.group(1))
                    self.files.append(rec)
                    if rec['guid']: self.by_guid[rec['guid']] = rec
                    if rec['cls']:
                        self.by_bc[(b, rec['cls'])].append(rec)
                        if rec['nm'] is not None: self.names[(b, rec['cls'], rec['nm'])].append(rec)
                        else:
                            for s in base_stems(rec['stem']): self.stems[(b, rec['cls'], s)].append(rec)

    @staticmethod
    def main_header(data, main):
        if main:
            m = re.search(r'^--- !u!(\d+) &(%d)\n(\w+):' % main, data, re.M)
            if m: return m
        return re.search(r'^--- !u!(\d+) &(-?\d+)\n(\w+):', data, re.M) if not main else None

    def candidates(self, b, cls, name):
        if name is None: return []
        out = []
        for n in {name, BAD.sub('_', name), name.strip()}:
            for r in self.names.get((b, cls, n), []) + self.stems.get((b, cls, n), []):
                if r not in out: out.append(r)
        return out

    # ---- fingerprints of project files
    def fp(self, rec, px=False):
        k = (rec['rel'], px)
        if k in self.cache: return self.cache[k]
        cls, full = rec['cls'], os.path.join(ABROOT, rec['rel'])
        f = None
        try:
            if cls == 'GameObject': f = fp_go_yaml(full)
            elif cls == 'Texture2D':
                with open(full, 'rb') as fh: h = fh.read(32)
                if h[:8] == b'\x89PNG\r\n\x1a\n': f = {'w': struct.unpack('>I', h[16:20])[0], 'h': struct.unpack('>I', h[20:24])[0]}
                if px and f is not None: f['px'] = png_px(full)
            elif cls in FP:
                d = main_doc(full, rec)
                if d is not None:
                    f = FP[cls](d)
                    if cls == 'MonoBehaviour': f['script'] = None
        except Exception as ex:
            f = {'err': repr(ex)[:100]}
        self.cache[k] = f
        return f

def proj_refs(P, rec):
    k = ('refs', rec['rel'])
    if k not in P.cache:
        try: P.cache[k] = collect_refs(main_doc(os.path.join(ABROOT, rec['rel']), rec), 'p', rec['cls'])
        except Exception: P.cache[k] = {}
    return P.cache[k]

def png_px(path):
    from PIL import Image
    return md5b(Image.open(path).convert('RGBA').tobytes())

def main_doc(path, rec):
    import yaml
    txt = open(path, encoding='utf-8', errors='replace').read()
    parts = re.split(r'^--- !u!(\d+) &(-?\d+).*$', txt, flags=re.M)
    # parts = [pre, cls, id, body, cls, id, body...]
    docs = [(parts[i], int(parts[i + 1]), parts[i + 2]) for i in range(1, len(parts) - 2, 3)]
    if not docs: return None
    pick = docs[0]
    if rec.get('main'):
        for d in docs:
            if d[1] == rec['main']: pick = d; break
    body = re.sub(r'^(\s*[\w]+: )([0-9a-fA-F]{16,})$', r"\1'\2'", pick[2], flags=re.M)   # all-digit hex would parse as int
    y = yaml.load(body, Loader=yaml.CSafeLoader)
    if isinstance(y, dict) and len(y) == 1: y = next(iter(y.values()))
    return y

def fp_go_yaml(path):
    txt = open(path, encoding='utf-8', errors='replace').read()
    cls = collections.Counter(); names = []; tfs = []
    for d in txt.split('\n--- !u!')[1:] if txt.startswith('%YAML') else []:
        m = re.match(r'(\d+) &', d)
        if not m: continue
        c = int(m.group(1)); cls[c] += 1
        if c == 1:
            nm = re.search(r'^  m_Name: ?(.*)$', d, re.M)
            names.append(unq(nm.group(1)) if nm else '')
        elif c in (4, 224):
            r = re.search(r'm_LocalRotation: \{x: (\S+), y: (\S+), z: (\S+), w: (\S+)\}', d)
            p = re.search(r'm_LocalPosition: \{x: (\S+), y: (\S+), z: (\S+)\}', d)
            sc = re.search(r'm_LocalScale: \{x: (\S+), y: (\S+), z: (\S+)\}', d)
            if r and p and sc:
                tfs.append([float(v) for v in p.groups() + r.groups() + sc.groups()])
    return go_fp(names, cls, tfs)


# ---------------------------------------------------------------- matching

KNOWN = {'GameObject', 'Mesh', 'Sprite', 'Texture2D', 'Material', 'AudioClip', 'AnimationClip', 'TextAsset', 'Shader',
         'SpriteAtlas', 'Font', 'Cubemap', 'RenderTexture', 'AvatarMask', 'ComputeShader', 'VideoClip',
         'ShaderVariantCollection', 'AudioMixer', 'Avatar'}

def orig_class(t):
    short = t.split('.')[-1]
    if t.startswith('UnityEngine.') and short in KNOWN: return short
    if short in ('RuntimeAnimatorController', 'AnimatorController', 'AnimatorOverrideController'): return 'AnimatorController'
    return 'MonoBehaviour'

def bundle_of(loc):
    b = loc['bundles'][0].replace('\\', '/').rsplit('/', 1)[-1]
    return re.sub(r'_[0-9a-f]{32}\.bundle$', '', b)

def full_agree(a, b):
    if not a or not b or 'err' in b: return False
    ok, tot = compare(a, b)
    return tot > 0 and ok == tot

def file_md5(rec):
    with open(os.path.join(ABROOT, rec['rel']), 'rb') as fh: return hashlib.md5(fh.read()).hexdigest()

class Matcher:
    def __init__(self, proj, scan):
        self.p = proj
        self.entries = collections.defaultdict(list)       # (bundle, guid) -> entries
        self.groups = collections.defaultdict(list)        # (bundle, cls, name) -> entries
        for b, es in scan.items():
            for e in es:
                e['b'] = b
                self.entries[(b, e['g'])].append(e)
                if e['c']: self.groups[(b, e['c'], e.get('n'))].append(e)
        self.deep = {}
        self.busy = set()
        self.bypid = collections.defaultdict(list)
        self.guid_of = {r['rel']: r['guid'] for r in proj.files}
        for es in self.entries.values():
            for e in es: self.bypid[e['p']].append(e)
        self.res = {}          # (bundle, pathId) -> (rel path | None, method | reason)
        self.glob = collections.defaultdict(list)           # (cls, name) -> files anywhere
        for r in proj.files:
            if r['cls']:
                self.glob[(r['cls'], r['nm'])].append(r)
                for s in base_stems(r['stem']) if r['nm'] is None else ():
                    self.glob[(r['cls'], s)].append(r)

    def pclasses(self, c):
        if c == 'AnimatorController': return ('AnimatorController', 'AnimatorOverrideController')
        if c == 'Cubemap': return ('Cubemap', 'Texture2D')     # cubemap sources are exported as plain textures
        return (c,)

    def cands(self, b, c, n):
        out = []
        for pc in self.pclasses(c):
            for r in self.p.candidates(b, pc, n):
                if r not in out: out.append(r)
        return out

    def target_rel(self, pid):
        es = self.bypid.get(pid, [])
        if len(es) != 1 or (es[0]['b'], es[0]['c'], es[0].get('n')) in self.busy: return None
        return self.resolve(es[0])[0]

    def refine(self, cl, F):
        """tie-break by what the objects point at: the referent's matched project file must carry the exported guid."""
        pick = {}
        for o in cl:
            refs = o.get('refs')
            if not refs: return None
            ok = []
            for f in F:
                pr = proj_refs(self.p, f); agree = bad = 0
                for k, pid in refs.items():
                    if k not in pr: continue
                    tgt = self.target_rel(pid)
                    if tgt is None: continue
                    if self.guid_of.get(tgt) == pr[k]: agree += 1
                    else: bad += 1
                if bad == 0 and agree > 0: ok.append(f)
            if len(ok) != 1: return None
            pick[o['p']] = ok[0]
        return pick if len({f['rel'] for f in pick.values()}) == len(cl) else None

    def resolve(self, e):
        key = (e['b'], e['p'])
        if key not in self.res: self.solve_group(e['b'], e['c'], e.get('n'))
        return self.res.get(key, (None, 'not-solved'))

    def solve_group(self, b, c, n):
        key = (b, c, n)
        if key in self.busy: return
        self.busy.add(key)
        try: self._solve_group(b, c, n)
        finally: self.busy.discard(key)

    def _solve_group(self, b, c, n):
        O = self.groups[(b, c, n)]
        C = self.cands(b, c, n)
        xb = False
        if not C:
            for pc in self.pclasses(c):
                for r in self.glob.get((pc, n), []) + self.glob.get((pc, BAD.sub('_', n or '')), []):
                    if r not in C: C.append(r)
            xb = bool(C)
        tag = '-xbundle' if xb else ''
        if not C:
            for o in O: self.res[(b, o['p'])] = (None, 'no project file named %r of class %s' % (n, c))
            return
        if len(O) == 1 and len(C) == 1:
            self.res[(b, O[0]['p'])] = (C[0]['rel'], 'unique-name' + tag); return
        use_px = c == 'Texture2D' and any('%s|%d' % (b, o['p']) in self.deep for o in O)
        if use_px:
            for o in O:
                d = self.deep.get('%s|%d' % (b, o['p']))
                if d and d.get('px') and o.get('f') is not None: o['f']['px'] = d['px']
        fps = {c_['rel']: self.p.fp(c_, use_px) for c_ in C}
        # classes of orig objects with equal fingerprints
        classes = []
        for o in O:
            for cl in classes:
                if full_agree(cl[0].get('f'), o.get('f')) and full_agree(o.get('f'), cl[0].get('f')): cl.append(o); break
            else: classes.append([o])
        taken = collections.Counter()
        agree = []
        for cl in classes:
            F = [c_ for c_ in C if full_agree(cl[0].get('f'), fps[c_['rel']])]
            agree.append(F)
            for f in F: taken[f['rel']] += 1
        for cl, F in zip(classes, agree):
            if not cl[0].get('f') or not F:
                why = 'no fingerprint' if not cl[0].get('f') else 'no candidate agrees (%d candidates)' % len(C)
                for o in cl: self.res[(b, o['p'])] = (None, why + ' for %s %r' % (c, n)); 
                continue
            same_bytes = len(F) >= len(cl) and len(set(file_md5(f) for f in F)) == 1
            if len(cl) == 1 and len(F) == 1 and taken[F[0]['rel']] == 1:
                self.res[(b, cl[0]['p'])] = (F[0]['rel'], 'fingerprint' + tag)
            elif same_bytes and all(taken[f['rel']] == 1 for f in F):
                for o, f in zip(sorted(cl, key=lambda x: x['p']), sorted(F, key=lambda x: x['rel'])):
                    self.res[(b, o['p'])] = (f['rel'], 'identical-content' + tag)
            elif len(F) > 1 and (pick := self.refine(cl, F)):
                for o in cl: self.res[(b, o['p'])] = (pick[o['p']]['rel'], 'fingerprint+refs' + tag)
            elif len(F) == len(cl) and all(taken[f['rel']] == 1 for f in F):
                for o, f in zip(sorted(cl, key=lambda x: x['p']), sorted(F, key=lambda x: x['rel'])):
                    self.res[(b, o['p'])] = (f['rel'], 'identical-fingerprint' + tag)
            else:
                for o in cl:
                    self.res[(b, o['p'])] = (None, 'ambiguous: %d objects, %d agreeing files, shared=%d for %s %r' % (
                        len(cl), len(F), sum(taken[f['rel']] > 1 for f in F), c, n))

def build(cat, scan, proj):
    M = Matcher(proj, scan)
    M.deep = _DEEP
    out = []   # per location: dict
    for loc in cat:
        r = {'key': loc['primaryKey'], 'type': loc['type'], 'short': loc['type'].split('.')[-1]}
        if 'AssetBundleProvider' in loc['provider']:
            r.update(path=None, how='n/a', reason='bundle resource (AssetBundleProvider), not an asset'); r['loc'] = loc; out.append(r); continue
        b = bundle_of(loc); g_ = loc['internalId']; c = orig_class(loc['type'])
        r['b'] = b
        if loc['type'].endswith('SceneInstance'):
            sp = os.path.join(PROJ, 'Assets', g_ + '.unity')
            if os.path.exists(sp): r.update(path=None, full='Assets/%s.unity' % g_, how='scene-guid')
            else: r.update(path=None, how='unmatched', reason='scene %s.unity not exported' % g_)
            r['loc'] = loc; out.append(r); continue
        ents = M.entries.get((b, g_), [])
        same = [e for e in ents if e['c'] == c]
        first = False
        if len(same) > 1:
            # several objects of this class under one guid (an FBX's meshes/clips): the loader returns the one named
            # like the asset, else the first of that type in container order
            stem = os.path.splitext(os.path.basename(loc['primaryKey']))[0]
            nm = [e for e in same if e.get('n') == stem]
            first = len(nm) != 1
            same = nm if len(nm) == 1 else same
        if same:
            path, how = M.resolve(same[0])
            if not path:
                # built-in sub-objects (a font's 'Font Material'/'Font Texture'): same file as the guid's other objects
                files = {M.resolve(e)[0] for e in ents if e is not same[0]} - {None}
                if len(files) == 1 and c in ('Material', 'Texture2D'): path, how = files.pop(), 'subasset'
            if path and first: how = 'first-in-container'
            r.update(path=path, how=how if path else 'unmatched', reason=None if path else how)
        elif ents:
            # sub-object of the guid's main asset: same file as the main object
            files = set(); why = None
            for e in ents:
                p, h = M.resolve(e)
                if p: files.add(p)
            if len(files) == 1: r.update(path=files.pop(), how='subasset')
            else: r.update(path=None, how='unmatched', reason='no %s object under guid and main objects resolve to %d files' % (c, len(files)))
        else:
            r.update(path=None, how='unmatched', reason='guid not in the bundle container')
        if not r.get('path'):
            fb = fallback(M, loc, b, c, ents)
            if fb:
                r.update(how=fb[1], reason=None)
                if fb[1] in ('package-path', 'atlas-rebuild'): r['full'] = fb[0]
                else: r['path'] = fb[0]
        r['loc'] = loc
        out.append(r)
    return out, M

def _load_sprite_atlases():
    p = os.path.join(PROJ, 'tools', 'rip', 'sprite_atlases.json')
    return {a['guid']: a for a in json.load(open(p))} if os.path.exists(p) else {}

SPRITE_ATLASES = _load_sprite_atlases()

def fallback(M, loc, b, c, ents):
    key = loc['primaryKey']
    if key.startswith('Packages/'):
        rest = key.split('/', 2)
        pk = os.path.join(PROJ, 'Packages', rest[1]); cache = os.path.join(PROJ, 'Library', 'PackageCache')
        hit = os.path.exists(os.path.join(PROJ, key)) or any(
            d.startswith(rest[1] + '@') and os.path.exists(os.path.join(cache, d, rest[2])) for d in (os.listdir(cache) if os.path.isdir(cache) else []))
        if hit: return key, 'package-path'
    if c == 'SpriteAtlas':
        # rebuilt by Assets/_VD/Editor/SpriteAtlasRebuild.cs from tools/rip/sprite_atlases.json, GUID kept
        sa = SPRITE_ATLASES.get(loc['internalId'])
        if sa:
            rel = 'Assets/_VD/SpriteAtlas/' + sa['name'] + '.spriteatlasv2'  # all 29 rebuilt as v2 (SpriteAtlasRebuild.cs)
            if os.path.exists(os.path.join(PROJ, rel)): return rel, 'atlas-rebuild'
    if c == 'Texture2D':
        atl = [e for e in ents if e['c'] == 'SpriteAtlas' and e.get('n')]
        if len(atl) == 1:
            pages = sorted(r for r in M.p.by_bc.get((b, 'Texture2D'), []) if ('-%s-' % atl[0]['n']) in r['stem'])
            pages = sorted(pages, key=lambda r: r['stem'])
            if pages: return pages[0]['rel'], 'atlas-page'
    return None


# ---------------------------------------------------------------- driver

def heavy(timeout, *cmd):
    r = subprocess.run([HEAVY, str(timeout), '4'] + list(cmd))
    if r.returncode: sys.exit('heavy job failed (%d): %s' % (r.returncode, ' '.join(cmd[:4])))

def run_deep(jobs, tag):
    os.makedirs(WORK, exist_ok=True)
    jp, op = os.path.join(WORK, 'deep_%s_jobs.json' % tag), os.path.join(WORK, 'deep_%s.json' % tag)
    json.dump(jobs, open(jp, 'w'))
    heavy(3000, VENV_PY, '-I', SELF, '_deep', jp, op)
    return json.load(open(op))

def full_path(r):
    return r.get('full') or ('Assets/AssetBundles/' + r['path'])

def mesh_positions(vd, data):
    """md5 of the vertex positions (channel 0, float32 x3), single-stream layouts only; rounded to 5 significant digits."""
    ch = vd['m_Channels']; n = vd['m_VertexCount']
    size = {0: 4, 1: 2, 2: 1, 3: 1, 4: 2, 5: 2, 6: 1, 7: 1, 8: 2, 9: 2, 10: 4, 11: 4}
    if not data or any(c['stream'] != 0 for c in ch if c['dimension']): return None
    stride = sum(c['dimension'] * size[c['format']] for c in ch if c['dimension'])
    p = ch[0]
    if p['dimension'] != 3 or p['format'] != 0 or len(data) < n * stride: return None
    vals = []
    for i in range(n):
        vals += struct.unpack_from('<3f', data, i * stride + p['offset'])
    return md5b(struct.pack('<%df' % len(vals), *[float('%.5g' % x) for x in vals]))

def mesh_pos_yaml(rec):
    d = main_doc(os.path.join(ABROOT, rec['rel']), rec)
    vd = d['m_VertexData']
    return mesh_positions(vd, blob(vd.get('_typelessdata')))

def verify(out, M, P, seed=1):
    rnd = random.Random(seed)
    def pick(method_set, n):
        pool = [r for r in out if r.get('path') and r['how'] in method_set and r['short'] in ('Mesh', 'Texture2D')]
        rnd.shuffle(pool); res = []
        for r in pool:
            if len(res) >= n: break
            if r['short'] == 'Mesh':
                try:
                    if mesh_pos_yaml(next(x for x in P.files if x['rel'] == r['path'])) is None: continue
                except Exception: continue
            res.append(r)
        return res
    sel = {'unique-name': pick({'unique-name'}, 20), 'fingerprint': pick({'fingerprint', 'identical-content'}, 20)}
    jobs, orig = [], {}
    for rs in sel.values():
        for r in rs:
            e = [e for e in M.entries[(r['b'], r['loc']['internalId'])] if e['c'] == orig_class(r['type'])]
            e = [x for x in e if M.res.get((x['b'], x['p']), (None,))[0] == r['path']][:1] or e[:1]
            orig[r['key'] + '|' + r['short']] = e[0]
            jobs.append([r['b'], e[0]['p'], r['short']])
    deep_res = run_deep(jobs, 'verify')
    print('verification (independent of the name/fingerprint step: Texture2D RGBA pixel md5, Mesh vertex-position md5)')
    for label, rs in sel.items():
        ok = bad = skip = 0; fails = []
        for r in rs:
            e = orig[r['key'] + '|' + r['short']]; d = deep_res.get('%s|%d' % (r['b'], e['p'])) or {}
            rec = next(x for x in P.files if x['rel'] == r['path'])
            try:
                if r['short'] == 'Texture2D': a_, b_ = d.get('px'), png_px(os.path.join(ABROOT, rec['rel']))
                else: a_, b_ = d.get('pos'), mesh_pos_yaml(rec)
            except Exception as ex:
                a_, b_ = None, None
            if a_ is None or b_ is None: skip += 1
            elif a_ == b_: ok += 1
            else: bad += 1; fails.append(r['key'] + ' -> ' + r['path'])
        print('  %-12s sampled %2d: identical %2d  different %2d  not comparable %2d' % (label, len(rs), ok, bad, skip))
        for f in fails[:5]: print('     MISMATCH', f)

def main():
    args = sys.argv[1:]
    os.makedirs(WORK, exist_ok=True)
    scan_path = os.path.join(WORK, 'orig_scan.json')
    if '--rescan' in args or not os.path.exists(scan_path):
        heavy(3000, VENV_PY, '-I', SELF, '_scan', BUNDLES, scan_path)
    scan = json.load(open(scan_path))
    cat = json.load(open(CATALOG))
    P = Proj()
    out, M = build(cat, scan, P)
    # texture ties: split with pixel hashes
    ties = collections.OrderedDict()
    # whole groups, not just the located entry
    gk = set()
    for r in out:
        if r['short'] == 'Texture2D' and r['how'] in ('identical-fingerprint', 'unmatched') and r.get('b'):
            for e in M.entries.get((r['b'], r['loc']['internalId']), []):
                if e['c'] == 'Texture2D': gk.add((e['b'], e['c'], e.get('n')))
    for k in gk:
        for e in M.groups[k]: ties['%s|%d' % (e['b'], e['p'])] = [e['b'], e['p'], 'Texture2D']
    if ties:
        M.deep = run_deep(list(ties.values()), 'tex')
        M.res = {}
        out, M2 = rebuild(cat, scan, P, M.deep)
        M = M2
    write_output(out)
    report(out)
    if '--no-check' not in args: verify(out, M, P)

def rebuild(cat, scan, P, deepres):
    global _DEEP
    _DEEP = deepres
    return build(cat, scan, P)

_DEEP = {}

def write_output(out):
    by = collections.OrderedDict()
    for r in out: by.setdefault(r['key'], []).append(r)
    entries, unmatched = {}, []
    for key, rs in by.items():
        ok = [r for r in rs if r.get('path') or r.get('full')]
        for r in rs:
            if r not in ok:
                why = r.get('reason') or r['how']
                if r['short'] == 'SpriteAtlas': why = 'SpriteAtlas object not exported by AssetRipper (its sprites are standalone Sprite assets; the atlas page texture is mapped under the Texture2D location)'
                unmatched.append({'address': key, 'type': r['type'], 'reason': why})
        if not ok: continue
        # a SpriteAtlas location shares its GUID with the atlas-page Texture2D location: the rebuilt atlas is the asset
        prim = next((r for r in ok if r['short'] == 'GameObject'), next((r for r in ok if r['short'] == 'SpriteAtlas'), ok[0]))
        labels = []
        for r in rs:
            for k in r['loc']['keys']:
                if k != key and k != r['loc']['internalId'] and not GUID_KEY.fullmatch(k) and k not in labels: labels.append(k)
        e = {'path': full_path(prim), 'type': prim['type'], 'labels': labels, 'bundle': prim.get('b') or bundle_of(prim['loc']),
             'how': prim['how']}
        # The original project's asset GUID: AssetReference fields and the catalog's GUID keys use it.
        guid = next((k for k in prim['loc']['keys'] if GUID_KEY.fullmatch(k)), None)
        if guid: e['guid'] = guid
        extra = [{'type': r['type'], 'path': full_path(r), 'how': r['how']} for r in ok if r is not prim and full_path(r) != e['path']]
        if extra: e['also'] = extra
        entries[key] = e
    cov = coverage(out)
    json.dump({'entries': entries, 'unmatched': unmatched, 'coverage': cov}, open(OUT, 'w'), separators=(',', ':'), ensure_ascii=False)
    print('wrote %s (%d addresses, %d unmatched locations, %.1f MB)' % (OUT, len(entries), len(unmatched), os.path.getsize(OUT) / 1e6))

def coverage(out):
    assets = [r for r in out if r['how'] != 'n/a']
    m = [r for r in assets if r.get('path') or r.get('full')]
    return {'locations_total': len(out), 'bundle_resources_skipped': len(out) - len(assets),
            'asset_locations': len(assets), 'matched': len(m),
            'by_method': dict(collections.Counter(r['how'] for r in m).most_common()),
            'by_type': {t: [sum(1 for r in m if r['short'] == t), sum(1 for r in assets if r['short'] == t)]
                        for t, _ in collections.Counter(r['short'] for r in assets).most_common()},
            'by_type_note': '[matched, total]'}

def report(out):
    c = coverage(out)
    print('coverage: %d / %d asset locations matched (%d bundle-resource locations skipped; %d locations in catalog)' % (
        c['matched'], c['asset_locations'], c['bundles_skipped'] if 'bundles_skipped' in c else c['bundle_resources_skipped'], c['locations_total']))
    print('by method:', c['by_method'])
    print('by type (matched/total):', {t: '%d/%d' % tuple(v) for t, v in list(c['by_type'].items())[:14]})
    reasons = collections.Counter((r['short'], (r.get('reason') or '')[:60]) for r in out if r['how'] == 'unmatched')
    for (t, why), n in reasons.most_common(12): print('  unmatched %4d %-24s %s' % (n, t, why))

if __name__ == '__main__' and len(sys.argv) > 1 and sys.argv[1] == '_scan':
    scan(sys.argv[2], sys.argv[3], set(sys.argv[4:])); sys.exit(0)
if __name__ == '__main__' and len(sys.argv) > 1 and sys.argv[1] == '_deep':
    deep(sys.argv[2], sys.argv[3]); sys.exit(0)

if __name__ == '__main__' and (len(sys.argv) == 1 or not sys.argv[1].startswith('_')):
    main()
