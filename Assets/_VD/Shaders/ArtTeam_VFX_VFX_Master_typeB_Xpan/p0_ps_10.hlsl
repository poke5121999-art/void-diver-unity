// ArtTeam/VFX/VFX_Master_typeB_Xpan | pass 0 "Universal Forward" | progFragment | keywords: _ALPHATEST_ON | blob 10
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

  r0.x = saturate(v2.x);
  r0.y = saturate(_DissolveSharpness);
  r0.z = -r0.y * 0.5 + 1;
  r0.w = r0.y * 0.5 + -1;
  r1.x = r0.z + -r0.w;
  r0.z = -r0.y * 0.5 + r0.z;
  r0.z = 1 / r0.z;
  r0.x = r0.x * r1.x + r0.w;
  r1.xy = float2(1,1) / _ScreenParams.xy;
  r1.xy = (_Pixelation).xx * r1.xy;
  r1.zw = v1.xy / r1.xy;
  r1.zw = floor(r1.zw);
  r2.xz = v2.zw;
  r2.yw = float2(0,1);
  r1.xy = r1.zw * r1.xy + r2.xy;
  r1.xy = r1.xy * r2.zw;
  r1.xy = r1.xy * _MainTex_ST.xy + _MainTex_ST.zw;
  r1.xyzw = _MainTex.SampleBias(sampler_MainTex, r1.xy, _GlobalMipBias.x).xyzw;
  r2.xy = float2(1,1) + -r1.zy;
  r0.x = r2.y + r0.x;
  r0.w = r2.x + -r1.z;
  r0.x = -r0.y * 0.5 + r0.x;
  r0.x = saturate(r0.x * r0.z);
  r0.y = r0.x * -2 + 3;
  r0.x = r0.x * r0.x;
  r0.x = saturate(-r0.y * r0.x + r1.w);
  r0.x = v4.w * r0.x;
  r0.yz = v1.xy * _MaskTex_ST.xy + _MaskTex_ST.zw;
  r2.xyzw = _MaskTex.SampleBias(sampler_MaskTex, r0.yz, _GlobalMipBias.x).xyzw;
  r0.y = r2.x * r2.w;
  r0.z = r0.x * r0.y;
  r0.x = r0.x * r0.y + -0.5;
  r0.y = ddx(r0.z);
  r0.z = ddy(r0.z);
  r0.y = abs(r0.y) + abs(r0.z);
  r0.z = -r0.y * 0.5 + r0.x;
  r0.y = max(9.99999975e-005, r0.y);
  r0.y = r0.z / r0.y;
  r0.y = saturate(1 + r0.y);
  r0.z = -9.99999975e-005 + r0.y;
  r1.y = cmp(_AlphaToMaskAvailable != 0.000000);
  r0.x = r1.y ? r0.z : r0.x;
  o0.w = r1.y ? r0.y : 1;
  r0.x = cmp(r0.x < 0);
  if (r0.x != 0) discard;
  r0.x = saturate(_InvertB);
  r0.x = r0.x * r0.w + r1.z;
  r0.x = v3.w * r0.x;
  r0.yzw = v4.xyz * r1.xxx;
  r1.xyz = -v4.xyz * r1.xxx + v3.xyz;
  r0.xyz = r0.xxx * r1.xyz + r0.yzw;
  o0.xyz = v2.yyy * r0.xyz;
  return;
}
