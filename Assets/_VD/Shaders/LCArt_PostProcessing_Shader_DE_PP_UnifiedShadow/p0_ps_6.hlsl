// LCArt/PostProcessing/Shader_DE_PP_UnifiedShadow | pass 0 "ScreenSpaceShadowCompute" | progFragment | keywords: _DE_FAKE_SHADOW_BLOB | blob 6
void frag(
  float4 v0 : SV_POSITION0,
  float4 v1 : VDREG1,
  out float4 o0 : SV_Target0)
{

  o0.xyzw = vd_asfloat(uint4(0x3f800000u, 0x00000000u, 0x00000000u, 0x3f800000u));
  return;
}
