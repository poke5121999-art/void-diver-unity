// LCArt/Character/shader_DE_Character_Normal | pass 4 "StencilColor" | progVertex | keywords: (none) | blob 107
void vert(
  float4 v0 : POSITION0,
  float2 v1 : TEXCOORD0,
  float4 v2 : COLOR0,
  out float4 o0 : SV_POSITION0,
  out float4 o1 : VDREG1,
  out float4 o2 : VDREG2,
  out float4 o3 : VDREG3,
  out float4 o4 : VDREG4,
  out float4 o5 : VDREG5)
{
#if UNITY_UV_STARTS_AT_TOP
  uint4 r0, r1, r2, r3;
  r0 = 0u; r1 = 0u; r2 = 0u; r3 = 0u;
#else
  uint4 r0, r1, r2, r3, r4;
  r0 = 0u; r1 = 0u; r2 = 0u; r3 = 0u; r4 = 0u;
#endif

  r0.x = vd_asuint((-(int)(vd_asfloat(v0.y) >= vd_asfloat(_OpticalRatioYThreshold))));
  r0.x = (r0.x & 0x3f800000u);
  r0.y = vd_asuint((vd_asfloat(_OpticalRatioYPositiveRatio) + -1.0));
  r0.x = vd_asuint((vd_asfloat(r0.x) * vd_asfloat(r0.y) + 1.0));
  r0.y = vd_asuint((-(int)(vd_asfloat(v0.y) >= 0.0)));
  r0.y = (r0.y & 0x3f800000u);
  r0.z = vd_asuint((1.0 / vd_asfloat((VD_COL(unity_MatrixV, 1).y))));
  r1.y = vd_asuint(min(abs(vd_asfloat(r0.z)), 1.5));
  r1.xz = uint2(0x00000000u, 0x00000000u);
  r2.x = vd_asuint((VD_COL(unity_MatrixV, 0).y));
  r2.y = vd_asuint((VD_COL(unity_MatrixV, 1).y));
  r2.z = vd_asuint((VD_COL(unity_MatrixV, 2).y));
  r1.xyz = vd_asuint((vd_asfloat(r1.xyz) + (-(vd_asfloat(r2.xyz)))));
  r0.yzw = vd_asuint((vd_asfloat(r0.yyy) * vd_asfloat(r1.xyz) + vd_asfloat(r2.xyz)));
  r1.xyz = vd_asuint(((-(vd_asfloat(_WorldSpaceCameraPos.xyz))) + vd_asfloat((VD_COL(unity_ObjectToWorld, 3).xyz))));
  r1.w = vd_asuint(dot(vd_asfloat(r1.xyz), vd_asfloat(r1.xyz)));
  r1.w = vd_asuint(rsqrt(vd_asfloat(r1.w)));
  r1.xyz = vd_asuint((vd_asfloat(r1.www) * vd_asfloat(r1.xyz)));
  r1.w = vd_asuint((-(vd_asfloat(r1.x))));
  r2.xy = vd_asuint((vd_asfloat(r1.zw) * vd_asfloat(v0.xx)));
  r2.xz = vd_asuint((vd_asfloat(r2.xy) * float2(1.17499995, 1.17499995)));
  r2.y = 0x00000000u;
  r0.yzw = vd_asuint((vd_asfloat(r0.yzw) * vd_asfloat(v0.yyy) + vd_asfloat(r2.xyz)));
  r2.xyz = vd_asuint((vd_asfloat(v0.yyy) * float3(0.0, 1.0, 0.0) + vd_asfloat(r2.xyz)));
  r2.xyz = vd_asuint((vd_asfloat(r1.xyz) * float3(0.200000003, 0.200000003, 0.200000003) + vd_asfloat(r2.xyz)));
  r3.xyz = vd_asuint((vd_asfloat(r1.xyz) * vd_asfloat(v0.zzz)));
  r0.xyz = vd_asuint((vd_asfloat(r3.xyz) * vd_asfloat(r0.xxx) + vd_asfloat(r0.yzw)));
  r0.w = vd_asuint(max((-(vd_asfloat(r0.y))), 0.0));
  r0.w = vd_asuint((vd_asfloat(r0.w) * vd_asfloat(_OpticalRatioYNegativeUnit)));
  r0.xyz = vd_asuint(((-(vd_asfloat(r1.xyz))) * vd_asfloat(r0.www) + vd_asfloat(r0.xyz)));
  r1.xyz = vd_asuint((vd_asfloat(r0.yyy) * vd_asfloat((VD_COL(unity_ObjectToWorld, 1).xyz))));
  r0.xyw = vd_asuint((vd_asfloat((VD_COL(unity_ObjectToWorld, 0).xyz)) * vd_asfloat(r0.xxx) + vd_asfloat(r1.xyz)));
  r0.xyz = vd_asuint((vd_asfloat((VD_COL(unity_ObjectToWorld, 2).xyz)) * vd_asfloat(r0.zzz) + vd_asfloat(r0.xyw)));
  r0.xyz = vd_asuint((vd_asfloat(r0.xyz) + vd_asfloat((VD_COL(unity_ObjectToWorld, 3).xyz))));
  r1.xyzw = vd_asuint((vd_asfloat(r0.yyyy) * vd_asfloat((VD_COL(unity_MatrixVP, 1).xyzw))));
  r1.xyzw = vd_asuint((vd_asfloat((VD_COL(unity_MatrixVP, 0).xyzw)) * vd_asfloat(r0.xxxx) + vd_asfloat(r1.xyzw)));
  r1.xyzw = vd_asuint((vd_asfloat((VD_COL(unity_MatrixVP, 2).xyzw)) * vd_asfloat(r0.zzzz) + vd_asfloat(r1.xyzw)));
  o2.xyz = vd_asfloat(r0.xyz);
  r0.xyzw = vd_asuint((vd_asfloat(r1.xyzw) + vd_asfloat((VD_COL(unity_MatrixVP, 3).xyzw))));
  o0.xyzw = vd_asfloat(r0.xyzw);
  o1.xy = (vd_asfloat(v1.xy) * vd_asfloat(_MainTex_ST.xy) + vd_asfloat(_MainTex_ST.zw));
  o1.zw = (vd_asfloat(v1.xy) * vd_asfloat(_NormalTex_ST.xy) + vd_asfloat(_NormalTex_ST.zw));
  r1.xy = vd_asuint(((-(vd_asfloat(_PlaneBoundMaxPos.xz))) + vd_asfloat((VD_COL(unity_ObjectToWorld, 3).xz))));
  r1.zw = vd_asuint((vd_asfloat(_PlaneBoundMinPos.xz) + (-(vd_asfloat(_PlaneBoundMaxPos.xz)))));
  r1.xy = vd_asuint(saturate((vd_asfloat(r1.xy) / vd_asfloat(r1.zw))));
  r3.xyzw = vd_asuint(_PlanePointBuffer.SampleLevel(sampler_PlanePointBuffer, vd_asfloat(r1.xy), 0.0).xyzw);
  r1.xyzw = vd_asuint(_PlaneSightRenderTarget.SampleLevel(sampler_PlaneSightRenderTarget, vd_asfloat(r1.xy), 0.0).xyzw);
  r1.x = vd_asuint(saturate((vd_asfloat(r3.x) * 4.0)));
  r1.y = vd_asuint((vd_asfloat(r1.x) * -2.0 + 3.0));
  r1.x = vd_asuint((vd_asfloat(r1.x) * vd_asfloat(r1.x)));
  r1.x = vd_asuint((vd_asfloat(r1.x) * vd_asfloat(r1.y)));
  r1.x = vd_asuint((vd_asfloat(r1.x) * vd_asfloat(_GlobalPlanePointBufferEnable)));
  r1.x = vd_asuint(max(vd_asfloat(r1.x), vd_asfloat(r1.w)));
  r1.x = vd_asuint((vd_asfloat(r1.x) + -1.0));
  r1.y = vd_asuint((vd_asfloat(_GlobalIsRuntime) * vd_asfloat(_GlobalPlaneSystemEnable)));
  o2.w = (vd_asfloat(r1.y) * vd_asfloat(r1.x) + 1.0);
  r1.xyz = vd_asuint((vd_asfloat(r2.yyy) * vd_asfloat((VD_COL(unity_ObjectToWorld, 1).xyz))));
  r1.xyz = vd_asuint((vd_asfloat((VD_COL(unity_ObjectToWorld, 0).xyz)) * vd_asfloat(r2.xxx) + vd_asfloat(r1.xyz)));
  r1.xyz = vd_asuint((vd_asfloat((VD_COL(unity_ObjectToWorld, 2).xyz)) * vd_asfloat(r2.zzz) + vd_asfloat(r1.xyz)));
  r1.xyz = vd_asuint((vd_asfloat(r1.xyz) + vd_asfloat((VD_COL(unity_ObjectToWorld, 3).xyz))));
  r1.w = vd_asuint((-(int)(vd_asfloat(_GhostEnable) >= 0.00100000005)));
  r1.w = ((r1.w != 0u) ? 0x00000000u : 0x3f800000u);
  o3.xyz = (vd_asfloat(r1.xyz) * vd_asfloat(r1.www));
  r0.y = vd_asuint((vd_asfloat(r0.y) * vd_asfloat(_ProjectionParams.x)));
  r1.xzw = vd_asuint((vd_asfloat(r0.xwy) * float3(0.5, 0.5, 0.5)));
#if UNITY_UV_STARTS_AT_TOP
  o4.zw = vd_asfloat(r0.zw);
#else
  o4.w = vd_asfloat(r0.w);
  r4.x = vd_asuint((vd_asfloat(r0.w) * 0.5));
  o4.z = (vd_asfloat(r0.z) * -0.5 + vd_asfloat(r4.x));
#endif
  o4.xy = (vd_asfloat(r1.zz) + vd_asfloat(r1.xw));
  r0.x = vd_asuint((-(int)(0.00100000005 < vd_asfloat(v2.w))));
  r0.yzw = vd_asuint((vd_asfloat(v2.xyz) / vd_asfloat(v2.www)));
  r0.xyz = ((r0.xxx != 0u) ? r0.yzw : vd_asuint(v2.xyz));
  r1.xyz = vd_asuint((vd_asfloat(r0.xyz) + float3(0.0549999997, 0.0549999997, 0.0549999997)));
  r1.xyz = vd_asuint((vd_asfloat(r1.xyz) * float3(0.947867334, 0.947867334, 0.947867334)));
  r1.xyz = vd_asuint(log2(abs(vd_asfloat(r1.xyz))));
  r1.xyz = vd_asuint((vd_asfloat(r1.xyz) * float3(2.4000001, 2.4000001, 2.4000001)));
  r1.xyz = vd_asuint(exp2(vd_asfloat(r1.xyz)));
  r2.xyz = vd_asuint((vd_asfloat(r0.xyz) * float3(0.0773993805, 0.0773993805, 0.0773993805)));
  r0.xyz = vd_asuint((-(int3)(float3(0.0404499993, 0.0404499993, 0.0404499993) >= vd_asfloat(r0.xyz))));
  o5.xyz = vd_asfloat(((r0.xyz != 0u) ? r2.xyz : r1.xyz));
  o5.w = vd_asfloat(vd_asuint(v2.w));
  return;
}
