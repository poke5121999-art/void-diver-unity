#!/bin/sh
# tools/unity-run.sh <logname> [extra unity args...]: run one batchmode Unity on the repo, record peak RSS.
# Goes through the host-wide heavy-job queue (~/handoff/heavy.sh): one heavy job at a time across sessions,
# started only when 8 GB is free (measured peak RSS of an import is ~6 GB).
set -u
if [ -z "${VD_IN_HEAVY_QUEUE:-}" ]; then
  VD_IN_HEAVY_QUEUE=1 exec "$HOME/handoff/heavy.sh" "${VD_UNITY_TIMEOUT:-3600}" "${VD_UNITY_MIN_GB:-8}" "$0" "$@"
fi
LOG=~/vd-rip/logs/$1.log; shift
U=~/Unity/Hub/Editor/6000.3.18f1/Editor/Unity
# Rendering needs an X display; batchmode opens no visible window. Borrow the desktop's Xwayland (our user's, with a
# readable auth file); when nobody is logged in, fall back to a private Xvfb (tools/xvfb.sh unpacks one from the
# Ubuntu package, no root needed).
AUTH=$(ps -o args= -u "$(id -u)" -C Xwayland | grep -o "\-auth [^ ]*" | head -1 | cut -d" " -f2)
if [ -z "${DISPLAY:-}" ] && [ -n "$AUTH" ] && [ -r "$AUTH" ]; then
  export DISPLAY=:0 XAUTHORITY="$AUTH"
elif [ -z "${DISPLAY:-}" ]; then
  export DISPLAY=$(sh "$(dirname "$0")/xvfb.sh")
  unset XAUTHORITY
fi
# The references are the English game; this machine's vi_VN locale makes the game pick Vietnamese text and makes
# culture-sensitive parsing read "2.5" as 25. Run under the C locale unless VD_KEEP_LOCALE is set.
[ -n "${VD_KEEP_LOCALE:-}" ] || export LANG=C.UTF-8 LC_ALL=C.UTF-8
QUIT=-quit
case " $* " in *" -runTests "*) QUIT= ;; esac  # the test runner exits by itself and rejects -quit
$U -batchmode $QUIT -projectPath "${VD_PROJECT:-$HOME/void-diver-unity}" -logFile "$LOG" "$@" &
PID=$!
peak=0
# A hung editor keeps burning CPU with a silent log (Burst hashing a bad assembly sat 35 min at 120% CPU).
SILENT_MAX=${VD_UNITY_SILENT_MAX:-600}
hung=0
while kill -0 $PID 2>/dev/null; do
  age=$(( $(date +%s) - $(stat -c %Y "$LOG" 2>/dev/null || date +%s) ))
  if [ "$age" -gt "$SILENT_MAX" ]; then
    echo "unity-run: log silent for ${age}s, killing Unity $PID" >&2
    kill $PID; sleep 10; kill -9 $PID 2>/dev/null; hung=1
    break
  fi
  U_PID=$PID; r=$(awk '/VmRSS/{print $2}' /proc/${U_PID:-0}/status 2>/dev/null || echo 0); [ "${r:-0}" -gt "$peak" ] && peak=$r
  sleep 5
done
wait $PID; rc=$?
echo "exit=$rc hung=$hung peakRSS=$((peak/1024))MB errorsCS=$(grep -E 'error CS' "$LOG" | sort -u | wc -l) crash=$(grep -c 'Caught fatal signal' "$LOG")"
