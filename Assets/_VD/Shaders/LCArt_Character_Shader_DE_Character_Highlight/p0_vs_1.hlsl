// LCArt/Character/Shader_DE_Character_Highlight | pass 0 "Glowline" | progVertex | keywords: (none) | blob 1
void vert(
  float4 v0 : POSITION0,
  float3 v1 : NORMAL0,
  float2 v2 : TEXCOORD0,
  float4 v3 : COLOR0,
  out float4 o0 : SV_POSITION0,
  out float4 o1 : VDREG1,
  out float4 o2 : VDREG2)
{
  uint4 r0, r1, r2;
  r0 = 0u; r1 = 0u; r2 = 0u;

  r0.x = vd_asuint((-(int)(vd_asfloat(v0.y) >= 0.0)));
  r0.x = (r0.x & 0x3f800000u);
  r0.y = vd_asuint((1.0 / vd_asfloat((VD_COL(unity_MatrixV, 1).y))));
  r1.y = vd_asuint(min(abs(vd_asfloat(r0.y)), 1.5));
  r1.xz = uint2(0x00000000u, 0x00000000u);
  r2.x = vd_asuint((VD_COL(unity_MatrixV, 0).y));
  r2.y = vd_asuint((VD_COL(unity_MatrixV, 1).y));
  r2.z = vd_asuint((VD_COL(unity_MatrixV, 2).y));
  r0.yzw = vd_asuint((vd_asfloat(r1.xyz) + (-(vd_asfloat(r2.xyz)))));
  r0.xyz = vd_asuint((vd_asfloat(r0.xxx) * vd_asfloat(r0.yzw) + vd_asfloat(r2.xyz)));
  r1.xyz = vd_asuint(((-(vd_asfloat(_WorldSpaceCameraPos.xyz))) + vd_asfloat((VD_COL(unity_ObjectToWorld, 3).xyz))));
  r0.w = vd_asuint(dot(vd_asfloat(r1.xyz), vd_asfloat(r1.xyz)));
  r0.w = vd_asuint(rsqrt(vd_asfloat(r0.w)));
  r1.xyz = vd_asuint((vd_asfloat(r0.www) * vd_asfloat(r1.xyz)));
  r1.w = vd_asuint((-(vd_asfloat(r1.x))));
  r2.xy = vd_asuint((vd_asfloat(r1.zw) * vd_asfloat(v0.xx)));
  r2.xz = vd_asuint((vd_asfloat(r2.xy) * float2(1.17499995, 1.17499995)));
  r2.y = 0x00000000u;
  r0.xyz = vd_asuint((vd_asfloat(r0.xyz) * vd_asfloat(v0.yyy) + vd_asfloat(r2.xyz)));
  r0.w = vd_asuint((-(int)(vd_asfloat(v0.y) >= vd_asfloat(_OpticalRatioYThreshold))));
  r0.w = (r0.w & 0x3f800000u);
  r1.w = vd_asuint((vd_asfloat(_OpticalRatioYPositiveRatio) + -1.0));
  r0.w = vd_asuint((vd_asfloat(r0.w) * vd_asfloat(r1.w) + 1.0));
  r2.xyz = vd_asuint((vd_asfloat(r1.xyz) * vd_asfloat(v0.zzz)));
  r0.xyz = vd_asuint((vd_asfloat(r2.xyz) * vd_asfloat(r0.www) + vd_asfloat(r0.xyz)));
  r0.w = vd_asuint(max((-(vd_asfloat(r0.y))), 0.0));
  r0.w = vd_asuint((vd_asfloat(r0.w) * vd_asfloat(_OpticalRatioYNegativeUnit)));
  r0.xyz = vd_asuint(((-(vd_asfloat(r1.xyz))) * vd_asfloat(r0.www) + vd_asfloat(r0.xyz)));
  r1.xyz = vd_asuint((vd_asfloat(r0.yyy) * vd_asfloat((VD_COL(unity_ObjectToWorld, 1).xyz))));
  r0.xyw = vd_asuint((vd_asfloat((VD_COL(unity_ObjectToWorld, 0).xyz)) * vd_asfloat(r0.xxx) + vd_asfloat(r1.xyz)));
  r0.xyz = vd_asuint((vd_asfloat((VD_COL(unity_ObjectToWorld, 2).xyz)) * vd_asfloat(r0.zzz) + vd_asfloat(r0.xyw)));
  r0.xyz = vd_asuint((vd_asfloat(r0.xyz) + vd_asfloat((VD_COL(unity_ObjectToWorld, 3).xyz))));
  r1.xyzw = vd_asuint((vd_asfloat(r0.yyyy) * vd_asfloat((VD_COL(unity_MatrixVP, 1).xyzw))));
  r1.xyzw = vd_asuint((vd_asfloat((VD_COL(unity_MatrixVP, 0).xyzw)) * vd_asfloat(r0.xxxx) + vd_asfloat(r1.xyzw)));
  r0.xyzw = vd_asuint((vd_asfloat((VD_COL(unity_MatrixVP, 2).xyzw)) * vd_asfloat(r0.zzzz) + vd_asfloat(r1.xyzw)));
  r0.xyzw = vd_asuint((vd_asfloat(r0.xyzw) + vd_asfloat((VD_COL(unity_MatrixVP, 3).xyzw))));
  r1.xy = vd_asuint((vd_asfloat(_HighlightLinePixelOffset.xy) + vd_asfloat(_HighlightLinePixelOffset.xy)));
  r1.xy = vd_asuint((vd_asfloat(r1.xy) / vd_asfloat(_ScreenParams.xy)));
  o0.xy = (vd_asfloat(r1.xy) * vd_asfloat(r0.ww) + vd_asfloat(r0.xy));
  o0.zw = vd_asfloat(r0.zw);
  o2.zw = vd_asfloat(r0.zw);
  r1.xy = vd_asuint(((-(vd_asfloat(_PlaneBoundMaxPos.xz))) + vd_asfloat((VD_COL(unity_ObjectToWorld, 3).xz))));
  r1.zw = vd_asuint((vd_asfloat(_PlaneBoundMinPos.xz) + (-(vd_asfloat(_PlaneBoundMaxPos.xz)))));
  r1.xy = vd_asuint(saturate((vd_asfloat(r1.xy) / vd_asfloat(r1.zw))));
  r2.xyzw = vd_asuint(_PlanePointBuffer.SampleLevel(sampler_PlanePointBuffer, vd_asfloat(r1.xy), 0.0).xyzw);
  r1.xyzw = vd_asuint(_PlaneSightRenderTarget.SampleLevel(sampler_PlaneSightRenderTarget, vd_asfloat(r1.xy), 0.0).xyzw);
  r0.z = vd_asuint(saturate((vd_asfloat(r2.x) * 4.0)));
  r1.x = vd_asuint((vd_asfloat(r0.z) * -2.0 + 3.0));
  r0.z = vd_asuint((vd_asfloat(r0.z) * vd_asfloat(r0.z)));
  r0.z = vd_asuint((vd_asfloat(r0.z) * vd_asfloat(r1.x)));
  r0.z = vd_asuint((vd_asfloat(r0.z) * vd_asfloat(_GlobalPlanePointBufferEnable)));
  r0.z = vd_asuint(max(vd_asfloat(r0.z), vd_asfloat(r1.w)));
  r0.z = vd_asuint((vd_asfloat(r0.z) + -1.0));
  r1.x = vd_asuint((vd_asfloat(_GlobalPlaneSystemEnable) * vd_asfloat(_GlobalIsRuntime)));
  r0.z = vd_asuint((vd_asfloat(r1.x) * vd_asfloat(r0.z) + 1.0));
  o1.z = (vd_asfloat(r0.z) * vd_asfloat(_FakeShadowVisibility));
  o1.xy = (vd_asfloat(v2.xy) * vd_asfloat(_MainTex_ST.xy) + vd_asfloat(_MainTex_ST.zw));
  o1.w = vd_asfloat(vd_asuint(v3.w));
  r0.y = vd_asuint((vd_asfloat(r0.y) * vd_asfloat(_ProjectionParams.x)));
  r0.xz = vd_asuint((vd_asfloat(r0.xw) * float2(0.5, 0.5)));
  r0.w = vd_asuint((vd_asfloat(r0.y) * 0.5));
  o2.xy = (vd_asfloat(r0.zz) + vd_asfloat(r0.xw));
  return;
}
