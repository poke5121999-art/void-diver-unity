// Hidden/ProBuilder/VertexPicker | pass 0 "Vertices" | progFragment | keywords: (none) | blob 3
void frag(
  float4 v0 : SV_POSITION0,
  float4 v1 : VDREG1,
  float4 v2 : VDREG2,
  out float4 o0 : SV_Target0)
{

  o0.xyzw = vd_asfloat(vd_asuint(v2.xyzw));
  return;
}
