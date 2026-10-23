// LCArt/Background/shader_DE_BG_CutoffMask | pass 0 "CustomDepth" | progFragment | keywords: (none) | blob 2
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
  uint4 r0, r1, r2;
  r0 = 0u; r1 = 0u; r2 = 0u;

  r0.xyzw = vd_asuint(_BaseMap.SampleBias(sampler_BaseMap, vd_asfloat(v1.xy), vd_asfloat(_GlobalMipBias.x)).xyzw);
  r0.x = vd_asuint((vd_asfloat(r0.w) + -0.100000001));
  r0.x = vd_asuint((-(int)(vd_asfloat(r0.x) < 0.0)));
  if (r0.x != 0u) discard;
  r0.x = vd_asuint((-(int)(0.5 < vd_asfloat(_DissolveProgress))));
  if (r0.x != 0u) {
    r0.x = vd_asuint(saturate((vd_asfloat(_DissolveProgress) * 2.0 + -1.0)));
    r0.x = vd_asuint(((-(vd_asfloat(r0.x))) + 0.999000013));
    r0.x = vd_asuint((-(int)(vd_asfloat(r0.x) < 0.0)));
    if (r0.x != 0u) discard;
  }
  r0.x = vd_asuint((-(int)(vd_asfloat(_GlobalBoxAreaEnable) >= 0.5)));
  if (r0.x != 0u) {
    r0.xy = vd_asuint((vd_asfloat(v6.xz) * float2(0.5, 0.5)));
    r0.zw = vd_asuint(floor(vd_asfloat(r0.xy)));
    r0.xy = vd_asuint(frac(vd_asfloat(r0.xy)));
    r1.xy = vd_asuint((vd_asfloat(r0.xy) * vd_asfloat(r0.xy)));
    r0.xy = vd_asuint(((-(vd_asfloat(r0.xy))) * float2(2.0, 2.0) + float2(3.0, 3.0)));
    r0.xy = vd_asuint((vd_asfloat(r0.xy) * vd_asfloat(r1.xy)));
    r1.x = vd_asuint(dot(vd_asfloat(r0.zw), float2(127.099998, 311.700012)));
    r1.x = vd_asuint(sin(vd_asfloat(r1.x)));
    r1.x = vd_asuint((vd_asfloat(r1.x) * 43758.5469));
    r2.xyzw = vd_asuint((vd_asfloat(r0.zwzw) + float4(1.0, 0.0, 0.0, 1.0)));
    r1.y = vd_asuint(dot(vd_asfloat(r2.xy), float2(127.099998, 311.700012)));
    r1.y = vd_asuint(sin(vd_asfloat(r1.y)));
    r1.y = vd_asuint((vd_asfloat(r1.y) * 43758.5469));
    r1.z = vd_asuint(dot(vd_asfloat(r2.zw), float2(127.099998, 311.700012)));
    r1.z = vd_asuint(sin(vd_asfloat(r1.z)));
    r1.z = vd_asuint((vd_asfloat(r1.z) * 43758.5469));
    r1.xyz = vd_asuint(frac(vd_asfloat(r1.xyz)));
    r0.zw = vd_asuint((vd_asfloat(r0.zw) + float2(1.0, 1.0)));
    r0.z = vd_asuint(dot(vd_asfloat(r0.zw), float2(127.099998, 311.700012)));
    r0.z = vd_asuint(sin(vd_asfloat(r0.z)));
    r0.z = vd_asuint((vd_asfloat(r0.z) * 43758.5469));
    r0.z = vd_asuint(frac(vd_asfloat(r0.z)));
    r0.w = vd_asuint(((-(vd_asfloat(r1.x))) + vd_asfloat(r1.y)));
    r0.w = vd_asuint((vd_asfloat(r0.x) * vd_asfloat(r0.w) + vd_asfloat(r1.x)));
    r0.z = vd_asuint(((-(vd_asfloat(r1.z))) + vd_asfloat(r0.z)));
    r0.x = vd_asuint((vd_asfloat(r0.x) * vd_asfloat(r0.z) + vd_asfloat(r1.z)));
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
    r1.xyz = vd_asuint((vd_asfloat(v6.xyz) + (-(vd_asfloat(_GlobalBoxAreaCenters[7].xyz)))));
    r1.xyz = vd_asuint(((-(abs(vd_asfloat(r1.xyz)))) + vd_asfloat(_GlobalBoxAreaExtents[7].xyz)));
    r0.z = vd_asuint(min(vd_asfloat(r1.z), vd_asfloat(r1.y)));
    r0.z = vd_asuint(min(vd_asfloat(r0.z), vd_asfloat(r1.x)));
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
  r0.xyz = vd_asuint((-(int3)(float3(1.5, 0.5, 1.5) < vd_asfloat(v8.wzz))));
  r0.w = vd_asuint((vd_asfloat(v7.y) + -0.5));
  r0.w = vd_asuint((vd_asfloat(r0.w) * 100.0 + vd_asfloat((VD_COL(unity_ObjectToWorld, 3).y))));
  r1.x = vd_asuint(((-(vd_asfloat(r0.w))) + vd_asfloat((VD_COL(unity_ObjectToWorld, 3).y))));
  r1.y = vd_asuint((vd_asfloat(_IsSingleObject) * vd_asfloat(r1.x) + vd_asfloat(r0.w)));
  r0.w = vd_asuint((vd_asfloat(v8.x) * 100.0));
  r1.z = vd_asuint(((-(vd_asfloat(v8.x))) * 100.0 + vd_asfloat(_ObjectBoundSize.y)));
  r0.w = vd_asuint((vd_asfloat(_IsSingleObject) * vd_asfloat(r1.z) + vd_asfloat(r0.w)));
  r1.z = vd_asuint((-(int)(vd_asfloat(r1.y) >= 1.5)));
  r1.z = (r1.z & 0x3f800000u);
  r0.w = vd_asuint((-(int)(vd_asfloat(r0.w) >= 1.5)));
  r0.w = (r0.w & 0x3f800000u);
  r0.w = vd_asuint(max(vd_asfloat(r0.w), vd_asfloat(r1.z)));
  r1.x = vd_asuint(((-(vd_asfloat(r0.w))) + 1.0));
  r0.xw = ((r0.xx != 0u) ? r1.xy : uint2(0x3f800000u, 0x00000000u));
  r0.w = vd_asuint((-(int)(vd_asfloat(r0.w) < 0.5)));
  r0.yw = (r0.yw & uint2(0x00000001u, 0x00000001u));
  r0.y = vd_asuint((vd_asint(r0.y) * vd_asint(r0.w)));
  r0.y = vd_asuint(((float)vd_asint(r0.y)));
  r0.y = vd_asuint(((-(vd_asfloat(r0.y))) + 1.0));
  r0.x = vd_asuint((vd_asfloat(r0.y) * vd_asfloat(r0.x)));
  r0.x = ((r0.z != 0u) ? 0x00000000u : r0.x);
  r0.y = vd_asuint(((-(vd_asfloat(r0.x))) + 1.0));
  r0.x = vd_asuint((vd_asfloat(_GlobalAlwaysStencil) * vd_asfloat(r0.y) + vd_asfloat(r0.x)));
  o0.y = (vd_asfloat(r0.x) * vd_asfloat(v0.z));
  o0.x = vd_asfloat(vd_asuint(v0.z));
  o0.zw = vd_asfloat(uint2(0x00000000u, 0x00000000u));
  return;
}
