// LCArt/UI/Shader_UI_Sharpness | pass 0 "Default" | progFragment | keywords: (none) | blob 7
void frag(
  float4 v0 : SV_POSITION0,
  float4 v1 : VDREG1,
  float4 v2 : VDREG2,
  float4 v3 : VDREG3,
  out float4 o0 : SV_Target0)
{
  uint4 r0, r1;
  r0 = 0u; r1 = 0u;

  r0.x = vd_asuint(((-(vd_asfloat(_SharpnessMin))) + vd_asfloat(_SharpnessMax)));
  r0.x = vd_asuint((1.0 / vd_asfloat(r0.x)));
  r1.xyzw = vd_asuint(_MainTex.Sample(sampler_MainTex, vd_asfloat(v2.xy)).xyzw);
  r1.xyzw = vd_asuint((vd_asfloat(r1.xyzw) + vd_asfloat(_TextureSampleAdd.xyzw)));
  r0.y = vd_asuint((vd_asfloat(r1.w) * vd_asfloat(v1.w) + (-(vd_asfloat(_SharpnessMin)))));
  r1.xyz = vd_asuint((vd_asfloat(r1.xyz) * vd_asfloat(v1.xyz)));
  o0.xyz = vd_asfloat(r1.xyz);
  r0.x = vd_asuint(saturate((vd_asfloat(r0.x) * vd_asfloat(r0.y))));
  r0.y = vd_asuint((vd_asfloat(r0.x) * -2.0 + 3.0));
  r0.x = vd_asuint((vd_asfloat(r0.x) * vd_asfloat(r0.x)));
  o0.w = (vd_asfloat(r0.x) * vd_asfloat(r0.y));
  return;
}
