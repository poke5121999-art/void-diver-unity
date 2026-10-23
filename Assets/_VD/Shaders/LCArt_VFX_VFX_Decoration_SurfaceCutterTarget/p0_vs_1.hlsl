// LCArt/VFX/VFX_Decoration_SurfaceCutterTarget | pass 0 "ForwardLit" | progVertex | keywords: (none) | blob 1
void vert(
  float4 v0 : POSITION0,
  float2 v1 : TEXCOORD0,
  float4 v2 : COLOR0,
  float4 v3 : TEXCOORD1,
  float4 v4 : TEXCOORD2,
  out float4 o0 : SV_POSITION0,
  out float4 o1 : VDREG1,
  out float4 o2 : VDREG2,
  out float4 o3 : VDREG3,
  out float4 o4 : VDREG4,
  out float4 o5 : VDREG5)
{
  uint4 r0, r1;
  r0 = 0u; r1 = 0u;

  r0.xyzw = vd_asuint((vd_asfloat(v0.yyyy) * vd_asfloat((VD_COL(unity_ObjectToWorld, 1).xyzw))));
  r0.xyzw = vd_asuint((vd_asfloat((VD_COL(unity_ObjectToWorld, 0).xyzw)) * vd_asfloat(v0.xxxx) + vd_asfloat(r0.xyzw)));
  r0.xyzw = vd_asuint((vd_asfloat((VD_COL(unity_ObjectToWorld, 2).xyzw)) * vd_asfloat(v0.zzzz) + vd_asfloat(r0.xyzw)));
  r1.xyzw = vd_asuint((vd_asfloat(r0.xyzw) + vd_asfloat((VD_COL(unity_ObjectToWorld, 3).xyzw))));
  o1.xyz = (vd_asfloat((VD_COL(unity_ObjectToWorld, 3).xyz)) * vd_asfloat(v0.www) + vd_asfloat(r0.xyz));
  r0.xyzw = vd_asuint((vd_asfloat(r1.yyyy) * vd_asfloat((VD_COL(unity_MatrixVP, 1).xyzw))));
  r0.xyzw = vd_asuint((vd_asfloat((VD_COL(unity_MatrixVP, 0).xyzw)) * vd_asfloat(r1.xxxx) + vd_asfloat(r0.xyzw)));
  r0.xyzw = vd_asuint((vd_asfloat((VD_COL(unity_MatrixVP, 2).xyzw)) * vd_asfloat(r1.zzzz) + vd_asfloat(r0.xyzw)));
  o0.xyzw = (vd_asfloat((VD_COL(unity_MatrixVP, 3).xyzw)) * vd_asfloat(r1.wwww) + vd_asfloat(r0.xyzw));
  o2.xy = vd_asfloat(vd_asuint(v1.xy));
  o3.xyzw = vd_asfloat(vd_asuint(v2.xyzw));
  o4.xyzw = vd_asfloat(vd_asuint(v3.xyzw));
  o5.xyzw = vd_asfloat(vd_asuint(v4.xyzw));
  return;
}
