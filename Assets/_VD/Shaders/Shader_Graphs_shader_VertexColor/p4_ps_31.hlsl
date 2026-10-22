// Shader Graphs/shader_VertexColor | pass 4 "Meta" | progFragment | keywords: (none) | blob 31
void frag(
  float4 v0 : SV_POSITION0,
  float4 v1 : INTERP0,
  float4 v2 : INTERP1,
  float4 v3 : INTERP2,
  float4 v4 : INTERP3,
  out float4 o0 : SV_TARGET0)
{
  float4 r0;
  uint4 bitmask, uiDest;
  float4 fDest;

  r0.x = saturate(unity_OneOverOutputBoost);
  r0.yzw = log2(abs(v4.xyz));
  r0.xyz = r0.xxx * r0.yzw;
  r0.xyz = exp2(r0.xyz);
  r0.xyz = min((unity_MaxOutputValue).xxx, r0.xyz);
  r0.w = 1;
  r0.xyzw = unity_MetaFragmentControl.xxxx ? r0.xyzw : float4(0,0,0,0);
  o0.xyzw = unity_MetaFragmentControl.yyyy ? float4(0,0,0,1) : r0.xyzw;
  return;
}
