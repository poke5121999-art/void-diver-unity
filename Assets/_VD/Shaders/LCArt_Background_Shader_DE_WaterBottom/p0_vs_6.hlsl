// LCArt/Background/Shader_DE_WaterBottom | pass 0 "ForwardLit" | progVertex | keywords: FOG_LINEAR _ADDITIONAL_LIGHTS _LIGHT_LAYERS | blob 6
void vert(
  float4 v0 : POSITION0,
  float3 v1 : NORMAL0,
  float4 v2 : TANGENT0,
  float4 v3 : TEXCOORD0,
  float2 v4 : TEXCOORD1,
  float4 v5 : COLOR0,
  out float2 o0 : TEXCOORD0,
  out float p0 : TEXCOORD9,
  out float p0 : TEXCOORD10,
  out float4 o1 : TEXCOORD1,
  out float4 o2 : TEXCOORD2,
  out float4 o3 : TEXCOORD3,
  out float4 o4 : TEXCOORD4,
  out float3 o5 : TEXCOORD7,
  out float4 o6 : SV_POSITION0,
  out float4 o7 : TEXCOORD8,
  out float4 o8 : COLOR0)
{
  float4 r0,r1,r2,r3;
  uint4 bitmask, uiDest;
  float4 fDest;

  r0.x = _GlobalWaterWaveParams.x * _Time.y;
  r0.xyz = float3(0.707099974,-0.400000006,0.692799985) * r0.xxx;
  r1.xyz = (VD_COL(unity_ObjectToWorld, 1).xyz) * v0.yyy;
  r1.xyz = (VD_COL(unity_ObjectToWorld, 0).xyz) * v0.xxx + r1.xyz;
  r1.xyz = (VD_COL(unity_ObjectToWorld, 2).xyz) * v0.zzz + r1.xyz;
  r1.xyz = (VD_COL(unity_ObjectToWorld, 3).xyz) + r1.xyz;
  r2.xy = _GlobalWaterWaveParams.yy * r1.xz;
  r0.yz = r2.xy * float2(0.699999988,0.699999988) + r0.yz;
  r0.xw = r1.xz * _GlobalWaterWaveParams.yy + r0.xx;
  r2.xyzw = _GlobalWaterNoiseTex.SampleLevel(sampler_vd_linear_repeat, r0.xw, 0).xyzw;
  r0.x = -0.5 + r2.x;
  r0.x = r0.x + r0.x;
  r2.xyzw = _GlobalWaterNoiseTex.SampleLevel(sampler_vd_linear_repeat, r0.yz, 0).xyzw;
  r0.y = -0.5 + r2.x;
  r0.y = r0.y * 2 + r0.x;
  r1.w = r0.x * _SurfaceWaveHeight + _GlobalWaterBottomParams.x;
  r0.xyzw = float4(0.5,0.5,0.5,0.5) * r0.yyyy;
  p0.x = r0.w;
  o8.xyzw = r0.xyzw;
  o0.xy = v3.xy * _BaseMap_ST.xy + _BaseMap_ST.zw;
  r0.xyzw = (VD_COL(unity_MatrixVP, 1).xyzw) * r1.wwww;
  o1.xyz = r1.xwz;
  r0.xyzw = (VD_COL(unity_MatrixVP, 0).xyzw) * r1.xxxx + r0.xyzw;
  r0.xyzw = (VD_COL(unity_MatrixVP, 2).xyzw) * r1.zzzz + r0.xyzw;
  r1.xyz = _WorldSpaceCameraPos.xyz + -r1.xyz;
  r0.xyzw = (VD_COL(unity_MatrixVP, 3).xyzw) + r0.xyzw;
  r1.w = r0.z / _ProjectionParams.y;
  r1.w = 1 + -r1.w;
  r1.w = _ProjectionParams.z * r1.w;
  r1.w = max(0, r1.w);
  p0.x = saturate(r1.w * unity_FogParams.z + unity_FogParams.w);
  r2.x = dot(v1.xyz, (VD_COL(unity_WorldToObject, 0).xyz));
  r2.y = dot(v1.xyz, (VD_COL(unity_WorldToObject, 1).xyz));
  r2.z = dot(v1.xyz, (VD_COL(unity_WorldToObject, 2).xyz));
  r1.w = dot(r2.xyz, r2.xyz);
  r1.w = max(1.17549435e-038, r1.w);
  r1.w = rsqrt(r1.w);
  r2.xyz = r2.xyz * r1.www;
  o2.xyz = r2.xyz;
  r1.w = cmp(unity_OrthoParams.w == 0.000000);
  o2.w = r1.w ? r1.x : (VD_COL(unity_MatrixV, 0).z);
  o3.w = r1.w ? r1.y : (VD_COL(unity_MatrixV, 1).z);
  o4.w = r1.w ? r1.z : (VD_COL(unity_MatrixV, 2).z);
  r1.xyz = (VD_COL(unity_ObjectToWorld, 1).xyz) * v2.yyy;
  r1.xyz = (VD_COL(unity_ObjectToWorld, 0).xyz) * v2.xxx + r1.xyz;
  r1.xyz = (VD_COL(unity_ObjectToWorld, 2).xyz) * v2.zzz + r1.xyz;
  r1.w = dot(r1.xyz, r1.xyz);
  r1.w = max(1.17549435e-038, r1.w);
  r1.w = rsqrt(r1.w);
  r1.xyz = r1.xyz * r1.www;
  o3.xyz = r1.xyz;
  r3.xyz = r2.zxy * r1.yzx;
  r1.xyz = r2.yzx * r1.zxy + -r3.xyz;
  r1.w = cmp(unity_WorldTransformParams.w >= 0);
  r1.w = r1.w ? 1 : -1;
  r1.w = v2.w * r1.w;
  o4.xyz = r1.xyz * r1.www;
  o5.xyz = float3(0,0,0);
  o6.xyzw = r0.xyzw;
  r0.y = _ProjectionParams.x * r0.y;
  r1.xzw = float3(0.5,0.5,0.5) * r0.xwy;
  o7.zw = r0.zw;
  o7.xy = r1.xw + r1.zz;
  return;
}
