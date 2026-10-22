// Hidden/Shader_DE_UI_DitheringScreen | pass 1 "Blit" | progFragment | keywords: (none) | blob 3
void frag(
  float4 v0 : SV_POSITION0,
  float4 v1 : INTERP0,
  float4 v2 : INTERP1,
  out float4 o0 : SV_TARGET0)
{
  o0.xyz = _Color.xyz;
  o0.w = 1;
  return;
}
