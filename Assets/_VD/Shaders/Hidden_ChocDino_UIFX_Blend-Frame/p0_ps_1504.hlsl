// Hidden/ChocDino/UIFX/Blend-Frame | pass 0 "Blend-Frame" | progFragment | keywords: BORDER CUTOUT GRADIENT_RADIAL SHAPE_CIRCLE UNITY_UI_CLIP_RECT USE_BORDER_TEXTURE | blob 1504
void frag(
  float4 v0 : SV_POSITION0,
  float4 v1 : VDREG1,
  float4 v2 : VDREG2,
  float4 v3 : VDREG3,
  out float4 o0 : SV_Target0)
{
  uint4 r0, r1, r2;
  r0 = 0u; r1 = 0u; r2 = 0u;

  r0.xy = vd_asuint((vd_asfloat(v1.zw) + float2(-0.5, -0.5)));
  r0.xy = vd_asuint((vd_asfloat(r0.xy) * vd_asfloat(_ResultTex_TexelSize.zw)));
  r0.x = vd_asuint(dot(vd_asfloat(r0.xy), vd_asfloat(r0.xy)));
  r0.x = vd_asuint(sqrt(vd_asfloat(r0.x)));
  r0.y = vd_asuint(((-(vd_asfloat(_BorderSizeSoft.x))) * 2.0 + vd_asfloat(_ResultTex_TexelSize.z)));
  r0.y = vd_asuint((vd_asfloat(r0.y) * 0.5 + vd_asfloat(_BorderSizeSoft.x)));
  r0.x = vd_asuint(((-(vd_asfloat(r0.y))) + vd_asfloat(r0.x)));
  r0.y = vd_asuint((vd_asfloat(r0.x) + vd_asfloat(_BorderSizeSoft.x)));
  r0.z = vd_asuint(saturate(((-(vd_asfloat(r0.y))) / vd_asfloat(_FillSoft))));
  r0.y = vd_asuint(saturate(((-(vd_asfloat(r0.y))) + 0.5)));
  r0.z = vd_asuint(((-(vd_asfloat(r0.y))) + vd_asfloat(r0.z)));
  r0.w = vd_asuint(saturate(vd_asfloat(_FillSoft)));
  r0.y = vd_asuint((vd_asfloat(r0.w) * vd_asfloat(r0.z) + vd_asfloat(r0.y)));
  r1.xyz = vd_asuint((vd_asfloat(_FillColor.www) * vd_asfloat(_FillColor.xyz)));
  r2.xyzw = vd_asuint(_SourceTex.Sample(sampler_SourceTex, vd_asfloat(v1.xy)).xyzw);
  r0.z = vd_asuint(((-(vd_asfloat(r2.w))) + 1.0));
  r1.yzw = vd_asuint((vd_asfloat(r1.xyz) * vd_asfloat(r0.zzz)));
  r1.x = vd_asuint((vd_asfloat(r0.z) * vd_asfloat(_FillColor.w)));
  r2.xyzw = vd_asuint((vd_asfloat(r1.xyzw) + vd_asfloat(r2.wxyz)));
  r1.xyzw = vd_asuint((vd_asfloat(r1.xyzw) + (-(vd_asfloat(r2.xyzw)))));
  r1.xyzw = vd_asuint((vd_asfloat((_CutoutAlpha).xxxx) * vd_asfloat(r1.xyzw) + vd_asfloat(r2.xyzw)));
  r0.zw = vd_asuint((vd_asfloat(v1.xy) * vd_asfloat(_BorderGradientAxisParams.xy) + vd_asfloat(_BorderGradientAxisParams.zw)));
  r2.x = vd_asuint((vd_asfloat(r0.w) + vd_asfloat(r0.z)));
  r2.y = vd_asuint(v1.y);
  r2.xyzw = vd_asuint(_BorderFillTex.Sample(sampler_BorderFillTex, vd_asfloat(r2.xy)).wxyz);
  r2.yzw = vd_asuint((vd_asfloat(r2.xxx) * vd_asfloat(r2.yzw)));
  r2.xyzw = vd_asuint((vd_asfloat(r2.xyzw) * vd_asfloat(_BorderColor.wwww)));
  r0.zw = vd_asuint(saturate(vd_asfloat(_BorderSizeSoft.yx)));
  r1.xyzw = vd_asuint(((-(vd_asfloat(r2.xyzw))) * vd_asfloat(r0.wwww) + vd_asfloat(r1.xyzw)));
  r2.xyzw = vd_asuint((vd_asfloat(r0.wwww) * vd_asfloat(r2.xyzw)));
  r1.xyzw = vd_asuint((vd_asfloat(r0.yyyy) * vd_asfloat(r1.xyzw) + vd_asfloat(r2.xyzw)));
  r0.y = vd_asuint(saturate(((-(vd_asfloat(r0.x))) / vd_asfloat(_BorderSizeSoft.y))));
  r0.x = vd_asuint(saturate(((-(vd_asfloat(r0.x))) + 0.5)));
  r0.y = vd_asuint(((-(vd_asfloat(r0.x))) + vd_asfloat(r0.y)));
  r0.x = vd_asuint((vd_asfloat(r0.z) * vd_asfloat(r0.y) + vd_asfloat(r0.x)));
  r0.xyzw = vd_asuint((vd_asfloat(r0.xxxx) * vd_asfloat(r1.xyzw)));
  r1.xy = vd_asuint(((-(vd_asfloat(_ClipRect.xy))) + vd_asfloat(_ClipRect.zw)));
  r1.xy = vd_asuint((vd_asfloat(r1.xy) + (-(abs(vd_asfloat(v3.xy))))));
  r1.xy = vd_asuint(saturate((vd_asfloat(r1.xy) * vd_asfloat(v3.zw))));
  r1.x = vd_asuint((vd_asfloat(r1.y) * vd_asfloat(r1.x)));
  r0.xyzw = vd_asuint((vd_asfloat(r0.yzwx) * vd_asfloat(r1.xxxx)));
  r0.xyz = vd_asuint((vd_asfloat(r0.xyz) * vd_asfloat(v2.www)));
  o0.xyzw = (vd_asfloat(r0.xyzw) * vd_asfloat(v2.xyzw));
  return;
}
