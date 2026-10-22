// LCArt/Background/shader_DE_BG_Base | pass 1 "DepthNormals" | progFragment | keywords: _DE_EDITOR_CAMERA _DE_REFLECTION_CAMERA | blob 67
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
  float4 r0,r1,r2,r3;
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
  r0.x = cmp(_GlobalBoxAreaEnable >= 0.5);
  if (r0.x != 0) {
    r0.xy = float2(0.5,0.5) * v6.xz;
    r0.zw = floor(r0.xy);
    r0.xy = frac(r0.xy);
    r2.xy = r0.xy * r0.xy;
    r0.xy = -r0.xy * float2(2,2) + float2(3,3);
    r0.xy = r2.xy * r0.xy;
    r1.w = dot(r0.zw, float2(127.099998,311.700012));
    r1.w = sin(r1.w);
    r1.w = 43758.5469 * r1.w;
    r1.w = frac(r1.w);
    r2.xyzw = float4(1,0,0,1) + r0.zwzw;
    r2.x = dot(r2.xy, float2(127.099998,311.700012));
    r2.x = sin(r2.x);
    r2.x = 43758.5469 * r2.x;
    r2.y = dot(r2.zw, float2(127.099998,311.700012));
    r2.y = sin(r2.y);
    r2.y = 43758.5469 * r2.y;
    r2.xy = frac(r2.xy);
    r0.zw = float2(1,1) + r0.zw;
    r0.z = dot(r0.zw, float2(127.099998,311.700012));
    r0.z = sin(r0.z);
    r0.z = 43758.5469 * r0.z;
    r0.z = frac(r0.z);
    r0.w = r2.x + -r1.w;
    r0.w = r0.x * r0.w + r1.w;
    r0.z = r0.z + -r2.y;
    r0.x = r0.x * r0.z + r2.y;
    r0.x = r0.x + -r0.w;
    r0.x = r0.y * r0.x + r0.w;
    r0.y = (int)_GlobalBoxAreaCenters[0].w;
    r0.y = cmp((int)r0.y == 4);
    if (r0.y != 0) {
      r0.yzw = -_GlobalBoxAreaCenters[0].xyz + v6.xyz;
      r0.yzw = _GlobalBoxAreaExtents[0].xyz + -abs(r0.yzw);
      r0.z = min(r0.z, r0.w);
      r0.y = min(r0.y, r0.z);
      r0.z = max(0.00100000005, _GlobalBoxAreaExtents[0].w);
      r0.y = saturate(r0.y / r0.z);
      r0.z = cmp(0 < r0.y);
      if (r0.z != 0) {
        r0.z = 1.125 + -_GlobalBoxAreaIntensities[0];
        r0.z = r0.x + -r0.z;
        r0.z = saturate(-4 * r0.z);
        r0.w = r0.z * -2 + 3;
        r0.z = r0.z * r0.z;
        r0.z = r0.w * r0.z + -0.5;
        r0.y = r0.z + -r0.y;
        r0.y = 1 + r0.y;
        r0.y = cmp(r0.y < 0);
        if (r0.y != 0) discard;
      }
    }
    r0.y = (int)_GlobalBoxAreaCenters[1].w;
    r0.y = cmp((int)r0.y == 4);
    if (r0.y != 0) {
      r0.yzw = -_GlobalBoxAreaCenters[1].xyz + v6.xyz;
      r0.yzw = _GlobalBoxAreaExtents[1].xyz + -abs(r0.yzw);
      r0.z = min(r0.z, r0.w);
      r0.y = min(r0.y, r0.z);
      r0.z = max(0.00100000005, _GlobalBoxAreaExtents[1].w);
      r0.y = saturate(r0.y / r0.z);
      r0.z = cmp(0 < r0.y);
      if (r0.z != 0) {
        r0.z = 1.125 + -_GlobalBoxAreaIntensities[1];
        r0.z = r0.x + -r0.z;
        r0.z = saturate(-4 * r0.z);
        r0.w = r0.z * -2 + 3;
        r0.z = r0.z * r0.z;
        r0.z = r0.w * r0.z + -0.5;
        r0.y = r0.z + -r0.y;
        r0.y = 1 + r0.y;
        r0.y = cmp(r0.y < 0);
        if (r0.y != 0) discard;
      }
    }
    r0.y = (int)_GlobalBoxAreaCenters[2].w;
    r0.y = cmp((int)r0.y == 4);
    if (r0.y != 0) {
      r0.yzw = -_GlobalBoxAreaCenters[2].xyz + v6.xyz;
      r0.yzw = _GlobalBoxAreaExtents[2].xyz + -abs(r0.yzw);
      r0.z = min(r0.z, r0.w);
      r0.y = min(r0.y, r0.z);
      r0.z = max(0.00100000005, _GlobalBoxAreaExtents[2].w);
      r0.y = saturate(r0.y / r0.z);
      r0.z = cmp(0 < r0.y);
      if (r0.z != 0) {
        r0.z = 1.125 + -_GlobalBoxAreaIntensities[2];
        r0.z = r0.x + -r0.z;
        r0.z = saturate(-4 * r0.z);
        r0.w = r0.z * -2 + 3;
        r0.z = r0.z * r0.z;
        r0.z = r0.w * r0.z + -0.5;
        r0.y = r0.z + -r0.y;
        r0.y = 1 + r0.y;
        r0.y = cmp(r0.y < 0);
        if (r0.y != 0) discard;
      }
    }
    r0.y = (int)_GlobalBoxAreaCenters[3].w;
    r0.y = cmp((int)r0.y == 4);
    if (r0.y != 0) {
      r0.yzw = -_GlobalBoxAreaCenters[3].xyz + v6.xyz;
      r0.yzw = _GlobalBoxAreaExtents[3].xyz + -abs(r0.yzw);
      r0.z = min(r0.z, r0.w);
      r0.y = min(r0.y, r0.z);
      r0.z = max(0.00100000005, _GlobalBoxAreaExtents[3].w);
      r0.y = saturate(r0.y / r0.z);
      r0.z = cmp(0 < r0.y);
      if (r0.z != 0) {
        r0.z = 1.125 + -_GlobalBoxAreaIntensities[3];
        r0.z = r0.x + -r0.z;
        r0.z = saturate(-4 * r0.z);
        r0.w = r0.z * -2 + 3;
        r0.z = r0.z * r0.z;
        r0.z = r0.w * r0.z + -0.5;
        r0.y = r0.z + -r0.y;
        r0.y = 1 + r0.y;
        r0.y = cmp(r0.y < 0);
        if (r0.y != 0) discard;
      }
    }
    r0.y = (int)_GlobalBoxAreaCenters[4].w;
    r0.y = cmp((int)r0.y == 4);
    if (r0.y != 0) {
      r0.yzw = -_GlobalBoxAreaCenters[4].xyz + v6.xyz;
      r0.yzw = _GlobalBoxAreaExtents[4].xyz + -abs(r0.yzw);
      r0.z = min(r0.z, r0.w);
      r0.y = min(r0.y, r0.z);
      r0.z = max(0.00100000005, _GlobalBoxAreaExtents[4].w);
      r0.y = saturate(r0.y / r0.z);
      r0.z = cmp(0 < r0.y);
      if (r0.z != 0) {
        r0.z = 1.125 + -_GlobalBoxAreaIntensities[4];
        r0.z = r0.x + -r0.z;
        r0.z = saturate(-4 * r0.z);
        r0.w = r0.z * -2 + 3;
        r0.z = r0.z * r0.z;
        r0.z = r0.w * r0.z + -0.5;
        r0.y = r0.z + -r0.y;
        r0.y = 1 + r0.y;
        r0.y = cmp(r0.y < 0);
        if (r0.y != 0) discard;
      }
    }
    r0.y = (int)_GlobalBoxAreaCenters[5].w;
    r0.y = cmp((int)r0.y == 4);
    if (r0.y != 0) {
      r0.yzw = -_GlobalBoxAreaCenters[5].xyz + v6.xyz;
      r0.yzw = _GlobalBoxAreaExtents[5].xyz + -abs(r0.yzw);
      r0.z = min(r0.z, r0.w);
      r0.y = min(r0.y, r0.z);
      r0.z = max(0.00100000005, _GlobalBoxAreaExtents[5].w);
      r0.y = saturate(r0.y / r0.z);
      r0.z = cmp(0 < r0.y);
      if (r0.z != 0) {
        r0.z = 1.125 + -_GlobalBoxAreaIntensities[5];
        r0.z = r0.x + -r0.z;
        r0.z = saturate(-4 * r0.z);
        r0.w = r0.z * -2 + 3;
        r0.z = r0.z * r0.z;
        r0.z = r0.w * r0.z + -0.5;
        r0.y = r0.z + -r0.y;
        r0.y = 1 + r0.y;
        r0.y = cmp(r0.y < 0);
        if (r0.y != 0) discard;
      }
    }
    r0.y = (int)_GlobalBoxAreaCenters[6].w;
    r0.y = cmp((int)r0.y == 4);
    if (r0.y != 0) {
      r0.yzw = -_GlobalBoxAreaCenters[6].xyz + v6.xyz;
      r0.yzw = _GlobalBoxAreaExtents[6].xyz + -abs(r0.yzw);
      r0.z = min(r0.z, r0.w);
      r0.y = min(r0.y, r0.z);
      r0.z = max(0.00100000005, _GlobalBoxAreaExtents[6].w);
      r0.y = saturate(r0.y / r0.z);
      r0.z = cmp(0 < r0.y);
      if (r0.z != 0) {
        r0.z = 1.125 + -_GlobalBoxAreaIntensities[6];
        r0.z = r0.x + -r0.z;
        r0.z = saturate(-4 * r0.z);
        r0.w = r0.z * -2 + 3;
        r0.z = r0.z * r0.z;
        r0.z = r0.w * r0.z + -0.5;
        r0.y = r0.z + -r0.y;
        r0.y = 1 + r0.y;
        r0.y = cmp(r0.y < 0);
        if (r0.y != 0) discard;
      }
    }
    r0.y = (int)_GlobalBoxAreaCenters[7].w;
    r0.y = cmp((int)r0.y == 4);
    r2.xyz = -_GlobalBoxAreaCenters[7].xyz + v6.xyz;
    r2.xyz = _GlobalBoxAreaExtents[7].xyz + -abs(r2.xyz);
    r0.z = min(r2.y, r2.z);
    r0.z = min(r2.x, r0.z);
    r0.w = max(0.00100000005, _GlobalBoxAreaExtents[7].w);
    r0.z = saturate(r0.z / r0.w);
    r0.w = cmp(0 < r0.z);
    r0.y = r0.w ? r0.y : 0;
    r0.w = 1.125 + -_GlobalBoxAreaIntensities[7];
    r0.x = r0.x + -r0.w;
    r0.x = saturate(-4 * r0.x);
    r0.w = r0.x * -2 + 3;
    r0.x = r0.x * r0.x;
    r0.x = r0.w * r0.x + -0.5;
    r0.x = r0.x + -r0.z;
    r0.x = 1 + r0.x;
    r0.x = cmp(r0.x < 0);
    r0.x = r0.y ? r0.x : 0;
    if (r0.x != 0) discard;
  }
  r0.x = cmp(vd_asint(_BackgroundType) == 4);
  if (r0.x != 0) discard;
  r0.xy = float2(1,1) / _ScaledScreenParams.xy;
  r0.xy = v0.xy * r0.xy;
  r0.y = r0.y * _ScaleBiasRt.x + _ScaleBiasRt.y;
  r0.z = 1 + -r0.y;
  r0.y = 1 + -_GlobalSeeThroughAlpha;
  r0.y = w1.x * r0.y + _GlobalSeeThroughAlpha;
  r0.y = -1 + r0.y;
  r0.y = _GlobalIsRuntime * r0.y + 1;
  r2.xy = _ScreenSize.xy / _ScreenParams.xy;
  r0.xz = _ScreenParams.xy * r0.xz;
  r0.xz = r0.xz * r2.xy;
  r0.xz = floor(r0.xz);
  r0.xz = (uint2)r0.xz;
  r0.xz = (int2)r0.xz & int2(3,3);
  r0.z = (uint)r0.z << 2;
  r0.x = (int)r0.z + (int)r0.x;
  r0.x = x0[r0.x+0].x;
  r0.x = r0.y * _BaseColor.w + -r0.x;
  r0.x = cmp(r0.x < 0);
  if (r0.x != 0) discard;
  r0.x = dot(r1.xyz, r1.xyz);
  r0.x = rsqrt(r0.x);
  o0.xyz = r1.xyz * r0.xxx;
  o0.w = 0;
  return;
}
