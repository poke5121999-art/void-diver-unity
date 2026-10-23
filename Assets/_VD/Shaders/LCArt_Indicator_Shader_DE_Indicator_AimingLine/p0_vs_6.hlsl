// LCArt/Indicator/Shader_DE_Indicator_AimingLine | pass 0 "" | progVertex | keywords: _ADDITIONAL_LIGHTS | blob 6
void vert(
  float4 v0 : POSITION0,
  float2 v1 : TEXCOORD0,
  float4 v2 : COLOR0,
  out float4 o0 : VDREG0,
  out float4 o1 : VDREG1,
  out float4 o2 : SV_POSITION0,
  out float4 o3 : VDREG3)
{
  uint4 r0, r1;
  r0 = 0u; r1 = 0u;

  o0.xy = (vd_asfloat(v1.xy) * vd_asfloat(_WeightTex_ST.xy) + vd_asfloat(_WeightTex_ST.zw));
  r0.x = vd_asuint((vd_asfloat(v0.y) + vd_asfloat(_Height)));
  r0.xyzw = vd_asuint((vd_asfloat(r0.xxxx) * vd_asfloat((VD_COL(unity_ObjectToWorld, 1).xyzw))));
  r1.xy = vd_asuint(v0.xz);
  r0.xyzw = vd_asuint((vd_asfloat((VD_COL(unity_ObjectToWorld, 0).xyzw)) * vd_asfloat(r1.xxxx) + vd_asfloat(r0.xyzw)));
  r0.xyzw = vd_asuint((vd_asfloat((VD_COL(unity_ObjectToWorld, 2).xyzw)) * vd_asfloat(r1.yyyy) + vd_asfloat(r0.xyzw)));
  r0.xyzw = vd_asuint((vd_asfloat(r0.xyzw) + vd_asfloat((VD_COL(unity_ObjectToWorld, 3).xyzw))));
  o1.xyz = vd_asfloat(r0.xyz);
  r1.xyzw = vd_asuint((vd_asfloat(r0.yyyy) * vd_asfloat((VD_COL(unity_MatrixVP, 1).xyzw))));
  r1.xyzw = vd_asuint((vd_asfloat((VD_COL(unity_MatrixVP, 0).xyzw)) * vd_asfloat(r0.xxxx) + vd_asfloat(r1.xyzw)));
  r1.xyzw = vd_asuint((vd_asfloat((VD_COL(unity_MatrixVP, 2).xyzw)) * vd_asfloat(r0.zzzz) + vd_asfloat(r1.xyzw)));
  o2.xyzw = (vd_asfloat((VD_COL(unity_MatrixVP, 3).xyzw)) * vd_asfloat(r0.wwww) + vd_asfloat(r1.xyzw));
  o3.xyzw = vd_asfloat(vd_asuint(v2.xyzw));
  return;
}
