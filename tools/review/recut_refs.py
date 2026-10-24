#!/usr/bin/env python3
"""Re-cut docs/review/ref/<key>/<NN>.jpg from the downloaded clips, decoding colour as the clips are tagged.

The clips are BT.709 limited range. ffmpeg's default RGB conversion for a .jpg output decodes with BT.601,
which shifts saturated colours (DECODING trap 39). Each frame is re-cut at the second in SOURCES.tsv with an
explicit bt709 input matrix and a full-range RGB output, then written as a standard (BT.601) JFIF JPEG.

Usage: recut_refs.py [--clips ~/vd-rip/clips] [--check]
  --check  only compare the new decode with the existing jpg (mean abs diff per channel), write nothing.
"""
import argparse
import csv
import os
import shutil
import re
import subprocess
import sys
import tempfile

REPO = os.path.dirname(os.path.dirname(os.path.dirname(os.path.abspath(__file__))))
REF = os.path.join(REPO, "docs", "review", "ref")


def clip_path(clips, url):
    vid = re.search(r"youtu\.be/([^?]+)", url).group(1)
    return os.path.join(clips, vid, vid + ".mp4")


def cut(src, seconds, out):
    # Decode to RGB in ffmpeg (bt709, limited range), then let PIL write the JPEG: a JFIF reader assumes BT.601
    # full-range YCbCr, and ffmpeg's own mjpeg path would hand it bt709 coefficients.
    png = out + ".png"
    vf = "scale=in_color_matrix=bt709:in_range=tv:out_range=pc,format=rgb24"
    subprocess.run(["timeout", "120", "ffmpeg", "-v", "error", "-y", "-ss", str(seconds), "-i", src,
                    "-frames:v", "1", "-vf", vf, png], check=True)
    from PIL import Image
    Image.open(png).convert("RGB").save(out, quality=95, subsampling=0)


def mean_diff(a, b):
    from PIL import Image, ImageChops, ImageStat
    x = Image.open(a).convert("RGB")
    y = Image.open(b).convert("RGB").resize(x.size)
    return [round(v, 2) for v in ImageStat.Stat(ImageChops.difference(x, y)).mean]


def main():
    ap = argparse.ArgumentParser()
    ap.add_argument("--clips", default=os.path.expanduser("~/vd-rip/clips"))
    ap.add_argument("--check", action="store_true")
    args = ap.parse_args()
    rows = list(csv.DictReader(open(os.path.join(REF, "SOURCES.tsv")), delimiter="\t"))
    bad = 0
    with tempfile.TemporaryDirectory() as tmp:
        for r in rows:
            dst = os.path.join(REF, r["key"], r["frame"] + ".jpg")
            new = os.path.join(tmp, "f.jpg")
            cut(clip_path(args.clips, r["url"]), r["seconds"], new)
            d = mean_diff(dst, new) if os.path.exists(dst) else None
            # A wrong second shows as a large diff; a colour-matrix change alone stays small.
            flag = "" if d is None or max(d) < 12 else "  DIFFERENT-FRAME?"
            bad += bool(flag)
            print(f"{r['key']}/{r['frame']} t={r['seconds']} meanabs={d}{flag}")
            if not args.check and not flag:
                shutil.move(new, dst)
    print(f"rows={len(rows)} suspicious={bad}" + (" (check only)" if args.check else ""))
    return 1 if bad else 0


if __name__ == "__main__":
    sys.exit(main())
