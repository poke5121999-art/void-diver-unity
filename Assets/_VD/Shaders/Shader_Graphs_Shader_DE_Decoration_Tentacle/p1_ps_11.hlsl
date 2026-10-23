// Shader Graphs/Shader_DE_Decoration_Tentacle | pass 1 "DepthOnly" | progFragment | keywords: (none) | blob 11
void frag(
  float4 v0 : SV_POSITION0,
  float4 v1 : VDREG1,
  out float4 o0 : SV_TARGET0)
{
  uint4 r0;
  r0 = 0u;

  r0.xyzw = vd_asuint(_Texture2D.SampleBias(sampler_Texture2D, vd_asfloat(v1.xy), vd_asfloat(_GlobalMipBias.x)).xyzw);
  r0.x = vd_asuint((vd_asfloat(r0.w) + -0.800000012));
  r0.x = vd_asuint((-(int)(vd_asfloat(r0.x) < 0.0)));
  if (r0.x != 0u) discard;
  o0.xyzw = vd_asfloat(vd_asuint(v0.zzzz));
  return;
}
