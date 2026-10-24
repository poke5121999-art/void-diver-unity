#!/usr/bin/env python3
"""Write ExposedReference names the way the editor reads them (rerunnable).

A built game keeps a PropertyName as its int hash; AssetRipper exports an ExposedReference as
    exposedName:
      id: 1162937878
but the editor serializes PropertyName as a plain string (`exposedName: def55ea2...`) and reads the `id:` form as an
empty name. A ControlPlayableAsset then resolves no sourceGameObject and does nothing: the campaign detail background
(TL_CampaignDetailBackgroundTimeline_master) never turns its theme diorama on and the panel shows only fog and rings.
The PlayableDirector side (`m_ExposedReferences.m_References: - 1162937878: {fileID: ...}`) is already a string key, so
writing the same number as the name makes both sides one PropertyName again. The original strings are lost (only the
hash ships); any string works as long as both sides match.

  python3 -I tools/rip/exposed_names.py [--dry-run]
"""
import os, re, subprocess, sys

REPO = os.path.dirname(os.path.dirname(os.path.dirname(os.path.abspath(__file__))))
DRY = '--dry-run' in sys.argv
PAT = re.compile(r'^(?P<i>[ \t]*)exposedName:\n[ \t]+id: (?P<id>-?\d+)\n', re.M)

out = subprocess.run(['grep', '-rlE', '--include=*.playable', '--include=*.asset', '--include=*.prefab', '--include=*.unity',
                      '^[[:space:]]*exposedName:$', os.path.join(REPO, 'Assets')], capture_output=True, text=True).stdout
files = refs = 0
for path in out.split():
    text = open(path, encoding='utf-8').read()
    new, n = PAT.subn(lambda m: f"{m['i']}exposedName: {m['id']}\n", text)
    if not n:
        continue
    files += 1
    refs += n
    if not DRY:
        open(path, 'w', encoding='utf-8').write(new)
print(f"exposed_names: {refs} names in {files} files{' (dry run)' if DRY else ''}")
