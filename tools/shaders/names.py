#!/usr/bin/env python3
"""List the shader names to rebuild: every AssetRipper dummy in the raw export that no package provides.

    names.py <raw export Assets dir> <donor dir> <repo> <out names.json> [--report report.json]

A dummy is a .shader containing //DummyShaderTextExporter (AssetRipper Free cannot decompile shaders). A name is left
to its package when a shader of that name exists in a donor package that tools/rip/build_project.py maps from
(packages.SHADER_DONORS) or in an embedded package under <repo>/Packages. The census list (captures/census.json, STUB
keys) only covers shaders that materials reference; shaders loaded by code (Shader.Find, post-processing) are dummies too.
"""
import json
import os
import re
import sys

SHADER_RE = re.compile(r'^\s*Shader\s+"([^"]+)"', re.M)


def shader_names(root, dummy=None, skip_tilde=False):
    out = {}
    for dp, dns, fs in os.walk(root, followlinks=True):
        if skip_tilde:  # Unity never imports a `name~` folder (package Samples~, Documentation~)
            dns[:] = [d for d in dns if not d.endswith('~')]
        for f in fs:
            if not f.endswith('.shader'):
                continue
            p = os.path.join(dp, f)
            text = open(p, encoding='utf-8', errors='replace').read()
            if dummy is not None and ('DummyShaderTextExporter' in text) != dummy:
                continue
            m = SHADER_RE.search(text)
            if m:
                out.setdefault(m.group(1), p)
    return out


def main():
    a = sys.argv[1:]
    export_assets, donor, repo, out = a[:4]
    report = a[a.index('--report') + 1] if '--report' in a else None
    sys.path.insert(0, os.path.join(repo, 'tools', 'rip'))
    from packages import SHADER_DONORS
    dummies = shader_names(export_assets, dummy=True)
    provided = {}
    for d in SHADER_DONORS:
        for n, p in shader_names(os.path.join(donor, d)).items():
            provided.setdefault(n, p)
    for n, p in shader_names(os.path.join(repo, 'Packages'), skip_tilde=True).items():
        provided.setdefault(n, p)
    names = sorted(n for n in dummies if n not in provided)
    left = {n: os.path.relpath(provided[n], repo if provided[n].startswith(repo) else donor) for n in sorted(dummies) if n in provided}
    json.dump(names, open(out, 'w'), indent=0)
    if report:
        json.dump({'dummies': len(dummies), 'rebuild': len(names), 'providedByPackage': left}, open(report, 'w'), indent=1)
    print('dummy shader names %d, provided by a package %d, to rebuild %d' % (len(dummies), len(left), len(names)))


if __name__ == '__main__':
    main()
