// LCArt/PostProcessing/Shader_DE_PP_CustomScreenSpaceAmbientOcclusion | pass 3 "SSAO_Bilateral_FinalBlur" | progFragment | keywords: (none) | blob 8
void frag(
  float4 v0 : SV_POSITION0,
  float4 v1 : VDREG1,
  out float4 o0 : SV_Target0)
{
  uint4 r0, r1, r2, r3, r4;
  r0 = 0u; r1 = 0u; r2 = 0u; r3 = 0u; r4 = 0u;

  r0.x = vd_asuint((1.0 / vd_asfloat(_SSAOParams.z)));
  r0.yz = vd_asuint(((-(vd_asfloat(_SourceSize.zw))) * vd_asfloat(r0.xx) + vd_asfloat(v1.xy)));
  r1.xyzw = vd_asuint(_BlitTexture.SampleBias(sampler_BlitTexture, vd_asfloat(r0.yz), vd_asfloat(_GlobalMipBias.x)).xyzw);
  r0.yzw = vd_asuint((vd_asfloat(r1.yzw) * float3(2.0, 2.0, 2.0) + float3(-1.0, -1.0, -1.0)));
  r2.xyzw = vd_asuint(_BlitTexture.SampleBias(sampler_BlitTexture, vd_asfloat(v1.xy), vd_asfloat(_GlobalMipBias.x)).xyzw);
  r1.yzw = vd_asuint((vd_asfloat(r2.yzw) * float3(2.0, 2.0, 2.0) + float3(-1.0, -1.0, -1.0)));
  r0.y = vd_asuint(dot(vd_asfloat(r1.yzw), vd_asfloat(r0.yzw)));
  r0.y = vd_asuint((vd_asfloat(r0.y) + -0.800000012));
  r0.y = vd_asuint(saturate((vd_asfloat(r0.y) * 5.00000048)));
  r0.z = vd_asuint((vd_asfloat(r0.y) * -2.0 + 3.0));
  r0.y = vd_asuint((vd_asfloat(r0.y) * vd_asfloat(r0.y)));
  r0.y = vd_asuint((vd_asfloat(r0.y) * vd_asfloat(r0.z)));
  r0.z = vd_asuint((vd_asfloat(r1.x) * vd_asfloat(r0.y) + vd_asfloat(r2.x)));
  r2.xy = vd_asuint((vd_asfloat(r0.xx) * vd_asfloat(_SourceSize.zw)));
  r0.xw = vd_asuint((vd_asfloat(_SourceSize.zw) * vd_asfloat(r0.xx) + vd_asfloat(v1.xy)));
  r3.xyzw = vd_asuint(_BlitTexture.SampleBias(sampler_BlitTexture, vd_asfloat(r0.xw), vd_asfloat(_GlobalMipBias.x)).xyzw);
  r2.zw = vd_asuint((-(vd_asfloat(r2.yx))));
  r2.xyzw = vd_asuint((vd_asfloat(r2.xzwy) + vd_asfloat(v1.xyxy)));
  r4.xyzw = vd_asuint(_BlitTexture.SampleBias(sampler_BlitTexture, vd_asfloat(r2.xy), vd_asfloat(_GlobalMipBias.x)).xyzw);
  r2.xyzw = vd_asuint(_BlitTexture.SampleBias(sampler_BlitTexture, vd_asfloat(r2.zw), vd_asfloat(_GlobalMipBias.x)).xyzw);
  r4.yzw = vd_asuint((vd_asfloat(r4.yzw) * float3(2.0, 2.0, 2.0) + float3(-1.0, -1.0, -1.0)));
  r0.x = vd_asuint(dot(vd_asfloat(r1.yzw), vd_asfloat(r4.yzw)));
  r0.x = vd_asuint((vd_asfloat(r0.x) + -0.800000012));
  r0.x = vd_asuint(saturate((vd_asfloat(r0.x) * 5.00000048)));
  r0.w = vd_asuint((vd_asfloat(r0.x) * -2.0 + 3.0));
  r0.x = vd_asuint((vd_asfloat(r0.x) * vd_asfloat(r0.x)));
  r1.x = vd_asuint((vd_asfloat(r0.x) * vd_asfloat(r0.w)));
  r0.x = vd_asuint((vd_asfloat(r0.w) * vd_asfloat(r0.x) + vd_asfloat(r0.y)));
  r0.y = vd_asuint((vd_asfloat(r4.x) * vd_asfloat(r1.x) + vd_asfloat(r0.z)));
  r2.yzw = vd_asuint((vd_asfloat(r2.yzw) * float3(2.0, 2.0, 2.0) + float3(-1.0, -1.0, -1.0)));
  r0.z = vd_asuint(dot(vd_asfloat(r1.yzw), vd_asfloat(r2.yzw)));
  r0.z = vd_asuint((vd_asfloat(r0.z) + -0.800000012));
  r0.z = vd_asuint(saturate((vd_asfloat(r0.z) * 5.00000048)));
  r0.w = vd_asuint((vd_asfloat(r0.z) * -2.0 + 3.0));
  r0.z = vd_asuint((vd_asfloat(r0.z) * vd_asfloat(r0.z)));
  r1.x = vd_asuint((vd_asfloat(r0.z) * vd_asfloat(r0.w)));
  r0.x = vd_asuint((vd_asfloat(r0.w) * vd_asfloat(r0.z) + vd_asfloat(r0.x)));
  r0.y = vd_asuint((vd_asfloat(r2.x) * vd_asfloat(r1.x) + vd_asfloat(r0.y)));
  r2.xyz = vd_asuint((vd_asfloat(r3.yzw) * float3(2.0, 2.0, 2.0) + float3(-1.0, -1.0, -1.0)));
  r0.z = vd_asuint(dot(vd_asfloat(r1.yzw), vd_asfloat(r2.xyz)));
  r0.z = vd_asuint((vd_asfloat(r0.z) + -0.800000012));
  r0.z = vd_asuint(saturate((vd_asfloat(r0.z) * 5.00000048)));
  r0.w = vd_asuint((vd_asfloat(r0.z) * -2.0 + 3.0));
  r0.z = vd_asuint((vd_asfloat(r0.z) * vd_asfloat(r0.z)));
  r1.x = vd_asuint((vd_asfloat(r0.z) * vd_asfloat(r0.w)));
  r0.x = vd_asuint((vd_asfloat(r0.w) * vd_asfloat(r0.z) + vd_asfloat(r0.x)));
  r0.x = vd_asuint((vd_asfloat(r0.x) + 1.0));
  r0.x = vd_asuint((1.0 / vd_asfloat(r0.x)));
  r0.y = vd_asuint((vd_asfloat(r3.x) * vd_asfloat(r1.x) + vd_asfloat(r0.y)));
  o0.xyzw = ((-(vd_asfloat(r0.yyyy))) * vd_asfloat(r0.xxxx) + float4(1.0, 1.0, 1.0, 1.0));
  return;
}
