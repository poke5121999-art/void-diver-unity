import sys, os, numpy as np
sys.path.insert(0, os.path.dirname(__file__))
from corr import load
SFX='Assets/AssetBundles/remote_sound_assets_sfx/'
def hp(x, fc=3000, sr=48000):
    X = np.fft.rfft(x); f = np.fft.rfftfreq(len(x), 1/sr); X[f < fc] = 0; return np.fft.irfft(X, len(x))
def peak(seg, c):
    n = 1 << (len(seg) + len(c)).bit_length()
    xc = np.fft.irfft(np.fft.rfft(seg, n) * np.conj(np.fft.rfft(c, n)), n)[:len(seg) - len(c) + 1]
    cs = np.concatenate([[0], np.cumsum(seg ** 2)]); e = cs[len(c):] - cs[:len(seg) - len(c) + 1]
    ncc = xc / (np.sqrt(e * (c ** 2).sum()) + 1e-12); k = int(np.argmax(ncc)); return float(ncc[k]), k
def run(wav, clip, times, null_times, fc=3000, ln=0.28):
    w = hp(load(wav), fc); c = hp(load(SFX + clip + '.ogg')[:int(ln * 48000)], fc)
    def at(t):
        a = max(0, int((t - 0.05) * 48000)); seg = w[a:a + int((0.2) * 48000) + len(c)]
        return peak(seg, c) if len(seg) > len(c) else (None, None)
    ev = [(t,) + tuple(round(v, 3) if v is not None and isinstance(v, float) else v for v in at(t)) for t in times]
    nl = [at(t)[0] for t in null_times]; nl = [v for v in nl if v is not None]
    return ev, (round(float(np.mean(nl)), 3), round(float(np.max(nl)), 3)) if nl else None
if __name__ == '__main__':
    wav, clip = sys.argv[1], sys.argv[2]
    times = [float(x) for x in sys.argv[3].split(',')]; nulls = [float(x) for x in sys.argv[4].split(',')]
    for fc in (1500, 3000, 6000):
        print(clip, 'hp', fc, run(wav, clip, times, nulls, fc))
