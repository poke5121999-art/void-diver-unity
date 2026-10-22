"""fileID Unity gives a MonoBehaviour class compiled into a DLL: first 4 bytes (little endian, signed) of
MD4("s\\0\\0\\0" + namespace + class name). Pure-python MD4 because OpenSSL 3 dropped it."""
import struct


def _md4(data: bytes) -> bytes:
    def f(x, y, z): return (x & y) | (~x & z)
    def g(x, y, z): return (x & y) | (x & z) | (y & z)
    def h(x, y, z): return x ^ y ^ z
    def rot(x, n): x &= 0xffffffff; return ((x << n) | (x >> (32 - n))) & 0xffffffff
    msg = data + b'\x80' + b'\x00' * ((55 - len(data)) % 64) + struct.pack('<Q', len(data) * 8)
    a, b, c, d = 0x67452301, 0xefcdab89, 0x98badcfe, 0x10325476
    for off in range(0, len(msg), 64):
        x = struct.unpack('<16I', msg[off:off + 64])
        aa, bb, cc, dd = a, b, c, d
        for i in range(16):
            k, s = i, (3, 7, 11, 19)[i % 4]
            if i % 4 == 0: a = rot(a + f(b, c, d) + x[k], s)
            elif i % 4 == 1: d = rot(d + f(a, b, c) + x[k], s)
            elif i % 4 == 2: c = rot(c + f(d, a, b) + x[k], s)
            else: b = rot(b + f(c, d, a) + x[k], s)
        for i in range(16):
            k, s = (0, 4, 8, 12, 1, 5, 9, 13, 2, 6, 10, 14, 3, 7, 11, 15)[i], (3, 5, 9, 13)[i % 4]
            t = 0x5a827999
            if i % 4 == 0: a = rot(a + g(b, c, d) + x[k] + t, s)
            elif i % 4 == 1: d = rot(d + g(a, b, c) + x[k] + t, s)
            elif i % 4 == 2: c = rot(c + g(d, a, b) + x[k] + t, s)
            else: b = rot(b + g(c, d, a) + x[k] + t, s)
        for i in range(16):
            k, s = (0, 8, 4, 12, 2, 10, 6, 14, 1, 9, 5, 13, 3, 11, 7, 15)[i], (3, 9, 11, 15)[i % 4]
            t = 0x6ed9eba1
            if i % 4 == 0: a = rot(a + h(b, c, d) + x[k] + t, s)
            elif i % 4 == 1: d = rot(d + h(a, b, c) + x[k] + t, s)
            elif i % 4 == 2: c = rot(c + h(d, a, b) + x[k] + t, s)
            else: b = rot(b + h(c, d, a) + x[k] + t, s)
        a, b, c, d = (a + aa) & 0xffffffff, (b + bb) & 0xffffffff, (c + cc) & 0xffffffff, (d + dd) & 0xffffffff
    return struct.pack('<4I', a, b, c, d)


def dll_script_file_id(namespace: str, name: str) -> int:
    return struct.unpack('<i', _md4(('s\0\0\0' + namespace + name).encode())[:4])[0]


if __name__ == '__main__':
    assert _md4(b'').hex() == '31d6cfe0d16ae931b73c59d7e0c089c0'
    assert _md4(b'abc').hex() == 'a448017aaf21d8525fc10ae87aa6729d'
    print(dll_script_file_id('DG.Tweening.Core', 'DOTweenSettings'))
