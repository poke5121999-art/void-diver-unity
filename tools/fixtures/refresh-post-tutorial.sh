#!/bin/sh
# Refresh saves-post-tutorial from the saves a passing A4_TutorialFlow run left in captures/review/a4-saves/.
set -eu
root=$(cd "$(dirname "$0")/../.." && pwd)
src=$root/captures/review/a4-saves
dst=$root/tools/fixtures/saves-post-tutorial
[ -f "$src/PlayerContext_0.sav" ] || { echo "no PlayerContext_0.sav in $src; run A4_TutorialFlow first" >&2; exit 1; }
cp "$src"/*.sav "$dst"/
for f in "$dst"/*.sav; do sha256sum "$f"; done
