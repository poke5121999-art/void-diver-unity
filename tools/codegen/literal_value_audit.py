#!/usr/bin/env python3
"""Find integer literals in the translated game code that the binary does not have.

Ghidra prints most constants in hex (`AddBuff(..., 0x2dc6c1, ...)`); converting them to decimal by hand went wrong
in places (0x2dc6c1 = 3000001 became 2934465, 0x1e9421 = 2004001 became 1987617, 0x98c560 = 10012000 became
10000000), and a table id that does not exist throws KeyNotFoundException at run time. For each C# file of the game
assemblies, every decimal literal >= 256 must appear in that class's decomp (as hex or decimal).
Prints the misses; constants defined only in C# (Const.cs, Win32 flags, GUIDs in InputActions) are expected noise.
Usage: python3 -I tools/codegen/literal_value_audit.py [min_value]
"""
import glob
import os
import re
import sys

DECOMP = os.path.expanduser('~/unity-decode-kit/out/vd/code/decomp')
REPO = os.path.dirname(os.path.dirname(os.path.dirname(os.path.abspath(__file__))))
SCRIPTS = os.path.join(REPO, 'Assets', 'Scripts')
MIN = int(sys.argv[1]) if len(sys.argv) > 1 else 256
NOISE = ('InputActions.cs', 'Const.cs', 'GraphicSettingService.cs', 'SROptions.cs')

decomp = {}
for f in glob.glob(DECOMP + '/**/*.c', recursive=True):
    decomp.setdefault(os.path.basename(f)[:-2].split('+')[0], []).append(f)
cache = {}


def text(cls):
    if cls not in cache:
        cache[cls] = ''.join(open(f, errors='ignore').read() for f in decomp.get(cls, [])).lower()
    return cache[cls]


files = glob.glob(SCRIPTS + '/De.*/**/*.cs', recursive=True) + glob.glob(SCRIPTS + '/NemoLib/**/*.cs', recursive=True)
for f in sorted(files):
    cls = os.path.basename(f)[:-3]
    if cls not in decomp or os.path.basename(f) in NOISE:
        continue
    src = open(f, errors='ignore').read()
    lines = src.split('\n')
    for m in re.finditer(r'(?<![\w.#"-])(\d{3,})(?![\w.])', src):
        v = int(m.group(1))
        if v < MIN:
            continue
        t = text(cls)
        if hex(v) in t or str(v) in t:
            continue
        line = src[:m.start()].count('\n') + 1
        if lines[line - 1].lstrip().startswith('//'):
            continue
        print(f'{os.path.relpath(f, REPO)}:{line}: {v} (0x{v:x}) not in the decomp | {lines[line - 1].strip()[:90]}')
