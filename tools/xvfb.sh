#!/bin/sh
# Print the display of a private Xvfb, starting it if needed. Used by unity-run.sh when no desktop session is
# logged in. Unpacks Ubuntu's xvfb package into ~/vd-rip/xvfb (apt-get download, no root).
set -eu
D=${VD_XVFB_DIR:-$HOME/vd-rip/xvfb}; N=${VD_XVFB_DISPLAY:-99}
if [ ! -x "$D/root/usr/bin/Xvfb" ]; then
  mkdir -p "$D/debs" && (cd "$D/debs" && apt-get download xvfb >/dev/null)
  dpkg-deb -x "$D"/debs/xvfb_*.deb "$D/root"
fi
if [ ! -S /tmp/.X11-unix/X$N ] || ! pgrep -u "$(id -u)" -f "Xvfb :$N " >/dev/null; then
  setsid "$D/root/usr/bin/Xvfb" :$N -screen 0 1920x1080x24 -nolisten tcp > "$D/xvfb.log" 2>&1 < /dev/null &
  i=0; while [ ! -S /tmp/.X11-unix/X$N ] && [ $i -lt 50 ]; do sleep 0.2; i=$((i+1)); done
fi
echo ":$N"
