# Audit: decomp labels StringLiteral__<X> name strings that start with punctuation ('@Text', '_Text');
# flag C# classes that use the bare "<X>" literal but none of the label's real strings.
import os, re, sys, collections
code = os.path.expanduser('~/unity-decode-kit/out/vd/code')
src = os.path.expanduser('~/void-diver-unity/Assets/Scripts')
lab = collections.defaultdict(set)
for line in open(os.path.join(code, 'strings.tsv'), encoding='utf-8', errors='replace'):
    p = line.rstrip('\n').split('\t')
    if len(p) >= 3: lab[p[0]].add(p[2])
cs = {}
for root, _, files in os.walk(src):
    for f in files:
        if f.endswith('.cs'): cs.setdefault(f[:-3], os.path.join(root, f))
hits = []
for asm in os.listdir(os.path.join(code, 'decomp')):
    d = os.path.join(code, 'decomp', asm)
    if not os.path.isdir(d): continue
    for f in [x for x in os.listdir(d) if os.path.isfile(os.path.join(d, x))]:
        cls = re.split(r'[+.]', f)[0]
        if cls not in cs: continue
        txt = open(os.path.join(d, f), encoding='utf-8', errors='replace').read()
        for m in set(re.findall(r'StringLiteral__([A-Za-z]\w*)\b', txt)):
            vals = lab.get('StringLiteral__' + m, set())
            if not vals or any(v == m for v in vals): continue
            c = open(cs[cls], encoding='utf-8', errors='replace').read()
            if ('"%s"' % m) in c and not any(('"%s"' % v) in c for v in vals):
                hits.append((cls, m, sorted(vals)))
for h in sorted(set((a, b, tuple(c)) for a, b, c in hits)): print(*h)
