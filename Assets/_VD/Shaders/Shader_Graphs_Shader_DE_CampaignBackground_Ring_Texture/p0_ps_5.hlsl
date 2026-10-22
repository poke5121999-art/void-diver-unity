// Shader Graphs/Shader_DE_CampaignBackground_Ring_Texture | pass 0 "Universal Forward" | progFragment | keywords: FOG_LINEAR | blob 5
void frag(
  float4 v0 : SV_POSITION0,
  float4 v1 : INTERP0,
  float3 v2 : INTERP1,
  float3 v3 : INTERP2,
  out float4 o0 : SV_Target0)
{
  float4 r0;
  uint4 bitmask, uiDest;
  float4 fDest;

  r0.xyzw = _BaseMap.SampleBias(sampler_BaseMap, v1.xy, _GlobalMipBias.x).xyzw;
  r0.w = log2(abs(r0.w));
  r0.xyz = _Color.xyz * r0.xyz;
  o0.xyz = float3(4,4,4) * r0.xyz;
  r0.x = _Pow * r0.w;
  o0.w = exp2(r0.x);
  return;
}
