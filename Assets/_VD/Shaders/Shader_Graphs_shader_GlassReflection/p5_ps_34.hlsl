// Shader Graphs/shader_GlassReflection | pass 5 "Universal 2D" | progFragment | keywords: (none) | blob 34
void frag(
  float4 v0 : SV_POSITION0,
  float4 v1 : VDREG1,
  out float4 o0 : SV_TARGET0)
{
  uint4 r0;
  r0 = 0u;

  r0.x = vd_asuint((vd_asfloat(v1.y) + (-(vd_asfloat(_GradStartEnd.x)))));
  r0.y = vd_asuint(((-(vd_asfloat(_GradStartEnd.x))) + vd_asfloat(_GradStartEnd.y)));
  r0.x = vd_asuint(saturate((vd_asfloat(r0.x) / vd_asfloat(r0.y))));
  r0.yzw = vd_asuint(((-(vd_asfloat(_Color.xyz))) + vd_asfloat(_GradColor.xyz)));
  o0.xyz = (vd_asfloat(r0.xxx) * vd_asfloat(r0.yzw) + vd_asfloat(_Color.xyz));
  o0.w = vd_asfloat(0x3f800000u);
  return;
}
