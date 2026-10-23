// LCArt/Character/shader_DE_Character_Normal | pass 3 "RimLight" | progVertex | keywords: _ADDITIONAL_LIGHTS _DE_EDITOR_CAMERA _DE_PRERENDERING_CAMERA | blob 86
void vert(
  float4 v0 : POSITION0,
  float3 v1 : NORMAL0,
  float2 v2 : TEXCOORD0,
  float4 v3 : COLOR0,
  out float4 o0 : SV_POSITION0,
  out float4 o1 : VDREG1,
  out float4 o2 : VDREG2,
  out float4 o3 : VDREG3,
  out float4 o4 : VDREG4,
  out float4 o5 : VDREG5)
{
  uint4 r0, r1, r2, r3, r4;
  r0 = 0u; r1 = 0u; r2 = 0u; r3 = 0u; r4 = 0u;

  r0.x = vd_asuint((1.0 / vd_asfloat((VD_COL(unity_MatrixV, 1).y))));
  r0.y = vd_asuint(min(abs(vd_asfloat(r0.x)), 1.5));
  r1.xyz = vd_asuint(((-(vd_asfloat(_WorldSpaceCameraPos.xyz))) + vd_asfloat((VD_COL(unity_ObjectToWorld, 3).xyz))));
  r0.w = vd_asuint(dot(vd_asfloat(r1.xyz), vd_asfloat(r1.xyz)));
  r0.w = vd_asuint(rsqrt(vd_asfloat(r0.w)));
  r1.xyz = vd_asuint((vd_asfloat(r0.www) * vd_asfloat(r1.xyz)));
  r1.w = vd_asuint((-(vd_asfloat(r1.x))));
  r2.xy = vd_asuint((vd_asfloat(r1.zw) * vd_asfloat(v0.xx)));
  r0.w = vd_asuint((-(int)(vd_asfloat(v0.y) >= 0.0)));
  r0.w = (r0.w & 0x3f800000u);
  r3.x = vd_asuint((VD_COL(unity_MatrixV, 0).y));
  r3.y = vd_asuint((VD_COL(unity_MatrixV, 1).y));
  r3.z = vd_asuint((VD_COL(unity_MatrixV, 2).y));
  r0.xz = uint2(0x00000000u, 0x00000000u);
  r0.xyz = vd_asuint((vd_asfloat(r0.xyz) + (-(vd_asfloat(r3.xyz)))));
  r0.xyz = vd_asuint((vd_asfloat(r0.www) * vd_asfloat(r0.xyz) + vd_asfloat(r3.xyz)));
  r0.w = vd_asuint((-(int)(vd_asfloat(v0.y) >= vd_asfloat(_OpticalRatioYThreshold))));
  r0.w = (r0.w & 0x3f800000u);
  r1.w = vd_asuint((vd_asfloat(_OpticalRatioYPositiveRatio) + -1.0));
  r0.w = vd_asuint((vd_asfloat(r0.w) * vd_asfloat(r1.w) + 1.0));
  r2.xz = vd_asuint((vd_asfloat(r2.xy) * float2(1.17499995, 1.17499995)));
  r2.y = 0x00000000u;
  r0.xyz = vd_asuint((vd_asfloat(r0.xyz) * vd_asfloat(v0.yyy) + vd_asfloat(r2.xyz)));
  r0.w = vd_asuint((vd_asfloat(v0.z) * vd_asfloat(r0.w) + 0.00999999978));
  r0.xyz = vd_asuint((vd_asfloat(r1.xyz) * vd_asfloat(r0.www) + vd_asfloat(r0.xyz)));
  r1.xyz = vd_asuint((vd_asfloat(r0.yyy) * vd_asfloat((VD_COL(unity_ObjectToWorld, 1).xyz))));
  r0.xyw = vd_asuint((vd_asfloat((VD_COL(unity_ObjectToWorld, 0).xyz)) * vd_asfloat(r0.xxx) + vd_asfloat(r1.xyz)));
  r0.xyz = vd_asuint((vd_asfloat((VD_COL(unity_ObjectToWorld, 2).xyz)) * vd_asfloat(r0.zzz) + vd_asfloat(r0.xyw)));
  r0.xyz = vd_asuint((vd_asfloat(r0.xyz) + vd_asfloat((VD_COL(unity_ObjectToWorld, 3).xyz))));
  r1.xyzw = vd_asuint((vd_asfloat(r0.yyyy) * vd_asfloat((VD_COL(unity_MatrixVP, 1).xyzw))));
  r1.xyzw = vd_asuint((vd_asfloat((VD_COL(unity_MatrixVP, 0).xyzw)) * vd_asfloat(r0.xxxx) + vd_asfloat(r1.xyzw)));
  r1.xyzw = vd_asuint((vd_asfloat((VD_COL(unity_MatrixVP, 2).xyzw)) * vd_asfloat(r0.zzzz) + vd_asfloat(r1.xyzw)));
  r1.xyzw = vd_asuint((vd_asfloat(r1.xyzw) + vd_asfloat((VD_COL(unity_MatrixVP, 3).xyzw))));
  r2.xz = vd_asuint((vd_asfloat(r1.xw) * float2(0.5, 0.5)));
  r0.w = vd_asuint((vd_asfloat(r1.y) * vd_asfloat(_ProjectionParams.x)));
  r2.w = vd_asuint((vd_asfloat(r0.w) * 0.5));
  o4.xy = (vd_asfloat(r2.zz) + vd_asfloat(r2.xw));
  r0.w = vd_asuint(dot(vd_asfloat(_MainLightPosition.xyz), vd_asfloat(_MainLightPosition.xyz)));
  r0.w = vd_asuint(rsqrt(vd_asfloat(r0.w)));
  r2.xyz = vd_asuint((vd_asfloat(r0.www) * vd_asfloat(_MainLightPosition.xyz)));
  r3.xyz = uint3(0x00000000u, 0x00000000u, 0x00000000u);
  r0.w = 0x00000000u;
  while (true) {
    r2.w = vd_asuint((-(int)(vd_asint(r0.w) >= vd_asint(_RimEffectCount))));
    if (r2.w != 0u) break;
    r4.xyz = vd_asuint((vd_asfloat(r0.xyz) + (-(vd_asfloat(_RimEffectPositions[(int)(r0.w)].xyz)))));
    r2.w = vd_asuint(dot(vd_asfloat(r4.xyz), vd_asfloat(r4.xyz)));
    r3.w = vd_asuint(rsqrt(vd_asfloat(r2.w)));
    r4.xyz = vd_asuint((vd_asfloat(r3.www) * vd_asfloat(r4.xyz)));
    r2.w = vd_asuint(sqrt(vd_asfloat(r2.w)));
    r2.w = vd_asuint((vd_asfloat(r2.w) / vd_asfloat(_RimEffectRanges[(int)(r0.w)])));
    r2.w = vd_asuint(saturate(((-(vd_asfloat(r2.w))) + 1.0)));
    r3.xyz = vd_asuint((vd_asfloat(r4.xyz) * vd_asfloat(r2.www) + vd_asfloat(r3.xyz)));
    r0.w = vd_asuint((vd_asint(r0.w) + 1));
  }
  r2.xyz = vd_asuint((vd_asfloat(r2.xyz) * float3(-0.5, -0.5, -0.5) + vd_asfloat(r3.xyz)));
  r0.w = vd_asuint(dot(vd_asfloat(r2.xyz), vd_asfloat(r2.xyz)));
  r0.w = vd_asuint(rsqrt(vd_asfloat(r0.w)));
  r2.xyz = vd_asuint((vd_asfloat(r0.www) * vd_asfloat(r2.xyz)));
  r0.w = vd_asuint((-(int)(vd_asfloat(unity_OrthoParams.w) == 0.0)));
  r3.xyz = vd_asuint(((-(vd_asfloat(r0.xyz))) + vd_asfloat(_WorldSpaceCameraPos.xyz)));
  r4.x = ((r0.w != 0u) ? r3.x : vd_asuint((VD_COL(unity_MatrixV, 0).z)));
  r4.y = ((r0.w != 0u) ? r3.y : vd_asuint((VD_COL(unity_MatrixV, 1).z)));
  r4.z = ((r0.w != 0u) ? r3.z : vd_asuint((VD_COL(unity_MatrixV, 2).z)));
  r0.w = vd_asuint(dot(vd_asfloat(r4.xyz), vd_asfloat(r4.xyz)));
  r0.w = vd_asuint(rsqrt(vd_asfloat(r0.w)));
  r3.xyz = vd_asuint((vd_asfloat(r0.www) * vd_asfloat(r4.xyz)));
  r0.w = vd_asuint(dot(vd_asfloat(r2.xyz), vd_asfloat(r3.xyz)));
  r2.xyz = vd_asuint(((-(vd_asfloat(r0.www))) * vd_asfloat(r3.xyz) + vd_asfloat(r2.xyz)));
  r0.w = vd_asuint(dot((-(vd_asfloat(r2.xyz))), (-(vd_asfloat(r2.xyz)))));
  r0.w = vd_asuint(rsqrt(vd_asfloat(r0.w)));
  r2.xyz = vd_asuint((vd_asfloat(r0.www) * (-(vd_asfloat(r2.xyz)))));
  r3.x = vd_asuint((vd_asfloat(_ScreenParams.x) / vd_asfloat(_ScreenParams.y)));
  r2.w = vd_asuint((vd_asfloat(r2.x) * 0.0125000002));
  r3.yz = uint2(0x3c4ccccdu, 0x3c4ccccdu);
  r2.xyz = vd_asuint((vd_asfloat(r2.wyz) * vd_asfloat(r3.xyz) + vd_asfloat(r0.xyz)));
  r2.yw = vd_asuint((vd_asfloat(r2.yy) * vd_asfloat((VD_COL(unity_MatrixVP, 1).xy))));
  r2.xy = vd_asuint((vd_asfloat((VD_COL(unity_MatrixVP, 0).xy)) * vd_asfloat(r2.xx) + vd_asfloat(r2.yw)));
  r2.xy = vd_asuint((vd_asfloat((VD_COL(unity_MatrixVP, 2).xy)) * vd_asfloat(r2.zz) + vd_asfloat(r2.xy)));
  r1.xy = vd_asuint((vd_asfloat(r2.xy) + vd_asfloat((VD_COL(unity_MatrixVP, 3).xy))));
  r2.xyz = vd_asuint((vd_asfloat(v1.yyy) * vd_asfloat((VD_COL(unity_ObjectToWorld, 1).xyz))));
  r2.xyz = vd_asuint((vd_asfloat((VD_COL(unity_ObjectToWorld, 0).xyz)) * vd_asfloat(v1.xxx) + vd_asfloat(r2.xyz)));
  r2.xyz = vd_asuint((vd_asfloat((VD_COL(unity_ObjectToWorld, 2).xyz)) * vd_asfloat(v1.zzz) + vd_asfloat(r2.xyz)));
  o1.xyz = (vd_asfloat(r2.xyz) + vd_asfloat((VD_COL(unity_ObjectToWorld, 3).xyz)));
  r2.xyz = uint3(0x00000000u, 0x00000000u, 0x00000000u);
  r0.w = 0x00000000u;
  while (true) {
    r2.w = vd_asuint((-(int)(vd_asint(r0.w) >= vd_asint(_RimEffectCount))));
    if (r2.w != 0u) break;
    r3.xyz = vd_asuint((vd_asfloat(r0.xyz) + (-(vd_asfloat(_RimEffectPositions[(int)(r0.w)].xyz)))));
    r2.w = vd_asuint(dot(vd_asfloat(r3.xyz), vd_asfloat(r3.xyz)));
    r2.w = vd_asuint(sqrt(vd_asfloat(r2.w)));
    r2.w = vd_asuint(saturate((vd_asfloat(r2.w) / vd_asfloat(_RimEffectRanges[(int)(r0.w)]))));
    r2.w = vd_asuint(((-(vd_asfloat(r2.w))) + 1.0));
    r3.xyz = vd_asuint((vd_asfloat(r2.www) * vd_asfloat(_RimEffectColors[(int)(r0.w)].xyz)));
    r2.xyz = vd_asuint(max(vd_asfloat(r2.xyz), vd_asfloat(r3.xyz)));
    r0.w = vd_asuint((vd_asint(r0.w) + 1));
  }
  r0.xyz = vd_asuint((vd_asfloat(r2.xyz) * float3(0.5, 0.5, 0.5)));
  o3.xyz = max(vd_asfloat(r0.xyz), vd_asfloat(_MainLightColor.xyz));
  r0.xyzw = vd_asuint(((-(vd_asfloat(r1.xyzw))) + float4(0.0, 0.0, 0.0, 1.0)));
  o0.xyzw = (vd_asfloat((_OutlineEnable).xxxx) * vd_asfloat(r0.xyzw) + vd_asfloat(r1.xyzw));
  o4.zw = vd_asfloat(r1.zw);
  o5.xyzw = vd_asfloat(vd_asuint(v3.xyzw));
  o2.xy = vd_asfloat(vd_asuint(v2.xy));
  o2.z = vd_asfloat(0x3f800000u);
  return;
}
