// ArtTeam/ETC/shader_Transparent_Unlit | pass 0 "Universal Forward" | progFragment | keywords: FOG_LINEAR | blob 9
void frag(
  float4 v0 : SV_POSITION0,
  float4 v1 : INTERP0,
  float3 v2 : INTERP1,
  float3 v3 : INTERP2,
  out float4 o0 : SV_Target0)
{
  float4 r0;
  uint4 bitmask, uiDest;
  float4 fDest;

  r0.xy = v1.xy * _Albedo_ST.xy + _Albedo_ST.zw;
  r0.xyzw = _Albedo.SampleBias(sampler_Albedo, r0.xy, _GlobalMipBias.x).xyzw;
  r0.yzw = r0.xyz + -r0.xxx;
  r0.xyz = (_isColor).xxx * r0.yzw + r0.xxx;
  o0.xyz = (_AlbedoStr).xxx * r0.xyz;
  o0.w = 1;
  return;
}
