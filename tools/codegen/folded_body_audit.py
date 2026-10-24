#!/usr/bin/env python3
"""Find methods and lambdas translated as empty although the binary gives them a body shared with another method.

IL2CPP builds link with identical-code folding: methods whose machine code is the same share one address. In the
Ghidra decomp each such method gets its own `// <signature>  @0x<addr>` header, but the body printed under it is
labelled with whichever method Ghidra named that address after (`/* Void <OnInputEnabled>b__11_0(...) */` under
`// Void OnAnimationEnd()`). A translator reading the label as "another method" drops the body. The most common
shared body is `this.Close(null)` at 0x180065E50 (SamplePopupPresenter.<OnInputEnabled>b__11_0), shared by every
popup's Esc lambda, OnCancel and OnAnimationEnd.

Reports, for each class:
- a named method whose C# body is empty while its folded decomp body makes a call;
- a class with empty delegates/lambdas while one of its folded lambdas (`b__`) makes a call.
Usage: python3 -I tools/codegen/folded_body_audit.py   (prints nothing when clean)
"""
import glob
import os
import re

DECOMP = os.path.expanduser('~/unity-decode-kit/out/vd/code/decomp')
REPO = os.path.dirname(os.path.dirname(os.path.dirname(os.path.abspath(__file__))))
SCRIPTS = os.path.join(REPO, 'Assets', 'Scripts')
HEADER = re.compile(r'^// (.+?)\s+@0x([0-9A-Fa-f]+)\s*$')

cs_files = {}
for f in glob.glob(SCRIPTS + '/**/*.cs', recursive=True):
    cs_files.setdefault(os.path.basename(f)[:-3], []).append(f)


def folded_calls(lines, i, name):
    """The first call in the body under header i when that body is labelled with another method's name."""
    j = i + 1
    while j < len(lines) and lines[j].startswith('/* WARNING'):
        j += 1
    label = lines[j] if j < len(lines) else ''
    if not label.startswith('/* ') or name in label:
        return None
    for x in lines[j + 1:j + 80]:
        if HEADER.match(x):
            break
        s = x.strip()
        if x[:1].isspace() and '(' in s and s.endswith(';') and not s.startswith(('/*', '//')):
            # UIToolkitTextSource.ModifyMesh is an empty method many empty bodies are folded into (a plain return).
            if 'ModifyMesh' in s or 'ModifyMesh' in lines[lines.index(x) - 1]:
                continue
            return s[:90]
    return None


for asm in sorted(os.listdir(DECOMP)):
    for f in sorted(glob.glob(os.path.join(DECOMP, asm, '*.c'))):
        cls = os.path.basename(f)[:-2]
        if '+' in cls or cls not in cs_files:
            continue
        lines = open(f, errors='ignore').read().split('\n')
        named, lambdas = [], []
        for i, line in enumerate(lines):
            m = HEADER.match(line)
            if not m:
                continue
            sig = re.search(r'([\w<>]+)\(', m.group(1))
            if not sig:
                continue
            name = sig.group(1)
            call = folded_calls(lines, i, name)
            if call is None:
                continue
            (lambdas if 'b__' in name else named).append((name, call))
        if not named and not lambdas:
            continue
        for cf in cs_files[cls]:
            src = open(cf, errors='ignore').read()
            rel = os.path.relpath(cf, REPO)
            for name, call in named:
                for m in re.finditer(r'\b' + re.escape(name) + r'\s*\([^)]*\)\s*\{\s*\}', src):
                    print(f'{rel}:{src[:m.start()].count(chr(10)) + 1}: {name} is empty; binary body: {call}')
            if lambdas:
                for m in re.finditer(r'delegate[^{;]*\{\s*\}|=>\s*\{\s*\}', src):
                    print(f'{rel}:{src[:m.start()].count(chr(10)) + 1}: empty lambda; folded lambdas of {cls}: '
                          + '; '.join(f'{n} -> {c}' for n, c in lambdas))
