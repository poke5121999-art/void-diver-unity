// Hidden/ChocDino/UIFX/Composite | pass 0 "Composite-FromAlphaBlended-ToPremultiplied" | progFragment | keywords: (none) | blob 3
void frag(
  float4 v0 : SV_POSITION0,
  float4 v1 : VDREG1,
  out float4 o0 : SV_Target0)
{
  uint4 r0;
  r0 = 0u;

  r0.xyzw = vd_asuint(_MainTex.Sample(sampler_MainTex, vd_asfloat(v1.xy)).xyzw);
  r0.w = vd_asuint(rsqrt(vd_asfloat(r0.w)));
  o0.xyz = vd_asfloat(r0.xyz);
  o0.w = (1.0 / vd_asfloat(r0.w));
  return;
}
