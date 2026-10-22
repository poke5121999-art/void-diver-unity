"""Turn the raw AssetRipper export into this repo's Unity project.

Usage: python build_project.py <ExportedProject> <donor_dir> <stage_dir> [--report out.json]

1. Index donor package sources (.cs and .shader with their .meta GUIDs).
2. Map each stub script / stub shader / exported package asset GUID to the donor GUID of the same type,
   shader name or package path.
3. Copy the export to stage_dir, dropping swapped and dropped stub assemblies and remapped files,
   rewriting every `guid:` reference in YAML assets through the map.
4. Write Packages/manifest.json and embed the non-registry packages.
Rerunnable: stage_dir is rebuilt from scratch. Syncing stage_dir into the repo is tools/rip/sync.sh.
"""
import json, os, re, shutil, sys
from collections import Counter

sys.path.insert(0, os.path.dirname(os.path.abspath(__file__)))
from packages import UNITY_REGISTRY, EMBEDDED, SWAP, DROP, SHADER_DONORS, NUGET, PATCHES

GUID_RE = re.compile(rb'^guid: ([0-9a-f]{32})', re.M)
NS_RE = re.compile(r'^\s*namespace\s+([\w.]+)', re.M)
SHADER_RE = re.compile(r'^\s*Shader\s+"([^"]+)"', re.M)
REF_RE = re.compile(rb'guid: ([0-9a-f]{32})')
SKIP_DIRS = ('/Editor/', '/Tests/', '/Samples~/', '/Documentation~/', '/Tests~/', '/Editor~/')


def meta_guid(path):
    try:
        m = GUID_RE.search(open(path + '.meta', 'rb').read())
    except FileNotFoundError:
        return None
    return m.group(1).decode() if m else None


def type_key(cs_path):
    text = open(cs_path, encoding='utf-8', errors='replace').read()
    ns = NS_RE.search(text)
    stem = os.path.basename(cs_path)[:-3]
    return f'{ns.group(1)}.{stem}' if ns else stem


def walk(root, ext):
    for dp, dns, fs in os.walk(root, followlinks=True):
        rel = dp.replace('\\', '/') + '/'
        if any(s in rel for s in SKIP_DIRS):
            continue
        for f in fs:
            if f.endswith(ext):
                yield os.path.join(dp, f)


def index_donor_scripts(donor, dirs):
    idx = {}
    for d in dirs:
        for p in walk(os.path.join(donor, d), '.cs'):
            g = meta_guid(p)
            if g:
                idx.setdefault(type_key(p), g)
    return idx


def index_shaders(roots):
    idx = {}
    for root in roots:
        for p in walk(root, '.shader'):
            m = SHADER_RE.search(open(p, encoding='utf-8', errors='replace').read())
            g = meta_guid(p)
            if m and g:
                idx.setdefault(m.group(1), (g, p))
    return idx


def main():
    src, donor, stage = sys.argv[1], sys.argv[2], sys.argv[3]
    report_path = sys.argv[sys.argv.index('--report') + 1] if '--report' in sys.argv else None
    assets = os.path.join(src, 'Assets')
    scripts = os.path.join(assets, 'Scripts')

    remap = {}
    removed = set()
    report = {'scripts': {}, 'unmapped_scripts': {}, 'shaders': {}, 'package_assets': {}}

    for asm, dirs in SWAP.items():
        stub_dir = os.path.join(scripts, asm)
        if not os.path.isdir(stub_dir):
            continue
        donor_idx = index_donor_scripts(donor, dirs)
        mapped = missing = 0
        for p in walk(stub_dir, '.cs'):
            g = meta_guid(p)
            new = donor_idx.get(type_key(p))
            if g and new:
                remap[g] = new
                mapped += 1
            else:
                report['unmapped_scripts'][g] = os.path.relpath(p, src)
                missing += 1
        report['scripts'][asm] = {'mapped': mapped, 'unmapped': missing}
        removed.add(os.path.realpath(stub_dir))
    for asm in DROP + list(NUGET):
        d = os.path.join(scripts, asm)
        if os.path.isdir(d):
            removed.add(os.path.realpath(d))

    donor_shaders = index_shaders([os.path.join(donor, d) for d in SHADER_DONORS])
    for p in walk(assets, '.shader'):
        m = SHADER_RE.search(open(p, encoding='utf-8', errors='replace').read())
        hit = m and donor_shaders.get(m.group(1))
        g = meta_guid(p)
        if hit and g:
            remap[g] = hit[0]
            removed.add(os.path.realpath(p))
            report['shaders'][m.group(1)] = os.path.relpath(hit[1], donor)

    pkg_root = os.path.join(assets, 'Packages')
    if os.path.isdir(pkg_root):
        for dp, _, fs in os.walk(pkg_root):
            for f in fs:
                if f.endswith('.meta'):
                    continue
                p = os.path.join(dp, f)
                rel = os.path.relpath(p, pkg_root)
                pkg, sub = rel.split(os.sep, 1)
                cand = [os.path.join(donor, pkg, sub)] + [os.path.join(donor, d, sub) for k, d in EMBEDDED.items() if k == pkg]
                hit = next((c for c in cand if os.path.exists(c) and meta_guid(c)), None)
                if hit:
                    remap[meta_guid(p)] = meta_guid(hit)
                    removed.add(os.path.realpath(p))
                    report['package_assets'][rel] = os.path.relpath(hit, donor)

    if os.path.exists(stage):
        shutil.rmtree(stage)
    rewritten = Counter()

    def sub(m):
        g = m.group(1).decode()
        n = remap.get(g)
        if n:
            rewritten[g] += 1
            return b'guid: ' + n.encode()
        return m.group(0)

    for top in ('Assets', 'ProjectSettings'):
        for dp, dns, fs in os.walk(os.path.join(src, top)):
            if os.path.realpath(dp) in removed:
                dns[:] = []
                continue
            dns[:] = [d for d in dns if os.path.realpath(os.path.join(dp, d)) not in removed]
            out_dir = os.path.join(stage, os.path.relpath(dp, src))
            os.makedirs(out_dir, exist_ok=True)
            for f in fs:
                p = os.path.join(dp, f)
                base = p[:-5] if f.endswith('.meta') else p
                if os.path.realpath(base) in removed:
                    continue
                if os.path.isdir(base) and os.path.realpath(base) in removed:
                    continue
                q = os.path.join(out_dir, f)
                with open(p, 'rb') as fh:
                    head = fh.read(5)
                if head == b'%YAML' and not f.endswith('.meta'):
                    data = open(p, 'rb').read()
                    open(q, 'wb').write(REF_RE.sub(sub, data))
                else:
                    shutil.copy2(p, q)

    for rel, old, new in PATCHES:
        p = os.path.join(stage, 'Assets', 'Scripts', rel)
        text = open(p, encoding='utf-8').read()
        if old not in text:
            sys.exit(f'patch target not found in {rel}: {old!r}')
        open(p, 'w', encoding='utf-8').write(text.replace(old, new, 1))
    nuget_dir = os.path.join(stage, 'Assets', 'Plugins', 'NuGet')
    os.makedirs(nuget_dir, exist_ok=True)
    for pid, _ in sorted(set(NUGET.values())):
        shutil.copy2(os.path.join(donor, 'nuget', pid + '.dll'), nuget_dir)

    pkgs = os.path.join(stage, 'Packages')
    os.makedirs(pkgs, exist_ok=True)
    manifest = json.load(open(os.path.join(src, 'Packages', 'manifest.json')))
    manifest['dependencies'].update(UNITY_REGISTRY)
    for name, rel in EMBEDDED.items():
        shutil.copytree(os.path.join(donor, rel), os.path.join(pkgs, name), symlinks=False)
    manifest['dependencies'] = dict(sorted(manifest['dependencies'].items()))
    json.dump(manifest, open(os.path.join(pkgs, 'manifest.json'), 'w'), indent=2)

    report['remapped_guids'] = len(remap)
    report['rewritten_refs'] = sum(rewritten.values())
    for asm, r in report['scripts'].items():
        print(f'{asm:50s} mapped {r["mapped"]:5d} unmapped {r["unmapped"]:4d}')
    print(f'shaders swapped: {len(report["shaders"])}, package assets swapped: {len(report["package_assets"])}')
    print(f'guid remaps: {len(remap)}, references rewritten: {report["rewritten_refs"]}')
    if report_path:
        json.dump(report, open(report_path, 'w'), indent=1)


main()
