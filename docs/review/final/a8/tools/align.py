import sys, glob, os, numpy as np
sys.path.insert(0, os.path.dirname(__file__))
from corr import load
SR=24000
def bands(x, hop=240, win=2048, nb=24, lo=60, hi=3500):
    n=(len(x)-win)//hop
    idx=np.arange(win)[None,:]+hop*np.arange(n)[:,None]
    m=np.abs(np.fft.rfft(x[idx]*np.hanning(win),axis=1))**2
    f=np.fft.rfftfreq(win,1/SR); e=np.geomspace(lo,hi,nb+1)
    b=np.stack([m[:,(f>=a)&(f<c)].sum(1) for a,c in zip(e[:-1],e[1:])],1)
    return np.log(b+1e-10)
def score(W,C):
    n=len(W); L=len(C)-n+1
    if L<1: return -1,0
    tot=np.zeros(L)
    for b in range(W.shape[1]):
        w=W[:,b]-W[:,b].mean(); w/= (np.sqrt((w**2).sum())+1e-9)
        c=C[:,b]; N=1<<(len(c)+n).bit_length()
        xc=np.fft.irfft(np.fft.rfft(c,N)*np.conj(np.fft.rfft(w,N)),N)[:L]
        cs=np.concatenate([[0],np.cumsum(c)]); cs2=np.concatenate([[0],np.cumsum(c**2)])
        s=cs[n:]-cs[:L]; s2=cs2[n:]-cs2[:L]
        var=s2-s*s/n
        tot+=xc/(np.sqrt(np.maximum(var,1e-9)))
    tot/=W.shape[1]; k=int(np.argmax(tot)); return float(tot[k]),k*0.01
def run(wav,folder,t0,t1,topn=4):
    x=load(wav,SR); w=x[int(t0*SR):int(t1*SR)]; W=bands(w)
    res=[]
    for f in glob.glob(folder+'/*.ogg'):
        c=load(f,SR); res.append((*score(W,bands(c)),os.path.basename(f)))
    res.sort(reverse=True); return [(round(a,3),round(b,1),n) for a,b,n in res[:topn]], res
if __name__=='__main__':
    top,res=run(sys.argv[1],sys.argv[2],float(sys.argv[3]),float(sys.argv[4]))
    print(top)
    if len(sys.argv)>5:
        print('target rank',[i for i,r in enumerate(res) if r[2].startswith(sys.argv[5])])
