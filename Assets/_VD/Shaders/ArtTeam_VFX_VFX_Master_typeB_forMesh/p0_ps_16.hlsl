// ArtTeam/VFX/VFX_Master_typeB_forMesh | pass 0 "Universal Forward" | progFragment | keywords: FOG_LINEAR | blob 16
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

  r0.xyz = _WorldSpaceCameraPos.xyz + -v2.xyz;
  r0.w = dot(r0.xyz, r0.xyz);
  r0.w = rsqrt(r0.w);
  r0.xyz = r0.xyz * r0.www;
  r0.w = cmp(unity_OrthoParams.w == 0.000000);
  r1.x = (VD_COL(unity_MatrixV, 0).z);
  r1.y = (VD_COL(unity_MatrixV, 1).z);
  r1.z = (VD_COL(unity_MatrixV, 2).z);
  r0.xyz = r0.www ? r0.xyz : r1.xyz;
  r0.w = dot(v3.xyz, v3.xyz);
  r0.w = sqrt(r0.w);
  r0.w = 1 / r0.w;
  r1.xyz = v3.xyz * r0.www;
  r0.w = dot(r1.xyz, r1.xyz);
  r0.w = rsqrt(r0.w);
  r1.xyz = r1.xyz * r0.www;
  r0.x = saturate(dot(r1.xyz, r0.xyz));
  r0.x = 1 + -r0.x;
  r0.x = log2(r0.x);
  r0.x = _FresnelPower * r0.x;
  r0.x = exp2(r0.x);
  r0.yz = v1.xy * _MaskTex_G_Dissolve_B_2ndColor_A_alpha_ST.xy + _MaskTex_G_Dissolve_B_2ndColor_A_alpha_ST.zw;
  r1.xyzw = _MaskTex_G_Dissolve_B_2ndColor_A_alpha.SampleBias(sampler_MaskTex_G_Dissolve_B_2ndColor_A_alpha, r0.yz, _GlobalMipBias.x).xyzw;
  r0.y = 1 + -r1.z;
  r0.y = r0.y + -r1.z;
  r0.z = saturate(_invertB);
  r0.y = r0.z * r0.y + r1.z;
  r0.y = _2ndColor.w * r0.y;
  r0.zw = v1.xy * _MainTex_ST.xy + _MainTex_ST.zw;
  r1.xyzw = _MainTex.SampleBias(sampler_MainTex, r0.zw, _GlobalMipBias.x).xyzw;
  r2.xyz = _TintColor.xyz * r1.xyz;
  r1.xyz = -_TintColor.xyz * r1.xyz + _2ndColor.xyz;
  r0.yzw = r0.yyy * r1.xyz + r2.xyz;
  r0.xyz = r0.xxx * _FresnelColor.xyz + r0.yzw;
  r1.xyz = (_Emission).xxx * r0.xyz;
  r0.xyz = -r0.xyz * (_Emission).xxx + unity_FogColor.xyz;
  r2.xyz = (VD_COL(unity_WorldToObject, 1).xyz) * v2.yyy;
  r2.xyz = (VD_COL(unity_WorldToObject, 0).xyz) * v2.xxx + r2.xyz;
  r2.xyz = (VD_COL(unity_WorldToObject, 2).xyz) * v2.zzz + r2.xyz;
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
  o0.w = 1;
  return;
}
