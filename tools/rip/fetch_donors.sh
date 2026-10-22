#!/bin/sh
# Collect real package sources ("donors") whose script and shader GUIDs replace AssetRipper's stubs.
# Unity packages come from the tarballs bundled with the editor; Spine 4.2 from its git repo.
set -eu
HERE=$(cd "$(dirname "$0")" && pwd)
EDITOR=${UNITY_EDITOR_DIR:-"$HOME/Unity/Hub/Editor/6000.3.18f1/Editor"}
DONOR=${1:-"$HOME/vd-rip/donor"}
SPINE_REV=${SPINE_REV:-e7dc1435fa4a0083ab431f1b28e083c14a1f5c68}
PM="$EDITOR/Data/Resources/PackageManager"
mkdir -p "$DONOR"
cd "$DONOR"

for p in com.unity.timeline-1.8.12 com.unity.inputsystem-1.19.0 com.unity.cinemachine-2.10.7 \
  com.unity.ai.navigation-2.0.13 com.unity.2d.animation-13.0.5 com.unity.2d.common-12.0.3 \
  com.unity.addressables-2.9.1 com.unity.scriptablebuildpipeline-2.6.1 com.unity.burst-1.8.29 \
  com.unity.collections-2.6.6 com.unity.mathematics-1.3.3 com.unity.netcode.gameobjects-2.12.0 \
  com.unity.transport-2.7.3 com.unity.nuget.newtonsoft-json-3.2.2 com.unity.2d.spriteshape-13.0.0 \
  com.unity.profiling.core-1.0.3; do
  n=${p%-*}
  rm -rf "$n"; mkdir -p "$n"
  tar xzf "$PM/Editor/$p.tgz" -C "$n" --strip-components=1
done
for n in com.unity.ugui com.unity.render-pipelines.universal com.unity.render-pipelines.core com.unity.shadergraph com.unity.textmeshpro; do
  ln -sfn "$PM/BuiltInPackages/$n" "$n"
done

if [ ! -d spine-runtimes/.git ]; then
  timeout 600 git clone -q --filter=blob:none --sparse --branch 4.2 https://github.com/EsotericSoftware/spine-runtimes.git spine-runtimes
fi
git -C spine-runtimes sparse-checkout set spine-csharp spine-unity/Assets/Spine spine-unity/Modules
git -C spine-runtimes fetch -q --depth 1 origin "$SPINE_REV" 2>/dev/null || true
git -C spine-runtimes checkout -q "$SPINE_REV"

# NuGet DLLs for the stubs listed in packages.py NUGET.
mkdir -p nuget
python3 -I - "$HERE" <<'PY'
import io, os, sys, urllib.request, zipfile
sys.path.insert(0, sys.argv[1])
from packages import NUGET
for pid, ver in set(NUGET.values()):
    dst = os.path.join('nuget', f'{pid}.dll')
    if os.path.exists(dst):
        continue
    url = f'https://www.nuget.org/api/v2/package/{pid}/{ver}'
    z = zipfile.ZipFile(io.BytesIO(urllib.request.urlopen(url, timeout=120).read()))
    for tfm in ('netstandard2.1', 'netstandard2.0'):
        name = f'lib/{tfm}/{pid}.dll'
        if name in z.namelist():
            open(dst, 'wb').write(z.read(name))
            print('nuget', pid, ver, tfm)
            break
    else:
        sys.exit(f'no netstandard dll in {pid} {ver}')
PY
echo "donors ready in $DONOR"
