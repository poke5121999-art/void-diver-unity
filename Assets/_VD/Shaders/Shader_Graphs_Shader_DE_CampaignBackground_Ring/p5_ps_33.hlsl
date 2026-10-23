// Shader Graphs/Shader_DE_CampaignBackground_Ring | pass 5 "Universal 2D" | progFragment | keywords: (none) | blob 33
void frag(
  float4 v0 : SV_POSITION0,
  out float4 o0 : SV_TARGET0)
{

  o0.xyz = (vd_asfloat(_Color.xyz) * vd_asfloat((_Intensity).xxx));
  o0.w = vd_asfloat(0x3f800000u);
  return;
}
