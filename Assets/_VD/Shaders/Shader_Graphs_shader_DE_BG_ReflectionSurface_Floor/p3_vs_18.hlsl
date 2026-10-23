// Shader Graphs/shader_DE_BG_ReflectionSurface_Floor | pass 3 "DecalMeshForwardEmissive" | progVertex | keywords: (none) | blob 18
void vert(
  float3 v0 : POSITION0,
  float3 v1 : NORMAL0,
  float4 v2 : TANGENT0,
  float4 v3 : TEXCOORD0,
  float4 v4 : TEXCOORD1,
  float4 v5 : TEXCOORD2,
  out float4 o0 : SV_POSITION0,
  out float4 o1 : VDREG1,
  out float4 o2 : VDREG2,
  out float4 o3 : VDREG3,
  out float4 o4 : VDREG4)
{
  uint4 r0, r1, r2;
  r0 = 0u; r1 = 0u; r2 = 0u;

  r0.x = vd_asuint((VD_COL(unity_MatrixV, 0).z));
  r0.y = vd_asuint((VD_COL(unity_MatrixV, 1).z));
  r0.z = vd_asuint((VD_COL(unity_MatrixV, 2).z));
  r1.x = vd_asuint(dot(vd_asfloat(r0.xyz), vd_asfloat((VD_COL(unity_ObjectToWorld, 0).xyz))));
  r1.y = vd_asuint(dot(vd_asfloat(r0.xyz), vd_asfloat((VD_COL(unity_ObjectToWorld, 1).xyz))));
  r1.z = vd_asuint(dot(vd_asfloat(r0.xyz), vd_asfloat((VD_COL(unity_ObjectToWorld, 2).xyz))));
  r0.x = vd_asuint(dot(vd_asfloat(r1.xyz), vd_asfloat(r1.xyz)));
  r0.x = vd_asuint(max(vd_asfloat(r0.x), 1.17549435e-38));
  r0.x = vd_asuint(rsqrt(vd_asfloat(r0.x)));
  r0.xyz = vd_asuint((vd_asfloat(r0.xxx) * vd_asfloat(r1.xyz)));
  r1.xyz = vd_asuint((vd_asfloat(_WorldSpaceCameraPos.yyy) * vd_asfloat((VD_COL(unity_WorldToObject, 1).xyz))));
  r1.xyz = vd_asuint((vd_asfloat((VD_COL(unity_WorldToObject, 0).xyz)) * vd_asfloat(_WorldSpaceCameraPos.xxx) + vd_asfloat(r1.xyz)));
  r1.xyz = vd_asuint((vd_asfloat((VD_COL(unity_WorldToObject, 2).xyz)) * vd_asfloat(_WorldSpaceCameraPos.zzz) + vd_asfloat(r1.xyz)));
  r1.xyz = vd_asuint((vd_asfloat(r1.xyz) + vd_asfloat((VD_COL(unity_WorldToObject, 3).xyz))));
  r1.xyz = vd_asuint((vd_asfloat(r1.xyz) + (-(vd_asfloat(v0.xyz)))));
  r0.w = vd_asuint(dot(vd_asfloat(r1.xyz), vd_asfloat(r1.xyz)));
  r0.w = vd_asuint(rsqrt(vd_asfloat(r0.w)));
  r1.xyz = vd_asuint((vd_asfloat(r0.www) * vd_asfloat(r1.xyz)));
  r0.w = vd_asuint((-(int)(vd_asfloat(unity_OrthoParams.w) == 0.0)));
  r0.xyz = ((r0.www != 0u) ? r1.xyz : r0.xyz);
  r0.xyz = vd_asuint((vd_asfloat(r0.xyz) * vd_asfloat((_DecalMeshViewBias).xxx) + vd_asfloat(v0.xyz)));
  r1.xy = vd_asuint((-(int2)(vd_asfloat((_DecalMeshBiasType).xx) == float2(1.0, 0.0))));
  r0.xyz = ((r1.xxx != 0u) ? r0.xyz : vd_asuint(v0.xyz));
  r1.xzw = vd_asuint((vd_asfloat(r0.yyy) * vd_asfloat((VD_COL(unity_ObjectToWorld, 1).xyz))));
  r0.xyw = vd_asuint((vd_asfloat((VD_COL(unity_ObjectToWorld, 0).xyz)) * vd_asfloat(r0.xxx) + vd_asfloat(r1.xzw)));
  r0.xyz = vd_asuint((vd_asfloat((VD_COL(unity_ObjectToWorld, 2).xyz)) * vd_asfloat(r0.zzz) + vd_asfloat(r0.xyw)));
  r0.xyz = vd_asuint((vd_asfloat(r0.xyz) + vd_asfloat((VD_COL(unity_ObjectToWorld, 3).xyz))));
  r2.xyzw = vd_asuint((vd_asfloat(r0.yyyy) * vd_asfloat((VD_COL(unity_MatrixVP, 1).xyzw))));
  r2.xyzw = vd_asuint((vd_asfloat((VD_COL(unity_MatrixVP, 0).xyzw)) * vd_asfloat(r0.xxxx) + vd_asfloat(r2.xyzw)));
  r2.xyzw = vd_asuint((vd_asfloat((VD_COL(unity_MatrixVP, 2).xyzw)) * vd_asfloat(r0.zzzz) + vd_asfloat(r2.xyzw)));
  o3.xyz = vd_asfloat(r0.xyz);
  r0.xyzw = vd_asuint((vd_asfloat(r2.xyzw) + vd_asfloat((VD_COL(unity_MatrixVP, 3).xyzw))));
  r1.x = vd_asuint((vd_asfloat(r0.z) + (-(vd_asfloat(_DecalMeshDepthBias)))));
  o0.z = vd_asfloat(((r1.y != 0u) ? r1.x : r0.z));
  o0.xyw = vd_asfloat(r0.xyw);
  r0.xyz = vd_asuint((vd_asfloat(v2.yyy) * vd_asfloat((VD_COL(unity_ObjectToWorld, 1).xyz))));
  r0.xyz = vd_asuint((vd_asfloat((VD_COL(unity_ObjectToWorld, 0).xyz)) * vd_asfloat(v2.xxx) + vd_asfloat(r0.xyz)));
  r0.xyz = vd_asuint((vd_asfloat((VD_COL(unity_ObjectToWorld, 2).xyz)) * vd_asfloat(v2.zzz) + vd_asfloat(r0.xyz)));
  r0.w = vd_asuint(dot(vd_asfloat(r0.xyz), vd_asfloat(r0.xyz)));
  r0.w = vd_asuint(max(vd_asfloat(r0.w), 1.17549435e-38));
  r0.w = vd_asuint(rsqrt(vd_asfloat(r0.w)));
  o1.xyz = (vd_asfloat(r0.www) * vd_asfloat(r0.xyz));
  o1.w = vd_asfloat(vd_asuint(v2.w));
  o2.xyzw = vd_asfloat(vd_asuint(v3.xyzw));
  r0.x = vd_asuint(dot(vd_asfloat(v1.xyz), vd_asfloat((VD_COL(unity_WorldToObject, 0).xyz))));
  r0.y = vd_asuint(dot(vd_asfloat(v1.xyz), vd_asfloat((VD_COL(unity_WorldToObject, 1).xyz))));
  r0.z = vd_asuint(dot(vd_asfloat(v1.xyz), vd_asfloat((VD_COL(unity_WorldToObject, 2).xyz))));
  r0.w = vd_asuint(dot(vd_asfloat(r0.xyz), vd_asfloat(r0.xyz)));
  r0.w = vd_asuint(max(vd_asfloat(r0.w), 1.17549435e-38));
  r0.w = vd_asuint(rsqrt(vd_asfloat(r0.w)));
  o4.xyz = (vd_asfloat(r0.www) * vd_asfloat(r0.xyz));
  return;
}
