// Shader Graphs/shader_BG_ReflectionSurface | pass 2 "DepthNormalsOnly" | progFragment | keywords: (none) | blob 13
void frag(
  float4 v0 : SV_POSITION0,
  float3 v1 : INTERP0,
  out float4 o0 : SV_Target0)
{
  float4 r0;
  uint4 bitmask, uiDest;
  float4 fDest;

  r0.x = dot(v1.xyz, v1.xyz);
  r0.x = rsqrt(r0.x);
  o0.xyz = v1.xyz * r0.xxx;
  o0.w = 0;
  return;
}
