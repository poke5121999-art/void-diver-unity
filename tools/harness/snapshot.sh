#!/bin/sh
# Snapshot the editor-compiled assemblies (packages, stubs, NuGet) that tools/harness/test.sh references.
# Run after a Unity import that compiled cleanly.
set -eu
REPO=$(cd "$(dirname "$0")/../.." && pwd)
REF=${HARNESS_REF:-"$HOME/vd-rip/harness/ref"}
# asm/: assemblies Unity compiles from asmdefs (visible only through asmdef references).
# pre/: precompiled DLLs, which Unity auto-references for every assembly.
rm -rf "$REF"; mkdir -p "$REF/asm" "$REF/pre"
cp "$REPO"/Library/ScriptAssemblies/*.dll "$REF/asm/"
find "$REPO/Library/PackageCache" "$REPO/Assets/Plugins" -name '*.dll' -not -path '*Editor*' -not -path '*Tests*' -exec cp {} "$REF/pre/" \; 2>/dev/null || true
rm -f "$REF"/asm/*Editor*.dll "$REF"/asm/*Tests*.dll "$REF"/pre/*Editor*.dll "$REF"/pre/*Tests*.dll
# Keep managed assemblies only (Burst, debugger and native plugins are PE files without CLI metadata).
python3 -I - "$REF/asm" "$REF/pre" <<'PY'
import os, struct, sys
for p in [os.path.join(d, f) for d in sys.argv[1:] for f in os.listdir(d)]:
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
echo "$(ls "$REF/asm" | wc -l) asmdef assemblies, $(ls "$REF/pre" | wc -l) precompiled kept"
