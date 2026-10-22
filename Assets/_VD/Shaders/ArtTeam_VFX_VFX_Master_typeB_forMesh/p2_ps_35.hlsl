// ArtTeam/VFX/VFX_Master_typeB_forMesh | pass 2 "MotionVectors" | progFragment | keywords: _ALPHATEST_ON | blob 35
void frag(
  float3 v0 : CLIP_POSITION_NO_JITTER0,
  float3 v1 : PREVIOUS_CLIP_POSITION_NO_JITTER0,
  float4 v2 : SV_POSITION0,
  float4 v3 : INTERP0,
  out float4 o0 : SV_Target0)
{
  float4 r0,r1;
  uint4 bitmask, uiDest;
  float4 fDest;

  r0.xy = v3.xy * _MaskTex_G_Dissolve_B_2ndColor_A_alpha_ST.xy + _MaskTex_G_Dissolve_B_2ndColor_A_alpha_ST.zw;
  r0.xyzw = _MaskTex_G_Dissolve_B_2ndColor_A_alpha.SampleBias(sampler_MaskTex_G_Dissolve_B_2ndColor_A_alpha, r0.xy, _GlobalMipBias.x).xyzw;
  r0.x = 1 + -r0.y;
  r0.yz = saturate(float2(_DissolveSharpness, _Dissolve));
  r1.x = -r0.y * 0.5 + 1;
  r1.y = r0.y * 0.5 + -1;
  r1.z = r1.x + -r1.y;
  r1.x = -r0.y * 0.5 + r1.x;
  r1.x = 1 / r1.x;
  r0.z = r0.z * r1.z + r1.y;
  r0.x = r0.x + r0.z;
  r0.x = -r0.y * 0.5 + r0.x;
  r0.x = saturate(r0.x * r1.x);
  r0.y = r0.x * -2 + 3;
  r0.x = r0.x * r0.x;
  r0.x = saturate(-r0.y * r0.x + r0.w);
  r0.x = _TintColor.w * r0.x + -0.5;
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
