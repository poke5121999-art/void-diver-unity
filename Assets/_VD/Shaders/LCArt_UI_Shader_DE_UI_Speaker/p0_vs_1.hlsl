// LCArt/UI/Shader_DE_UI_Speaker | pass 0 "Default" | progVertex | keywords: (none) | blob 1
void vert(
  float4 v0 : POSITION0,
  float4 v1 : COLOR0,
  float2 v2 : TEXCOORD0,
  out float4 o0 : SV_POSITION0,
  out float4 o1 : VDREG1,
  out float4 o2 : VDREG2,
  out float4 o3 : VDREG3,
  out float4 o4 : VDREG4)
{
  uint4 r0, r1, r2;
  r0 = 0u; r1 = 0u; r2 = 0u;

  r0.xyzw = vd_asuint((vd_asfloat(v0.yyyy) * vd_asfloat((VD_COL(unity_ObjectToWorld, 1).xyzw))));
  r0.xyzw = vd_asuint((vd_asfloat((VD_COL(unity_ObjectToWorld, 0).xyzw)) * vd_asfloat(v0.xxxx) + vd_asfloat(r0.xyzw)));
  r0.xyzw = vd_asuint((vd_asfloat((VD_COL(unity_ObjectToWorld, 2).xyzw)) * vd_asfloat(v0.zzzz) + vd_asfloat(r0.xyzw)));
  r0.xyzw = vd_asuint((vd_asfloat(r0.xyzw) + vd_asfloat((VD_COL(unity_ObjectToWorld, 3).xyzw))));
  r1.xyzw = vd_asuint((vd_asfloat(r0.yyyy) * vd_asfloat((VD_COL(unity_MatrixVP, 1).xyzw))));
  r1.xyzw = vd_asuint((vd_asfloat((VD_COL(unity_MatrixVP, 0).xyzw)) * vd_asfloat(r0.xxxx) + vd_asfloat(r1.xyzw)));
  r1.xyzw = vd_asuint((vd_asfloat((VD_COL(unity_MatrixVP, 2).xyzw)) * vd_asfloat(r0.zzzz) + vd_asfloat(r1.xyzw)));
  r0.xyzw = vd_asuint((vd_asfloat((VD_COL(unity_MatrixVP, 3).xyzw)) * vd_asfloat(r0.wwww) + vd_asfloat(r1.xyzw)));
  o0.xyzw = vd_asfloat(r0.xyzw);
  o1.xyzw = (vd_asfloat(v1.xyzw) * vd_asfloat(_Color.xyzw));
  o2.xy = vd_asfloat(vd_asuint(v2.xy));
  r1.x = vd_asuint((vd_asfloat(_GlobalDialogRimLightAngle) * 0.0174532887));
  r1.x = vd_asuint(sin(vd_asfloat(r1.x)));
  r2.x = vd_asuint(cos(vd_asfloat(r1.x)));
  o2.z = vd_asfloat(r2.x);
  o2.w = vd_asfloat(r1.x);
  o3.xyzw = vd_asfloat(vd_asuint(v0.xyzw));
  r0.y = vd_asuint((vd_asfloat(r0.y) * vd_asfloat(_ProjectionParams.x)));
  r1.xzw = vd_asuint((vd_asfloat(r0.xwy) * float3(0.5, 0.5, 0.5)));
  o4.zw = vd_asfloat(r0.zw);
  o4.xy = (vd_asfloat(r1.zz) + vd_asfloat(r1.xw));
  return;
}
