#!/usr/bin/env python3
"""Decompile every extracted DXBC blob to HLSL with 3Dmigoto's cmd_Decompiler under wine.

    decompile.py <work_dir> [--jobs N] [--only <shader dir name>...]

Reads <work_dir>/<shader>/dxbc/<n>.dxbc, writes <work_dir>/<shader>/dxbc/<n>.hlsl (skips ones already done).
Identical blobs (same sha1) are decompiled once and copied. Failures are listed in <work_dir>/decompile_failures.txt.
The decompiler (github.com/bo3b/3Dmigoto, cmd_Decompiler-1.3.16.zip) lives outside git in
~/vd-rip/third_party/cmd_decompiler (the zip ships the files as .ex/.dl; rename to .exe/.dll).
"""
import hashlib
import os
import shutil
import subprocess
import sys
from concurrent.futures import ThreadPoolExecutor

TOOL = os.path.expanduser('~/vd-rip/third_party/cmd_decompiler/cmd_Decompiler.exe')
BATCH = 40


def run_batch(files):
    env = dict(os.environ, WINEDEBUG='-all')
    try:
        p = subprocess.run(['wine', TOOL, '-D'] + files, env=env, capture_output=True, text=True, timeout=600)
        log = p.stdout + p.stderr
    except subprocess.TimeoutExpired:
        log = 'timeout'
    bad = [f for f in files if not os.path.exists(f[:-5] + '.hlsl')]
    return bad, log


def main():
    args = sys.argv[1:]
    work = args.pop(0)
    jobs = 4
    only = None
    while args:
        a = args.pop(0)
        if a == '--jobs':
            jobs = int(args.pop(0))
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
            if f.endswith('.dxbc') and not os.path.exists(os.path.join(dd, f[:-5] + '.hlsl')):
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
            still.append((f, log.strip().splitlines()[-1] if log.strip() else ''))
    for fs in by_hash.values():
        src = fs[0][:-5] + '.hlsl'
        if os.path.exists(src):
            for f in fs[1:]:
                shutil.copyfile(src, f[:-5] + '.hlsl')
    with open(os.path.join(work, 'decompile_failures.txt'), 'a') as out:
        for f, why in still:
            out.write('%s\t%s\n' % (f, why))
    print('failed', len(still), file=sys.stderr)


if __name__ == '__main__':
    main()
