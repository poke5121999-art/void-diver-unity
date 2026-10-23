// Hidden/ChocDino/UIFX/Blur-Zoom | pass 0 "Blur-Zoom" | progFragment | keywords: DIR_BOTH | blob 8
void frag(
  float4 v0 : SV_POSITION0,
  float4 v1 : VDREG1,
  out float4 o0 : SV_Target0)
{
  uint4 r0, r1, r2, r3, r4, r5, r6;
  r0 = 0u; r1 = 0u; r2 = 0u; r3 = 0u; r4 = 0u; r5 = 0u; r6 = 0u;

  r0.xy = vd_asuint((vd_asfloat(v1.xy) + (-(vd_asfloat(_CenterInvScale.xy)))));
  r0.xy = vd_asuint((vd_asfloat(r0.xy) * vd_asfloat(_CenterInvScale.zz) + vd_asfloat(_CenterInvScale.xy)));
  r0.zw = vd_asuint((vd_asfloat(v1.xy) * vd_asfloat(_MainTex_TexelSize.zw)));
  r0.xy = vd_asuint((vd_asfloat(r0.xy) * vd_asfloat(_MainTex_TexelSize.zw) + (-(vd_asfloat(r0.zw)))));
  r0.z = vd_asuint(dot(vd_asfloat(r0.xy), vd_asfloat(r0.xy)));
  r0.w = vd_asuint(sqrt(vd_asfloat(r0.z)));
  r1.x = vd_asuint(floor(vd_asfloat(r0.w)));
  r1.y = vd_asuint(((int)vd_asfloat(r1.x)));
  r0.z = vd_asuint(rsqrt(vd_asfloat(r0.z)));
  r0.xy = vd_asuint((vd_asfloat(r0.zz) * vd_asfloat(r0.xy)));
  r1.zw = vd_asuint((vd_asfloat(r0.xy) * vd_asfloat(_MainTex_TexelSize.xy)));
  r2.xyzw = vd_asuint(_MainTex.SampleLevel(sampler_MainTex, vd_asfloat(v1.xy), 0.0).xyzw);
  r3.xyzw = r2.xyzw;
  r4.xy = r1.zw;
  r0.z = 0x00000000u;
  while (true) {
    r4.z = vd_asuint((-(int)(vd_asint(r0.z) >= vd_asint(r1.y))));
    if (r4.z != 0u) break;
    r4.zw = vd_asuint((vd_asfloat(r4.xy) + vd_asfloat(v1.xy)));
    r5.xyzw = vd_asuint(_MainTex.SampleLevel(sampler_MainTex, vd_asfloat(r4.zw), 0.0).xyzw);
    r5.xyzw = vd_asuint((vd_asfloat(r3.xyzw) + vd_asfloat(r5.xyzw)));
    r4.zw = vd_asuint(((-(vd_asfloat(r4.xy))) + vd_asfloat(v1.xy)));
    r6.xyzw = vd_asuint(_MainTex.SampleLevel(sampler_MainTex, vd_asfloat(r4.zw), 0.0).xyzw);
    r3.xyzw = vd_asuint((vd_asfloat(r5.xyzw) + vd_asfloat(r6.xyzw)));
    r4.xy = vd_asuint((vd_asfloat(r0.xy) * vd_asfloat(_MainTex_TexelSize.xy) + vd_asfloat(r4.xy)));
    r0.z = vd_asuint((vd_asint(r0.z) + 1));
  }
  r0.x = vd_asuint((vd_asfloat(r0.w) + (-(vd_asfloat(r1.x)))));
  r0.yz = vd_asuint((vd_asfloat(r0.xx) * vd_asfloat(r1.zw)));
  r0.yz = vd_asuint((vd_asfloat(r1.zw) * vd_asfloat(r1.xx) + vd_asfloat(r0.yz)));
  r1.xy = vd_asuint((vd_asfloat(r0.yz) + vd_asfloat(v1.xy)));
  r1.xyzw = vd_asuint(_MainTex.SampleLevel(sampler_MainTex, vd_asfloat(r1.xy), 0.0).xyzw);
  r1.xyzw = vd_asuint((vd_asfloat(r1.xyzw) * vd_asfloat(r0.xxxx) + vd_asfloat(r3.xyzw)));
  r0.yz = vd_asuint(((-(vd_asfloat(r0.yz))) + vd_asfloat(v1.xy)));
  r2.xyzw = vd_asuint(_MainTex.SampleLevel(sampler_MainTex, vd_asfloat(r0.yz), 0.0).xyzw);
  r1.xyzw = vd_asuint((vd_asfloat(r2.xyzw) * vd_asfloat(r0.xxxx) + vd_asfloat(r1.xyzw)));
  r0.x = vd_asuint((vd_asfloat(r0.w) * 2.0 + 1.0));
  o0.xyzw = (vd_asfloat(r1.xyzw) / vd_asfloat(r0.xxxx));
  return;
}
