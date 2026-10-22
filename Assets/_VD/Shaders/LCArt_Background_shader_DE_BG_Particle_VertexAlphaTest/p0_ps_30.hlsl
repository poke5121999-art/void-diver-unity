// LCArt/Background/shader_DE_BG_Particle_VertexAlphaTest | pass 0 "ForwardLit" | progFragment | keywords: FOG_LINEAR _CLUSTER_LIGHT_LOOP _DE_EDITOR_CAMERA _LIGHT_LAYERS | blob 30
void frag(
  float2 v0 : TEXCOORD0,
  float w0 : TEXCOORD8,
  float4 v1 : TEXCOORD11,
  float4 v2 : TEXCOORD1,
  float4 v3 : TEXCOORD2,
  float4 v4 : TEXCOORD3,
  float4 v5 : TEXCOORD4,
  float3 v6 : TEXCOORD7,
  float4 v7 : SV_POSITION0,
  float4 v8 : COLOR0,
  float4 v9 : TEXCOORD9,
  float4 v10 : TEXCOORD5,
  float3 v11 : TEXCOORD10,
  out float4 o0 : SV_Target0)
{
  float4 r0,r1,r2,r3,r4,r5,r6,r7,r8,r9,r10,r11,r12;
  uint4 bitmask, uiDest;
  float4 fDest;

  float4 x0[16];
  x0[0].x = 0.058823999;
  x0[1].x = 0.529411972;
  x0[2].x = 0.176470995;
  x0[3].x = 0.647059023;
  x0[4].x = 0.764706016;
  x0[5].x = 0.294117987;
  x0[6].x = 0.882353008;
  x0[7].x = 0.411765009;
  x0[8].x = 0.235293999;
  x0[9].x = 0.705882013;
  x0[10].x = 0.117647;
  x0[11].x = 0.588235021;
  x0[12].x = 0.941175997;
  x0[13].x = 0.470587999;
  x0[14].x = 0.823529005;
  x0[15].x = 0.352941006;
  r0.x = cmp(_GlobalBoxAreaEnable >= 0.5);
  if (r0.x != 0) {
    r1.x = dot(float2(0.530324996,0.530324996), v2.xz);
    r1.y = dot(float3(-0.353500009,0.865999997,0.353500009), v2.xyz);
    r2.xw = float2(0.300000012,0.230000004) * _Time.yy;
    r2.yz = float2(0,0);
    r1.xyzw = r1.xyxy * float4(0.125,0.125,0.125,0.125) + r2.xyzw;
    r2.xyzw = _DirtNoiseTexture.SampleBias(sampler_vd_point_repeat, r1.xy, _GlobalMipBias.x).xyzw;
    r1.xyzw = _DirtNoiseTexture.SampleBias(sampler_vd_point_repeat, r1.zw, _GlobalMipBias.x).yxzw;
    r0.y = (int)_GlobalBoxAreaCenters[0].w;
    r0.z = cmp(_GlobalBoxAreaIntensities[0] >= 0.00100000005);
    r0.yw = cmp((int2)r0.yy == int2(1,2));
    r0.y = (int)r0.w | (int)r0.y;
    r0.y = r0.y ? r0.z : 0;
    r2.yzw = -_GlobalBoxAreaCenters[0].xyz + v2.xyz;
    r2.yzw = _GlobalBoxAreaExtents[0].xyz + -abs(r2.yzw);
    r0.z = min(r2.z, r2.w);
    r0.z = min(r2.y, r0.z);
    r0.w = max(0.00100000005, _GlobalBoxAreaExtents[0].w);
    r0.z = saturate(r0.z / r0.w);
    r1.x = r2.x;
    r1.xy = float2(-0.5,-0.5) + r1.xy;
    r1.xy = r1.xy + r1.xy;
    r1.zw = _GlobalBoxAreaShimmerParams[0].yy * r1.xy;
    r0.zw = r1.zw * r0.zz;
    r0.zw = (_GlobalBoxAreaIntensities[0]).xx * r0.zw;
    r0.yz = r0.yy ? r0.zw : 0;
    r0.w = (int)_GlobalBoxAreaCenters[1].w;
    r1.z = cmp(_GlobalBoxAreaIntensities[1] >= 0.00100000005);
    r2.xy = cmp((int2)r0.ww == int2(1,2));
    r0.w = (int)r2.y | (int)r2.x;
    r0.w = r1.z ? r0.w : 0;
    r2.xyz = -_GlobalBoxAreaCenters[1].xyz + v2.xyz;
    r2.xyz = _GlobalBoxAreaExtents[1].xyz + -abs(r2.xyz);
    r1.z = min(r2.y, r2.z);
    r1.z = min(r2.x, r1.z);
    r1.w = max(0.00100000005, _GlobalBoxAreaExtents[1].w);
    r1.z = saturate(r1.z / r1.w);
    r2.xy = _GlobalBoxAreaShimmerParams[1].yy * r1.xy;
    r1.zw = r2.xy * r1.zz;
    r1.zw = r1.zw * (_GlobalBoxAreaIntensities[1]).xx + r0.yz;
    r0.yz = r0.ww ? r1.zw : r0.yz;
    r0.w = (int)_GlobalBoxAreaCenters[2].w;
    r1.z = cmp(_GlobalBoxAreaIntensities[2] >= 0.00100000005);
    r2.xy = cmp((int2)r0.ww == int2(1,2));
    r0.w = (int)r2.y | (int)r2.x;
    r0.w = r1.z ? r0.w : 0;
    r2.xyz = -_GlobalBoxAreaCenters[2].xyz + v2.xyz;
    r2.xyz = _GlobalBoxAreaExtents[2].xyz + -abs(r2.xyz);
    r1.z = min(r2.y, r2.z);
    r1.z = min(r2.x, r1.z);
    r1.w = max(0.00100000005, _GlobalBoxAreaExtents[2].w);
    r1.z = saturate(r1.z / r1.w);
    r2.xy = _GlobalBoxAreaShimmerParams[2].yy * r1.xy;
    r1.zw = r2.xy * r1.zz;
    r1.zw = r1.zw * (_GlobalBoxAreaIntensities[2]).xx + r0.yz;
    r0.yz = r0.ww ? r1.zw : r0.yz;
    r0.w = (int)_GlobalBoxAreaCenters[3].w;
    r1.z = cmp(_GlobalBoxAreaIntensities[3] >= 0.00100000005);
    r2.xy = cmp((int2)r0.ww == int2(1,2));
    r0.w = (int)r2.y | (int)r2.x;
    r0.w = r1.z ? r0.w : 0;
    r2.xyz = -_GlobalBoxAreaCenters[3].xyz + v2.xyz;
    r2.xyz = _GlobalBoxAreaExtents[3].xyz + -abs(r2.xyz);
    r1.z = min(r2.y, r2.z);
    r1.z = min(r2.x, r1.z);
    r1.w = max(0.00100000005, _GlobalBoxAreaExtents[3].w);
    r1.z = saturate(r1.z / r1.w);
    r2.xy = _GlobalBoxAreaShimmerParams[3].yy * r1.xy;
    r1.zw = r2.xy * r1.zz;
    r1.zw = r1.zw * (_GlobalBoxAreaIntensities[3]).xx + r0.yz;
    r0.yz = r0.ww ? r1.zw : r0.yz;
    r0.w = (int)_GlobalBoxAreaCenters[4].w;
    r1.z = cmp(_GlobalBoxAreaIntensities[4] >= 0.00100000005);
    r2.xy = cmp((int2)r0.ww == int2(1,2));
    r0.w = (int)r2.y | (int)r2.x;
    r0.w = r1.z ? r0.w : 0;
    r2.xyz = -_GlobalBoxAreaCenters[4].xyz + v2.xyz;
    r2.xyz = _GlobalBoxAreaExtents[4].xyz + -abs(r2.xyz);
    r1.z = min(r2.y, r2.z);
    r1.z = min(r2.x, r1.z);
    r1.w = max(0.00100000005, _GlobalBoxAreaExtents[4].w);
    r1.z = saturate(r1.z / r1.w);
    r2.xy = _GlobalBoxAreaShimmerParams[4].yy * r1.xy;
    r1.zw = r2.xy * r1.zz;
    r1.zw = r1.zw * (_GlobalBoxAreaIntensities[4]).xx + r0.yz;
    r0.yz = r0.ww ? r1.zw : r0.yz;
    r0.w = (int)_GlobalBoxAreaCenters[5].w;
    r1.z = cmp(_GlobalBoxAreaIntensities[5] >= 0.00100000005);
    r2.xy = cmp((int2)r0.ww == int2(1,2));
    r0.w = (int)r2.y | (int)r2.x;
    r0.w = r1.z ? r0.w : 0;
    r2.xyz = -_GlobalBoxAreaCenters[5].xyz + v2.xyz;
    r2.xyz = _GlobalBoxAreaExtents[5].xyz + -abs(r2.xyz);
    r1.z = min(r2.y, r2.z);
    r1.z = min(r2.x, r1.z);
    r1.w = max(0.00100000005, _GlobalBoxAreaExtents[5].w);
    r1.z = saturate(r1.z / r1.w);
    r2.xy = _GlobalBoxAreaShimmerParams[5].yy * r1.xy;
    r1.zw = r2.xy * r1.zz;
    r1.zw = r1.zw * (_GlobalBoxAreaIntensities[5]).xx + r0.yz;
    r0.yz = r0.ww ? r1.zw : r0.yz;
    r0.w = (int)_GlobalBoxAreaCenters[6].w;
    r1.z = cmp(_GlobalBoxAreaIntensities[6] >= 0.00100000005);
    r2.xy = cmp((int2)r0.ww == int2(1,2));
    r0.w = (int)r2.y | (int)r2.x;
    r0.w = r1.z ? r0.w : 0;
    r2.xyz = -_GlobalBoxAreaCenters[6].xyz + v2.xyz;
    r2.xyz = _GlobalBoxAreaExtents[6].xyz + -abs(r2.xyz);
    r1.z = min(r2.y, r2.z);
    r1.z = min(r2.x, r1.z);
    r1.w = max(0.00100000005, _GlobalBoxAreaExtents[6].w);
    r1.z = saturate(r1.z / r1.w);
    r2.xy = _GlobalBoxAreaShimmerParams[6].yy * r1.xy;
    r1.zw = r2.xy * r1.zz;
    r1.zw = r1.zw * (_GlobalBoxAreaIntensities[6]).xx + r0.yz;
    r0.yz = r0.ww ? r1.zw : r0.yz;
    r0.w = (int)_GlobalBoxAreaCenters[7].w;
    r1.z = cmp(_GlobalBoxAreaIntensities[7] >= 0.00100000005);
    r2.xy = cmp((int2)r0.ww == int2(1,2));
    r0.w = (int)r2.y | (int)r2.x;
    r0.w = r1.z ? r0.w : 0;
    r2.xyz = -_GlobalBoxAreaCenters[7].xyz + v2.xyz;
    r2.xyz = _GlobalBoxAreaExtents[7].xyz + -abs(r2.xyz);
    r1.z = min(r2.y, r2.z);
    r1.z = min(r2.x, r1.z);
    r1.w = max(0.00100000005, _GlobalBoxAreaExtents[7].w);
    r1.z = saturate(r1.z / r1.w);
    r1.xy = _GlobalBoxAreaShimmerParams[7].yy * r1.xy;
    r1.xy = r1.xy * r1.zz;
    r1.xy = r1.xy * (_GlobalBoxAreaIntensities[7]).xx + r0.yz;
    r0.yz = r0.ww ? r1.xy : r0.yz;
  } else {
    r0.yz = float2(0,0);
  }
  r0.yz = v0.xy + r0.yz;
  r1.xyzw = _BaseMap.SampleBias(sampler_BaseMap, r0.yz, _GlobalMipBias.x).xyzw;
  r2.xyzw = _EmissionMap.SampleLevel(sampler_vd_point_repeat, r0.yz, 0).xyzw;
  r0.w = 0.100000001 + -r2.z;
  r0.w = cmp(r0.w < 0);
  if (r0.w != 0) discard;
  r0.w = cmp(r1.w < 0.5);
  if (r0.w != 0) discard;
  r2.xy = float2(1,1) / _ScaledScreenParams.xy;
  r2.xy = v7.xy * r2.xy;
  r0.w = r2.y * _ScaleBiasRt.x + _ScaleBiasRt.y;
  r2.z = 1 + -r0.w;
  r0.w = 1 + -_GlobalSeeThroughAlpha;
  r0.w = w0.x * r0.w + _GlobalSeeThroughAlpha;
  r2.y = saturate(dot(v8.xyz, float3(0.298999995,0.587000012,0.114)));
  r0.w = r2.y * r0.w;
  r2.yw = _ScreenSize.xy / _ScreenParams.xy;
  r2.xz = _ScreenParams.xy * r2.xz;
  r2.xy = r2.xz * r2.yw;
  r2.xy = floor(r2.xy);
  r2.xy = (uint2)r2.xy;
  r2.xy = (int2)r2.xy & int2(3,3);
  r2.y = (uint)r2.y << 2;
  r2.x = (int)r2.y + (int)r2.x;
  r2.x = x0[r2.x+0].x;
  r0.w = r0.w * _BaseColor.w + -r2.x;
  r0.w = cmp(r0.w < 0);
  if (r0.w != 0) discard;
  o0.w = _BaseColor.w * r1.w;
  r1.xyz = _BaseColor.xyz * r1.xyz;
  r2.xyzw = _BumpMap.SampleBias(sampler_vd_point_repeat, r0.yz, _GlobalMipBias.x).xyzw;
  r2.x = r2.w * r2.x;
  r3.xy = r2.xy + r2.xy;
  r2.xy = r2.xy * float2(2,2) + float2(-1,-1);
  r0.w = dot(r2.xy, r2.xy);
  r0.w = min(1, r0.w);
  r0.w = 1 + -r0.w;
  r0.w = sqrt(r0.w);
  r3.z = max(1.00000002e-016, r0.w);
  r2.xyz = float3(-1,-1,-1) + r3.xyz;
  r2.xyz = (_BumpScale).xxx * r2.xyz + float3(0,0,1);
  r3.xyzw = _EmissionMap.SampleBias(sampler_vd_point_repeat, r0.yz, _GlobalMipBias.x).xyzw;
  r0.w = cmp(r3.z >= 0.100000001);
  r0.w = r0.w ? 0 : 1;
  r0.w = r3.x * r0.w;
  r3.xyz = r0.www * r1.xyz;
  r1.w = cmp(0.5 < _UseWorldNormalMap);
  if (r1.w != 0) {
    r4.xyzw = _WorldNormalMap.SampleBias(sampler_vd_point_repeat, r0.yz, _GlobalMipBias.x).xyzw;
    r4.xyz = r4.xyz * float3(2,2,2) + float3(-1,-1,-1);
    r0.y = dot(r4.xyz, r4.xyz);
    r0.y = rsqrt(r0.y);
    r4.xyz = r4.xyz * r0.yyy;
    r0.y = dot(v4.xyz, r4.xyz);
    r5.xyz = -r4.xyz * r0.yyy + v4.xyz;
    r0.y = dot(r5.xyz, r5.xyz);
    r0.y = rsqrt(r0.y);
    r5.xyz = r5.xyz * r0.yyy;
    r0.y = dot(v5.xyz, r4.xyz);
    r6.xyz = -r4.xyz * r0.yyy + v5.xyz;
    r0.y = dot(r6.xyz, r6.xyz);
    r0.y = rsqrt(r0.y);
    r6.xyz = r6.xyz * r0.yyy;
    r6.xyz = r6.xyz * r2.yyy;
    r5.xyz = r2.xxx * r5.xyz + r6.xyz;
    r4.xyz = r2.zzz * r4.xyz + r5.xyz;
  } else {
    r5.xyz = v5.xyz * r2.yyy;
    r2.xyw = r2.xxx * v4.xyz + r5.xyz;
    r4.xyz = r2.zzz * v3.xyz + r2.xyw;
  }
  r0.y = dot(r4.xyz, r4.xyz);
  r0.y = rsqrt(r0.y);
  r2.xyz = r4.xyz * r0.yyy;
  r0.y = cmp(0.5 < v1.z);
  r0.z = cmp(v1.z < 1.5);
  r0.y = r0.z ? r0.y : 0;
  r0.z = cmp(0.125 >= v11.y);
  r0.y = r0.z ? r0.y : 0;
  r4.xyz = r0.yyy ? v11.xyz : v2.xyz;
  r2.xyz = r0.yyy ? float3(0,1,0) : r2.xyz;
  r0.yz = float2(1,1) / _ScaledScreenParams.xy;
  r5.xy = v7.xy * r0.yz;
  r0.y = r5.y * _ScaleBiasRt.x + _ScaleBiasRt.y;
  r5.z = 1 + -r0.y;
  r0.yz = _GlobalOverallMapBoundMax.xz + -_GlobalOverallMapBoundMin.xz;
  r0.yz = max(float2(1,1), r0.yz);
  r5.yw = -_GlobalOverallMapBoundMin.xz + v2.xz;
  r5.yw = r5.yw / r0.yz;
  r6.xyzw = _GlobalWaterAreaMap.SampleBias(sampler_vd_linear_clamp, r5.yw, _GlobalMipBias.x).xyzw;
  r1.w = cmp(_WeatherIntensity >= 0.00100000005);
  r2.w = 0.899999976 + r6.x;
  r2.w = saturate(0.5 * r2.w);
  r3.w = r2.w * -2 + 3;
  r2.w = r2.w * r2.w;
  r2.w = r3.w * r2.w;
  r2.w = max(_WeatherIndoorMask, r2.w);
  r2.w = 1 + -r2.w;
  r2.w = _WeatherIntensity * r2.w;
  r3.w = cmp(r2.w >= 0.00100000005);
  r4.w = _WeatherWetSurfaceIntensity * r2.w;
  r4.w = r4.w * -0.300000012 + 1;
  r6.xyz = r4.www * r1.xyz;
  r2.w = _WeatherBgTintIntensity * r2.w;
  r6.w = cmp(0.00100000005 < r2.w);
  r7.x = dot(r6.xyz, float3(0.298999995,0.587000012,0.114));
  r7.y = _WeatherBgDesaturation * r2.w;
  r7.xzw = -r1.xyz * r4.www + r7.xxx;
  r7.xyz = r7.yyy * r7.xzw + r6.xyz;
  r2.w = 0.300000012 * r2.w;
  r8.xyz = _WeatherBgTintColor.xyz + -r7.xyz;
  r7.xyz = r2.www * r8.xyz + r7.xyz;
  r6.xyz = r6.www ? r7.xyz : r6.xyz;
  r6.xyz = r3.www ? r6.xyz : r1.xyz;
  r6.xyz = r1.www ? r6.xyz : r1.xyz;
  if (_GlobalWaterAreaEnable != 0) {
    r7.xyzw = _GlobalWaterAreaMap.SampleLevel(sampler_vd_linear_clamp, r5.yw, 0).xyzw;
    r1.w = cmp(0.100000001 < r7.z);
    if (r1.w != 0) {
      r5.yw = _GlobalWaterDepthRange.ww * v2.xz;
      r8.xyzw = _GlobalWaterNoiseTex.SampleLevel(sampler_vd_linear_repeat, r5.yw, 0).xyzw;
      r1.w = -0.5 + r8.y;
      r1.w = _GlobalWaterDepthRange.z * r1.w;
      r2.w = r1.w * r7.z;
      r8.xy = float2(0.800000012,0.600000024) * _GlobalWaterWaveParams.yx;
      r8.z = -0.300000012;
      r5.yw = _Time.yy * r8.yz;
      r7.x = 0.400000006;
      r7.y = 0.600000024 * _GlobalWaterWaveParams.x;
      r5.yw = r7.xy * r5.yw;
      r5.yw = v2.xz * r8.xx + r5.yw;
      r8.xyzw = _GlobalWaterNoiseTex.SampleLevel(sampler_vd_linear_repeat, r5.yw, 0).xyzw;
      r3.w = -0.5 + r7.z;
      r3.w = 5 * abs(r3.w);
      r3.w = min(1, r3.w);
      r3.w = 1 + -r3.w;
      r4.w = -0.5 + r8.y;
      r2.w = r4.w * r3.w + r2.w;
      r2.w = r7.z + r2.w;
      r3.w = _GlobalWaterShoreEdge.y + -_GlobalWaterShoreEdge.x;
      r4.w = -_GlobalWaterShoreEdge.x + r2.w;
      r3.w = 1 / r3.w;
      r3.w = saturate(r4.w * r3.w);
      r4.w = r3.w * -2 + 3;
      r3.w = r3.w * r3.w;
      r3.w = r4.w * r3.w;
      r5.yw = (_GlobalWaterShoreStroke).xx + _GlobalWaterShoreEdge.xy;
      r4.w = r5.w + -r5.y;
      r5.y = -r5.y + r2.w;
      r4.w = 1 / r4.w;
      r4.w = saturate(r5.y * r4.w);
      r5.y = r4.w * -2 + 3;
      r4.w = r4.w * r4.w;
      r4.w = -r5.y * r4.w + 1;
      r1.w = r1.w * r7.z + r7.z;
      r5.y = _GlobalWaterDepthRange.y + -_GlobalWaterDepthRange.x;
      r1.w = -_GlobalWaterDepthRange.x + r1.w;
      r5.y = 1 / r5.y;
      r1.w = saturate(r5.y * r1.w);
      r5.w = r1.w * -2 + 3;
      r1.w = r1.w * r1.w;
      r6.w = r5.w * r1.w;
      r4.w = r4.w * r3.w;
      r7.x = cmp(r3.w >= 0.00999999978);
      if (r7.x != 0) {
        r7.xy = cmp(float2(0.00999999978,0.00999999978) < _GlobalWaterSurfaceWaveMapParams.zw);
        r7.x = r7.y ? r7.x : 0;
        if (r7.x != 0) {
          r7.xy = -_GlobalWaterSurfaceWaveMapParams.xy + v2.xz;
          r7.xy = r7.xy / _GlobalWaterSurfaceWaveMapParams.zw;
          r7.xy = float2(0.5,0.5) + r7.xy;
          r7.zw = float2(1,1) + -r7.xy;
          r7.zw = min(r7.xy, r7.zw);
          r7.z = min(r7.z, r7.w);
          r7.z = saturate(_GlobalWaterSurfaceWaveMapParams.z * r7.z);
          r8.xyzw = _GlobalWaterSurfaceWaveMap.SampleLevel(sampler_vd_linear_clamp, r7.xy, 0).xyzw;
          r7.xy = r8.yz * float2(2,2) + float2(-1,-1);
          r7.zw = r8.wx * r7.zz;
        } else {
          r7.xyzw = float4(0,0,0,0);
        }
        r1.w = -r5.w * r1.w + 1;
        r1.w = -_GlobalWaterShoreHeightDrop * r1.w + _GlobalWaterSurfaceLevel;
        r1.w = r7.w * 0.5 + r1.w;
        r5.w = -_GlobalWaterDepthOffset + v2.y;
        r1.w = r5.w + -r1.w;
        r5.w = saturate(r1.w * -2 + 0.5);
        r8.xy = _GlobalWaterWaveParams.yy * v2.xz;
        r8.z = _GlobalWaterWaveParams.x * _Time.y;
        r9.xyz = float3(0.707099974,-0.780000031,0.519999981) * r8.zzz;
        r8.zw = v2.xz * _GlobalWaterWaveParams.yy + r9.xx;
        r10.xyzw = _GlobalWaterNoiseTex.SampleLevel(sampler_vd_linear_repeat, r8.zw, 0).xyzw;
        r11.xw = _GlobalWaterWaveParams.xx;
        r11.yz = float2(0.699999988,0.300000012);
        r8.zw = _Time.yy * r11.xy;
        r8.zw = r8.zw * r11.zw;
        r8.zw = r8.xy * float2(1.5,1.5) + r8.zw;
        r9.xy = r8.xy * float2(2,2) + r9.yz;
        r11.xyzw = _GlobalWaterNormalMap.SampleLevel(sampler_vd_linear_repeat, r8.zw, 0).xyzw;
        r10.yzw = r11.xyz * float3(2,2,2) + float3(-1,-1,-1);
        r9.xyzw = _GlobalWaterNormalMap.SampleLevel(sampler_vd_linear_repeat, r9.xy, 0).xyzw;
        r9.xyz = r9.xyz * float3(2,2,2) + float3(-1,-1,-1);
        r11.xy = r10.yz + r9.xy;
        r11.z = r10.w * r9.z;
        r8.z = dot(r11.xyz, r11.xyz);
        r8.z = rsqrt(r8.z);
        r9.xyz = r11.xyz * r8.zzz;
        r8.z = saturate(dot(r9.xzy, float3(0.300000012,0.300000012,0.899999976)));
        r8.z = 0.150000006 * r8.z;
        r8.z = r8.z * r6.w;
        r7.xy = r7.xy * r7.zz;
        r7.z = log2(abs(r10.x));
        r7.z = _GlobalWaterWaveParams.z * r7.z;
        r7.z = exp2(r7.z);
        r7.z = min(1, r7.z);
        r7.z = _GlobalWaterWaveParams.w * r7.z;
        r4.w = _GlobalWaterShoreIntensity * r4.w;
        r4.w = max(r4.w, r7.z);
        r4.w = _GlobalWaterShallowColor.w * r4.w;
        r9.xyz = -_GlobalWaterDeepColor.xyz + _GlobalWaterShallowColor.xyz;
        r9.xyz = r4.www * r9.xyz + _GlobalWaterDeepColor.xyz;
        r10.yz = r7.xy * float2(0.5,0.5) + v2.xz;
        r10.yz = -_GlobalOverallMapBoundMin.xz + r10.yz;
        r0.yz = r10.yz / r0.yz;
        r11.xyzw = _GlobalWaterAreaMap.SampleLevel(sampler_vd_linear_clamp, r0.yz, 0).xyzw;
        r0.y = -_GlobalWaterDepthRange.x + r11.z;
        r0.y = saturate(r0.y * r5.y);
        r0.z = r0.y * -2 + 3;
        r0.y = r0.y * r0.y;
        r0.y = r0.z * r0.y + -r6.w;
        r0.y = abs(r0.y) * abs(r7.w);
        r0.y = 3 * r0.y;
        r0.y = min(1, r0.y);
        r10.yzw = _GlobalWaterShallowColor.xyz + -r9.xyz;
        r9.xyz = r0.yyy * r10.yzw + r9.xyz;
        r10.yzw = _GlobalWaterShallowColor.xyz + -r9.xyz;
        r9.xyz = r8.zzz * r10.yzw + r9.xyz;
        r0.y = cmp(r1.w >= 0);
        r0.y = r0.y ? 1.000000 : 0;
        r0.z = 1 / _GlobalWaterSurfaceEdgeWidth;
        r0.z = saturate(r1.w * r0.z);
        r1.w = r0.z * -2 + 3;
        r0.z = r0.z * r0.z;
        r0.z = -r1.w * r0.z + 1;
        r1.w = _GlobalWaterShallowColor.w * r0.y;
        r1.w = r1.w * r3.w;
        r1.w = 0.5 * r1.w;
        r10.yzw = _GlobalWaterShallowColor.xyz + -r9.xyz;
        r9.xyz = r1.www * r10.yzw + r9.xyz;
        r1.w = cmp(vd_asint(_PlaneReflectionActive) != 0);
        r4.w = cmp(0.00999999978 < _GlobalWaterReflectionIntensity);
        r1.w = r1.w ? r4.w : 0;
        if (r1.w != 0) {
          r10.y = -0.5;
          r10.z = _GlobalWaterWaveParams.x;
          r11.yz = _Time.yy * r10.yz;
          r11.x = _GlobalWaterWaveParams.x * r11.y;
          r7.zw = float2(0.800000012,0.692799985) * r11.xz;
          r7.zw = r8.xy * float2(0.699999988,0.699999988) + r7.zw;
          r8.xyzw = _GlobalWaterNoiseTex.SampleLevel(sampler_vd_linear_repeat, r7.zw, 0).yxzw;
          r8.x = r10.x;
          r7.zw = float2(-0.5,-0.5) + r8.xy;
          r7.zw = r7.zw + r7.zw;
          r7.zw = _GlobalWaterWaveParams2.xx * r7.zw;
          r8.xy = v7.xy / _ScreenParams.xy;
          r7.zw = r7.zw * float2(0.5,0.5) + r8.xy;
          r7.xy = r7.xy * float2(0.0199999996,0.0199999996) + r7.zw;
          r7.y = _GlobalWaterReflectionYOffset + r7.y;
          r7.xyzw = _PlaneReflectionTexture.SampleLevel(sampler_vd_linear_clamp, r7.xy, 0).xyzw;
          r1.w = r6.w * 0.699999988 + 0.300000012;
          r1.w = r1.w * r3.w;
          r1.w = _GlobalWaterReflectionIntensity * r1.w;
          r1.w = _PlaneReflectionOpacity * r1.w;
          r7.xyz = r7.xyz * r1.www;
          r1.w = -r0.z * r0.y + 1;
          r9.xyz = r7.xyz * r1.www + r9.xyz;
        }
        r0.y = saturate(r0.z * r0.y + r5.w);
        r1.w = _GlobalWaterDeepColor.w * r3.w;
        r2.w = rsqrt(r2.w);
        r2.w = 1 / r2.w;
        r1.w = r2.w * r1.w;
        r0.y = r1.w * r0.y;
        r0.y = r0.y * r0.z;
        r7.xyz = r9.xyz + -r6.xyz;
        r6.xyz = r0.yyy * r7.xyz + r6.xyz;
      }
    }
  }
  r0.y = vd_asint(_MainLightLayerMask) & vd_asint(unity_RenderingLayer.x);
  r0.z = dot(r2.xyz, _MainLightPosition.xyz);
  r0.z = saturate(r0.z * 0.5 + 0.5);
  r7.xyz = -_GlobalBackgroundShadowColor.xyz + _MainLightColor.xyz;
  r7.xyz = r7.xyz * r0.zzz + _GlobalBackgroundShadowColor.xyz;
  r7.xyz = r7.xyz * r6.xyz;
  r7.xyz = r0.yyy ? r7.xyz : float3(0,0,0);
  r0.yz = -_PlaneBoundMaxPos.xz + r4.xz;
  r5.yw = -_PlaneBoundMaxPos.xz + _PlaneBoundMinPos.xz;
  r0.yz = saturate(r0.yz / r5.yw);
  r8.xyzw = _PlaneSightRenderTarget.SampleLevel(sampler_PlaneSightRenderTarget, r0.yz, 0).xyzw;
  r0.y = -1 + r8.w;
  r0.y = _GlobalPlaneSystemEnable * r0.y;
  r5.xy = _FPParams1.xy * r5.xz;
  r5.xy = (uint2)r5.xy;
  r5.zw = (uint2)_FPParams1.zw;
  r0.z = mad((int)r5.y, (int)r5.z, (int)r5.x);
  r0.z = (int)r5.w * (int)r0.z;
  r5.x = -(VD_COL(unity_MatrixV, 0).z);
  r5.y = -(VD_COL(unity_MatrixV, 1).z);
  r5.z = -(VD_COL(unity_MatrixV, 2).z);
  r8.xyz = -_WorldSpaceCameraPos.xyz + r4.xyz;
  r1.w = dot(r5.xyz, r8.xyz);
  r2.w = cmp(unity_OrthoParams.w == 0.000000);
  r3.w = log2(r1.w);
  r1.w = r2.w ? r3.w : r1.w;
  r1.w = r1.w * _FPParams0.x + _FPParams0.y;
  r1.w = (uint)r1.w;
  r2.w = (uint)_FPParams2.x;
  r2.w = (int)r2.w + -1;
  r3.w = (int)r5.w + 2;
  r1.w = min((uint)r2.w, (uint)r1.w);
  r2.w = (int)r3.w * (int)r1.w;
  r1.w = mad((int)r1.w, (int)r3.w, 2);
  r3.w = (uint)r2.w >> 2;
  r5.x = (uint)r2.w << 31;
  r5.y = (uint)r2.w << 30;
  r5.xy = (uint2)r5.xy >> 31;
  r5.zw = (int2)r5.xx & vd_asint(urp_ZBins[(int)(r3.w)].wy);
  r8.xy = ~(int2)r5.xy;
  r8.xz = (int2)r8.xx & vd_asint(urp_ZBins[(int)(r3.w)].zx);
  r5.xz = (int2)r5.zw | (int2)r8.xz;
  r2.w = (int)r5.y & (int)r5.x;
  r3.w = (int)r8.y & (int)r5.z;
  r2.w = (int)r2.w | (int)r3.w;
  r3.w = (uint)_FPParams0.w;
  r5.x = r0.z;
  r5.y = r1.w;
  r5.z = 0;
  r5.w = r2.w;
  r8.xyz = float3(0,0,0);
  while (true) {
    r4.w = (uint)r5.w >> 16;
    r6.w = r5.z;
    r7.w = r5.w;
    while (true) {
      r8.w = cmp((int)r6.w == 0);
      r9.x = (int)r7.w & 0x0000ffff;
      r9.x = cmp((uint)r4.w >= (uint)r9.x);
      r8.w = r8.w ? r9.x : 0;
      if (r8.w == 0) break;
      r8.w = (uint)r7.w >> 5;
      r8.w = (int)r8.w & 2047;
      r9.xyzw = (int4)r5.xxyy + (int4)r8.wwww;
      r10.xy = (uint2)r9.yw >> 2;
      r11.xz = (uint2)r9.xz << 31;
      r11.yw = (uint2)r9.yw << 30;
      r9.xyzw = (uint4)r11.xyzw >> 31;
      r10.zw = (int2)r9.xx & vd_asint(urp_Tiles[(int)(r10.x)].wy);
      r11.xyzw = ~(int4)r9.xyzw;
      r12.xy = (int2)r11.xx & vd_asint(urp_Tiles[(int)(r10.x)].zx);
      r10.xz = (int2)r10.zw | (int2)r12.xy;
      r9.x = (int)r9.y & (int)r10.x;
      r9.y = (int)r11.y & (int)r10.z;
      r9.x = (int)r9.y | (int)r9.x;
      r9.yz = (int2)r9.zz & vd_asint(urp_ZBins[(int)(r10.y)].wy);
      r10.xy = (int2)r11.zz & vd_asint(urp_ZBins[(int)(r10.y)].zx);
      r9.yz = (int2)r9.yz | (int2)r10.xy;
      r9.y = (int)r9.w & (int)r9.y;
      r9.z = (int)r11.w & (int)r9.z;
      r9.y = (int)r9.z | (int)r9.y;
      r9.x = (int)r9.y & (int)r9.x;
      r9.y = -1 << (int)r7.w;
      r9.x = (int)r9.y & (int)r9.x;
      r8.w = (uint)r8.w << 5;
      r8.w = (int)r4.w + (int)-r8.w;
      r8.w = min(31, (uint)r8.w);
      r8.w = (int)-r8.w + 31;
      r8.w = -1 >> (uint)r8.w;
      r6.w = (int)r8.w & (int)r9.x;
      r8.w = (int)r7.w + 32;
      r7.w = (int)r8.w & -32;
    }
    r5.w = r7.w;
    if (r6.w == 0) break;
    r4.w = -(int)r6.w;
    r4.w = (int)r4.w & (int)r6.w;
    r4.w = (uint)r4.w;
    r4.w = (uint)r4.w >> 23;
    r4.w = (int)r4.w + -127;
    r8.w = 1 << (int)r4.w;
    r5.z = (int)r6.w ^ (int)r8.w;
    r8.w = (int)r5.w + -32;
    r8.w = (int)r8.w & 0x0000ffe0;
    r4.w = (int)r4.w + (int)r8.w;
    r4.w = (int)r3.w + (int)r4.w;
    r9.xyz = -r4.xyz * _AdditionalLightsPosition[(int)(r4.w)].www + _AdditionalLightsPosition[(int)(r4.w)].xyz;
    r8.w = dot(r9.xyz, r9.xyz);
    r8.w = max(6.10351563e-005, r8.w);
    r9.w = rsqrt(r8.w);
    r9.xyz = r9.xyz * r9.www;
    r9.w = 1 / r8.w;
    r8.w = _AdditionalLightsAttenuation[(int)(r4.w)].x * r8.w;
    r8.w = -r8.w * r8.w + 1;
    r8.w = max(0, r8.w);
    r8.w = r8.w * r8.w;
    r8.w = r9.w * r8.w;
    r9.w = dot(_AdditionalLightsSpotDir[(int)(r4.w)].xyz, r9.xyz);
    r9.w = saturate(r9.w * _AdditionalLightsAttenuation[(int)(r4.w)].z + _AdditionalLightsAttenuation[(int)(r4.w)].w);
    r9.w = r9.w * r9.w;
    r8.w = r9.w * r8.w;
    r9.w = cmp(2 == vd_asint(_AdditionalLightsLayerMasks[(int)(r4.w)]));
    r9.w = r9.w ? 1.000000 : 0;
    r9.w = r9.w * r0.y + 1;
    r8.w = sqrt(r8.w);
    r8.w = min(1, r8.w);
    r10.xyz = _AdditionalLightsColor[(int)(r4.w)].xyz * r8.www;
    r4.w = saturate(dot(r2.xyz, r9.xyz));
    r9.xyz = r10.xyz * r4.www;
    r9.xyz = r9.xyz * r6.xyz;
    r8.xyz = r9.xyz * r9.www + r8.xyz;
  }
  r2.xyz = r8.xyz + r7.xyz;
  r0.yzw = r0.www * r1.xyz + r2.xyz;
  r1.x = dot(float2(0.530324996,0.530324996), v2.xz);
  r1.y = dot(float3(-0.353500009,0.865999997,0.353500009), v2.xyz);
  r1.zw = float2(0.125,0.125) * r1.xy;
  r2.xyzw = _DirtNoiseTexture.SampleBias(sampler_vd_point_repeat, r1.zw, _GlobalMipBias.x).xyzw;
  r1.z = r2.x * 0.5 + 0.5;
  r1.w = r1.z * r1.z;
  r1.z = 1 + -r1.z;
  r1.z = r1.z * r1.z;
  r2.xyz = r0.yzw * r0.yzw;
  r2.xyz = min(float3(1,1,1), r2.xyz);
  r1.zw = _DirtNoiseColor.ww * r1.zw;
  r1.zw = (_UseDirtNoise).xx * r1.zw;
  r2.xyz = r2.xyz + -r0.yzw;
  r0.yzw = r1.www * r2.xyz + r0.yzw;
  r2.xyz = saturate(r0.yzw + r0.yzw);
  r2.xyz = r2.xyz + -r0.yzw;
  r0.yzw = r1.zzz * r2.xyz + r0.yzw;
  if (r0.x != 0) {
    r1.zw = float2(0.400000006,0.400000006) * r1.xy;
    r2.xyzw = _DirtNoiseTexture.SampleBias(sampler_vd_point_repeat, r1.zw, _GlobalMipBias.x).xyzw;
    r4.xyzw = _Time.yyyy * float4(1.5,1.5,0.800000012,0.800000012) + v2.zxzx;
    r1.zw = sin(r4.xw);
    r2.yz = cos(r4.yz);
    r4.zw = float2(0.0250000004,0.0250000004) * r2.yz;
    r4.xy = float2(0.0250000004,0.0250000004) * r1.zw;
    r1.zw = r1.xy * float2(0.150000006,0.150000006) + r4.xz;
    r5.xyzw = _DirtNoiseTexture.SampleBias(sampler_vd_point_repeat, r1.zw, _GlobalMipBias.x).xyzw;
    r1.xy = r1.xy * float2(0.1875,0.1875) + r4.wy;
    r1.xyzw = _DirtNoiseTexture.SampleBias(sampler_vd_point_repeat, r1.xy, _GlobalMipBias.x).xyzw;
    r0.x = r5.x + r1.x;
    r0.x = saturate(0.5 * r0.x);
    r0.x = rsqrt(r0.x);
    r0.x = 1 / r0.x;
    r1.x = cmp(_GlobalBoxAreaIntensities[0] >= 0.00100000005);
    if (r1.x != 0) {
      r1.x = (int)_GlobalBoxAreaCenters[0].w;
      r1.yz = cmp((int2)r1.xx == int2(1,4));
      r1.y = (int)r1.z | (int)r1.y;
      if (r1.y == 0) {
        r1.yzw = -_GlobalBoxAreaCenters[0].xyz + v2.xyz;
        r1.yzw = _GlobalBoxAreaExtents[0].xyz + -abs(r1.yzw);
        r1.z = min(r1.z, r1.w);
        r1.y = min(r1.y, r1.z);
        r1.z = max(0.00100000005, _GlobalBoxAreaExtents[0].w);
        r1.y = saturate(r1.y / r1.z);
        r2.yzw = log2(abs(_GlobalBoxAreaColors[0].xyz));
        r2.yzw = float3(2.20000005,2.20000005,2.20000005) * r2.yzw;
        r2.yzw = exp2(r2.yzw);
        r1.x = cmp((int)r1.x == 3);
        if (r1.x != 0) {
          r1.x = -1 + r0.x;
          r1.x = _GlobalBoxAreaShimmerParams[0].z * r1.x + 1;
        } else {
          r1.x = 1;
        }
        r1.z = cmp(_GlobalBoxAreaIntensities[0] >= r2.x);
        r1.z = r1.z ? 1.000000 : 0;
        r1.y = r1.y * r1.z;
        r1.y = _GlobalBoxAreaColors[0].w * r1.y;
        r1.x = r1.y * r1.x;
        r1.yzw = r2.yzw + -r0.yzw;
        r0.yzw = r1.xxx * r1.yzw + r0.yzw;
      }
    }
    r1.x = cmp(_GlobalBoxAreaIntensities[1] >= 0.00100000005);
    if (r1.x != 0) {
      r1.x = (int)_GlobalBoxAreaCenters[1].w;
      r1.yz = cmp((int2)r1.xx == int2(1,4));
      r1.y = (int)r1.z | (int)r1.y;
      if (r1.y == 0) {
        r1.yzw = -_GlobalBoxAreaCenters[1].xyz + v2.xyz;
        r1.yzw = _GlobalBoxAreaExtents[1].xyz + -abs(r1.yzw);
        r1.z = min(r1.z, r1.w);
        r1.y = min(r1.y, r1.z);
        r1.z = max(0.00100000005, _GlobalBoxAreaExtents[1].w);
        r1.y = saturate(r1.y / r1.z);
        r2.yzw = log2(abs(_GlobalBoxAreaColors[1].xyz));
        r2.yzw = float3(2.20000005,2.20000005,2.20000005) * r2.yzw;
        r2.yzw = exp2(r2.yzw);
        r1.x = cmp((int)r1.x == 3);
        if (r1.x != 0) {
          r1.x = -1 + r0.x;
          r1.x = _GlobalBoxAreaShimmerParams[1].z * r1.x + 1;
        } else {
          r1.x = 1;
        }
        r1.z = cmp(_GlobalBoxAreaIntensities[1] >= r2.x);
        r1.z = r1.z ? 1.000000 : 0;
        r1.y = r1.y * r1.z;
        r1.y = _GlobalBoxAreaColors[1].w * r1.y;
        r1.x = r1.y * r1.x;
        r1.yzw = r2.yzw + -r0.yzw;
        r0.yzw = r1.xxx * r1.yzw + r0.yzw;
      }
    }
    r1.x = cmp(_GlobalBoxAreaIntensities[2] >= 0.00100000005);
    if (r1.x != 0) {
      r1.x = (int)_GlobalBoxAreaCenters[2].w;
      r1.yz = cmp((int2)r1.xx == int2(1,4));
      r1.y = (int)r1.z | (int)r1.y;
      if (r1.y == 0) {
        r1.yzw = -_GlobalBoxAreaCenters[2].xyz + v2.xyz;
        r1.yzw = _GlobalBoxAreaExtents[2].xyz + -abs(r1.yzw);
        r1.z = min(r1.z, r1.w);
        r1.y = min(r1.y, r1.z);
        r1.z = max(0.00100000005, _GlobalBoxAreaExtents[2].w);
        r1.y = saturate(r1.y / r1.z);
        r2.yzw = log2(abs(_GlobalBoxAreaColors[2].xyz));
        r2.yzw = float3(2.20000005,2.20000005,2.20000005) * r2.yzw;
        r2.yzw = exp2(r2.yzw);
        r1.x = cmp((int)r1.x == 3);
        if (r1.x != 0) {
          r1.x = -1 + r0.x;
          r1.x = _GlobalBoxAreaShimmerParams[2].z * r1.x + 1;
        } else {
          r1.x = 1;
        }
        r1.z = cmp(_GlobalBoxAreaIntensities[2] >= r2.x);
        r1.z = r1.z ? 1.000000 : 0;
        r1.y = r1.y * r1.z;
        r1.y = _GlobalBoxAreaColors[2].w * r1.y;
        r1.x = r1.y * r1.x;
        r1.yzw = r2.yzw + -r0.yzw;
        r0.yzw = r1.xxx * r1.yzw + r0.yzw;
      }
    }
    r1.x = cmp(_GlobalBoxAreaIntensities[3] >= 0.00100000005);
    if (r1.x != 0) {
      r1.x = (int)_GlobalBoxAreaCenters[3].w;
      r1.yz = cmp((int2)r1.xx == int2(1,4));
      r1.y = (int)r1.z | (int)r1.y;
      if (r1.y == 0) {
        r1.yzw = -_GlobalBoxAreaCenters[3].xyz + v2.xyz;
        r1.yzw = _GlobalBoxAreaExtents[3].xyz + -abs(r1.yzw);
        r1.z = min(r1.z, r1.w);
        r1.y = min(r1.y, r1.z);
        r1.z = max(0.00100000005, _GlobalBoxAreaExtents[3].w);
        r1.y = saturate(r1.y / r1.z);
        r2.yzw = log2(abs(_GlobalBoxAreaColors[3].xyz));
        r2.yzw = float3(2.20000005,2.20000005,2.20000005) * r2.yzw;
        r2.yzw = exp2(r2.yzw);
        r1.x = cmp((int)r1.x == 3);
        if (r1.x != 0) {
          r1.x = -1 + r0.x;
          r1.x = _GlobalBoxAreaShimmerParams[3].z * r1.x + 1;
        } else {
          r1.x = 1;
        }
        r1.z = cmp(_GlobalBoxAreaIntensities[3] >= r2.x);
        r1.z = r1.z ? 1.000000 : 0;
        r1.y = r1.y * r1.z;
        r1.y = _GlobalBoxAreaColors[3].w * r1.y;
        r1.x = r1.y * r1.x;
        r1.yzw = r2.yzw + -r0.yzw;
        r0.yzw = r1.xxx * r1.yzw + r0.yzw;
      }
    }
    r1.x = cmp(_GlobalBoxAreaIntensities[4] >= 0.00100000005);
    if (r1.x != 0) {
      r1.x = (int)_GlobalBoxAreaCenters[4].w;
      r1.yz = cmp((int2)r1.xx == int2(1,4));
      r1.y = (int)r1.z | (int)r1.y;
      if (r1.y == 0) {
        r1.yzw = -_GlobalBoxAreaCenters[4].xyz + v2.xyz;
        r1.yzw = _GlobalBoxAreaExtents[4].xyz + -abs(r1.yzw);
        r1.z = min(r1.z, r1.w);
        r1.y = min(r1.y, r1.z);
        r1.z = max(0.00100000005, _GlobalBoxAreaExtents[4].w);
        r1.y = saturate(r1.y / r1.z);
        r2.yzw = log2(abs(_GlobalBoxAreaColors[4].xyz));
        r2.yzw = float3(2.20000005,2.20000005,2.20000005) * r2.yzw;
        r2.yzw = exp2(r2.yzw);
        r1.x = cmp((int)r1.x == 3);
        if (r1.x != 0) {
          r1.x = -1 + r0.x;
          r1.x = _GlobalBoxAreaShimmerParams[4].z * r1.x + 1;
        } else {
          r1.x = 1;
        }
        r1.z = cmp(_GlobalBoxAreaIntensities[4] >= r2.x);
        r1.z = r1.z ? 1.000000 : 0;
        r1.y = r1.y * r1.z;
        r1.y = _GlobalBoxAreaColors[4].w * r1.y;
        r1.x = r1.y * r1.x;
        r1.yzw = r2.yzw + -r0.yzw;
        r0.yzw = r1.xxx * r1.yzw + r0.yzw;
      }
    }
    r1.x = cmp(_GlobalBoxAreaIntensities[5] >= 0.00100000005);
    if (r1.x != 0) {
      r1.x = (int)_GlobalBoxAreaCenters[5].w;
      r1.yz = cmp((int2)r1.xx == int2(1,4));
      r1.y = (int)r1.z | (int)r1.y;
      if (r1.y == 0) {
        r1.yzw = -_GlobalBoxAreaCenters[5].xyz + v2.xyz;
        r1.yzw = _GlobalBoxAreaExtents[5].xyz + -abs(r1.yzw);
        r1.z = min(r1.z, r1.w);
        r1.y = min(r1.y, r1.z);
        r1.z = max(0.00100000005, _GlobalBoxAreaExtents[5].w);
        r1.y = saturate(r1.y / r1.z);
        r2.yzw = log2(abs(_GlobalBoxAreaColors[5].xyz));
        r2.yzw = float3(2.20000005,2.20000005,2.20000005) * r2.yzw;
        r2.yzw = exp2(r2.yzw);
        r1.x = cmp((int)r1.x == 3);
        if (r1.x != 0) {
          r1.x = -1 + r0.x;
          r1.x = _GlobalBoxAreaShimmerParams[5].z * r1.x + 1;
        } else {
          r1.x = 1;
        }
        r1.z = cmp(_GlobalBoxAreaIntensities[5] >= r2.x);
        r1.z = r1.z ? 1.000000 : 0;
        r1.y = r1.y * r1.z;
        r1.y = _GlobalBoxAreaColors[5].w * r1.y;
        r1.x = r1.y * r1.x;
        r1.yzw = r2.yzw + -r0.yzw;
        r0.yzw = r1.xxx * r1.yzw + r0.yzw;
      }
    }
    r1.x = cmp(_GlobalBoxAreaIntensities[6] >= 0.00100000005);
    if (r1.x != 0) {
      r1.x = (int)_GlobalBoxAreaCenters[6].w;
      r1.yz = cmp((int2)r1.xx == int2(1,4));
      r1.y = (int)r1.z | (int)r1.y;
      if (r1.y == 0) {
        r1.yzw = -_GlobalBoxAreaCenters[6].xyz + v2.xyz;
        r1.yzw = _GlobalBoxAreaExtents[6].xyz + -abs(r1.yzw);
        r1.z = min(r1.z, r1.w);
        r1.y = min(r1.y, r1.z);
        r1.z = max(0.00100000005, _GlobalBoxAreaExtents[6].w);
        r1.y = saturate(r1.y / r1.z);
        r2.yzw = log2(abs(_GlobalBoxAreaColors[6].xyz));
        r2.yzw = float3(2.20000005,2.20000005,2.20000005) * r2.yzw;
        r2.yzw = exp2(r2.yzw);
        r1.x = cmp((int)r1.x == 3);
        if (r1.x != 0) {
          r1.x = -1 + r0.x;
          r1.x = _GlobalBoxAreaShimmerParams[6].z * r1.x + 1;
        } else {
          r1.x = 1;
        }
        r1.z = cmp(_GlobalBoxAreaIntensities[6] >= r2.x);
        r1.z = r1.z ? 1.000000 : 0;
        r1.y = r1.y * r1.z;
        r1.y = _GlobalBoxAreaColors[6].w * r1.y;
        r1.x = r1.y * r1.x;
        r1.yzw = r2.yzw + -r0.yzw;
        r0.yzw = r1.xxx * r1.yzw + r0.yzw;
      }
    }
    r1.x = cmp(_GlobalBoxAreaIntensities[7] >= 0.00100000005);
    r1.y = (int)_GlobalBoxAreaCenters[7].w;
    r1.yzw = cmp((int3)r1.yyy == int3(1,4,3));
    r1.y = (int)r1.z | (int)r1.y;
    r2.yzw = -_GlobalBoxAreaCenters[7].xyz + v2.xyz;
    r2.yzw = _GlobalBoxAreaExtents[7].xyz + -abs(r2.yzw);
    r1.z = min(r2.z, r2.w);
    r1.z = min(r2.y, r1.z);
    r2.y = max(0.00100000005, _GlobalBoxAreaExtents[7].w);
    r1.z = saturate(r1.z / r2.y);
    r2.yzw = log2(abs(_GlobalBoxAreaColors[7].xyz));
    r2.yzw = float3(2.20000005,2.20000005,2.20000005) * r2.yzw;
    r2.yzw = exp2(r2.yzw);
    if (r1.w != 0) {
      r0.x = -1 + r0.x;
      r0.x = _GlobalBoxAreaShimmerParams[7].z * r0.x + 1;
    } else {
      r0.x = 1;
    }
    r1.w = cmp(_GlobalBoxAreaIntensities[7] >= r2.x);
    r1.w = r1.w ? 1.000000 : 0;
    r1.z = r1.z * r1.w;
    r1.z = _GlobalBoxAreaColors[7].w * r1.z;
    r0.x = r1.z * r0.x;
    r2.xyz = r2.yzw + -r0.yzw;
    r2.xyz = r0.xxx * r2.xyz + r0.yzw;
    r1.yzw = r1.yyy ? r0.yzw : r2.xyz;
    r0.yzw = r1.xxx ? r1.yzw : r0.yzw;
  }
  r1.xyz = (_EmissionIntensity).xxx * r3.xyz;
  r0.x = _Time.y + _Time.y;
  r1.w = dot(v8.xyz, float3(0.298999995,0.587000012,0.114));
  r2.x = dot(v10.xyz, float3(12.9898005,78.2330017,45.1640015));
  r2.x = frac(r2.x);
  r1.w = r2.x + r1.w;
  r2.xyzw = float4(91.3000031,37.0999985,53.7000008,3) * r1.wwww;
  r2.xyz = sin(r2.xyz);
  r2.xyz = float3(4337.5,1572.90002,9281.2002) * r2.xyz;
  r2.xyz = frac(r2.xyz);
  r2.xyz = r2.xyz * float3(4,6,12) + float3(3.70000005,7.19999981,15.3000002);
  r3.xyz = float3(6.28000021,3.1400001,4.71000004) * r1.www;
  r2.xyz = r0.xxx * r2.xyz + r3.xyz;
  r2.xyz = sin(r2.xyz);
  r2.xyz = r2.xyz * float3(0.5,0.5,0.5) + float3(0.5,0.5,0.5);
  r0.x = 1.60000002 * _Time.y;
  r0.x = r1.w * 2 + r0.x;
  r0.x = sin(r0.x);
  r2.w = _Time.y * 2.5999999 + r2.w;
  r2.w = sin(r2.w);
  r0.x = r2.w * r0.x;
  r0.x = r0.x * 0.5 + 0.5;
  r1.w = _Time.y * 27.3999996 + r1.w;
  r1.w = sin(r1.w);
  r1.w = 43758.5469 * r1.w;
  r1.w = frac(r1.w);
  r1.w = cmp(r1.w >= 0.970000029);
  r2.x = dot(r2.xyz, float3(0.150000006,0.150000006,0.200000003));
  r0.x = r0.x * 0.300000012 + r2.x;
  r1.w = r1.w ? 0.200000 : 0;
  r0.x = r1.w + r0.x;
  r0.x = -0.75 + r0.x;
  r0.x = saturate(-9.99999809 * r0.x);
  r1.w = r0.x * -2 + 3;
  r0.x = r0.x * r0.x;
  r0.x = r1.w * r0.x + -1;
  r0.x = _EmissionFlickRate * r0.x + 1;
  r0.xyz = r1.xyz * r0.xxx + r0.yzw;
  r1.xyz = _GlobalBackgroundColor.xyz + -r0.xyz;
  o0.xyz = (_GlobalBackgroundColorIntensity).xxx * r1.xyz + r0.xyz;
  return;
}
