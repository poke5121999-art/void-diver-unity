// Hidden/ChocDino/UIFX/Blend-LongShadow | pass 0 "Blend-LongShadow" | progFragment | keywords: (none) | blob 24
void frag(
  float4 v0 : SV_POSITION0,
  float4 v1 : VDREG1,
  float4 v2 : VDREG2,
  out float4 o0 : SV_Target0)
{
  uint4 r0;
  r0 = 0u;

  r0.xyzw = vd_asuint(_SourceTex.Sample(sampler_SourceTex, vd_asfloat(v1.xy)).xyzw);
  r0.xyzw = vd_asuint((vd_asfloat(r0.xyzw) * vd_asfloat((_SourceAlpha).xxxx)));
  r0.xyz = vd_asuint((vd_asfloat(r0.xyz) * vd_asfloat(v2.www)));
  o0.xyzw = (vd_asfloat(r0.xyzw) * vd_asfloat(v2.xyzw));
  return;
}
