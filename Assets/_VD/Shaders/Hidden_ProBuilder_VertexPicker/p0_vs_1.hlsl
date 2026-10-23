// Hidden/ProBuilder/VertexPicker | pass 0 "Vertices" | progVertex | keywords: (none) | blob 1
void vert(
  float4 v0 : POSITION0,
  float3 v1 : NORMAL0,
  float4 v2 : COLOR0,
  float4 v3 : TEXCOORD0,
  float2 v4 : TEXCOORD1,
  out float4 o0 : SV_POSITION0,
  out float4 o1 : VDREG1,
  out float4 o2 : VDREG2)
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
  r0.w = vd_asuint(((-(vd_asfloat((VD_COL(glstate_matrix_projection, 3).w)))) + 1.0));
  r1.x = vd_asuint((vd_asfloat(r0.w) * -0.0400000215 + 0.99000001));
  r0.xyz = vd_asuint((vd_asfloat(r0.xyz) * vd_asfloat(r1.xxx)));
  r1.xyzw = vd_asuint((vd_asfloat(r0.yyyy) * vd_asfloat((VD_COL(glstate_matrix_projection, 1).xyzw))));
  r1.xyzw = vd_asuint((vd_asfloat((VD_COL(glstate_matrix_projection, 0).xyzw)) * vd_asfloat(r0.xxxx) + vd_asfloat(r1.xyzw)));
  r1.xyzw = vd_asuint((vd_asfloat((VD_COL(glstate_matrix_projection, 2).xyzw)) * vd_asfloat(r0.zzzz) + vd_asfloat(r1.xyzw)));
  r1.xyzw = vd_asuint((vd_asfloat(r1.xyzw) + vd_asfloat((VD_COL(glstate_matrix_projection, 3).xyzw))));
  r0.xy = vd_asuint((vd_asfloat(r1.xy) / vd_asfloat(r1.ww)));
  r0.xy = vd_asuint((vd_asfloat(r0.xy) * float2(0.5, 0.5) + float2(0.5, 0.5)));
  r1.xy = vd_asuint((vd_asfloat(v4.xy) * float2(3.5, 3.5)));
  r0.xy = vd_asuint((vd_asfloat(r0.xy) * vd_asfloat(_ScreenParams.xy) + vd_asfloat(r1.xy)));
  r0.xy = vd_asuint((vd_asfloat(r0.xy) / vd_asfloat(_ScreenParams.xy)));
  r0.xy = vd_asuint((vd_asfloat(r0.xy) + float2(-0.5, -0.5)));
  r0.xy = vd_asuint((vd_asfloat(r1.ww) * vd_asfloat(r0.xy)));
  o0.xy = (vd_asfloat(r0.xy) + vd_asfloat(r0.xy));
  o0.z = ((-(vd_asfloat(r0.w))) * 9.99999975e-05 + vd_asfloat(r1.z));
  o0.w = vd_asfloat(r1.w);
  o1.xy = vd_asfloat(vd_asuint(v3.xy));
  o2.xyzw = vd_asfloat(vd_asuint(v2.xyzw));
  return;
}
