// ArtTeam/ETC/shader_Rain | pass 0 "Universal Forward" | progFragment | keywords: (none) | blob 5
void frag(
  float4 v0 : SV_POSITION0,
  float4 v1 : VDREG1,
  float4 v2 : VDREG2,
  float4 v3 : VDREG3,
  out float4 o0 : SV_Target0)
{

  o0.xyz = vd_asfloat(vd_asuint((_RainStrength).xxx));
  o0.w = vd_asfloat(0x3f800000u);
  return;
}
