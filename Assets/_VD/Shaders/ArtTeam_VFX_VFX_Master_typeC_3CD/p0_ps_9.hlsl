// ArtTeam/VFX/VFX_Master_typeC_3CD | pass 0 "Universal Forward" | progFragment | keywords: _DBUFFER_MRT3 | blob 9
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
  float4 r0,r1;
  uint4 bitmask, uiDest;
  float4 fDest;

  r0.x = saturate(v2.w);
  r0.xy = float2(1,2) + -r0.xx;
  r0.y = r0.y + r0.x;
  r0.z = 1 + -v3.w;
  r0.w = r0.z * r0.y;
  r0.x = r0.x * -2 + r0.w;
  r0.y = r0.z * r0.y + -r0.x;
  r0.y = 1 / r0.y;
  r0.zw = v1.xy * _MainTex_ST.xy + _MainTex_ST.zw;
  r1.xyzw = _MainTex.SampleBias(sampler_MainTex, r0.zw, _GlobalMipBias.x).xyzw;
  r0.x = r1.z + -r0.x;
  r0.x = saturate(r0.x * r0.y);
  r0.y = r0.x * -2 + 3;
  r0.x = r0.x * r0.x;
  r0.x = r0.y * r0.x;
  r0.y = r1.x + -r1.w;
  r0.y = v4.w * r0.y + r1.w;
  r1.xyz = v4.xyz * r0.yyy;
  r0.yzw = -v4.xyz * r0.yyy + v3.xyz;
  r0.xyz = r0.xxx * r0.yzw + r1.xyz;
  r0.xyz = v2.zzz * r0.xyz;
  r1.xy = (int2)v0.xy;
  r1.zw = float2(0,0);
  r1.xyzw = _DBufferTexture0.Load(r1.xyz).xyzw;
  o0.xyz = r0.xyz * r1.www + r1.xyz;
  o0.w = 1;
  return;
}
