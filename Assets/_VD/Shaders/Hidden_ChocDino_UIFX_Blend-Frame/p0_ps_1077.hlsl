// Hidden/ChocDino/UIFX/Blend-Frame | pass 0 "Blend-Frame" | progFragment | keywords: GRADIENT_RADIAL GRADIENT_RADIAL_BORDER SHAPE_CIRCLE USE_TEXTURE | blob 1077
void frag(
  float4 v0 : SV_POSITION0,
  float4 v1 : VDREG1,
  float4 v2 : VDREG2,
  out float4 o0 : SV_Target0)
{
  uint4 r0, r1, r2;
  r0 = 0u; r1 = 0u; r2 = 0u;

  r0.y = vd_asuint((vd_asfloat(v1.y) + -0.5));
  r0.z = vd_asuint((vd_asfloat(_ResultTex_TexelSize.y) / vd_asfloat(_ResultTex_TexelSize.x)));
  r1.xyzw = vd_asuint((vd_asfloat(v1.zwyx) + float4(-0.5, -0.5, -0.5, -0.5)));
  r0.x = vd_asuint((vd_asfloat(r0.z) * vd_asfloat(r1.w)));
  r0.xy = vd_asuint((vd_asfloat(r0.xy) + vd_asfloat(r0.xy)));
  r0.x = vd_asuint(dot(vd_asfloat(r0.xy), vd_asfloat(r0.xy)));
  r0.x = vd_asuint(sqrt(vd_asfloat(r0.x)));
  r0.x = vd_asuint((vd_asfloat(r0.x) / vd_asfloat(_GradientParams.x)));
  r0.z = vd_asuint((vd_asfloat(_BorderSizeSoft.x) + vd_asfloat(_BorderSizeSoft.x)));
  r0.z = vd_asuint((vd_asfloat(r0.z) / vd_asfloat(_ResultTex_TexelSize.w)));
  r0.z = vd_asuint(((-(vd_asfloat(r0.z))) + 1.0));
  r0.z = vd_asuint((vd_asfloat(r1.z) / vd_asfloat(r0.z)));
  r1.xy = vd_asuint((vd_asfloat(r1.xy) * vd_asfloat(_ResultTex_TexelSize.zw)));
  r0.w = vd_asuint(dot(vd_asfloat(r1.xy), vd_asfloat(r1.xy)));
  r0.w = vd_asuint(sqrt(vd_asfloat(r0.w)));
  r0.y = vd_asuint((vd_asfloat(r0.z) + 0.5));
  r1.xyzw = vd_asuint(_FillTex.Sample(sampler_FillTex, vd_asfloat(r0.xy)).xyzw);
  r1.xyz = vd_asuint((vd_asfloat(r1.www) * vd_asfloat(r1.xyz)));
  r1.xyzw = vd_asuint((vd_asfloat(r1.xyzw) * vd_asfloat(_FillColor.wwww)));
  r2.xyzw = vd_asuint(_SourceTex.Sample(sampler_SourceTex, vd_asfloat(v1.xy)).xyzw);
  r0.x = vd_asuint(((-(vd_asfloat(r2.w))) + 1.0));
  r1.xyzw = vd_asuint((vd_asfloat(r0.xxxx) * vd_asfloat(r1.xyzw) + vd_asfloat(r2.xyzw)));
  r0.x = vd_asuint(((-(vd_asfloat(_BorderSizeSoft.x))) * 2.0 + vd_asfloat(_ResultTex_TexelSize.z)));
  r0.x = vd_asuint((vd_asfloat(r0.x) * 0.5 + vd_asfloat(_BorderSizeSoft.x)));
  r0.x = vd_asuint(((-(vd_asfloat(r0.x))) + vd_asfloat(r0.w)));
  r0.x = vd_asuint((vd_asfloat(r0.x) + vd_asfloat(_BorderSizeSoft.x)));
  r0.y = vd_asuint(saturate(((-(vd_asfloat(r0.x))) / vd_asfloat(_FillSoft))));
  r0.x = vd_asuint(saturate(((-(vd_asfloat(r0.x))) + 0.5)));
  r0.y = vd_asuint(((-(vd_asfloat(r0.x))) + vd_asfloat(r0.y)));
  r0.z = vd_asuint(saturate(vd_asfloat(_FillSoft)));
  r0.x = vd_asuint((vd_asfloat(r0.z) * vd_asfloat(r0.y) + vd_asfloat(r0.x)));
  r0.xyzw = vd_asuint((vd_asfloat(r0.xxxx) * vd_asfloat(r1.xyzw)));
  r0.xyz = vd_asuint((vd_asfloat(r0.xyz) * vd_asfloat(v2.www)));
  o0.xyzw = (vd_asfloat(r0.xyzw) * vd_asfloat(v2.xyzw));
  return;
}
