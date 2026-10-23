// ProBuilder6/Standard Vertex Color | pass 5 "Universal 2D" | progFragment | keywords: (none) | blob 34
void frag(
  float4 v0 : SV_POSITION0,
  float4 v1 : VDREG1,
  float4 v2 : VDREG2,
  out float4 o0 : SV_TARGET0)
{
  uint4 r0;
  r0 = 0u;

  r0.xyzw = vd_asuint(_MainTex.SampleBias(sampler_MainTex, vd_asfloat(v1.xy), vd_asfloat(_GlobalMipBias.x)).xyzw);
  r0.xyz = vd_asuint((vd_asfloat(r0.xyz) * vd_asfloat(_Color.xyz)));
  o0.xyz = (vd_asfloat(r0.xyz) * vd_asfloat(v2.xyz));
  o0.w = vd_asfloat(0x3f800000u);
  return;
}
