// Hidden/ChocDino/UIFX/Blend-Outline | pass 0 "Blend-Outline" | progFragment | keywords: DIR_OUTSIDE DISTANCEMAP UNITY_UI_ALPHACLIP UNITY_UI_CLIP_RECT | blob 53
void frag(
  float4 v0 : SV_POSITION0,
  float4 v1 : VDREG1,
  float4 v2 : VDREG2,
  float4 v3 : VDREG3,
  out float4 o0 : SV_Target0)
{
  uint4 r0, r1, r2;
  r0 = 0u; r1 = 0u; r2 = 0u;

  r0.xyzw = vd_asuint(_ResultTex.Sample(sampler_ResultTex, vd_asfloat(v1.xy)).xyzw);
  r0.x = vd_asuint(((-(vd_asfloat(r0.x))) + vd_asfloat(_Size.x)));
  r0.y = vd_asuint(saturate((vd_asfloat(r0.x) + 0.5)));
  r0.y = vd_asuint((vd_asfloat(r0.y) * vd_asfloat(_Size.y)));
  r0.y = vd_asuint((1.0 / vd_asfloat(r0.y)));
  r0.x = vd_asuint(saturate((vd_asfloat(r0.y) * vd_asfloat(r0.x))));
  r0.y = vd_asuint((vd_asfloat(r0.x) * -2.0 + 3.0));
  r0.x = vd_asuint((vd_asfloat(r0.x) * vd_asfloat(r0.x)));
  r0.x = vd_asuint((vd_asfloat(r0.x) * vd_asfloat(r0.y)));
  r1.xyzw = vd_asuint(_SourceTex.Sample(sampler_SourceTex, vd_asfloat(v1.xy)).xyzw);
  r0.y = vd_asuint(((-(vd_asfloat(r1.w))) + 1.0));
  r0.x = vd_asuint((vd_asfloat(r0.y) * vd_asfloat(r0.x)));
  r0.yzw = vd_asuint((vd_asfloat(_OutlineColor.xyz) * float3(0.305306017, 0.305306017, 0.305306017) + float3(0.682171106, 0.682171106, 0.682171106)));
  r0.yzw = vd_asuint((vd_asfloat(_OutlineColor.xyz) * vd_asfloat(r0.yzw) + float3(0.0125228781, 0.0125228781, 0.0125228781)));
  r0.yzw = vd_asuint((vd_asfloat(r0.yzw) * vd_asfloat(_OutlineColor.xyz)));
  r2.yzw = vd_asuint(saturate((vd_asfloat(r0.yzw) * vd_asfloat(_OutlineColor.www))));
  r2.x = vd_asuint(saturate(vd_asfloat(_OutlineColor.w)));
  r0.xyzw = vd_asuint((vd_asfloat(r0.xxxx) * vd_asfloat(r2.xyzw)));
  r0.xyzw = vd_asuint((vd_asfloat((_SourceAlpha).xxxx) * vd_asfloat(r1.wxyz) + vd_asfloat(r0.xyzw)));
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
