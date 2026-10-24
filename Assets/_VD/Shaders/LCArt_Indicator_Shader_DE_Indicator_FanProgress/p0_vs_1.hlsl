// LCArt/Indicator/Shader_DE_Indicator_FanProgress | pass 0 "ForwardUnlit" | progVertex | keywords: (none) | blob 1
void vert(
  float4 v0 : POSITION0,
  float2 v1 : TEXCOORD0,
  out float4 o0 : SV_POSITION0,
  out float4 o1 : VDREG1,
  out float4 o2 : VDREG2,
  out float4 o3 : VDREG3)
{
#if UNITY_UV_STARTS_AT_TOP
  uint4 r0, r1;
  r0 = 0u; r1 = 0u;
#else
  uint4 r0, r1, r2;
  r0 = 0u; r1 = 0u; r2 = 0u;
#endif

  r0.xyzw = vd_asuint((vd_asfloat(v0.yyyy) * vd_asfloat((VD_COL(unity_ObjectToWorld, 1).xyzw))));
  r0.xyzw = vd_asuint((vd_asfloat((VD_COL(unity_ObjectToWorld, 0).xyzw)) * vd_asfloat(v0.xxxx) + vd_asfloat(r0.xyzw)));
  r0.xyzw = vd_asuint((vd_asfloat((VD_COL(unity_ObjectToWorld, 2).xyzw)) * vd_asfloat(v0.zzzz) + vd_asfloat(r0.xyzw)));
  r0.xyzw = vd_asuint((vd_asfloat(r0.xyzw) + vd_asfloat((VD_COL(unity_ObjectToWorld, 3).xyzw))));
  r1.xyzw = vd_asuint((vd_asfloat(r0.yyyy) * vd_asfloat((VD_COL(unity_MatrixVP, 1).xyzw))));
  r1.xyzw = vd_asuint((vd_asfloat((VD_COL(unity_MatrixVP, 0).xyzw)) * vd_asfloat(r0.xxxx) + vd_asfloat(r1.xyzw)));
  r1.xyzw = vd_asuint((vd_asfloat((VD_COL(unity_MatrixVP, 2).xyzw)) * vd_asfloat(r0.zzzz) + vd_asfloat(r1.xyzw)));
  r1.xyzw = vd_asuint((vd_asfloat((VD_COL(unity_MatrixVP, 3).xyzw)) * vd_asfloat(r0.wwww) + vd_asfloat(r1.xyzw)));
  o2.xyz = vd_asfloat(r0.xyz);
  o0.xyzw = vd_asfloat(r1.xyzw);
  o1.xy = (vd_asfloat(v1.xy) * vd_asfloat(_MainTex_ST.xy) + vd_asfloat(_MainTex_ST.zw));
  r0.x = vd_asuint((vd_asfloat(r1.y) * vd_asfloat(_ProjectionParams.x)));
  r0.w = vd_asuint((vd_asfloat(r0.x) * 0.5));
  r0.xz = vd_asuint((vd_asfloat(r1.xw) * float2(0.5, 0.5)));
#if UNITY_UV_STARTS_AT_TOP
  o3.zw = vd_asfloat(r1.zw);
#else
  o3.w = vd_asfloat(r1.w);
  r2.x = vd_asuint((vd_asfloat(r1.w) * 0.5));
  o3.z = (vd_asfloat(r1.z) * -0.5 + vd_asfloat(r2.x));
#endif
  o3.xy = (vd_asfloat(r0.zz) + vd_asfloat(r0.xw));
  return;
}
