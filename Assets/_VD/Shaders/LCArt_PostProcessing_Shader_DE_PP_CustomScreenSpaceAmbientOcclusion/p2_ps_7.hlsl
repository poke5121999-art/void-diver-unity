// LCArt/PostProcessing/Shader_DE_PP_CustomScreenSpaceAmbientOcclusion | pass 2 "SSAO_Bilateral_VerticalBlur" | progFragment | keywords: (none) | blob 7
void frag(
  float4 v0 : SV_POSITION0,
  float2 v1 : TEXCOORD0,
  out float4 o0 : SV_Target0)
{
  float4 r0,r1,r2,r3,r4;
  uint4 bitmask, uiDest;
  float4 fDest;

  r0.x = 1 / _SSAOParams.z;
  r0.x = _SourceSize.w * r0.x;
  r0.yz = float2(1.38461542,3.23076916);
  r1.xyzw = float4(-0,-1.38461542,-0,-3.23076916) * r0.yxzx + v1.xyxy;
  r0.xyzw = float4(0,1.38461542,0,3.23076916) * r0.yxzx + v1.xyxy;
  r2.xyzw = _BlitTexture.SampleBias(sampler_BlitTexture, r1.xy, _GlobalMipBias.x).xyzw;
  r1.xyzw = _BlitTexture.SampleBias(sampler_BlitTexture, r1.zw, _GlobalMipBias.x).xyzw;
  r2.yzw = r2.yzw * float3(2,2,2) + float3(-1,-1,-1);
  r3.xyzw = _BlitTexture.SampleBias(sampler_BlitTexture, v1.xy, _GlobalMipBias.x).xyzw;
  r3.yzw = r3.yzw * float3(2,2,2) + float3(-1,-1,-1);
  r2.y = dot(r3.yzw, r2.yzw);
  r2.y = -0.800000012 + r2.y;
  r2.y = saturate(5.00000048 * r2.y);
  r2.z = r2.y * -2 + 3;
  r2.y = r2.y * r2.y;
  r2.y = r2.z * r2.y;
  r2.y = 0.31621623 * r2.y;
  r2.x = r2.x * r2.y;
  r2.x = r3.x * 0.227027029 + r2.x;
  r4.xyzw = _BlitTexture.SampleBias(sampler_BlitTexture, r0.xy, _GlobalMipBias.x).xyzw;
  r0.xyzw = _BlitTexture.SampleBias(sampler_BlitTexture, r0.zw, _GlobalMipBias.x).xyzw;
  r4.yzw = r4.yzw * float3(2,2,2) + float3(-1,-1,-1);
  r2.z = dot(r3.yzw, r4.yzw);
  r2.z = -0.800000012 + r2.z;
  r2.z = saturate(5.00000048 * r2.z);
  r2.w = r2.z * -2 + 3;
  r2.z = r2.z * r2.z;
  r2.z = r2.w * r2.z;
  r2.w = 0.31621623 * r2.z;
  r2.y = r2.z * 0.31621623 + r2.y;
  r2.x = r4.x * r2.w + r2.x;
  r1.yzw = r1.yzw * float3(2,2,2) + float3(-1,-1,-1);
  r1.y = dot(r3.yzw, r1.yzw);
  r1.y = -0.800000012 + r1.y;
  r1.y = saturate(5.00000048 * r1.y);
  r1.z = r1.y * -2 + 3;
  r1.y = r1.y * r1.y;
  r1.y = r1.z * r1.y;
  r1.z = 0.0702702701 * r1.y;
  r1.y = r1.y * 0.0702702701 + r2.y;
  r1.x = r1.x * r1.z + r2.x;
  r0.yzw = r0.yzw * float3(2,2,2) + float3(-1,-1,-1);
  r0.y = dot(r3.yzw, r0.yzw);
  o0.yzw = r3.yzw * float3(0.5,0.5,0.5) + float3(0.5,0.5,0.5);
  r0.y = -0.800000012 + r0.y;
  r0.y = saturate(5.00000048 * r0.y);
  r0.z = r0.y * -2 + 3;
  r0.y = r0.y * r0.y;
  r0.y = r0.z * r0.y;
  r0.z = 0.0702702701 * r0.y;
  r0.y = r0.y * 0.0702702701 + r1.y;
  r0.y = 0.227027029 + r0.y;
  r0.y = 1 / r0.y;
  r0.x = r0.x * r0.z + r1.x;
  o0.x = r0.x * r0.y;
  return;
}
