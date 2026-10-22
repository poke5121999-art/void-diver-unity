// LCArt/Indicator/Shader_DE_Indicator_Billboard | pass 0 "" | progVertex | keywords: _ADDITIONAL_LIGHTS | blob 6
void vert(
  float4 v0 : POSITION0,
  float2 v1 : TEXCOORD0,
  float4 v2 : COLOR0,
  out float4 o0 : TEXCOORD0,
  out float3 o1 : TEXCOORD1,
  out float4 o2 : SV_POSITION0,
  out float4 o3 : COLOR0)
{
  float4 r0,r1;
  uint4 bitmask, uiDest;
  float4 fDest;

  o0.xy = v1.xy * _MainTex_ST.xy + _MainTex_ST.zw;
  r0.x = (VD_COL(unity_MatrixV, 0).x) * v0.x;
  r0.y = (VD_COL(unity_MatrixV, 1).x) * v0.x;
  r0.z = (VD_COL(unity_MatrixV, 2).x) * v0.x;
  r1.x = (VD_COL(unity_MatrixV, 0).y) * v0.y;
  r1.y = (VD_COL(unity_MatrixV, 1).y) * v0.y;
  r1.z = (VD_COL(unity_MatrixV, 2).y) * v0.y;
  r0.xyz = r1.xyz + r0.xyz;
  r0.xyz = r0.xyz * (_Scale).xxx + (VD_COL(unity_ObjectToWorld, 3).xyz);
  o1.xyz = r0.xyz;
  r1.xyzw = (VD_COL(unity_MatrixVP, 1).xyzw) * r0.yyyy;
  r1.xyzw = (VD_COL(unity_MatrixVP, 0).xyzw) * r0.xxxx + r1.xyzw;
  r0.xyzw = (VD_COL(unity_MatrixVP, 2).xyzw) * r0.zzzz + r1.xyzw;
  o2.xyzw = (VD_COL(unity_MatrixVP, 3).xyzw) + r0.xyzw;
  o3.xyzw = v2.xyzw;
  return;
}
