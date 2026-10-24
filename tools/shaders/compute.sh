#!/bin/sh
# tools/shaders/compute.sh: rebuild the game's compute shaders from the shipped DXBC into Assets/_VD/Shaders/Compute.
#   compute_extract (UnityPy: kernels, bindings, cbuffer layouts, D3D11 DXBC; every bundle + data.unity3d, heavy queue)
#   -> 3Dmigoto cmd_Decompiler -d (disassembly) -> compute_generate (asm2hlsl.translate_cs, one .compute per shader)
#   -> rsync into the repo. Rerunnable; work files in ~/vd-rip/shaders-work/cx. References are remapped by apply.sh
# (compute_remap.py on the stage).
set -eu
HERE=$(cd "$(dirname "$0")" && pwd)
REPO=$(cd "$HERE/../.." && pwd)
WORK=${VD_SHADER_WORK:-"$HOME/vd-rip/shaders-work"}
PY=${VD_PY:-"$HOME/vd-rip/venv/bin/python"}
GAME=${VD_GAME:-"$HOME/vd-src/VOID DIVER Escape from the Abyss Demo/VOID DIVER_Data"}
TOOL=$HOME/vd-rip/third_party/cmd_decompiler/cmd_Decompiler.exe
mkdir -p "$WORK"
rm -rf "$WORK/cx" "$WORK/cgen"
ls "$GAME"/StreamingAssets/aa/StandaloneWindows64/*.bundle > "$WORK/cx_sweep_list.txt"
echo "$GAME/data.unity3d" >> "$WORK/cx_sweep_list.txt"
"$HOME/handoff/heavy.sh" 3600 4 "$PY" -I "$HERE/compute_extract.py" "$WORK/cx" --list "$WORK/cx_sweep_list.txt" > "$WORK/cx_sweep.log"
for d in "$WORK"/cx/*/dxbc; do
  (cd "$d" && WINEDEBUG=-all wine "$TOOL" -d ./*.dxbc > /dev/null 2>&1) || true
done
python3 "$HERE/compute_generate.py" "$WORK/cx" "$WORK/cgen" --donor "${VD_DONOR:-$HOME/vd-rip/donor}" --report "$WORK/compute_report.json"
# the folder's own .meta keeps a fixed GUID (md5("vdcompute:Compute")) so a fresh checkout imports identically
printf 'fileFormatVersion: 2\nguid: %s\nfolderAsset: yes\nDefaultImporter:\n  externalObjects: {}\n  userData: \n  assetBundleName: \n  assetBundleVariant: \n' \
  "$(printf 'vdcompute:Compute' | md5sum | cut -c1-32)" > "$REPO/Assets/_VD/Shaders/Compute.meta"
mkdir -p "$REPO/Assets/_VD/Shaders/Compute"
rsync -rlc --delete "$WORK/cgen/" "$REPO/Assets/_VD/Shaders/Compute/"
echo "installed $(ls "$REPO"/Assets/_VD/Shaders/Compute/*.compute | wc -l) compute shaders into Assets/_VD/Shaders/Compute; report: $WORK/compute_report.json"
