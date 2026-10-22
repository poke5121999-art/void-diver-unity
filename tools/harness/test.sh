#!/bin/sh
# Build one game assembly from source plus test files into an exe and run it on the editor's .NET runtime.
# Usage: tools/harness/test.sh <Assembly[,Assembly...] under Assets/Scripts> <test.cs>...
# All listed assemblies compile from source into the one test exe. With no test files it only compiles
# (a library), which is the quick gate after editing an assembly.
# Dependencies resolve against a snapshot of Library/ScriptAssemblies (refresh with tools/harness/snapshot.sh),
# minus the assembly under test. Exit code is the test's exit code.
set -eu
REPO=$(cd "$(dirname "$0")/../.." && pwd)
E=${UNITY_EDITOR_DATA:-"$HOME/Unity/Hub/Editor/6000.3.18f1/Editor/Data"}
REF=${HARNESS_REF:-"$HOME/vd-rip/harness/ref"}
ASM=$1; shift
NTEST=$#
WORK=$(mktemp -d "$HOME/vd-rip/harness/run-$(echo "$ASM" | tr , _).XXXX")
trap 'rm -rf "$WORK"' EXIT
rm -rf "$WORK"; mkdir -p "$WORK/refs"
# Like Unity: an asmdef assembly sees precompiled DLLs plus only the assemblies its .asmdef references.
cp "$REF"/pre/*.dll "$WORK/refs/"
set -f
python3 -I - "$REPO/Assets/Scripts" "$ASM" <<'PY' | while IFS= read -r name; do
import glob, json, os, sys
root, asms = sys.argv[1], sys.argv[2].split(',')
refs = set()
for a in asms:
    f = glob.glob(os.path.join(root, a, '*.asmdef'))
    refs |= set(json.load(open(f[0])).get('references', [])) if f else {'*'}
print('\n'.join(sorted(r for r in refs if r not in asms)))
PY
  if [ "$name" = "*" ]; then cp "$REF"/asm/*.dll "$WORK/refs/"; else [ -f "$REF/asm/$name.dll" ] && cp "$REF/asm/$name.dll" "$WORK/refs/"; fi
done

: > "$WORK/sources.txt"
for a in $(echo "$ASM" | tr , ' '); do find "$REPO/Assets/Scripts/$a" -name '*.cs' -not -path '*/Properties/AssemblyInfo.cs' >> "$WORK/sources.txt"; done
set -- -unsafe "$@"
while IFS= read -r f; do set -- "$@" "$f"; done < "$WORK/sources.txt"
if [ "$NTEST" -eq 0 ]; then
  HARNESS_SCRIPT_ASSEMBLIES="$WORK/refs" sh "$REPO/tools/harness/csc.sh" "$WORK/build.dll" -target:library "$@" > "$WORK/build.log" 2>&1 && ok=1 || ok=0
  grep -E 'error CS' "$WORK/build.log" | sort -u
  echo "BUILD $ASM errors=$(grep -cE 'error CS' "$WORK/build.log") ok=$ok"
  [ "$ok" = 1 ]; exit $?
fi
HARNESS_SCRIPT_ASSEMBLIES="$WORK/refs" sh "$REPO/tools/harness/csc.sh" "$WORK/test.exe" "$@"
cp "$WORK"/refs/*.dll "$WORK/"
cp "$E"/Managed/UnityEngine/*.dll "$WORK/" 2>/dev/null || true
printf '{"runtimeOptions":{"tfm":"net6.0","framework":{"name":"Microsoft.NETCore.App","version":"6.0.0"}}}\n' > "$WORK/test.runtimeconfig.json"
cd "$REPO"
"$E/NetCoreRuntime/dotnet" "$WORK/test.exe"
