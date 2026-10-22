// ArtTeam/VFX/VFX_Master_typeA_Trail | pass 2 "MotionVectors" | progVertex | keywords: (none) | blob 34
void vert(
  float3 v0 : POSITION0,
  float3 v1 : NORMAL0,
  float4 v2 : TEXCOORD0,
  float4 v3 : COLOR0,
  float3 v4 : TEXCOORD4,
  out float3 o0 : CLIP_POSITION_NO_JITTER0,
  out float3 o1 : PREVIOUS_CLIP_POSITION_NO_JITTER0,
  out float4 o2 : SV_POSITION0,
  out float4 o3 : INTERP0,
  out float4 o4 : INTERP1,
  out float3 o5 : INTERP2,
  out float3 o6 : INTERP3)
{
  float4 r0,r1,r2;
  uint4 bitmask, uiDest;
  float4 fDest;

  r0.xyz = (VD_COL(unity_ObjectToWorld, 1).xyz) * v0.yyy;
  r0.xyz = (VD_COL(unity_ObjectToWorld, 0).xyz) * v0.xxx + r0.xyz;
  r0.xyz = (VD_COL(unity_ObjectToWorld, 2).xyz) * v0.zzz + r0.xyz;
  r0.xyz = (VD_COL(unity_ObjectToWorld, 3).xyz) + r0.xyz;
  r1.x = dot(v1.xyz, (VD_COL(unity_WorldToObject, 0).xyz));
  r1.y = dot(v1.xyz, (VD_COL(unity_WorldToObject, 1).xyz));
  r1.z = dot(v1.xyz, (VD_COL(unity_WorldToObject, 2).xyz));
  r0.w = dot(r1.xyz, r1.xyz);
  r0.w = max(1.17549435e-038, r0.w);
  r0.w = rsqrt(r0.w);
  o6.xyz = r1.xyz * r0.www;
  r1.xyzw = (VD_COL(unity_MatrixVP, 1).xyzw) * r0.yyyy;
  r1.xyzw = (VD_COL(unity_MatrixVP, 0).xyzw) * r0.xxxx + r1.xyzw;
  r1.xyzw = (VD_COL(unity_MatrixVP, 2).xyzw) * r0.zzzz + r1.xyzw;
  o2.xyzw = (VD_COL(unity_MatrixVP, 3).xyzw) + r1.xyzw;
  r0.w = cmp(unity_MotionVectorsParams.y != 0.000000);
  if (r0.w != 0) {
    r0.w = cmp(unity_MotionVectorsParams.x == 1.000000);
    r1.xyz = r0.www ? v4.xyz : v0.xyz;
    r2.xyz = (VD_COL(_NonJitteredViewProjMatrix, 1).xyw) * r0.yyy;
    r2.xyz = (VD_COL(_NonJitteredViewProjMatrix, 0).xyw) * r0.xxx + r2.xyz;
    r2.xyz = (VD_COL(_NonJitteredViewProjMatrix, 2).xyw) * r0.zzz + r2.xyz;
    o0.xyz = (VD_COL(_NonJitteredViewProjMatrix, 3).xyw) + r2.xyz;
    r2.xyzw = (VD_COL(unity_MatrixPreviousM, 1).xyzw) * r1.yyyy;
    r2.xyzw = (VD_COL(unity_MatrixPreviousM, 0).xyzw) * r1.xxxx + r2.xyzw;
    r1.xyzw = (VD_COL(unity_MatrixPreviousM, 2).xyzw) * r1.zzzz + r2.xyzw;
    r1.xyzw = (VD_COL(unity_MatrixPreviousM, 3).xyzw) + r1.xyzw;
    r2.xyz = (VD_COL(_PrevViewProjMatrix, 1).xyw) * r1.yyy;
    r2.xyz = (VD_COL(_PrevViewProjMatrix, 0).xyw) * r1.xxx + r2.xyz;
    r1.xyz = (VD_COL(_PrevViewProjMatrix, 2).xyw) * r1.zzz + r2.xyz;
    o1.xyz = (VD_COL(_PrevViewProjMatrix, 3).xyw) * r1.www + r1.xyz;
  } else {
    o0.xyz = float3(0,0,0);
    o1.xyz = float3(0,0,0);
  }
  o3.xyzw = v2.xyzw;
  o4.xyzw = v3.xyzw;
  o5.xyz = r0.xyz;
  return;
}
