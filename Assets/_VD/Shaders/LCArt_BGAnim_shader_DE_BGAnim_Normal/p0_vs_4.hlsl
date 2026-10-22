// LCArt/BGAnim/shader_DE_BGAnim_Normal | pass 0 "ForwardLit" | progVertex | keywords: _DE_EDITOR_CAMERA | blob 4
void vert(
  float4 v0 : POSITION0,
  float3 v1 : NORMAL0,
  float4 v2 : TANGENT0,
  float2 v3 : TEXCOORD0,
  float4 v4 : COLOR0,
  out float4 o0 : SV_POSITION0,
  out float2 o1 : TEXCOORD0,
  out float2 p1 : TEXCOORD2,
  out float3 o2 : NORMAL0,
  out float p2 : TEXCOORD9,
  out float4 o3 : TEXCOORD3,
  out float4 o4 : TEXCOORD4,
  out float4 o5 : TEXCOORD6,
  out float4 o6 : TEXCOORD7,
  out float4 o7 : TEXCOORD8,
  out float4 o8 : COLOR0)
{
  float4 r0,r1,r2,r3;
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
  r0.w = 1;
  r0.xyzw = -v0.xyzw + r0.xyzw;
  r0.xyzw = (_BillboardEnable).xxxx * r0.xyzw + v0.xyzw;
  r1.xyz = (VD_COL(unity_ObjectToWorld, 1).xyz) * r0.yyy;
  r1.xyz = (VD_COL(unity_ObjectToWorld, 0).xyz) * r0.xxx + r1.xyz;
  r0.xyz = (VD_COL(unity_ObjectToWorld, 2).xyz) * r0.zzz + r1.xyz;
  o4.xyz = (VD_COL(unity_ObjectToWorld, 3).xyz) * r0.www + r0.xyz;
  r0.xyz = (VD_COL(unity_ObjectToWorld, 3).xyz) + r0.xyz;
  r1.xyzw = (VD_COL(unity_MatrixVP, 1).xyzw) * r0.yyyy;
  r1.xyzw = (VD_COL(unity_MatrixVP, 0).xyzw) * r0.xxxx + r1.xyzw;
  r1.xyzw = (VD_COL(unity_MatrixVP, 2).xyzw) * r0.zzzz + r1.xyzw;
  r1.xyzw = (VD_COL(unity_MatrixVP, 3).xyzw) + r1.xyzw;
  o0.xyzw = r1.xyzw;
  o1.xy = v3.xy * _MainTex_ST.xy + _MainTex_ST.zw;
  p1.xy = v3.xy * _NormalTex_ST.xy + _NormalTex_ST.zw;
  r2.xy = -_PlaneBoundMaxPos.xz + r0.xz;
  o3.xyz = r0.xyz;
  r0.xy = -_PlaneBoundMaxPos.xz + _PlaneBoundMinPos.xz;
  r0.zw = saturate(r2.xy / r0.xy);
  r2.xyzw = _PlaneSightRenderTarget.SampleLevel(sampler_PlaneSightRenderTarget, r0.zw, 0).xyzw;
  r3.xyzw = _PlanePointBuffer.SampleLevel(sampler_PlanePointBuffer, r0.zw, 0).xyzw;
  r0.z = saturate(4 * r3.x);
  r2.xy = (VD_COL(unity_ObjectToWorld, 3).xz) + -_PlaneBoundMaxPos.xz;
  r0.xy = saturate(r2.xy / r0.xy);
  r3.xyzw = _PlaneSightRenderTarget.SampleLevel(sampler_PlaneSightRenderTarget, r0.xy, 0).xyzw;
  r0.x = max(r3.w, r2.w);
  r0.y = r0.z * -2 + 3;
  r0.z = r0.z * r0.z;
  r0.y = r0.y * r0.z;
  r0.y = _GlobalPlanePointBufferEnable * r0.y;
  r0.x = max(r0.x, r0.y);
  r0.x = -1 + r0.x;
  r0.y = _SightEffectEnable * _GlobalPlaneSystemEnable;
  r0.y = _GlobalBackgroundSeeThroughEnable * r0.y;
  p2.x = r0.y * r0.x + 1;
  r0.xyz = float3(0,1,0) + v1.xyz;
  r0.xyz = (_BillboardEnable).xxx * r0.xyz + -v1.xyz;
  r2.x = dot(r0.xyz, (VD_COL(unity_WorldToObject, 0).xyz));
  r2.y = dot(r0.xyz, (VD_COL(unity_WorldToObject, 1).xyz));
  r2.z = dot(r0.xyz, (VD_COL(unity_WorldToObject, 2).xyz));
  r0.x = dot(r2.xyz, r2.xyz);
  r0.x = max(1.17549435e-038, r0.x);
  r0.x = rsqrt(r0.x);
  o2.xyz = r2.xyz * r0.xxx;
  o5.xyz = float3(1,0,0);
  o6.xyz = float3(0,1,0);
  r0.x = _ProjectionParams.x * r1.y;
  r0.w = 0.5 * r0.x;
  r0.xz = float2(0.5,0.5) * r1.xw;
  o7.zw = r1.zw;
  o7.xy = r0.xw + r0.zz;
  o8.xyzw = v4.xyzw;
  return;
}
