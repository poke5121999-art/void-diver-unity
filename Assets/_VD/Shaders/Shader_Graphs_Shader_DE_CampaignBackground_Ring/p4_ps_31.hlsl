// Shader Graphs/Shader_DE_CampaignBackground_Ring | pass 4 "Meta" | progFragment | keywords: (none) | blob 31
void frag(
  float4 v0 : SV_POSITION0,
  float4 v1 : VDREG1,
  float4 v2 : VDREG2,
  float4 v3 : VDREG3,
  out float4 o0 : SV_TARGET0)
{
  uint4 r0, r1, r2;
  r0 = 0u; r1 = 0u; r2 = 0u;

  r0.x = vd_asuint(saturate(vd_asfloat(unity_OneOverOutputBoost)));
  r0.yzw = vd_asuint((vd_asfloat(_Color.xyz) * vd_asfloat((_Intensity).xxx)));
  r1.xyz = vd_asuint(log2(abs(vd_asfloat(r0.yzw))));
  r1.xyz = vd_asuint((vd_asfloat(r0.xxx) * vd_asfloat(r1.xyz)));
  r1.xyz = vd_asuint(exp2(vd_asfloat(r1.xyz)));
  r1.xyz = vd_asuint(min(vd_asfloat(r1.xyz), vd_asfloat((unity_MaxOutputValue).xxx)));
  r1.w = 0x3f800000u;
  r1.xyzw = ((vd_asuint(unity_MetaFragmentControl.xxxx) != 0u) ? r1.xyzw : uint4(0x00000000u, 0x00000000u, 0x00000000u, 0x00000000u));
  r2.xyz = vd_asuint((vd_asfloat(r0.yzw) * vd_asfloat(r0.yzw)));
  r0.x = vd_asuint((-(int)(0.0 != vd_asfloat(unity_UseLinearSpace))));
  r0.xyz = ((r0.xxx != 0u) ? r0.yzw : r2.xyz);
  r0.w = 0x3f800000u;
  o0.xyzw = vd_asfloat(((vd_asuint(unity_MetaFragmentControl.yyyy) != 0u) ? r0.xyzw : r1.xyzw));
  return;
}
