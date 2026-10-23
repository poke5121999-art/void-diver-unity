// Hidden/ChocDino/UIFX/Extrude | pass 0 "Extrude-Perspective" | progFragment | keywords: MULTIPLY_SOURCE_COLOR USE_GRADIENT_TEXTURE | blob 10
void frag(
  float4 v0 : SV_POSITION0,
  float4 v1 : VDREG1,
  float4 v2 : VDREG2,
  float4 v3 : VDREG3,
  out float4 o0 : SV_Target0)
{
  uint4 r0, r1, r2, r3, r4, r5;
  r0 = 0u; r1 = 0u; r2 = 0u; r3 = 0u; r4 = 0u; r5 = 0u;

  r0.x = vd_asuint(max(vd_asint(_Length), 1));
  r0.yz = vd_asuint((vd_asfloat(v1.xy) + float2(-0.5, -0.5)));
  r1.xy = vd_asuint((vd_asfloat(_VanishingPoint.xy) + float2(-0.5, -0.5)));
  r1.xy = vd_asuint((vd_asfloat(r1.xy) * vd_asfloat(_MainTex_TexelSize.xy)));
  r2.x = vd_asuint((vd_asfloat(_MainTex_TexelSize.z) / vd_asfloat(_MainTex_TexelSize.w)));
  r0.yz = vd_asuint((vd_asfloat(r0.yz) * vd_asfloat(_MainTex_TexelSize.xy) + (-(vd_asfloat(r1.xy)))));
  r2.yw = uint2(0x3f800000u, 0x00000000u);
  r0.yz = vd_asuint((vd_asfloat(r0.yz) * vd_asfloat(r2.xy)));
  r1.xy = vd_asuint((vd_asfloat(r0.yz) * vd_asfloat(_MainTex_TexelSize.zw)));
  r0.w = vd_asuint(dot(vd_asfloat(r1.xy), vd_asfloat(r1.xy)));
  r0.w = vd_asuint(rsqrt(vd_asfloat(r0.w)));
  r1.xy = vd_asuint((vd_asfloat(r0.ww) * vd_asfloat(r1.xy)));
  r1.xy = vd_asuint((vd_asfloat(r1.xy) * vd_asfloat(_MainTex_TexelSize.xy)));
  r1.zw = vd_asuint((vd_asfloat(r1.xy) * float2(0.5, 0.5)));
  r0.y = vd_asuint(dot(vd_asfloat(r0.yz), vd_asfloat(r0.yz)));
  r0.z = vd_asuint(dot(vd_asfloat(r1.zw), vd_asfloat(r1.zw)));
  r0.yz = vd_asuint(sqrt(vd_asfloat(r0.yz)));
  r0.y = vd_asuint((vd_asfloat(r0.y) / vd_asfloat(r0.z)));
  r0.x = vd_asuint(((float)vd_asint(r0.x)));
  r0.x = vd_asuint((vd_asfloat(r0.x) * vd_asfloat(_Ratio.z)));
  r0.x = vd_asuint((vd_asfloat(r0.y) * vd_asfloat(r0.x)));
  r0.x = vd_asuint(min(vd_asfloat(r0.x), 1024.0));
  r3.xyzw = uint4(0x00000000u, 0x00000000u, 0x00000000u, 0x00000000u);
  r0.yz = vd_asuint(v1.xy);
  r0.w = 0x00000000u;
  r1.zw = uint2(0x00000000u, 0x00000001u);
  while (true) {
    r2.x = vd_asuint(((float)vd_asint(r1.w)));
    r2.x = vd_asuint((-(int)(vd_asfloat(r0.x) < vd_asfloat(r2.x))));
    if (r2.x != 0u) break;
    r2.xy = vd_asuint((-(int2)(float2(1.0, 1.0) < vd_asfloat(r0.zy))));
    r4.xy = vd_asuint((-(int2)(vd_asfloat(r0.zy) < float2(0.0, 0.0))));
    r2.x = (r2.x | r4.x);
    r2.x = (r2.y | r2.x);
    r2.x = (r4.y | r2.x);
    if (r2.x != 0u) {
      break;
    }
    r4.xyzw = vd_asuint(_MainTex.SampleLevel(sampler_MainTex, vd_asfloat(r0.yz), 0.0).xyzw);
    r2.x = vd_asuint(saturate((vd_asfloat(r1.z) / vd_asfloat(r0.x))));
    r2.x = vd_asuint(((-(vd_asfloat(r2.x))) + 1.0));
    r2.x = vd_asuint((vd_asfloat(r2.x) * vd_asfloat(r4.w)));
    r2.x = vd_asuint(max(vd_asfloat(r0.w), vd_asfloat(r2.x)));
    r5.xyzw = vd_asuint(max(vd_asfloat(r3.xyzw), vd_asfloat(r4.xyzw)));
    r2.y = vd_asuint((-(int)(vd_asfloat(r4.w) >= 1.0)));
    if (r2.y != 0u) {
      r3.xyzw = r5.xyzw;
      r0.w = r2.x;
      break;
    }
    r1.z = vd_asuint((vd_asfloat(r1.z) + 1.0));
    r0.yz = vd_asuint((vd_asfloat(r1.xy) * float2(0.5, 0.5) + vd_asfloat(r0.yz)));
    r3.xyzw = r5.xyzw;
    r0.w = r2.x;
    r1.w = vd_asuint((vd_asint(r1.w) + 1));
  }
  r0.x = vd_asuint(((-(vd_asfloat(r0.w))) + vd_asfloat(_ReverseFill)));
  r0.x = vd_asuint(((-(abs(vd_asfloat(r0.x)))) + vd_asfloat(_Scroll)));
  r0.x = vd_asuint((vd_asfloat(r0.x) + 1.0));
  r0.y = vd_asuint((vd_asfloat(r0.x) * 0.5));
  r0.y = vd_asuint(floor(vd_asfloat(r0.y)));
  r0.x = vd_asuint(((-(vd_asfloat(r0.y))) * 2.0 + vd_asfloat(r0.x)));
  r0.x = vd_asuint(max(vd_asfloat(r0.x), 0.0));
  r0.x = vd_asuint(min(vd_asfloat(r0.x), 2.0));
  r0.x = vd_asuint((vd_asfloat(r0.x) + -1.0));
  r2.z = vd_asuint(((-(abs(vd_asfloat(r0.x)))) + 1.0));
  r0.xyzw = vd_asuint(_GradientTex.Sample(sampler_GradientTex, vd_asfloat(r2.zw)).xyzw);
  r0.xyz = vd_asuint((vd_asfloat(r0.www) * vd_asfloat(r0.xyz)));
  o0.xyzw = (vd_asfloat(r3.xyzw) * vd_asfloat(r0.xyzw));
  return;
}
