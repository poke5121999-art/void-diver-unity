// LCArt/Character/shader_DE_Character_Normal | pass 1 "SpineGrabpass" | progVertex | keywords: _DE_PRERENDERING_CAMERA | blob 61
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
  out float p2 : TEXCOORD5,
  out float4 o3 : TEXCOORD3,
  out float4 o4 : TEXCOORD4,
  out float4 o5 : TEXCOORD6,
  out float4 o6 : TEXCOORD7,
  out float4 o7 : TEXCOORD8,
  out float4 o8 : COLOR0,
  out float4 o9 : TEXCOORD9,
  out float2 o10 : TEXCOORD10,
  out float p10 : TEXCOORD11)
{
  float4 r0,r1,r2,r3,r4,r5,r6,r7;
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
  r0.w = cmp(r0.x >= 0);
  r0.w = r0.w ? 1.000000 : 0;
  r1.x = 1 / (VD_COL(unity_MatrixV, 1).y);
  r1.y = min(1.5, abs(r1.x));
  r1.xz = float2(0,0);
  r2.x = (VD_COL(unity_MatrixV, 0).y);
  r2.y = (VD_COL(unity_MatrixV, 1).y);
  r2.z = (VD_COL(unity_MatrixV, 2).y);
  r1.xyz = -r2.xyz + r1.xyz;
  r3.xyz = r0.www * r1.xyz + r2.xyz;
  r4.xyz = (VD_COL(unity_ObjectToWorld, 3).xyz) + -_WorldSpaceCameraPos.xyz;
  r0.w = dot(r4.xyz, r4.xyz);
  r0.w = rsqrt(r0.w);
  r4.xyz = r4.xyz * r0.www;
  r4.w = -r4.x;
  r5.xy = v0.xx * r4.zw;
  r5.xz = float2(1.17499995,1.17499995) * r5.xy;
  r5.y = 0;
  r3.xyz = r3.xyz * r0.xxx + r5.xyz;
  r6.xyz = v0.zzz * r4.xyz;
  r0.xyw = r6.xyz * r0.yyy + r3.xyz;
  r1.w = max(0, -r0.y);
  r1.w = _OpticalRatioYNegativeUnit * r1.w;
  r0.xyw = -r4.xyz * r1.www + r0.xyw;
  r1.w = cmp(_GhostEnable >= 0.00100000005);
  r1.w = r1.w ? 1.000000 : 0;
  r0.xyw = r1.www * r0.xyw;
  r3.xyz = (VD_COL(unity_ObjectToWorld, 1).xyz) * r0.yyy;
  r3.xyz = (VD_COL(unity_ObjectToWorld, 0).xyz) * r0.xxx + r3.xyz;
  r3.xyz = (VD_COL(unity_ObjectToWorld, 2).xyz) * r0.www + r3.xyz;
  r3.xyz = (VD_COL(unity_ObjectToWorld, 3).xyz) + r3.xyz;
  r7.xyzw = (VD_COL(unity_MatrixVP, 1).xyzw) * r3.yyyy;
  r7.xyzw = (VD_COL(unity_MatrixVP, 0).xyzw) * r3.xxxx + r7.xyzw;
  r7.xyzw = (VD_COL(unity_MatrixVP, 2).xyzw) * r3.zzzz + r7.xyzw;
  o3.xyz = r3.xyz;
  r3.xyzw = (VD_COL(unity_MatrixVP, 3).xyzw) + r7.xyzw;
  o0.xyzw = r3.xyzw;
  o1.xy = v3.xy * _MainTex_ST.xy + _MainTex_ST.zw;
  p1.xy = v3.xy * _NormalTex_ST.xy + _NormalTex_ST.zw;
  r7.x = (VD_COL(unity_WorldToObject, 0).y);
  r7.y = (VD_COL(unity_WorldToObject, 1).y);
  r7.z = (VD_COL(unity_WorldToObject, 2).y);
  r2.w = dot(r7.xyz, r7.xyz);
  r2.w = max(1.17549435e-038, r2.w);
  r2.w = rsqrt(r2.w);
  r7.xyz = r7.xyz * r2.www;
  o2.xyz = r7.xyz;
  o9.xyz = r7.xyz;
  p2.x = 1;
  r2.w = cmp(v0.y >= 0);
  r2.w = r2.w ? 1.000000 : 0;
  r1.xyz = r2.www * r1.xyz + r2.xyz;
  r1.xyz = r1.xyz * v0.yyy + r5.xyz;
  r2.x = cmp(v0.y >= _OpticalRatioYThreshold);
  r2.x = r2.x ? 1.000000 : 0;
  r0.z = r2.x * r0.z + 1;
  r1.xyz = r6.xyz * r0.zzz + r1.xyz;
  r0.z = max(0, -r1.y);
  r0.z = _OpticalRatioYNegativeUnit * r0.z;
  r1.xyz = -r4.xyz * r0.zzz + r1.xyz;
  r2.xyz = (VD_COL(unity_ObjectToWorld, 1).xyz) * r1.yyy;
  r2.xyz = (VD_COL(unity_ObjectToWorld, 0).xyz) * r1.xxx + r2.xyz;
  r1.xyz = (VD_COL(unity_ObjectToWorld, 2).xyz) * r1.zzz + r2.xyz;
  r1.xyz = (VD_COL(unity_ObjectToWorld, 3).xyz) + r1.xyz;
  o4.xyz = r1.www * r1.xyz;
  o5.xyz = float3(1,0,0);
  o6.xyz = float3(0,1,0);
  r0.z = _ProjectionParams.x * r3.y;
  r1.w = 0.5 * r0.z;
  r1.xz = float2(0.5,0.5) * r3.xw;
  o7.zw = r3.zw;
  o7.xy = r1.xw + r1.zz;
  r0.z = cmp(0.00100000005 < v4.w);
  r1.xyz = v4.xyz / v4.www;
  r1.xyz = r0.zzz ? r1.xyz : v4.xyz;
  r2.xyz = float3(0.0549999997,0.0549999997,0.0549999997) + r1.xyz;
  r2.xyz = float3(0.947867334,0.947867334,0.947867334) * r2.xyz;
  r2.xyz = log2(abs(r2.xyz));
  r2.xyz = float3(2.4000001,2.4000001,2.4000001) * r2.xyz;
  r2.xyz = exp2(r2.xyz);
  r3.xyz = float3(0.0773993805,0.0773993805,0.0773993805) * r1.xyz;
  r1.xyz = cmp(float3(0.0404499993,0.0404499993,0.0404499993) >= r1.xyz);
  o8.xyz = r1.xyz ? r3.xyz : r2.xyz;
  o8.w = v4.w;
  r1.xyzw = (VD_COL(unity_ObjectToWorld, 1).xyzw) * r0.yyyy;
  r1.xyzw = (VD_COL(unity_ObjectToWorld, 0).xyzw) * r0.xxxx + r1.xyzw;
  r0.xyzw = (VD_COL(unity_ObjectToWorld, 2).xyzw) * r0.wwww + r1.xyzw;
  r0.xyzw = (VD_COL(unity_ObjectToWorld, 3).xyzw) + r0.xyzw;
  r1.xyz = (VD_COL(unity_MatrixVP, 1).xyw) * r0.yyy;
  r1.xyz = (VD_COL(unity_MatrixVP, 0).xyw) * r0.xxx + r1.xyz;
  r0.xyz = (VD_COL(unity_MatrixVP, 2).xyw) * r0.zzz + r1.xyz;
  r0.xyz = (VD_COL(unity_MatrixVP, 3).xyw) * r0.www + r0.xyz;
  r0.xy = r0.xy / -r0.zz;
  r0.xy = r0.xy * float2(0.5,0.5) + float2(0.5,0.5);
  r0.z = 1 + -r0.x;
  r0.w = v0.y;
  o10.xyz = r0.zyw;
  return;
}
