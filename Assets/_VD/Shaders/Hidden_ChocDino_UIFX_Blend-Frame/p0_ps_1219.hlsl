// Hidden/ChocDino/UIFX/Blend-Frame | pass 0 "Blend-Frame" | progFragment | keywords: BORDER GRADIENT_RADIAL_BORDER UNITY_UI_ALPHACLIP USE_TEXTURE | blob 1219
void frag(
  float4 v0 : SV_POSITION0,
  float4 v1 : VDREG1,
  float4 v2 : VDREG2,
  out float4 o0 : SV_Target0)
{
  uint4 r0, r1, r2, r3;
  r0 = 0u; r1 = 0u; r2 = 0u; r3 = 0u;

  r0.xy = vd_asuint(((-(vd_asfloat(_BorderSizeSoft.xx))) * float2(2.0, 2.0) + vd_asfloat(_ResultTex_TexelSize.zw)));
  r1.xyzw = vd_asuint((vd_asfloat(v1.zwxy) + float4(-0.5, -0.5, -0.5, -0.5)));
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
  r0.y = vd_asuint((vd_asfloat(_BorderSizeSoft.x) + vd_asfloat(_BorderSizeSoft.x)));
  r1.xy = vd_asuint((vd_asfloat(r0.yy) / vd_asfloat(_ResultTex_TexelSize.zw)));
  r1.xy = vd_asuint(((-(vd_asfloat(r1.xy))) + float2(1.0, 1.0)));
  r1.xy = vd_asuint((vd_asfloat(r1.zw) / vd_asfloat(r1.xy)));
  r1.yz = vd_asuint((vd_asfloat(r1.xy) + float2(0.5, 0.5)));
  r1.yw = vd_asuint((vd_asfloat(r1.yz) * vd_asfloat(_GradientAxisParams.xy) + vd_asfloat(_GradientAxisParams.zw)));
  r1.x = vd_asuint((vd_asfloat(r1.w) + vd_asfloat(r1.y)));
  r1.xyzw = vd_asuint(_FillTex.Sample(sampler_FillTex, vd_asfloat(r1.xz)).xyzw);
  r1.xyz = vd_asuint((vd_asfloat(r1.www) * vd_asfloat(r1.xyz)));
  r1.xyzw = vd_asuint((vd_asfloat(r1.xyzw) * vd_asfloat(_FillColor.wwww)));
  r2.xyzw = vd_asuint(_SourceTex.Sample(sampler_SourceTex, vd_asfloat(v1.xy)).xyzw);
  r0.y = vd_asuint(((-(vd_asfloat(r2.w))) + 1.0));
  r1.xyzw = vd_asuint((vd_asfloat(r0.yyyy) * vd_asfloat(r1.xyzw) + vd_asfloat(r2.xyzw)));
  r2.xyz = vd_asuint((vd_asfloat(_BorderColor.www) * vd_asfloat(_BorderColor.xyz)));
  r2.w = vd_asuint(_BorderColor.w);
  r3.xy = vd_asuint(saturate(vd_asfloat(_BorderSizeSoft.yx)));
  r1.xyzw = vd_asuint(((-(vd_asfloat(r2.xyzw))) * vd_asfloat(r3.yyyy) + vd_asfloat(r1.xyzw)));
  r2.xyzw = vd_asuint((vd_asfloat(r2.xyzw) * vd_asfloat(r3.yyyy)));
  r1.xyzw = vd_asuint((vd_asfloat(r0.xxxx) * vd_asfloat(r1.xyzw) + vd_asfloat(r2.xyzw)));
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
  r0.y = vd_asuint((vd_asfloat(r1.w) * vd_asfloat(r0.x) + -0.00100000005));
  r1.xyzw = vd_asuint((vd_asfloat(r0.xxxx) * vd_asfloat(r1.xyzw)));
  r0.x = vd_asuint((-(int)(vd_asfloat(r0.y) < 0.0)));
  if (r0.x != 0u) discard;
  r1.xyz = vd_asuint((vd_asfloat(r1.xyz) * vd_asfloat(v2.www)));
  o0.xyzw = (vd_asfloat(r1.xyzw) * vd_asfloat(v2.xyzw));
  return;
}
