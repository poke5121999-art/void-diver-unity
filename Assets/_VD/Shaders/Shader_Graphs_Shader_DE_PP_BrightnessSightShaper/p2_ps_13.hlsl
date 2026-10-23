// Shader Graphs/Shader_DE_PP_BrightnessSightShaper | pass 2 "DepthNormalsOnly" | progFragment | keywords: (none) | blob 13
void frag(
  float4 v0 : SV_POSITION0,
  float4 v1 : VDREG1,
  float4 v2 : VDREG2,
  out float4 o0 : SV_Target0)
{
  uint4 r0;
  r0 = 0u;

  r0.x = vd_asuint(dot(vd_asfloat(v2.xyz), vd_asfloat(v2.xyz)));
  r0.x = vd_asuint(rsqrt(vd_asfloat(r0.x)));
  o0.xyz = (vd_asfloat(r0.xxx) * vd_asfloat(v2.xyz));
  o0.w = vd_asfloat(0x00000000u);
  return;
}
