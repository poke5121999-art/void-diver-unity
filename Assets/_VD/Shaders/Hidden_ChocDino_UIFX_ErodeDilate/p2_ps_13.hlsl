// Hidden/ChocDino/UIFX/ErodeDilate | pass 2 "ErodeDilateAlpha" | progFragment | keywords: DIST_CIRCLE | blob 13
void frag(
  float4 v0 : SV_POSITION0,
  float4 v1 : VDREG1,
  out float4 o0 : SV_Target0)
{
  uint4 r0, r1, r2, r3;
  r0 = 0u; r1 = 0u; r2 = 0u; r3 = 0u;

  r0.xy = vd_asuint(ceil(vd_asfloat(float2(_ErodeRadius, _DilateRadius))));
  r0.x = vd_asuint(max(vd_asfloat(r0.y), vd_asfloat(r0.x)));
  r0.x = vd_asuint(((int)vd_asfloat(r0.x)));
  r0.y = vd_asuint((-vd_asint(r0.x)));
  r0.zw = uint2(0x3f800000u, 0x00000000u);
  r1.x = r0.y;
  while (true) {
    r1.y = vd_asuint((-(int)(vd_asint(r0.x) < vd_asint(r1.x))));
    if (r1.y != 0u) break;
    r2.x = vd_asuint(((float)vd_asint(r1.x)));
    r1.yz = r0.zw;
    r1.w = r0.y;
    while (true) {
      r2.z = vd_asuint((-(int)(vd_asint(r0.x) < vd_asint(r1.w))));
      if (r2.z != 0u) break;
      r2.y = vd_asuint(((float)vd_asint(r1.w)));
      r2.zw = vd_asuint((vd_asfloat(_MainTex_TexelSize.xy) * vd_asfloat(r2.xy) + vd_asfloat(v1.xy)));
      r3.xyzw = vd_asuint(_MainTex.Sample(sampler_MainTex, vd_asfloat(r2.zw)).xyzw);
      r2.y = vd_asuint(dot(vd_asfloat(r2.xy), vd_asfloat(r2.xy)));
      r2.y = vd_asuint(sqrt(vd_asfloat(r2.y)));
      r2.yz = vd_asuint(saturate((vd_asfloat(r2.yy) + (-(vd_asfloat(float2(_ErodeRadius, _DilateRadius)))))));
      r3.yz = vd_asuint((vd_asfloat(r1.yz) + (-(vd_asfloat(r3.xx)))));
      r2.yz = vd_asuint((vd_asfloat(r2.yz) * vd_asfloat(r3.yz) + vd_asfloat(r3.xx)));
      r1.y = vd_asuint(min(vd_asfloat(r1.y), vd_asfloat(r2.y)));
      r1.z = vd_asuint(max(vd_asfloat(r1.z), vd_asfloat(r2.z)));
      r1.w = vd_asuint((vd_asint(r1.w) + 1));
    }
    r0.zw = r1.yz;
    r1.x = vd_asuint((vd_asint(r1.x) + 1));
  }
  r0.x = vd_asuint(((-(vd_asfloat(r0.w))) + vd_asfloat(r0.z)));
  o0.xyzw = abs(vd_asfloat(r0.xxxx));
  return;
}
