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
python3 -I "$HERE/check_refs.py" "$WORK/stage" "$WORK"/donor/com.unity.* > "$WORK/check_refs.out" || true
tail -1 "$WORK/check_refs.out"
grep -q '^dangling guids: 148, references: 223$' "$WORK/check_refs.out" || { echo "check_refs differs from the raw export baseline (148/223); not syncing"; exit 1; }
sh "$HERE/sync.sh" "$WORK/stage"
