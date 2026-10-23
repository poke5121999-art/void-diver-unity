// Shader Graphs/shader_VertexColor | pass 2 "DepthOnly" | progFragment | keywords: (none) | blob 25
void frag(
  float4 v0 : SV_POSITION0,
  out float4 o0 : SV_TARGET0)
{

  o0.xyzw = vd_asfloat(vd_asuint(v0.zzzz));
  return;
}
