// Universal Render Pipeline/Nature/SpeedTree8_PBRLit | pass 2 "DepthOnly" | progVertex | keywords: _WINDQUALITY_NONE | blob 38
void vert(
  float3 v0 : POSITION0,
  float3 v1 : NORMAL0,
  float4 v2 : TANGENT0,
  float4 v3 : TEXCOORD0,
  float4 v4 : TEXCOORD1,
  float4 v5 : TEXCOORD2,
  float4 v6 : TEXCOORD3,
  float4 v7 : COLOR0,
  out float4 o0 : SV_POSITION0,
  out float4 o1 : VDREG1,
  out float4 o2 : VDREG2,
  out float4 o3 : VDREG3,
  out float4 o4 : VDREG4,
  out float4 o5 : VDREG5,
  out float4 o6 : VDREG6,
  out float4 o7 : VDREG7)
{
  uint4 r0, r1, r2, r3, r4, r5, r6;
  r0 = 0u; r1 = 0u; r2 = 0u; r3 = 0u; r4 = 0u; r5 = 0u; r6 = 0u;

  r0.x = vd_asuint(dot(vd_asfloat((VD_COL(unity_ObjectToWorld, 0).xyz)), vd_asfloat((VD_COL(unity_ObjectToWorld, 0).xyz))));
  r0.x = vd_asuint(sqrt(vd_asfloat(r0.x)));
  r0.yzw = vd_asuint((vd_asfloat((VD_COL(unity_MatrixInvV, 0).yyy)) * vd_asfloat((VD_COL(unity_WorldToObject, 1).xyz))));
  r0.yzw = vd_asuint((vd_asfloat((VD_COL(unity_WorldToObject, 0).xyz)) * vd_asfloat((VD_COL(unity_MatrixInvV, 0).xxx)) + vd_asfloat(r0.yzw)));
  r0.yzw = vd_asuint((vd_asfloat((VD_COL(unity_WorldToObject, 2).yxz)) * vd_asfloat((VD_COL(unity_MatrixInvV, 0).zzz)) + vd_asfloat(r0.zyw)));
  r1.x = r0.z;
  r2.xyz = vd_asuint((vd_asfloat((VD_COL(unity_MatrixInvV, 1).yyy)) * vd_asfloat((VD_COL(unity_WorldToObject, 1).xyz))));
  r2.xyz = vd_asuint((vd_asfloat((VD_COL(unity_WorldToObject, 0).xyz)) * vd_asfloat((VD_COL(unity_MatrixInvV, 1).xxx)) + vd_asfloat(r2.xyz)));
  r2.xyz = vd_asuint((vd_asfloat((VD_COL(unity_WorldToObject, 2).xzy)) * vd_asfloat((VD_COL(unity_MatrixInvV, 1).zzz)) + vd_asfloat(r2.xzy)));
  r1.y = r2.x;
  r3.xyz = vd_asuint((vd_asfloat((VD_COL(unity_MatrixInvV, 2).yyy)) * vd_asfloat((VD_COL(unity_WorldToObject, 1).xyz))));
  r3.xyz = vd_asuint((vd_asfloat((VD_COL(unity_WorldToObject, 0).xyz)) * vd_asfloat((VD_COL(unity_MatrixInvV, 2).xxx)) + vd_asfloat(r3.xyz)));
  r3.xyz = vd_asuint((vd_asfloat((VD_COL(unity_WorldToObject, 2).xyz)) * vd_asfloat((VD_COL(unity_MatrixInvV, 2).zzz)) + vd_asfloat(r3.xyz)));
  r1.z = r3.x;
  r1.xyz = vd_asuint((vd_asfloat(r0.xxx) * vd_asfloat(r1.xyz)));
  r4.xyz = vd_asuint(((-(vd_asfloat(v0.xyz))) + vd_asfloat(v5.xyz)));
  r4.xyz = vd_asuint((vd_asfloat(unity_LODFade.xxx) * vd_asfloat(r4.xyz) + vd_asfloat(v0.xyz)));
  r0.x = vd_asuint((-(int)(1.0 < vd_asfloat(unity_LODFade.w))));
  r4.xyz = ((r0.xxx != 0u) ? vd_asuint(v0.xyz) : r4.xyz);
  r5.xy = vd_asuint(v4.zw);
  r5.z = vd_asuint(v5.w);
  r6.xyz = vd_asuint((vd_asfloat(r4.xyz) + (-(vd_asfloat(r5.xyz)))));
  r6.w = vd_asuint((-(vd_asfloat(r6.z))));
  r1.x = vd_asuint(dot(vd_asfloat(r1.xzy), vd_asfloat(r6.xyw)));
  r2.x = r0.w;
  r0.z = r2.z;
  r0.w = r3.y;
  r2.z = r3.z;
  r0.x = vd_asuint(dot(vd_asfloat((VD_COL(unity_ObjectToWorld, 1).xyz)), vd_asfloat((VD_COL(unity_ObjectToWorld, 1).xyz))));
  r0.x = vd_asuint(sqrt(vd_asfloat(r0.x)));
  r0.xyz = vd_asuint((vd_asfloat(r0.xxx) * vd_asfloat(r0.yzw)));
  r1.y = vd_asuint(dot(vd_asfloat(r0.xzy), vd_asfloat(r6.xyw)));
  r0.x = vd_asuint(dot(vd_asfloat((VD_COL(unity_ObjectToWorld, 2).xyz)), vd_asfloat((VD_COL(unity_ObjectToWorld, 2).xyz))));
  r0.x = vd_asuint(sqrt(vd_asfloat(r0.x)));
  r0.xyz = vd_asuint((vd_asfloat(r0.xxx) * vd_asfloat(r2.xyz)));
  r1.z = vd_asuint(dot(vd_asfloat(r0.xzy), vd_asfloat(r6.xyw)));
  r0.x = vd_asuint(dot(vd_asfloat(r6.xyz), vd_asfloat(r6.xyz)));
  r0.x = vd_asuint(sqrt(vd_asfloat(r0.x)));
  r0.y = vd_asuint(dot(vd_asfloat(r1.xyz), vd_asfloat(r1.xyz)));
  r0.y = vd_asuint(rsqrt(vd_asfloat(r0.y)));
  r0.yzw = vd_asuint((vd_asfloat(r0.yyy) * vd_asfloat(r1.xyz)));
  r0.xyz = vd_asuint((vd_asfloat(r0.yzw) * vd_asfloat(r0.xxx) + vd_asfloat(r5.xyz)));
  r0.w = vd_asuint((vd_asfloat(v6.w) + 0.25));
  r0.w = vd_asuint(((int)vd_asfloat(r0.w)));
  r1.x = vd_asuint((-(int)(3 < vd_asint(r0.w))));
  r1.y = vd_asuint((vd_asint(r0.w) + -2));
  r0.w = ((r1.x != 0u) ? r1.y : r0.w);
  r0.w = vd_asuint((-(int)(vd_asint(r0.w) == 3)));
  r0.xyz = ((r0.www != 0u) ? r0.xyz : r4.xyz);
  r1.xyz = vd_asuint((vd_asfloat(r0.yyy) * vd_asfloat((VD_COL(unity_ObjectToWorld, 1).xyz))));
  r0.xyw = vd_asuint((vd_asfloat((VD_COL(unity_ObjectToWorld, 0).xyz)) * vd_asfloat(r0.xxx) + vd_asfloat(r1.xyz)));
  r0.xyz = vd_asuint((vd_asfloat((VD_COL(unity_ObjectToWorld, 2).xyz)) * vd_asfloat(r0.zzz) + vd_asfloat(r0.xyw)));
  r0.xyz = vd_asuint((vd_asfloat(r0.xyz) + vd_asfloat((VD_COL(unity_ObjectToWorld, 3).xyz))));
  r1.xyzw = vd_asuint((vd_asfloat(r0.yyyy) * vd_asfloat((VD_COL(unity_MatrixVP, 1).xyzw))));
  r1.xyzw = vd_asuint((vd_asfloat((VD_COL(unity_MatrixVP, 0).xyzw)) * vd_asfloat(r0.xxxx) + vd_asfloat(r1.xyzw)));
  r1.xyzw = vd_asuint((vd_asfloat((VD_COL(unity_MatrixVP, 2).xyzw)) * vd_asfloat(r0.zzzz) + vd_asfloat(r1.xyzw)));
  o4.xyz = vd_asfloat(r0.xyz);
  o0.xyzw = (vd_asfloat(r1.xyzw) + vd_asfloat((VD_COL(unity_MatrixVP, 3).xyzw)));
  r0.xyz = vd_asuint((vd_asfloat(v2.yyy) * vd_asfloat((VD_COL(unity_ObjectToWorld, 1).xyz))));
  r0.xyz = vd_asuint((vd_asfloat((VD_COL(unity_ObjectToWorld, 0).xyz)) * vd_asfloat(v2.xxx) + vd_asfloat(r0.xyz)));
  r0.xyz = vd_asuint((vd_asfloat((VD_COL(unity_ObjectToWorld, 2).xyz)) * vd_asfloat(v2.zzz) + vd_asfloat(r0.xyz)));
  r0.w = vd_asuint(dot(vd_asfloat(r0.xyz), vd_asfloat(r0.xyz)));
  r0.w = vd_asuint(max(vd_asfloat(r0.w), 1.17549435e-38));
  r0.w = vd_asuint(rsqrt(vd_asfloat(r0.w)));
  r0.xyz = vd_asuint((vd_asfloat(r0.www) * vd_asfloat(r0.xyz)));
  o1.xyz = vd_asfloat(r0.xyz);
  o6.xyz = vd_asfloat(r0.xyz);
  o1.w = vd_asfloat(vd_asuint(v2.w));
  o2.xyzw = vd_asfloat(vd_asuint(v3.xyzw));
  o3.xyzw = vd_asfloat(vd_asuint(v7.xyzw));
  r0.x = vd_asuint(dot(vd_asfloat(v1.xyz), vd_asfloat((VD_COL(unity_WorldToObject, 0).xyz))));
  r0.y = vd_asuint(dot(vd_asfloat(v1.xyz), vd_asfloat((VD_COL(unity_WorldToObject, 1).xyz))));
  r0.z = vd_asuint(dot(vd_asfloat(v1.xyz), vd_asfloat((VD_COL(unity_WorldToObject, 2).xyz))));
  r0.w = vd_asuint(dot(vd_asfloat(r0.xyz), vd_asfloat(r0.xyz)));
  r0.w = vd_asuint(max(vd_asfloat(r0.w), 1.17549435e-38));
  r0.w = vd_asuint(rsqrt(vd_asfloat(r0.w)));
  r0.xyzw = vd_asuint((vd_asfloat(r0.wwww) * vd_asfloat(r0.xyzy)));
  o4.w = vd_asfloat(r0.x);
  o5.xyzw = vd_asfloat(r0.xyzw);
  o6.w = vd_asfloat(r0.z);
  r0.xyz = vd_asuint((vd_asfloat(v1.zxy) * vd_asfloat(v2.yzx)));
  r0.xyz = vd_asuint((vd_asfloat(v1.yzx) * vd_asfloat(v2.zxy) + (-(vd_asfloat(r0.xyz)))));
  r0.w = vd_asuint((-(int)(0.0 < vd_asfloat(v2.w))));
  r0.w = ((r0.w != 0u) ? 0x3f800000u : 0xbf800000u);
  r0.xyz = vd_asuint((vd_asfloat(r0.www) * vd_asfloat(r0.xyz)));
  r0.w = vd_asuint((-(int)(vd_asfloat(unity_WorldTransformParams.w) >= 0.0)));
  r0.w = ((r0.w != 0u) ? 0x3f800000u : 0xbf800000u);
  r0.xyz = vd_asuint((vd_asfloat(r0.www) * vd_asfloat(r0.xyz)));
  r0.w = vd_asuint(dot(vd_asfloat(r0.xyz), vd_asfloat(r0.xyz)));
  r0.w = vd_asuint(rsqrt(vd_asfloat(r0.w)));
  r0.xyz = vd_asuint((vd_asfloat(r0.www) * vd_asfloat(r0.xyz)));
  r1.xyz = vd_asuint((vd_asfloat(r0.yyy) * vd_asfloat((VD_COL(unity_ObjectToWorld, 1).xyz))));
  r0.xyw = vd_asuint((vd_asfloat((VD_COL(unity_ObjectToWorld, 0).xyz)) * vd_asfloat(r0.xxx) + vd_asfloat(r1.xyz)));
  r0.xyz = vd_asuint((vd_asfloat((VD_COL(unity_ObjectToWorld, 2).xyz)) * vd_asfloat(r0.zzz) + vd_asfloat(r0.xyw)));
  r0.w = vd_asuint(dot(vd_asfloat(r0.xyz), vd_asfloat(r0.xyz)));
  r0.w = vd_asuint(max(vd_asfloat(r0.w), 1.17549435e-38));
  r0.w = vd_asuint(rsqrt(vd_asfloat(r0.w)));
  o7.xyz = (vd_asfloat(r0.www) * vd_asfloat(r0.xyz));
  return;
}
