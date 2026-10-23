// Hidden/ChocDino/UIFX/Blend-Frame | pass 0 "Blend-Frame" | progFragment | keywords: GRADIENT_RADIAL_BORDER SHAPE_CIRCLE USE_BORDER_TEXTURE | blob 1071
void frag(
  float4 v0 : SV_POSITION0,
  float4 v1 : VDREG1,
  float4 v2 : VDREG2,
  out float4 o0 : SV_Target0)
{
  uint4 r0, r1, r2, r3;
  r0 = 0u; r1 = 0u; r2 = 0u; r3 = 0u;

  r0.xy = vd_asuint((vd_asfloat(v1.zw) + float2(-0.5, -0.5)));
  r0.xy = vd_asuint((vd_asfloat(r0.xy) * vd_asfloat(_ResultTex_TexelSize.zw)));
  r0.x = vd_asuint(dot(vd_asfloat(r0.xy), vd_asfloat(r0.xy)));
  r0.x = vd_asuint(sqrt(vd_asfloat(r0.x)));
  r0.y = vd_asuint(((-(vd_asfloat(_BorderSizeSoft.x))) * 2.0 + vd_asfloat(_ResultTex_TexelSize.z)));
  r0.y = vd_asuint((vd_asfloat(r0.y) * 0.5 + vd_asfloat(_BorderSizeSoft.x)));
  r0.x = vd_asuint(((-(vd_asfloat(r0.y))) + vd_asfloat(r0.x)));
  r0.x = vd_asuint((vd_asfloat(r0.x) + vd_asfloat(_BorderSizeSoft.x)));
  r0.y = vd_asuint(saturate(((-(vd_asfloat(r0.x))) / vd_asfloat(_FillSoft))));
  r0.x = vd_asuint(saturate(((-(vd_asfloat(r0.x))) + 0.5)));
  r0.y = vd_asuint(((-(vd_asfloat(r0.x))) + vd_asfloat(r0.y)));
  r0.z = vd_asuint(saturate(vd_asfloat(_FillSoft)));
  r0.x = vd_asuint((vd_asfloat(r0.z) * vd_asfloat(r0.y) + vd_asfloat(r0.x)));
  r0.yzw = vd_asuint((vd_asfloat(_FillColor.www) * vd_asfloat(_FillColor.xyz)));
  r1.xyzw = vd_asuint(_SourceTex.Sample(sampler_SourceTex, vd_asfloat(v1.xy)).xyzw);
  r2.x = vd_asuint(((-(vd_asfloat(r1.w))) + 1.0));
  r3.xyz = vd_asuint((vd_asfloat(r2.xxx) * vd_asfloat(r0.yzw) + vd_asfloat(r1.xyz)));
  r3.w = vd_asuint((vd_asfloat(r2.x) * vd_asfloat(_FillColor.w) + vd_asfloat(r1.w)));
  r0.xyzw = vd_asuint((vd_asfloat(r0.xxxx) * vd_asfloat(r3.xyzw)));
  r0.xyz = vd_asuint((vd_asfloat(r0.xyz) * vd_asfloat(v2.www)));
  o0.xyzw = (vd_asfloat(r0.xyzw) * vd_asfloat(v2.xyzw));
  return;
}
