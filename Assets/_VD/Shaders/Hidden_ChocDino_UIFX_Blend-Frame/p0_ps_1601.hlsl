// Hidden/ChocDino/UIFX/Blend-Frame | pass 0 "Blend-Frame" | progFragment | keywords: BORDER GRADIENT_RADIAL GRADIENT_RADIAL_BORDER UNITY_UI_ALPHACLIP UNITY_UI_CLIP_RECT USE_BORDER_TEXTURE | blob 1601
void frag(
  float4 v0 : SV_POSITION0,
  float4 v1 : VDREG1,
  float4 v2 : VDREG2,
  float4 v3 : VDREG3,
  out float4 o0 : SV_Target0)
{
  uint4 r0, r1, r2, r3;
  r0 = 0u; r1 = 0u; r2 = 0u; r3 = 0u;

  r0.xy = vd_asuint(((-(vd_asfloat(_BorderSizeSoft.xx))) * float2(2.0, 2.0) + vd_asfloat(_ResultTex_TexelSize.zw)));
  r1.xyz = vd_asuint((vd_asfloat(v1.zwx) + float3(-0.5, -0.5, -0.5)));
  r0.zw = vd_asuint((vd_asfloat(r1.xy) * vd_asfloat(_ResultTex_TexelSize.zw)));
  r0.xy = vd_asuint(((-(vd_asfloat(r0.xy))) * float2(0.5, 0.5) + abs(vd_asfloat(r0.zw))));
  r0.zw = vd_asuint(((-(vd_asfloat(_ResultTex_TexelSize.zw))) * float2(0.5, 0.5) + abs(vd_asfloat(r0.zw))));
  r1.xy = vd_asuint(max(vd_asfloat(r0.xy), float2(0.0, 0.0)));
  r0.x = vd_asuint(max(vd_asfloat(r0.y), vd_asfloat(r0.x)));
  r0.x = vd_asuint(min(vd_asfloat(r0.x), 0.0));
  r0.y = vd_asuint(dot(vd_asfloat(r1.xy), vd_asfloat(r1.xy)));
  r0.y = vd_asuint(sqrt(vd_asfloat(r0.y)));
  r0.x = vd_asuint((vd_asfloat(r0.x) + vd_asfloat(r0.y)));
  r0.y = vd_asuint(saturate(((-(vd_asfloat(r0.x))) / vd_asfloat(_FillSoft))));
  r0.x = vd_asuint(saturate(((-(vd_asfloat(r0.x))) + 0.5)));
  r0.y = vd_asuint(((-(vd_asfloat(r0.x))) + vd_asfloat(r0.y)));
  r1.x = vd_asuint(saturate(vd_asfloat(_FillSoft)));
  r0.x = vd_asuint((vd_asfloat(r1.x) * vd_asfloat(r0.y) + vd_asfloat(r0.x)));
  r0.y = vd_asuint((vd_asfloat(_ResultTex_TexelSize.y) / vd_asfloat(_ResultTex_TexelSize.x)));
  r1.x = vd_asuint((vd_asfloat(r0.y) * vd_asfloat(r1.z)));
  r1.y = vd_asuint((vd_asfloat(v1.y) + -0.5));
  r1.xy = vd_asuint((vd_asfloat(r1.xy) + vd_asfloat(r1.xy)));
  r0.y = vd_asuint(dot(vd_asfloat(r1.xy), vd_asfloat(r1.xy)));
  r0.y = vd_asuint(sqrt(vd_asfloat(r0.y)));
  r1.x = vd_asuint((vd_asfloat(r0.y) / vd_asfloat(_BorderGradientParams.x)));
  r1.y = vd_asuint(v1.y);
  r1.xyzw = vd_asuint(_BorderFillTex.Sample(sampler_BorderFillTex, vd_asfloat(r1.xy)).wxyz);
  r1.yzw = vd_asuint((vd_asfloat(r1.xxx) * vd_asfloat(r1.yzw)));
  r1.xyzw = vd_asuint((vd_asfloat(r1.xyzw) * vd_asfloat(_BorderColor.wwww)));
  r2.xyz = vd_asuint((vd_asfloat(_FillColor.www) * vd_asfloat(_FillColor.xyz)));
  r3.xyzw = vd_asuint(_SourceTex.Sample(sampler_SourceTex, vd_asfloat(v1.xy)).xyzw);
  r0.y = vd_asuint(((-(vd_asfloat(r3.w))) + 1.0));
  r2.yzw = vd_asuint((vd_asfloat(r0.yyy) * vd_asfloat(r2.xyz) + vd_asfloat(r3.xyz)));
  r2.x = vd_asuint((vd_asfloat(r0.y) * vd_asfloat(_FillColor.w) + vd_asfloat(r3.w)));
  r3.xy = vd_asuint(saturate(vd_asfloat(_BorderSizeSoft.yx)));
  r2.xyzw = vd_asuint(((-(vd_asfloat(r1.xyzw))) * vd_asfloat(r3.yyyy) + vd_asfloat(r2.xyzw)));
  r1.xyzw = vd_asuint((vd_asfloat(r1.xyzw) * vd_asfloat(r3.yyyy)));
  r1.xyzw = vd_asuint((vd_asfloat(r0.xxxx) * vd_asfloat(r2.xyzw) + vd_asfloat(r1.xyzw)));
  r0.xy = vd_asuint(max(vd_asfloat(r0.zw), float2(0.0, 0.0)));
  r0.z = vd_asuint(max(vd_asfloat(r0.w), vd_asfloat(r0.z)));
  r0.z = vd_asuint(min(vd_asfloat(r0.z), 0.0));
  r0.x = vd_asuint(dot(vd_asfloat(r0.xy), vd_asfloat(r0.xy)));
  r0.x = vd_asuint(sqrt(vd_asfloat(r0.x)));
  r0.x = vd_asuint((vd_asfloat(r0.z) + vd_asfloat(r0.x)));
  r0.y = vd_asuint(saturate(((-(vd_asfloat(r0.x))) / vd_asfloat(_BorderSizeSoft.y))));
  r0.x = vd_asuint(saturate(((-(vd_asfloat(r0.x))) + 0.5)));
  r0.y = vd_asuint(((-(vd_asfloat(r0.x))) + vd_asfloat(r0.y)));
  r0.x = vd_asuint((vd_asfloat(r3.x) * vd_asfloat(r0.y) + vd_asfloat(r0.x)));
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
