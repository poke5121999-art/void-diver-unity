// LCArt/PostProcessing/Shader_DE_PP_UnifiedShadow | pass 2 "ScreenSpaceShadowApply" | progFragment | keywords: (none) | blob 50
void frag(
  float4 v0 : SV_POSITION0,
  float4 v1 : VDREG1,
  out float4 o0 : SV_Target0)
{
  uint4 r0, r1;
  r0 = 0u; r1 = 0u;

  r0.xyzw = vd_asuint(_BlitTexture.SampleBias(sampler_vd_linear_clamp, vd_asfloat(v1.xy), vd_asfloat(_GlobalMipBias.x)).xyzw);
  r0.y = vd_asuint(((-(vd_asfloat(r0.y))) + 1.0));
  r1.xyz = vd_asuint(((-(vd_asfloat(_GlobalShadowColor.xyz))) + float3(1.0, 1.0, 1.0)));
  r0.yzw = vd_asuint((vd_asfloat(r0.yyy) * vd_asfloat(r1.xyz) + vd_asfloat(_GlobalShadowColor.xyz)));
  r1.xyz = vd_asuint((vd_asfloat(r0.xxx) * vd_asfloat(r1.xyz) + vd_asfloat(_GlobalShadowColor.xyz)));
  o0.xyz = (vd_asfloat(r0.yzw) * vd_asfloat(r1.xyz));
  o0.w = vd_asfloat(0x3f800000u);
  return;
}
