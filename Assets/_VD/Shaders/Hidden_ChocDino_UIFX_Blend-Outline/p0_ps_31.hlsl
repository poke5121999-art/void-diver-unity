// Hidden/ChocDino/UIFX/Blend-Outline | pass 0 "Blend-Outline" | progFragment | keywords: DIR_BOTH DISTANCEMAP | blob 31
void frag(
  float4 v0 : SV_POSITION0,
  float4 v1 : VDREG1,
  float4 v2 : VDREG2,
  out float4 o0 : SV_Target0)
{
  uint4 r0, r1, r2;
  r0 = 0u; r1 = 0u; r2 = 0u;

  r0.xyz = vd_asuint((vd_asfloat(_OutlineColor.xyz) * float3(0.305306017, 0.305306017, 0.305306017) + float3(0.682171106, 0.682171106, 0.682171106)));
  r0.xyz = vd_asuint((vd_asfloat(_OutlineColor.xyz) * vd_asfloat(r0.xyz) + float3(0.0125228781, 0.0125228781, 0.0125228781)));
  r0.xyz = vd_asuint((vd_asfloat(r0.xyz) * vd_asfloat(_OutlineColor.xyz)));
  r0.yzw = vd_asuint(saturate((vd_asfloat(r0.xyz) * vd_asfloat(_OutlineColor.www))));
  r1.xyzw = vd_asuint(_ResultTex.Sample(sampler_ResultTex, vd_asfloat(v1.xy)).xyzw);
  r1.x = vd_asuint(((-(vd_asfloat(r1.x))) + vd_asfloat(_Size.x)));
  r1.y = vd_asuint(saturate((vd_asfloat(r1.x) + 0.5)));
  r1.y = vd_asuint((vd_asfloat(r1.y) * vd_asfloat(_Size.y)));
  r1.y = vd_asuint((1.0 / vd_asfloat(r1.y)));
  r1.x = vd_asuint(saturate((vd_asfloat(r1.y) * vd_asfloat(r1.x))));
  r1.y = vd_asuint((vd_asfloat(r1.x) * -2.0 + 3.0));
  r1.x = vd_asuint((vd_asfloat(r1.x) * vd_asfloat(r1.x)));
  r1.x = vd_asuint((vd_asfloat(r1.x) * vd_asfloat(r1.y)));
  r0.x = vd_asuint(saturate(vd_asfloat(_OutlineColor.w)));
  r2.xyzw = vd_asuint((vd_asfloat(r0.xyzw) * vd_asfloat(r1.xxxx)));
  r0.x = vd_asuint(((-(vd_asfloat(r0.x))) * vd_asfloat(r1.x) + 1.0));
  r1.xyzw = vd_asuint(_SourceTex.Sample(sampler_SourceTex, vd_asfloat(v1.xy)).xyzw);
  r1.xyzw = vd_asuint((vd_asfloat(r1.wxyz) * vd_asfloat((_SourceAlpha).xxxx)));
  r0.xyzw = vd_asuint((vd_asfloat(r0.xxxx) * vd_asfloat(r1.yzwx) + vd_asfloat(r2.yzwx)));
  r0.xyz = vd_asuint((vd_asfloat(r0.xyz) * vd_asfloat(v2.www)));
  o0.xyzw = (vd_asfloat(r0.xyzw) * vd_asfloat(v2.xyzw));
  return;
}
