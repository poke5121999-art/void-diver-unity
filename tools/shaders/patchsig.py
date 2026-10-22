#!/usr/bin/env python3
"""Merge packed interpolator registers in a DXBC signature before decompiling.

fxc packs several varyings into one register (TEXCOORD0 in .xy, TEXCOORD1 in .z, TEXCOORD2 in .w). 3Dmigoto's
decompiler names the extra elements w<reg>/p<reg> (colliding from the third element on) and gets their relative
swizzles wrong. Interpolators only need to agree between the vertex output and the pixel input, so every register
of a VS output / PS input signature that holds more than one element is replaced by a single full-register element
with the synthetic semantic VDREG<register>. Vertex inputs (mesh streams), system values and PS outputs are untouched.

    patchsig.py <in.dxbc> <out.dxbc>      (also importable: patch(bytes) -> (bytes, info))

The container checksum is recomputed (DXBC's modified MD5, as in 3Dmigoto's DXBCChecksum.cpp).
"""
import struct
import sys

MASK_XYZW = 0xF


# ---------------------------------------------------------------- DXBC checksum (modified MD5)

def _md5_transform(state, block):
    S = [7, 12, 17, 22] * 4 + [5, 9, 14, 20] * 4 + [4, 11, 16, 23] * 4 + [6, 10, 15, 21] * 4
    K = [int(abs(__import__('math').sin(i + 1)) * 2 ** 32) & 0xFFFFFFFF for i in range(64)]
    M = struct.unpack('<16I', block)
    a, b, c, d = state
    for i in range(64):
        if i < 16:
            f = (b & c) | (~b & d)
            g = i
        elif i < 32:
            f = (d & b) | (~d & c)
            g = (5 * i + 1) % 16
        elif i < 48:
            f = b ^ c ^ d
            g = (3 * i + 5) % 16
        else:
            f = c ^ (b | ~d)
            g = (7 * i) % 16
        f = (f + a + K[i] + M[g]) & 0xFFFFFFFF
        a, d, c, b = d, c, b, (b + ((f << S[i]) | (f >> (32 - S[i]))) & 0xFFFFFFFF) & 0xFFFFFFFF
    return [(x + y) & 0xFFFFFFFF for x, y in zip(state, (a, b, c, d))]


def dxbc_checksum(data):
    """Hash of the container body (after the 20-byte header), DXBC's variant of MD5 without standard finalization."""
    body = data[20:]
    state = [0x67452301, 0xEFCDAB89, 0x98BADCFE, 0x10325476]
    nbits = (len(body) * 8) & 0xFFFFFFFF
    full = len(body) & ~63
    for i in range(0, full, 64):
        state = _md5_transform(state, body[i:i + 64])
    last = body[full:]
    pad = b'\x80' + b'\x00' * 63
    if len(last) >= 56:
        state = _md5_transform(state, last + pad[:64 - len(last)])
        block = struct.pack('<I', nbits) + b'\x00' * 56 + struct.pack('<I', (nbits >> 2) | 1)
        state = _md5_transform(state, block)
    else:
        block = struct.pack('<I', nbits) + last + pad[:64 - 4 - len(last) - 4] + struct.pack('<I', (nbits >> 2) | 1)
        state = _md5_transform(state, block)
    return struct.pack('<4I', *state)


# ---------------------------------------------------------------- container

def parse_container(data):
    assert data[:4] == b'DXBC'
    total = struct.unpack_from('<I', data, 24)[0]
    n = struct.unpack_from('<I', data, 28)[0]
    chunks = []
    for i in range(n):
        off = struct.unpack_from('<I', data, 32 + 4 * i)[0]
        fourcc = data[off:off + 4]
        size = struct.unpack_from('<I', data, off + 4)[0]
        chunks.append((fourcc, data[off + 8:off + 8 + size]))
    return chunks, total


def build_container(chunks):
    header_size = 32 + 4 * len(chunks)
    body = b''
    offsets = []
    for fourcc, payload in chunks:
        offsets.append(header_size + len(body))
        body += fourcc + struct.pack('<I', len(payload)) + payload
    total = header_size + len(body)
    out = b'DXBC' + b'\x00' * 16 + struct.pack('<III', 1, total, len(chunks)) + b''.join(struct.pack('<I', o) for o in offsets) + body
    return out[:4] + dxbc_checksum(out) + out[20:]


def shader_stage(chunks):
    for fourcc, payload in chunks:
        if fourcc in (b'SHDR', b'SHEX'):
            v = struct.unpack_from('<I', payload, 0)[0]
            return v >> 16  # 0 pixel, 1 vertex, 2 geometry, 3 hull, 4 domain, 5 compute
    return None


# ---------------------------------------------------------------- signatures

def parse_signature(fourcc, payload):
    count, _ = struct.unpack_from('<II', payload, 0)
    entry_size = 32 if fourcc in (b'ISG1', b'OSG1', b'PSG1') else 24
    base = 8
    elems = []
    for i in range(count):
        off = base + i * entry_size
        if entry_size == 32:
            stream, name_off, sem_idx, sysval, comp, reg, mask, rw, minprec = struct.unpack_from('<IIIIIIBBxxI', payload, off)
        else:
            name_off, sem_idx, sysval, comp, reg, mask, rw = struct.unpack_from('<IIIIIBBxx', payload, off)
            stream, minprec = 0, 0
        end = payload.index(b'\x00', name_off)
        name = payload[name_off:end].decode('ascii', errors='replace')
        elems.append({'stream': stream, 'name': name, 'index': sem_idx, 'sysval': sysval, 'comp': comp, 'reg': reg,
                      'mask': mask, 'rw': rw, 'minprec': minprec})
    return elems, entry_size


def build_signature(elems, entry_size):
    base = 8
    strings = b''
    name_offs = []
    table_start = base + entry_size * len(elems)
    for e in elems:
        name_offs.append(table_start + len(strings))
        strings += e['name'].encode('ascii') + b'\x00'
    while (table_start + len(strings)) % 4:
        strings += b'\x00'
    out = struct.pack('<II', len(elems), base)
    for e, no in zip(elems, name_offs):
        if entry_size == 32:
            out += struct.pack('<IIIIIIBBxxI', e['stream'], no, e['index'], e['sysval'], e['comp'], e['reg'], e['mask'], e['rw'], e['minprec'])
        else:
            out += struct.pack('<IIIIIBBxx', no, e['index'], e['sysval'], e['comp'], e['reg'], e['mask'], e['rw'])
    return out + strings


def merge_registers(elems):
    """Replace multi-element registers (no system values, one component type) by one VDREG<reg> element."""
    by_reg = {}
    for e in elems:
        by_reg.setdefault((e['stream'], e['reg']), []).append(e)
    out = []
    merged = []
    for e in elems:
        group = by_reg[(e['stream'], e['reg'])]
        if len(group) == 1:
            out.append(e)
            continue
        if e is not group[0]:
            continue
        if any(g['sysval'] != 0 for g in group) or len({g['comp'] for g in group}) != 1:
            out.extend(group)  # cannot merge; the decompiler may still collide, reported by validation
            continue
        rw = 0
        for g in group:
            rw |= g['rw']
        out.append({'stream': e['stream'], 'name': 'VDREG', 'index': e['reg'], 'sysval': 0, 'comp': e['comp'], 'reg': e['reg'],
                    'mask': MASK_XYZW, 'rw': rw if rw else MASK_XYZW, 'minprec': e['minprec']})
        merged.append((e['reg'], [(g['name'], g['index'], g['mask']) for g in group]))
    return out, merged


def patch(data):
    chunks, _ = parse_container(data)
    stage = shader_stage(chunks)
    target = {1: (b'OSGN', b'OSG1'), 0: (b'ISGN', b'ISG1')}.get(stage)
    info = {'stage': stage, 'merged': [], 'elements': {}}
    if target is None:
        return data, info
    new_chunks = []
    changed = False
    for fourcc, payload in chunks:
        if fourcc in target:
            elems, entry_size = parse_signature(fourcc, payload)
            info['elements'][fourcc.decode()] = [(e['name'], e['index'], e['reg'], e['mask']) for e in elems]
            new, merged = merge_registers(elems)
            if merged:
                payload = build_signature(new, entry_size)
                info['merged'] = merged
                changed = True
        new_chunks.append((fourcc, payload))
    if not changed:
        return data, info
    return build_container(new_chunks), info


def selftest(data):
    chunks, _ = parse_container(data)
    rebuilt = build_container(chunks)
    return rebuilt == data


if __name__ == '__main__':
    src = open(sys.argv[1], 'rb').read()
    if len(sys.argv) == 2:
        print('checksum roundtrip', selftest(src))
        sys.exit(0)
    out, info = patch(src)
    open(sys.argv[2], 'wb').write(out)
    print(info)
