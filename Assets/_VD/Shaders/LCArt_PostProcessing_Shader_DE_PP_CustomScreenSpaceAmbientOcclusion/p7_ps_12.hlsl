// LCArt/PostProcessing/Shader_DE_PP_CustomScreenSpaceAmbientOcclusion | pass 7 "SSAO_Gaussian_VerticalBlur_AfterOpaque" | progFragment | keywords: (none) | blob 12
void frag(
  float4 v0 : SV_POSITION0,
  float4 v1 : VDREG1,
  out float4 o0 : SV_Target0)
{
  uint4 r0, r1, r2, r3;
  r0 = 0u; r1 = 0u; r2 = 0u; r3 = 0u;

  r0.x = vd_asuint((1.0 / vd_asfloat(_SSAOParams.z)));
  r0.y = vd_asuint((vd_asfloat(r0.x) * vd_asfloat(_SourceSize.w)));
  r0.x = 0x00000000u;
  r1.xyzw = vd_asuint((float4(0.538049996, 0.538049996, 2.0627799, 2.0627799) * vd_asfloat(r0.xyxy) + vd_asfloat(v1.xyxy)));
  r0.xyzw = vd_asuint((float4(-0.538049996, -0.538049996, -2.0627799, -2.0627799) * vd_asfloat(r0.xyxy) + vd_asfloat(v1.xyxy)));
  r2.xyzw = vd_asuint(_BlitTexture.SampleBias(sampler_BlitTexture, vd_asfloat(r1.zw), vd_asfloat(_GlobalMipBias.x)).xyzw);
  r1.xyzw = vd_asuint(_BlitTexture.SampleBias(sampler_BlitTexture, vd_asfloat(r1.xy), vd_asfloat(_GlobalMipBias.x)).xyzw);
  r3.xyzw = vd_asuint(_BlitTexture.SampleBias(sampler_BlitTexture, vd_asfloat(r0.zw), vd_asfloat(_GlobalMipBias.x)).xyzw);
  r0.xyzw = vd_asuint(_BlitTexture.SampleBias(sampler_BlitTexture, vd_asfloat(r0.xy), vd_asfloat(_GlobalMipBias.x)).xyzw);
  r0.x = vd_asuint((vd_asfloat(r0.x) + vd_asfloat(r1.x)));
  r0.y = vd_asuint((vd_asfloat(r2.x) + vd_asfloat(r3.x)));
  r0.y = vd_asuint((vd_asfloat(r0.y) * 0.0509199984));
  r0.x = vd_asuint((vd_asfloat(r0.x) * 0.44907999 + vd_asfloat(r0.y)));
  r0.x = vd_asuint(((-(vd_asfloat(r0.x))) + 1.0));
  r0.x = vd_asuint(((-(vd_asfloat(r0.x))) + 1.0));
  r1.xyzw = vd_asuint(((-(vd_asfloat(_CustomColor.xyzw))) + vd_asfloat(_CustomSSAOOverrideColor.xyzw)));
  r1.xyzw = vd_asuint((vd_asfloat((_CustomSSAOEnableOverride).xxxx) * vd_asfloat(r1.xyzw) + vd_asfloat(_CustomColor.xyzw)));
  r0.x = vd_asuint((vd_asfloat(r0.x) * vd_asfloat(r1.w)));
  r0.yzw = vd_asuint((vd_asfloat(r1.xyz) + float3(-1.0, -1.0, -1.0)));
  r0.xyz = vd_asuint((vd_asfloat(r0.xxx) * vd_asfloat(r0.yzw) + float3(1.0, 1.0, 1.0)));
  o0.xyz = (vd_asfloat(r0.xyz) * float3(0.5, 0.5, 0.5));
  o0.w = vd_asfloat(0x3f800000u);
  return;
}
