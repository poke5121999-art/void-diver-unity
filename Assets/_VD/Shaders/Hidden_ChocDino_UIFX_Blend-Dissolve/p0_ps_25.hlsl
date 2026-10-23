// Hidden/ChocDino/UIFX/Blend-Dissolve | pass 0 "Blend-Dissolve" | progFragment | keywords: EDGE_RAMP UNITY_UI_ALPHACLIP | blob 25
void frag(
  float4 v0 : SV_POSITION0,
  float4 v1 : VDREG1,
  float4 v2 : VDREG2,
  out float4 o0 : SV_Target0)
{
  uint4 r0, r1;
  r0 = 0u; r1 = 0u;

  r0.xyzw = vd_asuint(_FillTex.Sample(sampler_FillTex, vd_asfloat(v1.zw)).xyzw);
  r0.x = vd_asuint((vd_asfloat(r0.y) + vd_asfloat(r0.x)));
  r0.x = vd_asuint((vd_asfloat(r0.z) + vd_asfloat(r0.x)));
  r0.x = vd_asuint((vd_asfloat(r0.x) * vd_asfloat(_InvertFactor.y)));
  r0.x = vd_asuint(((-(vd_asfloat(r0.x))) * 0.333333343 + vd_asfloat(_InvertFactor.x)));
  r0.x = vd_asuint((vd_asfloat(r0.x) + (-(vd_asfloat(_Dissolve.x)))));
  r0.y = vd_asuint((1.0 / vd_asfloat(_Dissolve.y)));
  r0.x = vd_asuint(saturate((vd_asfloat(r0.y) * vd_asfloat(r0.x))));
  r0.y = vd_asuint((vd_asfloat(r0.x) * -2.0 + 3.0));
  r0.x = vd_asuint((vd_asfloat(r0.x) * vd_asfloat(r0.x)));
  r0.x = vd_asuint(((-(vd_asfloat(r0.y))) * vd_asfloat(r0.x) + 1.0));
  r0.x = vd_asuint(max(vd_asfloat(r0.x), 0.0));
  r0.y = vd_asuint((vd_asfloat(r0.x) * 100.0));
  r0.xz = vd_asuint((vd_asfloat(r0.xx) + float2(-0.99000001, -0.00999999978)));
  r0.xz = vd_asuint((vd_asfloat(r0.zx) * float2(1.02040815, 100.0)));
  r0.y = vd_asuint(min(vd_asfloat(r0.y), 1.0));
  r0.y = vd_asuint(((-(vd_asfloat(r0.y))) + 1.0));
  r0.w = vd_asuint(max(vd_asfloat(r0.z), 0.0));
  r0.x = vd_asuint(saturate(vd_asfloat(r0.x)));
  r0.w = vd_asuint(max(vd_asfloat(r0.w), vd_asfloat(r0.y)));
  r0.w = vd_asuint(((-(vd_asfloat(r0.w))) + 1.0));
  r0.z = 0x00000000u;
  r1.xyzw = vd_asuint(_EdgeTex.Sample(sampler_EdgeTex, vd_asfloat(r0.xz)).xyzw);
  r1.xyz = vd_asuint((vd_asfloat(r1.xyz) * vd_asfloat((_EdgeEmissive).xxx)));
  r0.xzw = vd_asuint((vd_asfloat(r0.www) * vd_asfloat(r1.xyz)));
  r1.xyzw = vd_asuint(_ResultTex.Sample(sampler_ResultTex, vd_asfloat(v1.xy)).xyzw);
  r0.xyz = vd_asuint((vd_asfloat(r1.xyz) * vd_asfloat(r0.yyy) + vd_asfloat(r0.xzw)));
  r0.w = 0x3f800000u;
  o0.xyzw = (vd_asfloat(r1.wwww) * vd_asfloat(r0.xyzw));
  return;
}
