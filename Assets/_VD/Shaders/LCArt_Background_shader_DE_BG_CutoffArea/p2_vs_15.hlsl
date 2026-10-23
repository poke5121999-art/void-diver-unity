// LCArt/Background/shader_DE_BG_CutoffArea | pass 2 "MotionVectors" | progVertex | keywords: (none) | blob 15
void vert(
  float3 v0 : POSITION0,
  float3 v1 : TEXCOORD4,
  out float4 o0 : VDREG0,
  out float4 o1 : VDREG1,
  out float4 o2 : SV_POSITION0,
  out float4 o3 : VDREG3)
{
  uint4 r0, r1, r2, r3;
  r0 = 0u; r1 = 0u; r2 = 0u; r3 = 0u;

  r0.xyz = vd_asuint((vd_asfloat((VD_COL(unity_WorldToObject, 0).xyz)) + (-(vd_asfloat((VD_COL(unity_WorldToObject, 2).xyz))))));
  r0.w = vd_asuint(dot(vd_asfloat(r0.xyz), vd_asfloat(r0.xyz)));
  r0.w = vd_asuint(rsqrt(vd_asfloat(r0.w)));
  r0.xyz = vd_asuint((vd_asfloat(r0.www) * vd_asfloat(r0.xyz)));
  r1.xz = uint2(0x00000000u, 0x00000000u);
  r1.y = vd_asuint(v0.y);
  r1.xyz = vd_asuint((vd_asfloat(r0.xyz) * float3(0.00100000005, 0.00100000005, 0.00100000005) + vd_asfloat(r1.xyz)));
  r2.xz = vd_asuint(v0.xz);
  r2.y = 0x3ba3d70au;
  r1.xyz = vd_asuint((vd_asfloat(r1.xyz) + vd_asfloat(r2.xyz)));
  r2.xyz = vd_asuint((vd_asfloat(r1.yyy) * vd_asfloat((VD_COL(unity_ObjectToWorld, 1).xyz))));
  r2.xyz = vd_asuint((vd_asfloat((VD_COL(unity_ObjectToWorld, 0).xyz)) * vd_asfloat(r1.xxx) + vd_asfloat(r2.xyz)));
  r2.xyz = vd_asuint((vd_asfloat((VD_COL(unity_ObjectToWorld, 2).xyz)) * vd_asfloat(r1.zzz) + vd_asfloat(r2.xyz)));
  r2.xyz = vd_asuint((vd_asfloat(r2.xyz) + vd_asfloat((VD_COL(unity_ObjectToWorld, 3).xyz))));
  r3.xyzw = vd_asuint((vd_asfloat(r2.yyyy) * vd_asfloat((VD_COL(unity_MatrixVP, 1).xyzw))));
  r3.xyzw = vd_asuint((vd_asfloat((VD_COL(unity_MatrixVP, 0).xyzw)) * vd_asfloat(r2.xxxx) + vd_asfloat(r3.xyzw)));
  r3.xyzw = vd_asuint((vd_asfloat((VD_COL(unity_MatrixVP, 2).xyzw)) * vd_asfloat(r2.zzzz) + vd_asfloat(r3.xyzw)));
  o2.xyzw = (vd_asfloat(r3.xyzw) + vd_asfloat((VD_COL(unity_MatrixVP, 3).xyzw)));
  r0.w = vd_asuint((-(int)(vd_asfloat(unity_MotionVectorsParams.y) != 0.0)));
  if (r0.w != 0u) {
    r0.w = vd_asuint((-(int)(vd_asfloat(unity_MotionVectorsParams.x) == 1.0)));
    if (r0.w != 0u) {
      r3.xz = uint2(0x00000000u, 0x00000000u);
      r3.y = vd_asuint(v1.y);
      r0.xyz = vd_asuint((vd_asfloat(r0.xyz) * float3(0.00100000005, 0.00100000005, 0.00100000005) + vd_asfloat(r3.xyz)));
      r3.xz = vd_asuint(v1.xz);
      r3.y = 0x3ba3d70au;
      r1.xyz = vd_asuint((vd_asfloat(r0.xyz) + vd_asfloat(r3.xyz)));
    }
    r0.xyz = vd_asuint((vd_asfloat(r2.yyy) * vd_asfloat((VD_COL(_NonJitteredViewProjMatrix, 1).xyw))));
    r0.xyz = vd_asuint((vd_asfloat((VD_COL(_NonJitteredViewProjMatrix, 0).xyw)) * vd_asfloat(r2.xxx) + vd_asfloat(r0.xyz)));
    r0.xyz = vd_asuint((vd_asfloat((VD_COL(_NonJitteredViewProjMatrix, 2).xyw)) * vd_asfloat(r2.zzz) + vd_asfloat(r0.xyz)));
    o0.xyz = (vd_asfloat(r0.xyz) + vd_asfloat((VD_COL(_NonJitteredViewProjMatrix, 3).xyw)));
    r0.xyzw = vd_asuint((vd_asfloat(r1.yyyy) * vd_asfloat((VD_COL(unity_MatrixPreviousM, 1).xyzw))));
    r0.xyzw = vd_asuint((vd_asfloat((VD_COL(unity_MatrixPreviousM, 0).xyzw)) * vd_asfloat(r1.xxxx) + vd_asfloat(r0.xyzw)));
    r0.xyzw = vd_asuint((vd_asfloat((VD_COL(unity_MatrixPreviousM, 2).xyzw)) * vd_asfloat(r1.zzzz) + vd_asfloat(r0.xyzw)));
    r0.xyzw = vd_asuint((vd_asfloat(r0.xyzw) + vd_asfloat((VD_COL(unity_MatrixPreviousM, 3).xyzw))));
    r1.xyz = vd_asuint((vd_asfloat(r0.yyy) * vd_asfloat((VD_COL(_PrevViewProjMatrix, 1).xyw))));
    r1.xyz = vd_asuint((vd_asfloat((VD_COL(_PrevViewProjMatrix, 0).xyw)) * vd_asfloat(r0.xxx) + vd_asfloat(r1.xyz)));
    r0.xyz = vd_asuint((vd_asfloat((VD_COL(_PrevViewProjMatrix, 2).xyw)) * vd_asfloat(r0.zzz) + vd_asfloat(r1.xyz)));
    o1.xyz = (vd_asfloat((VD_COL(_PrevViewProjMatrix, 3).xyw)) * vd_asfloat(r0.www) + vd_asfloat(r0.xyz));
  } else {
    o0.xyz = vd_asfloat(uint3(0x00000000u, 0x00000000u, 0x00000000u));
    o1.xyz = vd_asfloat(uint3(0x00000000u, 0x00000000u, 0x00000000u));
  }
  o3.xyz = vd_asfloat(r2.xyz);
  return;
}
