// Hidden/ProBuilder/HideVertices | pass 0 "" | progVertex | keywords: (none) | blob 1
void vert(
  float4 v0 : POSITION0,
  out float4 o0 : SV_POSITION0)
{

  o0.xyzw = vd_asfloat(uint4(0x00000000u, 0x00000000u, 0x00000000u, 0x00000000u));
  return;
}
