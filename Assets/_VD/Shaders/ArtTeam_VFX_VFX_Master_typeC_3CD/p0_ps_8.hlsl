// ArtTeam/VFX/VFX_Master_typeC_3CD | pass 0 "Universal Forward" | progFragment | keywords: FOG_LINEAR _SURFACE_TYPE_TRANSPARENT | blob 8
void frag(
  float4 v0 : SV_POSITION0,
  float4 v1 : INTERP0,
  float4 v2 : INTERP1,
  float4 v3 : INTERP2,
  float4 v4 : INTERP3,
  float3 v5 : INTERP4,
  float3 v6 : INTERP5,
  out float4 o0 : SV_Target0)
{
  float4 r0,r1,r2,r3;
  uint4 bitmask, uiDest;
  float4 fDest;

  r0.x = 1 + -v3.w;
  r0.yzw = saturate(v2.wyx);
  r1.x = 1 + -r0.y;
  r0.y = r1.x + -r0.y;
  r0.y = 2 + r0.y;
  r1.y = r0.x * r0.y;
  r1.x = r1.x * -2 + r1.y;
  r0.x = r0.x * r0.y + -r1.x;
  r0.x = 1 / r0.x;
  r1.yz = v1.xy * _MainTex_ST.xy + _MainTex_ST.zw;
  r2.xyzw = _MainTex.SampleBias(sampler_MainTex, r1.yz, _GlobalMipBias.x).xyzw;
  r0.y = r2.z + -r1.x;
  r0.x = saturate(r0.y * r0.x);
  r0.y = r0.x * -2 + 3;
  r0.x = r0.x * r0.x;
  r0.x = r0.y * r0.x;
  r0.y = r2.x + -r2.w;
  r0.y = v4.w * r0.y + r2.w;
  r1.xyz = v4.xyz * r0.yyy;
  r3.xyz = -v4.xyz * r0.yyy + v3.xyz;
  r1.xyz = r0.xxx * r3.xyz + r1.xyz;
  o0.xyz = v2.zzz * r1.xyz;
  r0.x = -r0.z * 0.5 + 1;
  r0.y = r0.z * 0.5 + -1;
  r1.x = r0.x + -r0.y;
  r0.x = -r0.z * 0.5 + r0.x;
  r0.x = 1 / r0.x;
  r0.y = r0.w * r1.x + r0.y;
  r0.w = 1 + -r2.y;
  r0.y = r0.w + r0.y;
  r0.y = -r0.z * 0.5 + r0.y;
  r0.x = saturate(r0.y * r0.x);
  r0.y = r0.x * -2 + 3;
  r0.x = r0.x * r0.x;
  o0.w = saturate(-r0.y * r0.x + r2.w);
  return;
}
