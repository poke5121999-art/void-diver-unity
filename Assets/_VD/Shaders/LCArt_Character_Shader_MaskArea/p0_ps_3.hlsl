// LCArt/Character/Shader_MaskArea | pass 0 "" | progFragment | keywords: (none) | blob 3
void frag(
  float4 v0 : SV_POSITION0,
  float4 v1 : VDREG1,
  float4 v2 : VDREG2,
  out float4 o0 : SV_Target0)
{
  uint4 r0;
  r0 = 0u;

  r0.xyzw = vd_asuint(_MainTex.SampleBias(sampler_MainTex, vd_asfloat(v1.xy), vd_asfloat(_GlobalMipBias.x)).xyzw);
  o0.w = vd_asfloat(r0.x);
  o0.xy = vd_asfloat(uint2(0x00000000u, 0x00000000u));
  o0.z = vd_asfloat(vd_asuint(v1.z));
  return;
}
