# asm_build.py <asmdef> <out.dll> <refsdir>: compile one package asmdef with tools/harness/csc.sh to check our code against its API (docs/PACKAGES.md).
import json, os, re, subprocess, sys
asmdef, out, refs = sys.argv[1:4]
d = json.load(open(asmdef)); root = os.path.dirname(asmdef)
srcs = []
for dp, dns, fns in os.walk(root):
    if dp != root and any(f.endswith('.asmdef') or f.endswith('.asmref') for f in fns): dns[:] = []; continue
    srcs += [os.path.join(dp, f) for f in fns if f.endswith('.cs')]
def ver(s): return [int(x) for x in re.findall(r'\d+', s)[:3]]
defs = ['UNITY_2021_3_OR_NEWER', 'UNITY_2022_1_OR_NEWER', 'UNITY_2022_2_OR_NEWER', 'UNITY_2022_3_OR_NEWER', 'UNITY_2023_1_OR_NEWER',
        'UNITY_2023_2_OR_NEWER', 'UNITY_2023_3_OR_NEWER', 'UNITY_6000_0_OR_NEWER', 'UNITY_6000_1_OR_NEWER', 'UNITY_6000_2_OR_NEWER',
        'UNITY_6000_3_OR_NEWER', 'UNITY_STANDALONE', 'UNITY_STANDALONE_WIN', 'ENABLE_INPUT_SYSTEM', 'ENABLE_MONO', 'NET_STANDARD_2_1', 'CSHARP_7_3_OR_NEWER', 'UNITY_INCLUDE_TESTS', 'ENABLE_UNITY_COLLECTIONS_CHECKS', 'UNITY_EDITOR']
for v in d.get('versionDefines', []):
    n, e = v['name'], v.get('expression', '')
    if n.startswith('com.unity.modules.') or n in ('com.unity.ugui', 'com.unity.burst', 'com.unity.collections', 'com.unity.mathematics'):
        defs.append(v['define'])
    elif n == 'Unity':
        m = re.match(r'^\[?([\d.a-z]+)', e or '0')
        lo = ver(m.group(1)) if m else [0]
        if lo <= [6000, 3, 5] and ',' not in (e or '') or (e.startswith('[') and lo <= [6000, 3, 5] and ver(e.split(',')[1]) > [6000, 3, 5]): defs.append(v['define'])
defs += d.get('defineConstraints', [])
args = ['sh', os.path.expanduser('~/void-diver-unity/tools/harness/csc.sh'), out] + (['-unsafe'] if d.get('allowUnsafeCode') else []) + ['-d:' + x for x in defs if not x.startswith('!')] + srcs
r = subprocess.run(args, env=dict(os.environ, HARNESS_SCRIPT_ASSEMBLIES=refs), capture_output=True, text=True)
errs = [l for l in (r.stdout + r.stderr).split('\n') if 'error' in l]
print(os.path.basename(out), 'errors', len(errs)); print('\n'.join(errs[:6]))
