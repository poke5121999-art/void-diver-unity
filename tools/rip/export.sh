#!/bin/sh
# Export VOID DIVER as a raw Unity project with AssetRipper (headless REST API).
# Usage: tools/rip/export.sh [game_dir] [out_dir]
# Rerunnable: the output dir is wiped first.
set -eu

GAME=${1:-"$HOME/vd-src/VOID DIVER Escape from the Abyss Demo"}
OUT=${2:-"$HOME/vd-rip/ar-export"}
AR=${AR:-"$HOME/opt/assetripper-2.0.0/AssetRipper.GUI.Free"}
PORT=${PORT:-18765}
LOG="$HOME/vd-rip/logs/assetripper.log"

[ -x "$AR" ] || { echo "AssetRipper binary not found at $AR (download 2.0.0 linux_x64 from github.com/AssetRipper/AssetRipper/releases)"; exit 1; }
[ -d "$GAME" ] || { echo "game dir not found: $GAME"; exit 1; }
mkdir -p "$(dirname "$LOG")"
rm -rf "$OUT"

# AssetRipper holds ~6.5 GB while the game is loaded; it runs through the host-wide heavy-job queue.
if [ -z "${VD_IN_HEAVY_QUEUE:-}" ]; then
  VD_IN_HEAVY_QUEUE=1 exec "$HOME/handoff/heavy.sh" 3600 8 "$0" "$GAME" "$OUT"
fi
"$AR" --headless --port "$PORT" --log-path "$LOG" >/dev/null 2>&1 &
PID=$!
trap 'kill $PID 2>/dev/null || true' EXIT INT TERM

i=0
until curl -s -o /dev/null "localhost:$PORT/"; do
  i=$((i + 1)); [ $i -lt 60 ] || { echo "AssetRipper did not start"; exit 1; }
  sleep 1
done

# Settings that differ from the defaults. ScriptExportMode is forced to Hybrid by AssetRipper for IL2CPP.
curl -s -o /dev/null -X POST "localhost:$PORT/Settings/Update" \
  -d 'BundledAssetsExportMode=GroupByBundleName&ScriptContentLevel=Level2&AudioExportFormat=Default&ImageExportFormat=Png&LightmapTextureExportFormat=Exr&SpriteExportMode=Yaml&ShaderExportMode=Dummy&TextExportMode=Parse&ScriptLanguageVersion=AutoSafe&ScriptExportMode=Decompiled&PreferOriginalTextureExtension=&DefaultVersion=0.0.0a0&TargetVersion=0.0.0a0'

timeout 1800 curl -s -o /dev/null -X POST --data-urlencode "path=$GAME" "localhost:$PORT/LoadFolder"
timeout 3600 curl -s -o /dev/null -X POST --data-urlencode "path=$OUT" "localhost:$PORT/Export/UnityProject"

n=$(grep -c 'ExportProgress' "$LOG" || true)
e=$(grep -c '\[Error\]' "$LOG" || true)
echo "exported to $OUT: $n progress lines, $e errors (log: $LOG)"
