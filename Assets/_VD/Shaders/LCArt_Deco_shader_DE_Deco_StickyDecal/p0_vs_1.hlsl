// LCArt/Deco/shader_DE_Deco_StickyDecal | pass 0 "FORWARD" | progVertex | keywords: (none) | blob 1
void vert(
  float4 v0 : POSITION0,
  float3 v1 : NORMAL0,
  float2 v2 : TEXCOORD0,
  out float4 o0 : SV_POSITION0,
  out float4 o1 : TEXCOORD0,
  out float4 o2 : TEXCOORD1,
  out float4 o3 : TEXCOORD2,
  out float4 o4 : TEXCOORD3,
  out float4 o5 : TEXCOORD4,
  out float4 o6 : TEXCOORD5,
  out float4 o7 : TEXCOORD6)
{
  float4 r0,r1,r2,r3;
  uint4 bitmask, uiDest;
  float4 fDest;

  r0.xz = _DecalOffset.xz * v0.yy;
  r0.y = 0;
  r0.xyz = v0.xyz + r0.xyz;
  r1.xyz = (VD_COL(unity_ObjectToWorld, 1).xyz) * r0.yyy;
  r0.xyw = (VD_COL(unity_ObjectToWorld, 0).xyz) * r0.xxx + r1.xyz;
  r0.xyz = (VD_COL(unity_ObjectToWorld, 2).xyz) * r0.zzz + r0.xyw;
  r0.xyz = (VD_COL(unity_ObjectToWorld, 3).xyz) + r0.xyz;
  r1.xyzw = (VD_COL(unity_MatrixVP, 1).xyzw) * r0.yyyy;
  r1.xyzw = (VD_COL(unity_MatrixVP, 0).xyzw) * r0.xxxx + r1.xyzw;
  r1.xyzw = (VD_COL(unity_MatrixVP, 2).xyzw) * r0.zzzz + r1.xyzw;
  r1.xyzw = (VD_COL(unity_MatrixVP, 3).xyzw) + r1.xyzw;
  o0.xyzw = r1.xyzw;
  o1.xy = v2.xy * _MainTex_ST.xy + _MainTex_ST.zw;
  r2.x = dot(v1.xyz, (VD_COL(unity_WorldToObject, 0).xyz));
  r2.y = dot(v1.xyz, (VD_COL(unity_WorldToObject, 1).xyz));
  r2.z = dot(v1.xyz, (VD_COL(unity_WorldToObject, 2).xyz));
  r0.w = dot(r2.xyz, r2.xyz);
  r0.w = max(1.17549435e-038, r0.w);
  r0.w = rsqrt(r0.w);
  o2.xyz = r2.xyz * r0.www;
  o3.xyz = r0.xyz;
  r2.xyzw = (VD_COL(unity_ObjectToWorld, 1).xyzw) * v0.yyyy;
  r2.xyzw = (VD_COL(unity_ObjectToWorld, 0).xyzw) * v0.xxxx + r2.xyzw;
  r2.xyzw = (VD_COL(unity_ObjectToWorld, 2).xyzw) * v0.zzzz + r2.xyzw;
  r2.xyzw = (VD_COL(unity_ObjectToWorld, 3).xyzw) + r2.xyzw;
  r3.xyzw = (VD_COL(unity_MatrixVP, 1).xyzw) * r2.yyyy;
  r3.xyzw = (VD_COL(unity_MatrixVP, 0).xyzw) * r2.xxxx + r3.xyzw;
  r3.xyzw = (VD_COL(unity_MatrixVP, 2).xyzw) * r2.zzzz + r3.xyzw;
  o4.xyzw = (VD_COL(unity_MatrixVP, 3).xyzw) * r2.wwww + r3.xyzw;
  r0.w = _ProjectionParams.x * r1.y;
  r2.w = 0.5 * r0.w;
  r2.xz = float2(0.5,0.5) * r1.xw;
  o5.zw = r1.zw;
  o5.xy = r2.xw + r2.zz;
  o6.xyz = -_WorldSpaceCameraPos.xyz + r0.xyz;
  r0.xy = -_PlaneBoundMaxPos.xz + r0.xz;
  r0.zw = -_PlaneBoundMaxPos.xz + _PlaneBoundMinPos.xz;
  r0.xy = saturate(r0.xy / r0.zw);
  r1.xyzw = _PlaneSightRenderTarget.SampleLevel(sampler_PlaneSightRenderTarget, r0.xy, 0).xyzw;
  r0.xy = (VD_COL(unity_ObjectToWorld, 3).xz) + -_PlaneBoundMaxPos.xz;
  r0.xy = saturate(r0.xy / r0.zw);
  r0.xyzw = _PlaneSightRenderTarget.SampleLevel(sampler_PlaneSightRenderTarget, r0.xy, 0).xyzw;
  o7.xyzw = max(r1.xyzw, r0.xyzw);
  return;
}
