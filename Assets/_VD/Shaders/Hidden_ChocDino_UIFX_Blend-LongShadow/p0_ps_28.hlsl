// Hidden/ChocDino/UIFX/Blend-LongShadow | pass 0 "Blend-LongShadow" | progFragment | keywords: UNITY_UI_ALPHACLIP | blob 28
void frag(
  float4 v0 : SV_POSITION0,
  float4 v1 : VDREG1,
  float4 v2 : VDREG2,
  out float4 o0 : SV_Target0)
{
  uint4 r0, r1;
  r0 = 0u; r1 = 0u;

  r0.xyzw = vd_asuint(_SourceTex.Sample(sampler_SourceTex, vd_asfloat(v1.xy)).xyzw);
  r1.x = vd_asuint((vd_asfloat(_SourceAlpha) * vd_asfloat(r0.w) + -0.00100000005));
  r0.xyzw = vd_asuint((vd_asfloat(r0.xyzw) * vd_asfloat((_SourceAlpha).xxxx)));
  r1.x = vd_asuint((-(int)(vd_asfloat(r1.x) < 0.0)));
  if (r1.x != 0u) discard;
  r0.xyz = vd_asuint((vd_asfloat(r0.xyz) * vd_asfloat(v2.www)));
  o0.xyzw = (vd_asfloat(r0.xyzw) * vd_asfloat(v2.xyzw));
  return;
}
