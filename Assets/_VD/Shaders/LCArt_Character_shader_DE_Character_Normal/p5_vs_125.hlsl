// LCArt/Character/shader_DE_Character_Normal | pass 5 "Reflection" | progVertex | keywords: _DE_EDITOR_CAMERA | blob 125
void vert(
  float4 v0 : POSITION0,
  float3 v1 : NORMAL0,
  float4 v2 : TANGENT0,
  float2 v3 : TEXCOORD0,
  float4 v4 : COLOR0,
  out float4 o0 : SV_POSITION0,
  out float2 o1 : TEXCOORD0,
  out float2 p1 : TEXCOORD2,
  out float3 o2 : TEXCOORD1,
  out float p2 : TEXCOOR5,
  out float4 o3 : TEXCOORD3,
  out float4 o4 : TEXCOORD4,
  out float4 o5 : TEXCOORD6,
  out float4 o6 : TEXCOORD7,
  out float4 o7 : TEXCOORD8,
  out float4 o8 : COLOR0)
{
  float4 r0,r1,r2,r3,r4,r5;
  uint4 bitmask, uiDest;
  float4 fDest;

  r0.x = -0.25 + _PlaneReflectionPivotHeight;
  r0.x = cmp(r0.x >= (VD_COL(unity_ObjectToWorld, 3).y));
  r0.x = r0.x ? 1.000000 : 0;
  r0.xyz = r0.xxx * -v0.xyz + v0.xyz;
  r0.w = cmp(r0.y >= 0);
  r0.w = r0.w ? 1.000000 : 0;
  r1.x = 1 / (VD_COL(unity_MatrixV, 1).y);
  r1.y = min(1.5, abs(r1.x));
  r1.xz = float2(0,0);
  r2.x = (VD_COL(unity_MatrixV, 0).y);
  r2.y = (VD_COL(unity_MatrixV, 1).y);
  r2.z = (VD_COL(unity_MatrixV, 2).y);
  r1.xyz = -r2.xyz + r1.xyz;
  r3.xyz = r0.www * r1.xyz + r2.xyz;
  r3.xyz = r3.xyz * r0.yyy;
  r4.xyz = (VD_COL(unity_ObjectToWorld, 3).xyz) + -_WorldSpaceCameraPos.xyz;
  r0.w = dot(r4.xyz, r4.xyz);
  r0.w = rsqrt(r0.w);
  r4.xyz = r4.xyz * r0.www;
  r5.xz = float2(1.17499995,-1.17499995) * r4.zx;
  r5.y = 0;
  r3.xyz = r5.xyz * r0.xxx + r3.xyz;
  r0.x = cmp(r0.y >= _OpticalRatioYThreshold);
  r0.yzw = r4.xyz * r0.zzz;
  r0.x = r0.x ? 1.000000 : 0;
  r1.w = -1 + _OpticalRatioYPositiveRatio;
  r0.x = r0.x * r1.w + 1;
  r0.xyz = r0.yzw * r0.xxx + r3.xyz;
  r0.w = max(0, -r0.y);
  r0.w = _OpticalRatioYNegativeUnit * r0.w;
  r0.xyz = -r4.xyz * r0.www + r0.xyz;
  r3.xyz = (VD_COL(unity_ObjectToWorld, 1).xyz) * r0.yyy;
  r0.xyw = (VD_COL(unity_ObjectToWorld, 0).xyz) * r0.xxx + r3.xyz;
  r0.xyz = (VD_COL(unity_ObjectToWorld, 2).xyz) * r0.zzz + r0.xyw;
  r0.xyz = (VD_COL(unity_ObjectToWorld, 3).xyz) + r0.xyz;
  r3.xyzw = (VD_COL(unity_MatrixVP, 1).xyzw) * r0.yyyy;
  r3.xyzw = (VD_COL(unity_MatrixVP, 0).xyzw) * r0.xxxx + r3.xyzw;
  r3.xyzw = (VD_COL(unity_MatrixVP, 2).xyzw) * r0.zzzz + r3.xyzw;
  o3.xyz = r0.xyz;
  r0.xyzw = (VD_COL(unity_MatrixVP, 3).xyzw) + r3.xyzw;
  o0.xyzw = r0.xyzw;
  o1.xy = v3.xy * _MainTex_ST.xy + _MainTex_ST.zw;
  p1.xy = v3.xy * _NormalTex_ST.xy + _NormalTex_ST.zw;
  r2.w = cmp(v0.y >= 0);
  r2.w = r2.w ? 1.000000 : 0;
  r1.xyz = r2.www * r1.xyz + r2.xyz;
  r1.xyz = v0.yyy * r1.xyz;
  r1.xyz = r5.xyz * v0.xxx + r1.xyz;
  r2.x = cmp(v0.y >= _OpticalRatioYThreshold);
  r2.x = r2.x ? 1.000000 : 0;
  r1.w = r2.x * r1.w + 1;
  r2.xyz = v0.zzz * r4.xyz;
  r1.xyz = r2.xyz * r1.www + r1.xyz;
  r1.w = max(0, -r1.y);
  r1.w = _OpticalRatioYNegativeUnit * r1.w;
  r1.xyz = -r4.xyz * r1.www + r1.xyz;
  r2.xyz = (VD_COL(unity_ObjectToWorld, 1).xyz) * r1.yyy;
  r1.xyw = (VD_COL(unity_ObjectToWorld, 0).xyz) * r1.xxx + r2.xyz;
  r1.xyz = (VD_COL(unity_ObjectToWorld, 2).xyz) * r1.zzz + r1.xyw;
  r1.xyz = (VD_COL(unity_ObjectToWorld, 3).xyz) + r1.xyz;
  r2.xy = -_PlaneBoundMaxPos.xz + r1.xz;
  o4.xyz = r1.xyz;
  r1.xy = -_PlaneBoundMaxPos.xz + _PlaneBoundMinPos.xz;
  r1.zw = saturate(r2.xy / r1.xy);
  r2.xyzw = _PlaneSightRenderTarget.SampleLevel(sampler_PlaneSightRenderTarget, r1.zw, 0).xyzw;
  r1.zw = (VD_COL(unity_ObjectToWorld, 3).xz) + -_PlaneBoundMaxPos.xz;
  r1.xy = saturate(r1.zw / r1.xy);
  r3.xyzw = _PlaneSightRenderTarget.SampleLevel(sampler_PlaneSightRenderTarget, r1.xy, 0).xyzw;
  r1.xyzw = _PlanePointBuffer.SampleLevel(sampler_PlanePointBuffer, r1.xy, 0).xyzw;
  r1.x = saturate(4 * r1.x);
  r1.y = max(r3.w, r2.w);
  r1.z = r1.x * -2 + 3;
  r1.x = r1.x * r1.x;
  r1.x = r1.z * r1.x;
  r1.x = _GlobalPlanePointBufferEnable * r1.x;
  r1.x = max(r1.y, r1.x);
  r1.x = -1 + r1.x;
  r1.y = _GlobalPlaneSystemEnable * _GlobalIsRuntime;
  r1.x = r1.y * r1.x + 1;
  p2.x = max(0.5, r1.x);
  r1.x = (VD_COL(unity_WorldToObject, 0).y);
  r1.y = (VD_COL(unity_WorldToObject, 1).y);
  r1.z = (VD_COL(unity_WorldToObject, 2).y);
  r1.w = dot(r1.xyz, r1.xyz);
  r1.w = max(1.17549435e-038, r1.w);
  r1.w = rsqrt(r1.w);
  o2.xyz = r1.xyz * r1.www;
  o5.xyz = float3(1,0,0);
  o6.xyz = float3(0,1,0);
  r0.y = _ProjectionParams.x * r0.y;
  r1.xzw = float3(0.5,0.5,0.5) * r0.xwy;
  o7.zw = r0.zw;
  o7.xy = r1.xw + r1.zz;
  r0.x = cmp(0.00100000005 < v4.w);
  r0.yzw = v4.xyz / v4.www;
  r0.xyz = r0.xxx ? r0.yzw : v4.xyz;
  r1.xyz = float3(0.0549999997,0.0549999997,0.0549999997) + r0.xyz;
  r1.xyz = float3(0.947867334,0.947867334,0.947867334) * r1.xyz;
  r1.xyz = log2(abs(r1.xyz));
  r1.xyz = float3(2.4000001,2.4000001,2.4000001) * r1.xyz;
  r1.xyz = exp2(r1.xyz);
  r2.xyz = float3(0.0773993805,0.0773993805,0.0773993805) * r0.xyz;
  r0.xyz = cmp(float3(0.0404499993,0.0404499993,0.0404499993) >= r0.xyz);
  o8.xyz = r0.xyz ? r2.xyz : r1.xyz;
  o8.w = v4.w;
  return;
}
