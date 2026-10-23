// Hidden/ProBuilder/EdgePicker | pass 0 "Edges" | progFragment | keywords: (none) | blob 3
void frag(
  float4 v0 : SV_POSITION0,
  float4 v1 : VDREG1,
  out float4 o0 : SV_Target0)
{

  o0.xyzw = vd_asfloat(vd_asuint(v1.xyzw));
  return;
}
