// ArtTeam/VFX/VFX_Master_typeB_Xpan | pass 0 "Universal Forward" | progFragment | keywords: FOG_LINEAR | blob 12
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

  r0.xy = float2(1,1) / _ScreenParams.xy;
  r0.xy = (_Pixelation).xx * r0.xy;
  r0.zw = v1.xy / r0.xy;
  r0.zw = floor(r0.zw);
  r1.xz = v2.zw;
  r1.yw = float2(0,1);
  r0.xy = r0.zw * r0.xy + r1.xy;
  r0.xy = r0.xy * r1.zw;
  r0.xy = r0.xy * _MainTex_ST.xy + _MainTex_ST.zw;
  r0.xyzw = _MainTex.SampleBias(sampler_MainTex, r0.xy, _GlobalMipBias.x).xyzw;
  r0.y = 1 + -r0.z;
  r0.y = r0.y + -r0.z;
  r0.w = saturate(_InvertB);
  r0.y = r0.w * r0.y + r0.z;
  r0.y = v3.w * r0.y;
  r1.xyz = v4.xyz * r0.xxx;
  r0.xzw = -v4.xyz * r0.xxx + v3.xyz;
  r0.xyz = r0.yyy * r0.xzw + r1.xyz;
  o0.xyz = v2.yyy * r0.xyz;
  o0.w = 1;
  return;
}
