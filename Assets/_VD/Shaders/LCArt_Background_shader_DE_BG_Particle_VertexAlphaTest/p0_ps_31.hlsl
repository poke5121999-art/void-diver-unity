// LCArt/Background/shader_DE_BG_Particle_VertexAlphaTest | pass 0 "ForwardLit" | progFragment | keywords: _ADDITIONAL_LIGHTS _LIGHT_LAYERS | blob 31
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
  const float4 icb[] = { { 1.000000, 0, 0, 0},
                              { 0, 1.000000, 0, 0},
                              { 0, 0, 1.000000, 0},
                              { 0, 0, 0, 1.000000} };
  float4 r0,r1,r2,r3,r4,r5,r6,r7,r8,r9,r10,r11;
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
  r0.w = cmp(1.5 < v1.z);
  if (r0.w != 0) {
    r3.xyzw = _CustomDepthBuffer.SampleBias(sampler_CustomDepthBuffer, r2.xz, _GlobalMipBias.x).xyzw;
    r0.w = v7.z + -r3.z;
    r0.w = -2.49999994e-005 + r0.w;
    r0.w = saturate(-20000 * r0.w);
    r2.y = r0.w * -2 + 3;
    r0.w = r0.w * r0.w;
    r2.w = r2.y * r0.w;
    r3.xy = float2(1,1) + -float2(_GlobalSeeThroughCircleRange, _GlobalSeeThroughCircleAlpha);
    r3.z = 1 + -r3.x;
    r3.x = r3.w + -r3.x;
    r3.z = 1 / r3.z;
    r3.x = saturate(r3.x * r3.z);
    r3.z = r3.x * -2 + 3;
    r3.x = r3.x * r3.x;
    r3.x = r3.z * r3.x;
    r0.w = -r2.y * r0.w + 1;
    r0.w = r3.x * r0.w + r2.w;
    r2.y = _GlobalPlaneSystemEnable * _GlobalBackgroundSeeThroughEnable;
    r0.w = -1 + r0.w;
    r0.w = r2.y * r0.w + 1;
    r0.w = r0.w * r3.y + _GlobalSeeThroughCircleAlpha;
    r2.yw = _ScreenSize.xy / _ScreenParams.xy;
    r3.xy = _ScreenParams.xy * r2.xz;
    r2.yw = r3.xy * r2.yw;
    r2.yw = floor(r2.yw);
    r2.yw = (uint2)r2.yw;
    r2.yw = (int2)r2.yw & int2(3,3);
    r2.w = (uint)r2.w << 2;
    r2.y = (int)r2.w + (int)r2.y;
    r2.y = x0[r2.y+0].x;
    r0.w = -r2.y + r0.w;
    r0.w = cmp(r0.w < 0);
    if (r0.w != 0) discard;
  }
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
  r1.xyzw = _BaseColor.wxyz * r1.wxyz;
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
  r3.xyz = r0.www * r1.yzw;
  r2.w = cmp(0.5 < _UseWorldNormalMap);
  if (r2.w != 0) {
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
  r0.yz = _GlobalOverallMapBoundMax.xz + -_GlobalOverallMapBoundMin.xz;
  r0.yz = max(float2(1,1), r0.yz);
  r5.xy = -_GlobalOverallMapBoundMin.xz + v2.xz;
  r5.xy = r5.xy / r0.yz;
  r6.xyzw = _GlobalWaterAreaMap.SampleBias(sampler_vd_linear_clamp, r5.xy, _GlobalMipBias.x).xyzw;
  r2.w = cmp(_WeatherIntensity >= 0.00100000005);
  r3.w = 0.899999976 + r6.x;
  r3.w = saturate(0.5 * r3.w);
  r4.w = r3.w * -2 + 3;
  r3.w = r3.w * r3.w;
  r3.w = r4.w * r3.w;
  r3.w = max(_WeatherIndoorMask, r3.w);
  r3.w = 1 + -r3.w;
  r3.w = _WeatherIntensity * r3.w;
  r4.w = cmp(r3.w >= 0.00100000005);
  r5.z = _WeatherWetSurfaceIntensity * r3.w;
  r5.z = r5.z * -0.300000012 + 1;
  r6.xyz = r5.zzz * r1.yzw;
  r3.w = _WeatherBgTintIntensity * r3.w;
  r5.w = cmp(0.00100000005 < r3.w);
  r6.w = dot(r6.xyz, float3(0.298999995,0.587000012,0.114));
  r7.x = _WeatherBgDesaturation * r3.w;
  r7.yzw = -r1.yzw * r5.zzz + r6.www;
  r7.xyz = r7.xxx * r7.yzw + r6.xyz;
  r3.w = 0.300000012 * r3.w;
  r8.xyz = _WeatherBgTintColor.xyz + -r7.xyz;
  r7.xyz = r3.www * r8.xyz + r7.xyz;
  r6.xyz = r5.www ? r7.xyz : r6.xyz;
  r6.xyz = r4.www ? r6.xyz : r1.yzw;
  r6.xyz = r2.www ? r6.xyz : r1.yzw;
  if (_GlobalWaterAreaEnable != 0) {
    r5.xyzw = _GlobalWaterAreaMap.SampleLevel(sampler_vd_linear_clamp, r5.xy, 0).xyzw;
    r2.w = cmp(0.100000001 < r5.z);
    if (r2.w != 0) {
      r5.xy = _GlobalWaterDepthRange.ww * v2.xz;
      r7.xyzw = _GlobalWaterNoiseTex.SampleLevel(sampler_vd_linear_repeat, r5.xy, 0).xyzw;
      r2.w = -0.5 + r7.y;
      r2.w = _GlobalWaterDepthRange.z * r2.w;
      r3.w = r2.w * r5.z;
      r4.w = 0.800000012 * _GlobalWaterWaveParams.y;
      r5.x = _GlobalWaterWaveParams.x * _Time.y;
      r5.yw = float2(0.24000001,-0.180000007) * r5.xx;
      r5.yw = v2.xz * r4.ww + r5.yw;
      r7.xyzw = _GlobalWaterNoiseTex.SampleLevel(sampler_vd_linear_repeat, r5.yw, 0).xyzw;
      r4.w = -0.5 + r5.z;
      r4.w = 5 * abs(r4.w);
      r4.w = min(1, r4.w);
      r4.w = 1 + -r4.w;
      r5.y = -0.5 + r7.y;
      r3.w = r5.y * r4.w + r3.w;
      r3.w = r5.z + r3.w;
      r4.w = _GlobalWaterShoreEdge.y + -_GlobalWaterShoreEdge.x;
      r5.y = -_GlobalWaterShoreEdge.x + r3.w;
      r4.w = 1 / r4.w;
      r4.w = saturate(r5.y * r4.w);
      r5.y = r4.w * -2 + 3;
      r4.w = r4.w * r4.w;
      r4.w = r5.y * r4.w;
      r5.yw = (_GlobalWaterShoreStroke).xx + _GlobalWaterShoreEdge.xy;
      r5.w = r5.w + -r5.y;
      r5.y = -r5.y + r3.w;
      r5.w = 1 / r5.w;
      r5.y = saturate(r5.y * r5.w);
      r5.w = r5.y * -2 + 3;
      r5.y = r5.y * r5.y;
      r5.y = -r5.w * r5.y + 1;
      r2.w = r2.w * r5.z + r5.z;
      r5.z = _GlobalWaterDepthRange.y + -_GlobalWaterDepthRange.x;
      r2.w = -_GlobalWaterDepthRange.x + r2.w;
      r5.z = 1 / r5.z;
      r2.w = saturate(r5.z * r2.w);
      r5.w = r2.w * -2 + 3;
      r2.w = r2.w * r2.w;
      r6.w = r5.w * r2.w;
      r5.y = r5.y * r4.w;
      r7.x = cmp(r4.w >= 0.00999999978);
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
        r2.w = -r5.w * r2.w + 1;
        r2.w = -_GlobalWaterShoreHeightDrop * r2.w + _GlobalWaterSurfaceLevel;
        r2.w = r7.w * 0.5 + r2.w;
        r5.w = -_GlobalWaterDepthOffset + v2.y;
        r2.w = r5.w + -r2.w;
        r5.w = saturate(r2.w * -2 + 0.5);
        r8.xy = _GlobalWaterWaveParams.yy * v2.xz;
        r9.xyz = float3(0.707099974,-0.780000031,0.519999981) * r5.xxx;
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
        r5.x = dot(r11.xyz, r11.xyz);
        r5.x = rsqrt(r5.x);
        r9.xyz = r11.xyz * r5.xxx;
        r5.x = saturate(dot(r9.xzy, float3(0.300000012,0.300000012,0.899999976)));
        r5.x = 0.150000006 * r5.x;
        r5.x = r5.x * r6.w;
        r7.xy = r7.xy * r7.zz;
        r7.z = log2(abs(r10.x));
        r7.z = _GlobalWaterWaveParams.z * r7.z;
        r7.z = exp2(r7.z);
        r7.z = min(1, r7.z);
        r7.z = _GlobalWaterWaveParams.w * r7.z;
        r5.y = _GlobalWaterShoreIntensity * r5.y;
        r5.y = max(r5.y, r7.z);
        r5.y = _GlobalWaterShallowColor.w * r5.y;
        r9.xyz = -_GlobalWaterDeepColor.xyz + _GlobalWaterShallowColor.xyz;
        r9.xyz = r5.yyy * r9.xyz + _GlobalWaterDeepColor.xyz;
        r8.zw = r7.xy * float2(0.5,0.5) + v2.xz;
        r8.zw = -_GlobalOverallMapBoundMin.xz + r8.zw;
        r0.yz = r8.zw / r0.yz;
        r11.xyzw = _GlobalWaterAreaMap.SampleLevel(sampler_vd_linear_clamp, r0.yz, 0).xyzw;
        r0.y = -_GlobalWaterDepthRange.x + r11.z;
        r0.y = saturate(r0.y * r5.z);
        r0.z = r0.y * -2 + 3;
        r0.y = r0.y * r0.y;
        r0.y = r0.z * r0.y + -r6.w;
        r0.y = abs(r0.y) * abs(r7.w);
        r0.y = 3 * r0.y;
        r0.y = min(1, r0.y);
        r10.yzw = _GlobalWaterShallowColor.xyz + -r9.xyz;
        r9.xyz = r0.yyy * r10.yzw + r9.xyz;
        r10.yzw = _GlobalWaterShallowColor.xyz + -r9.xyz;
        r5.xyz = r5.xxx * r10.yzw + r9.xyz;
        r0.y = cmp(r2.w >= 0);
        r0.y = r0.y ? 1.000000 : 0;
        r0.z = 1 / _GlobalWaterSurfaceEdgeWidth;
        r0.z = saturate(r2.w * r0.z);
        r2.w = r0.z * -2 + 3;
        r0.z = r0.z * r0.z;
        r0.z = -r2.w * r0.z + 1;
        r2.w = _GlobalWaterShallowColor.w * r0.y;
        r2.w = r2.w * r4.w;
        r2.w = 0.5 * r2.w;
        r9.xyz = _GlobalWaterShallowColor.xyz + -r5.xyz;
        r5.xyz = r2.www * r9.xyz + r5.xyz;
        r2.w = cmp(vd_asint(_PlaneReflectionActive) != 0);
        r7.z = cmp(0.00999999978 < _GlobalWaterReflectionIntensity);
        r2.w = r2.w ? r7.z : 0;
        if (r2.w != 0) {
          r9.y = -0.5;
          r9.z = _GlobalWaterWaveParams.x;
          r9.yz = _Time.yy * r9.yz;
          r9.x = _GlobalWaterWaveParams.x * r9.y;
          r7.zw = float2(0.800000012,0.692799985) * r9.xz;
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
          r2.w = r6.w * 0.699999988 + 0.300000012;
          r2.w = r2.w * r4.w;
          r2.w = _GlobalWaterReflectionIntensity * r2.w;
          r2.w = _PlaneReflectionOpacity * r2.w;
          r7.xyz = r7.xyz * r2.www;
          r2.w = -r0.z * r0.y + 1;
          r5.xyz = r7.xyz * r2.www + r5.xyz;
        }
        r0.y = saturate(r0.z * r0.y + r5.w);
        r2.w = _GlobalWaterDeepColor.w * r4.w;
        r3.w = rsqrt(r3.w);
        r3.w = 1 / r3.w;
        r2.w = r3.w * r2.w;
        r0.y = r2.w * r0.y;
        r0.y = r0.y * r0.z;
        r5.xyz = r5.xyz + -r6.xyz;
        r6.xyz = r0.yyy * r5.xyz + r6.xyz;
      }
    }
  }
  r0.y = vd_asint(_MainLightLayerMask) & vd_asint(unity_RenderingLayer.x);
  r0.z = sqrt(unity_LightData.z);
  r0.z = min(1, r0.z);
  r2.w = dot(r2.xyz, _MainLightPosition.xyz);
  r2.w = saturate(r2.w * 0.5 + 0.5);
  r5.xyz = _MainLightColor.xyz * r0.zzz + -_GlobalBackgroundShadowColor.xyz;
  r5.xyz = r5.xyz * r2.www + _GlobalBackgroundShadowColor.xyz;
  r5.xyz = r5.xyz * r6.xyz;
  r5.xyz = r0.yyy ? r5.xyz : float3(0,0,0);
  r0.y = min(unity_LightData.y, _AdditionalLightsCount.x);
  r0.y = (int)r0.y;
  r7.xy = -_PlaneBoundMaxPos.xz + r4.xz;
  r7.zw = -_PlaneBoundMaxPos.xz + _PlaneBoundMinPos.xz;
  r7.xy = saturate(r7.xy / r7.zw);
  r7.xyzw = _PlaneSightRenderTarget.SampleLevel(sampler_PlaneSightRenderTarget, r7.xy, 0).xyzw;
  r0.z = -1 + r7.w;
  r0.z = _GlobalPlaneSystemEnable * r0.z;
  r7.xyz = float3(0,0,0);
  r2.w = 0;
  while (true) {
    r3.w = cmp((uint)r2.w >= (uint)r0.y);
    if (r3.w != 0) break;
    r3.w = (uint)r2.w >> 2;
    r4.w = (int)r2.w & 3;
    r3.w = dot(unity_LightIndices[(int)(r3.w)].xyzw, icb[r4.w+0].xyzw);
    r3.w = (int)r3.w;
    r8.xyz = -r4.xyz * _AdditionalLightsPosition[(int)(r3.w)].www + _AdditionalLightsPosition[(int)(r3.w)].xyz;
    r4.w = dot(r8.xyz, r8.xyz);
    r4.w = max(6.10351563e-005, r4.w);
    r5.w = rsqrt(r4.w);
    r8.xyz = r8.xyz * r5.www;
    r5.w = 1 / r4.w;
    r4.w = _AdditionalLightsAttenuation[(int)(r3.w)].x * r4.w;
    r4.w = -r4.w * r4.w + 1;
    r4.w = max(0, r4.w);
    r4.w = r4.w * r4.w;
    r4.w = r5.w * r4.w;
    r5.w = dot(_AdditionalLightsSpotDir[(int)(r3.w)].xyz, r8.xyz);
    r5.w = saturate(r5.w * _AdditionalLightsAttenuation[(int)(r3.w)].z + _AdditionalLightsAttenuation[(int)(r3.w)].w);
    r5.w = r5.w * r5.w;
    r4.w = r5.w * r4.w;
    r5.w = cmp(2 == vd_asint(_AdditionalLightsLayerMasks[(int)(r3.w)]));
    r5.w = r5.w ? 1.000000 : 0;
    r5.w = r5.w * r0.z + 1;
    r4.w = sqrt(r4.w);
    r4.w = min(1, r4.w);
    r9.xyz = _AdditionalLightsColor[(int)(r3.w)].xyz * r4.www;
    r3.w = saturate(dot(r2.xyz, r8.xyz));
    r8.xyz = r9.xyz * r3.www;
    r8.xyz = r8.xyz * r6.xyz;
    r7.xyz = r8.xyz * r5.www + r7.xyz;
    r2.w = (int)r2.w + 1;
  }
  r2.xyz = r7.xyz + r5.xyz;
  r0.yzw = r0.www * r1.yzw + r2.xyz;
  r2.x = dot(float2(0.530324996,0.530324996), v2.xz);
  r2.y = dot(float3(-0.353500009,0.865999997,0.353500009), v2.xyz);
  r1.yz = float2(0.125,0.125) * r2.xy;
  r4.xyzw = _DirtNoiseTexture.SampleBias(sampler_vd_point_repeat, r1.yz, _GlobalMipBias.x).xyzw;
  r1.y = r4.x * 0.5 + 0.5;
  r1.z = r1.y * r1.y;
  r1.y = 1 + -r1.y;
  r1.y = r1.y * r1.y;
  r4.xyz = r0.yzw * r0.yzw;
  r4.xyz = min(float3(1,1,1), r4.xyz);
  r1.yz = _DirtNoiseColor.ww * r1.yz;
  r1.yz = (_UseDirtNoise).xx * r1.yz;
  r4.xyz = r4.xyz + -r0.yzw;
  r0.yzw = r1.zzz * r4.xyz + r0.yzw;
  r4.xyz = saturate(r0.yzw + r0.yzw);
  r4.xyz = r4.xyz + -r0.yzw;
  r0.yzw = r1.yyy * r4.xyz + r0.yzw;
  if (r0.x != 0) {
    r1.yz = float2(0.400000006,0.400000006) * r2.xy;
    r4.xyzw = _DirtNoiseTexture.SampleBias(sampler_vd_point_repeat, r1.yz, _GlobalMipBias.x).xyzw;
    r5.xyzw = _Time.yyyy * float4(1.5,1.5,0.800000012,0.800000012) + v2.zxzx;
    r1.yz = sin(r5.xw);
    r2.zw = cos(r5.yz);
    r5.zw = float2(0.0250000004,0.0250000004) * r2.zw;
    r5.xy = float2(0.0250000004,0.0250000004) * r1.yz;
    r1.yz = r2.xy * float2(0.150000006,0.150000006) + r5.xz;
    r6.xyzw = _DirtNoiseTexture.SampleBias(sampler_vd_point_repeat, r1.yz, _GlobalMipBias.x).xyzw;
    r1.yz = r2.xy * float2(0.1875,0.1875) + r5.wy;
    r2.xyzw = _DirtNoiseTexture.SampleBias(sampler_vd_point_repeat, r1.yz, _GlobalMipBias.x).xyzw;
    r0.x = r6.x + r2.x;
    r0.x = saturate(0.5 * r0.x);
    r0.x = rsqrt(r0.x);
    r0.x = 1 / r0.x;
    r1.y = cmp(_GlobalBoxAreaIntensities[0] >= 0.00100000005);
    if (r1.y != 0) {
      r1.y = (int)_GlobalBoxAreaCenters[0].w;
      r1.zw = cmp((int2)r1.yy == int2(1,4));
      r1.z = (int)r1.w | (int)r1.z;
      if (r1.z == 0) {
        r2.xyz = -_GlobalBoxAreaCenters[0].xyz + v2.xyz;
        r2.xyz = _GlobalBoxAreaExtents[0].xyz + -abs(r2.xyz);
        r1.z = min(r2.y, r2.z);
        r1.z = min(r2.x, r1.z);
        r1.w = max(0.00100000005, _GlobalBoxAreaExtents[0].w);
        r1.z = saturate(r1.z / r1.w);
        r2.xyz = log2(abs(_GlobalBoxAreaColors[0].xyz));
        r2.xyz = float3(2.20000005,2.20000005,2.20000005) * r2.xyz;
        r2.xyz = exp2(r2.xyz);
        r1.y = cmp((int)r1.y == 3);
        if (r1.y != 0) {
          r1.y = -1 + r0.x;
          r1.y = _GlobalBoxAreaShimmerParams[0].z * r1.y + 1;
        } else {
          r1.y = 1;
        }
        r1.w = cmp(_GlobalBoxAreaIntensities[0] >= r4.x);
        r1.w = r1.w ? 1.000000 : 0;
        r1.z = r1.z * r1.w;
        r1.z = _GlobalBoxAreaColors[0].w * r1.z;
        r1.y = r1.z * r1.y;
        r2.xyz = r2.xyz + -r0.yzw;
        r0.yzw = r1.yyy * r2.xyz + r0.yzw;
      }
    }
    r1.y = cmp(_GlobalBoxAreaIntensities[1] >= 0.00100000005);
    if (r1.y != 0) {
      r1.y = (int)_GlobalBoxAreaCenters[1].w;
      r1.zw = cmp((int2)r1.yy == int2(1,4));
      r1.z = (int)r1.w | (int)r1.z;
      if (r1.z == 0) {
        r2.xyz = -_GlobalBoxAreaCenters[1].xyz + v2.xyz;
        r2.xyz = _GlobalBoxAreaExtents[1].xyz + -abs(r2.xyz);
        r1.z = min(r2.y, r2.z);
        r1.z = min(r2.x, r1.z);
        r1.w = max(0.00100000005, _GlobalBoxAreaExtents[1].w);
        r1.z = saturate(r1.z / r1.w);
        r2.xyz = log2(abs(_GlobalBoxAreaColors[1].xyz));
        r2.xyz = float3(2.20000005,2.20000005,2.20000005) * r2.xyz;
        r2.xyz = exp2(r2.xyz);
        r1.y = cmp((int)r1.y == 3);
        if (r1.y != 0) {
          r1.y = -1 + r0.x;
          r1.y = _GlobalBoxAreaShimmerParams[1].z * r1.y + 1;
        } else {
          r1.y = 1;
        }
        r1.w = cmp(_GlobalBoxAreaIntensities[1] >= r4.x);
        r1.w = r1.w ? 1.000000 : 0;
        r1.z = r1.z * r1.w;
        r1.z = _GlobalBoxAreaColors[1].w * r1.z;
        r1.y = r1.z * r1.y;
        r2.xyz = r2.xyz + -r0.yzw;
        r0.yzw = r1.yyy * r2.xyz + r0.yzw;
      }
    }
    r1.y = cmp(_GlobalBoxAreaIntensities[2] >= 0.00100000005);
    if (r1.y != 0) {
      r1.y = (int)_GlobalBoxAreaCenters[2].w;
      r1.zw = cmp((int2)r1.yy == int2(1,4));
      r1.z = (int)r1.w | (int)r1.z;
      if (r1.z == 0) {
        r2.xyz = -_GlobalBoxAreaCenters[2].xyz + v2.xyz;
        r2.xyz = _GlobalBoxAreaExtents[2].xyz + -abs(r2.xyz);
        r1.z = min(r2.y, r2.z);
        r1.z = min(r2.x, r1.z);
        r1.w = max(0.00100000005, _GlobalBoxAreaExtents[2].w);
        r1.z = saturate(r1.z / r1.w);
        r2.xyz = log2(abs(_GlobalBoxAreaColors[2].xyz));
        r2.xyz = float3(2.20000005,2.20000005,2.20000005) * r2.xyz;
        r2.xyz = exp2(r2.xyz);
        r1.y = cmp((int)r1.y == 3);
        if (r1.y != 0) {
          r1.y = -1 + r0.x;
          r1.y = _GlobalBoxAreaShimmerParams[2].z * r1.y + 1;
        } else {
          r1.y = 1;
        }
        r1.w = cmp(_GlobalBoxAreaIntensities[2] >= r4.x);
        r1.w = r1.w ? 1.000000 : 0;
        r1.z = r1.z * r1.w;
        r1.z = _GlobalBoxAreaColors[2].w * r1.z;
        r1.y = r1.z * r1.y;
        r2.xyz = r2.xyz + -r0.yzw;
        r0.yzw = r1.yyy * r2.xyz + r0.yzw;
      }
    }
    r1.y = cmp(_GlobalBoxAreaIntensities[3] >= 0.00100000005);
    if (r1.y != 0) {
      r1.y = (int)_GlobalBoxAreaCenters[3].w;
      r1.zw = cmp((int2)r1.yy == int2(1,4));
      r1.z = (int)r1.w | (int)r1.z;
      if (r1.z == 0) {
        r2.xyz = -_GlobalBoxAreaCenters[3].xyz + v2.xyz;
        r2.xyz = _GlobalBoxAreaExtents[3].xyz + -abs(r2.xyz);
        r1.z = min(r2.y, r2.z);
        r1.z = min(r2.x, r1.z);
        r1.w = max(0.00100000005, _GlobalBoxAreaExtents[3].w);
        r1.z = saturate(r1.z / r1.w);
        r2.xyz = log2(abs(_GlobalBoxAreaColors[3].xyz));
        r2.xyz = float3(2.20000005,2.20000005,2.20000005) * r2.xyz;
        r2.xyz = exp2(r2.xyz);
        r1.y = cmp((int)r1.y == 3);
        if (r1.y != 0) {
          r1.y = -1 + r0.x;
          r1.y = _GlobalBoxAreaShimmerParams[3].z * r1.y + 1;
        } else {
          r1.y = 1;
        }
        r1.w = cmp(_GlobalBoxAreaIntensities[3] >= r4.x);
        r1.w = r1.w ? 1.000000 : 0;
        r1.z = r1.z * r1.w;
        r1.z = _GlobalBoxAreaColors[3].w * r1.z;
        r1.y = r1.z * r1.y;
        r2.xyz = r2.xyz + -r0.yzw;
        r0.yzw = r1.yyy * r2.xyz + r0.yzw;
      }
    }
    r1.y = cmp(_GlobalBoxAreaIntensities[4] >= 0.00100000005);
    if (r1.y != 0) {
      r1.y = (int)_GlobalBoxAreaCenters[4].w;
      r1.zw = cmp((int2)r1.yy == int2(1,4));
      r1.z = (int)r1.w | (int)r1.z;
      if (r1.z == 0) {
        r2.xyz = -_GlobalBoxAreaCenters[4].xyz + v2.xyz;
        r2.xyz = _GlobalBoxAreaExtents[4].xyz + -abs(r2.xyz);
        r1.z = min(r2.y, r2.z);
        r1.z = min(r2.x, r1.z);
        r1.w = max(0.00100000005, _GlobalBoxAreaExtents[4].w);
        r1.z = saturate(r1.z / r1.w);
        r2.xyz = log2(abs(_GlobalBoxAreaColors[4].xyz));
        r2.xyz = float3(2.20000005,2.20000005,2.20000005) * r2.xyz;
        r2.xyz = exp2(r2.xyz);
        r1.y = cmp((int)r1.y == 3);
        if (r1.y != 0) {
          r1.y = -1 + r0.x;
          r1.y = _GlobalBoxAreaShimmerParams[4].z * r1.y + 1;
        } else {
          r1.y = 1;
        }
        r1.w = cmp(_GlobalBoxAreaIntensities[4] >= r4.x);
        r1.w = r1.w ? 1.000000 : 0;
        r1.z = r1.z * r1.w;
        r1.z = _GlobalBoxAreaColors[4].w * r1.z;
        r1.y = r1.z * r1.y;
        r2.xyz = r2.xyz + -r0.yzw;
        r0.yzw = r1.yyy * r2.xyz + r0.yzw;
      }
    }
    r1.y = cmp(_GlobalBoxAreaIntensities[5] >= 0.00100000005);
    if (r1.y != 0) {
      r1.y = (int)_GlobalBoxAreaCenters[5].w;
      r1.zw = cmp((int2)r1.yy == int2(1,4));
      r1.z = (int)r1.w | (int)r1.z;
      if (r1.z == 0) {
        r2.xyz = -_GlobalBoxAreaCenters[5].xyz + v2.xyz;
        r2.xyz = _GlobalBoxAreaExtents[5].xyz + -abs(r2.xyz);
        r1.z = min(r2.y, r2.z);
        r1.z = min(r2.x, r1.z);
        r1.w = max(0.00100000005, _GlobalBoxAreaExtents[5].w);
        r1.z = saturate(r1.z / r1.w);
        r2.xyz = log2(abs(_GlobalBoxAreaColors[5].xyz));
        r2.xyz = float3(2.20000005,2.20000005,2.20000005) * r2.xyz;
        r2.xyz = exp2(r2.xyz);
        r1.y = cmp((int)r1.y == 3);
        if (r1.y != 0) {
          r1.y = -1 + r0.x;
          r1.y = _GlobalBoxAreaShimmerParams[5].z * r1.y + 1;
        } else {
          r1.y = 1;
        }
        r1.w = cmp(_GlobalBoxAreaIntensities[5] >= r4.x);
        r1.w = r1.w ? 1.000000 : 0;
        r1.z = r1.z * r1.w;
        r1.z = _GlobalBoxAreaColors[5].w * r1.z;
        r1.y = r1.z * r1.y;
        r2.xyz = r2.xyz + -r0.yzw;
        r0.yzw = r1.yyy * r2.xyz + r0.yzw;
      }
    }
    r1.y = cmp(_GlobalBoxAreaIntensities[6] >= 0.00100000005);
    if (r1.y != 0) {
      r1.y = (int)_GlobalBoxAreaCenters[6].w;
      r1.zw = cmp((int2)r1.yy == int2(1,4));
      r1.z = (int)r1.w | (int)r1.z;
      if (r1.z == 0) {
        r2.xyz = -_GlobalBoxAreaCenters[6].xyz + v2.xyz;
        r2.xyz = _GlobalBoxAreaExtents[6].xyz + -abs(r2.xyz);
        r1.z = min(r2.y, r2.z);
        r1.z = min(r2.x, r1.z);
        r1.w = max(0.00100000005, _GlobalBoxAreaExtents[6].w);
        r1.z = saturate(r1.z / r1.w);
        r2.xyz = log2(abs(_GlobalBoxAreaColors[6].xyz));
        r2.xyz = float3(2.20000005,2.20000005,2.20000005) * r2.xyz;
        r2.xyz = exp2(r2.xyz);
        r1.y = cmp((int)r1.y == 3);
        if (r1.y != 0) {
          r1.y = -1 + r0.x;
          r1.y = _GlobalBoxAreaShimmerParams[6].z * r1.y + 1;
        } else {
          r1.y = 1;
        }
        r1.w = cmp(_GlobalBoxAreaIntensities[6] >= r4.x);
        r1.w = r1.w ? 1.000000 : 0;
        r1.z = r1.z * r1.w;
        r1.z = _GlobalBoxAreaColors[6].w * r1.z;
        r1.y = r1.z * r1.y;
        r2.xyz = r2.xyz + -r0.yzw;
        r0.yzw = r1.yyy * r2.xyz + r0.yzw;
      }
    }
    r1.y = cmp(_GlobalBoxAreaIntensities[7] >= 0.00100000005);
    r1.z = (int)_GlobalBoxAreaCenters[7].w;
    r2.xyz = cmp((int3)r1.zzz == int3(1,4,3));
    r1.z = (int)r2.y | (int)r2.x;
    r2.xyw = -_GlobalBoxAreaCenters[7].xyz + v2.xyz;
    r2.xyw = _GlobalBoxAreaExtents[7].xyz + -abs(r2.xyw);
    r1.w = min(r2.y, r2.w);
    r1.w = min(r2.x, r1.w);
    r2.x = max(0.00100000005, _GlobalBoxAreaExtents[7].w);
    r1.w = saturate(r1.w / r2.x);
    r2.xyw = log2(abs(_GlobalBoxAreaColors[7].xyz));
    r2.xyw = float3(2.20000005,2.20000005,2.20000005) * r2.xyw;
    r2.xyw = exp2(r2.xyw);
    if (r2.z != 0) {
      r0.x = -1 + r0.x;
      r0.x = _GlobalBoxAreaShimmerParams[7].z * r0.x + 1;
    } else {
      r0.x = 1;
    }
    r2.z = cmp(_GlobalBoxAreaIntensities[7] >= r4.x);
    r2.z = r2.z ? 1.000000 : 0;
    r1.w = r2.z * r1.w;
    r1.w = _GlobalBoxAreaColors[7].w * r1.w;
    r0.x = r1.w * r0.x;
    r2.xyz = r2.xyw + -r0.yzw;
    r2.xyz = r0.xxx * r2.xyz + r0.yzw;
    r2.xyz = r1.zzz ? r0.yzw : r2.xyz;
    r0.yzw = r1.yyy ? r2.xyz : r0.yzw;
  }
  r0.x = -_GlobalFogHeightMin + v2.y;
  r1.y = _GlobalFogHeightMax + -_GlobalFogHeightMin;
  r0.x = r0.x / r1.y;
  r0.x = saturate(1 + -r0.x);
  r0.x = _GlobalFogHeightEnable * r0.x;
  r0.x = max(0, r0.x);
  r1.yzw = unity_FogColor.xyz + -r0.yzw;
  r0.xyz = r0.xxx * r1.yzw + r0.yzw;
  r0.w = cmp(_Surface == 1.000000);
  o0.w = r0.w ? r1.x : 1;
  r1.xyz = (_EmissionIntensity).xxx * r3.xyz;
  r0.w = _Time.y + _Time.y;
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
  r2.xyz = r0.www * r2.xyz + r3.xyz;
  r2.xyz = sin(r2.xyz);
  r2.xyz = r2.xyz * float3(0.5,0.5,0.5) + float3(0.5,0.5,0.5);
  r0.w = 1.60000002 * _Time.y;
  r0.w = r1.w * 2 + r0.w;
  r0.w = sin(r0.w);
  r2.w = _Time.y * 2.5999999 + r2.w;
  r2.w = sin(r2.w);
  r0.w = r2.w * r0.w;
  r0.w = r0.w * 0.5 + 0.5;
  r1.w = _Time.y * 27.3999996 + r1.w;
  r1.w = sin(r1.w);
  r1.w = 43758.5469 * r1.w;
  r1.w = frac(r1.w);
  r1.w = cmp(r1.w >= 0.970000029);
  r2.x = dot(r2.xyz, float3(0.150000006,0.150000006,0.200000003));
  r0.w = r0.w * 0.300000012 + r2.x;
  r1.w = r1.w ? 0.200000 : 0;
  r0.w = r1.w + r0.w;
  r0.w = -0.75 + r0.w;
  r0.w = saturate(-9.99999809 * r0.w);
  r1.w = r0.w * -2 + 3;
  r0.w = r0.w * r0.w;
  r0.w = r1.w * r0.w + -1;
  r0.w = _EmissionFlickRate * r0.w + 1;
  r0.xyz = r1.xyz * r0.www + r0.xyz;
  r1.xyz = _GlobalBackgroundColor.xyz + -r0.xyz;
  o0.xyz = (_GlobalBackgroundColorIntensity).xxx * r1.xyz + r0.xyz;
  return;
}
