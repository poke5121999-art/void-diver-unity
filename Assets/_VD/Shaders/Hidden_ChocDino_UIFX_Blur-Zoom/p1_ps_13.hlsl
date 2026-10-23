// Hidden/ChocDino/UIFX/Blur-Zoom | pass 1 "Blur-Zoom-Weighted" | progFragment | keywords: USE_DITHER | blob 13
void frag(
  float4 v0 : SV_POSITION0,
  float4 v1 : VDREG1,
  out float4 o0 : SV_Target0)
{
  uint4 r0, r1, r2, r3, r4, r5;
  r0 = 0u; r1 = 0u; r2 = 0u; r3 = 0u; r4 = 0u; r5 = 0u;

  r0.xy = vd_asuint((vd_asfloat(v1.xy) + (-(vd_asfloat(_CenterInvScale.xy)))));
  r0.xy = vd_asuint((vd_asfloat(r0.xy) * vd_asfloat(_CenterInvScale.zz) + vd_asfloat(_CenterInvScale.xy)));
  r0.zw = vd_asuint((vd_asfloat(v1.xy) * vd_asfloat(_MainTex_TexelSize.zw)));
  r0.xy = vd_asuint((vd_asfloat(r0.xy) * vd_asfloat(_MainTex_TexelSize.zw) + (-(vd_asfloat(r0.zw)))));
  r0.z = vd_asuint(dot(vd_asfloat(r0.xy), vd_asfloat(r0.xy)));
  r0.w = vd_asuint(sqrt(vd_asfloat(r0.z)));
  r0.w = vd_asuint(floor(vd_asfloat(r0.w)));
  r1.x = vd_asuint(((int)vd_asfloat(r0.w)));
  r0.z = vd_asuint(rsqrt(vd_asfloat(r0.z)));
  r0.xy = vd_asuint((vd_asfloat(r0.zz) * vd_asfloat(r0.xy)));
  r1.yz = vd_asuint((vd_asfloat(r0.xy) * vd_asfloat(_MainTex_TexelSize.xy)));
  r2.xyzw = vd_asuint(_MainTex.SampleLevel(sampler_MainTex, vd_asfloat(v1.xy), 0.0).xyzw);
  r3.xyzw = r2.xyzw;
  r4.xy = r1.yz;
  r0.z = 0x3f800000u;
  r1.w = 0x00000001u;
  while (true) {
    r4.z = vd_asuint((-(int)(vd_asint(r1.w) >= vd_asint(r1.x))));
    if (r4.z != 0u) break;
    r4.z = vd_asuint(((float)vd_asint(r1.w)));
    r4.z = vd_asuint((vd_asfloat(r4.z) / vd_asfloat(r0.w)));
    r4.z = vd_asuint(((-(vd_asfloat(r4.z))) + 1.0));
    r4.z = vd_asuint(log2(vd_asfloat(r4.z)));
    r4.z = vd_asuint((vd_asfloat(r4.z) * vd_asfloat(_WeightsPower)));
    r4.z = vd_asuint(exp2(vd_asfloat(r4.z)));
    r5.xy = vd_asuint((vd_asfloat(r4.xy) + vd_asfloat(v1.xy)));
    r5.xyzw = vd_asuint(_MainTex.SampleLevel(sampler_MainTex, vd_asfloat(r5.xy), 0.0).xyzw);
    r3.xyzw = vd_asuint((vd_asfloat(r5.xyzw) * vd_asfloat(r4.zzzz) + vd_asfloat(r3.xyzw)));
    r0.z = vd_asuint((vd_asfloat(r0.z) + vd_asfloat(r4.z)));
    r4.xy = vd_asuint((vd_asfloat(r0.xy) * vd_asfloat(_MainTex_TexelSize.xy) + vd_asfloat(r4.xy)));
    r1.w = vd_asuint((vd_asint(r1.w) + 1));
  }
  r0.xyzw = vd_asuint((vd_asfloat(r3.xyzw) / vd_asfloat(r0.zzzz)));
  r1.xy = vd_asuint((vd_asfloat(v1.xy) * float2(264.554596, 264.554596)));
  r1.z = vd_asuint((vd_asfloat(_Time.w) * 0.103100002));
  r1.xyz = vd_asuint(frac(vd_asfloat(r1.xyz)));
  r2.xyz = vd_asuint((vd_asfloat(r1.yzx) + float3(19.1900005, 19.1900005, 19.1900005)));
  r1.w = vd_asuint(dot(vd_asfloat(r1.xyz), vd_asfloat(r2.xyz)));
  r1.xyz = vd_asuint((vd_asfloat(r1.www) + vd_asfloat(r1.xyz)));
  r1.x = vd_asuint((vd_asfloat(r1.y) + vd_asfloat(r1.x)));
  r1.x = vd_asuint((vd_asfloat(r1.z) * vd_asfloat(r1.x)));
  r1.x = vd_asuint(frac(vd_asfloat(r1.x)));
  r1.x = vd_asuint((vd_asfloat(r1.x) + -0.5));
  r1.x = vd_asuint((vd_asfloat(r1.x) * vd_asfloat(_Dither)));
  r1.yzw = vd_asuint(log2(vd_asfloat(r0.xyz)));
  r1.yzw = vd_asuint((vd_asfloat(r1.yzw) * float3(0.454545468, 0.454545468, 0.454545468)));
  r1.yzw = vd_asuint(exp2(vd_asfloat(r1.yzw)));
  r1.y = vd_asuint((vd_asfloat(r1.z) + vd_asfloat(r1.y)));
  r1.y = vd_asuint((vd_asfloat(r1.w) + vd_asfloat(r1.y)));
  r1.y = vd_asuint(min(vd_asfloat(r1.y), 1.0));
  r1.y = vd_asuint((vd_asfloat(r1.y) * 0.333333343));
  r1.z = vd_asuint((vd_asfloat(r0.w) * vd_asfloat(r1.y)));
  r1.z = vd_asuint(log2(vd_asfloat(r1.z)));
  r1.z = vd_asuint((vd_asfloat(r1.z) * 0.454545468));
  r1.z = vd_asuint(exp2(vd_asfloat(r1.z)));
  r1.z = vd_asuint(((-(vd_asfloat(r1.z))) + 1.0));
  r1.y = vd_asuint((vd_asfloat(r1.y) * vd_asfloat(r1.z)));
  o0.xyzw = (vd_asfloat(r1.xxxx) * vd_asfloat(r1.yyyy) + vd_asfloat(r0.xyzw));
  return;
}
