// Hidden/ProBuilder/EdgePicker | pass 0 "Edges" | progVertex | keywords: (none) | blob 1
void vert(
  float4 v0 : POSITION0,
  float4 v1 : COLOR0,
  out float4 o0 : SV_POSITION0,
  out float4 o1 : VDREG1)
{
  uint4 r0, r1;
  r0 = 0u; r1 = 0u;

  r0.xyzw = vd_asuint((vd_asfloat(v0.yyyy) * vd_asfloat((VD_COL(unity_ObjectToWorld, 1).xyzw))));
  r0.xyzw = vd_asuint((vd_asfloat((VD_COL(unity_ObjectToWorld, 0).xyzw)) * vd_asfloat(v0.xxxx) + vd_asfloat(r0.xyzw)));
  r0.xyzw = vd_asuint((vd_asfloat((VD_COL(unity_ObjectToWorld, 2).xyzw)) * vd_asfloat(v0.zzzz) + vd_asfloat(r0.xyzw)));
  r0.xyzw = vd_asuint((vd_asfloat(r0.xyzw) + vd_asfloat((VD_COL(unity_ObjectToWorld, 3).xyzw))));
  r1.xyz = vd_asuint((vd_asfloat(r0.yyy) * vd_asfloat((VD_COL(unity_MatrixV, 1).xyz))));
  r1.xyz = vd_asuint((vd_asfloat((VD_COL(unity_MatrixV, 0).xyz)) * vd_asfloat(r0.xxx) + vd_asfloat(r1.xyz)));
  r0.xyz = vd_asuint((vd_asfloat((VD_COL(unity_MatrixV, 2).xyz)) * vd_asfloat(r0.zzz) + vd_asfloat(r1.xyz)));
  r0.xyz = vd_asuint((vd_asfloat((VD_COL(unity_MatrixV, 3).xyz)) * vd_asfloat(r0.www) + vd_asfloat(r0.xyz)));
  r1.xy = vd_asuint((vd_asfloat(unity_OrthoParams.ww) * float2(0.00999999046, -0.0500000119) + float2(0.99000001, 1.0)));
  r0.xyz = vd_asuint((vd_asfloat(r0.xyz) * vd_asfloat(r1.xxx)));
  r0.z = vd_asuint((vd_asfloat(r1.y) * vd_asfloat(r0.z)));
  r1.xyzw = vd_asuint((vd_asfloat(r0.yyyy) * vd_asfloat((VD_COL(glstate_matrix_projection, 1).xyzw))));
  r1.xyzw = vd_asuint((vd_asfloat((VD_COL(glstate_matrix_projection, 0).xyzw)) * vd_asfloat(r0.xxxx) + vd_asfloat(r1.xyzw)));
  r0.xyzw = vd_asuint((vd_asfloat((VD_COL(glstate_matrix_projection, 2).xyzw)) * vd_asfloat(r0.zzzz) + vd_asfloat(r1.xyzw)));
  o0.xyzw = (vd_asfloat(r0.xyzw) + vd_asfloat((VD_COL(glstate_matrix_projection, 3).xyzw)));
  o1.xyzw = vd_asfloat(vd_asuint(v1.xyzw));
  return;
}
