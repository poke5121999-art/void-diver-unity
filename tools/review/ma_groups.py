#!/usr/bin/env python3
"""Dump the shipped MasterAudio setup (SoundManager.prefab): every sound group with its bus, group volume and variations
(clip file, AudioSource volume), and every playlist song. Usage: ma_groups.py [group-or-playlist-regex]
Output TSV: kind name bus groupVol variation clip srcVol extra. busIndex: 0 no bus, 1 new bus, n>=2 -> groupBuses[n-2]."""
import glob, os, re, sys

REPO = os.path.dirname(os.path.dirname(os.path.dirname(os.path.abspath(__file__))))
PREFAB = os.path.join(REPO, 'Assets/AssetBundles/dependencies_assets_prefab/SoundManager.prefab')
G_GROUP, G_VAR, G_MA = '448ad9ce542920a09241b674923c8ad9', 'e354d9517fe562a79d81932b1a3590a1', '333b29ff125ead6de596562115f1c2f1'


def docs(path):
    cur = None
    for line in open(path, encoding='utf-8'):
        m = re.match(r'--- !u!(\d+) &(-?\d+)', line)
        if m:
            if cur: yield cur
            cur = {'cls': int(m.group(1)), 'id': m.group(2), 'lines': []}
        elif cur is not None:
            cur['lines'].append(line.rstrip('\n'))
    if cur: yield cur


def field(d, name):
    for l in d['lines']:
        m = re.match(r'\s+' + name + r': (.*)$', l)
        if m: return m.group(1)
    return None


def clip_paths():
    out = {}
    for meta in glob.glob(os.path.join(REPO, 'Assets', '**', '*.meta'), recursive=True):
        if not re.search(r'\.(wav|ogg|mp3|aif|aiff)\.meta$', meta, re.I): continue
        for l in open(meta, encoding='utf-8', errors='ignore'):
            if l.startswith('guid:'):
                out[l.split()[1]] = os.path.relpath(meta[:-5], REPO); break
    return out


def main():
    pat = re.compile(sys.argv[1]) if len(sys.argv) > 1 else None
    clips = clip_paths()
    by_id, go_name, go_comps, parent = {}, {}, {}, {}
    for d in docs(PREFAB):
        by_id[d['id']] = d
        if d['cls'] == 1:
            go_name[d['id']] = field(d, 'm_Name')
    tr_go = {}
    for d in by_id.values():
        go = field(d, 'm_GameObject')
        if go:
            gid = re.search(r'fileID: (-?\d+)', go).group(1)
            go_comps.setdefault(gid, []).append(d)
            if d['cls'] == 4: tr_go[d['id']] = gid
    for d in by_id.values():
        if d['cls'] == 4:
            f = re.search(r'fileID: (-?\d+)', field(d, 'm_Father') or 'fileID: 0').group(1)
            parent[tr_go[d['id']]] = tr_go.get(f)
    ma = next(d for d in by_id.values() if d['cls'] == 114 and G_MA in (field(d, 'm_Script') or ''))
    buses = [l.split('busName: ')[1] for l in ma['lines'] if 'busName: ' in l]
    buses_vol = [l.split('volume: ')[1] for l in ma['lines'] if re.match(r'\s+volume: ', l)]

    def script(d): return field(d, 'm_Script') or ''
    rows = []
    for gid, comps in go_comps.items():
        grp = next((c for c in comps if c['cls'] == 114 and G_GROUP in script(c)), None)
        if not grp: continue
        name = go_name[gid]
        bi = int(field(grp, 'busIndex'))
        bus = buses[bi - 2] if bi >= 2 and bi - 2 < len(buses) else ('-' if bi == 0 else f'?{bi}')
        gv = field(grp, 'groupMasterVolume')
        extra = f"limitMode={field(grp, 'limitMode')} minBetween={field(grp, 'minimumTimeBetween')} voices={field(grp, 'voiceLimitCount') if field(grp, 'limitPolyphony') == '1' else '-'} blend={field(grp, 'spatialBlendType')}/{field(grp, 'spatialBlend')}"
        for vgid, p in parent.items():
            if p != gid: continue
            vc = go_comps.get(vgid, [])
            var = next((c for c in vc if c['cls'] == 114 and G_VAR in script(c)), None)
            src = next((c for c in vc if c['cls'] == 82), None)
            if not var: continue
            loc = field(var, 'audLocation')
            if loc == '2':
                g = None
                for i, l in enumerate(var['lines']):
                    if 'audioClipAddressable:' in l: g = var['lines'][i + 1].split('m_AssetGUID: ')[-1].strip()
                clip = clips.get(g, f'addr:{g}')
            else:
                clip = (re.search(r'guid: (\w+)', field(src, 'm_audioClip') or '') or [None, None])[1] if src else None
                clip = clips.get(clip, f'clip:{clip}') if loc == '0' else f"res:{field(var, 'resourceFileName')}"
            sv = field(src, 'm_Volume') if src else '?'
            rows.append(('group', name, bus, gv, go_name[vgid], clip, sv, extra + f" weight={field(var, 'weight')} randVol={field(var, 'useRandomVolume')}"))
    # playlists
    lines = ma['lines']
    pl = None
    for i, l in enumerate(lines):
        m = re.match(r'\s+playlistName: (.*)', l)
        if m: pl = m.group(1)
        m = re.match(r'\s+songName: (.*)', l)
        if m and pl:
            blk = lines[i:i + 20]
            g = next((b.split('m_AssetGUID:')[1].strip() for b in blk if 'm_AssetGUID' in b), None)
            if not g:  # audLocation 0: a direct clip reference instead of an Addressable
                g = next((re.search(r'guid: (\w+)', b).group(1) for b in lines[i - 3:i] if re.match(r'\s+clip: \{.*guid:', b)), None)
            vol = next((b.split('volume: ')[1] for b in blk if re.match(r'\s+volume: ', b)), '?')
            rows.append(('song', pl, 'music', '-', m.group(1), clips.get(g, f'addr:{g}'), vol, ''))
    for r in sorted(rows):
        if pat and not pat.search(r[1]): continue
        print('\t'.join(str(x) for x in r))
    print(f'# buses {list(zip(buses, buses_vol))}', file=sys.stderr)


if __name__ == '__main__':
    main()
