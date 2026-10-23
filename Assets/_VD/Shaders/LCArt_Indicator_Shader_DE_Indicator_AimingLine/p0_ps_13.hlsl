// LCArt/Indicator/Shader_DE_Indicator_AimingLine | pass 0 "" | progFragment | keywords: _DE_EDITOR_CAMERA | blob 13
void frag(
  float4 v0 : VDREG0,
  float4 v1 : VDREG1,
  float4 v2 : SV_POSITION0,
  float4 v3 : VDREG3,
  out float4 o0 : SV_Target0)
{
  uint4 r0, r1;
  r0 = 0u; r1 = 0u;

  r0.x = vd_asuint((vd_asfloat(_Progress) + -1.125));
  r0.x = vd_asuint(saturate((vd_asfloat(r0.x) * 8.0)));
  r0.y = vd_asuint((vd_asfloat(r0.x) * -2.0 + 3.0));
  r0.x = vd_asuint((vd_asfloat(r0.x) * vd_asfloat(r0.x)));
  r0.x = vd_asuint(((-(vd_asfloat(r0.y))) * vd_asfloat(r0.x) + 1.0));
  r0.y = vd_asuint(saturate(vd_asfloat(_Progress)));
  r0.z = vd_asuint((vd_asfloat(r0.y) * 8.0));
  r0.z = vd_asuint(min(vd_asfloat(r0.z), 1.0));
  r0.w = vd_asuint((vd_asfloat(r0.z) * -2.0 + 3.0));
  r0.z = vd_asuint((vd_asfloat(r0.z) * vd_asfloat(r0.z)));
  r0.z = vd_asuint((vd_asfloat(r0.z) * vd_asfloat(r0.w)));
  r1.xyzw = vd_asuint(_WeightTex.SampleBias(sampler_WeightTex, vd_asfloat(v0.xy), vd_asfloat(_GlobalMipBias.x)).xyzw);
  r1.xyzw = vd_asuint(saturate(vd_asfloat(r1.xyzw)));
  r0.z = vd_asuint((vd_asfloat(r0.z) * vd_asfloat(r1.w)));
  o0.w = (vd_asfloat(r0.x) * vd_asfloat(r0.z));
  r0.xzw = vd_asuint(((-(vd_asfloat(_StartColor.xyz))) + vd_asfloat(_EndColor.xyz)));
  r0.xyz = vd_asuint((vd_asfloat(r0.yyy) * vd_asfloat(r0.xzw) + vd_asfloat(_StartColor.xyz)));
  o0.xyz = (vd_asfloat(r0.xyz) * vd_asfloat(r1.xyz));
  return;
}
