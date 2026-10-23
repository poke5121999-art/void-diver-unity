// LCArt/Character/shader_DE_Character_Normal | pass 0 "ForwardLit" | progFragment | keywords: FOG_LINEAR _ADDITIONAL_LIGHTS _DE_EDITOR_CAMERA | blob 47
void frag(
  float4 v0 : SV_POSITION0,
  float4 v1 : VDREG1,
  float4 v2 : VDREG2,
  float4 v3 : VDREG3,
  float4 v4 : VDREG4,
  float4 v5 : VDREG5,
  float4 v6 : VDREG6,
  float4 v7 : VDREG7,
  float4 v8 : VDREG8,
  float4 v9 : VDREG9,
  out float4 o0 : SV_Target0)
{
  uint4 r0, r1, r2, r3, r4, r5, r6;
  r0 = 0u; r1 = 0u; r2 = 0u; r3 = 0u; r4 = 0u; r5 = 0u; r6 = 0u;
  uint4 x0[16];
  const uint4 icb[4] = { uint4(0x3f800000u, 0x00000000u, 0x00000000u, 0x00000000u), uint4(0x00000000u, 0x3f800000u, 0x00000000u, 0x00000000u), uint4(0x00000000u, 0x00000000u, 0x3f800000u, 0x00000000u), uint4(0x00000000u, 0x00000000u, 0x00000000u, 0x3f800000u) };

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
  r0.xyzw = vd_asuint(_MainTex.SampleBias(sampler_MainTex, vd_asfloat(v1.xy), vd_asfloat(_GlobalMipBias.x)).xyzw);
  r1.x = vd_asuint((vd_asfloat(r0.w) + -0.100000001));
  r1.x = vd_asuint((-(int)(vd_asfloat(r1.x) < 0.0)));
  if (r1.x != 0u) discard;
  r1.xyz = vd_asuint((vd_asfloat(r0.xyz) * vd_asfloat(v8.xyz)));
  r1.w = vd_asuint(saturate(vd_asfloat(_IsVertexColorAlphaUsed)));
  r2.x = vd_asuint((vd_asfloat(v8.w) + -1.0));
  r1.w = vd_asuint((vd_asfloat(r1.w) * vd_asfloat(r2.x) + 1.0));
  o0.w = (vd_asfloat(r0.w) * vd_asfloat(r1.w));
  r1.w = vd_asuint((vd_asfloat(v4.y) * vd_asfloat((VD_COL(unity_MatrixV, 1).z))));
  r1.w = vd_asuint((vd_asfloat((VD_COL(unity_MatrixV, 0).z)) * vd_asfloat(v4.x) + vd_asfloat(r1.w)));
  r1.w = vd_asuint((vd_asfloat((VD_COL(unity_MatrixV, 2).z)) * vd_asfloat(v4.z) + vd_asfloat(r1.w)));
  r1.w = vd_asuint((vd_asfloat(r1.w) + vd_asfloat((VD_COL(unity_MatrixV, 3).z))));
  r1.w = vd_asuint(((-(vd_asfloat(r1.w))) + (-(vd_asfloat(_ProjectionParams.y)))));
  r1.w = vd_asuint(max(vd_asfloat(r1.w), 0.0));
  r1.w = vd_asuint(saturate((vd_asfloat(r1.w) * vd_asfloat(unity_FogParams.z) + vd_asfloat(unity_FogParams.w))));
  r2.x = (vd_asuint(_MainLightLayerMask) & vd_asuint(unity_RenderingLayer.x));
  r2.y = vd_asuint(saturate((vd_asfloat(_MainLightPosition.y) * 0.5 + 0.5)));
  r3.xyz = vd_asuint((vd_asfloat(_MainLightColor.xyz) * vd_asfloat(unity_LightData.zzz) + (-(vd_asfloat(_GlobalCharacterShadowColor.xyz)))));
  r2.yzw = vd_asuint((vd_asfloat(r3.xyz) * vd_asfloat(r2.yyy) + vd_asfloat(_GlobalCharacterShadowColor.xyz)));
  r2.yzw = vd_asuint((vd_asfloat(r1.xyz) * vd_asfloat(r2.yzw)));
  r2.xyz = ((r2.xxx != 0u) ? r2.yzw : uint3(0x00000000u, 0x00000000u, 0x00000000u));
  r3.xy = vd_asuint((vd_asfloat(v4.xz) + (-(vd_asfloat(_PlaneBoundMaxPos.xz)))));
  r3.zw = vd_asuint((vd_asfloat(_PlaneBoundMinPos.xz) + (-(vd_asfloat(_PlaneBoundMaxPos.xz)))));
  r3.xy = vd_asuint(saturate((vd_asfloat(r3.xy) / vd_asfloat(r3.zw))));
  r4.xyzw = vd_asuint(_PlaneSightRenderTarget.SampleLevel(sampler_PlaneSightRenderTarget, vd_asfloat(r3.xy), 0.0).xyzw);
  r2.w = vd_asuint((vd_asfloat(r4.w) * vd_asfloat(_GlobalPlaneSystemEnable)));
  r3.xy = vd_asuint(((-(vd_asfloat(_PlaneBoundMaxPos.xz))) + vd_asfloat((VD_COL(unity_ObjectToWorld, 3).xz))));
  r3.xy = vd_asuint(saturate((vd_asfloat(r3.xy) / vd_asfloat(r3.zw))));
  r3.xyzw = vd_asuint(_PlanePointBuffer.SampleLevel(sampler_PlanePointBuffer, vd_asfloat(r3.xy), 0.0).xyzw);
  r3.x = vd_asuint(saturate((vd_asfloat(r3.w) * 4.0)));
  r3.y = vd_asuint((vd_asfloat(r3.x) * -2.0 + 3.0));
  r3.x = vd_asuint((vd_asfloat(r3.x) * vd_asfloat(r3.x)));
  r3.x = vd_asuint((vd_asfloat(r3.x) * vd_asfloat(r3.y)));
  r3.x = vd_asuint((vd_asfloat(r3.x) * vd_asfloat(_GlobalPlanePointBufferEnable)));
  r2.w = vd_asuint(max(vd_asfloat(r2.w), vd_asfloat(r3.x)));
  r3.xyz = vd_asuint((vd_asfloat(r1.xyz) * vd_asfloat(r2.www)));
  r3.xyz = vd_asuint((vd_asfloat(r3.xyz) * float3(0.25, 0.25, 0.25)));
  r2.w = vd_asuint(min(vd_asfloat(_AdditionalLightsCount.x), vd_asfloat(unity_LightData.y)));
  r2.w = vd_asuint(((int)vd_asfloat(r2.w)));
  r4.xyz = r3.xyz;
  r3.w = 0x00000000u;
  while (true) {
    r4.w = vd_asuint((-(int)(r3.w >= r2.w)));
    if (r4.w != 0u) break;
    r4.w = (r3.w >> (uint)2);
    r5.x = (r3.w & 0x00000003u);
    r4.w = vd_asuint(dot(vd_asfloat(unity_LightIndices[(int)(r4.w)].xyzw), vd_asfloat(icb[r5.x + 0].xyzw)));
    r4.w = vd_asuint(((int)vd_asfloat(r4.w)));
    r5.x = (vd_asuint(unity_RenderingLayer.x) & vd_asuint(_AdditionalLightsLayerMasks[(int)(r4.w)]));
    if (r5.x == 0u) {
      r5.x = vd_asuint((vd_asint(r3.w) + 1));
      r3.w = r5.x;
      continue;
    }
    r5.xyz = vd_asuint(((-(vd_asfloat(v4.xyz))) * vd_asfloat(_AdditionalLightsPosition[(int)(r4.w)].www) + vd_asfloat(_AdditionalLightsPosition[(int)(r4.w)].xyz)));
    r5.w = vd_asuint(dot(vd_asfloat(r5.xyz), vd_asfloat(r5.xyz)));
    r5.w = vd_asuint(max(vd_asfloat(r5.w), 6.10351562e-05));
    r6.x = vd_asuint(rsqrt(vd_asfloat(r5.w)));
    r5.xyz = vd_asuint((vd_asfloat(r5.xyz) * vd_asfloat(r6.xxx)));
    r6.x = vd_asuint((1.0 / vd_asfloat(r5.w)));
    r5.w = vd_asuint((vd_asfloat(r5.w) * vd_asfloat(_AdditionalLightsAttenuation[(int)(r4.w)].x)));
    r5.w = vd_asuint(((-(vd_asfloat(r5.w))) * vd_asfloat(r5.w) + 1.0));
    r5.w = vd_asuint(max(vd_asfloat(r5.w), 0.0));
    r5.w = vd_asuint((vd_asfloat(r5.w) * vd_asfloat(r5.w)));
    r5.w = vd_asuint((vd_asfloat(r5.w) * vd_asfloat(r6.x)));
    r5.x = vd_asuint(dot(vd_asfloat(_AdditionalLightsSpotDir[(int)(r4.w)].xyz), vd_asfloat(r5.xyz)));
    r5.x = vd_asuint(saturate((vd_asfloat(r5.x) * vd_asfloat(_AdditionalLightsAttenuation[(int)(r4.w)].z) + vd_asfloat(_AdditionalLightsAttenuation[(int)(r4.w)].w))));
    r5.x = vd_asuint((vd_asfloat(r5.x) * vd_asfloat(r5.x)));
    r5.x = vd_asuint((vd_asfloat(r5.x) * vd_asfloat(r5.w)));
    r5.xzw = vd_asuint((vd_asfloat(r5.xxx) * vd_asfloat(_AdditionalLightsColor[(int)(r4.w)].xyz)));
    r4.w = vd_asuint(saturate((vd_asfloat(r5.y) * 0.5 + 0.5)));
    r5.xyz = vd_asuint((vd_asfloat(r4.www) * vd_asfloat(r5.xzw)));
    r4.xyz = vd_asuint((vd_asfloat(r5.xyz) * vd_asfloat(r1.xyz) + vd_asfloat(r4.xyz)));
    r3.w = vd_asuint((vd_asint(r3.w) + 1));
  }
  r1.xyz = vd_asuint((vd_asfloat(r2.xyz) + vd_asfloat(r4.xyz)));
  r2.x = vd_asuint((-(int)(0.5 < vd_asfloat(_UseDarkColorMask))));
  if (r2.x != 0u) {
    r0.xyz = vd_asuint(((-(vd_asfloat(r0.xyz))) * vd_asfloat(v8.www) + vd_asfloat(r0.www)));
    r0.xyz = vd_asuint((vd_asfloat(r0.xyz) * vd_asfloat(v9.xyz) + vd_asfloat(r1.xyz)));
  } else {
    r0.xyz = vd_asuint((vd_asfloat(r1.xyz) + vd_asfloat(v9.xyz)));
  }
  r2.xyzw = vd_asuint(_MaskTex.SampleBias(sampler_vd_point_repeat, vd_asfloat(v1.xy), vd_asfloat(_GlobalMipBias.x)).xyzw);
  r3.xyzw = vd_asuint(((-(vd_asfloat(_MaskColor0.xyzw))) + vd_asfloat(_MaskColor1.xyzw)));
  r2.xyzw = vd_asuint((vd_asfloat(r2.xxxx) * vd_asfloat(r3.xyzw) + vd_asfloat(_MaskColor0.xyzw)));
  r0.w = vd_asuint((vd_asfloat(r2.w) * vd_asfloat(_MaskIntensity)));
  r1.xyz = vd_asuint(((-(vd_asfloat(r0.xyz))) + vd_asfloat(r2.xyz)));
  r0.xyz = vd_asuint((vd_asfloat(r0.www) * vd_asfloat(r1.xyz) + vd_asfloat(r0.xyz)));
  r1.xy = vd_asuint((vd_asfloat(v7.xy) / vd_asfloat(v7.ww)));
  r0.w = vd_asuint((vd_asfloat(v2.w) + -1.0));
  r0.w = vd_asuint((vd_asfloat(_SightEffectEnable) * vd_asfloat(r0.w) + vd_asfloat(_Effect)));
  r0.w = vd_asuint((vd_asfloat(r0.w) + 1.0));
  r2.xy = vd_asuint((vd_asfloat(_ScreenSize.xy) / vd_asfloat(_ScreenParams.xy)));
  r1.xy = vd_asuint((vd_asfloat(r1.xy) * vd_asfloat(_ScreenParams.xy)));
  r1.xy = vd_asuint((vd_asfloat(r1.xy) * float2(0.5, 0.5)));
  r1.xy = vd_asuint((vd_asfloat(r2.xy) * vd_asfloat(r1.xy)));
  r1.xy = vd_asuint(floor(vd_asfloat(r1.xy)));
  r1.xy = ((uint2)vd_asfloat(r1.xy));
  r1.xy = (r1.xy & uint2(0x00000003u, 0x00000003u));
  r1.y = (r1.y << (uint)2);
  r1.x = vd_asuint((vd_asint(r1.y) + vd_asint(r1.x)));
  r1.y = x0[r1.x + 0].x;
  r0.w = vd_asuint((vd_asfloat(r0.w) + (-(vd_asfloat(r1.y)))));
  r0.w = vd_asuint((-(int)(vd_asfloat(r0.w) < 0.0)));
  if (r0.w != 0u) discard;
  r0.w = vd_asuint((-(int)(vd_asfloat(_GlobalBoxAreaEnable) >= 0.5)));
  if (r0.w != 0u) {
    r0.w = 0x00000000u;
    while (true) {
      r1.y = vd_asuint((-(int)(vd_asint(r0.w) >= 8)));
      if (r1.y != 0u) break;
      r1.y = vd_asuint((-(int)(vd_asfloat(_GlobalBoxAreaCoverCharacters[(int)(r0.w)]) < 0.5)));
      if (r1.y != 0u) {
        r1.y = vd_asuint((vd_asint(r0.w) + 1));
        r0.w = r1.y;
        continue;
      }
      r2.xyz = vd_asuint((vd_asfloat((VD_COL(unity_ObjectToWorld, 3).xyz)) + (-(vd_asfloat(_GlobalBoxAreaCenters[(int)(r0.w)].xyz)))));
      r2.xyz = vd_asuint((abs(vd_asfloat(r2.xyz)) + (-(vd_asfloat(_GlobalBoxAreaExtents[(int)(r0.w)].xyz)))));
      r1.y = vd_asuint(max(vd_asfloat(r2.y), vd_asfloat(r2.x)));
      r1.y = vd_asuint(max(vd_asfloat(r2.z), vd_asfloat(r1.y)));
      r1.y = vd_asuint((-(int)(0.0 >= vd_asfloat(r1.y))));
      if (r1.y != 0u) {
        if (0xffffffffu != 0u) discard;
        break;
      }
      r0.w = vd_asuint((vd_asint(r0.w) + 1));
    }
  }
  r0.w = vd_asuint((vd_asfloat(_EffectColor.w) * vd_asfloat(_Effect)));
  r2.xyz = vd_asuint(((-(vd_asfloat(r0.xyz))) + vd_asfloat(_EffectColor.xyz)));
  r0.xyz = vd_asuint((vd_asfloat(r0.www) * vd_asfloat(r2.xyz) + vd_asfloat(r0.xyz)));
  r2.xyz = vd_asuint(((-(vd_asfloat(r0.xyz))) + float3(0.5, 0.5, 0.5)));
  r0.xyz = vd_asuint((vd_asfloat((_LightEffectIntensity).xxx) * vd_asfloat(r2.xyz) + vd_asfloat(r0.xyz)));
  r0.w = vd_asuint(saturate((vd_asfloat(_DissolveProgress) + vd_asfloat(_DissolveProgress))));
  r2.xyz = vd_asuint(((-(vd_asfloat(r0.xyz))) + vd_asfloat(_DissolveColor.xyz)));
  r0.xyz = vd_asuint((vd_asfloat(r0.www) * vd_asfloat(r2.xyz) + vd_asfloat(r0.xyz)));
  r0.w = vd_asuint(saturate((vd_asfloat(_DissolveProgress) + -0.5)));
  r0.w = vd_asuint(((-(vd_asfloat(r0.w))) * 2.0 + 1.0));
  r1.x = x0[r1.x + 0].x;
  r0.w = vd_asuint((vd_asfloat(r0.w) + (-(vd_asfloat(r1.x)))));
  r0.w = vd_asuint((-(int)(vd_asfloat(r0.w) < 0.0)));
  if (r0.w != 0u) discard;
  if (vd_asuint(_GlobalWaterAreaEnable) != 0u) {
    r1.xy = vd_asuint(((-(vd_asfloat(_GlobalOverallMapBoundMin.xz))) + vd_asfloat(_GlobalOverallMapBoundMax.xz)));
    r1.xy = vd_asuint(max(vd_asfloat(r1.xy), float2(1.0, 1.0)));
    r2.xy = vd_asuint((vd_asfloat(v4.xz) + (-(vd_asfloat(_GlobalOverallMapBoundMin.xz)))));
    r1.xy = vd_asuint((vd_asfloat(r2.xy) / vd_asfloat(r1.xy)));
    r2.xyzw = vd_asuint(_GlobalWaterAreaMap.SampleBias(sampler_vd_linear_clamp, vd_asfloat(r1.xy), vd_asfloat(_GlobalMipBias.x)).xyzw);
    r0.w = vd_asuint((-(int)(vd_asfloat(r2.z) >= 0.449999988)));
    if (r0.w != 0u) {
      r0.w = vd_asuint(((-(vd_asfloat(_GlobalWaterShoreEdge.x))) + vd_asfloat(_GlobalWaterShoreEdge.y)));
      r1.x = vd_asuint((vd_asfloat(r2.z) + (-(vd_asfloat(_GlobalWaterShoreEdge.x)))));
      r0.w = vd_asuint((1.0 / vd_asfloat(r0.w)));
      r0.w = vd_asuint(saturate((vd_asfloat(r0.w) * vd_asfloat(r1.x))));
      r1.x = vd_asuint((vd_asfloat(r0.w) * -2.0 + 3.0));
      r0.w = vd_asuint((vd_asfloat(r0.w) * vd_asfloat(r0.w)));
      r0.w = vd_asuint((vd_asfloat(r0.w) * vd_asfloat(r1.x)));
      r1.x = vd_asuint(((-(vd_asfloat(_GlobalWaterDepthRange.x))) + vd_asfloat(_GlobalWaterDepthRange.y)));
      r1.y = vd_asuint((vd_asfloat(r2.z) + (-(vd_asfloat(_GlobalWaterDepthRange.x)))));
      r1.x = vd_asuint((1.0 / vd_asfloat(r1.x)));
      r1.x = vd_asuint(saturate((vd_asfloat(r1.x) * vd_asfloat(r1.y))));
      r1.y = vd_asuint((vd_asfloat(r1.x) * -2.0 + 3.0));
      r1.x = vd_asuint((vd_asfloat(r1.x) * vd_asfloat(r1.x)));
      r1.x = vd_asuint((vd_asfloat(r1.x) * vd_asfloat(r1.y)));
      r1.yz = vd_asuint((-(int2)(float2(0.00999999978, 0.00999999978) < vd_asfloat(_GlobalWaterSurfaceWaveMapParams.zw))));
      r1.y = (r1.z & r1.y);
      if (r1.y != 0u) {
        r1.yz = vd_asuint((vd_asfloat(v4.xz) + (-(vd_asfloat(_GlobalWaterSurfaceWaveMapParams.xy)))));
        r1.yz = vd_asuint((vd_asfloat(r1.yz) / vd_asfloat(_GlobalWaterSurfaceWaveMapParams.zw)));
        r1.yz = vd_asuint((vd_asfloat(r1.yz) + float2(0.5, 0.5)));
        r2.xy = vd_asuint(((-(vd_asfloat(r1.yz))) + float2(1.0, 1.0)));
        r2.xy = vd_asuint(min(vd_asfloat(r1.yz), vd_asfloat(r2.xy)));
        r2.x = vd_asuint(min(vd_asfloat(r2.y), vd_asfloat(r2.x)));
        r2.x = vd_asuint(saturate((vd_asfloat(r2.x) * vd_asfloat(_GlobalWaterSurfaceWaveMapParams.z))));
        r3.xyzw = vd_asuint(_GlobalWaterSurfaceWaveMap.SampleBias(sampler_vd_linear_clamp, vd_asfloat(r1.yz), vd_asfloat(_GlobalMipBias.x)).xyzw);
        r1.y = vd_asuint((vd_asfloat(r2.x) * vd_asfloat(r3.x)));
      } else {
        r1.y = 0x00000000u;
      }
      r1.z = vd_asuint(dot(vd_asfloat((VD_COL(unity_ObjectToWorld, 1).xyz)), vd_asfloat((VD_COL(unity_ObjectToWorld, 1).xyz))));
      r1.z = vd_asuint(sqrt(vd_asfloat(r1.z)));
      r1.z = vd_asuint((vd_asfloat(v3.w) * vd_asfloat(r1.z) + vd_asfloat((VD_COL(unity_ObjectToWorld, 3).y))));
      r1.y = vd_asuint((vd_asfloat(r1.y) * 0.125));
      r1.y = vd_asuint((vd_asfloat(_GlobalWaterSurfaceLevel) * vd_asfloat(r1.x) + vd_asfloat(r1.y)));
      r1.y = vd_asuint(((-(vd_asfloat(r1.y))) + vd_asfloat(r1.z)));
      r1.z = vd_asuint((-(int)(0.00999999978 >= vd_asfloat(r1.y))));
      r2.x = vd_asuint(saturate((vd_asfloat(r1.y) * -3.0)));
      r1.x = vd_asuint((vd_asfloat(r1.x) * vd_asfloat(r2.x)));
      r2.y = vd_asuint(dot(vd_asfloat(r0.xyz), float3(0.298999995, 0.587000012, 0.114)));
      r2.y = vd_asuint(saturate((vd_asfloat(r2.y) * 2.0 + 0.300000012)));
      r3.xyzw = vd_asuint((vd_asfloat(_GlobalWaterDeepColor.xyzw) + (-(vd_asfloat(_GlobalWaterShadowColor.xyzw)))));
      r3.xyzw = vd_asuint((vd_asfloat(r2.yyyy) * vd_asfloat(r3.xyzw) + vd_asfloat(_GlobalWaterShadowColor.xyzw)));
      r4.xyzw = vd_asuint((vd_asfloat(_GlobalWaterShallowColor.xyzw) + (-(vd_asfloat(_GlobalWaterShallowShadowColor.xyzw)))));
      r4.xyzw = vd_asuint((vd_asfloat(r2.yyyy) * vd_asfloat(r4.xyzw) + vd_asfloat(_GlobalWaterShallowShadowColor.xyzw)));
      r3.xyzw = vd_asuint((vd_asfloat(r3.xyzw) + (-(vd_asfloat(r4.xyzw)))));
      r3.xyzw = vd_asuint((vd_asfloat(r1.xxxx) * vd_asfloat(r3.xyzw) + vd_asfloat(r4.xyzw)));
      r2.yzw = vd_asuint(((-(vd_asfloat(r0.xyz))) + vd_asfloat(r3.xyz)));
      r2.yzw = vd_asuint((vd_asfloat(r3.www) * vd_asfloat(r2.yzw) + vd_asfloat(r0.xyz)));
      r1.x = vd_asuint((vd_asfloat(r1.y) * (-(vd_asfloat(r1.y)))));
      r1.x = vd_asuint((vd_asfloat(r1.x) * 721.347534));
      r1.x = vd_asuint(exp2(vd_asfloat(r1.x)));
      r1.x = vd_asuint((vd_asfloat(r1.x) * 0.300000012));
      r3.xyz = vd_asuint(((-(vd_asfloat(r2.yzw))) + vd_asfloat(r4.xyz)));
      r2.yzw = vd_asuint((vd_asfloat(r1.xxx) * vd_asfloat(r3.xyz) + vd_asfloat(r2.yzw)));
      r0.w = vd_asuint((vd_asfloat(r0.w) * vd_asfloat(r2.x)));
      r2.xyz = vd_asuint(((-(vd_asfloat(r0.xyz))) + vd_asfloat(r2.yzw)));
      r2.xyz = vd_asuint((vd_asfloat(r0.www) * vd_asfloat(r2.xyz) + vd_asfloat(r0.xyz)));
      r0.xyz = ((r1.zzz != 0u) ? r2.xyz : r0.xyz);
    }
  }
  r0.w = vd_asuint(((-(vd_asfloat(r1.w))) + 1.0));
  r1.x = vd_asuint((vd_asfloat(v3.y) + (-(vd_asfloat(_GlobalFogHeightMin)))));
  r1.y = vd_asuint(((-(vd_asfloat(_GlobalFogHeightMin))) + vd_asfloat(_GlobalFogHeightMax)));
  r1.x = vd_asuint((vd_asfloat(r1.x) / vd_asfloat(r1.y)));
  r1.x = vd_asuint(saturate(((-(vd_asfloat(r1.x))) + 1.0)));
  r1.x = vd_asuint((vd_asfloat(r1.x) * vd_asfloat(_GlobalFogHeightEnable)));
  r0.w = vd_asuint(max(vd_asfloat(r0.w), vd_asfloat(r1.x)));
  r1.xyz = vd_asuint(((-(vd_asfloat(r0.xyz))) + vd_asfloat(unity_FogColor.xyz)));
  o0.xyz = (vd_asfloat(r0.www) * vd_asfloat(r1.xyz) + vd_asfloat(r0.xyz));
  return;
}
