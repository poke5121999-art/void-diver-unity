# pbedit.py in out: post-tutorial PlayerContext_0.sav -> reputation-ready save (exp 2000, coin 20,
# campaign 1101 cleared = ClearedCampaignFlags bit 39, Owner unlocked = NpcUnlockedFlags bit 3).
import sys
def rv(b,i):
    r=s=0
    while True:
        c=b[i]; i+=1; r|=(c&0x7f)<<s; s+=7
        if c<0x80: return r,i
def wv(n):
    o=bytearray()
    while True:
        c=n&0x7f; n>>=7
        if n: o.append(c|0x80)
        else: o.append(c); return bytes(o)
def parse(b):
    i=0; out=[]
    while i<len(b):
        k,i=rv(b,i); f,w=k>>3,k&7
        if w==0: v,i=rv(b,i); out.append((f,w,v))
        elif w==2: l,i=rv(b,i); out.append((f,w,b[i:i+l])); i+=l
        elif w==1: out.append((f,w,b[i:i+8])); i+=8
        elif w==5: out.append((f,w,b[i:i+4])); i+=4
        else: raise SystemExit('wire %d'%w)
    return out
def ser(fs):
    o=bytearray()
    for f,w,v in fs:
        o+=wv(f<<3|w)
        if w==0: o+=wv(v)
        elif w==2: o+=wv(len(v))+v
        else: o+=v
    return bytes(o)
def setv(fs,f,v):
    fs=[x for x in fs if x[0]!=f]; fs.append((f,0,v)); return sorted(fs,key=lambda x:x[0])
def orbit(fs,f,bit):
    sub=[x for x in fs if x[0]==f]
    flags=[]
    if sub:
        for g,w,v in parse(sub[0][2]):
            if g==1 and w==0: flags.append(v)
            elif g==1 and w==2:
                j=0
                while j<len(v): x,j=rv(v,j); flags.append(x)
    while len(flags)<=bit//64: flags.append(0)
    flags[bit//64]|=1<<(bit%64)
    body=b''.join(wv(1<<3|0)+wv(x) for x in flags)
    fs=[x for x in fs if x[0]!=f]; fs.append((f,2,body)); return sorted(fs,key=lambda x:x[0]), flags
b=open(sys.argv[1],'rb').read(); top=parse(b)
prof=[x for x in top if x[0]==6][0][2]; p=parse(prof)
p=setv(p,7,2000); p=setv(p,10,20)
top=[(f,w,ser(p) if f==6 else v) for f,w,v in top]
top,cf=orbit(top,16,39); top,nf=orbit(top,28,3); top,qf=orbit(top,25,1)
open(sys.argv[2],'wb').write(ser(top))
print('profile',[(f,v if w==0 else len(v)) for f,w,v in p],'campaignFlags',[hex(x) for x in cf],'npcFlags',[hex(x) for x in nf])
