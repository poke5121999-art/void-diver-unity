#!/usr/bin/env python3
"""Point URP's default volume profile at the game's DefaultVolumeProfile (rerunnable).

The shipped UniversalRenderPipelineGlobalSettings references DefaultVolumeProfile from its
URPDefaultVolumeProfileSettings (a [SerializeReference] entry); the export keeps the entry but not the reference,
so the volume stack started from the components' C# defaults with nothing overridden instead of the game's base
profile, which overrides every parameter of every component (URP and the game's own volume components).

  python3 -I tools/rip/urp_default_profile.py
"""
import os, re, sys

REPO = os.path.dirname(os.path.dirname(os.path.dirname(os.path.abspath(__file__))))
SETTINGS = os.path.join(REPO, 'Assets', 'MonoBehaviour', 'UniversalRenderPipelineGlobalSettings.asset')
PROFILE_META = os.path.join(REPO, 'Assets', 'MonoBehaviour', 'DefaultVolumeProfile.asset.meta')
guid = re.search(r'guid: ([0-9a-f]{32})', open(PROFILE_META).read()).group(1)
text = open(SETTINGS).read()
pat = re.compile(r'(type: \{class: URPDefaultVolumeProfileSettings, [^\n]*\n      data:\n        m_Version: \d+\n        m_VolumeProfile: )\{[^}]*\}')
if not pat.search(text):
    sys.exit('URPDefaultVolumeProfileSettings entry not found in %s' % SETTINGS)
new = pat.sub(lambda m: m.group(1) + '{fileID: 11400000, guid: %s, type: 2}' % guid, text)
if new != text:
    open(SETTINGS, 'w').write(new)
print('URP_DEFAULT_PROFILE %s -> DefaultVolumeProfile %s' % ('set' if new != text else 'unchanged', guid))
