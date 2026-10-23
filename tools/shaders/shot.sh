#!/bin/sh
# tools/shaders/shot.sh <logname> <unity args...>: run VD.EditorTools.ShaderShots.Run (or a Batch with it) and put
# ProjectSettings back afterwards. ShaderShots assigns the URP asset and, with -vdLinear 1, the linear color space in
# memory only, but the editor may write dirty settings on quit; this restores the three files it could touch to their
# bytes from before the run, so a capture never changes project settings.
set -u
HERE=$(cd "$(dirname "$0")" && pwd)
REPO=$(cd "$HERE/../.." && pwd)
SAVE=$(mktemp -d)
for f in GraphicsSettings.asset QualitySettings.asset ProjectSettings.asset; do cp -p "$REPO/ProjectSettings/$f" "$SAVE/$f"; done
"$REPO/tools/unity-run.sh" "$@"
rc=$?
for f in GraphicsSettings.asset QualitySettings.asset ProjectSettings.asset; do
  cmp -s "$SAVE/$f" "$REPO/ProjectSettings/$f" || { cp -p "$SAVE/$f" "$REPO/ProjectSettings/$f"; echo "shot.sh: restored ProjectSettings/$f"; }
done
rm -rf "$SAVE"
exit $rc
