// Hidden/LCArt/Outline/Overlay | pass 0 "" | progVertex | keywords: _BILLBOARD | blob 3
void vert(
  float4 v0 : POSITION0,
  float2 v1 : TEXCOORD0,
  float4 v2 : COLOR0,
  out float4 o0 : SV_POSITION0,
  out float4 o1 : VDREG1)
{
  uint4 r0, r1, r2;
  r0 = 0u; r1 = 0u; r2 = 0u;

  r0.x = vd_asuint((-(int)(vd_asfloat(v0.y) >= 0.0)));
  r0.x = (r0.x & 0x3f800000u);
  r0.y = vd_asuint((1.0 / vd_asfloat((VD_COL(unity_MatrixV, 1).y))));
  r1.y = vd_asuint(min(abs(vd_asfloat(r0.y)), 1.5));
  r1.xz = uint2(0x00000000u, 0x00000000u);
  r2.x = vd_asuint((VD_COL(unity_MatrixV, 0).y));
  r2.y = vd_asuint((VD_COL(unity_MatrixV, 1).y));
  r2.z = vd_asuint((VD_COL(unity_MatrixV, 2).y));
  r0.yzw = vd_asuint((vd_asfloat(r1.xyz) + (-(vd_asfloat(r2.xyz)))));
  r0.xyz = vd_asuint((vd_asfloat(r0.xxx) * vd_asfloat(r0.yzw) + vd_asfloat(r2.xyz)));
  r1.xyz = vd_asuint(((-(vd_asfloat(_WorldSpaceCameraPos.xyz))) + vd_asfloat((VD_COL(unity_ObjectToWorld, 3).xyz))));
  r0.w = vd_asuint(dot(vd_asfloat(r1.xyz), vd_asfloat(r1.xyz)));
  r0.w = vd_asuint(rsqrt(vd_asfloat(r0.w)));
  r1.xyz = vd_asuint((vd_asfloat(r0.www) * vd_asfloat(r1.xyz)));
  r1.w = vd_asuint((-(vd_asfloat(r1.x))));
  r2.xy = vd_asuint((vd_asfloat(r1.zw) * vd_asfloat(v0.xx)));
  r2.xz = vd_asuint((vd_asfloat(r2.xy) * float2(1.17499995, 1.17499995)));
  r2.y = 0x00000000u;
  r0.xyz = vd_asuint((vd_asfloat(r0.xyz) * vd_asfloat(v0.yyy) + vd_asfloat(r2.xyz)));
  r0.w = vd_asuint((-(int)(vd_asfloat(v0.y) >= vd_asfloat(_OpticalRatioYThreshold))));
  r0.w = (r0.w & 0x3f800000u);
  r1.w = vd_asuint((vd_asfloat(_OpticalRatioYPositiveRatio) + -1.0));
  r0.w = vd_asuint((vd_asfloat(r0.w) * vd_asfloat(r1.w) + 1.0));
  r2.xyz = vd_asuint((vd_asfloat(r1.xyz) * vd_asfloat(v0.zzz)));
  r0.xyz = vd_asuint((vd_asfloat(r2.xyz) * vd_asfloat(r0.www) + vd_asfloat(r0.xyz)));
  r0.w = vd_asuint(max((-(vd_asfloat(r0.y))), 0.0));
  r1.xyz = vd_asuint((vd_asfloat(r0.www) * (-(vd_asfloat(r1.xyz)))));
  r0.xyz = vd_asuint((vd_asfloat(r1.xyz) * vd_asfloat((_OpticalRatioYNegativeUnit).xxx) + vd_asfloat(r0.xyz)));
  r1.xyzw = vd_asuint((vd_asfloat(r0.yyyy) * vd_asfloat((VD_COL(unity_ObjectToWorld, 1).xyzw))));
  r1.xyzw = vd_asuint((vd_asfloat((VD_COL(unity_ObjectToWorld, 0).xyzw)) * vd_asfloat(r0.xxxx) + vd_asfloat(r1.xyzw)));
  r0.xyzw = vd_asuint((vd_asfloat((VD_COL(unity_ObjectToWorld, 2).xyzw)) * vd_asfloat(r0.zzzz) + vd_asfloat(r1.xyzw)));
  r0.xyzw = vd_asuint((vd_asfloat(r0.xyzw) + vd_asfloat((VD_COL(unity_ObjectToWorld, 3).xyzw))));
  r1.xyzw = vd_asuint((vd_asfloat(r0.yyyy) * vd_asfloat((VD_COL(unity_MatrixVP, 1).xyzw))));
  r1.xyzw = vd_asuint((vd_asfloat((VD_COL(unity_MatrixVP, 0).xyzw)) * vd_asfloat(r0.xxxx) + vd_asfloat(r1.xyzw)));
  r1.xyzw = vd_asuint((vd_asfloat((VD_COL(unity_MatrixVP, 2).xyzw)) * vd_asfloat(r0.zzzz) + vd_asfloat(r1.xyzw)));
  r0.xyzw = vd_asuint((vd_asfloat((VD_COL(unity_MatrixVP, 3).xyzw)) * vd_asfloat(r0.wwww) + vd_asfloat(r1.xyzw)));
  r1.xy = vd_asuint((vd_asfloat(_OutlinePixelOffset.xy) + vd_asfloat(_OutlinePixelOffset.xy)));
  r1.xy = vd_asuint((vd_asfloat(r1.xy) / vd_asfloat(_ScreenParams.xy)));
  o0.xy = (vd_asfloat(r1.xy) * vd_asfloat(r0.ww) + vd_asfloat(r0.xy));
  o0.zw = vd_asfloat(r0.zw);
  o1.xy = vd_asfloat(vd_asuint(v1.xy));
  o1.z = vd_asfloat(vd_asuint(v2.w));
  return;
}
