// Hidden/ChocDino/UIFX/GaussianBlur-Reference | pass 1 "BlurVertical" | progFragment | keywords: (none) | blob 4
void frag(
  float4 v0 : SV_POSITION0,
  float4 v1 : VDREG1,
  out float4 o0 : SV_Target0)
{
  uint4 r0, r1, r2, r3, r4, r5;
  r0 = 0u; r1 = 0u; r2 = 0u; r3 = 0u; r4 = 0u; r5 = 0u;

  r0.xyzw = vd_asuint(_MainTex.Sample(sampler_MainTex, vd_asfloat(v1.xy)).xyzw);
  r0.xyzw = vd_asuint((vd_asfloat(r0.xyzw) * vd_asfloat((_Weights[0]).xxxx)));
  r1.x = 0x00000000u;
  r1.y = vd_asuint(_MainTex_TexelSize.y);
  r2.xyzw = r0.xyzw;
  r3.x = 0x00000000u;
  r3.y = vd_asuint(_MainTex_TexelSize.y);
  r1.z = 0x00000001u;
  while (true) {
    r1.w = vd_asuint((-(int)(vd_asint(r1.z) >= vd_asint(_KernelRadius))));
    if (r1.w != 0u) break;
    r3.zw = vd_asuint((vd_asfloat(r3.xy) + vd_asfloat(v1.xy)));
    r4.xyzw = vd_asuint(_MainTex.Sample(sampler_MainTex, vd_asfloat(r3.zw)).xyzw);
    r4.xyzw = vd_asuint((vd_asfloat(r4.xyzw) * vd_asfloat((_Weights[(int)(r1.z)]).xxxx) + vd_asfloat(r2.xyzw)));
    r3.zw = vd_asuint(((-(vd_asfloat(r3.xy))) + vd_asfloat(v1.xy)));
    r5.xyzw = vd_asuint(_MainTex.Sample(sampler_MainTex, vd_asfloat(r3.zw)).xyzw);
    r2.xyzw = vd_asuint((vd_asfloat(r5.xyzw) * vd_asfloat((_Weights[(int)(r1.z)]).xxxx) + vd_asfloat(r4.xyzw)));
    r3.xy = vd_asuint((vd_asfloat(r1.xy) + vd_asfloat(r3.xy)));
    r1.z = vd_asuint((vd_asint(r1.z) + 1));
  }
  o0.xyzw = vd_asfloat(r2.xyzw);
  return;
}
