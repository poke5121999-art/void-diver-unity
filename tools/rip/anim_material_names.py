#!/usr/bin/env python3
"""Name the material-property curves the export left as hashes (rerunnable).

A clip that animates a material property binds it by attribute hash: CRC32 of the property name, low 28 bits, with
the component in the top 4 bits (0-3 = .x .y .z .w, 4-7 = .r .g .b .a, 8 = the float itself). When AssetRipper does
not know the name it writes the curve as `material.path_0x<hash>_<random>`, and the editor rebinds every curve from
its name, so the curve drives nothing. TL_Title_01's recorded clip tints the title's lounge backdrop that way
(material._BaseColor = 0.86, 0.89, 1): without the name the city lost its blue cast.
This finds each hash among the property names of our shaders and the exported materials and writes the name back.

  python3 -I tools/rip/anim_material_names.py [--dry-run]
"""
import glob, os, re, sys, zlib

REPO = os.path.dirname(os.path.dirname(os.path.dirname(os.path.abspath(__file__))))
DRY = '--dry-run' in sys.argv
SUFFIX = {0: '.x', 1: '.y', 2: '.z', 3: '.w', 4: '.r', 5: '.g', 6: '.b', 7: '.a', 8: ''}
PAT = re.compile(r'(?m)^(    attribute: )material\.path_0x([0-9A-Fa-f]{1,8})_[A-Za-z]+$')

clips = [f for f in glob.glob(os.path.join(REPO, 'Assets', '**', '*.anim'), recursive=True)
         if 'material.path_0x' in open(f, encoding='utf-8', errors='ignore').read()]
names = set()
sources = glob.glob(os.path.join(REPO, 'Assets', '_VD', 'Shaders', '**', '*.shader'), recursive=True) + \
          glob.glob(os.path.join(REPO, 'Assets', '**', '*.mat'), recursive=True)
for f in sources:
    names |= set(re.findall(r'\b(_[A-Za-z0-9_]+)\b', open(f, encoding='utf-8', errors='ignore').read()))
by_hash = {}
for n in names:
    by_hash.setdefault(zlib.crc32(n.encode()) & 0x0FFFFFFF, set()).add(n)

fixed = unresolved = 0
for f in clips:
    text = open(f, encoding='utf-8').read()

    def name(m):
        global fixed, unresolved
        h = int(m.group(2), 16)
        cands, comp = by_hash.get(h & 0x0FFFFFFF, set()), h >> 28
        if len(cands) != 1 or comp not in SUFFIX:
            unresolved += 1
            print('ANIM_MATERIAL unresolved %s 0x%08X candidates=%s' % (os.path.relpath(f, REPO), h, sorted(cands)))
            return m.group(0)
        fixed += 1
        return '%smaterial.%s%s' % (m.group(1), next(iter(cands)), SUFFIX[comp])

    new = PAT.sub(name, text)
    if new != text:
        print('ANIM_MATERIAL %s' % os.path.relpath(f, REPO))
        if not DRY:
            open(f, 'w', encoding='utf-8').write(new)
print('ANIM_MATERIALS fixed=%d unresolved=%d clips=%d%s' % (fixed, unresolved, len(clips), ' (dry run)' if DRY else ''))
