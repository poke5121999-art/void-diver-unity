// LCArt/PostProcessing/Shader_DE_PP_CustomScreenSpaceAmbientOcclusion | pass 5 "SSAO_Gaussian_HorizontalBlur" | progFragment | keywords: (none) | blob 10
void frag(
  float4 v0 : SV_POSITION0,
  float4 v1 : VDREG1,
  out float o0 : SV_Target0)
{
  uint4 r0, r1, r2, r3;
  r0 = 0u; r1 = 0u; r2 = 0u; r3 = 0u;

  r0.x = vd_asuint((1.0 / vd_asfloat(_SSAOParams.z)));
  r0.x = vd_asuint((vd_asfloat(r0.x) * vd_asfloat(_SourceSize.z)));
  r0.xz = vd_asuint((vd_asfloat(r0.xx) * float2(0.538049996, 2.0627799)));
  r0.yw = uint2(0x00000000u, 0x00000000u);
  r1.xyzw = vd_asuint((vd_asfloat(r0.xyzw) + vd_asfloat(v1.xyxy)));
  r0.xyzw = vd_asuint(((-(vd_asfloat(r0.xyzw))) + vd_asfloat(v1.xyxy)));
  r2.xyzw = vd_asuint(_BlitTexture.SampleBias(sampler_BlitTexture, vd_asfloat(r1.zw), vd_asfloat(_GlobalMipBias.x)).xyzw);
  r1.xyzw = vd_asuint(_BlitTexture.SampleBias(sampler_BlitTexture, vd_asfloat(r1.xy), vd_asfloat(_GlobalMipBias.x)).xyzw);
  r3.xyzw = vd_asuint(_BlitTexture.SampleBias(sampler_BlitTexture, vd_asfloat(r0.zw), vd_asfloat(_GlobalMipBias.x)).xyzw);
  r0.xyzw = vd_asuint(_BlitTexture.SampleBias(sampler_BlitTexture, vd_asfloat(r0.xy), vd_asfloat(_GlobalMipBias.x)).xyzw);
  r0.x = vd_asuint((vd_asfloat(r0.x) + vd_asfloat(r1.x)));
  r0.y = vd_asuint((vd_asfloat(r2.x) + vd_asfloat(r3.x)));
  r0.y = vd_asuint((vd_asfloat(r0.y) * 0.0509199984));
  o0.x = (vd_asfloat(r0.x) * 0.44907999 + vd_asfloat(r0.y));
  return;
}
