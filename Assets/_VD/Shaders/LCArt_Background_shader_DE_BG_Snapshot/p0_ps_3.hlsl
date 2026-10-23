// LCArt/Background/shader_DE_BG_Snapshot | pass 0 "ForwardLit" | progFragment | keywords: (none) | blob 3
void frag(
  float4 v0 : SV_POSITION0,
  float4 v1 : VDREG1,
  float4 v2 : VDREG2,
  float4 v3 : VDREG3,
  float4 v4 : VDREG4,
  out float4 o0 : SV_Target0)
{
  uint4 r0;
  r0 = 0u;

  r0.x = vd_asuint((-(int)(vd_asint(_SnapshotType) == 5)));
  if (r0.x != 0u) discard;
  r0.x = vd_asuint((-(int)(vd_asint(_SnapshotType) == 4)));
  if (r0.x != 0u) discard;
  r0.x = vd_asuint((-(int)(vd_asint(_SnapshotType) == 3)));
  if (r0.x != 0u) {
    r0.x = vd_asuint((-(int)(vd_asfloat(v1.y) < 0.125)));
    if (r0.x != 0u) discard;
    r0.x = vd_asuint((-(int)(1.5 < vd_asfloat(v1.y))));
    if (r0.x != 0u) discard;
    o0.xyzw = vd_asfloat(uint4(0x00000000u, 0x3f800000u, 0x00000000u, 0x3f800000u));
    return;
  }
  r0.x = vd_asuint((-(int)(vd_asint(_SnapshotType) == 2)));
  if (r0.x != 0u) discard;
  r0.x = vd_asuint((-(int)(vd_asint(_SnapshotType) == 1)));
  r0.y = vd_asuint((-(int)(0.100000001 < vd_asfloat(v1.y))));
  r0.x = (r0.y | r0.x);
  if (r0.x != 0u) {
    r0.x = vd_asuint((-(int)(vd_asfloat(v4.y) < 0.5)));
    if (r0.x != 0u) discard;
    r0.x = vd_asuint((-(int)(4.0 < vd_asfloat(v1.y))));
    if (r0.x != 0u) discard;
    o0.xyzw = vd_asfloat(uint4(0x00000000u, 0x3f800000u, 0x00000000u, 0x3f800000u));
    return;
  }
  o0.xyzw = vd_asfloat(uint4(0x3f800000u, 0x00000000u, 0x00000000u, 0x3f800000u));
  return;
}
