// Hidden/ChocDino/UIFX/DistanceMap | pass 4 "ResolveDistance" | progFragment | keywords: DIST_DIAMOND | blob 15
void frag(
  float4 v0 : SV_POSITION0,
  float4 v1 : VDREG1,
  out float4 o0 : SV_Target0)
{
  uint4 r0;
  r0 = 0u;

  r0.xyzw = vd_asuint(_MainTex.Sample(sampler_MainTex, vd_asfloat(v1.xy)).xyzw);
  r0.xy = vd_asuint((vd_asfloat(r0.xy) * vd_asfloat(_MainTex_TexelSize.zw)));
  r0.xy = vd_asuint((vd_asfloat(v1.xy) * vd_asfloat(_MainTex_TexelSize.zw) + (-(vd_asfloat(r0.xy)))));
  r0.x = vd_asuint((abs(vd_asfloat(r0.y)) + abs(vd_asfloat(r0.x))));
  r0.y = vd_asuint(((float)vd_asint(_DownSample)));
  o0.xyzw = (vd_asfloat(r0.yyyy) * vd_asfloat(r0.xxxx));
  return;
}
