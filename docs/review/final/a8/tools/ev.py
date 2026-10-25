import sys, os, numpy as np
sys.path.insert(0, os.path.dirname(__file__))
from corr import load
SFX='Assets/AssetBundles/remote_sound_assets_sfx/'
def best(wav, clip, t, pre=0.15, post=0.5, ln=0.25):
    w = load(wav); c = load(SFX + clip + '.ogg')
    c = c[:int(ln * 48000)]
    a = max(0, int((t - pre) * 48000)); b = min(len(w), int((t + post) * 48000) + len(c))
    seg = w[a:b]
    if len(seg) < len(c): return None
    n = 1 << (len(seg) + len(c)).bit_length()
    xc = np.fft.irfft(np.fft.rfft(seg, n) * np.conj(np.fft.rfft(c, n)), n)[:len(seg) - len(c) + 1]
    cs = np.concatenate([[0], np.cumsum(seg ** 2)]); e = cs[len(c):] - cs[:len(seg) - len(c) + 1]
    ncc = xc / (np.sqrt(e * (c ** 2).sum()) + 1e-12)
    k = int(np.argmax(ncc)); return round(float(ncc[k]), 3), round((a + k) / 48000 - t, 3), round(float(xc[k] / (c ** 2).sum()), 3)
W = sys.argv[1]
for spec in sys.argv[2:]:
    clip, t = spec.split('@')
    print(os.path.basename(W), clip, t, 'target', best(W, clip, float(t)), 'decoy DialogNext', best(W, 'DialogNext', float(t)), 'decoy Radio', best(W, 'Radio', float(t)))
