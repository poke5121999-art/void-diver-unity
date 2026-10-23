// Universal Render Pipeline/Nature/SpeedTree8_PBRLit | pass 1 "MotionVectors" | progVertex | keywords: _WINDQUALITY_NONE | blob 32
void vert(
  float3 v0 : POSITION0,
  float3 v1 : NORMAL0,
  float4 v2 : TANGENT0,
  float4 v3 : TEXCOORD0,
  float4 v4 : TEXCOORD1,
  float4 v5 : TEXCOORD2,
  float4 v6 : TEXCOORD3,
  float4 v7 : COLOR0,
  float3 v8 : TEXCOORD4,
  out float4 o0 : VDREG0,
  out float4 o1 : VDREG1,
  out float4 o2 : SV_POSITION0,
  out float4 o3 : VDREG3,
  out float4 o4 : VDREG4,
  out float4 o5 : VDREG5,
  out float4 o6 : VDREG6,
  out float4 o7 : VDREG7,
  out float4 o8 : VDREG8,
  out float4 o9 : VDREG9)
{
  uint4 r0, r1, r2, r3, r4, r5, r6, r7, r8, r9, r10;
  r0 = 0u; r1 = 0u; r2 = 0u; r3 = 0u; r4 = 0u; r5 = 0u; r6 = 0u; r7 = 0u; r8 = 0u; r9 = 0u; r10 = 0u;

  r0.x = vd_asuint(dot(vd_asfloat(v1.xyz), vd_asfloat((VD_COL(unity_WorldToObject, 0).xyz))));
  r0.y = vd_asuint(dot(vd_asfloat(v1.xyz), vd_asfloat((VD_COL(unity_WorldToObject, 1).xyz))));
  r0.z = vd_asuint(dot(vd_asfloat(v1.xyz), vd_asfloat((VD_COL(unity_WorldToObject, 2).xyz))));
  r0.w = vd_asuint(dot(vd_asfloat(r0.xyz), vd_asfloat(r0.xyz)));
  r0.w = vd_asuint(max(vd_asfloat(r0.w), 1.17549435e-38));
  r0.w = vd_asuint(rsqrt(vd_asfloat(r0.w)));
  r0.xyzw = vd_asuint((vd_asfloat(r0.wwww) * vd_asfloat(r0.xyzy)));
  r1.xyz = vd_asuint((vd_asfloat(v2.yyy) * vd_asfloat((VD_COL(unity_ObjectToWorld, 1).xyz))));
  r1.xyz = vd_asuint((vd_asfloat((VD_COL(unity_ObjectToWorld, 0).xyz)) * vd_asfloat(v2.xxx) + vd_asfloat(r1.xyz)));
  r1.xyz = vd_asuint((vd_asfloat((VD_COL(unity_ObjectToWorld, 2).xyz)) * vd_asfloat(v2.zzz) + vd_asfloat(r1.xyz)));
  r1.w = vd_asuint(dot(vd_asfloat(r1.xyz), vd_asfloat(r1.xyz)));
  r1.w = vd_asuint(max(vd_asfloat(r1.w), 1.17549435e-38));
  r1.w = vd_asuint(rsqrt(vd_asfloat(r1.w)));
  r1.xyz = vd_asuint((vd_asfloat(r1.www) * vd_asfloat(r1.xyz)));
  r2.xyz = vd_asuint((vd_asfloat(v1.zxy) * vd_asfloat(v2.yzx)));
  r2.xyz = vd_asuint((vd_asfloat(v1.yzx) * vd_asfloat(v2.zxy) + (-(vd_asfloat(r2.xyz)))));
  r1.w = vd_asuint((-(int)(0.0 < vd_asfloat(v2.w))));
  r1.w = ((r1.w != 0u) ? 0x3f800000u : 0xbf800000u);
  r2.xyz = vd_asuint((vd_asfloat(r1.www) * vd_asfloat(r2.xyz)));
  r1.w = vd_asuint((-(int)(vd_asfloat(unity_WorldTransformParams.w) >= 0.0)));
  r1.w = ((r1.w != 0u) ? 0x3f800000u : 0xbf800000u);
  r2.xyz = vd_asuint((vd_asfloat(r1.www) * vd_asfloat(r2.xyz)));
  r1.w = vd_asuint(dot(vd_asfloat(r2.xyz), vd_asfloat(r2.xyz)));
  r1.w = vd_asuint(rsqrt(vd_asfloat(r1.w)));
  r2.xyz = vd_asuint((vd_asfloat(r1.www) * vd_asfloat(r2.xyz)));
  r3.xyz = vd_asuint((vd_asfloat(r2.yyy) * vd_asfloat((VD_COL(unity_ObjectToWorld, 1).xyz))));
  r2.xyw = vd_asuint((vd_asfloat((VD_COL(unity_ObjectToWorld, 0).xyz)) * vd_asfloat(r2.xxx) + vd_asfloat(r3.xyz)));
  r2.xyz = vd_asuint((vd_asfloat((VD_COL(unity_ObjectToWorld, 2).xyz)) * vd_asfloat(r2.zzz) + vd_asfloat(r2.xyw)));
  r1.w = vd_asuint(dot(vd_asfloat(r2.xyz), vd_asfloat(r2.xyz)));
  r1.w = vd_asuint(max(vd_asfloat(r1.w), 1.17549435e-38));
  r1.w = vd_asuint(rsqrt(vd_asfloat(r1.w)));
  o9.xyz = (vd_asfloat(r1.www) * vd_asfloat(r2.xyz));
  r1.w = vd_asuint((-(int)(1.0 < vd_asfloat(unity_LODFade.w))));
  r2.xyz = vd_asuint(((-(vd_asfloat(v0.xyz))) + vd_asfloat(v5.xyz)));
  r2.xyz = vd_asuint((vd_asfloat(unity_LODFade.xxx) * vd_asfloat(r2.xyz) + vd_asfloat(v0.xyz)));
  r2.xyz = ((r1.www != 0u) ? vd_asuint(v0.xyz) : r2.xyz);
  r2.w = vd_asuint((vd_asfloat(v6.w) + 0.25));
  r2.w = vd_asuint(((int)vd_asfloat(r2.w)));
  r3.x = vd_asuint((-(int)(3 < vd_asint(r2.w))));
  r3.y = vd_asuint((vd_asint(r2.w) + -2));
  r2.w = ((r3.x != 0u) ? r3.y : r2.w);
  r2.w = vd_asuint((-(int)(vd_asint(r2.w) == 3)));
  r3.xy = vd_asuint(v4.zw);
  r3.z = vd_asuint(v5.w);
  r4.xyz = vd_asuint((vd_asfloat(r2.xyz) + (-(vd_asfloat(r3.xyz)))));
  r3.w = vd_asuint(dot(vd_asfloat(r4.xyz), vd_asfloat(r4.xyz)));
  r3.w = vd_asuint(sqrt(vd_asfloat(r3.w)));
  r5.x = vd_asuint(dot(vd_asfloat((VD_COL(unity_ObjectToWorld, 0).xyz)), vd_asfloat((VD_COL(unity_ObjectToWorld, 0).xyz))));
  r5.y = vd_asuint(dot(vd_asfloat((VD_COL(unity_ObjectToWorld, 1).xyz)), vd_asfloat((VD_COL(unity_ObjectToWorld, 1).xyz))));
  r5.z = vd_asuint(dot(vd_asfloat((VD_COL(unity_ObjectToWorld, 2).xyz)), vd_asfloat((VD_COL(unity_ObjectToWorld, 2).xyz))));
  r5.xyz = vd_asuint(sqrt(vd_asfloat(r5.xyz)));
  r6.xyz = vd_asuint((vd_asfloat((VD_COL(unity_MatrixInvV, 0).yyy)) * vd_asfloat((VD_COL(unity_WorldToObject, 1).xyz))));
  r6.xyz = vd_asuint((vd_asfloat((VD_COL(unity_WorldToObject, 0).xyz)) * vd_asfloat((VD_COL(unity_MatrixInvV, 0).xxx)) + vd_asfloat(r6.xyz)));
  r6.xyz = vd_asuint((vd_asfloat((VD_COL(unity_WorldToObject, 2).xyz)) * vd_asfloat((VD_COL(unity_MatrixInvV, 0).zzz)) + vd_asfloat(r6.xyz)));
  r7.xyz = vd_asuint((vd_asfloat((VD_COL(unity_MatrixInvV, 1).yyy)) * vd_asfloat((VD_COL(unity_WorldToObject, 1).xyz))));
  r7.xyz = vd_asuint((vd_asfloat((VD_COL(unity_WorldToObject, 0).xyz)) * vd_asfloat((VD_COL(unity_MatrixInvV, 1).xxx)) + vd_asfloat(r7.xyz)));
  r7.xyz = vd_asuint((vd_asfloat((VD_COL(unity_WorldToObject, 2).xyz)) * vd_asfloat((VD_COL(unity_MatrixInvV, 1).zzz)) + vd_asfloat(r7.xyz)));
  r8.xyz = vd_asuint((vd_asfloat((VD_COL(unity_MatrixInvV, 2).yyy)) * vd_asfloat((VD_COL(unity_WorldToObject, 1).xyz))));
  r8.xyz = vd_asuint((vd_asfloat((VD_COL(unity_WorldToObject, 0).xyz)) * vd_asfloat((VD_COL(unity_MatrixInvV, 2).xxx)) + vd_asfloat(r8.xyz)));
  r8.xyz = vd_asuint((vd_asfloat((VD_COL(unity_WorldToObject, 2).xyz)) * vd_asfloat((VD_COL(unity_MatrixInvV, 2).zzz)) + vd_asfloat(r8.xyz)));
  r9.x = r6.x;
  r9.y = r7.x;
  r9.z = r8.x;
  r9.xyz = vd_asuint((vd_asfloat(r5.xxx) * vd_asfloat(r9.xyz)));
  r10.x = r6.y;
  r10.y = r7.y;
  r10.z = r8.y;
  r5.xyw = vd_asuint((vd_asfloat(r5.yyy) * vd_asfloat(r10.xyz)));
  r8.x = r6.z;
  r8.y = r7.z;
  r6.xyz = vd_asuint((vd_asfloat(r5.zzz) * vd_asfloat(r8.xyz)));
  r4.w = vd_asuint((-(vd_asfloat(r4.z))));
  r7.x = vd_asuint(dot(vd_asfloat(r9.xzy), vd_asfloat(r4.xyw)));
  r7.y = vd_asuint(dot(vd_asfloat(r5.xwy), vd_asfloat(r4.xyw)));
  r7.z = vd_asuint(dot(vd_asfloat(r6.xzy), vd_asfloat(r4.xyw)));
  r4.x = vd_asuint(dot(vd_asfloat(r7.xyz), vd_asfloat(r7.xyz)));
  r4.x = vd_asuint(rsqrt(vd_asfloat(r4.x)));
  r4.xyz = vd_asuint((vd_asfloat(r4.xxx) * vd_asfloat(r7.xyz)));
  r4.xyz = vd_asuint((vd_asfloat(r4.xyz) * vd_asfloat(r3.www) + vd_asfloat(r3.xyz)));
  r2.xyz = ((r2.www != 0u) ? r4.xyz : r2.xyz);
  r4.xyz = vd_asuint((vd_asfloat(r2.yyy) * vd_asfloat((VD_COL(unity_ObjectToWorld, 1).xyz))));
  r4.xyz = vd_asuint((vd_asfloat((VD_COL(unity_ObjectToWorld, 0).xyz)) * vd_asfloat(r2.xxx) + vd_asfloat(r4.xyz)));
  r4.xyz = vd_asuint((vd_asfloat((VD_COL(unity_ObjectToWorld, 2).xyz)) * vd_asfloat(r2.zzz) + vd_asfloat(r4.xyz)));
  r4.xyz = vd_asuint((vd_asfloat(r4.xyz) + vd_asfloat((VD_COL(unity_ObjectToWorld, 3).xyz))));
  r7.xyzw = vd_asuint((vd_asfloat(r4.yyyy) * vd_asfloat((VD_COL(unity_MatrixVP, 1).xyzw))));
  r7.xyzw = vd_asuint((vd_asfloat((VD_COL(unity_MatrixVP, 0).xyzw)) * vd_asfloat(r4.xxxx) + vd_asfloat(r7.xyzw)));
  r7.xyzw = vd_asuint((vd_asfloat((VD_COL(unity_MatrixVP, 2).xyzw)) * vd_asfloat(r4.zzzz) + vd_asfloat(r7.xyzw)));
  o2.xyzw = (vd_asfloat(r7.xyzw) + vd_asfloat((VD_COL(unity_MatrixVP, 3).xyzw)));
  r3.w = vd_asuint((-(int)(vd_asfloat(unity_MotionVectorsParams.y) != 0.0)));
  if (r3.w != 0u) {
    r3.w = vd_asuint((-(int)(vd_asfloat(unity_MotionVectorsParams.x) == 1.0)));
    if (r3.w != 0u) {
      r7.xyz = vd_asuint((vd_asfloat(v5.xyz) + (-(vd_asfloat(v8.xyz)))));
      r7.xyz = vd_asuint((vd_asfloat(unity_LODFade.xxx) * vd_asfloat(r7.xyz) + vd_asfloat(v8.xyz)));
      r7.xyz = ((r1.www != 0u) ? vd_asuint(v8.xyz) : r7.xyz);
      r8.xyz = vd_asuint(((-(vd_asfloat(r3.xyz))) + vd_asfloat(r7.xyz)));
      r1.w = vd_asuint(dot(vd_asfloat(r8.xyz), vd_asfloat(r8.xyz)));
      r1.w = vd_asuint(sqrt(vd_asfloat(r1.w)));
      r8.w = vd_asuint((-(vd_asfloat(r8.z))));
      r9.x = vd_asuint(dot(vd_asfloat(r9.xzy), vd_asfloat(r8.xyw)));
      r9.y = vd_asuint(dot(vd_asfloat(r5.xwy), vd_asfloat(r8.xyw)));
      r9.z = vd_asuint(dot(vd_asfloat(r6.xzy), vd_asfloat(r8.xyw)));
      r3.w = vd_asuint(dot(vd_asfloat(r9.xyz), vd_asfloat(r9.xyz)));
      r3.w = vd_asuint(rsqrt(vd_asfloat(r3.w)));
      r5.xyz = vd_asuint((vd_asfloat(r3.www) * vd_asfloat(r9.xyz)));
      r3.xyz = vd_asuint((vd_asfloat(r5.xyz) * vd_asfloat(r1.www) + vd_asfloat(r3.xyz)));
      r2.xyz = ((r2.www != 0u) ? r3.xyz : r7.xyz);
    }
    r3.xyz = vd_asuint((vd_asfloat(r4.yyy) * vd_asfloat((VD_COL(_NonJitteredViewProjMatrix, 1).xyw))));
    r3.xyz = vd_asuint((vd_asfloat((VD_COL(_NonJitteredViewProjMatrix, 0).xyw)) * vd_asfloat(r4.xxx) + vd_asfloat(r3.xyz)));
    r3.xyz = vd_asuint((vd_asfloat((VD_COL(_NonJitteredViewProjMatrix, 2).xyw)) * vd_asfloat(r4.zzz) + vd_asfloat(r3.xyz)));
    o0.xyz = (vd_asfloat(r3.xyz) + vd_asfloat((VD_COL(_NonJitteredViewProjMatrix, 3).xyw)));
    r3.xyzw = vd_asuint((vd_asfloat(r2.yyyy) * vd_asfloat((VD_COL(unity_MatrixPreviousM, 1).xyzw))));
    r3.xyzw = vd_asuint((vd_asfloat((VD_COL(unity_MatrixPreviousM, 0).xyzw)) * vd_asfloat(r2.xxxx) + vd_asfloat(r3.xyzw)));
    r2.xyzw = vd_asuint((vd_asfloat((VD_COL(unity_MatrixPreviousM, 2).xyzw)) * vd_asfloat(r2.zzzz) + vd_asfloat(r3.xyzw)));
    r2.xyzw = vd_asuint((vd_asfloat(r2.xyzw) + vd_asfloat((VD_COL(unity_MatrixPreviousM, 3).xyzw))));
    r3.xyz = vd_asuint((vd_asfloat(r2.yyy) * vd_asfloat((VD_COL(_PrevViewProjMatrix, 1).xyw))));
    r3.xyz = vd_asuint((vd_asfloat((VD_COL(_PrevViewProjMatrix, 0).xyw)) * vd_asfloat(r2.xxx) + vd_asfloat(r3.xyz)));
    r2.xyz = vd_asuint((vd_asfloat((VD_COL(_PrevViewProjMatrix, 2).xyw)) * vd_asfloat(r2.zzz) + vd_asfloat(r3.xyz)));
    o1.xyz = (vd_asfloat((VD_COL(_PrevViewProjMatrix, 3).xyw)) * vd_asfloat(r2.www) + vd_asfloat(r2.xyz));
  } else {
    o0.xyz = vd_asfloat(uint3(0x00000000u, 0x00000000u, 0x00000000u));
    o1.xyz = vd_asfloat(uint3(0x00000000u, 0x00000000u, 0x00000000u));
  }
  o3.w = vd_asfloat(vd_asuint(v2.w));
  o3.xyz = vd_asfloat(r1.xyz);
  o4.xyzw = vd_asfloat(vd_asuint(v3.xyzw));
  o5.xyzw = vd_asfloat(vd_asuint(v7.xyzw));
  o6.w = vd_asfloat(r0.x);
  o6.xyz = vd_asfloat(r4.xyz);
  o7.xyzw = vd_asfloat(r0.xyzw);
  o8.xyz = vd_asfloat(r1.xyz);
  o8.w = vd_asfloat(r0.z);
  return;
}
