// LCArt/Character/shader_DE_Character_Normal | pass 4 "StencilColor" | progVertex | keywords: FOG_LINEAR | blob 108
void vert(
  float4 v0 : POSITION0,
  float2 v1 : TEXCOORD0,
  float4 v2 : COLOR0,
  out float4 o0 : SV_POSITION0,
  out float2 o1 : TEXCOORD0,
  out float2 p1 : TEXCOORD2,
  out float3 o2 : TEXCOORD3,
  out float p2 : TEXCOORD5,
  out float4 o3 : TEXCOORD4,
  out float4 o4 : TEXCOORD8,
  out float4 o5 : COLOR0)
{
  float4 r0,r1,r2,r3;
  uint4 bitmask, uiDest;
  float4 fDest;

  r0.x = cmp(v0.y >= _OpticalRatioYThreshold);
  r0.x = r0.x ? 1.000000 : 0;
  r0.y = -1 + _OpticalRatioYPositiveRatio;
  r0.x = r0.x * r0.y + 1;
  r0.y = cmp(v0.y >= 0);
  r0.y = r0.y ? 1.000000 : 0;
  r0.z = 1 / (VD_COL(unity_MatrixV, 1).y);
  r1.y = min(1.5, abs(r0.z));
  r1.xz = float2(0,0);
  r2.x = (VD_COL(unity_MatrixV, 0).y);
  r2.y = (VD_COL(unity_MatrixV, 1).y);
  r2.z = (VD_COL(unity_MatrixV, 2).y);
  r1.xyz = -r2.xyz + r1.xyz;
  r0.yzw = r0.yyy * r1.xyz + r2.xyz;
  r1.xyz = (VD_COL(unity_ObjectToWorld, 3).xyz) + -_WorldSpaceCameraPos.xyz;
  r1.w = dot(r1.xyz, r1.xyz);
  r1.w = rsqrt(r1.w);
  r1.xyz = r1.xyz * r1.www;
  r1.w = -r1.x;
  r2.xy = v0.xx * r1.zw;
  r2.xz = float2(1.17499995,1.17499995) * r2.xy;
  r2.y = 0;
  r0.yzw = r0.yzw * v0.yyy + r2.xyz;
  r2.xyz = v0.yyy * float3(0,1,0) + r2.xyz;
  r2.xyz = r1.xyz * float3(0.200000003,0.200000003,0.200000003) + r2.xyz;
  r3.xyz = v0.zzz * r1.xyz;
  r0.xyz = r3.xyz * r0.xxx + r0.yzw;
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
  o2.xyz = r0.xyz;
  r0.xyzw = (VD_COL(unity_MatrixVP, 3).xyzw) + r1.xyzw;
  o0.xyzw = r0.xyzw;
  o1.xy = v1.xy * _MainTex_ST.xy + _MainTex_ST.zw;
  p1.xy = v1.xy * _NormalTex_ST.xy + _NormalTex_ST.zw;
  r1.xy = (VD_COL(unity_ObjectToWorld, 3).xz) + -_PlaneBoundMaxPos.xz;
  r1.zw = -_PlaneBoundMaxPos.xz + _PlaneBoundMinPos.xz;
  r1.xy = saturate(r1.xy / r1.zw);
  r3.xyzw = _PlanePointBuffer.SampleLevel(sampler_PlanePointBuffer, r1.xy, 0).xyzw;
  r1.xyzw = _PlaneSightRenderTarget.SampleLevel(sampler_PlaneSightRenderTarget, r1.xy, 0).xyzw;
  r1.x = saturate(4 * r3.x);
  r1.y = r1.x * -2 + 3;
  r1.x = r1.x * r1.x;
  r1.x = r1.y * r1.x;
  r1.x = _GlobalPlanePointBufferEnable * r1.x;
  r1.x = max(r1.w, r1.x);
  r1.x = -1 + r1.x;
  r1.y = _GlobalPlaneSystemEnable * _GlobalIsRuntime;
  p2.x = r1.y * r1.x + 1;
  r1.xyz = (VD_COL(unity_ObjectToWorld, 1).xyz) * r2.yyy;
  r1.xyz = (VD_COL(unity_ObjectToWorld, 0).xyz) * r2.xxx + r1.xyz;
  r1.xyz = (VD_COL(unity_ObjectToWorld, 2).xyz) * r2.zzz + r1.xyz;
  r1.xyz = (VD_COL(unity_ObjectToWorld, 3).xyz) + r1.xyz;
  r1.w = cmp(_GhostEnable >= 0.00100000005);
  r1.w = r1.w ? 0 : 1;
  o3.xyz = r1.www * r1.xyz;
  r0.y = _ProjectionParams.x * r0.y;
  r1.xzw = float3(0.5,0.5,0.5) * r0.xwy;
  o4.zw = r0.zw;
  o4.xy = r1.xw + r1.zz;
  r0.x = cmp(0.00100000005 < v2.w);
  r0.yzw = v2.xyz / v2.www;
  r0.xyz = r0.xxx ? r0.yzw : v2.xyz;
  r1.xyz = float3(0.0549999997,0.0549999997,0.0549999997) + r0.xyz;
  r1.xyz = float3(0.947867334,0.947867334,0.947867334) * r1.xyz;
  r1.xyz = log2(abs(r1.xyz));
  r1.xyz = float3(2.4000001,2.4000001,2.4000001) * r1.xyz;
  r1.xyz = exp2(r1.xyz);
  r2.xyz = float3(0.0773993805,0.0773993805,0.0773993805) * r0.xyz;
  r0.xyz = cmp(float3(0.0404499993,0.0404499993,0.0404499993) >= r0.xyz);
  o5.xyz = r0.xyz ? r2.xyz : r1.xyz;
  o5.w = v2.w;
  return;
}
