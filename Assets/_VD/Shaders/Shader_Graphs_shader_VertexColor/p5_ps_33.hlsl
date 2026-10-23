// Shader Graphs/shader_VertexColor | pass 5 "Universal 2D" | progFragment | keywords: (none) | blob 33
void frag(
  float4 v0 : SV_POSITION0,
  float4 v1 : VDREG1,
  out float4 o0 : SV_TARGET0)
{

  o0.xyz = vd_asfloat(vd_asuint(v1.xyz));
  o0.w = vd_asfloat(0x3f800000u);
  return;
}
