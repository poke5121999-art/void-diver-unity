// LCArt/UI/Shader_DE_UI_Gamma | pass 0 "Default" | progFragment | keywords: UNITY_UI_ALPHACLIP | blob 8
void frag(
  float4 v0 : SV_POSITION0,
  float4 v1 : VDREG1,
  float4 v2 : VDREG2,
  float4 v3 : VDREG3,
  out float4 o0 : SV_Target0)
{
  uint4 r0, r1, r2;
  r0 = 0u; r1 = 0u; r2 = 0u;

  r0.xyzw = vd_asuint(_MainTex.SampleBias(sampler_MainTex, vd_asfloat(v2.xy), vd_asfloat(_GlobalMipBias.x)).xyzw);
  r0.xyzw = vd_asuint((vd_asfloat(r0.xyzw) + vd_asfloat(_TextureSampleAdd.xyzw)));
  r1.x = vd_asuint((vd_asfloat(r0.w) * vd_asfloat(v1.w) + -0.00100000005));
  r0.xyzw = vd_asuint((vd_asfloat(r0.xyzw) * vd_asfloat(v1.xyzw)));
  r1.x = vd_asuint((-(int)(vd_asfloat(r1.x) < 0.0)));
  if (r1.x != 0u) discard;
  r1.xyz = vd_asuint((-(int3)(float3(0.0, 0.0, 0.0) < vd_asfloat(r0.xyz))));
  r2.xyz = vd_asuint((-(int3)(vd_asfloat(r0.xyz) < float3(0.0, 0.0, 0.0))));
  r1.xyz = vd_asuint(((-(vd_asint(r1.xyz))) + vd_asint(r2.xyz)));
  r1.xyz = vd_asuint(((float3)vd_asint(r1.xyz)));
  r0.xyz = vd_asuint(log2(abs(vd_asfloat(r0.xyz))));
  o0.w = vd_asfloat(r0.w);
  r0.w = vd_asuint(max(vd_asfloat(_GammaDegree), 0.00100000005));
  r0.w = vd_asuint((1.0 / vd_asfloat(r0.w)));
  r0.xyz = vd_asuint((vd_asfloat(r0.xyz) * vd_asfloat(r0.www)));
  r0.xyz = vd_asuint(exp2(vd_asfloat(r0.xyz)));
  o0.xyz = (vd_asfloat(r0.xyz) * vd_asfloat(r1.xyz));
  return;
}
