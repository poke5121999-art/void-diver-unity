// LCArt/VFX/Shader_VFX_Grabpass_Distortion | pass 0 "" | progVertex | keywords: (none) | blob 1
void vert(
  float4 v0 : POSITION0,
  float3 v1 : NORMAL0,
  float4 v2 : COLOR0,
  float4 v3 : TEXCOORD0,
  float4 v4 : TEXCOORD1,
  float4 v5 : TEXCOORD2,
  out float4 o0 : SV_POSITION0,
  out float4 o1 : COLOR0,
  out float3 o2 : NORMAL0,
  out float2 o3 : TEXCOORD0,
  out float2 p3 : TEXCOORD2,
  out float4 o4 : TEXCOORD1,
  out float4 o5 : TEXCOORD3,
  out float4 o6 : TEXCOORD4)
{
  float4 r0,r1;
  uint4 bitmask, uiDest;
  float4 fDest;

  r0.xyzw = (VD_COL(unity_ObjectToWorld, 1).xyzw) * v0.yyyy;
  r0.xyzw = (VD_COL(unity_ObjectToWorld, 0).xyzw) * v0.xxxx + r0.xyzw;
  r0.xyzw = (VD_COL(unity_ObjectToWorld, 2).xyzw) * v0.zzzz + r0.xyzw;
  r1.xyzw = (VD_COL(unity_ObjectToWorld, 3).xyzw) + r0.xyzw;
  o4.xyz = (VD_COL(unity_ObjectToWorld, 3).xyz) * v0.www + r0.xyz;
  r0.xyzw = (VD_COL(unity_MatrixVP, 1).xyzw) * r1.yyyy;
  r0.xyzw = (VD_COL(unity_MatrixVP, 0).xyzw) * r1.xxxx + r0.xyzw;
  r0.xyzw = (VD_COL(unity_MatrixVP, 2).xyzw) * r1.zzzz + r0.xyzw;
  r0.xyzw = (VD_COL(unity_MatrixVP, 3).xyzw) * r1.wwww + r0.xyzw;
  o0.xyzw = r0.xyzw;
  r0.xy = r0.xy / -r0.ww;
  r0.xy = r0.xy * float2(0.5,0.5) + float2(0.5,0.5);
  p3.xy = r0.xy * float2(-1,1) + float2(1,0);
  o1.xyzw = v2.xyzw;
  r0.x = dot(v1.xyz, (VD_COL(unity_WorldToObject, 0).xyz));
  r0.y = dot(v1.xyz, (VD_COL(unity_WorldToObject, 1).xyz));
  r0.z = dot(v1.xyz, (VD_COL(unity_WorldToObject, 2).xyz));
  r0.w = dot(r0.xyz, r0.xyz);
  r0.w = max(1.17549435e-038, r0.w);
  r0.w = rsqrt(r0.w);
  o2.xyz = r0.xyz * r0.www;
  o3.xy = v3.xy * _NormalMap_ST.xy + _NormalMap_ST.zw;
  o5.xyzw = v4.xyzw;
  o6.xyzw = v5.xyzw;
  return;
}
