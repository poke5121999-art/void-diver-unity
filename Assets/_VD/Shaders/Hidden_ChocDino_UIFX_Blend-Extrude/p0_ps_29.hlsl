// Hidden/ChocDino/UIFX/Blend-Extrude | pass 0 "Blend-Extrude" | progFragment | keywords: STYLE_NORMAL UNITY_UI_ALPHACLIP | blob 29
void frag(
  float4 v0 : SV_POSITION0,
  float4 v1 : VDREG1,
  float4 v2 : VDREG2,
  out float4 o0 : SV_Target0)
{
  uint4 r0, r1, r2;
  r0 = 0u; r1 = 0u; r2 = 0u;

  r0.xyzw = vd_asuint(_ResultTex.Sample(sampler_ResultTex, vd_asfloat(v1.zw)).xyzw);
  r1.xyzw = vd_asuint(_SourceTex.Sample(sampler_SourceTex, vd_asfloat(v1.xy)).xyzw);
  r2.xyzw = vd_asuint((vd_asfloat(r1.wxyz) * vd_asfloat((_SourceAlpha).xxxx)));
  r1.x = vd_asuint(((-(vd_asfloat(_SourceAlpha))) * vd_asfloat(r1.w) + 1.0));
  r0.xyzw = vd_asuint((vd_asfloat(r1.xxxx) * vd_asfloat(r0.xyzw) + vd_asfloat(r2.yzwx)));
  r1.x = vd_asuint((vd_asfloat(r0.w) + -0.00100000005));
  r1.x = vd_asuint((-(int)(vd_asfloat(r1.x) < 0.0)));
  if (r1.x != 0u) discard;
  r0.xyz = vd_asuint((vd_asfloat(r0.xyz) * vd_asfloat(v2.www)));
  o0.xyzw = (vd_asfloat(r0.xyzw) * vd_asfloat(v2.xyzw));
  return;
}
