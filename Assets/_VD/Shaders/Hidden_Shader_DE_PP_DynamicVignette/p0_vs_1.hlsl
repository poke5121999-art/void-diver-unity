// Hidden/Shader_DE_PP_DynamicVignette | pass 0 "DrawProcedural" | progVertex | keywords: (none) | blob 1
void vert(
  uint v0 : SV_VertexID0,
  out float4 o0 : SV_POSITION0,
  out float4 o1 : INTERP0,
  out float4 o2 : INTERP1)
{
// Needs manual fix for instruction:
// unknown dcl_: dcl_input_sgv v0.x, vertex_id
  float4 r0,r1;
  uint4 bitmask, uiDest;
  float4 fDest;

  r0.x = (uint)v0.x << 1;
  r0.x = (int)r0.x & 2;
  r0.z = (int)v0.x & 2;
  r0.xy = (uint2)r0.xz;
  r0.xy = r0.xy * float2(2,2) + float2(-1,-1);
  o0.xy = r0.xy;
  o0.zw = float2(1,1);
  r1.xy = float2(0.5,0.5) * r0.xy;
  r1.z = 0.5;
  r1.xyz = float3(0.5,0.5,0.5) + r1.xyz;
  r0.z = 1 + -r1.y;
  r0.w = cmp(_FlipY < 0.5);
  r1.w = r0.w ? r0.z : r1.y;
  o1.xyzw = r1.xwzz;
  r1.xyzw = (VD_COL(unity_MatrixInvVP, 1).xyzw) * r0.yyyy;
  r0.xyzw = (VD_COL(unity_MatrixInvVP, 0).xyzw) * r0.xxxx + r1.xyzw;
  r0.xyzw = (VD_COL(unity_MatrixInvVP, 2).xyzw) + r0.xyzw;
  r0.xyzw = (VD_COL(unity_MatrixInvVP, 3).xyzw) + r0.xyzw;
  r0.xyz = r0.xyz / r0.www;
  r0.xyz = _WorldSpaceCameraPos.xyz + -r0.xyz;
  r0.w = cmp(unity_OrthoParams.w == 0.000000);
  o2.x = r0.w ? r0.x : (VD_COL(unity_MatrixV, 0).z);
  o2.y = r0.w ? r0.y : (VD_COL(unity_MatrixV, 1).z);
  o2.z = r0.w ? r0.z : (VD_COL(unity_MatrixV, 2).z);
  o2.w = 0;
  return;
}
