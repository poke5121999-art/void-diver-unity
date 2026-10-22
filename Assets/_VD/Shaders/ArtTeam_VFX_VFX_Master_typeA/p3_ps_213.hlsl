// ArtTeam/VFX/VFX_Master_typeA | pass 3 "DepthNormalsOnly" | progFragment | keywords: _DEFORM_USE _MASK_USE _PLANESYSTEMCULLINGTYPE_ALWAYS _SURFACE_TYPE_TRANSPARENT | blob 213
void frag(
  float4 v0 : SV_POSITION0,
  float4 v1 : INTERP0,
  float4 v2 : INTERP1,
  float4 v3 : INTERP2,
  float4 v4 : INTERP3,
  float3 v5 : INTERP4,
  float3 v6 : INTERP5,
  out float4 o0 : SV_Target0)
{
  float4 r0;
  uint4 bitmask, uiDest;
  float4 fDest;

  r0.x = dot(v6.xyz, v6.xyz);
  r0.x = rsqrt(r0.x);
  o0.xyz = v6.xyz * r0.xxx;
  o0.w = 0;
  return;
}
