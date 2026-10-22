// ArtTeam/VFX/VFX_Master_typeB_forMesh | pass 1 "DepthOnly" | progFragment | keywords: (none) | blob 28
void frag(
  float4 v0 : SV_POSITION0,
  float4 v1 : INTERP0,
  out float4 o0 : SV_TARGET0)
{
  o0.xyzw = v0.zzzz;
  return;
}
