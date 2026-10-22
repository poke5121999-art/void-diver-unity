#!/bin/sh
# tools/shaders/run.sh: rebuild the game's own shaders from the shipped DXBC into Assets/_VD/Shaders.
#   extract (UnityPy, parsed form + DXBC + cbuffer layouts) -> decompile (3Dmigoto cmd_Decompiler under wine)
#   -> generate (ShaderLab + per-variant HLSL includes with deterministic GUIDs) -> rsync into the repo.
# Rerunnable; work files live in ~/vd-rip/shaders-work (outside git). The bundle sweep for shaders that are not in
# the shader bundle reads every Addressables bundle, so it goes through the host-wide heavy queue.
set -eu
HERE=$(cd "$(dirname "$0")" && pwd)
REPO=$(cd "$HERE/../.." && pwd)
WORK=${VD_SHADER_WORK:-"$HOME/vd-rip/shaders-work"}
PY=${VD_PY:-"$HOME/vd-rip/venv/bin/python"}
GAME=${VD_GAME:-"$HOME/vd-src/VOID DIVER Escape from the Abyss Demo/VOID DIVER_Data"}
AA="$GAME/StreamingAssets/aa/StandaloneWindows64"
mkdir -p "$WORK"

# Shader names still on an AssetRipper dummy: from the last census (captures/census.json), keys prefixed "STUB ".
python3 - "$REPO/captures/census.json" "$WORK/stub_names.json" <<'EOF'
import json, sys
c = json.load(open(sys.argv[1]))
json.dump(sorted(k[5:] for k in c['materialsByShader'] if k.startswith('STUB ')), open(sys.argv[2], 'w'), indent=0)
EOF

case "${1:-all}" in
  extract|all)
    rm -rf "$WORK/x1"
    "$PY" -I "$HERE/extract.py" "$WORK/x1" --names "$WORK/stub_names.json" "$AA"/dependencies_assets_shader_*.bundle
    ls "$AA"/*.bundle | grep -v dependencies_assets_shader > "$WORK/sweep_list.txt"
    echo "$GAME/data.unity3d" >> "$WORK/sweep_list.txt"
    # shaders outside the shader bundle (post-processing, Hidden/*): sweep every container, heavy
    "$HOME/handoff/heavy.sh" 3600 6 "$PY" -I "$HERE/extract.py" "$WORK/x1" --names "$WORK/stub_names.json" --list "$WORK/sweep_list.txt"
    [ "${1:-all}" = extract ] && exit 0 ;;
esac
case "${1:-all}" in
  decompile|all)
    python3 "$HERE/decompile.py" "$WORK/x1" --jobs 6
    [ "${1:-all}" = decompile ] && exit 0 ;;
esac
rm -rf "$WORK/gen"
python3 "$HERE/generate.py" "$WORK/x1" "$WORK/gen" --report "$WORK/generate_report.json"
mkdir -p "$REPO/Assets/_VD/Shaders"
rsync -rlc --delete "$WORK/gen/" "$REPO/Assets/_VD/Shaders/"
echo "installed $(ls "$REPO"/Assets/_VD/Shaders/*.shader | wc -l) shaders into Assets/_VD/Shaders; report: $WORK/generate_report.json"
