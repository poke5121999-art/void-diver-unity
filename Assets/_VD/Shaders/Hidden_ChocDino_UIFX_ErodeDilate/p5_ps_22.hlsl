// Hidden/ChocDino/UIFX/ErodeDilate | pass 5 "CopyAlpha" | progFragment | keywords: (none) | blob 22
void frag(
  float4 v0 : SV_POSITION0,
  float4 v1 : VDREG1,
  out float4 o0 : SV_Target0)
{
  uint4 r0;
  r0 = 0u;

  r0.xyzw = vd_asuint(_MainTex.Sample(sampler_MainTex, vd_asfloat(v1.xy)).xyzw);
  o0.xyzw = vd_asfloat(r0.wwww);
  return;
}
