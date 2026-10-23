// LCArt/Character/shader_DE_Character_Normal | pass 0 "ForwardLit" | progVertex | keywords: _ADDITIONAL_LIGHTS _DE_EDITOR_CAMERA _DE_PRERENDERING_CAMERA | blob 18
void vert(
  float4 v0 : POSITION0,
  float3 v1 : NORMAL0,
  float4 v2 : TANGENT0,
  float4 v3 : TEXCOORD0,
  float4 v4 : TEXCOORD1,
  float2 v5 : TEXCOORD2,
  float4 v6 : COLOR0,
  out float4 o0 : SV_POSITION0,
  out float4 o1 : VDREG1,
  out float4 o2 : VDREG2,
  out float4 o3 : VDREG3,
  out float4 o4 : VDREG4,
  out float4 o5 : VDREG5,
  out float4 o6 : VDREG6,
  out float4 o7 : VDREG7,
  out float4 o8 : VDREG8,
  out float4 o9 : VDREG9)
{
  uint4 r0, r1, r2, r3;
  r0 = 0u; r1 = 0u; r2 = 0u; r3 = 0u;

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
  r0.z = vd_asuint((-(int)(vd_asfloat(r0.x) >= 0.0)));
  r0.z = (r0.z & 0x3f800000u);
  r0.w = vd_asuint((1.0 / vd_asfloat((VD_COL(unity_MatrixV, 1).y))));
  r1.y = vd_asuint(min(abs(vd_asfloat(r0.w)), 1.5));
  r1.xz = uint2(0x00000000u, 0x00000000u);
  r2.x = vd_asuint((VD_COL(unity_MatrixV, 0).y));
  r2.y = vd_asuint((VD_COL(unity_MatrixV, 1).y));
  r2.z = vd_asuint((VD_COL(unity_MatrixV, 2).y));
  r1.xyz = vd_asuint((vd_asfloat(r1.xyz) + (-(vd_asfloat(r2.xyz)))));
  r1.xyz = vd_asuint((vd_asfloat(r0.zzz) * vd_asfloat(r1.xyz) + vd_asfloat(r2.xyz)));
  r2.xyz = vd_asuint(((-(vd_asfloat(_WorldSpaceCameraPos.xyz))) + vd_asfloat((VD_COL(unity_ObjectToWorld, 3).xyz))));
  r0.z = vd_asuint(dot(vd_asfloat(r2.xyz), vd_asfloat(r2.xyz)));
  r0.z = vd_asuint(rsqrt(vd_asfloat(r0.z)));
  r2.xyz = vd_asuint((vd_asfloat(r0.zzz) * vd_asfloat(r2.xyz)));
  r2.w = vd_asuint((-(vd_asfloat(r2.x))));
  r0.zw = vd_asuint((vd_asfloat(r2.zw) * vd_asfloat(v0.xx)));
  r3.xz = vd_asuint((vd_asfloat(r0.zw) * float2(1.17499995, 1.17499995)));
  r3.y = 0x00000000u;
  r0.xzw = vd_asuint((vd_asfloat(r1.xyz) * vd_asfloat(r0.xxx) + vd_asfloat(r3.xyz)));
  r1.xyz = vd_asuint((vd_asfloat(v0.yyy) * float3(0.0, 1.0, 0.0) + vd_asfloat(r3.xyz)));
  r1.xyz = vd_asuint((vd_asfloat(r2.xyz) * float3(0.200000003, 0.200000003, 0.200000003) + vd_asfloat(r1.xyz)));
  r3.xyz = vd_asuint((vd_asfloat(r2.xyz) * vd_asfloat(v0.zzz)));
  r0.xyz = vd_asuint((vd_asfloat(r3.xyz) * vd_asfloat(r0.yyy) + vd_asfloat(r0.xzw)));
  r0.w = vd_asuint(max((-(vd_asfloat(r0.y))), 0.0));
  r0.w = vd_asuint((vd_asfloat(r0.w) * vd_asfloat(_OpticalRatioYNegativeUnit)));
  r0.xyz = vd_asuint(((-(vd_asfloat(r2.xyz))) * vd_asfloat(r0.www) + vd_asfloat(r0.xyz)));
  r0.w = vd_asuint((-(int)(vd_asfloat(_GhostEnable) >= 0.00100000005)));
  r0.w = ((r0.w != 0u) ? 0x00000000u : 0x3f800000u);
  r0.xyz = vd_asuint((vd_asfloat(r0.xyz) * vd_asfloat(r0.www)));
  r2.xyz = vd_asuint((vd_asfloat(r0.yyy) * vd_asfloat((VD_COL(unity_ObjectToWorld, 1).xyz))));
  r2.xyz = vd_asuint((vd_asfloat((VD_COL(unity_ObjectToWorld, 0).xyz)) * vd_asfloat(r0.xxx) + vd_asfloat(r2.xyz)));
  r0.xyz = vd_asuint((vd_asfloat((VD_COL(unity_ObjectToWorld, 2).xyz)) * vd_asfloat(r0.zzz) + vd_asfloat(r2.xyz)));
  r0.xyz = vd_asuint((vd_asfloat(r0.xyz) + vd_asfloat((VD_COL(unity_ObjectToWorld, 3).xyz))));
  r2.xyzw = vd_asuint((vd_asfloat(r0.yyyy) * vd_asfloat((VD_COL(unity_MatrixVP, 1).xyzw))));
  r2.xyzw = vd_asuint((vd_asfloat((VD_COL(unity_MatrixVP, 0).xyzw)) * vd_asfloat(r0.xxxx) + vd_asfloat(r2.xyzw)));
  r2.xyzw = vd_asuint((vd_asfloat((VD_COL(unity_MatrixVP, 2).xyzw)) * vd_asfloat(r0.zzzz) + vd_asfloat(r2.xyzw)));
  o3.xyz = vd_asfloat(r0.xyz);
  r2.xyzw = vd_asuint((vd_asfloat(r2.xyzw) + vd_asfloat((VD_COL(unity_MatrixVP, 3).xyzw))));
  o0.xyzw = vd_asfloat(r2.xyzw);
  o1.xy = (vd_asfloat(v3.xy) * vd_asfloat(_MainTex_ST.xy) + vd_asfloat(_MainTex_ST.zw));
  o1.zw = (vd_asfloat(v3.xy) * vd_asfloat(_NormalTex_ST.xy) + vd_asfloat(_NormalTex_ST.zw));
  r0.x = vd_asuint(dot(vd_asfloat(v1.xyz), vd_asfloat((VD_COL(unity_WorldToObject, 0).xyz))));
  r0.y = vd_asuint(dot(vd_asfloat(v1.xyz), vd_asfloat((VD_COL(unity_WorldToObject, 1).xyz))));
  r0.z = vd_asuint(dot(vd_asfloat(v1.xyz), vd_asfloat((VD_COL(unity_WorldToObject, 2).xyz))));
  r1.w = vd_asuint(dot(vd_asfloat(r0.xyz), vd_asfloat(r0.xyz)));
  r1.w = vd_asuint(max(vd_asfloat(r1.w), 1.17549435e-38));
  r1.w = vd_asuint(rsqrt(vd_asfloat(r1.w)));
  o2.xyz = (vd_asfloat(r0.xyz) * vd_asfloat(r1.www));
  o2.w = vd_asfloat(0x3f800000u);
  o3.w = vd_asfloat(vd_asuint(v0.y));
  r0.xyz = vd_asuint((vd_asfloat(r1.yyy) * vd_asfloat((VD_COL(unity_ObjectToWorld, 1).xyz))));
  r0.xyz = vd_asuint((vd_asfloat((VD_COL(unity_ObjectToWorld, 0).xyz)) * vd_asfloat(r1.xxx) + vd_asfloat(r0.xyz)));
  r0.xyz = vd_asuint((vd_asfloat((VD_COL(unity_ObjectToWorld, 2).xyz)) * vd_asfloat(r1.zzz) + vd_asfloat(r0.xyz)));
  r0.xyz = vd_asuint((vd_asfloat(r0.xyz) + vd_asfloat((VD_COL(unity_ObjectToWorld, 3).xyz))));
  o4.xyz = (vd_asfloat(r0.xyz) * vd_asfloat(r0.www));
  o5.xyz = vd_asfloat(uint3(0x3f800000u, 0x00000000u, 0x00000000u));
  o6.xyz = vd_asfloat(uint3(0x00000000u, 0x3f800000u, 0x00000000u));
  r0.x = vd_asuint((vd_asfloat(r2.y) * vd_asfloat(_ProjectionParams.x)));
  r0.w = vd_asuint((vd_asfloat(r0.x) * 0.5));
  r0.xz = vd_asuint((vd_asfloat(r2.xw) * float2(0.5, 0.5)));
  o7.zw = vd_asfloat(r2.zw);
  o7.xy = (vd_asfloat(r0.zz) + vd_asfloat(r0.xw));
  r0.x = vd_asuint((-(int)(0.00100000005 < vd_asfloat(v6.w))));
  r0.yzw = vd_asuint((vd_asfloat(v6.xyz) / vd_asfloat(v6.www)));
  r0.xyz = ((r0.xxx != 0u) ? r0.yzw : vd_asuint(v6.xyz));
  r1.xyz = vd_asuint((vd_asfloat(r0.xyz) + float3(0.0549999997, 0.0549999997, 0.0549999997)));
  r1.xyz = vd_asuint((vd_asfloat(r1.xyz) * float3(0.947867334, 0.947867334, 0.947867334)));
  r1.xyz = vd_asuint(log2(abs(vd_asfloat(r1.xyz))));
  r1.xyz = vd_asuint((vd_asfloat(r1.xyz) * float3(2.4000001, 2.4000001, 2.4000001)));
  r1.xyz = vd_asuint(exp2(vd_asfloat(r1.xyz)));
  r2.xyz = vd_asuint((vd_asfloat(r0.xyz) * float3(0.0773993805, 0.0773993805, 0.0773993805)));
  r0.xyz = vd_asuint((-(int3)(float3(0.0404499993, 0.0404499993, 0.0404499993) >= vd_asfloat(r0.xyz))));
  o8.xyz = vd_asfloat(((r0.xyz != 0u) ? r2.xyz : r1.xyz));
  o8.w = vd_asfloat(vd_asuint(v6.w));
  r0.xy = vd_asuint((vd_asfloat(v4.xy) + float2(0.0549999997, 0.0549999997)));
  r0.xy = vd_asuint((vd_asfloat(r0.xy) * float2(0.947867334, 0.947867334)));
  r0.xy = vd_asuint(log2(abs(vd_asfloat(r0.xy))));
  r0.xy = vd_asuint((vd_asfloat(r0.xy) * float2(2.4000001, 2.4000001)));
  r0.xy = vd_asuint(exp2(vd_asfloat(r0.xy)));
  r0.zw = vd_asuint((vd_asfloat(v4.xy) * float2(0.0773993805, 0.0773993805)));
  r1.xy = vd_asuint((-(int2)(float2(0.0404499993, 0.0404499993) >= vd_asfloat(v4.xy))));
  o9.xy = vd_asfloat(((r1.xy != 0u) ? r0.zw : r0.xy));
  r0.x = vd_asuint((vd_asfloat(v5.x) + 0.0549999997));
  r0.x = vd_asuint((vd_asfloat(r0.x) * 0.947867334));
  r0.x = vd_asuint(log2(abs(vd_asfloat(r0.x))));
  r0.x = vd_asuint((vd_asfloat(r0.x) * 2.4000001));
  r0.x = vd_asuint(exp2(vd_asfloat(r0.x)));
  r0.y = vd_asuint((vd_asfloat(v5.x) * 0.0773993805));
  r0.z = vd_asuint((-(int)(0.0404499993 >= vd_asfloat(v5.x))));
  o9.z = vd_asfloat(((r0.z != 0u) ? r0.y : r0.x));
  return;
}
