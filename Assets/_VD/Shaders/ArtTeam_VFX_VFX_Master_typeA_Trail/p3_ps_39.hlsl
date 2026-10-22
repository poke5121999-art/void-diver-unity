// ArtTeam/VFX/VFX_Master_typeA_Trail | pass 3 "DepthNormalsOnly" | progFragment | keywords: (none) | blob 39
void frag(
  float4 v0 : SV_POSITION0,
  float4 v1 : INTERP0,
  float4 v2 : INTERP1,
  float3 v3 : INTERP2,
  float3 v4 : INTERP3,
  out float4 o0 : SV_Target0)
{
  float4 r0;
  uint4 bitmask, uiDest;
  float4 fDest;

  r0.x = dot(v4.xyz, v4.xyz);
  r0.x = rsqrt(r0.x);
  o0.xyz = v4.xyz * r0.xxx;
  o0.w = 0;
  return;
}
