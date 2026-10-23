// Hidden/ChocDino/UIFX/Blur-Directional | pass 0 "Blur-Directional" | progFragment | keywords: USE_DITHER | blob 9
void frag(
  float4 v0 : SV_POSITION0,
  float4 v1 : VDREG1,
  out float4 o0 : SV_Target0)
{
  uint4 r0, r1, r2, r3, r4;
  r0 = 0u; r1 = 0u; r2 = 0u; r3 = 0u; r4 = 0u;

  r0.xyzw = vd_asuint(_MainTex.Sample(sampler_MainTex, vd_asfloat(v1.xy)).xyzw);
  r1.x = vd_asuint(floor(vd_asfloat(_KernelRadius)));
  r1.y = vd_asuint(((int)vd_asfloat(r1.x)));
  r2.xyzw = r0.xyzw;
  r1.zw = vd_asuint(_TexelStep.xy);
  r3.x = 0x00000000u;
  while (true) {
    r3.y = vd_asuint((-(int)(vd_asint(r3.x) >= vd_asint(r1.y))));
    if (r3.y != 0u) break;
    r3.yz = vd_asuint((vd_asfloat(r1.zw) + vd_asfloat(v1.xy)));
    r4.xyzw = vd_asuint(_MainTex.Sample(sampler_MainTex, vd_asfloat(r3.yz)).xyzw);
    r2.xyzw = vd_asuint((vd_asfloat(r2.xyzw) + vd_asfloat(r4.xyzw)));
    r1.zw = vd_asuint((vd_asfloat(r1.zw) + vd_asfloat(_TexelStep.xy)));
    r3.x = vd_asuint((vd_asint(r3.x) + 1));
  }
  r0.x = vd_asuint(((-(vd_asfloat(r1.x))) + vd_asfloat(_KernelRadius)));
  r0.y = vd_asuint((-(int)(0.0 < vd_asfloat(r0.x))));
  if (r0.y != 0u) {
    r0.yz = vd_asuint((vd_asfloat(r0.xx) * vd_asfloat(_TexelStep.xy)));
    r0.yz = vd_asuint((vd_asfloat(_TexelStep.xy) * vd_asfloat(r1.xx) + vd_asfloat(r0.yz)));
    r0.yz = vd_asuint((vd_asfloat(r0.yz) + vd_asfloat(v1.xy)));
    r1.xyzw = vd_asuint(_MainTex.Sample(sampler_MainTex, vd_asfloat(r0.yz)).xyzw);
    r2.xyzw = vd_asuint((vd_asfloat(r1.xyzw) * vd_asfloat(r0.xxxx) + vd_asfloat(r2.xyzw)));
  }
  r0.x = vd_asuint((vd_asfloat(_KernelRadius) + 1.0));
  r0.xyzw = vd_asuint((vd_asfloat(r2.xyzw) / vd_asfloat(r0.xxxx)));
  r1.xy = vd_asuint((vd_asfloat(v1.xy) * float2(264.554596, 264.554596)));
  r1.z = vd_asuint((vd_asfloat(_Time.x) * 0.103100002));
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
