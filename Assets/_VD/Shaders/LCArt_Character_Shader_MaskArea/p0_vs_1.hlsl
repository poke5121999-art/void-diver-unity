// LCArt/Character/Shader_MaskArea | pass 0 "" | progVertex | keywords: (none) | blob 1
void vert(
  float4 v0 : POSITION0,
  float2 v1 : TEXCOORD0,
  float3 v2 : NORMAL0,
  out float4 o0 : SV_POSITION0,
  out float4 o1 : VDREG1,
  out float4 o2 : VDREG2)
{
  uint4 r0, r1;
  r0 = 0u; r1 = 0u;

  r0.xyz = vd_asuint((vd_asfloat(v0.yyy) * vd_asfloat((VD_COL(unity_ObjectToWorld, 1).xyz))));
  r0.xyz = vd_asuint((vd_asfloat((VD_COL(unity_ObjectToWorld, 0).xyz)) * vd_asfloat(v0.xxx) + vd_asfloat(r0.xyz)));
  r0.xyz = vd_asuint((vd_asfloat((VD_COL(unity_ObjectToWorld, 2).xyz)) * vd_asfloat(v0.zzz) + vd_asfloat(r0.xyz)));
  r0.xyz = vd_asuint((vd_asfloat(r0.xyz) + vd_asfloat((VD_COL(unity_ObjectToWorld, 3).xyz))));
  r1.xyzw = vd_asuint((vd_asfloat(r0.yyyy) * vd_asfloat((VD_COL(unity_MatrixVP, 1).xyzw))));
  r1.xyzw = vd_asuint((vd_asfloat((VD_COL(unity_MatrixVP, 0).xyzw)) * vd_asfloat(r0.xxxx) + vd_asfloat(r1.xyzw)));
  r1.xyzw = vd_asuint((vd_asfloat((VD_COL(unity_MatrixVP, 2).xyzw)) * vd_asfloat(r0.zzzz) + vd_asfloat(r1.xyzw)));
  r0.xyz = vd_asuint(((-(vd_asfloat(r0.xyz))) + vd_asfloat(_WorldSpaceCameraPos.xyz)));
  r1.xyzw = vd_asuint((vd_asfloat(r1.xyzw) + vd_asfloat((VD_COL(unity_MatrixVP, 3).xyzw))));
  o0.xyzw = vd_asfloat(r1.xyzw);
  o1.z = (vd_asfloat(r1.z) / vd_asfloat(r1.w));
  o1.xy = (vd_asfloat(v1.xy) * vd_asfloat(_MainTex_ST.xy) + vd_asfloat(_MainTex_ST.zw));
  r0.w = vd_asuint((-(int)(vd_asfloat(unity_OrthoParams.w) == 0.0)));
  o2.x = vd_asfloat(((r0.w != 0u) ? r0.x : vd_asuint((VD_COL(unity_MatrixV, 0).z))));
  o2.y = vd_asfloat(((r0.w != 0u) ? r0.y : vd_asuint((VD_COL(unity_MatrixV, 1).z))));
  o2.z = vd_asfloat(((r0.w != 0u) ? r0.z : vd_asuint((VD_COL(unity_MatrixV, 2).z))));
  return;
}
