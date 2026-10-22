// Shader Graphs/shader_GlassReflection | pass 5 "Universal 2D" | progFragment | keywords: (none) | blob 34
void frag(
  float4 v0 : SV_POSITION0,
  float3 v1 : INTERP0,
  out float4 o0 : SV_TARGET0)
{
  float4 r0;
  uint4 bitmask, uiDest;
  float4 fDest;

  r0.x = -_GradStartEnd.x + v1.y;
  r0.y = _GradStartEnd.y + -_GradStartEnd.x;
  r0.x = saturate(r0.x / r0.y);
  r0.yzw = _GradColor.xyz + -_Color.xyz;
  o0.xyz = r0.xxx * r0.yzw + _Color.xyz;
  o0.w = 1;
  return;
}
