// ArtTeam/ETC/shader_Transparent_Unlit | pass 1 "DepthOnly" | progFragment | keywords: _ALPHATEST_ON | blob 22
void frag(
  float4 v0 : SV_POSITION0,
  float4 v1 : INTERP0,
  out float4 o0 : SV_TARGET0)
{
  float4 r0;
  uint4 bitmask, uiDest;
  float4 fDest;

  r0.xy = v1.xy * _Albedo_ST.xy + _Albedo_ST.zw;
  r0.xyzw = _Albedo.SampleBias(sampler_Albedo, r0.xy, _GlobalMipBias.x).xyzw;
  r0.x = -_AlphaClipThreshold + r0.w;
  r0.x = cmp(r0.x < 0);
  if (r0.x != 0) discard;
  o0.xyzw = v0.zzzz;
  return;
}
