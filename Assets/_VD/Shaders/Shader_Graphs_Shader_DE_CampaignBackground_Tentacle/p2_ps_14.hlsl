// Shader Graphs/Shader_DE_CampaignBackground_Tentacle | pass 2 "MotionVectors" | progFragment | keywords: (none) | blob 14
void frag(
  float3 v0 : CLIP_POSITION_NO_JITTER0,
  float3 v1 : PREVIOUS_CLIP_POSITION_NO_JITTER0,
  float4 v2 : SV_POSITION0,
  float4 v3 : INTERP0,
  out float4 o0 : SV_Target0)
{
  float4 r0;
  uint4 bitmask, uiDest;
  float4 fDest;

  r0.xyzw = _Texture2D.SampleBias(sampler_Texture2D, v3.xy, _GlobalMipBias.x).xyzw;
  r0.x = -0.800000012 + r0.w;
  r0.x = cmp(r0.x < 0);
  if (r0.x != 0) discard;
  r0.x = 1 / v1.z;
  r0.xy = v1.xy * r0.xx;
  r0.z = 1 / v0.z;
  r0.xy = v0.xy * r0.zz + -r0.xy;
  r0.xy = float2(0.5,-0.5) * r0.xy;
  r0.z = cmp(unity_MotionVectorsParams.y != 0.000000);
  o0.xy = r0.zz ? r0.xy : 0;
  o0.zw = float2(0,0);
  return;
}
