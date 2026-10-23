// LCArt/UI/Shader_UI_Additive | pass 0 "Default" | progFragment | keywords: UNITY_UI_ALPHACLIP | blob 8
void frag(
  float4 v0 : SV_POSITION0,
  float4 v1 : VDREG1,
  float4 v2 : VDREG2,
  float4 v3 : VDREG3,
  out float4 o0 : SV_Target0)
{
  uint4 r0, r1;
  r0 = 0u; r1 = 0u;

  r0.xyzw = vd_asuint(_MainTex.Sample(sampler_MainTex, vd_asfloat(v2.xy)).xyzw);
  r0.xyzw = vd_asuint((vd_asfloat(r0.xyzw) + vd_asfloat(_TextureSampleAdd.xyzw)));
  r1.x = vd_asuint((vd_asfloat(r0.w) * vd_asfloat(v1.w) + -0.00100000005));
  r0.xyzw = vd_asuint((vd_asfloat(r0.xyzw) * vd_asfloat(v1.xyzw)));
  o0.xyzw = vd_asfloat(r0.xyzw);
  r0.x = vd_asuint((-(int)(vd_asfloat(r1.x) < 0.0)));
  if (r0.x != 0u) discard;
  return;
}
