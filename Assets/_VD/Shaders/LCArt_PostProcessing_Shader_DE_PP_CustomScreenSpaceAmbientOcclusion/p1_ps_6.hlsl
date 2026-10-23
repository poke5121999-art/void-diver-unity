// LCArt/PostProcessing/Shader_DE_PP_CustomScreenSpaceAmbientOcclusion | pass 1 "SSAO_Bilateral_HorizontalBlur" | progFragment | keywords: (none) | blob 6
void frag(
  float4 v0 : SV_POSITION0,
  float4 v1 : VDREG1,
  out float4 o0 : SV_Target0)
{
  uint4 r0, r1, r2, r3, r4;
  r0 = 0u; r1 = 0u; r2 = 0u; r3 = 0u; r4 = 0u;

  r0.x = vd_asuint((1.0 / vd_asfloat(_SSAOParams.z)));
  r0.x = vd_asuint((vd_asfloat(r0.x) * vd_asfloat(_SourceSize.z)));
  r0.xz = vd_asuint((vd_asfloat(r0.xx) * float2(1.38461542, 3.23076916)));
  r0.yw = uint2(0x00000000u, 0x00000000u);
  r1.xyzw = vd_asuint(((-(vd_asfloat(r0.xyzw))) + vd_asfloat(v1.xyxy)));
  r0.xyzw = vd_asuint((vd_asfloat(r0.xyzw) + vd_asfloat(v1.xyxy)));
  r2.xyzw = vd_asuint(_BlitTexture.SampleBias(sampler_BlitTexture, vd_asfloat(r1.xy), vd_asfloat(_GlobalMipBias.x)).xyzw);
  r1.xyzw = vd_asuint(_BlitTexture.SampleBias(sampler_BlitTexture, vd_asfloat(r1.zw), vd_asfloat(_GlobalMipBias.x)).xyzw);
  r2.yzw = vd_asuint((vd_asfloat(r2.yzw) * float3(2.0, 2.0, 2.0) + float3(-1.0, -1.0, -1.0)));
  r3.xyzw = vd_asuint(_BlitTexture.SampleBias(sampler_BlitTexture, vd_asfloat(v1.xy), vd_asfloat(_GlobalMipBias.x)).xyzw);
  r3.yzw = vd_asuint((vd_asfloat(r3.yzw) * float3(2.0, 2.0, 2.0) + float3(-1.0, -1.0, -1.0)));
  r2.y = vd_asuint(dot(vd_asfloat(r3.yzw), vd_asfloat(r2.yzw)));
  r2.y = vd_asuint((vd_asfloat(r2.y) + -0.800000012));
  r2.y = vd_asuint(saturate((vd_asfloat(r2.y) * 5.00000048)));
  r2.z = vd_asuint((vd_asfloat(r2.y) * -2.0 + 3.0));
  r2.y = vd_asuint((vd_asfloat(r2.y) * vd_asfloat(r2.y)));
  r2.y = vd_asuint((vd_asfloat(r2.y) * vd_asfloat(r2.z)));
  r2.y = vd_asuint((vd_asfloat(r2.y) * 0.31621623));
  r2.x = vd_asuint((vd_asfloat(r2.y) * vd_asfloat(r2.x)));
  r2.x = vd_asuint((vd_asfloat(r3.x) * 0.227027029 + vd_asfloat(r2.x)));
  r4.xyzw = vd_asuint(_BlitTexture.SampleBias(sampler_BlitTexture, vd_asfloat(r0.xy), vd_asfloat(_GlobalMipBias.x)).xyzw);
  r0.xyzw = vd_asuint(_BlitTexture.SampleBias(sampler_BlitTexture, vd_asfloat(r0.zw), vd_asfloat(_GlobalMipBias.x)).xyzw);
  r4.yzw = vd_asuint((vd_asfloat(r4.yzw) * float3(2.0, 2.0, 2.0) + float3(-1.0, -1.0, -1.0)));
  r2.z = vd_asuint(dot(vd_asfloat(r3.yzw), vd_asfloat(r4.yzw)));
  r2.z = vd_asuint((vd_asfloat(r2.z) + -0.800000012));
  r2.z = vd_asuint(saturate((vd_asfloat(r2.z) * 5.00000048)));
  r2.w = vd_asuint((vd_asfloat(r2.z) * -2.0 + 3.0));
  r2.z = vd_asuint((vd_asfloat(r2.z) * vd_asfloat(r2.z)));
  r2.z = vd_asuint((vd_asfloat(r2.z) * vd_asfloat(r2.w)));
  r2.w = vd_asuint((vd_asfloat(r2.z) * 0.31621623));
  r2.y = vd_asuint((vd_asfloat(r2.z) * 0.31621623 + vd_asfloat(r2.y)));
  r2.x = vd_asuint((vd_asfloat(r4.x) * vd_asfloat(r2.w) + vd_asfloat(r2.x)));
  r1.yzw = vd_asuint((vd_asfloat(r1.yzw) * float3(2.0, 2.0, 2.0) + float3(-1.0, -1.0, -1.0)));
  r1.y = vd_asuint(dot(vd_asfloat(r3.yzw), vd_asfloat(r1.yzw)));
  r1.y = vd_asuint((vd_asfloat(r1.y) + -0.800000012));
  r1.y = vd_asuint(saturate((vd_asfloat(r1.y) * 5.00000048)));
  r1.z = vd_asuint((vd_asfloat(r1.y) * -2.0 + 3.0));
  r1.y = vd_asuint((vd_asfloat(r1.y) * vd_asfloat(r1.y)));
  r1.y = vd_asuint((vd_asfloat(r1.y) * vd_asfloat(r1.z)));
  r1.z = vd_asuint((vd_asfloat(r1.y) * 0.0702702701));
  r1.y = vd_asuint((vd_asfloat(r1.y) * 0.0702702701 + vd_asfloat(r2.y)));
  r1.x = vd_asuint((vd_asfloat(r1.x) * vd_asfloat(r1.z) + vd_asfloat(r2.x)));
  r0.yzw = vd_asuint((vd_asfloat(r0.yzw) * float3(2.0, 2.0, 2.0) + float3(-1.0, -1.0, -1.0)));
  r0.y = vd_asuint(dot(vd_asfloat(r3.yzw), vd_asfloat(r0.yzw)));
  o0.yzw = (vd_asfloat(r3.yzw) * float3(0.5, 0.5, 0.5) + float3(0.5, 0.5, 0.5));
  r0.y = vd_asuint((vd_asfloat(r0.y) + -0.800000012));
  r0.y = vd_asuint(saturate((vd_asfloat(r0.y) * 5.00000048)));
  r0.z = vd_asuint((vd_asfloat(r0.y) * -2.0 + 3.0));
  r0.y = vd_asuint((vd_asfloat(r0.y) * vd_asfloat(r0.y)));
  r0.y = vd_asuint((vd_asfloat(r0.y) * vd_asfloat(r0.z)));
  r0.z = vd_asuint((vd_asfloat(r0.y) * 0.0702702701));
  r0.y = vd_asuint((vd_asfloat(r0.y) * 0.0702702701 + vd_asfloat(r1.y)));
  r0.y = vd_asuint((vd_asfloat(r0.y) + 0.227027029));
  r0.y = vd_asuint((1.0 / vd_asfloat(r0.y)));
  r0.x = vd_asuint((vd_asfloat(r0.x) * vd_asfloat(r0.z) + vd_asfloat(r1.x)));
  o0.x = (vd_asfloat(r0.y) * vd_asfloat(r0.x));
  return;
}
