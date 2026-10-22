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
import collections, hashlib, json, os, random, re, struct, subprocess, sys, time

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

def fl(v):
    v = num(v)
    return float('%.6g' % v) if isinstance(v, float) else v

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
    vb = blob(vd.get('m_DataSize', vd.get('_typelessdata')))
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
    for k in ('m_RotationCurves', 'm_PositionCurves', 'm_ScaleCurves', 'm_FloatCurves', 'm_PPtrCurves', 'm_EulerCurves', 'm_Events'):
        f['n' + k[2:]] = len(t.get(k) or [])
    paths = []
    for k in ('m_RotationCurves', 'm_PositionCurves', 'm_ScaleCurves', 'm_FloatCurves', 'm_PPtrCurves', 'm_EulerCurves'):
        for c in t.get(k) or []:
            if isinstance(c, dict): paths.append('%s|%s|%s' % (k[2:5], c.get('path', ''), c.get('attribute', '')))
    f['paths'] = md5b('\n'.join(sorted(paths)).encode()) if paths else None
    return f

def flatten(x, path, out, cap=500):
    if len(out) >= cap: return
    x = num(x)
    if isinstance(x, dict):
        if set(x) >= {'m_FileID', 'm_PathID'} or set(x) >= {'fileID'}:
            out.append(path + '=ptr' + ('0' if is_null_ptr(x) else '1')); return
        for k, v in x.items():
            if k in ('m_Script', 'm_GameObject', 'm_CorrespondingSourceObject', 'm_PrefabInstance', 'm_PrefabAsset',
                     'm_ObjectHideFlags', 'serializedVersion'): continue
            flatten(v, path + '.' + str(k), out, cap)
    elif isinstance(x, (list, tuple)):
        if len(x) == 2 and isinstance(x[0], str) and not isinstance(x[1], str):  # typetree pair
            flatten(x[1], path + '{' + x[0] + '}', out, cap); return
        out.append(path + '#' + str(len(x)))
        for i, v in enumerate(x[:24]): flatten(v, path + '[]', out, cap)
    elif isinstance(x, float): out.append('%s=%.4g' % (path, x))
    elif isinstance(x, (bytes, bytearray)): out.append(path + '=blob%d' % len(x))
    elif isinstance(x, str) and len(x) > 80: out.append(path + '=str%d' % len(x))
    else: out.append('%s=%s' % (path, x))

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
            elif cls == 'Texture2D':
                e['f'] = {'w': t.get('m_Width'), 'h': t.get('m_Height')}
            elif cls == 'AudioClip':
                e['f'] = {'len': fl(t.get('m_Length')), 'ch': t.get('m_Channels'), 'hz': t.get('m_Frequency')}
            if cls == 'MonoBehaviour':
                e['f']['script'] = int(t.get('m_Script', {}).get('m_PathID') or 0)
    except Exception as ex:
        e['err'] = repr(ex)[:120]
    return e

def fp_go_orig(root, objs):
    names, cls = [], collections.Counter()
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
            for ch in tr.read_typetree()['m_Children']:
                co = objs.get(ch['m_PathID'])
                if co is None: continue
                gp = co.read_typetree()['m_GameObject']['m_PathID']
                if gp in objs: stack.append(objs[gp])
    return go_fp(names, cls)

def go_fp(names, cls):
    return {'root': None, 'ngo': len(names), 'cls': {str(k): v for k, v in sorted(cls.items())},
            'names': md5b('\n'.join(sorted(names)).encode())}

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
                    m = o.read(); m.process_data()
                    pos = m.m_Vertices
                    res['%s|%d' % (b, pid)] = {'pos': md5b(struct.pack('<%df' % len(pos), *[float('%.5g' % x) for x in pos]))
                                               if pos is not None and len(pos) else None, 'nv': m.m_VertexCount}
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
        for n in {name, BAD.sub('_', name)}:
            for r in self.names.get((b, cls, n), []) + self.stems.get((b, cls, n), []):
                if r not in out: out.append(r)
        return out

    # ---- fingerprints of project files
    def fp(self, rec):
        k = rec['rel']
        if k in self.cache: return self.cache[k]
        cls, full = rec['cls'], os.path.join(ABROOT, rec['rel'])
        f = None
        try:
            if cls == 'GameObject': f = fp_go_yaml(full)
            elif cls == 'Texture2D':
                with open(full, 'rb') as fh: h = fh.read(32)
                if h[:8] == b'\x89PNG\r\n\x1a\n': f = {'w': struct.unpack('>I', h[16:20])[0], 'h': struct.unpack('>I', h[20:24])[0]}
            elif cls in FP:
                d = main_doc(full, rec)
                if d is not None:
                    f = FP[cls](d)
                    if cls == 'MonoBehaviour': f['script'] = None
        except Exception as ex:
            f = {'err': repr(ex)[:100]}
        self.cache[k] = f
        return f

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
    cls = collections.Counter(); names = []
    for d in txt.split('\n--- !u!')[1:] if txt.startswith('%YAML') else []:
        m = re.match(r'(\d+) &', d)
        if not m: continue
        c = int(m.group(1)); cls[c] += 1
        if c == 1:
            nm = re.search(r'^  m_Name: ?(.*)$', d, re.M)
            names.append(unq(nm.group(1)) if nm else '')
    return go_fp(names, cls)


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
        self.res = {}          # (bundle, pathId) -> (rel path | None, method | reason)
        self.glob = collections.defaultdict(list)           # (cls, name) -> files anywhere
        for r in proj.files:
            if r['cls']:
                self.glob[(r['cls'], r['nm'])].append(r)
                for s in base_stems(r['stem']) if r['nm'] is None else ():
                    self.glob[(r['cls'], s)].append(r)

    def pclasses(self, c): return ('AnimatorController', 'AnimatorOverrideController') if c == 'AnimatorController' else (c,)

    def cands(self, b, c, n):
        out = []
        for pc in self.pclasses(c):
            for r in self.p.candidates(b, pc, n):
                if r not in out: out.append(r)
        return out

    def resolve(self, e):
        key = (e['b'], e['p'])
        if key not in self.res: self.solve_group(e['b'], e['c'], e.get('n'))
        return self.res.get(key, (None, 'not-solved'))

    def solve_group(self, b, c, n):
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
        fps = {c_['rel']: self.p.fp(c_) for c_ in C}
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
            if len(F) == len(cl) and all(taken[f['rel']] == 1 for f in F):
                for o, f in zip(sorted(cl, key=lambda x: x['p']), sorted(F, key=lambda x: x['rel'])):
                    self.res[(b, o['p'])] = (f['rel'], ('fingerprint' if len(cl) == 1 else 'identical-fingerprint') + tag)
            elif len(set(file_md5(f) for f in F)) == 1 and len(F) >= len(cl) and all(taken[f['rel']] == 1 for f in F):
                for o, f in zip(sorted(cl, key=lambda x: x['p']), sorted(F, key=lambda x: x['rel'])):
                    self.res[(b, o['p'])] = (f['rel'], 'identical-content' + tag)
            else:
                for o in cl:
                    self.res[(b, o['p'])] = (None, 'ambiguous: %d objects, %d agreeing files, shared=%d for %s %r' % (
                        len(cl), len(F), sum(taken[f['rel']] > 1 for f in F), c, n))

def build(cat, scan, proj):
    M = Matcher(proj, scan)
    out = []   # per location: dict
    for loc in cat:
        r = {'key': loc['primaryKey'], 'type': loc['type'], 'short': loc['type'].split('.')[-1]}
        if 'AssetBundleProvider' in loc['provider']:
            r.update(path=None, how='n/a', reason='bundle resource, not an asset'); out.append(r); continue
        b = bundle_of(loc); g_ = loc['internalId']; c = orig_class(loc['type'])
        r['b'] = b
        ents = M.entries.get((b, g_), [])
        same = [e for e in ents if e['c'] == c]
        if len(same) > 1:
            stem = os.path.splitext(os.path.basename(loc['primaryKey']))[0]
            nm = [e for e in same if e.get('n') == stem]
            same = nm if len(nm) == 1 else same
        if len(same) == 1:
            path, how = M.resolve(same[0])
            r.update(path=path, how=how if path else 'unmatched', reason=None if path else how)
        elif len(same) > 1:
            r.update(path=None, how='unmatched', reason='%d %s objects share the guid in the container' % (len(same), c))
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
        out.append(r)
    return out, M

if __name__ == '__main__' and len(sys.argv) > 1 and sys.argv[1] == '_scan':
    scan(sys.argv[2], sys.argv[3], set(sys.argv[4:])); sys.exit(0)
if __name__ == '__main__' and len(sys.argv) > 1 and sys.argv[1] == '_deep':
    deep(sys.argv[2], sys.argv[3]); sys.exit(0)
