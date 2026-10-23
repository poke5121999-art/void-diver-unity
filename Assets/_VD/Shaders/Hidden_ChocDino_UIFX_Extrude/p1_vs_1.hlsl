// Hidden/ChocDino/UIFX/Extrude | pass 1 "Extrude-Orthographic" | progVertex | keywords: (none) | blob 1
void vert(
  float4 v0 : POSITION0,
  float2 v1 : TEXCOORD0,
  out float4 o0 : SV_POSITION0,
  out float4 o1 : VDREG1,
  out float4 o2 : VDREG2,
  out float4 o3 : VDREG3)
{
  uint4 r0, r1;
  r0 = 0u; r1 = 0u;

  r0.xyzw = vd_asuint((vd_asfloat(v0.yyyy) * vd_asfloat((VD_COL(unity_ObjectToWorld, 1).xyzw))));
  r0.xyzw = vd_asuint((vd_asfloat((VD_COL(unity_ObjectToWorld, 0).xyzw)) * vd_asfloat(v0.xxxx) + vd_asfloat(r0.xyzw)));
  r0.xyzw = vd_asuint((vd_asfloat((VD_COL(unity_ObjectToWorld, 2).xyzw)) * vd_asfloat(v0.zzzz) + vd_asfloat(r0.xyzw)));
  r0.xyzw = vd_asuint((vd_asfloat(r0.xyzw) + vd_asfloat((VD_COL(unity_ObjectToWorld, 3).xyzw))));
  r1.xyzw = vd_asuint((vd_asfloat(r0.yyyy) * vd_asfloat((VD_COL(unity_MatrixVP, 1).xyzw))));
  r1.xyzw = vd_asuint((vd_asfloat((VD_COL(unity_MatrixVP, 0).xyzw)) * vd_asfloat(r0.xxxx) + vd_asfloat(r1.xyzw)));
  r1.xyzw = vd_asuint((vd_asfloat((VD_COL(unity_MatrixVP, 2).xyzw)) * vd_asfloat(r0.zzzz) + vd_asfloat(r1.xyzw)));
  o0.xyzw = (vd_asfloat((VD_COL(unity_MatrixVP, 3).xyzw)) * vd_asfloat(r0.wwww) + vd_asfloat(r1.xyzw));
  o1.xy = vd_asfloat(vd_asuint(v1.xy));
  r0.xyz = vd_asuint((vd_asfloat(_ColorFront.xyz) * float3(0.305306017, 0.305306017, 0.305306017) + float3(0.682171106, 0.682171106, 0.682171106)));
  r0.xyz = vd_asuint((vd_asfloat(_ColorFront.xyz) * vd_asfloat(r0.xyz) + float3(0.0125228781, 0.0125228781, 0.0125228781)));
  r0.xyz = vd_asuint((vd_asfloat(r0.xyz) * vd_asfloat(_ColorFront.xyz)));
  o2.xyz = (vd_asfloat(r0.xyz) * vd_asfloat(_ColorFront.www));
  o2.w = vd_asfloat(vd_asuint(_ColorFront.w));
  r0.xyz = vd_asuint((vd_asfloat(_ColorBack.xyz) * float3(0.305306017, 0.305306017, 0.305306017) + float3(0.682171106, 0.682171106, 0.682171106)));
  r0.xyz = vd_asuint((vd_asfloat(_ColorBack.xyz) * vd_asfloat(r0.xyz) + float3(0.0125228781, 0.0125228781, 0.0125228781)));
  r0.xyz = vd_asuint((vd_asfloat(r0.xyz) * vd_asfloat(_ColorBack.xyz)));
  o3.xyz = (vd_asfloat(r0.xyz) * vd_asfloat(_ColorBack.www));
  o3.w = vd_asfloat(vd_asuint(_ColorBack.w));
  return;
}
