// Hidden/ChocDino/UIFX/ErodeDilate | pass 3 "Erode" | progFragment | keywords: DIST_SQUARE | blob 14
void frag(
  float4 v0 : SV_POSITION0,
  float4 v1 : VDREG1,
  out float4 o0 : SV_Target0)
{
  uint4 r0, r1, r2, r3, r4, r5;
  r0 = 0u; r1 = 0u; r2 = 0u; r3 = 0u; r4 = 0u; r5 = 0u;

  r0.x = vd_asuint(ceil(vd_asfloat(_ErodeRadius)));
  r0.x = vd_asuint(((int)vd_asfloat(r0.x)));
  r0.y = vd_asuint((-vd_asint(r0.x)));
  r1.xyzw = uint4(0x3f800000u, 0x3f800000u, 0x3f800000u, 0x3f800000u);
  r0.z = r0.y;
  while (true) {
    r0.w = vd_asuint((-(int)(vd_asint(r0.x) < vd_asint(r0.z))));
    if (r0.w != 0u) break;
    r2.x = vd_asuint(((float)vd_asint(r0.z)));
    r3.xyzw = r1.xyzw;
    r0.w = r0.y;
    while (true) {
      r2.z = vd_asuint((-(int)(vd_asint(r0.x) < vd_asint(r0.w))));
      if (r2.z != 0u) break;
      r2.y = vd_asuint(((float)vd_asint(r0.w)));
      r2.zw = vd_asuint((vd_asfloat(_MainTex_TexelSize.xy) * vd_asfloat(r2.xy) + vd_asfloat(v1.xy)));
      r4.xyzw = vd_asuint(_MainTex.Sample(sampler_MainTex, vd_asfloat(r2.zw)).xyzw);
      r2.y = vd_asuint(max(abs(vd_asfloat(r2.y)), abs(vd_asfloat(r2.x))));
      r2.y = vd_asuint(saturate((vd_asfloat(r2.y) + (-(vd_asfloat(_ErodeRadius))))));
      r5.xyzw = vd_asuint((vd_asfloat(r3.xyzw) + (-(vd_asfloat(r4.xyzw)))));
      r4.xyzw = vd_asuint((vd_asfloat(r2.yyyy) * vd_asfloat(r5.xyzw) + vd_asfloat(r4.xyzw)));
      r3.xyzw = vd_asuint(min(vd_asfloat(r3.xyzw), vd_asfloat(r4.xyzw)));
      r0.w = vd_asuint((vd_asint(r0.w) + 1));
    }
    r1.xyzw = r3.xyzw;
    r0.z = vd_asuint((vd_asint(r0.z) + 1));
  }
  o0.xyzw = vd_asfloat(r1.xyzw);
  return;
}
