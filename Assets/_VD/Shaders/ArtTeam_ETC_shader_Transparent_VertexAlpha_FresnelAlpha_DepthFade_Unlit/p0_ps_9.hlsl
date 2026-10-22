// ArtTeam/ETC/shader_Transparent_VertexAlpha_FresnelAlpha_DepthFade_Unlit | pass 0 "Universal Forward" | progFragment | keywords: (none) | blob 9
void frag(
  float4 v0 : SV_POSITION0,
  float4 v1 : INTERP0,
  float4 v2 : INTERP1,
  float3 v3 : INTERP2,
  float3 v4 : INTERP3,
  out float4 o0 : SV_Target0)
{
  float4 r0;
  uint4 bitmask, uiDest;
  float4 fDest;

  r0.xy = v1.xy * _MainTex_ST.xy + _MainTex_ST.zw;
  r0.xyzw = _MainTex.SampleBias(sampler_MainTex, r0.xy, _GlobalMipBias.x).xyzw;
  o0.xyz = _TintColor.xyz * r0.xyz;
  o0.w = 1;
  return;
}
