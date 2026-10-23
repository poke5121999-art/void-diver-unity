// Shader Graphs/Shader_DE_CampaignBackground_Ring_Texture | pass 0 "Universal Forward" | progFragment | keywords: FOG_LINEAR _DBUFFER_MRT3 | blob 7
void frag(
  float4 v0 : SV_POSITION0,
  float4 v1 : VDREG1,
  float4 v2 : VDREG2,
  float4 v3 : VDREG3,
  out float4 o0 : SV_Target0)
{
  uint4 r0;
  r0 = 0u;

  r0.xyzw = vd_asuint(_BaseMap.SampleBias(sampler_BaseMap, vd_asfloat(v1.xy), vd_asfloat(_GlobalMipBias.x)).xyzw);
  r0.w = vd_asuint(log2(abs(vd_asfloat(r0.w))));
  r0.xyz = vd_asuint((vd_asfloat(r0.xyz) * vd_asfloat(_Color.xyz)));
  o0.xyz = (vd_asfloat(r0.xyz) * float3(4.0, 4.0, 4.0));
  r0.x = vd_asuint((vd_asfloat(r0.w) * vd_asfloat(_Pow)));
  o0.w = exp2(vd_asfloat(r0.x));
  return;
}
