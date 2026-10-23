#!/usr/bin/env python3
"""Dump the shipped SpriteAtlas objects -> tools/rip/sprite_atlases.json (run: ~/vd-rip/venv/bin/python -I sprite_atlases_extract.py)."""
import UnityPy, glob, json, os, subprocess
H = os.path.expanduser('~'); P = H + '/void-diver-unity'
B = glob.glob(H + '/vd-src/VOID*Demo/VOID DIVER_Data/StreamingAssets/aa/StandaloneWindows64')[0]
idx = json.load(open(H + '/vd-rip/bundle_index.json'))
cat = json.load(open(P + '/captures/catalog.json'))
by_path = {}
for g, v in idx.items(): by_path.setdefault(v['pathId'], []).append((g, v))
# project guid -> path (from .meta files)
gp = {}
out = subprocess.run(['grep', '-rH', '^guid: ', '--include=*.meta', P + '/Assets/AssetBundles'], capture_output=True, text=True).stdout
for l in out.splitlines():
    f, _, g = l.partition(':guid: ')
    gp[g.strip()] = os.path.relpath(f[:-5], P)
cat_sa = {l['internalId']: l for l in cat if l['type'].endswith('SpriteAtlas')}
res = []
files = sorted(glob.glob(B + '/remote_spriteatlas_assets_icon_*.bundle') + glob.glob(B + '/dependencies_assets_spriteatlas_*.bundle'))
for f in files:
    env = UnityPy.load(f)
    for o in env.objects:
        if o.type.name != 'SpriteAtlas': continue
        t = o.read_typetree(check_read=False)
        bundle = os.path.basename(f).rsplit('_', 1)[0]
        guid = [g for g, v in by_path.get(o.path_id, []) if v['type'] == 'SpriteAtlas' and v['bundle'] == bundle]
        n2i = {str(k): v for k, v in t['m_PackedSpriteNamesToIndex']} if t['m_PackedSpriteNamesToIndex'] and isinstance(t['m_PackedSpriteNamesToIndex'][0], (list, tuple)) else None
        names = t['m_PackedSpriteNamesToIndex']
        sprites = []
        for i, p in enumerate(t['m_PackedSprites']):
            cands = [(g, v) for g, v in by_path.get(p['m_PathID'], []) if v['type'] == 'Sprite']
            sprites.append({'pathId': p['m_PathID'], 'fileId': p['m_FileID'],
                            'guid': cands[0][0] if cands else None, 'name': cands[0][1]['name'] if cands else None,
                            'bundle': cands[0][1]['bundle'] if cands else None,
                            'project': gp.get(cands[0][0]) if cands else None})
        res.append({'name': t['m_Name'], 'tag': t['m_Tag'], 'isVariant': t['m_IsVariant'], 'guid': guid[0] if guid else None,
                    'bundle': bundle, 'packedSpriteNamesToIndex': [list(x) if isinstance(x, (tuple, list)) else x for x in names],
                    'catalogAddress': cat_sa[guid[0]]['primaryKey'] if guid and guid[0] in cat_sa else None, 'sprites': sprites})
res.sort(key=lambda a: a['name'])
json.dump(res, open(P + '/tools/rip/sprite_atlases.json', 'w'), indent=1)
tot = sum(len(a['sprites']) for a in res); un = sum(1 for a in res for s in a['sprites'] if not s['project'])
print(len(res), 'atlases', tot, 'sprites', un, 'unresolved')
for a in res:
    print(a['name'], a['tag'], a['guid'], a['catalogAddress'], len(a['sprites']), sum(1 for s in a['sprites'] if not s['project']))
