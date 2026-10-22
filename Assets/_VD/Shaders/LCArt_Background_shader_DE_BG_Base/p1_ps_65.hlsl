// LCArt/Background/shader_DE_BG_Base | pass 1 "DepthNormals" | progFragment | keywords: _DE_REFLECTION_CAMERA | blob 65
void frag(
  float4 v0 : SV_POSITION0,
  float2 v1 : TEXCOORD1,
  float w1 : TEXCOORD6,
  float4 v2 : TEXCOORD8,
  float4 v3 : TEXCOORD2,
  float4 v4 : TEXCOORD3,
  float4 v5 : TEXCOORD4,
  float3 v6 : TEXCOORD5,
  float4 v7 : COLOR0,
  float4 v8 : TEXCOORD7,
  out float4 o0 : SV_Target0)
{
  float4 r0,r1,r2,r3,r4;
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
  r0.xyzw = _BumpMap.SampleBias(sampler_vd_point_repeat, v1.xy, _GlobalMipBias.x).xyzw;
  r0.x = r0.w * r0.x;
  r1.xy = r0.xy + r0.xy;
  r0.xy = r0.xy * float2(2,2) + float2(-1,-1);
  r0.x = dot(r0.xy, r0.xy);
  r0.x = min(1, r0.x);
  r0.x = 1 + -r0.x;
  r0.x = sqrt(r0.x);
  r1.z = max(1.00000002e-016, r0.x);
  r0.xyz = float3(-1,-1,-1) + r1.xyz;
  r0.xyz = (_BumpScale).xxx * r0.xyz + float3(0,0,1);
  r0.w = cmp(0.5 < _UseWorldNormalMap);
  if (r0.w != 0) {
    r1.xyzw = _WorldNormalMap.SampleBias(sampler_vd_point_repeat, v1.xy, _GlobalMipBias.x).xyzw;
    r1.xyz = r1.xyz * float3(2,2,2) + float3(-1,-1,-1);
    r0.w = dot(r1.xyz, r1.xyz);
    r0.w = rsqrt(r0.w);
    r1.xyz = r1.xyz * r0.www;
    r0.w = dot(v4.xyz, r1.xyz);
    r2.xyz = -r1.xyz * r0.www + v4.xyz;
    r0.w = dot(r2.xyz, r2.xyz);
    r0.w = rsqrt(r0.w);
    r2.xyz = r2.xyz * r0.www;
    r0.w = dot(v5.xyz, r1.xyz);
    r3.xyz = -r1.xyz * r0.www + v5.xyz;
    r0.w = dot(r3.xyz, r3.xyz);
    r0.w = rsqrt(r0.w);
    r3.xyz = r3.xyz * r0.www;
    r3.xyz = r3.xyz * r0.yyy;
    r2.xyz = r0.xxx * r2.xyz + r3.xyz;
    r1.xyz = r0.zzz * r1.xyz + r2.xyz;
  } else {
    r2.xyz = v5.xyz * r0.yyy;
    r0.xyw = r0.xxx * v4.xyz + r2.xyz;
    r1.xyz = r0.zzz * v3.xyz + r0.xyw;
  }
  r0.x = dot(r1.xyz, r1.xyz);
  r0.x = rsqrt(r0.x);
  o0.xyz = r1.xyz * r0.xxx;
  r0.xyzw = _BaseMap.SampleBias(sampler_BaseMap, v1.xy, _GlobalMipBias.x).xyzw;
  r0.x = -0.5 + r0.w;
  r0.x = cmp(r0.x < 0);
  if (r0.x != 0) discard;
  r0.x = cmp(0.5 < _DissolveProgress);
  if (r0.x != 0) {
    r0.x = saturate(_DissolveProgress * 2 + -1);
    r0.x = 0.999000013 + -r0.x;
    r0.x = cmp(r0.x < 0);
    if (r0.x != 0) discard;
  }
  r0.xyzw = _EmissionMap.SampleBias(sampler_EmissionMap, v1.xy, _GlobalMipBias.x).xyzw;
  r0.x = saturate(dot(r0.xx, r0.zz));
  r0.x = -0.5 + r0.x;
  r0.xy = cmp(r0.xz < float2(0,0.5));
  r0.x = r0.y ? 0 : r0.x;
  if (r0.x != 0) discard;
  r0.xy = float2(1,1) / _ScaledScreenParams.xy;
  r0.xy = v0.xy * r0.xy;
  r0.y = r0.y * _ScaleBiasRt.x + _ScaleBiasRt.y;
  r0.z = 1 + -r0.y;
  r0.y = cmp(_GlobalBoxAreaEnable >= 0.5);
  if (r0.y != 0) {
    r0.yw = float2(0.5,0.5) * v6.xz;
    r1.xy = floor(r0.yw);
    r0.yw = frac(r0.yw);
    r1.zw = r0.yw * r0.yw;
    r0.yw = -r0.yw * float2(2,2) + float2(3,3);
    r0.yw = r1.zw * r0.yw;
    r1.z = dot(r1.xy, float2(127.099998,311.700012));
    r1.z = sin(r1.z);
    r1.z = 43758.5469 * r1.z;
    r2.xyzw = float4(1,0,0,1) + r1.xyxy;
    r1.w = dot(r2.xy, float2(127.099998,311.700012));
    r1.w = sin(r1.w);
    r1.w = 43758.5469 * r1.w;
    r2.x = dot(r2.zw, float2(127.099998,311.700012));
    r2.x = sin(r2.x);
    r2.x = 43758.5469 * r2.x;
    r2.x = frac(r2.x);
    r1.xy = float2(1,1) + r1.xy;
    r1.x = dot(r1.xy, float2(127.099998,311.700012));
    r1.x = sin(r1.x);
    r1.x = 43758.5469 * r1.x;
    r1.xzw = frac(r1.xzw);
    r1.y = r1.w + -r1.z;
    r1.y = r0.y * r1.y + r1.z;
    r1.x = r1.x + -r2.x;
    r0.y = r0.y * r1.x + r2.x;
    r0.y = r0.y + -r1.y;
    r0.y = r0.w * r0.y + r1.y;
    r0.w = (int)_GlobalBoxAreaCenters[0].w;
    r0.w = cmp((int)r0.w == 4);
    if (r0.w != 0) {
      r1.xyz = -_GlobalBoxAreaCenters[0].xyz + v6.xyz;
      r1.xyz = _GlobalBoxAreaExtents[0].xyz + -abs(r1.xyz);
      r0.w = min(r1.y, r1.z);
      r0.w = min(r1.x, r0.w);
      r1.x = max(0.00100000005, _GlobalBoxAreaExtents[0].w);
      r0.w = saturate(r0.w / r1.x);
      r1.x = cmp(0 < r0.w);
      if (r1.x != 0) {
        r1.x = 1.125 + -_GlobalBoxAreaIntensities[0];
        r1.x = -r1.x + r0.y;
        r1.x = saturate(-4 * r1.x);
        r1.y = r1.x * -2 + 3;
        r1.x = r1.x * r1.x;
        r1.x = r1.y * r1.x + -0.5;
        r0.w = r1.x + -r0.w;
        r0.w = 1 + r0.w;
        r0.w = cmp(r0.w < 0);
        if (r0.w != 0) discard;
      }
    }
    r0.w = (int)_GlobalBoxAreaCenters[1].w;
    r0.w = cmp((int)r0.w == 4);
    if (r0.w != 0) {
      r1.xyz = -_GlobalBoxAreaCenters[1].xyz + v6.xyz;
      r1.xyz = _GlobalBoxAreaExtents[1].xyz + -abs(r1.xyz);
      r0.w = min(r1.y, r1.z);
      r0.w = min(r1.x, r0.w);
      r1.x = max(0.00100000005, _GlobalBoxAreaExtents[1].w);
      r0.w = saturate(r0.w / r1.x);
      r1.x = cmp(0 < r0.w);
      if (r1.x != 0) {
        r1.x = 1.125 + -_GlobalBoxAreaIntensities[1];
        r1.x = -r1.x + r0.y;
        r1.x = saturate(-4 * r1.x);
        r1.y = r1.x * -2 + 3;
        r1.x = r1.x * r1.x;
        r1.x = r1.y * r1.x + -0.5;
        r0.w = r1.x + -r0.w;
        r0.w = 1 + r0.w;
        r0.w = cmp(r0.w < 0);
        if (r0.w != 0) discard;
      }
    }
    r0.w = (int)_GlobalBoxAreaCenters[2].w;
    r0.w = cmp((int)r0.w == 4);
    if (r0.w != 0) {
      r1.xyz = -_GlobalBoxAreaCenters[2].xyz + v6.xyz;
      r1.xyz = _GlobalBoxAreaExtents[2].xyz + -abs(r1.xyz);
      r0.w = min(r1.y, r1.z);
      r0.w = min(r1.x, r0.w);
      r1.x = max(0.00100000005, _GlobalBoxAreaExtents[2].w);
      r0.w = saturate(r0.w / r1.x);
      r1.x = cmp(0 < r0.w);
      if (r1.x != 0) {
        r1.x = 1.125 + -_GlobalBoxAreaIntensities[2];
        r1.x = -r1.x + r0.y;
        r1.x = saturate(-4 * r1.x);
        r1.y = r1.x * -2 + 3;
        r1.x = r1.x * r1.x;
        r1.x = r1.y * r1.x + -0.5;
        r0.w = r1.x + -r0.w;
        r0.w = 1 + r0.w;
        r0.w = cmp(r0.w < 0);
        if (r0.w != 0) discard;
      }
    }
    r0.w = (int)_GlobalBoxAreaCenters[3].w;
    r0.w = cmp((int)r0.w == 4);
    if (r0.w != 0) {
      r1.xyz = -_GlobalBoxAreaCenters[3].xyz + v6.xyz;
      r1.xyz = _GlobalBoxAreaExtents[3].xyz + -abs(r1.xyz);
      r0.w = min(r1.y, r1.z);
      r0.w = min(r1.x, r0.w);
      r1.x = max(0.00100000005, _GlobalBoxAreaExtents[3].w);
      r0.w = saturate(r0.w / r1.x);
      r1.x = cmp(0 < r0.w);
      if (r1.x != 0) {
        r1.x = 1.125 + -_GlobalBoxAreaIntensities[3];
        r1.x = -r1.x + r0.y;
        r1.x = saturate(-4 * r1.x);
        r1.y = r1.x * -2 + 3;
        r1.x = r1.x * r1.x;
        r1.x = r1.y * r1.x + -0.5;
        r0.w = r1.x + -r0.w;
        r0.w = 1 + r0.w;
        r0.w = cmp(r0.w < 0);
        if (r0.w != 0) discard;
      }
    }
    r0.w = (int)_GlobalBoxAreaCenters[4].w;
    r0.w = cmp((int)r0.w == 4);
    if (r0.w != 0) {
      r1.xyz = -_GlobalBoxAreaCenters[4].xyz + v6.xyz;
      r1.xyz = _GlobalBoxAreaExtents[4].xyz + -abs(r1.xyz);
      r0.w = min(r1.y, r1.z);
      r0.w = min(r1.x, r0.w);
      r1.x = max(0.00100000005, _GlobalBoxAreaExtents[4].w);
      r0.w = saturate(r0.w / r1.x);
      r1.x = cmp(0 < r0.w);
      if (r1.x != 0) {
        r1.x = 1.125 + -_GlobalBoxAreaIntensities[4];
        r1.x = -r1.x + r0.y;
        r1.x = saturate(-4 * r1.x);
        r1.y = r1.x * -2 + 3;
        r1.x = r1.x * r1.x;
        r1.x = r1.y * r1.x + -0.5;
        r0.w = r1.x + -r0.w;
        r0.w = 1 + r0.w;
        r0.w = cmp(r0.w < 0);
        if (r0.w != 0) discard;
      }
    }
    r0.w = (int)_GlobalBoxAreaCenters[5].w;
    r0.w = cmp((int)r0.w == 4);
    if (r0.w != 0) {
      r1.xyz = -_GlobalBoxAreaCenters[5].xyz + v6.xyz;
      r1.xyz = _GlobalBoxAreaExtents[5].xyz + -abs(r1.xyz);
      r0.w = min(r1.y, r1.z);
      r0.w = min(r1.x, r0.w);
      r1.x = max(0.00100000005, _GlobalBoxAreaExtents[5].w);
      r0.w = saturate(r0.w / r1.x);
      r1.x = cmp(0 < r0.w);
      if (r1.x != 0) {
        r1.x = 1.125 + -_GlobalBoxAreaIntensities[5];
        r1.x = -r1.x + r0.y;
        r1.x = saturate(-4 * r1.x);
        r1.y = r1.x * -2 + 3;
        r1.x = r1.x * r1.x;
        r1.x = r1.y * r1.x + -0.5;
        r0.w = r1.x + -r0.w;
        r0.w = 1 + r0.w;
        r0.w = cmp(r0.w < 0);
        if (r0.w != 0) discard;
      }
    }
    r0.w = (int)_GlobalBoxAreaCenters[6].w;
    r0.w = cmp((int)r0.w == 4);
    if (r0.w != 0) {
      r1.xyz = -_GlobalBoxAreaCenters[6].xyz + v6.xyz;
      r1.xyz = _GlobalBoxAreaExtents[6].xyz + -abs(r1.xyz);
      r0.w = min(r1.y, r1.z);
      r0.w = min(r1.x, r0.w);
      r1.x = max(0.00100000005, _GlobalBoxAreaExtents[6].w);
      r0.w = saturate(r0.w / r1.x);
      r1.x = cmp(0 < r0.w);
      if (r1.x != 0) {
        r1.x = 1.125 + -_GlobalBoxAreaIntensities[6];
        r1.x = -r1.x + r0.y;
        r1.x = saturate(-4 * r1.x);
        r1.y = r1.x * -2 + 3;
        r1.x = r1.x * r1.x;
        r1.x = r1.y * r1.x + -0.5;
        r0.w = r1.x + -r0.w;
        r0.w = 1 + r0.w;
        r0.w = cmp(r0.w < 0);
        if (r0.w != 0) discard;
      }
    }
    r0.w = (int)_GlobalBoxAreaCenters[7].w;
    r0.w = cmp((int)r0.w == 4);
    r1.xyz = -_GlobalBoxAreaCenters[7].xyz + v6.xyz;
    r1.xyz = _GlobalBoxAreaExtents[7].xyz + -abs(r1.xyz);
    r1.y = min(r1.y, r1.z);
    r1.x = min(r1.x, r1.y);
    r1.y = max(0.00100000005, _GlobalBoxAreaExtents[7].w);
    r1.x = saturate(r1.x / r1.y);
    r1.y = cmp(0 < r1.x);
    r0.w = r0.w ? r1.y : 0;
    r1.y = 1.125 + -_GlobalBoxAreaIntensities[7];
    r0.y = -r1.y + r0.y;
    r0.y = saturate(-4 * r0.y);
    r1.y = r0.y * -2 + 3;
    r0.y = r0.y * r0.y;
    r0.y = r1.y * r0.y + -0.5;
    r0.y = r0.y + -r1.x;
    r0.y = 1 + r0.y;
    r0.y = cmp(r0.y < 0);
    r0.y = r0.w ? r0.y : 0;
    if (r0.y != 0) discard;
  }
  r0.yw = cmp(float2(1.5,1.5) < v8.zw);
  if (r0.y != 0) {
    r1.xyzw = _CustomDepthBuffer.SampleBias(sampler_CustomDepthBuffer, r0.xz, _GlobalMipBias.x).xyzw;
    r0.y = v0.z + -r1.z;
    r0.y = -2.49999994e-005 + r0.y;
    r0.y = saturate(-20000 * r0.y);
    r1.x = r0.y * -2 + 3;
    r0.y = r0.y * r0.y;
    r1.y = r1.x * r0.y;
    r2.xy = float2(1,1) + -float2(_GlobalSeeThroughCircleRange, _GlobalSeeThroughCircleAlpha);
    r1.z = 1 + -r2.x;
    r1.w = -r2.x + r1.w;
    r1.z = 1 / r1.z;
    r1.z = saturate(r1.w * r1.z);
    r1.w = r1.z * -2 + 3;
    r1.z = r1.z * r1.z;
    r1.z = r1.w * r1.z;
    r0.y = -r1.x * r0.y + 1;
    r0.y = r1.z * r0.y + r1.y;
    r1.x = _GlobalPlaneSystemEnable * _GlobalBackgroundSeeThroughEnable;
    r0.y = -1 + r0.y;
    r0.y = r1.x * r0.y + 1;
    r0.y = r0.y * r2.y + _GlobalSeeThroughCircleAlpha;
    r1.yz = _ScreenSize.xy / _ScreenParams.xy;
    r2.xy = _ScreenParams.xy * r0.xz;
    r1.yz = r2.xy * r1.yz;
    r1.yz = floor(r1.yz);
    r1.yz = (uint2)r1.yz;
    r1.yz = (int2)r1.yz & int2(3,3);
    r1.z = (uint)r1.z << 2;
    r1.y = (int)r1.z + (int)r1.y;
    r1.y = x0[r1.y+0].x;
    r0.y = -r1.y + r0.y;
    r0.y = cmp(r0.y < 0);
    if (r0.y != 0) discard;
    r0.y = cmp(0.125 < v6.y);
    if (r0.y != 0) {
      r2.xyzw = _CharacterAreaMaskTexture.SampleLevel(sampler_CharacterAreaMaskTexture, r0.xz, 0).xyzw;
      r1.yz = round(_WorldSpaceCameraPos.xz);
      r1.yz = v6.xz + -r1.yz;
      r3.x = -(VD_COL(unity_MatrixV, 0).z);
      r3.y = -(VD_COL(unity_MatrixV, 2).z);
      r0.y = dot(r3.xy, r3.xy);
      r0.y = rsqrt(r0.y);
      r2.zw = r3.xy * r0.yy;
      r0.y = dot(r1.yz, r2.zw);
      r0.y = saturate(0.00999999978 * r0.y);
      r0.y = r2.y + -r0.y;
      r0.y = saturate(40 * r0.y);
      r1.y = r0.y * -2 + 3;
      r0.y = r0.y * r0.y;
      r0.y = r1.y * r0.y;
      r1.y = -1 + r2.x;
      r0.y = r1.y * r0.y;
      r0.y = _GlobalBackgroundSeeThroughEnable * r0.y + 1;
      r1.yzw = -_WorldSpaceCameraPos.xyz + v6.xyz;
      r2.x = -0.150000006 + -_WorldSpaceCameraPos.y;
      r1.z = r2.x / r1.z;
      r1.yz = r1.yw * r1.zz + _WorldSpaceCameraPos.xz;
      r1.yz = -_PlaneBoundMaxPos.xz + r1.yz;
      r2.xy = -_PlaneBoundMaxPos.xz + _PlaneBoundMinPos.xz;
      r1.yz = saturate(r1.yz / r2.xy);
      r2.xyzw = _PlaneSightRenderTarget.SampleLevel(sampler_PlaneSightRenderTarget, r1.yz, 0).xyzw;
      r1.y = -1 + r2.w;
      r1.y = _GlobalPlaneSystemEnable * r1.y + 1;
      r1.z = _SeeThroughDistanceMax + -_SeeThroughDistanceMin;
      r1.z = 1 / r1.z;
      r1.w = 1;
      r2.x = 0;
      while (true) {
        r2.y = cmp((int)r2.x >= 3);
        if (r2.y != 0) break;
        r2.yzw = -_SightPositions[(int)(r2.x)].xyz + v6.xyz;
        r2.y = dot(r2.yzw, r2.yzw);
        r2.y = sqrt(r2.y);
        r2.y = -_SeeThroughDistanceMin + r2.y;
        r2.y = saturate(r2.y * r1.z);
        r2.z = r2.y * -2 + 3;
        r2.y = r2.y * r2.y;
        r2.y = r2.z * r2.y;
        r1.w = min(r2.y, r1.w);
        r2.x = (int)r2.x + 1;
      }
      r1.y = r1.y * r1.y;
      r1.z = 1 + -r1.w;
      r1.y = -r1.y * r1.z + 1;
      r1.y = max(0, r1.y);
      r1.y = -1 + r1.y;
      r1.x = r1.x * r1.y + 1;
      r0.y = min(r1.x, r0.y);
    } else {
      r0.y = w1.x;
    }
    r1.x = _GlobalSeeThroughHeightMax + -_GlobalSeeThroughHeightMin;
    r1.y = -_GlobalSeeThroughHeightMin + v6.y;
    r1.x = 1 / r1.x;
    r1.x = saturate(r1.y * r1.x);
    r1.y = r1.x * -2 + 3;
    r1.x = r1.x * r1.x;
    r1.x = -r1.y * r1.x + 1;
    r0.y = max(r1.x, r0.y);
    r0.y = max(0.00100000005, r0.y);
  } else {
    r0.y = w1.x;
  }
  r1.xyz = cmp(vd_asint((_BackgroundType).xxx) == int3(2,3,7));
  r0.w = r0.w ? r1.x : 0;
  if (r0.w != 0) {
    r2.xyzw = _CustomDepthBuffer.SampleBias(sampler_CustomDepthBuffer, r0.xz, _GlobalMipBias.x).xyzw;
    r0.w = v0.z + -r2.z;
    r0.w = -2.49999994e-005 + r0.w;
    r0.w = saturate(-20000 * r0.w);
    r1.x = r0.w * -2 + 3;
    r0.w = r0.w * r0.w;
    r1.w = r1.x * r0.w;
    r2.xy = float2(1,1) + -float2(_GlobalSeeThroughCircleRange, _GlobalSeeThroughCircleAlpha);
    r2.z = 1 + -r2.x;
    r2.x = r2.w + -r2.x;
    r2.z = 1 / r2.z;
    r2.x = saturate(r2.x * r2.z);
    r2.z = r2.x * -2 + 3;
    r2.x = r2.x * r2.x;
    r2.x = r2.z * r2.x;
    r0.w = -r1.x * r0.w + 1;
    r0.w = r2.x * r0.w + r1.w;
    r1.x = _GlobalPlaneSystemEnable * _GlobalBackgroundSeeThroughEnable;
    r0.w = -1 + r0.w;
    r0.w = r1.x * r0.w + 1;
    r0.w = r0.w * r2.y + _GlobalSeeThroughCircleAlpha;
    r2.xy = _ScreenSize.xy / _ScreenParams.xy;
    r2.zw = _ScreenParams.xy * r0.xz;
    r2.xy = r2.zw * r2.xy;
    r2.xy = floor(r2.xy);
    r2.xy = (uint2)r2.xy;
    r2.xy = (int2)r2.xy & int2(3,3);
    r1.w = (uint)r2.y << 2;
    r1.w = (int)r1.w + (int)r2.x;
    r1.w = x0[r1.w+0].x;
    r0.w = -r1.w + r0.w;
    r0.w = cmp(r0.w < 0);
    if (r0.w != 0) discard;
    r0.w = cmp(0.125 < v6.y);
    if (r0.w != 0) {
      r2.xyzw = _CharacterAreaMaskTexture.SampleLevel(sampler_CharacterAreaMaskTexture, r0.xz, 0).xyzw;
      r2.zw = round(_WorldSpaceCameraPos.xz);
      r2.zw = v6.xz + -r2.zw;
      r3.x = -(VD_COL(unity_MatrixV, 0).z);
      r3.y = -(VD_COL(unity_MatrixV, 2).z);
      r0.w = dot(r3.xy, r3.xy);
      r0.w = rsqrt(r0.w);
      r3.xy = r3.xy * r0.ww;
      r0.w = dot(r2.zw, r3.xy);
      r0.w = saturate(0.00999999978 * r0.w);
      r0.w = r2.y + -r0.w;
      r0.w = saturate(40 * r0.w);
      r1.w = r0.w * -2 + 3;
      r0.w = r0.w * r0.w;
      r0.w = r1.w * r0.w;
      r1.w = -1 + r2.x;
      r0.w = r1.w * r0.w;
      r0.w = _GlobalBackgroundSeeThroughEnable * r0.w + 1;
      r2.xyz = -_WorldSpaceCameraPos.xyz + v6.xyz;
      r1.w = -0.150000006 + -_WorldSpaceCameraPos.y;
      r1.w = r1.w / r2.y;
      r2.xy = r2.xz * r1.ww + _WorldSpaceCameraPos.xz;
      r2.xy = -_PlaneBoundMaxPos.xz + r2.xy;
      r2.zw = -_PlaneBoundMaxPos.xz + _PlaneBoundMinPos.xz;
      r2.xy = saturate(r2.xy / r2.zw);
      r2.xyzw = _PlaneSightRenderTarget.SampleLevel(sampler_PlaneSightRenderTarget, r2.xy, 0).xyzw;
      r1.w = -1 + r2.w;
      r1.w = _GlobalPlaneSystemEnable * r1.w + 1;
      r2.x = _SeeThroughDistanceMax + -_SeeThroughDistanceMin;
      r2.x = 1 / r2.x;
      r2.yz = float2(1,0);
      while (true) {
        r2.w = cmp((int)r2.z >= 3);
        if (r2.w != 0) break;
        r3.xyz = -_SightPositions[(int)(r2.z)].xyz + v6.xyz;
        r2.w = dot(r3.xyz, r3.xyz);
        r2.w = sqrt(r2.w);
        r2.w = -_SeeThroughDistanceMin + r2.w;
        r2.w = saturate(r2.w * r2.x);
        r3.x = r2.w * -2 + 3;
        r2.w = r2.w * r2.w;
        r2.w = r3.x * r2.w;
        r2.y = min(r2.y, r2.w);
        r2.z = (int)r2.z + 1;
      }
      r1.w = r1.w * r1.w;
      r2.x = 1 + -r2.y;
      r1.w = -r1.w * r2.x + 1;
      r1.w = max(0, r1.w);
      r1.w = -1 + r1.w;
      r1.x = r1.x * r1.w + 1;
      r0.y = min(r1.x, r0.w);
    }
    r0.w = _GlobalSeeThroughHeightMax + -_GlobalSeeThroughHeightMin;
    r1.x = -_GlobalSeeThroughHeightMin + v6.y;
    r0.w = 1 / r0.w;
    r0.w = saturate(r1.x * r0.w);
    r1.x = r0.w * -2 + 3;
    r0.w = r0.w * r0.w;
    r0.w = -r1.x * r0.w + 1;
    r0.w = max(r0.w, r0.y);
    r0.y = max(0.00100000005, r0.w);
  }
  r0.w = (int)r1.z | (int)r1.y;
  if (r0.w != 0) {
    r0.w = -_GlobalSeeThroughBuildingHeightMin + _GlobalSeeThroughBuildingHeightMax;
    r1.x = -_GlobalSeeThroughBuildingHeightMin + v6.y;
    r0.w = 1 / r0.w;
    r0.w = saturate(r1.x * r0.w);
    r1.x = r0.w * -2 + 3;
    r0.w = r0.w * r0.w;
    r0.w = -r1.x * r0.w + 1;
    r1.x = 1 + -_GlobalSeeThroughBuildingAlpha;
    r0.w = r0.w * r1.x + _GlobalSeeThroughBuildingAlpha;
    r1.x = 1 + -r0.w;
    r0.w = r0.y * r1.x + r0.w;
    r0.w = -1 + r0.w;
    r0.y = _GlobalIsRuntime * r0.w + 1;
    r1.xy = _ScreenSize.xy / _ScreenParams.xy;
    r1.zw = _ScreenParams.xy * r0.xz;
    r1.xy = r1.zw * r1.xy;
    r1.xy = floor(r1.xy);
    r1.xy = (uint2)r1.xy;
    r1.xy = (int2)r1.xy & int2(3,3);
    r0.w = (uint)r1.y << 2;
    r0.w = (int)r0.w + (int)r1.x;
    r0.w = x0[r0.w+0].x;
    r0.w = r0.y + -r0.w;
    r0.w = cmp(r0.w < 0);
    if (r0.w != 0) discard;
  } else {
    r0.w = cmp(vd_asint(_BackgroundType) == 4);
    if (r0.w != 0) {
      r0.w = cmp(0.125 < v6.y);
      if (r0.w != 0) {
        r1.xyzw = _CharacterAreaMaskTexture.SampleLevel(sampler_CharacterAreaMaskTexture, r0.xz, 0).xyzw;
        r1.zw = round(_WorldSpaceCameraPos.xz);
        r1.zw = v6.xz + -r1.zw;
        r2.x = -(VD_COL(unity_MatrixV, 0).z);
        r2.y = -(VD_COL(unity_MatrixV, 2).z);
        r0.w = dot(r2.xy, r2.xy);
        r0.w = rsqrt(r0.w);
        r2.xy = r2.xy * r0.ww;
        r0.w = dot(r1.zw, r2.xy);
        r0.w = saturate(0.00999999978 * r0.w);
        r0.w = r1.y + -r0.w;
        r0.w = saturate(40 * r0.w);
        r1.y = r0.w * -2 + 3;
        r0.w = r0.w * r0.w;
        r0.w = r1.y * r0.w;
        r1.x = -1 + r1.x;
        r0.w = r1.x * r0.w;
        r0.w = _GlobalBackgroundSeeThroughEnable * r0.w + 1;
        r1.xyz = -_WorldSpaceCameraPos.xyz + v6.xyz;
        r2.xyzw = float4(2,2,1,1) + -_WorldSpaceCameraPos.yyyy;
        r2.xyzw = r2.xyzw / r1.yyyy;
        r2.xyzw = r1.xzxz * r2.xyzw + _WorldSpaceCameraPos.xzxz;
        r2.xyzw = -_PlaneBoundMaxPos.xzxz + r2.xyzw;
        r3.xy = -_PlaneBoundMaxPos.xz + _PlaneBoundMinPos.xz;
        r2.xyzw = saturate(r2.xyzw / r3.xyxy);
        r4.xyzw = _PlaneSightRenderTarget.SampleLevel(sampler_PlaneSightRenderTarget, r2.xy, 0).xyzw;
        r1.w = -1 + r4.w;
        r1.w = _GlobalPlaneSystemEnable * r1.w + 1;
        r2.x = _SeeThroughDistanceMax + -_SeeThroughDistanceMin;
        r2.x = 1 / r2.x;
        r2.y = 1;
        r3.z = 0;
        while (true) {
          r3.w = cmp((int)r3.z >= 3);
          if (r3.w != 0) break;
          r4.xyz = -_SightPositions[(int)(r3.z)].xyz + v6.xyz;
          r3.w = dot(r4.xyz, r4.xyz);
          r3.w = sqrt(r3.w);
          r3.w = -_SeeThroughDistanceMin + r3.w;
          r3.w = saturate(r3.w * r2.x);
          r4.x = r3.w * -2 + 3;
          r3.w = r3.w * r3.w;
          r3.w = r4.x * r3.w;
          r2.y = min(r3.w, r2.y);
          r3.z = (int)r3.z + 1;
        }
        r3.z = _GlobalPlaneSystemEnable * _GlobalBackgroundSeeThroughEnable;
        r1.w = r1.w * r1.w;
        r2.y = 1 + -r2.y;
        r1.w = -r1.w * r2.y + 1;
        r1.w = max(0, r1.w);
        r1.w = -1 + r1.w;
        r1.w = r3.z * r1.w + 1;
        r4.xyzw = _PlaneSightRenderTarget.SampleLevel(sampler_PlaneSightRenderTarget, r2.zw, 0).xyzw;
        r2.y = -1 + r4.w;
        r2.y = _GlobalPlaneSystemEnable * r2.y + 1;
        r2.zw = float2(1,0);
        while (true) {
          r3.w = cmp((int)r2.w >= 3);
          if (r3.w != 0) break;
          r4.xyz = -_SightPositions[(int)(r2.w)].xyz + v6.xyz;
          r3.w = dot(r4.xyz, r4.xyz);
          r3.w = sqrt(r3.w);
          r3.w = -_SeeThroughDistanceMin + r3.w;
          r3.w = saturate(r3.w * r2.x);
          r4.x = r3.w * -2 + 3;
          r3.w = r3.w * r3.w;
          r3.w = r4.x * r3.w;
          r2.z = min(r3.w, r2.z);
          r2.w = (int)r2.w + 1;
        }
        r2.y = r2.y * r2.y;
        r2.z = 1 + -r2.z;
        r2.y = -r2.y * r2.z + 1;
        r2.y = max(0, r2.y);
        r2.y = -1 + r2.y;
        r2.y = r3.z * r2.y + 1;
        r1.y = -_WorldSpaceCameraPos.y / r1.y;
        r1.xy = r1.xz * r1.yy + _WorldSpaceCameraPos.xz;
        r1.xy = -_PlaneBoundMaxPos.xz + r1.xy;
        r1.xy = saturate(r1.xy / r3.xy);
        r4.xyzw = _PlaneSightRenderTarget.SampleLevel(sampler_PlaneSightRenderTarget, r1.xy, 0).xyzw;
        r1.x = -1 + r4.w;
        r1.x = _GlobalPlaneSystemEnable * r1.x + 1;
        r1.yz = float2(1,0);
        while (true) {
          r2.z = cmp((int)r1.z >= 3);
          if (r2.z != 0) break;
          r3.xyw = -_SightPositions[(int)(r1.z)].xyz + v6.xyz;
          r2.z = dot(r3.xyw, r3.xyw);
          r2.z = sqrt(r2.z);
          r2.z = -_SeeThroughDistanceMin + r2.z;
          r2.z = saturate(r2.z * r2.x);
          r2.w = r2.z * -2 + 3;
          r2.z = r2.z * r2.z;
          r2.z = r2.w * r2.z;
          r1.y = min(r2.z, r1.y);
          r1.z = (int)r1.z + 1;
        }
        r1.x = r1.x * r1.x;
        r1.y = 1 + -r1.y;
        r1.x = -r1.x * r1.y + 1;
        r1.x = max(0, r1.x);
        r1.x = -1 + r1.x;
        r1.x = r3.z * r1.x + 1;
        r1.x = min(r2.y, r1.x);
        r1.x = min(r1.w, r1.x);
        r0.w = min(r1.x, r0.w);
        r0.w = min(_GlobalCoverSeeThroughFactor, r0.w);
        r0.w = -1 + r0.w;
        r0.y = _GlobalIsRuntime * r0.w + 1;
        r1.xy = _ScreenSize.xy / _ScreenParams.xy;
        r1.zw = _ScreenParams.xy * r0.xz;
        r1.xy = r1.zw * r1.xy;
        r1.xy = floor(r1.xy);
        r1.xy = (uint2)r1.xy;
        r1.xy = (int2)r1.xy & int2(3,3);
        r0.w = (uint)r1.y << 2;
        r0.w = (int)r0.w + (int)r1.x;
        r0.w = x0[r0.w+0].x;
        r0.w = r0.y + -r0.w;
        r0.w = cmp(r0.w < 0);
        if (r0.w != 0) discard;
      }
    }
  }
  r0.w = 1 + -_GlobalSeeThroughAlpha;
  r0.y = r0.y * r0.w + _GlobalSeeThroughAlpha;
  r0.y = -1 + r0.y;
  r0.y = _GlobalIsRuntime * r0.y + 1;
  r1.xy = _ScreenSize.xy / _ScreenParams.xy;
  r0.xz = _ScreenParams.xy * r0.xz;
  r0.xz = r0.xz * r1.xy;
  r0.xz = floor(r0.xz);
  r0.xz = (uint2)r0.xz;
  r0.xz = (int2)r0.xz & int2(3,3);
  r0.z = (uint)r0.z << 2;
  r0.x = (int)r0.z + (int)r0.x;
  r0.x = x0[r0.x+0].x;
  r0.x = r0.y * _BaseColor.w + -r0.x;
  r0.x = cmp(r0.x < 0);
  if (r0.x != 0) discard;
  o0.w = 0;
  return;
}
