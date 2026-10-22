#!/bin/sh
# tools/unity-run.sh <logname> [extra unity args...]: run one batchmode Unity on the repo, record peak RSS.
# Goes through the host-wide heavy-job queue (~/handoff/heavy.sh): one heavy job at a time across sessions,
# started only when 8 GB is free (measured peak RSS of an import is ~6 GB).
set -u
if [ -z "${VD_IN_HEAVY_QUEUE:-}" ]; then
  VD_IN_HEAVY_QUEUE=1 exec "$HOME/handoff/heavy.sh" 7300 "${VD_UNITY_MIN_GB:-8}" "$0" "$@"
fi
LOG=~/vd-rip/logs/$1.log; shift
U=~/Unity/Hub/Editor/6000.3.18f1/Editor/Unity
# Rendering needs an X display; batchmode opens no visible window. Borrow the desktop Xwayland and its auth file.
export DISPLAY=${DISPLAY:-:0}
export XAUTHORITY=${XAUTHORITY:-$(ps -o args= -C Xwayland | grep -o "\-auth [^ ]*" | head -1 | cut -d" " -f2)}
$U -batchmode -quit -projectPath "${VD_PROJECT:-$HOME/void-diver-unity}" -logFile "$LOG" "$@" &
PID=$!
peak=0
while kill -0 $PID 2>/dev/null; do
  U_PID=$(pgrep -P $PID -x Unity | head -1); r=$(awk '/VmRSS/{print $2}' /proc/${U_PID:-0}/status 2>/dev/null || echo 0); [ "${r:-0}" -gt "$peak" ] && peak=$r
  sleep 5
done
wait $PID; rc=$?
echo "exit=$rc peakRSS=$((peak/1024))MB errorsCS=$(grep -E 'error CS' "$LOG" | sort -u | wc -l) crash=$(grep -c 'Caught fatal signal' "$LOG")"
