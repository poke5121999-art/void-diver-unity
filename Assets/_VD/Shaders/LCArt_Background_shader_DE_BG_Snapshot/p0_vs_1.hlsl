// LCArt/Background/shader_DE_BG_Snapshot | pass 0 "ForwardLit" | progVertex | keywords: (none) | blob 1
void vert(
  float4 v0 : POSITION0,
  float4 v1 : COLOR0,
  float2 v2 : TEXCOORD0,
  out float4 o0 : SV_POSITION0,
  out float3 o1 : TEXCOORD1,
  out float4 o2 : COLOR0,
  out float4 o3 : TEXCOORD0,
  out float3 o4 : TEXCOORD2)
{
  float4 r0,r1;
  uint4 bitmask, uiDest;
  float4 fDest;

  r0.xyz = (VD_COL(unity_ObjectToWorld, 1).xyz) * v0.yyy;
  r0.xyz = (VD_COL(unity_ObjectToWorld, 0).xyz) * v0.xxx + r0.xyz;
  r0.xyz = (VD_COL(unity_ObjectToWorld, 2).xyz) * v0.zzz + r0.xyz;
  r0.xyz = (VD_COL(unity_ObjectToWorld, 3).xyz) * v0.www + r0.xyz;
  r0.y = 2 + r0.y;
  r1.xyzw = (VD_COL(unity_WorldToObject, 1).xyzw) * r0.yyyy;
  r1.xyzw = (VD_COL(unity_WorldToObject, 0).xyzw) * r0.xxxx + r1.xyzw;
  r0.xyzw = (VD_COL(unity_WorldToObject, 2).xyzw) * r0.zzzz + r1.xyzw;
  r0.xyzw = (VD_COL(unity_WorldToObject, 3).xyzw) + r0.xyzw;
  r1.x = cmp(vd_asint(_SnapshotType) == 1);
  r0.xyzw = r1.xxxx ? r0.xyzw : v0.xyzw;
  r1.xyzw = (VD_COL(unity_ObjectToWorld, 1).xyzw) * r0.yyyy;
  r1.xyzw = (VD_COL(unity_ObjectToWorld, 0).xyzw) * r0.xxxx + r1.xyzw;
  r1.xyzw = (VD_COL(unity_ObjectToWorld, 2).xyzw) * r0.zzzz + r1.xyzw;
  o1.xyz = (VD_COL(unity_ObjectToWorld, 3).xyz) * r0.www + r1.xyz;
  r0.xyzw = (VD_COL(unity_ObjectToWorld, 3).xyzw) + r1.xyzw;
  r1.xyzw = (VD_COL(unity_MatrixVP, 1).xyzw) * r0.yyyy;
  r1.xyzw = (VD_COL(unity_MatrixVP, 0).xyzw) * r0.xxxx + r1.xyzw;
  r1.xyzw = (VD_COL(unity_MatrixVP, 2).xyzw) * r0.zzzz + r1.xyzw;
  o0.xyzw = (VD_COL(unity_MatrixVP, 3).xyzw) * r0.wwww + r1.xyzw;
  o2.xyzw = v1.xyzw;
  o3.xy = v2.xy;
  o4.xyz = (VD_COL(unity_ObjectToWorld, 1).xyz);
  return;
}
