// ArtTeam/VFX/VFX_Master_typeA | pass 1 "DepthOnly" | progFragment | keywords: _MASK_USE _PLANESYSTEMCULLINGTYPE_ALWAYS | blob 160
void frag(
  float4 v0 : SV_POSITION0,
  float4 v1 : INTERP0,
  float4 v2 : INTERP1,
  float4 v3 : INTERP2,
  float4 v4 : INTERP3,
  float3 v5 : INTERP4,
  float3 v6 : INTERP5,
  out float4 o0 : SV_TARGET0)
{
  o0.xyzw = v0.zzzz;
  return;
}
