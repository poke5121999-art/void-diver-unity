// LCArt/WorldUI/Shader_DE_WorldUI_Default | pass 0 "Default" | progVertex | keywords: UNITY_UI_ALPHACLIP | blob 2
void vert(
  float4 v0 : POSITION0,
  float4 v1 : COLOR0,
  float2 v2 : TEXCOORD0,
  out float4 o0 : SV_POSITION0,
  out float4 o1 : COLOR0,
  out float2 o2 : TEXCOORD0,
  out float p2 : TEXCOORD2,
  out float4 o3 : TEXCOORD1)
{
  float4 r0,r1,r2;
  uint4 bitmask, uiDest;
  float4 fDest;

  r0.xyzw = (VD_COL(unity_ObjectToWorld, 1).xyzw) * v0.yyyy;
  r0.xyzw = (VD_COL(unity_ObjectToWorld, 0).xyzw) * v0.xxxx + r0.xyzw;
  r0.xyzw = (VD_COL(unity_ObjectToWorld, 2).xyzw) * v0.zzzz + r0.xyzw;
  r0.xyzw = (VD_COL(unity_ObjectToWorld, 3).xyzw) + r0.xyzw;
  r1.xyzw = (VD_COL(unity_MatrixVP, 1).xyzw) * r0.yyyy;
  r1.xyzw = (VD_COL(unity_MatrixVP, 0).xyzw) * r0.xxxx + r1.xyzw;
  r1.xyzw = (VD_COL(unity_MatrixVP, 2).xyzw) * r0.zzzz + r1.xyzw;
  o0.xyzw = (VD_COL(unity_MatrixVP, 3).xyzw) * r0.wwww + r1.xyzw;
  r0.xyz = v1.xyz * float3(0.265884995,0.265884995,0.265884995) + float3(0.736584008,0.736584008,0.736584008);
  r0.xyz = v1.xyz * r0.xyz + float3(-0.00980184041,-0.00980184041,-0.00980184041);
  r0.xyz = v1.xyz * r0.xyz + float3(0.00319697009,0.00319697009,0.00319697009);
  r1.xyz = v1.xyz * float3(0.0849710032,0.0849710032,0.0849710032) + float3(-0.000163029006,-0.000163029006,-0.000163029006);
  r2.xyz = cmp(v1.xyz < float3(0.0725490004,0.0725490004,0.0725490004));
  r0.xyz = r2.xyz ? r1.xyz : r0.xyz;
  r0.xyz = (_UIVertexColorAlwaysGammaSpace).xxx ? r0.xyz : v1.xyz;
  r0.w = v1.w;
  o1.xyzw = _Color.xyzw * r0.xyzw;
  r0.xy = (VD_COL(unity_ObjectToWorld, 3).xz) + -_PlaneBoundMaxPos.xz;
  r0.zw = -_PlaneBoundMaxPos.xz + _PlaneBoundMinPos.xz;
  r0.xy = saturate(r0.xy / r0.zw);
  r0.xyzw = _PlaneSightRenderTarget.SampleLevel(sampler_PlaneSightRenderTarget, r0.xy, 0).xyzw;
  r0.x = max(r0.x, r0.y);
  r0.x = max(r0.x, r0.z);
  r0.x = -1 + r0.x;
  p2.x = _GlobalPlaneSystemEnable * r0.x + 1;
  o2.xy = v2.xy * _MainTex_ST.xy + _MainTex_ST.zw;
  o3.xyzw = v0.xyzw;
  return;
}
