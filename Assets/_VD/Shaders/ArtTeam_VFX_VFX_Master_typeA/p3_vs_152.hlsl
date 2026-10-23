// ArtTeam/VFX/VFX_Master_typeA | pass 3 "DepthNormalsOnly" | progVertex | keywords: _DEFORM_USE _MASK_USE _PLANESYSTEMCULLINGTYPE_ALWAYS _VERTEXDISPLACEMENT_USE | blob 152
void vert(
  float3 v0 : POSITION0,
  float3 v1 : NORMAL0,
  float4 v2 : TANGENT0,
  float4 v3 : TEXCOORD0,
  float4 v4 : TEXCOORD1,
  float4 v5 : TEXCOORD2,
  float4 v6 : COLOR0,
  out float4 o0 : SV_POSITION0,
  out float4 o1 : VDREG1,
  out float4 o2 : VDREG2,
  out float4 o3 : VDREG3,
  out float4 o4 : VDREG4,
  out float4 o5 : VDREG5,
  out float4 o6 : VDREG6)
{
  uint4 r0, r1;
  r0 = 0u; r1 = 0u;

  r0.x = vd_asuint((-(int)(abs(vd_asfloat(_Deform_Time_Offset_UV2y)) >= 0.00100000005)));
  r0.x = (r0.x & 0x3f800000u);
  r0.y = vd_asuint((vd_asfloat(_Deform_Time_Offset_UV2y) * vd_asfloat(_TimeParameters.x) + 1.0));
  r0.y = vd_asuint((vd_asfloat(r0.y) * vd_asfloat(v5.y)));
  r0.z = vd_asuint((vd_asfloat(_Deform_Time_Offset_UV2y) * vd_asfloat(_TimeParameters.x) + (-(vd_asfloat(r0.y)))));
  r0.x = vd_asuint((vd_asfloat(r0.x) * vd_asfloat(r0.z) + vd_asfloat(r0.y)));
  r0.xy = vd_asuint((vd_asfloat(r0.xx) * vd_asfloat(_DeformDirection.xy)));
  r0.zw = vd_asuint((float2(1.0, 1.0) / vd_asfloat(_ScreenParams.xy)));
  r0.zw = vd_asuint((vd_asfloat(r0.zw) * vd_asfloat((_Pixelation).xx)));
  r1.xy = vd_asuint((vd_asfloat(v3.xy) / vd_asfloat(r0.zw)));
  r1.xy = vd_asuint(floor(vd_asfloat(r1.xy)));
  r0.xy = vd_asuint((vd_asfloat(r1.xy) * vd_asfloat(r0.zw) + vd_asfloat(r0.xy)));
  r0.xy = vd_asuint((vd_asfloat(r0.xy) * vd_asfloat(_Vertex_Tex_ST.xy) + vd_asfloat(_Vertex_Tex_ST.zw)));
  r0.xyzw = vd_asuint(_Vertex_Tex.SampleLevel(sampler_vd_linear_repeat, vd_asfloat(r0.xy), 0.0).xyzw);
  r0.xyz = vd_asuint((vd_asfloat(r0.xxx) * vd_asfloat(v1.xyz)));
  r0.w = vd_asuint((-(int)(abs(vd_asfloat(_Vertex_Intensity_UV2x)) >= 0.00100000005)));
  r0.w = (r0.w & 0x3f800000u);
  r1.x = vd_asuint((vd_asfloat(v5.x) + 1.0));
  r1.y = vd_asuint((vd_asfloat(r1.x) * vd_asfloat(_Vertex_Intensity_UV2x)));
  r1.x = vd_asuint(((-(vd_asfloat(_Vertex_Intensity_UV2x))) * vd_asfloat(r1.x) + vd_asfloat(_Vertex_Intensity_UV2x)));
  r0.w = vd_asuint((vd_asfloat(r0.w) * vd_asfloat(r1.x) + vd_asfloat(r1.y)));
  r0.xyz = vd_asuint((vd_asfloat(r0.xyz) * vd_asfloat(r0.www) + vd_asfloat(v0.xyz)));
  r1.xyz = vd_asuint((vd_asfloat(r0.yyy) * vd_asfloat((VD_COL(unity_ObjectToWorld, 1).xyz))));
  r0.xyw = vd_asuint((vd_asfloat((VD_COL(unity_ObjectToWorld, 0).xyz)) * vd_asfloat(r0.xxx) + vd_asfloat(r1.xyz)));
  r0.xyz = vd_asuint((vd_asfloat((VD_COL(unity_ObjectToWorld, 2).xyz)) * vd_asfloat(r0.zzz) + vd_asfloat(r0.xyw)));
  r0.xyz = vd_asuint((vd_asfloat(r0.xyz) + vd_asfloat((VD_COL(unity_ObjectToWorld, 3).xyz))));
  r1.xyzw = vd_asuint((vd_asfloat(r0.yyyy) * vd_asfloat((VD_COL(unity_MatrixVP, 1).xyzw))));
  r1.xyzw = vd_asuint((vd_asfloat((VD_COL(unity_MatrixVP, 0).xyzw)) * vd_asfloat(r0.xxxx) + vd_asfloat(r1.xyzw)));
  r1.xyzw = vd_asuint((vd_asfloat((VD_COL(unity_MatrixVP, 2).xyzw)) * vd_asfloat(r0.zzzz) + vd_asfloat(r1.xyzw)));
  o5.xyz = vd_asfloat(r0.xyz);
  o0.xyzw = (vd_asfloat(r1.xyzw) + vd_asfloat((VD_COL(unity_MatrixVP, 3).xyzw)));
  o1.xyzw = vd_asfloat(vd_asuint(v3.xyzw));
  o2.xyzw = vd_asfloat(vd_asuint(v4.xyzw));
  o3.xyzw = vd_asfloat(vd_asuint(v5.xyzw));
  o4.xyzw = vd_asfloat(vd_asuint(v6.xyzw));
  r0.x = vd_asuint(dot(vd_asfloat(v1.xyz), vd_asfloat((VD_COL(unity_WorldToObject, 0).xyz))));
  r0.y = vd_asuint(dot(vd_asfloat(v1.xyz), vd_asfloat((VD_COL(unity_WorldToObject, 1).xyz))));
  r0.z = vd_asuint(dot(vd_asfloat(v1.xyz), vd_asfloat((VD_COL(unity_WorldToObject, 2).xyz))));
  r0.w = vd_asuint(dot(vd_asfloat(r0.xyz), vd_asfloat(r0.xyz)));
  r0.w = vd_asuint(max(vd_asfloat(r0.w), 1.17549435e-38));
  r0.w = vd_asuint(rsqrt(vd_asfloat(r0.w)));
  o6.xyz = (vd_asfloat(r0.www) * vd_asfloat(r0.xyz));
  return;
}
