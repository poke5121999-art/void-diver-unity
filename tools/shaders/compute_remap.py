#!/usr/bin/env python3
"""Point the staged project's ComputeShader references at the rebuilt .compute files (run by apply.sh on the stage).

    compute_remap.py <stage_dir> <compute_dir> <donor_dir> [--report out.json]

AssetRipper exports a ComputeShader as a YAML .asset holding only the shipped D3D11/D3D12 variants, so on the OpenGL
editor every kernel is missing ("Kernel 'CSMain' not found"). build_project.py remaps dummy *graphics* shaders by
name (repo_shaders); this does the same for compute shaders after it: every staged `!u!72` .asset whose m_Name has a
<compute_dir>/<name>.compute (tools/shaders/compute_generate.py) or a donor package <name>.compute is dropped from the
stage, and every reference to its GUID is rewritten to the .compute's GUID (fileID 7200000 is the main object of
both; type 2 = native asset becomes type 3 = imported asset).
"""
import json
import os
import re
import sys

sys.path.insert(0, os.path.dirname(os.path.abspath(__file__)))
from compute_generate import donor_provides  # noqa: E402

GUID_RE = re.compile(rb'^guid: ([0-9a-f]{32})', re.M)
NAME_RE = re.compile(rb'^  m_Name: (.*)$', re.M)
YAML_EXT = ('.asset', '.prefab', '.unity', '.mat', '.controller', '.overrideController', '.anim', '.playable',
            '.spriteatlas', '.spriteatlasv2', '.lighting', '.preset', '.mask', '.signal', '.renderTexture', '.flare',
            '.guiskin', '.fontsettings', '.physicMaterial', '.physicsMaterial2D', '.cubemap', '.shadervariants')


def meta_guid(path):
    try:
        m = GUID_RE.search(open(path + '.meta', 'rb').read())
    except FileNotFoundError:
        return None
    return m.group(1).decode() if m else None


def main():
    args = sys.argv[1:]
    stage, compute_dir, donor = args[:3]
    report_path = args[args.index('--report') + 1] if '--report' in args else None
    remap, dropped = {}, {}
    for dp, _, fs in os.walk(os.path.join(stage, 'Assets')):
        for f in fs:
            if not f.endswith('.asset'):
                continue
            p = os.path.join(dp, f)
            with open(p, 'rb') as fh:
                head = fh.read(4096)
            if b'--- !u!72 ' not in head:
                continue
            m = NAME_RE.search(head)
            name = m.group(1).decode().strip() if m else None
            target = os.path.join(compute_dir, name + '.compute') if name else None
            if target and os.path.exists(target):
                new = meta_guid(target)
                src = os.path.relpath(target, os.path.dirname(os.path.dirname(compute_dir)))
            else:
                hit = name and donor_provides(donor, name)
                new = meta_guid(os.path.join(donor, hit)) if hit else None
                src = hit
            old = meta_guid(p)
            rel = os.path.relpath(p, stage)
            if not new or not old:
                print('compute_remap: no rebuilt or package .compute for %s (%s), kept' % (rel, name))
                continue
            remap[old] = new
            dropped[rel] = {'name': name, 'old': old, 'new': new, 'to': src}
            os.remove(p)
            os.remove(p + '.meta')
    if not remap:
        print('compute_remap: nothing to remap')
        return 0
    ref = re.compile(rb'\{fileID: 7200000, guid: (' + b'|'.join(g.encode() for g in remap) + rb'), type: 2\}')
    anyref = re.compile(rb'guid: (' + b'|'.join(g.encode() for g in remap) + rb')')
    files = refs = 0
    for top in ('Assets', 'ProjectSettings'):
        for dp, _, fs in os.walk(os.path.join(stage, top)):
            for f in fs:
                if not (f.endswith(YAML_EXT) or f.endswith('.asset')):
                    continue
                p = os.path.join(dp, f)
                data = open(p, 'rb').read()
                if not anyref.search(data):
                    continue
                new, n = ref.subn(lambda m: b'{fileID: 7200000, guid: %s, type: 3}' % remap[m.group(1).decode()].encode(), data)
                if anyref.search(new):
                    sys.exit('compute_remap: %s references a remapped compute GUID in an unexpected form' % p)
                open(p, 'wb').write(new)
                files += 1
                refs += n
    print('compute_remap: %d ComputeShader assets -> .compute, %d references in %d files' % (len(remap), refs, files))
    if report_path:
        json.dump({'dropped': dropped, 'references': refs, 'files': files}, open(report_path, 'w'), indent=1)
    return 0


if __name__ == '__main__':
    sys.exit(main())
