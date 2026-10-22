// ArtTeam/ETC/shader_Transparent_Unlit | pass 0 "Universal Forward" | progFragment | keywords: _ALPHATEST_ON _SURFACE_TYPE_TRANSPARENT | blob 8
void frag(
  float4 v0 : SV_POSITION0,
  float4 v1 : INTERP0,
  float3 v2 : INTERP1,
  float3 v3 : INTERP2,
  out float4 o0 : SV_Target0)
{
  float4 r0,r1,r2;
  uint4 bitmask, uiDest;
  float4 fDest;

  r0.xy = v1.xy * _Albedo_ST.xy + _Albedo_ST.zw;
  r0.xyzw = _Albedo.SampleBias(sampler_Albedo, r0.xy, _GlobalMipBias.x).xyzw;
  r1.x = ddx(r0.w);
  r1.y = ddy(r0.w);
  r1.x = abs(r1.x) + abs(r1.y);
  r1.y = -_AlphaClipThreshold + r0.w;
  r1.z = -r1.x * 0.5 + r1.y;
  r1.x = max(9.99999975e-005, r1.x);
  r1.x = r1.z / r1.x;
  r1.x = saturate(1 + r1.x);
  r1.z = cmp(0 >= _AlphaClipThreshold);
  r1.x = r1.z ? 1 : r1.x;
  r1.z = ~(int)r1.z;
  r1.w = -9.99999975e-005 + r1.x;
  r2.x = cmp(_AlphaToMaskAvailable != 0.000000);
  r1.z = r1.z ? r2.x : 0;
  o0.w = r2.x ? r1.x : r0.w;
  r0.w = r1.z ? r1.w : r1.y;
  r0.w = cmp(r0.w < 0);
  if (r0.w != 0) discard;
  r0.yzw = r0.xyz + -r0.xxx;
  r0.xyz = (_isColor).xxx * r0.yzw + r0.xxx;
  o0.xyz = (_AlbedoStr).xxx * r0.xyz;
  return;
}
