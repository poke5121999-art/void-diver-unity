#!/bin/sh
# Copy the staged project (build_project.py output) into the repo.
# Generated content is mirrored; paths the repo owns after seeding are only created, never overwritten:
#   game code stubs (we write the method bodies there, keeping each script's GUID), ProjectSettings,
#   Packages/manifest.json, and our own Assets/_VD.
# Assets/AddressableAssetsData is written by Unity (Batch step addressables), so the mirror leaves it alone.
set -eu
STAGE=${1:-"$HOME/vd-rip/stage"}
REPO=$(cd "$(dirname "$0")/../.." && pwd)
OWNED="De.Base De.InGame De.Scenes De.Lua De.Const De.Input De.Cheat Global NemoLib NemoMapTools Assembly-CSharp Base.Art LCArt LCArt.URP SomeNet.Unity Serilog.Sinks.Unity3D"

EX=""
for a in $OWNED; do EX="$EX --exclude=/Scripts/$a/ --exclude=/Scripts/$a.meta"; done
# shellcheck disable=SC2086
rsync -rlc --delete --exclude=/_VD/ --exclude=/_VD.meta --exclude=/AddressableAssetsData/ --exclude=/AddressableAssetsData.meta --exclude=/Scripts.meta $EX "$STAGE/Assets/" "$REPO/Assets/"
for a in $OWNED; do
  [ -d "$STAGE/Assets/Scripts/$a" ] && rsync -rl --ignore-existing "$STAGE/Assets/Scripts/$a" "$REPO/Assets/Scripts/"
  [ -f "$STAGE/Assets/Scripts/$a.meta" ] && rsync -rl --ignore-existing "$STAGE/Assets/Scripts/$a.meta" "$REPO/Assets/Scripts/"
done
rsync -rl --ignore-existing "$STAGE/ProjectSettings/" "$REPO/ProjectSettings/"
mkdir -p "$REPO/Packages"
rsync -rl --ignore-existing "$STAGE/Packages/manifest.json" "$REPO/Packages/"
for p in "$STAGE"/Packages/*/; do rsync -rlc --delete "$p" "$REPO/Packages/$(basename "$p")/"; done
echo "synced $STAGE -> $REPO"
