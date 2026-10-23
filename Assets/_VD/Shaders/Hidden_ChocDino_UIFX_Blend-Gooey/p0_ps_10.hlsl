// Hidden/ChocDino/UIFX/Blend-Gooey | pass 0 "Blend-Gooey" | progFragment | keywords: UNITY_UI_CLIP_RECT | blob 10
void frag(
  float4 v0 : SV_POSITION0,
  float4 v1 : VDREG1,
  float4 v2 : VDREG2,
  float4 v3 : VDREG3,
  out float4 o0 : SV_Target0)
{
  uint4 r0, r1;
  r0 = 0u; r1 = 0u;

  r0.xyzw = vd_asuint(_ResultTex.Sample(sampler_ResultTex, vd_asfloat(v1.xy)).xyzw);
  r1.x = vd_asuint((-(int)(0.0 < vd_asfloat(r0.w))));
  r1.yzw = vd_asuint((vd_asfloat(r0.xyz) / vd_asfloat(r0.www)));
  r0.xyz = ((r1.xxx != 0u) ? r1.yzw : r0.xyz);
  r0.w = vd_asuint((vd_asfloat(r0.w) + (-(vd_asfloat(_ThresholdOffset)))));
  r0.w = vd_asuint(saturate((vd_asfloat(r0.w) * vd_asfloat(_ThresholdScale) + vd_asfloat(_ThresholdOffset))));
  r0.xyz = vd_asuint((vd_asfloat(r0.www) * vd_asfloat(r0.xyz)));
  r1.xyzw = vd_asuint(_SourceTex.Sample(sampler_SourceTex, vd_asfloat(v1.xy)).xyzw);
  r1.w = vd_asuint(((-(vd_asfloat(r1.w))) + 1.0));
  r0.xyz = vd_asuint((vd_asfloat(r0.xyz) * vd_asfloat(r1.www)));
  r0.xyz = vd_asuint((vd_asfloat(r0.www) * vd_asfloat(r1.xyz) + vd_asfloat(r0.xyz)));
  r1.xy = vd_asuint(((-(vd_asfloat(_ClipRect.xy))) + vd_asfloat(_ClipRect.zw)));
  r1.xy = vd_asuint((vd_asfloat(r1.xy) + (-(abs(vd_asfloat(v3.xy))))));
  r1.xy = vd_asuint(saturate((vd_asfloat(r1.xy) * vd_asfloat(v3.zw))));
  r1.x = vd_asuint((vd_asfloat(r1.y) * vd_asfloat(r1.x)));
  r0.xyz = vd_asuint((vd_asfloat(r0.xyz) * vd_asfloat(r1.xxx)));
  r1.w = vd_asuint((vd_asfloat(r0.w) * vd_asfloat(r1.x)));
  r1.xyz = vd_asuint((vd_asfloat(r0.xyz) * vd_asfloat(v2.www)));
  o0.xyzw = (vd_asfloat(r1.xyzw) * vd_asfloat(v2.xyzw));
  return;
}
