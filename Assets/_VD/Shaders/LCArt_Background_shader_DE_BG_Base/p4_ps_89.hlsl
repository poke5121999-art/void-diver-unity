// LCArt/Background/shader_DE_BG_Base | pass 4 "Glass" | progFragment | keywords: _ADDITIONAL_LIGHTS _LIGHT_LAYERS | blob 89
void frag(
  float4 v0 : VDREG0,
  float4 v1 : VDREG1,
  float4 v2 : VDREG2,
  float4 v3 : VDREG3,
  float4 v4 : VDREG4,
  float4 v5 : VDREG5,
  float4 v6 : VDREG6,
  float4 v7 : SV_POSITION0,
  float4 v8 : VDREG8,
  float4 v9 : VDREG9,
  float4 v10 : VDREG10,
  float4 v11 : VDREG11,
  out float4 o0 : SV_Target0)
{
#if UNITY_UV_STARTS_AT_TOP
  uint4 r0, r1, r2, r3, r4, r5, r6, r7, r8, r9, r10;
  r0 = 0u; r1 = 0u; r2 = 0u; r3 = 0u; r4 = 0u; r5 = 0u; r6 = 0u; r7 = 0u; r8 = 0u; r9 = 0u; r10 = 0u;
#else
  uint4 r0, r1, r2, r3, r4, r5, r6, r7, r8, r9, r10, r11;
  r0 = 0u; r1 = 0u; r2 = 0u; r3 = 0u; r4 = 0u; r5 = 0u; r6 = 0u; r7 = 0u; r8 = 0u; r9 = 0u; r10 = 0u; r11 = 0u;
#endif
  uint4 x0[16];
  const uint4 icb[4] = { uint4(0x3f800000u, 0x00000000u, 0x00000000u, 0x00000000u), uint4(0x00000000u, 0x3f800000u, 0x00000000u, 0x00000000u), uint4(0x00000000u, 0x00000000u, 0x3f800000u, 0x00000000u), uint4(0x00000000u, 0x00000000u, 0x00000000u, 0x3f800000u) };

#if UNITY_UV_STARTS_AT_TOP
#else
  r11.xyzw = vd_asuint(v7.xyzw);
  r11.z = vd_asuint(((-(vd_asfloat(v7.z))) + 1.0));
#endif
  x0[0].x = 0x3d70f16fu;
  x0[1].x = 0x3f07878bu;
  x0[2].x = 0x3e34b4d0u;
  x0[3].x = 0x3f25a5a9u;
  x0[4].x = 0x3f43c3c6u;
  x0[5].x = 0x3e9696a2u;
  x0[6].x = 0x3f61e1e3u;
  x0[7].x = 0x3ed2d2ddu;
  x0[8].x = 0x3e70f0e9u;
  x0[9].x = 0x3f34b4afu;
  x0[10].x = 0x3df0f0e9u;
  x0[11].x = 0x3f169692u;
  x0[12].x = 0x3f70f0e9u;
  x0[13].x = 0x3ef0f0e9u;
  x0[14].x = 0x3f52d2ccu;
  x0[15].x = 0x3eb4b4afu;
  r0.xyzw = vd_asuint(_EmissionMap.SampleBias(sampler_vd_point_repeat, vd_asfloat(v0.xy), vd_asfloat(_GlobalMipBias.x)).xyzw);
  r0.y = vd_asuint((-(int)(vd_asfloat(r0.z) < 0.5)));
  if (r0.y != 0u) discard;
  r1.xyzw = vd_asuint(_BaseMap.SampleBias(sampler_BaseMap, vd_asfloat(v0.xy), vd_asfloat(_GlobalMipBias.x)).xyzw);
  r1.xyz = vd_asuint((vd_asfloat(r1.xyz) * vd_asfloat(_BaseColor.xyz)));
  r2.xyzw = vd_asuint(_BumpMap.SampleBias(sampler_vd_point_repeat, vd_asfloat(v0.xy), vd_asfloat(_GlobalMipBias.x)).xyzw);
  r2.x = vd_asuint((vd_asfloat(r2.x) * vd_asfloat(r2.w)));
  r3.xy = vd_asuint((vd_asfloat(r2.xy) + vd_asfloat(r2.xy)));
  r0.yw = vd_asuint((vd_asfloat(r2.xy) * float2(2.0, 2.0) + float2(-1.0, -1.0)));
  r0.y = vd_asuint(dot(vd_asfloat(r0.yw), vd_asfloat(r0.yw)));
  r0.y = vd_asuint(min(vd_asfloat(r0.y), 1.0));
  r0.y = vd_asuint(((-(vd_asfloat(r0.y))) + 1.0));
  r0.y = vd_asuint(sqrt(vd_asfloat(r0.y)));
  r3.z = vd_asuint(max(vd_asfloat(r0.y), 1.00000002e-16));
  r2.xyz = vd_asuint((vd_asfloat(r3.xyz) + float3(-1.0, -1.0, -1.0)));
  r2.xyz = vd_asuint((vd_asfloat((_BumpScale).xxx) * vd_asfloat(r2.xyz) + float3(0.0, 0.0, 1.0)));
  r0.yw = vd_asuint((-(int2)(float2(1.5, 0.5) < vd_asfloat(v1.wz))));
  if (r0.y != 0u) {
    r3.xy = vd_asuint((float2(1.0, 1.0) / vd_asfloat(_ScaledScreenParams.xy)));
#if UNITY_UV_STARTS_AT_TOP
    r3.xy = vd_asuint((vd_asfloat(r3.xy) * vd_asfloat(v7.xy)));
    r0.y = vd_asuint((vd_asfloat(r3.y) * vd_asfloat(_ScaleBiasRt.x) + vd_asfloat(_ScaleBiasRt.y)));
    r3.z = vd_asuint(((-(vd_asfloat(r0.y))) + 1.0));
#else
    r3.xy = vd_asuint((vd_asfloat(r3.xy) * vd_asfloat(r11.xy)));
    r0.y = r3.y;
    r3.z = r0.y;
#endif
    r4.xyzw = vd_asuint((-(int4)(vd_asint((_BackgroundType).xxxx) == int4(2, 4, 3, 9))));
    r0.y = (r4.y | r4.x);
    r0.y = (r4.z | r0.y);
    if (r0.y != 0u) {
      r5.xyzw = vd_asuint(_CustomDepthBuffer.SampleBias(sampler_CustomDepthBuffer, vd_asfloat(r3.xz), vd_asfloat(_GlobalMipBias.x)).xyzw);
#if UNITY_UV_STARTS_AT_TOP
      r0.y = vd_asuint(((-(vd_asfloat(r5.z))) + vd_asfloat(v7.z)));
#else
      r0.y = vd_asuint(((-(vd_asfloat(r5.z))) + vd_asfloat(r11.z)));
#endif
      r0.y = vd_asuint((vd_asfloat(r0.y) + -2.49999994e-05));
      r0.y = vd_asuint(saturate((vd_asfloat(r0.y) * -20000.0)));
      r1.w = vd_asuint((vd_asfloat(r0.y) * -2.0 + 3.0));
      r0.y = vd_asuint((vd_asfloat(r0.y) * vd_asfloat(r0.y)));
      r2.w = vd_asuint((vd_asfloat(r0.y) * vd_asfloat(r1.w)));
      r3.yw = vd_asuint(((-(vd_asfloat(float2(_GlobalSeeThroughCircleRange, _GlobalSeeThroughCircleAlpha)))) + float2(1.0, 1.0)));
      r5.x = vd_asuint(((-(vd_asfloat(r3.y))) + 1.0));
      r3.y = vd_asuint(((-(vd_asfloat(r3.y))) + vd_asfloat(r5.w)));
      r5.x = vd_asuint((1.0 / vd_asfloat(r5.x)));
      r3.y = vd_asuint(saturate((vd_asfloat(r3.y) * vd_asfloat(r5.x))));
      r5.x = vd_asuint((vd_asfloat(r3.y) * -2.0 + 3.0));
      r3.y = vd_asuint((vd_asfloat(r3.y) * vd_asfloat(r3.y)));
      r3.y = vd_asuint((vd_asfloat(r3.y) * vd_asfloat(r5.x)));
      r0.y = vd_asuint(((-(vd_asfloat(r1.w))) * vd_asfloat(r0.y) + 1.0));
      r0.y = vd_asuint((vd_asfloat(r3.y) * vd_asfloat(r0.y) + vd_asfloat(r2.w)));
      r1.w = vd_asuint((vd_asfloat(_GlobalBackgroundSeeThroughEnable) * vd_asfloat(_GlobalPlaneSystemEnable)));
      r0.y = vd_asuint((vd_asfloat(r0.y) + -1.0));
      r0.y = vd_asuint((vd_asfloat(r1.w) * vd_asfloat(r0.y) + 1.0));
      r0.y = vd_asuint((vd_asfloat(r0.y) * vd_asfloat(r3.w) + vd_asfloat(_GlobalSeeThroughCircleAlpha)));
      r3.yw = vd_asuint((vd_asfloat(_ScreenSize.xy) / vd_asfloat(_ScreenParams.xy)));
      r5.xy = vd_asuint((vd_asfloat(r3.xz) * vd_asfloat(_ScreenParams.xy)));
      r3.yw = vd_asuint((vd_asfloat(r3.yw) * vd_asfloat(r5.xy)));
      r3.yw = vd_asuint(floor(vd_asfloat(r3.yw)));
      r3.yw = ((uint2)vd_asfloat(r3.yw));
      r3.yw = (r3.yw & uint2(0x00000003u, 0x00000003u));
      r1.w = (r3.w << (uint)2);
      r1.w = vd_asuint((vd_asint(r1.w) + vd_asint(r3.y)));
      r1.w = x0[r1.w + 0].x;
      r0.y = vd_asuint((vd_asfloat(r0.y) + (-(vd_asfloat(r1.w)))));
      r0.y = vd_asuint((-(int)(vd_asfloat(r0.y) < 0.0)));
      if (r0.y != 0u) discard;
    }
    r0.y = vd_asuint((-(int)(1.5 < vd_asfloat(v1.z))));
    if (r0.y != 0u) {
      r5.xyzw = vd_asuint(_CustomDepthBuffer.SampleBias(sampler_CustomDepthBuffer, vd_asfloat(r3.xz), vd_asfloat(_GlobalMipBias.x)).xyzw);
#if UNITY_UV_STARTS_AT_TOP
      r0.y = vd_asuint(((-(vd_asfloat(r5.z))) + vd_asfloat(v7.z)));
#else
      r0.y = vd_asuint(((-(vd_asfloat(r5.z))) + vd_asfloat(r11.z)));
#endif
      r0.y = vd_asuint((vd_asfloat(r0.y) + -2.49999994e-05));
      r0.y = vd_asuint(saturate((vd_asfloat(r0.y) * -20000.0)));
      r1.w = vd_asuint((vd_asfloat(r0.y) * -2.0 + 3.0));
      r0.y = vd_asuint((vd_asfloat(r0.y) * vd_asfloat(r0.y)));
      r2.w = vd_asuint((vd_asfloat(r0.y) * vd_asfloat(r1.w)));
      r3.yw = vd_asuint(((-(vd_asfloat(float2(_GlobalSeeThroughCircleRange, _GlobalSeeThroughCircleAlpha)))) + float2(1.0, 1.0)));
      r5.x = vd_asuint(((-(vd_asfloat(r3.y))) + 1.0));
      r3.y = vd_asuint(((-(vd_asfloat(r3.y))) + vd_asfloat(r5.w)));
      r5.x = vd_asuint((1.0 / vd_asfloat(r5.x)));
      r3.y = vd_asuint(saturate((vd_asfloat(r3.y) * vd_asfloat(r5.x))));
      r5.x = vd_asuint((vd_asfloat(r3.y) * -2.0 + 3.0));
      r3.y = vd_asuint((vd_asfloat(r3.y) * vd_asfloat(r3.y)));
      r3.y = vd_asuint((vd_asfloat(r3.y) * vd_asfloat(r5.x)));
      r0.y = vd_asuint(((-(vd_asfloat(r1.w))) * vd_asfloat(r0.y) + 1.0));
      r0.y = vd_asuint((vd_asfloat(r3.y) * vd_asfloat(r0.y) + vd_asfloat(r2.w)));
      r1.w = vd_asuint((vd_asfloat(_GlobalBackgroundSeeThroughEnable) * vd_asfloat(_GlobalPlaneSystemEnable)));
      r0.y = vd_asuint((vd_asfloat(r0.y) + -1.0));
      r0.y = vd_asuint((vd_asfloat(r1.w) * vd_asfloat(r0.y) + 1.0));
      r0.y = vd_asuint((vd_asfloat(r0.y) * vd_asfloat(r3.w) + vd_asfloat(_GlobalSeeThroughCircleAlpha)));
      r3.yw = vd_asuint((vd_asfloat(_ScreenSize.xy) / vd_asfloat(_ScreenParams.xy)));
      r5.xy = vd_asuint((vd_asfloat(r3.xz) * vd_asfloat(_ScreenParams.xy)));
      r3.yw = vd_asuint((vd_asfloat(r3.yw) * vd_asfloat(r5.xy)));
      r3.yw = vd_asuint(floor(vd_asfloat(r3.yw)));
      r3.yw = ((uint2)vd_asfloat(r3.yw));
      r3.yw = (r3.yw & uint2(0x00000003u, 0x00000003u));
      r2.w = (r3.w << (uint)2);
      r2.w = vd_asuint((vd_asint(r2.w) + vd_asint(r3.y)));
      r2.w = x0[r2.w + 0].x;
      r0.y = vd_asuint((vd_asfloat(r0.y) + (-(vd_asfloat(r2.w)))));
      r0.y = vd_asuint((-(int)(vd_asfloat(r0.y) < 0.0)));
      if (r0.y != 0u) discard;
      r0.y = vd_asuint((-(int)(0.125 < vd_asfloat(v2.y))));
      if (r0.y != 0u) {
        r5.xyzw = vd_asuint(_CharacterAreaMaskTexture.SampleLevel(sampler_CharacterAreaMaskTexture, vd_asfloat(r3.xz), 0.0).xyzw);
        r3.yw = vd_asuint(round(vd_asfloat(_WorldSpaceCameraPos.xz)));
        r3.yw = vd_asuint(((-(vd_asfloat(r3.yw))) + vd_asfloat(v2.xz)));
        r6.x = vd_asuint((-(vd_asfloat((VD_COL(unity_MatrixV, 0).z)))));
        r6.y = vd_asuint((-(vd_asfloat((VD_COL(unity_MatrixV, 2).z)))));
        r0.y = vd_asuint(dot(vd_asfloat(r6.xy), vd_asfloat(r6.xy)));
        r0.y = vd_asuint(rsqrt(vd_asfloat(r0.y)));
        r5.zw = vd_asuint((vd_asfloat(r0.yy) * vd_asfloat(r6.xy)));
        r0.y = vd_asuint(dot(vd_asfloat(r3.yw), vd_asfloat(r5.zw)));
        r0.y = vd_asuint(saturate((vd_asfloat(r0.y) * 0.00999999978)));
        r0.y = vd_asuint(((-(vd_asfloat(r0.y))) + vd_asfloat(r5.y)));
        r0.y = vd_asuint(saturate((vd_asfloat(r0.y) * 40.0)));
        r2.w = vd_asuint((vd_asfloat(r0.y) * -2.0 + 3.0));
        r0.y = vd_asuint((vd_asfloat(r0.y) * vd_asfloat(r0.y)));
        r0.y = vd_asuint((vd_asfloat(r0.y) * vd_asfloat(r2.w)));
        r2.w = vd_asuint((vd_asfloat(r5.x) + -1.0));
        r0.y = vd_asuint((vd_asfloat(r0.y) * vd_asfloat(r2.w)));
        r0.y = vd_asuint((vd_asfloat(_GlobalBackgroundSeeThroughEnable) * vd_asfloat(r0.y) + 1.0));
        r5.xyz = vd_asuint((vd_asfloat(v2.xyz) + (-(vd_asfloat(_WorldSpaceCameraPos.xyz)))));
        r2.w = vd_asuint(((-(vd_asfloat(_WorldSpaceCameraPos.y))) + -0.150000006));
        r2.w = vd_asuint((vd_asfloat(r2.w) / vd_asfloat(r5.y)));
        r3.yw = vd_asuint((vd_asfloat(r5.xz) * vd_asfloat(r2.ww) + vd_asfloat(_WorldSpaceCameraPos.xz)));
        r3.yw = vd_asuint((vd_asfloat(r3.yw) + (-(vd_asfloat(_PlaneBoundMaxPos.xz)))));
        r5.xy = vd_asuint((vd_asfloat(_PlaneBoundMinPos.xz) + (-(vd_asfloat(_PlaneBoundMaxPos.xz)))));
        r3.yw = vd_asuint(saturate((vd_asfloat(r3.yw) / vd_asfloat(r5.xy))));
        r5.xyzw = vd_asuint(_PlaneSightRenderTarget.SampleLevel(sampler_PlaneSightRenderTarget, vd_asfloat(r3.yw), 0.0).xyzw);
        r2.w = vd_asuint((vd_asfloat(r5.w) + -1.0));
        r2.w = vd_asuint((vd_asfloat(_GlobalPlaneSystemEnable) * vd_asfloat(r2.w) + 1.0));
        r3.y = vd_asuint(((-(vd_asfloat(_SeeThroughDistanceMin))) + vd_asfloat(_SeeThroughDistanceMax)));
        r3.y = vd_asuint((1.0 / vd_asfloat(r3.y)));
        r3.w = 0x3f800000u;
        r5.x = 0x00000000u;
        while (true) {
          r5.y = vd_asuint((-(int)(vd_asint(r5.x) >= 3)));
          if (r5.y != 0u) break;
          r5.yzw = vd_asuint((vd_asfloat(v2.xyz) + (-(vd_asfloat(_SightPositions[(int)(r5.x)].xyz)))));
          r5.y = vd_asuint(dot(vd_asfloat(r5.yzw), vd_asfloat(r5.yzw)));
          r5.y = vd_asuint(sqrt(vd_asfloat(r5.y)));
          r5.y = vd_asuint((vd_asfloat(r5.y) + (-(vd_asfloat(_SeeThroughDistanceMin)))));
          r5.y = vd_asuint(saturate((vd_asfloat(r3.y) * vd_asfloat(r5.y))));
          r5.z = vd_asuint((vd_asfloat(r5.y) * -2.0 + 3.0));
          r5.y = vd_asuint((vd_asfloat(r5.y) * vd_asfloat(r5.y)));
          r5.y = vd_asuint((vd_asfloat(r5.y) * vd_asfloat(r5.z)));
          r3.w = vd_asuint(min(vd_asfloat(r3.w), vd_asfloat(r5.y)));
          r5.x = vd_asuint((vd_asint(r5.x) + 1));
        }
        r2.w = vd_asuint((vd_asfloat(r2.w) * vd_asfloat(r2.w)));
        r3.y = vd_asuint(((-(vd_asfloat(r3.w))) + 1.0));
        r2.w = vd_asuint(((-(vd_asfloat(r2.w))) * vd_asfloat(r3.y) + 1.0));
        r2.w = vd_asuint(max(vd_asfloat(r2.w), 0.0));
        r2.w = vd_asuint((vd_asfloat(r2.w) + -1.0));
        r1.w = vd_asuint((vd_asfloat(r1.w) * vd_asfloat(r2.w) + 1.0));
        r0.y = vd_asuint(min(vd_asfloat(r0.y), vd_asfloat(r1.w)));
      } else {
        r0.y = vd_asuint(v0.z);
      }
      r1.w = vd_asuint(((-(vd_asfloat(_GlobalSeeThroughHeightMin))) + vd_asfloat(_GlobalSeeThroughHeightMax)));
      r2.w = vd_asuint((vd_asfloat(v2.y) + (-(vd_asfloat(_GlobalSeeThroughHeightMin)))));
      r1.w = vd_asuint((1.0 / vd_asfloat(r1.w)));
      r1.w = vd_asuint(saturate((vd_asfloat(r1.w) * vd_asfloat(r2.w))));
      r2.w = vd_asuint((vd_asfloat(r1.w) * -2.0 + 3.0));
      r1.w = vd_asuint((vd_asfloat(r1.w) * vd_asfloat(r1.w)));
      r1.w = vd_asuint(((-(vd_asfloat(r2.w))) * vd_asfloat(r1.w) + 1.0));
      r0.y = vd_asuint(max(vd_asfloat(r0.y), vd_asfloat(r1.w)));
      r0.y = vd_asuint(max(vd_asfloat(r0.y), 0.00100000005));
    } else {
      r0.y = vd_asuint(v0.z);
    }
    if (r4.x != 0u) {
      r1.w = vd_asuint((-(int)(0.125 < vd_asfloat(v2.y))));
      if (r1.w != 0u) {
        r5.xyzw = vd_asuint(_CharacterAreaMaskTexture.SampleLevel(sampler_CharacterAreaMaskTexture, vd_asfloat(r3.xz), 0.0).xyzw);
        r3.yw = vd_asuint(round(vd_asfloat(_WorldSpaceCameraPos.xz)));
        r3.yw = vd_asuint(((-(vd_asfloat(r3.yw))) + vd_asfloat(v2.xz)));
        r6.x = vd_asuint((-(vd_asfloat((VD_COL(unity_MatrixV, 0).z)))));
        r6.y = vd_asuint((-(vd_asfloat((VD_COL(unity_MatrixV, 2).z)))));
        r1.w = vd_asuint(dot(vd_asfloat(r6.xy), vd_asfloat(r6.xy)));
        r1.w = vd_asuint(rsqrt(vd_asfloat(r1.w)));
        r5.zw = vd_asuint((vd_asfloat(r1.ww) * vd_asfloat(r6.xy)));
        r1.w = vd_asuint(dot(vd_asfloat(r3.yw), vd_asfloat(r5.zw)));
        r1.w = vd_asuint(saturate((vd_asfloat(r1.w) * 0.00999999978)));
        r1.w = vd_asuint(((-(vd_asfloat(r1.w))) + vd_asfloat(r5.y)));
        r1.w = vd_asuint(saturate((vd_asfloat(r1.w) * 40.0)));
        r2.w = vd_asuint((vd_asfloat(r1.w) * -2.0 + 3.0));
        r1.w = vd_asuint((vd_asfloat(r1.w) * vd_asfloat(r1.w)));
        r1.w = vd_asuint((vd_asfloat(r1.w) * vd_asfloat(r2.w)));
        r2.w = vd_asuint((vd_asfloat(r5.x) + -1.0));
        r1.w = vd_asuint((vd_asfloat(r1.w) * vd_asfloat(r2.w)));
        r1.w = vd_asuint((vd_asfloat(_GlobalBackgroundSeeThroughEnable) * vd_asfloat(r1.w) + 1.0));
        r5.xyz = vd_asuint((vd_asfloat(v2.xyz) + (-(vd_asfloat(_WorldSpaceCameraPos.xyz)))));
        r2.w = vd_asuint(((-(vd_asfloat(_WorldSpaceCameraPos.y))) + -0.150000006));
        r2.w = vd_asuint((vd_asfloat(r2.w) / vd_asfloat(r5.y)));
        r3.yw = vd_asuint((vd_asfloat(r5.xz) * vd_asfloat(r2.ww) + vd_asfloat(_WorldSpaceCameraPos.xz)));
        r3.yw = vd_asuint((vd_asfloat(r3.yw) + (-(vd_asfloat(_PlaneBoundMaxPos.xz)))));
        r5.xy = vd_asuint((vd_asfloat(_PlaneBoundMinPos.xz) + (-(vd_asfloat(_PlaneBoundMaxPos.xz)))));
        r3.yw = vd_asuint(saturate((vd_asfloat(r3.yw) / vd_asfloat(r5.xy))));
        r5.xyzw = vd_asuint(_PlaneSightRenderTarget.SampleLevel(sampler_PlaneSightRenderTarget, vd_asfloat(r3.yw), 0.0).xyzw);
        r2.w = vd_asuint((vd_asfloat(r5.w) + -1.0));
        r2.w = vd_asuint((vd_asfloat(_GlobalPlaneSystemEnable) * vd_asfloat(r2.w) + 1.0));
        r3.y = vd_asuint(((-(vd_asfloat(_SeeThroughDistanceMin))) + vd_asfloat(_SeeThroughDistanceMax)));
        r3.y = vd_asuint((1.0 / vd_asfloat(r3.y)));
        r3.w = 0x3f800000u;
        r4.x = 0x00000000u;
        while (true) {
          r5.x = vd_asuint((-(int)(vd_asint(r4.x) >= 3)));
          if (r5.x != 0u) break;
          r5.xyz = vd_asuint((vd_asfloat(v2.xyz) + (-(vd_asfloat(_SightPositions[(int)(r4.x)].xyz)))));
          r5.x = vd_asuint(dot(vd_asfloat(r5.xyz), vd_asfloat(r5.xyz)));
          r5.x = vd_asuint(sqrt(vd_asfloat(r5.x)));
          r5.x = vd_asuint((vd_asfloat(r5.x) + (-(vd_asfloat(_SeeThroughDistanceMin)))));
          r5.x = vd_asuint(saturate((vd_asfloat(r3.y) * vd_asfloat(r5.x))));
          r5.y = vd_asuint((vd_asfloat(r5.x) * -2.0 + 3.0));
          r5.x = vd_asuint((vd_asfloat(r5.x) * vd_asfloat(r5.x)));
          r5.x = vd_asuint((vd_asfloat(r5.x) * vd_asfloat(r5.y)));
          r3.w = vd_asuint(min(vd_asfloat(r3.w), vd_asfloat(r5.x)));
          r4.x = vd_asuint((vd_asint(r4.x) + 1));
        }
        r3.y = vd_asuint((vd_asfloat(_GlobalBackgroundSeeThroughEnable) * vd_asfloat(_GlobalPlaneSystemEnable)));
        r2.w = vd_asuint((vd_asfloat(r2.w) * vd_asfloat(r2.w)));
        r3.w = vd_asuint(((-(vd_asfloat(r3.w))) + 1.0));
        r2.w = vd_asuint(((-(vd_asfloat(r2.w))) * vd_asfloat(r3.w) + 1.0));
        r2.w = vd_asuint(max(vd_asfloat(r2.w), 0.0));
        r2.w = vd_asuint((vd_asfloat(r2.w) + -1.0));
        r2.w = vd_asuint((vd_asfloat(r3.y) * vd_asfloat(r2.w) + 1.0));
        r0.y = vd_asuint(min(vd_asfloat(r1.w), vd_asfloat(r2.w)));
      }
      r1.w = vd_asuint(((-(vd_asfloat(_GlobalSeeThroughHeightMin))) + vd_asfloat(_GlobalSeeThroughHeightMax)));
      r2.w = vd_asuint((vd_asfloat(v2.y) + (-(vd_asfloat(_GlobalSeeThroughHeightMin)))));
      r1.w = vd_asuint((1.0 / vd_asfloat(r1.w)));
      r1.w = vd_asuint(saturate((vd_asfloat(r1.w) * vd_asfloat(r2.w))));
      r2.w = vd_asuint((vd_asfloat(r1.w) * -2.0 + 3.0));
      r1.w = vd_asuint((vd_asfloat(r1.w) * vd_asfloat(r1.w)));
      r1.w = vd_asuint(((-(vd_asfloat(r2.w))) * vd_asfloat(r1.w) + 1.0));
      r1.w = vd_asuint(max(vd_asfloat(r0.y), vd_asfloat(r1.w)));
      r0.y = vd_asuint(max(vd_asfloat(r1.w), 0.00100000005));
    } else {
      r1.w = vd_asuint((-(int)(vd_asint(_BackgroundType) == 7)));
      r1.w = (r1.w | r4.z);
      if (r1.w != 0u) {
        r1.w = vd_asuint((vd_asfloat(_GlobalSeeThroughBuildingHeightMax) + (-(vd_asfloat(_GlobalSeeThroughBuildingHeightMin)))));
        r2.w = vd_asuint((vd_asfloat(v2.y) + (-(vd_asfloat(_GlobalSeeThroughBuildingHeightMin)))));
        r1.w = vd_asuint((1.0 / vd_asfloat(r1.w)));
        r1.w = vd_asuint(saturate((vd_asfloat(r1.w) * vd_asfloat(r2.w))));
        r2.w = vd_asuint((vd_asfloat(r1.w) * -2.0 + 3.0));
        r1.w = vd_asuint((vd_asfloat(r1.w) * vd_asfloat(r1.w)));
        r1.w = vd_asuint(((-(vd_asfloat(r2.w))) * vd_asfloat(r1.w) + 1.0));
        r2.w = vd_asuint(((-(vd_asfloat(_GlobalSeeThroughBuildingAlpha))) + 1.0));
        r1.w = vd_asuint((vd_asfloat(r1.w) * vd_asfloat(r2.w) + vd_asfloat(_GlobalSeeThroughBuildingAlpha)));
        r2.w = vd_asuint(((-(vd_asfloat(r1.w))) + 1.0));
        r1.w = vd_asuint((vd_asfloat(r0.y) * vd_asfloat(r2.w) + vd_asfloat(r1.w)));
        r2.w = vd_asuint((-(int)(0.125 < vd_asfloat(v2.y))));
        if (r2.w != 0u) {
          r5.xyzw = vd_asuint(_CharacterAreaMaskTexture.SampleLevel(sampler_CharacterAreaMaskTexture, vd_asfloat(r3.xz), 0.0).xyzw);
          r3.yw = vd_asuint(round(vd_asfloat(_WorldSpaceCameraPos.xz)));
          r3.yw = vd_asuint(((-(vd_asfloat(r3.yw))) + vd_asfloat(v2.xz)));
          r6.x = vd_asuint((-(vd_asfloat((VD_COL(unity_MatrixV, 0).z)))));
          r6.y = vd_asuint((-(vd_asfloat((VD_COL(unity_MatrixV, 2).z)))));
          r2.w = vd_asuint(dot(vd_asfloat(r6.xy), vd_asfloat(r6.xy)));
          r2.w = vd_asuint(rsqrt(vd_asfloat(r2.w)));
          r4.xz = vd_asuint((vd_asfloat(r2.ww) * vd_asfloat(r6.xy)));
          r2.w = vd_asuint(dot(vd_asfloat(r3.yw), vd_asfloat(r4.xz)));
          r2.w = vd_asuint(saturate((vd_asfloat(r2.w) * 0.00999999978)));
          r2.w = vd_asuint(((-(vd_asfloat(r2.w))) + vd_asfloat(r5.y)));
          r2.w = vd_asuint(saturate((vd_asfloat(r2.w) * 40.0)));
          r3.y = vd_asuint((vd_asfloat(r2.w) * -2.0 + 3.0));
          r2.w = vd_asuint((vd_asfloat(r2.w) * vd_asfloat(r2.w)));
          r2.w = vd_asuint((vd_asfloat(r2.w) * vd_asfloat(r3.y)));
          r3.y = vd_asuint((vd_asfloat(r5.x) + -1.0));
          r2.w = vd_asuint((vd_asfloat(r2.w) * vd_asfloat(r3.y)));
          r2.w = vd_asuint((vd_asfloat(_GlobalBackgroundSeeThroughEnable) * vd_asfloat(r2.w) + 1.0));
          r2.w = vd_asuint(min(vd_asfloat(r1.w), vd_asfloat(r2.w)));
          r5.xyz = vd_asuint((vd_asfloat(v2.xyz) + (-(vd_asfloat(_WorldSpaceCameraPos.xyz)))));
          r3.y = vd_asuint(((-(vd_asfloat(_WorldSpaceCameraPos.y))) + -0.150000006));
          r3.y = vd_asuint((vd_asfloat(r3.y) / vd_asfloat(r5.y)));
          r3.yw = vd_asuint((vd_asfloat(r5.xz) * vd_asfloat(r3.yy) + vd_asfloat(_WorldSpaceCameraPos.xz)));
          r3.yw = vd_asuint((vd_asfloat(r3.yw) + (-(vd_asfloat(_PlaneBoundMaxPos.xz)))));
          r4.xz = vd_asuint((vd_asfloat(_PlaneBoundMinPos.xz) + (-(vd_asfloat(_PlaneBoundMaxPos.xz)))));
          r3.yw = vd_asuint(saturate((vd_asfloat(r3.yw) / vd_asfloat(r4.xz))));
          r5.xyzw = vd_asuint(_PlaneSightRenderTarget.SampleLevel(sampler_PlaneSightRenderTarget, vd_asfloat(r3.yw), 0.0).xyzw);
          r3.y = vd_asuint((vd_asfloat(r5.w) + -1.0));
          r3.y = vd_asuint((vd_asfloat(_GlobalPlaneSystemEnable) * vd_asfloat(r3.y) + 1.0));
          r3.w = vd_asuint(((-(vd_asfloat(_SeeThroughDistanceMin))) + vd_asfloat(_SeeThroughDistanceMax)));
          r3.w = vd_asuint((1.0 / vd_asfloat(r3.w)));
          r4.xz = uint2(0x3f800000u, 0x00000000u);
          while (true) {
            r5.x = vd_asuint((-(int)(vd_asint(r4.z) >= 3)));
            if (r5.x != 0u) break;
            r5.xyz = vd_asuint((vd_asfloat(v2.xyz) + (-(vd_asfloat(_SightPositions[(int)(r4.z)].xyz)))));
            r5.x = vd_asuint(dot(vd_asfloat(r5.xyz), vd_asfloat(r5.xyz)));
            r5.x = vd_asuint(sqrt(vd_asfloat(r5.x)));
            r5.x = vd_asuint((vd_asfloat(r5.x) + (-(vd_asfloat(_SeeThroughDistanceMin)))));
            r5.x = vd_asuint(saturate((vd_asfloat(r3.w) * vd_asfloat(r5.x))));
            r5.y = vd_asuint((vd_asfloat(r5.x) * -2.0 + 3.0));
            r5.x = vd_asuint((vd_asfloat(r5.x) * vd_asfloat(r5.x)));
            r5.x = vd_asuint((vd_asfloat(r5.x) * vd_asfloat(r5.y)));
            r4.x = vd_asuint(min(vd_asfloat(r4.x), vd_asfloat(r5.x)));
            r4.z = vd_asuint((vd_asint(r4.z) + 1));
          }
          r3.w = vd_asuint((vd_asfloat(_GlobalBackgroundSeeThroughEnable) * vd_asfloat(_GlobalPlaneSystemEnable)));
          r3.y = vd_asuint((vd_asfloat(r3.y) * vd_asfloat(r3.y)));
          r4.x = vd_asuint(((-(vd_asfloat(r4.x))) + 1.0));
          r3.y = vd_asuint(((-(vd_asfloat(r3.y))) * vd_asfloat(r4.x) + 1.0));
          r3.y = vd_asuint(max(vd_asfloat(r3.y), 0.0));
          r3.y = vd_asuint((vd_asfloat(r3.y) + -1.0));
          r3.y = vd_asuint((vd_asfloat(r3.w) * vd_asfloat(r3.y) + 1.0));
          r1.w = vd_asuint(min(vd_asfloat(r2.w), vd_asfloat(r3.y)));
        }
        r1.w = vd_asuint((vd_asfloat(r1.w) + -1.0));
        r0.y = vd_asuint((vd_asfloat(_GlobalIsRuntime) * vd_asfloat(r1.w) + 1.0));
        r3.yw = vd_asuint((vd_asfloat(_ScreenSize.xy) / vd_asfloat(_ScreenParams.xy)));
        r4.xz = vd_asuint((vd_asfloat(r3.xz) * vd_asfloat(_ScreenParams.xy)));
        r3.yw = vd_asuint((vd_asfloat(r3.yw) * vd_asfloat(r4.xz)));
        r3.yw = vd_asuint(floor(vd_asfloat(r3.yw)));
        r3.yw = ((uint2)vd_asfloat(r3.yw));
        r3.yw = (r3.yw & uint2(0x00000003u, 0x00000003u));
        r1.w = (r3.w << (uint)2);
        r1.w = vd_asuint((vd_asint(r1.w) + vd_asint(r3.y)));
        r1.w = x0[r1.w + 0].x;
        r1.w = vd_asuint((vd_asfloat(r0.y) + (-(vd_asfloat(r1.w)))));
        r1.w = vd_asuint((-(int)(vd_asfloat(r1.w) < 0.0)));
        if (r1.w != 0u) discard;
      } else {
        if (r4.y != 0u) {
          r5.xyzw = vd_asuint(_CharacterAreaMaskTexture.SampleLevel(sampler_CharacterAreaMaskTexture, vd_asfloat(r3.xz), 0.0).xyzw);
          r3.yw = vd_asuint(round(vd_asfloat(_WorldSpaceCameraPos.xz)));
          r3.yw = vd_asuint(((-(vd_asfloat(r3.yw))) + vd_asfloat(v2.xz)));
          r4.x = vd_asuint((-(vd_asfloat((VD_COL(unity_MatrixV, 0).z)))));
          r4.y = vd_asuint((-(vd_asfloat((VD_COL(unity_MatrixV, 2).z)))));
          r1.w = vd_asuint(dot(vd_asfloat(r4.xy), vd_asfloat(r4.xy)));
          r1.w = vd_asuint(rsqrt(vd_asfloat(r1.w)));
          r4.xy = vd_asuint((vd_asfloat(r1.ww) * vd_asfloat(r4.xy)));
          r1.w = vd_asuint(dot(vd_asfloat(r3.yw), vd_asfloat(r4.xy)));
          r1.w = vd_asuint(saturate((vd_asfloat(r1.w) * 0.00999999978)));
          r1.w = vd_asuint(((-(vd_asfloat(r1.w))) + vd_asfloat(r5.y)));
          r1.w = vd_asuint(saturate((vd_asfloat(r1.w) * 40.0)));
          r2.w = vd_asuint((vd_asfloat(r1.w) * -2.0 + 3.0));
          r1.w = vd_asuint((vd_asfloat(r1.w) * vd_asfloat(r1.w)));
          r1.w = vd_asuint((vd_asfloat(r1.w) * vd_asfloat(r2.w)));
          r2.w = vd_asuint((vd_asfloat(r5.x) + -1.0));
          r1.w = vd_asuint((vd_asfloat(r1.w) * vd_asfloat(r2.w)));
          r1.w = vd_asuint((vd_asfloat(_GlobalBackgroundSeeThroughEnable) * vd_asfloat(r1.w) + 1.0));
          r4.xyz = vd_asuint((vd_asfloat(v2.xyz) + (-(vd_asfloat(_WorldSpaceCameraPos.xyz)))));
          r5.xyzw = vd_asuint(((-(vd_asfloat(_WorldSpaceCameraPos.yyyy))) + float4(2.0, 2.0, 1.0, 1.0)));
          r5.xyzw = vd_asuint((vd_asfloat(r5.xyzw) / vd_asfloat(r4.yyyy)));
          r5.xyzw = vd_asuint((vd_asfloat(r4.xzxz) * vd_asfloat(r5.xyzw) + vd_asfloat(_WorldSpaceCameraPos.xzxz)));
          r5.xyzw = vd_asuint((vd_asfloat(r5.xyzw) + (-(vd_asfloat(_PlaneBoundMaxPos.xzxz)))));
          r3.yw = vd_asuint((vd_asfloat(_PlaneBoundMinPos.xz) + (-(vd_asfloat(_PlaneBoundMaxPos.xz)))));
          r5.xyzw = vd_asuint(saturate((vd_asfloat(r5.xyzw) / vd_asfloat(r3.ywyw))));
          r6.xyzw = vd_asuint(_PlaneSightRenderTarget.SampleLevel(sampler_PlaneSightRenderTarget, vd_asfloat(r5.xy), 0.0).xyzw);
          r2.w = vd_asuint((vd_asfloat(r6.w) + -1.0));
          r2.w = vd_asuint((vd_asfloat(_GlobalPlaneSystemEnable) * vd_asfloat(r2.w) + 1.0));
          r5.x = vd_asuint(((-(vd_asfloat(_SeeThroughDistanceMin))) + vd_asfloat(_SeeThroughDistanceMax)));
          r5.x = vd_asuint((1.0 / vd_asfloat(r5.x)));
          r5.y = 0x3f800000u;
          r6.x = 0x00000000u;
          while (true) {
            r6.y = vd_asuint((-(int)(vd_asint(r6.x) >= 3)));
            if (r6.y != 0u) break;
            r6.yzw = vd_asuint((vd_asfloat(v2.xyz) + (-(vd_asfloat(_SightPositions[(int)(r6.x)].xyz)))));
            r6.y = vd_asuint(dot(vd_asfloat(r6.yzw), vd_asfloat(r6.yzw)));
            r6.y = vd_asuint(sqrt(vd_asfloat(r6.y)));
            r6.y = vd_asuint((vd_asfloat(r6.y) + (-(vd_asfloat(_SeeThroughDistanceMin)))));
            r6.y = vd_asuint(saturate((vd_asfloat(r5.x) * vd_asfloat(r6.y))));
            r6.z = vd_asuint((vd_asfloat(r6.y) * -2.0 + 3.0));
            r6.y = vd_asuint((vd_asfloat(r6.y) * vd_asfloat(r6.y)));
            r6.y = vd_asuint((vd_asfloat(r6.y) * vd_asfloat(r6.z)));
            r5.y = vd_asuint(min(vd_asfloat(r5.y), vd_asfloat(r6.y)));
            r6.x = vd_asuint((vd_asint(r6.x) + 1));
          }
          r6.x = vd_asuint((vd_asfloat(_GlobalBackgroundSeeThroughEnable) * vd_asfloat(_GlobalPlaneSystemEnable)));
          r2.w = vd_asuint((vd_asfloat(r2.w) * vd_asfloat(r2.w)));
          r5.y = vd_asuint(((-(vd_asfloat(r5.y))) + 1.0));
          r2.w = vd_asuint(((-(vd_asfloat(r2.w))) * vd_asfloat(r5.y) + 1.0));
          r2.w = vd_asuint(max(vd_asfloat(r2.w), 0.0));
          r2.w = vd_asuint((vd_asfloat(r2.w) + -1.0));
          r2.w = vd_asuint((vd_asfloat(r6.x) * vd_asfloat(r2.w) + 1.0));
          r7.xyzw = vd_asuint(_PlaneSightRenderTarget.SampleLevel(sampler_PlaneSightRenderTarget, vd_asfloat(r5.zw), 0.0).xyzw);
          r5.y = vd_asuint((vd_asfloat(r7.w) + -1.0));
          r5.y = vd_asuint((vd_asfloat(_GlobalPlaneSystemEnable) * vd_asfloat(r5.y) + 1.0));
          r5.zw = uint2(0x3f800000u, 0x00000000u);
          while (true) {
            r6.y = vd_asuint((-(int)(vd_asint(r5.w) >= 3)));
            if (r6.y != 0u) break;
            r6.yzw = vd_asuint((vd_asfloat(v2.xyz) + (-(vd_asfloat(_SightPositions[(int)(r5.w)].xyz)))));
            r6.y = vd_asuint(dot(vd_asfloat(r6.yzw), vd_asfloat(r6.yzw)));
            r6.y = vd_asuint(sqrt(vd_asfloat(r6.y)));
            r6.y = vd_asuint((vd_asfloat(r6.y) + (-(vd_asfloat(_SeeThroughDistanceMin)))));
            r6.y = vd_asuint(saturate((vd_asfloat(r5.x) * vd_asfloat(r6.y))));
            r6.z = vd_asuint((vd_asfloat(r6.y) * -2.0 + 3.0));
            r6.y = vd_asuint((vd_asfloat(r6.y) * vd_asfloat(r6.y)));
            r6.y = vd_asuint((vd_asfloat(r6.y) * vd_asfloat(r6.z)));
            r5.z = vd_asuint(min(vd_asfloat(r5.z), vd_asfloat(r6.y)));
            r5.w = vd_asuint((vd_asint(r5.w) + 1));
          }
          r5.y = vd_asuint((vd_asfloat(r5.y) * vd_asfloat(r5.y)));
          r5.z = vd_asuint(((-(vd_asfloat(r5.z))) + 1.0));
          r5.y = vd_asuint(((-(vd_asfloat(r5.y))) * vd_asfloat(r5.z) + 1.0));
          r5.y = vd_asuint(max(vd_asfloat(r5.y), 0.0));
          r5.y = vd_asuint((vd_asfloat(r5.y) + -1.0));
          r5.y = vd_asuint((vd_asfloat(r6.x) * vd_asfloat(r5.y) + 1.0));
          r4.y = vd_asuint(((-(vd_asfloat(_WorldSpaceCameraPos.y))) / vd_asfloat(r4.y)));
          r4.xy = vd_asuint((vd_asfloat(r4.xz) * vd_asfloat(r4.yy) + vd_asfloat(_WorldSpaceCameraPos.xz)));
          r4.xy = vd_asuint((vd_asfloat(r4.xy) + (-(vd_asfloat(_PlaneBoundMaxPos.xz)))));
          r3.yw = vd_asuint(saturate((vd_asfloat(r4.xy) / vd_asfloat(r3.yw))));
          r7.xyzw = vd_asuint(_PlaneSightRenderTarget.SampleLevel(sampler_PlaneSightRenderTarget, vd_asfloat(r3.yw), 0.0).xyzw);
          r3.y = vd_asuint((vd_asfloat(r7.w) + -1.0));
          r3.y = vd_asuint((vd_asfloat(_GlobalPlaneSystemEnable) * vd_asfloat(r3.y) + 1.0));
          r3.w = 0x3f800000u;
          r4.x = 0x00000000u;
          while (true) {
            r4.y = vd_asuint((-(int)(vd_asint(r4.x) >= 3)));
            if (r4.y != 0u) break;
            r6.yzw = vd_asuint((vd_asfloat(v2.xyz) + (-(vd_asfloat(_SightPositions[(int)(r4.x)].xyz)))));
            r4.y = vd_asuint(dot(vd_asfloat(r6.yzw), vd_asfloat(r6.yzw)));
            r4.y = vd_asuint(sqrt(vd_asfloat(r4.y)));
            r4.y = vd_asuint((vd_asfloat(r4.y) + (-(vd_asfloat(_SeeThroughDistanceMin)))));
            r4.y = vd_asuint(saturate((vd_asfloat(r5.x) * vd_asfloat(r4.y))));
            r4.z = vd_asuint((vd_asfloat(r4.y) * -2.0 + 3.0));
            r4.y = vd_asuint((vd_asfloat(r4.y) * vd_asfloat(r4.y)));
            r4.y = vd_asuint((vd_asfloat(r4.y) * vd_asfloat(r4.z)));
            r3.w = vd_asuint(min(vd_asfloat(r3.w), vd_asfloat(r4.y)));
            r4.x = vd_asuint((vd_asint(r4.x) + 1));
          }
          r3.y = vd_asuint((vd_asfloat(r3.y) * vd_asfloat(r3.y)));
          r3.w = vd_asuint(((-(vd_asfloat(r3.w))) + 1.0));
          r3.y = vd_asuint(((-(vd_asfloat(r3.y))) * vd_asfloat(r3.w) + 1.0));
          r3.y = vd_asuint(max(vd_asfloat(r3.y), 0.0));
          r3.y = vd_asuint((vd_asfloat(r3.y) + -1.0));
          r3.y = vd_asuint((vd_asfloat(r6.x) * vd_asfloat(r3.y) + 1.0));
          r3.y = vd_asuint(min(vd_asfloat(r3.y), vd_asfloat(r5.y)));
          r2.w = vd_asuint(min(vd_asfloat(r2.w), vd_asfloat(r3.y)));
          r1.w = vd_asuint(min(vd_asfloat(r1.w), vd_asfloat(r2.w)));
          r1.w = vd_asuint(min(vd_asfloat(r1.w), vd_asfloat(_GlobalCoverSeeThroughFactor)));
          r1.w = vd_asuint((vd_asfloat(r1.w) + -1.0));
          r0.y = vd_asuint((vd_asfloat(_GlobalIsRuntime) * vd_asfloat(r1.w) + 1.0));
          r3.yw = vd_asuint((vd_asfloat(_ScreenSize.xy) / vd_asfloat(_ScreenParams.xy)));
          r4.xy = vd_asuint((vd_asfloat(r3.xz) * vd_asfloat(_ScreenParams.xy)));
          r3.yw = vd_asuint((vd_asfloat(r3.yw) * vd_asfloat(r4.xy)));
          r3.yw = vd_asuint(floor(vd_asfloat(r3.yw)));
          r3.yw = ((uint2)vd_asfloat(r3.yw));
          r3.yw = (r3.yw & uint2(0x00000003u, 0x00000003u));
          r1.w = (r3.w << (uint)2);
          r1.w = vd_asuint((vd_asint(r1.w) + vd_asint(r3.y)));
          r1.w = x0[r1.w + 0].x;
          r1.w = vd_asuint((vd_asfloat(r0.y) + (-(vd_asfloat(r1.w)))));
          r1.w = vd_asuint((-(int)(vd_asfloat(r1.w) < 0.0)));
          if (r1.w != 0u) discard;
        }
      }
    }
    r1.w = vd_asuint(((-(vd_asfloat(_GlobalSeeThroughAlpha))) + 1.0));
    r0.y = vd_asuint((vd_asfloat(r0.y) * vd_asfloat(r1.w) + vd_asfloat(_GlobalSeeThroughAlpha)));
    r1.w = vd_asuint((vd_asfloat(r0.y) * vd_asfloat(v8.w)));
    r0.y = vd_asuint((vd_asfloat(r0.y) + -1.0));
    r0.y = vd_asuint((vd_asfloat(_GlobalIsRuntime) * vd_asfloat(r0.y) + 1.0));
    r0.y = ((r4.w != 0u) ? r1.w : r0.y);
    r3.yw = vd_asuint((vd_asfloat(_ScreenSize.xy) / vd_asfloat(_ScreenParams.xy)));
    r3.xz = vd_asuint((vd_asfloat(r3.xz) * vd_asfloat(_ScreenParams.xy)));
    r3.xy = vd_asuint((vd_asfloat(r3.yw) * vd_asfloat(r3.xz)));
    r3.xy = vd_asuint(floor(vd_asfloat(r3.xy)));
    r3.xy = ((uint2)vd_asfloat(r3.xy));
    r3.xy = (r3.xy & uint2(0x00000003u, 0x00000003u));
    r1.w = (r3.y << (uint)2);
    r1.w = vd_asuint((vd_asint(r1.w) + vd_asint(r3.x)));
    r1.w = x0[r1.w + 0].x;
    r0.y = vd_asuint((vd_asfloat(r0.y) * vd_asfloat(_BaseColor.w) + (-(vd_asfloat(r1.w)))));
    r0.y = vd_asuint((-(int)(vd_asfloat(r0.y) < 0.0)));
    if (r0.y != 0u) discard;
  }
  r0.y = vd_asuint((-(int)(0.5 < vd_asfloat(_UseWorldNormalMap))));
  if (r0.y != 0u) {
    r3.xyzw = vd_asuint(_WorldNormalMap.SampleBias(sampler_vd_point_repeat, vd_asfloat(v0.xy), vd_asfloat(_GlobalMipBias.x)).xyzw);
    r3.xyz = vd_asuint((vd_asfloat(r3.xyz) * float3(2.0, 2.0, 2.0) + float3(-1.0, -1.0, -1.0)));
    r0.y = vd_asuint(dot(vd_asfloat(r3.xyz), vd_asfloat(r3.xyz)));
    r0.y = vd_asuint(rsqrt(vd_asfloat(r0.y)));
    r3.xyz = vd_asuint((vd_asfloat(r0.yyy) * vd_asfloat(r3.xyz)));
    r0.y = vd_asuint(dot(vd_asfloat(v4.xyz), vd_asfloat(r3.xyz)));
    r4.xyz = vd_asuint(((-(vd_asfloat(r3.xyz))) * vd_asfloat(r0.yyy) + vd_asfloat(v4.xyz)));
    r0.y = vd_asuint(dot(vd_asfloat(r4.xyz), vd_asfloat(r4.xyz)));
    r0.y = vd_asuint(rsqrt(vd_asfloat(r0.y)));
    r4.xyz = vd_asuint((vd_asfloat(r0.yyy) * vd_asfloat(r4.xyz)));
    r0.y = vd_asuint(dot(vd_asfloat(v5.xyz), vd_asfloat(r3.xyz)));
    r5.xyz = vd_asuint(((-(vd_asfloat(r3.xyz))) * vd_asfloat(r0.yyy) + vd_asfloat(v5.xyz)));
    r0.y = vd_asuint(dot(vd_asfloat(r5.xyz), vd_asfloat(r5.xyz)));
    r0.y = vd_asuint(rsqrt(vd_asfloat(r0.y)));
    r5.xyz = vd_asuint((vd_asfloat(r0.yyy) * vd_asfloat(r5.xyz)));
    r5.xyz = vd_asuint((vd_asfloat(r2.yyy) * vd_asfloat(r5.xyz)));
    r4.xyz = vd_asuint((vd_asfloat(r2.xxx) * vd_asfloat(r4.xyz) + vd_asfloat(r5.xyz)));
    r3.xyz = vd_asuint((vd_asfloat(r2.zzz) * vd_asfloat(r3.xyz) + vd_asfloat(r4.xyz)));
  } else {
    r4.xyz = vd_asuint((vd_asfloat(r2.yyy) * vd_asfloat(v5.xyz)));
    r2.xyw = vd_asuint((vd_asfloat(r2.xxx) * vd_asfloat(v4.xyz) + vd_asfloat(r4.xyz)));
    r3.xyz = vd_asuint((vd_asfloat(r2.zzz) * vd_asfloat(v3.xyz) + vd_asfloat(r2.xyw)));
  }
  r0.y = vd_asuint(dot(vd_asfloat(r3.xyz), vd_asfloat(r3.xyz)));
  r0.y = vd_asuint(rsqrt(vd_asfloat(r0.y)));
  r2.xyz = vd_asuint((vd_asfloat(r0.yyy) * vd_asfloat(r3.xyz)));
  r0.y = vd_asuint((-(int)(vd_asfloat(v1.z) < 1.5)));
  r0.y = (r0.y & r0.w);
  r0.w = vd_asuint((-(int)(0.125 >= vd_asfloat(v11.y))));
  r0.y = (r0.w & r0.y);
  r3.xyz = ((r0.yyy != 0u) ? vd_asuint(v11.xyz) : vd_asuint(v2.xyz));
  r4.xyz = ((r0.yyy != 0u) ? uint3(0x00000000u, 0x3f800000u, 0x00000000u) : r2.xyz);
  r5.xyz = vd_asuint((-(int3)(vd_asint((_BackgroundType).xxx) == int3(0, 1, 11))));
  r0.w = (r5.y | r5.x);
  r0.w = (r5.z | r0.w);
  r1.w = vd_asuint((-(int)(0.0 >= vd_asfloat(r3.y))));
  r2.w = vd_asuint((-(int)(vd_asfloat(r4.y) >= 0.850000024)));
  r0.y = (r0.y | r2.w);
  r2.xyz = ((r0.yyy != 0u) ? uint3(0x00000000u, 0x3f800000u, 0x00000000u) : r2.xyz);
  r2.xyz = ((r1.www != 0u) ? r2.xyz : r4.xyz);
  r2.xyz = ((r0.www != 0u) ? r2.xyz : r4.xyz);
  r0.y = vd_asuint((-(int)(vd_asfloat(unity_OrthoParams.w) == 0.0)));
  r4.xyz = vd_asuint(((-(vd_asfloat(r3.xyz))) + vd_asfloat(_WorldSpaceCameraPos.xyz)));
  r0.w = vd_asuint(dot(vd_asfloat(r4.xyz), vd_asfloat(r4.xyz)));
  r0.w = vd_asuint(rsqrt(vd_asfloat(r0.w)));
  r4.xyz = vd_asuint((vd_asfloat(r0.www) * vd_asfloat(r4.xyz)));
  r5.x = ((r0.y != 0u) ? r4.x : vd_asuint((VD_COL(unity_MatrixV, 0).z)));
  r5.y = ((r0.y != 0u) ? r4.y : vd_asuint((VD_COL(unity_MatrixV, 1).z)));
  r5.z = ((r0.y != 0u) ? r4.z : vd_asuint((VD_COL(unity_MatrixV, 2).z)));
  r4.xyz = vd_asuint((vd_asfloat(r3.yyy) * vd_asfloat((VD_COL(unity_MatrixVP, 1).xyw))));
  r4.xyz = vd_asuint((vd_asfloat((VD_COL(unity_MatrixVP, 0).xyw)) * vd_asfloat(r3.xxx) + vd_asfloat(r4.xyz)));
  r4.xyz = vd_asuint((vd_asfloat((VD_COL(unity_MatrixVP, 2).xyw)) * vd_asfloat(r3.zzz) + vd_asfloat(r4.xyz)));
  r4.xyz = vd_asuint((vd_asfloat(r4.xyz) + vd_asfloat((VD_COL(unity_MatrixVP, 3).xyw))));
  r0.yw = vd_asuint((vd_asfloat(r4.xy) / (-(vd_asfloat(r4.zz)))));
  r4.xy = vd_asuint((vd_asfloat(r0.yw) * float2(0.5, 0.5) + float2(0.5, 0.5)));
  r0.x = vd_asuint(saturate(dot(vd_asfloat(r0.xx), vd_asfloat(r0.zz))));
  r0.y = (vd_asuint(_MainLightLayerMask) & vd_asuint(unity_RenderingLayer.x));
  r6.xyz = vd_asuint((vd_asfloat(r0.xxx) * vd_asfloat(r1.xyz)));
  r6.xyz = vd_asuint((vd_asfloat(r6.xyz) * vd_asfloat(_MainLightColor.xyz)));
  r7.xyz = vd_asuint((vd_asfloat(r5.xyz) + vd_asfloat(_MainLightPosition.xyz)));
  r0.z = vd_asuint(dot(vd_asfloat(r7.xyz), vd_asfloat(r7.xyz)));
  r0.z = vd_asuint(max(vd_asfloat(r0.z), 6.10351562e-05));
  r0.z = vd_asuint(rsqrt(vd_asfloat(r0.z)));
  r7.xyz = vd_asuint((vd_asfloat(r0.zzz) * vd_asfloat(r7.xyz)));
  r0.z = vd_asuint(saturate(dot(vd_asfloat(r2.xyz), vd_asfloat(r7.xyz))));
  r0.z = vd_asuint(log2(vd_asfloat(r0.z)));
  r0.z = vd_asuint((vd_asfloat(r0.z) * 128.0));
  r0.z = vd_asuint(exp2(vd_asfloat(r0.z)));
  r7.xyz = vd_asuint((vd_asfloat(r0.zzz) * vd_asfloat(_MainLightColor.xyz)));
  r7.xyz = vd_asuint((vd_asfloat(r0.xxx) * vd_asfloat(r7.xyz)));
  r7.xyz = vd_asuint((vd_asfloat(r7.xyz) * vd_asfloat(unity_LightData.zzz)));
  r6.xyz = vd_asuint((vd_asfloat(r6.xyz) * float3(0.600000024, 0.600000024, 0.600000024) + vd_asfloat(r7.xyz)));
  r0.yzw = ((r0.yyy != 0u) ? r6.xyz : uint3(0x00000000u, 0x00000000u, 0x00000000u));
  r1.w = vd_asuint(min(vd_asfloat(_AdditionalLightsCount.x), vd_asfloat(unity_LightData.y)));
  r1.w = vd_asuint(((int)vd_asfloat(r1.w)));
  r6.xy = vd_asuint((vd_asfloat(r3.xz) + (-(vd_asfloat(_PlaneBoundMaxPos.xz)))));
  r6.zw = vd_asuint((vd_asfloat(_PlaneBoundMinPos.xz) + (-(vd_asfloat(_PlaneBoundMaxPos.xz)))));
  r6.xy = vd_asuint(saturate((vd_asfloat(r6.xy) / vd_asfloat(r6.zw))));
  r6.xyzw = vd_asuint(_PlaneSightRenderTarget.SampleLevel(sampler_PlaneSightRenderTarget, vd_asfloat(r6.xy), 0.0).xyzw);
  r2.w = vd_asuint((vd_asfloat(r6.w) + -1.0));
  r2.w = vd_asuint((vd_asfloat(r2.w) * vd_asfloat(_GlobalPlaneSystemEnable)));
  r6.xyz = vd_asuint((vd_asfloat(r3.xyz) * float3(32.0, 32.0, 32.0)));
  r6.xyz = vd_asuint(floor(vd_asfloat(r6.xyz)));
  r3.w = vd_asuint((vd_asfloat(r0.x) * 0.5 + 0.5));
  r7.xyz = uint3(0x00000000u, 0x00000000u, 0x00000000u);
  r4.w = 0x00000000u;
  while (true) {
    r5.w = vd_asuint((-(int)(r4.w >= r1.w)));
    if (r5.w != 0u) break;
    r5.w = (r4.w >> (uint)2);
    r6.w = (r4.w & 0x00000003u);
    r5.w = vd_asuint(dot(vd_asfloat(unity_LightIndices[(int)(r5.w)].xyzw), vd_asfloat(icb[r6.w + 0].xyzw)));
    r5.w = vd_asuint(((int)vd_asfloat(r5.w)));
    r8.xyz = vd_asuint(((-(vd_asfloat(r3.xyz))) * vd_asfloat(_AdditionalLightsPosition[(int)(r5.w)].www) + vd_asfloat(_AdditionalLightsPosition[(int)(r5.w)].xyz)));
    r6.w = vd_asuint(dot(vd_asfloat(r8.xyz), vd_asfloat(r8.xyz)));
    r6.w = vd_asuint(max(vd_asfloat(r6.w), 6.10351562e-05));
    r7.w = vd_asuint(rsqrt(vd_asfloat(r6.w)));
    r8.xyz = vd_asuint((vd_asfloat(r7.www) * vd_asfloat(r8.xyz)));
    r7.w = vd_asuint((1.0 / vd_asfloat(r6.w)));
    r6.w = vd_asuint((vd_asfloat(r6.w) * vd_asfloat(_AdditionalLightsAttenuation[(int)(r5.w)].x)));
    r6.w = vd_asuint(((-(vd_asfloat(r6.w))) * vd_asfloat(r6.w) + 1.0));
    r6.w = vd_asuint(max(vd_asfloat(r6.w), 0.0));
    r6.w = vd_asuint((vd_asfloat(r6.w) * vd_asfloat(r6.w)));
    r6.w = vd_asuint((vd_asfloat(r6.w) * vd_asfloat(r7.w)));
    r7.w = vd_asuint(dot(vd_asfloat(_AdditionalLightsSpotDir[(int)(r5.w)].xyz), vd_asfloat(r8.xyz)));
    r7.w = vd_asuint(saturate((vd_asfloat(r7.w) * vd_asfloat(_AdditionalLightsAttenuation[(int)(r5.w)].z) + vd_asfloat(_AdditionalLightsAttenuation[(int)(r5.w)].w))));
    r7.w = vd_asuint((vd_asfloat(r7.w) * vd_asfloat(r7.w)));
    r6.w = vd_asuint((vd_asfloat(r6.w) * vd_asfloat(r7.w)));
    r7.w = vd_asuint((-(int)(2 == vd_asint(_AdditionalLightsLayerMasks[(int)(r5.w)]))));
    r7.w = (r7.w & 0x3f800000u);
    r7.w = vd_asuint((vd_asfloat(r7.w) * vd_asfloat(r2.w) + 1.0));
    r8.xyz = vd_asuint((vd_asfloat(r6.xyz) * float3(0.03125, 0.03125, 0.03125) + (-(vd_asfloat(_AdditionalLightsPosition[(int)(r4.w)].xyz)))));
    r8.w = vd_asuint(dot(vd_asfloat(r8.xyz), vd_asfloat(r8.xyz)));
    r8.w = vd_asuint(rsqrt(vd_asfloat(r8.w)));
    r9.xyz = vd_asuint((vd_asfloat(r8.www) * vd_asfloat(r8.xyz)));
    r9.w = vd_asuint(dot(vd_asfloat(r9.xyz), vd_asfloat(r2.xyz)));
    r10.xyz = vd_asuint(((-(vd_asfloat(r9.www))) * vd_asfloat(r2.xyz) + vd_asfloat(r9.xyz)));
    r9.w = vd_asuint(dot(vd_asfloat(r10.xyz), vd_asfloat(r10.xyz)));
    r9.w = vd_asuint(rsqrt(vd_asfloat(r9.w)));
    r10.xyz = vd_asuint((vd_asfloat(r9.www) * vd_asfloat(r10.xyz)));
    r9.w = vd_asuint(dot(vd_asfloat(r9.xyz), vd_asfloat(r10.xyz)));
    r9.w = vd_asuint((vd_asfloat(r9.w) + vd_asfloat(r9.w)));
    r9.xyz = vd_asuint((vd_asfloat(r10.xyz) * (-(vd_asfloat(r9.www))) + vd_asfloat(r9.xyz)));
    r8.xyz = vd_asuint((vd_asfloat(r8.xyz) * vd_asfloat(r8.www) + vd_asfloat(r5.xyz)));
    r8.w = vd_asuint(dot(vd_asfloat(r8.xyz), vd_asfloat(r8.xyz)));
    r8.w = vd_asuint(max(vd_asfloat(r8.w), 1.17549435e-38));
    r8.w = vd_asuint(rsqrt(vd_asfloat(r8.w)));
    r8.xyz = vd_asuint((vd_asfloat(r8.www) * vd_asfloat(r8.xyz)));
    r8.x = vd_asuint(saturate(dot(vd_asfloat(r2.xyz), vd_asfloat(r8.xyz))));
    r8.yzw = vd_asuint((vd_asfloat(r5.xyz) + (-(vd_asfloat(r9.xyz)))));
    r9.x = vd_asuint(dot(vd_asfloat(r8.yzw), vd_asfloat(r8.yzw)));
    r9.x = vd_asuint(max(vd_asfloat(r9.x), 1.17549435e-38));
    r9.x = vd_asuint(rsqrt(vd_asfloat(r9.x)));
    r8.yzw = vd_asuint((vd_asfloat(r8.yzw) * vd_asfloat(r9.xxx)));
    r8.y = vd_asuint(saturate(dot(vd_asfloat(r2.xyz), vd_asfloat(r8.yzw))));
    r8.x = vd_asuint(max(vd_asfloat(r8.y), vd_asfloat(r8.x)));
    r8.x = vd_asuint(log2(vd_asfloat(r8.x)));
    r8.x = vd_asuint((vd_asfloat(r8.x) * 256.0));
    r8.x = vd_asuint(exp2(vd_asfloat(r8.x)));
    r8.yzw = vd_asuint((vd_asfloat(_AdditionalLightsColor[(int)(r5.w)].xyz) * vd_asfloat(_AdditionalLightsColor[(int)(r5.w)].xyz)));
    r8.xyz = vd_asuint((vd_asfloat(r8.xxx) * vd_asfloat(r8.yzw)));
    r8.xyz = vd_asuint((vd_asfloat(r6.www) * vd_asfloat(r8.xyz)));
    r8.xyz = vd_asuint((vd_asfloat(r3.www) * vd_asfloat(r8.xyz)));
    r8.xyz = vd_asuint((vd_asfloat(r1.xyz) * vd_asfloat(r8.xyz)));
    r8.xyz = vd_asuint((vd_asfloat(r8.xyz) * float3(0.25, 0.25, 0.25)));
    r7.xyz = vd_asuint((vd_asfloat(r8.xyz) * vd_asfloat(r7.www) + vd_asfloat(r7.xyz)));
    r4.w = vd_asuint((vd_asint(r4.w) + 1));
  }
  r0.yzw = vd_asuint((vd_asfloat(r0.yzw) + vd_asfloat(r7.xyz)));
  r0.x = vd_asuint(((-(vd_asfloat(r0.x))) + 1.0));
  r1.w = vd_asuint(dot(vd_asfloat(r1.xyz), float3(0.298999995, 0.587000012, 0.114)));
  r1.w = vd_asuint((vd_asfloat(r1.w) * vd_asfloat(r1.w)));
  r2.y = vd_asuint((vd_asfloat(r1.w) * vd_asfloat(r1.w)));
  r2.x = 0x3c4ccccdu;
  r2.xy = vd_asuint((vd_asfloat(r2.xy) * float2(0.0, -0.0125000002)));
  r4.z = vd_asuint(((-(vd_asfloat(r4.x))) + 1.0));
  r2.xy = vd_asuint((vd_asfloat(r2.xy) * vd_asfloat(r0.xx) + vd_asfloat(r4.zy)));
  r2.xyzw = vd_asuint(_AfterRenderingGrabpassTexture.SampleBias(sampler_vd_linear_clamp, vd_asfloat(r2.xy), vd_asfloat(_GlobalMipBias.x)).xyzw);
  o0.xyz = (vd_asfloat(r2.xyz) * vd_asfloat(r1.xyz) + vd_asfloat(r0.yzw));
  o0.w = vd_asfloat(0x3f800000u);
  return;
}
