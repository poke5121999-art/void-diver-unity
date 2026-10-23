// Hidden/ChocDino/UIFX/Blend-Doom | pass 0 "Blend-Doom" | progFragment | keywords: (none) | blob 8
void frag(
  float4 v0 : SV_POSITION0,
  float4 v1 : VDREG1,
  float4 v2 : VDREG2,
  out float4 o0 : SV_Target0)
{
  uint4 r0;
  r0 = 0u;

  r0.x = vd_asuint((vd_asfloat(v1.x) * vd_asfloat(_ResultTex_TexelSize.z)));
  r0.x = vd_asuint((vd_asfloat(r0.x) * 0.0009765625));
  r0.y = 0x00000000u;
  r0.xyzw = vd_asuint(_TimingTex.Sample(sampler_TimingTex, vd_asfloat(r0.xy)).xyzw);
  r0.x = vd_asuint((vd_asfloat(_Timing) + vd_asfloat(_Timing)));
  r0.x = vd_asuint(saturate(((-(vd_asfloat(r0.w))) + abs(vd_asfloat(r0.x)))));
  r0.y = vd_asuint((-(int)(0.0 < vd_asfloat(_Timing))));
  r0.z = vd_asuint((-(int)(vd_asfloat(_Timing) < 0.0)));
  r0.y = vd_asuint(((-(vd_asint(r0.y))) + vd_asint(r0.z)));
  r0.y = vd_asuint(((float)vd_asint(r0.y)));
  r0.y = vd_asuint((vd_asfloat(r0.x) * vd_asfloat(r0.y) + vd_asfloat(v1.y)));
  r0.z = vd_asuint((-(int)(vd_asfloat(r0.y) < 0.0)));
  r0.w = vd_asuint((-(int)(1.0 < vd_asfloat(r0.y))));
  r0.z = (r0.w | r0.z);
  if (r0.z != 0u) {
    o0.xyzw = vd_asfloat(uint4(0x00000000u, 0x00000000u, 0x00000000u, 0x00000000u));
    return;
  }
  r0.x = vd_asuint(v1.x);
  r0.xyzw = vd_asuint(_SourceTex.Sample(sampler_SourceTex, vd_asfloat(r0.xy)).xyzw);
  r0.xyz = vd_asuint((vd_asfloat(r0.xyz) * vd_asfloat(v2.www)));
  o0.xyzw = (vd_asfloat(r0.xyzw) * vd_asfloat(v2.xyzw));
  return;
}
