// LCArt/Deco/shader_DE_Deco_StickyDecal | pass 0 "FORWARD" | progVertex | keywords: (none) | blob 1
void vert(
  float4 v0 : POSITION0,
  float3 v1 : NORMAL0,
  float2 v2 : TEXCOORD0,
  out float4 o0 : SV_POSITION0,
  out float4 o1 : VDREG1,
  out float4 o2 : VDREG2,
  out float4 o3 : VDREG3,
  out float4 o4 : VDREG4,
  out float4 o5 : VDREG5,
  out float4 o6 : VDREG6,
  out float4 o7 : VDREG7)
{
  uint4 r0, r1, r2, r3;
  r0 = 0u; r1 = 0u; r2 = 0u; r3 = 0u;

  r0.xz = vd_asuint((vd_asfloat(v0.yy) * vd_asfloat(_DecalOffset.xz)));
  r0.y = 0x00000000u;
  r0.xyz = vd_asuint((vd_asfloat(r0.xyz) + vd_asfloat(v0.xyz)));
  r1.xyz = vd_asuint((vd_asfloat(r0.yyy) * vd_asfloat((VD_COL(unity_ObjectToWorld, 1).xyz))));
  r0.xyw = vd_asuint((vd_asfloat((VD_COL(unity_ObjectToWorld, 0).xyz)) * vd_asfloat(r0.xxx) + vd_asfloat(r1.xyz)));
  r0.xyz = vd_asuint((vd_asfloat((VD_COL(unity_ObjectToWorld, 2).xyz)) * vd_asfloat(r0.zzz) + vd_asfloat(r0.xyw)));
  r0.xyz = vd_asuint((vd_asfloat(r0.xyz) + vd_asfloat((VD_COL(unity_ObjectToWorld, 3).xyz))));
  r1.xyzw = vd_asuint((vd_asfloat(r0.yyyy) * vd_asfloat((VD_COL(unity_MatrixVP, 1).xyzw))));
  r1.xyzw = vd_asuint((vd_asfloat((VD_COL(unity_MatrixVP, 0).xyzw)) * vd_asfloat(r0.xxxx) + vd_asfloat(r1.xyzw)));
  r1.xyzw = vd_asuint((vd_asfloat((VD_COL(unity_MatrixVP, 2).xyzw)) * vd_asfloat(r0.zzzz) + vd_asfloat(r1.xyzw)));
  r1.xyzw = vd_asuint((vd_asfloat(r1.xyzw) + vd_asfloat((VD_COL(unity_MatrixVP, 3).xyzw))));
  o0.xyzw = vd_asfloat(r1.xyzw);
  o1.xy = (vd_asfloat(v2.xy) * vd_asfloat(_MainTex_ST.xy) + vd_asfloat(_MainTex_ST.zw));
  r2.x = vd_asuint(dot(vd_asfloat(v1.xyz), vd_asfloat((VD_COL(unity_WorldToObject, 0).xyz))));
  r2.y = vd_asuint(dot(vd_asfloat(v1.xyz), vd_asfloat((VD_COL(unity_WorldToObject, 1).xyz))));
  r2.z = vd_asuint(dot(vd_asfloat(v1.xyz), vd_asfloat((VD_COL(unity_WorldToObject, 2).xyz))));
  r0.w = vd_asuint(dot(vd_asfloat(r2.xyz), vd_asfloat(r2.xyz)));
  r0.w = vd_asuint(max(vd_asfloat(r0.w), 1.17549435e-38));
  r0.w = vd_asuint(rsqrt(vd_asfloat(r0.w)));
  o2.xyz = (vd_asfloat(r0.www) * vd_asfloat(r2.xyz));
  o3.xyz = vd_asfloat(r0.xyz);
  r2.xyzw = vd_asuint((vd_asfloat(v0.yyyy) * vd_asfloat((VD_COL(unity_ObjectToWorld, 1).xyzw))));
  r2.xyzw = vd_asuint((vd_asfloat((VD_COL(unity_ObjectToWorld, 0).xyzw)) * vd_asfloat(v0.xxxx) + vd_asfloat(r2.xyzw)));
  r2.xyzw = vd_asuint((vd_asfloat((VD_COL(unity_ObjectToWorld, 2).xyzw)) * vd_asfloat(v0.zzzz) + vd_asfloat(r2.xyzw)));
  r2.xyzw = vd_asuint((vd_asfloat(r2.xyzw) + vd_asfloat((VD_COL(unity_ObjectToWorld, 3).xyzw))));
  r3.xyzw = vd_asuint((vd_asfloat(r2.yyyy) * vd_asfloat((VD_COL(unity_MatrixVP, 1).xyzw))));
  r3.xyzw = vd_asuint((vd_asfloat((VD_COL(unity_MatrixVP, 0).xyzw)) * vd_asfloat(r2.xxxx) + vd_asfloat(r3.xyzw)));
  r3.xyzw = vd_asuint((vd_asfloat((VD_COL(unity_MatrixVP, 2).xyzw)) * vd_asfloat(r2.zzzz) + vd_asfloat(r3.xyzw)));
  o4.xyzw = (vd_asfloat((VD_COL(unity_MatrixVP, 3).xyzw)) * vd_asfloat(r2.wwww) + vd_asfloat(r3.xyzw));
  r0.w = vd_asuint((vd_asfloat(r1.y) * vd_asfloat(_ProjectionParams.x)));
  r2.w = vd_asuint((vd_asfloat(r0.w) * 0.5));
  r2.xz = vd_asuint((vd_asfloat(r1.xw) * float2(0.5, 0.5)));
  o5.zw = vd_asfloat(r1.zw);
  o5.xy = (vd_asfloat(r2.zz) + vd_asfloat(r2.xw));
  o6.xyz = (vd_asfloat(r0.xyz) + (-(vd_asfloat(_WorldSpaceCameraPos.xyz))));
  r0.xy = vd_asuint((vd_asfloat(r0.xz) + (-(vd_asfloat(_PlaneBoundMaxPos.xz)))));
  r0.zw = vd_asuint((vd_asfloat(_PlaneBoundMinPos.xz) + (-(vd_asfloat(_PlaneBoundMaxPos.xz)))));
  r0.xy = vd_asuint(saturate((vd_asfloat(r0.xy) / vd_asfloat(r0.zw))));
  r1.xyzw = vd_asuint(_PlaneSightRenderTarget.SampleLevel(sampler_PlaneSightRenderTarget, vd_asfloat(r0.xy), 0.0).xyzw);
  r0.xy = vd_asuint(((-(vd_asfloat(_PlaneBoundMaxPos.xz))) + vd_asfloat((VD_COL(unity_ObjectToWorld, 3).xz))));
  r0.xy = vd_asuint(saturate((vd_asfloat(r0.xy) / vd_asfloat(r0.zw))));
  r0.xyzw = vd_asuint(_PlaneSightRenderTarget.SampleLevel(sampler_PlaneSightRenderTarget, vd_asfloat(r0.xy), 0.0).xyzw);
  o7.xyzw = max(vd_asfloat(r0.xyzw), vd_asfloat(r1.xyzw));
  return;
}
