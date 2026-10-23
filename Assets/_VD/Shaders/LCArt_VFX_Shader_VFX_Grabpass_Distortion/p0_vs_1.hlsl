// LCArt/VFX/Shader_VFX_Grabpass_Distortion | pass 0 "" | progVertex | keywords: (none) | blob 1
void vert(
  float4 v0 : POSITION0,
  float3 v1 : NORMAL0,
  float4 v2 : COLOR0,
  float4 v3 : TEXCOORD0,
  float4 v4 : TEXCOORD1,
  float4 v5 : TEXCOORD2,
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

  r0.xyzw = vd_asuint((vd_asfloat(v0.yyyy) * vd_asfloat((VD_COL(unity_ObjectToWorld, 1).xyzw))));
  r0.xyzw = vd_asuint((vd_asfloat((VD_COL(unity_ObjectToWorld, 0).xyzw)) * vd_asfloat(v0.xxxx) + vd_asfloat(r0.xyzw)));
  r0.xyzw = vd_asuint((vd_asfloat((VD_COL(unity_ObjectToWorld, 2).xyzw)) * vd_asfloat(v0.zzzz) + vd_asfloat(r0.xyzw)));
  r1.xyzw = vd_asuint((vd_asfloat(r0.xyzw) + vd_asfloat((VD_COL(unity_ObjectToWorld, 3).xyzw))));
  o4.xyz = (vd_asfloat((VD_COL(unity_ObjectToWorld, 3).xyz)) * vd_asfloat(v0.www) + vd_asfloat(r0.xyz));
  r0.xyzw = vd_asuint((vd_asfloat(r1.yyyy) * vd_asfloat((VD_COL(unity_MatrixVP, 1).xyzw))));
  r0.xyzw = vd_asuint((vd_asfloat((VD_COL(unity_MatrixVP, 0).xyzw)) * vd_asfloat(r1.xxxx) + vd_asfloat(r0.xyzw)));
  r0.xyzw = vd_asuint((vd_asfloat((VD_COL(unity_MatrixVP, 2).xyzw)) * vd_asfloat(r1.zzzz) + vd_asfloat(r0.xyzw)));
  r0.xyzw = vd_asuint((vd_asfloat((VD_COL(unity_MatrixVP, 3).xyzw)) * vd_asfloat(r1.wwww) + vd_asfloat(r0.xyzw)));
  o0.xyzw = vd_asfloat(r0.xyzw);
  r0.xy = vd_asuint((vd_asfloat(r0.xy) / (-(vd_asfloat(r0.ww)))));
  r0.xy = vd_asuint((vd_asfloat(r0.xy) * float2(0.5, 0.5) + float2(0.5, 0.5)));
  o3.zw = (vd_asfloat(r0.xy) * float2(-1.0, 1.0) + float2(1.0, 0.0));
  o1.xyzw = vd_asfloat(vd_asuint(v2.xyzw));
  r0.x = vd_asuint(dot(vd_asfloat(v1.xyz), vd_asfloat((VD_COL(unity_WorldToObject, 0).xyz))));
  r0.y = vd_asuint(dot(vd_asfloat(v1.xyz), vd_asfloat((VD_COL(unity_WorldToObject, 1).xyz))));
  r0.z = vd_asuint(dot(vd_asfloat(v1.xyz), vd_asfloat((VD_COL(unity_WorldToObject, 2).xyz))));
  r0.w = vd_asuint(dot(vd_asfloat(r0.xyz), vd_asfloat(r0.xyz)));
  r0.w = vd_asuint(max(vd_asfloat(r0.w), 1.17549435e-38));
  r0.w = vd_asuint(rsqrt(vd_asfloat(r0.w)));
  o2.xyz = (vd_asfloat(r0.www) * vd_asfloat(r0.xyz));
  o3.xy = (vd_asfloat(v3.xy) * vd_asfloat(_NormalMap_ST.xy) + vd_asfloat(_NormalMap_ST.zw));
  o5.xyzw = vd_asfloat(vd_asuint(v4.xyzw));
  o6.xyzw = vd_asfloat(vd_asuint(v5.xyzw));
  return;
}
