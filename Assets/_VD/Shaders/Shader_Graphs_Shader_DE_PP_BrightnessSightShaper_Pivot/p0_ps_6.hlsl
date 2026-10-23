// Shader Graphs/Shader_DE_PP_BrightnessSightShaper_Pivot | pass 0 "Universal Forward" | progFragment | keywords: _DBUFFER_MRT3 | blob 6
void frag(
  float4 v0 : SV_POSITION0,
  float4 v1 : VDREG1,
  float4 v2 : VDREG2,
  out float4 o0 : SV_Target0)
{
  uint4 r0;
  r0 = 0u;

  r0.x = vd_asuint((vd_asfloat(_Intensity) + vd_asfloat(_Intensity)));
  r0.x = vd_asuint(max(vd_asfloat(r0.x), 0.0));
  o0.xyz = (vd_asfloat(r0.xxx) * vd_asfloat(_Color.xyz));
  o0.w = vd_asfloat(0x3f800000u);
  return;
}
