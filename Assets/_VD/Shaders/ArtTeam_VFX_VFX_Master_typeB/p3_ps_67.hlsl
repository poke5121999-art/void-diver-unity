// ArtTeam/VFX/VFX_Master_typeB | pass 3 "DepthNormalsOnly" | progFragment | keywords: _PLANESYSTEMCULLINGTYPE_EQUAL _SURFACE_TYPE_TRANSPARENT | blob 67
void frag(
  float4 v0 : SV_POSITION0,
  float4 v1 : VDREG1,
  float4 v2 : VDREG2,
  float4 v3 : VDREG3,
  float4 v4 : VDREG4,
  float4 v5 : VDREG5,
  out float4 o0 : SV_Target0)
{
  uint4 r0;
  r0 = 0u;

  r0.x = vd_asuint(dot(vd_asfloat(v5.xyz), vd_asfloat(v5.xyz)));
  r0.x = vd_asuint(rsqrt(vd_asfloat(r0.x)));
  o0.xyz = (vd_asfloat(r0.xxx) * vd_asfloat(v5.xyz));
  o0.w = vd_asfloat(0x00000000u);
  return;
}
