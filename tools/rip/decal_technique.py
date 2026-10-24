#!/usr/bin/env python3
"""Pin URP's decal technique to DBuffer in the game's DecalRendererFeature assets (rerunnable).

The game ships its decal Shader Graphs (Shader_Decoration_Puddle...) with the DBufferProjector/DBufferMesh passes only:
it runs on D3D11, where the shipped setting `technique: Automatic` resolves to DBuffer. On OpenGL (the Linux editor)
Automatic resolves to ScreenSpace, the decals have no DecalScreenSpaceProjector pass, and URP's DecalDrawErrorSystem
draws every such projector box with its magenta error material (the ground near the dive map's boundary). DBuffer is
what Automatic already picks on the game's platforms, so D3D/Vulkan render the same; on OpenGL URP refuses DBuffer
and draws no decals instead of error boxes.

  python3 -I tools/rip/decal_technique.py
"""
import glob, os, re

REPO = os.path.dirname(os.path.dirname(os.path.dirname(os.path.abspath(__file__))))
SCRIPT_GUID = 'a1614fc811f8f184697d9bee70ab9fe5'  # UnityEngine.Rendering.Universal.DecalRendererFeature
changed = seen = 0
for path in glob.glob(os.path.join(REPO, 'Assets', 'MonoBehaviour', '*.asset')):
    text = open(path, encoding='utf-8').read()
    if 'guid: %s' % SCRIPT_GUID not in text:
        continue
    seen += 1
    new = re.sub(r'(\n  m_Settings:\n    technique: )0\n', r'\g<1>1\n', text)
    if new != text:
        open(path, 'w', encoding='utf-8', newline='\n').write(new)
        changed += 1
print('DECAL_TECHNIQUE features=%d set_to_dbuffer=%d' % (seen, changed))
