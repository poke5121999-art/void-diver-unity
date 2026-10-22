// LCArt/PostProcessing/Shader_DE_PP_CustomScreenSpaceAmbientOcclusion | pass 9 "SSAO_Kawase_AfterOpaque" | progFragment | keywords: (none) | blob 14
void frag(
  float4 v0 : SV_POSITION0,
  float2 v1 : TEXCOORD0,
  out float4 o0 : SV_Target0)
{
  float4 r0,r1,r2;
  uint4 bitmask, uiDest;
  float4 fDest;

  r0.x = 1 / _SSAOParams.z;
  r0.xy = _SourceSize.wz * r0.xx;
  r0.zw = float2(0.5,0.5) * r0.xy;
  r0.xy = r0.xy * (_KawaseBlurIteration).xx + r0.zw;
  r1.xy = v1.xy + -r0.yx;
  r1.zw = v1.yx + r0.xy;
  r0.xyzw = _BlitTexture.SampleBias(sampler_BlitTexture, r1.xz, _GlobalMipBias.x).xyzw;
  r2.xyzw = _BlitTexture.SampleBias(sampler_BlitTexture, r1.wz, _GlobalMipBias.x).xyzw;
  r0.x = r2.x + r0.x;
  r2.xyzw = _BlitTexture.SampleBias(sampler_BlitTexture, r1.wy, _GlobalMipBias.x).xyzw;
  r0.x = r2.x + r0.x;
  r1.xyzw = _BlitTexture.SampleBias(sampler_BlitTexture, r1.xy, _GlobalMipBias.x).xyzw;
  r0.x = r1.x + r0.x;
  r0.y = 0.25 * r0.x;
  if (_LastKawasePass != 0) {
    r0.y = -r0.x * 0.25 + 1;
  }
  r1.xyzw = _CustomSSAOOverrideColor.xyzw + -_CustomColor.xyzw;
  r0.x = 1 + -r0.y;
  r1.xyzw = (_CustomSSAOEnableOverride).xxxx * r1.xyzw + _CustomColor.xyzw;
  r0.x = r1.w * r0.x;
  r0.yzw = float3(-1,-1,-1) + r1.xyz;
  r0.xyz = r0.xxx * r0.yzw + float3(1,1,1);
  o0.xyz = float3(0.5,0.5,0.5) * r0.xyz;
  o0.w = 1;
  return;
}
