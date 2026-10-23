// Hidden/ChocDino/UIFX/Blend-Frame | pass 0 "Blend-Frame" | progFragment | keywords: CUTOUT GRADIENT_RADIAL SHAPE_ROUNDRECT UNITY_UI_ALPHACLIP USE_TEXTURE | blob 1364
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
  r0.y = vd_asuint((vd_asfloat(r0.z) + 0.5));
  r0.xyzw = vd_asuint(_FillTex.Sample(sampler_FillTex, vd_asfloat(r0.xy)).xyzw);
  r0.xyz = vd_asuint((vd_asfloat(r0.www) * vd_asfloat(r0.xyz)));
  r0.xyzw = vd_asuint((vd_asfloat(r0.xyzw) * vd_asfloat(_FillColor.wwww)));
  r2.xyzw = vd_asuint(_SourceTex.Sample(sampler_SourceTex, vd_asfloat(v1.xy)).xyzw);
  r1.z = vd_asuint(((-(vd_asfloat(r2.w))) + 1.0));
  r2.xyzw = vd_asuint((vd_asfloat(r1.zzzz) * vd_asfloat(r0.xyzw) + vd_asfloat(r2.xyzw)));
  r0.xyzw = vd_asuint((vd_asfloat(r1.zzzz) * vd_asfloat(r0.xyzw) + (-(vd_asfloat(r2.xyzw)))));
  r0.xyzw = vd_asuint((vd_asfloat((_CutoutAlpha).xxxx) * vd_asfloat(r0.xyzw) + vd_asfloat(r2.xyzw)));
  r1.z = vd_asuint((-(int)(vd_asfloat(r1.x) < 0.0)));
  r1.zw = ((r1.zz != 0u) ? vd_asuint(_EdgeRounding.xz) : vd_asuint(_EdgeRounding.yw));
  r2.x = vd_asuint((-(int)(0.0 < vd_asfloat(r1.y))));
  r1.z = ((r2.x != 0u) ? r1.z : r1.w);
  r2.xy = vd_asuint(((-(vd_asfloat(_BorderSizeSoft.xx))) * float2(2.0, 2.0) + vd_asfloat(_ResultTex_TexelSize.zw)));
  r2.xy = vd_asuint((vd_asfloat(r2.xy) * float2(0.5, 0.5) + vd_asfloat(_BorderSizeSoft.xx)));
  r1.xy = vd_asuint((abs(vd_asfloat(r1.xy)) + (-(vd_asfloat(r2.xy)))));
  r1.xy = vd_asuint((vd_asfloat(r1.zz) + vd_asfloat(r1.xy)));
  r1.w = vd_asuint(max(vd_asfloat(r1.y), vd_asfloat(r1.x)));
  r1.xy = vd_asuint(max(vd_asfloat(r1.xy), float2(0.0, 0.0)));
  r1.x = vd_asuint(dot(vd_asfloat(r1.xy), vd_asfloat(r1.xy)));
  r1.x = vd_asuint(sqrt(vd_asfloat(r1.x)));
  r1.y = vd_asuint(min(vd_asfloat(r1.w), 0.0));
  r1.x = vd_asuint((vd_asfloat(r1.x) + vd_asfloat(r1.y)));
  r1.x = vd_asuint(((-(vd_asfloat(r1.z))) + vd_asfloat(r1.x)));
  r1.x = vd_asuint((vd_asfloat(r1.x) + vd_asfloat(_BorderSizeSoft.x)));
  r1.y = vd_asuint(saturate(((-(vd_asfloat(r1.x))) / vd_asfloat(_FillSoft))));
  r1.x = vd_asuint(saturate(((-(vd_asfloat(r1.x))) + 0.5)));
  r1.y = vd_asuint(((-(vd_asfloat(r1.x))) + vd_asfloat(r1.y)));
  r1.z = vd_asuint(saturate(vd_asfloat(_FillSoft)));
  r1.x = vd_asuint((vd_asfloat(r1.z) * vd_asfloat(r1.y) + vd_asfloat(r1.x)));
  r1.y = vd_asuint((vd_asfloat(r0.w) * vd_asfloat(r1.x) + -0.00100000005));
  r0.xyzw = vd_asuint((vd_asfloat(r0.xyzw) * vd_asfloat(r1.xxxx)));
  r1.x = vd_asuint((-(int)(vd_asfloat(r1.y) < 0.0)));
  if (r1.x != 0u) discard;
  r0.xyz = vd_asuint((vd_asfloat(r0.xyz) * vd_asfloat(v2.www)));
  o0.xyzw = (vd_asfloat(r0.xyzw) * vd_asfloat(v2.xyzw));
  return;
}
