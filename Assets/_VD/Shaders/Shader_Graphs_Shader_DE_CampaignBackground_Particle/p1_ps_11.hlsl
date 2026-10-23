// Shader Graphs/Shader_DE_CampaignBackground_Particle | pass 1 "DepthOnly" | progFragment | keywords: (none) | blob 11
void frag(
  float4 v0 : SV_POSITION0,
  out float4 o0 : SV_TARGET0)
{

  o0.xyzw = vd_asfloat(vd_asuint(v0.zzzz));
  return;
}
