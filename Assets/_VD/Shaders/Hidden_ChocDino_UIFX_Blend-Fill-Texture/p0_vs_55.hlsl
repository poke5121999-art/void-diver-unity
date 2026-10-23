// Hidden/ChocDino/UIFX/Blend-Fill-Texture | pass 0 "Blend-Fill-Texture" | progVertex | keywords: BLEND_DARKEN UNITY_UI_ALPHACLIP WRAP_MIRROR | blob 55
void vert(
  float4 v0 : POSITION0,
  float2 v1 : TEXCOORD0,
  float4 v2 : COLOR0,
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
  r1.xyzw = vd_asuint((vd_asfloat(r0.yyyy) * vd_asfloat((VD_COL(unity_MatrixVP, 1).xyzw))));
  r1.xyzw = vd_asuint((vd_asfloat((VD_COL(unity_MatrixVP, 0).xyzw)) * vd_asfloat(r0.xxxx) + vd_asfloat(r1.xyzw)));
  r1.xyzw = vd_asuint((vd_asfloat((VD_COL(unity_MatrixVP, 2).xyzw)) * vd_asfloat(r0.zzzz) + vd_asfloat(r1.xyzw)));
  o0.xyzw = (vd_asfloat((VD_COL(unity_MatrixVP, 3).xyzw)) * vd_asfloat(r0.wwww) + vd_asfloat(r1.xyzw));
  r0.xy = vd_asuint((vd_asfloat(v1.yy) * vd_asfloat((VD_COL(_FillTex_Matrix, 1).xy))));
  r0.xy = vd_asuint((vd_asfloat((VD_COL(_FillTex_Matrix, 0).xy)) * vd_asfloat(v1.xx) + vd_asfloat(r0.xy)));
  o1.zw = (vd_asfloat(r0.xy) + vd_asfloat((VD_COL(_FillTex_Matrix, 3).xy)));
  o1.xy = (vd_asfloat(v1.xy) * vd_asfloat(_ResultTex_ST.xy) + vd_asfloat(_ResultTex_ST.zw));
  o2.xyzw = vd_asfloat(vd_asuint(v2.xyzw));
  return;
}
