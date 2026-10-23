// ArtTeam/VFX/VFX_Master_typeA | pass 2 "MotionVectors" | progVertex | keywords: _MASK_USE _PLANESYSTEMCULLINGTYPE_ALWAYS _VERTEXDISPLACEMENT_USE | blob 185
void vert(
  float3 v0 : POSITION0,
  float3 v1 : NORMAL0,
  float4 v2 : TEXCOORD0,
  float4 v3 : TEXCOORD1,
  float4 v4 : TEXCOORD2,
  float4 v5 : COLOR0,
  float3 v6 : TEXCOORD4,
  out float4 o0 : VDREG0,
  out float4 o1 : VDREG1,
  out float4 o2 : SV_POSITION0,
  out float4 o3 : VDREG3,
  out float4 o4 : VDREG4,
  out float4 o5 : VDREG5,
  out float4 o6 : VDREG6,
  out float4 o7 : VDREG7,
  out float4 o8 : VDREG8)
{
  uint4 r0, r1, r2, r3;
  r0 = 0u; r1 = 0u; r2 = 0u; r3 = 0u;

  r0.xy = vd_asuint((float2(1.0, 1.0) / vd_asfloat(_ScreenParams.xy)));
  r0.xy = vd_asuint((vd_asfloat(r0.xy) * vd_asfloat((_Pixelation).xx)));
  r0.zw = vd_asuint((vd_asfloat(v2.xy) / vd_asfloat(r0.xy)));
  r0.zw = vd_asuint(floor(vd_asfloat(r0.zw)));
  r1.x = vd_asuint((vd_asfloat(_Deform_Time_Offset_UV2y) * vd_asfloat(_TimeParameters.x) + 1.0));
  r1.x = vd_asuint((vd_asfloat(r1.x) * vd_asfloat(v4.y)));
  r1.y = vd_asuint((-(int)(abs(vd_asfloat(_Deform_Time_Offset_UV2y)) >= 0.00100000005)));
  r1.y = (r1.y & 0x3f800000u);
  r1.z = vd_asuint((vd_asfloat(_Deform_Time_Offset_UV2y) * vd_asfloat(_TimeParameters.x) + (-(vd_asfloat(r1.x)))));
  r1.x = vd_asuint((vd_asfloat(r1.y) * vd_asfloat(r1.z) + vd_asfloat(r1.x)));
  r1.xy = vd_asuint((vd_asfloat(r1.xx) * vd_asfloat(_DeformDirection.xy)));
  r0.xy = vd_asuint((vd_asfloat(r0.zw) * vd_asfloat(r0.xy) + vd_asfloat(r1.xy)));
  r0.xy = vd_asuint((vd_asfloat(r0.xy) * vd_asfloat(_Vertex_Tex_ST.xy) + vd_asfloat(_Vertex_Tex_ST.zw)));
  r0.xyzw = vd_asuint(_Vertex_Tex.SampleLevel(sampler_vd_linear_repeat, vd_asfloat(r0.xy), 0.0).xyzw);
  r0.xyz = vd_asuint((vd_asfloat(r0.xxx) * vd_asfloat(v1.xyz)));
  r0.w = vd_asuint((vd_asfloat(v4.x) + 1.0));
  r1.x = vd_asuint((vd_asfloat(r0.w) * vd_asfloat(_Vertex_Intensity_UV2x)));
  r1.y = vd_asuint((-(int)(abs(vd_asfloat(_Vertex_Intensity_UV2x)) >= 0.00100000005)));
  r1.y = (r1.y & 0x3f800000u);
  r0.w = vd_asuint(((-(vd_asfloat(_Vertex_Intensity_UV2x))) * vd_asfloat(r0.w) + vd_asfloat(_Vertex_Intensity_UV2x)));
  r0.w = vd_asuint((vd_asfloat(r1.y) * vd_asfloat(r0.w) + vd_asfloat(r1.x)));
  r1.xyz = vd_asuint((vd_asfloat(r0.xyz) * vd_asfloat(r0.www) + vd_asfloat(v0.xyz)));
  r2.xyz = vd_asuint((vd_asfloat(r1.yyy) * vd_asfloat((VD_COL(unity_ObjectToWorld, 1).xyz))));
  r2.xyz = vd_asuint((vd_asfloat((VD_COL(unity_ObjectToWorld, 0).xyz)) * vd_asfloat(r1.xxx) + vd_asfloat(r2.xyz)));
  r2.xyz = vd_asuint((vd_asfloat((VD_COL(unity_ObjectToWorld, 2).xyz)) * vd_asfloat(r1.zzz) + vd_asfloat(r2.xyz)));
  r2.xyz = vd_asuint((vd_asfloat(r2.xyz) + vd_asfloat((VD_COL(unity_ObjectToWorld, 3).xyz))));
  r3.x = vd_asuint(dot(vd_asfloat(v1.xyz), vd_asfloat((VD_COL(unity_WorldToObject, 0).xyz))));
  r3.y = vd_asuint(dot(vd_asfloat(v1.xyz), vd_asfloat((VD_COL(unity_WorldToObject, 1).xyz))));
  r3.z = vd_asuint(dot(vd_asfloat(v1.xyz), vd_asfloat((VD_COL(unity_WorldToObject, 2).xyz))));
  r1.w = vd_asuint(dot(vd_asfloat(r3.xyz), vd_asfloat(r3.xyz)));
  r1.w = vd_asuint(max(vd_asfloat(r1.w), 1.17549435e-38));
  r1.w = vd_asuint(rsqrt(vd_asfloat(r1.w)));
  o8.xyz = (vd_asfloat(r1.www) * vd_asfloat(r3.xyz));
  r3.xyzw = vd_asuint((vd_asfloat(r2.yyyy) * vd_asfloat((VD_COL(unity_MatrixVP, 1).xyzw))));
  r3.xyzw = vd_asuint((vd_asfloat((VD_COL(unity_MatrixVP, 0).xyzw)) * vd_asfloat(r2.xxxx) + vd_asfloat(r3.xyzw)));
  r3.xyzw = vd_asuint((vd_asfloat((VD_COL(unity_MatrixVP, 2).xyzw)) * vd_asfloat(r2.zzzz) + vd_asfloat(r3.xyzw)));
  o2.xyzw = (vd_asfloat(r3.xyzw) + vd_asfloat((VD_COL(unity_MatrixVP, 3).xyzw)));
  r1.w = vd_asuint((-(int)(vd_asfloat(unity_MotionVectorsParams.y) != 0.0)));
  if (r1.w != 0u) {
    r1.w = vd_asuint((-(int)(vd_asfloat(unity_MotionVectorsParams.x) == 1.0)));
    if (r1.w != 0u) {
      r1.xyz = vd_asuint((vd_asfloat(r0.xyz) * vd_asfloat(r0.www) + vd_asfloat(v6.xyz)));
    }
    r0.xyz = vd_asuint((vd_asfloat(r2.yyy) * vd_asfloat((VD_COL(_NonJitteredViewProjMatrix, 1).xyw))));
    r0.xyz = vd_asuint((vd_asfloat((VD_COL(_NonJitteredViewProjMatrix, 0).xyw)) * vd_asfloat(r2.xxx) + vd_asfloat(r0.xyz)));
    r0.xyz = vd_asuint((vd_asfloat((VD_COL(_NonJitteredViewProjMatrix, 2).xyw)) * vd_asfloat(r2.zzz) + vd_asfloat(r0.xyz)));
    o0.xyz = (vd_asfloat(r0.xyz) + vd_asfloat((VD_COL(_NonJitteredViewProjMatrix, 3).xyw)));
    r0.xyzw = vd_asuint((vd_asfloat(r1.yyyy) * vd_asfloat((VD_COL(unity_MatrixPreviousM, 1).xyzw))));
    r0.xyzw = vd_asuint((vd_asfloat((VD_COL(unity_MatrixPreviousM, 0).xyzw)) * vd_asfloat(r1.xxxx) + vd_asfloat(r0.xyzw)));
    r0.xyzw = vd_asuint((vd_asfloat((VD_COL(unity_MatrixPreviousM, 2).xyzw)) * vd_asfloat(r1.zzzz) + vd_asfloat(r0.xyzw)));
    r0.xyzw = vd_asuint((vd_asfloat(r0.xyzw) + vd_asfloat((VD_COL(unity_MatrixPreviousM, 3).xyzw))));
    r1.xyz = vd_asuint((vd_asfloat(r0.yyy) * vd_asfloat((VD_COL(_PrevViewProjMatrix, 1).xyw))));
    r1.xyz = vd_asuint((vd_asfloat((VD_COL(_PrevViewProjMatrix, 0).xyw)) * vd_asfloat(r0.xxx) + vd_asfloat(r1.xyz)));
    r0.xyz = vd_asuint((vd_asfloat((VD_COL(_PrevViewProjMatrix, 2).xyw)) * vd_asfloat(r0.zzz) + vd_asfloat(r1.xyz)));
    o1.xyz = (vd_asfloat((VD_COL(_PrevViewProjMatrix, 3).xyw)) * vd_asfloat(r0.www) + vd_asfloat(r0.xyz));
  } else {
    o0.xyz = vd_asfloat(uint3(0x00000000u, 0x00000000u, 0x00000000u));
    o1.xyz = vd_asfloat(uint3(0x00000000u, 0x00000000u, 0x00000000u));
  }
  o3.xyzw = vd_asfloat(vd_asuint(v2.xyzw));
  o4.xyzw = vd_asfloat(vd_asuint(v3.xyzw));
  o5.xyzw = vd_asfloat(vd_asuint(v4.xyzw));
  o6.xyzw = vd_asfloat(vd_asuint(v5.xyzw));
  o7.xyz = vd_asfloat(r2.xyz);
  return;
}
