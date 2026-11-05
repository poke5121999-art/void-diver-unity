#!/bin/sh
# tools/win-prep.sh [dest]: refresh the Windows build copy of this project (default ~/vd-win-proj) for Unity 6000.3, the
# project's own editor. The Windows player is built from a copy because the build switches the active target to
# Windows (a reimport the 6000.3 test lane should not pay) and drops Steam. The first refresh copies this tree's
# Library too, so only what differs for Windows is imported again.
# What differs in the copy:
#   - Steam is gone, as in tools/webgl-prep.sh: no Steamworks.NET package (its editor script re-adds STEAMWORKS_NET to
#     the active target's defines on every load), no SteamNetworkingSockets transport, no STEAMWORKS_NET in the
#     Standalone defines. The player starts without Steam: EnvironmentSettings goes to the Local platform during the
#     build (WindowsBuild.BuildCLI).
#   - No Assets/StreamingAssets/aa (the shipped game's catalog): the build writes its own Addressables content.
#   - InputIcons' Steamworks extension is guarded by STEAMWORKS_NET (tools/webgl/patch-plugins.py).
# Nothing else changes: same editor, packages, URP assets, decals and shaders as the desktop game.
# Then build with:
#   VD_UNITY=~/Unity/Hub/Editor/6000.3.18f1/Editor/Unity VD_PROJECT=<dest> sh tools/unity-run.sh <log> \
#     -buildTarget Win64 -executeMethod VD.EditorTools.WindowsBuild.BuildCLI -vdOut <dir>
set -eu
SRC=$(cd "$(dirname "$0")/.." && pwd)
DEST=${1:-$HOME/vd-win-proj}
mkdir -p "$DEST"
if [ ! -d "$DEST/Library" ] && [ -d "$SRC/Library" ]; then
  rsync -a --exclude=/Library/Bee --exclude=/Library/il2cpp_cache "$SRC/Library" "$DEST/"
fi
rsync -a --delete \
  --exclude=/Library --exclude=/Temp --exclude=/Logs --exclude=/.git --exclude=/captures --exclude='/mono_crash*' \
  --exclude=/SavesDir --exclude=/UserSettings --exclude=/.claude --exclude=/docs --exclude=/.audit --exclude=/brain \
  --exclude=/Packages/com.rlabrecque.steamworks.net \
  --exclude='/Assets/Scripts/SteamNetworkingSockets Transport for Netcode for GameObjects' \
  --exclude='/Assets/Scripts/SteamNetworkingSockets Transport for Netcode for GameObjects.meta' \
  --exclude=/Packages/packages-lock.json \
  --exclude=/Assets/StreamingAssets/aa --exclude=/Assets/StreamingAssets/aa.meta \
  "$SRC/" "$DEST/"
rm -rf "$DEST/Assets/StreamingAssets/aa" "$DEST/Assets/StreamingAssets/aa.meta" "$DEST/Packages/com.rlabrecque.steamworks.net" "$DEST/Assets/Scripts/SteamNetworkingSockets Transport for Netcode for GameObjects" \
  "$DEST/Assets/Scripts/SteamNetworkingSockets Transport for Netcode for GameObjects.meta"
python3 "$SRC/tools/webgl/patch-plugins.py" "$DEST"
python3 - "$DEST" <<'PY'
import re, sys
p = sys.argv[1] + "/ProjectSettings/ProjectSettings.asset"
s = open(p, encoding="utf-8").read()
# Only inside the scriptingDefineSymbols map.
s2 = re.sub(r"(  scriptingDefineSymbols:\n(?:    [^\n]*\n)*?)    Standalone: [^\n]*\n", r"\1", s)
open(p, "w", encoding="utf-8").write(s2)
PY
if grep -A8 "scriptingDefineSymbols:" "$DEST/ProjectSettings/ProjectSettings.asset" | grep -q "STEAMWORKS_NET"; then
  echo "win-prep: STEAMWORKS_NET is still in the defines of $DEST" >&2; exit 1
fi
echo "win-prep: $DEST ready ($(du -sh "$DEST" --exclude=Library | cut -f1) without Library)"
