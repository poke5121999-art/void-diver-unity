#!/bin/sh
# Collect real package sources ("donors") whose script and shader GUIDs replace AssetRipper's stubs.
# Unity packages come from the tarballs bundled with the editor; Spine 4.2 from its git repo.
set -eu
HERE=$(cd "$(dirname "$0")" && pwd)
EDITOR=${UNITY_EDITOR_DIR:-"$HOME/Unity/Hub/Editor/6000.3.18f1/Editor"}
DONOR=${1:-"$HOME/vd-rip/donor"}
SPINE_REV=${SPINE_REV:-e7dc1435fa4a0083ab431f1b28e083c14a1f5c68}
PM="$EDITOR/Data/Resources/PackageManager"
mkdir -p "$DONOR"
cd "$DONOR"

for p in com.unity.timeline-1.8.12 com.unity.inputsystem-1.19.0 com.unity.cinemachine-2.10.7 \
  com.unity.ai.navigation-2.0.13 com.unity.2d.animation-13.0.5 com.unity.2d.common-12.0.3 \
  com.unity.addressables-2.9.1 com.unity.scriptablebuildpipeline-2.6.1 com.unity.burst-1.8.29 \
  com.unity.collections-2.6.6 com.unity.mathematics-1.3.3 com.unity.netcode.gameobjects-2.12.0 \
  com.unity.transport-2.7.3 com.unity.nuget.newtonsoft-json-3.2.2 com.unity.2d.spriteshape-13.0.0 \
  com.unity.profiling.core-1.0.3; do
  n=${p%-*}
  rm -rf "$n"; mkdir -p "$n"
  tar xzf "$PM/Editor/$p.tgz" -C "$n" --strip-components=1
done
for n in com.unity.ugui com.unity.render-pipelines.universal com.unity.render-pipelines.core com.unity.shadergraph com.unity.textmeshpro; do
  ln -sfn "$PM/BuiltInPackages/$n" "$n"
done

if [ ! -d spine-runtimes/.git ]; then
  timeout 600 git clone -q --filter=blob:none --sparse --branch 4.2 https://github.com/EsotericSoftware/spine-runtimes.git spine-runtimes
fi
git -C spine-runtimes sparse-checkout set spine-csharp spine-unity/Assets/Spine spine-unity/Modules
git -C spine-runtimes fetch -q --depth 1 origin "$SPINE_REV" 2>/dev/null || true
git -C spine-runtimes checkout -q "$SPINE_REV"

# NuGet DLLs for the stubs listed in packages.py NUGET.
mkdir -p nuget
python3 -I - "$HERE" <<'PY'
import io, os, sys, urllib.request, zipfile
sys.path.insert(0, sys.argv[1])
from packages import NUGET
for spec in set(NUGET.values()):
    pid, ver = spec[:2]
    name = spec[2] if len(spec) > 2 else pid
    dst = os.path.join('nuget', f'{name}.dll')
    if os.path.exists(dst):
        continue
    url = f'https://www.nuget.org/api/v2/package/{pid}/{ver}'
    z = zipfile.ZipFile(io.BytesIO(urllib.request.urlopen(url, timeout=120).read()))
    for tfm in ('netstandard2.1', 'netstandard2.0', 'netstandard1.6'):
        entry = f'lib/{tfm}/{name}.dll'
        if entry in z.namelist():
            open(dst, 'wb').write(z.read(entry))
            print('nuget', pid, ver, tfm)
            break
    else:
        sys.exit(f'no netstandard dll in {pid} {ver}')
PY
echo "donors ready in $DONOR"

# DOTween (free) ships closed-source as a DLL plus module sources.
if [ ! -f dotween/DOTween/DOTween.dll ]; then
  mkdir -p dotween
  timeout 120 curl -sL https://dotween.demigiant.com/downloads/DOTween_1_3_045.zip -o dotween/DOTween.zip
  python3 -I -c "import zipfile; zipfile.ZipFile('dotween/DOTween.zip').extractall('dotween')"
fi

for r in "Cysharp/UniTask src/UniTask/Assets/Plugins/UniTask" "hadashiA/VContainer VContainer/Assets/VContainer" \
         "neuecc/UniRx Assets/Plugins/UniRx/Scripts" "mob-sakai/SoftMaskForUGUI Packages/src"; do
  set -- $r; d=$(basename "$1")
  [ -d "$d/.git" ] || timeout 300 git clone -q --depth 1 --filter=blob:none --sparse "https://github.com/$1.git" "$d"
  git -C "$d" sparse-checkout set "$2"
done

# Steamworks.NET (MIT): the UPM package folder at the tag whose Version.cs the game shipped (2024.8.0, SDK 1.60).
STEAMWORKS_TAG=${STEAMWORKS_TAG:-2024.8.0}
if [ ! -d Steamworks.NET/.git ]; then
  timeout 300 git clone -q --filter=blob:none --no-checkout https://github.com/rlabrecque/Steamworks.NET.git Steamworks.NET
fi
git -C Steamworks.NET sparse-checkout set com.rlabrecque.steamworks.net
git -C Steamworks.NET checkout -q "$STEAMWORKS_TAG"

# Odin Serializer (Apache-2.0). The repo has no tag after 1.1.1.0 (2018); the game predates the 2026-10-14 batch
# (no DictionaryComparerUtility, FloatingPointFormatting, SubstituteGenericParameters...), so pin the last commit
# before it. Its namespace is OdinSerializer; the game's is Sirenix.Serialization, so a package is generated:
# namespace renamed, files that live in the Sirenix.Serialization.Config stub (loggers, DataFormat, global config)
# and editor-only files left out, and a .meta with a stable GUID per file so the Serialized* base classes remap.
ODIN_REV=${ODIN_REV:-ba19025b3dc38de8cebb10f94a583d1ff303ad59}
if [ ! -d odin-serializer/.git ]; then
  timeout 300 git clone -q https://github.com/TeamSirenix/odin-serializer.git odin-serializer
fi
git -C odin-serializer checkout -q "$ODIN_REV"
python3 -I - "$DONOR/odin-serializer" "$DONOR/odin-sirenix" <<'PY'
import hashlib, os, re, shutil, sys
src, dst = sys.argv[1], sys.argv[2]
shutil.rmtree(dst, ignore_errors=True)
SKIP = {  # live in the Sirenix.Serialization.Config / Utilities stubs, or editor-only
    'Config/GlobalSerializationConfig.cs', 'Config/AssemblyBuildInfo.cs', 'Properties/AssemblyInfo.cs',
    'Core/Misc/DataFormat.cs', 'Core/Misc/ILogger.cs', 'Core/Misc/DefaultLoggers.cs', 'Core/Misc/CustomLogger.cs',
    'Core/Misc/LoggingPolicy.cs', 'Core/Misc/ErrorHandlingPolicy.cs',
    'Unity Integration/AOTSupportScanner.cs', 'Unity Integration/AOTSupportUtilities.cs',
    'Utilities/Misc/AssemblyImportSettingsUtilities.cs',
}
INSPECTOR = ('SerializedBehaviour', 'SerializedComponent', 'SerializedMonoBehaviour', 'SerializedScriptableObject',
             'SerializedStateMachineBehaviour', 'SerializedUnityObject')
def meta(rel, body):
    g = hashlib.md5(('odin:' + rel).encode()).hexdigest()
    return f'fileFormatVersion: 2\nguid: {g}\n{body}'
MONO = 'MonoImporter:\n  externalObjects: {}\n  serializedVersion: 2\n  defaultReferences: []\n  executionOrder: 0\n  icon: {instanceID: 0}\n  userData: \n  assetBundleName: \n  assetBundleVariant: \n'
def put(rel, text, body):
    q = os.path.join(dst, rel)
    os.makedirs(os.path.dirname(q), exist_ok=True)
    open(q, 'w', encoding='utf-8', newline='').write(text)
    open(q + '.meta', 'w').write(meta(rel, body))
root = os.path.join(src, 'OdinSerializer')
n = 0
for dp, _, fs in os.walk(root):
    for f in sorted(fs):
        if not f.endswith('.cs'):
            continue
        p = os.path.join(dp, f)
        rel = os.path.relpath(p, root).replace(os.sep, '/')
        if rel in SKIP:
            continue
        t = open(p, encoding='utf-8-sig', newline='').read()
        t = t.replace('"OdinSerializer.AOTGenerated"', '"Sirenix.Serialization.AOTGenerated"')
        t = t.replace('"OdinSerializer.RuntimeEmitted"', '"Sirenix.Serialization.RuntimeEmitted"')
        t = t.replace('.Name == "OdinSerializer"', '.Name == "Sirenix.Serialization"')
        if os.path.splitext(f)[0] in INSPECTOR and 'SerializedUnityObjects' in rel:
            t = re.sub(r'^namespace OdinSerializer\b', 'namespace Sirenix.OdinInspector', t, flags=re.M)
            t = t.replace('namespace Sirenix.OdinInspector\r\n{', 'namespace Sirenix.OdinInspector\r\n{\r\n    using Sirenix.Serialization;', 1) if '\r\n' in t \
                else t.replace('namespace Sirenix.OdinInspector\n{', 'namespace Sirenix.OdinInspector\n{\n    using Sirenix.Serialization;', 1)
        t = re.sub(r'\bOdinSerializer\b(?!Policies)', 'Sirenix.Serialization', t)
        t = t.replace('namespace Sirenix.Serialization.Sirenix', 'namespace Sirenix')
        put('Runtime/' + rel, t, MONO)
        n += 1
put('Runtime/Sirenix.Serialization.asmdef',
    '{\n  "name": "Sirenix.Serialization",\n  "references": [\n    "Sirenix.Serialization.Config"\n  ],\n  "allowUnsafeCode": true\n}\n',
    'AssemblyDefinitionImporter:\n  externalObjects: {}\n  userData: \n  assetBundleName: \n  assetBundleVariant: \n')
put('Runtime/AssemblyInfo.cs',
    'using System;\nusing System.Reflection;\nusing Sirenix.Serialization;\n\n[assembly: CLSCompliant(false)]\n[assembly: AssemblyTitle("OdinSerializer")]\n[assembly: AssemblyCompany("Sirenix ApS")]\n[assembly: AssemblyProduct("OdinSerializer")]\n', MONO)
shutil.copy(os.path.join(src, 'LICENSE'), os.path.join(dst, 'LICENSE'))
open(os.path.join(dst, 'package.json'), 'w').write('{\n  "name": "com.sirenix.odin-serializer",\n  "displayName": "Odin Serializer",\n  "version": "1.1.1",\n  "unity": "2019.4",\n  "license": "Apache-2.0",\n  "description": "Open-source Odin Serializer, namespace Sirenix.Serialization."\n}\n')
print('odin-sirenix ->', dst, n, 'files')
PY

# Sirenix.Utilities is closed (Odin Inspector), but odin-serializer carries public copies of 21 source files of it
# (namespace OdinSerializer.Utilities). They overlay the stub files of the same name (OVERLAY in packages.py).
# LinqExtensions in the open copy lacks 31 members the closed one has; they are reconstructed below because
# the game calls IsNullOrEmpty (and ForEach, which the stub leaves returning null).
python3 -I - "$DONOR/odin-serializer/OdinSerializer/Utilities" "$DONOR/odin-utilities" <<'PY'
import os, re, shutil, sys
src, dst = sys.argv[1], sys.argv[2]
shutil.rmtree(dst, ignore_errors=True); os.makedirs(dst)
FILES = {'GarbageFreeIterators': 'Extensions', 'DoubleLookupDictionary': 'Misc', 'EmitUtilities': 'Misc',
         'FieldInfoExtensions': 'Extensions', 'Flags': 'Misc', 'ImmutableList': 'Misc', 'LinqExtensions': 'Extensions',
         'MemberAliasFieldInfo': 'Misc', 'MemberAliasMethodInfo': 'Misc', 'MemberAliasPropertyInfo': 'Misc',
         'MemberInfoExtensions': 'Extensions', 'MethodInfoExtensions': 'Extensions', 'Operator': 'Extensions',
         'PathUtilities': 'Extensions', 'PropertyInfoExtensions': 'Extensions', 'ReferenceEqualityComparer': 'Misc',
         'StringExtensions': 'Extensions', 'TypeExtensions': 'Extensions', 'UnityExtensions': 'Extensions',
         'UnityVersion': 'Misc', 'UnsafeUtilities': 'Misc'}
SHIM = """
        // ---- reconstructed: members of the closed Sirenix.Utilities.LinqExtensions missing from the open copy ----
        public static bool IsNullOrEmpty<T>(this IList<T> list) { return list == null || list.Count == 0; }
        public static void AddRange<T>(this HashSet<T> hashSet, IEnumerable<T> range) { foreach (var item in range) hashSet.Add(item); }
        public static void AddRange<T>(this IList<T> list, IEnumerable<T> collection) { foreach (var item in collection) list.Add(item); }
        public static void Populate<T>(this IList<T> list, T item) { for (int i = 0; i < list.Count; i++) list[i] = item; }
        public static void Sort<T>(this IList<T> list) { var a = new List<T>(list); a.Sort(); for (int i = 0; i < a.Count; i++) list[i] = a[i]; }
        public static void Sort<T>(this IList<T> list, Comparison<T> comparison) { var a = new List<T>(list); a.Sort(comparison); for (int i = 0; i < a.Count; i++) list[i] = a[i]; }
        public static ImmutableList<T> ToImmutableList<T>(this IEnumerable<T> source) { return new ImmutableList<T>(new List<T>(source)); }
        public static IEnumerable<T> Examine<T>(this IEnumerable<T> source, Action<T> action) { foreach (var item in source) { action(item); yield return item; } }
        public static IEnumerable<T> Convert<T>(this System.Collections.IEnumerable source, Func<object, T> converter) { foreach (var item in source) yield return converter(item); }
        public static IEnumerable<T> FilterCast<T>(this System.Collections.IEnumerable source) { foreach (var item in source) if (item is T) yield return (T)item; }
        public static IEnumerable<T> AppendWith<T>(this IEnumerable<T> source, Func<T> append) { foreach (var i in source) yield return i; yield return append(); }
        public static IEnumerable<T> AppendWith<T>(this IEnumerable<T> source, T append) { foreach (var i in source) yield return i; yield return append; }
        public static IEnumerable<T> AppendWith<T>(this IEnumerable<T> source, IEnumerable<T> append) { foreach (var i in source) yield return i; foreach (var i in append) yield return i; }
        public static IEnumerable<T> AppendIf<T>(this IEnumerable<T> source, bool condition, Func<T> append) { return condition ? source.AppendWith(append) : source; }
        public static IEnumerable<T> AppendIf<T>(this IEnumerable<T> source, bool condition, T append) { return condition ? source.AppendWith(append) : source; }
        public static IEnumerable<T> AppendIf<T>(this IEnumerable<T> source, bool condition, IEnumerable<T> append) { return condition ? source.AppendWith(append) : source; }
        public static IEnumerable<T> AppendIf<T>(this IEnumerable<T> source, Func<bool> condition, Func<T> append) { return condition() ? source.AppendWith(append) : source; }
        public static IEnumerable<T> AppendIf<T>(this IEnumerable<T> source, Func<bool> condition, T append) { return condition() ? source.AppendWith(append) : source; }
        public static IEnumerable<T> AppendIf<T>(this IEnumerable<T> source, Func<bool> condition, IEnumerable<T> append) { return condition() ? source.AppendWith(append) : source; }
        public static IEnumerable<T> PrependWith<T>(this IEnumerable<T> source, Func<T> prepend) { yield return prepend(); foreach (var i in source) yield return i; }
        public static IEnumerable<T> PrependWith<T>(this IEnumerable<T> source, T prepend) { yield return prepend; foreach (var i in source) yield return i; }
        public static IEnumerable<T> PrependWith<T>(this IEnumerable<T> source, IEnumerable<T> prepend) { foreach (var i in prepend) yield return i; foreach (var i in source) yield return i; }
        public static IEnumerable<T> PrependIf<T>(this IEnumerable<T> source, bool condition, Func<T> prepend) { return condition ? source.PrependWith(prepend) : source; }
        public static IEnumerable<T> PrependIf<T>(this IEnumerable<T> source, bool condition, T prepend) { return condition ? source.PrependWith(prepend) : source; }
        public static IEnumerable<T> PrependIf<T>(this IEnumerable<T> source, bool condition, IEnumerable<T> prepend) { return condition ? source.PrependWith(prepend) : source; }
        public static IEnumerable<T> PrependIf<T>(this IEnumerable<T> source, Func<bool> condition, Func<T> prepend) { return condition() ? source.PrependWith(prepend) : source; }
        public static IEnumerable<T> PrependIf<T>(this IEnumerable<T> source, Func<bool> condition, T prepend) { return condition() ? source.PrependWith(prepend) : source; }
        public static IEnumerable<T> PrependIf<T>(this IEnumerable<T> source, Func<bool> condition, IEnumerable<T> prepend) { return condition() ? source.PrependWith(prepend) : source; }
        public static IEnumerable<T> PrependIf<T>(this IEnumerable<T> source, Func<IEnumerable<T>, bool> condition, Func<T> prepend) { return condition(source) ? source.PrependWith(prepend) : source; }
        public static IEnumerable<T> PrependIf<T>(this IEnumerable<T> source, Func<IEnumerable<T>, bool> condition, T prepend) { return condition(source) ? source.PrependWith(prepend) : source; }
        public static IEnumerable<T> PrependIf<T>(this IEnumerable<T> source, Func<IEnumerable<T>, bool> condition, IEnumerable<T> prepend) { return condition(source) ? source.PrependWith(prepend) : source; }
"""
for name, sub in FILES.items():
    t = open(os.path.join(src, sub, name + '.cs'), encoding='utf-8-sig').read()
    t = re.sub(r'\bOdinSerializer\.Utilities\b', 'Sirenix.Utilities', t)
    t = re.sub(r'\bOdinSerializer\b', 'Sirenix.Serialization', t)
    if name == 'LinqExtensions':
        i = t.rindex('}', 0, t.rindex('}'))
        t = t[:i] + SHIM + '    ' + t[i:]
    open(os.path.join(dst, name + '.cs'), 'w', encoding='utf-8', newline='').write(t)
print('odin-utilities ->', dst, len(FILES), 'files')
PY

# Sirenix.Serialization.Config keeps its stub GlobalSerializationConfig (a ScriptableObject over the closed
# GlobalConfig<T>), but its six plain files (loggers, DataFormat, policies) are in the open copy too.
rm -rf odin-config; mkdir -p odin-config
for f in CustomLogger DataFormat DefaultLoggers ErrorHandlingPolicy ILogger LoggingPolicy; do
  sed 's/\bOdinSerializer\b/Sirenix.Serialization/g' "odin-serializer/OdinSerializer/Core/Misc/$f.cs" > "odin-config/$f.cs"
done

# TMP Essential Resources ship as a .unitypackage (guid/asset, guid/asset.meta, guid/pathname).
python3 -I - "$DONOR/com.unity.ugui/Package Resources/TMP Essential Resources.unitypackage" "$DONOR/tmp-essentials" <<'PY'
import os, sys, tarfile
src, dst = sys.argv[1], sys.argv[2]
with tarfile.open(src, 'r:gz') as t:
    members = {m.name: m for m in t.getmembers() if m.isfile()}
    for name in members:
        if not name.endswith('/pathname'):
            continue
        guid = name.split('/')[-2]
        rel = t.extractfile(members[name]).read().decode().splitlines()[0]
        out = os.path.join(dst, rel)
        asset = members.get(f'{guid}/asset') or members.get(f'./{guid}/asset')
        meta = members.get(f'{guid}/asset.meta') or members.get(f'./{guid}/asset.meta')
        if asset:
            os.makedirs(os.path.dirname(out), exist_ok=True)
            open(out, 'wb').write(t.extractfile(asset).read())
        if meta:
            os.makedirs(os.path.dirname(out), exist_ok=True)
            open(out + '.meta', 'wb').write(t.extractfile(meta).read())
print('tmp essentials ->', dst)
PY

# The shipped game is IL2CPP: Odin never takes its Reflection.Emit path there, and its precompiled DLLs never met
# the "#error ... .NET Standard 2.0" guard. Building from source under the game's .NET Standard API level needs both
# gone, or the editor compiles code (CAN_EMIT) the game never ran.
python3 -I - "$DONOR/odin-sirenix" "$DONOR/odin-utilities" <<'PY'
import os, re, sys
n = 0
for root in sys.argv[1:]:
    for dp, _, fs in os.walk(root):
        for f in fs:
            if not f.endswith('.cs'):
                continue
            p = os.path.join(dp, f)
            t = open(p, encoding='utf-8').read()
            u = re.sub(r'#if NET_STANDARD_2_0\s*\n#error[^\n]*\n#endif\s*\n', '', t)
            u = re.sub(r'^(\s*)#define CAN_EMIT\s*$', r'\1// CAN_EMIT left undefined: the shipped IL2CPP build never emits', u, flags=re.M)
            if u != t:
                open(p, 'w', encoding='utf-8', newline='').write(u)
                n += 1
print('odin il2cpp-path patch:', n, 'files')
PY
