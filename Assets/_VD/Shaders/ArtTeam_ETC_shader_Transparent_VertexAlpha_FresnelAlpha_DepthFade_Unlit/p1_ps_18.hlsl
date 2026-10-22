// ArtTeam/ETC/shader_Transparent_VertexAlpha_FresnelAlpha_DepthFade_Unlit | pass 1 "DepthOnly" | progFragment | keywords: (none) | blob 18
void frag(
  float4 v0 : SV_POSITION0,
  float4 v1 : INTERP0,
  float4 v2 : INTERP1,
  float3 v3 : INTERP2,
  float3 v4 : INTERP3,
  out float4 o0 : SV_TARGET0)
{
  o0.xyzw = v0.zzzz;
  return;
}
