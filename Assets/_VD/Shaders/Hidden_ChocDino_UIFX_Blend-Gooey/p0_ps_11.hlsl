// Hidden/ChocDino/UIFX/Blend-Gooey | pass 0 "Blend-Gooey" | progFragment | keywords: UNITY_UI_ALPHACLIP UNITY_UI_CLIP_RECT | blob 11
void frag(
  float4 v0 : SV_POSITION0,
  float4 v1 : VDREG1,
  float4 v2 : VDREG2,
  float4 v3 : VDREG3,
  out float4 o0 : SV_Target0)
{
  uint4 r0, r1, r2;
  r0 = 0u; r1 = 0u; r2 = 0u;

  r0.xy = vd_asuint(((-(vd_asfloat(_ClipRect.xy))) + vd_asfloat(_ClipRect.zw)));
  r0.xy = vd_asuint((vd_asfloat(r0.xy) + (-(abs(vd_asfloat(v3.xy))))));
  r0.xy = vd_asuint(saturate((vd_asfloat(r0.xy) * vd_asfloat(v3.zw))));
  r0.x = vd_asuint((vd_asfloat(r0.y) * vd_asfloat(r0.x)));
  r1.xyzw = vd_asuint(_ResultTex.Sample(sampler_ResultTex, vd_asfloat(v1.xy)).xyzw);
  r0.y = vd_asuint((vd_asfloat(r1.w) + (-(vd_asfloat(_ThresholdOffset)))));
  r0.y = vd_asuint(saturate((vd_asfloat(r0.y) * vd_asfloat(_ThresholdScale) + vd_asfloat(_ThresholdOffset))));
  r0.z = vd_asuint((vd_asfloat(r0.y) * vd_asfloat(r0.x) + -0.00100000005));
  r0.z = vd_asuint((-(int)(vd_asfloat(r0.z) < 0.0)));
  if (r0.z != 0u) discard;
  r0.z = vd_asuint((-(int)(0.0 < vd_asfloat(r1.w))));
  r2.xyz = vd_asuint((vd_asfloat(r1.xyz) / vd_asfloat(r1.www)));
  r1.xyz = ((r0.zzz != 0u) ? r2.xyz : r1.xyz);
  r1.xyz = vd_asuint((vd_asfloat(r0.yyy) * vd_asfloat(r1.xyz)));
  r2.xyzw = vd_asuint(_SourceTex.Sample(sampler_SourceTex, vd_asfloat(v1.xy)).xyzw);
  r0.z = vd_asuint(((-(vd_asfloat(r2.w))) + 1.0));
  r1.xyz = vd_asuint((vd_asfloat(r1.xyz) * vd_asfloat(r0.zzz)));
  r1.xyz = vd_asuint((vd_asfloat(r0.yyy) * vd_asfloat(r2.xyz) + vd_asfloat(r1.xyz)));
  r2.w = vd_asuint((vd_asfloat(r0.x) * vd_asfloat(r0.y)));
  r0.xyz = vd_asuint((vd_asfloat(r0.xxx) * vd_asfloat(r1.xyz)));
  r2.xyz = vd_asuint((vd_asfloat(r0.xyz) * vd_asfloat(v2.www)));
  o0.xyzw = (vd_asfloat(r2.xyzw) * vd_asfloat(v2.xyzw));
  return;
}
