// Hidden/ChocDino/UIFX/Blend-Glow | pass 0 "Blend-Glow" | progFragment | keywords: DIR_BOTH UNITY_UI_ALPHACLIP UNITY_UI_CLIP_RECT USE_CURVE_FALLOFF | blob 95
void frag(
  float4 v0 : SV_POSITION0,
  float4 v1 : VDREG1,
  float4 v2 : VDREG2,
  float4 v3 : VDREG3,
  out float4 o0 : SV_Target0)
{
  uint4 r0, r1, r2, r3;
  r0 = 0u; r1 = 0u; r2 = 0u; r3 = 0u;

  r0.xy = vd_asuint((vd_asfloat(v1.xy) * float2(264.554596, 264.554596)));
  r0.z = vd_asuint((vd_asfloat(_Time.x) * 0.103100002));
  r0.xyz = vd_asuint(frac(vd_asfloat(r0.xyz)));
  r1.xyz = vd_asuint((vd_asfloat(r0.yzx) + float3(19.1900005, 19.1900005, 19.1900005)));
  r0.w = vd_asuint(dot(vd_asfloat(r0.xyz), vd_asfloat(r1.xyz)));
  r0.xyz = vd_asuint((vd_asfloat(r0.www) + vd_asfloat(r0.xyz)));
  r0.x = vd_asuint((vd_asfloat(r0.y) + vd_asfloat(r0.x)));
  r0.x = vd_asuint((vd_asfloat(r0.z) * vd_asfloat(r0.x)));
  r0.x = vd_asuint(frac(vd_asfloat(r0.x)));
  r0.x = vd_asuint((vd_asfloat(r0.x) + -0.5));
  r1.xyzw = vd_asuint(_ResultTex.Sample(sampler_ResultTex, vd_asfloat(v1.xy)).xyzw);
  r0.y = vd_asuint(saturate((abs(vd_asfloat(r1.x)) / vd_asfloat(_MaxDistance))));
  r0.y = vd_asuint(((-(vd_asfloat(r0.y))) + 1.0));
  r0.y = vd_asuint(log2(vd_asfloat(r0.y)));
  r0.y = vd_asuint((vd_asfloat(r0.y) * vd_asfloat(_FalloffParams.w)));
  r0.y = vd_asuint(exp2(vd_asfloat(r0.y)));
  r1.x = vd_asuint(((-(vd_asfloat(r0.y))) + 1.0));
  r1.y = 0x00000000u;
  r1.xyzw = vd_asuint(_FalloffTex.Sample(sampler_FalloffTex, vd_asfloat(r1.xy)).xyzw);
  r1.x = vd_asuint(saturate(vd_asfloat(r1.x)));
  r0.x = vd_asuint((vd_asfloat(r0.x) * vd_asfloat(r1.x)));
  r0.x = vd_asuint((vd_asfloat(r0.x) * 0.100000001 + vd_asfloat(r1.x)));
  r1.xyzw = vd_asuint((vd_asfloat(r0.xxxx) * vd_asfloat(_GlowColor.xyzw)));
  r1.xyzw = vd_asuint((vd_asfloat(r1.xyzw) * float4(-1.44269502, -1.44269502, -1.44269502, -1.44269502)));
  r1.xyzw = vd_asuint(exp2(vd_asfloat(r1.xyzw)));
  r1.xyzw = vd_asuint(((-(vd_asfloat(r1.xyzw))) + float4(1.0, 1.0, 1.0, 1.0)));
  r0.y = vd_asuint(((-(vd_asfloat(r1.w))) + 1.0));
  r2.xyzw = vd_asuint(_SourceTex.Sample(sampler_SourceTex, vd_asfloat(v1.xy)).xyzw);
  r3.xyzw = vd_asuint((vd_asfloat(r2.xyzw) * vd_asfloat((_SourceAlpha).xxxx)));
  r2.xyzw = vd_asuint((vd_asfloat((_SourceAlpha).xxxx) * vd_asfloat(r2.xyzw) + vd_asfloat(r1.xyzw)));
  r1.xyzw = vd_asuint((vd_asfloat(r0.yyyy) * vd_asfloat(r3.xyzw) + vd_asfloat(r1.xyzw)));
  r2.xyzw = vd_asuint(((-(vd_asfloat(r1.xyzw))) + vd_asfloat(r2.xyzw)));
  r1.xyzw = vd_asuint((vd_asfloat((_AdditiveFactor).xxxx) * vd_asfloat(r2.xyzw) + vd_asfloat(r1.xyzw)));
  r0.y = vd_asuint(((-(vd_asfloat(r1.w))) + 1.0));
  r0.x = vd_asuint(min(vd_asfloat(r0.y), vd_asfloat(r0.x)));
  r0.x = vd_asuint(((-(vd_asfloat(r0.x))) * vd_asfloat(_AdditiveFactor) + vd_asfloat(r1.w)));
  r0.yz = vd_asuint(((-(vd_asfloat(_ClipRect.xy))) + vd_asfloat(_ClipRect.zw)));
  r0.yz = vd_asuint((vd_asfloat(r0.yz) + (-(abs(vd_asfloat(v3.xy))))));
  r0.yz = vd_asuint(saturate((vd_asfloat(r0.yz) * vd_asfloat(v3.zw))));
  r0.y = vd_asuint((vd_asfloat(r0.z) * vd_asfloat(r0.y)));
  r0.z = vd_asuint((vd_asfloat(r0.x) * vd_asfloat(r0.y) + -0.00100000005));
  r2.w = vd_asuint((vd_asfloat(r0.y) * vd_asfloat(r0.x)));
  r0.xyw = vd_asuint((vd_asfloat(r0.yyy) * vd_asfloat(r1.xyz)));
  r2.xyz = vd_asuint((vd_asfloat(r0.xyw) * vd_asfloat(v2.www)));
  o0.xyzw = (vd_asfloat(r2.xyzw) * vd_asfloat(v2.xyzw));
  r0.x = vd_asuint((-(int)(vd_asfloat(r0.z) < 0.0)));
  if (r0.x != 0u) discard;
  return;
}
