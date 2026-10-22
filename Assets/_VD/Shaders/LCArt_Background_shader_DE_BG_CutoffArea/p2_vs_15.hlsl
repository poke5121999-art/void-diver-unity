// LCArt/Background/shader_DE_BG_CutoffArea | pass 2 "MotionVectors" | progVertex | keywords: (none) | blob 15
void vert(
  float3 v0 : POSITION0,
  float3 v1 : TEXCOORD4,
  out float3 o0 : CLIP_POSITION_NO_JITTER0,
  out float3 o1 : PREVIOUS_CLIP_POSITION_NO_JITTER0,
  out float4 o2 : SV_POSITION0,
  out float3 o3 : INTERP0)
{
  float4 r0,r1,r2,r3;
  uint4 bitmask, uiDest;
  float4 fDest;

  r0.xyz = -(VD_COL(unity_WorldToObject, 2).xyz) + (VD_COL(unity_WorldToObject, 0).xyz);
  r0.w = dot(r0.xyz, r0.xyz);
  r0.w = rsqrt(r0.w);
  r0.xyz = r0.xyz * r0.www;
  r1.xz = float2(0,0);
  r1.y = v0.y;
  r1.xyz = r0.xyz * float3(0.00100000005,0.00100000005,0.00100000005) + r1.xyz;
  r2.xz = v0.xz;
  r2.y = 0.00499999989;
  r1.xyz = r2.xyz + r1.xyz;
  r2.xyz = (VD_COL(unity_ObjectToWorld, 1).xyz) * r1.yyy;
  r2.xyz = (VD_COL(unity_ObjectToWorld, 0).xyz) * r1.xxx + r2.xyz;
  r2.xyz = (VD_COL(unity_ObjectToWorld, 2).xyz) * r1.zzz + r2.xyz;
  r2.xyz = (VD_COL(unity_ObjectToWorld, 3).xyz) + r2.xyz;
  r3.xyzw = (VD_COL(unity_MatrixVP, 1).xyzw) * r2.yyyy;
  r3.xyzw = (VD_COL(unity_MatrixVP, 0).xyzw) * r2.xxxx + r3.xyzw;
  r3.xyzw = (VD_COL(unity_MatrixVP, 2).xyzw) * r2.zzzz + r3.xyzw;
  o2.xyzw = (VD_COL(unity_MatrixVP, 3).xyzw) + r3.xyzw;
  r0.w = cmp(unity_MotionVectorsParams.y != 0.000000);
  if (r0.w != 0) {
    r0.w = cmp(unity_MotionVectorsParams.x == 1.000000);
    if (r0.w != 0) {
      r3.xz = float2(0,0);
      r3.y = v1.y;
      r0.xyz = r0.xyz * float3(0.00100000005,0.00100000005,0.00100000005) + r3.xyz;
      r3.xz = v1.xz;
      r3.y = 0.00499999989;
      r1.xyz = r3.xyz + r0.xyz;
    }
    r0.xyz = (VD_COL(_NonJitteredViewProjMatrix, 1).xyw) * r2.yyy;
    r0.xyz = (VD_COL(_NonJitteredViewProjMatrix, 0).xyw) * r2.xxx + r0.xyz;
    r0.xyz = (VD_COL(_NonJitteredViewProjMatrix, 2).xyw) * r2.zzz + r0.xyz;
    o0.xyz = (VD_COL(_NonJitteredViewProjMatrix, 3).xyw) + r0.xyz;
    r0.xyzw = (VD_COL(unity_MatrixPreviousM, 1).xyzw) * r1.yyyy;
    r0.xyzw = (VD_COL(unity_MatrixPreviousM, 0).xyzw) * r1.xxxx + r0.xyzw;
    r0.xyzw = (VD_COL(unity_MatrixPreviousM, 2).xyzw) * r1.zzzz + r0.xyzw;
    r0.xyzw = (VD_COL(unity_MatrixPreviousM, 3).xyzw) + r0.xyzw;
    r1.xyz = (VD_COL(_PrevViewProjMatrix, 1).xyw) * r0.yyy;
    r1.xyz = (VD_COL(_PrevViewProjMatrix, 0).xyw) * r0.xxx + r1.xyz;
    r0.xyz = (VD_COL(_PrevViewProjMatrix, 2).xyw) * r0.zzz + r1.xyz;
    o1.xyz = (VD_COL(_PrevViewProjMatrix, 3).xyw) * r0.www + r0.xyz;
  } else {
    o0.xyz = float3(0,0,0);
    o1.xyz = float3(0,0,0);
  }
  o3.xyz = r2.xyz;
  return;
}
