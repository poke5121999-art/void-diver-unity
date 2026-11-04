#!/bin/sh
# gate-continue.sh <url> <run-dir>: seeds the post-tutorial save with Raven (4th diver) selected, presses Continue and
# asserts: no ArgumentNullException, stage reaches Playing, the player's character exists (no "not found in
# PlayerCharacters"), and the character moves on W.
S=/tmp/claude-1000/-run-media-bui-thanh-thuong-C0E02AC6E02AC310-vd-archive-void-diver-unity/f5d68fb3-474c-4fb1-a69a-c344d77e8e52/scratchpad
url=$1; d=$2; rm -rf $d; mkdir -p $d; touch $d/cmd.jsonl
PLAY=1 setsid nohup timeout 1800 node $S/drive.mjs "$url" $d $S/plan-raven.json > $d.out 2>&1 < /dev/null &
for i in $(seq 1 120); do [ -f $d/done.txt ] && break; sleep 5; done
grep -q '=> "seeded' $d/console.log || { echo "FAIL seed"; exit 1; }
$S/pcmd.sh $d '[{"shot":"title"},{"eval":"console.log(\"GATE continue\"),1"},{"click":[0.6,0.3]},{"wait":2},{"hover":[0.12,0.65]},{"click":[0.12,0.65]},{"wait":8},{"shot":"continue"},{"hover":[0.425,0.66]},{"click":[0.425,0.66]},{"wait":50},{"shot":"after-continue"},{"key":"s","hold":0.8},{"wait":1},{"shot":"after-continue-moved"},{"wait":15}]' 300
echo '[{"quit":true}]' >> $d/cmd.jsonl; sleep 3
c=$(grep -n '\[log\] GATE continue' $d/console.log | head -1 | cut -d: -f1); [ -n "$c" ] || { echo "FAIL marker"; exit 1; }
tail -n +$c $d/console.log > $d/continue.log
python3 -I -c "
from PIL import Image, ImageChops
a=Image.open('$d/after-continue.png').convert('L'); b=Image.open('$d/after-continue-moved.png').convert('L')
diff=sum(1 for v in ImageChops.difference(a,b).getdata() if v>25); print('frames differ by', diff, 'pixels')
open('$d/moved.txt','w').write(str(diff))" 2>/dev/null
echo "--- assertions"; ok=1
grep -q "PlayerContext_0 read" $d/continue.log && echo "PASS save with Raven loaded" || { echo "FAIL save not loaded"; ok=0; }
grep -q "ArgumentNullException" $d/continue.log && { echo "FAIL ArgumentNullException:"; grep -m1 -A3 "VD:Stack" $d/continue.log | sed 's/^/  /'; ok=0; } || echo "PASS no exception"
grep -q "VD:Stage\] state Playing" $d/continue.log && echo "PASS stage Playing" || { echo "FAIL stage never Playing (last: $(grep 'VD:Stage\] state' $d/continue.log | tail -1 | sed 's/.*state //'))"; ok=0; }
grep -q "not found in PlayerCharacters" $d/continue.log && { echo "FAIL no player character"; ok=0; } || echo "PASS player character present"
[ "$(cat $d/moved.txt 2>/dev/null || echo 0)" -gt 2000 ] && echo "PASS view changes when moving" || { echo "FAIL view did not change on S"; ok=0; }
[ $ok = 1 ] && echo "GATE PASS" || echo "GATE FAIL"
