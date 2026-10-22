// ArtTeam/VFX/VFX_Master_typeB | pass 1 "DepthOnly" | progFragment | keywords: _ALPHATEST_ON _PLANESYSTEMCULLINGTYPE_EQUAL | blob 53
void frag(
  float4 v0 : SV_POSITION0,
  float4 v1 : INTERP0,
  float4 v2 : INTERP1,
  float4 v3 : INTERP2,
  float3 v4 : INTERP3,
  out float4 o0 : SV_TARGET0)
{
  float4 r0,r1,r2;
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
  r0.x = r0.x * r0.y;
  r0.y = -0.200000003 + -_WorldSpaceCameraPos.y;
  r1.xyz = -_WorldSpaceCameraPos.xyz + v4.xyz;
  r0.y = r0.y / r1.y;
  r0.yz = r1.xz * r0.yy + _WorldSpaceCameraPos.xz;
  r0.yz = _PlaneBoundMaxPos.xz + -r0.yz;
  r1.xy = _PlaneBoundMaxPos.xz + -_PlaneBoundMinPos.xz;
  r0.yz = r0.yz / r1.xy;
  r1.xyzw = _PlanePointBuffer.SampleBias(sampler_PlanePointBuffer, r0.yz, _GlobalMipBias.x).xyzw;
  r2.xyzw = _PlaneSightRenderTarget.SampleBias(sampler_PlaneSightRenderTarget, r0.yz, _GlobalMipBias.x).xyzw;
  r0.y = _PlaneSystemUsingLightBuffer * _GlobalPlanePointBufferEnable;
  r0.y = r1.x * r0.y;
  r0.y = max(r2.w, r0.y);
  r0.y = -1 + r0.y;
  r0.y = _GlobalPlaneSystemEnable * r0.y;
  r0.y = _GlobalIsRuntime * r0.y + 1;
  r0.x = saturate(r0.x * r0.y);
  r0.x = -_AlphaClipThreshold + r0.x;
  r0.x = cmp(r0.x < 0);
  if (r0.x != 0) discard;
  o0.xyzw = v0.zzzz;
  return;
}
