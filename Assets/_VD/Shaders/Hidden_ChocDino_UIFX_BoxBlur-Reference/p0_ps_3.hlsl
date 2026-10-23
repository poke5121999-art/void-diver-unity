// Hidden/ChocDino/UIFX/BoxBlur-Reference | pass 0 "BlurHorizontal" | progFragment | keywords: (none) | blob 3
void frag(
  float4 v0 : SV_POSITION0,
  float4 v1 : VDREG1,
  out float4 o0 : SV_Target0)
{
  uint4 r0, r1, r2, r3, r4, r5, r6;
  r0 = 0u; r1 = 0u; r2 = 0u; r3 = 0u; r4 = 0u; r5 = 0u; r6 = 0u;

  r0.xyzw = vd_asuint(_MainTex.Sample(sampler_MainTex, vd_asfloat(v1.xy)).xyzw);
  r1.x = vd_asuint(floor(vd_asfloat(_KernelRadius)));
  r1.y = vd_asuint(((int)vd_asfloat(r1.x)));
  r2.x = vd_asuint(_MainTex_TexelSize.x);
  r2.y = 0x00000000u;
  r3.xyzw = r0.xyzw;
  r4.x = vd_asuint(_MainTex_TexelSize.x);
  r4.y = 0x00000000u;
  r1.z = 0x00000000u;
  while (true) {
    r1.w = vd_asuint((-(int)(vd_asint(r1.z) >= vd_asint(r1.y))));
    if (r1.w != 0u) break;
    r2.zw = vd_asuint((vd_asfloat(r4.xy) + vd_asfloat(v1.xy)));
    r5.xyzw = vd_asuint(_MainTex.Sample(sampler_MainTex, vd_asfloat(r2.zw)).xyzw);
    r5.xyzw = vd_asuint((vd_asfloat(r3.xyzw) + vd_asfloat(r5.xyzw)));
    r2.zw = vd_asuint(((-(vd_asfloat(r4.xy))) + vd_asfloat(v1.xy)));
    r6.xyzw = vd_asuint(_MainTex.Sample(sampler_MainTex, vd_asfloat(r2.zw)).xyzw);
    r3.xyzw = vd_asuint((vd_asfloat(r5.xyzw) + vd_asfloat(r6.xyzw)));
    r4.xy = vd_asuint((vd_asfloat(r2.xy) + vd_asfloat(r4.xy)));
    r1.z = vd_asuint((vd_asint(r1.z) + 1));
  }
  r0.x = vd_asuint(((-(vd_asfloat(r1.x))) + vd_asfloat(_KernelRadius)));
  r0.y = vd_asuint((-(int)(0.0 < vd_asfloat(r0.x))));
  if (r0.y != 0u) {
    r2.x = vd_asuint(_MainTex_TexelSize.x);
    r2.y = 0x00000000u;
    r0.yz = vd_asuint((vd_asfloat(r0.xx) * vd_asfloat(r2.xy)));
    r0.yz = vd_asuint((vd_asfloat(r2.xy) * vd_asfloat(r1.xx) + vd_asfloat(r0.yz)));
    r1.xy = vd_asuint((vd_asfloat(r0.yz) + vd_asfloat(v1.xy)));
    r1.xyzw = vd_asuint(_MainTex.Sample(sampler_MainTex, vd_asfloat(r1.xy)).xyzw);
    r1.xyzw = vd_asuint((vd_asfloat(r1.xyzw) * vd_asfloat(r0.xxxx) + vd_asfloat(r3.xyzw)));
    r0.yz = vd_asuint(((-(vd_asfloat(r0.yz))) + vd_asfloat(v1.xy)));
    r2.xyzw = vd_asuint(_MainTex.Sample(sampler_MainTex, vd_asfloat(r0.yz)).xyzw);
    r3.xyzw = vd_asuint((vd_asfloat(r2.xyzw) * vd_asfloat(r0.xxxx) + vd_asfloat(r1.xyzw)));
  }
  r0.x = vd_asuint((vd_asfloat(_KernelRadius) * 2.0 + 1.0));
  o0.xyzw = (vd_asfloat(r3.xyzw) / vd_asfloat(r0.xxxx));
  return;
}
