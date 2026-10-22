"""Find GUID references in a staged project that point at nothing.

Usage: python check_refs.py <project dir> [extra package dirs...]
A reference resolves if some .meta under Assets/, Packages/ or an extra dir (e.g. the donor packages that
Unity installs from the registry) carries that GUID, or it is one of Unity's built-in resource GUIDs.
Prints the dangling GUIDs by reference count and exits 1 if any.
"""
import os, re, sys
from collections import Counter

BUILTIN = {'0000000000000000e000000000000000', '0000000000000000f000000000000000', '0000000000000000d000000000000000',
           '00000000000000000000000000000000'}
GUID_RE = re.compile(rb'^guid: ([0-9a-f]{32})', re.M)
REF_RE = re.compile(rb'guid: ([0-9a-f]{32})')
YAML_EXT = ('.prefab', '.unity', '.asset', '.mat', '.controller', '.overrideController', '.playable', '.anim',
            '.mixer', '.spriteatlas', '.lighting', '.signal', '.preset', '.physicMaterial', '.mask', '.flare',
            '.renderTexture', '.cubemap', '.guiskin', '.fontsettings', '.terrainlayer', '.brush')


def known_guids(roots):
    out = set(BUILTIN)
    for r in roots:
        for dp, _, fs in os.walk(r, followlinks=True):
            for f in fs:
                if f.endswith('.meta'):
                    m = GUID_RE.search(open(os.path.join(dp, f), 'rb').read(400))
                    if m:
                        out.add(m.group(1).decode())
    return out


def main():
    proj = sys.argv[1]
    known = known_guids([os.path.join(proj, 'Assets'), os.path.join(proj, 'Packages')] + sys.argv[2:])
    dangling, where = Counter(), {}
    for dp, _, fs in os.walk(os.path.join(proj, 'Assets')):
        for f in fs:
            if not f.endswith(YAML_EXT):
                continue
            p = os.path.join(dp, f)
            for g in REF_RE.findall(open(p, 'rb').read()):
                g = g.decode()
                if g not in known:
                    dangling[g] += 1
                    where.setdefault(g, os.path.relpath(p, proj))
    for g, n in dangling.most_common(40):
        print(f'{n:7d}  {g}  e.g. {where[g]}')
    print(f'dangling guids: {len(dangling)}, references: {sum(dangling.values())}')
    sys.exit(1 if dangling else 0)


main()
