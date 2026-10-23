// Hidden/ChocDino/UIFX/Blend-Fill-Color | pass 0 "Blend-Fill-Color" | progFragment | keywords: COMP_FG_REPLACE MODE_VERTICAL | blob 74
void frag(
  float4 v0 : SV_POSITION0,
  float4 v1 : VDREG1,
  float4 v2 : VDREG2,
  out float4 o0 : SV_Target0)
{
  uint4 r0, r1, r2;
  r0 = 0u; r1 = 0u; r2 = 0u;

  r0.x = vd_asuint(((-(vd_asfloat(_ColorScaleBias.y))) + 1.0));
  r0.y = vd_asuint((vd_asfloat(v1.y) + -0.5));
  r0.x = vd_asuint(saturate((vd_asfloat(r0.y) * vd_asfloat(_ColorScaleBias.x) + vd_asfloat(r0.x))));
  r1.xyzw = vd_asuint((vd_asfloat(_FillColorA.xyzw) + (-(vd_asfloat(_FillColorB.xyzw)))));
  r0.xyzw = vd_asuint((vd_asfloat(r0.xxxx) * vd_asfloat(r1.xyzw) + vd_asfloat(_FillColorB.xyzw)));
  r0.xyz = vd_asuint((vd_asfloat(r0.www) * vd_asfloat(r0.xyz)));
  r1.xyzw = vd_asuint(_ResultTex.Sample(sampler_ResultTex, vd_asfloat(v1.xy)).xyzw);
  r2.xyzw = vd_asuint((vd_asfloat(r0.xyzw) * vd_asfloat(r1.wwww)));
  r0.xyzw = vd_asuint(((-(vd_asfloat(r1.xyzw))) + vd_asfloat(r2.xyzw)));
  r0.xyzw = vd_asuint((vd_asfloat((_Strength).xxxx) * vd_asfloat(r0.xyzw) + vd_asfloat(r1.xyzw)));
  r0.xyz = vd_asuint((vd_asfloat(r0.xyz) * vd_asfloat(v2.www)));
  o0.xyzw = (vd_asfloat(r0.xyzw) * vd_asfloat(v2.xyzw));
  return;
}
