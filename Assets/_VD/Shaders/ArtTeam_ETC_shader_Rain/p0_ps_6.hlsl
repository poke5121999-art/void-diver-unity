// ArtTeam/ETC/shader_Rain | pass 0 "Universal Forward" | progFragment | keywords: FOG_LINEAR | blob 6
void frag(
  float4 v0 : SV_POSITION0,
  float4 v1 : INTERP0,
  float3 v2 : INTERP1,
  float3 v3 : INTERP2,
  out float4 o0 : SV_Target0)
{
  o0.xyz = (_RainStrength).xxx;
  o0.w = 1;
  return;
}
