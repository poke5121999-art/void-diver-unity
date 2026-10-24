# pkg_diff.py <types.cs> <strings.tsv> <cache> <package> <asms> <v1> <v2> ... (docs/PACKAGES.md)
# For consecutive versions: tokens (Type.member, string literals >= 12 chars) added/removed, and which the build has.
import glob, os, re, sys
types_cs, strings_tsv, cache, pkg, asms = sys.argv[1:6]; vers = sys.argv[6:]
asms = asms.split(',')
lines = open(types_cs, encoding='utf-8', errors='replace').read().split('\n')
ranges = []
for l in lines:
    m = re.match(r'// Image \d+: (\S+)\.dll - .* - Types (\d+)-(\d+)', l)
    if m and m.group(1) in asms: ranges.append((int(m.group(2)), int(m.group(3))))
decl = re.compile(r'\b(class|struct|enum|interface)\s+(\w+)(?:<[^>]*>)?[^/]*// TypeDefIndex: (\d+)')
ident = re.compile(r'\b([A-Za-z_]\w*)\s*(?:\(|;|=|\{| //)')
build, cur = set(), None
for l in lines:
    m = decl.search(l)
    if m:
        i = int(m.group(3)); cur = m.group(2) if any(a <= i <= b for a, b in ranges) else None
        if cur: build.add('T:' + cur)
        continue
    if cur and re.match(r'^\s+(public|private|internal|protected)', l):
        for n in ident.findall(l.split('//')[0]):
            build.add(cur + '.' + n)
lits = set()
for l in open(strings_tsv, encoding='utf-8', errors='replace'):
    p = l.rstrip('\n').split('\t', 2)
    if len(p) == 3 and len(p[2]) >= 12: lits.add('S:' + p[2])
build |= lits
sdecl = re.compile(r'^\s*(?:\[[^\]]*\]\s*)*(?:(?:public|private|internal|protected|static|sealed|abstract|partial|readonly|ref|unsafe|new)\s+)*(?:class|struct|enum|interface)\s+(\w+)')
smem = re.compile(r'^\s*(?:\[[^\]]*\]\s*)*(?:(?:public|private|internal|protected|static|virtual|override|abstract|sealed|unsafe|new|extern|async|readonly|const|volatile|event|partial)\s+)+[\w<>\[\],.?() ]*?\s(\w+)\s*(?:<[^>()]*>)?\s*(?:\(|;|=|\{|$)')
slit = re.compile(r'"((?:[^"\\]|\\.){12,})"')
def tokens(v):
    t = set()
    for f in glob.glob(os.path.join(cache, pkg, v, '**', '*.cs'), recursive=True):
        if any(x in f for x in ('/Editor/', '/Tests/', 'Samples~', 'Documentation', '/Editor.')): continue
        cls = None
        for l in open(f, encoding='utf-8', errors='replace'):
            if l.strip().startswith('//'): continue
            m = sdecl.match(l)
            if m: cls = m.group(1); t.add('T:' + cls); continue
            mm = smem.match(l)
            if mm and cls: t.add(cls + '.' + mm.group(1))
            for s in slit.findall(l):
                if '{' not in s and '\\' not in s: t.add('S:' + s)
    return t
prev = None
for v in vers:
    t = tokens(v)
    if prev:
        pv, pt = prev
        add, rem = t - pt, pt - t
        ah, rh = sorted(add & build), sorted(rem & build)
        print(f'{pv} -> {v}: added {len(add)} (build has {len(ah)}: {ah[:4]}) removed {len(rem)} (build has {len(rh)}: {rh[:4]})')
    prev = (v, t)
