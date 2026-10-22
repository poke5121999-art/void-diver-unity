// ArtTeam/VFX/VFX_Master_typeB | pass 3 "DepthNormalsOnly" | progFragment | keywords: _ALPHATEST_ON _PLANESYSTEMCULLINGTYPE_ALWAYS _SURFACE_TYPE_TRANSPARENT | blob 70
void frag(
  float4 v0 : SV_POSITION0,
  float4 v1 : INTERP0,
  float4 v2 : INTERP1,
  float4 v3 : INTERP2,
  float3 v4 : INTERP3,
  float3 v5 : INTERP4,
  out float4 o0 : SV_Target0)
{
  float4 r0,r1;
  uint4 bitmask, uiDest;
  float4 fDest;

  r0.xy = float2(1,1) / _ScreenParams.xy;
  r0.xy = (_Pixelation).xx * r0.xy;
  r0.zw = v1.xy / r0.xy;
  r0.zw = floor(r0.zw);
  r0.xy = r0.zw * r0.xy;
  r0.xy = r0.xy * _MainTex_ST.xy + _MainTex_ST.zw;
  r0.xyzw = _MainTex.SampleBias(sampler_MainTex, r0.xy, _GlobalMipBias.x).xyzw;
  r0.x = 1 + -r0.y;
  r0.yz = saturate(v2.yx);
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
  r0.x = v3.w * r0.x;
  r0.yz = v1.xy * _MaskTex_ST.xy + _MaskTex_ST.zw;
  r1.xyzw = _MaskTex.SampleBias(sampler_MaskTex, r0.yz, _GlobalMipBias.x).xyzw;
  r0.y = r1.x * r1.w;
  r0.x = saturate(r0.x * r0.y);
  r0.x = -_AlphaClipThreshold + r0.x;
  r0.x = cmp(r0.x < 0);
  if (r0.x != 0) discard;
  r0.x = dot(v5.xyz, v5.xyz);
  r0.x = rsqrt(r0.x);
  o0.xyz = v5.xyz * r0.xxx;
  o0.w = 0;
  return;
}
