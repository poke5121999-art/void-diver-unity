"""Rewrite GUIDs in the files the repo owns (ProjectSettings, Assets/_VD) after build_project.py switched assets
to their original GUIDs. sync.sh only seeds those paths, so the build cannot rewrite them itself.

  python3 -I tools/rip/repo_guids.py ~/vd-rip/build_report.json
"""
import json, os, re, sys

REPO = os.path.dirname(os.path.dirname(os.path.dirname(os.path.abspath(__file__))))
switch = json.load(open(sys.argv[1])).get('original_guids', {})
GUID = re.compile(r'\b[0-9a-f]{32}\b')
changed = 0
for root in ('ProjectSettings', os.path.join('Assets', '_VD')):
    for dp, _, fs in os.walk(os.path.join(REPO, root)):
        for f in fs:
            p = os.path.join(dp, f)
            if f.endswith(('.png', '.jpg', '.dll')):
                continue
            try:
                text = open(p, encoding='utf-8').read()
            except UnicodeDecodeError:
                continue
            new = GUID.sub(lambda m: switch.get(m.group(0), m.group(0)), text)
            if new != text:
                open(p, 'w', encoding='utf-8').write(new)
                changed += 1
                print('rewrote', os.path.relpath(p, REPO))
print('REPO_GUIDS switched=%d files=%d' % (len(switch), changed))
