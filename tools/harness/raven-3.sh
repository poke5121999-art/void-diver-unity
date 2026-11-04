#!/bin/sh
# raven-3.sh <url> <tag> <n>: n independent gate-raven runs, one line per run.
S=/tmp/claude-1000/-run-media-bui-thanh-thuong-C0E02AC6E02AC310-vd-archive-void-diver-unity/f5d68fb3-474c-4fb1-a69a-c344d77e8e52/scratchpad
for k in $(seq 1 $3); do
  timeout 1500 $S/gate-raven.sh "$1" $S/runs/$2-$k > $S/runs/$2-$k.gate 2>&1
  echo "try $k: $(grep -o 'GATE PASS\|GATE FAIL' $S/runs/$2-$k.gate | tail -1) | $(grep -o 'opened on attempt [0-9]\|FAIL[^:]*' $S/runs/$2-$k.gate | tr '\n' ';')"
  pgrep -f "user-data-dir=$S/runs/$2-$k" >/dev/null && { echo "  leftover chrome killed"; pkill -9 -f "user-data-dir=$S/runs/$2-$k"; }
done
