// LCArt/Character/Shader_DE_Character_Highlight | pass 1 "Silhouette" | progVertex | keywords: (none) | blob 3
void vert(
  float4 v0 : POSITION0,
  float3 v1 : NORMAL0,
  float2 v2 : TEXCOORD0,
  float4 v3 : COLOR0,
  out float4 o0 : SV_POSITION0,
  out float2 o1 : TEXCOORD0,
  out float p1 : TEXCOORD1,
  out float p1 : TEXCOORD3,
  out float4 o2 : TEXCOORD2)
{
  float4 r0,r1,r2,r3;
  uint4 bitmask, uiDest;
  float4 fDest;

  r0.x = 1 / (VD_COL(unity_MatrixV, 1).y);
  r0.y = min(1.5, abs(r0.x));
  r0.xz = float2(0,0);
  r1.x = (VD_COL(unity_MatrixV, 0).y);
  r1.y = (VD_COL(unity_MatrixV, 1).y);
  r1.z = (VD_COL(unity_MatrixV, 2).y);
  r0.xyz = -r1.xyz + r0.xyz;
  r2.xyz = _HighlightSilhouetteScale.xyz * v0.xyz;
  r0.w = cmp(r2.y >= 0);
  r0.w = r0.w ? 1.000000 : 0;
  r0.xyz = r0.www * r0.xyz + r1.xyz;
  r1.y = 0;
  r3.xyz = (VD_COL(unity_ObjectToWorld, 3).xyz) + -_WorldSpaceCameraPos.xyz;
  r0.w = dot(r3.xyz, r3.xyz);
  r0.w = rsqrt(r0.w);
  r3.xyz = r3.xyz * r0.www;
  r3.w = -r3.x;
  r2.xw = r3.zw * r2.xx;
  r1.xz = float2(1.17499995,1.17499995) * r2.xw;
  r0.xyz = r0.xyz * r2.yyy + r1.xyz;
  r0.w = cmp(r2.y >= _OpticalRatioYThreshold);
  r1.xyz = r3.xyz * r2.zzz;
  r0.w = r0.w ? 1.000000 : 0;
  r1.w = -1 + _OpticalRatioYPositiveRatio;
  r0.w = r0.w * r1.w + 1;
  r0.xyz = r1.xyz * r0.www + r0.xyz;
  r0.w = max(0, -r0.y);
  r0.w = _OpticalRatioYNegativeUnit * r0.w;
  r0.xyz = -r3.xyz * r0.www + r0.xyz;
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
  r1.y = _GlobalIsRuntime * _GlobalPlaneSystemEnable;
  r1.x = r1.y * r1.x + 1;
  p1.x = _FakeShadowVisibility * r1.x;
  o1.xy = v2.xy * _MainTex_ST.xy + _MainTex_ST.zw;
  p1.x = v3.w;
  r0.y = _ProjectionParams.x * r0.y;
  r1.xzw = float3(0.5,0.5,0.5) * r0.xwy;
  o2.zw = r0.zw;
  o2.xy = r1.xw + r1.zz;
  return;
}
