#!/bin/sh
# Refresh saves-post-tutorial from the saves a passing A4_TutorialFlow run left in captures/review/a4-saves/.
set -eu
root=$(cd "$(dirname "$0")/../.." && pwd)
src=$root/captures/review/a4-saves
dst=$root/tools/fixtures/saves-post-tutorial
[ -f "$src/PlayerContext_0.sav" ] || { echo "no PlayerContext_0.sav in $src; run A4_TutorialFlow first" >&2; exit 1; }
grep -q '^test=VD.PlayTests.A4_TutorialFlow\.' "$src/PROVENANCE.txt" 2>/dev/null \
  || { echo "$src has no A4_TutorialFlow provenance (exported by another test, or by an A4 older than PROVENANCE.txt)" >&2; exit 1; }
cat "$src/PROVENANCE.txt"
for f in PlayerContext_0.sav AccountContext.sav GamePlaySettingContext.sav AccountRecordContext.sav; do
  [ -f "$src/$f" ] && cp "$src/$f" "$dst/"
done
for f in "$dst"/*.sav; do sha256sum "$f"; done
