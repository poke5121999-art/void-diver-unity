// LCArt/Background/shader_DE_BG_Snapshot | pass 0 "ForwardLit" | progVertex | keywords: (none) | blob 1
void vert(
  float4 v0 : POSITION0,
  float4 v1 : COLOR0,
  float2 v2 : TEXCOORD0,
  out float4 o0 : SV_POSITION0,
  out float4 o1 : VDREG1,
  out float4 o2 : VDREG2,
  out float4 o3 : VDREG3,
  out float4 o4 : VDREG4)
{
  uint4 r0, r1;
  r0 = 0u; r1 = 0u;

  r0.xyz = vd_asuint((vd_asfloat(v0.yyy) * vd_asfloat((VD_COL(unity_ObjectToWorld, 1).xyz))));
  r0.xyz = vd_asuint((vd_asfloat((VD_COL(unity_ObjectToWorld, 0).xyz)) * vd_asfloat(v0.xxx) + vd_asfloat(r0.xyz)));
  r0.xyz = vd_asuint((vd_asfloat((VD_COL(unity_ObjectToWorld, 2).xyz)) * vd_asfloat(v0.zzz) + vd_asfloat(r0.xyz)));
  r0.xyz = vd_asuint((vd_asfloat((VD_COL(unity_ObjectToWorld, 3).xyz)) * vd_asfloat(v0.www) + vd_asfloat(r0.xyz)));
  r0.y = vd_asuint((vd_asfloat(r0.y) + 2.0));
  r1.xyzw = vd_asuint((vd_asfloat(r0.yyyy) * vd_asfloat((VD_COL(unity_WorldToObject, 1).xyzw))));
  r1.xyzw = vd_asuint((vd_asfloat((VD_COL(unity_WorldToObject, 0).xyzw)) * vd_asfloat(r0.xxxx) + vd_asfloat(r1.xyzw)));
  r0.xyzw = vd_asuint((vd_asfloat((VD_COL(unity_WorldToObject, 2).xyzw)) * vd_asfloat(r0.zzzz) + vd_asfloat(r1.xyzw)));
  r0.xyzw = vd_asuint((vd_asfloat(r0.xyzw) + vd_asfloat((VD_COL(unity_WorldToObject, 3).xyzw))));
  r1.x = vd_asuint((-(int)(vd_asint(_SnapshotType) == 1)));
  r0.xyzw = ((r1.xxxx != 0u) ? r0.xyzw : vd_asuint(v0.xyzw));
  r1.xyzw = vd_asuint((vd_asfloat(r0.yyyy) * vd_asfloat((VD_COL(unity_ObjectToWorld, 1).xyzw))));
  r1.xyzw = vd_asuint((vd_asfloat((VD_COL(unity_ObjectToWorld, 0).xyzw)) * vd_asfloat(r0.xxxx) + vd_asfloat(r1.xyzw)));
  r1.xyzw = vd_asuint((vd_asfloat((VD_COL(unity_ObjectToWorld, 2).xyzw)) * vd_asfloat(r0.zzzz) + vd_asfloat(r1.xyzw)));
  o1.xyz = (vd_asfloat((VD_COL(unity_ObjectToWorld, 3).xyz)) * vd_asfloat(r0.www) + vd_asfloat(r1.xyz));
  r0.xyzw = vd_asuint((vd_asfloat(r1.xyzw) + vd_asfloat((VD_COL(unity_ObjectToWorld, 3).xyzw))));
  r1.xyzw = vd_asuint((vd_asfloat(r0.yyyy) * vd_asfloat((VD_COL(unity_MatrixVP, 1).xyzw))));
  r1.xyzw = vd_asuint((vd_asfloat((VD_COL(unity_MatrixVP, 0).xyzw)) * vd_asfloat(r0.xxxx) + vd_asfloat(r1.xyzw)));
  r1.xyzw = vd_asuint((vd_asfloat((VD_COL(unity_MatrixVP, 2).xyzw)) * vd_asfloat(r0.zzzz) + vd_asfloat(r1.xyzw)));
  o0.xyzw = (vd_asfloat((VD_COL(unity_MatrixVP, 3).xyzw)) * vd_asfloat(r0.wwww) + vd_asfloat(r1.xyzw));
  o2.xyzw = vd_asfloat(vd_asuint(v1.xyzw));
  o3.xy = vd_asfloat(vd_asuint(v2.xy));
  o4.xyz = vd_asfloat(vd_asuint((VD_COL(unity_ObjectToWorld, 1).xyz)));
  return;
}
