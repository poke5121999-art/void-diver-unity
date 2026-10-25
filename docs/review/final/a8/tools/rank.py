import sys, glob, os, numpy as np
sys.path.insert(0, os.path.dirname(__file__))
from corr import load
def ltas(x, win=4096):
    n = (len(x) - win) // win
    if n < 1: return None
    fr = x[:n * win].reshape(n, win) * np.hanning(win)
    m = (np.abs(np.fft.rfft(fr, axis=1)) ** 2).mean(0)
    edges = np.unique(np.geomspace(4, len(m) - 1, 60).astype(int))
    b = np.array([m[a:c + 1].sum() for a, c in zip(edges[:-1], edges[1:])])
    return np.log(b + 1e-12)
def rank(wav, folder, t0=0.0, t1=None, top=4, lag=0.0):
    w = load(wav); w = w[int(t0 * 48000):int((t1 or len(w) / 48000) * 48000)]
    lw = ltas(w); res = []
    for f in glob.glob(folder + '/*.ogg'):
        c = load(f)[int(lag * 48000):]
        c = c[:len(w)]
        lc = ltas(c)
        if lc is None: continue
        a = lw - lw.mean(); b = lc - lc.mean()
        res.append((float((a * b).sum() / np.sqrt((a * a).sum() * (b * b).sum())), os.path.basename(f)))
    res.sort(reverse=True); return res[:top], len(res)
if __name__ == '__main__':
    print(rank(sys.argv[1], sys.argv[2], float(sys.argv[3]), float(sys.argv[4]) if len(sys.argv) > 4 else None))
