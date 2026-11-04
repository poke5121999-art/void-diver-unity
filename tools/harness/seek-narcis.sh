#!/bin/sh
# seek-narcis.sh <run-dir>: closed-loop steering of the player to Narcis in a PLAY=1 drive.mjs session.
# Each round: screenshot, find the "Change Character / Narcis" label, move the player (always at the screen centre)
# toward a spot left of Narcis with key holds proportional to the distance. Ends with Narcis' menu open, or exits 1.
S=/tmp/claude-1000/-run-media-bui-thanh-thuong-C0E02AC6E02AC310-vd-archive-void-diver-unity/f5d68fb3-474c-4fb1-a69a-c344d77e8e52/scratchpad
d=$1; miss=0
for round in $(seq 1 14); do
  $S/pcmd.sh $d '[{"wait":0.5},{"shot":"seek"}]' 60 || exit 1
  r=$(python3 -I $S/tools/findlabel.py $S/tools/tpl-narcis.png $d/seek.png 100)
  if [ "$r" = none ]; then
    miss=$((miss+1)); echo "round $round: Narcis not on screen"
    # Narcis is up and right of the spawn; explore that way.
    $S/pcmd.sh $d '[{"key":"w","hold":0.4},{"wait":0.2},{"key":"d","hold":0.4}]' 60; continue
  fi
  set -- $r; lx=$1; ly=$2
  # player stands left of Narcis' feet: label top-left + (-10, +60) in an 800x544 shot
  dx=$((lx - 10 - 400)); dy=$((ly + 60 - 272))
  echo "round $round: label at $lx,$ly score $3, offset $dx,$dy"
  adx=${dx#-}; ady=${dy#-}
  if [ $adx -le 22 ] && [ $ady -le 18 ]; then echo "in range"; exit 0; fi
  steps=''
  if [ $adx -gt 22 ]; then k=d; [ $dx -lt 0 ] && k=a; h=$(python3 -c "print(min(0.5,max(0.04,$adx/320)))"); steps="{\"key\":\"$k\",\"hold\":$h},{\"wait\":0.2}"; fi
  if [ $ady -gt 18 ]; then k=s; [ $dy -lt 0 ] && k=w; h=$(python3 -c "print(min(0.5,max(0.04,$ady/320)))"); steps="${steps:+$steps,}{\"key\":\"$k\",\"hold\":$h},{\"wait\":0.2}"; fi
  $S/pcmd.sh $d "[$steps]" 60
done
echo "not in range after 14 rounds"; exit 1
