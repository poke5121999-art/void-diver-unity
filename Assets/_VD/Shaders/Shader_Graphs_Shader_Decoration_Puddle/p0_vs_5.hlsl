// Shader Graphs/Shader_Decoration_Puddle | pass 0 "DBufferProjector" | progVertex | keywords: _DE_WEATHER_RAIN | blob 5
void vert(
  float3 v0 : POSITION0,
  float4 v1 : TEXCOORD0,
  out float4 o0 : SV_POSITION0,
  out float4 o1 : INTERP0,
  out float3 o2 : INTERP1)
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
  o2.xyz = r0.xyz;
  o0.xyzw = (VD_COL(unity_MatrixVP, 3).xyzw) + r1.xyzw;
  o1.xyzw = v1.xyzw;
  return;
}
