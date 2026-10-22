// ArtTeam/VFX/VFX_Master_typeB_Dither | pass 3 "DepthNormalsOnly" | progFragment | keywords: _ALPHATEST_ON _PLANESYSTEMCULLINGTYPE_EQUAL | blob 49
void frag(
  float4 v0 : SV_POSITION0,
  float4 v1 : INTERP0,
  float4 v2 : INTERP1,
  float4 v3 : INTERP2,
  float3 v4 : INTERP3,
  float3 v5 : INTERP4,
  out float4 o0 : SV_Target0)
{
  float4 r0,r1,r2;
  uint4 bitmask, uiDest;
  float4 fDest;

  float4 x0[16];
  x0[0].x = 0.0588235296;
  x0[1].x = 0.529411793;
  x0[2].x = 0.176470593;
  x0[3].x = 0.647058845;
  x0[4].x = 0.764705896;
  x0[5].x = 0.294117659;
  x0[6].x = 0.882352948;
  x0[7].x = 0.411764711;
  x0[8].x = 0.235294119;
  x0[9].x = 0.70588237;
  x0[10].x = 0.117647059;
  x0[11].x = 0.588235319;
  x0[12].x = 0.941176474;
  x0[13].x = 0.470588237;
  x0[14].x = 0.823529422;
  x0[15].x = 0.352941185;
  r0.x = cmp(_ProjectionParams.x < 0);
  r0.y = _ScaledScreenParams.y + -v0.y;
  r0.y = r0.x ? r0.y : v0.y;
  r0.x = v0.x;
  r0.xy = r0.xy / _ScaledScreenParams.xy;
  r0.z = 1 + -r0.y;
  r0.xy = _ScreenParams.xy * r0.xz;
  r0.xy = (uint2)r0.xy;
  r0.x = (uint)r0.x << 2;
  r0.xy = (int2)r0.xy & int2(12,3);
  r0.x = (int)r0.y + (int)r0.x;
  r0.x = x0[r0.x+0].x;
  r0.yz = float2(1,1) / _ScreenParams.xy;
  r0.yz = (_Pixelation).xx * r0.yz;
  r1.xy = v1.xy / r0.yz;
  r1.xy = floor(r1.xy);
  r0.yz = r1.xy * r0.yz;
  r0.yz = r0.yz * _MainTex_ST.xy + _MainTex_ST.zw;
  r1.xyzw = _MainTex.SampleBias(sampler_MainTex, r0.yz, _GlobalMipBias.x).xyzw;
  r0.y = 1 + -r1.y;
  r0.zw = saturate(v2.yx);
  r1.x = -r0.z * 0.5 + 1;
  r1.y = r0.z * 0.5 + -1;
  r1.z = r1.x + -r1.y;
  r1.x = -r0.z * 0.5 + r1.x;
  r1.x = 1 / r1.x;
  r0.w = r0.w * r1.z + r1.y;
  r0.y = r0.y + r0.w;
  r0.y = -r0.z * 0.5 + r0.y;
  r0.y = saturate(r0.y * r1.x);
  r0.z = r0.y * -2 + 3;
  r0.y = r0.y * r0.y;
  r0.y = saturate(-r0.z * r0.y + r1.w);
  r0.y = v3.w * r0.y;
  r0.zw = v1.xy * _MaskTex_ST.xy + _MaskTex_ST.zw;
  r1.xyzw = _MaskTex.SampleBias(sampler_MaskTex, r0.zw, _GlobalMipBias.x).xyzw;
  r0.z = r1.x * r1.w;
  r0.y = r0.y * r0.z;
  r0.z = -0.200000003 + -_WorldSpaceCameraPos.y;
  r1.xyz = -_WorldSpaceCameraPos.xyz + v4.xyz;
  r0.z = r0.z / r1.y;
  r0.zw = r1.xz * r0.zz + _WorldSpaceCameraPos.xz;
  r0.zw = _PlaneBoundMaxPos.xz + -r0.zw;
  r1.xy = _PlaneBoundMaxPos.xz + -_PlaneBoundMinPos.xz;
  r0.zw = r0.zw / r1.xy;
  r1.xyzw = _PlanePointBuffer.SampleBias(sampler_PlanePointBuffer, r0.zw, _GlobalMipBias.x).xyzw;
  r2.xyzw = _PlaneSightRenderTarget.SampleBias(sampler_PlaneSightRenderTarget, r0.zw, _GlobalMipBias.x).xyzw;
  r0.z = _PlaneSystemUsingLightBuffer * _GlobalPlanePointBufferEnable;
  r0.z = r1.x * r0.z;
  r0.z = max(r2.w, r0.z);
  r0.z = -1 + r0.z;
  r0.z = _GlobalPlaneSystemEnable * r0.z;
  r0.z = _GlobalIsRuntime * r0.z + 1;
  r0.x = r0.y * r0.z + -r0.x;
  r0.x = -_AlphaClipThreshold + r0.x;
  r0.x = cmp(r0.x < 0);
  if (r0.x != 0) discard;
  r0.x = dot(v5.xyz, v5.xyz);
  r0.x = rsqrt(r0.x);
  o0.xyz = v5.xyz * r0.xxx;
  o0.w = 0;
  return;
}
