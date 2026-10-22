#!/bin/sh
# kill-match.sh <regex>: kill processes whose command line matches, never this script or its ancestors.
# Replaces `pkill -f`, which matches the calling shell's own command line and kills it.
set -u
me=$$; anc=" $me "
p=$me; while [ "$p" -gt 1 ]; do p=$(awk '/^PPid:/{print $2}' /proc/$p/status 2>/dev/null || echo 1); anc="$anc$p "; done
for pid in $(ls /proc | grep -E '^[0-9]+$'); do
  case "$anc" in *" $pid "*) continue ;; esac
  cmd=$( { tr "\0" " " < /proc/$pid/cmdline; } 2>/dev/null) || continue
  echo "$cmd" | grep -qE "$1" && kill "$pid" 2>/dev/null && echo "killed $pid ${cmd%% *}"
done
exit 0
