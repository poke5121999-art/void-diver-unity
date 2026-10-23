// Hidden/ChocDino/UIFX/Extrude | pass 1 "Extrude-Orthographic" | progFragment | keywords: MULTIPLY_SOURCE_COLOR USE_GRADIENT_TEXTURE | blob 14
void frag(
  float4 v0 : SV_POSITION0,
  float4 v1 : VDREG1,
  float4 v2 : VDREG2,
  float4 v3 : VDREG3,
  out float4 o0 : SV_Target0)
{
  uint4 r0, r1, r2, r3, r4;
  r0 = 0u; r1 = 0u; r2 = 0u; r3 = 0u; r4 = 0u;

  r0.x = vd_asuint(max(vd_asint(_Length), 1));
  r0.y = vd_asuint(((float)vd_asint(r0.x)));
  r1.xyzw = uint4(0x00000000u, 0x00000000u, 0x00000000u, 0x00000000u);
  r0.zw = vd_asuint(v1.xy);
  r2.xyz = uint3(0x00000000u, 0x00000000u, 0x00000001u);
  while (true) {
    r2.w = vd_asuint((-(int)(vd_asint(r0.x) < vd_asint(r2.z))));
    if (r2.w != 0u) break;
    r3.xy = vd_asuint((-(int2)(float2(1.0, 1.0) < vd_asfloat(r0.wz))));
    r3.zw = vd_asuint((-(int2)(vd_asfloat(r0.wz) < float2(0.0, 0.0))));
    r2.w = (r3.z | r3.x);
    r2.w = (r3.y | r2.w);
    r2.w = (r3.w | r2.w);
    if (r2.w != 0u) {
      break;
    }
    r3.xyzw = vd_asuint(_MainTex.SampleLevel(sampler_MainTex, vd_asfloat(r0.zw), 0.0).xyzw);
    r2.w = vd_asuint(saturate((vd_asfloat(r2.y) / vd_asfloat(r0.y))));
    r2.w = vd_asuint(((-(vd_asfloat(r2.w))) + 1.0));
    r2.w = vd_asuint((vd_asfloat(r2.w) * vd_asfloat(r3.w)));
    r2.w = vd_asuint(max(vd_asfloat(r2.w), vd_asfloat(r2.x)));
    r4.xyzw = vd_asuint(max(vd_asfloat(r1.xyzw), vd_asfloat(r3.xyzw)));
    r3.x = vd_asuint((-(int)(vd_asfloat(r3.w) >= 1.0)));
    if (r3.x != 0u) {
      r1.xyzw = r4.xyzw;
      r2.x = r2.w;
      break;
    }
    r2.y = vd_asuint((vd_asfloat(r2.y) + 1.0));
    r0.zw = vd_asuint((vd_asfloat(r0.zw) + vd_asfloat(_PixelStep.xy)));
    r1.xyzw = r4.xyzw;
    r2.xy = r2.wy;
    r2.z = vd_asuint((vd_asint(r2.z) + 1));
  }
  r0.x = vd_asuint(((-(vd_asfloat(r2.x))) + vd_asfloat(_ReverseFill)));
  r0.x = vd_asuint(((-(abs(vd_asfloat(r0.x)))) + vd_asfloat(_Scroll)));
  r0.x = vd_asuint((vd_asfloat(r0.x) + 1.0));
  r0.y = vd_asuint((vd_asfloat(r0.x) * 0.5));
  r0.y = vd_asuint(floor(vd_asfloat(r0.y)));
  r0.x = vd_asuint(((-(vd_asfloat(r0.y))) * 2.0 + vd_asfloat(r0.x)));
  r0.x = vd_asuint(max(vd_asfloat(r0.x), 0.0));
  r0.x = vd_asuint(min(vd_asfloat(r0.x), 2.0));
  r0.x = vd_asuint((vd_asfloat(r0.x) + -1.0));
  r0.x = vd_asuint(((-(abs(vd_asfloat(r0.x)))) + 1.0));
  r0.y = 0x00000000u;
  r0.xyzw = vd_asuint(_GradientTex.Sample(sampler_GradientTex, vd_asfloat(r0.xy)).xyzw);
  r0.xyz = vd_asuint((vd_asfloat(r0.www) * vd_asfloat(r0.xyz)));
  o0.xyzw = (vd_asfloat(r1.xyzw) * vd_asfloat(r0.xyzw));
  return;
}
