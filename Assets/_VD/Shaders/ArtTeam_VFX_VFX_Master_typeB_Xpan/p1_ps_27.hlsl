// ArtTeam/VFX/VFX_Master_typeB_Xpan | pass 1 "DepthOnly" | progFragment | keywords: (none) | blob 27
void frag(
  float4 v0 : SV_POSITION0,
  float4 v1 : VDREG1,
  float4 v2 : VDREG2,
  float4 v3 : VDREG3,
  out float4 o0 : SV_TARGET0)
{

  o0.xyzw = vd_asfloat(vd_asuint(v0.zzzz));
  return;
}
