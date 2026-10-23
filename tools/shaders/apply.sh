#!/bin/sh
# tools/shaders/apply.sh: tools/rip/rebuild.sh with the rebuilt shaders counted as present.
# build_project.py remaps every material from an AssetRipper dummy to Assets/_VD/Shaders by shader name, but the stage
# it builds never contains Assets/_VD (sync.sh leaves the repo's _VD alone), so rebuild.sh's check_refs sees every
# remapped shader GUID as dangling (243 GUIDs / 2249 references instead of the 148/223 baseline) and refuses to sync.
# This runs the same steps under the same lock and passes the repo's Assets/_VD to check_refs as a known root.
# Drop it once rebuild.sh does the same.
set -eu
HERE=$(cd "$(dirname "$0")" && pwd)
RIP="$HERE/../rip"
REPO=$(cd "$HERE/../.." && pwd)
WORK=${VD_WORK:-"$HOME/vd-rip"}
PY=${VD_PY:-python3}
exec 8>"$WORK/build.lock"
flock 8
"$PY" -I "$RIP/build_project.py" "$WORK/ar-export/ExportedProject" "$WORK/donor" "$WORK/stage" --report "$WORK/build_report.json" > "$WORK/build.out"
tail -3 "$WORK/build.out"
python3 -I "$RIP/fix_stubs.py" "$WORK/stage/Assets/Scripts"
python3 -I "$RIP/check_refs.py" "$WORK/stage" "$WORK"/donor/com.unity.* "$REPO/Assets/_VD" > "$WORK/check_refs.out" || true
tail -1 "$WORK/check_refs.out"
grep -q '^dangling guids: 148, references: 223$' "$WORK/check_refs.out" || { echo "check_refs differs from the raw export baseline (148/223); not syncing"; exit 1; }
sh "$RIP/sync.sh" "$WORK/stage"
