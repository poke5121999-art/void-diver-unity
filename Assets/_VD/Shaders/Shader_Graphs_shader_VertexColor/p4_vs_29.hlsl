// Shader Graphs/shader_VertexColor | pass 4 "Meta" | progVertex | keywords: (none) | blob 29
void vert(
  float3 v0 : POSITION0,
  float3 v1 : NORMAL0,
  float4 v2 : TANGENT0,
  float4 v3 : TEXCOORD0,
  float4 v4 : TEXCOORD1,
  float4 v5 : TEXCOORD2,
  float4 v6 : COLOR0,
  uint v7 : SV_InstanceID0,
  out float4 o0 : SV_POSITION0,
  out float4 o1 : VDREG1,
  out float4 o2 : VDREG2,
  out float4 o3 : VDREG3,
  out float4 o4 : VDREG4)
{
  uint4 r0, r1;
  r0 = 0u; r1 = 0u;

  r0.x = vd_asuint((-(int)(0.0 < vd_asfloat(v0.z))));
  r0.z = (r0.x & 0x00800000u);
  r0.xy = vd_asuint((vd_asfloat(v4.xy) * vd_asfloat(unity_LightmapST.xy) + vd_asfloat(unity_LightmapST.zw)));
  r0.xyz = ((vd_asuint(unity_MetaVertexControl.xxx) != 0u) ? r0.xyz : vd_asuint(v0.xyz));
  r0.w = vd_asuint((-(int)(0.0 < vd_asfloat(r0.z))));
  r1.z = (r0.w & 0x00800000u);
  r1.xy = vd_asuint((vd_asfloat(v5.xy) * vd_asfloat(unity_DynamicLightmapST.xy) + vd_asfloat(unity_DynamicLightmapST.zw)));
  r0.xyz = ((vd_asuint(unity_MetaVertexControl.yyy) != 0u) ? r1.xyz : r0.xyz);
  r1.xyzw = vd_asuint((vd_asfloat(r0.yyyy) * vd_asfloat((VD_COL(unity_MatrixVP, 1).xyzw))));
  r1.xyzw = vd_asuint((vd_asfloat((VD_COL(unity_MatrixVP, 0).xyzw)) * vd_asfloat(r0.xxxx) + vd_asfloat(r1.xyzw)));
  r0.xyzw = vd_asuint((vd_asfloat((VD_COL(unity_MatrixVP, 2).xyzw)) * vd_asfloat(r0.zzzz) + vd_asfloat(r1.xyzw)));
  o0.xyzw = (vd_asfloat(r0.xyzw) + vd_asfloat((VD_COL(unity_MatrixVP, 3).xyzw)));
  o1.xyzw = vd_asfloat(vd_asuint(v3.xyzw));
  o2.xyzw = vd_asfloat(vd_asuint(v4.xyzw));
  o3.xyzw = vd_asfloat(vd_asuint(v5.xyzw));
  o4.xyzw = vd_asfloat(vd_asuint(v6.xyzw));
  return;
}
