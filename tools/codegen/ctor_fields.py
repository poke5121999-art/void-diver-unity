#!/usr/bin/env python3
"""List fields the IL2CPP constructor assigns that the C# translation never initialises.

C# field initialisers compile into `.ctor` (instance) and `.cctor` (static). For every class/struct declared in the
translated assemblies, read the Ghidra pseudo-C of its constructors and collect `(this->fields).X = ...` and
`TypeInfo__T->static_fields->X = ...`. A field is reported when the C# type declares it but neither gives it an
initialiser nor assigns it inside a constructor. A field whose every store is 0/null/false/Vector*.zero is skipped
(that is the default anyway); struct fields count as set when any component store is non-default.
Base-class stores (`(this->fields)._.X`) and compiler-generated types (`<>c`, display classes, state machines) are out
of scope.

Usage: python3 tools/codegen/ctor_fields.py [Assembly ...]
Prints a count per assembly and writes the TSV to ~/vd-rip/w4/ctor_fields.tsv (override with CTOR_FIELDS_TSV).
"""
import os
import re
import sys
from pathlib import Path

ROOT = Path(__file__).resolve().parents[2]
SCRIPTS = ROOT / "Assets/Scripts"
HOME = Path.home()
DECOMP = HOME / "unity-decode-kit/out/vd/code/decomp"
if not DECOMP.is_dir():
    DECOMP = HOME / "unity-decode-kit/out/vd/code.old/decomp"
W4 = HOME / "vd-rip/w4"
TSV = Path(os.environ.get("CTOR_FIELDS_TSV", W4 / "ctor_fields.tsv"))

ASSEMBLIES = ["NemoLib", "Base.Art", "LCArt", "NemoMapTools", "LCArt.URP", "SomeNet.Unity", "De.Const", "De.Input",
              "De.Lua", "Global", "Assembly-CSharp", "De.Cheat", "De.Scenes", "De.Base", "De.InGame"]
# Files other agents still have to translate.
# Batch lists (~/vd-rip/w4/<name>.txt) still being translated; their files are skipped. The W4 batches are all done:
# a stale entry here hid ~1300 files from the audit (RewardBoxSpawnData.SlotCount = -1 went unseen, A5 run 4).
PENDING = []

DEFAULT_RHS = re.compile(r"^(?:\([^()]*\)\s*)?(?:0|0x0|false|NULL|'\\0'|-?0\.0|.*->zeroVector\)\.[xyzw])$")


def pending_files():
    out = set()
    for name in PENDING:
        p = W4 / (name + ".txt")
        if p.exists():
            out |= {line.strip() for line in p.read_text().splitlines() if line.strip()}
    return out


# ---------------------------------------------------------------- C# side

def strip_code(src):
    """Blank out comments, strings and chars, keeping offsets (so brace matching is safe)."""
    out, i, n = [], 0, len(src)
    while i < n:
        c = src[i]
        if src.startswith("//", i):
            j = src.find("\n", i)
            j = n if j < 0 else j
            out.append(" " * (j - i)); i = j
        elif src.startswith("/*", i):
            j = src.find("*/", i + 2)
            j = n if j < 0 else j + 2
            out.append(re.sub(r"[^\n]", " ", src[i:j])); i = j
        elif c == '"' or src.startswith('@"', i) or src.startswith('$"', i) or src.startswith('$@"', i) \
                or src.startswith('@$"', i):
            k = src.index('"', i)
            verbatim = "@" in src[i:k]
            j = k + 1
            while j < n:
                if src[j] == "\\" and not verbatim:
                    j += 2; continue
                if src[j] == '"':
                    if verbatim and j + 1 < n and src[j + 1] == '"':
                        j += 2; continue
                    break
                j += 1
            out.append('"' + " " * (j - i - 1) + '"' if j < n else " " * (j - i)); i = j + 1
        elif c == "'":
            j = i + 1
            while j < n and src[j] != "'":
                j += 2 if src[j] == "\\" else 1
            out.append(" " * (j - i + 1)); i = j + 1
        else:
            out.append(c); i += 1
    return "".join(out)


def match_brace(s, i):
    depth = 0
    for j in range(i, len(s)):
        if s[j] == "{":
            depth += 1
        elif s[j] == "}":
            depth -= 1
            if depth == 0:
                return j
    return len(s) - 1


TYPE_RE = re.compile(r"\b(?:class|struct|record)\s+(\w+)(?:\s*<[^>{]*>)?[^{;]*\{")
NS_RE = re.compile(r"\bnamespace\s+([\w.]+)")


def cs_types(path):
    """Yield (pseudo-C name, namespace, declared fields, initialised-or-ctor-assigned names) per type in the file."""
    code = strip_code(path.read_text(encoding="utf-8-sig"))
    ns_m = NS_RE.search(code)
    ns = ns_m.group(1) if ns_m else ""
    spans = []
    for m in TYPE_RE.finditer(code):
        start = m.end() - 1
        spans.append((m.group(1), start, match_brace(code, start)))
    for name, start, end in spans:
        outer = [o for o in spans if o[1] < start and o[2] > end]
        outer.sort(key=lambda o: o[1])
        cname = "+".join([o[0] for o in outer] + [name])
        body = list(code[start + 1:end])
        for _, s2, e2 in spans:  # mask nested types
            if start < s2 and e2 < end:
                for k in range(s2 - start - 1, e2 - start):
                    body[k] = " "
        body = "".join(body)
        assigned = set()
        for cm in re.finditer(r"\b%s\s*\([^)]*\)\s*(?::\s*(?:base|this)\s*\([^)]*\)\s*)?\{" % re.escape(name), body):
            b0 = cm.end() - 1
            ctor = body[b0:match_brace(body, b0) + 1]
            assigned |= set(re.findall(r"(\w+)\s*(?:\?\?)?=(?![=>])", ctor))
            assigned |= set(re.findall(r"\bout\s+(\w+)", ctor))
        # top level only: drop every {...} block, then look at ';'-separated member declarations
        top, depth = [], 0
        for k, ch in enumerate(body):
            if ch == "{":
                depth += 1
            elif ch == "}":
                depth -= 1
                # a closed member block ends the member, unless an initialiser follows: P { get; } = x;
                if depth == 0 and not re.match(r"\s*=(?![=>])", body[k + 1:k + 40]):
                    top.append(";")
                continue
            if depth == 0:
                top.append(ch)
        declared = set()
        for stmt in "".join(top).split(";"):
            stmt = re.sub(r"\[[^\[\]]*(?:\[[^\[\]]*\][^\[\]]*)*\]", " ", stmt).strip()
            if not stmt:
                continue
            head = re.split(r"=(?![=>])", stmt, maxsplit=1)
            if "=>" in head[0] or head[0].count("(") != head[0].count(")"):
                continue  # expression-bodied member, or a default parameter value
            names = re.findall(r"(\w+)\s*$", head[0])
            if not names:
                continue
            declared.add(names[0])
            if len(head) > 1:
                assigned.add(names[0])
                # int a = 1, b = 2;
                assigned |= set(re.findall(r",\s*(\w+)\s*=(?![=>])", head[1]))
            for extra in re.findall(r",\s*(\w+)\s*(?==(?![=>])|,|$)", head[0]):
                declared.add(extra)
        yield cname, ns, declared, assigned


# ---------------------------------------------------------------- pseudo-C side

HEADER = re.compile(r"^// (.*?)\s+@0x[0-9A-Fa-f]+\s*$", re.M)
INST = re.compile(r"\(this->fields\)\.(?!_\.)(\w+)(?:\.\w+)*\s*=(?!=)\s*([^;]*);")
STAT = re.compile(r"TypeInfo__(\w+)->static_fields->(\w+)\)?(?:\.\w+)*\s*=(?!=)\s*([^;]*);")
STAT_PTR = re.compile(r"(\w+) = TypeInfo__(\w+)->static_fields;")


def field_name(c_name):
    m = re.fullmatch(r"_(\w+)_k__BackingField", c_name)
    return m.group(1) if m else c_name


def c_file(asm, ns, cname):
    base = DECOMP / asm
    cands = []
    if ns:
        cands.append(base.joinpath(*ns.split("."), cname + ".c"))
    cands.append(base / (cname + ".c"))
    for c in cands:
        if c.exists():
            return c
    found = sorted(base.rglob(cname + ".c")) if base.is_dir() else []
    return found[0] if len(found) == 1 else None


def ctor_stores(path, cname):
    """{field: (kind, [rhs, ...])} for stores in the constructors of cname in this .c file."""
    text = path.read_text(errors="replace")
    heads = list(HEADER.finditer(text))
    short = cname.split("+")[-1]
    stores = {}
    for k, h in enumerate(heads):
        sig = h.group(1)
        if not re.fullmatch(r"(?:%s|%s)\(.*\)" % (re.escape(cname), re.escape(short)), sig):
            continue
        body = text[h.end():heads[k + 1].start() if k + 1 < len(heads) else len(text)]
        for fm in INST.finditer(body):
            stores.setdefault(field_name(fm.group(1)), ("instance", []))[1].append(" ".join(fm.group(2).split()))
        mine = lambda t: t == short or t.endswith("__" + short) or t.endswith("__" + cname.replace("+", "_"))
        for sm in STAT.finditer(body):
            if mine(sm.group(1)):
                stores.setdefault(field_name(sm.group(2)), ("static", []))[1].append(" ".join(sm.group(3).split()))
        for pm in STAT_PTR.finditer(body):  # p = TypeInfo__T->static_fields; (p->X).r = ...
            if mine(pm.group(2)):
                for sm in re.finditer(r"\(?%s->(\w+)\)?(?:\.\w+)*\s*=(?!=)\s*([^;]*);" % pm.group(1), body):
                    stores.setdefault(field_name(sm.group(1)), ("static", []))[1].append(" ".join(sm.group(2).split()))
    return stores


def main():
    asms = sys.argv[1:] or ASSEMBLIES
    skip = pending_files()
    rows, counts = [], {}
    for asm in asms:
        counts[asm] = 0
        for cs in sorted((SCRIPTS / asm).rglob("*.cs")):
            rel = cs.relative_to(SCRIPTS).as_posix()
            if rel in skip:
                continue
            for cname, ns, declared, assigned in cs_types(cs):
                cf = c_file(asm, ns, cname)
                if cf is None:
                    continue
                for fld, (kind, rhss) in sorted(ctor_stores(cf, cname).items()):
                    if fld not in declared or fld in assigned or all(DEFAULT_RHS.match(r) for r in rhss):
                        continue
                    if any("zeroVector" in r for r in rhss) and all(re.fullmatch(r"fVar\d+", r) or "zeroVector" in r
                                                                   for r in rhss):
                        continue  # Vector*.zero copied component by component
                    rhs = " | ".join(rhss)
                    counts[asm] += 1
                    rows.append((asm, rel, cname, fld, kind, rhs, str(cf.relative_to(DECOMP))))
    TSV.parent.mkdir(parents=True, exist_ok=True)
    with open(TSV, "w") as f:
        f.write("assembly\tfile\ttype\tfield\tkind\trhs\tpseudo_c\n")
        for r in rows:
            f.write("\t".join(x.replace("\t", " ") for x in r) + "\n")
    for asm in asms:
        print("%-16s %d" % (asm, counts[asm]))
    print("total            %d  -> %s" % (sum(counts.values()), TSV))


if __name__ == "__main__":
    main()
