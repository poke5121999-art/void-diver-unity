"""Index every Addressables bundle container entry: original asset GUID -> bundle, type, object name, path id.

Usage: python bundle_index.py <StreamingAssets/aa/StandaloneWindows64> <out.json>   (needs UnityPy; heavy, run
through ~/handoff/heavy.sh). The container key of this game's bundles is the original asset GUID, which is also the
catalog's internal id, so this joins catalog addresses to bundle objects.
"""
import json, os, sys
import UnityPy

src, dst = sys.argv[1], sys.argv[2]
out = {}
for f in sorted(os.listdir(src)):
    if not f.endswith('.bundle'):
        continue
    bundle = f.rsplit('_', 1)[0]
    env = UnityPy.load(os.path.join(src, f))
    for key, obj in env.container.items():
        if obj.path_id == 0:
            continue
        try:
            name = obj.peek_name() if hasattr(obj, 'peek_name') else obj.read().m_Name
        except Exception:
            name = None
        out[key] = {'bundle': bundle, 'type': obj.type.name, 'name': name, 'pathId': obj.path_id}
json.dump(out, open(dst, 'w'), indent=0, sort_keys=True)
print(f'{len(out)} container entries -> {dst}')
