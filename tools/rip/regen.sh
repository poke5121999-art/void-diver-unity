#!/bin/sh
# Regenerate every git-ignored asset of this project from the installed game.
# Needs: the game folder (default ~/vd-src/VOID DIVER Escape from the Abyss Demo), Unity 6000.3.18f1,
# AssetRipper 2.0.0 (tools/rip/export.sh says where to get it), python with no extra modules.
set -eu
HERE=$(cd "$(dirname "$0")" && pwd)
GAME=${1:-"$HOME/vd-src/VOID DIVER Escape from the Abyss Demo"}
WORK=${VD_WORK:-"$HOME/vd-rip"}

sh "$HERE/fetch_donors.sh" "$WORK/donor"
sh "$HERE/export.sh" "$GAME" "$WORK/ar-export"
python3 -I "$HERE/build_project.py" "$WORK/ar-export/ExportedProject" "$WORK/donor" "$WORK/stage" --report "$WORK/build_report.json"
python3 -I "$HERE/fix_stubs.py" "$WORK/stage/Assets/Scripts"
sh "$HERE/sync.sh" "$WORK/stage"
