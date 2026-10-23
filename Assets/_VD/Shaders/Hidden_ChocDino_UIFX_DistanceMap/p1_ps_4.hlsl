// Hidden/ChocDino/UIFX/DistanceMap | pass 1 "InvAlphaToUV" | progFragment | keywords: (none) | blob 4
void frag(
  float4 v0 : SV_POSITION0,
  float4 v1 : VDREG1,
  out float2 o0 : SV_Target0)
{
  uint4 r0, r1, r2, r3, r4, r5, r6, r7, r8;
  r0 = 0u; r1 = 0u; r2 = 0u; r3 = 0u; r4 = 0u; r5 = 0u; r6 = 0u; r7 = 0u; r8 = 0u;

  r0.xyzw = vd_asuint(_MainTex.Sample(sampler_MainTex, vd_asfloat(v1.xy)).xyzw);
  r0.x = vd_asuint(((-(vd_asfloat(r0.w))) + 1.0));
  r0.y = vd_asuint((-(int)(0.99000001 < vd_asfloat(r0.x))));
  if (r0.y != 0u) {
    o0.xy = vd_asfloat(vd_asuint(v1.xy));
    return;
  }
  r0.y = vd_asuint((-(int)(vd_asfloat(r0.x) < 0.00999999978)));
  if (r0.y != 0u) {
    o0.xy = vd_asfloat(uint2(0xbf800000u, 0xbf800000u));
    return;
  }
  r1.xyw = vd_asuint((-(vd_asfloat(_MainTex_TexelSize.xyy))));
  r0.yz = vd_asuint((vd_asfloat(r1.xy) + vd_asfloat(v1.xy)));
  r2.xyzw = vd_asuint(_MainTex.Sample(sampler_MainTex, vd_asfloat(r0.yz)).xyzw);
  r1.z = 0x00000000u;
  r0.yz = vd_asuint((vd_asfloat(r1.zw) + vd_asfloat(v1.xy)));
  r1.xyzw = vd_asuint(_MainTex.Sample(sampler_MainTex, vd_asfloat(r0.yz)).xyzw);
  r3.xyzw = vd_asuint((vd_asfloat(_MainTex_TexelSize.xyxy) * float4(1.0, -1.0, -1.0, 1.0) + vd_asfloat(v1.xyxy)));
  r4.xyzw = vd_asuint(_MainTex.Sample(sampler_MainTex, vd_asfloat(r3.xy)).xyzw);
  r5.x = vd_asuint((-(vd_asfloat(_MainTex_TexelSize.x))));
  r5.yw = uint2(0x00000000u, 0x00000000u);
  r0.yz = vd_asuint((vd_asfloat(r5.xy) + vd_asfloat(v1.xy)));
  r6.xyzw = vd_asuint(_MainTex.Sample(sampler_MainTex, vd_asfloat(r0.yz)).xyzw);
  r5.z = vd_asuint(_MainTex_TexelSize.x);
  r0.yz = vd_asuint((vd_asfloat(r5.zw) + vd_asfloat(v1.xy)));
  r5.xyzw = vd_asuint(_MainTex.Sample(sampler_MainTex, vd_asfloat(r0.yz)).xyzw);
  r3.xyzw = vd_asuint(_MainTex.Sample(sampler_MainTex, vd_asfloat(r3.zw)).xyzw);
  r1.x = 0x00000000u;
  r1.y = vd_asuint(_MainTex_TexelSize.y);
  r0.yz = vd_asuint((vd_asfloat(r1.xy) + vd_asfloat(v1.xy)));
  r7.xyzw = vd_asuint(_MainTex.Sample(sampler_MainTex, vd_asfloat(r0.yz)).xyzw);
  r0.yz = vd_asuint((vd_asfloat(v1.xy) + vd_asfloat(_MainTex_TexelSize.xy)));
  r8.xyzw = vd_asuint(_MainTex.Sample(sampler_MainTex, vd_asfloat(r0.yz)).xyzw);
  r0.y = vd_asuint((vd_asfloat(r6.w) * 2.0 + vd_asfloat(r2.w)));
  r0.y = vd_asuint((vd_asfloat(r3.w) + vd_asfloat(r0.y)));
  r0.y = vd_asuint(((-(vd_asfloat(r4.w))) + vd_asfloat(r0.y)));
  r0.y = vd_asuint(((-(vd_asfloat(r5.w))) * 2.0 + vd_asfloat(r0.y)));
  r1.x = vd_asuint(((-(vd_asfloat(r8.w))) + vd_asfloat(r0.y)));
  r0.y = vd_asuint((vd_asfloat(r1.w) * 2.0 + vd_asfloat(r2.w)));
  r0.y = vd_asuint((vd_asfloat(r4.w) + vd_asfloat(r0.y)));
  r0.y = vd_asuint(((-(vd_asfloat(r3.w))) + vd_asfloat(r0.y)));
  r0.y = vd_asuint(((-(vd_asfloat(r7.w))) * 2.0 + vd_asfloat(r0.y)));
  r1.y = vd_asuint(((-(vd_asfloat(r8.w))) + vd_asfloat(r0.y)));
  r0.yz = vd_asuint((-(int2)(float2(0.00499999989, 0.00499999989) >= abs(vd_asfloat(r1.xy)))));
  r0.y = (r0.z & r0.y);
  if (r0.y != 0u) {
    o0.xy = vd_asfloat(vd_asuint(v1.xy));
    return;
  }
  r0.y = vd_asuint(dot(vd_asfloat(r1.xy), vd_asfloat(r1.xy)));
  r0.y = vd_asuint(rsqrt(vd_asfloat(r0.y)));
  r0.yz = vd_asuint((vd_asfloat(r0.yy) * vd_asfloat(r1.xy)));
  r0.x = vd_asuint(((-(vd_asfloat(r0.x))) + 1.0));
  r0.xy = vd_asuint((vd_asfloat(r0.xx) * vd_asfloat(r0.yz)));
  o0.xy = (vd_asfloat(_MainTex_TexelSize.xy) * vd_asfloat(r0.xy) + vd_asfloat(v1.xy));
  return;
}
