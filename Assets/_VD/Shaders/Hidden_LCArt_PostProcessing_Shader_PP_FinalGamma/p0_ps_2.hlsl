// Hidden/LCArt/PostProcessing/Shader_PP_FinalGamma | pass 0 "FinalGammaPass" | progFragment | keywords: (none) | blob 2
void frag(
  float4 v0 : SV_POSITION0,
  float4 v1 : VDREG1,
  out float4 o0 : SV_Target0)
{
  uint4 r0, r1, r2;
  r0 = 0u; r1 = 0u; r2 = 0u;

  r0.xyzw = vd_asuint(_BlitTexture.SampleBias(sampler_vd_linear_clamp, vd_asfloat(v1.xy), vd_asfloat(_GlobalMipBias.x)).xyzw);
  r1.xyz = vd_asuint((-(int3)(float3(0.0, 0.0, 0.0) < vd_asfloat(r0.xyz))));
  r2.xyz = vd_asuint((-(int3)(vd_asfloat(r0.xyz) < float3(0.0, 0.0, 0.0))));
  r1.xyz = vd_asuint(((-(vd_asint(r1.xyz))) + vd_asint(r2.xyz)));
  r1.xyz = vd_asuint(((float3)vd_asint(r1.xyz)));
  r1.w = vd_asuint(max(vd_asfloat(_GlobalGammaDegree), 0.00100000005));
  r1.w = vd_asuint((1.0 / vd_asfloat(r1.w)));
  r0.xyz = vd_asuint(log2(abs(vd_asfloat(r0.xyz))));
  o0.w = vd_asfloat(r0.w);
  r0.xyz = vd_asuint((vd_asfloat(r0.xyz) * vd_asfloat(r1.www)));
  r0.xyz = vd_asuint(exp2(vd_asfloat(r0.xyz)));
  o0.xyz = (vd_asfloat(r0.xyz) * vd_asfloat(r1.xyz));
  return;
}
