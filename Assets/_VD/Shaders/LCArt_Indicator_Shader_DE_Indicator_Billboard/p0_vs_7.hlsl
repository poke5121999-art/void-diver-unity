// LCArt/Indicator/Shader_DE_Indicator_Billboard | pass 0 "" | progVertex | keywords: _ADDITIONAL_LIGHTS _DE_EDITOR_CAMERA | blob 7
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

  o0.xy = (vd_asfloat(v1.xy) * vd_asfloat(_MainTex_ST.xy) + vd_asfloat(_MainTex_ST.zw));
  r0.x = vd_asuint((vd_asfloat(v0.x) * vd_asfloat((VD_COL(unity_MatrixV, 0).x))));
  r0.y = vd_asuint((vd_asfloat(v0.x) * vd_asfloat((VD_COL(unity_MatrixV, 1).x))));
  r0.z = vd_asuint((vd_asfloat(v0.x) * vd_asfloat((VD_COL(unity_MatrixV, 2).x))));
  r1.x = vd_asuint((vd_asfloat(v0.y) * vd_asfloat((VD_COL(unity_MatrixV, 0).y))));
  r1.y = vd_asuint((vd_asfloat(v0.y) * vd_asfloat((VD_COL(unity_MatrixV, 1).y))));
  r1.z = vd_asuint((vd_asfloat(v0.y) * vd_asfloat((VD_COL(unity_MatrixV, 2).y))));
  r0.xyz = vd_asuint((vd_asfloat(r0.xyz) + vd_asfloat(r1.xyz)));
  r0.xyz = vd_asuint((vd_asfloat(r0.xyz) * vd_asfloat((_Scale).xxx) + vd_asfloat((VD_COL(unity_ObjectToWorld, 3).xyz))));
  o1.xyz = vd_asfloat(r0.xyz);
  r1.xyzw = vd_asuint((vd_asfloat(r0.yyyy) * vd_asfloat((VD_COL(unity_MatrixVP, 1).xyzw))));
  r1.xyzw = vd_asuint((vd_asfloat((VD_COL(unity_MatrixVP, 0).xyzw)) * vd_asfloat(r0.xxxx) + vd_asfloat(r1.xyzw)));
  r0.xyzw = vd_asuint((vd_asfloat((VD_COL(unity_MatrixVP, 2).xyzw)) * vd_asfloat(r0.zzzz) + vd_asfloat(r1.xyzw)));
  o2.xyzw = (vd_asfloat(r0.xyzw) + vd_asfloat((VD_COL(unity_MatrixVP, 3).xyzw)));
  o3.xyzw = vd_asfloat(vd_asuint(v2.xyzw));
  return;
}
