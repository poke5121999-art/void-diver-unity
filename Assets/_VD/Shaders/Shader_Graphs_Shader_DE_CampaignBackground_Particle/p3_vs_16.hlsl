// Shader Graphs/Shader_DE_CampaignBackground_Particle | pass 3 "DepthNormalsOnly" | progVertex | keywords: (none) | blob 16
void vert(
  float3 v0 : POSITION0,
  float3 v1 : NORMAL0,
  float4 v2 : TANGENT0,
  out float4 o0 : SV_POSITION0,
  out float3 o1 : INTERP0)
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
  r0.xyzw = (VD_COL(unity_MatrixVP, 2).xyzw) * r0.zzzz + r1.xyzw;
  o0.xyzw = (VD_COL(unity_MatrixVP, 3).xyzw) + r0.xyzw;
  r0.x = dot(v1.xyz, (VD_COL(unity_WorldToObject, 0).xyz));
  r0.y = dot(v1.xyz, (VD_COL(unity_WorldToObject, 1).xyz));
  r0.z = dot(v1.xyz, (VD_COL(unity_WorldToObject, 2).xyz));
  r0.w = dot(r0.xyz, r0.xyz);
  r0.w = max(1.17549435e-038, r0.w);
  r0.w = rsqrt(r0.w);
  o1.xyz = r0.xyz * r0.www;
  return;
}
