# findlabel.py <template.png> <shot.png>: finds the NPC name label (white text) in a game screenshot.
# Prints "x y score" (template top-left in shot pixels, score 0..1) of the best match, or "none".
import sys
from PIL import Image
T = int(sys.argv[3]) if len(sys.argv) > 3 else 150
tpl = Image.open(sys.argv[1]).convert('L'); shot = Image.open(sys.argv[2]).convert('L')
tw, th = tpl.size; sw, sh = shot.size
tp = tpl.load(); on = [(x, y) for y in range(th) for x in range(tw) if tp[x, y] > T]
off = [(x, y) for y in range(th) for x in range(tw) if tp[x, y] < 90][::3]
sp = shot.load()
ax, ay = on[0]
best = (0, None)
for y in range(sh):
    for x in range(sw):
        if sp[x, y] <= T: continue
        ox, oy = x - ax, y - ay
        if ox < 0 or oy < 0 or ox + tw > sw or oy + th > sh: continue
        hit = 0
        for (px, py) in on:
            if sp[ox + px, oy + py] > T: hit += 1
        if hit < len(on) * 0.6: continue
        bad = sum(1 for (px, py) in off if sp[ox + px, oy + py] > T)
        score = hit / len(on) - bad / max(1, len(off))
        if score > best[0]: best = (score, (ox, oy))
print('none' if best[1] is None or best[0] < 0.55 else '%d %d %.2f' % (best[1][0], best[1][1], best[0]))
