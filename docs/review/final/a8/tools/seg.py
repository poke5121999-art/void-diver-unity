import re,sys
rows=[]
for l in open(sys.argv[1],errors='replace'):
    m=re.match(r'VD_A4 t=([\d.]+) scene=(\S+) audio (\S+) mode=\w+ secs=([\d.]+).*?rms=(\S+)dBFS peak=(\S+)dBFS.*?clipped=(\d+).*?file=(\S+) clips=\[(.*)\]\s*$',l)
    if not m: continue
    t,sc,name,secs,rms,pk,cl,f,clips=m.groups()
    items=[]
    for c in clips.split(' | '):
        g=re.match(r'([\d.]+)s (\w+) group=(\S+) bus=(\S+) busVol=\S+ groupVol=\S+ var=\S+ clip=(\S+) src=(\S+) mixer=(\S+)',c)
        if g: items.append(g.groups())
    rows.append((name,secs,rms,pk,cl,items))
for name,secs,rms,pk,cl,items in rows:
    print(f'## {name} secs={secs} rms={rms} peak={pk} clipped={cl} nclips={len(items)}')
    if '-v' in sys.argv:
        for i in items: print('   ',i[0],i[1],i[2],i[5])
