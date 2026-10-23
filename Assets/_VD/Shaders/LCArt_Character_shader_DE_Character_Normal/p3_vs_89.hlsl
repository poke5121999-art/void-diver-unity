// LCArt/Character/shader_DE_Character_Normal | pass 3 "RimLight" | progVertex | keywords: _CLUSTER_LIGHT_LOOP _DE_EDITOR_CAMERA | blob 89
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
  uint4 r0, r1, r2, r3, r4, r5;
  r0 = 0u; r1 = 0u; r2 = 0u; r3 = 0u; r4 = 0u; r5 = 0u;

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
  r2.xy = vd_asuint((vd_asfloat(r2.zz) + vd_asfloat(r2.xw)));
  r0.w = vd_asuint(dot(vd_asfloat(_MainLightPosition.xyz), vd_asfloat(_MainLightPosition.xyz)));
  r0.w = vd_asuint(rsqrt(vd_asfloat(r0.w)));
  r3.xyz = vd_asuint((vd_asfloat(r0.www) * vd_asfloat(_MainLightPosition.xyz)));
  r4.xyz = uint3(0x00000000u, 0x00000000u, 0x00000000u);
  r0.w = 0x00000000u;
  while (true) {
    r3.w = vd_asuint((-(int)(vd_asint(r0.w) >= vd_asint(_RimEffectCount))));
    if (r3.w != 0u) break;
    r5.xyz = vd_asuint((vd_asfloat(r0.xyz) + (-(vd_asfloat(_RimEffectPositions[(int)(r0.w)].xyz)))));
    r3.w = vd_asuint(dot(vd_asfloat(r5.xyz), vd_asfloat(r5.xyz)));
    r4.w = vd_asuint(rsqrt(vd_asfloat(r3.w)));
    r5.xyz = vd_asuint((vd_asfloat(r4.www) * vd_asfloat(r5.xyz)));
    r3.w = vd_asuint(sqrt(vd_asfloat(r3.w)));
    r3.w = vd_asuint((vd_asfloat(r3.w) / vd_asfloat(_RimEffectRanges[(int)(r0.w)])));
    r3.w = vd_asuint(saturate(((-(vd_asfloat(r3.w))) + 1.0)));
    r4.xyz = vd_asuint((vd_asfloat(r5.xyz) * vd_asfloat(r3.www) + vd_asfloat(r4.xyz)));
    r0.w = vd_asuint((vd_asint(r0.w) + 1));
  }
  r3.xyz = vd_asuint((vd_asfloat(r3.xyz) * float3(-0.5, -0.5, -0.5) + vd_asfloat(r4.xyz)));
  r0.w = vd_asuint(dot(vd_asfloat(r3.xyz), vd_asfloat(r3.xyz)));
  r0.w = vd_asuint(rsqrt(vd_asfloat(r0.w)));
  r3.xyz = vd_asuint((vd_asfloat(r0.www) * vd_asfloat(r3.xyz)));
  r0.w = vd_asuint((-(int)(vd_asfloat(unity_OrthoParams.w) == 0.0)));
  r4.xyz = vd_asuint(((-(vd_asfloat(r0.xyz))) + vd_asfloat(_WorldSpaceCameraPos.xyz)));
  r5.x = ((r0.w != 0u) ? r4.x : vd_asuint((VD_COL(unity_MatrixV, 0).z)));
  r5.y = ((r0.w != 0u) ? r4.y : vd_asuint((VD_COL(unity_MatrixV, 1).z)));
  r5.z = ((r0.w != 0u) ? r4.z : vd_asuint((VD_COL(unity_MatrixV, 2).z)));
  r0.w = vd_asuint(dot(vd_asfloat(r5.xyz), vd_asfloat(r5.xyz)));
  r0.w = vd_asuint(rsqrt(vd_asfloat(r0.w)));
  r4.xyz = vd_asuint((vd_asfloat(r0.www) * vd_asfloat(r5.xyz)));
  r0.w = vd_asuint(dot(vd_asfloat(r3.xyz), vd_asfloat(r4.xyz)));
  r3.xyz = vd_asuint(((-(vd_asfloat(r0.www))) * vd_asfloat(r4.xyz) + vd_asfloat(r3.xyz)));
  r0.w = vd_asuint(dot((-(vd_asfloat(r3.xyz))), (-(vd_asfloat(r3.xyz)))));
  r0.w = vd_asuint(rsqrt(vd_asfloat(r0.w)));
  r3.xyz = vd_asuint((vd_asfloat(r0.www) * (-(vd_asfloat(r3.xyz)))));
  r4.x = vd_asuint((vd_asfloat(_ScreenParams.x) / vd_asfloat(_ScreenParams.y)));
  r3.w = vd_asuint((vd_asfloat(r3.x) * 0.0125000002));
  r4.yz = uint2(0x3c4ccccdu, 0x3c4ccccdu);
  r3.xyz = vd_asuint((vd_asfloat(r3.wyz) * vd_asfloat(r4.xyz) + vd_asfloat(r0.xyz)));
  r3.yw = vd_asuint((vd_asfloat(r3.yy) * vd_asfloat((VD_COL(unity_MatrixVP, 1).xy))));
  r3.xy = vd_asuint((vd_asfloat((VD_COL(unity_MatrixVP, 0).xy)) * vd_asfloat(r3.xx) + vd_asfloat(r3.yw)));
  r3.xy = vd_asuint((vd_asfloat((VD_COL(unity_MatrixVP, 2).xy)) * vd_asfloat(r3.zz) + vd_asfloat(r3.xy)));
  r1.xy = vd_asuint((vd_asfloat(r3.xy) + vd_asfloat((VD_COL(unity_MatrixVP, 3).xy))));
  r3.xyz = vd_asuint((vd_asfloat(v1.yyy) * vd_asfloat((VD_COL(unity_ObjectToWorld, 1).xyz))));
  r3.xyz = vd_asuint((vd_asfloat((VD_COL(unity_ObjectToWorld, 0).xyz)) * vd_asfloat(v1.xxx) + vd_asfloat(r3.xyz)));
  r3.xyz = vd_asuint((vd_asfloat((VD_COL(unity_ObjectToWorld, 2).xyz)) * vd_asfloat(v1.zzz) + vd_asfloat(r3.xyz)));
  o1.xyz = (vd_asfloat(r3.xyz) + vd_asfloat((VD_COL(unity_ObjectToWorld, 3).xyz)));
  r3.xyz = uint3(0x00000000u, 0x00000000u, 0x00000000u);
  r0.w = 0x00000000u;
  while (true) {
    r3.w = vd_asuint((-(int)(vd_asint(r0.w) >= vd_asint(_RimEffectCount))));
    if (r3.w != 0u) break;
    r4.xyz = vd_asuint((vd_asfloat(r0.xyz) + (-(vd_asfloat(_RimEffectPositions[(int)(r0.w)].xyz)))));
    r3.w = vd_asuint(dot(vd_asfloat(r4.xyz), vd_asfloat(r4.xyz)));
    r3.w = vd_asuint(sqrt(vd_asfloat(r3.w)));
    r3.w = vd_asuint(saturate((vd_asfloat(r3.w) / vd_asfloat(_RimEffectRanges[(int)(r0.w)]))));
    r3.w = vd_asuint(((-(vd_asfloat(r3.w))) + 1.0));
    r4.xyz = vd_asuint((vd_asfloat(r3.www) * vd_asfloat(_RimEffectColors[(int)(r0.w)].xyz)));
    r3.xyz = vd_asuint(max(vd_asfloat(r3.xyz), vd_asfloat(r4.xyz)));
    r0.w = vd_asuint((vd_asint(r0.w) + 1));
  }
  r0.xyz = vd_asuint((vd_asfloat(r3.xyz) * float3(0.5, 0.5, 0.5)));
  o3.xyz = max(vd_asfloat(r0.xyz), vd_asfloat(_MainLightColor.xyz));
  r0.xy = vd_asuint(((-(vd_asfloat(_PlaneBoundMaxPos.xz))) + vd_asfloat((VD_COL(unity_ObjectToWorld, 3).xz))));
  r0.zw = vd_asuint((vd_asfloat(_PlaneBoundMinPos.xz) + (-(vd_asfloat(_PlaneBoundMaxPos.xz)))));
  r0.xy = vd_asuint(saturate((vd_asfloat(r0.xy) / vd_asfloat(r0.zw))));
  r3.xyzw = vd_asuint(_PlaneSightRenderTarget.SampleLevel(sampler_PlaneSightRenderTarget, vd_asfloat(r0.xy), 0.0).xyzw);
  r0.xyzw = vd_asuint(_PlanePointBuffer.SampleLevel(sampler_PlanePointBuffer, vd_asfloat(r0.xy), 0.0).xyzw);
  r0.x = vd_asuint(saturate((vd_asfloat(r0.x) * 4.0)));
  r0.y = vd_asuint((vd_asfloat(r0.x) * -2.0 + 3.0));
  r0.x = vd_asuint((vd_asfloat(r0.x) * vd_asfloat(r0.x)));
  r0.x = vd_asuint((vd_asfloat(r0.x) * vd_asfloat(r0.y)));
  r0.x = vd_asuint((vd_asfloat(r0.x) * vd_asfloat(_GlobalPlanePointBufferEnable)));
  r0.x = vd_asuint(max(vd_asfloat(r0.x), vd_asfloat(r3.w)));
  r0.y = vd_asuint((vd_asfloat(_GlobalIsRuntime) * vd_asfloat(_GlobalPlaneSystemEnable)));
  r0.x = vd_asuint((vd_asfloat(r0.x) + -1.0));
  r0.x = vd_asuint((vd_asfloat(r0.y) * vd_asfloat(r0.x) + 1.0));
  r0.x = vd_asuint(max(vd_asfloat(r0.x), 0.5));
  r0.y = vd_asuint((-(int)(vd_asfloat(r0.x) >= 0.949999988)));
  r0.y = (r0.y & 0x3f800000u);
  r3.xyzw = vd_asuint((vd_asfloat(r1.xyzw) + float4(-0.0, -0.0, -0.0, -1.0)));
  r3.xyzw = vd_asuint((vd_asfloat(r0.yyyy) * vd_asfloat(r3.xyzw) + float4(0.0, 0.0, 0.0, 1.0)));
  r2.zw = r1.zw;
  r1.xyzw = vd_asuint((vd_asfloat(r2.xyzw) + float4(-0.0, -0.0, -0.0, -1.0)));
  o4.xyzw = (vd_asfloat(r0.yyyy) * vd_asfloat(r1.xyzw) + float4(0.0, 0.0, 0.0, 1.0));
  r1.xyzw = vd_asuint(((-(vd_asfloat(r3.xyzw))) + float4(0.0, 0.0, 0.0, 1.0)));
  o0.xyzw = (vd_asfloat((_OutlineEnable).xxxx) * vd_asfloat(r1.xyzw) + vd_asfloat(r3.xyzw));
  o5.xyzw = vd_asfloat(vd_asuint(v3.xyzw));
  o2.xy = vd_asfloat(vd_asuint(v2.xy));
  o2.z = vd_asfloat(r0.x);
  return;
}
