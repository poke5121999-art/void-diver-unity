// ArtTeam/VFX/VFX_Master_typeA | pass 2 "MotionVectors" | progFragment | keywords: _DEFORM_USE _MASK_USE _PLANESYSTEMCULLINGTYPE_ALWAYS _VERTEXDISPLACEMENT_USE | blob 199
void frag(
  float3 v0 : CLIP_POSITION_NO_JITTER0,
  float3 v1 : PREVIOUS_CLIP_POSITION_NO_JITTER0,
  float4 v2 : SV_POSITION0,
  float4 v3 : INTERP0,
  float4 v4 : INTERP1,
  float4 v5 : INTERP2,
  float4 v6 : INTERP3,
  float3 v7 : INTERP4,
  float3 v8 : INTERP5,
  out float4 o0 : SV_Target0)
{
  float4 r0;
  uint4 bitmask, uiDest;
  float4 fDest;

  r0.x = 1 / v1.z;
  r0.xy = v1.xy * r0.xx;
  r0.z = 1 / v0.z;
  r0.xy = v0.xy * r0.zz + -r0.xy;
  r0.xy = float2(0.5,-0.5) * r0.xy;
  r0.z = cmp(unity_MotionVectorsParams.y != 0.000000);
  o0.xy = r0.zz ? r0.xy : 0;
  o0.zw = float2(0,0);
  return;
}
