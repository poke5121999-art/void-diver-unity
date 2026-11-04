#!/bin/sh
# gate-raven.sh <url> <run-dir>: from the post-tutorial save, pick the 4th diver (Raven) at Narcis, then reload the page
# and Continue. Asserts from the page console: no ArgumentNullException, the character respawns, Continue reaches
# stage state Playing, and the lounge shows Raven.
S=/tmp/claude-1000/-run-media-bui-thanh-thuong-C0E02AC6E02AC310-vd-archive-void-diver-unity/f5d68fb3-474c-4fb1-a69a-c344d77e8e52/scratchpad
url=$1; d=$2; rm -rf $d; mkdir -p $d; touch $d/cmd.jsonl
PLAY=1 setsid nohup timeout 3600 node $S/drive.mjs "$url" $d $S/plan-lounge.json > $d.out 2>&1 < /dev/null &
for i in $(seq 1 120); do [ -f $d/done.txt ] && break; sleep 5; done
grep -q '=> "seeded' $d/console.log || { echo "FAIL seed"; exit 1; }
$S/pcmd.sh $d '[{"click":[0.6,0.3]},{"wait":2},{"hover":[0.12,0.65]},{"click":[0.12,0.65]},{"wait":8},{"hover":[0.425,0.66]},{"click":[0.425,0.66]},{"waitLog":"state Playing","timeout":120},{"wait":6},{"click":[0.5,0.85]},{"wait":1},{"key":"w","hold":1.5},{"wait":0.5},{"key":"d","hold":0.15},{"wait":0.5},{"key":"a","hold":0.24},{"wait":0.5},{"key":"w","hold":0.1},{"wait":0.5},{"key":"s","hold":0.4},{"wait":0.3},{"key":"a","hold":1.0},{"wait":0.5},{"key":"w","hold":0.3},{"wait":0.5},{"key":"d","hold":0.5},{"wait":0.5},{"key":"w","hold":0.1},{"wait":0.5},{"key":"w","hold":0.25},{"wait":0.5},{"key":"d","hold":0.85},{"wait":0.5},{"key":"w","hold":1.6},{"wait":0.5},{"key":"w","hold":1.4},{"wait":0.5},{"key":"s","hold":0.4},{"wait":0.5},{"key":"d","hold":0.9},{"wait":0.5},{"key":"w","hold":0.3},{"wait":0.5},{"key":"d","hold":0.1},{"wait":0.5},{"key":"d","hold":0.35},{"wait":0.6},{"hover":[0.6,0.45]},{"wait":1},{"shot":"at-narcis"},{"key":"f"},{"wait":2.5},{"shot":"narcis-menu"},{"hover":[0.63,0.424]},{"click":[0.63,0.424]},{"wait":3},{"shot":"select-0"},{"eval":"console.log(\"GATE pick Raven\"),1"},{"hover":[0.552,0.066]},{"click":[0.552,0.066]},{"wait":3},{"shot":"select-raven"},{"key":"Escape"},{"wait":4},{"shot":"lounge-raven"},{"key":"s","hold":0.8},{"wait":1},{"shot":"lounge-raven-moved"}]' 700
$S/pcmd.sh $d '[{"eval":"console.log(\"GATE reload\"),1"},{"eval":"location.reload(),1"},{"wait":90},{"shot":"title-again"},{"click":[0.6,0.3]},{"wait":2},{"hover":[0.12,0.65]},{"click":[0.12,0.65]},{"wait":8},{"shot":"continue"},{"hover":[0.425,0.66]},{"click":[0.425,0.66]},{"wait":50},{"shot":"after-continue"},{"key":"d","hold":0.5},{"wait":1},{"shot":"after-continue-moved"}]' 400
echo '[{"quit":true}]' >> $d/cmd.jsonl; sleep 3
echo "--- assertions"
p=$(grep -n '\[log\] GATE pick Raven' $d/console.log | head -1 | cut -d: -f1); r=$(grep -n '\[log\] GATE reload' $d/console.log | head -1 | cut -d: -f1)
[ -n "$p" ] && [ -n "$r" ] || { echo "FAIL markers missing (pick=$p reload=$r)"; exit 1; }
sed -n "$p,${r}p" $d/console.log > $d/pick.log; tail -n +$r $d/console.log > $d/continue.log
ok=1
grep -q "VD:Popup\] end open CharacterSettingPopupPresenter" $d/console.log && echo "PASS character select opened (path reached Narcis)" || { echo "FAIL path did not reach Narcis' character select: nothing was tested"; ok=0; }
grep -q "ArgumentNullException" $d/pick.log && { echo "FAIL pick Raven: ArgumentNullException"; ok=0; } || echo "PASS pick Raven: no exception"
grep -q "not found in PlayerCharacters" $d/pick.log && { echo "FAIL pick Raven: character missing after respawn"; ok=0; } || echo "PASS pick Raven: character present"
grep -q "ArgumentNullException" $d/continue.log && { echo "FAIL Continue: ArgumentNullException"; ok=0; } || echo "PASS Continue: no exception"
grep -q "VD:Stage\] state Playing" $d/continue.log && echo "PASS Continue: stage Playing" || { echo "FAIL Continue: never reached stage Playing"; ok=0; }
grep "VD:Stage\] state" $d/continue.log | tail -3 | sed 's/^/  /'
[ $ok = 1 ] && echo "GATE PASS" || echo "GATE FAIL"
