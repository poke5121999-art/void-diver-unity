// LCArt/Background/shader_DE_BG_Base | pass 0 "ForwardLit" | progFragment | keywords: _CLUSTER_LIGHT_LOOP _CURSOR_LINE_ENABLE _DE_EDITOR_CAMERA _LIGHT_LAYERS | blob 54
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
  uint4 r0, r1, r2, r3, r4, r5, r6, r7, r8, r9, r10, r11, r12, r13;
  r0 = 0u; r1 = 0u; r2 = 0u; r3 = 0u; r4 = 0u; r5 = 0u; r6 = 0u; r7 = 0u; r8 = 0u; r9 = 0u; r10 = 0u; r11 = 0u; r12 = 0u; r13 = 0u;
  uint4 x0[16];

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
  r0.x = vd_asuint((-(int)(vd_asfloat(_GlobalBoxAreaEnable) >= 0.5)));
  if (r0.x != 0u) {
    r1.x = vd_asuint(dot(float2(0.530324996, 0.530324996), vd_asfloat(v2.xz)));
    r1.y = vd_asuint(dot(float3(-0.353500009, 0.865999997, 0.353500009), vd_asfloat(v2.xyz)));
    r2.xw = vd_asuint((vd_asfloat(_Time.yy) * float2(0.300000012, 0.230000004)));
    r2.yz = uint2(0x00000000u, 0x00000000u);
    r1.xyzw = vd_asuint((vd_asfloat(r1.xyxy) * float4(0.125, 0.125, 0.125, 0.125) + vd_asfloat(r2.xyzw)));
    r2.xyzw = vd_asuint(_DirtNoiseTexture.SampleBias(sampler_vd_point_repeat, vd_asfloat(r1.xy), vd_asfloat(_GlobalMipBias.x)).xyzw);
    r1.xyzw = vd_asuint(_DirtNoiseTexture.SampleBias(sampler_vd_point_repeat, vd_asfloat(r1.zw), vd_asfloat(_GlobalMipBias.x)).yxzw);
    r0.y = vd_asuint(((int)vd_asfloat(_GlobalBoxAreaCenters[0].w)));
    r0.z = vd_asuint((-(int)(vd_asfloat(_GlobalBoxAreaIntensities[0]) >= 0.00100000005)));
    r0.yw = vd_asuint((-(int2)(vd_asint(r0.yy) == int2(1, 2))));
    r0.y = (r0.w | r0.y);
    r0.y = (r0.y & r0.z);
    r2.yzw = vd_asuint((vd_asfloat(v2.xyz) + (-(vd_asfloat(_GlobalBoxAreaCenters[0].xyz)))));
    r2.yzw = vd_asuint(((-(abs(vd_asfloat(r2.yzw)))) + vd_asfloat(_GlobalBoxAreaExtents[0].xyz)));
    r0.z = vd_asuint(min(vd_asfloat(r2.w), vd_asfloat(r2.z)));
    r0.z = vd_asuint(min(vd_asfloat(r0.z), vd_asfloat(r2.y)));
    r0.w = vd_asuint(max(vd_asfloat(_GlobalBoxAreaExtents[0].w), 0.00100000005));
    r0.z = vd_asuint(saturate((vd_asfloat(r0.z) / vd_asfloat(r0.w))));
    r1.x = r2.x;
    r1.xy = vd_asuint((vd_asfloat(r1.xy) + float2(-0.5, -0.5)));
    r1.xy = vd_asuint((vd_asfloat(r1.xy) + vd_asfloat(r1.xy)));
    r1.zw = vd_asuint((vd_asfloat(r1.xy) * vd_asfloat(_GlobalBoxAreaShimmerParams[0].yy)));
    r0.zw = vd_asuint((vd_asfloat(r0.zz) * vd_asfloat(r1.zw)));
    r0.zw = vd_asuint((vd_asfloat(r0.zw) * vd_asfloat((_GlobalBoxAreaIntensities[0]).xx)));
    r0.yz = (r0.zw & r0.yy);
    r0.w = vd_asuint(((int)vd_asfloat(_GlobalBoxAreaCenters[1].w)));
    r1.z = vd_asuint((-(int)(vd_asfloat(_GlobalBoxAreaIntensities[1]) >= 0.00100000005)));
    r2.xy = vd_asuint((-(int2)(vd_asint(r0.ww) == int2(1, 2))));
    r0.w = (r2.y | r2.x);
    r0.w = (r0.w & r1.z);
    r2.xyz = vd_asuint((vd_asfloat(v2.xyz) + (-(vd_asfloat(_GlobalBoxAreaCenters[1].xyz)))));
    r2.xyz = vd_asuint(((-(abs(vd_asfloat(r2.xyz)))) + vd_asfloat(_GlobalBoxAreaExtents[1].xyz)));
    r1.z = vd_asuint(min(vd_asfloat(r2.z), vd_asfloat(r2.y)));
    r1.z = vd_asuint(min(vd_asfloat(r1.z), vd_asfloat(r2.x)));
    r1.w = vd_asuint(max(vd_asfloat(_GlobalBoxAreaExtents[1].w), 0.00100000005));
    r1.z = vd_asuint(saturate((vd_asfloat(r1.z) / vd_asfloat(r1.w))));
    r2.xy = vd_asuint((vd_asfloat(r1.xy) * vd_asfloat(_GlobalBoxAreaShimmerParams[1].yy)));
    r1.zw = vd_asuint((vd_asfloat(r1.zz) * vd_asfloat(r2.xy)));
    r1.zw = vd_asuint((vd_asfloat(r1.zw) * vd_asfloat((_GlobalBoxAreaIntensities[1]).xx) + vd_asfloat(r0.yz)));
    r0.yz = ((r0.ww != 0u) ? r1.zw : r0.yz);
    r0.w = vd_asuint(((int)vd_asfloat(_GlobalBoxAreaCenters[2].w)));
    r1.z = vd_asuint((-(int)(vd_asfloat(_GlobalBoxAreaIntensities[2]) >= 0.00100000005)));
    r2.xy = vd_asuint((-(int2)(vd_asint(r0.ww) == int2(1, 2))));
    r0.w = (r2.y | r2.x);
    r0.w = (r0.w & r1.z);
    r2.xyz = vd_asuint((vd_asfloat(v2.xyz) + (-(vd_asfloat(_GlobalBoxAreaCenters[2].xyz)))));
    r2.xyz = vd_asuint(((-(abs(vd_asfloat(r2.xyz)))) + vd_asfloat(_GlobalBoxAreaExtents[2].xyz)));
    r1.z = vd_asuint(min(vd_asfloat(r2.z), vd_asfloat(r2.y)));
    r1.z = vd_asuint(min(vd_asfloat(r1.z), vd_asfloat(r2.x)));
    r1.w = vd_asuint(max(vd_asfloat(_GlobalBoxAreaExtents[2].w), 0.00100000005));
    r1.z = vd_asuint(saturate((vd_asfloat(r1.z) / vd_asfloat(r1.w))));
    r2.xy = vd_asuint((vd_asfloat(r1.xy) * vd_asfloat(_GlobalBoxAreaShimmerParams[2].yy)));
    r1.zw = vd_asuint((vd_asfloat(r1.zz) * vd_asfloat(r2.xy)));
    r1.zw = vd_asuint((vd_asfloat(r1.zw) * vd_asfloat((_GlobalBoxAreaIntensities[2]).xx) + vd_asfloat(r0.yz)));
    r0.yz = ((r0.ww != 0u) ? r1.zw : r0.yz);
    r0.w = vd_asuint(((int)vd_asfloat(_GlobalBoxAreaCenters[3].w)));
    r1.z = vd_asuint((-(int)(vd_asfloat(_GlobalBoxAreaIntensities[3]) >= 0.00100000005)));
    r2.xy = vd_asuint((-(int2)(vd_asint(r0.ww) == int2(1, 2))));
    r0.w = (r2.y | r2.x);
    r0.w = (r0.w & r1.z);
    r2.xyz = vd_asuint((vd_asfloat(v2.xyz) + (-(vd_asfloat(_GlobalBoxAreaCenters[3].xyz)))));
    r2.xyz = vd_asuint(((-(abs(vd_asfloat(r2.xyz)))) + vd_asfloat(_GlobalBoxAreaExtents[3].xyz)));
    r1.z = vd_asuint(min(vd_asfloat(r2.z), vd_asfloat(r2.y)));
    r1.z = vd_asuint(min(vd_asfloat(r1.z), vd_asfloat(r2.x)));
    r1.w = vd_asuint(max(vd_asfloat(_GlobalBoxAreaExtents[3].w), 0.00100000005));
    r1.z = vd_asuint(saturate((vd_asfloat(r1.z) / vd_asfloat(r1.w))));
    r2.xy = vd_asuint((vd_asfloat(r1.xy) * vd_asfloat(_GlobalBoxAreaShimmerParams[3].yy)));
    r1.zw = vd_asuint((vd_asfloat(r1.zz) * vd_asfloat(r2.xy)));
    r1.zw = vd_asuint((vd_asfloat(r1.zw) * vd_asfloat((_GlobalBoxAreaIntensities[3]).xx) + vd_asfloat(r0.yz)));
    r0.yz = ((r0.ww != 0u) ? r1.zw : r0.yz);
    r0.w = vd_asuint(((int)vd_asfloat(_GlobalBoxAreaCenters[4].w)));
    r1.z = vd_asuint((-(int)(vd_asfloat(_GlobalBoxAreaIntensities[4]) >= 0.00100000005)));
    r2.xy = vd_asuint((-(int2)(vd_asint(r0.ww) == int2(1, 2))));
    r0.w = (r2.y | r2.x);
    r0.w = (r0.w & r1.z);
    r2.xyz = vd_asuint((vd_asfloat(v2.xyz) + (-(vd_asfloat(_GlobalBoxAreaCenters[4].xyz)))));
    r2.xyz = vd_asuint(((-(abs(vd_asfloat(r2.xyz)))) + vd_asfloat(_GlobalBoxAreaExtents[4].xyz)));
    r1.z = vd_asuint(min(vd_asfloat(r2.z), vd_asfloat(r2.y)));
    r1.z = vd_asuint(min(vd_asfloat(r1.z), vd_asfloat(r2.x)));
    r1.w = vd_asuint(max(vd_asfloat(_GlobalBoxAreaExtents[4].w), 0.00100000005));
    r1.z = vd_asuint(saturate((vd_asfloat(r1.z) / vd_asfloat(r1.w))));
    r2.xy = vd_asuint((vd_asfloat(r1.xy) * vd_asfloat(_GlobalBoxAreaShimmerParams[4].yy)));
    r1.zw = vd_asuint((vd_asfloat(r1.zz) * vd_asfloat(r2.xy)));
    r1.zw = vd_asuint((vd_asfloat(r1.zw) * vd_asfloat((_GlobalBoxAreaIntensities[4]).xx) + vd_asfloat(r0.yz)));
    r0.yz = ((r0.ww != 0u) ? r1.zw : r0.yz);
    r0.w = vd_asuint(((int)vd_asfloat(_GlobalBoxAreaCenters[5].w)));
    r1.z = vd_asuint((-(int)(vd_asfloat(_GlobalBoxAreaIntensities[5]) >= 0.00100000005)));
    r2.xy = vd_asuint((-(int2)(vd_asint(r0.ww) == int2(1, 2))));
    r0.w = (r2.y | r2.x);
    r0.w = (r0.w & r1.z);
    r2.xyz = vd_asuint((vd_asfloat(v2.xyz) + (-(vd_asfloat(_GlobalBoxAreaCenters[5].xyz)))));
    r2.xyz = vd_asuint(((-(abs(vd_asfloat(r2.xyz)))) + vd_asfloat(_GlobalBoxAreaExtents[5].xyz)));
    r1.z = vd_asuint(min(vd_asfloat(r2.z), vd_asfloat(r2.y)));
    r1.z = vd_asuint(min(vd_asfloat(r1.z), vd_asfloat(r2.x)));
    r1.w = vd_asuint(max(vd_asfloat(_GlobalBoxAreaExtents[5].w), 0.00100000005));
    r1.z = vd_asuint(saturate((vd_asfloat(r1.z) / vd_asfloat(r1.w))));
    r2.xy = vd_asuint((vd_asfloat(r1.xy) * vd_asfloat(_GlobalBoxAreaShimmerParams[5].yy)));
    r1.zw = vd_asuint((vd_asfloat(r1.zz) * vd_asfloat(r2.xy)));
    r1.zw = vd_asuint((vd_asfloat(r1.zw) * vd_asfloat((_GlobalBoxAreaIntensities[5]).xx) + vd_asfloat(r0.yz)));
    r0.yz = ((r0.ww != 0u) ? r1.zw : r0.yz);
    r0.w = vd_asuint(((int)vd_asfloat(_GlobalBoxAreaCenters[6].w)));
    r1.z = vd_asuint((-(int)(vd_asfloat(_GlobalBoxAreaIntensities[6]) >= 0.00100000005)));
    r2.xy = vd_asuint((-(int2)(vd_asint(r0.ww) == int2(1, 2))));
    r0.w = (r2.y | r2.x);
    r0.w = (r0.w & r1.z);
    r2.xyz = vd_asuint((vd_asfloat(v2.xyz) + (-(vd_asfloat(_GlobalBoxAreaCenters[6].xyz)))));
    r2.xyz = vd_asuint(((-(abs(vd_asfloat(r2.xyz)))) + vd_asfloat(_GlobalBoxAreaExtents[6].xyz)));
    r1.z = vd_asuint(min(vd_asfloat(r2.z), vd_asfloat(r2.y)));
    r1.z = vd_asuint(min(vd_asfloat(r1.z), vd_asfloat(r2.x)));
    r1.w = vd_asuint(max(vd_asfloat(_GlobalBoxAreaExtents[6].w), 0.00100000005));
    r1.z = vd_asuint(saturate((vd_asfloat(r1.z) / vd_asfloat(r1.w))));
    r2.xy = vd_asuint((vd_asfloat(r1.xy) * vd_asfloat(_GlobalBoxAreaShimmerParams[6].yy)));
    r1.zw = vd_asuint((vd_asfloat(r1.zz) * vd_asfloat(r2.xy)));
    r1.zw = vd_asuint((vd_asfloat(r1.zw) * vd_asfloat((_GlobalBoxAreaIntensities[6]).xx) + vd_asfloat(r0.yz)));
    r0.yz = ((r0.ww != 0u) ? r1.zw : r0.yz);
    r0.w = vd_asuint(((int)vd_asfloat(_GlobalBoxAreaCenters[7].w)));
    r1.z = vd_asuint((-(int)(vd_asfloat(_GlobalBoxAreaIntensities[7]) >= 0.00100000005)));
    r2.xy = vd_asuint((-(int2)(vd_asint(r0.ww) == int2(1, 2))));
    r0.w = (r2.y | r2.x);
    r0.w = (r0.w & r1.z);
    r2.xyz = vd_asuint((vd_asfloat(v2.xyz) + (-(vd_asfloat(_GlobalBoxAreaCenters[7].xyz)))));
    r2.xyz = vd_asuint(((-(abs(vd_asfloat(r2.xyz)))) + vd_asfloat(_GlobalBoxAreaExtents[7].xyz)));
    r1.z = vd_asuint(min(vd_asfloat(r2.z), vd_asfloat(r2.y)));
    r1.z = vd_asuint(min(vd_asfloat(r1.z), vd_asfloat(r2.x)));
    r1.w = vd_asuint(max(vd_asfloat(_GlobalBoxAreaExtents[7].w), 0.00100000005));
    r1.z = vd_asuint(saturate((vd_asfloat(r1.z) / vd_asfloat(r1.w))));
    r1.xy = vd_asuint((vd_asfloat(r1.xy) * vd_asfloat(_GlobalBoxAreaShimmerParams[7].yy)));
    r1.xy = vd_asuint((vd_asfloat(r1.zz) * vd_asfloat(r1.xy)));
    r1.xy = vd_asuint((vd_asfloat(r1.xy) * vd_asfloat((_GlobalBoxAreaIntensities[7]).xx) + vd_asfloat(r0.yz)));
    r0.yz = ((r0.ww != 0u) ? r1.xy : r0.yz);
  } else {
    r0.yz = uint2(0x00000000u, 0x00000000u);
  }
  r0.yz = vd_asuint((vd_asfloat(r0.yz) + vd_asfloat(v0.xy)));
  r1.xyzw = vd_asuint(_BaseMap.SampleBias(sampler_BaseMap, vd_asfloat(r0.yz), vd_asfloat(_GlobalMipBias.x)).xyzw);
  r0.w = vd_asuint((vd_asfloat(r1.w) * vd_asfloat(_BaseColor.w)));
  r2.xyzw = vd_asuint(_EmissionMap.SampleLevel(sampler_vd_point_repeat, vd_asfloat(r0.yz), 0.0).xyzw);
  r1.w = vd_asuint(((-(vd_asfloat(r2.z))) + 0.100000001));
  r1.w = vd_asuint((-(int)(vd_asfloat(r1.w) < 0.0)));
  if (r1.w != 0u) discard;
  r1.w = vd_asuint((-(int)(vd_asfloat(r0.w) < 0.5)));
  if (r1.w != 0u) discard;
  r1.w = vd_asuint((-(int)(1.5 < vd_asfloat(v1.w))));
  if (r1.w != 0u) {
    r2.xz = vd_asuint((float2(1.0, 1.0) / vd_asfloat(_ScaledScreenParams.xy)));
    r3.xy = vd_asuint((vd_asfloat(r2.xz) * vd_asfloat(v7.xy)));
#if UNITY_UV_STARTS_AT_TOP
    r1.w = vd_asuint((vd_asfloat(r3.y) * vd_asfloat(_ScaleBiasRt.x) + vd_asfloat(_ScaleBiasRt.y)));
    r3.z = vd_asuint(((-(vd_asfloat(r1.w))) + 1.0));
#else
    r1.w = r3.y;
    r3.z = r1.w;
#endif
    r1.w = vd_asuint(((-(vd_asfloat(_GlobalSeeThroughAlpha))) + 1.0));
    r1.w = vd_asuint((vd_asfloat(v0.z) * vd_asfloat(r1.w) + vd_asfloat(_GlobalSeeThroughAlpha)));
    r2.x = vd_asuint((-(int)(vd_asint(_BackgroundType) == 9)));
    r2.z = vd_asuint((vd_asfloat(r1.w) * vd_asfloat(v8.w)));
    r1.w = vd_asuint((vd_asfloat(r1.w) + -1.0));
    r1.w = vd_asuint((vd_asfloat(_GlobalIsRuntime) * vd_asfloat(r1.w) + 1.0));
    r1.w = ((r2.x != 0u) ? r2.z : r1.w);
    r2.xz = vd_asuint((vd_asfloat(_ScreenSize.xy) / vd_asfloat(_ScreenParams.xy)));
    r3.xy = vd_asuint((vd_asfloat(r3.xz) * vd_asfloat(_ScreenParams.xy)));
    r2.xz = vd_asuint((vd_asfloat(r2.xz) * vd_asfloat(r3.xy)));
    r2.xz = vd_asuint(floor(vd_asfloat(r2.xz)));
    r2.xz = ((uint2)vd_asfloat(r2.xz));
    r2.xz = (r2.xz & uint2(0x00000003u, 0x00000003u));
    r2.z = (r2.z << (uint)2);
    r2.x = vd_asuint((vd_asint(r2.z) + vd_asint(r2.x)));
    r2.x = x0[r2.x + 0].x;
    r1.w = vd_asuint((vd_asfloat(r1.w) * vd_asfloat(_BaseColor.w) + (-(vd_asfloat(r2.x)))));
    r1.w = vd_asuint((-(int)(vd_asfloat(r1.w) < 0.0)));
    if (r1.w != 0u) discard;
  }
  r1.xyz = vd_asuint((vd_asfloat(r1.xyz) * vd_asfloat(_BaseColor.xyz)));
  r3.xyzw = vd_asuint(_BumpMap.SampleBias(sampler_vd_point_repeat, vd_asfloat(r0.yz), vd_asfloat(_GlobalMipBias.x)).xyzw);
  r3.x = vd_asuint((vd_asfloat(r3.x) * vd_asfloat(r3.w)));
  r4.xy = vd_asuint((vd_asfloat(r3.xy) + vd_asfloat(r3.xy)));
  r2.xz = vd_asuint((vd_asfloat(r3.xy) * float2(2.0, 2.0) + float2(-1.0, -1.0)));
  r1.w = vd_asuint(dot(vd_asfloat(r2.xz), vd_asfloat(r2.xz)));
  r1.w = vd_asuint(min(vd_asfloat(r1.w), 1.0));
  r1.w = vd_asuint(((-(vd_asfloat(r1.w))) + 1.0));
  r1.w = vd_asuint(sqrt(vd_asfloat(r1.w)));
  r4.z = vd_asuint(max(vd_asfloat(r1.w), 1.00000002e-16));
  r2.xzw = vd_asuint((vd_asfloat(r4.xyz) + float3(-1.0, -1.0, -1.0)));
  r2.xzw = vd_asuint((vd_asfloat((_BumpScale).xxx) * vd_asfloat(r2.xzw) + float3(0.0, 0.0, 1.0)));
  r3.xyzw = vd_asuint(_EmissionMap.SampleBias(sampler_vd_point_repeat, vd_asfloat(r0.yz), vd_asfloat(_GlobalMipBias.x)).xyzw);
  r1.w = vd_asuint((-(int)(vd_asfloat(r3.z) >= 0.100000001)));
  r1.w = ((r1.w != 0u) ? 0x00000000u : 0x3f800000u);
  r1.w = vd_asuint((vd_asfloat(r1.w) * vd_asfloat(r3.x)));
  r3.xyz = vd_asuint((vd_asfloat(r1.xyz) * vd_asfloat(r1.www)));
  if (r0.x != 0u) {
    r4.xy = vd_asuint((vd_asfloat(v2.xz) * float2(0.5, 0.5)));
    r4.zw = vd_asuint(floor(vd_asfloat(r4.xy)));
    r4.xy = vd_asuint(frac(vd_asfloat(r4.xy)));
    r5.xy = vd_asuint((vd_asfloat(r4.xy) * vd_asfloat(r4.xy)));
    r4.xy = vd_asuint(((-(vd_asfloat(r4.xy))) * float2(2.0, 2.0) + float2(3.0, 3.0)));
    r4.xy = vd_asuint((vd_asfloat(r4.xy) * vd_asfloat(r5.xy)));
    r3.w = vd_asuint(dot(vd_asfloat(r4.zw), float2(127.099998, 311.700012)));
    r3.w = vd_asuint(sin(vd_asfloat(r3.w)));
    r3.w = vd_asuint((vd_asfloat(r3.w) * 43758.5469));
    r3.w = vd_asuint(frac(vd_asfloat(r3.w)));
    r5.xyzw = vd_asuint((vd_asfloat(r4.zwzw) + float4(1.0, 0.0, 0.0, 1.0)));
    r5.x = vd_asuint(dot(vd_asfloat(r5.xy), float2(127.099998, 311.700012)));
    r5.x = vd_asuint(sin(vd_asfloat(r5.x)));
    r5.x = vd_asuint((vd_asfloat(r5.x) * 43758.5469));
    r5.y = vd_asuint(dot(vd_asfloat(r5.zw), float2(127.099998, 311.700012)));
    r5.y = vd_asuint(sin(vd_asfloat(r5.y)));
    r5.y = vd_asuint((vd_asfloat(r5.y) * 43758.5469));
    r5.xy = vd_asuint(frac(vd_asfloat(r5.xy)));
    r4.zw = vd_asuint((vd_asfloat(r4.zw) + float2(1.0, 1.0)));
    r4.z = vd_asuint(dot(vd_asfloat(r4.zw), float2(127.099998, 311.700012)));
    r4.z = vd_asuint(sin(vd_asfloat(r4.z)));
    r4.z = vd_asuint((vd_asfloat(r4.z) * 43758.5469));
    r4.z = vd_asuint(frac(vd_asfloat(r4.z)));
    r4.w = vd_asuint(((-(vd_asfloat(r3.w))) + vd_asfloat(r5.x)));
    r3.w = vd_asuint((vd_asfloat(r4.x) * vd_asfloat(r4.w) + vd_asfloat(r3.w)));
    r4.z = vd_asuint(((-(vd_asfloat(r5.y))) + vd_asfloat(r4.z)));
    r4.x = vd_asuint((vd_asfloat(r4.x) * vd_asfloat(r4.z) + vd_asfloat(r5.y)));
    r4.x = vd_asuint(((-(vd_asfloat(r3.w))) + vd_asfloat(r4.x)));
    r3.w = vd_asuint((vd_asfloat(r4.y) * vd_asfloat(r4.x) + vd_asfloat(r3.w)));
    r4.x = vd_asuint(((int)vd_asfloat(_GlobalBoxAreaCenters[0].w)));
    r4.x = vd_asuint((-(int)(vd_asint(r4.x) == 4)));
    if (r4.x != 0u) {
      r4.xyz = vd_asuint((vd_asfloat(v2.xyz) + (-(vd_asfloat(_GlobalBoxAreaCenters[0].xyz)))));
      r4.xyz = vd_asuint(((-(abs(vd_asfloat(r4.xyz)))) + vd_asfloat(_GlobalBoxAreaExtents[0].xyz)));
      r4.y = vd_asuint(min(vd_asfloat(r4.z), vd_asfloat(r4.y)));
      r4.x = vd_asuint(min(vd_asfloat(r4.y), vd_asfloat(r4.x)));
      r4.y = vd_asuint(max(vd_asfloat(_GlobalBoxAreaExtents[0].w), 0.00100000005));
      r4.x = vd_asuint(saturate((vd_asfloat(r4.x) / vd_asfloat(r4.y))));
      r4.y = vd_asuint((-(int)(0.0 < vd_asfloat(r4.x))));
      if (r4.y != 0u) {
        r4.y = vd_asuint(((-(vd_asfloat(_GlobalBoxAreaIntensities[0]))) + 1.125));
        r4.y = vd_asuint((vd_asfloat(r3.w) + (-(vd_asfloat(r4.y)))));
        r4.y = vd_asuint(saturate((vd_asfloat(r4.y) * -4.0)));
        r4.z = vd_asuint((vd_asfloat(r4.y) * -2.0 + 3.0));
        r4.y = vd_asuint((vd_asfloat(r4.y) * vd_asfloat(r4.y)));
        r4.y = vd_asuint((vd_asfloat(r4.z) * vd_asfloat(r4.y) + -0.5));
        r4.x = vd_asuint(((-(vd_asfloat(r4.x))) + vd_asfloat(r4.y)));
        r4.x = vd_asuint((vd_asfloat(r4.x) + 1.0));
        r4.x = vd_asuint((-(int)(vd_asfloat(r4.x) < 0.0)));
        if (r4.x != 0u) discard;
      }
    }
    r4.x = vd_asuint(((int)vd_asfloat(_GlobalBoxAreaCenters[1].w)));
    r4.x = vd_asuint((-(int)(vd_asint(r4.x) == 4)));
    if (r4.x != 0u) {
      r4.xyz = vd_asuint((vd_asfloat(v2.xyz) + (-(vd_asfloat(_GlobalBoxAreaCenters[1].xyz)))));
      r4.xyz = vd_asuint(((-(abs(vd_asfloat(r4.xyz)))) + vd_asfloat(_GlobalBoxAreaExtents[1].xyz)));
      r4.y = vd_asuint(min(vd_asfloat(r4.z), vd_asfloat(r4.y)));
      r4.x = vd_asuint(min(vd_asfloat(r4.y), vd_asfloat(r4.x)));
      r4.y = vd_asuint(max(vd_asfloat(_GlobalBoxAreaExtents[1].w), 0.00100000005));
      r4.x = vd_asuint(saturate((vd_asfloat(r4.x) / vd_asfloat(r4.y))));
      r4.y = vd_asuint((-(int)(0.0 < vd_asfloat(r4.x))));
      if (r4.y != 0u) {
        r4.y = vd_asuint(((-(vd_asfloat(_GlobalBoxAreaIntensities[1]))) + 1.125));
        r4.y = vd_asuint((vd_asfloat(r3.w) + (-(vd_asfloat(r4.y)))));
        r4.y = vd_asuint(saturate((vd_asfloat(r4.y) * -4.0)));
        r4.z = vd_asuint((vd_asfloat(r4.y) * -2.0 + 3.0));
        r4.y = vd_asuint((vd_asfloat(r4.y) * vd_asfloat(r4.y)));
        r4.y = vd_asuint((vd_asfloat(r4.z) * vd_asfloat(r4.y) + -0.5));
        r4.x = vd_asuint(((-(vd_asfloat(r4.x))) + vd_asfloat(r4.y)));
        r4.x = vd_asuint((vd_asfloat(r4.x) + 1.0));
        r4.x = vd_asuint((-(int)(vd_asfloat(r4.x) < 0.0)));
        if (r4.x != 0u) discard;
      }
    }
    r4.x = vd_asuint(((int)vd_asfloat(_GlobalBoxAreaCenters[2].w)));
    r4.x = vd_asuint((-(int)(vd_asint(r4.x) == 4)));
    if (r4.x != 0u) {
      r4.xyz = vd_asuint((vd_asfloat(v2.xyz) + (-(vd_asfloat(_GlobalBoxAreaCenters[2].xyz)))));
      r4.xyz = vd_asuint(((-(abs(vd_asfloat(r4.xyz)))) + vd_asfloat(_GlobalBoxAreaExtents[2].xyz)));
      r4.y = vd_asuint(min(vd_asfloat(r4.z), vd_asfloat(r4.y)));
      r4.x = vd_asuint(min(vd_asfloat(r4.y), vd_asfloat(r4.x)));
      r4.y = vd_asuint(max(vd_asfloat(_GlobalBoxAreaExtents[2].w), 0.00100000005));
      r4.x = vd_asuint(saturate((vd_asfloat(r4.x) / vd_asfloat(r4.y))));
      r4.y = vd_asuint((-(int)(0.0 < vd_asfloat(r4.x))));
      if (r4.y != 0u) {
        r4.y = vd_asuint(((-(vd_asfloat(_GlobalBoxAreaIntensities[2]))) + 1.125));
        r4.y = vd_asuint((vd_asfloat(r3.w) + (-(vd_asfloat(r4.y)))));
        r4.y = vd_asuint(saturate((vd_asfloat(r4.y) * -4.0)));
        r4.z = vd_asuint((vd_asfloat(r4.y) * -2.0 + 3.0));
        r4.y = vd_asuint((vd_asfloat(r4.y) * vd_asfloat(r4.y)));
        r4.y = vd_asuint((vd_asfloat(r4.z) * vd_asfloat(r4.y) + -0.5));
        r4.x = vd_asuint(((-(vd_asfloat(r4.x))) + vd_asfloat(r4.y)));
        r4.x = vd_asuint((vd_asfloat(r4.x) + 1.0));
        r4.x = vd_asuint((-(int)(vd_asfloat(r4.x) < 0.0)));
        if (r4.x != 0u) discard;
      }
    }
    r4.x = vd_asuint(((int)vd_asfloat(_GlobalBoxAreaCenters[3].w)));
    r4.x = vd_asuint((-(int)(vd_asint(r4.x) == 4)));
    if (r4.x != 0u) {
      r4.xyz = vd_asuint((vd_asfloat(v2.xyz) + (-(vd_asfloat(_GlobalBoxAreaCenters[3].xyz)))));
      r4.xyz = vd_asuint(((-(abs(vd_asfloat(r4.xyz)))) + vd_asfloat(_GlobalBoxAreaExtents[3].xyz)));
      r4.y = vd_asuint(min(vd_asfloat(r4.z), vd_asfloat(r4.y)));
      r4.x = vd_asuint(min(vd_asfloat(r4.y), vd_asfloat(r4.x)));
      r4.y = vd_asuint(max(vd_asfloat(_GlobalBoxAreaExtents[3].w), 0.00100000005));
      r4.x = vd_asuint(saturate((vd_asfloat(r4.x) / vd_asfloat(r4.y))));
      r4.y = vd_asuint((-(int)(0.0 < vd_asfloat(r4.x))));
      if (r4.y != 0u) {
        r4.y = vd_asuint(((-(vd_asfloat(_GlobalBoxAreaIntensities[3]))) + 1.125));
        r4.y = vd_asuint((vd_asfloat(r3.w) + (-(vd_asfloat(r4.y)))));
        r4.y = vd_asuint(saturate((vd_asfloat(r4.y) * -4.0)));
        r4.z = vd_asuint((vd_asfloat(r4.y) * -2.0 + 3.0));
        r4.y = vd_asuint((vd_asfloat(r4.y) * vd_asfloat(r4.y)));
        r4.y = vd_asuint((vd_asfloat(r4.z) * vd_asfloat(r4.y) + -0.5));
        r4.x = vd_asuint(((-(vd_asfloat(r4.x))) + vd_asfloat(r4.y)));
        r4.x = vd_asuint((vd_asfloat(r4.x) + 1.0));
        r4.x = vd_asuint((-(int)(vd_asfloat(r4.x) < 0.0)));
        if (r4.x != 0u) discard;
      }
    }
    r4.x = vd_asuint(((int)vd_asfloat(_GlobalBoxAreaCenters[4].w)));
    r4.x = vd_asuint((-(int)(vd_asint(r4.x) == 4)));
    if (r4.x != 0u) {
      r4.xyz = vd_asuint((vd_asfloat(v2.xyz) + (-(vd_asfloat(_GlobalBoxAreaCenters[4].xyz)))));
      r4.xyz = vd_asuint(((-(abs(vd_asfloat(r4.xyz)))) + vd_asfloat(_GlobalBoxAreaExtents[4].xyz)));
      r4.y = vd_asuint(min(vd_asfloat(r4.z), vd_asfloat(r4.y)));
      r4.x = vd_asuint(min(vd_asfloat(r4.y), vd_asfloat(r4.x)));
      r4.y = vd_asuint(max(vd_asfloat(_GlobalBoxAreaExtents[4].w), 0.00100000005));
      r4.x = vd_asuint(saturate((vd_asfloat(r4.x) / vd_asfloat(r4.y))));
      r4.y = vd_asuint((-(int)(0.0 < vd_asfloat(r4.x))));
      if (r4.y != 0u) {
        r4.y = vd_asuint(((-(vd_asfloat(_GlobalBoxAreaIntensities[4]))) + 1.125));
        r4.y = vd_asuint((vd_asfloat(r3.w) + (-(vd_asfloat(r4.y)))));
        r4.y = vd_asuint(saturate((vd_asfloat(r4.y) * -4.0)));
        r4.z = vd_asuint((vd_asfloat(r4.y) * -2.0 + 3.0));
        r4.y = vd_asuint((vd_asfloat(r4.y) * vd_asfloat(r4.y)));
        r4.y = vd_asuint((vd_asfloat(r4.z) * vd_asfloat(r4.y) + -0.5));
        r4.x = vd_asuint(((-(vd_asfloat(r4.x))) + vd_asfloat(r4.y)));
        r4.x = vd_asuint((vd_asfloat(r4.x) + 1.0));
        r4.x = vd_asuint((-(int)(vd_asfloat(r4.x) < 0.0)));
        if (r4.x != 0u) discard;
      }
    }
    r4.x = vd_asuint(((int)vd_asfloat(_GlobalBoxAreaCenters[5].w)));
    r4.x = vd_asuint((-(int)(vd_asint(r4.x) == 4)));
    if (r4.x != 0u) {
      r4.xyz = vd_asuint((vd_asfloat(v2.xyz) + (-(vd_asfloat(_GlobalBoxAreaCenters[5].xyz)))));
      r4.xyz = vd_asuint(((-(abs(vd_asfloat(r4.xyz)))) + vd_asfloat(_GlobalBoxAreaExtents[5].xyz)));
      r4.y = vd_asuint(min(vd_asfloat(r4.z), vd_asfloat(r4.y)));
      r4.x = vd_asuint(min(vd_asfloat(r4.y), vd_asfloat(r4.x)));
      r4.y = vd_asuint(max(vd_asfloat(_GlobalBoxAreaExtents[5].w), 0.00100000005));
      r4.x = vd_asuint(saturate((vd_asfloat(r4.x) / vd_asfloat(r4.y))));
      r4.y = vd_asuint((-(int)(0.0 < vd_asfloat(r4.x))));
      if (r4.y != 0u) {
        r4.y = vd_asuint(((-(vd_asfloat(_GlobalBoxAreaIntensities[5]))) + 1.125));
        r4.y = vd_asuint((vd_asfloat(r3.w) + (-(vd_asfloat(r4.y)))));
        r4.y = vd_asuint(saturate((vd_asfloat(r4.y) * -4.0)));
        r4.z = vd_asuint((vd_asfloat(r4.y) * -2.0 + 3.0));
        r4.y = vd_asuint((vd_asfloat(r4.y) * vd_asfloat(r4.y)));
        r4.y = vd_asuint((vd_asfloat(r4.z) * vd_asfloat(r4.y) + -0.5));
        r4.x = vd_asuint(((-(vd_asfloat(r4.x))) + vd_asfloat(r4.y)));
        r4.x = vd_asuint((vd_asfloat(r4.x) + 1.0));
        r4.x = vd_asuint((-(int)(vd_asfloat(r4.x) < 0.0)));
        if (r4.x != 0u) discard;
      }
    }
    r4.x = vd_asuint(((int)vd_asfloat(_GlobalBoxAreaCenters[6].w)));
    r4.x = vd_asuint((-(int)(vd_asint(r4.x) == 4)));
    if (r4.x != 0u) {
      r4.xyz = vd_asuint((vd_asfloat(v2.xyz) + (-(vd_asfloat(_GlobalBoxAreaCenters[6].xyz)))));
      r4.xyz = vd_asuint(((-(abs(vd_asfloat(r4.xyz)))) + vd_asfloat(_GlobalBoxAreaExtents[6].xyz)));
      r4.y = vd_asuint(min(vd_asfloat(r4.z), vd_asfloat(r4.y)));
      r4.x = vd_asuint(min(vd_asfloat(r4.y), vd_asfloat(r4.x)));
      r4.y = vd_asuint(max(vd_asfloat(_GlobalBoxAreaExtents[6].w), 0.00100000005));
      r4.x = vd_asuint(saturate((vd_asfloat(r4.x) / vd_asfloat(r4.y))));
      r4.y = vd_asuint((-(int)(0.0 < vd_asfloat(r4.x))));
      if (r4.y != 0u) {
        r4.y = vd_asuint(((-(vd_asfloat(_GlobalBoxAreaIntensities[6]))) + 1.125));
        r4.y = vd_asuint((vd_asfloat(r3.w) + (-(vd_asfloat(r4.y)))));
        r4.y = vd_asuint(saturate((vd_asfloat(r4.y) * -4.0)));
        r4.z = vd_asuint((vd_asfloat(r4.y) * -2.0 + 3.0));
        r4.y = vd_asuint((vd_asfloat(r4.y) * vd_asfloat(r4.y)));
        r4.y = vd_asuint((vd_asfloat(r4.z) * vd_asfloat(r4.y) + -0.5));
        r4.x = vd_asuint(((-(vd_asfloat(r4.x))) + vd_asfloat(r4.y)));
        r4.x = vd_asuint((vd_asfloat(r4.x) + 1.0));
        r4.x = vd_asuint((-(int)(vd_asfloat(r4.x) < 0.0)));
        if (r4.x != 0u) discard;
      }
    }
    r4.x = vd_asuint(((int)vd_asfloat(_GlobalBoxAreaCenters[7].w)));
    r4.x = vd_asuint((-(int)(vd_asint(r4.x) == 4)));
    r4.yzw = vd_asuint((vd_asfloat(v2.xyz) + (-(vd_asfloat(_GlobalBoxAreaCenters[7].xyz)))));
    r4.yzw = vd_asuint(((-(abs(vd_asfloat(r4.yzw)))) + vd_asfloat(_GlobalBoxAreaExtents[7].xyz)));
    r4.z = vd_asuint(min(vd_asfloat(r4.w), vd_asfloat(r4.z)));
    r4.y = vd_asuint(min(vd_asfloat(r4.z), vd_asfloat(r4.y)));
    r4.z = vd_asuint(max(vd_asfloat(_GlobalBoxAreaExtents[7].w), 0.00100000005));
    r4.y = vd_asuint(saturate((vd_asfloat(r4.y) / vd_asfloat(r4.z))));
    r4.z = vd_asuint((-(int)(0.0 < vd_asfloat(r4.y))));
    r4.x = (r4.z & r4.x);
    r4.z = vd_asuint(((-(vd_asfloat(_GlobalBoxAreaIntensities[7]))) + 1.125));
    r3.w = vd_asuint((vd_asfloat(r3.w) + (-(vd_asfloat(r4.z)))));
    r3.w = vd_asuint(saturate((vd_asfloat(r3.w) * -4.0)));
    r4.z = vd_asuint((vd_asfloat(r3.w) * -2.0 + 3.0));
    r3.w = vd_asuint((vd_asfloat(r3.w) * vd_asfloat(r3.w)));
    r3.w = vd_asuint((vd_asfloat(r4.z) * vd_asfloat(r3.w) + -0.5));
    r3.w = vd_asuint(((-(vd_asfloat(r4.y))) + vd_asfloat(r3.w)));
    r3.w = vd_asuint((vd_asfloat(r3.w) + 1.0));
    r3.w = vd_asuint((-(int)(vd_asfloat(r3.w) < 0.0)));
    r3.w = (r4.x & r3.w);
    if (r3.w != 0u) discard;
  }
  r3.w = vd_asuint((-(int)(0.5 < vd_asfloat(_UseWorldNormalMap))));
  if (r3.w != 0u) {
    r4.xyzw = vd_asuint(_WorldNormalMap.SampleBias(sampler_vd_point_repeat, vd_asfloat(r0.yz), vd_asfloat(_GlobalMipBias.x)).xyzw);
    r4.xyz = vd_asuint((vd_asfloat(r4.xyz) * float3(2.0, 2.0, 2.0) + float3(-1.0, -1.0, -1.0)));
    r0.y = vd_asuint(dot(vd_asfloat(r4.xyz), vd_asfloat(r4.xyz)));
    r0.y = vd_asuint(rsqrt(vd_asfloat(r0.y)));
    r4.xyz = vd_asuint((vd_asfloat(r0.yyy) * vd_asfloat(r4.xyz)));
    r0.y = vd_asuint(dot(vd_asfloat(v4.xyz), vd_asfloat(r4.xyz)));
    r5.xyz = vd_asuint(((-(vd_asfloat(r4.xyz))) * vd_asfloat(r0.yyy) + vd_asfloat(v4.xyz)));
    r0.y = vd_asuint(dot(vd_asfloat(r5.xyz), vd_asfloat(r5.xyz)));
    r0.y = vd_asuint(rsqrt(vd_asfloat(r0.y)));
    r5.xyz = vd_asuint((vd_asfloat(r0.yyy) * vd_asfloat(r5.xyz)));
    r0.y = vd_asuint(dot(vd_asfloat(v5.xyz), vd_asfloat(r4.xyz)));
    r6.xyz = vd_asuint(((-(vd_asfloat(r4.xyz))) * vd_asfloat(r0.yyy) + vd_asfloat(v5.xyz)));
    r0.y = vd_asuint(dot(vd_asfloat(r6.xyz), vd_asfloat(r6.xyz)));
    r0.y = vd_asuint(rsqrt(vd_asfloat(r0.y)));
    r6.xyz = vd_asuint((vd_asfloat(r0.yyy) * vd_asfloat(r6.xyz)));
    r6.xyz = vd_asuint((vd_asfloat(r2.zzz) * vd_asfloat(r6.xyz)));
    r5.xyz = vd_asuint((vd_asfloat(r2.xxx) * vd_asfloat(r5.xyz) + vd_asfloat(r6.xyz)));
    r4.xyz = vd_asuint((vd_asfloat(r2.www) * vd_asfloat(r4.xyz) + vd_asfloat(r5.xyz)));
  } else {
    r5.xyz = vd_asuint((vd_asfloat(r2.zzz) * vd_asfloat(v5.xyz)));
    r5.xyz = vd_asuint((vd_asfloat(r2.xxx) * vd_asfloat(v4.xyz) + vd_asfloat(r5.xyz)));
    r4.xyz = vd_asuint((vd_asfloat(r2.www) * vd_asfloat(v3.xyz) + vd_asfloat(r5.xyz)));
  }
  r0.y = vd_asuint(dot(vd_asfloat(r4.xyz), vd_asfloat(r4.xyz)));
  r0.y = vd_asuint(rsqrt(vd_asfloat(r0.y)));
  r2.xzw = vd_asuint((vd_asfloat(r0.yyy) * vd_asfloat(r4.xyz)));
  r0.y = vd_asuint((-(int)(0.5 < vd_asfloat(v1.z))));
  r0.z = vd_asuint((-(int)(vd_asfloat(v1.z) < 1.5)));
  r0.y = (r0.z & r0.y);
  r0.z = vd_asuint((-(int)(0.125 >= vd_asfloat(v11.y))));
  r0.y = (r0.z & r0.y);
  r4.xyz = ((r0.yyy != 0u) ? vd_asuint(v11.xyz) : vd_asuint(v2.xyz));
  r5.xyz = ((r0.yyy != 0u) ? uint3(0x00000000u, 0x3f800000u, 0x00000000u) : r2.xzw);
  r6.xyz = vd_asuint((-(int3)(vd_asint((_BackgroundType).xxx) == int3(0, 1, 11))));
  r0.z = (r6.y | r6.x);
  r0.z = (r6.z | r0.z);
  r3.w = vd_asuint((-(int)(0.0 >= vd_asfloat(r4.y))));
  r4.w = vd_asuint((-(int)(vd_asfloat(r5.y) >= 0.850000024)));
  r0.y = (r0.y | r4.w);
  r2.xzw = ((r0.yyy != 0u) ? uint3(0x00000000u, 0x3f800000u, 0x00000000u) : r2.xzw);
  r2.xzw = ((r3.www != 0u) ? r2.xzw : r5.xyz);
  r2.xzw = ((r0.zzz != 0u) ? r2.xzw : r5.xyz);
  r0.yz = vd_asuint((float2(1.0, 1.0) / vd_asfloat(_ScaledScreenParams.xy)));
  r5.xy = vd_asuint((vd_asfloat(r0.yz) * vd_asfloat(v7.xy)));
#if UNITY_UV_STARTS_AT_TOP
  r0.y = vd_asuint((vd_asfloat(r5.y) * vd_asfloat(_ScaleBiasRt.x) + vd_asfloat(_ScaleBiasRt.y)));
  r5.z = vd_asuint(((-(vd_asfloat(r0.y))) + 1.0));
#else
  r0.y = r5.y;
  r5.z = r0.y;
#endif
  r0.yz = vd_asuint(((-(vd_asfloat(_GlobalOverallMapBoundMin.xz))) + vd_asfloat(_GlobalOverallMapBoundMax.xz)));
  r0.yz = vd_asuint(max(vd_asfloat(r0.yz), float2(1.0, 1.0)));
  r5.yw = vd_asuint((vd_asfloat(v2.xz) + (-(vd_asfloat(_GlobalOverallMapBoundMin.xz)))));
  r5.yw = vd_asuint((vd_asfloat(r5.yw) / vd_asfloat(r0.yz)));
  r6.xyzw = vd_asuint(_GlobalWaterAreaMap.SampleBias(sampler_vd_linear_clamp, vd_asfloat(r5.yw), vd_asfloat(_GlobalMipBias.x)).xyzw);
  r3.w = vd_asuint((-(int)(vd_asfloat(_WeatherIntensity) >= 0.00100000005)));
  r4.w = vd_asuint((vd_asfloat(r6.x) + 0.899999976));
  r4.w = vd_asuint(saturate((vd_asfloat(r4.w) * 0.5)));
  r6.x = vd_asuint((vd_asfloat(r4.w) * -2.0 + 3.0));
  r4.w = vd_asuint((vd_asfloat(r4.w) * vd_asfloat(r4.w)));
  r4.w = vd_asuint((vd_asfloat(r4.w) * vd_asfloat(r6.x)));
  r4.w = vd_asuint(max(vd_asfloat(r4.w), vd_asfloat(_WeatherIndoorMask)));
  r4.w = vd_asuint(((-(vd_asfloat(r4.w))) + 1.0));
  r4.w = vd_asuint((vd_asfloat(r4.w) * vd_asfloat(_WeatherIntensity)));
  r6.x = vd_asuint((-(int)(vd_asfloat(r4.w) >= 0.00100000005)));
  r6.y = vd_asuint((vd_asfloat(r4.w) * vd_asfloat(_WeatherWetSurfaceIntensity)));
  r6.y = vd_asuint((vd_asfloat(r6.y) * -0.300000012 + 1.0));
  r7.xyz = vd_asuint((vd_asfloat(r1.xyz) * vd_asfloat(r6.yyy)));
  r4.w = vd_asuint((vd_asfloat(r4.w) * vd_asfloat(_WeatherBgTintIntensity)));
  r6.z = vd_asuint((-(int)(0.00100000005 < vd_asfloat(r4.w))));
  r6.w = vd_asuint(dot(vd_asfloat(r7.xyz), float3(0.298999995, 0.587000012, 0.114)));
  r7.w = vd_asuint((vd_asfloat(r4.w) * vd_asfloat(_WeatherBgDesaturation)));
  r8.xyz = vd_asuint(((-(vd_asfloat(r1.xyz))) * vd_asfloat(r6.yyy) + vd_asfloat(r6.www)));
  r8.xyz = vd_asuint((vd_asfloat(r7.www) * vd_asfloat(r8.xyz) + vd_asfloat(r7.xyz)));
  r4.w = vd_asuint((vd_asfloat(r4.w) * 0.300000012));
  r9.xyz = vd_asuint(((-(vd_asfloat(r8.xyz))) + vd_asfloat(_WeatherBgTintColor.xyz)));
  r8.xyz = vd_asuint((vd_asfloat(r4.www) * vd_asfloat(r9.xyz) + vd_asfloat(r8.xyz)));
  r6.yzw = ((r6.zzz != 0u) ? r8.xyz : r7.xyz);
  r6.xyz = ((r6.xxx != 0u) ? r6.yzw : r1.xyz);
  r6.xyz = ((r3.www != 0u) ? r6.xyz : r1.xyz);
  if (vd_asuint(_GlobalWaterAreaEnable) != 0u) {
    r7.xyzw = vd_asuint(_GlobalWaterAreaMap.SampleLevel(sampler_vd_linear_clamp, vd_asfloat(r5.yw), 0.0).xyzw);
    r3.w = vd_asuint((-(int)(0.100000001 < vd_asfloat(r7.z))));
    if (r3.w != 0u) {
      r5.yw = vd_asuint((vd_asfloat(v2.xz) * vd_asfloat(_GlobalWaterDepthRange.ww)));
      r8.xyzw = vd_asuint(_GlobalWaterNoiseTex.SampleLevel(sampler_vd_linear_repeat, vd_asfloat(r5.yw), 0.0).xyzw);
      r3.w = vd_asuint((vd_asfloat(r8.y) + -0.5));
      r3.w = vd_asuint((vd_asfloat(r3.w) * vd_asfloat(_GlobalWaterDepthRange.z)));
      r4.w = vd_asuint((vd_asfloat(r7.z) * vd_asfloat(r3.w)));
      r8.xy = vd_asuint((vd_asfloat(_GlobalWaterWaveParams.yx) * float2(0.800000012, 0.600000024)));
      r8.z = 0xbe99999au;
      r5.yw = vd_asuint((vd_asfloat(r8.yz) * vd_asfloat(_Time.yy)));
      r7.x = 0x3ecccccdu;
      r7.y = vd_asuint((vd_asfloat(_GlobalWaterWaveParams.x) * 0.600000024));
      r5.yw = vd_asuint((vd_asfloat(r5.yw) * vd_asfloat(r7.xy)));
      r5.yw = vd_asuint((vd_asfloat(v2.xz) * vd_asfloat(r8.xx) + vd_asfloat(r5.yw)));
      r8.xyzw = vd_asuint(_GlobalWaterNoiseTex.SampleLevel(sampler_vd_linear_repeat, vd_asfloat(r5.yw), 0.0).xyzw);
      r5.y = vd_asuint((vd_asfloat(r7.z) + -0.5));
      r5.y = vd_asuint((abs(vd_asfloat(r5.y)) * 5.0));
      r5.y = vd_asuint(min(vd_asfloat(r5.y), 1.0));
      r5.y = vd_asuint(((-(vd_asfloat(r5.y))) + 1.0));
      r5.w = vd_asuint((vd_asfloat(r8.y) + -0.5));
      r4.w = vd_asuint((vd_asfloat(r5.w) * vd_asfloat(r5.y) + vd_asfloat(r4.w)));
      r4.w = vd_asuint((vd_asfloat(r4.w) + vd_asfloat(r7.z)));
      r5.y = vd_asuint(((-(vd_asfloat(_GlobalWaterShoreEdge.x))) + vd_asfloat(_GlobalWaterShoreEdge.y)));
      r5.w = vd_asuint((vd_asfloat(r4.w) + (-(vd_asfloat(_GlobalWaterShoreEdge.x)))));
      r5.y = vd_asuint((1.0 / vd_asfloat(r5.y)));
      r5.y = vd_asuint(saturate((vd_asfloat(r5.y) * vd_asfloat(r5.w))));
      r5.w = vd_asuint((vd_asfloat(r5.y) * -2.0 + 3.0));
      r5.y = vd_asuint((vd_asfloat(r5.y) * vd_asfloat(r5.y)));
      r5.y = vd_asuint((vd_asfloat(r5.y) * vd_asfloat(r5.w)));
      r7.xy = vd_asuint((vd_asfloat(_GlobalWaterShoreEdge.xy) + vd_asfloat((_GlobalWaterShoreStroke).xx)));
      r5.w = vd_asuint(((-(vd_asfloat(r7.x))) + vd_asfloat(r7.y)));
      r6.w = vd_asuint((vd_asfloat(r4.w) + (-(vd_asfloat(r7.x)))));
      r5.w = vd_asuint((1.0 / vd_asfloat(r5.w)));
      r5.w = vd_asuint(saturate((vd_asfloat(r5.w) * vd_asfloat(r6.w))));
      r6.w = vd_asuint((vd_asfloat(r5.w) * -2.0 + 3.0));
      r5.w = vd_asuint((vd_asfloat(r5.w) * vd_asfloat(r5.w)));
      r5.w = vd_asuint(((-(vd_asfloat(r6.w))) * vd_asfloat(r5.w) + 1.0));
      r3.w = vd_asuint((vd_asfloat(r3.w) * vd_asfloat(r7.z) + vd_asfloat(r7.z)));
      r6.w = vd_asuint(((-(vd_asfloat(_GlobalWaterDepthRange.x))) + vd_asfloat(_GlobalWaterDepthRange.y)));
      r3.w = vd_asuint((vd_asfloat(r3.w) + (-(vd_asfloat(_GlobalWaterDepthRange.x)))));
      r6.w = vd_asuint((1.0 / vd_asfloat(r6.w)));
      r3.w = vd_asuint(saturate((vd_asfloat(r3.w) * vd_asfloat(r6.w))));
      r7.x = vd_asuint((vd_asfloat(r3.w) * -2.0 + 3.0));
      r3.w = vd_asuint((vd_asfloat(r3.w) * vd_asfloat(r3.w)));
      r7.y = vd_asuint((vd_asfloat(r3.w) * vd_asfloat(r7.x)));
      r5.w = vd_asuint((vd_asfloat(r5.w) * vd_asfloat(r5.y)));
      r7.z = vd_asuint((-(int)(vd_asfloat(r5.y) >= 0.00999999978)));
      if (r7.z != 0u) {
        r7.zw = vd_asuint((-(int2)(float2(0.00999999978, 0.00999999978) < vd_asfloat(_GlobalWaterSurfaceWaveMapParams.zw))));
        r7.z = (r7.w & r7.z);
        if (r7.z != 0u) {
          r7.zw = vd_asuint((vd_asfloat(v2.xz) + (-(vd_asfloat(_GlobalWaterSurfaceWaveMapParams.xy)))));
          r7.zw = vd_asuint((vd_asfloat(r7.zw) / vd_asfloat(_GlobalWaterSurfaceWaveMapParams.zw)));
          r7.zw = vd_asuint((vd_asfloat(r7.zw) + float2(0.5, 0.5)));
          r8.xy = vd_asuint(((-(vd_asfloat(r7.zw))) + float2(1.0, 1.0)));
          r8.xy = vd_asuint(min(vd_asfloat(r7.zw), vd_asfloat(r8.xy)));
          r8.x = vd_asuint(min(vd_asfloat(r8.y), vd_asfloat(r8.x)));
          r8.x = vd_asuint(saturate((vd_asfloat(r8.x) * vd_asfloat(_GlobalWaterSurfaceWaveMapParams.z))));
          r9.xyzw = vd_asuint(_GlobalWaterSurfaceWaveMap.SampleLevel(sampler_vd_linear_clamp, vd_asfloat(r7.zw), 0.0).xyzw);
          r7.zw = vd_asuint((vd_asfloat(r9.yz) * float2(2.0, 2.0) + float2(-1.0, -1.0)));
          r8.xy = vd_asuint((vd_asfloat(r8.xx) * vd_asfloat(r9.wx)));
        } else {
          r7.zw = uint2(0x00000000u, 0x00000000u);
          r8.xy = uint2(0x00000000u, 0x00000000u);
        }
        r3.w = vd_asuint(((-(vd_asfloat(r7.x))) * vd_asfloat(r3.w) + 1.0));
        r3.w = vd_asuint(((-(vd_asfloat(_GlobalWaterShoreHeightDrop))) * vd_asfloat(r3.w) + vd_asfloat(_GlobalWaterSurfaceLevel)));
        r3.w = vd_asuint((vd_asfloat(r8.y) * 0.5 + vd_asfloat(r3.w)));
        r7.x = vd_asuint((vd_asfloat(v2.y) + (-(vd_asfloat(_GlobalWaterDepthOffset)))));
        r3.w = vd_asuint(((-(vd_asfloat(r3.w))) + vd_asfloat(r7.x)));
        r7.x = vd_asuint(saturate((vd_asfloat(r3.w) * -2.0 + 0.5)));
        r8.zw = vd_asuint((vd_asfloat(v2.xz) * vd_asfloat(_GlobalWaterWaveParams.yy)));
        r9.x = vd_asuint((vd_asfloat(_Time.y) * vd_asfloat(_GlobalWaterWaveParams.x)));
        r9.xyz = vd_asuint((vd_asfloat(r9.xxx) * float3(0.707099974, -0.780000031, 0.519999981)));
        r9.xw = vd_asuint((vd_asfloat(v2.xz) * vd_asfloat(_GlobalWaterWaveParams.yy) + vd_asfloat(r9.xx)));
        r10.xyzw = vd_asuint(_GlobalWaterNoiseTex.SampleLevel(sampler_vd_linear_repeat, vd_asfloat(r9.xw), 0.0).xyzw);
        r11.xw = vd_asuint(_GlobalWaterWaveParams.xx);
        r11.yz = uint2(0x3f333333u, 0x3e99999au);
        r9.xw = vd_asuint((vd_asfloat(r11.xy) * vd_asfloat(_Time.yy)));
        r9.xw = vd_asuint((vd_asfloat(r11.zw) * vd_asfloat(r9.xw)));
        r9.xw = vd_asuint((vd_asfloat(r8.zw) * float2(1.5, 1.5) + vd_asfloat(r9.xw)));
        r9.yz = vd_asuint((vd_asfloat(r8.zw) * float2(2.0, 2.0) + vd_asfloat(r9.yz)));
        r11.xyzw = vd_asuint(_GlobalWaterNormalMap.SampleLevel(sampler_vd_linear_repeat, vd_asfloat(r9.xw), 0.0).xyzw);
        r10.yzw = vd_asuint((vd_asfloat(r11.xyz) * float3(2.0, 2.0, 2.0) + float3(-1.0, -1.0, -1.0)));
        r9.xyzw = vd_asuint(_GlobalWaterNormalMap.SampleLevel(sampler_vd_linear_repeat, vd_asfloat(r9.yz), 0.0).xyzw);
        r9.xyz = vd_asuint((vd_asfloat(r9.xyz) * float3(2.0, 2.0, 2.0) + float3(-1.0, -1.0, -1.0)));
        r11.xy = vd_asuint((vd_asfloat(r9.xy) + vd_asfloat(r10.yz)));
        r11.z = vd_asuint((vd_asfloat(r9.z) * vd_asfloat(r10.w)));
        r9.x = vd_asuint(dot(vd_asfloat(r11.xyz), vd_asfloat(r11.xyz)));
        r9.x = vd_asuint(rsqrt(vd_asfloat(r9.x)));
        r9.xyz = vd_asuint((vd_asfloat(r9.xxx) * vd_asfloat(r11.xyz)));
        r9.x = vd_asuint(saturate(dot(vd_asfloat(r9.xzy), float3(0.300000012, 0.300000012, 0.899999976))));
        r9.x = vd_asuint((vd_asfloat(r7.y) * vd_asfloat(r9.x)));
        r9.x = vd_asuint((vd_asfloat(r9.x) * 0.150000006));
        r7.zw = vd_asuint((vd_asfloat(r7.zw) * vd_asfloat(r8.xx)));
        r8.x = vd_asuint(log2(abs(vd_asfloat(r10.x))));
        r8.x = vd_asuint((vd_asfloat(r8.x) * vd_asfloat(_GlobalWaterWaveParams.z)));
        r8.x = vd_asuint(exp2(vd_asfloat(r8.x)));
        r8.x = vd_asuint(min(vd_asfloat(r8.x), 1.0));
        r8.x = vd_asuint((vd_asfloat(r8.x) * vd_asfloat(_GlobalWaterWaveParams.w)));
        r5.w = vd_asuint((vd_asfloat(r5.w) * vd_asfloat(_GlobalWaterShoreIntensity)));
        r5.w = vd_asuint(max(vd_asfloat(r8.x), vd_asfloat(r5.w)));
        r5.w = vd_asuint((vd_asfloat(r5.w) * vd_asfloat(_GlobalWaterShallowColor.w)));
        r9.yzw = vd_asuint((vd_asfloat(_GlobalWaterShallowColor.xyz) + (-(vd_asfloat(_GlobalWaterDeepColor.xyz)))));
        r9.yzw = vd_asuint((vd_asfloat(r5.www) * vd_asfloat(r9.yzw) + vd_asfloat(_GlobalWaterDeepColor.xyz)));
        r10.yz = vd_asuint((vd_asfloat(r7.zw) * float2(0.5, 0.5) + vd_asfloat(v2.xz)));
        r10.yz = vd_asuint((vd_asfloat(r10.yz) + (-(vd_asfloat(_GlobalOverallMapBoundMin.xz)))));
        r0.yz = vd_asuint((vd_asfloat(r10.yz) / vd_asfloat(r0.yz)));
        r11.xyzw = vd_asuint(_GlobalWaterAreaMap.SampleLevel(sampler_vd_linear_clamp, vd_asfloat(r0.yz), 0.0).xyzw);
        r0.y = vd_asuint((vd_asfloat(r11.z) + (-(vd_asfloat(_GlobalWaterDepthRange.x)))));
        r0.y = vd_asuint(saturate((vd_asfloat(r6.w) * vd_asfloat(r0.y))));
        r0.z = vd_asuint((vd_asfloat(r0.y) * -2.0 + 3.0));
        r0.y = vd_asuint((vd_asfloat(r0.y) * vd_asfloat(r0.y)));
        r0.y = vd_asuint((vd_asfloat(r0.z) * vd_asfloat(r0.y) + (-(vd_asfloat(r7.y)))));
        r0.y = vd_asuint((abs(vd_asfloat(r8.y)) * abs(vd_asfloat(r0.y))));
        r0.y = vd_asuint((vd_asfloat(r0.y) * 3.0));
        r0.y = vd_asuint(min(vd_asfloat(r0.y), 1.0));
        r10.yzw = vd_asuint(((-(vd_asfloat(r9.yzw))) + vd_asfloat(_GlobalWaterShallowColor.xyz)));
        r9.yzw = vd_asuint((vd_asfloat(r0.yyy) * vd_asfloat(r10.yzw) + vd_asfloat(r9.yzw)));
        r10.yzw = vd_asuint(((-(vd_asfloat(r9.yzw))) + vd_asfloat(_GlobalWaterShallowColor.xyz)));
        r9.xyz = vd_asuint((vd_asfloat(r9.xxx) * vd_asfloat(r10.yzw) + vd_asfloat(r9.yzw)));
        r0.y = vd_asuint((-(int)(vd_asfloat(r3.w) >= 0.0)));
        r0.y = (r0.y & 0x3f800000u);
        r0.z = vd_asuint((1.0 / vd_asfloat(_GlobalWaterSurfaceEdgeWidth)));
        r0.z = vd_asuint(saturate((vd_asfloat(r0.z) * vd_asfloat(r3.w))));
        r3.w = vd_asuint((vd_asfloat(r0.z) * -2.0 + 3.0));
        r0.z = vd_asuint((vd_asfloat(r0.z) * vd_asfloat(r0.z)));
        r0.z = vd_asuint(((-(vd_asfloat(r3.w))) * vd_asfloat(r0.z) + 1.0));
        r3.w = vd_asuint((vd_asfloat(r0.y) * vd_asfloat(_GlobalWaterShallowColor.w)));
        r3.w = vd_asuint((vd_asfloat(r5.y) * vd_asfloat(r3.w)));
        r3.w = vd_asuint((vd_asfloat(r3.w) * 0.5));
        r10.yzw = vd_asuint(((-(vd_asfloat(r9.xyz))) + vd_asfloat(_GlobalWaterShallowColor.xyz)));
        r9.xyz = vd_asuint((vd_asfloat(r3.www) * vd_asfloat(r10.yzw) + vd_asfloat(r9.xyz)));
        r3.w = vd_asuint((-(int)(vd_asint(_PlaneReflectionActive) != 0)));
        r5.w = vd_asuint((-(int)(0.00999999978 < vd_asfloat(_GlobalWaterReflectionIntensity))));
        r3.w = (r3.w & r5.w);
        if (r3.w != 0u) {
          r10.y = 0xbf000000u;
          r10.z = vd_asuint(_GlobalWaterWaveParams.x);
          r11.yz = vd_asuint((vd_asfloat(r10.yz) * vd_asfloat(_Time.yy)));
          r11.x = vd_asuint((vd_asfloat(r11.y) * vd_asfloat(_GlobalWaterWaveParams.x)));
          r8.xy = vd_asuint((vd_asfloat(r11.xz) * float2(0.800000012, 0.692799985)));
          r8.xy = vd_asuint((vd_asfloat(r8.zw) * float2(0.699999988, 0.699999988) + vd_asfloat(r8.xy)));
          r8.xyzw = vd_asuint(_GlobalWaterNoiseTex.SampleLevel(sampler_vd_linear_repeat, vd_asfloat(r8.xy), 0.0).yxzw);
          r8.x = r10.x;
          r8.xy = vd_asuint((vd_asfloat(r8.xy) + float2(-0.5, -0.5)));
          r8.xy = vd_asuint((vd_asfloat(r8.xy) + vd_asfloat(r8.xy)));
          r8.xy = vd_asuint((vd_asfloat(r8.xy) * vd_asfloat(_GlobalWaterWaveParams2.xx)));
          r8.zw = vd_asuint((vd_asfloat(v7.xy) / vd_asfloat(_ScreenParams.xy)));
          r8.xy = vd_asuint((vd_asfloat(r8.xy) * float2(0.5, 0.5) + vd_asfloat(r8.zw)));
          r7.zw = vd_asuint((vd_asfloat(r7.zw) * float2(0.0199999996, 0.0199999996) + vd_asfloat(r8.xy)));
          r7.w = vd_asuint((vd_asfloat(r7.w) + vd_asfloat(_GlobalWaterReflectionYOffset)));
          r8.xyzw = vd_asuint(_PlaneReflectionTexture.SampleLevel(sampler_vd_linear_clamp, vd_asfloat(r7.zw), 0.0).xyzw);
          r3.w = vd_asuint((vd_asfloat(r7.y) * 0.699999988 + 0.300000012));
          r3.w = vd_asuint((vd_asfloat(r5.y) * vd_asfloat(r3.w)));
          r3.w = vd_asuint((vd_asfloat(r3.w) * vd_asfloat(_GlobalWaterReflectionIntensity)));
          r3.w = vd_asuint((vd_asfloat(r3.w) * vd_asfloat(_PlaneReflectionOpacity)));
          r7.yzw = vd_asuint((vd_asfloat(r3.www) * vd_asfloat(r8.xyz)));
          r3.w = vd_asuint(((-(vd_asfloat(r0.z))) * vd_asfloat(r0.y) + 1.0));
          r9.xyz = vd_asuint((vd_asfloat(r7.yzw) * vd_asfloat(r3.www) + vd_asfloat(r9.xyz)));
        }
        r0.y = vd_asuint(saturate((vd_asfloat(r0.z) * vd_asfloat(r0.y) + vd_asfloat(r7.x))));
        r3.w = vd_asuint((vd_asfloat(r5.y) * vd_asfloat(_GlobalWaterDeepColor.w)));
        r4.w = vd_asuint(rsqrt(vd_asfloat(r4.w)));
        r4.w = vd_asuint((1.0 / vd_asfloat(r4.w)));
        r3.w = vd_asuint((vd_asfloat(r3.w) * vd_asfloat(r4.w)));
        r0.y = vd_asuint((vd_asfloat(r0.y) * vd_asfloat(r3.w)));
        r0.y = vd_asuint((vd_asfloat(r0.z) * vd_asfloat(r0.y)));
        r7.xyz = vd_asuint(((-(vd_asfloat(r6.xyz))) + vd_asfloat(r9.xyz)));
        r6.xyz = vd_asuint((vd_asfloat(r0.yyy) * vd_asfloat(r7.xyz) + vd_asfloat(r6.xyz)));
      }
    }
  }
  r0.y = (vd_asuint(_MainLightLayerMask) & vd_asuint(unity_RenderingLayer.x));
  r0.z = vd_asuint(dot(vd_asfloat(r2.xzw), vd_asfloat(_MainLightPosition.xyz)));
  r0.z = vd_asuint(saturate((vd_asfloat(r0.z) * 0.5 + 0.5)));
  r7.xyz = vd_asuint((vd_asfloat(_MainLightColor.xyz) + (-(vd_asfloat(_GlobalBackgroundShadowColor.xyz)))));
  r7.xyz = vd_asuint((vd_asfloat(r7.xyz) * vd_asfloat(r0.zzz) + vd_asfloat(_GlobalBackgroundShadowColor.xyz)));
  r7.xyz = vd_asuint((vd_asfloat(r6.xyz) * vd_asfloat(r7.xyz)));
  r7.xyz = ((r0.yyy != 0u) ? r7.xyz : uint3(0x00000000u, 0x00000000u, 0x00000000u));
  r0.yz = vd_asuint((vd_asfloat(r4.xz) + (-(vd_asfloat(_PlaneBoundMaxPos.xz)))));
  r5.yw = vd_asuint((vd_asfloat(_PlaneBoundMinPos.xz) + (-(vd_asfloat(_PlaneBoundMaxPos.xz)))));
  r0.yz = vd_asuint(saturate((vd_asfloat(r0.yz) / vd_asfloat(r5.yw))));
  r8.xyzw = vd_asuint(_PlaneSightRenderTarget.SampleLevel(sampler_PlaneSightRenderTarget, vd_asfloat(r0.yz), 0.0).xyzw);
  r0.y = vd_asuint((vd_asfloat(r8.w) + -1.0));
  r0.y = vd_asuint((vd_asfloat(r0.y) * vd_asfloat(_GlobalPlaneSystemEnable)));
  r5.yw = vd_asuint((vd_asfloat(r5.xz) * vd_asfloat(_FPParams1.xy)));
  r5.yw = ((uint2)vd_asfloat(r5.yw));
  r8.xy = ((uint2)vd_asfloat(_FPParams1.zw));
  r0.z = vd_asuint((vd_asint(r5.w) * vd_asint(r8.x) + vd_asint(r5.y)));
  r0.z = vd_asuint((vd_asint(r8.y) * vd_asint(r0.z)));
  r9.x = vd_asuint((-(vd_asfloat((VD_COL(unity_MatrixV, 0).z)))));
  r9.y = vd_asuint((-(vd_asfloat((VD_COL(unity_MatrixV, 1).z)))));
  r9.z = vd_asuint((-(vd_asfloat((VD_COL(unity_MatrixV, 2).z)))));
  r8.xzw = vd_asuint((vd_asfloat(r4.xyz) + (-(vd_asfloat(_WorldSpaceCameraPos.xyz)))));
  r3.w = vd_asuint(dot(vd_asfloat(r9.xyz), vd_asfloat(r8.xzw)));
  r4.w = vd_asuint((-(int)(vd_asfloat(unity_OrthoParams.w) == 0.0)));
  r5.y = vd_asuint(log2(vd_asfloat(r3.w)));
  r3.w = ((r4.w != 0u) ? r5.y : r3.w);
  r3.w = vd_asuint((vd_asfloat(r3.w) * vd_asfloat(_FPParams0.x) + vd_asfloat(_FPParams0.y)));
  r3.w = ((uint)vd_asfloat(r3.w));
  r4.w = ((uint)vd_asfloat(_FPParams2.x));
  r4.w = vd_asuint((vd_asint(r4.w) + -1));
  r5.y = vd_asuint((vd_asint(r8.y) + 2));
  r3.w = min(r3.w, r4.w);
  r4.w = vd_asuint((vd_asint(r5.y) * vd_asint(r3.w)));
  r3.w = vd_asuint((vd_asint(r3.w) * vd_asint(r5.y) + 2));
  r5.y = (r4.w >> (uint)2);
  r8.x = (r4.w << (uint)31);
  r8.y = (r4.w << (uint)30);
  r8.xy = vd_asuint((vd_asint(r8.xy) >> 31));
  r8.zw = (r8.xx & vd_asuint(urp_ZBins[(int)(r5.y)].wy));
  r9.xy = (~r8.xy);
  r5.yw = (r9.xx & vd_asuint(urp_ZBins[(int)(r5.y)].zx));
  r5.yw = (r5.yw | r8.zw);
  r4.w = (r8.y & r5.y);
  r5.y = (r9.y & r5.w);
  r4.w = (r4.w | r5.y);
  r5.y = ((uint)vd_asfloat(_FPParams0.w));
  r8.x = r0.z;
  r8.y = r3.w;
  r8.z = 0x00000000u;
  r8.w = r4.w;
  r9.xyz = uint3(0x00000000u, 0x00000000u, 0x00000000u);
  while (true) {
    r5.w = (r8.w >> (uint)16);
    r6.w = r8.z;
    r7.w = r8.w;
    while (true) {
      r9.w = vd_asuint((-(int)(vd_asint(r6.w) == 0)));
      r10.x = (r7.w & 0x0000ffffu);
      r10.x = vd_asuint((-(int)(r5.w >= r10.x)));
      r9.w = (r9.w & r10.x);
      if (r9.w == 0u) break;
      r9.w = (r7.w >> (uint)5);
      r9.w = (r9.w & 0x000007ffu);
      r10.xyzw = vd_asuint((vd_asint(r8.xxyy) + vd_asint(r9.wwww)));
      r11.xy = (r10.yw >> (uint2)2);
      r12.xz = (r10.xz << (uint2)31);
      r12.yw = (r10.yw << (uint2)30);
      r10.xyzw = vd_asuint((vd_asint(r12.xyzw) >> 31));
      r11.zw = (r10.xx & vd_asuint(urp_Tiles[(int)(r11.x)].wy));
      r12.xyzw = (~r10.xyzw);
      r13.xy = (r12.xx & vd_asuint(urp_Tiles[(int)(r11.x)].zx));
      r11.xz = (r11.zw | r13.xy);
      r10.x = (r10.y & r11.x);
      r10.y = (r12.y & r11.z);
      r10.x = (r10.y | r10.x);
      r10.yz = (r10.zz & vd_asuint(urp_ZBins[(int)(r11.y)].wy));
      r11.xy = (r12.zz & vd_asuint(urp_ZBins[(int)(r11.y)].zx));
      r10.yz = (r10.yz | r11.xy);
      r10.y = (r10.w & r10.y);
      r10.z = (r12.w & r10.z);
      r10.y = (r10.z | r10.y);
      r10.x = (r10.y & r10.x);
      r10.y = (0xffffffffu << (uint)(int)(r7.w & 31u));
      r10.x = (r10.y & r10.x);
      r9.w = (r9.w << (uint)5);
      r9.w = vd_asuint((vd_asint(r5.w) + (-(vd_asint(r9.w)))));
      r9.w = min(r9.w, 0x0000001fu);
      r9.w = vd_asuint(((-(vd_asint(r9.w))) + 31));
      r9.w = (0xffffffffu >> (uint)(int)(r9.w & 31u));
      r6.w = (r9.w & r10.x);
      r9.w = vd_asuint((vd_asint(r7.w) + 32));
      r7.w = (r9.w & 0xffffffe0u);
    }
    r8.w = r7.w;
    if (r6.w == 0u) break;
    r5.w = vd_asuint((-vd_asint(r6.w)));
    r5.w = (r5.w & r6.w);
    r5.w = vd_asuint(((float)r5.w));
    r5.w = (r5.w >> (uint)23);
    r5.w = vd_asuint((vd_asint(r5.w) + -127));
    r9.w = (0x00000001u << (uint)(int)(r5.w & 31u));
    r8.z = (r6.w ^ r9.w);
    r9.w = vd_asuint((vd_asint(r8.w) + -32));
    r9.w = (r9.w & 0x0000ffe0u);
    r5.w = vd_asuint((vd_asint(r5.w) + vd_asint(r9.w)));
    r5.w = vd_asuint((vd_asint(r5.y) + vd_asint(r5.w)));
    r10.xyz = vd_asuint(((-(vd_asfloat(r4.xyz))) * vd_asfloat(_AdditionalLightsPosition[(int)(r5.w)].www) + vd_asfloat(_AdditionalLightsPosition[(int)(r5.w)].xyz)));
    r9.w = vd_asuint(dot(vd_asfloat(r10.xyz), vd_asfloat(r10.xyz)));
    r9.w = vd_asuint(max(vd_asfloat(r9.w), 6.10351562e-05));
    r10.w = vd_asuint(rsqrt(vd_asfloat(r9.w)));
    r10.xyz = vd_asuint((vd_asfloat(r10.www) * vd_asfloat(r10.xyz)));
    r10.w = vd_asuint((1.0 / vd_asfloat(r9.w)));
    r9.w = vd_asuint((vd_asfloat(r9.w) * vd_asfloat(_AdditionalLightsAttenuation[(int)(r5.w)].x)));
    r9.w = vd_asuint(((-(vd_asfloat(r9.w))) * vd_asfloat(r9.w) + 1.0));
    r9.w = vd_asuint(max(vd_asfloat(r9.w), 0.0));
    r9.w = vd_asuint((vd_asfloat(r9.w) * vd_asfloat(r9.w)));
    r9.w = vd_asuint((vd_asfloat(r9.w) * vd_asfloat(r10.w)));
    r10.w = vd_asuint(dot(vd_asfloat(_AdditionalLightsSpotDir[(int)(r5.w)].xyz), vd_asfloat(r10.xyz)));
    r10.w = vd_asuint(saturate((vd_asfloat(r10.w) * vd_asfloat(_AdditionalLightsAttenuation[(int)(r5.w)].z) + vd_asfloat(_AdditionalLightsAttenuation[(int)(r5.w)].w))));
    r10.w = vd_asuint((vd_asfloat(r10.w) * vd_asfloat(r10.w)));
    r9.w = vd_asuint((vd_asfloat(r9.w) * vd_asfloat(r10.w)));
    r10.w = vd_asuint((-(int)(2 == vd_asint(_AdditionalLightsLayerMasks[(int)(r5.w)]))));
    r10.w = (r10.w & 0x3f800000u);
    r10.w = vd_asuint((vd_asfloat(r10.w) * vd_asfloat(r0.y) + 1.0));
    r9.w = vd_asuint(sqrt(vd_asfloat(r9.w)));
    r9.w = vd_asuint(min(vd_asfloat(r9.w), 1.0));
    r11.xyz = vd_asuint((vd_asfloat(r9.www) * vd_asfloat(_AdditionalLightsColor[(int)(r5.w)].xyz)));
    r5.w = vd_asuint(saturate(dot(vd_asfloat(r2.xzw), vd_asfloat(r10.xyz))));
    r10.xyz = vd_asuint((vd_asfloat(r5.www) * vd_asfloat(r11.xyz)));
    r10.xyz = vd_asuint((vd_asfloat(r6.xyz) * vd_asfloat(r10.xyz)));
    r9.xyz = vd_asuint((vd_asfloat(r10.xyz) * vd_asfloat(r10.www) + vd_asfloat(r9.xyz)));
  }
  r2.xzw = vd_asuint((vd_asfloat(r7.xyz) + vd_asfloat(r9.xyz)));
  r1.xyz = vd_asuint((vd_asfloat(r1.www) * vd_asfloat(r1.xyz) + vd_asfloat(r2.xzw)));
  r4.x = vd_asuint(dot(float2(0.530324996, 0.530324996), vd_asfloat(v2.xz)));
  r4.y = vd_asuint(dot(float3(-0.353500009, 0.865999997, 0.353500009), vd_asfloat(v2.xyz)));
  r0.yz = vd_asuint((vd_asfloat(r4.xy) * float2(0.125, 0.125)));
  r6.xyzw = vd_asuint(_DirtNoiseTexture.SampleBias(sampler_vd_point_repeat, vd_asfloat(r0.yz), vd_asfloat(_GlobalMipBias.x)).xyzw);
  r0.y = vd_asuint((vd_asfloat(r6.x) * 0.5 + 0.5));
  r0.z = vd_asuint((vd_asfloat(r0.y) * vd_asfloat(r0.y)));
  r0.y = vd_asuint(((-(vd_asfloat(r0.y))) + 1.0));
  r0.y = vd_asuint((vd_asfloat(r0.y) * vd_asfloat(r0.y)));
  r2.xzw = vd_asuint((vd_asfloat(r1.xyz) * vd_asfloat(r1.xyz)));
  r2.xzw = vd_asuint(min(vd_asfloat(r2.xzw), float3(1.0, 1.0, 1.0)));
  r0.yz = vd_asuint((vd_asfloat(r0.yz) * vd_asfloat(_DirtNoiseColor.ww)));
  r0.yz = vd_asuint((vd_asfloat(r0.yz) * vd_asfloat((_UseDirtNoise).xx)));
  r2.xzw = vd_asuint(((-(vd_asfloat(r1.xyz))) + vd_asfloat(r2.xzw)));
  r1.xyz = vd_asuint((vd_asfloat(r0.zzz) * vd_asfloat(r2.xzw) + vd_asfloat(r1.xyz)));
  r2.xzw = vd_asuint(saturate((vd_asfloat(r1.xyz) + vd_asfloat(r1.xyz))));
  r2.xzw = vd_asuint(((-(vd_asfloat(r1.xyz))) + vd_asfloat(r2.xzw)));
  r1.xyz = vd_asuint((vd_asfloat(r0.yyy) * vd_asfloat(r2.xzw) + vd_asfloat(r1.xyz)));
  if (r0.x != 0u) {
    r0.xy = vd_asuint((vd_asfloat(r4.xy) * float2(0.400000006, 0.400000006)));
    r6.xyzw = vd_asuint(_DirtNoiseTexture.SampleBias(sampler_vd_point_repeat, vd_asfloat(r0.xy), vd_asfloat(_GlobalMipBias.x)).xyzw);
    r7.xyzw = vd_asuint((vd_asfloat(_Time.yyyy) * float4(1.5, 1.5, 0.800000012, 0.800000012) + vd_asfloat(v2.zxzx)));
    r0.xy = vd_asuint(sin(vd_asfloat(r7.xw)));
    r2.xz = vd_asuint(cos(vd_asfloat(r7.yz)));
    r7.zw = vd_asuint((vd_asfloat(r2.xz) * float2(0.0250000004, 0.0250000004)));
    r7.xy = vd_asuint((vd_asfloat(r0.xy) * float2(0.0250000004, 0.0250000004)));
    r0.xy = vd_asuint((vd_asfloat(r4.xy) * float2(0.150000006, 0.150000006) + vd_asfloat(r7.xz)));
    r8.xyzw = vd_asuint(_DirtNoiseTexture.SampleBias(sampler_vd_point_repeat, vd_asfloat(r0.xy), vd_asfloat(_GlobalMipBias.x)).xyzw);
    r0.xy = vd_asuint((vd_asfloat(r4.xy) * float2(0.1875, 0.1875) + vd_asfloat(r7.wy)));
    r4.xyzw = vd_asuint(_DirtNoiseTexture.SampleBias(sampler_vd_point_repeat, vd_asfloat(r0.xy), vd_asfloat(_GlobalMipBias.x)).xyzw);
    r0.x = vd_asuint((vd_asfloat(r4.x) + vd_asfloat(r8.x)));
    r0.x = vd_asuint(saturate((vd_asfloat(r0.x) * 0.5)));
    r0.x = vd_asuint(rsqrt(vd_asfloat(r0.x)));
    r0.x = vd_asuint((1.0 / vd_asfloat(r0.x)));
    r0.y = vd_asuint((-(int)(vd_asfloat(_GlobalBoxAreaIntensities[0]) >= 0.00100000005)));
    if (r0.y != 0u) {
      r0.y = vd_asuint(((int)vd_asfloat(_GlobalBoxAreaCenters[0].w)));
      r2.xz = vd_asuint((-(int2)(vd_asint(r0.yy) == int2(1, 4))));
      r0.z = (r2.z | r2.x);
      if (r0.z == 0u) {
        r2.xzw = vd_asuint((vd_asfloat(v2.xyz) + (-(vd_asfloat(_GlobalBoxAreaCenters[0].xyz)))));
        r2.xzw = vd_asuint(((-(abs(vd_asfloat(r2.xzw)))) + vd_asfloat(_GlobalBoxAreaExtents[0].xyz)));
        r0.z = vd_asuint(min(vd_asfloat(r2.w), vd_asfloat(r2.z)));
        r0.z = vd_asuint(min(vd_asfloat(r0.z), vd_asfloat(r2.x)));
        r1.w = vd_asuint(max(vd_asfloat(_GlobalBoxAreaExtents[0].w), 0.00100000005));
        r0.z = vd_asuint(saturate((vd_asfloat(r0.z) / vd_asfloat(r1.w))));
        r2.xzw = vd_asuint(log2(abs(vd_asfloat(_GlobalBoxAreaColors[0].xyz))));
        r2.xzw = vd_asuint((vd_asfloat(r2.xzw) * float3(2.20000005, 2.20000005, 2.20000005)));
        r2.xzw = vd_asuint(exp2(vd_asfloat(r2.xzw)));
        r0.y = vd_asuint((-(int)(vd_asint(r0.y) == 3)));
        if (r0.y != 0u) {
          r0.y = vd_asuint((vd_asfloat(r0.x) + -1.0));
          r0.y = vd_asuint((vd_asfloat(_GlobalBoxAreaShimmerParams[0].z) * vd_asfloat(r0.y) + 1.0));
        } else {
          r0.y = 0x3f800000u;
        }
        r1.w = vd_asuint((-(int)(vd_asfloat(_GlobalBoxAreaIntensities[0]) >= vd_asfloat(r6.x))));
        r1.w = (r1.w & 0x3f800000u);
        r0.z = vd_asuint((vd_asfloat(r0.z) * vd_asfloat(r1.w)));
        r0.z = vd_asuint((vd_asfloat(r0.z) * vd_asfloat(_GlobalBoxAreaColors[0].w)));
        r0.y = vd_asuint((vd_asfloat(r0.y) * vd_asfloat(r0.z)));
        r2.xzw = vd_asuint(((-(vd_asfloat(r1.xyz))) + vd_asfloat(r2.xzw)));
        r1.xyz = vd_asuint((vd_asfloat(r0.yyy) * vd_asfloat(r2.xzw) + vd_asfloat(r1.xyz)));
      }
    }
    r0.y = vd_asuint((-(int)(vd_asfloat(_GlobalBoxAreaIntensities[1]) >= 0.00100000005)));
    if (r0.y != 0u) {
      r0.y = vd_asuint(((int)vd_asfloat(_GlobalBoxAreaCenters[1].w)));
      r2.xz = vd_asuint((-(int2)(vd_asint(r0.yy) == int2(1, 4))));
      r0.z = (r2.z | r2.x);
      if (r0.z == 0u) {
        r2.xzw = vd_asuint((vd_asfloat(v2.xyz) + (-(vd_asfloat(_GlobalBoxAreaCenters[1].xyz)))));
        r2.xzw = vd_asuint(((-(abs(vd_asfloat(r2.xzw)))) + vd_asfloat(_GlobalBoxAreaExtents[1].xyz)));
        r0.z = vd_asuint(min(vd_asfloat(r2.w), vd_asfloat(r2.z)));
        r0.z = vd_asuint(min(vd_asfloat(r0.z), vd_asfloat(r2.x)));
        r1.w = vd_asuint(max(vd_asfloat(_GlobalBoxAreaExtents[1].w), 0.00100000005));
        r0.z = vd_asuint(saturate((vd_asfloat(r0.z) / vd_asfloat(r1.w))));
        r2.xzw = vd_asuint(log2(abs(vd_asfloat(_GlobalBoxAreaColors[1].xyz))));
        r2.xzw = vd_asuint((vd_asfloat(r2.xzw) * float3(2.20000005, 2.20000005, 2.20000005)));
        r2.xzw = vd_asuint(exp2(vd_asfloat(r2.xzw)));
        r0.y = vd_asuint((-(int)(vd_asint(r0.y) == 3)));
        if (r0.y != 0u) {
          r0.y = vd_asuint((vd_asfloat(r0.x) + -1.0));
          r0.y = vd_asuint((vd_asfloat(_GlobalBoxAreaShimmerParams[1].z) * vd_asfloat(r0.y) + 1.0));
        } else {
          r0.y = 0x3f800000u;
        }
        r1.w = vd_asuint((-(int)(vd_asfloat(_GlobalBoxAreaIntensities[1]) >= vd_asfloat(r6.x))));
        r1.w = (r1.w & 0x3f800000u);
        r0.z = vd_asuint((vd_asfloat(r0.z) * vd_asfloat(r1.w)));
        r0.z = vd_asuint((vd_asfloat(r0.z) * vd_asfloat(_GlobalBoxAreaColors[1].w)));
        r0.y = vd_asuint((vd_asfloat(r0.y) * vd_asfloat(r0.z)));
        r2.xzw = vd_asuint(((-(vd_asfloat(r1.xyz))) + vd_asfloat(r2.xzw)));
        r1.xyz = vd_asuint((vd_asfloat(r0.yyy) * vd_asfloat(r2.xzw) + vd_asfloat(r1.xyz)));
      }
    }
    r0.y = vd_asuint((-(int)(vd_asfloat(_GlobalBoxAreaIntensities[2]) >= 0.00100000005)));
    if (r0.y != 0u) {
      r0.y = vd_asuint(((int)vd_asfloat(_GlobalBoxAreaCenters[2].w)));
      r2.xz = vd_asuint((-(int2)(vd_asint(r0.yy) == int2(1, 4))));
      r0.z = (r2.z | r2.x);
      if (r0.z == 0u) {
        r2.xzw = vd_asuint((vd_asfloat(v2.xyz) + (-(vd_asfloat(_GlobalBoxAreaCenters[2].xyz)))));
        r2.xzw = vd_asuint(((-(abs(vd_asfloat(r2.xzw)))) + vd_asfloat(_GlobalBoxAreaExtents[2].xyz)));
        r0.z = vd_asuint(min(vd_asfloat(r2.w), vd_asfloat(r2.z)));
        r0.z = vd_asuint(min(vd_asfloat(r0.z), vd_asfloat(r2.x)));
        r1.w = vd_asuint(max(vd_asfloat(_GlobalBoxAreaExtents[2].w), 0.00100000005));
        r0.z = vd_asuint(saturate((vd_asfloat(r0.z) / vd_asfloat(r1.w))));
        r2.xzw = vd_asuint(log2(abs(vd_asfloat(_GlobalBoxAreaColors[2].xyz))));
        r2.xzw = vd_asuint((vd_asfloat(r2.xzw) * float3(2.20000005, 2.20000005, 2.20000005)));
        r2.xzw = vd_asuint(exp2(vd_asfloat(r2.xzw)));
        r0.y = vd_asuint((-(int)(vd_asint(r0.y) == 3)));
        if (r0.y != 0u) {
          r0.y = vd_asuint((vd_asfloat(r0.x) + -1.0));
          r0.y = vd_asuint((vd_asfloat(_GlobalBoxAreaShimmerParams[2].z) * vd_asfloat(r0.y) + 1.0));
        } else {
          r0.y = 0x3f800000u;
        }
        r1.w = vd_asuint((-(int)(vd_asfloat(_GlobalBoxAreaIntensities[2]) >= vd_asfloat(r6.x))));
        r1.w = (r1.w & 0x3f800000u);
        r0.z = vd_asuint((vd_asfloat(r0.z) * vd_asfloat(r1.w)));
        r0.z = vd_asuint((vd_asfloat(r0.z) * vd_asfloat(_GlobalBoxAreaColors[2].w)));
        r0.y = vd_asuint((vd_asfloat(r0.y) * vd_asfloat(r0.z)));
        r2.xzw = vd_asuint(((-(vd_asfloat(r1.xyz))) + vd_asfloat(r2.xzw)));
        r1.xyz = vd_asuint((vd_asfloat(r0.yyy) * vd_asfloat(r2.xzw) + vd_asfloat(r1.xyz)));
      }
    }
    r0.y = vd_asuint((-(int)(vd_asfloat(_GlobalBoxAreaIntensities[3]) >= 0.00100000005)));
    if (r0.y != 0u) {
      r0.y = vd_asuint(((int)vd_asfloat(_GlobalBoxAreaCenters[3].w)));
      r2.xz = vd_asuint((-(int2)(vd_asint(r0.yy) == int2(1, 4))));
      r0.z = (r2.z | r2.x);
      if (r0.z == 0u) {
        r2.xzw = vd_asuint((vd_asfloat(v2.xyz) + (-(vd_asfloat(_GlobalBoxAreaCenters[3].xyz)))));
        r2.xzw = vd_asuint(((-(abs(vd_asfloat(r2.xzw)))) + vd_asfloat(_GlobalBoxAreaExtents[3].xyz)));
        r0.z = vd_asuint(min(vd_asfloat(r2.w), vd_asfloat(r2.z)));
        r0.z = vd_asuint(min(vd_asfloat(r0.z), vd_asfloat(r2.x)));
        r1.w = vd_asuint(max(vd_asfloat(_GlobalBoxAreaExtents[3].w), 0.00100000005));
        r0.z = vd_asuint(saturate((vd_asfloat(r0.z) / vd_asfloat(r1.w))));
        r2.xzw = vd_asuint(log2(abs(vd_asfloat(_GlobalBoxAreaColors[3].xyz))));
        r2.xzw = vd_asuint((vd_asfloat(r2.xzw) * float3(2.20000005, 2.20000005, 2.20000005)));
        r2.xzw = vd_asuint(exp2(vd_asfloat(r2.xzw)));
        r0.y = vd_asuint((-(int)(vd_asint(r0.y) == 3)));
        if (r0.y != 0u) {
          r0.y = vd_asuint((vd_asfloat(r0.x) + -1.0));
          r0.y = vd_asuint((vd_asfloat(_GlobalBoxAreaShimmerParams[3].z) * vd_asfloat(r0.y) + 1.0));
        } else {
          r0.y = 0x3f800000u;
        }
        r1.w = vd_asuint((-(int)(vd_asfloat(_GlobalBoxAreaIntensities[3]) >= vd_asfloat(r6.x))));
        r1.w = (r1.w & 0x3f800000u);
        r0.z = vd_asuint((vd_asfloat(r0.z) * vd_asfloat(r1.w)));
        r0.z = vd_asuint((vd_asfloat(r0.z) * vd_asfloat(_GlobalBoxAreaColors[3].w)));
        r0.y = vd_asuint((vd_asfloat(r0.y) * vd_asfloat(r0.z)));
        r2.xzw = vd_asuint(((-(vd_asfloat(r1.xyz))) + vd_asfloat(r2.xzw)));
        r1.xyz = vd_asuint((vd_asfloat(r0.yyy) * vd_asfloat(r2.xzw) + vd_asfloat(r1.xyz)));
      }
    }
    r0.y = vd_asuint((-(int)(vd_asfloat(_GlobalBoxAreaIntensities[4]) >= 0.00100000005)));
    if (r0.y != 0u) {
      r0.y = vd_asuint(((int)vd_asfloat(_GlobalBoxAreaCenters[4].w)));
      r2.xz = vd_asuint((-(int2)(vd_asint(r0.yy) == int2(1, 4))));
      r0.z = (r2.z | r2.x);
      if (r0.z == 0u) {
        r2.xzw = vd_asuint((vd_asfloat(v2.xyz) + (-(vd_asfloat(_GlobalBoxAreaCenters[4].xyz)))));
        r2.xzw = vd_asuint(((-(abs(vd_asfloat(r2.xzw)))) + vd_asfloat(_GlobalBoxAreaExtents[4].xyz)));
        r0.z = vd_asuint(min(vd_asfloat(r2.w), vd_asfloat(r2.z)));
        r0.z = vd_asuint(min(vd_asfloat(r0.z), vd_asfloat(r2.x)));
        r1.w = vd_asuint(max(vd_asfloat(_GlobalBoxAreaExtents[4].w), 0.00100000005));
        r0.z = vd_asuint(saturate((vd_asfloat(r0.z) / vd_asfloat(r1.w))));
        r2.xzw = vd_asuint(log2(abs(vd_asfloat(_GlobalBoxAreaColors[4].xyz))));
        r2.xzw = vd_asuint((vd_asfloat(r2.xzw) * float3(2.20000005, 2.20000005, 2.20000005)));
        r2.xzw = vd_asuint(exp2(vd_asfloat(r2.xzw)));
        r0.y = vd_asuint((-(int)(vd_asint(r0.y) == 3)));
        if (r0.y != 0u) {
          r0.y = vd_asuint((vd_asfloat(r0.x) + -1.0));
          r0.y = vd_asuint((vd_asfloat(_GlobalBoxAreaShimmerParams[4].z) * vd_asfloat(r0.y) + 1.0));
        } else {
          r0.y = 0x3f800000u;
        }
        r1.w = vd_asuint((-(int)(vd_asfloat(_GlobalBoxAreaIntensities[4]) >= vd_asfloat(r6.x))));
        r1.w = (r1.w & 0x3f800000u);
        r0.z = vd_asuint((vd_asfloat(r0.z) * vd_asfloat(r1.w)));
        r0.z = vd_asuint((vd_asfloat(r0.z) * vd_asfloat(_GlobalBoxAreaColors[4].w)));
        r0.y = vd_asuint((vd_asfloat(r0.y) * vd_asfloat(r0.z)));
        r2.xzw = vd_asuint(((-(vd_asfloat(r1.xyz))) + vd_asfloat(r2.xzw)));
        r1.xyz = vd_asuint((vd_asfloat(r0.yyy) * vd_asfloat(r2.xzw) + vd_asfloat(r1.xyz)));
      }
    }
    r0.y = vd_asuint((-(int)(vd_asfloat(_GlobalBoxAreaIntensities[5]) >= 0.00100000005)));
    if (r0.y != 0u) {
      r0.y = vd_asuint(((int)vd_asfloat(_GlobalBoxAreaCenters[5].w)));
      r2.xz = vd_asuint((-(int2)(vd_asint(r0.yy) == int2(1, 4))));
      r0.z = (r2.z | r2.x);
      if (r0.z == 0u) {
        r2.xzw = vd_asuint((vd_asfloat(v2.xyz) + (-(vd_asfloat(_GlobalBoxAreaCenters[5].xyz)))));
        r2.xzw = vd_asuint(((-(abs(vd_asfloat(r2.xzw)))) + vd_asfloat(_GlobalBoxAreaExtents[5].xyz)));
        r0.z = vd_asuint(min(vd_asfloat(r2.w), vd_asfloat(r2.z)));
        r0.z = vd_asuint(min(vd_asfloat(r0.z), vd_asfloat(r2.x)));
        r1.w = vd_asuint(max(vd_asfloat(_GlobalBoxAreaExtents[5].w), 0.00100000005));
        r0.z = vd_asuint(saturate((vd_asfloat(r0.z) / vd_asfloat(r1.w))));
        r2.xzw = vd_asuint(log2(abs(vd_asfloat(_GlobalBoxAreaColors[5].xyz))));
        r2.xzw = vd_asuint((vd_asfloat(r2.xzw) * float3(2.20000005, 2.20000005, 2.20000005)));
        r2.xzw = vd_asuint(exp2(vd_asfloat(r2.xzw)));
        r0.y = vd_asuint((-(int)(vd_asint(r0.y) == 3)));
        if (r0.y != 0u) {
          r0.y = vd_asuint((vd_asfloat(r0.x) + -1.0));
          r0.y = vd_asuint((vd_asfloat(_GlobalBoxAreaShimmerParams[5].z) * vd_asfloat(r0.y) + 1.0));
        } else {
          r0.y = 0x3f800000u;
        }
        r1.w = vd_asuint((-(int)(vd_asfloat(_GlobalBoxAreaIntensities[5]) >= vd_asfloat(r6.x))));
        r1.w = (r1.w & 0x3f800000u);
        r0.z = vd_asuint((vd_asfloat(r0.z) * vd_asfloat(r1.w)));
        r0.z = vd_asuint((vd_asfloat(r0.z) * vd_asfloat(_GlobalBoxAreaColors[5].w)));
        r0.y = vd_asuint((vd_asfloat(r0.y) * vd_asfloat(r0.z)));
        r2.xzw = vd_asuint(((-(vd_asfloat(r1.xyz))) + vd_asfloat(r2.xzw)));
        r1.xyz = vd_asuint((vd_asfloat(r0.yyy) * vd_asfloat(r2.xzw) + vd_asfloat(r1.xyz)));
      }
    }
    r0.y = vd_asuint((-(int)(vd_asfloat(_GlobalBoxAreaIntensities[6]) >= 0.00100000005)));
    if (r0.y != 0u) {
      r0.y = vd_asuint(((int)vd_asfloat(_GlobalBoxAreaCenters[6].w)));
      r2.xz = vd_asuint((-(int2)(vd_asint(r0.yy) == int2(1, 4))));
      r0.z = (r2.z | r2.x);
      if (r0.z == 0u) {
        r2.xzw = vd_asuint((vd_asfloat(v2.xyz) + (-(vd_asfloat(_GlobalBoxAreaCenters[6].xyz)))));
        r2.xzw = vd_asuint(((-(abs(vd_asfloat(r2.xzw)))) + vd_asfloat(_GlobalBoxAreaExtents[6].xyz)));
        r0.z = vd_asuint(min(vd_asfloat(r2.w), vd_asfloat(r2.z)));
        r0.z = vd_asuint(min(vd_asfloat(r0.z), vd_asfloat(r2.x)));
        r1.w = vd_asuint(max(vd_asfloat(_GlobalBoxAreaExtents[6].w), 0.00100000005));
        r0.z = vd_asuint(saturate((vd_asfloat(r0.z) / vd_asfloat(r1.w))));
        r2.xzw = vd_asuint(log2(abs(vd_asfloat(_GlobalBoxAreaColors[6].xyz))));
        r2.xzw = vd_asuint((vd_asfloat(r2.xzw) * float3(2.20000005, 2.20000005, 2.20000005)));
        r2.xzw = vd_asuint(exp2(vd_asfloat(r2.xzw)));
        r0.y = vd_asuint((-(int)(vd_asint(r0.y) == 3)));
        if (r0.y != 0u) {
          r0.y = vd_asuint((vd_asfloat(r0.x) + -1.0));
          r0.y = vd_asuint((vd_asfloat(_GlobalBoxAreaShimmerParams[6].z) * vd_asfloat(r0.y) + 1.0));
        } else {
          r0.y = 0x3f800000u;
        }
        r1.w = vd_asuint((-(int)(vd_asfloat(_GlobalBoxAreaIntensities[6]) >= vd_asfloat(r6.x))));
        r1.w = (r1.w & 0x3f800000u);
        r0.z = vd_asuint((vd_asfloat(r0.z) * vd_asfloat(r1.w)));
        r0.z = vd_asuint((vd_asfloat(r0.z) * vd_asfloat(_GlobalBoxAreaColors[6].w)));
        r0.y = vd_asuint((vd_asfloat(r0.y) * vd_asfloat(r0.z)));
        r2.xzw = vd_asuint(((-(vd_asfloat(r1.xyz))) + vd_asfloat(r2.xzw)));
        r1.xyz = vd_asuint((vd_asfloat(r0.yyy) * vd_asfloat(r2.xzw) + vd_asfloat(r1.xyz)));
      }
    }
    r0.y = vd_asuint((-(int)(vd_asfloat(_GlobalBoxAreaIntensities[7]) >= 0.00100000005)));
    r0.z = vd_asuint(((int)vd_asfloat(_GlobalBoxAreaCenters[7].w)));
    r2.xzw = vd_asuint((-(int3)(vd_asint(r0.zzz) == int3(1, 4, 3))));
    r0.z = (r2.z | r2.x);
    r4.xyz = vd_asuint((vd_asfloat(v2.xyz) + (-(vd_asfloat(_GlobalBoxAreaCenters[7].xyz)))));
    r4.xyz = vd_asuint(((-(abs(vd_asfloat(r4.xyz)))) + vd_asfloat(_GlobalBoxAreaExtents[7].xyz)));
    r1.w = vd_asuint(min(vd_asfloat(r4.z), vd_asfloat(r4.y)));
    r1.w = vd_asuint(min(vd_asfloat(r1.w), vd_asfloat(r4.x)));
    r2.x = vd_asuint(max(vd_asfloat(_GlobalBoxAreaExtents[7].w), 0.00100000005));
    r1.w = vd_asuint(saturate((vd_asfloat(r1.w) / vd_asfloat(r2.x))));
    r4.xyz = vd_asuint(log2(abs(vd_asfloat(_GlobalBoxAreaColors[7].xyz))));
    r4.xyz = vd_asuint((vd_asfloat(r4.xyz) * float3(2.20000005, 2.20000005, 2.20000005)));
    r4.xyz = vd_asuint(exp2(vd_asfloat(r4.xyz)));
    if (r2.w != 0u) {
      r0.x = vd_asuint((vd_asfloat(r0.x) + -1.0));
      r0.x = vd_asuint((vd_asfloat(_GlobalBoxAreaShimmerParams[7].z) * vd_asfloat(r0.x) + 1.0));
    } else {
      r0.x = 0x3f800000u;
    }
    r2.x = vd_asuint((-(int)(vd_asfloat(_GlobalBoxAreaIntensities[7]) >= vd_asfloat(r6.x))));
    r2.x = (r2.x & 0x3f800000u);
    r1.w = vd_asuint((vd_asfloat(r1.w) * vd_asfloat(r2.x)));
    r1.w = vd_asuint((vd_asfloat(r1.w) * vd_asfloat(_GlobalBoxAreaColors[7].w)));
    r0.x = vd_asuint((vd_asfloat(r0.x) * vd_asfloat(r1.w)));
    r2.xzw = vd_asuint(((-(vd_asfloat(r1.xyz))) + vd_asfloat(r4.xyz)));
    r2.xzw = vd_asuint((vd_asfloat(r0.xxx) * vd_asfloat(r2.xzw) + vd_asfloat(r1.xyz)));
    r2.xzw = ((r0.zzz != 0u) ? r1.xyz : r2.xzw);
    r1.xyz = ((r0.yyy != 0u) ? r2.xzw : r1.xyz);
  }
  r0.x = vd_asuint((-(int)(0.0 < vd_asfloat(_DissolveProgress))));
  if (r0.x != 0u) {
    r0.x = vd_asuint((vd_asfloat(_DissolveProgress) + vd_asfloat(_DissolveProgress)));
    r0.x = vd_asuint(saturate(vd_asfloat(r0.x)));
    r0.y = vd_asuint(saturate((vd_asfloat(_DissolveProgress) * 2.0 + -1.0)));
    r0.z = vd_asuint(((-(vd_asfloat(r2.y))) + 1.0));
    r0.z = vd_asuint(((-(vd_asfloat(r0.x))) + vd_asfloat(r0.z)));
    r0.z = vd_asuint(saturate((vd_asfloat(r0.z) * 4.0)));
    r1.w = vd_asuint((vd_asfloat(r0.z) * -2.0 + 3.0));
    r0.z = vd_asuint((vd_asfloat(r0.z) * vd_asfloat(r0.z)));
    r0.z = vd_asuint(((-(vd_asfloat(r1.w))) * vd_asfloat(r0.z) + 1.0));
    r0.x = vd_asuint((vd_asfloat(r0.x) * vd_asfloat(r0.z)));
    r0.x = vd_asuint((vd_asfloat(r0.x) * vd_asfloat(_DissolveEdgeColor.w)));
    r2.xyz = vd_asuint(((-(vd_asfloat(r1.xyz))) + vd_asfloat(_DissolveEdgeColor.xyz)));
    r1.xyz = vd_asuint((vd_asfloat(r0.xxx) * vd_asfloat(r2.xyz) + vd_asfloat(r1.xyz)));
    r0.x = vd_asuint(((-(vd_asfloat(r0.y))) + 1.0));
    r0.yz = vd_asuint((vd_asfloat(_ScreenSize.xy) / vd_asfloat(_ScreenParams.xy)));
    r2.xy = vd_asuint((vd_asfloat(r5.xz) * vd_asfloat(_ScreenParams.xy)));
    r0.yz = vd_asuint((vd_asfloat(r0.yz) * vd_asfloat(r2.xy)));
    r0.yz = vd_asuint(floor(vd_asfloat(r0.yz)));
    r0.yz = ((uint2)vd_asfloat(r0.yz));
    r0.yz = (r0.yz & uint2(0x00000003u, 0x00000003u));
    r0.z = (r0.z << (uint)2);
    r0.y = vd_asuint((vd_asint(r0.z) + vd_asint(r0.y)));
    r0.y = x0[r0.y + 0].x;
    r0.x = vd_asuint(((-(vd_asfloat(r0.y))) + vd_asfloat(r0.x)));
    r0.x = vd_asuint((-(int)(vd_asfloat(r0.x) < 0.0)));
    if (r0.x != 0u) discard;
  }
  r0.xyz = vd_asuint((vd_asfloat(r3.xyz) * vd_asfloat((_EmissionIntensity).xxx)));
  r1.w = vd_asuint((vd_asfloat(_Time.y) + vd_asfloat(_Time.y)));
  r2.x = vd_asuint(dot(vd_asfloat(v8.xyz), float3(0.298999995, 0.587000012, 0.114)));
  r2.y = vd_asuint(dot(vd_asfloat(v10.xyz), float3(12.9898005, 78.2330017, 45.1640015)));
  r2.y = vd_asuint(frac(vd_asfloat(r2.y)));
  r2.x = vd_asuint((vd_asfloat(r2.y) + vd_asfloat(r2.x)));
  r3.xyzw = vd_asuint((vd_asfloat(r2.xxxx) * float4(91.3000031, 37.0999985, 53.7000008, 3.0)));
  r2.yzw = vd_asuint(sin(vd_asfloat(r3.xyz)));
  r2.yzw = vd_asuint((vd_asfloat(r2.yzw) * float3(4337.5, 1572.90002, 9281.2002)));
  r2.yzw = vd_asuint(frac(vd_asfloat(r2.yzw)));
  r2.yzw = vd_asuint((vd_asfloat(r2.yzw) * float3(4.0, 6.0, 12.0) + float3(3.70000005, 7.19999981, 15.3000002)));
  r3.xyz = vd_asuint((vd_asfloat(r2.xxx) * float3(6.28000021, 3.1400001, 4.71000004)));
  r2.yzw = vd_asuint((vd_asfloat(r1.www) * vd_asfloat(r2.yzw) + vd_asfloat(r3.xyz)));
  r2.yzw = vd_asuint(sin(vd_asfloat(r2.yzw)));
  r2.yzw = vd_asuint((vd_asfloat(r2.yzw) * float3(0.5, 0.5, 0.5) + float3(0.5, 0.5, 0.5)));
  r1.w = vd_asuint((vd_asfloat(_Time.y) * 1.60000002));
  r1.w = vd_asuint((vd_asfloat(r2.x) * 2.0 + vd_asfloat(r1.w)));
  r1.w = vd_asuint(sin(vd_asfloat(r1.w)));
  r3.x = vd_asuint((vd_asfloat(_Time.y) * 2.5999999 + vd_asfloat(r3.w)));
  r3.x = vd_asuint(sin(vd_asfloat(r3.x)));
  r1.w = vd_asuint((vd_asfloat(r1.w) * vd_asfloat(r3.x)));
  r1.w = vd_asuint((vd_asfloat(r1.w) * 0.5 + 0.5));
  r2.x = vd_asuint((vd_asfloat(_Time.y) * 27.3999996 + vd_asfloat(r2.x)));
  r2.x = vd_asuint(sin(vd_asfloat(r2.x)));
  r2.x = vd_asuint((vd_asfloat(r2.x) * 43758.5469));
  r2.x = vd_asuint(frac(vd_asfloat(r2.x)));
  r2.x = vd_asuint((-(int)(vd_asfloat(r2.x) >= 0.970000029)));
  r2.y = vd_asuint(dot(vd_asfloat(r2.yzw), float3(0.150000006, 0.150000006, 0.200000003)));
  r1.w = vd_asuint((vd_asfloat(r1.w) * 0.300000012 + vd_asfloat(r2.y)));
  r2.x = (r2.x & 0x3e4ccccdu);
  r1.w = vd_asuint((vd_asfloat(r1.w) + vd_asfloat(r2.x)));
  r1.w = vd_asuint((vd_asfloat(r1.w) + -0.75));
  r1.w = vd_asuint(saturate((vd_asfloat(r1.w) * -9.99999809)));
  r2.x = vd_asuint((vd_asfloat(r1.w) * -2.0 + 3.0));
  r1.w = vd_asuint((vd_asfloat(r1.w) * vd_asfloat(r1.w)));
  r1.w = vd_asuint((vd_asfloat(r2.x) * vd_asfloat(r1.w) + -1.0));
  r1.w = vd_asuint((vd_asfloat(_EmissionFlickRate) * vd_asfloat(r1.w) + 1.0));
  r0.xyz = vd_asuint((vd_asfloat(r0.xyz) * vd_asfloat(r1.www) + vd_asfloat(r1.xyz)));
  r1.xyz = vd_asuint(((-(vd_asfloat(r0.xyz))) + vd_asfloat(_GlobalBackgroundColor.xyz)));
  r0.xyz = vd_asuint((vd_asfloat((_GlobalBackgroundColorIntensity).xxx) * vd_asfloat(r1.xyz) + vd_asfloat(r0.xyz)));
  r1.xy = vd_asuint((vd_asfloat(v2.xz) + (-(vd_asfloat(_GlobalPlayerPosition.xz)))));
  r1.z = vd_asuint(dot(vd_asfloat(r1.xy), vd_asfloat(r1.xy)));
  r2.xy = vd_asuint((vd_asfloat(v2.xz) + (-(vd_asfloat(_GlobalCursorPosition.xz)))));
  r1.w = vd_asuint(dot(vd_asfloat(r2.xy), vd_asfloat(r2.xy)));
  r2.xy = vd_asuint(((-(vd_asfloat(_GlobalPlayerPosition.xz))) + vd_asfloat(_GlobalCursorPosition.xz)));
  r2.z = vd_asuint(dot(vd_asfloat(r2.xy), vd_asfloat(r2.xy)));
  r2.z = vd_asuint(max(vd_asfloat(r2.z), 9.99999997e-07));
  r1.x = vd_asuint(dot(vd_asfloat(r1.xy), vd_asfloat(r2.xy)));
  r1.x = vd_asuint(saturate((vd_asfloat(r1.x) / vd_asfloat(r2.z))));
  r1.xy = vd_asuint((vd_asfloat(r2.xy) * vd_asfloat(r1.xx) + vd_asfloat(_GlobalPlayerPosition.xz)));
  r1.xy = vd_asuint(((-(vd_asfloat(r1.xy))) + vd_asfloat(v2.xz)));
  r1.x = vd_asuint(dot(vd_asfloat(r1.xy), vd_asfloat(r1.xy)));
  r1.xzw = vd_asuint(sqrt(vd_asfloat(r1.xzw)));
  r1.xz = vd_asuint((vd_asfloat(r1.xz) + (-(vd_asfloat(float2(_GlobalCursorLineThickness, _GlobalCursorPlayerRadius))))));
  r1.y = vd_asuint((1.0 / vd_asfloat(_GlobalCursorLineSmoothness)));
  r1.x = vd_asuint(saturate((vd_asfloat(r1.y) * vd_asfloat(r1.x))));
  r2.x = vd_asuint((vd_asfloat(r1.x) * -2.0 + 3.0));
  r1.x = vd_asuint((vd_asfloat(r1.x) * vd_asfloat(r1.x)));
  r1.x = vd_asuint(((-(vd_asfloat(r2.x))) * vd_asfloat(r1.x) + 1.0));
  r2.x = vd_asuint(saturate((vd_asfloat(r1.y) * vd_asfloat(r1.z))));
  r2.y = vd_asuint((vd_asfloat(r2.x) * -2.0 + 3.0));
  r2.x = vd_asuint((vd_asfloat(r2.x) * vd_asfloat(r2.x)));
  r2.x = vd_asuint((vd_asfloat(r2.x) * vd_asfloat(r2.y)));
  r1.x = vd_asuint((vd_asfloat(r1.x) * vd_asfloat(r2.x)));
  r1.w = vd_asuint((vd_asfloat(r1.w) + (-(vd_asfloat(_GlobalCursorRadius)))));
  r1.y = vd_asuint(saturate((vd_asfloat(r1.y) * vd_asfloat(r1.w))));
  r1.w = vd_asuint((vd_asfloat(r1.y) * -2.0 + 3.0));
  r1.y = vd_asuint((vd_asfloat(r1.y) * vd_asfloat(r1.y)));
  r2.x = vd_asuint((vd_asfloat(r1.y) * vd_asfloat(r1.w)));
  r1.x = vd_asuint((vd_asfloat(r1.x) * vd_asfloat(r2.x)));
  r1.z = vd_asuint((abs(vd_asfloat(r1.z)) + (-(vd_asfloat(_GlobalCursorPlayerLineThickness)))));
  r2.x = vd_asuint((1.0 / vd_asfloat(_GlobalCursorPlayerLineSmoothness)));
  r1.z = vd_asuint(saturate((vd_asfloat(r1.z) * vd_asfloat(r2.x))));
  r2.x = vd_asuint((vd_asfloat(r1.z) * -2.0 + 3.0));
  r1.z = vd_asuint((vd_asfloat(r1.z) * vd_asfloat(r1.z)));
  r1.z = vd_asuint(((-(vd_asfloat(r2.x))) * vd_asfloat(r1.z) + 1.0));
  r1.y = vd_asuint(((-(vd_asfloat(r1.w))) * vd_asfloat(r1.y) + 1.0));
  r1.x = vd_asuint(max(vd_asfloat(r1.z), vd_asfloat(r1.x)));
  r1.x = vd_asuint(max(vd_asfloat(r1.y), vd_asfloat(r1.x)));
  r1.x = vd_asuint(saturate((vd_asfloat(r1.x) * vd_asfloat(_GlobalCursorLineColor.w))));
  r1.yzw = vd_asuint(((-(vd_asfloat(r0.xyz))) + vd_asfloat(_GlobalCursorLineColor.xyz)));
  o0.xyz = (vd_asfloat(r1.xxx) * vd_asfloat(r1.yzw) + vd_asfloat(r0.xyz));
  o0.w = vd_asfloat(r0.w);
  return;
}
