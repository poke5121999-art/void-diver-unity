// LCArt/Background/Shader_DE_WaterBottom | pass 0 "ForwardLit" | progVertex | keywords: FOG_LINEAR _ADDITIONAL_LIGHTS _DE_EDITOR_CAMERA _LIGHT_LAYERS | blob 4
void vert(
  float4 v0 : POSITION0,
  float3 v1 : NORMAL0,
  float4 v2 : TANGENT0,
  float4 v3 : TEXCOORD0,
  float2 v4 : TEXCOORD1,
  float4 v5 : COLOR0,
  out float4 o0 : VDREG0,
  out float4 o1 : VDREG1,
  out float4 o2 : VDREG2,
  out float4 o3 : VDREG3,
  out float4 o4 : VDREG4,
  out float4 o5 : VDREG5,
  out float4 o6 : SV_POSITION0,
  out float4 o7 : VDREG7,
  out float4 o8 : VDREG8)
{
  uint4 r0, r1, r2, r3;
  r0 = 0u; r1 = 0u; r2 = 0u; r3 = 0u;

  r0.x = vd_asuint((vd_asfloat(_Time.y) * vd_asfloat(_GlobalWaterWaveParams.x)));
  r0.xyz = vd_asuint((vd_asfloat(r0.xxx) * float3(0.707099974, -0.400000006, 0.692799985)));
  r1.xyz = vd_asuint((vd_asfloat(v0.yyy) * vd_asfloat((VD_COL(unity_ObjectToWorld, 1).xyz))));
  r1.xyz = vd_asuint((vd_asfloat((VD_COL(unity_ObjectToWorld, 0).xyz)) * vd_asfloat(v0.xxx) + vd_asfloat(r1.xyz)));
  r1.xyz = vd_asuint((vd_asfloat((VD_COL(unity_ObjectToWorld, 2).xyz)) * vd_asfloat(v0.zzz) + vd_asfloat(r1.xyz)));
  r1.xyz = vd_asuint((vd_asfloat(r1.xyz) + vd_asfloat((VD_COL(unity_ObjectToWorld, 3).xyz))));
  r2.xy = vd_asuint((vd_asfloat(r1.xz) * vd_asfloat(_GlobalWaterWaveParams.yy)));
  r0.yz = vd_asuint((vd_asfloat(r2.xy) * float2(0.699999988, 0.699999988) + vd_asfloat(r0.yz)));
  r0.xw = vd_asuint((vd_asfloat(r1.xz) * vd_asfloat(_GlobalWaterWaveParams.yy) + vd_asfloat(r0.xx)));
  r2.xyzw = vd_asuint(_GlobalWaterNoiseTex.SampleLevel(sampler_vd_linear_repeat, vd_asfloat(r0.xw), 0.0).xyzw);
  r0.x = vd_asuint((vd_asfloat(r2.x) + -0.5));
  r0.x = vd_asuint((vd_asfloat(r0.x) + vd_asfloat(r0.x)));
  r2.xyzw = vd_asuint(_GlobalWaterNoiseTex.SampleLevel(sampler_vd_linear_repeat, vd_asfloat(r0.yz), 0.0).xyzw);
  r0.y = vd_asuint((vd_asfloat(r2.x) + -0.5));
  r0.y = vd_asuint((vd_asfloat(r0.y) * 2.0 + vd_asfloat(r0.x)));
  r1.w = vd_asuint((vd_asfloat(r0.x) * vd_asfloat(_SurfaceWaveHeight) + vd_asfloat(_GlobalWaterBottomParams.x)));
  r0.xyzw = vd_asuint((vd_asfloat(r0.yyyy) * float4(0.5, 0.5, 0.5, 0.5)));
  o0.z = vd_asfloat(r0.w);
  o8.xyzw = vd_asfloat(r0.xyzw);
  o0.xy = (vd_asfloat(v3.xy) * vd_asfloat(_BaseMap_ST.xy) + vd_asfloat(_BaseMap_ST.zw));
  r0.xyzw = vd_asuint((vd_asfloat(r1.wwww) * vd_asfloat((VD_COL(unity_MatrixVP, 1).xyzw))));
  o1.xyz = vd_asfloat(r1.xwz);
  r0.xyzw = vd_asuint((vd_asfloat((VD_COL(unity_MatrixVP, 0).xyzw)) * vd_asfloat(r1.xxxx) + vd_asfloat(r0.xyzw)));
  r0.xyzw = vd_asuint((vd_asfloat((VD_COL(unity_MatrixVP, 2).xyzw)) * vd_asfloat(r1.zzzz) + vd_asfloat(r0.xyzw)));
  r1.xyz = vd_asuint(((-(vd_asfloat(r1.xyz))) + vd_asfloat(_WorldSpaceCameraPos.xyz)));
  r0.xyzw = vd_asuint((vd_asfloat(r0.xyzw) + vd_asfloat((VD_COL(unity_MatrixVP, 3).xyzw))));
  r1.w = vd_asuint((vd_asfloat(r0.z) / vd_asfloat(_ProjectionParams.y)));
  r1.w = vd_asuint(((-(vd_asfloat(r1.w))) + 1.0));
  r1.w = vd_asuint((vd_asfloat(r1.w) * vd_asfloat(_ProjectionParams.z)));
  r1.w = vd_asuint(max(vd_asfloat(r1.w), 0.0));
  o0.w = saturate((vd_asfloat(r1.w) * vd_asfloat(unity_FogParams.z) + vd_asfloat(unity_FogParams.w)));
  r2.x = vd_asuint(dot(vd_asfloat(v1.xyz), vd_asfloat((VD_COL(unity_WorldToObject, 0).xyz))));
  r2.y = vd_asuint(dot(vd_asfloat(v1.xyz), vd_asfloat((VD_COL(unity_WorldToObject, 1).xyz))));
  r2.z = vd_asuint(dot(vd_asfloat(v1.xyz), vd_asfloat((VD_COL(unity_WorldToObject, 2).xyz))));
  r1.w = vd_asuint(dot(vd_asfloat(r2.xyz), vd_asfloat(r2.xyz)));
  r1.w = vd_asuint(max(vd_asfloat(r1.w), 1.17549435e-38));
  r1.w = vd_asuint(rsqrt(vd_asfloat(r1.w)));
  r2.xyz = vd_asuint((vd_asfloat(r1.www) * vd_asfloat(r2.xyz)));
  o2.xyz = vd_asfloat(r2.xyz);
  r1.w = vd_asuint((-(int)(vd_asfloat(unity_OrthoParams.w) == 0.0)));
  o2.w = vd_asfloat(((r1.w != 0u) ? r1.x : vd_asuint((VD_COL(unity_MatrixV, 0).z))));
  o3.w = vd_asfloat(((r1.w != 0u) ? r1.y : vd_asuint((VD_COL(unity_MatrixV, 1).z))));
  o4.w = vd_asfloat(((r1.w != 0u) ? r1.z : vd_asuint((VD_COL(unity_MatrixV, 2).z))));
  r1.xyz = vd_asuint((vd_asfloat(v2.yyy) * vd_asfloat((VD_COL(unity_ObjectToWorld, 1).xyz))));
  r1.xyz = vd_asuint((vd_asfloat((VD_COL(unity_ObjectToWorld, 0).xyz)) * vd_asfloat(v2.xxx) + vd_asfloat(r1.xyz)));
  r1.xyz = vd_asuint((vd_asfloat((VD_COL(unity_ObjectToWorld, 2).xyz)) * vd_asfloat(v2.zzz) + vd_asfloat(r1.xyz)));
  r1.w = vd_asuint(dot(vd_asfloat(r1.xyz), vd_asfloat(r1.xyz)));
  r1.w = vd_asuint(max(vd_asfloat(r1.w), 1.17549435e-38));
  r1.w = vd_asuint(rsqrt(vd_asfloat(r1.w)));
  r1.xyz = vd_asuint((vd_asfloat(r1.www) * vd_asfloat(r1.xyz)));
  o3.xyz = vd_asfloat(r1.xyz);
  r3.xyz = vd_asuint((vd_asfloat(r1.yzx) * vd_asfloat(r2.zxy)));
  r1.xyz = vd_asuint((vd_asfloat(r2.yzx) * vd_asfloat(r1.zxy) + (-(vd_asfloat(r3.xyz)))));
  r1.w = vd_asuint((-(int)(vd_asfloat(unity_WorldTransformParams.w) >= 0.0)));
  r1.w = ((r1.w != 0u) ? 0x3f800000u : 0xbf800000u);
  r1.w = vd_asuint((vd_asfloat(r1.w) * vd_asfloat(v2.w)));
  o4.xyz = (vd_asfloat(r1.www) * vd_asfloat(r1.xyz));
  o5.xyz = vd_asfloat(uint3(0x00000000u, 0x00000000u, 0x00000000u));
  o6.xyzw = vd_asfloat(r0.xyzw);
  r0.y = vd_asuint((vd_asfloat(r0.y) * vd_asfloat(_ProjectionParams.x)));
  r1.xzw = vd_asuint((vd_asfloat(r0.xwy) * float3(0.5, 0.5, 0.5)));
  o7.zw = vd_asfloat(r0.zw);
  o7.xy = (vd_asfloat(r1.zz) + vd_asfloat(r1.xw));
  return;
}
