// ArtTeam/VFX/VFX_Master_typeB | pass 0 "Universal Forward" | progFragment | keywords: FOG_LINEAR _ALPHATEST_ON _PLANESYSTEMCULLINGTYPE_ALWAYS | blob 30
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

  r0.xy = float2(1,1) / _ScreenParams.xy;
  r0.xy = (_Pixelation).xx * r0.xy;
  r0.zw = v1.xy / r0.xy;
  r0.zw = floor(r0.zw);
  r0.xy = r0.zw * r0.xy;
  r0.xy = r0.xy * _MainTex_ST.xy + _MainTex_ST.zw;
  r0.xyzw = _MainTex.SampleBias(sampler_MainTex, r0.xy, _GlobalMipBias.x).xyzw;
  r1.xy = float2(1,1) + -r0.zy;
  r1.zw = saturate(v2.yx);
  r0.y = -r1.z * 0.5 + 1;
  r2.x = r1.z * 0.5 + -1;
  r2.y = -r2.x + r0.y;
  r0.y = -r1.z * 0.5 + r0.y;
  r0.y = 1 / r0.y;
  r1.w = r1.w * r2.y + r2.x;
  r1.y = r1.y + r1.w;
  r1.x = r1.x + -r0.z;
  r1.y = -r1.z * 0.5 + r1.y;
  r0.y = saturate(r1.y * r0.y);
  r1.y = r0.y * -2 + 3;
  r0.y = r0.y * r0.y;
  r0.y = saturate(-r1.y * r0.y + r0.w);
  r0.y = v4.w * r0.y;
  r1.yz = v1.xy * _MaskTex_ST.xy + _MaskTex_ST.zw;
  r2.xyzw = _MaskTex.SampleBias(sampler_MaskTex, r1.yz, _GlobalMipBias.x).xyzw;
  r0.w = r2.x * r2.w;
  r0.y = saturate(r0.y * r0.w);
  r0.w = ddx(r0.y);
  r1.y = ddy(r0.y);
  r0.y = -_AlphaClipThreshold + r0.y;
  r0.w = abs(r1.y) + abs(r0.w);
  r1.y = -r0.w * 0.5 + r0.y;
  r0.w = max(9.99999975e-005, r0.w);
  r0.w = r1.y / r0.w;
  r0.w = saturate(1 + r0.w);
  r1.y = cmp(0 >= _AlphaClipThreshold);
  r0.w = r1.y ? 1 : r0.w;
  r1.y = ~(int)r1.y;
  r1.z = -9.99999975e-005 + r0.w;
  r1.w = cmp(_AlphaToMaskAvailable != 0.000000);
  r1.y = r1.y ? r1.w : 0;
  o0.w = r1.w ? r0.w : 1;
  r0.y = r1.y ? r1.z : r0.y;
  r0.y = cmp(r0.y < 0);
  if (r0.y != 0) discard;
  r0.y = saturate(_invertB);
  r0.y = r0.y * r1.x + r0.z;
  r0.xzw = v4.xyz * r0.xxx;
  r0.y = v3.w * r0.y;
  r1.xyz = _WorldSpaceCameraPos.xyz + -v5.xyz;
  r1.w = dot(r1.xyz, r1.xyz);
  r1.w = rsqrt(r1.w);
  r1.xyz = r1.xyz * r1.www;
  r1.w = cmp(unity_OrthoParams.w == 0.000000);
  r2.x = (VD_COL(unity_MatrixV, 0).z);
  r2.y = (VD_COL(unity_MatrixV, 1).z);
  r2.z = (VD_COL(unity_MatrixV, 2).z);
  r1.xyz = r1.www ? r1.xyz : r2.xyz;
  r1.w = dot(v6.xyz, v6.xyz);
  r1.w = sqrt(r1.w);
  r1.w = 1 / r1.w;
  r2.xyz = v6.xyz * r1.www;
  r1.w = dot(r2.xyz, r2.xyz);
  r1.w = rsqrt(r1.w);
  r2.xyz = r2.xyz * r1.www;
  r1.x = saturate(dot(r2.xyz, r1.xyz));
  r1.x = 1 + -r1.x;
  r1.x = log2(r1.x);
  r1.x = _FresnelPower * r1.x;
  r1.x = exp2(r1.x);
  r1.x = _isFresnel * r1.x;
  r1.y = max(1, v2.w);
  r1.yzw = r0.xzw * r1.yyy + -r0.xzw;
  r0.xzw = r1.xxx * r1.yzw + r0.xzw;
  r1.xyz = v3.xyz + -r0.xzw;
  r0.xyz = r0.yyy * r1.xyz + r0.xzw;
  r1.xyz = v2.zzz * r0.xyz;
  r0.xyz = -r0.xyz * v2.zzz + unity_FogColor.xyz;
  r2.xyz = (VD_COL(unity_WorldToObject, 1).xyz) * v5.yyy;
  r2.xyz = (VD_COL(unity_WorldToObject, 0).xyz) * v5.xxx + r2.xyz;
  r2.xyz = (VD_COL(unity_WorldToObject, 2).xyz) * v5.zzz + r2.xyz;
  r2.xyz = (VD_COL(unity_WorldToObject, 3).xyz) + r2.xyz;
  r3.xyz = (VD_COL(unity_ObjectToWorld, 1).xyz) * r2.yyy;
  r2.xyw = (VD_COL(unity_ObjectToWorld, 0).xyz) * r2.xxx + r3.xyz;
  r2.xyz = (VD_COL(unity_ObjectToWorld, 2).xyz) * r2.zzz + r2.xyw;
  r2.xyz = (VD_COL(unity_ObjectToWorld, 3).xyz) + r2.xyz;
  r0.w = (VD_COL(unity_MatrixV, 1).z) * r2.y;
  r0.w = (VD_COL(unity_MatrixV, 0).z) * r2.x + r0.w;
  r0.w = (VD_COL(unity_MatrixV, 2).z) * r2.z + r0.w;
  r0.w = (VD_COL(unity_MatrixV, 3).z) + r0.w;
  r0.w = -_ProjectionParams.y + -r0.w;
  r0.w = max(0, r0.w);
  r0.w = saturate(r0.w * unity_FogParams.z + unity_FogParams.w);
  r0.w = 1 + -r0.w;
  o0.xyz = r0.www * r0.xyz + r1.xyz;
  return;
}
