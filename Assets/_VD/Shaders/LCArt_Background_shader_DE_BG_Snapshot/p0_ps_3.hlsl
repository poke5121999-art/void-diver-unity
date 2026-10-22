// LCArt/Background/shader_DE_BG_Snapshot | pass 0 "ForwardLit" | progFragment | keywords: (none) | blob 3
void frag(
  float4 v0 : SV_POSITION0,
  float3 v1 : TEXCOORD1,
  float4 v2 : COLOR0,
  float4 v3 : TEXCOORD0,
  float3 v4 : TEXCOORD2,
  out float4 o0 : SV_Target0)
{
  float4 r0;
  uint4 bitmask, uiDest;
  float4 fDest;

  r0.x = cmp(vd_asint(_SnapshotType) == 5);
  if (r0.x != 0) discard;
  r0.x = cmp(vd_asint(_SnapshotType) == 4);
  if (r0.x != 0) discard;
  r0.x = cmp(vd_asint(_SnapshotType) == 3);
  if (r0.x != 0) {
    r0.x = cmp(v1.y < 0.125);
    if (r0.x != 0) discard;
    r0.x = cmp(1.5 < v1.y);
    if (r0.x != 0) discard;
    o0.xyzw = float4(0,1,0,1);
    return;
  }
  r0.x = cmp(vd_asint(_SnapshotType) == 2);
  if (r0.x != 0) discard;
  r0.x = cmp(vd_asint(_SnapshotType) == 1);
  r0.y = cmp(0.100000001 < v1.y);
  r0.x = (int)r0.y | (int)r0.x;
  if (r0.x != 0) {
    r0.x = cmp(v4.y < 0.5);
    if (r0.x != 0) discard;
    r0.x = cmp(4 < v1.y);
    if (r0.x != 0) discard;
    o0.xyzw = float4(0,1,0,1);
    return;
  }
  o0.xyzw = float4(1,0,0,1);
  return;
}
