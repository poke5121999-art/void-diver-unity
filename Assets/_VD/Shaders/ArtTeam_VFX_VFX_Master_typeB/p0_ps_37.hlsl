// ArtTeam/VFX/VFX_Master_typeB | pass 0 "Universal Forward" | progFragment | keywords: _ALPHATEST_ON _DBUFFER_MRT3 _PLANESYSTEMCULLINGTYPE_EQUAL | blob 37
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

  r0.x = -0.200000003 + -_WorldSpaceCameraPos.y;
  r0.yzw = -_WorldSpaceCameraPos.xyz + v5.xyz;
  r0.x = r0.x / r0.z;
  r0.xy = r0.yw * r0.xx + _WorldSpaceCameraPos.xz;
  r0.xy = _PlaneBoundMaxPos.xz + -r0.xy;
  r0.zw = _PlaneBoundMaxPos.xz + -_PlaneBoundMinPos.xz;
  r0.xy = r0.xy / r0.zw;
  r1.xyzw = _PlanePointBuffer.SampleBias(sampler_PlanePointBuffer, r0.xy, _GlobalMipBias.x).xyzw;
  r0.xyzw = _PlaneSightRenderTarget.SampleBias(sampler_PlaneSightRenderTarget, r0.xy, _GlobalMipBias.x).xyzw;
  r0.x = _PlaneSystemUsingLightBuffer * _GlobalPlanePointBufferEnable;
  r0.x = r1.x * r0.x;
  r0.x = max(r0.w, r0.x);
  r0.x = -1 + r0.x;
  r0.x = _GlobalPlaneSystemEnable * r0.x;
  r0.x = _GlobalIsRuntime * r0.x + 1;
  r0.yz = float2(1,1) / _ScreenParams.xy;
  r0.yz = (_Pixelation).xx * r0.yz;
  r1.xy = v1.xy / r0.yz;
  r1.xy = floor(r1.xy);
  r0.yz = r1.xy * r0.yz;
  r0.yz = r0.yz * _MainTex_ST.xy + _MainTex_ST.zw;
  r1.xyzw = _MainTex.SampleBias(sampler_MainTex, r0.yz, _GlobalMipBias.x).xyzw;
  r0.yz = float2(1,1) + -r1.zy;
  r2.xy = saturate(v2.yx);
  r0.w = -r2.x * 0.5 + 1;
  r1.y = r2.x * 0.5 + -1;
  r2.z = -r1.y + r0.w;
  r0.w = -r2.x * 0.5 + r0.w;
  r0.w = 1 / r0.w;
  r1.y = r2.y * r2.z + r1.y;
  r0.z = r1.y + r0.z;
  r0.y = r0.y + -r1.z;
  r0.z = -r2.x * 0.5 + r0.z;
  r0.z = saturate(r0.z * r0.w);
  r0.w = r0.z * -2 + 3;
  r0.z = r0.z * r0.z;
  r0.z = saturate(-r0.w * r0.z + r1.w);
  r0.z = v4.w * r0.z;
  r1.yw = v1.xy * _MaskTex_ST.xy + _MaskTex_ST.zw;
  r2.xyzw = _MaskTex.SampleBias(sampler_MaskTex, r1.yw, _GlobalMipBias.x).xyzw;
  r0.w = r2.x * r2.w;
  r0.z = r0.z * r0.w;
  r0.x = saturate(r0.z * r0.x);
  r0.z = ddx(r0.x);
  r0.w = ddy(r0.x);
  r0.x = -_AlphaClipThreshold + r0.x;
  r0.z = abs(r0.z) + abs(r0.w);
  r0.w = -r0.z * 0.5 + r0.x;
  r0.z = max(9.99999975e-005, r0.z);
  r0.z = r0.w / r0.z;
  r0.z = saturate(1 + r0.z);
  r0.w = cmp(0 >= _AlphaClipThreshold);
  r0.z = r0.w ? 1 : r0.z;
  r0.w = ~(int)r0.w;
  r1.y = -9.99999975e-005 + r0.z;
  r1.w = cmp(_AlphaToMaskAvailable != 0.000000);
  r0.w = r0.w ? r1.w : 0;
  o0.w = r1.w ? r0.z : 1;
  r0.x = r0.w ? r1.y : r0.x;
  r0.x = cmp(r0.x < 0);
  if (r0.x != 0) discard;
  r0.x = saturate(_invertB);
  r0.x = r0.x * r0.y + r1.z;
  r0.yzw = v4.xyz * r1.xxx;
  r0.x = v3.w * r0.x;
  r1.x = dot(v6.xyz, v6.xyz);
  r1.x = sqrt(r1.x);
  r1.x = 1 / r1.x;
  r1.xyz = v6.xyz * r1.xxx;
  r1.w = dot(r1.xyz, r1.xyz);
  r1.w = rsqrt(r1.w);
  r1.xyz = r1.xyz * r1.www;
  r2.xyz = _WorldSpaceCameraPos.xyz + -v5.xyz;
  r1.w = dot(r2.xyz, r2.xyz);
  r1.w = rsqrt(r1.w);
  r2.xyz = r2.xyz * r1.www;
  r1.w = cmp(unity_OrthoParams.w == 0.000000);
  r3.x = r1.w ? r2.x : (VD_COL(unity_MatrixV, 0).z);
  r3.y = r1.w ? r2.y : (VD_COL(unity_MatrixV, 1).z);
  r3.z = r1.w ? r2.z : (VD_COL(unity_MatrixV, 2).z);
  r1.x = saturate(dot(r1.xyz, r3.xyz));
  r1.x = 1 + -r1.x;
  r1.x = log2(r1.x);
  r1.x = _FresnelPower * r1.x;
  r1.x = exp2(r1.x);
  r1.x = _isFresnel * r1.x;
  r1.y = max(1, v2.w);
  r1.yzw = r0.yzw * r1.yyy + -r0.yzw;
  r0.yzw = r1.xxx * r1.yzw + r0.yzw;
  r1.xyz = v3.xyz + -r0.yzw;
  r0.xyz = r0.xxx * r1.xyz + r0.yzw;
  r0.xyz = v2.zzz * r0.xyz;
  r1.xy = (int2)v0.xy;
  r1.zw = float2(0,0);
  r1.xyzw = _DBufferTexture0.Load(r1.xyz).xyzw;
  o0.xyz = r0.xyz * r1.www + r1.xyz;
  return;
}
