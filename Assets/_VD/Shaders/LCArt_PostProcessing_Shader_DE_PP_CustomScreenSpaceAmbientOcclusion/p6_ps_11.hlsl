// LCArt/PostProcessing/Shader_DE_PP_CustomScreenSpaceAmbientOcclusion | pass 6 "SSAO_Gaussian_VerticalBlur" | progFragment | keywords: (none) | blob 11
void frag(
  float4 v0 : SV_POSITION0,
  float2 v1 : TEXCOORD0,
  out float o0 : SV_Target0)
{
  float4 r0,r1,r2,r3;
  uint4 bitmask, uiDest;
  float4 fDest;

  r0.x = 1 / _SSAOParams.z;
  r0.y = _SourceSize.w * r0.x;
  r0.x = 0;
  r1.xyzw = float4(0.538049996,0.538049996,2.0627799,2.0627799) * r0.xyxy + v1.xyxy;
  r0.xyzw = float4(-0.538049996,-0.538049996,-2.0627799,-2.0627799) * r0.xyxy + v1.xyxy;
  r2.xyzw = _BlitTexture.SampleBias(sampler_BlitTexture, r1.zw, _GlobalMipBias.x).xyzw;
  r1.xyzw = _BlitTexture.SampleBias(sampler_BlitTexture, r1.xy, _GlobalMipBias.x).xyzw;
  r3.xyzw = _BlitTexture.SampleBias(sampler_BlitTexture, r0.zw, _GlobalMipBias.x).xyzw;
  r0.xyzw = _BlitTexture.SampleBias(sampler_BlitTexture, r0.xy, _GlobalMipBias.x).xyzw;
  r0.x = r1.x + r0.x;
  r0.y = r3.x + r2.x;
  r0.y = 0.0509199984 * r0.y;
  r0.x = r0.x * 0.44907999 + r0.y;
  o0.x = 1 + -r0.x;
  return;
}
