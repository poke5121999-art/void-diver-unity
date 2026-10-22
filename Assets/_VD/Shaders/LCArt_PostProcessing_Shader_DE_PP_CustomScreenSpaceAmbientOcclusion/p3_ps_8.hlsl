// LCArt/PostProcessing/Shader_DE_PP_CustomScreenSpaceAmbientOcclusion | pass 3 "SSAO_Bilateral_FinalBlur" | progFragment | keywords: (none) | blob 8
void frag(
  float4 v0 : SV_POSITION0,
  float2 v1 : TEXCOORD0,
  out float4 o0 : SV_Target0)
{
  float4 r0,r1,r2,r3,r4;
  uint4 bitmask, uiDest;
  float4 fDest;

  r0.x = 1 / _SSAOParams.z;
  r0.yz = -_SourceSize.zw * r0.xx + v1.xy;
  r1.xyzw = _BlitTexture.SampleBias(sampler_BlitTexture, r0.yz, _GlobalMipBias.x).xyzw;
  r0.yzw = r1.yzw * float3(2,2,2) + float3(-1,-1,-1);
  r2.xyzw = _BlitTexture.SampleBias(sampler_BlitTexture, v1.xy, _GlobalMipBias.x).xyzw;
  r1.yzw = r2.yzw * float3(2,2,2) + float3(-1,-1,-1);
  r0.y = dot(r1.yzw, r0.yzw);
  r0.y = -0.800000012 + r0.y;
  r0.y = saturate(5.00000048 * r0.y);
  r0.z = r0.y * -2 + 3;
  r0.y = r0.y * r0.y;
  r0.y = r0.z * r0.y;
  r0.z = r1.x * r0.y + r2.x;
  r2.xy = _SourceSize.zw * r0.xx;
  r0.xw = _SourceSize.zw * r0.xx + v1.xy;
  r3.xyzw = _BlitTexture.SampleBias(sampler_BlitTexture, r0.xw, _GlobalMipBias.x).xyzw;
  r2.zw = -r2.yx;
  r2.xyzw = v1.xyxy + r2.xzwy;
  r4.xyzw = _BlitTexture.SampleBias(sampler_BlitTexture, r2.xy, _GlobalMipBias.x).xyzw;
  r2.xyzw = _BlitTexture.SampleBias(sampler_BlitTexture, r2.zw, _GlobalMipBias.x).xyzw;
  r4.yzw = r4.yzw * float3(2,2,2) + float3(-1,-1,-1);
  r0.x = dot(r1.yzw, r4.yzw);
  r0.x = -0.800000012 + r0.x;
  r0.x = saturate(5.00000048 * r0.x);
  r0.w = r0.x * -2 + 3;
  r0.x = r0.x * r0.x;
  r1.x = r0.w * r0.x;
  r0.x = r0.w * r0.x + r0.y;
  r0.y = r4.x * r1.x + r0.z;
  r2.yzw = r2.yzw * float3(2,2,2) + float3(-1,-1,-1);
  r0.z = dot(r1.yzw, r2.yzw);
  r0.z = -0.800000012 + r0.z;
  r0.z = saturate(5.00000048 * r0.z);
  r0.w = r0.z * -2 + 3;
  r0.z = r0.z * r0.z;
  r1.x = r0.w * r0.z;
  r0.x = r0.w * r0.z + r0.x;
  r0.y = r2.x * r1.x + r0.y;
  r2.xyz = r3.yzw * float3(2,2,2) + float3(-1,-1,-1);
  r0.z = dot(r1.yzw, r2.xyz);
  r0.z = -0.800000012 + r0.z;
  r0.z = saturate(5.00000048 * r0.z);
  r0.w = r0.z * -2 + 3;
  r0.z = r0.z * r0.z;
  r1.x = r0.w * r0.z;
  r0.x = r0.w * r0.z + r0.x;
  r0.x = 1 + r0.x;
  r0.x = 1 / r0.x;
  r0.y = r3.x * r1.x + r0.y;
  o0.xyzw = -r0.yyyy * r0.xxxx + float4(1,1,1,1);
  return;
}
