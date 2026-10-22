// ArtTeam/VFX/VFX_Master_typeC_3CD | pass 3 "DepthNormalsOnly" | progFragment | keywords: (none) | blob 21
void frag(
  float4 v0 : SV_POSITION0,
  float4 v1 : INTERP0,
  float4 v2 : INTERP1,
  float3 v3 : INTERP2,
  out float4 o0 : SV_Target0)
{
  float4 r0;
  uint4 bitmask, uiDest;
  float4 fDest;

  r0.x = dot(v3.xyz, v3.xyz);
  r0.x = rsqrt(r0.x);
  o0.xyz = v3.xyz * r0.xxx;
  o0.w = 0;
  return;
}
