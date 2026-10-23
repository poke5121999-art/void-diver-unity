// LCArt/PostProcessing/Shader_DE_PP_UnifiedShadow | pass 1 "FakeShadowCast" | progFragment | keywords: (none) | blob 33
void frag(
  float4 v0 : SV_POSITION0,
  float4 v1 : VDREG1,
  out float4 o0 : SV_Target0)
{
  uint4 r0;
  r0 = 0u;

  r0.xy = vd_asuint((vd_asfloat(v1.xy) * vd_asfloat(_MainTex_ST.xy) + vd_asfloat(_MainTex_ST.zw)));
  r0.xyzw = vd_asuint(_MainTex.SampleBias(sampler_MainTex, vd_asfloat(r0.xy), vd_asfloat(_GlobalMipBias.x)).xyzw);
  r0.x = vd_asuint((vd_asfloat(r0.w) + -0.5));
  r0.x = vd_asuint((-(int)(vd_asfloat(r0.x) < 0.0)));
  r0.y = vd_asuint((-(int)(0.0 >= vd_asfloat(_FakeShadowScale))));
  r0.z = (~r0.y);
  r0.x = (r0.z & r0.x);
  if (r0.x != 0u) discard;
  r0.x = vd_asuint((vd_asfloat(v1.z) * vd_asfloat(_FakeShadowAlpha)));
  r0.x = vd_asuint((vd_asfloat(r0.x) * vd_asfloat(v1.w)));
  r0.z = vd_asuint((vd_asfloat(r0.w) * vd_asfloat(r0.x)));
  o0.y = saturate(((r0.y != 0u) ? vd_asfloat(r0.z) : vd_asfloat(r0.x)));
  o0.xzw = vd_asfloat(uint3(0x00000000u, 0x00000000u, 0x3f800000u));
  return;
}
