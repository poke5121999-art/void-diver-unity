// Hidden/ChocDino/UIFX/Blend-Fill-Color | pass 0 "Blend-Fill-Color" | progFragment | keywords: COMP_BG_REPLACE MODE_CORNERS UNITY_UI_ALPHACLIP | blob 99
void frag(
  float4 v0 : SV_POSITION0,
  float4 v1 : VDREG1,
  float4 v2 : VDREG2,
  out float4 o0 : SV_Target0)
{
  uint4 r0, r1, r2, r3;
  r0 = 0u; r1 = 0u; r2 = 0u; r3 = 0u;

  r0.xyzw = vd_asuint(((-(vd_asfloat(_FillColorTL.xyzw))) + vd_asfloat(_FillColorTR.xyzw)));
  r1.xy = vd_asuint((vd_asfloat(v1.xy) + float2(-0.5, -0.5)));
  r1.xy = vd_asuint(saturate((vd_asfloat(r1.xy) * vd_asfloat(_ColorScaleBias.xx) + float2(0.5, 0.5))));
  r0.xyzw = vd_asuint((vd_asfloat(r1.xxxx) * vd_asfloat(r0.xyzw) + vd_asfloat(_FillColorTL.xyzw)));
  r2.xyzw = vd_asuint(((-(vd_asfloat(_FillColorBL.xyzw))) + vd_asfloat(_FillColorBR.xyzw)));
  r2.xyzw = vd_asuint((vd_asfloat(r1.xxxx) * vd_asfloat(r2.xyzw) + vd_asfloat(_FillColorBL.xyzw)));
  r0.xyzw = vd_asuint((vd_asfloat(r0.xyzw) + (-(vd_asfloat(r2.xyzw)))));
  r0.xyzw = vd_asuint((vd_asfloat(r1.yyyy) * vd_asfloat(r0.xyzw) + vd_asfloat(r2.xyzw)));
  r0.xyz = vd_asuint((vd_asfloat(r0.www) * vd_asfloat(r0.xyz)));
  r1.xyzw = vd_asuint(_ResultTex.Sample(sampler_ResultTex, vd_asfloat(v1.xy)).xyzw);
  r2.x = vd_asuint(((-(vd_asfloat(r1.w))) + 1.0));
  r3.xyz = vd_asuint((vd_asfloat(r2.xxx) * vd_asfloat(r0.xyz) + vd_asfloat(r1.xyz)));
  r3.w = vd_asuint((vd_asfloat(r2.x) * vd_asfloat(r0.w) + vd_asfloat(r1.w)));
  r0.xyzw = vd_asuint(((-(vd_asfloat(r1.xyzw))) + vd_asfloat(r3.xyzw)));
  r0.xyzw = vd_asuint((vd_asfloat((_Strength).xxxx) * vd_asfloat(r0.xyzw) + vd_asfloat(r1.xyzw)));
  r1.x = vd_asuint((vd_asfloat(r0.w) + -0.00100000005));
  r1.x = vd_asuint((-(int)(vd_asfloat(r1.x) < 0.0)));
  if (r1.x != 0u) discard;
  r0.xyz = vd_asuint((vd_asfloat(r0.xyz) * vd_asfloat(v2.www)));
  o0.xyzw = (vd_asfloat(r0.xyzw) * vd_asfloat(v2.xyzw));
  return;
}
