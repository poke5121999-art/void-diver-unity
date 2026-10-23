#!/usr/bin/env python3
"""Decompile every extracted DXBC blob to HLSL with 3Dmigoto's cmd_Decompiler under wine.

    decompile.py <work_dir> [--jobs N] [--force] [--only <shader dir name>...]

Reads <work_dir>/<shader>/dxbc/<n>.dxbc, writes <work_dir>/<shader>/dxbc/<n>.hlsl and the disassembly <n>.asm (skips ones already done unless
--force). Each blob first goes through patchsig.patch (interpolator registers renamed VDREG<reg>, see patchsig.py) into
<n>.sig.dxbc, and the decompiler runs with its validation pass (-V: the HLSL is recompiled with d3dcompiler and must
compile), so a program that does not recompile has no .hlsl and generate.py reports its shader as failed.
Identical blobs (same sha1) are decompiled once and copied. Failures are listed in <work_dir>/decompile_failures.txt.
The decompiler (github.com/bo3b/3Dmigoto, cmd_Decompiler-1.3.16.zip) lives outside git in
~/vd-rip/third_party/cmd_decompiler (the zip ships the files as .ex/.dl; rename to .exe/.dll).
"""
import hashlib
import os
sys_path_here = os.path.dirname(os.path.abspath(__file__))
import shutil
import subprocess
import sys
from concurrent.futures import ThreadPoolExecutor

sys.path.insert(0, sys_path_here)
import patchsig  # noqa: E402

TOOL = os.path.expanduser('~/vd-rip/third_party/cmd_decompiler/cmd_Decompiler.exe')
BATCH = 40


def sig_path(f):
    return f[:-5] + '.sig.dxbc'


def run_batch(files):
    """files: original <n>.dxbc paths. Decompiles their patched copies and moves <n>.sig.hlsl to <n>.hlsl."""
    env = dict(os.environ, WINEDEBUG='-all')
    sigs = []
    for f in files:
        data, _ = patchsig.patch(open(f, 'rb').read())
        with open(sig_path(f), 'wb') as out:
            out.write(data)
        sigs.append(sig_path(f))
    try:
        p = subprocess.run(['wine', TOOL, '-D', '-V'] + sigs, env=env, capture_output=True, text=True, timeout=900)
        log = p.stdout + p.stderr
    except subprocess.TimeoutExpired:
        log = 'timeout'
    for f in files:
        h = f[:-5] + '.sig.hlsl'
        if os.path.exists(h):
            os.replace(h, f[:-5] + '.hlsl')
        os.remove(sig_path(f))
    # the disassembly (<n>.asm) is what generate.py translates (asm2hlsl.py); the HLSL above supplies the signature
    try:
        subprocess.run(['wine', TOOL, '-d'] + files, env=env, capture_output=True, timeout=900)
    except subprocess.TimeoutExpired:
        log += '\ndisassembly timeout'
    bad = [f for f in files if not os.path.exists(f[:-5] + '.hlsl') or not os.path.exists(f[:-5] + '.asm')]
    return bad, log


def main():
    args = sys.argv[1:]
    work = args.pop(0)
    jobs = 4
    only = None
    force = False
    while args:
        a = args.pop(0)
        if a == '--jobs':
            jobs = int(args.pop(0))
        elif a == '--force':
            force = True
        elif a == '--only':
            only = set(args)
            break
    todo = []
    for d in sorted(os.listdir(work)):
        if only and d not in only:
            continue
        dd = os.path.join(work, d, 'dxbc')
        if not os.path.isdir(dd):
            continue
        for f in sorted(os.listdir(dd)):
            if not f.endswith('.dxbc') or f.endswith('.sig.dxbc'):
                continue
            h = os.path.join(dd, f[:-5] + '.hlsl')
            if force and os.path.exists(h):
                os.remove(h)
            if not os.path.exists(h) or not os.path.exists(h[:-5] + '.asm'):
                todo.append(os.path.join(dd, f))
    # dedup by content
    by_hash = {}
    for f in todo:
        h = hashlib.sha1(open(f, 'rb').read()).hexdigest()
        by_hash.setdefault(h, []).append(f)
    uniq = [fs[0] for fs in by_hash.values()]
    print('to decompile', len(todo), 'unique', len(uniq), file=sys.stderr)
    batches = [uniq[i:i + BATCH] for i in range(0, len(uniq), BATCH)]
    failures = []
    with ThreadPoolExecutor(max_workers=jobs) as ex:
        for i, (bad, log) in enumerate(ex.map(run_batch, batches)):
            failures += bad
            print('batch %d/%d done, failed %d' % (i + 1, len(batches), len(bad)), file=sys.stderr)
    # retry failures one by one (a crash in a batch loses the rest of that batch)
    still = []
    for f in failures:
        bad, log = run_batch([f])
        if bad:
            errs = [ln.split('): ', 1)[-1] for ln in log.splitlines() if ': error X' in ln]
            still.append((f, '; '.join(errs)[:400] or (log.strip().splitlines()[-1] if log.strip() else '')))
    for fs in by_hash.values():
        for ext in ('.hlsl', '.asm'):
            src = fs[0][:-5] + ext
            if os.path.exists(src):
                for f in fs[1:]:
                    shutil.copyfile(src, f[:-5] + ext)
    with open(os.path.join(work, 'decompile_failures.txt'), 'a') as out:
        for f, why in still:
            out.write('%s\t%s\n' % (f, why))
    print('failed', len(still), file=sys.stderr)


if __name__ == '__main__':
    main()
