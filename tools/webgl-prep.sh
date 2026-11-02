#!/bin/sh
# tools/webgl-prep.sh [dest]: refresh the WebGL build copy of this project (default ~/vd-webgl-proj) for Unity 6000.0.
# The web player is built from a copy, never from this tree: opening the repo with 6000.0 would downgrade it and
# reimport the Library the 6000.3 test lane uses. The copy keeps its own Library between runs.
# What differs in the copy:
#   - Steam is gone: no Steamworks.NET package (its editor script re-adds STEAMWORKS_NET to the active target's
#     defines on every load, which turned the guards back on for WebGL), no SteamNetworkingSockets transport, and
#     no STEAMWORKS_NET in the WebGL defines.
#   - The packages built for 6000.3 are pinned to the versions that ship with 6000.0.77f1, and the URP assets to
#     the asset versions URP 17.0.4 builds with.
#   - No Assets/StreamingAssets/aa: that is the shipped game's Addressables catalog, and the player build refuses a
#     StreamingAssets/aa that its own Addressables build did not write.
#   - The ripped plugins get the WebGL compile fixes of tools/webgl/patch-plugins.py.
# Then build with:
#   VD_UNITY=~/Unity/Hub/Editor/6000.0.77f1/Editor/Unity VD_PROJECT=<dest> VD_UNITY_TIMEOUT=18000 \
#     VD_UNITY_SILENT_MAX=3600 sh tools/unity-run.sh <log> -buildTarget WebGL -executeMethod VD.EditorTools.WebGLBuild.BuildCLI
set -eu
SRC=$(cd "$(dirname "$0")/.." && pwd)
DEST=${1:-$HOME/vd-webgl-proj}
mkdir -p "$DEST"
rsync -a --delete \
  --exclude=/Library --exclude=/Temp --exclude=/Logs --exclude=/.git --exclude=/captures --exclude='/mono_crash*' \
  --exclude=/SavesDir --exclude=/UserSettings --exclude=/.claude --exclude=/docs --exclude=/.audit \
  --exclude=/Packages/com.rlabrecque.steamworks.net \
  --exclude='/Assets/Scripts/SteamNetworkingSockets Transport for Netcode for GameObjects' \
  --exclude='/Assets/Scripts/SteamNetworkingSockets Transport for Netcode for GameObjects.meta' \
  --exclude=/Packages/packages-lock.json \
  --exclude=/Assets/StreamingAssets/aa --exclude=/Assets/StreamingAssets/aa.meta \
  --exclude=/ProjectSettings/ProjectVersion.txt \
  "$SRC/" "$DEST/"
# rsync leaves excluded paths alone in the copy; remove the Steam ones a previous refresh may have brought.
rm -rf "$DEST/Assets/StreamingAssets/aa" "$DEST/Assets/StreamingAssets/aa.meta" "$DEST/Packages/com.rlabrecque.steamworks.net" "$DEST/Assets/Scripts/SteamNetworkingSockets Transport for Netcode for GameObjects" \
  "$DEST/Assets/Scripts/SteamNetworkingSockets Transport for Netcode for GameObjects.meta"
python3 "$SRC/tools/webgl/patch-plugins.py" "$DEST"
python3 - "$DEST" <<'EOF'
import json, re, sys
dest = sys.argv[1]
p = dest + "/Packages/manifest.json"
d = json.load(open(p))
d["dependencies"].update({
    "com.unity.render-pipelines.core": "17.0.4",
    "com.unity.render-pipelines.universal": "17.0.4",
    "com.unity.2d.animation": "10.2.2",
    "com.unity.2d.spriteshape": "10.0.7",
})
json.dump(d, open(p, "w"), indent=2)
p = dest + "/ProjectSettings/ProjectSettings.asset"
s = open(p, encoding="utf-8").read()
# Only inside the scriptingDefineSymbols map: other per-platform maps also have "    WebGL:" keys.
s2 = re.sub(r"(  scriptingDefineSymbols:\n(?:    [^\n]*\n)*?)    WebGL: [^\n]*\n", r"\1", s)
open(p, "w", encoding="utf-8").write(s2)
EOF
# URP 17.0.4 refuses to build with assets saved by 17.3 ("is not at last version"): its last versions are 12 for the
# pipeline asset and 8 for the global settings. Write those numbers; the fields both versions share load as before.
sed -i -E 's/^(  k_AssetVersion|  k_AssetPreviousVersion): 13$/\1: 12/' "$DEST/Assets/MonoBehaviour/UniveralRP_VD.asset"
sed -i -E 's/^  m_AssetVersion: 10$/  m_AssetVersion: 8/' "$DEST/Assets/MonoBehaviour/UniversalRenderPipelineGlobalSettings.asset"
# WebGL 2 is OpenGL ES 3: the DBuffer decal technique is refused there, every frame ("Decal DBuffer technique is not
# supported with OpenGL"). Automatic (0) picks screen-space decals on GL and DBuffer where it can.
sed -i -E 's/^    technique: 1$/    technique: 0/' "$DEST"/Assets/MonoBehaviour/DecalRendererFeature*.asset
# WebGL 2 runs shader target 3.5-4.0 at most. These shaders declare 5.0 (the target of the shipped D3D11 bytecode) but
# use nothing past 4.0 (Texture.Load, discard), and a shader the GPU refuses is drawn with Unity's magenta error
# shader: MaskArea and CutoffMask, meant for the CustomDepth buffer only, showed as magenta areas in the map
# ("LCArt/Character/Shader_MaskArea shader is not supported on this GPU", 2026-11-02).
for s in LCArt_Character_Shader_MaskArea LCArt_Background_shader_DE_BG_CutoffMask Shader_Graphs_Shader_Decoration_Puddle Shader_Graphs_shader_DE_BG_ReflectionSurface_Floor; do
  f="$DEST/Assets/_VD/Shaders/$s.shader"
  grep -q '#pragma target 5.0' "$f" || { echo "webgl-prep: $f has no '#pragma target 5.0' to lower" >&2; exit 1; }
  sed -i 's/#pragma target 5\.0$/#pragma target 4.0/' "$f"
done
printf 'm_EditorVersion: 6000.0.77f1\nm_EditorVersionWithRevision: 6000.0.77f1\n' > "$DEST/ProjectSettings/ProjectVersion.txt"
if grep -A8 "scriptingDefineSymbols:" "$DEST/ProjectSettings/ProjectSettings.asset" | grep -q "WebGL:.*STEAMWORKS_NET"; then
  echo "webgl-prep: STEAMWORKS_NET is still in the WebGL defines of $DEST" >&2; exit 1
fi
echo "webgl-prep: $DEST ready ($(du -sh "$DEST" --exclude=Library | cut -f1) without Library)"
