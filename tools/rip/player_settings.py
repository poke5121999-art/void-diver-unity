"""Copy the shipped game's PlayerSettings into ProjectSettings/ProjectSettings.asset (rerunnable).

AssetRipper does not export PlayerSettings, so Unity seeded the project with its defaults: Gamma colour space,
the old Input Manager, and no preloaded assets (without VContainerSettings preloaded, VContainer never builds
RootLifetimeScope and every [Inject] field stays null). This reads PlayerSettings from data.unity3d and writes
every top-level scalar the project file also has, plus preloadedAssets resolved by asset name.

  ~/vd-rip/venv/bin/python -I tools/rip/player_settings.py [game_data_dir]
"""
import os, re, sys

import UnityPy

REPO = os.path.dirname(os.path.dirname(os.path.dirname(os.path.abspath(__file__))))
DATA = sys.argv[1] if len(sys.argv) > 1 else os.path.expanduser(
    '~/vd-src/VOID DIVER Escape from the Abyss Demo/VOID DIVER_Data')
TARGET = os.path.join(REPO, 'ProjectSettings', 'ProjectSettings.asset')

env = UnityPy.load(os.path.join(DATA, 'data.unity3d'))
ps = next(o for o in env.objects if o.type.name == 'PlayerSettings')
orig = ps.read_typetree(check_read=False)

names = {}
for o in env.objects:
    if o.assets_file.name.endswith('globalgamemanagers.assets'):
        try:
            names[o.path_id] = o.peek_name()
        except Exception:
            pass
preload = [names[p['m_PathID']] for p in orig.get('preloadedAssets', [])]


def guid_of(asset_name):
    for root, _, files in os.walk(os.path.join(REPO, 'Assets')):
        for f in files:
            # Skip emptied copies: SoftMask moves its settings into its own asset and blanks the exported one.
            if (f.endswith('.meta') and f[:-5].rsplit('.', 1)[0] == asset_name
                    and os.path.getsize(os.path.join(root, f[:-5])) > 100):
                m = re.search(r'guid: ([0-9a-f]{32})', open(os.path.join(root, f)).read())
                if m:
                    return m.group(1)
    raise SystemExit('preloaded asset %s not found under Assets/' % asset_name)


def yaml_scalar(v):
    if isinstance(v, bool):
        return '1' if v else '0'
    if isinstance(v, float):  # stored as float32; print the shortest decimal that round-trips it
        return '%.7g' % v
    return str(v)


lines = open(TARGET).read().split('\n')
changed = []
for i, line in enumerate(lines):
    m = re.match(r'^  ([A-Za-z_][A-Za-z0-9_]*): ?(.*)$', line)
    if not m or m.group(1) not in orig:
        continue
    key, cur, v = m.group(1), m.group(2), orig[m.group(1)]
    if isinstance(v, (bool, int, float, str)) and not (isinstance(v, str) and '\n' in v):
        new = yaml_scalar(v)
        if cur != new and not cur.startswith(('{', '[')):
            lines[i] = '  %s: %s' % (key, new)
            changed.append('%s %s -> %s' % (key, cur, new))

start = next(i for i, l in enumerate(lines) if l == '  preloadedAssets:')
end = start + 1
while end < len(lines) and lines[end].startswith('  - '):
    end += 1
entries = ['  - {fileID: 11400000, guid: %s, type: 2}' % guid_of(n) for n in preload]
if lines[start + 1:end] != entries:
    changed.append('preloadedAssets -> %s' % ', '.join(preload))
    lines[start + 1:end] = entries

open(TARGET, 'w').write('\n'.join(lines))
print('\n'.join(changed) or 'no change')
print('PLAYER_SETTINGS changed=%d preload=%d' % (len(changed), len(preload)))
