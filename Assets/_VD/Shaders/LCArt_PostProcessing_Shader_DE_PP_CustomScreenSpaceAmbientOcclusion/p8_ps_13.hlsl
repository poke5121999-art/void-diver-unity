// LCArt/PostProcessing/Shader_DE_PP_CustomScreenSpaceAmbientOcclusion | pass 8 "SSAO_Kawase" | progFragment | keywords: (none) | blob 13
void frag(
  float4 v0 : SV_POSITION0,
  float2 v1 : TEXCOORD0,
  out float o0 : SV_Target0)
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
    o0.x = -r0.x * 0.25 + 1;
  } else {
    o0.x = r0.y;
  }
  return;
}
