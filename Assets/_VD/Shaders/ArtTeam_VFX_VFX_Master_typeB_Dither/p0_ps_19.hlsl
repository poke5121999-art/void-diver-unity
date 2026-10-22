// ArtTeam/VFX/VFX_Master_typeB_Dither | pass 0 "Universal Forward" | progFragment | keywords: _ALPHATEST_ON _PLANESYSTEMCULLINGTYPE_EQUAL | blob 19
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
  r0.y = -0.200000003 + -_WorldSpaceCameraPos.y;
  r1.xyz = -_WorldSpaceCameraPos.xyz + v5.xyz;
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
  r0.zw = float2(1,1) / _ScreenParams.xy;
  r0.zw = (_Pixelation).xx * r0.zw;
  r1.xy = v1.xy / r0.zw;
  r1.xy = floor(r1.xy);
  r0.zw = r1.xy * r0.zw;
  r0.zw = r0.zw * _MainTex_ST.xy + _MainTex_ST.zw;
  r1.xyzw = _MainTex.SampleBias(sampler_MainTex, r0.zw, _GlobalMipBias.x).xyzw;
  r0.zw = float2(1,1) + -r1.zy;
  r2.xy = saturate(v2.yx);
  r1.y = -r2.x * 0.5 + 1;
  r2.z = r2.x * 0.5 + -1;
  r2.w = -r2.z + r1.y;
  r1.y = -r2.x * 0.5 + r1.y;
  r1.y = 1 / r1.y;
  r2.y = r2.y * r2.w + r2.z;
  r0.w = r2.y + r0.w;
  r0.z = r0.z + -r1.z;
  r0.w = -r2.x * 0.5 + r0.w;
  r0.w = saturate(r0.w * r1.y);
  r1.y = r0.w * -2 + 3;
  r0.w = r0.w * r0.w;
  r0.w = saturate(-r1.y * r0.w + r1.w);
  r0.w = v4.w * r0.w;
  r1.yw = v1.xy * _MaskTex_ST.xy + _MaskTex_ST.zw;
  r2.xyzw = _MaskTex.SampleBias(sampler_MaskTex, r1.yw, _GlobalMipBias.x).xyzw;
  r1.y = r2.x * r2.w;
  r0.w = r1.y * r0.w;
  r0.x = r0.w * r0.y + -r0.x;
  r0.y = ddx(r0.x);
  r0.w = ddy(r0.x);
  r0.x = -_AlphaClipThreshold + r0.x;
  r0.y = abs(r0.y) + abs(r0.w);
  r0.w = -r0.y * 0.5 + r0.x;
  r0.y = max(9.99999975e-005, r0.y);
  r0.y = r0.w / r0.y;
  r0.y = saturate(1 + r0.y);
  r0.w = cmp(0 >= _AlphaClipThreshold);
  r0.y = r0.w ? 1 : r0.y;
  r0.w = ~(int)r0.w;
  r1.y = -9.99999975e-005 + r0.y;
  r1.w = cmp(_AlphaToMaskAvailable != 0.000000);
  r0.w = r0.w ? r1.w : 0;
  o0.w = r1.w ? r0.y : 1;
  r0.x = r0.w ? r1.y : r0.x;
  r0.x = cmp(r0.x < 0);
  if (r0.x != 0) discard;
  r0.x = saturate(_invertB);
  r0.x = r0.x * r0.z + r1.z;
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
  r1.yzw = v3.xyz * r1.yyy + -r0.yzw;
  r0.yzw = r1.xxx * r1.yzw + r0.yzw;
  r1.xyz = v3.xyz + -r0.yzw;
  r0.xyz = r0.xxx * r1.xyz + r0.yzw;
  o0.xyz = v2.zzz * r0.xyz;
  return;
}
