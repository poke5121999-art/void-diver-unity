// LCArt/Character/Shader_MaskArea | pass 0 "" | progVertex | keywords: (none) | blob 1
void vert(
  float4 v0 : POSITION0,
  float2 v1 : TEXCOORD0,
  float3 v2 : NORMAL0,
  out float4 o0 : SV_POSITION0,
  out float2 o1 : TEXCOORD0,
  out float p1 : TEXCOORD2,
  out float3 o2 : TEXCOORD1)
{
  float4 r0,r1;
  uint4 bitmask, uiDest;
  float4 fDest;

  r0.xyz = (VD_COL(unity_ObjectToWorld, 1).xyz) * v0.yyy;
  r0.xyz = (VD_COL(unity_ObjectToWorld, 0).xyz) * v0.xxx + r0.xyz;
  r0.xyz = (VD_COL(unity_ObjectToWorld, 2).xyz) * v0.zzz + r0.xyz;
  r0.xyz = (VD_COL(unity_ObjectToWorld, 3).xyz) + r0.xyz;
  r1.xyzw = (VD_COL(unity_MatrixVP, 1).xyzw) * r0.yyyy;
  r1.xyzw = (VD_COL(unity_MatrixVP, 0).xyzw) * r0.xxxx + r1.xyzw;
  r1.xyzw = (VD_COL(unity_MatrixVP, 2).xyzw) * r0.zzzz + r1.xyzw;
  r0.xyz = _WorldSpaceCameraPos.xyz + -r0.xyz;
  r1.xyzw = (VD_COL(unity_MatrixVP, 3).xyzw) + r1.xyzw;
  o0.xyzw = r1.xyzw;
  p1.x = r1.z / r1.w;
  o1.xy = v1.xy * _MainTex_ST.xy + _MainTex_ST.zw;
  r0.w = cmp(unity_OrthoParams.w == 0.000000);
  o2.x = r0.w ? r0.x : (VD_COL(unity_MatrixV, 0).z);
  o2.y = r0.w ? r0.y : (VD_COL(unity_MatrixV, 1).z);
  o2.z = r0.w ? r0.z : (VD_COL(unity_MatrixV, 2).z);
  return;
}
