// LCArt/Background/shader_DE_BG_Base | pass 0 "ForwardLit" | progFragment | keywords: FOG_LINEAR _CURSOR_LINE_ENABLE _DE_EDITOR_CAMERA _LIGHT_LAYERS | blob 51
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
  r0.w = _BaseColor.w * r1.w;
  r2.xyzw = _EmissionMap.SampleLevel(sampler_vd_point_repeat, r0.yz, 0).xyzw;
  r1.w = 0.100000001 + -r2.z;
  r1.w = cmp(r1.w < 0);
  if (r1.w != 0) discard;
  r1.w = cmp(r0.w < 0.5);
  if (r1.w != 0) discard;
  r1.w = cmp(1.5 < v1.w);
  if (r1.w != 0) {
    r2.xz = float2(1,1) / _ScaledScreenParams.xy;
    r3.xy = v7.xy * r2.xz;
    r1.w = r3.y * _ScaleBiasRt.x + _ScaleBiasRt.y;
    r3.z = 1 + -r1.w;
    r1.w = 1 + -_GlobalSeeThroughAlpha;
    r1.w = w0.x * r1.w + _GlobalSeeThroughAlpha;
    r2.x = cmp(vd_asint(_BackgroundType) == 9);
    r2.z = v8.w * r1.w;
    r1.w = -1 + r1.w;
    r1.w = _GlobalIsRuntime * r1.w + 1;
    r1.w = r2.x ? r2.z : r1.w;
    r2.xz = _ScreenSize.xy / _ScreenParams.xy;
    r3.xy = _ScreenParams.xy * r3.xz;
    r2.xz = r3.xy * r2.xz;
    r2.xz = floor(r2.xz);
    r2.xz = (uint2)r2.xz;
    r2.xz = (int2)r2.xz & int2(3,3);
    r2.z = (uint)r2.z << 2;
    r2.x = (int)r2.z + (int)r2.x;
    r2.x = x0[r2.x+0].x;
    r1.w = r1.w * _BaseColor.w + -r2.x;
    r1.w = cmp(r1.w < 0);
    if (r1.w != 0) discard;
  }
  r1.xyz = _BaseColor.xyz * r1.xyz;
  r3.xyzw = _BumpMap.SampleBias(sampler_vd_point_repeat, r0.yz, _GlobalMipBias.x).xyzw;
  r3.x = r3.w * r3.x;
  r4.xy = r3.xy + r3.xy;
  r2.xz = r3.xy * float2(2,2) + float2(-1,-1);
  r1.w = dot(r2.xz, r2.xz);
  r1.w = min(1, r1.w);
  r1.w = 1 + -r1.w;
  r1.w = sqrt(r1.w);
  r4.z = max(1.00000002e-016, r1.w);
  r2.xzw = float3(-1,-1,-1) + r4.xyz;
  r2.xzw = (_BumpScale).xxx * r2.xzw + float3(0,0,1);
  r3.xyzw = _EmissionMap.SampleBias(sampler_vd_point_repeat, r0.yz, _GlobalMipBias.x).xyzw;
  r1.w = cmp(r3.z >= 0.100000001);
  r1.w = r1.w ? 0 : 1;
  r1.w = r3.x * r1.w;
  r3.xyz = r1.www * r1.xyz;
  if (r0.x != 0) {
    r4.xy = float2(0.5,0.5) * v2.xz;
    r4.zw = floor(r4.xy);
    r4.xy = frac(r4.xy);
    r5.xy = r4.xy * r4.xy;
    r4.xy = -r4.xy * float2(2,2) + float2(3,3);
    r4.xy = r5.xy * r4.xy;
    r3.w = dot(r4.zw, float2(127.099998,311.700012));
    r3.w = sin(r3.w);
    r3.w = 43758.5469 * r3.w;
    r3.w = frac(r3.w);
    r5.xyzw = float4(1,0,0,1) + r4.zwzw;
    r5.x = dot(r5.xy, float2(127.099998,311.700012));
    r5.x = sin(r5.x);
    r5.x = 43758.5469 * r5.x;
    r5.y = dot(r5.zw, float2(127.099998,311.700012));
    r5.y = sin(r5.y);
    r5.y = 43758.5469 * r5.y;
    r5.xy = frac(r5.xy);
    r4.zw = float2(1,1) + r4.zw;
    r4.z = dot(r4.zw, float2(127.099998,311.700012));
    r4.z = sin(r4.z);
    r4.z = 43758.5469 * r4.z;
    r4.z = frac(r4.z);
    r4.w = r5.x + -r3.w;
    r3.w = r4.x * r4.w + r3.w;
    r4.z = r4.z + -r5.y;
    r4.x = r4.x * r4.z + r5.y;
    r4.x = r4.x + -r3.w;
    r3.w = r4.y * r4.x + r3.w;
    r4.x = (int)_GlobalBoxAreaCenters[0].w;
    r4.x = cmp((int)r4.x == 4);
    if (r4.x != 0) {
      r4.xyz = -_GlobalBoxAreaCenters[0].xyz + v2.xyz;
      r4.xyz = _GlobalBoxAreaExtents[0].xyz + -abs(r4.xyz);
      r4.y = min(r4.y, r4.z);
      r4.x = min(r4.x, r4.y);
      r4.y = max(0.00100000005, _GlobalBoxAreaExtents[0].w);
      r4.x = saturate(r4.x / r4.y);
      r4.y = cmp(0 < r4.x);
      if (r4.y != 0) {
        r4.y = 1.125 + -_GlobalBoxAreaIntensities[0];
        r4.y = -r4.y + r3.w;
        r4.y = saturate(-4 * r4.y);
        r4.z = r4.y * -2 + 3;
        r4.y = r4.y * r4.y;
        r4.y = r4.z * r4.y + -0.5;
        r4.x = r4.y + -r4.x;
        r4.x = 1 + r4.x;
        r4.x = cmp(r4.x < 0);
        if (r4.x != 0) discard;
      }
    }
    r4.x = (int)_GlobalBoxAreaCenters[1].w;
    r4.x = cmp((int)r4.x == 4);
    if (r4.x != 0) {
      r4.xyz = -_GlobalBoxAreaCenters[1].xyz + v2.xyz;
      r4.xyz = _GlobalBoxAreaExtents[1].xyz + -abs(r4.xyz);
      r4.y = min(r4.y, r4.z);
      r4.x = min(r4.x, r4.y);
      r4.y = max(0.00100000005, _GlobalBoxAreaExtents[1].w);
      r4.x = saturate(r4.x / r4.y);
      r4.y = cmp(0 < r4.x);
      if (r4.y != 0) {
        r4.y = 1.125 + -_GlobalBoxAreaIntensities[1];
        r4.y = -r4.y + r3.w;
        r4.y = saturate(-4 * r4.y);
        r4.z = r4.y * -2 + 3;
        r4.y = r4.y * r4.y;
        r4.y = r4.z * r4.y + -0.5;
        r4.x = r4.y + -r4.x;
        r4.x = 1 + r4.x;
        r4.x = cmp(r4.x < 0);
        if (r4.x != 0) discard;
      }
    }
    r4.x = (int)_GlobalBoxAreaCenters[2].w;
    r4.x = cmp((int)r4.x == 4);
    if (r4.x != 0) {
      r4.xyz = -_GlobalBoxAreaCenters[2].xyz + v2.xyz;
      r4.xyz = _GlobalBoxAreaExtents[2].xyz + -abs(r4.xyz);
      r4.y = min(r4.y, r4.z);
      r4.x = min(r4.x, r4.y);
      r4.y = max(0.00100000005, _GlobalBoxAreaExtents[2].w);
      r4.x = saturate(r4.x / r4.y);
      r4.y = cmp(0 < r4.x);
      if (r4.y != 0) {
        r4.y = 1.125 + -_GlobalBoxAreaIntensities[2];
        r4.y = -r4.y + r3.w;
        r4.y = saturate(-4 * r4.y);
        r4.z = r4.y * -2 + 3;
        r4.y = r4.y * r4.y;
        r4.y = r4.z * r4.y + -0.5;
        r4.x = r4.y + -r4.x;
        r4.x = 1 + r4.x;
        r4.x = cmp(r4.x < 0);
        if (r4.x != 0) discard;
      }
    }
    r4.x = (int)_GlobalBoxAreaCenters[3].w;
    r4.x = cmp((int)r4.x == 4);
    if (r4.x != 0) {
      r4.xyz = -_GlobalBoxAreaCenters[3].xyz + v2.xyz;
      r4.xyz = _GlobalBoxAreaExtents[3].xyz + -abs(r4.xyz);
      r4.y = min(r4.y, r4.z);
      r4.x = min(r4.x, r4.y);
      r4.y = max(0.00100000005, _GlobalBoxAreaExtents[3].w);
      r4.x = saturate(r4.x / r4.y);
      r4.y = cmp(0 < r4.x);
      if (r4.y != 0) {
        r4.y = 1.125 + -_GlobalBoxAreaIntensities[3];
        r4.y = -r4.y + r3.w;
        r4.y = saturate(-4 * r4.y);
        r4.z = r4.y * -2 + 3;
        r4.y = r4.y * r4.y;
        r4.y = r4.z * r4.y + -0.5;
        r4.x = r4.y + -r4.x;
        r4.x = 1 + r4.x;
        r4.x = cmp(r4.x < 0);
        if (r4.x != 0) discard;
      }
    }
    r4.x = (int)_GlobalBoxAreaCenters[4].w;
    r4.x = cmp((int)r4.x == 4);
    if (r4.x != 0) {
      r4.xyz = -_GlobalBoxAreaCenters[4].xyz + v2.xyz;
      r4.xyz = _GlobalBoxAreaExtents[4].xyz + -abs(r4.xyz);
      r4.y = min(r4.y, r4.z);
      r4.x = min(r4.x, r4.y);
      r4.y = max(0.00100000005, _GlobalBoxAreaExtents[4].w);
      r4.x = saturate(r4.x / r4.y);
      r4.y = cmp(0 < r4.x);
      if (r4.y != 0) {
        r4.y = 1.125 + -_GlobalBoxAreaIntensities[4];
        r4.y = -r4.y + r3.w;
        r4.y = saturate(-4 * r4.y);
        r4.z = r4.y * -2 + 3;
        r4.y = r4.y * r4.y;
        r4.y = r4.z * r4.y + -0.5;
        r4.x = r4.y + -r4.x;
        r4.x = 1 + r4.x;
        r4.x = cmp(r4.x < 0);
        if (r4.x != 0) discard;
      }
    }
    r4.x = (int)_GlobalBoxAreaCenters[5].w;
    r4.x = cmp((int)r4.x == 4);
    if (r4.x != 0) {
      r4.xyz = -_GlobalBoxAreaCenters[5].xyz + v2.xyz;
      r4.xyz = _GlobalBoxAreaExtents[5].xyz + -abs(r4.xyz);
      r4.y = min(r4.y, r4.z);
      r4.x = min(r4.x, r4.y);
      r4.y = max(0.00100000005, _GlobalBoxAreaExtents[5].w);
      r4.x = saturate(r4.x / r4.y);
      r4.y = cmp(0 < r4.x);
      if (r4.y != 0) {
        r4.y = 1.125 + -_GlobalBoxAreaIntensities[5];
        r4.y = -r4.y + r3.w;
        r4.y = saturate(-4 * r4.y);
        r4.z = r4.y * -2 + 3;
        r4.y = r4.y * r4.y;
        r4.y = r4.z * r4.y + -0.5;
        r4.x = r4.y + -r4.x;
        r4.x = 1 + r4.x;
        r4.x = cmp(r4.x < 0);
        if (r4.x != 0) discard;
      }
    }
    r4.x = (int)_GlobalBoxAreaCenters[6].w;
    r4.x = cmp((int)r4.x == 4);
    if (r4.x != 0) {
      r4.xyz = -_GlobalBoxAreaCenters[6].xyz + v2.xyz;
      r4.xyz = _GlobalBoxAreaExtents[6].xyz + -abs(r4.xyz);
      r4.y = min(r4.y, r4.z);
      r4.x = min(r4.x, r4.y);
      r4.y = max(0.00100000005, _GlobalBoxAreaExtents[6].w);
      r4.x = saturate(r4.x / r4.y);
      r4.y = cmp(0 < r4.x);
      if (r4.y != 0) {
        r4.y = 1.125 + -_GlobalBoxAreaIntensities[6];
        r4.y = -r4.y + r3.w;
        r4.y = saturate(-4 * r4.y);
        r4.z = r4.y * -2 + 3;
        r4.y = r4.y * r4.y;
        r4.y = r4.z * r4.y + -0.5;
        r4.x = r4.y + -r4.x;
        r4.x = 1 + r4.x;
        r4.x = cmp(r4.x < 0);
        if (r4.x != 0) discard;
      }
    }
    r4.x = (int)_GlobalBoxAreaCenters[7].w;
    r4.x = cmp((int)r4.x == 4);
    r4.yzw = -_GlobalBoxAreaCenters[7].xyz + v2.xyz;
    r4.yzw = _GlobalBoxAreaExtents[7].xyz + -abs(r4.yzw);
    r4.z = min(r4.z, r4.w);
    r4.y = min(r4.y, r4.z);
    r4.z = max(0.00100000005, _GlobalBoxAreaExtents[7].w);
    r4.y = saturate(r4.y / r4.z);
    r4.z = cmp(0 < r4.y);
    r4.x = r4.z ? r4.x : 0;
    r4.z = 1.125 + -_GlobalBoxAreaIntensities[7];
    r3.w = -r4.z + r3.w;
    r3.w = saturate(-4 * r3.w);
    r4.z = r3.w * -2 + 3;
    r3.w = r3.w * r3.w;
    r3.w = r4.z * r3.w + -0.5;
    r3.w = r3.w + -r4.y;
    r3.w = 1 + r3.w;
    r3.w = cmp(r3.w < 0);
    r3.w = r4.x ? r3.w : 0;
    if (r3.w != 0) discard;
  }
  r3.w = cmp(0.5 < _UseWorldNormalMap);
  if (r3.w != 0) {
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
    r6.xyz = r6.xyz * r2.zzz;
    r5.xyz = r2.xxx * r5.xyz + r6.xyz;
    r4.xyz = r2.www * r4.xyz + r5.xyz;
  } else {
    r5.xyz = v5.xyz * r2.zzz;
    r5.xyz = r2.xxx * v4.xyz + r5.xyz;
    r4.xyz = r2.www * v3.xyz + r5.xyz;
  }
  r0.y = dot(r4.xyz, r4.xyz);
  r0.y = rsqrt(r0.y);
  r4.yzw = r4.xyz * r0.yyy;
  r0.y = cmp(0.5 < v1.z);
  r0.z = cmp(v1.z < 1.5);
  r0.y = r0.z ? r0.y : 0;
  r0.z = cmp(0.125 >= v11.y);
  r0.y = r0.z ? r0.y : 0;
  r5.x = v11.y;
  r5.yzw = float3(0,1,0);
  r4.x = v2.y;
  r5.xyzw = r0.yyyy ? r5.xyzw : r4.xyzw;
  r2.xzw = cmp(vd_asint((_BackgroundType).xxx) == int3(0,1,11));
  r0.z = (int)r2.z | (int)r2.x;
  r2.x = cmp(0 >= r5.x);
  r2.z = cmp(r5.z >= 0.850000024);
  r0.yz = (int2)r0.yz | (int2)r2.zw;
  r4.xyz = r0.yyy ? float3(0,1,0) : r4.yzw;
  r2.xzw = r2.xxx ? r4.xyz : r5.yzw;
  r2.xzw = r0.zzz ? r2.xzw : r5.yzw;
  r0.yz = _GlobalOverallMapBoundMax.xz + -_GlobalOverallMapBoundMin.xz;
  r0.yz = max(float2(1,1), r0.yz);
  r4.xy = -_GlobalOverallMapBoundMin.xz + v2.xz;
  r4.xy = r4.xy / r0.yz;
  r5.xyzw = _GlobalWaterAreaMap.SampleBias(sampler_vd_linear_clamp, r4.xy, _GlobalMipBias.x).xyzw;
  r3.w = cmp(_WeatherIntensity >= 0.00100000005);
  r4.z = 0.899999976 + r5.x;
  r4.z = saturate(0.5 * r4.z);
  r4.w = r4.z * -2 + 3;
  r4.z = r4.z * r4.z;
  r4.z = r4.w * r4.z;
  r4.z = max(_WeatherIndoorMask, r4.z);
  r4.z = 1 + -r4.z;
  r4.z = _WeatherIntensity * r4.z;
  r4.w = cmp(r4.z >= 0.00100000005);
  r5.x = _WeatherWetSurfaceIntensity * r4.z;
  r5.x = r5.x * -0.300000012 + 1;
  r5.yzw = r5.xxx * r1.xyz;
  r4.z = _WeatherBgTintIntensity * r4.z;
  r6.x = cmp(0.00100000005 < r4.z);
  r6.y = dot(r5.yzw, float3(0.298999995,0.587000012,0.114));
  r6.z = _WeatherBgDesaturation * r4.z;
  r7.xyz = -r1.xyz * r5.xxx + r6.yyy;
  r6.yzw = r6.zzz * r7.xyz + r5.yzw;
  r4.z = 0.300000012 * r4.z;
  r7.xyz = _WeatherBgTintColor.xyz + -r6.yzw;
  r6.yzw = r4.zzz * r7.xyz + r6.yzw;
  r5.xyz = r6.xxx ? r6.yzw : r5.yzw;
  r5.xyz = r4.www ? r5.xyz : r1.xyz;
  r5.xyz = r3.www ? r5.xyz : r1.xyz;
  if (_GlobalWaterAreaEnable != 0) {
    r4.xyzw = _GlobalWaterAreaMap.SampleLevel(sampler_vd_linear_clamp, r4.xy, 0).xyzw;
    r3.w = cmp(0.100000001 < r4.z);
    if (r3.w != 0) {
      r4.xy = _GlobalWaterDepthRange.ww * v2.xz;
      r6.xyzw = _GlobalWaterNoiseTex.SampleLevel(sampler_vd_linear_repeat, r4.xy, 0).xyzw;
      r3.w = -0.5 + r6.y;
      r3.w = _GlobalWaterDepthRange.z * r3.w;
      r4.x = r3.w * r4.z;
      r6.xy = float2(0.800000012,0.600000024) * _GlobalWaterWaveParams.yx;
      r6.z = -0.300000012;
      r4.yw = _Time.yy * r6.yz;
      r7.x = 0.400000006;
      r7.y = 0.600000024 * _GlobalWaterWaveParams.x;
      r4.yw = r7.xy * r4.yw;
      r4.yw = v2.xz * r6.xx + r4.yw;
      r6.xyzw = _GlobalWaterNoiseTex.SampleLevel(sampler_vd_linear_repeat, r4.yw, 0).xyzw;
      r4.y = -0.5 + r4.z;
      r4.y = 5 * abs(r4.y);
      r4.y = min(1, r4.y);
      r4.y = 1 + -r4.y;
      r4.w = -0.5 + r6.y;
      r4.x = r4.w * r4.y + r4.x;
      r4.x = r4.z + r4.x;
      r4.y = _GlobalWaterShoreEdge.y + -_GlobalWaterShoreEdge.x;
      r4.w = -_GlobalWaterShoreEdge.x + r4.x;
      r4.y = 1 / r4.y;
      r4.y = saturate(r4.w * r4.y);
      r4.w = r4.y * -2 + 3;
      r4.y = r4.y * r4.y;
      r4.y = r4.w * r4.y;
      r6.xy = (_GlobalWaterShoreStroke).xx + _GlobalWaterShoreEdge.xy;
      r4.w = r6.y + -r6.x;
      r5.w = -r6.x + r4.x;
      r4.w = 1 / r4.w;
      r4.w = saturate(r5.w * r4.w);
      r5.w = r4.w * -2 + 3;
      r4.w = r4.w * r4.w;
      r4.w = -r5.w * r4.w + 1;
      r3.w = r3.w * r4.z + r4.z;
      r4.z = _GlobalWaterDepthRange.y + -_GlobalWaterDepthRange.x;
      r3.w = -_GlobalWaterDepthRange.x + r3.w;
      r4.z = 1 / r4.z;
      r3.w = saturate(r4.z * r3.w);
      r5.w = r3.w * -2 + 3;
      r3.w = r3.w * r3.w;
      r6.x = r5.w * r3.w;
      r4.w = r4.y * r4.w;
      r6.y = cmp(r4.y >= 0.00999999978);
      if (r6.y != 0) {
        r6.yz = cmp(float2(0.00999999978,0.00999999978) < _GlobalWaterSurfaceWaveMapParams.zw);
        r6.y = r6.z ? r6.y : 0;
        if (r6.y != 0) {
          r6.yz = -_GlobalWaterSurfaceWaveMapParams.xy + v2.xz;
          r6.yz = r6.yz / _GlobalWaterSurfaceWaveMapParams.zw;
          r6.yz = float2(0.5,0.5) + r6.yz;
          r7.xy = float2(1,1) + -r6.yz;
          r7.xy = min(r7.xy, r6.yz);
          r6.w = min(r7.x, r7.y);
          r6.w = saturate(_GlobalWaterSurfaceWaveMapParams.z * r6.w);
          r7.xyzw = _GlobalWaterSurfaceWaveMap.SampleLevel(sampler_vd_linear_clamp, r6.yz, 0).xyzw;
          r6.yz = r7.yz * float2(2,2) + float2(-1,-1);
          r7.xy = r7.wx * r6.ww;
        } else {
          r6.yz = float2(0,0);
          r7.xy = float2(0,0);
        }
        r3.w = -r5.w * r3.w + 1;
        r3.w = -_GlobalWaterShoreHeightDrop * r3.w + _GlobalWaterSurfaceLevel;
        r3.w = r7.y * 0.5 + r3.w;
        r5.w = -_GlobalWaterDepthOffset + v2.y;
        r3.w = r5.w + -r3.w;
        r5.w = saturate(r3.w * -2 + 0.5);
        r7.zw = _GlobalWaterWaveParams.yy * v2.xz;
        r6.w = _GlobalWaterWaveParams.x * _Time.y;
        r8.xyz = float3(0.707099974,-0.780000031,0.519999981) * r6.www;
        r8.xw = v2.xz * _GlobalWaterWaveParams.yy + r8.xx;
        r9.xyzw = _GlobalWaterNoiseTex.SampleLevel(sampler_vd_linear_repeat, r8.xw, 0).xyzw;
        r10.xw = _GlobalWaterWaveParams.xx;
        r10.yz = float2(0.699999988,0.300000012);
        r8.xw = _Time.yy * r10.xy;
        r8.xw = r8.xw * r10.zw;
        r8.xw = r7.zw * float2(1.5,1.5) + r8.xw;
        r8.yz = r7.zw * float2(2,2) + r8.yz;
        r10.xyzw = _GlobalWaterNormalMap.SampleLevel(sampler_vd_linear_repeat, r8.xw, 0).xyzw;
        r9.yzw = r10.xyz * float3(2,2,2) + float3(-1,-1,-1);
        r8.xyzw = _GlobalWaterNormalMap.SampleLevel(sampler_vd_linear_repeat, r8.yz, 0).xyzw;
        r8.xyz = r8.xyz * float3(2,2,2) + float3(-1,-1,-1);
        r10.xy = r9.yz + r8.xy;
        r10.z = r9.w * r8.z;
        r6.w = dot(r10.xyz, r10.xyz);
        r6.w = rsqrt(r6.w);
        r8.xyz = r10.xyz * r6.www;
        r6.w = saturate(dot(r8.xzy, float3(0.300000012,0.300000012,0.899999976)));
        r6.w = r6.w * r6.x;
        r6.w = 0.150000006 * r6.w;
        r6.yz = r7.xx * r6.yz;
        r7.x = log2(abs(r9.x));
        r7.x = _GlobalWaterWaveParams.z * r7.x;
        r7.x = exp2(r7.x);
        r7.x = min(1, r7.x);
        r7.x = _GlobalWaterWaveParams.w * r7.x;
        r4.w = _GlobalWaterShoreIntensity * r4.w;
        r4.w = max(r4.w, r7.x);
        r4.w = _GlobalWaterShallowColor.w * r4.w;
        r8.xyz = -_GlobalWaterDeepColor.xyz + _GlobalWaterShallowColor.xyz;
        r8.xyz = r4.www * r8.xyz + _GlobalWaterDeepColor.xyz;
        r9.yz = r6.yz * float2(0.5,0.5) + v2.xz;
        r9.yz = -_GlobalOverallMapBoundMin.xz + r9.yz;
        r0.yz = r9.yz / r0.yz;
        r10.xyzw = _GlobalWaterAreaMap.SampleLevel(sampler_vd_linear_clamp, r0.yz, 0).xyzw;
        r0.y = -_GlobalWaterDepthRange.x + r10.z;
        r0.y = saturate(r0.y * r4.z);
        r0.z = r0.y * -2 + 3;
        r0.y = r0.y * r0.y;
        r0.y = r0.z * r0.y + -r6.x;
        r0.y = abs(r0.y) * abs(r7.y);
        r0.y = 3 * r0.y;
        r0.y = min(1, r0.y);
        r9.yzw = _GlobalWaterShallowColor.xyz + -r8.xyz;
        r8.xyz = r0.yyy * r9.yzw + r8.xyz;
        r9.yzw = _GlobalWaterShallowColor.xyz + -r8.xyz;
        r8.xyz = r6.www * r9.yzw + r8.xyz;
        r0.y = cmp(r3.w >= 0);
        r0.y = r0.y ? 1.000000 : 0;
        r0.z = 1 / _GlobalWaterSurfaceEdgeWidth;
        r0.z = saturate(r3.w * r0.z);
        r3.w = r0.z * -2 + 3;
        r0.z = r0.z * r0.z;
        r0.z = -r3.w * r0.z + 1;
        r3.w = _GlobalWaterShallowColor.w * r0.y;
        r3.w = r3.w * r4.y;
        r3.w = 0.5 * r3.w;
        r9.yzw = _GlobalWaterShallowColor.xyz + -r8.xyz;
        r8.xyz = r3.www * r9.yzw + r8.xyz;
        r3.w = cmp(vd_asint(_PlaneReflectionActive) != 0);
        r4.z = cmp(0.00999999978 < _GlobalWaterReflectionIntensity);
        r3.w = r3.w ? r4.z : 0;
        if (r3.w != 0) {
          r9.y = -0.5;
          r9.z = _GlobalWaterWaveParams.x;
          r10.yz = _Time.yy * r9.yz;
          r10.x = _GlobalWaterWaveParams.x * r10.y;
          r4.zw = float2(0.800000012,0.692799985) * r10.xz;
          r4.zw = r7.zw * float2(0.699999988,0.699999988) + r4.zw;
          r7.xyzw = _GlobalWaterNoiseTex.SampleLevel(sampler_vd_linear_repeat, r4.zw, 0).yxzw;
          r7.x = r9.x;
          r4.zw = float2(-0.5,-0.5) + r7.xy;
          r4.zw = r4.zw + r4.zw;
          r4.zw = _GlobalWaterWaveParams2.xx * r4.zw;
          r7.xy = v7.xy / _ScreenParams.xy;
          r4.zw = r4.zw * float2(0.5,0.5) + r7.xy;
          r4.zw = r6.yz * float2(0.0199999996,0.0199999996) + r4.zw;
          r4.w = _GlobalWaterReflectionYOffset + r4.w;
          r7.xyzw = _PlaneReflectionTexture.SampleLevel(sampler_vd_linear_clamp, r4.zw, 0).xyzw;
          r3.w = r6.x * 0.699999988 + 0.300000012;
          r3.w = r3.w * r4.y;
          r3.w = _GlobalWaterReflectionIntensity * r3.w;
          r3.w = _PlaneReflectionOpacity * r3.w;
          r6.xyz = r7.xyz * r3.www;
          r3.w = -r0.z * r0.y + 1;
          r8.xyz = r6.xyz * r3.www + r8.xyz;
        }
        r0.y = saturate(r0.z * r0.y + r5.w);
        r3.w = _GlobalWaterDeepColor.w * r4.y;
        r4.x = rsqrt(r4.x);
        r4.x = 1 / r4.x;
        r3.w = r4.x * r3.w;
        r0.y = r3.w * r0.y;
        r0.y = r0.y * r0.z;
        r4.xyz = r8.xyz + -r5.xyz;
        r5.xyz = r0.yyy * r4.xyz + r5.xyz;
      }
    }
  }
  r0.y = vd_asint(_MainLightLayerMask) & vd_asint(unity_RenderingLayer.x);
  r0.z = sqrt(unity_LightData.z);
  r0.z = min(1, r0.z);
  r2.x = dot(r2.xzw, _MainLightPosition.xyz);
  r2.x = saturate(r2.x * 0.5 + 0.5);
  r4.xyz = _MainLightColor.xyz * r0.zzz + -_GlobalBackgroundShadowColor.xyz;
  r2.xzw = r4.xyz * r2.xxx + _GlobalBackgroundShadowColor.xyz;
  r2.xzw = r2.xzw * r5.xyz;
  r2.xzw = r0.yyy ? r2.xzw : float3(0,0,0);
  r1.xyz = r1.www * r1.xyz + r2.xzw;
  r4.x = dot(float2(0.530324996,0.530324996), v2.xz);
  r4.y = dot(float3(-0.353500009,0.865999997,0.353500009), v2.xyz);
  r0.yz = float2(0.125,0.125) * r4.xy;
  r5.xyzw = _DirtNoiseTexture.SampleBias(sampler_vd_point_repeat, r0.yz, _GlobalMipBias.x).xyzw;
  r0.y = r5.x * 0.5 + 0.5;
  r0.z = r0.y * r0.y;
  r0.y = 1 + -r0.y;
  r0.y = r0.y * r0.y;
  r2.xzw = r1.xyz * r1.xyz;
  r2.xzw = min(float3(1,1,1), r2.xzw);
  r0.yz = _DirtNoiseColor.ww * r0.yz;
  r0.yz = (_UseDirtNoise).xx * r0.yz;
  r2.xzw = r2.xzw + -r1.xyz;
  r1.xyz = r0.zzz * r2.xzw + r1.xyz;
  r2.xzw = saturate(r1.xyz + r1.xyz);
  r2.xzw = r2.xzw + -r1.xyz;
  r1.xyz = r0.yyy * r2.xzw + r1.xyz;
  if (r0.x != 0) {
    r0.xy = float2(0.400000006,0.400000006) * r4.xy;
    r5.xyzw = _DirtNoiseTexture.SampleBias(sampler_vd_point_repeat, r0.xy, _GlobalMipBias.x).xyzw;
    r6.xyzw = _Time.yyyy * float4(1.5,1.5,0.800000012,0.800000012) + v2.zxzx;
    r0.xy = sin(r6.xw);
    r2.xz = cos(r6.yz);
    r6.zw = float2(0.0250000004,0.0250000004) * r2.xz;
    r6.xy = float2(0.0250000004,0.0250000004) * r0.xy;
    r0.xy = r4.xy * float2(0.150000006,0.150000006) + r6.xz;
    r7.xyzw = _DirtNoiseTexture.SampleBias(sampler_vd_point_repeat, r0.xy, _GlobalMipBias.x).xyzw;
    r0.xy = r4.xy * float2(0.1875,0.1875) + r6.wy;
    r4.xyzw = _DirtNoiseTexture.SampleBias(sampler_vd_point_repeat, r0.xy, _GlobalMipBias.x).xyzw;
    r0.x = r7.x + r4.x;
    r0.x = saturate(0.5 * r0.x);
    r0.x = rsqrt(r0.x);
    r0.x = 1 / r0.x;
    r0.y = cmp(_GlobalBoxAreaIntensities[0] >= 0.00100000005);
    if (r0.y != 0) {
      r0.y = (int)_GlobalBoxAreaCenters[0].w;
      r2.xz = cmp((int2)r0.yy == int2(1,4));
      r0.z = (int)r2.z | (int)r2.x;
      if (r0.z == 0) {
        r2.xzw = -_GlobalBoxAreaCenters[0].xyz + v2.xyz;
        r2.xzw = _GlobalBoxAreaExtents[0].xyz + -abs(r2.xzw);
        r0.z = min(r2.z, r2.w);
        r0.z = min(r2.x, r0.z);
        r1.w = max(0.00100000005, _GlobalBoxAreaExtents[0].w);
        r0.z = saturate(r0.z / r1.w);
        r2.xzw = log2(abs(_GlobalBoxAreaColors[0].xyz));
        r2.xzw = float3(2.20000005,2.20000005,2.20000005) * r2.xzw;
        r2.xzw = exp2(r2.xzw);
        r0.y = cmp((int)r0.y == 3);
        if (r0.y != 0) {
          r0.y = -1 + r0.x;
          r0.y = _GlobalBoxAreaShimmerParams[0].z * r0.y + 1;
        } else {
          r0.y = 1;
        }
        r1.w = cmp(_GlobalBoxAreaIntensities[0] >= r5.x);
        r1.w = r1.w ? 1.000000 : 0;
        r0.z = r1.w * r0.z;
        r0.z = _GlobalBoxAreaColors[0].w * r0.z;
        r0.y = r0.z * r0.y;
        r2.xzw = r2.xzw + -r1.xyz;
        r1.xyz = r0.yyy * r2.xzw + r1.xyz;
      }
    }
    r0.y = cmp(_GlobalBoxAreaIntensities[1] >= 0.00100000005);
    if (r0.y != 0) {
      r0.y = (int)_GlobalBoxAreaCenters[1].w;
      r2.xz = cmp((int2)r0.yy == int2(1,4));
      r0.z = (int)r2.z | (int)r2.x;
      if (r0.z == 0) {
        r2.xzw = -_GlobalBoxAreaCenters[1].xyz + v2.xyz;
        r2.xzw = _GlobalBoxAreaExtents[1].xyz + -abs(r2.xzw);
        r0.z = min(r2.z, r2.w);
        r0.z = min(r2.x, r0.z);
        r1.w = max(0.00100000005, _GlobalBoxAreaExtents[1].w);
        r0.z = saturate(r0.z / r1.w);
        r2.xzw = log2(abs(_GlobalBoxAreaColors[1].xyz));
        r2.xzw = float3(2.20000005,2.20000005,2.20000005) * r2.xzw;
        r2.xzw = exp2(r2.xzw);
        r0.y = cmp((int)r0.y == 3);
        if (r0.y != 0) {
          r0.y = -1 + r0.x;
          r0.y = _GlobalBoxAreaShimmerParams[1].z * r0.y + 1;
        } else {
          r0.y = 1;
        }
        r1.w = cmp(_GlobalBoxAreaIntensities[1] >= r5.x);
        r1.w = r1.w ? 1.000000 : 0;
        r0.z = r1.w * r0.z;
        r0.z = _GlobalBoxAreaColors[1].w * r0.z;
        r0.y = r0.z * r0.y;
        r2.xzw = r2.xzw + -r1.xyz;
        r1.xyz = r0.yyy * r2.xzw + r1.xyz;
      }
    }
    r0.y = cmp(_GlobalBoxAreaIntensities[2] >= 0.00100000005);
    if (r0.y != 0) {
      r0.y = (int)_GlobalBoxAreaCenters[2].w;
      r2.xz = cmp((int2)r0.yy == int2(1,4));
      r0.z = (int)r2.z | (int)r2.x;
      if (r0.z == 0) {
        r2.xzw = -_GlobalBoxAreaCenters[2].xyz + v2.xyz;
        r2.xzw = _GlobalBoxAreaExtents[2].xyz + -abs(r2.xzw);
        r0.z = min(r2.z, r2.w);
        r0.z = min(r2.x, r0.z);
        r1.w = max(0.00100000005, _GlobalBoxAreaExtents[2].w);
        r0.z = saturate(r0.z / r1.w);
        r2.xzw = log2(abs(_GlobalBoxAreaColors[2].xyz));
        r2.xzw = float3(2.20000005,2.20000005,2.20000005) * r2.xzw;
        r2.xzw = exp2(r2.xzw);
        r0.y = cmp((int)r0.y == 3);
        if (r0.y != 0) {
          r0.y = -1 + r0.x;
          r0.y = _GlobalBoxAreaShimmerParams[2].z * r0.y + 1;
        } else {
          r0.y = 1;
        }
        r1.w = cmp(_GlobalBoxAreaIntensities[2] >= r5.x);
        r1.w = r1.w ? 1.000000 : 0;
        r0.z = r1.w * r0.z;
        r0.z = _GlobalBoxAreaColors[2].w * r0.z;
        r0.y = r0.z * r0.y;
        r2.xzw = r2.xzw + -r1.xyz;
        r1.xyz = r0.yyy * r2.xzw + r1.xyz;
      }
    }
    r0.y = cmp(_GlobalBoxAreaIntensities[3] >= 0.00100000005);
    if (r0.y != 0) {
      r0.y = (int)_GlobalBoxAreaCenters[3].w;
      r2.xz = cmp((int2)r0.yy == int2(1,4));
      r0.z = (int)r2.z | (int)r2.x;
      if (r0.z == 0) {
        r2.xzw = -_GlobalBoxAreaCenters[3].xyz + v2.xyz;
        r2.xzw = _GlobalBoxAreaExtents[3].xyz + -abs(r2.xzw);
        r0.z = min(r2.z, r2.w);
        r0.z = min(r2.x, r0.z);
        r1.w = max(0.00100000005, _GlobalBoxAreaExtents[3].w);
        r0.z = saturate(r0.z / r1.w);
        r2.xzw = log2(abs(_GlobalBoxAreaColors[3].xyz));
        r2.xzw = float3(2.20000005,2.20000005,2.20000005) * r2.xzw;
        r2.xzw = exp2(r2.xzw);
        r0.y = cmp((int)r0.y == 3);
        if (r0.y != 0) {
          r0.y = -1 + r0.x;
          r0.y = _GlobalBoxAreaShimmerParams[3].z * r0.y + 1;
        } else {
          r0.y = 1;
        }
        r1.w = cmp(_GlobalBoxAreaIntensities[3] >= r5.x);
        r1.w = r1.w ? 1.000000 : 0;
        r0.z = r1.w * r0.z;
        r0.z = _GlobalBoxAreaColors[3].w * r0.z;
        r0.y = r0.z * r0.y;
        r2.xzw = r2.xzw + -r1.xyz;
        r1.xyz = r0.yyy * r2.xzw + r1.xyz;
      }
    }
    r0.y = cmp(_GlobalBoxAreaIntensities[4] >= 0.00100000005);
    if (r0.y != 0) {
      r0.y = (int)_GlobalBoxAreaCenters[4].w;
      r2.xz = cmp((int2)r0.yy == int2(1,4));
      r0.z = (int)r2.z | (int)r2.x;
      if (r0.z == 0) {
        r2.xzw = -_GlobalBoxAreaCenters[4].xyz + v2.xyz;
        r2.xzw = _GlobalBoxAreaExtents[4].xyz + -abs(r2.xzw);
        r0.z = min(r2.z, r2.w);
        r0.z = min(r2.x, r0.z);
        r1.w = max(0.00100000005, _GlobalBoxAreaExtents[4].w);
        r0.z = saturate(r0.z / r1.w);
        r2.xzw = log2(abs(_GlobalBoxAreaColors[4].xyz));
        r2.xzw = float3(2.20000005,2.20000005,2.20000005) * r2.xzw;
        r2.xzw = exp2(r2.xzw);
        r0.y = cmp((int)r0.y == 3);
        if (r0.y != 0) {
          r0.y = -1 + r0.x;
          r0.y = _GlobalBoxAreaShimmerParams[4].z * r0.y + 1;
        } else {
          r0.y = 1;
        }
        r1.w = cmp(_GlobalBoxAreaIntensities[4] >= r5.x);
        r1.w = r1.w ? 1.000000 : 0;
        r0.z = r1.w * r0.z;
        r0.z = _GlobalBoxAreaColors[4].w * r0.z;
        r0.y = r0.z * r0.y;
        r2.xzw = r2.xzw + -r1.xyz;
        r1.xyz = r0.yyy * r2.xzw + r1.xyz;
      }
    }
    r0.y = cmp(_GlobalBoxAreaIntensities[5] >= 0.00100000005);
    if (r0.y != 0) {
      r0.y = (int)_GlobalBoxAreaCenters[5].w;
      r2.xz = cmp((int2)r0.yy == int2(1,4));
      r0.z = (int)r2.z | (int)r2.x;
      if (r0.z == 0) {
        r2.xzw = -_GlobalBoxAreaCenters[5].xyz + v2.xyz;
        r2.xzw = _GlobalBoxAreaExtents[5].xyz + -abs(r2.xzw);
        r0.z = min(r2.z, r2.w);
        r0.z = min(r2.x, r0.z);
        r1.w = max(0.00100000005, _GlobalBoxAreaExtents[5].w);
        r0.z = saturate(r0.z / r1.w);
        r2.xzw = log2(abs(_GlobalBoxAreaColors[5].xyz));
        r2.xzw = float3(2.20000005,2.20000005,2.20000005) * r2.xzw;
        r2.xzw = exp2(r2.xzw);
        r0.y = cmp((int)r0.y == 3);
        if (r0.y != 0) {
          r0.y = -1 + r0.x;
          r0.y = _GlobalBoxAreaShimmerParams[5].z * r0.y + 1;
        } else {
          r0.y = 1;
        }
        r1.w = cmp(_GlobalBoxAreaIntensities[5] >= r5.x);
        r1.w = r1.w ? 1.000000 : 0;
        r0.z = r1.w * r0.z;
        r0.z = _GlobalBoxAreaColors[5].w * r0.z;
        r0.y = r0.z * r0.y;
        r2.xzw = r2.xzw + -r1.xyz;
        r1.xyz = r0.yyy * r2.xzw + r1.xyz;
      }
    }
    r0.y = cmp(_GlobalBoxAreaIntensities[6] >= 0.00100000005);
    if (r0.y != 0) {
      r0.y = (int)_GlobalBoxAreaCenters[6].w;
      r2.xz = cmp((int2)r0.yy == int2(1,4));
      r0.z = (int)r2.z | (int)r2.x;
      if (r0.z == 0) {
        r2.xzw = -_GlobalBoxAreaCenters[6].xyz + v2.xyz;
        r2.xzw = _GlobalBoxAreaExtents[6].xyz + -abs(r2.xzw);
        r0.z = min(r2.z, r2.w);
        r0.z = min(r2.x, r0.z);
        r1.w = max(0.00100000005, _GlobalBoxAreaExtents[6].w);
        r0.z = saturate(r0.z / r1.w);
        r2.xzw = log2(abs(_GlobalBoxAreaColors[6].xyz));
        r2.xzw = float3(2.20000005,2.20000005,2.20000005) * r2.xzw;
        r2.xzw = exp2(r2.xzw);
        r0.y = cmp((int)r0.y == 3);
        if (r0.y != 0) {
          r0.y = -1 + r0.x;
          r0.y = _GlobalBoxAreaShimmerParams[6].z * r0.y + 1;
        } else {
          r0.y = 1;
        }
        r1.w = cmp(_GlobalBoxAreaIntensities[6] >= r5.x);
        r1.w = r1.w ? 1.000000 : 0;
        r0.z = r1.w * r0.z;
        r0.z = _GlobalBoxAreaColors[6].w * r0.z;
        r0.y = r0.z * r0.y;
        r2.xzw = r2.xzw + -r1.xyz;
        r1.xyz = r0.yyy * r2.xzw + r1.xyz;
      }
    }
    r0.y = cmp(_GlobalBoxAreaIntensities[7] >= 0.00100000005);
    r0.z = (int)_GlobalBoxAreaCenters[7].w;
    r2.xzw = cmp((int3)r0.zzz == int3(1,4,3));
    r0.z = (int)r2.z | (int)r2.x;
    r4.xyz = -_GlobalBoxAreaCenters[7].xyz + v2.xyz;
    r4.xyz = _GlobalBoxAreaExtents[7].xyz + -abs(r4.xyz);
    r1.w = min(r4.y, r4.z);
    r1.w = min(r4.x, r1.w);
    r2.x = max(0.00100000005, _GlobalBoxAreaExtents[7].w);
    r1.w = saturate(r1.w / r2.x);
    r4.xyz = log2(abs(_GlobalBoxAreaColors[7].xyz));
    r4.xyz = float3(2.20000005,2.20000005,2.20000005) * r4.xyz;
    r4.xyz = exp2(r4.xyz);
    if (r2.w != 0) {
      r0.x = -1 + r0.x;
      r0.x = _GlobalBoxAreaShimmerParams[7].z * r0.x + 1;
    } else {
      r0.x = 1;
    }
    r2.x = cmp(_GlobalBoxAreaIntensities[7] >= r5.x);
    r2.x = r2.x ? 1.000000 : 0;
    r1.w = r2.x * r1.w;
    r1.w = _GlobalBoxAreaColors[7].w * r1.w;
    r0.x = r1.w * r0.x;
    r2.xzw = r4.xyz + -r1.xyz;
    r2.xzw = r0.xxx * r2.xzw + r1.xyz;
    r2.xzw = r0.zzz ? r1.xyz : r2.xzw;
    r1.xyz = r0.yyy ? r2.xzw : r1.xyz;
  }
  r0.x = cmp(0 < _DissolveProgress);
  if (r0.x != 0) {
    r0.x = _DissolveProgress + _DissolveProgress;
    r0.x = saturate(r0.x);
    r0.y = saturate(_DissolveProgress * 2 + -1);
    r0.z = 1 + -r2.y;
    r0.z = r0.z + -r0.x;
    r0.z = saturate(4 * r0.z);
    r1.w = r0.z * -2 + 3;
    r0.z = r0.z * r0.z;
    r0.z = -r1.w * r0.z + 1;
    r0.x = r0.z * r0.x;
    r0.x = _DissolveEdgeColor.w * r0.x;
    r2.xyz = _DissolveEdgeColor.xyz + -r1.xyz;
    r1.xyz = r0.xxx * r2.xyz + r1.xyz;
    r0.xz = float2(1,1) / _ScaledScreenParams.xy;
    r2.xy = v7.xy * r0.xz;
    r0.x = r2.y * _ScaleBiasRt.x + _ScaleBiasRt.y;
    r2.z = 1 + -r0.x;
    r0.x = 1 + -r0.y;
    r0.yz = _ScreenSize.xy / _ScreenParams.xy;
    r2.xy = _ScreenParams.xy * r2.xz;
    r0.yz = r2.xy * r0.yz;
    r0.yz = floor(r0.yz);
    r0.yz = (uint2)r0.yz;
    r0.yz = (int2)r0.yz & int2(3,3);
    r0.z = (uint)r0.z << 2;
    r0.y = (int)r0.z + (int)r0.y;
    r0.y = x0[r0.y+0].x;
    r0.x = r0.x + -r0.y;
    r0.x = cmp(r0.x < 0);
    if (r0.x != 0) discard;
  }
  r0.xyz = (_EmissionIntensity).xxx * r3.xyz;
  r1.w = _Time.y + _Time.y;
  r2.x = dot(v8.xyz, float3(0.298999995,0.587000012,0.114));
  r2.y = dot(v10.xyz, float3(12.9898005,78.2330017,45.1640015));
  r2.y = frac(r2.y);
  r2.x = r2.x + r2.y;
  r3.xyzw = float4(91.3000031,37.0999985,53.7000008,3) * r2.xxxx;
  r2.yzw = sin(r3.xyz);
  r2.yzw = float3(4337.5,1572.90002,9281.2002) * r2.yzw;
  r2.yzw = frac(r2.yzw);
  r2.yzw = r2.yzw * float3(4,6,12) + float3(3.70000005,7.19999981,15.3000002);
  r3.xyz = float3(6.28000021,3.1400001,4.71000004) * r2.xxx;
  r2.yzw = r1.www * r2.yzw + r3.xyz;
  r2.yzw = sin(r2.yzw);
  r2.yzw = r2.yzw * float3(0.5,0.5,0.5) + float3(0.5,0.5,0.5);
  r1.w = 1.60000002 * _Time.y;
  r1.w = r2.x * 2 + r1.w;
  r1.w = sin(r1.w);
  r3.x = _Time.y * 2.5999999 + r3.w;
  r3.x = sin(r3.x);
  r1.w = r3.x * r1.w;
  r1.w = r1.w * 0.5 + 0.5;
  r2.x = _Time.y * 27.3999996 + r2.x;
  r2.x = sin(r2.x);
  r2.x = 43758.5469 * r2.x;
  r2.x = frac(r2.x);
  r2.x = cmp(r2.x >= 0.970000029);
  r2.y = dot(r2.yzw, float3(0.150000006,0.150000006,0.200000003));
  r1.w = r1.w * 0.300000012 + r2.y;
  r2.x = r2.x ? 0.200000 : 0;
  r1.w = r2.x + r1.w;
  r1.w = -0.75 + r1.w;
  r1.w = saturate(-9.99999809 * r1.w);
  r2.x = r1.w * -2 + 3;
  r1.w = r1.w * r1.w;
  r1.w = r2.x * r1.w + -1;
  r1.w = _EmissionFlickRate * r1.w + 1;
  r0.xyz = r0.xyz * r1.www + r1.xyz;
  r1.xyz = _GlobalBackgroundColor.xyz + -r0.xyz;
  r0.xyz = (_GlobalBackgroundColorIntensity).xxx * r1.xyz + r0.xyz;
  r1.xy = -_GlobalPlayerPosition.xz + v2.xz;
  r1.z = dot(r1.xy, r1.xy);
  r2.xy = -_GlobalCursorPosition.xz + v2.xz;
  r1.w = dot(r2.xy, r2.xy);
  r2.xy = _GlobalCursorPosition.xz + -_GlobalPlayerPosition.xz;
  r2.z = dot(r2.xy, r2.xy);
  r2.z = max(9.99999997e-007, r2.z);
  r1.x = dot(r1.xy, r2.xy);
  r1.x = saturate(r1.x / r2.z);
  r1.xy = r2.xy * r1.xx + _GlobalPlayerPosition.xz;
  r1.xy = v2.xz + -r1.xy;
  r1.x = dot(r1.xy, r1.xy);
  r1.xzw = sqrt(r1.xzw);
  r1.xz = -float2(_GlobalCursorLineThickness, _GlobalCursorPlayerRadius) + r1.xz;
  r1.y = 1 / _GlobalCursorLineSmoothness;
  r1.x = saturate(r1.x * r1.y);
  r2.x = r1.x * -2 + 3;
  r1.x = r1.x * r1.x;
  r1.x = -r2.x * r1.x + 1;
  r2.x = saturate(r1.z * r1.y);
  r2.y = r2.x * -2 + 3;
  r2.x = r2.x * r2.x;
  r2.x = r2.y * r2.x;
  r1.x = r2.x * r1.x;
  r1.w = -_GlobalCursorRadius + r1.w;
  r1.y = saturate(r1.w * r1.y);
  r1.w = r1.y * -2 + 3;
  r1.y = r1.y * r1.y;
  r2.x = r1.w * r1.y;
  r1.x = r2.x * r1.x;
  r1.z = -_GlobalCursorPlayerLineThickness + abs(r1.z);
  r2.x = 1 / _GlobalCursorPlayerLineSmoothness;
  r1.z = saturate(r2.x * r1.z);
  r2.x = r1.z * -2 + 3;
  r1.z = r1.z * r1.z;
  r1.z = -r2.x * r1.z + 1;
  r1.y = -r1.w * r1.y + 1;
  r1.x = max(r1.x, r1.z);
  r1.x = max(r1.x, r1.y);
  r1.x = saturate(_GlobalCursorLineColor.w * r1.x);
  r1.yzw = _GlobalCursorLineColor.xyz + -r0.xyz;
  o0.xyz = r1.xxx * r1.yzw + r0.xyz;
  o0.w = r0.w;
  return;
}
