import subprocess, sys, numpy as np

def load(path, sr=48000):
    raw = subprocess.run(['ffmpeg', '-v', 'error', '-i', path, '-ac', '1', '-ar', str(sr), '-f', 'f32le', '-'],
                         capture_output=True, check=True).stdout
    return np.frombuffer(raw, dtype=np.float32).astype(np.float64)

def stats(x):
    rms = np.sqrt(np.mean(x ** 2)) if len(x) else 0
    return 20 * np.log10(rms + 1e-12), 20 * np.log10(np.max(np.abs(x)) + 1e-12), int(np.sum(np.abs(x) >= 0.999))

def corr(wav, clip, clip_off=0.0, clip_len=4.0, sr=48000):
    """Normalized cross-correlation of an excerpt of the source clip against the recording. Returns (peak ncc, lag s, gain)."""
    w = load(wav, sr); c = load(clip, sr)
    c = c[int(clip_off * sr):int((clip_off + clip_len) * sr)]
    n = len(w) + len(c)
    N = 1 << (n - 1).bit_length()
    X = np.fft.rfft(w, N); Y = np.fft.rfft(c, N)
    xc = np.fft.irfft(X * np.conj(Y), N)[:len(w) - len(c) + 1]
    # local energy of w
    cs = np.concatenate([[0], np.cumsum(w ** 2)])
    e = cs[len(c):] - cs[:len(w) - len(c) + 1]
    ncc = xc / (np.sqrt(e * np.sum(c ** 2)) + 1e-12)
    k = int(np.argmax(ncc))
    gain = xc[k] / np.sum(c ** 2)
    # noise floor: median abs ncc
    return float(ncc[k]), k / sr, float(gain), float(np.median(np.abs(ncc)))

if __name__ == '__main__':
    wav, clip = sys.argv[1], sys.argv[2]
    off = float(sys.argv[3]) if len(sys.argv) > 3 else 0.0
    ln = float(sys.argv[4]) if len(sys.argv) > 4 else 4.0
    print('wav', stats(load(wav)), 'clip', stats(load(clip)))
    print('ncc,lag,gain,median', corr(wav, clip, off, ln))

def spec(x, sr=48000, hop=480, win=2048, bands=40):
    n = (len(x) - win) // hop
    idx = np.arange(win)[None, :] + hop * np.arange(n)[:, None]
    fr = x[idx] * np.hanning(win)
    m = np.abs(np.fft.rfft(fr, axis=1)) ** 2
    edges = np.unique(np.geomspace(2, m.shape[1] - 1, bands + 1).astype(int))
    b = np.stack([m[:, a:c + 1].sum(1) for a, c in zip(edges[:-1], edges[1:])], 1)
    return np.log(b + 1e-9)

def corr2(wav, clip, clip_off=0.0, clip_len=2.0, sr=48000):
    """Spectral-shape match: slide the clip's log-band spectrogram over the recording's. Returns (best ncc, lag s, median ncc, p99 ncc)."""
    W = spec(load(wav, sr)); C = spec(load(clip, sr)[int(clip_off * sr):int((clip_off + clip_len) * sr)])
    n = len(C); out = []
    Cz = (C - C.mean(0)) / (C.std(0) + 1e-9)
    for k in range(len(W) - n + 1):
        w = W[k:k + n]; wz = (w - w.mean(0)) / (w.std(0) + 1e-9)
        out.append(float((wz * Cz).mean()))
    out = np.array(out)
    k = int(np.argmax(out))
    return float(out[k]), k * 0.01, float(np.median(out)), float(np.percentile(out, 99))
