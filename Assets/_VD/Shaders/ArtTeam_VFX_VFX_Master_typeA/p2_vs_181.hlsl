// ArtTeam/VFX/VFX_Master_typeA | pass 2 "MotionVectors" | progVertex | keywords: _DEFORM_USE _MASK_USE _PLANESYSTEMCULLINGTYPE_ALWAYS | blob 181
void vert(
  float3 v0 : POSITION0,
  float3 v1 : NORMAL0,
  float4 v2 : TEXCOORD0,
  float4 v3 : TEXCOORD1,
  float4 v4 : TEXCOORD2,
  float4 v5 : COLOR0,
  float3 v6 : TEXCOORD4,
  out float4 o0 : VDREG0,
  out float4 o1 : VDREG1,
  out float4 o2 : SV_POSITION0,
  out float4 o3 : VDREG3,
  out float4 o4 : VDREG4,
  out float4 o5 : VDREG5,
  out float4 o6 : VDREG6,
  out float4 o7 : VDREG7,
  out float4 o8 : VDREG8)
{
  uint4 r0, r1, r2;
  r0 = 0u; r1 = 0u; r2 = 0u;

  r0.xyz = vd_asuint((vd_asfloat(v0.yyy) * vd_asfloat((VD_COL(unity_ObjectToWorld, 1).xyz))));
  r0.xyz = vd_asuint((vd_asfloat((VD_COL(unity_ObjectToWorld, 0).xyz)) * vd_asfloat(v0.xxx) + vd_asfloat(r0.xyz)));
  r0.xyz = vd_asuint((vd_asfloat((VD_COL(unity_ObjectToWorld, 2).xyz)) * vd_asfloat(v0.zzz) + vd_asfloat(r0.xyz)));
  r0.xyz = vd_asuint((vd_asfloat(r0.xyz) + vd_asfloat((VD_COL(unity_ObjectToWorld, 3).xyz))));
  r1.x = vd_asuint(dot(vd_asfloat(v1.xyz), vd_asfloat((VD_COL(unity_WorldToObject, 0).xyz))));
  r1.y = vd_asuint(dot(vd_asfloat(v1.xyz), vd_asfloat((VD_COL(unity_WorldToObject, 1).xyz))));
  r1.z = vd_asuint(dot(vd_asfloat(v1.xyz), vd_asfloat((VD_COL(unity_WorldToObject, 2).xyz))));
  r0.w = vd_asuint(dot(vd_asfloat(r1.xyz), vd_asfloat(r1.xyz)));
  r0.w = vd_asuint(max(vd_asfloat(r0.w), 1.17549435e-38));
  r0.w = vd_asuint(rsqrt(vd_asfloat(r0.w)));
  o8.xyz = (vd_asfloat(r0.www) * vd_asfloat(r1.xyz));
  r1.xyzw = vd_asuint((vd_asfloat(r0.yyyy) * vd_asfloat((VD_COL(unity_MatrixVP, 1).xyzw))));
  r1.xyzw = vd_asuint((vd_asfloat((VD_COL(unity_MatrixVP, 0).xyzw)) * vd_asfloat(r0.xxxx) + vd_asfloat(r1.xyzw)));
  r1.xyzw = vd_asuint((vd_asfloat((VD_COL(unity_MatrixVP, 2).xyzw)) * vd_asfloat(r0.zzzz) + vd_asfloat(r1.xyzw)));
  o2.xyzw = (vd_asfloat(r1.xyzw) + vd_asfloat((VD_COL(unity_MatrixVP, 3).xyzw)));
  r0.w = vd_asuint((-(int)(vd_asfloat(unity_MotionVectorsParams.y) != 0.0)));
  if (r0.w != 0u) {
    r0.w = vd_asuint((-(int)(vd_asfloat(unity_MotionVectorsParams.x) == 1.0)));
    if (r0.w != 0u) {
      r0.w = vd_asuint(v6.x);
      r1.xy = vd_asuint(v6.yz);
    } else {
      r0.w = vd_asuint(v0.x);
      r1.xy = vd_asuint(v0.yz);
    }
    r2.xyz = vd_asuint((vd_asfloat(r0.yyy) * vd_asfloat((VD_COL(_NonJitteredViewProjMatrix, 1).xyw))));
    r2.xyz = vd_asuint((vd_asfloat((VD_COL(_NonJitteredViewProjMatrix, 0).xyw)) * vd_asfloat(r0.xxx) + vd_asfloat(r2.xyz)));
    r2.xyz = vd_asuint((vd_asfloat((VD_COL(_NonJitteredViewProjMatrix, 2).xyw)) * vd_asfloat(r0.zzz) + vd_asfloat(r2.xyz)));
    o0.xyz = (vd_asfloat(r2.xyz) + vd_asfloat((VD_COL(_NonJitteredViewProjMatrix, 3).xyw)));
    r2.xyzw = vd_asuint((vd_asfloat(r1.xxxx) * vd_asfloat((VD_COL(unity_MatrixPreviousM, 1).xyzw))));
    r2.xyzw = vd_asuint((vd_asfloat((VD_COL(unity_MatrixPreviousM, 0).xyzw)) * vd_asfloat(r0.wwww) + vd_asfloat(r2.xyzw)));
    r1.xyzw = vd_asuint((vd_asfloat((VD_COL(unity_MatrixPreviousM, 2).xyzw)) * vd_asfloat(r1.yyyy) + vd_asfloat(r2.xyzw)));
    r1.xyzw = vd_asuint((vd_asfloat(r1.xyzw) + vd_asfloat((VD_COL(unity_MatrixPreviousM, 3).xyzw))));
    r2.xyz = vd_asuint((vd_asfloat(r1.yyy) * vd_asfloat((VD_COL(_PrevViewProjMatrix, 1).xyw))));
    r2.xyz = vd_asuint((vd_asfloat((VD_COL(_PrevViewProjMatrix, 0).xyw)) * vd_asfloat(r1.xxx) + vd_asfloat(r2.xyz)));
    r1.xyz = vd_asuint((vd_asfloat((VD_COL(_PrevViewProjMatrix, 2).xyw)) * vd_asfloat(r1.zzz) + vd_asfloat(r2.xyz)));
    o1.xyz = (vd_asfloat((VD_COL(_PrevViewProjMatrix, 3).xyw)) * vd_asfloat(r1.www) + vd_asfloat(r1.xyz));
  } else {
    o0.xyz = vd_asfloat(uint3(0x00000000u, 0x00000000u, 0x00000000u));
    o1.xyz = vd_asfloat(uint3(0x00000000u, 0x00000000u, 0x00000000u));
  }
  o3.xyzw = vd_asfloat(vd_asuint(v2.xyzw));
  o4.xyzw = vd_asfloat(vd_asuint(v3.xyzw));
  o5.xyzw = vd_asfloat(vd_asuint(v4.xyzw));
  o6.xyzw = vd_asfloat(vd_asuint(v5.xyzw));
  o7.xyz = vd_asfloat(r0.xyz);
  return;
}
