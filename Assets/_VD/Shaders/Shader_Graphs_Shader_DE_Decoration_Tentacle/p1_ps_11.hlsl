// Shader Graphs/Shader_DE_Decoration_Tentacle | pass 1 "DepthOnly" | progFragment | keywords: (none) | blob 11
void frag(
  float4 v0 : SV_POSITION0,
  float4 v1 : INTERP0,
  out float4 o0 : SV_TARGET0)
{
  float4 r0;
  uint4 bitmask, uiDest;
  float4 fDest;

  r0.xyzw = _Texture2D.SampleBias(sampler_Texture2D, v1.xy, _GlobalMipBias.x).xyzw;
  r0.x = -0.800000012 + r0.w;
  r0.x = cmp(r0.x < 0);
  if (r0.x != 0) discard;
  o0.xyzw = v0.zzzz;
  return;
}
