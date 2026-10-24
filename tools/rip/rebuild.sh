#!/bin/sh
# Rebuild the project from the existing export and donors, then sync it into the repo.
# Serialized on ~/vd-rip/build.lock because every caller writes the same stage dir and repo.
set -eu
HERE=$(cd "$(dirname "$0")" && pwd)
WORK=${VD_WORK:-"$HOME/vd-rip"}
PY=${VD_PY:-python3}
exec 8>"$WORK/build.lock"
flock 8
# Each step must succeed before sync touches the repo; no pipes, so a failure stops here.
"$PY" -I "$HERE/build_project.py" "$WORK/ar-export/ExportedProject" "$WORK/donor" "$WORK/stage" --report "$WORK/build_report.json" > "$WORK/build.out"
tail -3 "$WORK/build.out"
python3 -I "$HERE/fix_stubs.py" "$WORK/stage/Assets/Scripts"
python3 -I "$HERE/check_refs.py" "$WORK/stage" "$WORK"/donor/com.unity.* "$HERE/../../Assets/_VD" > "$WORK/check_refs.out" || true
tail -1 "$WORK/check_refs.out"
grep -q '^dangling guids: 148, references: 223$' "$WORK/check_refs.out" || { echo "check_refs differs from the raw export baseline (148/223); not syncing"; exit 1; }
sh "$HERE/sync.sh" "$WORK/stage"
python3 -I "$HERE/repo_guids.py" "$WORK/build_report.json"
# The export keeps atlas-packed sprites at their trimmed rect; put the shipped rect and padding back (UI Image sizes by it).
"$WORK/venv/bin/python" -I "$HERE/sprite_rects.py" > "$WORK/sprite_rects.out"
tail -1 "$WORK/sprite_rects.out"
# Material-property curves the export left as hashes get their names back (the editor binds curves by name).
python3 -I "$HERE/anim_material_names.py" > "$WORK/anim_material_names.out"
tail -1 "$WORK/anim_material_names.out"
# URP's default volume profile reference lives in a [SerializeReference] entry the export empties.
python3 -I "$HERE/urp_default_profile.py"
# ExposedReference names come out as `id: <hash>`; the editor reads a plain string, else Control tracks find nothing.
python3 -I "$HERE/exposed_names.py"
