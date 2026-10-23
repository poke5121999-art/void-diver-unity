// Hidden/ChocDino/UIFX/Blend-Dissolve | pass 0 "Blend-Dissolve" | progFragment | keywords: EDGE_COLOR UNITY_UI_CLIP_RECT | blob 27
void frag(
  float4 v0 : SV_POSITION0,
  float4 v1 : VDREG1,
  float4 v2 : VDREG2,
  float4 v3 : VDREG3,
  out float4 o0 : SV_Target0)
{
  uint4 r0, r1, r2;
  r0 = 0u; r1 = 0u; r2 = 0u;

  r0.xyzw = vd_asuint(_FillTex.Sample(sampler_FillTex, vd_asfloat(v1.zw)).xyzw);
  r0.x = vd_asuint((vd_asfloat(r0.y) + vd_asfloat(r0.x)));
  r0.x = vd_asuint((vd_asfloat(r0.z) + vd_asfloat(r0.x)));
  r0.x = vd_asuint((vd_asfloat(r0.x) * vd_asfloat(_InvertFactor.y)));
  r0.x = vd_asuint(((-(vd_asfloat(r0.x))) * 0.333333343 + vd_asfloat(_InvertFactor.x)));
  r0.x = vd_asuint((vd_asfloat(r0.x) + (-(vd_asfloat(_Dissolve.x)))));
  r0.y = vd_asuint((1.0 / vd_asfloat(_Dissolve.y)));
  r0.x = vd_asuint(saturate((vd_asfloat(r0.y) * vd_asfloat(r0.x))));
  r0.y = vd_asuint((vd_asfloat(r0.x) * -2.0 + 3.0));
  r0.x = vd_asuint((vd_asfloat(r0.x) * vd_asfloat(r0.x)));
  r0.x = vd_asuint(((-(vd_asfloat(r0.y))) * vd_asfloat(r0.x) + 1.0));
  r0.x = vd_asuint(max(vd_asfloat(r0.x), 0.0));
  r0.y = vd_asuint((vd_asfloat(r0.x) + -0.899999976));
  r0.x = vd_asuint((vd_asfloat(r0.x) * 10.0));
  r0.x = vd_asuint(min(vd_asfloat(r0.x), 1.0));
  r0.x = vd_asuint(((-(vd_asfloat(r0.x))) + 1.0));
  r0.y = vd_asuint(saturate((vd_asfloat(r0.y) * 10.0)));
  r0.y = vd_asuint(((-(vd_asfloat(r0.y))) + 1.0));
  r0.z = vd_asuint(((-(vd_asfloat(r0.x))) + 1.0));
  r0.y = vd_asuint(min(vd_asfloat(r0.y), vd_asfloat(r0.z)));
  r1.xyzw = vd_asuint((vd_asfloat(_EdgeColor.xyzw) * vd_asfloat((_EdgeEmissive).xxxx)));
  r1.xyzw = vd_asuint((vd_asfloat(r0.yyyy) * vd_asfloat(r1.xyzw)));
  r2.xyzw = vd_asuint(_ResultTex.Sample(sampler_ResultTex, vd_asfloat(v1.xy)).xyzw);
  r0.xyzw = vd_asuint((vd_asfloat(r2.xyzw) * vd_asfloat(r0.xxxx) + vd_asfloat(r1.xyzw)));
  o0.xyzw = (vd_asfloat(r2.wwww) * vd_asfloat(r0.xyzw));
  return;
}
