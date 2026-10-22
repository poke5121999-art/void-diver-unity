// ArtTeam/VFX/VFX_Master_typeB | pass 0 "Universal Forward" | progFragment | keywords: FOG_LINEAR _DBUFFER_MRT3 _PLANESYSTEMCULLINGTYPE_ALWAYS | blob 39
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

  r0.xyz = _WorldSpaceCameraPos.xyz + -v5.xyz;
  r0.w = dot(r0.xyz, r0.xyz);
  r0.w = rsqrt(r0.w);
  r0.xyz = r0.xyz * r0.www;
  r0.w = cmp(unity_OrthoParams.w == 0.000000);
  r1.x = (VD_COL(unity_MatrixV, 0).z);
  r1.y = (VD_COL(unity_MatrixV, 1).z);
  r1.z = (VD_COL(unity_MatrixV, 2).z);
  r0.xyz = r0.www ? r0.xyz : r1.xyz;
  r0.w = dot(v6.xyz, v6.xyz);
  r0.w = sqrt(r0.w);
  r0.w = 1 / r0.w;
  r1.xyz = v6.xyz * r0.www;
  r0.w = dot(r1.xyz, r1.xyz);
  r0.w = rsqrt(r0.w);
  r1.xyz = r1.xyz * r0.www;
  r0.x = saturate(dot(r1.xyz, r0.xyz));
  r0.x = 1 + -r0.x;
  r0.x = log2(r0.x);
  r0.x = _FresnelPower * r0.x;
  r0.x = exp2(r0.x);
  r0.x = _isFresnel * r0.x;
  r0.y = max(1, v2.w);
  r0.zw = float2(1,1) / _ScreenParams.xy;
  r0.zw = (_Pixelation).xx * r0.zw;
  r1.xy = v1.xy / r0.zw;
  r1.xy = floor(r1.xy);
  r0.zw = r1.xy * r0.zw;
  r0.zw = r0.zw * _MainTex_ST.xy + _MainTex_ST.zw;
  r1.xyzw = _MainTex.SampleBias(sampler_MainTex, r0.zw, _GlobalMipBias.x).xyzw;
  r1.xyw = v4.xyz * r1.xxx;
  r0.yzw = r1.xyw * r0.yyy + -r1.xyw;
  r0.xyz = r0.xxx * r0.yzw + r1.xyw;
  r1.xyw = v3.xyz + -r0.xyz;
  r0.w = 1 + -r1.z;
  r0.w = r0.w + -r1.z;
  r2.x = saturate(_invertB);
  r0.w = r2.x * r0.w + r1.z;
  r0.w = v3.w * r0.w;
  r0.xyz = r0.www * r1.xyw + r0.xyz;
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
  r0.xyz = r0.www * r0.xyz + r1.xyz;
  r1.xy = (int2)v0.xy;
  r1.zw = float2(0,0);
  r1.xyzw = _DBufferTexture0.Load(r1.xyz).xyzw;
  o0.xyz = r0.xyz * r1.www + r1.xyz;
  o0.w = 1;
  return;
}
