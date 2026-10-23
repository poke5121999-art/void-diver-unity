// Hidden/ChocDino/UIFX/Blend-Frame | pass 0 "Blend-Frame" | progVertex | keywords: CUTOUT GRADIENT_RADIAL_BORDER UNITY_UI_CLIP_RECT | blob 427
void vert(
  float4 v0 : POSITION0,
  float2 v1 : TEXCOORD0,
  float4 v2 : COLOR0,
  out float4 o0 : SV_POSITION0,
  out float4 o1 : VDREG1,
  out float4 o2 : VDREG2,
  out float4 o3 : VDREG3)
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
  r0.xyzw = vd_asuint((vd_asfloat((VD_COL(unity_MatrixVP, 3).xyzw)) * vd_asfloat(r0.wwww) + vd_asfloat(r1.xyzw)));
  o0.xyzw = vd_asfloat(r0.xyzw);
  o1.xy = (vd_asfloat(v1.xy) * vd_asfloat(_ResultTex_ST.xy) + vd_asfloat(_ResultTex_ST.zw));
  o1.zw = (vd_asfloat(v1.xy) * vd_asfloat(_Rect_ST.xy) + vd_asfloat(_Rect_ST.zw));
  o2.xyzw = vd_asfloat(vd_asuint(v2.xyzw));
  r0.xy = vd_asuint((vd_asfloat(_ScreenParams.yy) * vd_asfloat((VD_COL(glstate_matrix_projection, 1).xy))));
  r0.xy = vd_asuint((vd_asfloat((VD_COL(glstate_matrix_projection, 0).xy)) * vd_asfloat(_ScreenParams.xx) + vd_asfloat(r0.xy)));
  r0.xy = vd_asuint((vd_asfloat(r0.ww) / abs(vd_asfloat(r0.xy))));
  r0.xy = vd_asuint((vd_asfloat(float2(_UIMaskSoftnessX, _UIMaskSoftnessY)) * float2(0.25, 0.25) + abs(vd_asfloat(r0.xy))));
  o3.zw = (float2(0.25, 0.25) / vd_asfloat(r0.xy));
  r0.xyzw = vd_asuint(max(vd_asfloat(_ClipRect.xyzw), float4(-2e+10, -2e+10, -2e+10, -2e+10)));
  r0.xyzw = vd_asuint(min(vd_asfloat(r0.xyzw), float4(2e+10, 2e+10, 2e+10, 2e+10)));
  r0.xy = vd_asuint((vd_asfloat(v0.xy) * float2(2.0, 2.0) + (-(vd_asfloat(r0.xy)))));
  o3.xy = ((-(vd_asfloat(r0.zw))) + vd_asfloat(r0.xy));
  return;
}
