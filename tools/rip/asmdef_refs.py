"""Give every game asmdef a reference to every runtime library assembly the project compiles.

AssetRipper writes asmdef `references` that miss assemblies the original code really used (NemoLib calls
UniTask.Addressables and Serilog.Sinks.Unity3D, neither listed). Library assemblies never reference game
assemblies, so adding all of them cannot create a cycle; references between game assemblies stay as exported.
Usage: python asmdef_refs.py <Assets/Scripts> <dir of compiled assemblies (Library/ScriptAssemblies or a snapshot)>
Idempotent.
"""
import glob, json, os, re, sys

GAME = {'De.Base', 'De.InGame', 'De.Scenes', 'De.Lua', 'De.Const', 'De.Input', 'De.Cheat', 'Global', 'NemoLib',
        'NemoMapTools', 'Assembly-CSharp', 'Base.Art', 'LCArt', 'LCArt.URP', 'SomeNet.Unity'}
NOT_RUNTIME = re.compile(r'(?i)editor|test|example|demo|sample|codegen|updater|DocCodeSamples|PerformanceTesting|'
                         r'MCPForUnity|PPv2URPConverters|ScriptablePacker|ShaderLibrary|\.Shaders$|ShaderGraph')

scripts, compiled = sys.argv[1], sys.argv[2]
libs = sorted(os.path.splitext(f)[0] for f in os.listdir(compiled)
              if f.endswith('.dll') and os.path.splitext(f)[0] not in GAME and not NOT_RUNTIME.search(f))
changed = 0
for asm in sorted(GAME):
    for path in glob.glob(os.path.join(scripts, asm, '*.asmdef')):
        d = json.load(open(path))
        refs = d.get('references', [])
        new = refs + [l for l in libs if l not in refs]
        if new != refs:
            d['references'] = new
            json.dump(d, open(path, 'w'), indent=2)
            changed += 1
print(f'asmdef_refs: {changed} asmdefs extended with up to {len(libs)} library references')
