// LCArt/Background/shader_DE_BG_Particle_VertexAlphaTest | pass 0 "ForwardLit" | progFragment | keywords: _DE_EDITOR_CAMERA _LIGHT_LAYERS | blob 25
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
  float4 r0,r1,r2,r3,r4,r5,r6,r7,r8,r9,r10;
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
  r2.xyz = r0.yyy ? float3(0,1,0) : r2.xyz;
  r0.yz = _GlobalOverallMapBoundMax.xz + -_GlobalOverallMapBoundMin.xz;
  r0.yz = max(float2(1,1), r0.yz);
  r4.xy = -_GlobalOverallMapBoundMin.xz + v2.xz;
  r4.xy = r4.xy / r0.yz;
  r5.xyzw = _GlobalWaterAreaMap.SampleBias(sampler_vd_linear_clamp, r4.xy, _GlobalMipBias.x).xyzw;
  r1.w = cmp(_WeatherIntensity >= 0.00100000005);
  r2.w = 0.899999976 + r5.x;
  r2.w = saturate(0.5 * r2.w);
  r3.w = r2.w * -2 + 3;
  r2.w = r2.w * r2.w;
  r2.w = r3.w * r2.w;
  r2.w = max(_WeatherIndoorMask, r2.w);
  r2.w = 1 + -r2.w;
  r2.w = _WeatherIntensity * r2.w;
  r3.w = cmp(r2.w >= 0.00100000005);
  r4.z = _WeatherWetSurfaceIntensity * r2.w;
  r4.z = r4.z * -0.300000012 + 1;
  r5.xyz = r4.zzz * r1.xyz;
  r2.w = _WeatherBgTintIntensity * r2.w;
  r4.w = cmp(0.00100000005 < r2.w);
  r5.w = dot(r5.xyz, float3(0.298999995,0.587000012,0.114));
  r6.x = _WeatherBgDesaturation * r2.w;
  r6.yzw = -r1.xyz * r4.zzz + r5.www;
  r6.xyz = r6.xxx * r6.yzw + r5.xyz;
  r2.w = 0.300000012 * r2.w;
  r7.xyz = _WeatherBgTintColor.xyz + -r6.xyz;
  r6.xyz = r2.www * r7.xyz + r6.xyz;
  r5.xyz = r4.www ? r6.xyz : r5.xyz;
  r5.xyz = r3.www ? r5.xyz : r1.xyz;
  r5.xyz = r1.www ? r5.xyz : r1.xyz;
  if (_GlobalWaterAreaEnable != 0) {
    r4.xyzw = _GlobalWaterAreaMap.SampleLevel(sampler_vd_linear_clamp, r4.xy, 0).xyzw;
    r1.w = cmp(0.100000001 < r4.z);
    if (r1.w != 0) {
      r4.xy = _GlobalWaterDepthRange.ww * v2.xz;
      r6.xyzw = _GlobalWaterNoiseTex.SampleLevel(sampler_vd_linear_repeat, r4.xy, 0).xyzw;
      r1.w = -0.5 + r6.y;
      r1.w = _GlobalWaterDepthRange.z * r1.w;
      r2.w = r1.w * r4.z;
      r6.xy = float2(0.800000012,0.600000024) * _GlobalWaterWaveParams.yx;
      r6.z = -0.300000012;
      r4.xy = _Time.yy * r6.yz;
      r7.x = 0.400000006;
      r7.y = 0.600000024 * _GlobalWaterWaveParams.x;
      r4.xy = r7.xy * r4.xy;
      r4.xy = v2.xz * r6.xx + r4.xy;
      r6.xyzw = _GlobalWaterNoiseTex.SampleLevel(sampler_vd_linear_repeat, r4.xy, 0).xyzw;
      r3.w = -0.5 + r4.z;
      r3.w = 5 * abs(r3.w);
      r3.w = min(1, r3.w);
      r3.w = 1 + -r3.w;
      r4.x = -0.5 + r6.y;
      r2.w = r4.x * r3.w + r2.w;
      r2.w = r4.z + r2.w;
      r3.w = _GlobalWaterShoreEdge.y + -_GlobalWaterShoreEdge.x;
      r4.x = -_GlobalWaterShoreEdge.x + r2.w;
      r3.w = 1 / r3.w;
      r3.w = saturate(r4.x * r3.w);
      r4.x = r3.w * -2 + 3;
      r3.w = r3.w * r3.w;
      r3.w = r4.x * r3.w;
      r4.xy = (_GlobalWaterShoreStroke).xx + _GlobalWaterShoreEdge.xy;
      r4.y = r4.y + -r4.x;
      r4.x = -r4.x + r2.w;
      r4.y = 1 / r4.y;
      r4.x = saturate(r4.x * r4.y);
      r4.y = r4.x * -2 + 3;
      r4.x = r4.x * r4.x;
      r4.x = -r4.y * r4.x + 1;
      r1.w = r1.w * r4.z + r4.z;
      r4.y = _GlobalWaterDepthRange.y + -_GlobalWaterDepthRange.x;
      r1.w = -_GlobalWaterDepthRange.x + r1.w;
      r4.y = 1 / r4.y;
      r1.w = saturate(r4.y * r1.w);
      r4.z = r1.w * -2 + 3;
      r1.w = r1.w * r1.w;
      r4.w = r4.z * r1.w;
      r4.x = r4.x * r3.w;
      r5.w = cmp(r3.w >= 0.00999999978);
      if (r5.w != 0) {
        r6.xy = cmp(float2(0.00999999978,0.00999999978) < _GlobalWaterSurfaceWaveMapParams.zw);
        r5.w = r6.y ? r6.x : 0;
        if (r5.w != 0) {
          r6.xy = -_GlobalWaterSurfaceWaveMapParams.xy + v2.xz;
          r6.xy = r6.xy / _GlobalWaterSurfaceWaveMapParams.zw;
          r6.xy = float2(0.5,0.5) + r6.xy;
          r6.zw = float2(1,1) + -r6.xy;
          r6.zw = min(r6.xy, r6.zw);
          r5.w = min(r6.z, r6.w);
          r5.w = saturate(_GlobalWaterSurfaceWaveMapParams.z * r5.w);
          r6.xyzw = _GlobalWaterSurfaceWaveMap.SampleLevel(sampler_vd_linear_clamp, r6.xy, 0).xyzw;
          r6.yz = r6.yz * float2(2,2) + float2(-1,-1);
          r6.xw = r6.wx * r5.ww;
        } else {
          r6.xyzw = float4(0,0,0,0);
        }
        r1.w = -r4.z * r1.w + 1;
        r1.w = -_GlobalWaterShoreHeightDrop * r1.w + _GlobalWaterSurfaceLevel;
        r1.w = r6.w * 0.5 + r1.w;
        r4.z = -_GlobalWaterDepthOffset + v2.y;
        r1.w = r4.z + -r1.w;
        r4.z = saturate(r1.w * -2 + 0.5);
        r7.xy = _GlobalWaterWaveParams.yy * v2.xz;
        r5.w = _GlobalWaterWaveParams.x * _Time.y;
        r8.xyz = float3(0.707099974,-0.780000031,0.519999981) * r5.www;
        r7.zw = v2.xz * _GlobalWaterWaveParams.yy + r8.xx;
        r9.xyzw = _GlobalWaterNoiseTex.SampleLevel(sampler_vd_linear_repeat, r7.zw, 0).xyzw;
        r10.xw = _GlobalWaterWaveParams.xx;
        r10.yz = float2(0.699999988,0.300000012);
        r7.zw = _Time.yy * r10.xy;
        r7.zw = r7.zw * r10.zw;
        r7.zw = r7.xy * float2(1.5,1.5) + r7.zw;
        r8.xy = r7.xy * float2(2,2) + r8.yz;
        r10.xyzw = _GlobalWaterNormalMap.SampleLevel(sampler_vd_linear_repeat, r7.zw, 0).xyzw;
        r9.yzw = r10.xyz * float3(2,2,2) + float3(-1,-1,-1);
        r8.xyzw = _GlobalWaterNormalMap.SampleLevel(sampler_vd_linear_repeat, r8.xy, 0).xyzw;
        r8.xyz = r8.xyz * float3(2,2,2) + float3(-1,-1,-1);
        r10.xy = r9.yz + r8.xy;
        r10.z = r9.w * r8.z;
        r5.w = dot(r10.xyz, r10.xyz);
        r5.w = rsqrt(r5.w);
        r8.xyz = r10.xyz * r5.www;
        r5.w = saturate(dot(r8.xzy, float3(0.300000012,0.300000012,0.899999976)));
        r5.w = 0.150000006 * r5.w;
        r5.w = r5.w * r4.w;
        r6.xy = r6.yz * r6.xx;
        r6.z = log2(abs(r9.x));
        r6.z = _GlobalWaterWaveParams.z * r6.z;
        r6.z = exp2(r6.z);
        r6.z = min(1, r6.z);
        r6.z = _GlobalWaterWaveParams.w * r6.z;
        r4.x = _GlobalWaterShoreIntensity * r4.x;
        r4.x = max(r4.x, r6.z);
        r4.x = _GlobalWaterShallowColor.w * r4.x;
        r8.xyz = -_GlobalWaterDeepColor.xyz + _GlobalWaterShallowColor.xyz;
        r8.xyz = r4.xxx * r8.xyz + _GlobalWaterDeepColor.xyz;
        r7.zw = r6.xy * float2(0.5,0.5) + v2.xz;
        r7.zw = -_GlobalOverallMapBoundMin.xz + r7.zw;
        r0.yz = r7.zw / r0.yz;
        r10.xyzw = _GlobalWaterAreaMap.SampleLevel(sampler_vd_linear_clamp, r0.yz, 0).xyzw;
        r0.y = -_GlobalWaterDepthRange.x + r10.z;
        r0.y = saturate(r0.y * r4.y);
        r0.z = r0.y * -2 + 3;
        r0.y = r0.y * r0.y;
        r0.y = r0.z * r0.y + -r4.w;
        r0.y = abs(r0.y) * abs(r6.w);
        r0.y = 3 * r0.y;
        r0.y = min(1, r0.y);
        r9.yzw = _GlobalWaterShallowColor.xyz + -r8.xyz;
        r8.xyz = r0.yyy * r9.yzw + r8.xyz;
        r9.yzw = _GlobalWaterShallowColor.xyz + -r8.xyz;
        r8.xyz = r5.www * r9.yzw + r8.xyz;
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
        r9.yzw = _GlobalWaterShallowColor.xyz + -r8.xyz;
        r8.xyz = r1.www * r9.yzw + r8.xyz;
        r1.w = cmp(vd_asint(_PlaneReflectionActive) != 0);
        r4.x = cmp(0.00999999978 < _GlobalWaterReflectionIntensity);
        r1.w = r1.w ? r4.x : 0;
        if (r1.w != 0) {
          r9.y = -0.5;
          r9.z = _GlobalWaterWaveParams.x;
          r10.yz = _Time.yy * r9.yz;
          r10.x = _GlobalWaterWaveParams.x * r10.y;
          r4.xy = float2(0.800000012,0.692799985) * r10.xz;
          r4.xy = r7.xy * float2(0.699999988,0.699999988) + r4.xy;
          r7.xyzw = _GlobalWaterNoiseTex.SampleLevel(sampler_vd_linear_repeat, r4.xy, 0).yxzw;
          r7.x = r9.x;
          r4.xy = float2(-0.5,-0.5) + r7.xy;
          r4.xy = r4.xy + r4.xy;
          r4.xy = _GlobalWaterWaveParams2.xx * r4.xy;
          r6.zw = v7.xy / _ScreenParams.xy;
          r4.xy = r4.xy * float2(0.5,0.5) + r6.zw;
          r4.xy = r6.xy * float2(0.0199999996,0.0199999996) + r4.xy;
          r4.y = _GlobalWaterReflectionYOffset + r4.y;
          r6.xyzw = _PlaneReflectionTexture.SampleLevel(sampler_vd_linear_clamp, r4.xy, 0).xyzw;
          r1.w = r4.w * 0.699999988 + 0.300000012;
          r1.w = r1.w * r3.w;
          r1.w = _GlobalWaterReflectionIntensity * r1.w;
          r1.w = _PlaneReflectionOpacity * r1.w;
          r4.xyw = r6.xyz * r1.www;
          r1.w = -r0.z * r0.y + 1;
          r8.xyz = r4.xyw * r1.www + r8.xyz;
        }
        r0.y = saturate(r0.z * r0.y + r4.z);
        r1.w = _GlobalWaterDeepColor.w * r3.w;
        r2.w = rsqrt(r2.w);
        r2.w = 1 / r2.w;
        r1.w = r2.w * r1.w;
        r0.y = r1.w * r0.y;
        r0.y = r0.y * r0.z;
        r4.xyz = r8.xyz + -r5.xyz;
        r5.xyz = r0.yyy * r4.xyz + r5.xyz;
      }
    }
  }
  r0.y = vd_asint(_MainLightLayerMask) & vd_asint(unity_RenderingLayer.x);
  r0.z = sqrt(unity_LightData.z);
  r0.z = min(1, r0.z);
  r1.w = dot(r2.xyz, _MainLightPosition.xyz);
  r1.w = saturate(r1.w * 0.5 + 0.5);
  r2.xyz = _MainLightColor.xyz * r0.zzz + -_GlobalBackgroundShadowColor.xyz;
  r2.xyz = r2.xyz * r1.www + _GlobalBackgroundShadowColor.xyz;
  r2.xyz = r2.xyz * r5.xyz;
  r2.xyz = r0.yyy ? r2.xyz : float3(0,0,0);
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
