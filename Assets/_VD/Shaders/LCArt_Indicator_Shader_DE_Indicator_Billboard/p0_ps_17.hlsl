// LCArt/Indicator/Shader_DE_Indicator_Billboard | pass 0 "" | progFragment | keywords: _ADDITIONAL_LIGHTS _DE_EDITOR_CAMERA | blob 17
void frag(
  float4 v0 : VDREG0,
  float4 v1 : VDREG1,
  float4 v2 : SV_POSITION0,
  float4 v3 : VDREG3,
  out float4 o0 : SV_Target0)
{
  uint4 r0, r1;
  r0 = 0u; r1 = 0u;

  r0.x = vd_asuint(saturate(vd_asfloat(_Progress)));
  r0.yzw = vd_asuint(((-(vd_asfloat(_StartColor.xyz))) + vd_asfloat(_EndColor.xyz)));
  r0.xyz = vd_asuint((vd_asfloat(r0.xxx) * vd_asfloat(r0.yzw) + vd_asfloat(_StartColor.xyz)));
  r1.xyzw = vd_asuint(_MainTex.SampleBias(sampler_MainTex, vd_asfloat(v0.xy), vd_asfloat(_GlobalMipBias.x)).xyzw);
  r1.xyzw = vd_asuint(saturate(vd_asfloat(r1.xyzw)));
  o0.xyz = (vd_asfloat(r0.xyz) * vd_asfloat(r1.xyz));
  r0.x = vd_asuint((vd_asfloat(r1.w) * vd_asfloat(r1.w)));
  o0.w = (vd_asfloat(r0.x) * vd_asfloat(r0.x));
  return;
}
