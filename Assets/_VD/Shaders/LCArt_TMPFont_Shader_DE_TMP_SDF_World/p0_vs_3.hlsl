// LCArt/TMPFont/Shader_DE_TMP_SDF_World | pass 0 "TextMeshPro SDF" | progVertex | keywords: UNITY_UI_CLIP_RECT | blob 3
void vert(
  float4 v0 : POSITION0,
  float3 v1 : NORMAL0,
  float4 v2 : COLOR0,
  float4 v3 : TEXCOORD0,
  float2 v4 : TEXCOORD1,
  out float4 o0 : SV_POSITION0,
  out float4 o1 : VDREG1,
  out float4 o2 : VDREG2,
  out float4 o3 : VDREG3,
  out float4 o4 : VDREG4,
  out float4 o5 : VDREG5,
  out float4 o6 : VDREG6)
{
  uint4 r0, r1, r2, r3;
  r0 = 0u; r1 = 0u; r2 = 0u; r3 = 0u;

  r0.xy = vd_asuint((vd_asfloat(v0.xy) + vd_asfloat(float2(_VertexOffsetX, _VertexOffsetY))));
  r1.xyzw = vd_asuint((vd_asfloat(r0.yyyy) * vd_asfloat((VD_COL(unity_ObjectToWorld, 1).xyzw))));
  r1.xyzw = vd_asuint((vd_asfloat((VD_COL(unity_ObjectToWorld, 0).xyzw)) * vd_asfloat(r0.xxxx) + vd_asfloat(r1.xyzw)));
  r1.xyzw = vd_asuint((vd_asfloat((VD_COL(unity_ObjectToWorld, 2).xyzw)) * vd_asfloat(v0.zzzz) + vd_asfloat(r1.xyzw)));
  r1.xyzw = vd_asuint((vd_asfloat(r1.xyzw) + vd_asfloat((VD_COL(unity_ObjectToWorld, 3).xyzw))));
  r2.xyzw = vd_asuint((vd_asfloat(r1.yyyy) * vd_asfloat((VD_COL(unity_MatrixVP, 1).xyzw))));
  r2.xyzw = vd_asuint((vd_asfloat((VD_COL(unity_MatrixVP, 0).xyzw)) * vd_asfloat(r1.xxxx) + vd_asfloat(r2.xyzw)));
  r2.xyzw = vd_asuint((vd_asfloat((VD_COL(unity_MatrixVP, 2).xyzw)) * vd_asfloat(r1.zzzz) + vd_asfloat(r2.xyzw)));
  r2.xyzw = vd_asuint((vd_asfloat((VD_COL(unity_MatrixVP, 3).xyzw)) * vd_asfloat(r1.wwww) + vd_asfloat(r2.xyzw)));
  r1.xyz = vd_asuint(((-(vd_asfloat(r1.xyz))) + vd_asfloat(_WorldSpaceCameraPos.xyz)));
  o0.xyzw = vd_asfloat(r2.xyzw);
  o1.xyzw = vd_asfloat(vd_asuint(v2.xyzw));
  o2.xy = vd_asfloat(vd_asuint(v3.xy));
  r2.x = vd_asuint(dot(vd_asfloat(v1.xyz), vd_asfloat((VD_COL(unity_WorldToObject, 0).xyz))));
  r2.y = vd_asuint(dot(vd_asfloat(v1.xyz), vd_asfloat((VD_COL(unity_WorldToObject, 1).xyz))));
  r2.z = vd_asuint(dot(vd_asfloat(v1.xyz), vd_asfloat((VD_COL(unity_WorldToObject, 2).xyz))));
  r0.z = vd_asuint(dot(vd_asfloat(r2.xyz), vd_asfloat(r2.xyz)));
  r0.z = vd_asuint(max(vd_asfloat(r0.z), 1.17549435e-38));
  r0.z = vd_asuint(rsqrt(vd_asfloat(r0.z)));
  r2.xyz = vd_asuint((vd_asfloat(r0.zzz) * vd_asfloat(r2.xyz)));
  r0.z = vd_asuint(dot(vd_asfloat(r1.xyz), vd_asfloat(r1.xyz)));
  r0.z = vd_asuint(rsqrt(vd_asfloat(r0.z)));
  r3.xyz = vd_asuint((vd_asfloat(r0.zzz) * vd_asfloat(r1.xyz)));
  r0.z = vd_asuint(dot(vd_asfloat(r2.xyz), vd_asfloat(r3.xyz)));
  r3.z = vd_asuint((vd_asfloat(_ScreenParams.x) * vd_asfloat((VD_COL(glstate_matrix_projection, 0).x))));
  r3.w = vd_asuint((vd_asfloat(_ScreenParams.y) * vd_asfloat((VD_COL(glstate_matrix_projection, 1).y))));
  r2.xy = vd_asuint((abs(vd_asfloat(r3.zw)) * vd_asfloat(float2(_ScaleX, _ScaleY))));
  r2.xy = vd_asuint((vd_asfloat(r2.ww) / vd_asfloat(r2.xy)));
  r0.w = vd_asuint(dot(vd_asfloat(r2.xy), vd_asfloat(r2.xy)));
  r2.xy = vd_asuint((vd_asfloat(float2(_MaskSoftnessX, _MaskSoftnessY)) * float2(0.25, 0.25) + vd_asfloat(r2.xy)));
  o4.zw = (float2(0.25, 0.25) / vd_asfloat(r2.xy));
  r0.w = vd_asuint(rsqrt(vd_asfloat(r0.w)));
  r1.w = vd_asuint((abs(vd_asfloat(v3.w)) * vd_asfloat(_GradientScale)));
  r2.x = vd_asuint((vd_asfloat(_Sharpness) + 1.0));
  r1.w = vd_asuint((vd_asfloat(r1.w) * vd_asfloat(r2.x)));
  r2.x = vd_asuint((vd_asfloat(r0.w) * vd_asfloat(r1.w)));
  r2.y = vd_asuint(((-(vd_asfloat(_PerspectiveFilter))) + 1.0));
  r2.y = vd_asuint((vd_asfloat(r2.y) * abs(vd_asfloat(r2.x))));
  r0.w = vd_asuint((vd_asfloat(r0.w) * vd_asfloat(r1.w) + (-(vd_asfloat(r2.y)))));
  r0.z = vd_asuint((abs(vd_asfloat(r0.z)) * vd_asfloat(r0.w) + vd_asfloat(r2.y)));
  r0.w = vd_asuint((-(int)(vd_asfloat((VD_COL(glstate_matrix_projection, 3).w)) < 0.00999999978)));
  r2.y = ((r0.w != 0u) ? r0.z : r2.x);
  r0.z = vd_asuint((-(int)(0.0 >= vd_asfloat(v3.w))));
  r0.z = (r0.z & 0x3f800000u);
  r0.w = vd_asuint(((-(vd_asfloat(_WeightNormal))) + vd_asfloat(_WeightBold)));
  r0.z = vd_asuint((vd_asfloat(r0.z) * vd_asfloat(r0.w) + vd_asfloat(_WeightNormal)));
  r0.z = vd_asuint((vd_asfloat(r0.z) * 0.25 + vd_asfloat(_FaceDilate)));
  r0.z = vd_asuint((vd_asfloat(r0.z) * vd_asfloat(_ScaleRatioA)));
  r2.w = vd_asuint((vd_asfloat(r0.z) * 0.5));
  o3.yw = vd_asfloat(r2.yw);
  r0.w = vd_asuint((0.5 / vd_asfloat(r2.y)));
  r1.w = vd_asuint(((-(vd_asfloat(_OutlineWidth))) * vd_asfloat(_ScaleRatioA) + 1.0));
  r1.w = vd_asuint(((-(vd_asfloat(_OutlineSoftness))) * vd_asfloat(_ScaleRatioA) + vd_asfloat(r1.w)));
  r1.w = vd_asuint((vd_asfloat(r1.w) * 0.5 + (-(vd_asfloat(r0.w)))));
  o3.x = ((-(vd_asfloat(r0.z))) * 0.5 + vd_asfloat(r1.w));
  r0.z = vd_asuint(((-(vd_asfloat(r0.z))) * 0.5 + 0.5));
  o3.z = (vd_asfloat(r0.w) + vd_asfloat(r0.z));
  r2.xyzw = vd_asuint(max(vd_asfloat(_ClipRect.xyzw), float4(-2e+10, -2e+10, -2e+10, -2e+10)));
  r2.xyzw = vd_asuint(min(vd_asfloat(r2.xyzw), float4(2e+10, 2e+10, 2e+10, 2e+10)));
  r0.xy = vd_asuint((vd_asfloat(r0.xy) * float2(2.0, 2.0) + (-(vd_asfloat(r2.xy)))));
  o4.xy = ((-(vd_asfloat(r2.zw))) + vd_asfloat(r0.xy));
  r0.xyz = vd_asuint((vd_asfloat(r1.yyy) * vd_asfloat((VD_COL(_EnvMatrix, 1).xyz))));
  r0.xyz = vd_asuint((vd_asfloat((VD_COL(_EnvMatrix, 0).xyz)) * vd_asfloat(r1.xxx) + vd_asfloat(r0.xyz)));
  o5.xyz = (vd_asfloat((VD_COL(_EnvMatrix, 2).xyz)) * vd_asfloat(r1.zzz) + vd_asfloat(r0.xyz));
  o6.xy = (vd_asfloat(v4.xy) * vd_asfloat(_FaceTex_ST.xy) + vd_asfloat(_FaceTex_ST.zw));
  o6.zw = (vd_asfloat(v4.xy) * vd_asfloat(_OutlineTex_ST.xy) + vd_asfloat(_OutlineTex_ST.zw));
  return;
}
