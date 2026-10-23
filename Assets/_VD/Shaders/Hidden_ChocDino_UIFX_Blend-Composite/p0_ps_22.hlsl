// Hidden/ChocDino/UIFX/Blend-Composite | pass 0 "Blend-Composite" | progFragment | keywords: (none) | blob 22
void frag(
  float4 v0 : SV_POSITION0,
  float4 v1 : VDREG1,
  float4 v2 : VDREG2,
  out float4 o0 : SV_Target0)
{
  uint4 r0;
  r0 = 0u;

  r0.xyzw = vd_asuint(_ResultTex.Sample(sampler_ResultTex, vd_asfloat(v1.xy)).xyzw);
  r0.xyzw = vd_asuint((vd_asfloat(r0.xyzw) * vd_asfloat(_TintColor.xyzw)));
  r0.xyzw = vd_asuint(log2(vd_asfloat(r0.xyzw)));
  r0.xyzw = vd_asuint((vd_asfloat(r0.xyzw) * vd_asfloat(_PowerIntensity.xxxx)));
  r0.xyzw = vd_asuint(exp2(vd_asfloat(r0.xyzw)));
  r0.xyzw = vd_asuint(saturate((vd_asfloat(r0.xyzw) * vd_asfloat(_PowerIntensity.yyyy))));
  r0.xyz = vd_asuint((vd_asfloat(r0.xyz) * vd_asfloat(v2.www)));
  o0.xyzw = (vd_asfloat(r0.xyzw) * vd_asfloat(v2.xyzw));
  return;
}
