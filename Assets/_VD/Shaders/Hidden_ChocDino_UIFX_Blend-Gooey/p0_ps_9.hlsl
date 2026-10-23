// Hidden/ChocDino/UIFX/Blend-Gooey | pass 0 "Blend-Gooey" | progFragment | keywords: UNITY_UI_ALPHACLIP | blob 9
void frag(
  float4 v0 : SV_POSITION0,
  float4 v1 : VDREG1,
  float4 v2 : VDREG2,
  out float4 o0 : SV_Target0)
{
  uint4 r0, r1, r2;
  r0 = 0u; r1 = 0u; r2 = 0u;

  r0.xyzw = vd_asuint(_ResultTex.Sample(sampler_ResultTex, vd_asfloat(v1.xy)).xyzw);
  r1.x = vd_asuint((vd_asfloat(r0.w) + (-(vd_asfloat(_ThresholdOffset)))));
  r1.w = vd_asuint(saturate((vd_asfloat(r1.x) * vd_asfloat(_ThresholdScale) + vd_asfloat(_ThresholdOffset))));
  r2.x = vd_asuint((vd_asfloat(r1.w) + -0.00100000005));
  r2.x = vd_asuint((-(int)(vd_asfloat(r2.x) < 0.0)));
  if (r2.x != 0u) discard;
  r2.x = vd_asuint((-(int)(0.0 < vd_asfloat(r0.w))));
  r2.yzw = vd_asuint((vd_asfloat(r0.xyz) / vd_asfloat(r0.www)));
  r0.xyz = ((r2.xxx != 0u) ? r2.yzw : r0.xyz);
  r0.xyz = vd_asuint((vd_asfloat(r1.www) * vd_asfloat(r0.xyz)));
  r2.xyzw = vd_asuint(_SourceTex.Sample(sampler_SourceTex, vd_asfloat(v1.xy)).xyzw);
  r0.w = vd_asuint(((-(vd_asfloat(r2.w))) + 1.0));
  r0.xyz = vd_asuint((vd_asfloat(r0.xyz) * vd_asfloat(r0.www)));
  r0.xyz = vd_asuint((vd_asfloat(r1.www) * vd_asfloat(r2.xyz) + vd_asfloat(r0.xyz)));
  r1.xyz = vd_asuint((vd_asfloat(r0.xyz) * vd_asfloat(v2.www)));
  o0.xyzw = (vd_asfloat(r1.xyzw) * vd_asfloat(v2.xyzw));
  return;
}
