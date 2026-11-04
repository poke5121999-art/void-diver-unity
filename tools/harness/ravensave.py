# ravensave.py in out: the post-tutorial save with Profile.SelectedCharacterId (field 6 -> 3) = 100005 (Raven).
import sys, importlib.util
spec = importlib.util.spec_from_file_location('pb', sys.argv[3]); pb = importlib.util.module_from_spec(spec)
src = open(sys.argv[3]).read().split('b=open(sys.argv[1]')[0]; exec(src, pb.__dict__)
b = open(sys.argv[1], 'rb').read(); top = pb.parse(b)
prof = pb.parse([x for x in top if x[0] == 6][0][2]); prof = pb.setv(prof, 3, 100005)
top = [(f, w, pb.ser(prof) if f == 6 else v) for f, w, v in top]
open(sys.argv[2], 'wb').write(pb.ser(top)); print('profile', [(f, v if w == 0 else len(v)) for f, w, v in prof])
