// Shader Graphs/shader_GlassReflection | pass 1 "MotionVectors" | progVertex | keywords: (none) | blob 19
void vert(
  float3 v0 : POSITION0,
  float3 v1 : TEXCOORD4,
  out float4 o0 : VDREG0,
  out float4 o1 : VDREG1,
  out float4 o2 : SV_POSITION0)
{
  uint4 r0, r1, r2;
  r0 = 0u; r1 = 0u; r2 = 0u;

  r0.xyz = vd_asuint((vd_asfloat(v0.yyy) * vd_asfloat((VD_COL(unity_ObjectToWorld, 1).xyz))));
  r0.xyz = vd_asuint((vd_asfloat((VD_COL(unity_ObjectToWorld, 0).xyz)) * vd_asfloat(v0.xxx) + vd_asfloat(r0.xyz)));
  r0.xyz = vd_asuint((vd_asfloat((VD_COL(unity_ObjectToWorld, 2).xyz)) * vd_asfloat(v0.zzz) + vd_asfloat(r0.xyz)));
  r0.xyz = vd_asuint((vd_asfloat(r0.xyz) + vd_asfloat((VD_COL(unity_ObjectToWorld, 3).xyz))));
  r1.xyzw = vd_asuint((vd_asfloat(r0.yyyy) * vd_asfloat((VD_COL(unity_MatrixVP, 1).xyzw))));
  r1.xyzw = vd_asuint((vd_asfloat((VD_COL(unity_MatrixVP, 0).xyzw)) * vd_asfloat(r0.xxxx) + vd_asfloat(r1.xyzw)));
  r1.xyzw = vd_asuint((vd_asfloat((VD_COL(unity_MatrixVP, 2).xyzw)) * vd_asfloat(r0.zzzz) + vd_asfloat(r1.xyzw)));
  o2.xyzw = (vd_asfloat(r1.xyzw) + vd_asfloat((VD_COL(unity_MatrixVP, 3).xyzw)));
  r0.w = vd_asuint((-(int)(vd_asfloat(unity_MotionVectorsParams.y) != 0.0)));
  if (r0.w != 0u) {
    r0.w = vd_asuint((-(int)(vd_asfloat(unity_MotionVectorsParams.x) == 1.0)));
    r1.xyz = ((r0.www != 0u) ? vd_asuint(v1.xyz) : vd_asuint(v0.xyz));
    r2.xyz = vd_asuint((vd_asfloat(r0.yyy) * vd_asfloat((VD_COL(_NonJitteredViewProjMatrix, 1).xyw))));
    r0.xyw = vd_asuint((vd_asfloat((VD_COL(_NonJitteredViewProjMatrix, 0).xyw)) * vd_asfloat(r0.xxx) + vd_asfloat(r2.xyz)));
    r0.xyz = vd_asuint((vd_asfloat((VD_COL(_NonJitteredViewProjMatrix, 2).xyw)) * vd_asfloat(r0.zzz) + vd_asfloat(r0.xyw)));
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
  return;
}
