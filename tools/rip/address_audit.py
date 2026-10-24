#!/usr/bin/env python3
"""Audit every Addressables key the game can ask for against what we register and what the game shipped.

Usage: python3 tools/rip/address_audit.py [--verbose]     exit 1 when a reachable key is missing

Three parts:
  A. key-builder parity   for every class, the NemoLib.AssetManagement.Extensions helpers its decomp calls versus the
                          ones our C# calls. Each helper builds a different key prefix (SpawnVfx "Vfx/", SpawnSkillVfx
                          "Vfx/Skill/", SpawnStatusVfx "Vfx/Status/"), so a port that calls a sibling helper asks for
                          keys that never existed. This is the bug class behind "Resource prefab(Vfx/1001/...) not found".
  B. table-driven keys    every value of a table field that feeds a key builder, put through the builder the game uses
                          (prefix read from the decomp) and, when part A finds a mismatch, the builder our port uses.
  C. literal keys         every string literal of the shipped code (strings.tsv) and of our C# that looks like a key.

Key status:
  ok           registered in Assets/AddressableAssetsData/AssetGroups, and preload keys carry the label the game ensures
  unregistered in the shipped catalog (captures/catalog.json) but not in our groups: rip pipeline / address map gap
  nolabel      registered, but a preload-only lookup (AssetPreloader.Get) needs the "Login" label and it has none
  absent       not in the shipped catalog either: the shipped data has no such asset (docs/MISSING.md)
"""
import collections, csv, glob, io, json, os, re, sys

ROOT = os.path.dirname(os.path.dirname(os.path.dirname(os.path.abspath(__file__))))
SCRIPTS = os.path.join(ROOT, 'Assets', 'Scripts')
GROUPS = os.path.join(ROOT, 'Assets', 'AddressableAssetsData', 'AssetGroups')
CATALOG = os.path.join(ROOT, 'captures', 'catalog.json')
TABLES = os.path.join(ROOT, 'Assets', 'StreamingAssets', 'TableEncrypted')
CODE = os.path.expanduser('~/unity-decode-kit/out/vd/code')
DECOMP = os.path.join(CODE, 'decomp')
EXT_CS = os.path.join(SCRIPTS, 'De.InGame', 'NemoLib', 'AssetManagement', 'Extensions.cs')
EXT_C = os.path.join(DECOMP, 'De.InGame', 'NemoLib', 'AssetManagement', 'Extensions.c')
PRELOAD_LABEL = 'Login'   # SplashScene: _assetManager.EnsureAsync("Login") is the only label the game preloads
GUID = re.compile(r'^[0-9a-f]{32}$')
KEY_EXT = r'(?:prefab|asset|png|jpg|mat|controller|overrideController|anim|spriteatlas|spriteatlasv2|playable|mp3|wav|ogg|bytes|json|txt|mask)'
KEY_LIT = re.compile(r'^[\w ./&()\-]+/[\w ./&()\-]+\.' + KEY_EXT + r'$')

# ------------------------------------------------------------------------------------------------ catalogs

def registered():
    """Our Addressables groups: address -> set(labels)."""
    out = {}
    for path in glob.glob(os.path.join(GROUPS, '*.asset')):
        addr, labels, in_labels = None, None, False
        for line in open(path, encoding='utf-8'):
            m = re.match(r'\s+m_Address: (.*)$', line)
            if m:
                if addr is not None: out.setdefault(addr, set()).update(labels)
                addr, labels, in_labels = m.group(1).strip().strip("'\""), set(), False
                continue
            if addr is None: continue
            if re.match(r'\s+m_SerializedLabels:', line):
                in_labels = '[]' not in line
                continue
            m = re.match(r'\s+- (\S.*)$', line)
            if in_labels and m:
                labels.add(m.group(1).strip())
                continue
            in_labels = False
        if addr is not None: out.setdefault(addr, set()).update(labels)
    return out


def shipped():
    """The shipped catalog: primary key -> set(labels)."""
    out = collections.defaultdict(set)
    for loc in json.load(open(CATALOG, encoding='utf-8')):
        pk = loc.get('primaryKey')
        if not pk: continue
        out[pk].update(k for k in loc.get('keys', []) if k != pk and not GUID.match(k))
    return out

# ------------------------------------------------------------------------------------------------ A: builder parity

def helper_prefixes():
    """Extensions helper -> key prefix, from the decomp's string literal and from our C#."""
    lit = {}
    for line in open(os.path.join(CODE, 'strings.tsv'), encoding='utf-8', errors='replace'):
        p = line.rstrip('\n').split('\t')
        if len(p) >= 3: lit[p[0]] = p[2]
    game, port = {}, {}
    txt = open(EXT_C, encoding='utf-8', errors='replace').read()
    for m in re.finditer(r'^// \S+ (Spawn\w*Vfx)\(AssetManager, String, Transform\)\s+@0x[0-9A-F]+\n(.*?)(?=^// \S+ \w+\(|\Z)',
                         txt, re.S | re.M):
        s = re.search(r'\((StringLiteral_\w+),vfxName,StringLiteral__prefab', m.group(2))
        if s: game[m.group(1)] = lit[s.group(1)]
    cs = open(EXT_CS, encoding='utf-8').read()
    for m in re.finditer(r'public static \S+ (Spawn\w*Vfx)\(this AssetManager[^)]*\)\s*\{\s*string key = "([^"]+)" \+', cs):
        port[m.group(1)] = m.group(2)
    return game, port


def ext_helpers():
    return set(re.findall(r'public static [^(=]+? (\w+)(?:<[^>]*>)?\(this AssetManager', open(EXT_CS, encoding='utf-8').read()))


def parity(helpers):
    """class -> (helpers called in the decomp, helpers called in our C#)."""
    game = collections.defaultdict(set)
    for path in glob.glob(os.path.join(DECOMP, '**', '*.c'), recursive=True):
        cls = re.split(r'[+]', os.path.basename(path)[:-2])[0]
        if cls == 'Extensions': continue
        for name in re.findall(r'NemoLib__AssetManagement__Extensions__([A-Za-z0-9]+)_NemoLib__AssetManagement__AssetManager',
                               open(path, encoding='utf-8', errors='replace').read()):
            if name in helpers and (cls, name) not in INLINED: game[cls].add(name)
    port = collections.defaultdict(set)
    call = re.compile(r'(?:\b\w*[aA]ssetManager\w*\)?|\bExtensions)\s*\.\s*(%s)\s*(?:<[^>()]*>)?\s*\(' % '|'.join(sorted(helpers)))
    for path in glob.glob(os.path.join(SCRIPTS, '**', '*.cs'), recursive=True):
        cls = os.path.basename(path)[:-3]
        if path == EXT_CS: continue
        txt = re.sub(r'//[^\n]*', '', open(path, encoding='utf-8', errors='replace').read())
        port[cls].update(call.findall(txt))
    return {c: (game.get(c, set()), port.get(c, set())) for c in set(game) | set(port) if game.get(c, set()) != port.get(c, set())}

# ------------------------------------------------------------------------------------------------ B: table-driven keys

def table(name):
    raw = open(os.path.join(TABLES, name + '.bytes'), 'rb').read()
    rows = list(csv.reader(io.StringIO(bytes(b ^ 0xCC for b in raw).decode('utf-8-sig'), newline='')))
    return [dict(zip(rows[0], r)) for r in rows[2:]]


def values(name, path):
    """Strings at `path` in table `name`. `a.b[].c` walks JSON cells; `**.x` matches field x at any depth."""
    col, _, rest = path.partition('.')
    out = []
    def walk(v, parts):
        if parts and parts[0] == '**':
            walk(v, parts[1:])
            if isinstance(v, dict):
                for x in v.values(): walk(x, parts)
            elif isinstance(v, list):
                for x in v: walk(x, parts)
            return
        if not parts:
            if isinstance(v, str) and v: out.append(v)
            elif isinstance(v, int) and not isinstance(v, bool): out.append(str(v))
            return
        key, many = (parts[0][:-2], True) if parts[0].endswith('[]') else (parts[0], False)
        if isinstance(v, dict) and key in v:
            items = v[key] if many and isinstance(v[key], list) else [v[key]]
            for x in items: walk(x, parts[1:])
    for row in table(name):
        cell = row.get(col, '')
        if not rest:
            if cell: out.append(cell)
            continue
        try: walk(json.loads(cell), rest.split('.'))
        except ValueError: pass
    return out


def enum_names(enum):
    for path in glob.glob(os.path.join(SCRIPTS, '**', enum + '.cs'), recursive=True):
        body = re.search(r'enum\s+%s\b[^{]*\{(.*?)\}' % enum, open(path, encoding='utf-8').read(), re.S)
        if body: return [n for n in re.findall(r'^\s*(\w+)\s*(?:=|,|$)', body.group(1), re.M)]
    return []

# (what, table, field path, key template, consumer class, helper the decomp's consumer calls, preload?)
# Templates follow the original code: a "{}" value goes through `helper` (prefix from the decomp) or a literal prefix.
VFX = [
    ('HitBox vfx',           'HitBox', 'HitBoxInfo.vfx',                        '{}',              'HitBox',          'SpawnSkillVfx'),
    ('HitBox hitVfx',        'HitBox', 'HitBoxInfo.hitVfx',                     'Skill/{}',        'GameVfxManager',  'SpawnVfx'),
    ('HitBox FireVfx',       'HitBox', 'HitBoxInfo.FireVfx',                    'Skill/{}',        'GameVfxManager',  'SpawnVfx'),
    ('HitBox fx event',      'HitBox', 'HitBoxInfo.collisionEvents[].Vfx',      'Skill/{}',        'GameVfxManager',  'SpawnVfx'),
    ('Skill vfx event',      'Skill',  'RootActionNode.**.actionEvents[].prefab', 'Skill/{}',      'GameVfxManager',  'SpawnVfx'),
    ('Buff front guard',     'Buff',   'BuffEffects[].Vfx',                     'Skill/{}',        'GameVfxManager',  'SpawnVfx'),
    ('Monster spawn',        'Monster', 'SpawnVfx',                             'Spawn/{}',        'GameVfxManager',  'SpawnVfx'),
    ('BuffVfx',              'BuffVfx', 'Vfx',                                  '{}',              'UnitView',        'SpawnStatusVfx'),
    ('BuffVfx dot',          'BuffVfx', 'DotVfx',                               'Status/{}',       'GameVfxManager',  'SpawnVfx'),
    ('StatusEffectTag',      'StatusEffectTag', 'Vfx',                          '{}',              'UnitView',        'SpawnStatusVfx'),
    ('StatusEffectTag dot',  'StatusEffectTag', 'DotVfx',                       'Status/{}',       'GameVfxManager',  'SpawnVfx'),
]
# Calls the decomp shows only because IL2CPP inlined the callee that makes them; the port calls that callee, same key.
INLINED = {
    ('GameWave', 'InstantiateSpecialField'): 'StartWave inlines GameObjectManager.SpawnSpecialField (uses its _assetManager)',
}
# (what, table, field path, key template, preload?) for builders with a fixed prefix (Extensions.cs, same literals as the decomp)
PREFABS = [
    ('Character',        'Character', 'Id',              'Unit/Character/{}.prefab', True),
    ('Character spine',  'Character', 'Id',              'Character/{}.asset', False),
    ('Monster',          'Monster', 'Id',            'Unit/Monster/{}.prefab', True),
    ('Npc',              'Npc', 'Id',                    'Object/Npc/{}.prefab', True),
    ('Npc spine',        'Npc', 'Id',                    'Npc/{}.asset', False),
    ('Pet',              'Pet', 'Id',                    'Unit/Pet/{}.prefab', True),
    ('ExtraUnit',        'ExtraUnit', 'Id',              'Unit/ExtraUnit/{}.prefab', True),
    ('Trap',             'Trap', 'Id',                   'Object/Trap/{}.prefab', True),
    ('Exit',             'Exit', 'Type',                 'Object/Exit/{}.prefab', True),
    ('BreakableProp',    'BreakableProp', 'PrefabName',  'Object/BreakableProp/{}.prefab', True),
    ('SpecialField',     'SpecialField', 'PrefabName',   'Object/SpecialField/{}.prefab', True),
    ('DisposableObject', 'DisposableObject', 'PrefabName', 'Object/DisposableObject/{}.prefab', True),
    ('Entrance',         'Entrance', 'PrefabName',       'Object/Entrance/{}.prefab', True),
    ('Mimic',            'Mimic', 'PrefabName',          'Object/Mimic/{}.prefab', True),
    ('RewardBox',        'RewardBox', 'PrefabName',      'Object/RewardBox/{}.prefab', True),
    ('RewardBox fail',   'RewardBox', 'StressFailPrefabName', 'Object/VisualProp/{}.prefab', True),
    ('WaveExecutor',     'WaveExecutor', 'PrefabName',   'Object/WaveExecutor/{}.prefab', True),
    ('SkillExecutor',    'SkillExecutor', 'PrefabName',  'Object/SkillExecutor/{}.prefab', True),
    ('ZoneSpawn trigger', 'ZoneSpawn', 'SpawnDatas[].PrefabName', 'Object/InteractiveTrigger/{}.prefab', True),
    ('ZoneSpawn marker', 'ZoneSpawn', 'SpawnDatas[].MarkerName', 'EventUi/MiniMapMarker/{}.prefab', True),
    ('Sector',           'Sector', 'Id',                 'Sector/{}.prefab', False),
    ('Sector decoration', 'Sector', 'AreaDecorationObjectSpawnDatas[].PrefabNames[]', 'Object/AreaDecorationObject/{}.prefab', True),
    ('Sector locked',    'Sector', 'LockedAreaObjectSpawnDatas[].PrefabName', 'Object/LockedAreaObject/{}.prefab', True),
    ('Indicator',        'Skill', 'RootActionNode.**.IndicatorInfo.IndicatorVfxType', 'Combat/SkillIndicator/Vfx/IndicatorVfx_{}.prefab', True),
]
ENUMS = [
    ('HitBox collision', 'ECollisionType',    'Combat/HitBox/HitBox{}.prefab', True),
    ('Ping',             'EPingType',         'EventUi/Ping/{}.prefab', True),
    ('FloatingText',     'EFloatingTextType', 'EventUi/FloatingText/{}.prefab', True),
]

# ------------------------------------------------------------------------------------------------ main

def main():
    verbose = '--verbose' in sys.argv
    reg, ship = registered(), shipped()
    helpers = ext_helpers()
    game_pfx, port_pfx = helper_prefixes()
    diffs = parity(helpers)

    print('== A. key-builder parity (Extensions helpers, decomp vs C#)')
    vfx_helpers = set(game_pfx)
    for cls in sorted(diffs):
        g, p = diffs[cls]
        print('  %-34s game-only %-40s port-only %s' % (cls, sorted(g - p) or '-', sorted(p - g) or '-'))
    for h in sorted(vfx_helpers):
        if game_pfx.get(h) != port_pfx.get(h):
            print('  helper %s builds "%s" in the game, "%s" in the port' % (h, game_pfx.get(h), port_pfx.get(h)))

    def status(key, preload):
        if key in reg:
            return 'nolabel' if preload and PRELOAD_LABEL not in reg[key] else 'ok'
        return 'unregistered' if key in ship else 'absent'

    rows = []   # (source, side, key, status)
    def port_helper(cls, helper):
        """The helper our port calls where the game calls `helper` (the sibling in the same family, when part A
        shows exactly one swap in that class)."""
        if cls not in diffs: return helper
        g, p = diffs[cls]
        if helper in p or helper not in g: return helper
        swap = [h for h in p - g if h in vfx_helpers]
        return swap[0] if len(swap) == 1 else helper

    for what, tbl, path, tmpl, cls, helper in VFX:
        names = set(values(tbl, path))
        if what == 'HitBox hitVfx':
            # UseElementalHitVfx: HitBox asks for "{hitVfx}_{EElementalType}" instead of the bare name
            names = set()
            for row in table('HitBox'):
                try: info = json.loads(row.get('HitBoxInfo') or '{}')
                except ValueError: continue
                if not info.get('hitVfx'): continue
                if info.get('UseElementalHitVfx'):
                    names.update('%s_%s' % (info['hitVfx'], e) for e in enum_names('EElementalType'))
                else:
                    names.add(info['hitVfx'])
        for side, h in (('game', helper), ('port', port_helper(cls, helper))):
            pfx = (game_pfx if side == 'game' else port_pfx).get(h, '')
            for n in sorted(names):
                key = pfx + tmpl.format(n) + '.prefab'
                rows.append((what, side, key, status(key, True)))
    for what, tbl, path, tmpl, preload in PREFABS:
        for n in sorted(set(values(tbl, path))):
            if n == 'None': continue
            key = tmpl.format(n)
            for side in ('game', 'port'): rows.append((what, side, key, status(key, preload)))
    for what, enum, tmpl, preload in ENUMS:
        for n in enum_names(enum):
            if n in ('None', 'Max', 'Count'): continue
            key = tmpl.format(n)
            for side in ('game', 'port'): rows.append((what, side, key, status(key, preload)))

    # C: literal keys. A literal whose top folder is no address root of the shipped catalog (Assets/..., Packages/...,
    # /TableEncrypted/...) is a file or Resources path, not an Addressables key.
    roots = {k.split('/')[0] for k in ship if '/' in k} - {'Assets', 'Packages'}
    lits = set()
    for line in open(os.path.join(CODE, 'strings.tsv'), encoding='utf-8', errors='replace'):
        p = line.rstrip('\n').split('\t')
        if len(p) >= 3 and KEY_LIT.match(p[2]): lits.add(('game', p[2]))
    for path in glob.glob(os.path.join(SCRIPTS, '**', '*.cs'), recursive=True):
        if '/Editor/' in path: continue
        for s in re.findall(r'"([^"\n]+)"', open(path, encoding='utf-8', errors='replace').read()):
            if KEY_LIT.match(s): lits.add(('port', s))
    for side, key in sorted(lits):
        if key.split('/')[0] not in roots: continue
        rows.append(('literal', side, key, status(key, False)))

    print('\n== B/C. keys by source (game = original key construction, port = ours)')
    by = collections.defaultdict(collections.Counter)
    for what, side, key, st in rows: by[(what, side)][st] += 1
    for (what, side), c in sorted(by.items()):
        bad = sum(v for k, v in c.items() if k != 'ok')
        print('  %-20s %-4s total %4d  ok %4d  %s' % (what, side, sum(c.values()), c['ok'],
              '  '.join('%s %d' % (k, v) for k, v in sorted(c.items()) if k != 'ok') if bad else ''))

    missing = collections.defaultdict(set)
    for what, side, key, st in rows:
        if st != 'ok': missing[(side, st)].add((what, key))
    game_absent = missing[('game', 'absent')]
    # the port's own failures first (keys the original never asks for), then what the shipped data itself lacks
    lists = [('port keys %s' % st, missing[('port', st)] - (game_absent if st == 'absent' else set()))
             for st in ('unregistered', 'nolabel', 'absent')]
    lists += [('game keys %s' % st, missing[('game', st)]) for st in ('unregistered', 'nolabel', 'absent')]
    for title, items in lists:
        if not items: continue
        print('\n== %s: %d' % (title, len(items)))
        for what, key in sorted(items)[: None if verbose else 40]:
            print('  %-20s %s' % (what, key))
        if not verbose and len(items) > 40: print('  ... (--verbose for all)')

    port_bad = {k for (side, st), v in missing.items() if side == 'port' and st != 'absent' for k in v}
    port_absent_extra = missing[('port', 'absent')] - game_absent
    id_rows = {w for w, _, p, _, _ in PREFABS if p == 'Id'}
    named = {(w, k) for w, k in game_absent if w not in id_rows}
    print('\nsummary: builder mismatches %d | port keys not loadable %d (pipeline %d, absent from shipped data only in port %d)'
          ' | game keys absent from shipped data %d (%d named by data, %d table ids with no asset: an upper bound)'
          % (len(diffs), len(port_bad) + len(port_absent_extra), len(port_bad), len(port_absent_extra), len(game_absent),
             len(named), len(game_absent) - len(named)))
    return 1 if diffs or port_bad or port_absent_extra else 0


if __name__ == '__main__':
    sys.exit(main())
