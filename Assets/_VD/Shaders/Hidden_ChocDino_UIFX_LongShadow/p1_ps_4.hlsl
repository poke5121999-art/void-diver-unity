// Hidden/ChocDino/UIFX/LongShadow | pass 1 "LongShadow-DistanceMap" | progFragment | keywords: (none) | blob 4
void frag(
  float4 v0 : SV_POSITION0,
  float4 v1 : VDREG1,
  float4 v2 : VDREG2,
  float4 v3 : VDREG3,
  out float4 o0 : SV_Target0)
{
  uint4 r0, r1, r2, r3;
  r0 = 0u; r1 = 0u; r2 = 0u; r3 = 0u;

  r0.x = vd_asuint(max(vd_asint(_Length), 1));
  r0.y = vd_asuint(((float)vd_asint(r0.x)));
  r1.xyz = uint3(0x00000000u, 0x00000000u, 0x00000000u);
  r0.z = 0x00000001u;
  while (true) {
    r0.w = vd_asuint((-(int)(vd_asint(r0.x) < vd_asint(r0.z))));
    if (r0.w != 0u) break;
    r2.xy = vd_asuint((vd_asfloat(_PixelStep.xy) * vd_asfloat(r1.zz) + vd_asfloat(v1.xy)));
    r2.zw = vd_asuint((-(int2)(float2(1.0, 1.0) < vd_asfloat(r2.yx))));
    r3.xy = vd_asuint((-(int2)(vd_asfloat(r2.yx) < float2(0.0, 0.0))));
    r0.w = (r2.z | r3.x);
    r0.w = (r2.w | r0.w);
    r0.w = (r3.y | r0.w);
    if (r0.w != 0u) {
      break;
    }
    r3.xyzw = vd_asuint(_DistanceTex.SampleLevel(sampler_DistanceTex, vd_asfloat(r2.xy), 0.0).xyzw);
    r0.w = vd_asuint((-(int)(4.0 >= vd_asfloat(r3.x))));
    if (r0.w != 0u) {
      r2.xyzw = vd_asuint(_MainTex.SampleLevel(sampler_MainTex, vd_asfloat(r2.xy), 0.0).xyzw);
      r0.w = vd_asuint(saturate((vd_asfloat(r1.z) / vd_asfloat(r0.y))));
      r0.w = vd_asuint(((-(vd_asfloat(r0.w))) + 1.0));
      r0.w = vd_asuint((vd_asfloat(r0.w) * vd_asfloat(r2.w)));
      r2.y = vd_asuint(max(vd_asfloat(r0.w), vd_asfloat(r1.y)));
      r2.x = vd_asuint(max(vd_asfloat(r1.x), vd_asfloat(r2.w)));
      r0.w = vd_asuint((-(int)(vd_asfloat(r2.x) >= 1.0)));
      if (r0.w != 0u) {
        r1.xy = r2.xy;
        break;
      }
    } else {
      r2.xy = r1.xy;
    }
    r0.w = vd_asuint(max(vd_asfloat(r3.x), 1.0));
    r2.z = vd_asuint((vd_asfloat(r0.w) + vd_asfloat(r1.z)));
    r0.w = vd_asuint((-(int)(vd_asfloat(r0.y) < vd_asfloat(r2.z))));
    if (r0.w != 0u) {
      r1.xy = r2.xy;
      break;
    }
    r1.xyz = r2.xyz;
    r0.z = vd_asuint((vd_asint(r0.z) + 1));
  }
  r0.xyzw = vd_asuint((vd_asfloat(v2.xyzw) + (-(vd_asfloat(v3.xyzw)))));
  r0.xyzw = vd_asuint((vd_asfloat(r1.yyyy) * vd_asfloat(r0.xyzw) + vd_asfloat(v3.xyzw)));
  o0.xyzw = (vd_asfloat(r1.xxxx) * vd_asfloat(r0.xyzw));
  return;
}
