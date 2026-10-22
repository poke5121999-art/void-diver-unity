// Shader Graphs/Shader_DE_PP_BrightnessSightShaper_Pivot | pass 0 "Universal Forward" | progFragment | keywords: _DBUFFER_MRT3 | blob 6
void frag(
  float4 v0 : SV_POSITION0,
  float3 v1 : INTERP0,
  float3 v2 : INTERP1,
  out float4 o0 : SV_Target0)
{
  float4 r0;
  uint4 bitmask, uiDest;
  float4 fDest;

  r0.x = _Intensity + _Intensity;
  r0.x = max(0, r0.x);
  o0.xyz = _Color.xyz * r0.xxx;
  o0.w = 1;
  return;
}
