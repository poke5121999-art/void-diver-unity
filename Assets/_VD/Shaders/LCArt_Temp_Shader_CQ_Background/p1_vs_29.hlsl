// LCArt/Temp/Shader_CQ_Background | pass 1 "DepthNormals" | progVertex | keywords: (none) | blob 29
void vert(
  float4 v0 : POSITION0,
  float4 v1 : TANGENT0,
  float2 v2 : TEXCOORD0,
  float3 v3 : NORMAL0,
  out float4 o0 : SV_POSITION0,
  out float4 o1 : TEXCOORD2,
  out float3 o2 : TEXCOORD3)
{
  float4 r0,r1;
  uint4 bitmask, uiDest;
  float4 fDest;

  r0.xyzw = (VD_COL(unity_ObjectToWorld, 1).xyzw) * v0.yyyy;
  r0.xyzw = (VD_COL(unity_ObjectToWorld, 0).xyzw) * v0.xxxx + r0.xyzw;
  r0.xyzw = (VD_COL(unity_ObjectToWorld, 2).xyzw) * v0.zzzz + r0.xyzw;
  r0.xyzw = (VD_COL(unity_ObjectToWorld, 3).xyzw) + r0.xyzw;
  r1.xyzw = (VD_COL(unity_MatrixVP, 1).xyzw) * r0.yyyy;
  r1.xyzw = (VD_COL(unity_MatrixVP, 0).xyzw) * r0.xxxx + r1.xyzw;
  r1.xyzw = (VD_COL(unity_MatrixVP, 2).xyzw) * r0.zzzz + r1.xyzw;
  o0.xyzw = (VD_COL(unity_MatrixVP, 3).xyzw) * r0.wwww + r1.xyzw;
  r0.x = dot(v3.xyz, (VD_COL(unity_WorldToObject, 0).xyz));
  r0.y = dot(v3.xyz, (VD_COL(unity_WorldToObject, 1).xyz));
  r0.z = dot(v3.xyz, (VD_COL(unity_WorldToObject, 2).xyz));
  r0.w = dot(r0.xyz, r0.xyz);
  r0.w = max(1.17549435e-038, r0.w);
  r0.w = rsqrt(r0.w);
  r0.xyz = r0.xyz * r0.www;
  r0.w = dot(r0.xyz, r0.xyz);
  r0.w = rsqrt(r0.w);
  o1.xyz = r0.xyz * r0.www;
  o2.xyz = float3(0,0,0);
  return;
}
