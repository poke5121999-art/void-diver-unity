// LCArt/PostProcessing/Shader_DE_PP_UnifiedShadow | pass 0 "ScreenSpaceShadowCompute" | progFragment | keywords: _DE_FAKE_SHADOW_BLOB | blob 6
void frag(
  float4 v0 : SV_POSITION0,
  float2 v1 : TEXCOORD0,
  out float4 o0 : SV_Target0)
{
  o0.xyzw = float4(1,0,0,1);
  return;
}
