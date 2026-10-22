// ArtTeam/VFX/VFX_Master_typeB_Xpan | pass 1 "DepthOnly" | progFragment | keywords: (none) | blob 27
void frag(
  float4 v0 : SV_POSITION0,
  float4 v1 : INTERP0,
  float4 v2 : INTERP1,
  float4 v3 : INTERP2,
  out float4 o0 : SV_TARGET0)
{
  o0.xyzw = v0.zzzz;
  return;
}
