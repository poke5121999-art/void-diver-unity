#!/bin/sh
# gate-raven.sh <url> <run-dir>: from the post-tutorial save, steer to Narcis (closed loop, tools/seek-narcis.sh),
# pick the 4th diver (Raven), reload, Continue. A run that never opens the character select is a FAIL ("nothing was
# tested"), never a pass. Asserts: no ArgumentNullException, the character exists after the pick, Continue reaches
# stage Playing.
S=/tmp/claude-1000/-run-media-bui-thanh-thuong-C0E02AC6E02AC310-vd-archive-void-diver-unity/f5d68fb3-474c-4fb1-a69a-c344d77e8e52/scratchpad
url=$1; d=$2; rm -rf $d; mkdir -p $d; touch $d/cmd.jsonl
quit() { echo '[{"quit":true}]' >> $d/cmd.jsonl; sleep 3; }
PLAY=1 setsid nohup timeout 1800 node $S/drive.mjs "$url" $d $S/plan-lounge.json > $d.out 2>&1 < /dev/null &
for i in $(seq 1 120); do [ -f $d/done.txt ] && break; sleep 5; done
grep -q '=> "seeded' $d/console.log || { echo "FAIL seed"; quit; exit 1; }
$S/pcmd.sh $d '[{"click":[0.6,0.3]},{"wait":2},{"hover":[0.12,0.65]},{"click":[0.12,0.65]},{"wait":8},{"hover":[0.425,0.66]},{"click":[0.425,0.66]},{"waitLog":"state Playing","timeout":120},{"wait":6},{"shot":"lounge-start"},{"key":"w","hold":1.5},{"wait":0.5},{"key":"d","hold":0.15},{"wait":0.5},{"key":"a","hold":0.24},{"wait":0.5},{"key":"w","hold":0.1},{"wait":0.5}]' 300
opened=0
for attempt in 1 2 3; do
  $S/tools/seek-narcis.sh $d > $d/seek$attempt.log 2>&1; cat $d/seek$attempt.log | sed "s/^/  seek$attempt: /" | tail -4
  r=$(python3 -I $S/tools/findlabel.py $S/tools/tpl-narcis.png $d/seek.png 100); [ "$r" = none ] && continue
  set -- $r
  hx=$(python3 -c "print(round(($1+80)/800,3))"); hy=$(python3 -c "print(round(($2+53)/544,3))")
  cx=$(python3 -c "print(round(($1+112)/800,3))"); cy=$(python3 -c "print(round(($2+38)/544,3))")
  n=$(wc -l < $d/console.log)
  $S/pcmd.sh $d "[{\"hover\":[$hx,$hy]},{\"wait\":1},{\"key\":\"f\"},{\"wait\":2.5},{\"shot\":\"narcis-menu\"},{\"hover\":[$cx,$cy]},{\"click\":[$cx,$cy]},{\"wait\":3},{\"shot\":\"select-0\"}]" 60
  if tail -n +$n $d/console.log | grep -q "VD:Popup\] end open CharacterSettingPopupPresenter"; then opened=1; echo "  character select opened on attempt $attempt"; break; fi
  echo "  attempt $attempt: no character select; closing whatever opened"
  $S/pcmd.sh $d '[{"key":"Escape"},{"wait":1.5}]' 30
done
if [ $opened = 0 ]; then echo "--- assertions"; echo "FAIL path did not reach Narcis' character select: nothing was tested"; echo "GATE FAIL"; quit; exit 1; fi
$S/pcmd.sh $d '[{"eval":"console.log(\"GATE pick Raven\"),1"},{"hover":[0.552,0.066]},{"click":[0.552,0.066]},{"wait":3},{"shot":"select-raven"},{"key":"Escape"},{"wait":4},{"shot":"lounge-raven"},{"key":"s","hold":0.8},{"wait":1},{"shot":"lounge-raven-moved"}]' 120
$S/pcmd.sh $d '[{"eval":"console.log(\"GATE reload\"),1"},{"eval":"location.reload(),1"},{"wait":90},{"shot":"title-again"},{"click":[0.6,0.3]},{"wait":2},{"hover":[0.12,0.65]},{"click":[0.12,0.65]},{"wait":8},{"shot":"continue"},{"hover":[0.425,0.66]},{"click":[0.425,0.66]},{"wait":50},{"shot":"after-continue"},{"key":"d","hold":0.5},{"wait":1},{"shot":"after-continue-moved"}]' 400
quit
echo "--- assertions"
p=$(grep -n '\[log\] GATE pick Raven' $d/console.log | head -1 | cut -d: -f1); r=$(grep -n '\[log\] GATE reload' $d/console.log | head -1 | cut -d: -f1)
[ -n "$p" ] && [ -n "$r" ] || { echo "FAIL markers missing (pick=$p reload=$r)"; echo "GATE FAIL"; exit 1; }
sed -n "$p,${r}p" $d/console.log > $d/pick.log; tail -n +$r $d/console.log > $d/continue.log
ok=1; echo "PASS character select opened (path reached Narcis)"
grep -q "Raven\|100005" $d/pick.log >/dev/null; 
grep -q "ArgumentNullException" $d/pick.log && { echo "FAIL pick Raven: ArgumentNullException"; ok=0; } || echo "PASS pick Raven: no exception"
grep -q "not found in PlayerCharacters" $d/pick.log && { echo "FAIL pick Raven: character missing after respawn"; ok=0; } || echo "PASS pick Raven: character present"
grep -q "ArgumentNullException" $d/continue.log && { echo "FAIL Continue: ArgumentNullException"; ok=0; } || echo "PASS Continue: no exception"
grep -q "VD:Stage\] state Playing" $d/continue.log && echo "PASS Continue: stage Playing" || { echo "FAIL Continue: never reached stage Playing"; ok=0; }
[ $ok = 1 ] && echo "GATE PASS" || echo "GATE FAIL"
