// ArtTeam/VFX/VFX_Master_typeB_forMesh | pass 0 "Universal Forward" | progFragment | keywords: FOG_LINEAR _DBUFFER_MRT3 _SURFACE_TYPE_TRANSPARENT | blob 23
void frag(
  float4 v0 : SV_POSITION0,
  float4 v1 : INTERP0,
  float3 v2 : INTERP1,
  float3 v3 : INTERP2,
  out float4 o0 : SV_Target0)
{
  float4 r0,r1,r2,r3;
  uint4 bitmask, uiDest;
  float4 fDest;

  r0.xyz = (VD_COL(unity_WorldToObject, 1).xyz) * v2.yyy;
  r0.xyz = (VD_COL(unity_WorldToObject, 0).xyz) * v2.xxx + r0.xyz;
  r0.xyz = (VD_COL(unity_WorldToObject, 2).xyz) * v2.zzz + r0.xyz;
  r0.xyz = (VD_COL(unity_WorldToObject, 3).xyz) + r0.xyz;
  r1.xyz = (VD_COL(unity_ObjectToWorld, 1).xyz) * r0.yyy;
  r0.xyw = (VD_COL(unity_ObjectToWorld, 0).xyz) * r0.xxx + r1.xyz;
  r0.xyz = (VD_COL(unity_ObjectToWorld, 2).xyz) * r0.zzz + r0.xyw;
  r0.xyz = (VD_COL(unity_ObjectToWorld, 3).xyz) + r0.xyz;
  r0.y = (VD_COL(unity_MatrixV, 1).z) * r0.y;
  r0.x = (VD_COL(unity_MatrixV, 0).z) * r0.x + r0.y;
  r0.x = (VD_COL(unity_MatrixV, 2).z) * r0.z + r0.x;
  r0.x = (VD_COL(unity_MatrixV, 3).z) + r0.x;
  r0.x = -_ProjectionParams.y + -r0.x;
  r0.x = max(0, r0.x);
  r0.x = saturate(r0.x * unity_FogParams.z + unity_FogParams.w);
  r0.yzw = _WorldSpaceCameraPos.xyz + -v2.xyz;
  r1.x = dot(r0.yzw, r0.yzw);
  r1.x = rsqrt(r1.x);
  r0.yzw = r1.xxx * r0.yzw;
  r1.x = cmp(unity_OrthoParams.w == 0.000000);
  r2.x = (VD_COL(unity_MatrixV, 0).z);
  r2.y = (VD_COL(unity_MatrixV, 1).z);
  r2.z = (VD_COL(unity_MatrixV, 2).z);
  r0.yzw = r1.xxx ? r0.yzw : r2.xyz;
  r1.x = dot(v3.xyz, v3.xyz);
  r1.x = sqrt(r1.x);
  r1.x = 1 / r1.x;
  r1.xyz = v3.xyz * r1.xxx;
  r1.w = dot(r1.xyz, r1.xyz);
  r1.w = rsqrt(r1.w);
  r1.xyz = r1.xyz * r1.www;
  r0.y = saturate(dot(r1.xyz, r0.yzw));
  r0.xy = float2(1,1) + -r0.xy;
  r0.y = log2(r0.y);
  r0.y = _FresnelPower * r0.y;
  r0.y = exp2(r0.y);
  r0.z = saturate(_invertB);
  r1.xy = v1.xy * _MaskTex_G_Dissolve_B_2ndColor_A_alpha_ST.xy + _MaskTex_G_Dissolve_B_2ndColor_A_alpha_ST.zw;
  r1.xyzw = _MaskTex_G_Dissolve_B_2ndColor_A_alpha.SampleBias(sampler_MaskTex_G_Dissolve_B_2ndColor_A_alpha, r1.xy, _GlobalMipBias.x).xyzw;
  r0.w = r1.z * -2 + 1;
  r0.z = r0.z * r0.w + r1.z;
  r0.z = _2ndColor.w * r0.z;
  r1.xz = v1.xy * _MainTex_ST.xy + _MainTex_ST.zw;
  r2.xyzw = _MainTex.SampleBias(sampler_MainTex, r1.xz, _GlobalMipBias.x).xyzw;
  r3.xyz = _TintColor.xyz * r2.xyz;
  r2.xyz = -_TintColor.xyz * r2.xyz + _2ndColor.xyz;
  r2.xyz = r0.zzz * r2.xyz + r3.xyz;
  r0.yzw = r0.yyy * _FresnelColor.xyz + r2.xyz;
  r2.xyz = (_Emission).xxx * r0.yzw;
  r0.yzw = -r0.yzw * (_Emission).xxx + unity_FogColor.xyz;
  o0.xyz = r0.xxx * r0.yzw + r2.xyz;
  r0.x = 1 + -r1.y;
  r0.yz = saturate(float2(_DissolveSharpness, _Dissolve));
  r0.w = -r0.y * 0.5 + 1;
  r1.x = r0.y * 0.5 + -1;
  r1.y = -r1.x + r0.w;
  r0.w = -r0.y * 0.5 + r0.w;
  r0.w = 1 / r0.w;
  r0.z = r0.z * r1.y + r1.x;
  r0.x = r0.x + r0.z;
  r0.x = -r0.y * 0.5 + r0.x;
  r0.x = saturate(r0.x * r0.w);
  r0.y = r0.x * -2 + 3;
  r0.x = r0.x * r0.x;
  r0.x = saturate(-r0.y * r0.x + r1.w);
  o0.w = _TintColor.w * r0.x;
  return;
}
