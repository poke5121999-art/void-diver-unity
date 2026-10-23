// LCArt/Character/shader_DE_Character_Normal | pass 1 "SpineGrabpass" | progVertex | keywords: _DE_PRERENDERING_CAMERA | blob 61
void vert(
  float4 v0 : POSITION0,
  float3 v1 : NORMAL0,
  float4 v2 : TANGENT0,
  float2 v3 : TEXCOORD0,
  float4 v4 : COLOR0,
  out float4 o0 : SV_POSITION0,
  out float4 o1 : VDREG1,
  out float4 o2 : VDREG2,
  out float4 o3 : VDREG3,
  out float4 o4 : VDREG4,
  out float4 o5 : VDREG5,
  out float4 o6 : VDREG6,
  out float4 o7 : VDREG7,
  out float4 o8 : VDREG8,
  out float4 o9 : VDREG9,
  out float4 o10 : VDREG10)
{
  uint4 r0, r1, r2, r3, r4, r5, r6, r7;
  r0 = 0u; r1 = 0u; r2 = 0u; r3 = 0u; r4 = 0u; r5 = 0u; r6 = 0u; r7 = 0u;

  r0.x = vd_asuint((vd_asfloat(v0.y) + -0.125));
  r0.x = vd_asuint(saturate((vd_asfloat(r0.x) * 2.66666675)));
  r0.y = vd_asuint((vd_asfloat(r0.x) * -2.0 + 3.0));
  r0.x = vd_asuint((vd_asfloat(r0.x) * vd_asfloat(r0.x)));
  r0.x = vd_asuint((vd_asfloat(r0.x) * vd_asfloat(r0.y)));
  r0.y = vd_asuint((vd_asfloat(v0.x) + vd_asfloat(_Time.y)));
  r0.y = vd_asuint((vd_asfloat(r0.y) * 123.339996));
  r0.y = vd_asuint(frac(vd_asfloat(r0.y)));
  r0.x = vd_asuint((vd_asfloat(r0.x) * vd_asfloat(r0.y)));
  r0.x = vd_asuint((vd_asfloat(r0.x) * vd_asfloat(_LightEffectIntensity)));
  r0.x = vd_asuint((vd_asfloat(r0.x) * 0.25 + vd_asfloat(v0.y)));
  r0.y = vd_asuint((-(int)(vd_asfloat(r0.x) >= vd_asfloat(_OpticalRatioYThreshold))));
  r0.y = (r0.y & 0x3f800000u);
  r0.z = vd_asuint((vd_asfloat(_OpticalRatioYPositiveRatio) + -1.0));
  r0.y = vd_asuint((vd_asfloat(r0.y) * vd_asfloat(r0.z) + 1.0));
  r0.w = vd_asuint((-(int)(vd_asfloat(r0.x) >= 0.0)));
  r0.w = (r0.w & 0x3f800000u);
  r1.x = vd_asuint((1.0 / vd_asfloat((VD_COL(unity_MatrixV, 1).y))));
  r1.y = vd_asuint(min(abs(vd_asfloat(r1.x)), 1.5));
  r1.xz = uint2(0x00000000u, 0x00000000u);
  r2.x = vd_asuint((VD_COL(unity_MatrixV, 0).y));
  r2.y = vd_asuint((VD_COL(unity_MatrixV, 1).y));
  r2.z = vd_asuint((VD_COL(unity_MatrixV, 2).y));
  r1.xyz = vd_asuint((vd_asfloat(r1.xyz) + (-(vd_asfloat(r2.xyz)))));
  r3.xyz = vd_asuint((vd_asfloat(r0.www) * vd_asfloat(r1.xyz) + vd_asfloat(r2.xyz)));
  r4.xyz = vd_asuint(((-(vd_asfloat(_WorldSpaceCameraPos.xyz))) + vd_asfloat((VD_COL(unity_ObjectToWorld, 3).xyz))));
  r0.w = vd_asuint(dot(vd_asfloat(r4.xyz), vd_asfloat(r4.xyz)));
  r0.w = vd_asuint(rsqrt(vd_asfloat(r0.w)));
  r4.xyz = vd_asuint((vd_asfloat(r0.www) * vd_asfloat(r4.xyz)));
  r4.w = vd_asuint((-(vd_asfloat(r4.x))));
  r5.xy = vd_asuint((vd_asfloat(r4.zw) * vd_asfloat(v0.xx)));
  r5.xz = vd_asuint((vd_asfloat(r5.xy) * float2(1.17499995, 1.17499995)));
  r5.y = 0x00000000u;
  r3.xyz = vd_asuint((vd_asfloat(r3.xyz) * vd_asfloat(r0.xxx) + vd_asfloat(r5.xyz)));
  r6.xyz = vd_asuint((vd_asfloat(r4.xyz) * vd_asfloat(v0.zzz)));
  r0.xyw = vd_asuint((vd_asfloat(r6.xyz) * vd_asfloat(r0.yyy) + vd_asfloat(r3.xyz)));
  r1.w = vd_asuint(max((-(vd_asfloat(r0.y))), 0.0));
  r1.w = vd_asuint((vd_asfloat(r1.w) * vd_asfloat(_OpticalRatioYNegativeUnit)));
  r0.xyw = vd_asuint(((-(vd_asfloat(r4.xyz))) * vd_asfloat(r1.www) + vd_asfloat(r0.xyw)));
  r1.w = vd_asuint((-(int)(vd_asfloat(_GhostEnable) >= 0.00100000005)));
  r1.w = (r1.w & 0x3f800000u);
  r0.xyw = vd_asuint((vd_asfloat(r0.xyw) * vd_asfloat(r1.www)));
  r3.xyz = vd_asuint((vd_asfloat(r0.yyy) * vd_asfloat((VD_COL(unity_ObjectToWorld, 1).xyz))));
  r3.xyz = vd_asuint((vd_asfloat((VD_COL(unity_ObjectToWorld, 0).xyz)) * vd_asfloat(r0.xxx) + vd_asfloat(r3.xyz)));
  r3.xyz = vd_asuint((vd_asfloat((VD_COL(unity_ObjectToWorld, 2).xyz)) * vd_asfloat(r0.www) + vd_asfloat(r3.xyz)));
  r3.xyz = vd_asuint((vd_asfloat(r3.xyz) + vd_asfloat((VD_COL(unity_ObjectToWorld, 3).xyz))));
  r7.xyzw = vd_asuint((vd_asfloat(r3.yyyy) * vd_asfloat((VD_COL(unity_MatrixVP, 1).xyzw))));
  r7.xyzw = vd_asuint((vd_asfloat((VD_COL(unity_MatrixVP, 0).xyzw)) * vd_asfloat(r3.xxxx) + vd_asfloat(r7.xyzw)));
  r7.xyzw = vd_asuint((vd_asfloat((VD_COL(unity_MatrixVP, 2).xyzw)) * vd_asfloat(r3.zzzz) + vd_asfloat(r7.xyzw)));
  o3.xyz = vd_asfloat(r3.xyz);
  r3.xyzw = vd_asuint((vd_asfloat(r7.xyzw) + vd_asfloat((VD_COL(unity_MatrixVP, 3).xyzw))));
  o0.xyzw = vd_asfloat(r3.xyzw);
  o1.xy = (vd_asfloat(v3.xy) * vd_asfloat(_MainTex_ST.xy) + vd_asfloat(_MainTex_ST.zw));
  o1.zw = (vd_asfloat(v3.xy) * vd_asfloat(_NormalTex_ST.xy) + vd_asfloat(_NormalTex_ST.zw));
  r7.x = vd_asuint((VD_COL(unity_WorldToObject, 0).y));
  r7.y = vd_asuint((VD_COL(unity_WorldToObject, 1).y));
  r7.z = vd_asuint((VD_COL(unity_WorldToObject, 2).y));
  r2.w = vd_asuint(dot(vd_asfloat(r7.xyz), vd_asfloat(r7.xyz)));
  r2.w = vd_asuint(max(vd_asfloat(r2.w), 1.17549435e-38));
  r2.w = vd_asuint(rsqrt(vd_asfloat(r2.w)));
  r7.xyz = vd_asuint((vd_asfloat(r2.www) * vd_asfloat(r7.xyz)));
  o2.xyz = vd_asfloat(r7.xyz);
  o9.xyz = vd_asfloat(r7.xyz);
  o2.w = vd_asfloat(0x3f800000u);
  r2.w = vd_asuint((-(int)(vd_asfloat(v0.y) >= 0.0)));
  r2.w = (r2.w & 0x3f800000u);
  r1.xyz = vd_asuint((vd_asfloat(r2.www) * vd_asfloat(r1.xyz) + vd_asfloat(r2.xyz)));
  r1.xyz = vd_asuint((vd_asfloat(r1.xyz) * vd_asfloat(v0.yyy) + vd_asfloat(r5.xyz)));
  r2.x = vd_asuint((-(int)(vd_asfloat(v0.y) >= vd_asfloat(_OpticalRatioYThreshold))));
  r2.x = (r2.x & 0x3f800000u);
  r0.z = vd_asuint((vd_asfloat(r2.x) * vd_asfloat(r0.z) + 1.0));
  r1.xyz = vd_asuint((vd_asfloat(r6.xyz) * vd_asfloat(r0.zzz) + vd_asfloat(r1.xyz)));
  r0.z = vd_asuint(max((-(vd_asfloat(r1.y))), 0.0));
  r0.z = vd_asuint((vd_asfloat(r0.z) * vd_asfloat(_OpticalRatioYNegativeUnit)));
  r1.xyz = vd_asuint(((-(vd_asfloat(r4.xyz))) * vd_asfloat(r0.zzz) + vd_asfloat(r1.xyz)));
  r2.xyz = vd_asuint((vd_asfloat(r1.yyy) * vd_asfloat((VD_COL(unity_ObjectToWorld, 1).xyz))));
  r2.xyz = vd_asuint((vd_asfloat((VD_COL(unity_ObjectToWorld, 0).xyz)) * vd_asfloat(r1.xxx) + vd_asfloat(r2.xyz)));
  r1.xyz = vd_asuint((vd_asfloat((VD_COL(unity_ObjectToWorld, 2).xyz)) * vd_asfloat(r1.zzz) + vd_asfloat(r2.xyz)));
  r1.xyz = vd_asuint((vd_asfloat(r1.xyz) + vd_asfloat((VD_COL(unity_ObjectToWorld, 3).xyz))));
  o4.xyz = (vd_asfloat(r1.xyz) * vd_asfloat(r1.www));
  o5.xyz = vd_asfloat(uint3(0x3f800000u, 0x00000000u, 0x00000000u));
  o6.xyz = vd_asfloat(uint3(0x00000000u, 0x3f800000u, 0x00000000u));
  r0.z = vd_asuint((vd_asfloat(r3.y) * vd_asfloat(_ProjectionParams.x)));
  r1.w = vd_asuint((vd_asfloat(r0.z) * 0.5));
  r1.xz = vd_asuint((vd_asfloat(r3.xw) * float2(0.5, 0.5)));
  o7.zw = vd_asfloat(r3.zw);
  o7.xy = (vd_asfloat(r1.zz) + vd_asfloat(r1.xw));
  r0.z = vd_asuint((-(int)(0.00100000005 < vd_asfloat(v4.w))));
  r1.xyz = vd_asuint((vd_asfloat(v4.xyz) / vd_asfloat(v4.www)));
  r1.xyz = ((r0.zzz != 0u) ? r1.xyz : vd_asuint(v4.xyz));
  r2.xyz = vd_asuint((vd_asfloat(r1.xyz) + float3(0.0549999997, 0.0549999997, 0.0549999997)));
  r2.xyz = vd_asuint((vd_asfloat(r2.xyz) * float3(0.947867334, 0.947867334, 0.947867334)));
  r2.xyz = vd_asuint(log2(abs(vd_asfloat(r2.xyz))));
  r2.xyz = vd_asuint((vd_asfloat(r2.xyz) * float3(2.4000001, 2.4000001, 2.4000001)));
  r2.xyz = vd_asuint(exp2(vd_asfloat(r2.xyz)));
  r3.xyz = vd_asuint((vd_asfloat(r1.xyz) * float3(0.0773993805, 0.0773993805, 0.0773993805)));
  r1.xyz = vd_asuint((-(int3)(float3(0.0404499993, 0.0404499993, 0.0404499993) >= vd_asfloat(r1.xyz))));
  o8.xyz = vd_asfloat(((r1.xyz != 0u) ? r3.xyz : r2.xyz));
  o8.w = vd_asfloat(vd_asuint(v4.w));
  r1.xyzw = vd_asuint((vd_asfloat(r0.yyyy) * vd_asfloat((VD_COL(unity_ObjectToWorld, 1).xyzw))));
  r1.xyzw = vd_asuint((vd_asfloat((VD_COL(unity_ObjectToWorld, 0).xyzw)) * vd_asfloat(r0.xxxx) + vd_asfloat(r1.xyzw)));
  r0.xyzw = vd_asuint((vd_asfloat((VD_COL(unity_ObjectToWorld, 2).xyzw)) * vd_asfloat(r0.wwww) + vd_asfloat(r1.xyzw)));
  r0.xyzw = vd_asuint((vd_asfloat(r0.xyzw) + vd_asfloat((VD_COL(unity_ObjectToWorld, 3).xyzw))));
  r1.xyz = vd_asuint((vd_asfloat(r0.yyy) * vd_asfloat((VD_COL(unity_MatrixVP, 1).xyw))));
  r1.xyz = vd_asuint((vd_asfloat((VD_COL(unity_MatrixVP, 0).xyw)) * vd_asfloat(r0.xxx) + vd_asfloat(r1.xyz)));
  r0.xyz = vd_asuint((vd_asfloat((VD_COL(unity_MatrixVP, 2).xyw)) * vd_asfloat(r0.zzz) + vd_asfloat(r1.xyz)));
  r0.xyz = vd_asuint((vd_asfloat((VD_COL(unity_MatrixVP, 3).xyw)) * vd_asfloat(r0.www) + vd_asfloat(r0.xyz)));
  r0.xy = vd_asuint((vd_asfloat(r0.xy) / (-(vd_asfloat(r0.zz)))));
  r0.xy = vd_asuint((vd_asfloat(r0.xy) * float2(0.5, 0.5) + float2(0.5, 0.5)));
  r0.z = vd_asuint(((-(vd_asfloat(r0.x))) + 1.0));
  r0.w = vd_asuint(v0.y);
  o10.xyz = vd_asfloat(r0.zyw);
  return;
}
