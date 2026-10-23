// ArtTeam/UI/Shader_DE_UI_VFXMaster | pass 0 "Default" | progVertex | keywords: _USE_DEFORM_ON | blob 2
void vert(
  float4 v0 : POSITION0,
  float4 v1 : COLOR0,
  float2 v2 : TEXCOORD0,
  out float4 o0 : SV_POSITION0,
  out float4 o1 : VDREG1,
  out float4 o2 : VDREG2,
  out float4 o3 : VDREG3,
  out float4 o4 : VDREG4,
  out float4 o5 : VDREG5)
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
  o1.xyzw = (vd_asfloat(v1.xyzw) * vd_asfloat(_Color.xyzw));
  o2.xy = vd_asfloat(vd_asuint(v2.xy));
  o3.xyzw = vd_asfloat(vd_asuint(v0.xyzw));
  r1.z = vd_asuint((vd_asfloat(_ScreenParams.x) * vd_asfloat((VD_COL(glstate_matrix_projection, 0).x))));
  r1.w = vd_asuint((vd_asfloat(_ScreenParams.y) * vd_asfloat((VD_COL(glstate_matrix_projection, 1).y))));
  r0.xy = vd_asuint((vd_asfloat(r0.ww) / abs(vd_asfloat(r1.zw))));
  r0.xy = vd_asuint((vd_asfloat(float2(_UIMaskSoftnessX, _UIMaskSoftnessY)) * float2(0.25, 0.25) + abs(vd_asfloat(r0.xy))));
  o4.zw = (float2(0.25, 0.25) / vd_asfloat(r0.xy));
  r0.xyzw = vd_asuint(max(vd_asfloat(_ClipRect.xyzw), float4(-2e+10, -2e+10, -2e+10, -2e+10)));
  r0.xyzw = vd_asuint(min(vd_asfloat(r0.xyzw), float4(2e+10, 2e+10, 2e+10, 2e+10)));
  r0.xy = vd_asuint((vd_asfloat(v0.xy) * float2(2.0, 2.0) + (-(vd_asfloat(r0.xy)))));
  o4.xy = ((-(vd_asfloat(r0.zw))) + vd_asfloat(r0.xy));
  o5.xyzw = vd_asfloat(vd_asuint(v0.xyzw));
  return;
}
