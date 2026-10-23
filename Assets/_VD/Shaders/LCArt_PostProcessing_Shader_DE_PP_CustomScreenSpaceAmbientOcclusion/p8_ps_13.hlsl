// LCArt/PostProcessing/Shader_DE_PP_CustomScreenSpaceAmbientOcclusion | pass 8 "SSAO_Kawase" | progFragment | keywords: (none) | blob 13
void frag(
  float4 v0 : SV_POSITION0,
  float4 v1 : VDREG1,
  out float o0 : SV_Target0)
{
  uint4 r0, r1, r2;
  r0 = 0u; r1 = 0u; r2 = 0u;

  r0.x = vd_asuint((1.0 / vd_asfloat(_SSAOParams.z)));
  r0.xy = vd_asuint((vd_asfloat(r0.xx) * vd_asfloat(_SourceSize.wz)));
  r0.zw = vd_asuint((vd_asfloat(r0.xy) * float2(0.5, 0.5)));
  r0.xy = vd_asuint((vd_asfloat(r0.xy) * vd_asfloat((_KawaseBlurIteration).xx) + vd_asfloat(r0.zw)));
  r1.xy = vd_asuint(((-(vd_asfloat(r0.yx))) + vd_asfloat(v1.xy)));
  r1.zw = vd_asuint((vd_asfloat(r0.xy) + vd_asfloat(v1.yx)));
  r0.xyzw = vd_asuint(_BlitTexture.SampleBias(sampler_BlitTexture, vd_asfloat(r1.xz), vd_asfloat(_GlobalMipBias.x)).xyzw);
  r2.xyzw = vd_asuint(_BlitTexture.SampleBias(sampler_BlitTexture, vd_asfloat(r1.wz), vd_asfloat(_GlobalMipBias.x)).xyzw);
  r0.x = vd_asuint((vd_asfloat(r0.x) + vd_asfloat(r2.x)));
  r2.xyzw = vd_asuint(_BlitTexture.SampleBias(sampler_BlitTexture, vd_asfloat(r1.wy), vd_asfloat(_GlobalMipBias.x)).xyzw);
  r0.x = vd_asuint((vd_asfloat(r0.x) + vd_asfloat(r2.x)));
  r1.xyzw = vd_asuint(_BlitTexture.SampleBias(sampler_BlitTexture, vd_asfloat(r1.xy), vd_asfloat(_GlobalMipBias.x)).xyzw);
  r0.x = vd_asuint((vd_asfloat(r0.x) + vd_asfloat(r1.x)));
  r0.y = vd_asuint((vd_asfloat(r0.x) * 0.25));
  if (vd_asuint(_LastKawasePass) != 0u) {
    o0.x = ((-(vd_asfloat(r0.x))) * 0.25 + 1.0);
  } else {
    o0.x = vd_asfloat(r0.y);
  }
  return;
}
