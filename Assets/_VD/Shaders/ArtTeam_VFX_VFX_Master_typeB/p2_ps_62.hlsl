// ArtTeam/VFX/VFX_Master_typeB | pass 2 "MotionVectors" | progFragment | keywords: _ALPHATEST_ON _PLANESYSTEMCULLINGTYPE_EQUAL | blob 62
void frag(
  float3 v0 : CLIP_POSITION_NO_JITTER0,
  float3 v1 : PREVIOUS_CLIP_POSITION_NO_JITTER0,
  float4 v2 : SV_POSITION0,
  float4 v3 : INTERP0,
  float4 v4 : INTERP1,
  float4 v5 : INTERP2,
  float3 v6 : INTERP3,
  out float4 o0 : SV_Target0)
{
  float4 r0,r1,r2;
  uint4 bitmask, uiDest;
  float4 fDest;

  r0.xy = float2(1,1) / _ScreenParams.xy;
  r0.xy = (_Pixelation).xx * r0.xy;
  r0.zw = v3.xy / r0.xy;
  r0.zw = floor(r0.zw);
  r0.xy = r0.zw * r0.xy;
  r0.xy = r0.xy * _MainTex_ST.xy + _MainTex_ST.zw;
  r0.xyzw = _MainTex.SampleBias(sampler_MainTex, r0.xy, _GlobalMipBias.x).xyzw;
  r0.x = 1 + -r0.y;
  r0.yz = saturate(v4.yx);
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
  r0.x = v5.w * r0.x;
  r0.yz = v3.xy * _MaskTex_ST.xy + _MaskTex_ST.zw;
  r1.xyzw = _MaskTex.SampleBias(sampler_MaskTex, r0.yz, _GlobalMipBias.x).xyzw;
  r0.y = r1.x * r1.w;
  r0.x = r0.x * r0.y;
  r0.y = -0.200000003 + -_WorldSpaceCameraPos.y;
  r1.xyz = -_WorldSpaceCameraPos.xyz + v6.xyz;
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
