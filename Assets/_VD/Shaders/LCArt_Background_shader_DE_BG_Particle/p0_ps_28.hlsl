// LCArt/Background/shader_DE_BG_Particle | pass 0 "ForwardLit" | progFragment | keywords: FOG_LINEAR _CLUSTER_LIGHT_LOOP _LIGHT_LAYERS | blob 28
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
  float4 r0,r1,r2,r3,r4,r5,r6,r7,r8,r9,r10,r11,r12,r13;
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
    r1.w = cmp(1.5 < v1.z);
    if (r1.w != 0) {
      r4.xyzw = _CustomDepthBuffer.SampleBias(sampler_CustomDepthBuffer, r3.xz, _GlobalMipBias.x).xyzw;
      r1.w = v7.z + -r4.z;
      r1.w = -2.49999994e-005 + r1.w;
      r1.w = saturate(-20000 * r1.w);
      r2.x = r1.w * -2 + 3;
      r1.w = r1.w * r1.w;
      r2.z = r2.x * r1.w;
      r3.yw = float2(1,1) + -float2(_GlobalSeeThroughCircleRange, _GlobalSeeThroughCircleAlpha);
      r2.w = 1 + -r3.y;
      r3.y = r4.w + -r3.y;
      r2.w = 1 / r2.w;
      r2.w = saturate(r3.y * r2.w);
      r3.y = r2.w * -2 + 3;
      r2.w = r2.w * r2.w;
      r2.w = r3.y * r2.w;
      r1.w = -r2.x * r1.w + 1;
      r1.w = r2.w * r1.w + r2.z;
      r2.x = _GlobalPlaneSystemEnable * _GlobalBackgroundSeeThroughEnable;
      r1.w = -1 + r1.w;
      r1.w = r2.x * r1.w + 1;
      r1.w = r1.w * r3.w + _GlobalSeeThroughCircleAlpha;
      r2.zw = _ScreenSize.xy / _ScreenParams.xy;
      r3.yw = _ScreenParams.xy * r3.xz;
      r2.zw = r3.yw * r2.zw;
      r2.zw = floor(r2.zw);
      r2.zw = (uint2)r2.zw;
      r2.zw = (int2)r2.zw & int2(3,3);
      r2.w = (uint)r2.w << 2;
      r2.z = (int)r2.w + (int)r2.z;
      r2.z = x0[r2.z+0].x;
      r1.w = -r2.z + r1.w;
      r1.w = cmp(r1.w < 0);
      if (r1.w != 0) discard;
      r1.w = cmp(0.125 < v2.y);
      if (r1.w != 0) {
        r4.xyzw = _CharacterAreaMaskTexture.SampleLevel(sampler_CharacterAreaMaskTexture, r3.xz, 0).xyzw;
        r2.zw = round(_WorldSpaceCameraPos.xz);
        r2.zw = v2.xz + -r2.zw;
        r5.x = -(VD_COL(unity_MatrixV, 0).z);
        r5.y = -(VD_COL(unity_MatrixV, 2).z);
        r1.w = dot(r5.xy, r5.xy);
        r1.w = rsqrt(r1.w);
        r3.yw = r5.xy * r1.ww;
        r1.w = dot(r2.zw, r3.yw);
        r1.w = saturate(0.00999999978 * r1.w);
        r1.w = r4.y + -r1.w;
        r1.w = saturate(40 * r1.w);
        r2.z = r1.w * -2 + 3;
        r1.w = r1.w * r1.w;
        r1.w = r2.z * r1.w;
        r2.z = -1 + r4.x;
        r1.w = r2.z * r1.w;
        r1.w = _GlobalBackgroundSeeThroughEnable * r1.w + 1;
        r4.xyz = -_WorldSpaceCameraPos.xyz + v2.xyz;
        r2.z = -0.150000006 + -_WorldSpaceCameraPos.y;
        r2.z = r2.z / r4.y;
        r2.zw = r4.xz * r2.zz + _WorldSpaceCameraPos.xz;
        r2.zw = -_PlaneBoundMaxPos.xz + r2.zw;
        r3.yw = -_PlaneBoundMaxPos.xz + _PlaneBoundMinPos.xz;
        r2.zw = saturate(r2.zw / r3.yw);
        r4.xyzw = _PlaneSightRenderTarget.SampleLevel(sampler_PlaneSightRenderTarget, r2.zw, 0).xyzw;
        r2.z = -1 + r4.w;
        r2.z = _GlobalPlaneSystemEnable * r2.z + 1;
        r2.w = _SeeThroughDistanceMax + -_SeeThroughDistanceMin;
        r2.w = 1 / r2.w;
        r3.yw = float2(1,0);
        while (true) {
          r4.x = cmp((int)r3.w >= 3);
          if (r4.x != 0) break;
          r4.xyz = -_SightPositions[(int)(r3.w)].xyz + v2.xyz;
          r4.x = dot(r4.xyz, r4.xyz);
          r4.x = sqrt(r4.x);
          r4.x = -_SeeThroughDistanceMin + r4.x;
          r4.x = saturate(r4.x * r2.w);
          r4.y = r4.x * -2 + 3;
          r4.x = r4.x * r4.x;
          r4.x = r4.y * r4.x;
          r3.y = min(r4.x, r3.y);
          r3.w = (int)r3.w + 1;
        }
        r2.z = r2.z * r2.z;
        r2.w = 1 + -r3.y;
        r2.z = -r2.z * r2.w + 1;
        r2.z = max(0, r2.z);
        r2.z = -1 + r2.z;
        r2.x = r2.x * r2.z + 1;
        r1.w = min(r2.x, r1.w);
      } else {
        r1.w = w0.x;
      }
      r2.x = _GlobalSeeThroughHeightMax + -_GlobalSeeThroughHeightMin;
      r2.z = -_GlobalSeeThroughHeightMin + v2.y;
      r2.x = 1 / r2.x;
      r2.x = saturate(r2.z * r2.x);
      r2.z = r2.x * -2 + 3;
      r2.x = r2.x * r2.x;
      r2.x = -r2.z * r2.x + 1;
      r1.w = max(r2.x, r1.w);
      r1.w = max(0.00100000005, r1.w);
    } else {
      r1.w = w0.x;
    }
    r2.x = 1 + -_GlobalSeeThroughAlpha;
    r1.w = r1.w * r2.x + _GlobalSeeThroughAlpha;
    r1.w = v8.w * r1.w;
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
  r2.xzw = r4.xyz * r0.yyy;
  r0.y = cmp(0.5 < v1.z);
  r0.z = cmp(v1.z < 1.5);
  r0.y = r0.z ? r0.y : 0;
  r0.z = cmp(0.125 >= v11.y);
  r0.y = r0.z ? r0.y : 0;
  r4.xyz = r0.yyy ? v11.xyz : v2.xyz;
  r2.xzw = r0.yyy ? float3(0,1,0) : r2.xzw;
  r0.yz = float2(1,1) / _ScaledScreenParams.xy;
  r5.xy = v7.xy * r0.yz;
  r0.y = r5.y * _ScaleBiasRt.x + _ScaleBiasRt.y;
  r5.z = 1 + -r0.y;
  r0.y = (VD_COL(unity_MatrixV, 1).z) * r4.y;
  r0.y = (VD_COL(unity_MatrixV, 0).z) * r4.x + r0.y;
  r0.y = (VD_COL(unity_MatrixV, 2).z) * r4.z + r0.y;
  r0.y = (VD_COL(unity_MatrixV, 3).z) + r0.y;
  r0.y = -_ProjectionParams.y + -r0.y;
  r0.y = max(0, r0.y);
  r0.y = saturate(r0.y * unity_FogParams.z + unity_FogParams.w);
  r5.yw = _GlobalOverallMapBoundMax.xz + -_GlobalOverallMapBoundMin.xz;
  r5.yw = max(float2(1,1), r5.yw);
  r6.xy = -_GlobalOverallMapBoundMin.xz + v2.xz;
  r6.xy = r6.xy / r5.yw;
  r7.xyzw = _GlobalWaterAreaMap.SampleBias(sampler_vd_linear_clamp, r6.xy, _GlobalMipBias.x).xyzw;
  r0.z = cmp(_WeatherIntensity >= 0.00100000005);
  r3.w = 0.899999976 + r7.x;
  r3.w = saturate(0.5 * r3.w);
  r4.w = r3.w * -2 + 3;
  r3.w = r3.w * r3.w;
  r3.w = r4.w * r3.w;
  r3.w = max(_WeatherIndoorMask, r3.w);
  r3.w = 1 + -r3.w;
  r3.w = _WeatherIntensity * r3.w;
  r4.w = cmp(r3.w >= 0.00100000005);
  r6.z = _WeatherWetSurfaceIntensity * r3.w;
  r6.z = r6.z * -0.300000012 + 1;
  r7.xyz = r6.zzz * r1.xyz;
  r3.w = _WeatherBgTintIntensity * r3.w;
  r6.w = cmp(0.00100000005 < r3.w);
  r7.w = dot(r7.xyz, float3(0.298999995,0.587000012,0.114));
  r8.x = _WeatherBgDesaturation * r3.w;
  r8.yzw = -r1.xyz * r6.zzz + r7.www;
  r8.xyz = r8.xxx * r8.yzw + r7.xyz;
  r3.w = 0.300000012 * r3.w;
  r9.xyz = _WeatherBgTintColor.xyz + -r8.xyz;
  r8.xyz = r3.www * r9.xyz + r8.xyz;
  r7.xyz = r6.www ? r8.xyz : r7.xyz;
  r7.xyz = r4.www ? r7.xyz : r1.xyz;
  r7.xyz = r0.zzz ? r7.xyz : r1.xyz;
  if (_GlobalWaterAreaEnable != 0) {
    r6.xyzw = _GlobalWaterAreaMap.SampleLevel(sampler_vd_linear_clamp, r6.xy, 0).xyzw;
    r0.z = cmp(0.100000001 < r6.z);
    if (r0.z != 0) {
      r6.xy = _GlobalWaterDepthRange.ww * v2.xz;
      r8.xyzw = _GlobalWaterNoiseTex.SampleLevel(sampler_vd_linear_repeat, r6.xy, 0).xyzw;
      r0.z = -0.5 + r8.y;
      r0.z = _GlobalWaterDepthRange.z * r0.z;
      r3.w = r0.z * r6.z;
      r8.xy = float2(0.800000012,0.600000024) * _GlobalWaterWaveParams.yx;
      r8.z = -0.300000012;
      r6.xy = _Time.yy * r8.yz;
      r9.x = 0.400000006;
      r9.y = 0.600000024 * _GlobalWaterWaveParams.x;
      r6.xy = r9.xy * r6.xy;
      r6.xy = v2.xz * r8.xx + r6.xy;
      r8.xyzw = _GlobalWaterNoiseTex.SampleLevel(sampler_vd_linear_repeat, r6.xy, 0).xyzw;
      r4.w = -0.5 + r6.z;
      r4.w = 5 * abs(r4.w);
      r4.w = min(1, r4.w);
      r4.w = 1 + -r4.w;
      r6.x = -0.5 + r8.y;
      r3.w = r6.x * r4.w + r3.w;
      r3.w = r6.z + r3.w;
      r4.w = _GlobalWaterShoreEdge.y + -_GlobalWaterShoreEdge.x;
      r6.x = -_GlobalWaterShoreEdge.x + r3.w;
      r4.w = 1 / r4.w;
      r4.w = saturate(r6.x * r4.w);
      r6.x = r4.w * -2 + 3;
      r4.w = r4.w * r4.w;
      r4.w = r6.x * r4.w;
      r6.xy = (_GlobalWaterShoreStroke).xx + _GlobalWaterShoreEdge.xy;
      r6.y = r6.y + -r6.x;
      r6.x = -r6.x + r3.w;
      r6.y = 1 / r6.y;
      r6.x = saturate(r6.x * r6.y);
      r6.y = r6.x * -2 + 3;
      r6.x = r6.x * r6.x;
      r6.x = -r6.y * r6.x + 1;
      r0.z = r0.z * r6.z + r6.z;
      r6.y = _GlobalWaterDepthRange.y + -_GlobalWaterDepthRange.x;
      r0.z = -_GlobalWaterDepthRange.x + r0.z;
      r6.y = 1 / r6.y;
      r0.z = saturate(r6.y * r0.z);
      r6.z = r0.z * -2 + 3;
      r0.z = r0.z * r0.z;
      r6.w = r6.z * r0.z;
      r6.x = r6.x * r4.w;
      r7.w = cmp(r4.w >= 0.00999999978);
      if (r7.w != 0) {
        r8.xy = cmp(float2(0.00999999978,0.00999999978) < _GlobalWaterSurfaceWaveMapParams.zw);
        r7.w = r8.y ? r8.x : 0;
        if (r7.w != 0) {
          r8.xy = -_GlobalWaterSurfaceWaveMapParams.xy + v2.xz;
          r8.xy = r8.xy / _GlobalWaterSurfaceWaveMapParams.zw;
          r8.xy = float2(0.5,0.5) + r8.xy;
          r8.zw = float2(1,1) + -r8.xy;
          r8.zw = min(r8.xy, r8.zw);
          r7.w = min(r8.z, r8.w);
          r7.w = saturate(_GlobalWaterSurfaceWaveMapParams.z * r7.w);
          r8.xyzw = _GlobalWaterSurfaceWaveMap.SampleLevel(sampler_vd_linear_clamp, r8.xy, 0).xyzw;
          r8.yz = r8.yz * float2(2,2) + float2(-1,-1);
          r8.xw = r8.wx * r7.ww;
        } else {
          r8.xyzw = float4(0,0,0,0);
        }
        r0.z = -r6.z * r0.z + 1;
        r0.z = -_GlobalWaterShoreHeightDrop * r0.z + _GlobalWaterSurfaceLevel;
        r0.z = r8.w * 0.5 + r0.z;
        r6.z = -_GlobalWaterDepthOffset + v2.y;
        r0.z = r6.z + -r0.z;
        r6.z = saturate(r0.z * -2 + 0.5);
        r9.xy = _GlobalWaterWaveParams.yy * v2.xz;
        r7.w = _GlobalWaterWaveParams.x * _Time.y;
        r10.xyz = float3(0.707099974,-0.780000031,0.519999981) * r7.www;
        r9.zw = v2.xz * _GlobalWaterWaveParams.yy + r10.xx;
        r11.xyzw = _GlobalWaterNoiseTex.SampleLevel(sampler_vd_linear_repeat, r9.zw, 0).xyzw;
        r12.xw = _GlobalWaterWaveParams.xx;
        r12.yz = float2(0.699999988,0.300000012);
        r9.zw = _Time.yy * r12.xy;
        r9.zw = r9.zw * r12.zw;
        r9.zw = r9.xy * float2(1.5,1.5) + r9.zw;
        r10.xy = r9.xy * float2(2,2) + r10.yz;
        r12.xyzw = _GlobalWaterNormalMap.SampleLevel(sampler_vd_linear_repeat, r9.zw, 0).xyzw;
        r11.yzw = r12.xyz * float3(2,2,2) + float3(-1,-1,-1);
        r10.xyzw = _GlobalWaterNormalMap.SampleLevel(sampler_vd_linear_repeat, r10.xy, 0).xyzw;
        r10.xyz = r10.xyz * float3(2,2,2) + float3(-1,-1,-1);
        r12.xy = r11.yz + r10.xy;
        r12.z = r11.w * r10.z;
        r7.w = dot(r12.xyz, r12.xyz);
        r7.w = rsqrt(r7.w);
        r10.xyz = r12.xyz * r7.www;
        r7.w = saturate(dot(r10.xzy, float3(0.300000012,0.300000012,0.899999976)));
        r7.w = r7.w * r6.w;
        r7.w = 0.150000006 * r7.w;
        r8.xy = r8.yz * r8.xx;
        r8.z = log2(abs(r11.x));
        r8.z = _GlobalWaterWaveParams.z * r8.z;
        r8.z = exp2(r8.z);
        r8.z = min(1, r8.z);
        r8.z = _GlobalWaterWaveParams.w * r8.z;
        r6.x = _GlobalWaterShoreIntensity * r6.x;
        r6.x = max(r6.x, r8.z);
        r6.x = _GlobalWaterShallowColor.w * r6.x;
        r10.xyz = -_GlobalWaterDeepColor.xyz + _GlobalWaterShallowColor.xyz;
        r10.xyz = r6.xxx * r10.xyz + _GlobalWaterDeepColor.xyz;
        r9.zw = r8.xy * float2(0.5,0.5) + v2.xz;
        r9.zw = -_GlobalOverallMapBoundMin.xz + r9.zw;
        r5.yw = r9.zw / r5.yw;
        r12.xyzw = _GlobalWaterAreaMap.SampleLevel(sampler_vd_linear_clamp, r5.yw, 0).xyzw;
        r5.y = -_GlobalWaterDepthRange.x + r12.z;
        r5.y = saturate(r5.y * r6.y);
        r5.w = r5.y * -2 + 3;
        r5.y = r5.y * r5.y;
        r5.y = r5.w * r5.y + -r6.w;
        r5.y = abs(r5.y) * abs(r8.w);
        r5.y = 3 * r5.y;
        r5.y = min(1, r5.y);
        r11.yzw = _GlobalWaterShallowColor.xyz + -r10.xyz;
        r10.xyz = r5.yyy * r11.yzw + r10.xyz;
        r11.yzw = _GlobalWaterShallowColor.xyz + -r10.xyz;
        r10.xyz = r7.www * r11.yzw + r10.xyz;
        r5.y = cmp(r0.z >= 0);
        r5.y = r5.y ? 1.000000 : 0;
        r5.w = 1 / _GlobalWaterSurfaceEdgeWidth;
        r0.z = saturate(r5.w * r0.z);
        r5.w = r0.z * -2 + 3;
        r0.z = r0.z * r0.z;
        r0.z = -r5.w * r0.z + 1;
        r5.w = _GlobalWaterShallowColor.w * r5.y;
        r5.w = r5.w * r4.w;
        r5.w = 0.5 * r5.w;
        r11.yzw = _GlobalWaterShallowColor.xyz + -r10.xyz;
        r10.xyz = r5.www * r11.yzw + r10.xyz;
        r5.w = cmp(vd_asint(_PlaneReflectionActive) != 0);
        r6.x = cmp(0.00999999978 < _GlobalWaterReflectionIntensity);
        r5.w = r5.w ? r6.x : 0;
        if (r5.w != 0) {
          r11.y = -0.5;
          r11.z = _GlobalWaterWaveParams.x;
          r12.yz = _Time.yy * r11.yz;
          r12.x = _GlobalWaterWaveParams.x * r12.y;
          r6.xy = float2(0.800000012,0.692799985) * r12.xz;
          r6.xy = r9.xy * float2(0.699999988,0.699999988) + r6.xy;
          r9.xyzw = _GlobalWaterNoiseTex.SampleLevel(sampler_vd_linear_repeat, r6.xy, 0).yxzw;
          r9.x = r11.x;
          r6.xy = float2(-0.5,-0.5) + r9.xy;
          r6.xy = r6.xy + r6.xy;
          r6.xy = _GlobalWaterWaveParams2.xx * r6.xy;
          r8.zw = v7.xy / _ScreenParams.xy;
          r6.xy = r6.xy * float2(0.5,0.5) + r8.zw;
          r6.xy = r8.xy * float2(0.0199999996,0.0199999996) + r6.xy;
          r6.y = _GlobalWaterReflectionYOffset + r6.y;
          r8.xyzw = _PlaneReflectionTexture.SampleLevel(sampler_vd_linear_clamp, r6.xy, 0).xyzw;
          r5.w = r6.w * 0.699999988 + 0.300000012;
          r5.w = r5.w * r4.w;
          r5.w = _GlobalWaterReflectionIntensity * r5.w;
          r5.w = _PlaneReflectionOpacity * r5.w;
          r6.xyw = r8.xyz * r5.www;
          r5.w = -r0.z * r5.y + 1;
          r10.xyz = r6.xyw * r5.www + r10.xyz;
        }
        r5.y = saturate(r0.z * r5.y + r6.z);
        r4.w = _GlobalWaterDeepColor.w * r4.w;
        r3.w = rsqrt(r3.w);
        r3.w = 1 / r3.w;
        r3.w = r4.w * r3.w;
        r3.w = r3.w * r5.y;
        r0.z = r3.w * r0.z;
        r6.xyz = r10.xyz + -r7.xyz;
        r7.xyz = r0.zzz * r6.xyz + r7.xyz;
      }
    }
  }
  r0.z = vd_asint(_MainLightLayerMask) & vd_asint(unity_RenderingLayer.x);
  r3.w = dot(r2.xzw, _MainLightPosition.xyz);
  r3.w = saturate(r3.w * 0.5 + 0.5);
  r6.xyz = -_GlobalBackgroundShadowColor.xyz + _MainLightColor.xyz;
  r6.xyz = r6.xyz * r3.www + _GlobalBackgroundShadowColor.xyz;
  r6.xyz = r6.xyz * r7.xyz;
  r6.xyz = r0.zzz ? r6.xyz : float3(0,0,0);
  r5.yw = -_PlaneBoundMaxPos.xz + r4.xz;
  r8.xy = -_PlaneBoundMaxPos.xz + _PlaneBoundMinPos.xz;
  r5.yw = saturate(r5.yw / r8.xy);
  r8.xyzw = _PlaneSightRenderTarget.SampleLevel(sampler_PlaneSightRenderTarget, r5.yw, 0).xyzw;
  r0.z = -1 + r8.w;
  r0.z = _GlobalPlaneSystemEnable * r0.z;
  r5.yw = _FPParams1.xy * r5.xz;
  r5.yw = (uint2)r5.yw;
  r8.xy = (uint2)_FPParams1.zw;
  r3.w = mad((int)r5.w, (int)r8.x, (int)r5.y);
  r3.w = (int)r8.y * (int)r3.w;
  r9.x = (VD_COL(unity_MatrixV, 0).z);
  r9.y = (VD_COL(unity_MatrixV, 1).z);
  r9.z = (VD_COL(unity_MatrixV, 2).z);
  r8.xzw = -_WorldSpaceCameraPos.xyz + r4.xyz;
  r4.w = dot(-r9.xyz, r8.xzw);
  r5.y = cmp(unity_OrthoParams.w == 0.000000);
  r5.w = log2(r4.w);
  r4.w = r5.y ? r5.w : r4.w;
  r4.w = r4.w * _FPParams0.x + _FPParams0.y;
  r4.w = (uint)r4.w;
  r5.y = (uint)_FPParams2.x;
  r5.y = (int)r5.y + -1;
  r5.w = (int)r8.y + 2;
  r4.w = min((uint)r5.y, (uint)r4.w);
  r5.y = (int)r5.w * (int)r4.w;
  r4.w = mad((int)r4.w, (int)r5.w, 2);
  r5.w = (uint)r5.y >> 2;
  r8.x = (uint)r5.y << 31;
  r8.y = (uint)r5.y << 30;
  r8.xy = (uint2)r8.xy >> 31;
  r8.zw = (int2)r8.xx & vd_asint(urp_ZBins[(int)(r5.w)].wy);
  r9.xy = ~(int2)r8.xy;
  r5.yw = (int2)r9.xx & vd_asint(urp_ZBins[(int)(r5.w)].zx);
  r5.yw = (int2)r5.yw | (int2)r8.zw;
  r5.y = (int)r8.y & (int)r5.y;
  r5.w = (int)r9.y & (int)r5.w;
  r5.y = (int)r5.w | (int)r5.y;
  r5.w = (uint)_FPParams0.w;
  r8.x = r3.w;
  r8.y = r4.w;
  r8.z = 0;
  r8.w = r5.y;
  r9.xyz = float3(0,0,0);
  while (true) {
    r6.w = (uint)r8.w >> 16;
    r7.w = r8.z;
    r9.w = r8.w;
    while (true) {
      r10.x = cmp((int)r7.w == 0);
      r10.y = (int)r9.w & 0x0000ffff;
      r10.y = cmp((uint)r6.w >= (uint)r10.y);
      r10.x = r10.y ? r10.x : 0;
      if (r10.x == 0) break;
      r10.x = (uint)r9.w >> 5;
      r10.x = (int)r10.x & 2047;
      r11.xyzw = (int4)r8.xxyy + (int4)r10.xxxx;
      r10.yz = (uint2)r11.yw >> 2;
      r12.xz = (uint2)r11.xz << 31;
      r12.yw = (uint2)r11.yw << 30;
      r11.xyzw = (uint4)r12.xyzw >> 31;
      r12.xy = (int2)r11.xx & vd_asint(urp_Tiles[(int)(r10.y)].wy);
      r13.xyzw = ~(int4)r11.xyzw;
      r10.yw = (int2)r13.xx & vd_asint(urp_Tiles[(int)(r10.y)].zx);
      r10.yw = (int2)r10.yw | (int2)r12.xy;
      r10.y = (int)r11.y & (int)r10.y;
      r10.w = (int)r13.y & (int)r10.w;
      r10.y = (int)r10.w | (int)r10.y;
      r11.xy = (int2)r11.zz & vd_asint(urp_ZBins[(int)(r10.z)].wy);
      r10.zw = (int2)r13.zz & vd_asint(urp_ZBins[(int)(r10.z)].zx);
      r10.zw = (int2)r10.zw | (int2)r11.xy;
      r10.z = (int)r11.w & (int)r10.z;
      r10.w = (int)r13.w & (int)r10.w;
      r10.z = (int)r10.w | (int)r10.z;
      r10.y = (int)r10.z & (int)r10.y;
      r10.z = -1 << (int)r9.w;
      r10.y = (int)r10.z & (int)r10.y;
      r10.x = (uint)r10.x << 5;
      r10.x = (int)r6.w + (int)-r10.x;
      r10.x = min(31, (uint)r10.x);
      r10.x = (int)-r10.x + 31;
      r10.x = -1 >> (uint)r10.x;
      r7.w = (int)r10.x & (int)r10.y;
      r10.x = (int)r9.w + 32;
      r9.w = (int)r10.x & -32;
    }
    r8.w = r9.w;
    if (r7.w == 0) break;
    r6.w = -(int)r7.w;
    r6.w = (int)r6.w & (int)r7.w;
    r6.w = (uint)r6.w;
    r6.w = (uint)r6.w >> 23;
    r6.w = (int)r6.w + -127;
    r10.x = 1 << (int)r6.w;
    r8.z = (int)r7.w ^ (int)r10.x;
    r10.x = (int)r8.w + -32;
    r10.x = (int)r10.x & 0x0000ffe0;
    r6.w = (int)r6.w + (int)r10.x;
    r6.w = (int)r5.w + (int)r6.w;
    r10.xyz = -r4.xyz * _AdditionalLightsPosition[(int)(r6.w)].www + _AdditionalLightsPosition[(int)(r6.w)].xyz;
    r10.w = dot(r10.xyz, r10.xyz);
    r10.w = max(6.10351563e-005, r10.w);
    r11.x = rsqrt(r10.w);
    r10.xyz = r11.xxx * r10.xyz;
    r11.x = 1 / r10.w;
    r10.w = _AdditionalLightsAttenuation[(int)(r6.w)].x * r10.w;
    r10.w = -r10.w * r10.w + 1;
    r10.w = max(0, r10.w);
    r10.w = r10.w * r10.w;
    r10.w = r11.x * r10.w;
    r11.x = dot(_AdditionalLightsSpotDir[(int)(r6.w)].xyz, r10.xyz);
    r11.x = saturate(r11.x * _AdditionalLightsAttenuation[(int)(r6.w)].z + _AdditionalLightsAttenuation[(int)(r6.w)].w);
    r11.x = r11.x * r11.x;
    r10.w = r11.x * r10.w;
    r11.x = cmp(2 == vd_asint(_AdditionalLightsLayerMasks[(int)(r6.w)]));
    r11.x = r11.x ? 1.000000 : 0;
    r11.x = r11.x * r0.z + 1;
    r10.w = sqrt(r10.w);
    r10.w = min(1, r10.w);
    r11.yzw = _AdditionalLightsColor[(int)(r6.w)].xyz * r10.www;
    r6.w = saturate(dot(r2.xzw, r10.xyz));
    r10.xyz = r11.yzw * r6.www;
    r10.xyz = r10.xyz * r7.xyz;
    r9.xyz = r10.xyz * r11.xxx + r9.xyz;
  }
  r2.xzw = r9.xyz + r6.xyz;
  r1.xyz = r1.www * r1.xyz + r2.xzw;
  r4.x = dot(float2(0.530324996,0.530324996), v2.xz);
  r4.y = dot(float3(-0.353500009,0.865999997,0.353500009), v2.xyz);
  r2.xz = float2(0.125,0.125) * r4.xy;
  r6.xyzw = _DirtNoiseTexture.SampleBias(sampler_vd_point_repeat, r2.xz, _GlobalMipBias.x).xyzw;
  r0.z = r6.x * 0.5 + 0.5;
  r1.w = r0.z * r0.z;
  r0.z = 1 + -r0.z;
  r0.z = r0.z * r0.z;
  r2.xzw = r1.xyz * r1.xyz;
  r2.xzw = min(float3(1,1,1), r2.xzw);
  r1.w = _DirtNoiseColor.w * r1.w;
  r1.w = _UseDirtNoise * r1.w;
  r2.xzw = r2.xzw + -r1.xyz;
  r1.xyz = r1.www * r2.xzw + r1.xyz;
  r2.xzw = saturate(r1.xyz + r1.xyz);
  r0.z = _DirtNoiseColor.w * r0.z;
  r0.z = _UseDirtNoise * r0.z;
  r2.xzw = r2.xzw + -r1.xyz;
  r1.xyz = r0.zzz * r2.xzw + r1.xyz;
  if (r0.x != 0) {
    r0.xz = float2(0.400000006,0.400000006) * r4.xy;
    r6.xyzw = _DirtNoiseTexture.SampleBias(sampler_vd_point_repeat, r0.xz, _GlobalMipBias.x).xyzw;
    r7.xyzw = _Time.yyyy * float4(1.5,1.5,0.800000012,0.800000012) + v2.zxzx;
    r0.xz = sin(r7.xw);
    r2.xz = cos(r7.yz);
    r7.zw = float2(0.0250000004,0.0250000004) * r2.xz;
    r7.xy = float2(0.0250000004,0.0250000004) * r0.xz;
    r0.xz = r4.xy * float2(0.150000006,0.150000006) + r7.xz;
    r8.xyzw = _DirtNoiseTexture.SampleBias(sampler_vd_point_repeat, r0.xz, _GlobalMipBias.x).xyzw;
    r0.xz = r4.xy * float2(0.1875,0.1875) + r7.wy;
    r4.xyzw = _DirtNoiseTexture.SampleBias(sampler_vd_point_repeat, r0.xz, _GlobalMipBias.x).xyzw;
    r0.x = r8.x + r4.x;
    r0.x = saturate(0.5 * r0.x);
    r0.x = rsqrt(r0.x);
    r0.x = 1 / r0.x;
    r0.z = cmp(_GlobalBoxAreaIntensities[0] >= 0.00100000005);
    if (r0.z != 0) {
      r0.z = (int)_GlobalBoxAreaCenters[0].w;
      r2.xz = cmp((int2)r0.zz == int2(1,4));
      r1.w = (int)r2.z | (int)r2.x;
      if (r1.w == 0) {
        r2.xzw = -_GlobalBoxAreaCenters[0].xyz + v2.xyz;
        r2.xzw = _GlobalBoxAreaExtents[0].xyz + -abs(r2.xzw);
        r1.w = min(r2.z, r2.w);
        r1.w = min(r2.x, r1.w);
        r2.x = max(0.00100000005, _GlobalBoxAreaExtents[0].w);
        r1.w = saturate(r1.w / r2.x);
        r2.xzw = log2(abs(_GlobalBoxAreaColors[0].xyz));
        r2.xzw = float3(2.20000005,2.20000005,2.20000005) * r2.xzw;
        r2.xzw = exp2(r2.xzw);
        r0.z = cmp((int)r0.z == 3);
        if (r0.z != 0) {
          r0.z = -1 + r0.x;
          r0.z = _GlobalBoxAreaShimmerParams[0].z * r0.z + 1;
        } else {
          r0.z = 1;
        }
        r3.w = cmp(_GlobalBoxAreaIntensities[0] >= r6.x);
        r3.w = r3.w ? 1.000000 : 0;
        r1.w = r3.w * r1.w;
        r1.w = _GlobalBoxAreaColors[0].w * r1.w;
        r0.z = r1.w * r0.z;
        r2.xzw = r2.xzw + -r1.xyz;
        r1.xyz = r0.zzz * r2.xzw + r1.xyz;
      }
    }
    r0.z = cmp(_GlobalBoxAreaIntensities[1] >= 0.00100000005);
    if (r0.z != 0) {
      r0.z = (int)_GlobalBoxAreaCenters[1].w;
      r2.xz = cmp((int2)r0.zz == int2(1,4));
      r1.w = (int)r2.z | (int)r2.x;
      if (r1.w == 0) {
        r2.xzw = -_GlobalBoxAreaCenters[1].xyz + v2.xyz;
        r2.xzw = _GlobalBoxAreaExtents[1].xyz + -abs(r2.xzw);
        r1.w = min(r2.z, r2.w);
        r1.w = min(r2.x, r1.w);
        r2.x = max(0.00100000005, _GlobalBoxAreaExtents[1].w);
        r1.w = saturate(r1.w / r2.x);
        r2.xzw = log2(abs(_GlobalBoxAreaColors[1].xyz));
        r2.xzw = float3(2.20000005,2.20000005,2.20000005) * r2.xzw;
        r2.xzw = exp2(r2.xzw);
        r0.z = cmp((int)r0.z == 3);
        if (r0.z != 0) {
          r0.z = -1 + r0.x;
          r0.z = _GlobalBoxAreaShimmerParams[1].z * r0.z + 1;
        } else {
          r0.z = 1;
        }
        r3.w = cmp(_GlobalBoxAreaIntensities[1] >= r6.x);
        r3.w = r3.w ? 1.000000 : 0;
        r1.w = r3.w * r1.w;
        r1.w = _GlobalBoxAreaColors[1].w * r1.w;
        r0.z = r1.w * r0.z;
        r2.xzw = r2.xzw + -r1.xyz;
        r1.xyz = r0.zzz * r2.xzw + r1.xyz;
      }
    }
    r0.z = cmp(_GlobalBoxAreaIntensities[2] >= 0.00100000005);
    if (r0.z != 0) {
      r0.z = (int)_GlobalBoxAreaCenters[2].w;
      r2.xz = cmp((int2)r0.zz == int2(1,4));
      r1.w = (int)r2.z | (int)r2.x;
      if (r1.w == 0) {
        r2.xzw = -_GlobalBoxAreaCenters[2].xyz + v2.xyz;
        r2.xzw = _GlobalBoxAreaExtents[2].xyz + -abs(r2.xzw);
        r1.w = min(r2.z, r2.w);
        r1.w = min(r2.x, r1.w);
        r2.x = max(0.00100000005, _GlobalBoxAreaExtents[2].w);
        r1.w = saturate(r1.w / r2.x);
        r2.xzw = log2(abs(_GlobalBoxAreaColors[2].xyz));
        r2.xzw = float3(2.20000005,2.20000005,2.20000005) * r2.xzw;
        r2.xzw = exp2(r2.xzw);
        r0.z = cmp((int)r0.z == 3);
        if (r0.z != 0) {
          r0.z = -1 + r0.x;
          r0.z = _GlobalBoxAreaShimmerParams[2].z * r0.z + 1;
        } else {
          r0.z = 1;
        }
        r3.w = cmp(_GlobalBoxAreaIntensities[2] >= r6.x);
        r3.w = r3.w ? 1.000000 : 0;
        r1.w = r3.w * r1.w;
        r1.w = _GlobalBoxAreaColors[2].w * r1.w;
        r0.z = r1.w * r0.z;
        r2.xzw = r2.xzw + -r1.xyz;
        r1.xyz = r0.zzz * r2.xzw + r1.xyz;
      }
    }
    r0.z = cmp(_GlobalBoxAreaIntensities[3] >= 0.00100000005);
    if (r0.z != 0) {
      r0.z = (int)_GlobalBoxAreaCenters[3].w;
      r2.xz = cmp((int2)r0.zz == int2(1,4));
      r1.w = (int)r2.z | (int)r2.x;
      if (r1.w == 0) {
        r2.xzw = -_GlobalBoxAreaCenters[3].xyz + v2.xyz;
        r2.xzw = _GlobalBoxAreaExtents[3].xyz + -abs(r2.xzw);
        r1.w = min(r2.z, r2.w);
        r1.w = min(r2.x, r1.w);
        r2.x = max(0.00100000005, _GlobalBoxAreaExtents[3].w);
        r1.w = saturate(r1.w / r2.x);
        r2.xzw = log2(abs(_GlobalBoxAreaColors[3].xyz));
        r2.xzw = float3(2.20000005,2.20000005,2.20000005) * r2.xzw;
        r2.xzw = exp2(r2.xzw);
        r0.z = cmp((int)r0.z == 3);
        if (r0.z != 0) {
          r0.z = -1 + r0.x;
          r0.z = _GlobalBoxAreaShimmerParams[3].z * r0.z + 1;
        } else {
          r0.z = 1;
        }
        r3.w = cmp(_GlobalBoxAreaIntensities[3] >= r6.x);
        r3.w = r3.w ? 1.000000 : 0;
        r1.w = r3.w * r1.w;
        r1.w = _GlobalBoxAreaColors[3].w * r1.w;
        r0.z = r1.w * r0.z;
        r2.xzw = r2.xzw + -r1.xyz;
        r1.xyz = r0.zzz * r2.xzw + r1.xyz;
      }
    }
    r0.z = cmp(_GlobalBoxAreaIntensities[4] >= 0.00100000005);
    if (r0.z != 0) {
      r0.z = (int)_GlobalBoxAreaCenters[4].w;
      r2.xz = cmp((int2)r0.zz == int2(1,4));
      r1.w = (int)r2.z | (int)r2.x;
      if (r1.w == 0) {
        r2.xzw = -_GlobalBoxAreaCenters[4].xyz + v2.xyz;
        r2.xzw = _GlobalBoxAreaExtents[4].xyz + -abs(r2.xzw);
        r1.w = min(r2.z, r2.w);
        r1.w = min(r2.x, r1.w);
        r2.x = max(0.00100000005, _GlobalBoxAreaExtents[4].w);
        r1.w = saturate(r1.w / r2.x);
        r2.xzw = log2(abs(_GlobalBoxAreaColors[4].xyz));
        r2.xzw = float3(2.20000005,2.20000005,2.20000005) * r2.xzw;
        r2.xzw = exp2(r2.xzw);
        r0.z = cmp((int)r0.z == 3);
        if (r0.z != 0) {
          r0.z = -1 + r0.x;
          r0.z = _GlobalBoxAreaShimmerParams[4].z * r0.z + 1;
        } else {
          r0.z = 1;
        }
        r3.w = cmp(_GlobalBoxAreaIntensities[4] >= r6.x);
        r3.w = r3.w ? 1.000000 : 0;
        r1.w = r3.w * r1.w;
        r1.w = _GlobalBoxAreaColors[4].w * r1.w;
        r0.z = r1.w * r0.z;
        r2.xzw = r2.xzw + -r1.xyz;
        r1.xyz = r0.zzz * r2.xzw + r1.xyz;
      }
    }
    r0.z = cmp(_GlobalBoxAreaIntensities[5] >= 0.00100000005);
    if (r0.z != 0) {
      r0.z = (int)_GlobalBoxAreaCenters[5].w;
      r2.xz = cmp((int2)r0.zz == int2(1,4));
      r1.w = (int)r2.z | (int)r2.x;
      if (r1.w == 0) {
        r2.xzw = -_GlobalBoxAreaCenters[5].xyz + v2.xyz;
        r2.xzw = _GlobalBoxAreaExtents[5].xyz + -abs(r2.xzw);
        r1.w = min(r2.z, r2.w);
        r1.w = min(r2.x, r1.w);
        r2.x = max(0.00100000005, _GlobalBoxAreaExtents[5].w);
        r1.w = saturate(r1.w / r2.x);
        r2.xzw = log2(abs(_GlobalBoxAreaColors[5].xyz));
        r2.xzw = float3(2.20000005,2.20000005,2.20000005) * r2.xzw;
        r2.xzw = exp2(r2.xzw);
        r0.z = cmp((int)r0.z == 3);
        if (r0.z != 0) {
          r0.z = -1 + r0.x;
          r0.z = _GlobalBoxAreaShimmerParams[5].z * r0.z + 1;
        } else {
          r0.z = 1;
        }
        r3.w = cmp(_GlobalBoxAreaIntensities[5] >= r6.x);
        r3.w = r3.w ? 1.000000 : 0;
        r1.w = r3.w * r1.w;
        r1.w = _GlobalBoxAreaColors[5].w * r1.w;
        r0.z = r1.w * r0.z;
        r2.xzw = r2.xzw + -r1.xyz;
        r1.xyz = r0.zzz * r2.xzw + r1.xyz;
      }
    }
    r0.z = cmp(_GlobalBoxAreaIntensities[6] >= 0.00100000005);
    if (r0.z != 0) {
      r0.z = (int)_GlobalBoxAreaCenters[6].w;
      r2.xz = cmp((int2)r0.zz == int2(1,4));
      r1.w = (int)r2.z | (int)r2.x;
      if (r1.w == 0) {
        r2.xzw = -_GlobalBoxAreaCenters[6].xyz + v2.xyz;
        r2.xzw = _GlobalBoxAreaExtents[6].xyz + -abs(r2.xzw);
        r1.w = min(r2.z, r2.w);
        r1.w = min(r2.x, r1.w);
        r2.x = max(0.00100000005, _GlobalBoxAreaExtents[6].w);
        r1.w = saturate(r1.w / r2.x);
        r2.xzw = log2(abs(_GlobalBoxAreaColors[6].xyz));
        r2.xzw = float3(2.20000005,2.20000005,2.20000005) * r2.xzw;
        r2.xzw = exp2(r2.xzw);
        r0.z = cmp((int)r0.z == 3);
        if (r0.z != 0) {
          r0.z = -1 + r0.x;
          r0.z = _GlobalBoxAreaShimmerParams[6].z * r0.z + 1;
        } else {
          r0.z = 1;
        }
        r3.w = cmp(_GlobalBoxAreaIntensities[6] >= r6.x);
        r3.w = r3.w ? 1.000000 : 0;
        r1.w = r3.w * r1.w;
        r1.w = _GlobalBoxAreaColors[6].w * r1.w;
        r0.z = r1.w * r0.z;
        r2.xzw = r2.xzw + -r1.xyz;
        r1.xyz = r0.zzz * r2.xzw + r1.xyz;
      }
    }
    r0.z = cmp(_GlobalBoxAreaIntensities[7] >= 0.00100000005);
    r1.w = (int)_GlobalBoxAreaCenters[7].w;
    r2.xzw = cmp((int3)r1.www == int3(1,4,3));
    r1.w = (int)r2.z | (int)r2.x;
    r4.xyz = -_GlobalBoxAreaCenters[7].xyz + v2.xyz;
    r4.xyz = _GlobalBoxAreaExtents[7].xyz + -abs(r4.xyz);
    r2.x = min(r4.y, r4.z);
    r2.x = min(r4.x, r2.x);
    r2.z = max(0.00100000005, _GlobalBoxAreaExtents[7].w);
    r2.x = saturate(r2.x / r2.z);
    r4.xyz = log2(abs(_GlobalBoxAreaColors[7].xyz));
    r4.xyz = float3(2.20000005,2.20000005,2.20000005) * r4.xyz;
    r4.xyz = exp2(r4.xyz);
    if (r2.w != 0) {
      r0.x = -1 + r0.x;
      r0.x = _GlobalBoxAreaShimmerParams[7].z * r0.x + 1;
    } else {
      r0.x = 1;
    }
    r2.z = cmp(_GlobalBoxAreaIntensities[7] >= r6.x);
    r2.z = r2.z ? 1.000000 : 0;
    r2.x = r2.x * r2.z;
    r2.x = _GlobalBoxAreaColors[7].w * r2.x;
    r0.x = r2.x * r0.x;
    r2.xzw = r4.xyz + -r1.xyz;
    r2.xzw = r0.xxx * r2.xzw + r1.xyz;
    r2.xzw = r1.www ? r1.xyz : r2.xzw;
    r1.xyz = r0.zzz ? r2.xzw : r1.xyz;
  }
  r0.x = cmp(0 < _DissolveProgress);
  if (r0.x != 0) {
    r0.x = _DissolveProgress + _DissolveProgress;
    r0.x = saturate(r0.x);
    r0.z = saturate(_DissolveProgress * 2 + -1);
    r1.w = 1 + -r2.y;
    r1.w = r1.w + -r0.x;
    r1.w = saturate(4 * r1.w);
    r2.x = r1.w * -2 + 3;
    r1.w = r1.w * r1.w;
    r1.w = -r2.x * r1.w + 1;
    r0.x = r1.w * r0.x;
    r0.x = _DissolveEdgeColor.w * r0.x;
    r2.xyz = _DissolveEdgeColor.xyz + -r1.xyz;
    r1.xyz = r0.xxx * r2.xyz + r1.xyz;
    r0.x = 1 + -r0.z;
    r2.xy = _ScreenSize.xy / _ScreenParams.xy;
    r2.zw = _ScreenParams.xy * r5.xz;
    r2.xy = r2.zw * r2.xy;
    r2.xy = floor(r2.xy);
    r2.xy = (uint2)r2.xy;
    r2.xy = (int2)r2.xy & int2(3,3);
    r0.z = (uint)r2.y << 2;
    r0.z = (int)r0.z + (int)r2.x;
    r0.z = x0[r0.z+0].x;
    r0.x = r0.x + -r0.z;
    r0.x = cmp(r0.x < 0);
    if (r0.x != 0) discard;
  }
  r0.x = 1 + -r0.y;
  r0.y = -_GlobalFogHeightMin + v2.y;
  r0.z = _GlobalFogHeightMax + -_GlobalFogHeightMin;
  r0.y = r0.y / r0.z;
  r0.y = saturate(1 + -r0.y);
  r0.y = _GlobalFogHeightEnable * r0.y;
  r0.x = max(r0.x, r0.y);
  r2.xyz = unity_FogColor.xyz + -r1.xyz;
  r0.xyz = r0.xxx * r2.xyz + r1.xyz;
  r1.x = cmp(_Surface == 1.000000);
  o0.w = r1.x ? r0.w : 1;
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
