// LCArt/Character/shader_DE_Character_Normal | pass 2 "DepthNormals" | progVertex | keywords: (none) | blob 71
void vert(
  float4 v0 : POSITION0,
  float2 v1 : TEXCOORD0,
  float4 v2 : COLOR0,
  out float4 o0 : SV_POSITION0,
  out float2 o1 : TEXCOORD0,
  out float p1 : TEXCOORD1,
  out float4 o2 : TEXCOORD2,
  out float4 o3 : COLOR0)
{
  float4 r0,r1,r2;
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
  r0.xyzw = (VD_COL(unity_MatrixVP, 2).xyzw) * r0.zzzz + r1.xyzw;
  r0.xyzw = (VD_COL(unity_MatrixVP, 3).xyzw) + r0.xyzw;
  o0.xyzw = r0.xyzw;
  r1.xy = (VD_COL(unity_ObjectToWorld, 3).xz) + -_PlaneBoundMaxPos.xz;
  r1.zw = -_PlaneBoundMaxPos.xz + _PlaneBoundMinPos.xz;
  r1.xy = saturate(r1.xy / r1.zw);
  r2.xyzw = _PlanePointBuffer.SampleLevel(sampler_PlanePointBuffer, r1.xy, 0).xyzw;
  r1.xyzw = _PlaneSightRenderTarget.SampleLevel(sampler_PlaneSightRenderTarget, r1.xy, 0).xyzw;
  r1.x = saturate(4 * r2.x);
  r1.y = r1.x * -2 + 3;
  r1.x = r1.x * r1.x;
  r1.x = r1.y * r1.x;
  r1.x = _GlobalPlanePointBufferEnable * r1.x;
  r1.x = max(r1.w, r1.x);
  r1.x = -1 + r1.x;
  r1.y = _GlobalPlaneSystemEnable * _GlobalIsRuntime;
  r1.x = r1.y * r1.x + 1;
  p1.x = r1.x;
  r1.x = cmp(r1.x >= 0.850000024);
  r1.x = r1.x ? 1.000000 : 0;
  o1.xy = v1.xy * _MainTex_ST.xy + _MainTex_ST.zw;
  r1.y = _ProjectionParams.x * r0.y;
  r2.w = 0.5 * r1.y;
  r2.xz = float2(0.5,0.5) * r0.xw;
  r0.xy = r2.xw + r2.zz;
  r0.xyzw = float4(-0,-0,-0,-1) + r0.xyzw;
  o2.xyzw = r1.xxxx * r0.xyzw + float4(0,0,0,1);
  o3.xyzw = v2.xyzw;
  return;
}
