import sys, glob, os, numpy as np
sys.path.insert(0, os.path.dirname(__file__))
from corr import load
def lt(fr):
    m = (np.abs(np.fft.rfft(fr * np.hanning(fr.shape[1]), axis=1)) ** 2).mean(0)
    e = np.unique(np.geomspace(4, len(m) - 1, 60).astype(int))
    return np.log(np.array([m[a:c + 1].sum() for a, c in zip(e[:-1], e[1:])]) + 1e-12)
def frames(x, win=4096):
    n = len(x) // win; return x[:n * win].reshape(n, win)
wav, folder, t0, t1, q = sys.argv[1], sys.argv[2], float(sys.argv[3]), float(sys.argv[4]), float(sys.argv[5])
w = load(wav)[int(t0 * 48000):int(t1 * 48000)]; fr = frames(w); en = (fr ** 2).mean(1)
lw = lt(fr[en <= np.quantile(en, q)]); res = []
for f in glob.glob(folder + '/*.ogg'):
    c = load(f)[:int(30 * 48000)]; lc = lt(frames(c)); a = lw - lw.mean(); b = lc - lc.mean()
    res.append((round(float((a * b).sum() / np.sqrt((a * a).sum() * (b * b).sum())), 3), os.path.basename(f)))
res.sort(reverse=True); print(res[:5]); print([i for i, r in enumerate(res) if r[1].startswith(sys.argv[6])], len(res))
