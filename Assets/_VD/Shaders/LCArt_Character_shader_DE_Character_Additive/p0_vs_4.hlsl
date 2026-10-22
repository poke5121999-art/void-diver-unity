// LCArt/Character/shader_DE_Character_Additive | pass 0 "Additive" | progVertex | keywords: _DE_EDITOR_CAMERA | blob 4
void vert(
  float4 v0 : POSITION0,
  float3 v1 : NORMAL0,
  float2 v2 : TEXCOORD0,
  float4 v3 : COLOR0,
  out float4 o0 : SV_POSITION0,
  out float2 o1 : TEXCOORD0,
  out float p1 : TEXCOORD2,
  out float4 o2 : TEXCOORD1,
  out float4 o3 : TEXCOORD6,
  out float4 o4 : TEXCOORD3,
  out float3 o5 : TEXCOORD4,
  out float4 o6 : COLOR0)
{
  float4 r0,r1,r2,r3,r4;
  uint4 bitmask, uiDest;
  float4 fDest;

  r0.x = cmp(v0.y >= 0);
  r0.x = r0.x ? 1.000000 : 0;
  r0.y = 1 / (VD_COL(unity_MatrixV, 1).y);
  r1.y = min(1.5, abs(r0.y));
  r1.xz = float2(0,0);
  r2.x = (VD_COL(unity_MatrixV, 0).y);
  r2.y = (VD_COL(unity_MatrixV, 1).y);
  r2.z = (VD_COL(unity_MatrixV, 2).y);
  r0.yzw = -r2.xyz + r1.xyz;
  r0.xyz = r0.xxx * r0.yzw + r2.xyz;
  r1.xyz = (VD_COL(unity_ObjectToWorld, 3).xyz) + -_WorldSpaceCameraPos.xyz;
  r0.w = dot(r1.xyz, r1.xyz);
  r0.w = rsqrt(r0.w);
  r1.xyz = r1.xyz * r0.www;
  r1.w = -r1.x;
  r2.xy = v0.xx * r1.zw;
  r2.xz = float2(1.17499995,1.17499995) * r2.xy;
  r2.y = 0;
  r0.xyz = r0.xyz * v0.yyy + r2.xyz;
  r0.w = cmp(v0.y >= _OpticalRatioYThreshold);
  r0.w = r0.w ? 1.000000 : 0;
  r1.w = -1 + _OpticalRatioYPositiveRatio;
  r0.w = r0.w * r1.w + 1;
  r2.xyz = v0.zzz * r1.xyz;
  r0.xyz = r2.xyz * r0.www + r0.xyz;
  r0.w = max(0, -r0.y);
  r0.w = _OpticalRatioYNegativeUnit * r0.w;
  r0.xyz = -r1.xyz * r0.www + r0.xyz;
  r1.xyz = (VD_COL(unity_ObjectToWorld, 1).xyz) * r0.yyy;
  r0.xyw = (VD_COL(unity_ObjectToWorld, 0).xyz) * r0.xxx + r1.xyz;
  r0.xyz = (VD_COL(unity_ObjectToWorld, 2).xyz) * r0.zzz + r0.xyw;
  r0.xyz = (VD_COL(unity_ObjectToWorld, 3).xyz) + r0.xyz;
  r1.xyzw = (VD_COL(unity_MatrixVP, 1).xyzw) * r0.yyyy;
  r1.xyzw = (VD_COL(unity_MatrixVP, 0).xyzw) * r0.xxxx + r1.xyzw;
  r1.xyzw = (VD_COL(unity_MatrixVP, 2).xyzw) * r0.zzzz + r1.xyzw;
  r1.xyzw = (VD_COL(unity_MatrixVP, 3).xyzw) + r1.xyzw;
  o0.xyzw = r1.xyzw;
  r2.xy = -_PlaneBoundMaxPos.xz + r0.xz;
  r2.zw = -_PlaneBoundMaxPos.xz + _PlaneBoundMinPos.xz;
  r2.xy = saturate(r2.xy / r2.zw);
  r3.xyzw = _PlaneSightRenderTarget.SampleLevel(sampler_PlaneSightRenderTarget, r2.xy, 0).xyzw;
  r2.xy = (VD_COL(unity_ObjectToWorld, 3).xz) + -_PlaneBoundMaxPos.xz;
  r2.xy = saturate(r2.xy / r2.zw);
  r4.xyzw = _PlaneSightRenderTarget.SampleLevel(sampler_PlaneSightRenderTarget, r2.xy, 0).xyzw;
  r2.xyzw = _PlanePointBuffer.SampleLevel(sampler_PlanePointBuffer, r2.xy, 0).xyzw;
  r0.w = saturate(4 * r2.x);
  r2.x = max(r4.w, r3.w);
  r2.y = r0.w * -2 + 3;
  r0.w = r0.w * r0.w;
  r0.w = r2.y * r0.w;
  r0.w = _GlobalPlanePointBufferEnable * r0.w;
  r0.w = max(r2.x, r0.w);
  r0.w = -1 + r0.w;
  r0.w = _GlobalPlaneSystemEnable * r0.w + 1;
  p1.x = max(0.5, r0.w);
  o1.xy = v2.xy * _MainTex_ST.xy + _MainTex_ST.zw;
  o2.xyz = r0.xyz;
  o5.xyz = r0.xyz;
  r0.x = (VD_COL(unity_WorldToObject, 0).y);
  r0.y = (VD_COL(unity_WorldToObject, 1).y);
  r0.z = (VD_COL(unity_WorldToObject, 2).y);
  r0.w = dot(r0.xyz, r0.xyz);
  r0.w = max(1.17549435e-038, r0.w);
  r0.w = rsqrt(r0.w);
  r0.xyz = r0.xyz * r0.www;
  r0.w = dot(r0.xyz, r0.xyz);
  r0.w = rsqrt(r0.w);
  o3.xyz = r0.xyz * r0.www;
  r0.x = _ProjectionParams.x * r1.y;
  r0.w = 0.5 * r0.x;
  r0.xz = float2(0.5,0.5) * r1.xw;
  o4.zw = r1.zw;
  o4.xy = r0.xw + r0.zz;
  r0.x = cmp(0.00100000005 < v3.w);
  r0.yzw = v3.xyz / v3.www;
  r0.xyz = r0.xxx ? r0.yzw : v3.xyz;
  r1.xyz = float3(0.0549999997,0.0549999997,0.0549999997) + r0.xyz;
  r1.xyz = float3(0.947867334,0.947867334,0.947867334) * r1.xyz;
  r1.xyz = log2(abs(r1.xyz));
  r1.xyz = float3(2.4000001,2.4000001,2.4000001) * r1.xyz;
  r1.xyz = exp2(r1.xyz);
  r2.xyz = float3(0.0773993805,0.0773993805,0.0773993805) * r0.xyz;
  r0.xyz = cmp(float3(0.0404499993,0.0404499993,0.0404499993) >= r0.xyz);
  r0.xyz = r0.xyz ? r2.xyz : r1.xyz;
  r1.x = cmp(0.5 < _UseLinearVertexColor);
  r0.w = v3.w;
  o6.xyzw = r1.xxxx ? r0.xyzw : v3.xyzw;
  return;
}
