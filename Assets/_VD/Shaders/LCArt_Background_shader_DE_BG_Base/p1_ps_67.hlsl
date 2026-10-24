// LCArt/Background/shader_DE_BG_Base | pass 1 "DepthNormals" | progFragment | keywords: _DE_EDITOR_CAMERA _DE_REFLECTION_CAMERA | blob 67
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
  uint4 r0, r1, r2, r3;
  r0 = 0u; r1 = 0u; r2 = 0u; r3 = 0u;
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
  r0.x = vd_asuint((-(int)(vd_asfloat(_GlobalBoxAreaEnable) >= 0.5)));
  if (r0.x != 0u) {
    r0.xy = vd_asuint((vd_asfloat(v6.xz) * float2(0.5, 0.5)));
    r0.zw = vd_asuint(floor(vd_asfloat(r0.xy)));
    r0.xy = vd_asuint(frac(vd_asfloat(r0.xy)));
    r2.xy = vd_asuint((vd_asfloat(r0.xy) * vd_asfloat(r0.xy)));
    r0.xy = vd_asuint(((-(vd_asfloat(r0.xy))) * float2(2.0, 2.0) + float2(3.0, 3.0)));
    r0.xy = vd_asuint((vd_asfloat(r0.xy) * vd_asfloat(r2.xy)));
    r1.w = vd_asuint(dot(vd_asfloat(r0.zw), float2(127.099998, 311.700012)));
    r1.w = vd_asuint(sin(vd_asfloat(r1.w)));
    r1.w = vd_asuint((vd_asfloat(r1.w) * 43758.5469));
    r1.w = vd_asuint(frac(vd_asfloat(r1.w)));
    r2.xyzw = vd_asuint((vd_asfloat(r0.zwzw) + float4(1.0, 0.0, 0.0, 1.0)));
    r2.x = vd_asuint(dot(vd_asfloat(r2.xy), float2(127.099998, 311.700012)));
    r2.x = vd_asuint(sin(vd_asfloat(r2.x)));
    r2.x = vd_asuint((vd_asfloat(r2.x) * 43758.5469));
    r2.y = vd_asuint(dot(vd_asfloat(r2.zw), float2(127.099998, 311.700012)));
    r2.y = vd_asuint(sin(vd_asfloat(r2.y)));
    r2.y = vd_asuint((vd_asfloat(r2.y) * 43758.5469));
    r2.xy = vd_asuint(frac(vd_asfloat(r2.xy)));
    r0.zw = vd_asuint((vd_asfloat(r0.zw) + float2(1.0, 1.0)));
    r0.z = vd_asuint(dot(vd_asfloat(r0.zw), float2(127.099998, 311.700012)));
    r0.z = vd_asuint(sin(vd_asfloat(r0.z)));
    r0.z = vd_asuint((vd_asfloat(r0.z) * 43758.5469));
    r0.z = vd_asuint(frac(vd_asfloat(r0.z)));
    r0.w = vd_asuint(((-(vd_asfloat(r1.w))) + vd_asfloat(r2.x)));
    r0.w = vd_asuint((vd_asfloat(r0.x) * vd_asfloat(r0.w) + vd_asfloat(r1.w)));
    r0.z = vd_asuint(((-(vd_asfloat(r2.y))) + vd_asfloat(r0.z)));
    r0.x = vd_asuint((vd_asfloat(r0.x) * vd_asfloat(r0.z) + vd_asfloat(r2.y)));
    r0.x = vd_asuint(((-(vd_asfloat(r0.w))) + vd_asfloat(r0.x)));
    r0.x = vd_asuint((vd_asfloat(r0.y) * vd_asfloat(r0.x) + vd_asfloat(r0.w)));
    r0.y = vd_asuint(((int)vd_asfloat(_GlobalBoxAreaCenters[0].w)));
    r0.y = vd_asuint((-(int)(vd_asint(r0.y) == 4)));
    if (r0.y != 0u) {
      r0.yzw = vd_asuint((vd_asfloat(v6.xyz) + (-(vd_asfloat(_GlobalBoxAreaCenters[0].xyz)))));
      r0.yzw = vd_asuint(((-(abs(vd_asfloat(r0.yzw)))) + vd_asfloat(_GlobalBoxAreaExtents[0].xyz)));
      r0.z = vd_asuint(min(vd_asfloat(r0.w), vd_asfloat(r0.z)));
      r0.y = vd_asuint(min(vd_asfloat(r0.z), vd_asfloat(r0.y)));
      r0.z = vd_asuint(max(vd_asfloat(_GlobalBoxAreaExtents[0].w), 0.00100000005));
      r0.y = vd_asuint(saturate((vd_asfloat(r0.y) / vd_asfloat(r0.z))));
      r0.z = vd_asuint((-(int)(0.0 < vd_asfloat(r0.y))));
      if (r0.z != 0u) {
        r0.z = vd_asuint(((-(vd_asfloat(_GlobalBoxAreaIntensities[0]))) + 1.125));
        r0.z = vd_asuint(((-(vd_asfloat(r0.z))) + vd_asfloat(r0.x)));
        r0.z = vd_asuint(saturate((vd_asfloat(r0.z) * -4.0)));
        r0.w = vd_asuint((vd_asfloat(r0.z) * -2.0 + 3.0));
        r0.z = vd_asuint((vd_asfloat(r0.z) * vd_asfloat(r0.z)));
        r0.z = vd_asuint((vd_asfloat(r0.w) * vd_asfloat(r0.z) + -0.5));
        r0.y = vd_asuint(((-(vd_asfloat(r0.y))) + vd_asfloat(r0.z)));
        r0.y = vd_asuint((vd_asfloat(r0.y) + 1.0));
        r0.y = vd_asuint((-(int)(vd_asfloat(r0.y) < 0.0)));
        if (r0.y != 0u) discard;
      }
    }
    r0.y = vd_asuint(((int)vd_asfloat(_GlobalBoxAreaCenters[1].w)));
    r0.y = vd_asuint((-(int)(vd_asint(r0.y) == 4)));
    if (r0.y != 0u) {
      r0.yzw = vd_asuint((vd_asfloat(v6.xyz) + (-(vd_asfloat(_GlobalBoxAreaCenters[1].xyz)))));
      r0.yzw = vd_asuint(((-(abs(vd_asfloat(r0.yzw)))) + vd_asfloat(_GlobalBoxAreaExtents[1].xyz)));
      r0.z = vd_asuint(min(vd_asfloat(r0.w), vd_asfloat(r0.z)));
      r0.y = vd_asuint(min(vd_asfloat(r0.z), vd_asfloat(r0.y)));
      r0.z = vd_asuint(max(vd_asfloat(_GlobalBoxAreaExtents[1].w), 0.00100000005));
      r0.y = vd_asuint(saturate((vd_asfloat(r0.y) / vd_asfloat(r0.z))));
      r0.z = vd_asuint((-(int)(0.0 < vd_asfloat(r0.y))));
      if (r0.z != 0u) {
        r0.z = vd_asuint(((-(vd_asfloat(_GlobalBoxAreaIntensities[1]))) + 1.125));
        r0.z = vd_asuint(((-(vd_asfloat(r0.z))) + vd_asfloat(r0.x)));
        r0.z = vd_asuint(saturate((vd_asfloat(r0.z) * -4.0)));
        r0.w = vd_asuint((vd_asfloat(r0.z) * -2.0 + 3.0));
        r0.z = vd_asuint((vd_asfloat(r0.z) * vd_asfloat(r0.z)));
        r0.z = vd_asuint((vd_asfloat(r0.w) * vd_asfloat(r0.z) + -0.5));
        r0.y = vd_asuint(((-(vd_asfloat(r0.y))) + vd_asfloat(r0.z)));
        r0.y = vd_asuint((vd_asfloat(r0.y) + 1.0));
        r0.y = vd_asuint((-(int)(vd_asfloat(r0.y) < 0.0)));
        if (r0.y != 0u) discard;
      }
    }
    r0.y = vd_asuint(((int)vd_asfloat(_GlobalBoxAreaCenters[2].w)));
    r0.y = vd_asuint((-(int)(vd_asint(r0.y) == 4)));
    if (r0.y != 0u) {
      r0.yzw = vd_asuint((vd_asfloat(v6.xyz) + (-(vd_asfloat(_GlobalBoxAreaCenters[2].xyz)))));
      r0.yzw = vd_asuint(((-(abs(vd_asfloat(r0.yzw)))) + vd_asfloat(_GlobalBoxAreaExtents[2].xyz)));
      r0.z = vd_asuint(min(vd_asfloat(r0.w), vd_asfloat(r0.z)));
      r0.y = vd_asuint(min(vd_asfloat(r0.z), vd_asfloat(r0.y)));
      r0.z = vd_asuint(max(vd_asfloat(_GlobalBoxAreaExtents[2].w), 0.00100000005));
      r0.y = vd_asuint(saturate((vd_asfloat(r0.y) / vd_asfloat(r0.z))));
      r0.z = vd_asuint((-(int)(0.0 < vd_asfloat(r0.y))));
      if (r0.z != 0u) {
        r0.z = vd_asuint(((-(vd_asfloat(_GlobalBoxAreaIntensities[2]))) + 1.125));
        r0.z = vd_asuint(((-(vd_asfloat(r0.z))) + vd_asfloat(r0.x)));
        r0.z = vd_asuint(saturate((vd_asfloat(r0.z) * -4.0)));
        r0.w = vd_asuint((vd_asfloat(r0.z) * -2.0 + 3.0));
        r0.z = vd_asuint((vd_asfloat(r0.z) * vd_asfloat(r0.z)));
        r0.z = vd_asuint((vd_asfloat(r0.w) * vd_asfloat(r0.z) + -0.5));
        r0.y = vd_asuint(((-(vd_asfloat(r0.y))) + vd_asfloat(r0.z)));
        r0.y = vd_asuint((vd_asfloat(r0.y) + 1.0));
        r0.y = vd_asuint((-(int)(vd_asfloat(r0.y) < 0.0)));
        if (r0.y != 0u) discard;
      }
    }
    r0.y = vd_asuint(((int)vd_asfloat(_GlobalBoxAreaCenters[3].w)));
    r0.y = vd_asuint((-(int)(vd_asint(r0.y) == 4)));
    if (r0.y != 0u) {
      r0.yzw = vd_asuint((vd_asfloat(v6.xyz) + (-(vd_asfloat(_GlobalBoxAreaCenters[3].xyz)))));
      r0.yzw = vd_asuint(((-(abs(vd_asfloat(r0.yzw)))) + vd_asfloat(_GlobalBoxAreaExtents[3].xyz)));
      r0.z = vd_asuint(min(vd_asfloat(r0.w), vd_asfloat(r0.z)));
      r0.y = vd_asuint(min(vd_asfloat(r0.z), vd_asfloat(r0.y)));
      r0.z = vd_asuint(max(vd_asfloat(_GlobalBoxAreaExtents[3].w), 0.00100000005));
      r0.y = vd_asuint(saturate((vd_asfloat(r0.y) / vd_asfloat(r0.z))));
      r0.z = vd_asuint((-(int)(0.0 < vd_asfloat(r0.y))));
      if (r0.z != 0u) {
        r0.z = vd_asuint(((-(vd_asfloat(_GlobalBoxAreaIntensities[3]))) + 1.125));
        r0.z = vd_asuint(((-(vd_asfloat(r0.z))) + vd_asfloat(r0.x)));
        r0.z = vd_asuint(saturate((vd_asfloat(r0.z) * -4.0)));
        r0.w = vd_asuint((vd_asfloat(r0.z) * -2.0 + 3.0));
        r0.z = vd_asuint((vd_asfloat(r0.z) * vd_asfloat(r0.z)));
        r0.z = vd_asuint((vd_asfloat(r0.w) * vd_asfloat(r0.z) + -0.5));
        r0.y = vd_asuint(((-(vd_asfloat(r0.y))) + vd_asfloat(r0.z)));
        r0.y = vd_asuint((vd_asfloat(r0.y) + 1.0));
        r0.y = vd_asuint((-(int)(vd_asfloat(r0.y) < 0.0)));
        if (r0.y != 0u) discard;
      }
    }
    r0.y = vd_asuint(((int)vd_asfloat(_GlobalBoxAreaCenters[4].w)));
    r0.y = vd_asuint((-(int)(vd_asint(r0.y) == 4)));
    if (r0.y != 0u) {
      r0.yzw = vd_asuint((vd_asfloat(v6.xyz) + (-(vd_asfloat(_GlobalBoxAreaCenters[4].xyz)))));
      r0.yzw = vd_asuint(((-(abs(vd_asfloat(r0.yzw)))) + vd_asfloat(_GlobalBoxAreaExtents[4].xyz)));
      r0.z = vd_asuint(min(vd_asfloat(r0.w), vd_asfloat(r0.z)));
      r0.y = vd_asuint(min(vd_asfloat(r0.z), vd_asfloat(r0.y)));
      r0.z = vd_asuint(max(vd_asfloat(_GlobalBoxAreaExtents[4].w), 0.00100000005));
      r0.y = vd_asuint(saturate((vd_asfloat(r0.y) / vd_asfloat(r0.z))));
      r0.z = vd_asuint((-(int)(0.0 < vd_asfloat(r0.y))));
      if (r0.z != 0u) {
        r0.z = vd_asuint(((-(vd_asfloat(_GlobalBoxAreaIntensities[4]))) + 1.125));
        r0.z = vd_asuint(((-(vd_asfloat(r0.z))) + vd_asfloat(r0.x)));
        r0.z = vd_asuint(saturate((vd_asfloat(r0.z) * -4.0)));
        r0.w = vd_asuint((vd_asfloat(r0.z) * -2.0 + 3.0));
        r0.z = vd_asuint((vd_asfloat(r0.z) * vd_asfloat(r0.z)));
        r0.z = vd_asuint((vd_asfloat(r0.w) * vd_asfloat(r0.z) + -0.5));
        r0.y = vd_asuint(((-(vd_asfloat(r0.y))) + vd_asfloat(r0.z)));
        r0.y = vd_asuint((vd_asfloat(r0.y) + 1.0));
        r0.y = vd_asuint((-(int)(vd_asfloat(r0.y) < 0.0)));
        if (r0.y != 0u) discard;
      }
    }
    r0.y = vd_asuint(((int)vd_asfloat(_GlobalBoxAreaCenters[5].w)));
    r0.y = vd_asuint((-(int)(vd_asint(r0.y) == 4)));
    if (r0.y != 0u) {
      r0.yzw = vd_asuint((vd_asfloat(v6.xyz) + (-(vd_asfloat(_GlobalBoxAreaCenters[5].xyz)))));
      r0.yzw = vd_asuint(((-(abs(vd_asfloat(r0.yzw)))) + vd_asfloat(_GlobalBoxAreaExtents[5].xyz)));
      r0.z = vd_asuint(min(vd_asfloat(r0.w), vd_asfloat(r0.z)));
      r0.y = vd_asuint(min(vd_asfloat(r0.z), vd_asfloat(r0.y)));
      r0.z = vd_asuint(max(vd_asfloat(_GlobalBoxAreaExtents[5].w), 0.00100000005));
      r0.y = vd_asuint(saturate((vd_asfloat(r0.y) / vd_asfloat(r0.z))));
      r0.z = vd_asuint((-(int)(0.0 < vd_asfloat(r0.y))));
      if (r0.z != 0u) {
        r0.z = vd_asuint(((-(vd_asfloat(_GlobalBoxAreaIntensities[5]))) + 1.125));
        r0.z = vd_asuint(((-(vd_asfloat(r0.z))) + vd_asfloat(r0.x)));
        r0.z = vd_asuint(saturate((vd_asfloat(r0.z) * -4.0)));
        r0.w = vd_asuint((vd_asfloat(r0.z) * -2.0 + 3.0));
        r0.z = vd_asuint((vd_asfloat(r0.z) * vd_asfloat(r0.z)));
        r0.z = vd_asuint((vd_asfloat(r0.w) * vd_asfloat(r0.z) + -0.5));
        r0.y = vd_asuint(((-(vd_asfloat(r0.y))) + vd_asfloat(r0.z)));
        r0.y = vd_asuint((vd_asfloat(r0.y) + 1.0));
        r0.y = vd_asuint((-(int)(vd_asfloat(r0.y) < 0.0)));
        if (r0.y != 0u) discard;
      }
    }
    r0.y = vd_asuint(((int)vd_asfloat(_GlobalBoxAreaCenters[6].w)));
    r0.y = vd_asuint((-(int)(vd_asint(r0.y) == 4)));
    if (r0.y != 0u) {
      r0.yzw = vd_asuint((vd_asfloat(v6.xyz) + (-(vd_asfloat(_GlobalBoxAreaCenters[6].xyz)))));
      r0.yzw = vd_asuint(((-(abs(vd_asfloat(r0.yzw)))) + vd_asfloat(_GlobalBoxAreaExtents[6].xyz)));
      r0.z = vd_asuint(min(vd_asfloat(r0.w), vd_asfloat(r0.z)));
      r0.y = vd_asuint(min(vd_asfloat(r0.z), vd_asfloat(r0.y)));
      r0.z = vd_asuint(max(vd_asfloat(_GlobalBoxAreaExtents[6].w), 0.00100000005));
      r0.y = vd_asuint(saturate((vd_asfloat(r0.y) / vd_asfloat(r0.z))));
      r0.z = vd_asuint((-(int)(0.0 < vd_asfloat(r0.y))));
      if (r0.z != 0u) {
        r0.z = vd_asuint(((-(vd_asfloat(_GlobalBoxAreaIntensities[6]))) + 1.125));
        r0.z = vd_asuint(((-(vd_asfloat(r0.z))) + vd_asfloat(r0.x)));
        r0.z = vd_asuint(saturate((vd_asfloat(r0.z) * -4.0)));
        r0.w = vd_asuint((vd_asfloat(r0.z) * -2.0 + 3.0));
        r0.z = vd_asuint((vd_asfloat(r0.z) * vd_asfloat(r0.z)));
        r0.z = vd_asuint((vd_asfloat(r0.w) * vd_asfloat(r0.z) + -0.5));
        r0.y = vd_asuint(((-(vd_asfloat(r0.y))) + vd_asfloat(r0.z)));
        r0.y = vd_asuint((vd_asfloat(r0.y) + 1.0));
        r0.y = vd_asuint((-(int)(vd_asfloat(r0.y) < 0.0)));
        if (r0.y != 0u) discard;
      }
    }
    r0.y = vd_asuint(((int)vd_asfloat(_GlobalBoxAreaCenters[7].w)));
    r0.y = vd_asuint((-(int)(vd_asint(r0.y) == 4)));
    r2.xyz = vd_asuint((vd_asfloat(v6.xyz) + (-(vd_asfloat(_GlobalBoxAreaCenters[7].xyz)))));
    r2.xyz = vd_asuint(((-(abs(vd_asfloat(r2.xyz)))) + vd_asfloat(_GlobalBoxAreaExtents[7].xyz)));
    r0.z = vd_asuint(min(vd_asfloat(r2.z), vd_asfloat(r2.y)));
    r0.z = vd_asuint(min(vd_asfloat(r0.z), vd_asfloat(r2.x)));
    r0.w = vd_asuint(max(vd_asfloat(_GlobalBoxAreaExtents[7].w), 0.00100000005));
    r0.z = vd_asuint(saturate((vd_asfloat(r0.z) / vd_asfloat(r0.w))));
    r0.w = vd_asuint((-(int)(0.0 < vd_asfloat(r0.z))));
    r0.y = (r0.w & r0.y);
    r0.w = vd_asuint(((-(vd_asfloat(_GlobalBoxAreaIntensities[7]))) + 1.125));
    r0.x = vd_asuint(((-(vd_asfloat(r0.w))) + vd_asfloat(r0.x)));
    r0.x = vd_asuint(saturate((vd_asfloat(r0.x) * -4.0)));
    r0.w = vd_asuint((vd_asfloat(r0.x) * -2.0 + 3.0));
    r0.x = vd_asuint((vd_asfloat(r0.x) * vd_asfloat(r0.x)));
    r0.x = vd_asuint((vd_asfloat(r0.w) * vd_asfloat(r0.x) + -0.5));
    r0.x = vd_asuint(((-(vd_asfloat(r0.z))) + vd_asfloat(r0.x)));
    r0.x = vd_asuint((vd_asfloat(r0.x) + 1.0));
    r0.x = vd_asuint((-(int)(vd_asfloat(r0.x) < 0.0)));
    r0.x = (r0.y & r0.x);
    if (r0.x != 0u) discard;
  }
  r0.x = vd_asuint((-(int)(vd_asint(_BackgroundType) == 4)));
  if (r0.x != 0u) discard;
  r0.xy = vd_asuint((float2(1.0, 1.0) / vd_asfloat(_ScaledScreenParams.xy)));
  r0.xy = vd_asuint((vd_asfloat(r0.xy) * vd_asfloat(v0.xy)));
#if UNITY_UV_STARTS_AT_TOP
  r0.y = vd_asuint((vd_asfloat(r0.y) * vd_asfloat(_ScaleBiasRt.x) + vd_asfloat(_ScaleBiasRt.y)));
  r0.z = vd_asuint(((-(vd_asfloat(r0.y))) + 1.0));
#else
  r0.y = r0.y;
  r0.z = r0.y;
#endif
  r0.y = vd_asuint(((-(vd_asfloat(_GlobalSeeThroughAlpha))) + 1.0));
  r0.y = vd_asuint((vd_asfloat(v1.z) * vd_asfloat(r0.y) + vd_asfloat(_GlobalSeeThroughAlpha)));
  r0.y = vd_asuint((vd_asfloat(r0.y) + -1.0));
  r0.y = vd_asuint((vd_asfloat(_GlobalIsRuntime) * vd_asfloat(r0.y) + 1.0));
  r2.xy = vd_asuint((vd_asfloat(_ScreenSize.xy) / vd_asfloat(_ScreenParams.xy)));
  r0.xz = vd_asuint((vd_asfloat(r0.xz) * vd_asfloat(_ScreenParams.xy)));
  r0.xz = vd_asuint((vd_asfloat(r2.xy) * vd_asfloat(r0.xz)));
  r0.xz = vd_asuint(floor(vd_asfloat(r0.xz)));
  r0.xz = ((uint2)vd_asfloat(r0.xz));
  r0.xz = (r0.xz & uint2(0x00000003u, 0x00000003u));
  r0.z = (r0.z << (uint)2);
  r0.x = vd_asuint((vd_asint(r0.z) + vd_asint(r0.x)));
  r0.x = x0[r0.x + 0].x;
  r0.x = vd_asuint((vd_asfloat(r0.y) * vd_asfloat(_BaseColor.w) + (-(vd_asfloat(r0.x)))));
  r0.x = vd_asuint((-(int)(vd_asfloat(r0.x) < 0.0)));
  if (r0.x != 0u) discard;
  r0.x = vd_asuint(dot(vd_asfloat(r1.xyz), vd_asfloat(r1.xyz)));
  r0.x = vd_asuint(rsqrt(vd_asfloat(r0.x)));
  o0.xyz = (vd_asfloat(r0.xxx) * vd_asfloat(r1.xyz));
  o0.w = vd_asfloat(0x00000000u);
  return;
}
