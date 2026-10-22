// LCArt/Temp/Shader_CQ_Background | pass 0 "ForwardLit" | progVertex | keywords: FOG_LINEAR _CLUSTER_LIGHT_LOOP _LIGHT_LAYERS | blob 7
void vert(
  float4 v0 : POSITION0,
  float3 v1 : NORMAL0,
  float4 v2 : TANGENT0,
  float4 v3 : TEXCOORD0,
  float4 v4 : TEXCOORD1,
  float2 v5 : TEXCOORD2,
  float4 v6 : COLOR0,
  out float2 o0 : TEXCOORD0,
  out float p0 : TEXCOORD5,
  out float4 o1 : TEXCOORD1,
  out float4 o2 : TEXCOORD2,
  out float3 o3 : TEXCOORD7,
  out float4 o4 : SV_POSITION0,
  out float4 o5 : COLOR0)
{
  float4 r0,r1;
  uint4 bitmask, uiDest;
  float4 fDest;

  o0.xy = v3.xy * _BaseMap_ST.xy + _BaseMap_ST.zw;
  p0.x = 0;
  r0.xyz = (VD_COL(unity_ObjectToWorld, 1).xyz) * v0.yyy;
  r0.xyz = (VD_COL(unity_ObjectToWorld, 0).xyz) * v0.xxx + r0.xyz;
  r0.xyz = (VD_COL(unity_ObjectToWorld, 2).xyz) * v0.zzz + r0.xyz;
  r0.xyz = (VD_COL(unity_ObjectToWorld, 3).xyz) + r0.xyz;
  o1.xyz = r0.xyz;
  r1.x = dot(v1.xyz, (VD_COL(unity_WorldToObject, 0).xyz));
  r1.y = dot(v1.xyz, (VD_COL(unity_WorldToObject, 1).xyz));
  r1.z = dot(v1.xyz, (VD_COL(unity_WorldToObject, 2).xyz));
  r0.w = dot(r1.xyz, r1.xyz);
  r0.w = max(1.17549435e-038, r0.w);
  r0.w = rsqrt(r0.w);
  r1.xyz = r1.xyz * r0.www;
  r0.w = dot(r1.xyz, r1.xyz);
  r0.w = rsqrt(r0.w);
  o2.xyz = r1.xyz * r0.www;
  o3.xyz = float3(0,0,0);
  r1.xyzw = (VD_COL(unity_MatrixVP, 1).xyzw) * r0.yyyy;
  r1.xyzw = (VD_COL(unity_MatrixVP, 0).xyzw) * r0.xxxx + r1.xyzw;
  r0.xyzw = (VD_COL(unity_MatrixVP, 2).xyzw) * r0.zzzz + r1.xyzw;
  o4.xyzw = (VD_COL(unity_MatrixVP, 3).xyzw) + r0.xyzw;
  o5.xyzw = v6.xyzw;
  return;
}
