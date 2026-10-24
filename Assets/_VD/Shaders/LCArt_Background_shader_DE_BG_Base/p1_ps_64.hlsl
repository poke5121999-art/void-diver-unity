// LCArt/Background/shader_DE_BG_Base | pass 1 "DepthNormals" | progFragment | keywords: (none) | blob 64
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
  out float4 o0 : SV_Target0)
{
#if UNITY_UV_STARTS_AT_TOP
  uint4 r0, r1, r2, r3, r4;
  r0 = 0u; r1 = 0u; r2 = 0u; r3 = 0u; r4 = 0u;
#else
  uint4 r0, r1, r2, r3, r4, r5;
  r0 = 0u; r1 = 0u; r2 = 0u; r3 = 0u; r4 = 0u; r5 = 0u;
#endif
  uint4 x0[16];

#if UNITY_UV_STARTS_AT_TOP
#else
  r5.xyzw = vd_asuint(v0.xyzw);
  r5.z = vd_asuint(((-(vd_asfloat(v0.z))) + 1.0));
#endif
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
  r0.xyzw = vd_asuint(_BumpMap.SampleBias(sampler_vd_point_repeat, vd_asfloat(v1.xy), vd_asfloat(_GlobalMipBias.x)).xyzw);
  r0.x = vd_asuint((vd_asfloat(r0.x) * vd_asfloat(r0.w)));
  r1.xy = vd_asuint((vd_asfloat(r0.xy) + vd_asfloat(r0.xy)));
  r0.xy = vd_asuint((vd_asfloat(r0.xy) * float2(2.0, 2.0) + float2(-1.0, -1.0)));
  r0.x = vd_asuint(dot(vd_asfloat(r0.xy), vd_asfloat(r0.xy)));
  r0.x = vd_asuint(min(vd_asfloat(r0.x), 1.0));
  r0.x = vd_asuint(((-(vd_asfloat(r0.x))) + 1.0));
  r0.x = vd_asuint(sqrt(vd_asfloat(r0.x)));
  r1.z = vd_asuint(max(vd_asfloat(r0.x), 1.00000002e-16));
  r0.xyz = vd_asuint((vd_asfloat(r1.xyz) + float3(-1.0, -1.0, -1.0)));
  r0.xyz = vd_asuint((vd_asfloat((_BumpScale).xxx) * vd_asfloat(r0.xyz) + float3(0.0, 0.0, 1.0)));
  r0.w = vd_asuint((-(int)(0.5 < vd_asfloat(_UseWorldNormalMap))));
  if (r0.w != 0u) {
    r1.xyzw = vd_asuint(_WorldNormalMap.SampleBias(sampler_vd_point_repeat, vd_asfloat(v1.xy), vd_asfloat(_GlobalMipBias.x)).xyzw);
    r1.xyz = vd_asuint((vd_asfloat(r1.xyz) * float3(2.0, 2.0, 2.0) + float3(-1.0, -1.0, -1.0)));
    r0.w = vd_asuint(dot(vd_asfloat(r1.xyz), vd_asfloat(r1.xyz)));
    r0.w = vd_asuint(rsqrt(vd_asfloat(r0.w)));
    r1.xyz = vd_asuint((vd_asfloat(r0.www) * vd_asfloat(r1.xyz)));
    r0.w = vd_asuint(dot(vd_asfloat(v4.xyz), vd_asfloat(r1.xyz)));
    r2.xyz = vd_asuint(((-(vd_asfloat(r1.xyz))) * vd_asfloat(r0.www) + vd_asfloat(v4.xyz)));
    r0.w = vd_asuint(dot(vd_asfloat(r2.xyz), vd_asfloat(r2.xyz)));
    r0.w = vd_asuint(rsqrt(vd_asfloat(r0.w)));
    r2.xyz = vd_asuint((vd_asfloat(r0.www) * vd_asfloat(r2.xyz)));
    r0.w = vd_asuint(dot(vd_asfloat(v5.xyz), vd_asfloat(r1.xyz)));
    r3.xyz = vd_asuint(((-(vd_asfloat(r1.xyz))) * vd_asfloat(r0.www) + vd_asfloat(v5.xyz)));
    r0.w = vd_asuint(dot(vd_asfloat(r3.xyz), vd_asfloat(r3.xyz)));
    r0.w = vd_asuint(rsqrt(vd_asfloat(r0.w)));
    r3.xyz = vd_asuint((vd_asfloat(r0.www) * vd_asfloat(r3.xyz)));
    r3.xyz = vd_asuint((vd_asfloat(r0.yyy) * vd_asfloat(r3.xyz)));
    r2.xyz = vd_asuint((vd_asfloat(r0.xxx) * vd_asfloat(r2.xyz) + vd_asfloat(r3.xyz)));
    r1.xyz = vd_asuint((vd_asfloat(r0.zzz) * vd_asfloat(r1.xyz) + vd_asfloat(r2.xyz)));
  } else {
    r2.xyz = vd_asuint((vd_asfloat(r0.yyy) * vd_asfloat(v5.xyz)));
    r0.xyw = vd_asuint((vd_asfloat(r0.xxx) * vd_asfloat(v4.xyz) + vd_asfloat(r2.xyz)));
    r1.xyz = vd_asuint((vd_asfloat(r0.zzz) * vd_asfloat(v3.xyz) + vd_asfloat(r0.xyw)));
  }
  r0.x = vd_asuint(dot(vd_asfloat(r1.xyz), vd_asfloat(r1.xyz)));
  r0.x = vd_asuint(rsqrt(vd_asfloat(r0.x)));
  o0.xyz = (vd_asfloat(r0.xxx) * vd_asfloat(r1.xyz));
  r0.xyzw = vd_asuint(_BaseMap.SampleBias(sampler_BaseMap, vd_asfloat(v1.xy), vd_asfloat(_GlobalMipBias.x)).xyzw);
  r0.x = vd_asuint((vd_asfloat(r0.w) + -0.5));
  r0.x = vd_asuint((-(int)(vd_asfloat(r0.x) < 0.0)));
  if (r0.x != 0u) discard;
  r0.x = vd_asuint((-(int)(0.5 < vd_asfloat(_DissolveProgress))));
  if (r0.x != 0u) {
    r0.x = vd_asuint(saturate((vd_asfloat(_DissolveProgress) * 2.0 + -1.0)));
    r0.x = vd_asuint(((-(vd_asfloat(r0.x))) + 0.999000013));
    r0.x = vd_asuint((-(int)(vd_asfloat(r0.x) < 0.0)));
    if (r0.x != 0u) discard;
  }
  r0.xyzw = vd_asuint(_EmissionMap.SampleBias(sampler_EmissionMap, vd_asfloat(v1.xy), vd_asfloat(_GlobalMipBias.x)).xyzw);
  r0.x = vd_asuint(saturate(dot(vd_asfloat(r0.xx), vd_asfloat(r0.zz))));
  r0.x = vd_asuint((vd_asfloat(r0.x) + -0.5));
  r0.xy = vd_asuint((-(int2)(vd_asfloat(r0.xz) < float2(0.0, 0.5))));
  r0.x = ((r0.y != 0u) ? 0x00000000u : r0.x);
  if (r0.x != 0u) discard;
  r0.xy = vd_asuint((float2(1.0, 1.0) / vd_asfloat(_ScaledScreenParams.xy)));
#if UNITY_UV_STARTS_AT_TOP
  r0.xy = vd_asuint((vd_asfloat(r0.xy) * vd_asfloat(v0.xy)));
  r0.y = vd_asuint((vd_asfloat(r0.y) * vd_asfloat(_ScaleBiasRt.x) + vd_asfloat(_ScaleBiasRt.y)));
  r0.z = vd_asuint(((-(vd_asfloat(r0.y))) + 1.0));
#else
  r0.xy = vd_asuint((vd_asfloat(r0.xy) * vd_asfloat(r5.xy)));
  r0.y = r0.y;
  r0.z = r0.y;
#endif
  r0.y = vd_asuint((-(int)(vd_asfloat(_GlobalBoxAreaEnable) >= 0.5)));
  if (r0.y != 0u) {
    r0.yw = vd_asuint((vd_asfloat(v6.xz) * float2(0.5, 0.5)));
    r1.xy = vd_asuint(floor(vd_asfloat(r0.yw)));
    r0.yw = vd_asuint(frac(vd_asfloat(r0.yw)));
    r1.zw = vd_asuint((vd_asfloat(r0.yw) * vd_asfloat(r0.yw)));
    r0.yw = vd_asuint(((-(vd_asfloat(r0.yw))) * float2(2.0, 2.0) + float2(3.0, 3.0)));
    r0.yw = vd_asuint((vd_asfloat(r0.yw) * vd_asfloat(r1.zw)));
    r1.z = vd_asuint(dot(vd_asfloat(r1.xy), float2(127.099998, 311.700012)));
    r1.z = vd_asuint(sin(vd_asfloat(r1.z)));
    r1.z = vd_asuint((vd_asfloat(r1.z) * 43758.5469));
    r2.xyzw = vd_asuint((vd_asfloat(r1.xyxy) + float4(1.0, 0.0, 0.0, 1.0)));
    r1.w = vd_asuint(dot(vd_asfloat(r2.xy), float2(127.099998, 311.700012)));
    r1.w = vd_asuint(sin(vd_asfloat(r1.w)));
    r1.w = vd_asuint((vd_asfloat(r1.w) * 43758.5469));
    r2.x = vd_asuint(dot(vd_asfloat(r2.zw), float2(127.099998, 311.700012)));
    r2.x = vd_asuint(sin(vd_asfloat(r2.x)));
    r2.x = vd_asuint((vd_asfloat(r2.x) * 43758.5469));
    r2.x = vd_asuint(frac(vd_asfloat(r2.x)));
    r1.xy = vd_asuint((vd_asfloat(r1.xy) + float2(1.0, 1.0)));
    r1.x = vd_asuint(dot(vd_asfloat(r1.xy), float2(127.099998, 311.700012)));
    r1.x = vd_asuint(sin(vd_asfloat(r1.x)));
    r1.x = vd_asuint((vd_asfloat(r1.x) * 43758.5469));
    r1.xzw = vd_asuint(frac(vd_asfloat(r1.xzw)));
    r1.y = vd_asuint(((-(vd_asfloat(r1.z))) + vd_asfloat(r1.w)));
    r1.y = vd_asuint((vd_asfloat(r0.y) * vd_asfloat(r1.y) + vd_asfloat(r1.z)));
    r1.x = vd_asuint(((-(vd_asfloat(r2.x))) + vd_asfloat(r1.x)));
    r0.y = vd_asuint((vd_asfloat(r0.y) * vd_asfloat(r1.x) + vd_asfloat(r2.x)));
    r0.y = vd_asuint(((-(vd_asfloat(r1.y))) + vd_asfloat(r0.y)));
    r0.y = vd_asuint((vd_asfloat(r0.w) * vd_asfloat(r0.y) + vd_asfloat(r1.y)));
    r0.w = vd_asuint(((int)vd_asfloat(_GlobalBoxAreaCenters[0].w)));
    r0.w = vd_asuint((-(int)(vd_asint(r0.w) == 4)));
    if (r0.w != 0u) {
      r1.xyz = vd_asuint((vd_asfloat(v6.xyz) + (-(vd_asfloat(_GlobalBoxAreaCenters[0].xyz)))));
      r1.xyz = vd_asuint(((-(abs(vd_asfloat(r1.xyz)))) + vd_asfloat(_GlobalBoxAreaExtents[0].xyz)));
      r0.w = vd_asuint(min(vd_asfloat(r1.z), vd_asfloat(r1.y)));
      r0.w = vd_asuint(min(vd_asfloat(r0.w), vd_asfloat(r1.x)));
      r1.x = vd_asuint(max(vd_asfloat(_GlobalBoxAreaExtents[0].w), 0.00100000005));
      r0.w = vd_asuint(saturate((vd_asfloat(r0.w) / vd_asfloat(r1.x))));
      r1.x = vd_asuint((-(int)(0.0 < vd_asfloat(r0.w))));
      if (r1.x != 0u) {
        r1.x = vd_asuint(((-(vd_asfloat(_GlobalBoxAreaIntensities[0]))) + 1.125));
        r1.x = vd_asuint((vd_asfloat(r0.y) + (-(vd_asfloat(r1.x)))));
        r1.x = vd_asuint(saturate((vd_asfloat(r1.x) * -4.0)));
        r1.y = vd_asuint((vd_asfloat(r1.x) * -2.0 + 3.0));
        r1.x = vd_asuint((vd_asfloat(r1.x) * vd_asfloat(r1.x)));
        r1.x = vd_asuint((vd_asfloat(r1.y) * vd_asfloat(r1.x) + -0.5));
        r0.w = vd_asuint(((-(vd_asfloat(r0.w))) + vd_asfloat(r1.x)));
        r0.w = vd_asuint((vd_asfloat(r0.w) + 1.0));
        r0.w = vd_asuint((-(int)(vd_asfloat(r0.w) < 0.0)));
        if (r0.w != 0u) discard;
      }
    }
    r0.w = vd_asuint(((int)vd_asfloat(_GlobalBoxAreaCenters[1].w)));
    r0.w = vd_asuint((-(int)(vd_asint(r0.w) == 4)));
    if (r0.w != 0u) {
      r1.xyz = vd_asuint((vd_asfloat(v6.xyz) + (-(vd_asfloat(_GlobalBoxAreaCenters[1].xyz)))));
      r1.xyz = vd_asuint(((-(abs(vd_asfloat(r1.xyz)))) + vd_asfloat(_GlobalBoxAreaExtents[1].xyz)));
      r0.w = vd_asuint(min(vd_asfloat(r1.z), vd_asfloat(r1.y)));
      r0.w = vd_asuint(min(vd_asfloat(r0.w), vd_asfloat(r1.x)));
      r1.x = vd_asuint(max(vd_asfloat(_GlobalBoxAreaExtents[1].w), 0.00100000005));
      r0.w = vd_asuint(saturate((vd_asfloat(r0.w) / vd_asfloat(r1.x))));
      r1.x = vd_asuint((-(int)(0.0 < vd_asfloat(r0.w))));
      if (r1.x != 0u) {
        r1.x = vd_asuint(((-(vd_asfloat(_GlobalBoxAreaIntensities[1]))) + 1.125));
        r1.x = vd_asuint((vd_asfloat(r0.y) + (-(vd_asfloat(r1.x)))));
        r1.x = vd_asuint(saturate((vd_asfloat(r1.x) * -4.0)));
        r1.y = vd_asuint((vd_asfloat(r1.x) * -2.0 + 3.0));
        r1.x = vd_asuint((vd_asfloat(r1.x) * vd_asfloat(r1.x)));
        r1.x = vd_asuint((vd_asfloat(r1.y) * vd_asfloat(r1.x) + -0.5));
        r0.w = vd_asuint(((-(vd_asfloat(r0.w))) + vd_asfloat(r1.x)));
        r0.w = vd_asuint((vd_asfloat(r0.w) + 1.0));
        r0.w = vd_asuint((-(int)(vd_asfloat(r0.w) < 0.0)));
        if (r0.w != 0u) discard;
      }
    }
    r0.w = vd_asuint(((int)vd_asfloat(_GlobalBoxAreaCenters[2].w)));
    r0.w = vd_asuint((-(int)(vd_asint(r0.w) == 4)));
    if (r0.w != 0u) {
      r1.xyz = vd_asuint((vd_asfloat(v6.xyz) + (-(vd_asfloat(_GlobalBoxAreaCenters[2].xyz)))));
      r1.xyz = vd_asuint(((-(abs(vd_asfloat(r1.xyz)))) + vd_asfloat(_GlobalBoxAreaExtents[2].xyz)));
      r0.w = vd_asuint(min(vd_asfloat(r1.z), vd_asfloat(r1.y)));
      r0.w = vd_asuint(min(vd_asfloat(r0.w), vd_asfloat(r1.x)));
      r1.x = vd_asuint(max(vd_asfloat(_GlobalBoxAreaExtents[2].w), 0.00100000005));
      r0.w = vd_asuint(saturate((vd_asfloat(r0.w) / vd_asfloat(r1.x))));
      r1.x = vd_asuint((-(int)(0.0 < vd_asfloat(r0.w))));
      if (r1.x != 0u) {
        r1.x = vd_asuint(((-(vd_asfloat(_GlobalBoxAreaIntensities[2]))) + 1.125));
        r1.x = vd_asuint((vd_asfloat(r0.y) + (-(vd_asfloat(r1.x)))));
        r1.x = vd_asuint(saturate((vd_asfloat(r1.x) * -4.0)));
        r1.y = vd_asuint((vd_asfloat(r1.x) * -2.0 + 3.0));
        r1.x = vd_asuint((vd_asfloat(r1.x) * vd_asfloat(r1.x)));
        r1.x = vd_asuint((vd_asfloat(r1.y) * vd_asfloat(r1.x) + -0.5));
        r0.w = vd_asuint(((-(vd_asfloat(r0.w))) + vd_asfloat(r1.x)));
        r0.w = vd_asuint((vd_asfloat(r0.w) + 1.0));
        r0.w = vd_asuint((-(int)(vd_asfloat(r0.w) < 0.0)));
        if (r0.w != 0u) discard;
      }
    }
    r0.w = vd_asuint(((int)vd_asfloat(_GlobalBoxAreaCenters[3].w)));
    r0.w = vd_asuint((-(int)(vd_asint(r0.w) == 4)));
    if (r0.w != 0u) {
      r1.xyz = vd_asuint((vd_asfloat(v6.xyz) + (-(vd_asfloat(_GlobalBoxAreaCenters[3].xyz)))));
      r1.xyz = vd_asuint(((-(abs(vd_asfloat(r1.xyz)))) + vd_asfloat(_GlobalBoxAreaExtents[3].xyz)));
      r0.w = vd_asuint(min(vd_asfloat(r1.z), vd_asfloat(r1.y)));
      r0.w = vd_asuint(min(vd_asfloat(r0.w), vd_asfloat(r1.x)));
      r1.x = vd_asuint(max(vd_asfloat(_GlobalBoxAreaExtents[3].w), 0.00100000005));
      r0.w = vd_asuint(saturate((vd_asfloat(r0.w) / vd_asfloat(r1.x))));
      r1.x = vd_asuint((-(int)(0.0 < vd_asfloat(r0.w))));
      if (r1.x != 0u) {
        r1.x = vd_asuint(((-(vd_asfloat(_GlobalBoxAreaIntensities[3]))) + 1.125));
        r1.x = vd_asuint((vd_asfloat(r0.y) + (-(vd_asfloat(r1.x)))));
        r1.x = vd_asuint(saturate((vd_asfloat(r1.x) * -4.0)));
        r1.y = vd_asuint((vd_asfloat(r1.x) * -2.0 + 3.0));
        r1.x = vd_asuint((vd_asfloat(r1.x) * vd_asfloat(r1.x)));
        r1.x = vd_asuint((vd_asfloat(r1.y) * vd_asfloat(r1.x) + -0.5));
        r0.w = vd_asuint(((-(vd_asfloat(r0.w))) + vd_asfloat(r1.x)));
        r0.w = vd_asuint((vd_asfloat(r0.w) + 1.0));
        r0.w = vd_asuint((-(int)(vd_asfloat(r0.w) < 0.0)));
        if (r0.w != 0u) discard;
      }
    }
    r0.w = vd_asuint(((int)vd_asfloat(_GlobalBoxAreaCenters[4].w)));
    r0.w = vd_asuint((-(int)(vd_asint(r0.w) == 4)));
    if (r0.w != 0u) {
      r1.xyz = vd_asuint((vd_asfloat(v6.xyz) + (-(vd_asfloat(_GlobalBoxAreaCenters[4].xyz)))));
      r1.xyz = vd_asuint(((-(abs(vd_asfloat(r1.xyz)))) + vd_asfloat(_GlobalBoxAreaExtents[4].xyz)));
      r0.w = vd_asuint(min(vd_asfloat(r1.z), vd_asfloat(r1.y)));
      r0.w = vd_asuint(min(vd_asfloat(r0.w), vd_asfloat(r1.x)));
      r1.x = vd_asuint(max(vd_asfloat(_GlobalBoxAreaExtents[4].w), 0.00100000005));
      r0.w = vd_asuint(saturate((vd_asfloat(r0.w) / vd_asfloat(r1.x))));
      r1.x = vd_asuint((-(int)(0.0 < vd_asfloat(r0.w))));
      if (r1.x != 0u) {
        r1.x = vd_asuint(((-(vd_asfloat(_GlobalBoxAreaIntensities[4]))) + 1.125));
        r1.x = vd_asuint((vd_asfloat(r0.y) + (-(vd_asfloat(r1.x)))));
        r1.x = vd_asuint(saturate((vd_asfloat(r1.x) * -4.0)));
        r1.y = vd_asuint((vd_asfloat(r1.x) * -2.0 + 3.0));
        r1.x = vd_asuint((vd_asfloat(r1.x) * vd_asfloat(r1.x)));
        r1.x = vd_asuint((vd_asfloat(r1.y) * vd_asfloat(r1.x) + -0.5));
        r0.w = vd_asuint(((-(vd_asfloat(r0.w))) + vd_asfloat(r1.x)));
        r0.w = vd_asuint((vd_asfloat(r0.w) + 1.0));
        r0.w = vd_asuint((-(int)(vd_asfloat(r0.w) < 0.0)));
        if (r0.w != 0u) discard;
      }
    }
    r0.w = vd_asuint(((int)vd_asfloat(_GlobalBoxAreaCenters[5].w)));
    r0.w = vd_asuint((-(int)(vd_asint(r0.w) == 4)));
    if (r0.w != 0u) {
      r1.xyz = vd_asuint((vd_asfloat(v6.xyz) + (-(vd_asfloat(_GlobalBoxAreaCenters[5].xyz)))));
      r1.xyz = vd_asuint(((-(abs(vd_asfloat(r1.xyz)))) + vd_asfloat(_GlobalBoxAreaExtents[5].xyz)));
      r0.w = vd_asuint(min(vd_asfloat(r1.z), vd_asfloat(r1.y)));
      r0.w = vd_asuint(min(vd_asfloat(r0.w), vd_asfloat(r1.x)));
      r1.x = vd_asuint(max(vd_asfloat(_GlobalBoxAreaExtents[5].w), 0.00100000005));
      r0.w = vd_asuint(saturate((vd_asfloat(r0.w) / vd_asfloat(r1.x))));
      r1.x = vd_asuint((-(int)(0.0 < vd_asfloat(r0.w))));
      if (r1.x != 0u) {
        r1.x = vd_asuint(((-(vd_asfloat(_GlobalBoxAreaIntensities[5]))) + 1.125));
        r1.x = vd_asuint((vd_asfloat(r0.y) + (-(vd_asfloat(r1.x)))));
        r1.x = vd_asuint(saturate((vd_asfloat(r1.x) * -4.0)));
        r1.y = vd_asuint((vd_asfloat(r1.x) * -2.0 + 3.0));
        r1.x = vd_asuint((vd_asfloat(r1.x) * vd_asfloat(r1.x)));
        r1.x = vd_asuint((vd_asfloat(r1.y) * vd_asfloat(r1.x) + -0.5));
        r0.w = vd_asuint(((-(vd_asfloat(r0.w))) + vd_asfloat(r1.x)));
        r0.w = vd_asuint((vd_asfloat(r0.w) + 1.0));
        r0.w = vd_asuint((-(int)(vd_asfloat(r0.w) < 0.0)));
        if (r0.w != 0u) discard;
      }
    }
    r0.w = vd_asuint(((int)vd_asfloat(_GlobalBoxAreaCenters[6].w)));
    r0.w = vd_asuint((-(int)(vd_asint(r0.w) == 4)));
    if (r0.w != 0u) {
      r1.xyz = vd_asuint((vd_asfloat(v6.xyz) + (-(vd_asfloat(_GlobalBoxAreaCenters[6].xyz)))));
      r1.xyz = vd_asuint(((-(abs(vd_asfloat(r1.xyz)))) + vd_asfloat(_GlobalBoxAreaExtents[6].xyz)));
      r0.w = vd_asuint(min(vd_asfloat(r1.z), vd_asfloat(r1.y)));
      r0.w = vd_asuint(min(vd_asfloat(r0.w), vd_asfloat(r1.x)));
      r1.x = vd_asuint(max(vd_asfloat(_GlobalBoxAreaExtents[6].w), 0.00100000005));
      r0.w = vd_asuint(saturate((vd_asfloat(r0.w) / vd_asfloat(r1.x))));
      r1.x = vd_asuint((-(int)(0.0 < vd_asfloat(r0.w))));
      if (r1.x != 0u) {
        r1.x = vd_asuint(((-(vd_asfloat(_GlobalBoxAreaIntensities[6]))) + 1.125));
        r1.x = vd_asuint((vd_asfloat(r0.y) + (-(vd_asfloat(r1.x)))));
        r1.x = vd_asuint(saturate((vd_asfloat(r1.x) * -4.0)));
        r1.y = vd_asuint((vd_asfloat(r1.x) * -2.0 + 3.0));
        r1.x = vd_asuint((vd_asfloat(r1.x) * vd_asfloat(r1.x)));
        r1.x = vd_asuint((vd_asfloat(r1.y) * vd_asfloat(r1.x) + -0.5));
        r0.w = vd_asuint(((-(vd_asfloat(r0.w))) + vd_asfloat(r1.x)));
        r0.w = vd_asuint((vd_asfloat(r0.w) + 1.0));
        r0.w = vd_asuint((-(int)(vd_asfloat(r0.w) < 0.0)));
        if (r0.w != 0u) discard;
      }
    }
    r0.w = vd_asuint(((int)vd_asfloat(_GlobalBoxAreaCenters[7].w)));
    r0.w = vd_asuint((-(int)(vd_asint(r0.w) == 4)));
    r1.xyz = vd_asuint((vd_asfloat(v6.xyz) + (-(vd_asfloat(_GlobalBoxAreaCenters[7].xyz)))));
    r1.xyz = vd_asuint(((-(abs(vd_asfloat(r1.xyz)))) + vd_asfloat(_GlobalBoxAreaExtents[7].xyz)));
    r1.y = vd_asuint(min(vd_asfloat(r1.z), vd_asfloat(r1.y)));
    r1.x = vd_asuint(min(vd_asfloat(r1.y), vd_asfloat(r1.x)));
    r1.y = vd_asuint(max(vd_asfloat(_GlobalBoxAreaExtents[7].w), 0.00100000005));
    r1.x = vd_asuint(saturate((vd_asfloat(r1.x) / vd_asfloat(r1.y))));
    r1.y = vd_asuint((-(int)(0.0 < vd_asfloat(r1.x))));
    r0.w = (r0.w & r1.y);
    r1.y = vd_asuint(((-(vd_asfloat(_GlobalBoxAreaIntensities[7]))) + 1.125));
    r0.y = vd_asuint((vd_asfloat(r0.y) + (-(vd_asfloat(r1.y)))));
    r0.y = vd_asuint(saturate((vd_asfloat(r0.y) * -4.0)));
    r1.y = vd_asuint((vd_asfloat(r0.y) * -2.0 + 3.0));
    r0.y = vd_asuint((vd_asfloat(r0.y) * vd_asfloat(r0.y)));
    r0.y = vd_asuint((vd_asfloat(r1.y) * vd_asfloat(r0.y) + -0.5));
    r0.y = vd_asuint(((-(vd_asfloat(r1.x))) + vd_asfloat(r0.y)));
    r0.y = vd_asuint((vd_asfloat(r0.y) + 1.0));
    r0.y = vd_asuint((-(int)(vd_asfloat(r0.y) < 0.0)));
    r0.y = (r0.w & r0.y);
    if (r0.y != 0u) discard;
  }
  r0.yw = vd_asuint((-(int2)(float2(1.5, 1.5) < vd_asfloat(v8.zw))));
  if (r0.y != 0u) {
    r1.xyzw = vd_asuint(_CustomDepthBuffer.SampleBias(sampler_CustomDepthBuffer, vd_asfloat(r0.xz), vd_asfloat(_GlobalMipBias.x)).xyzw);
#if UNITY_UV_STARTS_AT_TOP
    r0.y = vd_asuint(((-(vd_asfloat(r1.z))) + vd_asfloat(v0.z)));
#else
    r0.y = vd_asuint(((-(vd_asfloat(r1.z))) + vd_asfloat(r5.z)));
#endif
    r0.y = vd_asuint((vd_asfloat(r0.y) + -2.49999994e-05));
    r0.y = vd_asuint(saturate((vd_asfloat(r0.y) * -20000.0)));
    r1.x = vd_asuint((vd_asfloat(r0.y) * -2.0 + 3.0));
    r0.y = vd_asuint((vd_asfloat(r0.y) * vd_asfloat(r0.y)));
    r1.y = vd_asuint((vd_asfloat(r0.y) * vd_asfloat(r1.x)));
    r2.xy = vd_asuint(((-(vd_asfloat(float2(_GlobalSeeThroughCircleRange, _GlobalSeeThroughCircleAlpha)))) + float2(1.0, 1.0)));
    r1.z = vd_asuint(((-(vd_asfloat(r2.x))) + 1.0));
    r1.w = vd_asuint((vd_asfloat(r1.w) + (-(vd_asfloat(r2.x)))));
    r1.z = vd_asuint((1.0 / vd_asfloat(r1.z)));
    r1.z = vd_asuint(saturate((vd_asfloat(r1.z) * vd_asfloat(r1.w))));
    r1.w = vd_asuint((vd_asfloat(r1.z) * -2.0 + 3.0));
    r1.z = vd_asuint((vd_asfloat(r1.z) * vd_asfloat(r1.z)));
    r1.z = vd_asuint((vd_asfloat(r1.z) * vd_asfloat(r1.w)));
    r0.y = vd_asuint(((-(vd_asfloat(r1.x))) * vd_asfloat(r0.y) + 1.0));
    r0.y = vd_asuint((vd_asfloat(r1.z) * vd_asfloat(r0.y) + vd_asfloat(r1.y)));
    r1.x = vd_asuint((vd_asfloat(_GlobalBackgroundSeeThroughEnable) * vd_asfloat(_GlobalPlaneSystemEnable)));
    r0.y = vd_asuint((vd_asfloat(r0.y) + -1.0));
    r0.y = vd_asuint((vd_asfloat(r1.x) * vd_asfloat(r0.y) + 1.0));
    r0.y = vd_asuint((vd_asfloat(r0.y) * vd_asfloat(r2.y) + vd_asfloat(_GlobalSeeThroughCircleAlpha)));
    r1.yz = vd_asuint((vd_asfloat(_ScreenSize.xy) / vd_asfloat(_ScreenParams.xy)));
    r2.xy = vd_asuint((vd_asfloat(r0.xz) * vd_asfloat(_ScreenParams.xy)));
    r1.yz = vd_asuint((vd_asfloat(r1.yz) * vd_asfloat(r2.xy)));
    r1.yz = vd_asuint(floor(vd_asfloat(r1.yz)));
    r1.yz = ((uint2)vd_asfloat(r1.yz));
    r1.yz = (r1.yz & uint2(0x00000003u, 0x00000003u));
    r1.z = (r1.z << (uint)2);
    r1.y = vd_asuint((vd_asint(r1.z) + vd_asint(r1.y)));
    r1.y = x0[r1.y + 0].x;
    r0.y = vd_asuint((vd_asfloat(r0.y) + (-(vd_asfloat(r1.y)))));
    r0.y = vd_asuint((-(int)(vd_asfloat(r0.y) < 0.0)));
    if (r0.y != 0u) discard;
    r0.y = vd_asuint((-(int)(0.125 < vd_asfloat(v6.y))));
    if (r0.y != 0u) {
      r2.xyzw = vd_asuint(_CharacterAreaMaskTexture.SampleLevel(sampler_CharacterAreaMaskTexture, vd_asfloat(r0.xz), 0.0).xyzw);
      r1.yz = vd_asuint(round(vd_asfloat(_WorldSpaceCameraPos.xz)));
      r1.yz = vd_asuint(((-(vd_asfloat(r1.yz))) + vd_asfloat(v6.xz)));
      r3.x = vd_asuint((-(vd_asfloat((VD_COL(unity_MatrixV, 0).z)))));
      r3.y = vd_asuint((-(vd_asfloat((VD_COL(unity_MatrixV, 2).z)))));
      r0.y = vd_asuint(dot(vd_asfloat(r3.xy), vd_asfloat(r3.xy)));
      r0.y = vd_asuint(rsqrt(vd_asfloat(r0.y)));
      r2.zw = vd_asuint((vd_asfloat(r0.yy) * vd_asfloat(r3.xy)));
      r0.y = vd_asuint(dot(vd_asfloat(r1.yz), vd_asfloat(r2.zw)));
      r0.y = vd_asuint(saturate((vd_asfloat(r0.y) * 0.00999999978)));
      r0.y = vd_asuint(((-(vd_asfloat(r0.y))) + vd_asfloat(r2.y)));
      r0.y = vd_asuint(saturate((vd_asfloat(r0.y) * 40.0)));
      r1.y = vd_asuint((vd_asfloat(r0.y) * -2.0 + 3.0));
      r0.y = vd_asuint((vd_asfloat(r0.y) * vd_asfloat(r0.y)));
      r0.y = vd_asuint((vd_asfloat(r0.y) * vd_asfloat(r1.y)));
      r1.y = vd_asuint((vd_asfloat(r2.x) + -1.0));
      r0.y = vd_asuint((vd_asfloat(r0.y) * vd_asfloat(r1.y)));
      r0.y = vd_asuint((vd_asfloat(_GlobalBackgroundSeeThroughEnable) * vd_asfloat(r0.y) + 1.0));
      r1.yzw = vd_asuint((vd_asfloat(v6.xyz) + (-(vd_asfloat(_WorldSpaceCameraPos.xyz)))));
      r2.x = vd_asuint(((-(vd_asfloat(_WorldSpaceCameraPos.y))) + -0.150000006));
      r1.z = vd_asuint((vd_asfloat(r2.x) / vd_asfloat(r1.z)));
      r1.yz = vd_asuint((vd_asfloat(r1.yw) * vd_asfloat(r1.zz) + vd_asfloat(_WorldSpaceCameraPos.xz)));
      r1.yz = vd_asuint((vd_asfloat(r1.yz) + (-(vd_asfloat(_PlaneBoundMaxPos.xz)))));
      r2.xy = vd_asuint((vd_asfloat(_PlaneBoundMinPos.xz) + (-(vd_asfloat(_PlaneBoundMaxPos.xz)))));
      r1.yz = vd_asuint(saturate((vd_asfloat(r1.yz) / vd_asfloat(r2.xy))));
      r2.xyzw = vd_asuint(_PlaneSightRenderTarget.SampleLevel(sampler_PlaneSightRenderTarget, vd_asfloat(r1.yz), 0.0).xyzw);
      r1.y = vd_asuint((vd_asfloat(r2.w) + -1.0));
      r1.y = vd_asuint((vd_asfloat(_GlobalPlaneSystemEnable) * vd_asfloat(r1.y) + 1.0));
      r1.z = vd_asuint(((-(vd_asfloat(_SeeThroughDistanceMin))) + vd_asfloat(_SeeThroughDistanceMax)));
      r1.z = vd_asuint((1.0 / vd_asfloat(r1.z)));
      r1.w = 0x3f800000u;
      r2.x = 0x00000000u;
      while (true) {
        r2.y = vd_asuint((-(int)(vd_asint(r2.x) >= 3)));
        if (r2.y != 0u) break;
        r2.yzw = vd_asuint((vd_asfloat(v6.xyz) + (-(vd_asfloat(_SightPositions[(int)(r2.x)].xyz)))));
        r2.y = vd_asuint(dot(vd_asfloat(r2.yzw), vd_asfloat(r2.yzw)));
        r2.y = vd_asuint(sqrt(vd_asfloat(r2.y)));
        r2.y = vd_asuint((vd_asfloat(r2.y) + (-(vd_asfloat(_SeeThroughDistanceMin)))));
        r2.y = vd_asuint(saturate((vd_asfloat(r1.z) * vd_asfloat(r2.y))));
        r2.z = vd_asuint((vd_asfloat(r2.y) * -2.0 + 3.0));
        r2.y = vd_asuint((vd_asfloat(r2.y) * vd_asfloat(r2.y)));
        r2.y = vd_asuint((vd_asfloat(r2.y) * vd_asfloat(r2.z)));
        r1.w = vd_asuint(min(vd_asfloat(r1.w), vd_asfloat(r2.y)));
        r2.x = vd_asuint((vd_asint(r2.x) + 1));
      }
      r1.y = vd_asuint((vd_asfloat(r1.y) * vd_asfloat(r1.y)));
      r1.z = vd_asuint(((-(vd_asfloat(r1.w))) + 1.0));
      r1.y = vd_asuint(((-(vd_asfloat(r1.y))) * vd_asfloat(r1.z) + 1.0));
      r1.y = vd_asuint(max(vd_asfloat(r1.y), 0.0));
      r1.y = vd_asuint((vd_asfloat(r1.y) + -1.0));
      r1.x = vd_asuint((vd_asfloat(r1.x) * vd_asfloat(r1.y) + 1.0));
      r0.y = vd_asuint(min(vd_asfloat(r0.y), vd_asfloat(r1.x)));
    } else {
      r0.y = vd_asuint(v1.z);
    }
    r1.x = vd_asuint(((-(vd_asfloat(_GlobalSeeThroughHeightMin))) + vd_asfloat(_GlobalSeeThroughHeightMax)));
    r1.y = vd_asuint((vd_asfloat(v6.y) + (-(vd_asfloat(_GlobalSeeThroughHeightMin)))));
    r1.x = vd_asuint((1.0 / vd_asfloat(r1.x)));
    r1.x = vd_asuint(saturate((vd_asfloat(r1.x) * vd_asfloat(r1.y))));
    r1.y = vd_asuint((vd_asfloat(r1.x) * -2.0 + 3.0));
    r1.x = vd_asuint((vd_asfloat(r1.x) * vd_asfloat(r1.x)));
    r1.x = vd_asuint(((-(vd_asfloat(r1.y))) * vd_asfloat(r1.x) + 1.0));
    r0.y = vd_asuint(max(vd_asfloat(r0.y), vd_asfloat(r1.x)));
    r0.y = vd_asuint(max(vd_asfloat(r0.y), 0.00100000005));
  } else {
    r0.y = vd_asuint(v1.z);
  }
  r1.xyz = vd_asuint((-(int3)(vd_asint((_BackgroundType).xxx) == int3(2, 3, 7))));
  r0.w = (r0.w & r1.x);
  if (r0.w != 0u) {
    r2.xyzw = vd_asuint(_CustomDepthBuffer.SampleBias(sampler_CustomDepthBuffer, vd_asfloat(r0.xz), vd_asfloat(_GlobalMipBias.x)).xyzw);
#if UNITY_UV_STARTS_AT_TOP
    r0.w = vd_asuint(((-(vd_asfloat(r2.z))) + vd_asfloat(v0.z)));
#else
    r0.w = vd_asuint(((-(vd_asfloat(r2.z))) + vd_asfloat(r5.z)));
#endif
    r0.w = vd_asuint((vd_asfloat(r0.w) + -2.49999994e-05));
    r0.w = vd_asuint(saturate((vd_asfloat(r0.w) * -20000.0)));
    r1.x = vd_asuint((vd_asfloat(r0.w) * -2.0 + 3.0));
    r0.w = vd_asuint((vd_asfloat(r0.w) * vd_asfloat(r0.w)));
    r1.w = vd_asuint((vd_asfloat(r0.w) * vd_asfloat(r1.x)));
    r2.xy = vd_asuint(((-(vd_asfloat(float2(_GlobalSeeThroughCircleRange, _GlobalSeeThroughCircleAlpha)))) + float2(1.0, 1.0)));
    r2.z = vd_asuint(((-(vd_asfloat(r2.x))) + 1.0));
    r2.x = vd_asuint(((-(vd_asfloat(r2.x))) + vd_asfloat(r2.w)));
    r2.z = vd_asuint((1.0 / vd_asfloat(r2.z)));
    r2.x = vd_asuint(saturate((vd_asfloat(r2.z) * vd_asfloat(r2.x))));
    r2.z = vd_asuint((vd_asfloat(r2.x) * -2.0 + 3.0));
    r2.x = vd_asuint((vd_asfloat(r2.x) * vd_asfloat(r2.x)));
    r2.x = vd_asuint((vd_asfloat(r2.x) * vd_asfloat(r2.z)));
    r0.w = vd_asuint(((-(vd_asfloat(r1.x))) * vd_asfloat(r0.w) + 1.0));
    r0.w = vd_asuint((vd_asfloat(r2.x) * vd_asfloat(r0.w) + vd_asfloat(r1.w)));
    r1.x = vd_asuint((vd_asfloat(_GlobalBackgroundSeeThroughEnable) * vd_asfloat(_GlobalPlaneSystemEnable)));
    r0.w = vd_asuint((vd_asfloat(r0.w) + -1.0));
    r0.w = vd_asuint((vd_asfloat(r1.x) * vd_asfloat(r0.w) + 1.0));
    r0.w = vd_asuint((vd_asfloat(r0.w) * vd_asfloat(r2.y) + vd_asfloat(_GlobalSeeThroughCircleAlpha)));
    r2.xy = vd_asuint((vd_asfloat(_ScreenSize.xy) / vd_asfloat(_ScreenParams.xy)));
    r2.zw = vd_asuint((vd_asfloat(r0.xz) * vd_asfloat(_ScreenParams.xy)));
    r2.xy = vd_asuint((vd_asfloat(r2.xy) * vd_asfloat(r2.zw)));
    r2.xy = vd_asuint(floor(vd_asfloat(r2.xy)));
    r2.xy = ((uint2)vd_asfloat(r2.xy));
    r2.xy = (r2.xy & uint2(0x00000003u, 0x00000003u));
    r1.w = (r2.y << (uint)2);
    r1.w = vd_asuint((vd_asint(r1.w) + vd_asint(r2.x)));
    r1.w = x0[r1.w + 0].x;
    r0.w = vd_asuint((vd_asfloat(r0.w) + (-(vd_asfloat(r1.w)))));
    r0.w = vd_asuint((-(int)(vd_asfloat(r0.w) < 0.0)));
    if (r0.w != 0u) discard;
    r0.w = vd_asuint((-(int)(0.125 < vd_asfloat(v6.y))));
    if (r0.w != 0u) {
      r2.xyzw = vd_asuint(_CharacterAreaMaskTexture.SampleLevel(sampler_CharacterAreaMaskTexture, vd_asfloat(r0.xz), 0.0).xyzw);
      r2.zw = vd_asuint(round(vd_asfloat(_WorldSpaceCameraPos.xz)));
      r2.zw = vd_asuint(((-(vd_asfloat(r2.zw))) + vd_asfloat(v6.xz)));
      r3.x = vd_asuint((-(vd_asfloat((VD_COL(unity_MatrixV, 0).z)))));
      r3.y = vd_asuint((-(vd_asfloat((VD_COL(unity_MatrixV, 2).z)))));
      r0.w = vd_asuint(dot(vd_asfloat(r3.xy), vd_asfloat(r3.xy)));
      r0.w = vd_asuint(rsqrt(vd_asfloat(r0.w)));
      r3.xy = vd_asuint((vd_asfloat(r0.ww) * vd_asfloat(r3.xy)));
      r0.w = vd_asuint(dot(vd_asfloat(r2.zw), vd_asfloat(r3.xy)));
      r0.w = vd_asuint(saturate((vd_asfloat(r0.w) * 0.00999999978)));
      r0.w = vd_asuint(((-(vd_asfloat(r0.w))) + vd_asfloat(r2.y)));
      r0.w = vd_asuint(saturate((vd_asfloat(r0.w) * 40.0)));
      r1.w = vd_asuint((vd_asfloat(r0.w) * -2.0 + 3.0));
      r0.w = vd_asuint((vd_asfloat(r0.w) * vd_asfloat(r0.w)));
      r0.w = vd_asuint((vd_asfloat(r0.w) * vd_asfloat(r1.w)));
      r1.w = vd_asuint((vd_asfloat(r2.x) + -1.0));
      r0.w = vd_asuint((vd_asfloat(r0.w) * vd_asfloat(r1.w)));
      r0.w = vd_asuint((vd_asfloat(_GlobalBackgroundSeeThroughEnable) * vd_asfloat(r0.w) + 1.0));
      r2.xyz = vd_asuint((vd_asfloat(v6.xyz) + (-(vd_asfloat(_WorldSpaceCameraPos.xyz)))));
      r1.w = vd_asuint(((-(vd_asfloat(_WorldSpaceCameraPos.y))) + -0.150000006));
      r1.w = vd_asuint((vd_asfloat(r1.w) / vd_asfloat(r2.y)));
      r2.xy = vd_asuint((vd_asfloat(r2.xz) * vd_asfloat(r1.ww) + vd_asfloat(_WorldSpaceCameraPos.xz)));
      r2.xy = vd_asuint((vd_asfloat(r2.xy) + (-(vd_asfloat(_PlaneBoundMaxPos.xz)))));
      r2.zw = vd_asuint((vd_asfloat(_PlaneBoundMinPos.xz) + (-(vd_asfloat(_PlaneBoundMaxPos.xz)))));
      r2.xy = vd_asuint(saturate((vd_asfloat(r2.xy) / vd_asfloat(r2.zw))));
      r2.xyzw = vd_asuint(_PlaneSightRenderTarget.SampleLevel(sampler_PlaneSightRenderTarget, vd_asfloat(r2.xy), 0.0).xyzw);
      r1.w = vd_asuint((vd_asfloat(r2.w) + -1.0));
      r1.w = vd_asuint((vd_asfloat(_GlobalPlaneSystemEnable) * vd_asfloat(r1.w) + 1.0));
      r2.x = vd_asuint(((-(vd_asfloat(_SeeThroughDistanceMin))) + vd_asfloat(_SeeThroughDistanceMax)));
      r2.x = vd_asuint((1.0 / vd_asfloat(r2.x)));
      r2.yz = uint2(0x3f800000u, 0x00000000u);
      while (true) {
        r2.w = vd_asuint((-(int)(vd_asint(r2.z) >= 3)));
        if (r2.w != 0u) break;
        r3.xyz = vd_asuint((vd_asfloat(v6.xyz) + (-(vd_asfloat(_SightPositions[(int)(r2.z)].xyz)))));
        r2.w = vd_asuint(dot(vd_asfloat(r3.xyz), vd_asfloat(r3.xyz)));
        r2.w = vd_asuint(sqrt(vd_asfloat(r2.w)));
        r2.w = vd_asuint((vd_asfloat(r2.w) + (-(vd_asfloat(_SeeThroughDistanceMin)))));
        r2.w = vd_asuint(saturate((vd_asfloat(r2.x) * vd_asfloat(r2.w))));
        r3.x = vd_asuint((vd_asfloat(r2.w) * -2.0 + 3.0));
        r2.w = vd_asuint((vd_asfloat(r2.w) * vd_asfloat(r2.w)));
        r2.w = vd_asuint((vd_asfloat(r2.w) * vd_asfloat(r3.x)));
        r2.y = vd_asuint(min(vd_asfloat(r2.w), vd_asfloat(r2.y)));
        r2.z = vd_asuint((vd_asint(r2.z) + 1));
      }
      r1.w = vd_asuint((vd_asfloat(r1.w) * vd_asfloat(r1.w)));
      r2.x = vd_asuint(((-(vd_asfloat(r2.y))) + 1.0));
      r1.w = vd_asuint(((-(vd_asfloat(r1.w))) * vd_asfloat(r2.x) + 1.0));
      r1.w = vd_asuint(max(vd_asfloat(r1.w), 0.0));
      r1.w = vd_asuint((vd_asfloat(r1.w) + -1.0));
      r1.x = vd_asuint((vd_asfloat(r1.x) * vd_asfloat(r1.w) + 1.0));
      r0.y = vd_asuint(min(vd_asfloat(r0.w), vd_asfloat(r1.x)));
    }
    r0.w = vd_asuint(((-(vd_asfloat(_GlobalSeeThroughHeightMin))) + vd_asfloat(_GlobalSeeThroughHeightMax)));
    r1.x = vd_asuint((vd_asfloat(v6.y) + (-(vd_asfloat(_GlobalSeeThroughHeightMin)))));
    r0.w = vd_asuint((1.0 / vd_asfloat(r0.w)));
    r0.w = vd_asuint(saturate((vd_asfloat(r0.w) * vd_asfloat(r1.x))));
    r1.x = vd_asuint((vd_asfloat(r0.w) * -2.0 + 3.0));
    r0.w = vd_asuint((vd_asfloat(r0.w) * vd_asfloat(r0.w)));
    r0.w = vd_asuint(((-(vd_asfloat(r1.x))) * vd_asfloat(r0.w) + 1.0));
    r0.w = vd_asuint(max(vd_asfloat(r0.y), vd_asfloat(r0.w)));
    r0.y = vd_asuint(max(vd_asfloat(r0.w), 0.00100000005));
  }
  r0.w = (r1.z | r1.y);
  if (r0.w != 0u) {
    r0.w = vd_asuint((vd_asfloat(_GlobalSeeThroughBuildingHeightMax) + (-(vd_asfloat(_GlobalSeeThroughBuildingHeightMin)))));
    r1.x = vd_asuint((vd_asfloat(v6.y) + (-(vd_asfloat(_GlobalSeeThroughBuildingHeightMin)))));
    r0.w = vd_asuint((1.0 / vd_asfloat(r0.w)));
    r0.w = vd_asuint(saturate((vd_asfloat(r0.w) * vd_asfloat(r1.x))));
    r1.x = vd_asuint((vd_asfloat(r0.w) * -2.0 + 3.0));
    r0.w = vd_asuint((vd_asfloat(r0.w) * vd_asfloat(r0.w)));
    r0.w = vd_asuint(((-(vd_asfloat(r1.x))) * vd_asfloat(r0.w) + 1.0));
    r1.x = vd_asuint(((-(vd_asfloat(_GlobalSeeThroughBuildingAlpha))) + 1.0));
    r0.w = vd_asuint((vd_asfloat(r0.w) * vd_asfloat(r1.x) + vd_asfloat(_GlobalSeeThroughBuildingAlpha)));
    r1.x = vd_asuint(((-(vd_asfloat(r0.w))) + 1.0));
    r0.w = vd_asuint((vd_asfloat(r0.y) * vd_asfloat(r1.x) + vd_asfloat(r0.w)));
    r0.w = vd_asuint((vd_asfloat(r0.w) + -1.0));
    r0.y = vd_asuint((vd_asfloat(_GlobalIsRuntime) * vd_asfloat(r0.w) + 1.0));
    r1.xy = vd_asuint((vd_asfloat(_ScreenSize.xy) / vd_asfloat(_ScreenParams.xy)));
    r1.zw = vd_asuint((vd_asfloat(r0.xz) * vd_asfloat(_ScreenParams.xy)));
    r1.xy = vd_asuint((vd_asfloat(r1.xy) * vd_asfloat(r1.zw)));
    r1.xy = vd_asuint(floor(vd_asfloat(r1.xy)));
    r1.xy = ((uint2)vd_asfloat(r1.xy));
    r1.xy = (r1.xy & uint2(0x00000003u, 0x00000003u));
    r0.w = (r1.y << (uint)2);
    r0.w = vd_asuint((vd_asint(r0.w) + vd_asint(r1.x)));
    r0.w = x0[r0.w + 0].x;
    r0.w = vd_asuint(((-(vd_asfloat(r0.w))) + vd_asfloat(r0.y)));
    r0.w = vd_asuint((-(int)(vd_asfloat(r0.w) < 0.0)));
    if (r0.w != 0u) discard;
  } else {
    r0.w = vd_asuint((-(int)(vd_asint(_BackgroundType) == 4)));
    if (r0.w != 0u) {
      r0.w = vd_asuint((-(int)(0.125 < vd_asfloat(v6.y))));
      if (r0.w != 0u) {
        r1.xyzw = vd_asuint(_CharacterAreaMaskTexture.SampleLevel(sampler_CharacterAreaMaskTexture, vd_asfloat(r0.xz), 0.0).xyzw);
        r1.zw = vd_asuint(round(vd_asfloat(_WorldSpaceCameraPos.xz)));
        r1.zw = vd_asuint(((-(vd_asfloat(r1.zw))) + vd_asfloat(v6.xz)));
        r2.x = vd_asuint((-(vd_asfloat((VD_COL(unity_MatrixV, 0).z)))));
        r2.y = vd_asuint((-(vd_asfloat((VD_COL(unity_MatrixV, 2).z)))));
        r0.w = vd_asuint(dot(vd_asfloat(r2.xy), vd_asfloat(r2.xy)));
        r0.w = vd_asuint(rsqrt(vd_asfloat(r0.w)));
        r2.xy = vd_asuint((vd_asfloat(r0.ww) * vd_asfloat(r2.xy)));
        r0.w = vd_asuint(dot(vd_asfloat(r1.zw), vd_asfloat(r2.xy)));
        r0.w = vd_asuint(saturate((vd_asfloat(r0.w) * 0.00999999978)));
        r0.w = vd_asuint(((-(vd_asfloat(r0.w))) + vd_asfloat(r1.y)));
        r0.w = vd_asuint(saturate((vd_asfloat(r0.w) * 40.0)));
        r1.y = vd_asuint((vd_asfloat(r0.w) * -2.0 + 3.0));
        r0.w = vd_asuint((vd_asfloat(r0.w) * vd_asfloat(r0.w)));
        r0.w = vd_asuint((vd_asfloat(r0.w) * vd_asfloat(r1.y)));
        r1.x = vd_asuint((vd_asfloat(r1.x) + -1.0));
        r0.w = vd_asuint((vd_asfloat(r0.w) * vd_asfloat(r1.x)));
        r0.w = vd_asuint((vd_asfloat(_GlobalBackgroundSeeThroughEnable) * vd_asfloat(r0.w) + 1.0));
        r1.xyz = vd_asuint((vd_asfloat(v6.xyz) + (-(vd_asfloat(_WorldSpaceCameraPos.xyz)))));
        r2.xyzw = vd_asuint(((-(vd_asfloat(_WorldSpaceCameraPos.yyyy))) + float4(2.0, 2.0, 1.0, 1.0)));
        r2.xyzw = vd_asuint((vd_asfloat(r2.xyzw) / vd_asfloat(r1.yyyy)));
        r2.xyzw = vd_asuint((vd_asfloat(r1.xzxz) * vd_asfloat(r2.xyzw) + vd_asfloat(_WorldSpaceCameraPos.xzxz)));
        r2.xyzw = vd_asuint((vd_asfloat(r2.xyzw) + (-(vd_asfloat(_PlaneBoundMaxPos.xzxz)))));
        r3.xy = vd_asuint((vd_asfloat(_PlaneBoundMinPos.xz) + (-(vd_asfloat(_PlaneBoundMaxPos.xz)))));
        r2.xyzw = vd_asuint(saturate((vd_asfloat(r2.xyzw) / vd_asfloat(r3.xyxy))));
        r4.xyzw = vd_asuint(_PlaneSightRenderTarget.SampleLevel(sampler_PlaneSightRenderTarget, vd_asfloat(r2.xy), 0.0).xyzw);
        r1.w = vd_asuint((vd_asfloat(r4.w) + -1.0));
        r1.w = vd_asuint((vd_asfloat(_GlobalPlaneSystemEnable) * vd_asfloat(r1.w) + 1.0));
        r2.x = vd_asuint(((-(vd_asfloat(_SeeThroughDistanceMin))) + vd_asfloat(_SeeThroughDistanceMax)));
        r2.x = vd_asuint((1.0 / vd_asfloat(r2.x)));
        r2.y = 0x3f800000u;
        r3.z = 0x00000000u;
        while (true) {
          r3.w = vd_asuint((-(int)(vd_asint(r3.z) >= 3)));
          if (r3.w != 0u) break;
          r4.xyz = vd_asuint((vd_asfloat(v6.xyz) + (-(vd_asfloat(_SightPositions[(int)(r3.z)].xyz)))));
          r3.w = vd_asuint(dot(vd_asfloat(r4.xyz), vd_asfloat(r4.xyz)));
          r3.w = vd_asuint(sqrt(vd_asfloat(r3.w)));
          r3.w = vd_asuint((vd_asfloat(r3.w) + (-(vd_asfloat(_SeeThroughDistanceMin)))));
          r3.w = vd_asuint(saturate((vd_asfloat(r2.x) * vd_asfloat(r3.w))));
          r4.x = vd_asuint((vd_asfloat(r3.w) * -2.0 + 3.0));
          r3.w = vd_asuint((vd_asfloat(r3.w) * vd_asfloat(r3.w)));
          r3.w = vd_asuint((vd_asfloat(r3.w) * vd_asfloat(r4.x)));
          r2.y = vd_asuint(min(vd_asfloat(r2.y), vd_asfloat(r3.w)));
          r3.z = vd_asuint((vd_asint(r3.z) + 1));
        }
        r3.z = vd_asuint((vd_asfloat(_GlobalBackgroundSeeThroughEnable) * vd_asfloat(_GlobalPlaneSystemEnable)));
        r1.w = vd_asuint((vd_asfloat(r1.w) * vd_asfloat(r1.w)));
        r2.y = vd_asuint(((-(vd_asfloat(r2.y))) + 1.0));
        r1.w = vd_asuint(((-(vd_asfloat(r1.w))) * vd_asfloat(r2.y) + 1.0));
        r1.w = vd_asuint(max(vd_asfloat(r1.w), 0.0));
        r1.w = vd_asuint((vd_asfloat(r1.w) + -1.0));
        r1.w = vd_asuint((vd_asfloat(r3.z) * vd_asfloat(r1.w) + 1.0));
        r4.xyzw = vd_asuint(_PlaneSightRenderTarget.SampleLevel(sampler_PlaneSightRenderTarget, vd_asfloat(r2.zw), 0.0).xyzw);
        r2.y = vd_asuint((vd_asfloat(r4.w) + -1.0));
        r2.y = vd_asuint((vd_asfloat(_GlobalPlaneSystemEnable) * vd_asfloat(r2.y) + 1.0));
        r2.zw = uint2(0x3f800000u, 0x00000000u);
        while (true) {
          r3.w = vd_asuint((-(int)(vd_asint(r2.w) >= 3)));
          if (r3.w != 0u) break;
          r4.xyz = vd_asuint((vd_asfloat(v6.xyz) + (-(vd_asfloat(_SightPositions[(int)(r2.w)].xyz)))));
          r3.w = vd_asuint(dot(vd_asfloat(r4.xyz), vd_asfloat(r4.xyz)));
          r3.w = vd_asuint(sqrt(vd_asfloat(r3.w)));
          r3.w = vd_asuint((vd_asfloat(r3.w) + (-(vd_asfloat(_SeeThroughDistanceMin)))));
          r3.w = vd_asuint(saturate((vd_asfloat(r2.x) * vd_asfloat(r3.w))));
          r4.x = vd_asuint((vd_asfloat(r3.w) * -2.0 + 3.0));
          r3.w = vd_asuint((vd_asfloat(r3.w) * vd_asfloat(r3.w)));
          r3.w = vd_asuint((vd_asfloat(r3.w) * vd_asfloat(r4.x)));
          r2.z = vd_asuint(min(vd_asfloat(r2.z), vd_asfloat(r3.w)));
          r2.w = vd_asuint((vd_asint(r2.w) + 1));
        }
        r2.y = vd_asuint((vd_asfloat(r2.y) * vd_asfloat(r2.y)));
        r2.z = vd_asuint(((-(vd_asfloat(r2.z))) + 1.0));
        r2.y = vd_asuint(((-(vd_asfloat(r2.y))) * vd_asfloat(r2.z) + 1.0));
        r2.y = vd_asuint(max(vd_asfloat(r2.y), 0.0));
        r2.y = vd_asuint((vd_asfloat(r2.y) + -1.0));
        r2.y = vd_asuint((vd_asfloat(r3.z) * vd_asfloat(r2.y) + 1.0));
        r1.y = vd_asuint(((-(vd_asfloat(_WorldSpaceCameraPos.y))) / vd_asfloat(r1.y)));
        r1.xy = vd_asuint((vd_asfloat(r1.xz) * vd_asfloat(r1.yy) + vd_asfloat(_WorldSpaceCameraPos.xz)));
        r1.xy = vd_asuint((vd_asfloat(r1.xy) + (-(vd_asfloat(_PlaneBoundMaxPos.xz)))));
        r1.xy = vd_asuint(saturate((vd_asfloat(r1.xy) / vd_asfloat(r3.xy))));
        r4.xyzw = vd_asuint(_PlaneSightRenderTarget.SampleLevel(sampler_PlaneSightRenderTarget, vd_asfloat(r1.xy), 0.0).xyzw);
        r1.x = vd_asuint((vd_asfloat(r4.w) + -1.0));
        r1.x = vd_asuint((vd_asfloat(_GlobalPlaneSystemEnable) * vd_asfloat(r1.x) + 1.0));
        r1.yz = uint2(0x3f800000u, 0x00000000u);
        while (true) {
          r2.z = vd_asuint((-(int)(vd_asint(r1.z) >= 3)));
          if (r2.z != 0u) break;
          r3.xyw = vd_asuint((vd_asfloat(v6.xyz) + (-(vd_asfloat(_SightPositions[(int)(r1.z)].xyz)))));
          r2.z = vd_asuint(dot(vd_asfloat(r3.xyw), vd_asfloat(r3.xyw)));
          r2.z = vd_asuint(sqrt(vd_asfloat(r2.z)));
          r2.z = vd_asuint((vd_asfloat(r2.z) + (-(vd_asfloat(_SeeThroughDistanceMin)))));
          r2.z = vd_asuint(saturate((vd_asfloat(r2.x) * vd_asfloat(r2.z))));
          r2.w = vd_asuint((vd_asfloat(r2.z) * -2.0 + 3.0));
          r2.z = vd_asuint((vd_asfloat(r2.z) * vd_asfloat(r2.z)));
          r2.z = vd_asuint((vd_asfloat(r2.z) * vd_asfloat(r2.w)));
          r1.y = vd_asuint(min(vd_asfloat(r1.y), vd_asfloat(r2.z)));
          r1.z = vd_asuint((vd_asint(r1.z) + 1));
        }
        r1.x = vd_asuint((vd_asfloat(r1.x) * vd_asfloat(r1.x)));
        r1.y = vd_asuint(((-(vd_asfloat(r1.y))) + 1.0));
        r1.x = vd_asuint(((-(vd_asfloat(r1.x))) * vd_asfloat(r1.y) + 1.0));
        r1.x = vd_asuint(max(vd_asfloat(r1.x), 0.0));
        r1.x = vd_asuint((vd_asfloat(r1.x) + -1.0));
        r1.x = vd_asuint((vd_asfloat(r3.z) * vd_asfloat(r1.x) + 1.0));
        r1.x = vd_asuint(min(vd_asfloat(r1.x), vd_asfloat(r2.y)));
        r1.x = vd_asuint(min(vd_asfloat(r1.x), vd_asfloat(r1.w)));
        r0.w = vd_asuint(min(vd_asfloat(r0.w), vd_asfloat(r1.x)));
        r0.w = vd_asuint(min(vd_asfloat(r0.w), vd_asfloat(_GlobalCoverSeeThroughFactor)));
        r0.w = vd_asuint((vd_asfloat(r0.w) + -1.0));
        r0.y = vd_asuint((vd_asfloat(_GlobalIsRuntime) * vd_asfloat(r0.w) + 1.0));
        r1.xy = vd_asuint((vd_asfloat(_ScreenSize.xy) / vd_asfloat(_ScreenParams.xy)));
        r1.zw = vd_asuint((vd_asfloat(r0.xz) * vd_asfloat(_ScreenParams.xy)));
        r1.xy = vd_asuint((vd_asfloat(r1.xy) * vd_asfloat(r1.zw)));
        r1.xy = vd_asuint(floor(vd_asfloat(r1.xy)));
        r1.xy = ((uint2)vd_asfloat(r1.xy));
        r1.xy = (r1.xy & uint2(0x00000003u, 0x00000003u));
        r0.w = (r1.y << (uint)2);
        r0.w = vd_asuint((vd_asint(r0.w) + vd_asint(r1.x)));
        r0.w = x0[r0.w + 0].x;
        r0.w = vd_asuint(((-(vd_asfloat(r0.w))) + vd_asfloat(r0.y)));
        r0.w = vd_asuint((-(int)(vd_asfloat(r0.w) < 0.0)));
        if (r0.w != 0u) discard;
      }
    }
  }
  r0.w = vd_asuint(((-(vd_asfloat(_GlobalSeeThroughAlpha))) + 1.0));
  r0.y = vd_asuint((vd_asfloat(r0.y) * vd_asfloat(r0.w) + vd_asfloat(_GlobalSeeThroughAlpha)));
  r0.y = vd_asuint((vd_asfloat(r0.y) + -1.0));
  r0.y = vd_asuint((vd_asfloat(_GlobalIsRuntime) * vd_asfloat(r0.y) + 1.0));
  r1.xy = vd_asuint((vd_asfloat(_ScreenSize.xy) / vd_asfloat(_ScreenParams.xy)));
  r0.xz = vd_asuint((vd_asfloat(r0.xz) * vd_asfloat(_ScreenParams.xy)));
  r0.xz = vd_asuint((vd_asfloat(r1.xy) * vd_asfloat(r0.xz)));
  r0.xz = vd_asuint(floor(vd_asfloat(r0.xz)));
  r0.xz = ((uint2)vd_asfloat(r0.xz));
  r0.xz = (r0.xz & uint2(0x00000003u, 0x00000003u));
  r0.z = (r0.z << (uint)2);
  r0.x = vd_asuint((vd_asint(r0.z) + vd_asint(r0.x)));
  r0.x = x0[r0.x + 0].x;
  r0.x = vd_asuint((vd_asfloat(r0.y) * vd_asfloat(_BaseColor.w) + (-(vd_asfloat(r0.x)))));
  r0.x = vd_asuint((-(int)(vd_asfloat(r0.x) < 0.0)));
  if (r0.x != 0u) discard;
  o0.w = vd_asfloat(0x00000000u);
  return;
}
