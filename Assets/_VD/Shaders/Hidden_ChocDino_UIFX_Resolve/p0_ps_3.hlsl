// Hidden/ChocDino/UIFX/Resolve | pass 0 "Normal" | progFragment | keywords: (none) | blob 3
void frag(
  float4 v0 : SV_POSITION0,
  float4 v1 : VDREG1,
  out float4 o0 : SV_Target0)
{
  uint4 r0, r1;
  r0 = 0u; r1 = 0u;

  r0.xyzw = vd_asuint(_MainTex.Sample(sampler_MainTex, vd_asfloat(v1.xy)).xyzw);
  r1.x = vd_asuint((-(int)(0.0 < vd_asfloat(r0.w))));
  r1.yzw = vd_asuint((vd_asfloat(r0.xyz) / vd_asfloat(r0.www)));
  o0.xyz = vd_asfloat(((r1.xxx != 0u) ? r1.yzw : r0.xyz));
  o0.w = vd_asfloat(r0.w);
  return;
}
