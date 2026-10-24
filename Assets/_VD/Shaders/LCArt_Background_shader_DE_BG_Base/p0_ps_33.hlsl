// LCArt/Background/shader_DE_BG_Base | pass 0 "ForwardLit" | progFragment | keywords: FOG_LINEAR _LIGHT_LAYERS | blob 33
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
  uint4 r0, r1, r2, r3, r4, r5, r6, r7, r8, r9, r10, r11;
  r0 = 0u; r1 = 0u; r2 = 0u; r3 = 0u; r4 = 0u; r5 = 0u; r6 = 0u; r7 = 0u; r8 = 0u; r9 = 0u; r10 = 0u; r11 = 0u;
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
    r4.xyzw = vd_asuint((-(int4)(vd_asint((_BackgroundType).xxxx) == int4(2, 4, 3, 9))));
    r1.w = (r4.y | r4.x);
    r1.w = (r4.z | r1.w);
    if (r1.w != 0u) {
      r5.xyzw = vd_asuint(_CustomDepthBuffer.SampleBias(sampler_CustomDepthBuffer, vd_asfloat(r3.xz), vd_asfloat(_GlobalMipBias.x)).xyzw);
      r1.w = vd_asuint(((-(vd_asfloat(r5.z))) + vd_asfloat(v7.z)));
      r1.w = vd_asuint((vd_asfloat(r1.w) + -2.49999994e-05));
      r1.w = vd_asuint(saturate((vd_asfloat(r1.w) * -20000.0)));
      r2.x = vd_asuint((vd_asfloat(r1.w) * -2.0 + 3.0));
      r1.w = vd_asuint((vd_asfloat(r1.w) * vd_asfloat(r1.w)));
      r2.z = vd_asuint((vd_asfloat(r1.w) * vd_asfloat(r2.x)));
      r3.yw = vd_asuint(((-(vd_asfloat(float2(_GlobalSeeThroughCircleRange, _GlobalSeeThroughCircleAlpha)))) + float2(1.0, 1.0)));
      r2.w = vd_asuint(((-(vd_asfloat(r3.y))) + 1.0));
      r3.y = vd_asuint(((-(vd_asfloat(r3.y))) + vd_asfloat(r5.w)));
      r2.w = vd_asuint((1.0 / vd_asfloat(r2.w)));
      r2.w = vd_asuint(saturate((vd_asfloat(r2.w) * vd_asfloat(r3.y))));
      r3.y = vd_asuint((vd_asfloat(r2.w) * -2.0 + 3.0));
      r2.w = vd_asuint((vd_asfloat(r2.w) * vd_asfloat(r2.w)));
      r2.w = vd_asuint((vd_asfloat(r2.w) * vd_asfloat(r3.y)));
      r1.w = vd_asuint(((-(vd_asfloat(r2.x))) * vd_asfloat(r1.w) + 1.0));
      r1.w = vd_asuint((vd_asfloat(r2.w) * vd_asfloat(r1.w) + vd_asfloat(r2.z)));
      r2.x = vd_asuint((vd_asfloat(_GlobalBackgroundSeeThroughEnable) * vd_asfloat(_GlobalPlaneSystemEnable)));
      r1.w = vd_asuint((vd_asfloat(r1.w) + -1.0));
      r1.w = vd_asuint((vd_asfloat(r2.x) * vd_asfloat(r1.w) + 1.0));
      r1.w = vd_asuint((vd_asfloat(r1.w) * vd_asfloat(r3.w) + vd_asfloat(_GlobalSeeThroughCircleAlpha)));
      r2.xz = vd_asuint((vd_asfloat(_ScreenSize.xy) / vd_asfloat(_ScreenParams.xy)));
      r3.yw = vd_asuint((vd_asfloat(r3.xz) * vd_asfloat(_ScreenParams.xy)));
      r2.xz = vd_asuint((vd_asfloat(r2.xz) * vd_asfloat(r3.yw)));
      r2.xz = vd_asuint(floor(vd_asfloat(r2.xz)));
      r2.xz = ((uint2)vd_asfloat(r2.xz));
      r2.xz = (r2.xz & uint2(0x00000003u, 0x00000003u));
      r2.z = (r2.z << (uint)2);
      r2.x = vd_asuint((vd_asint(r2.z) + vd_asint(r2.x)));
      r2.x = x0[r2.x + 0].x;
      r1.w = vd_asuint((vd_asfloat(r1.w) + (-(vd_asfloat(r2.x)))));
      r1.w = vd_asuint((-(int)(vd_asfloat(r1.w) < 0.0)));
      if (r1.w != 0u) discard;
    }
    r1.w = vd_asuint((-(int)(1.5 < vd_asfloat(v1.z))));
    if (r1.w != 0u) {
      r5.xyzw = vd_asuint(_CustomDepthBuffer.SampleBias(sampler_CustomDepthBuffer, vd_asfloat(r3.xz), vd_asfloat(_GlobalMipBias.x)).xyzw);
      r1.w = vd_asuint(((-(vd_asfloat(r5.z))) + vd_asfloat(v7.z)));
      r1.w = vd_asuint((vd_asfloat(r1.w) + -2.49999994e-05));
      r1.w = vd_asuint(saturate((vd_asfloat(r1.w) * -20000.0)));
      r2.x = vd_asuint((vd_asfloat(r1.w) * -2.0 + 3.0));
      r1.w = vd_asuint((vd_asfloat(r1.w) * vd_asfloat(r1.w)));
      r2.z = vd_asuint((vd_asfloat(r1.w) * vd_asfloat(r2.x)));
      r3.yw = vd_asuint(((-(vd_asfloat(float2(_GlobalSeeThroughCircleRange, _GlobalSeeThroughCircleAlpha)))) + float2(1.0, 1.0)));
      r2.w = vd_asuint(((-(vd_asfloat(r3.y))) + 1.0));
      r3.y = vd_asuint(((-(vd_asfloat(r3.y))) + vd_asfloat(r5.w)));
      r2.w = vd_asuint((1.0 / vd_asfloat(r2.w)));
      r2.w = vd_asuint(saturate((vd_asfloat(r2.w) * vd_asfloat(r3.y))));
      r3.y = vd_asuint((vd_asfloat(r2.w) * -2.0 + 3.0));
      r2.w = vd_asuint((vd_asfloat(r2.w) * vd_asfloat(r2.w)));
      r2.w = vd_asuint((vd_asfloat(r2.w) * vd_asfloat(r3.y)));
      r1.w = vd_asuint(((-(vd_asfloat(r2.x))) * vd_asfloat(r1.w) + 1.0));
      r1.w = vd_asuint((vd_asfloat(r2.w) * vd_asfloat(r1.w) + vd_asfloat(r2.z)));
      r2.x = vd_asuint((vd_asfloat(_GlobalBackgroundSeeThroughEnable) * vd_asfloat(_GlobalPlaneSystemEnable)));
      r1.w = vd_asuint((vd_asfloat(r1.w) + -1.0));
      r1.w = vd_asuint((vd_asfloat(r2.x) * vd_asfloat(r1.w) + 1.0));
      r1.w = vd_asuint((vd_asfloat(r1.w) * vd_asfloat(r3.w) + vd_asfloat(_GlobalSeeThroughCircleAlpha)));
      r2.zw = vd_asuint((vd_asfloat(_ScreenSize.xy) / vd_asfloat(_ScreenParams.xy)));
      r3.yw = vd_asuint((vd_asfloat(r3.xz) * vd_asfloat(_ScreenParams.xy)));
      r2.zw = vd_asuint((vd_asfloat(r2.zw) * vd_asfloat(r3.yw)));
      r2.zw = vd_asuint(floor(vd_asfloat(r2.zw)));
      r2.zw = ((uint2)vd_asfloat(r2.zw));
      r2.zw = (r2.zw & uint2(0x00000003u, 0x00000003u));
      r2.w = (r2.w << (uint)2);
      r2.z = vd_asuint((vd_asint(r2.w) + vd_asint(r2.z)));
      r2.z = x0[r2.z + 0].x;
      r1.w = vd_asuint((vd_asfloat(r1.w) + (-(vd_asfloat(r2.z)))));
      r1.w = vd_asuint((-(int)(vd_asfloat(r1.w) < 0.0)));
      if (r1.w != 0u) discard;
      r1.w = vd_asuint((-(int)(0.125 < vd_asfloat(v2.y))));
      if (r1.w != 0u) {
        r5.xyzw = vd_asuint(_CharacterAreaMaskTexture.SampleLevel(sampler_CharacterAreaMaskTexture, vd_asfloat(r3.xz), 0.0).xyzw);
        r2.zw = vd_asuint(round(vd_asfloat(_WorldSpaceCameraPos.xz)));
        r2.zw = vd_asuint(((-(vd_asfloat(r2.zw))) + vd_asfloat(v2.xz)));
        r6.x = vd_asuint((-(vd_asfloat((VD_COL(unity_MatrixV, 0).z)))));
        r6.y = vd_asuint((-(vd_asfloat((VD_COL(unity_MatrixV, 2).z)))));
        r1.w = vd_asuint(dot(vd_asfloat(r6.xy), vd_asfloat(r6.xy)));
        r1.w = vd_asuint(rsqrt(vd_asfloat(r1.w)));
        r3.yw = vd_asuint((vd_asfloat(r1.ww) * vd_asfloat(r6.xy)));
        r1.w = vd_asuint(dot(vd_asfloat(r2.zw), vd_asfloat(r3.yw)));
        r1.w = vd_asuint(saturate((vd_asfloat(r1.w) * 0.00999999978)));
        r1.w = vd_asuint(((-(vd_asfloat(r1.w))) + vd_asfloat(r5.y)));
        r1.w = vd_asuint(saturate((vd_asfloat(r1.w) * 40.0)));
        r2.z = vd_asuint((vd_asfloat(r1.w) * -2.0 + 3.0));
        r1.w = vd_asuint((vd_asfloat(r1.w) * vd_asfloat(r1.w)));
        r1.w = vd_asuint((vd_asfloat(r1.w) * vd_asfloat(r2.z)));
        r2.z = vd_asuint((vd_asfloat(r5.x) + -1.0));
        r1.w = vd_asuint((vd_asfloat(r1.w) * vd_asfloat(r2.z)));
        r1.w = vd_asuint((vd_asfloat(_GlobalBackgroundSeeThroughEnable) * vd_asfloat(r1.w) + 1.0));
        r5.xyz = vd_asuint((vd_asfloat(v2.xyz) + (-(vd_asfloat(_WorldSpaceCameraPos.xyz)))));
        r2.z = vd_asuint(((-(vd_asfloat(_WorldSpaceCameraPos.y))) + -0.150000006));
        r2.z = vd_asuint((vd_asfloat(r2.z) / vd_asfloat(r5.y)));
        r2.zw = vd_asuint((vd_asfloat(r5.xz) * vd_asfloat(r2.zz) + vd_asfloat(_WorldSpaceCameraPos.xz)));
        r2.zw = vd_asuint((vd_asfloat(r2.zw) + (-(vd_asfloat(_PlaneBoundMaxPos.xz)))));
        r3.yw = vd_asuint((vd_asfloat(_PlaneBoundMinPos.xz) + (-(vd_asfloat(_PlaneBoundMaxPos.xz)))));
        r2.zw = vd_asuint(saturate((vd_asfloat(r2.zw) / vd_asfloat(r3.yw))));
        r5.xyzw = vd_asuint(_PlaneSightRenderTarget.SampleLevel(sampler_PlaneSightRenderTarget, vd_asfloat(r2.zw), 0.0).xyzw);
        r2.z = vd_asuint((vd_asfloat(r5.w) + -1.0));
        r2.z = vd_asuint((vd_asfloat(_GlobalPlaneSystemEnable) * vd_asfloat(r2.z) + 1.0));
        r2.w = vd_asuint(((-(vd_asfloat(_SeeThroughDistanceMin))) + vd_asfloat(_SeeThroughDistanceMax)));
        r2.w = vd_asuint((1.0 / vd_asfloat(r2.w)));
        r3.yw = uint2(0x3f800000u, 0x00000000u);
        while (true) {
          r5.x = vd_asuint((-(int)(vd_asint(r3.w) >= 3)));
          if (r5.x != 0u) break;
          r5.xyz = vd_asuint((vd_asfloat(v2.xyz) + (-(vd_asfloat(_SightPositions[(int)(r3.w)].xyz)))));
          r5.x = vd_asuint(dot(vd_asfloat(r5.xyz), vd_asfloat(r5.xyz)));
          r5.x = vd_asuint(sqrt(vd_asfloat(r5.x)));
          r5.x = vd_asuint((vd_asfloat(r5.x) + (-(vd_asfloat(_SeeThroughDistanceMin)))));
          r5.x = vd_asuint(saturate((vd_asfloat(r2.w) * vd_asfloat(r5.x))));
          r5.y = vd_asuint((vd_asfloat(r5.x) * -2.0 + 3.0));
          r5.x = vd_asuint((vd_asfloat(r5.x) * vd_asfloat(r5.x)));
          r5.x = vd_asuint((vd_asfloat(r5.x) * vd_asfloat(r5.y)));
          r3.y = vd_asuint(min(vd_asfloat(r3.y), vd_asfloat(r5.x)));
          r3.w = vd_asuint((vd_asint(r3.w) + 1));
        }
        r2.z = vd_asuint((vd_asfloat(r2.z) * vd_asfloat(r2.z)));
        r2.w = vd_asuint(((-(vd_asfloat(r3.y))) + 1.0));
        r2.z = vd_asuint(((-(vd_asfloat(r2.z))) * vd_asfloat(r2.w) + 1.0));
        r2.z = vd_asuint(max(vd_asfloat(r2.z), 0.0));
        r2.z = vd_asuint((vd_asfloat(r2.z) + -1.0));
        r2.x = vd_asuint((vd_asfloat(r2.x) * vd_asfloat(r2.z) + 1.0));
        r1.w = vd_asuint(min(vd_asfloat(r1.w), vd_asfloat(r2.x)));
      } else {
        r1.w = vd_asuint(v0.z);
      }
      r2.x = vd_asuint(((-(vd_asfloat(_GlobalSeeThroughHeightMin))) + vd_asfloat(_GlobalSeeThroughHeightMax)));
      r2.z = vd_asuint((vd_asfloat(v2.y) + (-(vd_asfloat(_GlobalSeeThroughHeightMin)))));
      r2.x = vd_asuint((1.0 / vd_asfloat(r2.x)));
      r2.x = vd_asuint(saturate((vd_asfloat(r2.x) * vd_asfloat(r2.z))));
      r2.z = vd_asuint((vd_asfloat(r2.x) * -2.0 + 3.0));
      r2.x = vd_asuint((vd_asfloat(r2.x) * vd_asfloat(r2.x)));
      r2.x = vd_asuint(((-(vd_asfloat(r2.z))) * vd_asfloat(r2.x) + 1.0));
      r1.w = vd_asuint(max(vd_asfloat(r1.w), vd_asfloat(r2.x)));
      r1.w = vd_asuint(max(vd_asfloat(r1.w), 0.00100000005));
    } else {
      r1.w = vd_asuint(v0.z);
    }
    if (r4.x != 0u) {
      r2.x = vd_asuint((-(int)(0.125 < vd_asfloat(v2.y))));
      if (r2.x != 0u) {
        r5.xyzw = vd_asuint(_CharacterAreaMaskTexture.SampleLevel(sampler_CharacterAreaMaskTexture, vd_asfloat(r3.xz), 0.0).xyzw);
        r2.xz = vd_asuint(round(vd_asfloat(_WorldSpaceCameraPos.xz)));
        r2.xz = vd_asuint(((-(vd_asfloat(r2.xz))) + vd_asfloat(v2.xz)));
        r6.x = vd_asuint((-(vd_asfloat((VD_COL(unity_MatrixV, 0).z)))));
        r6.y = vd_asuint((-(vd_asfloat((VD_COL(unity_MatrixV, 2).z)))));
        r2.w = vd_asuint(dot(vd_asfloat(r6.xy), vd_asfloat(r6.xy)));
        r2.w = vd_asuint(rsqrt(vd_asfloat(r2.w)));
        r3.yw = vd_asuint((vd_asfloat(r2.ww) * vd_asfloat(r6.xy)));
        r2.x = vd_asuint(dot(vd_asfloat(r2.xz), vd_asfloat(r3.yw)));
        r2.x = vd_asuint(saturate((vd_asfloat(r2.x) * 0.00999999978)));
        r2.x = vd_asuint(((-(vd_asfloat(r2.x))) + vd_asfloat(r5.y)));
        r2.x = vd_asuint(saturate((vd_asfloat(r2.x) * 40.0)));
        r2.z = vd_asuint((vd_asfloat(r2.x) * -2.0 + 3.0));
        r2.x = vd_asuint((vd_asfloat(r2.x) * vd_asfloat(r2.x)));
        r2.x = vd_asuint((vd_asfloat(r2.x) * vd_asfloat(r2.z)));
        r2.z = vd_asuint((vd_asfloat(r5.x) + -1.0));
        r2.x = vd_asuint((vd_asfloat(r2.z) * vd_asfloat(r2.x)));
        r2.x = vd_asuint((vd_asfloat(_GlobalBackgroundSeeThroughEnable) * vd_asfloat(r2.x) + 1.0));
        r5.xyz = vd_asuint((vd_asfloat(v2.xyz) + (-(vd_asfloat(_WorldSpaceCameraPos.xyz)))));
        r2.z = vd_asuint(((-(vd_asfloat(_WorldSpaceCameraPos.y))) + -0.150000006));
        r2.z = vd_asuint((vd_asfloat(r2.z) / vd_asfloat(r5.y)));
        r2.zw = vd_asuint((vd_asfloat(r5.xz) * vd_asfloat(r2.zz) + vd_asfloat(_WorldSpaceCameraPos.xz)));
        r2.zw = vd_asuint((vd_asfloat(r2.zw) + (-(vd_asfloat(_PlaneBoundMaxPos.xz)))));
        r3.yw = vd_asuint((vd_asfloat(_PlaneBoundMinPos.xz) + (-(vd_asfloat(_PlaneBoundMaxPos.xz)))));
        r2.zw = vd_asuint(saturate((vd_asfloat(r2.zw) / vd_asfloat(r3.yw))));
        r5.xyzw = vd_asuint(_PlaneSightRenderTarget.SampleLevel(sampler_PlaneSightRenderTarget, vd_asfloat(r2.zw), 0.0).xyzw);
        r2.z = vd_asuint((vd_asfloat(r5.w) + -1.0));
        r2.z = vd_asuint((vd_asfloat(_GlobalPlaneSystemEnable) * vd_asfloat(r2.z) + 1.0));
        r2.w = vd_asuint(((-(vd_asfloat(_SeeThroughDistanceMin))) + vd_asfloat(_SeeThroughDistanceMax)));
        r2.w = vd_asuint((1.0 / vd_asfloat(r2.w)));
        r3.yw = uint2(0x3f800000u, 0x00000000u);
        while (true) {
          r4.x = vd_asuint((-(int)(vd_asint(r3.w) >= 3)));
          if (r4.x != 0u) break;
          r5.xyz = vd_asuint((vd_asfloat(v2.xyz) + (-(vd_asfloat(_SightPositions[(int)(r3.w)].xyz)))));
          r4.x = vd_asuint(dot(vd_asfloat(r5.xyz), vd_asfloat(r5.xyz)));
          r4.x = vd_asuint(sqrt(vd_asfloat(r4.x)));
          r4.x = vd_asuint((vd_asfloat(r4.x) + (-(vd_asfloat(_SeeThroughDistanceMin)))));
          r4.x = vd_asuint(saturate((vd_asfloat(r2.w) * vd_asfloat(r4.x))));
          r5.x = vd_asuint((vd_asfloat(r4.x) * -2.0 + 3.0));
          r4.x = vd_asuint((vd_asfloat(r4.x) * vd_asfloat(r4.x)));
          r4.x = vd_asuint((vd_asfloat(r4.x) * vd_asfloat(r5.x)));
          r3.y = vd_asuint(min(vd_asfloat(r3.y), vd_asfloat(r4.x)));
          r3.w = vd_asuint((vd_asint(r3.w) + 1));
        }
        r2.w = vd_asuint((vd_asfloat(_GlobalBackgroundSeeThroughEnable) * vd_asfloat(_GlobalPlaneSystemEnable)));
        r2.z = vd_asuint((vd_asfloat(r2.z) * vd_asfloat(r2.z)));
        r3.y = vd_asuint(((-(vd_asfloat(r3.y))) + 1.0));
        r2.z = vd_asuint(((-(vd_asfloat(r2.z))) * vd_asfloat(r3.y) + 1.0));
        r2.z = vd_asuint(max(vd_asfloat(r2.z), 0.0));
        r2.z = vd_asuint((vd_asfloat(r2.z) + -1.0));
        r2.z = vd_asuint((vd_asfloat(r2.w) * vd_asfloat(r2.z) + 1.0));
        r1.w = vd_asuint(min(vd_asfloat(r2.z), vd_asfloat(r2.x)));
      }
      r2.x = vd_asuint(((-(vd_asfloat(_GlobalSeeThroughHeightMin))) + vd_asfloat(_GlobalSeeThroughHeightMax)));
      r2.z = vd_asuint((vd_asfloat(v2.y) + (-(vd_asfloat(_GlobalSeeThroughHeightMin)))));
      r2.x = vd_asuint((1.0 / vd_asfloat(r2.x)));
      r2.x = vd_asuint(saturate((vd_asfloat(r2.x) * vd_asfloat(r2.z))));
      r2.z = vd_asuint((vd_asfloat(r2.x) * -2.0 + 3.0));
      r2.x = vd_asuint((vd_asfloat(r2.x) * vd_asfloat(r2.x)));
      r2.x = vd_asuint(((-(vd_asfloat(r2.z))) * vd_asfloat(r2.x) + 1.0));
      r2.x = vd_asuint(max(vd_asfloat(r1.w), vd_asfloat(r2.x)));
      r1.w = vd_asuint(max(vd_asfloat(r2.x), 0.00100000005));
    } else {
      r2.x = vd_asuint((-(int)(vd_asint(_BackgroundType) == 7)));
      r2.x = (r2.x | r4.z);
      if (r2.x != 0u) {
        r2.x = vd_asuint((vd_asfloat(_GlobalSeeThroughBuildingHeightMax) + (-(vd_asfloat(_GlobalSeeThroughBuildingHeightMin)))));
        r2.z = vd_asuint((vd_asfloat(v2.y) + (-(vd_asfloat(_GlobalSeeThroughBuildingHeightMin)))));
        r2.x = vd_asuint((1.0 / vd_asfloat(r2.x)));
        r2.x = vd_asuint(saturate((vd_asfloat(r2.x) * vd_asfloat(r2.z))));
        r2.z = vd_asuint((vd_asfloat(r2.x) * -2.0 + 3.0));
        r2.x = vd_asuint((vd_asfloat(r2.x) * vd_asfloat(r2.x)));
        r2.x = vd_asuint(((-(vd_asfloat(r2.z))) * vd_asfloat(r2.x) + 1.0));
        r2.z = vd_asuint(((-(vd_asfloat(_GlobalSeeThroughBuildingAlpha))) + 1.0));
        r2.x = vd_asuint((vd_asfloat(r2.x) * vd_asfloat(r2.z) + vd_asfloat(_GlobalSeeThroughBuildingAlpha)));
        r2.z = vd_asuint(((-(vd_asfloat(r2.x))) + 1.0));
        r2.x = vd_asuint((vd_asfloat(r1.w) * vd_asfloat(r2.z) + vd_asfloat(r2.x)));
        r2.z = vd_asuint((-(int)(0.125 < vd_asfloat(v2.y))));
        if (r2.z != 0u) {
          r5.xyzw = vd_asuint(_CharacterAreaMaskTexture.SampleLevel(sampler_CharacterAreaMaskTexture, vd_asfloat(r3.xz), 0.0).xyzw);
          r2.zw = vd_asuint(round(vd_asfloat(_WorldSpaceCameraPos.xz)));
          r2.zw = vd_asuint(((-(vd_asfloat(r2.zw))) + vd_asfloat(v2.xz)));
          r6.x = vd_asuint((-(vd_asfloat((VD_COL(unity_MatrixV, 0).z)))));
          r6.y = vd_asuint((-(vd_asfloat((VD_COL(unity_MatrixV, 2).z)))));
          r3.y = vd_asuint(dot(vd_asfloat(r6.xy), vd_asfloat(r6.xy)));
          r3.y = vd_asuint(rsqrt(vd_asfloat(r3.y)));
          r3.yw = vd_asuint((vd_asfloat(r3.yy) * vd_asfloat(r6.xy)));
          r2.z = vd_asuint(dot(vd_asfloat(r2.zw), vd_asfloat(r3.yw)));
          r2.z = vd_asuint(saturate((vd_asfloat(r2.z) * 0.00999999978)));
          r2.z = vd_asuint(((-(vd_asfloat(r2.z))) + vd_asfloat(r5.y)));
          r2.z = vd_asuint(saturate((vd_asfloat(r2.z) * 40.0)));
          r2.w = vd_asuint((vd_asfloat(r2.z) * -2.0 + 3.0));
          r2.z = vd_asuint((vd_asfloat(r2.z) * vd_asfloat(r2.z)));
          r2.z = vd_asuint((vd_asfloat(r2.z) * vd_asfloat(r2.w)));
          r2.w = vd_asuint((vd_asfloat(r5.x) + -1.0));
          r2.z = vd_asuint((vd_asfloat(r2.w) * vd_asfloat(r2.z)));
          r2.z = vd_asuint((vd_asfloat(_GlobalBackgroundSeeThroughEnable) * vd_asfloat(r2.z) + 1.0));
          r2.z = vd_asuint(min(vd_asfloat(r2.z), vd_asfloat(r2.x)));
          r5.xyz = vd_asuint((vd_asfloat(v2.xyz) + (-(vd_asfloat(_WorldSpaceCameraPos.xyz)))));
          r2.w = vd_asuint(((-(vd_asfloat(_WorldSpaceCameraPos.y))) + -0.150000006));
          r2.w = vd_asuint((vd_asfloat(r2.w) / vd_asfloat(r5.y)));
          r3.yw = vd_asuint((vd_asfloat(r5.xz) * vd_asfloat(r2.ww) + vd_asfloat(_WorldSpaceCameraPos.xz)));
          r3.yw = vd_asuint((vd_asfloat(r3.yw) + (-(vd_asfloat(_PlaneBoundMaxPos.xz)))));
          r4.xz = vd_asuint((vd_asfloat(_PlaneBoundMinPos.xz) + (-(vd_asfloat(_PlaneBoundMaxPos.xz)))));
          r3.yw = vd_asuint(saturate((vd_asfloat(r3.yw) / vd_asfloat(r4.xz))));
          r5.xyzw = vd_asuint(_PlaneSightRenderTarget.SampleLevel(sampler_PlaneSightRenderTarget, vd_asfloat(r3.yw), 0.0).xyzw);
          r2.w = vd_asuint((vd_asfloat(r5.w) + -1.0));
          r2.w = vd_asuint((vd_asfloat(_GlobalPlaneSystemEnable) * vd_asfloat(r2.w) + 1.0));
          r3.y = vd_asuint(((-(vd_asfloat(_SeeThroughDistanceMin))) + vd_asfloat(_SeeThroughDistanceMax)));
          r3.y = vd_asuint((1.0 / vd_asfloat(r3.y)));
          r3.w = 0x3f800000u;
          r4.x = 0x00000000u;
          while (true) {
            r4.z = vd_asuint((-(int)(vd_asint(r4.x) >= 3)));
            if (r4.z != 0u) break;
            r5.xyz = vd_asuint((vd_asfloat(v2.xyz) + (-(vd_asfloat(_SightPositions[(int)(r4.x)].xyz)))));
            r4.z = vd_asuint(dot(vd_asfloat(r5.xyz), vd_asfloat(r5.xyz)));
            r4.z = vd_asuint(sqrt(vd_asfloat(r4.z)));
            r4.z = vd_asuint((vd_asfloat(r4.z) + (-(vd_asfloat(_SeeThroughDistanceMin)))));
            r4.z = vd_asuint(saturate((vd_asfloat(r3.y) * vd_asfloat(r4.z))));
            r5.x = vd_asuint((vd_asfloat(r4.z) * -2.0 + 3.0));
            r4.z = vd_asuint((vd_asfloat(r4.z) * vd_asfloat(r4.z)));
            r4.z = vd_asuint((vd_asfloat(r4.z) * vd_asfloat(r5.x)));
            r3.w = vd_asuint(min(vd_asfloat(r3.w), vd_asfloat(r4.z)));
            r4.x = vd_asuint((vd_asint(r4.x) + 1));
          }
          r3.y = vd_asuint((vd_asfloat(_GlobalBackgroundSeeThroughEnable) * vd_asfloat(_GlobalPlaneSystemEnable)));
          r2.w = vd_asuint((vd_asfloat(r2.w) * vd_asfloat(r2.w)));
          r3.w = vd_asuint(((-(vd_asfloat(r3.w))) + 1.0));
          r2.w = vd_asuint(((-(vd_asfloat(r2.w))) * vd_asfloat(r3.w) + 1.0));
          r2.w = vd_asuint(max(vd_asfloat(r2.w), 0.0));
          r2.w = vd_asuint((vd_asfloat(r2.w) + -1.0));
          r2.w = vd_asuint((vd_asfloat(r3.y) * vd_asfloat(r2.w) + 1.0));
          r2.x = vd_asuint(min(vd_asfloat(r2.w), vd_asfloat(r2.z)));
        }
        r2.x = vd_asuint((vd_asfloat(r2.x) + -1.0));
        r1.w = vd_asuint((vd_asfloat(_GlobalIsRuntime) * vd_asfloat(r2.x) + 1.0));
        r2.xz = vd_asuint((vd_asfloat(_ScreenSize.xy) / vd_asfloat(_ScreenParams.xy)));
        r3.yw = vd_asuint((vd_asfloat(r3.xz) * vd_asfloat(_ScreenParams.xy)));
        r2.xz = vd_asuint((vd_asfloat(r2.xz) * vd_asfloat(r3.yw)));
        r2.xz = vd_asuint(floor(vd_asfloat(r2.xz)));
        r2.xz = ((uint2)vd_asfloat(r2.xz));
        r2.xz = (r2.xz & uint2(0x00000003u, 0x00000003u));
        r2.z = (r2.z << (uint)2);
        r2.x = vd_asuint((vd_asint(r2.z) + vd_asint(r2.x)));
        r2.x = x0[r2.x + 0].x;
        r2.x = vd_asuint((vd_asfloat(r1.w) + (-(vd_asfloat(r2.x)))));
        r2.x = vd_asuint((-(int)(vd_asfloat(r2.x) < 0.0)));
        if (r2.x != 0u) discard;
      } else {
        if (r4.y != 0u) {
          r5.xyzw = vd_asuint(_CharacterAreaMaskTexture.SampleLevel(sampler_CharacterAreaMaskTexture, vd_asfloat(r3.xz), 0.0).xyzw);
          r2.xz = vd_asuint(round(vd_asfloat(_WorldSpaceCameraPos.xz)));
          r2.xz = vd_asuint(((-(vd_asfloat(r2.xz))) + vd_asfloat(v2.xz)));
          r4.x = vd_asuint((-(vd_asfloat((VD_COL(unity_MatrixV, 0).z)))));
          r4.y = vd_asuint((-(vd_asfloat((VD_COL(unity_MatrixV, 2).z)))));
          r2.w = vd_asuint(dot(vd_asfloat(r4.xy), vd_asfloat(r4.xy)));
          r2.w = vd_asuint(rsqrt(vd_asfloat(r2.w)));
          r3.yw = vd_asuint((vd_asfloat(r2.ww) * vd_asfloat(r4.xy)));
          r2.x = vd_asuint(dot(vd_asfloat(r2.xz), vd_asfloat(r3.yw)));
          r2.x = vd_asuint(saturate((vd_asfloat(r2.x) * 0.00999999978)));
          r2.x = vd_asuint(((-(vd_asfloat(r2.x))) + vd_asfloat(r5.y)));
          r2.x = vd_asuint(saturate((vd_asfloat(r2.x) * 40.0)));
          r2.z = vd_asuint((vd_asfloat(r2.x) * -2.0 + 3.0));
          r2.x = vd_asuint((vd_asfloat(r2.x) * vd_asfloat(r2.x)));
          r2.x = vd_asuint((vd_asfloat(r2.x) * vd_asfloat(r2.z)));
          r2.z = vd_asuint((vd_asfloat(r5.x) + -1.0));
          r2.x = vd_asuint((vd_asfloat(r2.z) * vd_asfloat(r2.x)));
          r2.x = vd_asuint((vd_asfloat(_GlobalBackgroundSeeThroughEnable) * vd_asfloat(r2.x) + 1.0));
          r4.xyz = vd_asuint((vd_asfloat(v2.xyz) + (-(vd_asfloat(_WorldSpaceCameraPos.xyz)))));
          r5.xyzw = vd_asuint(((-(vd_asfloat(_WorldSpaceCameraPos.yyyy))) + float4(2.0, 2.0, 1.0, 1.0)));
          r5.xyzw = vd_asuint((vd_asfloat(r5.xyzw) / vd_asfloat(r4.yyyy)));
          r5.xyzw = vd_asuint((vd_asfloat(r4.xzxz) * vd_asfloat(r5.xyzw) + vd_asfloat(_WorldSpaceCameraPos.xzxz)));
          r5.xyzw = vd_asuint((vd_asfloat(r5.xyzw) + (-(vd_asfloat(_PlaneBoundMaxPos.xzxz)))));
          r2.zw = vd_asuint((vd_asfloat(_PlaneBoundMinPos.xz) + (-(vd_asfloat(_PlaneBoundMaxPos.xz)))));
          r5.xyzw = vd_asuint(saturate((vd_asfloat(r5.xyzw) / vd_asfloat(r2.zwzw))));
          r6.xyzw = vd_asuint(_PlaneSightRenderTarget.SampleLevel(sampler_PlaneSightRenderTarget, vd_asfloat(r5.xy), 0.0).xyzw);
          r3.y = vd_asuint((vd_asfloat(r6.w) + -1.0));
          r3.y = vd_asuint((vd_asfloat(_GlobalPlaneSystemEnable) * vd_asfloat(r3.y) + 1.0));
          r3.w = vd_asuint(((-(vd_asfloat(_SeeThroughDistanceMin))) + vd_asfloat(_SeeThroughDistanceMax)));
          r3.w = vd_asuint((1.0 / vd_asfloat(r3.w)));
          r5.xy = uint2(0x3f800000u, 0x00000000u);
          while (true) {
            r6.x = vd_asuint((-(int)(vd_asint(r5.y) >= 3)));
            if (r6.x != 0u) break;
            r6.xyz = vd_asuint((vd_asfloat(v2.xyz) + (-(vd_asfloat(_SightPositions[(int)(r5.y)].xyz)))));
            r6.x = vd_asuint(dot(vd_asfloat(r6.xyz), vd_asfloat(r6.xyz)));
            r6.x = vd_asuint(sqrt(vd_asfloat(r6.x)));
            r6.x = vd_asuint((vd_asfloat(r6.x) + (-(vd_asfloat(_SeeThroughDistanceMin)))));
            r6.x = vd_asuint(saturate((vd_asfloat(r3.w) * vd_asfloat(r6.x))));
            r6.y = vd_asuint((vd_asfloat(r6.x) * -2.0 + 3.0));
            r6.x = vd_asuint((vd_asfloat(r6.x) * vd_asfloat(r6.x)));
            r6.x = vd_asuint((vd_asfloat(r6.x) * vd_asfloat(r6.y)));
            r5.x = vd_asuint(min(vd_asfloat(r5.x), vd_asfloat(r6.x)));
            r5.y = vd_asuint((vd_asint(r5.y) + 1));
          }
          r5.y = vd_asuint((vd_asfloat(_GlobalBackgroundSeeThroughEnable) * vd_asfloat(_GlobalPlaneSystemEnable)));
          r3.y = vd_asuint((vd_asfloat(r3.y) * vd_asfloat(r3.y)));
          r5.x = vd_asuint(((-(vd_asfloat(r5.x))) + 1.0));
          r3.y = vd_asuint(((-(vd_asfloat(r3.y))) * vd_asfloat(r5.x) + 1.0));
          r3.y = vd_asuint(max(vd_asfloat(r3.y), 0.0));
          r3.y = vd_asuint((vd_asfloat(r3.y) + -1.0));
          r3.y = vd_asuint((vd_asfloat(r5.y) * vd_asfloat(r3.y) + 1.0));
          r6.xyzw = vd_asuint(_PlaneSightRenderTarget.SampleLevel(sampler_PlaneSightRenderTarget, vd_asfloat(r5.zw), 0.0).xyzw);
          r5.x = vd_asuint((vd_asfloat(r6.w) + -1.0));
          r5.x = vd_asuint((vd_asfloat(_GlobalPlaneSystemEnable) * vd_asfloat(r5.x) + 1.0));
          r5.zw = uint2(0x3f800000u, 0x00000000u);
          while (true) {
            r6.x = vd_asuint((-(int)(vd_asint(r5.w) >= 3)));
            if (r6.x != 0u) break;
            r6.xyz = vd_asuint((vd_asfloat(v2.xyz) + (-(vd_asfloat(_SightPositions[(int)(r5.w)].xyz)))));
            r6.x = vd_asuint(dot(vd_asfloat(r6.xyz), vd_asfloat(r6.xyz)));
            r6.x = vd_asuint(sqrt(vd_asfloat(r6.x)));
            r6.x = vd_asuint((vd_asfloat(r6.x) + (-(vd_asfloat(_SeeThroughDistanceMin)))));
            r6.x = vd_asuint(saturate((vd_asfloat(r3.w) * vd_asfloat(r6.x))));
            r6.y = vd_asuint((vd_asfloat(r6.x) * -2.0 + 3.0));
            r6.x = vd_asuint((vd_asfloat(r6.x) * vd_asfloat(r6.x)));
            r6.x = vd_asuint((vd_asfloat(r6.x) * vd_asfloat(r6.y)));
            r5.z = vd_asuint(min(vd_asfloat(r5.z), vd_asfloat(r6.x)));
            r5.w = vd_asuint((vd_asint(r5.w) + 1));
          }
          r5.x = vd_asuint((vd_asfloat(r5.x) * vd_asfloat(r5.x)));
          r5.z = vd_asuint(((-(vd_asfloat(r5.z))) + 1.0));
          r5.x = vd_asuint(((-(vd_asfloat(r5.x))) * vd_asfloat(r5.z) + 1.0));
          r5.x = vd_asuint(max(vd_asfloat(r5.x), 0.0));
          r5.x = vd_asuint((vd_asfloat(r5.x) + -1.0));
          r5.x = vd_asuint((vd_asfloat(r5.y) * vd_asfloat(r5.x) + 1.0));
          r4.y = vd_asuint(((-(vd_asfloat(_WorldSpaceCameraPos.y))) / vd_asfloat(r4.y)));
          r4.xy = vd_asuint((vd_asfloat(r4.xz) * vd_asfloat(r4.yy) + vd_asfloat(_WorldSpaceCameraPos.xz)));
          r4.xy = vd_asuint((vd_asfloat(r4.xy) + (-(vd_asfloat(_PlaneBoundMaxPos.xz)))));
          r2.zw = vd_asuint(saturate((vd_asfloat(r4.xy) / vd_asfloat(r2.zw))));
          r6.xyzw = vd_asuint(_PlaneSightRenderTarget.SampleLevel(sampler_PlaneSightRenderTarget, vd_asfloat(r2.zw), 0.0).xyzw);
          r2.z = vd_asuint((vd_asfloat(r6.w) + -1.0));
          r2.z = vd_asuint((vd_asfloat(_GlobalPlaneSystemEnable) * vd_asfloat(r2.z) + 1.0));
          r2.w = 0x3f800000u;
          r4.x = 0x00000000u;
          while (true) {
            r4.y = vd_asuint((-(int)(vd_asint(r4.x) >= 3)));
            if (r4.y != 0u) break;
            r6.xyz = vd_asuint((vd_asfloat(v2.xyz) + (-(vd_asfloat(_SightPositions[(int)(r4.x)].xyz)))));
            r4.y = vd_asuint(dot(vd_asfloat(r6.xyz), vd_asfloat(r6.xyz)));
            r4.y = vd_asuint(sqrt(vd_asfloat(r4.y)));
            r4.y = vd_asuint((vd_asfloat(r4.y) + (-(vd_asfloat(_SeeThroughDistanceMin)))));
            r4.y = vd_asuint(saturate((vd_asfloat(r3.w) * vd_asfloat(r4.y))));
            r4.z = vd_asuint((vd_asfloat(r4.y) * -2.0 + 3.0));
            r4.y = vd_asuint((vd_asfloat(r4.y) * vd_asfloat(r4.y)));
            r4.y = vd_asuint((vd_asfloat(r4.y) * vd_asfloat(r4.z)));
            r2.w = vd_asuint(min(vd_asfloat(r2.w), vd_asfloat(r4.y)));
            r4.x = vd_asuint((vd_asint(r4.x) + 1));
          }
          r2.z = vd_asuint((vd_asfloat(r2.z) * vd_asfloat(r2.z)));
          r2.w = vd_asuint(((-(vd_asfloat(r2.w))) + 1.0));
          r2.z = vd_asuint(((-(vd_asfloat(r2.z))) * vd_asfloat(r2.w) + 1.0));
          r2.z = vd_asuint(max(vd_asfloat(r2.z), 0.0));
          r2.z = vd_asuint((vd_asfloat(r2.z) + -1.0));
          r2.z = vd_asuint((vd_asfloat(r5.y) * vd_asfloat(r2.z) + 1.0));
          r2.z = vd_asuint(min(vd_asfloat(r2.z), vd_asfloat(r5.x)));
          r2.z = vd_asuint(min(vd_asfloat(r2.z), vd_asfloat(r3.y)));
          r2.x = vd_asuint(min(vd_asfloat(r2.z), vd_asfloat(r2.x)));
          r2.x = vd_asuint(min(vd_asfloat(r2.x), vd_asfloat(_GlobalCoverSeeThroughFactor)));
          r2.x = vd_asuint((vd_asfloat(r2.x) + -1.0));
          r1.w = vd_asuint((vd_asfloat(_GlobalIsRuntime) * vd_asfloat(r2.x) + 1.0));
          r2.xz = vd_asuint((vd_asfloat(_ScreenSize.xy) / vd_asfloat(_ScreenParams.xy)));
          r3.yw = vd_asuint((vd_asfloat(r3.xz) * vd_asfloat(_ScreenParams.xy)));
          r2.xz = vd_asuint((vd_asfloat(r2.xz) * vd_asfloat(r3.yw)));
          r2.xz = vd_asuint(floor(vd_asfloat(r2.xz)));
          r2.xz = ((uint2)vd_asfloat(r2.xz));
          r2.xz = (r2.xz & uint2(0x00000003u, 0x00000003u));
          r2.z = (r2.z << (uint)2);
          r2.x = vd_asuint((vd_asint(r2.z) + vd_asint(r2.x)));
          r2.x = x0[r2.x + 0].x;
          r2.x = vd_asuint((vd_asfloat(r1.w) + (-(vd_asfloat(r2.x)))));
          r2.x = vd_asuint((-(int)(vd_asfloat(r2.x) < 0.0)));
          if (r2.x != 0u) discard;
        }
      }
    }
    r2.x = vd_asuint(((-(vd_asfloat(_GlobalSeeThroughAlpha))) + 1.0));
    r1.w = vd_asuint((vd_asfloat(r1.w) * vd_asfloat(r2.x) + vd_asfloat(_GlobalSeeThroughAlpha)));
    r2.x = vd_asuint((vd_asfloat(r1.w) * vd_asfloat(v8.w)));
    r1.w = vd_asuint((vd_asfloat(r1.w) + -1.0));
    r1.w = vd_asuint((vd_asfloat(_GlobalIsRuntime) * vd_asfloat(r1.w) + 1.0));
    r1.w = ((r4.w != 0u) ? r2.x : r1.w);
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
  r0.y = vd_asuint((vd_asfloat(r4.y) * vd_asfloat((VD_COL(unity_MatrixV, 1).z))));
  r0.y = vd_asuint((vd_asfloat((VD_COL(unity_MatrixV, 0).z)) * vd_asfloat(r4.x) + vd_asfloat(r0.y)));
  r0.y = vd_asuint((vd_asfloat((VD_COL(unity_MatrixV, 2).z)) * vd_asfloat(r4.z) + vd_asfloat(r0.y)));
  r0.y = vd_asuint((vd_asfloat(r0.y) + vd_asfloat((VD_COL(unity_MatrixV, 3).z))));
  r0.y = vd_asuint(((-(vd_asfloat(r0.y))) + (-(vd_asfloat(_ProjectionParams.y)))));
  r0.y = vd_asuint(max(vd_asfloat(r0.y), 0.0));
  r0.y = vd_asuint(saturate((vd_asfloat(r0.y) * vd_asfloat(unity_FogParams.z) + vd_asfloat(unity_FogParams.w))));
  r4.xy = vd_asuint(((-(vd_asfloat(_GlobalOverallMapBoundMin.xz))) + vd_asfloat(_GlobalOverallMapBoundMax.xz)));
  r4.xy = vd_asuint(max(vd_asfloat(r4.xy), float2(1.0, 1.0)));
  r4.zw = vd_asuint((vd_asfloat(v2.xz) + (-(vd_asfloat(_GlobalOverallMapBoundMin.xz)))));
  r4.zw = vd_asuint((vd_asfloat(r4.zw) / vd_asfloat(r4.xy)));
  r5.xyzw = vd_asuint(_GlobalWaterAreaMap.SampleBias(sampler_vd_linear_clamp, vd_asfloat(r4.zw), vd_asfloat(_GlobalMipBias.x)).xyzw);
  r0.z = vd_asuint((-(int)(vd_asfloat(_WeatherIntensity) >= 0.00100000005)));
  r3.w = vd_asuint((vd_asfloat(r5.x) + 0.899999976));
  r3.w = vd_asuint(saturate((vd_asfloat(r3.w) * 0.5)));
  r5.x = vd_asuint((vd_asfloat(r3.w) * -2.0 + 3.0));
  r3.w = vd_asuint((vd_asfloat(r3.w) * vd_asfloat(r3.w)));
  r3.w = vd_asuint((vd_asfloat(r3.w) * vd_asfloat(r5.x)));
  r3.w = vd_asuint(max(vd_asfloat(r3.w), vd_asfloat(_WeatherIndoorMask)));
  r3.w = vd_asuint(((-(vd_asfloat(r3.w))) + 1.0));
  r3.w = vd_asuint((vd_asfloat(r3.w) * vd_asfloat(_WeatherIntensity)));
  r5.x = vd_asuint((-(int)(vd_asfloat(r3.w) >= 0.00100000005)));
  r5.y = vd_asuint((vd_asfloat(r3.w) * vd_asfloat(_WeatherWetSurfaceIntensity)));
  r5.y = vd_asuint((vd_asfloat(r5.y) * -0.300000012 + 1.0));
  r6.xyz = vd_asuint((vd_asfloat(r1.xyz) * vd_asfloat(r5.yyy)));
  r3.w = vd_asuint((vd_asfloat(r3.w) * vd_asfloat(_WeatherBgTintIntensity)));
  r5.z = vd_asuint((-(int)(0.00100000005 < vd_asfloat(r3.w))));
  r5.w = vd_asuint(dot(vd_asfloat(r6.xyz), float3(0.298999995, 0.587000012, 0.114)));
  r6.w = vd_asuint((vd_asfloat(r3.w) * vd_asfloat(_WeatherBgDesaturation)));
  r7.xyz = vd_asuint(((-(vd_asfloat(r1.xyz))) * vd_asfloat(r5.yyy) + vd_asfloat(r5.www)));
  r7.xyz = vd_asuint((vd_asfloat(r6.www) * vd_asfloat(r7.xyz) + vd_asfloat(r6.xyz)));
  r3.w = vd_asuint((vd_asfloat(r3.w) * 0.300000012));
  r8.xyz = vd_asuint(((-(vd_asfloat(r7.xyz))) + vd_asfloat(_WeatherBgTintColor.xyz)));
  r7.xyz = vd_asuint((vd_asfloat(r3.www) * vd_asfloat(r8.xyz) + vd_asfloat(r7.xyz)));
  r5.yzw = ((r5.zzz != 0u) ? r7.xyz : r6.xyz);
  r5.xyz = ((r5.xxx != 0u) ? r5.yzw : r1.xyz);
  r5.xyz = ((r0.zzz != 0u) ? r5.xyz : r1.xyz);
  if (vd_asuint(_GlobalWaterAreaEnable) != 0u) {
    r6.xyzw = vd_asuint(_GlobalWaterAreaMap.SampleLevel(sampler_vd_linear_clamp, vd_asfloat(r4.zw), 0.0).xyzw);
    r0.z = vd_asuint((-(int)(0.100000001 < vd_asfloat(r6.z))));
    if (r0.z != 0u) {
      r4.zw = vd_asuint((vd_asfloat(v2.xz) * vd_asfloat(_GlobalWaterDepthRange.ww)));
      r7.xyzw = vd_asuint(_GlobalWaterNoiseTex.SampleLevel(sampler_vd_linear_repeat, vd_asfloat(r4.zw), 0.0).xyzw);
      r0.z = vd_asuint((vd_asfloat(r7.y) + -0.5));
      r0.z = vd_asuint((vd_asfloat(r0.z) * vd_asfloat(_GlobalWaterDepthRange.z)));
      r3.w = vd_asuint((vd_asfloat(r6.z) * vd_asfloat(r0.z)));
      r4.z = vd_asuint((vd_asfloat(_GlobalWaterWaveParams.y) * 0.800000012));
      r4.w = vd_asuint((vd_asfloat(_Time.y) * vd_asfloat(_GlobalWaterWaveParams.x)));
      r6.xy = vd_asuint((vd_asfloat(r4.ww) * float2(0.24000001, -0.180000007)));
      r6.xy = vd_asuint((vd_asfloat(v2.xz) * vd_asfloat(r4.zz) + vd_asfloat(r6.xy)));
      r7.xyzw = vd_asuint(_GlobalWaterNoiseTex.SampleLevel(sampler_vd_linear_repeat, vd_asfloat(r6.xy), 0.0).xyzw);
      r4.z = vd_asuint((vd_asfloat(r6.z) + -0.5));
      r4.z = vd_asuint((abs(vd_asfloat(r4.z)) * 5.0));
      r4.z = vd_asuint(min(vd_asfloat(r4.z), 1.0));
      r4.z = vd_asuint(((-(vd_asfloat(r4.z))) + 1.0));
      r5.w = vd_asuint((vd_asfloat(r7.y) + -0.5));
      r3.w = vd_asuint((vd_asfloat(r5.w) * vd_asfloat(r4.z) + vd_asfloat(r3.w)));
      r3.w = vd_asuint((vd_asfloat(r3.w) + vd_asfloat(r6.z)));
      r4.z = vd_asuint(((-(vd_asfloat(_GlobalWaterShoreEdge.x))) + vd_asfloat(_GlobalWaterShoreEdge.y)));
      r5.w = vd_asuint((vd_asfloat(r3.w) + (-(vd_asfloat(_GlobalWaterShoreEdge.x)))));
      r4.z = vd_asuint((1.0 / vd_asfloat(r4.z)));
      r4.z = vd_asuint(saturate((vd_asfloat(r4.z) * vd_asfloat(r5.w))));
      r5.w = vd_asuint((vd_asfloat(r4.z) * -2.0 + 3.0));
      r4.z = vd_asuint((vd_asfloat(r4.z) * vd_asfloat(r4.z)));
      r4.z = vd_asuint((vd_asfloat(r4.z) * vd_asfloat(r5.w)));
      r6.xy = vd_asuint((vd_asfloat(_GlobalWaterShoreEdge.xy) + vd_asfloat((_GlobalWaterShoreStroke).xx)));
      r5.w = vd_asuint(((-(vd_asfloat(r6.x))) + vd_asfloat(r6.y)));
      r6.x = vd_asuint((vd_asfloat(r3.w) + (-(vd_asfloat(r6.x)))));
      r5.w = vd_asuint((1.0 / vd_asfloat(r5.w)));
      r5.w = vd_asuint(saturate((vd_asfloat(r5.w) * vd_asfloat(r6.x))));
      r6.x = vd_asuint((vd_asfloat(r5.w) * -2.0 + 3.0));
      r5.w = vd_asuint((vd_asfloat(r5.w) * vd_asfloat(r5.w)));
      r5.w = vd_asuint(((-(vd_asfloat(r6.x))) * vd_asfloat(r5.w) + 1.0));
      r0.z = vd_asuint((vd_asfloat(r0.z) * vd_asfloat(r6.z) + vd_asfloat(r6.z)));
      r6.x = vd_asuint(((-(vd_asfloat(_GlobalWaterDepthRange.x))) + vd_asfloat(_GlobalWaterDepthRange.y)));
      r0.z = vd_asuint((vd_asfloat(r0.z) + (-(vd_asfloat(_GlobalWaterDepthRange.x)))));
      r6.x = vd_asuint((1.0 / vd_asfloat(r6.x)));
      r0.z = vd_asuint(saturate((vd_asfloat(r0.z) * vd_asfloat(r6.x))));
      r6.y = vd_asuint((vd_asfloat(r0.z) * -2.0 + 3.0));
      r0.z = vd_asuint((vd_asfloat(r0.z) * vd_asfloat(r0.z)));
      r6.z = vd_asuint((vd_asfloat(r0.z) * vd_asfloat(r6.y)));
      r5.w = vd_asuint((vd_asfloat(r4.z) * vd_asfloat(r5.w)));
      r6.w = vd_asuint((-(int)(vd_asfloat(r4.z) >= 0.00999999978)));
      if (r6.w != 0u) {
        r7.xy = vd_asuint((-(int2)(float2(0.00999999978, 0.00999999978) < vd_asfloat(_GlobalWaterSurfaceWaveMapParams.zw))));
        r6.w = (r7.y & r7.x);
        if (r6.w != 0u) {
          r7.xy = vd_asuint((vd_asfloat(v2.xz) + (-(vd_asfloat(_GlobalWaterSurfaceWaveMapParams.xy)))));
          r7.xy = vd_asuint((vd_asfloat(r7.xy) / vd_asfloat(_GlobalWaterSurfaceWaveMapParams.zw)));
          r7.xy = vd_asuint((vd_asfloat(r7.xy) + float2(0.5, 0.5)));
          r7.zw = vd_asuint(((-(vd_asfloat(r7.xy))) + float2(1.0, 1.0)));
          r7.zw = vd_asuint(min(vd_asfloat(r7.zw), vd_asfloat(r7.xy)));
          r6.w = vd_asuint(min(vd_asfloat(r7.w), vd_asfloat(r7.z)));
          r6.w = vd_asuint(saturate((vd_asfloat(r6.w) * vd_asfloat(_GlobalWaterSurfaceWaveMapParams.z))));
          r7.xyzw = vd_asuint(_GlobalWaterSurfaceWaveMap.SampleLevel(sampler_vd_linear_clamp, vd_asfloat(r7.xy), 0.0).xyzw);
          r7.yz = vd_asuint((vd_asfloat(r7.yz) * float2(2.0, 2.0) + float2(-1.0, -1.0)));
          r7.xw = vd_asuint((vd_asfloat(r6.ww) * vd_asfloat(r7.wx)));
        } else {
          r7.xyzw = uint4(0x00000000u, 0x00000000u, 0x00000000u, 0x00000000u);
        }
        r0.z = vd_asuint(((-(vd_asfloat(r6.y))) * vd_asfloat(r0.z) + 1.0));
        r0.z = vd_asuint(((-(vd_asfloat(_GlobalWaterShoreHeightDrop))) * vd_asfloat(r0.z) + vd_asfloat(_GlobalWaterSurfaceLevel)));
        r0.z = vd_asuint((vd_asfloat(r7.w) * 0.5 + vd_asfloat(r0.z)));
        r6.y = vd_asuint((vd_asfloat(v2.y) + (-(vd_asfloat(_GlobalWaterDepthOffset)))));
        r0.z = vd_asuint(((-(vd_asfloat(r0.z))) + vd_asfloat(r6.y)));
        r6.y = vd_asuint(saturate((vd_asfloat(r0.z) * -2.0 + 0.5)));
        r8.xy = vd_asuint((vd_asfloat(v2.xz) * vd_asfloat(_GlobalWaterWaveParams.yy)));
        r9.xyz = vd_asuint((vd_asfloat(r4.www) * float3(0.707099974, -0.780000031, 0.519999981)));
        r8.zw = vd_asuint((vd_asfloat(v2.xz) * vd_asfloat(_GlobalWaterWaveParams.yy) + vd_asfloat(r9.xx)));
        r10.xyzw = vd_asuint(_GlobalWaterNoiseTex.SampleLevel(sampler_vd_linear_repeat, vd_asfloat(r8.zw), 0.0).xyzw);
        r11.xw = uint2(0x3e99999au, 0x3f333333u);
        r11.yz = vd_asuint(_GlobalWaterWaveParams.xx);
        r8.zw = vd_asuint((vd_asfloat(r11.xy) * vd_asfloat(_Time.yy)));
        r8.zw = vd_asuint((vd_asfloat(r11.zw) * vd_asfloat(r8.zw)));
        r8.zw = vd_asuint((vd_asfloat(r8.xy) * float2(1.5, 1.5) + vd_asfloat(r8.zw)));
        r9.xy = vd_asuint((vd_asfloat(r8.xy) * float2(2.0, 2.0) + vd_asfloat(r9.yz)));
        r11.xyzw = vd_asuint(_GlobalWaterNormalMap.SampleLevel(sampler_vd_linear_repeat, vd_asfloat(r8.zw), 0.0).xyzw);
        r10.yzw = vd_asuint((vd_asfloat(r11.xyz) * float3(2.0, 2.0, 2.0) + float3(-1.0, -1.0, -1.0)));
        r9.xyzw = vd_asuint(_GlobalWaterNormalMap.SampleLevel(sampler_vd_linear_repeat, vd_asfloat(r9.xy), 0.0).xyzw);
        r9.xyz = vd_asuint((vd_asfloat(r9.xyz) * float3(2.0, 2.0, 2.0) + float3(-1.0, -1.0, -1.0)));
        r11.xy = vd_asuint((vd_asfloat(r9.xy) + vd_asfloat(r10.yz)));
        r11.z = vd_asuint((vd_asfloat(r9.z) * vd_asfloat(r10.w)));
        r4.w = vd_asuint(dot(vd_asfloat(r11.xyz), vd_asfloat(r11.xyz)));
        r4.w = vd_asuint(rsqrt(vd_asfloat(r4.w)));
        r9.xyz = vd_asuint((vd_asfloat(r4.www) * vd_asfloat(r11.xyz)));
        r4.w = vd_asuint(saturate(dot(vd_asfloat(r9.xzy), float3(0.300000012, 0.300000012, 0.899999976))));
        r4.w = vd_asuint((vd_asfloat(r6.z) * vd_asfloat(r4.w)));
        r7.xy = vd_asuint((vd_asfloat(r7.xx) * vd_asfloat(r7.yz)));
        r6.w = vd_asuint(log2(abs(vd_asfloat(r10.x))));
        r6.w = vd_asuint((vd_asfloat(r6.w) * vd_asfloat(_GlobalWaterWaveParams.z)));
        r6.w = vd_asuint(exp2(vd_asfloat(r6.w)));
        r6.w = vd_asuint(min(vd_asfloat(r6.w), 1.0));
        r6.w = vd_asuint((vd_asfloat(r6.w) * vd_asfloat(_GlobalWaterWaveParams.w)));
        r5.w = vd_asuint((vd_asfloat(r5.w) * vd_asfloat(_GlobalWaterShoreIntensity)));
        r5.w = vd_asuint(max(vd_asfloat(r6.w), vd_asfloat(r5.w)));
        r5.w = vd_asuint((vd_asfloat(r5.w) * vd_asfloat(_GlobalWaterShallowColor.w)));
        r9.xyz = vd_asuint((vd_asfloat(_GlobalWaterShallowColor.xyz) + (-(vd_asfloat(_GlobalWaterDeepColor.xyz)))));
        r9.xyz = vd_asuint((vd_asfloat(r5.www) * vd_asfloat(r9.xyz) + vd_asfloat(_GlobalWaterDeepColor.xyz)));
        r8.zw = vd_asuint((vd_asfloat(r7.xy) * float2(0.5, 0.5) + vd_asfloat(v2.xz)));
        r8.zw = vd_asuint((vd_asfloat(r8.zw) + (-(vd_asfloat(_GlobalOverallMapBoundMin.xz)))));
        r4.xy = vd_asuint((vd_asfloat(r8.zw) / vd_asfloat(r4.xy)));
        r11.xyzw = vd_asuint(_GlobalWaterAreaMap.SampleLevel(sampler_vd_linear_clamp, vd_asfloat(r4.xy), 0.0).xyzw);
        r4.x = vd_asuint((vd_asfloat(r11.z) + (-(vd_asfloat(_GlobalWaterDepthRange.x)))));
        r4.x = vd_asuint(saturate((vd_asfloat(r6.x) * vd_asfloat(r4.x))));
        r4.y = vd_asuint((vd_asfloat(r4.x) * -2.0 + 3.0));
        r4.x = vd_asuint((vd_asfloat(r4.x) * vd_asfloat(r4.x)));
        r4.x = vd_asuint((vd_asfloat(r4.y) * vd_asfloat(r4.x) + (-(vd_asfloat(r6.z)))));
        r4.x = vd_asuint((abs(vd_asfloat(r7.w)) * abs(vd_asfloat(r4.x))));
        r4.xw = vd_asuint((vd_asfloat(r4.xw) * float2(3.0, 0.150000006)));
        r4.x = vd_asuint(min(vd_asfloat(r4.x), 1.0));
        r10.yzw = vd_asuint(((-(vd_asfloat(r9.xyz))) + vd_asfloat(_GlobalWaterShallowColor.xyz)));
        r9.xyz = vd_asuint((vd_asfloat(r4.xxx) * vd_asfloat(r10.yzw) + vd_asfloat(r9.xyz)));
        r10.yzw = vd_asuint(((-(vd_asfloat(r9.xyz))) + vd_asfloat(_GlobalWaterShallowColor.xyz)));
        r4.xyw = vd_asuint((vd_asfloat(r4.www) * vd_asfloat(r10.yzw) + vd_asfloat(r9.xyz)));
        r5.w = vd_asuint((-(int)(vd_asfloat(r0.z) >= 0.0)));
        r5.w = (r5.w & 0x3f800000u);
        r6.x = vd_asuint((1.0 / vd_asfloat(_GlobalWaterSurfaceEdgeWidth)));
        r0.z = vd_asuint(saturate((vd_asfloat(r0.z) * vd_asfloat(r6.x))));
        r6.x = vd_asuint((vd_asfloat(r0.z) * -2.0 + 3.0));
        r0.z = vd_asuint((vd_asfloat(r0.z) * vd_asfloat(r0.z)));
        r0.z = vd_asuint(((-(vd_asfloat(r6.x))) * vd_asfloat(r0.z) + 1.0));
        r6.x = vd_asuint((vd_asfloat(r5.w) * vd_asfloat(_GlobalWaterShallowColor.w)));
        r6.x = vd_asuint((vd_asfloat(r4.z) * vd_asfloat(r6.x)));
        r6.x = vd_asuint((vd_asfloat(r6.x) * 0.5));
        r9.xyz = vd_asuint(((-(vd_asfloat(r4.xyw))) + vd_asfloat(_GlobalWaterShallowColor.xyz)));
        r4.xyw = vd_asuint((vd_asfloat(r6.xxx) * vd_asfloat(r9.xyz) + vd_asfloat(r4.xyw)));
        r6.x = vd_asuint((-(int)(vd_asint(_PlaneReflectionActive) != 0)));
        r6.w = vd_asuint((-(int)(0.00999999978 < vd_asfloat(_GlobalWaterReflectionIntensity))));
        r6.x = (r6.w & r6.x);
        if (r6.x != 0u) {
          r9.y = 0xbf000000u;
          r9.z = vd_asuint(_GlobalWaterWaveParams.x);
          r9.yz = vd_asuint((vd_asfloat(r9.yz) * vd_asfloat(_Time.yy)));
          r9.x = vd_asuint((vd_asfloat(r9.y) * vd_asfloat(_GlobalWaterWaveParams.x)));
          r6.xw = vd_asuint((vd_asfloat(r9.xz) * float2(0.800000012, 0.692799985)));
          r6.xw = vd_asuint((vd_asfloat(r8.xy) * float2(0.699999988, 0.699999988) + vd_asfloat(r6.xw)));
          r8.xyzw = vd_asuint(_GlobalWaterNoiseTex.SampleLevel(sampler_vd_linear_repeat, vd_asfloat(r6.xw), 0.0).yxzw);
          r8.x = r10.x;
          r6.xw = vd_asuint((vd_asfloat(r8.xy) + float2(-0.5, -0.5)));
          r6.xw = vd_asuint((vd_asfloat(r6.xw) + vd_asfloat(r6.xw)));
          r6.xw = vd_asuint((vd_asfloat(r6.xw) * vd_asfloat(_GlobalWaterWaveParams2.xx)));
          r7.zw = vd_asuint((vd_asfloat(v7.xy) / vd_asfloat(_ScreenParams.xy)));
          r6.xw = vd_asuint((vd_asfloat(r6.xw) * float2(0.5, 0.5) + vd_asfloat(r7.zw)));
          r6.xw = vd_asuint((vd_asfloat(r7.xy) * float2(0.0199999996, 0.0199999996) + vd_asfloat(r6.xw)));
          r6.w = vd_asuint((vd_asfloat(r6.w) + vd_asfloat(_GlobalWaterReflectionYOffset)));
          r7.xyzw = vd_asuint(_PlaneReflectionTexture.SampleLevel(sampler_vd_linear_clamp, vd_asfloat(r6.xw), 0.0).xyzw);
          r6.x = vd_asuint((vd_asfloat(r6.z) * 0.699999988 + 0.300000012));
          r6.x = vd_asuint((vd_asfloat(r4.z) * vd_asfloat(r6.x)));
          r6.x = vd_asuint((vd_asfloat(r6.x) * vd_asfloat(_GlobalWaterReflectionIntensity)));
          r6.x = vd_asuint((vd_asfloat(r6.x) * vd_asfloat(_PlaneReflectionOpacity)));
          r6.xzw = vd_asuint((vd_asfloat(r6.xxx) * vd_asfloat(r7.xyz)));
          r7.x = vd_asuint(((-(vd_asfloat(r0.z))) * vd_asfloat(r5.w) + 1.0));
          r4.xyw = vd_asuint((vd_asfloat(r6.xzw) * vd_asfloat(r7.xxx) + vd_asfloat(r4.xyw)));
        }
        r5.w = vd_asuint(saturate((vd_asfloat(r0.z) * vd_asfloat(r5.w) + vd_asfloat(r6.y))));
        r4.z = vd_asuint((vd_asfloat(r4.z) * vd_asfloat(_GlobalWaterDeepColor.w)));
        r3.w = vd_asuint(rsqrt(vd_asfloat(r3.w)));
        r3.w = vd_asuint((1.0 / vd_asfloat(r3.w)));
        r3.w = vd_asuint((vd_asfloat(r3.w) * vd_asfloat(r4.z)));
        r3.w = vd_asuint((vd_asfloat(r5.w) * vd_asfloat(r3.w)));
        r0.z = vd_asuint((vd_asfloat(r0.z) * vd_asfloat(r3.w)));
        r4.xyz = vd_asuint(((-(vd_asfloat(r5.xyz))) + vd_asfloat(r4.xyw)));
        r5.xyz = vd_asuint((vd_asfloat(r0.zzz) * vd_asfloat(r4.xyz) + vd_asfloat(r5.xyz)));
      }
    }
  }
  r0.z = (vd_asuint(_MainLightLayerMask) & vd_asuint(unity_RenderingLayer.x));
  r3.w = vd_asuint(sqrt(vd_asfloat(unity_LightData.z)));
  r3.w = vd_asuint(min(vd_asfloat(r3.w), 1.0));
  r2.x = vd_asuint(dot(vd_asfloat(r2.xzw), vd_asfloat(_MainLightPosition.xyz)));
  r2.x = vd_asuint(saturate((vd_asfloat(r2.x) * 0.5 + 0.5)));
  r4.xyz = vd_asuint((vd_asfloat(_MainLightColor.xyz) * vd_asfloat(r3.www) + (-(vd_asfloat(_GlobalBackgroundShadowColor.xyz)))));
  r2.xzw = vd_asuint((vd_asfloat(r4.xyz) * vd_asfloat(r2.xxx) + vd_asfloat(_GlobalBackgroundShadowColor.xyz)));
  r2.xzw = vd_asuint((vd_asfloat(r5.xyz) * vd_asfloat(r2.xzw)));
  r2.xzw = ((r0.zzz != 0u) ? r2.xzw : uint3(0x00000000u, 0x00000000u, 0x00000000u));
  r1.xyz = vd_asuint((vd_asfloat(r1.www) * vd_asfloat(r1.xyz) + vd_asfloat(r2.xzw)));
  r4.x = vd_asuint(dot(float2(0.530324996, 0.530324996), vd_asfloat(v2.xz)));
  r4.y = vd_asuint(dot(float3(-0.353500009, 0.865999997, 0.353500009), vd_asfloat(v2.xyz)));
  r2.xz = vd_asuint((vd_asfloat(r4.xy) * float2(0.125, 0.125)));
  r5.xyzw = vd_asuint(_DirtNoiseTexture.SampleBias(sampler_vd_point_repeat, vd_asfloat(r2.xz), vd_asfloat(_GlobalMipBias.x)).xyzw);
  r0.z = vd_asuint((vd_asfloat(r5.x) * 0.5 + 0.5));
  r1.w = vd_asuint((vd_asfloat(r0.z) * vd_asfloat(r0.z)));
  r0.z = vd_asuint(((-(vd_asfloat(r0.z))) + 1.0));
  r0.z = vd_asuint((vd_asfloat(r0.z) * vd_asfloat(r0.z)));
  r2.xzw = vd_asuint((vd_asfloat(r1.xyz) * vd_asfloat(r1.xyz)));
  r2.xzw = vd_asuint(min(vd_asfloat(r2.xzw), float3(1.0, 1.0, 1.0)));
  r1.w = vd_asuint((vd_asfloat(r1.w) * vd_asfloat(_DirtNoiseColor.w)));
  r1.w = vd_asuint((vd_asfloat(r1.w) * vd_asfloat(_UseDirtNoise)));
  r2.xzw = vd_asuint(((-(vd_asfloat(r1.xyz))) + vd_asfloat(r2.xzw)));
  r1.xyz = vd_asuint((vd_asfloat(r1.www) * vd_asfloat(r2.xzw) + vd_asfloat(r1.xyz)));
  r2.xzw = vd_asuint(saturate((vd_asfloat(r1.xyz) + vd_asfloat(r1.xyz))));
  r0.z = vd_asuint((vd_asfloat(r0.z) * vd_asfloat(_DirtNoiseColor.w)));
  r0.z = vd_asuint((vd_asfloat(r0.z) * vd_asfloat(_UseDirtNoise)));
  r2.xzw = vd_asuint(((-(vd_asfloat(r1.xyz))) + vd_asfloat(r2.xzw)));
  r1.xyz = vd_asuint((vd_asfloat(r0.zzz) * vd_asfloat(r2.xzw) + vd_asfloat(r1.xyz)));
  if (r0.x != 0u) {
    r0.xz = vd_asuint((vd_asfloat(r4.xy) * float2(0.400000006, 0.400000006)));
    r5.xyzw = vd_asuint(_DirtNoiseTexture.SampleBias(sampler_vd_point_repeat, vd_asfloat(r0.xz), vd_asfloat(_GlobalMipBias.x)).xyzw);
    r6.xyzw = vd_asuint((vd_asfloat(_Time.yyyy) * float4(1.5, 1.5, 0.800000012, 0.800000012) + vd_asfloat(v2.zxzx)));
    r0.xz = vd_asuint(sin(vd_asfloat(r6.xw)));
    r2.xz = vd_asuint(cos(vd_asfloat(r6.yz)));
    r6.zw = vd_asuint((vd_asfloat(r2.xz) * float2(0.0250000004, 0.0250000004)));
    r6.xy = vd_asuint((vd_asfloat(r0.xz) * float2(0.0250000004, 0.0250000004)));
    r0.xz = vd_asuint((vd_asfloat(r4.xy) * float2(0.150000006, 0.150000006) + vd_asfloat(r6.xz)));
    r7.xyzw = vd_asuint(_DirtNoiseTexture.SampleBias(sampler_vd_point_repeat, vd_asfloat(r0.xz), vd_asfloat(_GlobalMipBias.x)).xyzw);
    r0.xz = vd_asuint((vd_asfloat(r4.xy) * float2(0.1875, 0.1875) + vd_asfloat(r6.wy)));
    r4.xyzw = vd_asuint(_DirtNoiseTexture.SampleBias(sampler_vd_point_repeat, vd_asfloat(r0.xz), vd_asfloat(_GlobalMipBias.x)).xyzw);
    r0.x = vd_asuint((vd_asfloat(r4.x) + vd_asfloat(r7.x)));
    r0.x = vd_asuint(saturate((vd_asfloat(r0.x) * 0.5)));
    r0.x = vd_asuint(rsqrt(vd_asfloat(r0.x)));
    r0.x = vd_asuint((1.0 / vd_asfloat(r0.x)));
    r0.z = vd_asuint((-(int)(vd_asfloat(_GlobalBoxAreaIntensities[0]) >= 0.00100000005)));
    if (r0.z != 0u) {
      r0.z = vd_asuint(((int)vd_asfloat(_GlobalBoxAreaCenters[0].w)));
      r2.xz = vd_asuint((-(int2)(vd_asint(r0.zz) == int2(1, 4))));
      r1.w = (r2.z | r2.x);
      if (r1.w == 0u) {
        r2.xzw = vd_asuint((vd_asfloat(v2.xyz) + (-(vd_asfloat(_GlobalBoxAreaCenters[0].xyz)))));
        r2.xzw = vd_asuint(((-(abs(vd_asfloat(r2.xzw)))) + vd_asfloat(_GlobalBoxAreaExtents[0].xyz)));
        r1.w = vd_asuint(min(vd_asfloat(r2.w), vd_asfloat(r2.z)));
        r1.w = vd_asuint(min(vd_asfloat(r1.w), vd_asfloat(r2.x)));
        r2.x = vd_asuint(max(vd_asfloat(_GlobalBoxAreaExtents[0].w), 0.00100000005));
        r1.w = vd_asuint(saturate((vd_asfloat(r1.w) / vd_asfloat(r2.x))));
        r2.xzw = vd_asuint(log2(abs(vd_asfloat(_GlobalBoxAreaColors[0].xyz))));
        r2.xzw = vd_asuint((vd_asfloat(r2.xzw) * float3(2.20000005, 2.20000005, 2.20000005)));
        r2.xzw = vd_asuint(exp2(vd_asfloat(r2.xzw)));
        r0.z = vd_asuint((-(int)(vd_asint(r0.z) == 3)));
        if (r0.z != 0u) {
          r0.z = vd_asuint((vd_asfloat(r0.x) + -1.0));
          r0.z = vd_asuint((vd_asfloat(_GlobalBoxAreaShimmerParams[0].z) * vd_asfloat(r0.z) + 1.0));
        } else {
          r0.z = 0x3f800000u;
        }
        r3.w = vd_asuint((-(int)(vd_asfloat(_GlobalBoxAreaIntensities[0]) >= vd_asfloat(r5.x))));
        r3.w = (r3.w & 0x3f800000u);
        r1.w = vd_asuint((vd_asfloat(r1.w) * vd_asfloat(r3.w)));
        r1.w = vd_asuint((vd_asfloat(r1.w) * vd_asfloat(_GlobalBoxAreaColors[0].w)));
        r0.z = vd_asuint((vd_asfloat(r0.z) * vd_asfloat(r1.w)));
        r2.xzw = vd_asuint(((-(vd_asfloat(r1.xyz))) + vd_asfloat(r2.xzw)));
        r1.xyz = vd_asuint((vd_asfloat(r0.zzz) * vd_asfloat(r2.xzw) + vd_asfloat(r1.xyz)));
      }
    }
    r0.z = vd_asuint((-(int)(vd_asfloat(_GlobalBoxAreaIntensities[1]) >= 0.00100000005)));
    if (r0.z != 0u) {
      r0.z = vd_asuint(((int)vd_asfloat(_GlobalBoxAreaCenters[1].w)));
      r2.xz = vd_asuint((-(int2)(vd_asint(r0.zz) == int2(1, 4))));
      r1.w = (r2.z | r2.x);
      if (r1.w == 0u) {
        r2.xzw = vd_asuint((vd_asfloat(v2.xyz) + (-(vd_asfloat(_GlobalBoxAreaCenters[1].xyz)))));
        r2.xzw = vd_asuint(((-(abs(vd_asfloat(r2.xzw)))) + vd_asfloat(_GlobalBoxAreaExtents[1].xyz)));
        r1.w = vd_asuint(min(vd_asfloat(r2.w), vd_asfloat(r2.z)));
        r1.w = vd_asuint(min(vd_asfloat(r1.w), vd_asfloat(r2.x)));
        r2.x = vd_asuint(max(vd_asfloat(_GlobalBoxAreaExtents[1].w), 0.00100000005));
        r1.w = vd_asuint(saturate((vd_asfloat(r1.w) / vd_asfloat(r2.x))));
        r2.xzw = vd_asuint(log2(abs(vd_asfloat(_GlobalBoxAreaColors[1].xyz))));
        r2.xzw = vd_asuint((vd_asfloat(r2.xzw) * float3(2.20000005, 2.20000005, 2.20000005)));
        r2.xzw = vd_asuint(exp2(vd_asfloat(r2.xzw)));
        r0.z = vd_asuint((-(int)(vd_asint(r0.z) == 3)));
        if (r0.z != 0u) {
          r0.z = vd_asuint((vd_asfloat(r0.x) + -1.0));
          r0.z = vd_asuint((vd_asfloat(_GlobalBoxAreaShimmerParams[1].z) * vd_asfloat(r0.z) + 1.0));
        } else {
          r0.z = 0x3f800000u;
        }
        r3.w = vd_asuint((-(int)(vd_asfloat(_GlobalBoxAreaIntensities[1]) >= vd_asfloat(r5.x))));
        r3.w = (r3.w & 0x3f800000u);
        r1.w = vd_asuint((vd_asfloat(r1.w) * vd_asfloat(r3.w)));
        r1.w = vd_asuint((vd_asfloat(r1.w) * vd_asfloat(_GlobalBoxAreaColors[1].w)));
        r0.z = vd_asuint((vd_asfloat(r0.z) * vd_asfloat(r1.w)));
        r2.xzw = vd_asuint(((-(vd_asfloat(r1.xyz))) + vd_asfloat(r2.xzw)));
        r1.xyz = vd_asuint((vd_asfloat(r0.zzz) * vd_asfloat(r2.xzw) + vd_asfloat(r1.xyz)));
      }
    }
    r0.z = vd_asuint((-(int)(vd_asfloat(_GlobalBoxAreaIntensities[2]) >= 0.00100000005)));
    if (r0.z != 0u) {
      r0.z = vd_asuint(((int)vd_asfloat(_GlobalBoxAreaCenters[2].w)));
      r2.xz = vd_asuint((-(int2)(vd_asint(r0.zz) == int2(1, 4))));
      r1.w = (r2.z | r2.x);
      if (r1.w == 0u) {
        r2.xzw = vd_asuint((vd_asfloat(v2.xyz) + (-(vd_asfloat(_GlobalBoxAreaCenters[2].xyz)))));
        r2.xzw = vd_asuint(((-(abs(vd_asfloat(r2.xzw)))) + vd_asfloat(_GlobalBoxAreaExtents[2].xyz)));
        r1.w = vd_asuint(min(vd_asfloat(r2.w), vd_asfloat(r2.z)));
        r1.w = vd_asuint(min(vd_asfloat(r1.w), vd_asfloat(r2.x)));
        r2.x = vd_asuint(max(vd_asfloat(_GlobalBoxAreaExtents[2].w), 0.00100000005));
        r1.w = vd_asuint(saturate((vd_asfloat(r1.w) / vd_asfloat(r2.x))));
        r2.xzw = vd_asuint(log2(abs(vd_asfloat(_GlobalBoxAreaColors[2].xyz))));
        r2.xzw = vd_asuint((vd_asfloat(r2.xzw) * float3(2.20000005, 2.20000005, 2.20000005)));
        r2.xzw = vd_asuint(exp2(vd_asfloat(r2.xzw)));
        r0.z = vd_asuint((-(int)(vd_asint(r0.z) == 3)));
        if (r0.z != 0u) {
          r0.z = vd_asuint((vd_asfloat(r0.x) + -1.0));
          r0.z = vd_asuint((vd_asfloat(_GlobalBoxAreaShimmerParams[2].z) * vd_asfloat(r0.z) + 1.0));
        } else {
          r0.z = 0x3f800000u;
        }
        r3.w = vd_asuint((-(int)(vd_asfloat(_GlobalBoxAreaIntensities[2]) >= vd_asfloat(r5.x))));
        r3.w = (r3.w & 0x3f800000u);
        r1.w = vd_asuint((vd_asfloat(r1.w) * vd_asfloat(r3.w)));
        r1.w = vd_asuint((vd_asfloat(r1.w) * vd_asfloat(_GlobalBoxAreaColors[2].w)));
        r0.z = vd_asuint((vd_asfloat(r0.z) * vd_asfloat(r1.w)));
        r2.xzw = vd_asuint(((-(vd_asfloat(r1.xyz))) + vd_asfloat(r2.xzw)));
        r1.xyz = vd_asuint((vd_asfloat(r0.zzz) * vd_asfloat(r2.xzw) + vd_asfloat(r1.xyz)));
      }
    }
    r0.z = vd_asuint((-(int)(vd_asfloat(_GlobalBoxAreaIntensities[3]) >= 0.00100000005)));
    if (r0.z != 0u) {
      r0.z = vd_asuint(((int)vd_asfloat(_GlobalBoxAreaCenters[3].w)));
      r2.xz = vd_asuint((-(int2)(vd_asint(r0.zz) == int2(1, 4))));
      r1.w = (r2.z | r2.x);
      if (r1.w == 0u) {
        r2.xzw = vd_asuint((vd_asfloat(v2.xyz) + (-(vd_asfloat(_GlobalBoxAreaCenters[3].xyz)))));
        r2.xzw = vd_asuint(((-(abs(vd_asfloat(r2.xzw)))) + vd_asfloat(_GlobalBoxAreaExtents[3].xyz)));
        r1.w = vd_asuint(min(vd_asfloat(r2.w), vd_asfloat(r2.z)));
        r1.w = vd_asuint(min(vd_asfloat(r1.w), vd_asfloat(r2.x)));
        r2.x = vd_asuint(max(vd_asfloat(_GlobalBoxAreaExtents[3].w), 0.00100000005));
        r1.w = vd_asuint(saturate((vd_asfloat(r1.w) / vd_asfloat(r2.x))));
        r2.xzw = vd_asuint(log2(abs(vd_asfloat(_GlobalBoxAreaColors[3].xyz))));
        r2.xzw = vd_asuint((vd_asfloat(r2.xzw) * float3(2.20000005, 2.20000005, 2.20000005)));
        r2.xzw = vd_asuint(exp2(vd_asfloat(r2.xzw)));
        r0.z = vd_asuint((-(int)(vd_asint(r0.z) == 3)));
        if (r0.z != 0u) {
          r0.z = vd_asuint((vd_asfloat(r0.x) + -1.0));
          r0.z = vd_asuint((vd_asfloat(_GlobalBoxAreaShimmerParams[3].z) * vd_asfloat(r0.z) + 1.0));
        } else {
          r0.z = 0x3f800000u;
        }
        r3.w = vd_asuint((-(int)(vd_asfloat(_GlobalBoxAreaIntensities[3]) >= vd_asfloat(r5.x))));
        r3.w = (r3.w & 0x3f800000u);
        r1.w = vd_asuint((vd_asfloat(r1.w) * vd_asfloat(r3.w)));
        r1.w = vd_asuint((vd_asfloat(r1.w) * vd_asfloat(_GlobalBoxAreaColors[3].w)));
        r0.z = vd_asuint((vd_asfloat(r0.z) * vd_asfloat(r1.w)));
        r2.xzw = vd_asuint(((-(vd_asfloat(r1.xyz))) + vd_asfloat(r2.xzw)));
        r1.xyz = vd_asuint((vd_asfloat(r0.zzz) * vd_asfloat(r2.xzw) + vd_asfloat(r1.xyz)));
      }
    }
    r0.z = vd_asuint((-(int)(vd_asfloat(_GlobalBoxAreaIntensities[4]) >= 0.00100000005)));
    if (r0.z != 0u) {
      r0.z = vd_asuint(((int)vd_asfloat(_GlobalBoxAreaCenters[4].w)));
      r2.xz = vd_asuint((-(int2)(vd_asint(r0.zz) == int2(1, 4))));
      r1.w = (r2.z | r2.x);
      if (r1.w == 0u) {
        r2.xzw = vd_asuint((vd_asfloat(v2.xyz) + (-(vd_asfloat(_GlobalBoxAreaCenters[4].xyz)))));
        r2.xzw = vd_asuint(((-(abs(vd_asfloat(r2.xzw)))) + vd_asfloat(_GlobalBoxAreaExtents[4].xyz)));
        r1.w = vd_asuint(min(vd_asfloat(r2.w), vd_asfloat(r2.z)));
        r1.w = vd_asuint(min(vd_asfloat(r1.w), vd_asfloat(r2.x)));
        r2.x = vd_asuint(max(vd_asfloat(_GlobalBoxAreaExtents[4].w), 0.00100000005));
        r1.w = vd_asuint(saturate((vd_asfloat(r1.w) / vd_asfloat(r2.x))));
        r2.xzw = vd_asuint(log2(abs(vd_asfloat(_GlobalBoxAreaColors[4].xyz))));
        r2.xzw = vd_asuint((vd_asfloat(r2.xzw) * float3(2.20000005, 2.20000005, 2.20000005)));
        r2.xzw = vd_asuint(exp2(vd_asfloat(r2.xzw)));
        r0.z = vd_asuint((-(int)(vd_asint(r0.z) == 3)));
        if (r0.z != 0u) {
          r0.z = vd_asuint((vd_asfloat(r0.x) + -1.0));
          r0.z = vd_asuint((vd_asfloat(_GlobalBoxAreaShimmerParams[4].z) * vd_asfloat(r0.z) + 1.0));
        } else {
          r0.z = 0x3f800000u;
        }
        r3.w = vd_asuint((-(int)(vd_asfloat(_GlobalBoxAreaIntensities[4]) >= vd_asfloat(r5.x))));
        r3.w = (r3.w & 0x3f800000u);
        r1.w = vd_asuint((vd_asfloat(r1.w) * vd_asfloat(r3.w)));
        r1.w = vd_asuint((vd_asfloat(r1.w) * vd_asfloat(_GlobalBoxAreaColors[4].w)));
        r0.z = vd_asuint((vd_asfloat(r0.z) * vd_asfloat(r1.w)));
        r2.xzw = vd_asuint(((-(vd_asfloat(r1.xyz))) + vd_asfloat(r2.xzw)));
        r1.xyz = vd_asuint((vd_asfloat(r0.zzz) * vd_asfloat(r2.xzw) + vd_asfloat(r1.xyz)));
      }
    }
    r0.z = vd_asuint((-(int)(vd_asfloat(_GlobalBoxAreaIntensities[5]) >= 0.00100000005)));
    if (r0.z != 0u) {
      r0.z = vd_asuint(((int)vd_asfloat(_GlobalBoxAreaCenters[5].w)));
      r2.xz = vd_asuint((-(int2)(vd_asint(r0.zz) == int2(1, 4))));
      r1.w = (r2.z | r2.x);
      if (r1.w == 0u) {
        r2.xzw = vd_asuint((vd_asfloat(v2.xyz) + (-(vd_asfloat(_GlobalBoxAreaCenters[5].xyz)))));
        r2.xzw = vd_asuint(((-(abs(vd_asfloat(r2.xzw)))) + vd_asfloat(_GlobalBoxAreaExtents[5].xyz)));
        r1.w = vd_asuint(min(vd_asfloat(r2.w), vd_asfloat(r2.z)));
        r1.w = vd_asuint(min(vd_asfloat(r1.w), vd_asfloat(r2.x)));
        r2.x = vd_asuint(max(vd_asfloat(_GlobalBoxAreaExtents[5].w), 0.00100000005));
        r1.w = vd_asuint(saturate((vd_asfloat(r1.w) / vd_asfloat(r2.x))));
        r2.xzw = vd_asuint(log2(abs(vd_asfloat(_GlobalBoxAreaColors[5].xyz))));
        r2.xzw = vd_asuint((vd_asfloat(r2.xzw) * float3(2.20000005, 2.20000005, 2.20000005)));
        r2.xzw = vd_asuint(exp2(vd_asfloat(r2.xzw)));
        r0.z = vd_asuint((-(int)(vd_asint(r0.z) == 3)));
        if (r0.z != 0u) {
          r0.z = vd_asuint((vd_asfloat(r0.x) + -1.0));
          r0.z = vd_asuint((vd_asfloat(_GlobalBoxAreaShimmerParams[5].z) * vd_asfloat(r0.z) + 1.0));
        } else {
          r0.z = 0x3f800000u;
        }
        r3.w = vd_asuint((-(int)(vd_asfloat(_GlobalBoxAreaIntensities[5]) >= vd_asfloat(r5.x))));
        r3.w = (r3.w & 0x3f800000u);
        r1.w = vd_asuint((vd_asfloat(r1.w) * vd_asfloat(r3.w)));
        r1.w = vd_asuint((vd_asfloat(r1.w) * vd_asfloat(_GlobalBoxAreaColors[5].w)));
        r0.z = vd_asuint((vd_asfloat(r0.z) * vd_asfloat(r1.w)));
        r2.xzw = vd_asuint(((-(vd_asfloat(r1.xyz))) + vd_asfloat(r2.xzw)));
        r1.xyz = vd_asuint((vd_asfloat(r0.zzz) * vd_asfloat(r2.xzw) + vd_asfloat(r1.xyz)));
      }
    }
    r0.z = vd_asuint((-(int)(vd_asfloat(_GlobalBoxAreaIntensities[6]) >= 0.00100000005)));
    if (r0.z != 0u) {
      r0.z = vd_asuint(((int)vd_asfloat(_GlobalBoxAreaCenters[6].w)));
      r2.xz = vd_asuint((-(int2)(vd_asint(r0.zz) == int2(1, 4))));
      r1.w = (r2.z | r2.x);
      if (r1.w == 0u) {
        r2.xzw = vd_asuint((vd_asfloat(v2.xyz) + (-(vd_asfloat(_GlobalBoxAreaCenters[6].xyz)))));
        r2.xzw = vd_asuint(((-(abs(vd_asfloat(r2.xzw)))) + vd_asfloat(_GlobalBoxAreaExtents[6].xyz)));
        r1.w = vd_asuint(min(vd_asfloat(r2.w), vd_asfloat(r2.z)));
        r1.w = vd_asuint(min(vd_asfloat(r1.w), vd_asfloat(r2.x)));
        r2.x = vd_asuint(max(vd_asfloat(_GlobalBoxAreaExtents[6].w), 0.00100000005));
        r1.w = vd_asuint(saturate((vd_asfloat(r1.w) / vd_asfloat(r2.x))));
        r2.xzw = vd_asuint(log2(abs(vd_asfloat(_GlobalBoxAreaColors[6].xyz))));
        r2.xzw = vd_asuint((vd_asfloat(r2.xzw) * float3(2.20000005, 2.20000005, 2.20000005)));
        r2.xzw = vd_asuint(exp2(vd_asfloat(r2.xzw)));
        r0.z = vd_asuint((-(int)(vd_asint(r0.z) == 3)));
        if (r0.z != 0u) {
          r0.z = vd_asuint((vd_asfloat(r0.x) + -1.0));
          r0.z = vd_asuint((vd_asfloat(_GlobalBoxAreaShimmerParams[6].z) * vd_asfloat(r0.z) + 1.0));
        } else {
          r0.z = 0x3f800000u;
        }
        r3.w = vd_asuint((-(int)(vd_asfloat(_GlobalBoxAreaIntensities[6]) >= vd_asfloat(r5.x))));
        r3.w = (r3.w & 0x3f800000u);
        r1.w = vd_asuint((vd_asfloat(r1.w) * vd_asfloat(r3.w)));
        r1.w = vd_asuint((vd_asfloat(r1.w) * vd_asfloat(_GlobalBoxAreaColors[6].w)));
        r0.z = vd_asuint((vd_asfloat(r0.z) * vd_asfloat(r1.w)));
        r2.xzw = vd_asuint(((-(vd_asfloat(r1.xyz))) + vd_asfloat(r2.xzw)));
        r1.xyz = vd_asuint((vd_asfloat(r0.zzz) * vd_asfloat(r2.xzw) + vd_asfloat(r1.xyz)));
      }
    }
    r0.z = vd_asuint((-(int)(vd_asfloat(_GlobalBoxAreaIntensities[7]) >= 0.00100000005)));
    r1.w = vd_asuint(((int)vd_asfloat(_GlobalBoxAreaCenters[7].w)));
    r2.xzw = vd_asuint((-(int3)(vd_asint(r1.www) == int3(1, 4, 3))));
    r1.w = (r2.z | r2.x);
    r4.xyz = vd_asuint((vd_asfloat(v2.xyz) + (-(vd_asfloat(_GlobalBoxAreaCenters[7].xyz)))));
    r4.xyz = vd_asuint(((-(abs(vd_asfloat(r4.xyz)))) + vd_asfloat(_GlobalBoxAreaExtents[7].xyz)));
    r2.x = vd_asuint(min(vd_asfloat(r4.z), vd_asfloat(r4.y)));
    r2.x = vd_asuint(min(vd_asfloat(r2.x), vd_asfloat(r4.x)));
    r2.z = vd_asuint(max(vd_asfloat(_GlobalBoxAreaExtents[7].w), 0.00100000005));
    r2.x = vd_asuint(saturate((vd_asfloat(r2.x) / vd_asfloat(r2.z))));
    r4.xyz = vd_asuint(log2(abs(vd_asfloat(_GlobalBoxAreaColors[7].xyz))));
    r4.xyz = vd_asuint((vd_asfloat(r4.xyz) * float3(2.20000005, 2.20000005, 2.20000005)));
    r4.xyz = vd_asuint(exp2(vd_asfloat(r4.xyz)));
    if (r2.w != 0u) {
      r0.x = vd_asuint((vd_asfloat(r0.x) + -1.0));
      r0.x = vd_asuint((vd_asfloat(_GlobalBoxAreaShimmerParams[7].z) * vd_asfloat(r0.x) + 1.0));
    } else {
      r0.x = 0x3f800000u;
    }
    r2.z = vd_asuint((-(int)(vd_asfloat(_GlobalBoxAreaIntensities[7]) >= vd_asfloat(r5.x))));
    r2.z = (r2.z & 0x3f800000u);
    r2.x = vd_asuint((vd_asfloat(r2.z) * vd_asfloat(r2.x)));
    r2.x = vd_asuint((vd_asfloat(r2.x) * vd_asfloat(_GlobalBoxAreaColors[7].w)));
    r0.x = vd_asuint((vd_asfloat(r0.x) * vd_asfloat(r2.x)));
    r2.xzw = vd_asuint(((-(vd_asfloat(r1.xyz))) + vd_asfloat(r4.xyz)));
    r2.xzw = vd_asuint((vd_asfloat(r0.xxx) * vd_asfloat(r2.xzw) + vd_asfloat(r1.xyz)));
    r2.xzw = ((r1.www != 0u) ? r1.xyz : r2.xzw);
    r1.xyz = ((r0.zzz != 0u) ? r2.xzw : r1.xyz);
  }
  r0.x = vd_asuint((-(int)(0.0 < vd_asfloat(_DissolveProgress))));
  if (r0.x != 0u) {
    r0.x = vd_asuint((vd_asfloat(_DissolveProgress) + vd_asfloat(_DissolveProgress)));
    r0.x = vd_asuint(saturate(vd_asfloat(r0.x)));
    r0.z = vd_asuint(saturate((vd_asfloat(_DissolveProgress) * 2.0 + -1.0)));
    r1.w = vd_asuint(((-(vd_asfloat(r2.y))) + 1.0));
    r1.w = vd_asuint(((-(vd_asfloat(r0.x))) + vd_asfloat(r1.w)));
    r1.w = vd_asuint(saturate((vd_asfloat(r1.w) * 4.0)));
    r2.x = vd_asuint((vd_asfloat(r1.w) * -2.0 + 3.0));
    r1.w = vd_asuint((vd_asfloat(r1.w) * vd_asfloat(r1.w)));
    r1.w = vd_asuint(((-(vd_asfloat(r2.x))) * vd_asfloat(r1.w) + 1.0));
    r0.x = vd_asuint((vd_asfloat(r0.x) * vd_asfloat(r1.w)));
    r0.x = vd_asuint((vd_asfloat(r0.x) * vd_asfloat(_DissolveEdgeColor.w)));
    r2.xyz = vd_asuint(((-(vd_asfloat(r1.xyz))) + vd_asfloat(_DissolveEdgeColor.xyz)));
    r1.xyz = vd_asuint((vd_asfloat(r0.xxx) * vd_asfloat(r2.xyz) + vd_asfloat(r1.xyz)));
    r2.xy = vd_asuint((float2(1.0, 1.0) / vd_asfloat(_ScaledScreenParams.xy)));
    r2.xy = vd_asuint((vd_asfloat(r2.xy) * vd_asfloat(v7.xy)));
#if UNITY_UV_STARTS_AT_TOP
    r0.x = vd_asuint((vd_asfloat(r2.y) * vd_asfloat(_ScaleBiasRt.x) + vd_asfloat(_ScaleBiasRt.y)));
    r2.z = vd_asuint(((-(vd_asfloat(r0.x))) + 1.0));
#else
    r0.x = r2.y;
    r2.z = r0.x;
#endif
    r0.x = vd_asuint(((-(vd_asfloat(r0.z))) + 1.0));
    r2.yw = vd_asuint((vd_asfloat(_ScreenSize.xy) / vd_asfloat(_ScreenParams.xy)));
    r2.xz = vd_asuint((vd_asfloat(r2.xz) * vd_asfloat(_ScreenParams.xy)));
    r2.xy = vd_asuint((vd_asfloat(r2.yw) * vd_asfloat(r2.xz)));
    r2.xy = vd_asuint(floor(vd_asfloat(r2.xy)));
    r2.xy = ((uint2)vd_asfloat(r2.xy));
    r2.xy = (r2.xy & uint2(0x00000003u, 0x00000003u));
    r0.z = (r2.y << (uint)2);
    r0.z = vd_asuint((vd_asint(r0.z) + vd_asint(r2.x)));
    r0.z = x0[r0.z + 0].x;
    r0.x = vd_asuint(((-(vd_asfloat(r0.z))) + vd_asfloat(r0.x)));
    r0.x = vd_asuint((-(int)(vd_asfloat(r0.x) < 0.0)));
    if (r0.x != 0u) discard;
  }
  r0.x = vd_asuint(((-(vd_asfloat(r0.y))) + 1.0));
  r0.y = vd_asuint((vd_asfloat(v2.y) + (-(vd_asfloat(_GlobalFogHeightMin)))));
  r0.z = vd_asuint(((-(vd_asfloat(_GlobalFogHeightMin))) + vd_asfloat(_GlobalFogHeightMax)));
  r0.y = vd_asuint((vd_asfloat(r0.y) / vd_asfloat(r0.z)));
  r0.y = vd_asuint(saturate(((-(vd_asfloat(r0.y))) + 1.0)));
  r0.y = vd_asuint((vd_asfloat(r0.y) * vd_asfloat(_GlobalFogHeightEnable)));
  r0.x = vd_asuint(max(vd_asfloat(r0.y), vd_asfloat(r0.x)));
  r2.xyz = vd_asuint(((-(vd_asfloat(r1.xyz))) + vd_asfloat(unity_FogColor.xyz)));
  r0.xyz = vd_asuint((vd_asfloat(r0.xxx) * vd_asfloat(r2.xyz) + vd_asfloat(r1.xyz)));
  r1.x = vd_asuint((-(int)(vd_asfloat(_Surface) == 1.0)));
  o0.w = vd_asfloat(((r1.x != 0u) ? r0.w : 0x3f800000u));
  r1.xyz = vd_asuint((vd_asfloat(r3.xyz) * vd_asfloat((_EmissionIntensity).xxx)));
  r0.w = vd_asuint((vd_asfloat(_Time.y) + vd_asfloat(_Time.y)));
  r1.w = vd_asuint(dot(vd_asfloat(v8.xyz), float3(0.298999995, 0.587000012, 0.114)));
  r2.x = vd_asuint(dot(vd_asfloat(v10.xyz), float3(12.9898005, 78.2330017, 45.1640015)));
  r2.x = vd_asuint(frac(vd_asfloat(r2.x)));
  r1.w = vd_asuint((vd_asfloat(r1.w) + vd_asfloat(r2.x)));
  r2.xyzw = vd_asuint((vd_asfloat(r1.wwww) * float4(91.3000031, 37.0999985, 53.7000008, 3.0)));
  r2.xyz = vd_asuint(sin(vd_asfloat(r2.xyz)));
  r2.xyz = vd_asuint((vd_asfloat(r2.xyz) * float3(4337.5, 1572.90002, 9281.2002)));
  r2.xyz = vd_asuint(frac(vd_asfloat(r2.xyz)));
  r2.xyz = vd_asuint((vd_asfloat(r2.xyz) * float3(4.0, 6.0, 12.0) + float3(3.70000005, 7.19999981, 15.3000002)));
  r3.xyz = vd_asuint((vd_asfloat(r1.www) * float3(6.28000021, 3.1400001, 4.71000004)));
  r2.xyz = vd_asuint((vd_asfloat(r0.www) * vd_asfloat(r2.xyz) + vd_asfloat(r3.xyz)));
  r2.xyz = vd_asuint(sin(vd_asfloat(r2.xyz)));
  r2.xyz = vd_asuint((vd_asfloat(r2.xyz) * float3(0.5, 0.5, 0.5) + float3(0.5, 0.5, 0.5)));
  r0.w = vd_asuint((vd_asfloat(_Time.y) * 1.60000002));
  r0.w = vd_asuint((vd_asfloat(r1.w) * 2.0 + vd_asfloat(r0.w)));
  r0.w = vd_asuint(sin(vd_asfloat(r0.w)));
  r2.w = vd_asuint((vd_asfloat(_Time.y) * 2.5999999 + vd_asfloat(r2.w)));
  r2.w = vd_asuint(sin(vd_asfloat(r2.w)));
  r0.w = vd_asuint((vd_asfloat(r0.w) * vd_asfloat(r2.w)));
  r0.w = vd_asuint((vd_asfloat(r0.w) * 0.5 + 0.5));
  r1.w = vd_asuint((vd_asfloat(_Time.y) * 27.3999996 + vd_asfloat(r1.w)));
  r1.w = vd_asuint(sin(vd_asfloat(r1.w)));
  r1.w = vd_asuint((vd_asfloat(r1.w) * 43758.5469));
  r1.w = vd_asuint(frac(vd_asfloat(r1.w)));
  r1.w = vd_asuint((-(int)(vd_asfloat(r1.w) >= 0.970000029)));
  r2.x = vd_asuint(dot(vd_asfloat(r2.xyz), float3(0.150000006, 0.150000006, 0.200000003)));
  r0.w = vd_asuint((vd_asfloat(r0.w) * 0.300000012 + vd_asfloat(r2.x)));
  r1.w = (r1.w & 0x3e4ccccdu);
  r0.w = vd_asuint((vd_asfloat(r0.w) + vd_asfloat(r1.w)));
  r0.w = vd_asuint((vd_asfloat(r0.w) + -0.75));
  r0.w = vd_asuint(saturate((vd_asfloat(r0.w) * -9.99999809)));
  r1.w = vd_asuint((vd_asfloat(r0.w) * -2.0 + 3.0));
  r0.w = vd_asuint((vd_asfloat(r0.w) * vd_asfloat(r0.w)));
  r0.w = vd_asuint((vd_asfloat(r1.w) * vd_asfloat(r0.w) + -1.0));
  r0.w = vd_asuint((vd_asfloat(_EmissionFlickRate) * vd_asfloat(r0.w) + 1.0));
  r0.xyz = vd_asuint((vd_asfloat(r1.xyz) * vd_asfloat(r0.www) + vd_asfloat(r0.xyz)));
  r1.xyz = vd_asuint(((-(vd_asfloat(r0.xyz))) + vd_asfloat(_GlobalBackgroundColor.xyz)));
  o0.xyz = (vd_asfloat((_GlobalBackgroundColorIntensity).xxx) * vd_asfloat(r1.xyz) + vd_asfloat(r0.xyz));
  return;
}
