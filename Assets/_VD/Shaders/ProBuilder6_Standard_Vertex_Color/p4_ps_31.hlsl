// ProBuilder6/Standard Vertex Color | pass 4 "Meta" | progFragment | keywords: (none) | blob 31
void frag(
  float4 v0 : SV_POSITION0,
  float4 v1 : VDREG1,
  float4 v2 : VDREG2,
  float4 v3 : VDREG3,
  float4 v4 : VDREG4,
  out float4 o0 : SV_TARGET0)
{
  uint4 r0;
  r0 = 0u;

  r0.xyzw = vd_asuint(_MainTex.SampleBias(sampler_MainTex, vd_asfloat(v1.xy), vd_asfloat(_GlobalMipBias.x)).xyzw);
  r0.xyz = vd_asuint((vd_asfloat(r0.xyz) * vd_asfloat(_Color.xyz)));
  r0.xyz = vd_asuint((vd_asfloat(r0.xyz) * vd_asfloat(v4.xyz)));
  r0.xyz = vd_asuint(log2(abs(vd_asfloat(r0.xyz))));
  r0.w = vd_asuint(saturate(vd_asfloat(unity_OneOverOutputBoost)));
  r0.xyz = vd_asuint((vd_asfloat(r0.xyz) * vd_asfloat(r0.www)));
  r0.xyz = vd_asuint(exp2(vd_asfloat(r0.xyz)));
  r0.xyz = vd_asuint(min(vd_asfloat(r0.xyz), vd_asfloat((unity_MaxOutputValue).xxx)));
  r0.w = 0x3f800000u;
  r0.xyzw = ((vd_asuint(unity_MetaFragmentControl.xxxx) != 0u) ? r0.xyzw : uint4(0x00000000u, 0x00000000u, 0x00000000u, 0x00000000u));
  o0.xyzw = vd_asfloat(((vd_asuint(unity_MetaFragmentControl.yyyy) != 0u) ? uint4(0x00000000u, 0x00000000u, 0x00000000u, 0x3f800000u) : r0.xyzw));
  return;
}
