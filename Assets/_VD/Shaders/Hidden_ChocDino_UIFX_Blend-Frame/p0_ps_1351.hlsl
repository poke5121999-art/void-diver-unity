// Hidden/ChocDino/UIFX/Blend-Frame | pass 0 "Blend-Frame" | progFragment | keywords: BORDER GRADIENT_RADIAL_BORDER SHAPE_ROUNDRECT UNITY_UI_ALPHACLIP USE_BORDER_TEXTURE USE_TEXTURE | blob 1351
void frag(
  float4 v0 : SV_POSITION0,
  float4 v1 : VDREG1,
  float4 v2 : VDREG2,
  out float4 o0 : SV_Target0)
{
  uint4 r0, r1, r2, r3;
  r0 = 0u; r1 = 0u; r2 = 0u; r3 = 0u;

  r0.x = vd_asuint((vd_asfloat(_BorderSizeSoft.x) + vd_asfloat(_BorderSizeSoft.x)));
  r0.xy = vd_asuint((vd_asfloat(r0.xx) / vd_asfloat(_ResultTex_TexelSize.zw)));
  r0.xy = vd_asuint(((-(vd_asfloat(r0.xy))) + float2(1.0, 1.0)));
  r1.xyzw = vd_asuint((vd_asfloat(v1.zwxy) + float4(-0.5, -0.5, -0.5, -0.5)));
  r0.xy = vd_asuint((vd_asfloat(r1.zw) / vd_asfloat(r0.xy)));
  r0.yz = vd_asuint((vd_asfloat(r0.xy) + float2(0.5, 0.5)));
  r0.yw = vd_asuint((vd_asfloat(r0.yz) * vd_asfloat(_GradientAxisParams.xy) + vd_asfloat(_GradientAxisParams.zw)));
  r0.x = vd_asuint((vd_asfloat(r0.w) + vd_asfloat(r0.y)));
  r0.xyzw = vd_asuint(_FillTex.Sample(sampler_FillTex, vd_asfloat(r0.xz)).xyzw);
  r0.xyz = vd_asuint((vd_asfloat(r0.www) * vd_asfloat(r0.xyz)));
  r0.xyzw = vd_asuint((vd_asfloat(r0.xyzw) * vd_asfloat(_FillColor.wwww)));
  r2.xyzw = vd_asuint(_SourceTex.Sample(sampler_SourceTex, vd_asfloat(v1.xy)).xyzw);
  r1.w = vd_asuint(((-(vd_asfloat(r2.w))) + 1.0));
  r0.xyzw = vd_asuint((vd_asfloat(r1.wwww) * vd_asfloat(r0.xyzw) + vd_asfloat(r2.xyzw)));
  r1.w = vd_asuint((vd_asfloat(_ResultTex_TexelSize.y) / vd_asfloat(_ResultTex_TexelSize.x)));
  r2.x = vd_asuint((vd_asfloat(r1.w) * vd_asfloat(r1.z)));
  r1.xy = vd_asuint((vd_asfloat(r1.xy) * vd_asfloat(_ResultTex_TexelSize.zw)));
  r2.y = vd_asuint((vd_asfloat(v1.y) + -0.5));
  r1.zw = vd_asuint((vd_asfloat(r2.xy) + vd_asfloat(r2.xy)));
  r1.z = vd_asuint(dot(vd_asfloat(r1.zw), vd_asfloat(r1.zw)));
  r1.z = vd_asuint(sqrt(vd_asfloat(r1.z)));
  r2.x = vd_asuint((vd_asfloat(r1.z) / vd_asfloat(_BorderGradientParams.x)));
  r2.y = vd_asuint(v1.y);
  r2.xyzw = vd_asuint(_BorderFillTex.Sample(sampler_BorderFillTex, vd_asfloat(r2.xy)).xyzw);
  r2.xyz = vd_asuint((vd_asfloat(r2.www) * vd_asfloat(r2.xyz)));
  r2.xyzw = vd_asuint((vd_asfloat(r2.xyzw) * vd_asfloat(_BorderColor.wwww)));
  r1.zw = vd_asuint(saturate(vd_asfloat(_BorderSizeSoft.yx)));
  r0.xyzw = vd_asuint(((-(vd_asfloat(r2.xyzw))) * vd_asfloat(r1.wwww) + vd_asfloat(r0.xyzw)));
  r2.xyzw = vd_asuint((vd_asfloat(r1.wwww) * vd_asfloat(r2.xyzw)));
  r1.w = vd_asuint((-(int)(vd_asfloat(r1.x) < 0.0)));
  r3.xy = ((r1.ww != 0u) ? vd_asuint(_EdgeRounding.xz) : vd_asuint(_EdgeRounding.yw));
  r1.w = vd_asuint((-(int)(0.0 < vd_asfloat(r1.y))));
  r1.w = ((r1.w != 0u) ? r3.x : r3.y);
  r3.xy = vd_asuint(((-(vd_asfloat(_BorderSizeSoft.xx))) * float2(2.0, 2.0) + vd_asfloat(_ResultTex_TexelSize.zw)));
  r3.xy = vd_asuint((vd_asfloat(r3.xy) * float2(0.5, 0.5) + vd_asfloat(_BorderSizeSoft.xx)));
  r1.xy = vd_asuint((abs(vd_asfloat(r1.xy)) + (-(vd_asfloat(r3.xy)))));
  r1.xy = vd_asuint((vd_asfloat(r1.ww) + vd_asfloat(r1.xy)));
  r3.x = vd_asuint(max(vd_asfloat(r1.y), vd_asfloat(r1.x)));
  r1.xy = vd_asuint(max(vd_asfloat(r1.xy), float2(0.0, 0.0)));
  r1.x = vd_asuint(dot(vd_asfloat(r1.xy), vd_asfloat(r1.xy)));
  r1.x = vd_asuint(sqrt(vd_asfloat(r1.x)));
  r1.y = vd_asuint(min(vd_asfloat(r3.x), 0.0));
  r1.x = vd_asuint((vd_asfloat(r1.x) + vd_asfloat(r1.y)));
  r1.x = vd_asuint(((-(vd_asfloat(r1.w))) + vd_asfloat(r1.x)));
  r1.y = vd_asuint((vd_asfloat(r1.x) + vd_asfloat(_BorderSizeSoft.x)));
  r1.w = vd_asuint(saturate(((-(vd_asfloat(r1.y))) / vd_asfloat(_FillSoft))));
  r1.y = vd_asuint(saturate(((-(vd_asfloat(r1.y))) + 0.5)));
  r1.w = vd_asuint(((-(vd_asfloat(r1.y))) + vd_asfloat(r1.w)));
  r3.x = vd_asuint(saturate(vd_asfloat(_FillSoft)));
  r1.y = vd_asuint((vd_asfloat(r3.x) * vd_asfloat(r1.w) + vd_asfloat(r1.y)));
  r0.xyzw = vd_asuint((vd_asfloat(r1.yyyy) * vd_asfloat(r0.xyzw) + vd_asfloat(r2.xyzw)));
  r1.y = vd_asuint(saturate(((-(vd_asfloat(r1.x))) / vd_asfloat(_BorderSizeSoft.y))));
  r1.x = vd_asuint(saturate(((-(vd_asfloat(r1.x))) + 0.5)));
  r1.y = vd_asuint(((-(vd_asfloat(r1.x))) + vd_asfloat(r1.y)));
  r1.x = vd_asuint((vd_asfloat(r1.z) * vd_asfloat(r1.y) + vd_asfloat(r1.x)));
  r1.y = vd_asuint((vd_asfloat(r0.w) * vd_asfloat(r1.x) + -0.00100000005));
  r0.xyzw = vd_asuint((vd_asfloat(r0.xyzw) * vd_asfloat(r1.xxxx)));
  r1.x = vd_asuint((-(int)(vd_asfloat(r1.y) < 0.0)));
  if (r1.x != 0u) discard;
  r0.xyz = vd_asuint((vd_asfloat(r0.xyz) * vd_asfloat(v2.www)));
  o0.xyzw = (vd_asfloat(r0.xyzw) * vd_asfloat(v2.xyzw));
  return;
}
