#!/bin/sh
# Snapshot the editor-compiled assemblies (packages, stubs, NuGet) that tools/harness/test.sh references.
# Run after a Unity import that compiled cleanly.
set -eu
REPO=$(cd "$(dirname "$0")/../.." && pwd)
REF=${HARNESS_REF:-"$HOME/vd-rip/harness/ref"}
rm -rf "$REF"; mkdir -p "$REF"
cp "$REPO"/Library/ScriptAssemblies/*.dll "$REF/"
find "$REPO/Library/PackageCache" "$REPO/Assets/Plugins" -name '*.dll' -not -path '*Editor*' -not -path '*Tests*' -exec cp {} "$REF/" \; 2>/dev/null || true
rm -f "$REF"/*Editor*.dll "$REF"/*Tests*.dll
echo "$(ls "$REF" | wc -l) assemblies in $REF"
# Keep managed assemblies only (Burst, debugger and native plugins are PE files without CLI metadata).
python3 -I - "$REF" <<'PY'
import os, struct, sys
d = sys.argv[1]
for f in os.listdir(d):
    p = os.path.join(d, f)
    b = open(p, 'rb').read(4096)
    try:
        pe = struct.unpack_from('<I', b, 0x3c)[0]
        magic = struct.unpack_from('<H', b, pe + 24)[0]
        cli = pe + 24 + (224 if magic == 0x20b else 208)
        managed = struct.unpack_from('<I', b, cli)[0] != 0
    except struct.error:
        managed = False
    if not managed:
        os.remove(p)
PY
echo "$(ls "$REF" | wc -l) managed assemblies kept"
