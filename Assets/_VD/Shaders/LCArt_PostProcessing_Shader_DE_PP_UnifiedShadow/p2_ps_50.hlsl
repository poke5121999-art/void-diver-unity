// LCArt/PostProcessing/Shader_DE_PP_UnifiedShadow | pass 2 "ScreenSpaceShadowApply" | progFragment | keywords: (none) | blob 50
void frag(
  float4 v0 : SV_POSITION0,
  float2 v1 : TEXCOORD0,
  out float4 o0 : SV_Target0)
{
  float4 r0,r1;
  uint4 bitmask, uiDest;
  float4 fDest;

  r0.xyzw = _BlitTexture.SampleBias(sampler_vd_linear_clamp, v1.xy, _GlobalMipBias.x).xyzw;
  r0.y = 1 + -r0.y;
  r1.xyz = float3(1,1,1) + -_GlobalShadowColor.xyz;
  r0.yzw = r0.yyy * r1.xyz + _GlobalShadowColor.xyz;
  r1.xyz = r0.xxx * r1.xyz + _GlobalShadowColor.xyz;
  o0.xyz = r1.xyz * r0.yzw;
  o0.w = 1;
  return;
}
