// LCArt/Character/shader_DE_Character_Normal | pass 0 "ForwardLit" | progVertex | keywords: FOG_LINEAR _CLUSTER_LIGHT_LOOP | blob 21
void vert(
  float4 v0 : POSITION0,
  float3 v1 : NORMAL0,
  float4 v2 : TANGENT0,
  float4 v3 : TEXCOORD0,
  float4 v4 : TEXCOORD1,
  float2 v5 : TEXCOORD2,
  float4 v6 : COLOR0,
  out float4 o0 : SV_POSITION0,
  out float2 o1 : TEXCOORD0,
  out float2 p1 : TEXCOORD2,
  out float3 o2 : TEXCOORD1,
  out float p2 : TEXCOORD5,
  out float3 o3 : TEXCOORD3,
  out float p3 : TEXCOORD9,
  out float4 o4 : TEXCOORD4,
  out float4 o5 : TEXCOORD6,
  out float4 o6 : TEXCOORD7,
  out float4 o7 : TEXCOORD8,
  out float4 o8 : COLOR0,
  out float3 o9 : TEXCOORD10)
{
  float4 r0,r1,r2,r3,r4;
  uint4 bitmask, uiDest;
  float4 fDest;

  r0.x = -0.125 + v0.y;
  r0.x = saturate(2.66666675 * r0.x);
  r0.y = r0.x * -2 + 3;
  r0.x = r0.x * r0.x;
  r0.x = r0.y * r0.x;
  r0.y = _Time.y + v0.x;
  r0.y = 123.339996 * r0.y;
  r0.y = frac(r0.y);
  r0.x = r0.y * r0.x;
  r0.x = _LightEffectIntensity * r0.x;
  r0.x = r0.x * 0.25 + v0.y;
  r0.y = cmp(r0.x >= _OpticalRatioYThreshold);
  r0.y = r0.y ? 1.000000 : 0;
  r0.z = -1 + _OpticalRatioYPositiveRatio;
  r0.y = r0.y * r0.z + 1;
  r0.z = cmp(r0.x >= 0);
  r0.z = r0.z ? 1.000000 : 0;
  r0.w = 1 / (VD_COL(unity_MatrixV, 1).y);
  r1.y = min(1.5, abs(r0.w));
  r1.xz = float2(0,0);
  r2.x = (VD_COL(unity_MatrixV, 0).y);
  r2.y = (VD_COL(unity_MatrixV, 1).y);
  r2.z = (VD_COL(unity_MatrixV, 2).y);
  r1.xyz = -r2.xyz + r1.xyz;
  r1.xyz = r0.zzz * r1.xyz + r2.xyz;
  r2.xyz = (VD_COL(unity_ObjectToWorld, 3).xyz) + -_WorldSpaceCameraPos.xyz;
  r0.z = dot(r2.xyz, r2.xyz);
  r0.z = rsqrt(r0.z);
  r2.xyz = r2.xyz * r0.zzz;
  r2.w = -r2.x;
  r0.zw = v0.xx * r2.zw;
  r3.xz = float2(1.17499995,1.17499995) * r0.zw;
  r3.y = 0;
  r0.xzw = r1.xyz * r0.xxx + r3.xyz;
  r1.xyz = v0.yyy * float3(0,1,0) + r3.xyz;
  r1.xyz = r2.xyz * float3(0.200000003,0.200000003,0.200000003) + r1.xyz;
  r3.xyz = v0.zzz * r2.xyz;
  r0.xyz = r3.xyz * r0.yyy + r0.xzw;
  r0.w = max(0, -r0.y);
  r0.w = _OpticalRatioYNegativeUnit * r0.w;
  r0.xyz = -r2.xyz * r0.www + r0.xyz;
  r0.w = cmp(_GhostEnable >= 0.00100000005);
  r0.w = r0.w ? 0 : 1;
  r0.xyz = r0.www * r0.xyz;
  r2.xyz = (VD_COL(unity_ObjectToWorld, 1).xyz) * r0.yyy;
  r2.xyz = (VD_COL(unity_ObjectToWorld, 0).xyz) * r0.xxx + r2.xyz;
  r0.xyz = (VD_COL(unity_ObjectToWorld, 2).xyz) * r0.zzz + r2.xyz;
  r0.xyz = (VD_COL(unity_ObjectToWorld, 3).xyz) + r0.xyz;
  r2.xyzw = (VD_COL(unity_MatrixVP, 1).xyzw) * r0.yyyy;
  r2.xyzw = (VD_COL(unity_MatrixVP, 0).xyzw) * r0.xxxx + r2.xyzw;
  r2.xyzw = (VD_COL(unity_MatrixVP, 2).xyzw) * r0.zzzz + r2.xyzw;
  o3.xyz = r0.xyz;
  r2.xyzw = (VD_COL(unity_MatrixVP, 3).xyzw) + r2.xyzw;
  o0.xyzw = r2.xyzw;
  o1.xy = v3.xy * _MainTex_ST.xy + _MainTex_ST.zw;
  p1.xy = v3.xy * _NormalTex_ST.xy + _NormalTex_ST.zw;
  r0.xy = (VD_COL(unity_ObjectToWorld, 3).xz) + -_PlaneBoundMaxPos.xz;
  r3.xy = -_PlaneBoundMaxPos.xz + _PlaneBoundMinPos.xz;
  r0.xy = saturate(r0.xy / r3.xy);
  r3.xyzw = _PlanePointBuffer.SampleLevel(sampler_PlanePointBuffer, r0.xy, 0).xyzw;
  r4.xyzw = _PlaneSightRenderTarget.SampleLevel(sampler_PlaneSightRenderTarget, r0.xy, 0).xyzw;
  r0.x = saturate(4 * r3.x);
  r0.y = r0.x * -2 + 3;
  r0.x = r0.x * r0.x;
  r0.x = r0.y * r0.x;
  r0.x = _GlobalPlanePointBufferEnable * r0.x;
  r0.x = max(r4.w, r0.x);
  r0.x = -1 + r0.x;
  r0.y = _GlobalPlaneSystemEnable * _GlobalIsRuntime;
  p2.x = r0.y * r0.x + 1;
  r0.x = dot(v1.xyz, (VD_COL(unity_WorldToObject, 0).xyz));
  r0.y = dot(v1.xyz, (VD_COL(unity_WorldToObject, 1).xyz));
  r0.z = dot(v1.xyz, (VD_COL(unity_WorldToObject, 2).xyz));
  r1.w = dot(r0.xyz, r0.xyz);
  r1.w = max(1.17549435e-038, r1.w);
  r1.w = rsqrt(r1.w);
  o2.xyz = r1.www * r0.xyz;
  p3.x = v0.y;
  r0.xyz = (VD_COL(unity_ObjectToWorld, 1).xyz) * r1.yyy;
  r0.xyz = (VD_COL(unity_ObjectToWorld, 0).xyz) * r1.xxx + r0.xyz;
  r0.xyz = (VD_COL(unity_ObjectToWorld, 2).xyz) * r1.zzz + r0.xyz;
  r0.xyz = (VD_COL(unity_ObjectToWorld, 3).xyz) + r0.xyz;
  o4.xyz = r0.www * r0.xyz;
  o5.xyz = float3(1,0,0);
  o6.xyz = float3(0,1,0);
  r0.x = _ProjectionParams.x * r2.y;
  r0.w = 0.5 * r0.x;
  r0.xz = float2(0.5,0.5) * r2.xw;
  o7.zw = r2.zw;
  o7.xy = r0.xw + r0.zz;
  r0.x = cmp(0.00100000005 < v6.w);
  r0.yzw = v6.xyz / v6.www;
  r0.xyz = r0.xxx ? r0.yzw : v6.xyz;
  r1.xyz = float3(0.0549999997,0.0549999997,0.0549999997) + r0.xyz;
  r1.xyz = float3(0.947867334,0.947867334,0.947867334) * r1.xyz;
  r1.xyz = log2(abs(r1.xyz));
  r1.xyz = float3(2.4000001,2.4000001,2.4000001) * r1.xyz;
  r1.xyz = exp2(r1.xyz);
  r2.xyz = float3(0.0773993805,0.0773993805,0.0773993805) * r0.xyz;
  r0.xyz = cmp(float3(0.0404499993,0.0404499993,0.0404499993) >= r0.xyz);
  o8.xyz = r0.xyz ? r2.xyz : r1.xyz;
  o8.w = v6.w;
  r0.xy = float2(0.0549999997,0.0549999997) + v4.xy;
  r0.xy = float2(0.947867334,0.947867334) * r0.xy;
  r0.xy = log2(abs(r0.xy));
  r0.xy = float2(2.4000001,2.4000001) * r0.xy;
  r0.xy = exp2(r0.xy);
  r0.zw = float2(0.0773993805,0.0773993805) * v4.xy;
  r1.xy = cmp(float2(0.0404499993,0.0404499993) >= v4.xy);
  o9.xy = r1.xy ? r0.zw : r0.xy;
  r0.x = 0.0549999997 + v5.x;
  r0.x = 0.947867334 * r0.x;
  r0.x = log2(abs(r0.x));
  r0.x = 2.4000001 * r0.x;
  r0.x = exp2(r0.x);
  r0.y = 0.0773993805 * v5.x;
  r0.z = cmp(0.0404499993 >= v5.x);
  o9.z = r0.z ? r0.y : r0.x;
  return;
}
