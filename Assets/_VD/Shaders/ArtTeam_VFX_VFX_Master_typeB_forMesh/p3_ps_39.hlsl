// ArtTeam/VFX/VFX_Master_typeB_forMesh | pass 3 "DepthNormalsOnly" | progFragment | keywords: _ALPHATEST_ON | blob 39
void frag(
  float4 v0 : SV_POSITION0,
  float4 v1 : INTERP0,
  float3 v2 : INTERP1,
  out float4 o0 : SV_Target0)
{
  float4 r0,r1;
  uint4 bitmask, uiDest;
  float4 fDest;

  r0.xy = v1.xy * _MaskTex_G_Dissolve_B_2ndColor_A_alpha_ST.xy + _MaskTex_G_Dissolve_B_2ndColor_A_alpha_ST.zw;
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
  r0.x = dot(v2.xyz, v2.xyz);
  r0.x = rsqrt(r0.x);
  o0.xyz = v2.xyz * r0.xxx;
  o0.w = 0;
  return;
}
