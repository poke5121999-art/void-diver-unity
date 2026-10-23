// Hidden/ChocDino/UIFX/Blend-Frame | pass 0 "Blend-Frame" | progFragment | keywords: BORDER CUTOUT SHAPE_ROUNDRECT UNITY_UI_ALPHACLIP UNITY_UI_CLIP_RECT USE_BORDER_TEXTURE USE_TEXTURE | blob 1766
void frag(
  float4 v0 : SV_POSITION0,
  float4 v1 : VDREG1,
  float4 v2 : VDREG2,
  float4 v3 : VDREG3,
  out float4 o0 : SV_Target0)
{
  uint4 r0, r1, r2;
  r0 = 0u; r1 = 0u; r2 = 0u;

  r0.xyzw = vd_asuint((vd_asfloat(v1.zwxy) + float4(-0.5, -0.5, -0.5, -0.5)));
  r0.xy = vd_asuint((vd_asfloat(r0.xy) * vd_asfloat(_ResultTex_TexelSize.zw)));
  r1.x = vd_asuint((-(int)(vd_asfloat(r0.x) < 0.0)));
  r1.xy = ((r1.xx != 0u) ? vd_asuint(_EdgeRounding.xz) : vd_asuint(_EdgeRounding.yw));
  r1.z = vd_asuint((-(int)(0.0 < vd_asfloat(r0.y))));
  r1.x = ((r1.z != 0u) ? r1.x : r1.y);
  r1.yz = vd_asuint(((-(vd_asfloat(_BorderSizeSoft.xx))) * float2(2.0, 2.0) + vd_asfloat(_ResultTex_TexelSize.zw)));
  r1.yz = vd_asuint((vd_asfloat(r1.yz) * float2(0.5, 0.5) + vd_asfloat(_BorderSizeSoft.xx)));
  r0.xy = vd_asuint((abs(vd_asfloat(r0.xy)) + (-(vd_asfloat(r1.yz)))));
  r0.xy = vd_asuint((vd_asfloat(r1.xx) + vd_asfloat(r0.xy)));
  r1.yz = vd_asuint(max(vd_asfloat(r0.xy), float2(0.0, 0.0)));
  r0.x = vd_asuint(max(vd_asfloat(r0.y), vd_asfloat(r0.x)));
  r0.x = vd_asuint(min(vd_asfloat(r0.x), 0.0));
  r0.y = vd_asuint(dot(vd_asfloat(r1.yz), vd_asfloat(r1.yz)));
  r0.y = vd_asuint(sqrt(vd_asfloat(r0.y)));
  r0.x = vd_asuint((vd_asfloat(r0.y) + vd_asfloat(r0.x)));
  r0.x = vd_asuint(((-(vd_asfloat(r1.x))) + vd_asfloat(r0.x)));
  r0.y = vd_asuint((vd_asfloat(r0.x) + vd_asfloat(_BorderSizeSoft.x)));
  r1.x = vd_asuint(saturate(((-(vd_asfloat(r0.y))) / vd_asfloat(_FillSoft))));
  r0.y = vd_asuint(saturate(((-(vd_asfloat(r0.y))) + 0.5)));
  r1.x = vd_asuint(((-(vd_asfloat(r0.y))) + vd_asfloat(r1.x)));
  r1.y = vd_asuint(saturate(vd_asfloat(_FillSoft)));
  r0.y = vd_asuint((vd_asfloat(r1.y) * vd_asfloat(r1.x) + vd_asfloat(r0.y)));
  r1.x = vd_asuint((vd_asfloat(_BorderSizeSoft.x) + vd_asfloat(_BorderSizeSoft.x)));
  r1.xy = vd_asuint((vd_asfloat(r1.xx) / vd_asfloat(_ResultTex_TexelSize.zw)));
  r1.xy = vd_asuint(((-(vd_asfloat(r1.xy))) + float2(1.0, 1.0)));
  r0.zw = vd_asuint((vd_asfloat(r0.zw) / vd_asfloat(r1.xy)));
  r1.yz = vd_asuint((vd_asfloat(r0.zw) + float2(0.5, 0.5)));
  r0.zw = vd_asuint((vd_asfloat(r1.yz) * vd_asfloat(_GradientAxisParams.xy) + vd_asfloat(_GradientAxisParams.zw)));
  r1.x = vd_asuint((vd_asfloat(r0.w) + vd_asfloat(r0.z)));
  r1.xyzw = vd_asuint(_FillTex.Sample(sampler_FillTex, vd_asfloat(r1.xz)).wxyz);
  r1.yzw = vd_asuint((vd_asfloat(r1.xxx) * vd_asfloat(r1.yzw)));
  r1.xyzw = vd_asuint((vd_asfloat(r1.xyzw) * vd_asfloat(_FillColor.wwww)));
  r2.xyzw = vd_asuint(_SourceTex.Sample(sampler_SourceTex, vd_asfloat(v1.xy)).xyzw);
  r0.z = vd_asuint(((-(vd_asfloat(r2.w))) + 1.0));
  r2.xyzw = vd_asuint((vd_asfloat(r0.zzzz) * vd_asfloat(r1.xyzw) + vd_asfloat(r2.wxyz)));
  r1.xyzw = vd_asuint((vd_asfloat(r0.zzzz) * vd_asfloat(r1.xyzw) + (-(vd_asfloat(r2.xyzw)))));
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
  r1.y = vd_asuint((vd_asfloat(r0.x) * vd_asfloat(r1.x) + -0.00100000005));
  r0.xyzw = vd_asuint((vd_asfloat(r0.yzwx) * vd_asfloat(r1.xxxx)));
  r1.x = vd_asuint((-(int)(vd_asfloat(r1.y) < 0.0)));
  if (r1.x != 0u) discard;
  r0.xyz = vd_asuint((vd_asfloat(r0.xyz) * vd_asfloat(v2.www)));
  o0.xyzw = (vd_asfloat(r0.xyzw) * vd_asfloat(v2.xyzw));
  return;
}
