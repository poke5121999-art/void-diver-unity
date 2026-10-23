// LCArt/WorldUI/Shader_DE_WorldUI_Default | pass 0 "Default" | progVertex | keywords: UNITY_UI_CLIP_RECT | blob 3
void vert(
  float4 v0 : POSITION0,
  float4 v1 : COLOR0,
  float2 v2 : TEXCOORD0,
  out float4 o0 : SV_POSITION0,
  out float4 o1 : VDREG1,
  out float4 o2 : VDREG2,
  out float4 o3 : VDREG3)
{
  uint4 r0, r1, r2;
  r0 = 0u; r1 = 0u; r2 = 0u;

  r0.xyzw = vd_asuint((vd_asfloat(v0.yyyy) * vd_asfloat((VD_COL(unity_ObjectToWorld, 1).xyzw))));
  r0.xyzw = vd_asuint((vd_asfloat((VD_COL(unity_ObjectToWorld, 0).xyzw)) * vd_asfloat(v0.xxxx) + vd_asfloat(r0.xyzw)));
  r0.xyzw = vd_asuint((vd_asfloat((VD_COL(unity_ObjectToWorld, 2).xyzw)) * vd_asfloat(v0.zzzz) + vd_asfloat(r0.xyzw)));
  r0.xyzw = vd_asuint((vd_asfloat(r0.xyzw) + vd_asfloat((VD_COL(unity_ObjectToWorld, 3).xyzw))));
  r1.xyzw = vd_asuint((vd_asfloat(r0.yyyy) * vd_asfloat((VD_COL(unity_MatrixVP, 1).xyzw))));
  r1.xyzw = vd_asuint((vd_asfloat((VD_COL(unity_MatrixVP, 0).xyzw)) * vd_asfloat(r0.xxxx) + vd_asfloat(r1.xyzw)));
  r1.xyzw = vd_asuint((vd_asfloat((VD_COL(unity_MatrixVP, 2).xyzw)) * vd_asfloat(r0.zzzz) + vd_asfloat(r1.xyzw)));
  o0.xyzw = (vd_asfloat((VD_COL(unity_MatrixVP, 3).xyzw)) * vd_asfloat(r0.wwww) + vd_asfloat(r1.xyzw));
  r0.xyz = vd_asuint((vd_asfloat(v1.xyz) * float3(0.265884995, 0.265884995, 0.265884995) + float3(0.736584008, 0.736584008, 0.736584008)));
  r0.xyz = vd_asuint((vd_asfloat(v1.xyz) * vd_asfloat(r0.xyz) + float3(-0.00980184041, -0.00980184041, -0.00980184041)));
  r0.xyz = vd_asuint((vd_asfloat(v1.xyz) * vd_asfloat(r0.xyz) + float3(0.00319697009, 0.00319697009, 0.00319697009)));
  r1.xyz = vd_asuint((vd_asfloat(v1.xyz) * float3(0.0849710032, 0.0849710032, 0.0849710032) + float3(-0.000163029006, -0.000163029006, -0.000163029006)));
  r2.xyz = vd_asuint((-(int3)(vd_asfloat(v1.xyz) < float3(0.0725490004, 0.0725490004, 0.0725490004))));
  r0.xyz = ((r2.xyz != 0u) ? r1.xyz : r0.xyz);
  r0.xyz = ((vd_asuint((_UIVertexColorAlwaysGammaSpace).xxx) != 0u) ? r0.xyz : vd_asuint(v1.xyz));
  r0.w = vd_asuint(v1.w);
  o1.xyzw = (vd_asfloat(r0.xyzw) * vd_asfloat(_Color.xyzw));
  r0.xy = vd_asuint(((-(vd_asfloat(_PlaneBoundMaxPos.xz))) + vd_asfloat((VD_COL(unity_ObjectToWorld, 3).xz))));
  r0.zw = vd_asuint((vd_asfloat(_PlaneBoundMinPos.xz) + (-(vd_asfloat(_PlaneBoundMaxPos.xz)))));
  r0.xy = vd_asuint(saturate((vd_asfloat(r0.xy) / vd_asfloat(r0.zw))));
  r0.xyzw = vd_asuint(_PlaneSightRenderTarget.SampleLevel(sampler_PlaneSightRenderTarget, vd_asfloat(r0.xy), 0.0).xyzw);
  r0.x = vd_asuint(max(vd_asfloat(r0.y), vd_asfloat(r0.x)));
  r0.x = vd_asuint(max(vd_asfloat(r0.z), vd_asfloat(r0.x)));
  r0.x = vd_asuint((vd_asfloat(r0.x) + -1.0));
  o2.z = (vd_asfloat(_GlobalPlaneSystemEnable) * vd_asfloat(r0.x) + 1.0);
  o2.xy = (vd_asfloat(v2.xy) * vd_asfloat(_MainTex_ST.xy) + vd_asfloat(_MainTex_ST.zw));
  o3.xyzw = vd_asfloat(vd_asuint(v0.xyzw));
  return;
}
