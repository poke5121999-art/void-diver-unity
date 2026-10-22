// ArtTeam/ETC/shader_Transparent_Unlit | pass 0 "Universal Forward" | progFragment | keywords: _DBUFFER_MRT3 | blob 12
void frag(
  float4 v0 : SV_POSITION0,
  float4 v1 : INTERP0,
  float3 v2 : INTERP1,
  float3 v3 : INTERP2,
  out float4 o0 : SV_Target0)
{
  float4 r0,r1;
  uint4 bitmask, uiDest;
  float4 fDest;

  r0.xy = v1.xy * _Albedo_ST.xy + _Albedo_ST.zw;
  r0.xyzw = _Albedo.SampleBias(sampler_Albedo, r0.xy, _GlobalMipBias.x).xyzw;
  r0.yzw = r0.xyz + -r0.xxx;
  r0.xyz = (_isColor).xxx * r0.yzw + r0.xxx;
  r0.xyz = (_AlbedoStr).xxx * r0.xyz;
  r1.xy = (int2)v0.xy;
  r1.zw = float2(0,0);
  r1.xyzw = _DBufferTexture0.Load(r1.xyz).xyzw;
  o0.xyz = r0.xyz * r1.www + r1.xyz;
  o0.w = 1;
  return;
}
