// Hidden/ChocDino/UIFX/Blend-Fill-Color | pass 0 "Blend-Fill-Color" | progFragment | keywords: COMP_CUTOUT MODE_CORNERS UNITY_UI_ALPHACLIP UNITY_UI_CLIP_RECT | blob 135
void frag(
  float4 v0 : SV_POSITION0,
  float4 v1 : VDREG1,
  float4 v2 : VDREG2,
  float4 v3 : VDREG3,
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
  r3.xyzw = vd_asuint((vd_asfloat(r0.wxyz) * vd_asfloat(r2.xxxx)));
  r0.xyzw = vd_asuint(((-(vd_asfloat(r1.wxyz))) + vd_asfloat(r3.xyzw)));
  r0.xyzw = vd_asuint((vd_asfloat((_Strength).xxxx) * vd_asfloat(r0.xyzw) + vd_asfloat(r1.wxyz)));
  r1.xy = vd_asuint(((-(vd_asfloat(_ClipRect.xy))) + vd_asfloat(_ClipRect.zw)));
  r1.xy = vd_asuint((vd_asfloat(r1.xy) + (-(abs(vd_asfloat(v3.xy))))));
  r1.xy = vd_asuint(saturate((vd_asfloat(r1.xy) * vd_asfloat(v3.zw))));
  r1.x = vd_asuint((vd_asfloat(r1.y) * vd_asfloat(r1.x)));
  r1.y = vd_asuint((vd_asfloat(r0.x) * vd_asfloat(r1.x) + -0.00100000005));
  r0.xyzw = vd_asuint((vd_asfloat(r0.yzwx) * vd_asfloat(r1.xxxx)));
  r1.x = vd_asuint((-(int)(vd_asfloat(r1.y) < 0.0)));
  if (r1.x != 0u) discard;
  r0.xyz = vd_asuint((vd_asfloat(r0.xyz) * vd_asfloat(v2.www)));
  o0.xyzw = (vd_asfloat(r0.xyzw) * vd_asfloat(v2.xyzw));
  return;
}
