// Hidden/ChocDino/UIFX/Blend-Fill-Color | pass 0 "Blend-Fill-Color" | progFragment | keywords: COMP_BG_REPLACE MODE_HORIZONTAL UNITY_UI_CLIP_RECT | blob 113
void frag(
  float4 v0 : SV_POSITION0,
  float4 v1 : VDREG1,
  float4 v2 : VDREG2,
  float4 v3 : VDREG3,
  out float4 o0 : SV_Target0)
{
  uint4 r0, r1, r2, r3;
  r0 = 0u; r1 = 0u; r2 = 0u; r3 = 0u;

  r0.x = vd_asuint((vd_asfloat(v1.x) + -0.5));
  r0.x = vd_asuint(saturate((vd_asfloat(r0.x) * vd_asfloat(_ColorScaleBias.x) + vd_asfloat(_ColorScaleBias.y))));
  r1.xyzw = vd_asuint(((-(vd_asfloat(_FillColorA.xyzw))) + vd_asfloat(_FillColorB.xyzw)));
  r0.xyzw = vd_asuint((vd_asfloat(r0.xxxx) * vd_asfloat(r1.xyzw) + vd_asfloat(_FillColorA.xyzw)));
  r0.xyz = vd_asuint((vd_asfloat(r0.www) * vd_asfloat(r0.xyz)));
  r1.xyzw = vd_asuint(_ResultTex.Sample(sampler_ResultTex, vd_asfloat(v1.xy)).xyzw);
  r2.x = vd_asuint(((-(vd_asfloat(r1.w))) + 1.0));
  r3.yzw = vd_asuint((vd_asfloat(r2.xxx) * vd_asfloat(r0.xyz) + vd_asfloat(r1.xyz)));
  r3.x = vd_asuint((vd_asfloat(r2.x) * vd_asfloat(r0.w) + vd_asfloat(r1.w)));
  r0.xyzw = vd_asuint(((-(vd_asfloat(r1.wxyz))) + vd_asfloat(r3.xyzw)));
  r0.xyzw = vd_asuint((vd_asfloat((_Strength).xxxx) * vd_asfloat(r0.xyzw) + vd_asfloat(r1.wxyz)));
  r1.xy = vd_asuint(((-(vd_asfloat(_ClipRect.xy))) + vd_asfloat(_ClipRect.zw)));
  r1.xy = vd_asuint((vd_asfloat(r1.xy) + (-(abs(vd_asfloat(v3.xy))))));
  r1.xy = vd_asuint(saturate((vd_asfloat(r1.xy) * vd_asfloat(v3.zw))));
  r1.x = vd_asuint((vd_asfloat(r1.y) * vd_asfloat(r1.x)));
  r0.xyzw = vd_asuint((vd_asfloat(r0.yzwx) * vd_asfloat(r1.xxxx)));
  r0.xyz = vd_asuint((vd_asfloat(r0.xyz) * vd_asfloat(v2.www)));
  o0.xyzw = (vd_asfloat(r0.xyzw) * vd_asfloat(v2.xyzw));
  return;
}
