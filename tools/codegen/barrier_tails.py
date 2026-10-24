#!/usr/bin/env python3
"""Flag methods whose Ghidra pseudo-C lost statements after an `il2cpp_write_barrier` call.

Ghidra sometimes ends a function right after a write barrier: the field store, dispose or whole tail that follows is
missing from the pseudo-C, so the C# translated from it misses it too. Every reference-field store in IL2CPP code
is followed by a write barrier (a `call`/tail `jmp` to the thunk in the disassembly; either a call or the inlined
card-marking loop `LOCK(); ... UNLOCK(); } while (a != b);` in the pseudo-C), so a dropped store shows as fewer
barriers in the pseudo-C than in the machine code.

For every method in the decomp output of the given assemblies this script counts both and reports the method when
the disassembly (linear, from the method address to the first int3 or the next method start) has more barriers than the pseudo-C; the tail
`jmp` barriers of all branches count as one, because Ghidra merges them into one shared block. For each
report it also prints the straight-line tail after the last barrier call in the disassembly (calls, stores before the
next ret/jmp) as a hint of what was lost.
Heuristic: the compiler can duplicate a barrier into two paths (false positive, read the disassembly), and a method
that lost only non-reference statements is not seen. An async `MoveNext` is only reported when the gap is 2 or more (its exception funclet always adds one).
Methods that share one body (COMDAT) are reported once per name.
Reviewed methods (restored in C#, or false positives) go in tools/codegen/barrier_tails.ok, one `Type.Method` per
line (`#` comments allowed), and are skipped.

Usage: ~/vd-rip/venv/bin/python -I tools/codegen/barrier_tails.py [Assembly ...]   (default: the translated assemblies)
Needs capstone and pefile. Environment: VD_GAME = game folder with GameAssembly.dll.
Prints `<Assembly> <Type>.<Method> @addr barriers pseudoC=<n> asm=<n> tail(calls/stores)=<c>/<s>` and a count
per assembly on stderr.
"""
import os
import re
import struct
import sys
from pathlib import Path

import pefile
from capstone import Cs, CS_ARCH_X86, CS_MODE_64

ROOT = Path(__file__).resolve().parents[2]
HOME = Path.home()
DECOMP = HOME / "unity-decode-kit/out/vd/code/decomp"
GAME = Path(os.environ.get("VD_GAME", HOME / "vd-src/VOID DIVER Escape from the Abyss Demo"))
DLL = GAME / "GameAssembly.dll"
# il2cpp helpers that are not source statements: init metadata, class-init check, null-reference raise
HELPERS = {0x1849B7580, 0x1849B78C0, 0x1849B7820}
BARRIER = 0x1849B67F0  # il2cpp_write_barrier thunk (Il2CppComObject.Finalize area; every `call` before `ret` of a store)
OKFILE = Path(__file__).with_name("barrier_tails.ok")
ASSEMBLIES = ["De.Base", "De.InGame", "De.Scenes", "NemoLib", "Global", "LCArt", "LCArt.URP", "SomeNet.Unity"]

HEADER = re.compile(r"^// (.*?)\s+@0x([0-9A-Fa-f]+)\s*$", re.M)
# Ghidra shows a write barrier either as a call or as the inlined card-marking loop (LOCK/UNLOCK ... while (a != b);).
BARRIER_C = re.compile(r"il2cpp_write_barrier\s*\(|UNLOCK\(\);\s*\}\s*while\s*\([^;{}]*\);")

data = DLL.read_bytes()
pe = pefile.PE(str(DLL), fast_load=True)
BASE = pe.OPTIONAL_HEADER.ImageBase
SECS = [(BASE + s.VirtualAddress, min(s.Misc_VirtualSize, s.SizeOfRawData), s.PointerToRawData) for s in pe.sections]
md = Cs(CS_ARCH_X86, CS_MODE_64)
md.detail = False


def va2off(v):
    for va, n, raw in SECS:
        if va <= v < va + n:
            return raw + (v - va)
    return None


_STARTS = None


def next_start(addr):
    """Address of the next method start in any decomp file (functions are not always int3-padded)."""
    global _STARTS
    if _STARTS is None:
        import bisect  # noqa: F401
        found = set()
        for path in DECOMP.rglob("*.c"):
            found.update(int(m.group(2), 16) for m in HEADER.finditer(path.read_text(errors="replace")))
        _STARTS = sorted(found)
    import bisect
    k = bisect.bisect_right(_STARTS, addr)
    return _STARTS[k] if k < len(_STARTS) else addr + 0x6000


def asm_barriers(addr):
    """(barrier count, (calls, stores) on the straight-line path after the last barrier call) or (None, None)."""
    off = va2off(addr)
    if off is None:
        return None, None
    ins_list = []
    end = next_start(addr)
    for ins in md.disasm(data[off:off + min(0x6000, end - addr)], addr):
        if ins.mnemonic == "int3":
            break  # padding, or the end of a no-return call
        ins_list.append(ins)
    # Ghidra merges the tail-`jmp` barriers of several branches into one shared block, so they count once
    ncall = sum(1 for ins in ins_list if ins.mnemonic == "call" and ins.op_str == hex(BARRIER))
    njmp = sum(1 for ins in ins_list if ins.mnemonic == "jmp" and ins.op_str == hex(BARRIER))
    nbar = ncall + (1 if njmp else 0)
    last = None
    for k, ins in enumerate(ins_list):
        # a `jmp` to the thunk is a tail call ending its path; what follows in memory is another block
        if ins.mnemonic == "call" and ins.op_str.startswith("0x") and int(ins.op_str, 16) == BARRIER:
            last = k
    if last is None:
        return nbar, (0, 0)
    calls = stores = 0
    for ins in ins_list[last + 1:]:
        if ins.mnemonic == "ret":
            break
        if ins.mnemonic in ("call", "jmp") and ins.op_str.startswith("0x"):
            tgt = int(ins.op_str, 16)
            if ins.mnemonic == "jmp" and addr <= tgt <= ins_list[-1].address:
                break  # local jump: the straight-line path after the barrier ends here
            if tgt != BARRIER and tgt not in HELPERS:
                calls += 1
            if ins.mnemonic == "jmp":
                break  # tail call
        elif ins.mnemonic in ("call", "jmp"):
            calls += 1
            if ins.mnemonic == "jmp":
                break
        elif ins.mnemonic == "mov" and ins.op_str.startswith(("qword ptr [", "dword ptr [", "byte ptr [")):
            dst = ins.op_str.split(",")[0]
            if "rsp" not in dst and "rbp" not in dst:
                stores += 1
    return nbar, (calls, stores)


def reviewed():
    out = set()
    if OKFILE.exists():
        for line in OKFILE.read_text().splitlines():
            line = line.split("#")[0].strip()
            if line:
                out.add(line)
    return out


def main():
    asms = sys.argv[1:] or ASSEMBLIES
    ok = reviewed()
    total = 0
    for asm in asms:
        n = 0
        for path in sorted((DECOMP / asm).rglob("*.c")):
            text = path.read_text(errors="replace")
            heads = list(HEADER.finditer(text))
            for k, h in enumerate(heads):
                body = text[h.end():heads[k + 1].start() if k + 1 < len(heads) else len(text)]
                addr = int(h.group(2), 16)
                nc = len(BARRIER_C.findall(body))
                na, tail = asm_barriers(addr)
                if na is None or na <= nc:
                    continue
                # An async MoveNext always has one extra barrier in the exception funclet (SetException /
                # PublishUnobservedTaskException) that Ghidra leaves out; only a bigger gap means a lost statement.
                if "MoveNext" in h.group(1) and na - nc < 2:
                    continue
                sig = h.group(1)
                name = "%s.%s" % (path.stem, (sig if sig.find(" ") > sig.find("(") >= 0 or " " not in sig
                                              else sig.split(" ", 1)[1]).split("(")[0])
                if name in ok or "%s.%s" % (path.stem.split("+")[-1], name.split(".", 1)[1]) in ok:
                    continue
                n += 1
                print("%s %s @0x%x barriers pseudoC=%d asm=%d tail(calls/stores)=%d/%d"
                      % (asm, name, addr, nc, na, tail[0], tail[1]))
        print("%-14s %d" % (asm, n), file=sys.stderr)
        total += n
    print("total          %d" % total, file=sys.stderr)


if __name__ == "__main__":
    main()
