// LCArt/Character/Shader_DE_Character_FakeShadowCaster_Billboard | pass 0 "FakeShadowCast" | progFragment | keywords: _DE_PRERENDERING_CAMERA | blob 8
void frag(
  float4 v0 : SV_POSITION0,
  float4 v1 : VDREG1,
  out float4 o0 : SV_Target0)
{
  uint4 r0;
  r0 = 0u;

  r0.xyzw = vd_asuint(_MainTex.SampleBias(sampler_MainTex, vd_asfloat(v1.xy), vd_asfloat(_GlobalMipBias.x)).xyzw);
  r0.x = vd_asuint((vd_asfloat(r0.w) + -0.5));
  r0.x = vd_asuint((-(int)(vd_asfloat(r0.x) < 0.0)));
  if (r0.x != 0u) discard;
  r0.x = vd_asuint((vd_asfloat(v1.z) * vd_asfloat(_FakeShadowAlpha)));
  o0.y = (vd_asfloat(r0.x) * vd_asfloat(_FakeShadowVisibility));
  o0.xzw = vd_asfloat(uint3(0x00000000u, 0x00000000u, 0x3f800000u));
  return;
}
