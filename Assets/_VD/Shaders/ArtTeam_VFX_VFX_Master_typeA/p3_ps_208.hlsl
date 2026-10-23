// ArtTeam/VFX/VFX_Master_typeA | pass 3 "DepthNormalsOnly" | progFragment | keywords: _PLANESYSTEMCULLINGTYPE_ALWAYS | blob 208
void frag(
  float4 v0 : SV_POSITION0,
  float4 v1 : VDREG1,
  float4 v2 : VDREG2,
  float4 v3 : VDREG3,
  float4 v4 : VDREG4,
  float4 v5 : VDREG5,
  float4 v6 : VDREG6,
  out float4 o0 : SV_Target0)
{
  uint4 r0;
  r0 = 0u;

  r0.x = vd_asuint(dot(vd_asfloat(v6.xyz), vd_asfloat(v6.xyz)));
  r0.x = vd_asuint(rsqrt(vd_asfloat(r0.x)));
  o0.xyz = (vd_asfloat(r0.xxx) * vd_asfloat(v6.xyz));
  o0.w = vd_asfloat(0x00000000u);
  return;
}
