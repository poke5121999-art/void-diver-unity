// ArtTeam/VFX/VFX_Master_typeB_Xpan | pass 0 "Universal Forward" | progFragment | keywords: _DBUFFER_MRT3 _SURFACE_TYPE_TRANSPARENT | blob 17
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
  float4 r0,r1,r2;
  uint4 bitmask, uiDest;
  float4 fDest;

  r0.x = saturate(_InvertB);
  r0.yz = float2(1,1) / _ScreenParams.xy;
  r0.yz = (_Pixelation).xx * r0.yz;
  r1.xy = v1.xy / r0.yz;
  r1.xy = floor(r1.xy);
  r2.xz = v2.zw;
  r2.yw = float2(0,1);
  r0.yz = r1.xy * r0.yz + r2.xy;
  r0.yz = r0.yz * r2.zw;
  r0.yz = r0.yz * _MainTex_ST.xy + _MainTex_ST.zw;
  r1.xyzw = _MainTex.SampleBias(sampler_MainTex, r0.yz, _GlobalMipBias.x).xyzw;
  r0.y = r1.z * -2 + 1;
  r0.x = r0.x * r0.y + r1.z;
  r0.x = v3.w * r0.x;
  r0.yzw = v4.xyz * r1.xxx;
  r2.xyz = -v4.xyz * r1.xxx + v3.xyz;
  r0.xyz = r0.xxx * r2.xyz + r0.yzw;
  o0.xyz = v2.yyy * r0.xyz;
  r0.x = 1 + -r1.y;
  r0.y = saturate(v2.x);
  r0.z = saturate(_DissolveSharpness);
  r0.w = -r0.z * 0.5 + 1;
  r1.x = r0.z * 0.5 + -1;
  r1.y = -r1.x + r0.w;
  r0.w = -r0.z * 0.5 + r0.w;
  r0.w = 1 / r0.w;
  r0.y = r0.y * r1.y + r1.x;
  r0.x = r0.x + r0.y;
  r0.x = -r0.z * 0.5 + r0.x;
  r0.x = saturate(r0.x * r0.w);
  r0.y = r0.x * -2 + 3;
  r0.x = r0.x * r0.x;
  r0.x = saturate(-r0.y * r0.x + r1.w);
  r0.x = v4.w * r0.x;
  r0.yz = v1.xy * _MaskTex_ST.xy + _MaskTex_ST.zw;
  r1.xyzw = _MaskTex.SampleBias(sampler_MaskTex, r0.yz, _GlobalMipBias.x).xyzw;
  r0.y = r1.x * r1.w;
  o0.w = r0.x * r0.y;
  return;
}
