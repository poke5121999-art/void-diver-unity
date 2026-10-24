// ArtTeam/VFX/VFX_Master_typeB_Dither | pass 1 "DepthOnly" | progFragment | keywords: _ALPHATEST_ON _PLANESYSTEMCULLINGTYPE_ALWAYS | blob 35
void frag(
  float4 v0 : SV_POSITION0,
  float4 v1 : VDREG1,
  float4 v2 : VDREG2,
  float4 v3 : VDREG3,
  float4 v4 : VDREG4,
  out float4 o0 : SV_TARGET0)
{
  uint4 r0, r1;
  r0 = 0u; r1 = 0u;
  uint4 x0[16];

  x0[0].x = 0x3d70f0f1u;
  x0[1].x = 0x3f078788u;
  x0[2].x = 0x3e34b4b5u;
  x0[3].x = 0x3f25a5a6u;
  x0[4].x = 0x3f43c3c4u;
  x0[5].x = 0x3e969697u;
  x0[6].x = 0x3f61e1e2u;
  x0[7].x = 0x3ed2d2d3u;
  x0[8].x = 0x3e70f0f1u;
  x0[9].x = 0x3f34b4b5u;
  x0[10].x = 0x3df0f0f1u;
  x0[11].x = 0x3f169697u;
  x0[12].x = 0x3f70f0f1u;
  x0[13].x = 0x3ef0f0f1u;
  x0[14].x = 0x3f52d2d3u;
  x0[15].x = 0x3eb4b4b5u;
#if UNITY_UV_STARTS_AT_TOP
  r0.x = vd_asuint((-(int)(vd_asfloat(_ProjectionParams.x) < 0.0)));
#else
  r0.x = vd_asuint((-(int)(0.0 < vd_asfloat(_ProjectionParams.x))));
#endif
  r0.y = vd_asuint(((-(vd_asfloat(v0.y))) + vd_asfloat(_ScaledScreenParams.y)));
  r0.y = ((r0.x != 0u) ? r0.y : vd_asuint(v0.y));
  r0.x = vd_asuint(v0.x);
  r0.xy = vd_asuint((vd_asfloat(r0.xy) / vd_asfloat(_ScaledScreenParams.xy)));
  r0.z = vd_asuint(((-(vd_asfloat(r0.y))) + 1.0));
  r0.xy = vd_asuint((vd_asfloat(r0.xz) * vd_asfloat(_ScreenParams.xy)));
  r0.xy = ((uint2)vd_asfloat(r0.xy));
  r0.x = (r0.x << (uint)2);
  r0.xy = (r0.xy & uint2(0x0000000cu, 0x00000003u));
  r0.x = vd_asuint((vd_asint(r0.y) + vd_asint(r0.x)));
  r0.x = x0[r0.x + 0].x;
  r0.yz = vd_asuint((float2(1.0, 1.0) / vd_asfloat(_ScreenParams.xy)));
  r0.yz = vd_asuint((vd_asfloat(r0.yz) * vd_asfloat((_Pixelation).xx)));
  r1.xy = vd_asuint((vd_asfloat(v1.xy) / vd_asfloat(r0.yz)));
  r1.xy = vd_asuint(floor(vd_asfloat(r1.xy)));
  r0.yz = vd_asuint((vd_asfloat(r0.yz) * vd_asfloat(r1.xy)));
  r0.yz = vd_asuint((vd_asfloat(r0.yz) * vd_asfloat(_MainTex_ST.xy) + vd_asfloat(_MainTex_ST.zw)));
  r1.xyzw = vd_asuint(_MainTex.SampleBias(sampler_MainTex, vd_asfloat(r0.yz), vd_asfloat(_GlobalMipBias.x)).xyzw);
  r0.y = vd_asuint(((-(vd_asfloat(r1.y))) + 1.0));
  r0.zw = vd_asuint(saturate(vd_asfloat(v2.yx)));
  r1.x = vd_asuint(((-(vd_asfloat(r0.z))) * 0.5 + 1.0));
  r1.y = vd_asuint((vd_asfloat(r0.z) * 0.5 + -1.0));
  r1.z = vd_asuint(((-(vd_asfloat(r1.y))) + vd_asfloat(r1.x)));
  r1.x = vd_asuint(((-(vd_asfloat(r0.z))) * 0.5 + vd_asfloat(r1.x)));
  r1.x = vd_asuint((1.0 / vd_asfloat(r1.x)));
  r0.w = vd_asuint((vd_asfloat(r0.w) * vd_asfloat(r1.z) + vd_asfloat(r1.y)));
  r0.y = vd_asuint((vd_asfloat(r0.w) + vd_asfloat(r0.y)));
  r0.y = vd_asuint(((-(vd_asfloat(r0.z))) * 0.5 + vd_asfloat(r0.y)));
  r0.y = vd_asuint(saturate((vd_asfloat(r1.x) * vd_asfloat(r0.y))));
  r0.z = vd_asuint((vd_asfloat(r0.y) * -2.0 + 3.0));
  r0.y = vd_asuint((vd_asfloat(r0.y) * vd_asfloat(r0.y)));
  r0.y = vd_asuint(saturate(((-(vd_asfloat(r0.z))) * vd_asfloat(r0.y) + vd_asfloat(r1.w))));
  r0.y = vd_asuint((vd_asfloat(r0.y) * vd_asfloat(v3.w)));
  r0.zw = vd_asuint((vd_asfloat(v1.xy) * vd_asfloat(_MaskTex_ST.xy) + vd_asfloat(_MaskTex_ST.zw)));
  r1.xyzw = vd_asuint(_MaskTex.SampleBias(sampler_MaskTex, vd_asfloat(r0.zw), vd_asfloat(_GlobalMipBias.x)).xyzw);
  r0.z = vd_asuint((vd_asfloat(r1.w) * vd_asfloat(r1.x)));
  r0.x = vd_asuint((vd_asfloat(r0.y) * vd_asfloat(r0.z) + (-(vd_asfloat(r0.x)))));
  r0.x = vd_asuint((vd_asfloat(r0.x) + (-(vd_asfloat(_AlphaClipThreshold)))));
  r0.x = vd_asuint((-(int)(vd_asfloat(r0.x) < 0.0)));
  if (r0.x != 0u) discard;
  o0.xyzw = vd_asfloat(vd_asuint(v0.zzzz));
  return;
}
