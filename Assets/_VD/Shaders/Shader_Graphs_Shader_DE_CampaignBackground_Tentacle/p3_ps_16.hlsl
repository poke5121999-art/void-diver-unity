// Shader Graphs/Shader_DE_CampaignBackground_Tentacle | pass 3 "DepthNormalsOnly" | progFragment | keywords: (none) | blob 16
void frag(
  float4 v0 : SV_POSITION0,
  float4 v1 : INTERP0,
  float3 v2 : INTERP1,
  out float4 o0 : SV_Target0)
{
  float4 r0;
  uint4 bitmask, uiDest;
  float4 fDest;

  r0.xyzw = _Texture2D.SampleBias(sampler_Texture2D, v1.xy, _GlobalMipBias.x).xyzw;
  r0.x = -0.800000012 + r0.w;
  r0.x = cmp(r0.x < 0);
  if (r0.x != 0) discard;
  r0.x = dot(v2.xyz, v2.xyz);
  r0.x = rsqrt(r0.x);
  o0.xyz = v2.xyz * r0.xxx;
  o0.w = 0;
  return;
}
