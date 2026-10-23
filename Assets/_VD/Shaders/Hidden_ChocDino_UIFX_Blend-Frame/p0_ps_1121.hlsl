// Hidden/ChocDino/UIFX/Blend-Frame | pass 0 "Blend-Frame" | progFragment | keywords: BORDER CUTOUT GRADIENT_RADIAL GRADIENT_RADIAL_BORDER SHAPE_CIRCLE USE_BORDER_TEXTURE | blob 1121
void frag(
  float4 v0 : SV_POSITION0,
  float4 v1 : VDREG1,
  float4 v2 : VDREG2,
  out float4 o0 : SV_Target0)
{
  uint4 r0, r1, r2, r3;
  r0 = 0u; r1 = 0u; r2 = 0u; r3 = 0u;

  r0.xyz = vd_asuint((vd_asfloat(_FillColor.www) * vd_asfloat(_FillColor.xyz)));
  r1.xyzw = vd_asuint(_SourceTex.Sample(sampler_SourceTex, vd_asfloat(v1.xy)).xyzw);
  r0.w = vd_asuint(((-(vd_asfloat(r1.w))) + 1.0));
  r2.xyz = vd_asuint((vd_asfloat(r0.xyz) * vd_asfloat(r0.www)));
  r2.w = vd_asuint((vd_asfloat(r0.w) * vd_asfloat(_FillColor.w)));
  r0.xyzw = vd_asuint((vd_asfloat(r1.xyzw) + vd_asfloat(r2.xyzw)));
  r1.xyzw = vd_asuint(((-(vd_asfloat(r0.xyzw))) + vd_asfloat(r2.xyzw)));
  r0.xyzw = vd_asuint((vd_asfloat((_CutoutAlpha).xxxx) * vd_asfloat(r1.xyzw) + vd_asfloat(r0.xyzw)));
  r1.x = vd_asuint((vd_asfloat(_ResultTex_TexelSize.y) / vd_asfloat(_ResultTex_TexelSize.x)));
  r1.yzw = vd_asuint((vd_asfloat(v1.zwx) + float3(-0.5, -0.5, -0.5)));
  r2.x = vd_asuint((vd_asfloat(r1.x) * vd_asfloat(r1.w)));
  r1.xy = vd_asuint((vd_asfloat(r1.yz) * vd_asfloat(_ResultTex_TexelSize.zw)));
  r1.x = vd_asuint(dot(vd_asfloat(r1.xy), vd_asfloat(r1.xy)));
  r2.y = vd_asuint((vd_asfloat(v1.y) + -0.5));
  r1.yz = vd_asuint((vd_asfloat(r2.xy) + vd_asfloat(r2.xy)));
  r1.y = vd_asuint(dot(vd_asfloat(r1.yz), vd_asfloat(r1.yz)));
  r1.xy = vd_asuint(sqrt(vd_asfloat(r1.xy)));
  r2.x = vd_asuint((vd_asfloat(r1.y) / vd_asfloat(_BorderGradientParams.x)));
  r2.y = vd_asuint(v1.y);
  r2.xyzw = vd_asuint(_BorderFillTex.Sample(sampler_BorderFillTex, vd_asfloat(r2.xy)).xyzw);
  r2.xyz = vd_asuint((vd_asfloat(r2.www) * vd_asfloat(r2.xyz)));
  r2.xyzw = vd_asuint((vd_asfloat(r2.xyzw) * vd_asfloat(_BorderColor.wwww)));
  r1.yz = vd_asuint(saturate(vd_asfloat(_BorderSizeSoft.yx)));
  r0.xyzw = vd_asuint(((-(vd_asfloat(r2.xyzw))) * vd_asfloat(r1.zzzz) + vd_asfloat(r0.xyzw)));
  r2.xyzw = vd_asuint((vd_asfloat(r1.zzzz) * vd_asfloat(r2.xyzw)));
  r1.z = vd_asuint(((-(vd_asfloat(_BorderSizeSoft.x))) * 2.0 + vd_asfloat(_ResultTex_TexelSize.z)));
  r1.z = vd_asuint((vd_asfloat(r1.z) * 0.5 + vd_asfloat(_BorderSizeSoft.x)));
  r1.x = vd_asuint(((-(vd_asfloat(r1.z))) + vd_asfloat(r1.x)));
  r1.z = vd_asuint((vd_asfloat(r1.x) + vd_asfloat(_BorderSizeSoft.x)));
  r1.w = vd_asuint(saturate(((-(vd_asfloat(r1.z))) / vd_asfloat(_FillSoft))));
  r1.z = vd_asuint(saturate(((-(vd_asfloat(r1.z))) + 0.5)));
  r1.w = vd_asuint(((-(vd_asfloat(r1.z))) + vd_asfloat(r1.w)));
  r3.x = vd_asuint(saturate(vd_asfloat(_FillSoft)));
  r1.z = vd_asuint((vd_asfloat(r3.x) * vd_asfloat(r1.w) + vd_asfloat(r1.z)));
  r0.xyzw = vd_asuint((vd_asfloat(r1.zzzz) * vd_asfloat(r0.xyzw) + vd_asfloat(r2.xyzw)));
  r1.z = vd_asuint(saturate(((-(vd_asfloat(r1.x))) / vd_asfloat(_BorderSizeSoft.y))));
  r1.x = vd_asuint(saturate(((-(vd_asfloat(r1.x))) + 0.5)));
  r1.z = vd_asuint(((-(vd_asfloat(r1.x))) + vd_asfloat(r1.z)));
  r1.x = vd_asuint((vd_asfloat(r1.y) * vd_asfloat(r1.z) + vd_asfloat(r1.x)));
  r0.xyzw = vd_asuint((vd_asfloat(r0.xyzw) * vd_asfloat(r1.xxxx)));
  r0.xyz = vd_asuint((vd_asfloat(r0.xyz) * vd_asfloat(v2.www)));
  o0.xyzw = (vd_asfloat(r0.xyzw) * vd_asfloat(v2.xyzw));
  return;
}
