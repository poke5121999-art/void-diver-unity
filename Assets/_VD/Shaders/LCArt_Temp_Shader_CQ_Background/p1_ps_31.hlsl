// LCArt/Temp/Shader_CQ_Background | pass 1 "DepthNormals" | progFragment | keywords: (none) | blob 31
void frag(
  float4 v0 : SV_POSITION0,
  float4 v1 : TEXCOORD2,
  float3 v2 : TEXCOORD3,
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
