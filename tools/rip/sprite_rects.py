#!/usr/bin/env python3
"""Put back the full rect of every atlas-packed sprite (rerunnable).

AssetRipper exports a sprite that the game packed into a SpriteAtlas with its packed rect: m_Rect becomes
m_RD.textureRect (the trimmed pixels), m_RD.textureRectOffset becomes 0 and m_Offset/m_Pivot are recomputed so the
mesh stays in place. A SpriteRenderer draws the same, but a UI Image sizes the sprite by m_Rect and insets it by the
padding (textureRectOffset), so the trimmed sprite fills the whole RectTransform. The title logo shows it: the game's
Logo sprite is 516x192 with its pixels at (38, 32) and 440x122 in size, the export says 440x122, and the logo drew
516/440 = 1.17x too wide. This writes the shipped m_Rect, m_Offset, m_Pivot and m_RD.textureRectOffset back into each
exported sprite .asset whose shipped rect differs; textureRect, atlasRectOffset and the mesh are already right.

  ~/vd-rip/venv/bin/python -I tools/rip/sprite_rects.py [--dry-run] [game_dir]
"""
import glob, json, os, re, subprocess, sys

import UnityPy

HOME = os.path.expanduser('~')
REPO = os.path.dirname(os.path.dirname(os.path.dirname(os.path.abspath(__file__))))
args = [a for a in sys.argv[1:] if a != '--dry-run']
DRY = '--dry-run' in sys.argv
GAME = args[0] if args else os.path.join(HOME, 'vd-src', 'VOID DIVER Escape from the Abyss Demo')
BUNDLES = os.path.join(GAME, 'VOID DIVER_Data', 'StreamingAssets', 'aa', 'StandaloneWindows64')
INDEX = json.load(open(os.path.join(HOME, 'vd-rip', 'bundle_index.json')))

# (bundle, pathId) -> original GUID, which the exported .asset keeps as its own GUID.
guid_of = {(v['bundle'], v['pathId']): g for g, v in INDEX.items() if v['type'] == 'Sprite'}
path_of = {}
out = subprocess.run(['grep', '-rH', '^guid: ', '--include=*.asset.meta', os.path.join(REPO, 'Assets')],
                     capture_output=True, text=True).stdout
for line in out.splitlines():
    f, _, g = line.partition(':guid: ')
    path_of[g.strip()] = f[:-5]


def num(v):
    return ('%.9g' % v) if v != int(v) else str(int(v))


def patch(text, t):
    r, o, p, tro = t['m_Rect'], t['m_Offset'], t['m_Pivot'], t['m_RD']['textureRectOffset']
    rect = ('  m_Rect:\n    serializedVersion: 2\n    x: %s\n    y: %s\n    width: %s\n    height: %s\n'
            % (num(r['x']), num(r['y']), num(r['width']), num(r['height'])))
    text, n1 = re.subn(r'  m_Rect:\n    serializedVersion: 2\n    x: .*\n    y: .*\n    width: .*\n    height: .*\n',
                       lambda _: rect, text, count=1)
    text, n2 = re.subn(r'(?m)^  m_Offset: \{.*\}$', '  m_Offset: {x: %s, y: %s}' % (num(o['x']), num(o['y'])), text, count=1)
    text, n3 = re.subn(r'(?m)^  m_Pivot: \{.*\}$', '  m_Pivot: {x: %s, y: %s}' % (num(p['x']), num(p['y'])), text, count=1)
    # The first textureRectOffset is m_RD's; m_AtlasRD's comes after it.
    text, n4 = re.subn(r'(?m)^    textureRectOffset: \{.*\}$',
                       '    textureRectOffset: {x: %s, y: %s}' % (num(tro['x']), num(tro['y'])), text, count=1)
    return text, (n1, n2, n3, n4) == (1, 1, 1, 1)


def exported_rect(text):
    m = re.search(r'  m_Rect:\n    serializedVersion: 2\n    x: .*\n    y: .*\n    width: (.*)\n    height: (.*)\n', text)
    return (float(m.group(1)), float(m.group(2))) if m else None


changed = same = missing = failed = 0
for f in sorted(glob.glob(os.path.join(BUNDLES, '*.bundle'))):
    bundle = os.path.basename(f).rsplit('_', 1)[0]
    if not any(b == bundle for b, _ in guid_of):
        continue
    env = UnityPy.load(f)
    for obj in env.objects:
        if obj.type.name != 'Sprite':
            continue
        guid = guid_of.get((bundle, obj.path_id))
        path = path_of.get(guid)
        if not path:
            missing += 1
            continue
        t = obj.read_typetree(check_read=False)
        text = open(path, encoding='utf-8').read()
        new, ok = patch(text, t)
        if not ok:
            failed += 1
            print('SPRITE_RECT unpatchable %s' % os.path.relpath(path, REPO))
            continue
        if new == text:
            same += 1
            continue
        changed += 1
        was = exported_rect(text)
        print('SPRITE_RECT %s %gx%g -> %gx%g' % (os.path.relpath(path, REPO), was[0], was[1],
                                                 t['m_Rect']['width'], t['m_Rect']['height']))
        if not DRY:
            open(path, 'w', encoding='utf-8').write(new)
print('SPRITE_RECTS changed=%d same=%d missing=%d failed=%d%s' % (changed, same, missing, failed, ' (dry run)' if DRY else ''))
sys.exit(1 if failed else 0)
