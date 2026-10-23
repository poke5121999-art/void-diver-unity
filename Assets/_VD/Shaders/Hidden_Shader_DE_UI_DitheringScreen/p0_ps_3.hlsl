// Hidden/Shader_DE_UI_DitheringScreen | pass 0 "DrawProcedural" | progFragment | keywords: (none) | blob 3
void frag(
  float4 v0 : SV_POSITION0,
  float4 v1 : VDREG1,
  float4 v2 : VDREG2,
  out float4 o0 : SV_TARGET0)
{

  o0.xyz = vd_asfloat(vd_asuint(_Color.xyz));
  o0.w = vd_asfloat(0x3f800000u);
  return;
}
