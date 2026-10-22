// Shader Graphs/Shader_Decoration_Puddle | pass 1 "DBufferMesh" | progVertex | keywords: _DE_WEATHER_RAIN | blob 24
void vert(
  float3 v0 : POSITION0,
  float3 v1 : NORMAL0,
  float4 v2 : TANGENT0,
  float4 v3 : TEXCOORD0,
  float4 v4 : TEXCOORD1,
  float4 v5 : TEXCOORD2,
  out float4 o0 : SV_POSITION0,
  out float4 o1 : INTERP0,
  out float4 o2 : INTERP1,
  out float3 o3 : INTERP2,
  out float3 o4 : INTERP3)
{
  float4 r0,r1,r2;
  uint4 bitmask, uiDest;
  float4 fDest;

  r0.x = (VD_COL(unity_MatrixV, 0).z);
  r0.y = (VD_COL(unity_MatrixV, 1).z);
  r0.z = (VD_COL(unity_MatrixV, 2).z);
  r1.x = dot(r0.xyz, (VD_COL(unity_ObjectToWorld, 0).xyz));
  r1.y = dot(r0.xyz, (VD_COL(unity_ObjectToWorld, 1).xyz));
  r1.z = dot(r0.xyz, (VD_COL(unity_ObjectToWorld, 2).xyz));
  r0.x = dot(r1.xyz, r1.xyz);
  r0.x = max(1.17549435e-038, r0.x);
  r0.x = rsqrt(r0.x);
  r0.xyz = r1.xyz * r0.xxx;
  r1.xyz = (VD_COL(unity_WorldToObject, 1).xyz) * _WorldSpaceCameraPos.yyy;
  r1.xyz = (VD_COL(unity_WorldToObject, 0).xyz) * _WorldSpaceCameraPos.xxx + r1.xyz;
  r1.xyz = (VD_COL(unity_WorldToObject, 2).xyz) * _WorldSpaceCameraPos.zzz + r1.xyz;
  r1.xyz = (VD_COL(unity_WorldToObject, 3).xyz) + r1.xyz;
  r1.xyz = -v0.xyz + r1.xyz;
  r0.w = dot(r1.xyz, r1.xyz);
  r0.w = rsqrt(r0.w);
  r1.xyz = r1.xyz * r0.www;
  r0.w = cmp(unity_OrthoParams.w == 0.000000);
  r0.xyz = r0.www ? r1.xyz : r0.xyz;
  r0.xyz = r0.xyz * (_DecalMeshViewBias).xxx + v0.xyz;
  r1.xy = cmp((_DecalMeshBiasType).xx == float2(1,0));
  r0.xyz = r1.xxx ? r0.xyz : v0.xyz;
  r1.xzw = (VD_COL(unity_ObjectToWorld, 1).xyz) * r0.yyy;
  r0.xyw = (VD_COL(unity_ObjectToWorld, 0).xyz) * r0.xxx + r1.xzw;
  r0.xyz = (VD_COL(unity_ObjectToWorld, 2).xyz) * r0.zzz + r0.xyw;
  r0.xyz = (VD_COL(unity_ObjectToWorld, 3).xyz) + r0.xyz;
  r2.xyzw = (VD_COL(unity_MatrixVP, 1).xyzw) * r0.yyyy;
  r2.xyzw = (VD_COL(unity_MatrixVP, 0).xyzw) * r0.xxxx + r2.xyzw;
  r2.xyzw = (VD_COL(unity_MatrixVP, 2).xyzw) * r0.zzzz + r2.xyzw;
  o3.xyz = r0.xyz;
  r0.xyzw = (VD_COL(unity_MatrixVP, 3).xyzw) + r2.xyzw;
  r1.x = -_DecalMeshDepthBias + r0.z;
  o0.z = r1.y ? r1.x : r0.z;
  o0.xyw = r0.xyw;
  r0.xyz = (VD_COL(unity_ObjectToWorld, 1).xyz) * v2.yyy;
  r0.xyz = (VD_COL(unity_ObjectToWorld, 0).xyz) * v2.xxx + r0.xyz;
  r0.xyz = (VD_COL(unity_ObjectToWorld, 2).xyz) * v2.zzz + r0.xyz;
  r0.w = dot(r0.xyz, r0.xyz);
  r0.w = max(1.17549435e-038, r0.w);
  r0.w = rsqrt(r0.w);
  o1.xyz = r0.xyz * r0.www;
  o1.w = v2.w;
  o2.xyzw = v3.xyzw;
  r0.x = dot(v1.xyz, (VD_COL(unity_WorldToObject, 0).xyz));
  r0.y = dot(v1.xyz, (VD_COL(unity_WorldToObject, 1).xyz));
  r0.z = dot(v1.xyz, (VD_COL(unity_WorldToObject, 2).xyz));
  r0.w = dot(r0.xyz, r0.xyz);
  r0.w = max(1.17549435e-038, r0.w);
  r0.w = rsqrt(r0.w);
  o4.xyz = r0.xyz * r0.www;
  return;
}
