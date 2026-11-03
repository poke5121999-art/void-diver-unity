#!/bin/sh
# gate-confirm.sh <url> <run-dir>: plays the confirm->spinner flows on a web build from a crafted save
# (post-tutorial + campaign 1101 cleared + Owner unlocked + quest 80100 done + exp 2000 + 20 coins) and asserts
# from the page console that each flow ends: popup opened, spinner block released, no safety release.
S=/tmp/claude-1000/-run-media-bui-thanh-thuong-C0E02AC6E02AC310-vd-archive-void-diver-unity/f5d68fb3-474c-4fb1-a69a-c344d77e8e52/scratchpad
url=$1; d=$2; rm -rf $d; mkdir -p $d; touch $d/cmd.jsonl
INIT_SCRIPT=$S/audio-probe.js PLAY=1 setsid nohup timeout 3600 node $S/drive.mjs "$url" $d $S/plan-rep.json > $d.out 2>&1 < /dev/null &
for i in $(seq 1 120); do [ -f $d/done.txt ] && break; sleep 5; done
grep -q '=> "seeded' $d/console.log || { echo "FAIL seed"; exit 1; }
# title: sound after a click
$S/pcmd.sh $d '[{"click":[0.5,0.5]},{"wait":3},{"eval":"__audioLevel()"},{"wait":1},{"eval":"__audioLevel()"}]' 60
# lounge, walk to the Owner (path measured 2026-11-03), Reputation Level -> Yes
$S/pcmd.sh $d '[{"click":[0.6,0.3]},{"wait":2},{"hover":[0.12,0.65]},{"click":[0.12,0.65]},{"wait":8},{"hover":[0.425,0.66]},{"click":[0.425,0.66]},{"wait":50},{"eval":"__audioLevel()"},{"key":"w","hold":1.5},{"wait":0.3},{"key":"a","hold":2.5},{"wait":0.5},{"key":"s","hold":2.5},{"wait":0.5},{"key":"a","hold":1.5},{"wait":0.5},{"key":"a","hold":1.2},{"wait":0.3},{"key":"w","hold":0.6},{"wait":0.6},{"key":"d","hold":0.8},{"wait":0.3},{"key":"s","hold":2.5},{"wait":0.6},{"key":"s","hold":2.0},{"wait":0.6},{"key":"a","hold":1.5},{"wait":0.6},{"key":"a","hold":0.6},{"wait":0.5},{"hover":[0.34,0.5]},{"wait":1},{"shot":"owner"},{"key":"f"},{"wait":2.5},{"hover":[0.55,0.354]},{"click":[0.55,0.354]},{"wait":3},{"shot":"ask"},{"wait":2},{"eval":"console.log(\"GATE click Yes\"),1"},{"hover":[0.84,0.672]},{"click":[0.84,0.672]},{"wait":4},{"shot":"after-yes"},{"wait":20},{"shot":"after-yes-24s"},{"key":"Escape"},{"wait":2},{"click":[0.5,0.5]},{"wait":2},{"key":" "},{"wait":2},{"key":" "},{"wait":2},{"shot":"after-close"},{"eval":"__audioLevel()"}]' 400
echo "--- assertions"
since=$(grep -n '\[log\] GATE click Yes' $d/console.log | head -1 | cut -d: -f1); [ -n "$since" ] || { echo "FAIL Yes never clicked"; exit 1; }
tail -n +$since $d/console.log > $d/after-yes.log
ok=1
grep -q 'VD:Popup\] end open LevelUpPopupPresenter' $d/after-yes.log && echo "PASS LevelUpPopup opened" || { echo "FAIL LevelUpPopup never finished opening"; ok=0; }
grep -q 'VD:Popup\] end open NewContentPopupPresenter' $d/after-yes.log && echo "PASS NewContentPopup opened" || echo "INFO NewContentPopup not opened (no unlock this time)"
grep -q 'VD:Released\]' $d/after-yes.log && { echo "FAIL safety release fired:"; grep 'VD:Released' $d/after-yes.log; ok=0; } || echo "PASS no safety release"
grep -q 'VD:Hang\]' $d/after-yes.log && { echo "FAIL spinner held over 8 s"; ok=0; } || echo "PASS spinner never held 8 s"
grep -q 'VD:Pending\]' $d/after-yes.log && { echo "FAIL pending operations:"; grep -A4 'VD:Pending' $d/after-yes.log | head -8; ok=0; } || echo "PASS nothing pending over 10 s"
grep '__audioLevel' $d/console.log | sed 's/.*=> /audio /'
[ $ok = 1 ] && echo "GATE PASS" || echo "GATE FAIL"
