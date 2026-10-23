// ArtTeam/VFX/VFX_Master_typeB_Xpan | pass 2 "MotionVectors" | progFragment | keywords: _ALPHATEST_ON | blob 34
void frag(
  float4 v0 : VDREG0,
  float4 v1 : VDREG1,
  float4 v2 : SV_POSITION0,
  float4 v3 : VDREG3,
  float4 v4 : VDREG4,
  float4 v5 : VDREG5,
  out float4 o0 : SV_Target0)
{
  uint4 r0, r1;
  r0 = 0u; r1 = 0u;

  r0.xy = vd_asuint((float2(1.0, 1.0) / vd_asfloat(_ScreenParams.xy)));
  r0.xy = vd_asuint((vd_asfloat(r0.xy) * vd_asfloat((_Pixelation).xx)));
  r0.zw = vd_asuint((vd_asfloat(v3.xy) / vd_asfloat(r0.xy)));
  r0.zw = vd_asuint(floor(vd_asfloat(r0.zw)));
  r1.xz = vd_asuint(v4.zw);
  r1.yw = uint2(0x00000000u, 0x3f800000u);
  r0.xy = vd_asuint((vd_asfloat(r0.zw) * vd_asfloat(r0.xy) + vd_asfloat(r1.xy)));
  r0.xy = vd_asuint((vd_asfloat(r1.zw) * vd_asfloat(r0.xy)));
  r0.xy = vd_asuint((vd_asfloat(r0.xy) * vd_asfloat(_MainTex_ST.xy) + vd_asfloat(_MainTex_ST.zw)));
  r0.xyzw = vd_asuint(_MainTex.SampleBias(sampler_MainTex, vd_asfloat(r0.xy), vd_asfloat(_GlobalMipBias.x)).xyzw);
  r0.x = vd_asuint(((-(vd_asfloat(r0.y))) + 1.0));
  r0.y = vd_asuint(saturate(vd_asfloat(v4.x)));
  r0.z = vd_asuint(saturate(vd_asfloat(_DissolveSharpness)));
  r1.x = vd_asuint(((-(vd_asfloat(r0.z))) * 0.5 + 1.0));
  r1.y = vd_asuint((vd_asfloat(r0.z) * 0.5 + -1.0));
  r1.z = vd_asuint(((-(vd_asfloat(r1.y))) + vd_asfloat(r1.x)));
  r1.x = vd_asuint(((-(vd_asfloat(r0.z))) * 0.5 + vd_asfloat(r1.x)));
  r1.x = vd_asuint((1.0 / vd_asfloat(r1.x)));
  r0.y = vd_asuint((vd_asfloat(r0.y) * vd_asfloat(r1.z) + vd_asfloat(r1.y)));
  r0.x = vd_asuint((vd_asfloat(r0.y) + vd_asfloat(r0.x)));
  r0.x = vd_asuint(((-(vd_asfloat(r0.z))) * 0.5 + vd_asfloat(r0.x)));
  r0.x = vd_asuint(saturate((vd_asfloat(r1.x) * vd_asfloat(r0.x))));
  r0.y = vd_asuint((vd_asfloat(r0.x) * -2.0 + 3.0));
  r0.x = vd_asuint((vd_asfloat(r0.x) * vd_asfloat(r0.x)));
  r0.x = vd_asuint(saturate(((-(vd_asfloat(r0.y))) * vd_asfloat(r0.x) + vd_asfloat(r0.w))));
  r0.x = vd_asuint((vd_asfloat(r0.x) * vd_asfloat(v5.w)));
  r0.yz = vd_asuint((vd_asfloat(v3.xy) * vd_asfloat(_MaskTex_ST.xy) + vd_asfloat(_MaskTex_ST.zw)));
  r1.xyzw = vd_asuint(_MaskTex.SampleBias(sampler_MaskTex, vd_asfloat(r0.yz), vd_asfloat(_GlobalMipBias.x)).xyzw);
  r0.y = vd_asuint((vd_asfloat(r1.w) * vd_asfloat(r1.x)));
  r0.x = vd_asuint((vd_asfloat(r0.x) * vd_asfloat(r0.y) + -0.5));
  r0.x = vd_asuint((-(int)(vd_asfloat(r0.x) < 0.0)));
  if (r0.x != 0u) discard;
  r0.x = vd_asuint((1.0 / vd_asfloat(v1.z)));
  r0.xy = vd_asuint((vd_asfloat(r0.xx) * vd_asfloat(v1.xy)));
  r0.z = vd_asuint((1.0 / vd_asfloat(v0.z)));
  r0.xy = vd_asuint((vd_asfloat(v0.xy) * vd_asfloat(r0.zz) + (-(vd_asfloat(r0.xy)))));
  r0.xy = vd_asuint((vd_asfloat(r0.xy) * float2(0.5, -0.5)));
  r0.z = vd_asuint((-(int)(vd_asfloat(unity_MotionVectorsParams.y) != 0.0)));
  o0.xy = vd_asfloat((r0.xy & r0.zz));
  o0.zw = vd_asfloat(uint2(0x00000000u, 0x00000000u));
  return;
}
