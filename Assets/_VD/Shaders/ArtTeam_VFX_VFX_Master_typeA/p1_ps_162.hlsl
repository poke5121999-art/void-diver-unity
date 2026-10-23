// ArtTeam/VFX/VFX_Master_typeA | pass 1 "DepthOnly" | progFragment | keywords: _DEFORM_USE _MASK_USE _PLANESYSTEMCULLINGTYPE_EQUAL | blob 162
void frag(
  float4 v0 : SV_POSITION0,
  float4 v1 : VDREG1,
  float4 v2 : VDREG2,
  float4 v3 : VDREG3,
  float4 v4 : VDREG4,
  float4 v5 : VDREG5,
  float4 v6 : VDREG6,
  out float4 o0 : SV_TARGET0)
{

  o0.xyzw = vd_asfloat(vd_asuint(v0.zzzz));
  return;
}
