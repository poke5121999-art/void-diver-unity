// ArtTeam/ETC/shader_Transparent_Unlit | pass 1 "DepthOnly" | progFragment | keywords: (none) | blob 21
void frag(
  float4 v0 : SV_POSITION0,
  float4 v1 : INTERP0,
  out float4 o0 : SV_TARGET0)
{
  o0.xyzw = v0.zzzz;
  return;
}
