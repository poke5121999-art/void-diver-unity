// Shader Graphs/Shader_DE_CampaignBackground_Ring | pass 4 "Meta" | progFragment | keywords: (none) | blob 31
void frag(
  float4 v0 : SV_POSITION0,
  float4 v1 : INTERP0,
  float4 v2 : INTERP1,
  float4 v3 : INTERP2,
  out float4 o0 : SV_TARGET0)
{
  float4 r0,r1,r2;
  uint4 bitmask, uiDest;
  float4 fDest;

  r0.x = saturate(unity_OneOverOutputBoost);
  r0.yzw = (_Intensity).xxx * _Color.xyz;
  r1.xyz = log2(abs(r0.yzw));
  r1.xyz = r1.xyz * r0.xxx;
  r1.xyz = exp2(r1.xyz);
  r1.xyz = min((unity_MaxOutputValue).xxx, r1.xyz);
  r1.w = 1;
  r1.xyzw = unity_MetaFragmentControl.xxxx ? r1.xyzw : float4(0,0,0,0);
  r2.xyz = r0.yzw * r0.yzw;
  r0.x = cmp(0 != unity_UseLinearSpace);
  r0.xyz = r0.xxx ? r0.yzw : r2.xyz;
  r0.w = 1;
  o0.xyzw = unity_MetaFragmentControl.yyyy ? r0.xyzw : r1.xyzw;
  return;
}
