"""Write tools/rip/bundle_names.json: Addressables bundle hash name -> readable bundle file name.

AssetRipper's GroupByBundleName export names folders after the AssetBundle's internal name, which in this
game is a hash ("75ce627e...bundle"). The bundle file on disk carries the readable group name
("dependencies_assets_animation_<hash>.bundle").
Usage: python bundle_names.py <StreamingAssets/aa/StandaloneWindows64> <out.json>   (needs UnityPy)
"""
import json, os, sys
import UnityPy

src, dst = sys.argv[1], sys.argv[2]
out = {}
for f in sorted(os.listdir(src)):
    if not f.endswith('.bundle'):
        continue
    env = UnityPy.load(os.path.join(src, f))
    for o in env.objects:
        if o.type.name == 'AssetBundle':
            out[o.read().m_Name.removesuffix('.bundle')] = f.rsplit('_', 1)[0]
json.dump(out, open(dst, 'w'), indent=1, sort_keys=True)
print(f'{len(out)} bundles -> {dst}')
