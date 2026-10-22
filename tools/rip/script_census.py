"""Count how often serialized assets reference each script, grouped by assembly.

Usage: python script_census.py <ExportedProject> [out.json]
Prints assemblies by reference count; writes per-class counts to out.json.
"""
import json, os, re, sys
from collections import Counter

ROOT = sys.argv[1]
ASSETS = os.path.join(ROOT, 'Assets')
SCRIPTS = os.path.join(ASSETS, 'Scripts')
YAML_EXT = ('.prefab', '.unity', '.asset', '.mat', '.controller', '.overrideController', '.playable', '.anim', '.mixer', '.spriteatlas', '.lighting', '.signal', '.preset', '.inputactions')

GUID_RE = re.compile(rb'^guid: ([0-9a-f]{32})', re.M)
REF_RE = re.compile(rb'm_Script: \{fileID: 11500000, guid: ([0-9a-f]{32})')
NS_RE = re.compile(r'^\s*namespace\s+([\w.]+)', re.M)


def script_index():
    idx = {}
    for asm in os.listdir(SCRIPTS):
        base = os.path.join(SCRIPTS, asm)
        for dp, _, fs in os.walk(base):
            for f in fs:
                if not f.endswith('.cs.meta'):
                    continue
                m = GUID_RE.search(open(os.path.join(dp, f), 'rb').read())
                src = os.path.join(dp, f[:-5])
                text = open(src, encoding='utf-8', errors='replace').read() if os.path.exists(src) else ''
                ns = NS_RE.search(text)
                idx[m.group(1).decode()] = {'asm': asm, 'ns': ns.group(1) if ns else '', 'cls': f[:-8],
                                            'path': os.path.relpath(src, ROOT)}
    return idx


def main():
    idx = script_index()
    per_class = Counter()
    unknown = Counter()
    for dp, _, fs in os.walk(ASSETS):
        for f in fs:
            if not f.endswith(YAML_EXT):
                continue
            for g in REF_RE.findall(open(os.path.join(dp, f), 'rb').read()):
                g = g.decode()
                (per_class if g in idx else unknown)[g] += 1
    per_asm = Counter()
    for g, n in per_class.items():
        per_asm[idx[g]['asm']] += n
    for asm, n in per_asm.most_common():
        classes = sum(1 for g in per_class if idx[g]['asm'] == asm)
        print(f'{n:8d} refs {classes:4d} classes  {asm}')
    print(f'unresolved script guids: {len(unknown)} ({sum(unknown.values())} refs)')
    if len(sys.argv) > 2:
        out = {g: dict(idx[g], refs=n) for g, n in per_class.most_common()}
        json.dump({'classes': out, 'unresolved': dict(unknown)}, open(sys.argv[2], 'w'), indent=1)


main()
