// Hidden/ChocDino/UIFX/ErodeDilate | pass 0 "ErodeAlpha" | progFragment | keywords: DIST_SQUARE | blob 5
void frag(
  float4 v0 : SV_POSITION0,
  float4 v1 : VDREG1,
  out float4 o0 : SV_Target0)
{
  uint4 r0, r1, r2;
  r0 = 0u; r1 = 0u; r2 = 0u;

  r0.x = vd_asuint(ceil(vd_asfloat(_ErodeRadius)));
  r0.x = vd_asuint(((int)vd_asfloat(r0.x)));
  r0.y = vd_asuint((-vd_asint(r0.x)));
  r0.z = 0x3f800000u;
  r0.w = r0.y;
  while (true) {
    r1.x = vd_asuint((-(int)(vd_asint(r0.x) < vd_asint(r0.w))));
    if (r1.x != 0u) break;
    r1.x = vd_asuint(((float)vd_asint(r0.w)));
    r1.z = r0.z;
    r1.w = r0.y;
    while (true) {
      r2.x = vd_asuint((-(int)(vd_asint(r0.x) < vd_asint(r1.w))));
      if (r2.x != 0u) break;
      r1.y = vd_asuint(((float)vd_asint(r1.w)));
      r2.xy = vd_asuint((vd_asfloat(_MainTex_TexelSize.xy) * vd_asfloat(r1.xy) + vd_asfloat(v1.xy)));
      r2.xyzw = vd_asuint(_MainTex.Sample(sampler_MainTex, vd_asfloat(r2.xy)).xyzw);
      r1.y = vd_asuint(max(abs(vd_asfloat(r1.y)), abs(vd_asfloat(r1.x))));
      r1.y = vd_asuint(saturate((vd_asfloat(r1.y) + (-(vd_asfloat(_ErodeRadius))))));
      r2.y = vd_asuint((vd_asfloat(r1.z) + (-(vd_asfloat(r2.x)))));
      r1.y = vd_asuint((vd_asfloat(r1.y) * vd_asfloat(r2.y) + vd_asfloat(r2.x)));
      r1.z = vd_asuint(min(vd_asfloat(r1.y), vd_asfloat(r1.z)));
      r1.w = vd_asuint((vd_asint(r1.w) + 1));
    }
    r0.z = r1.z;
    r0.w = vd_asuint((vd_asint(r0.w) + 1));
  }
  o0.xyzw = vd_asfloat(r0.zzzz);
  return;
}
