// ArtTeam/VFX/VFX_Master_typeB_Xpan | pass 0 "Universal Forward" | progFragment | keywords: FOG_LINEAR _ALPHATEST_ON _DBUFFER_MRT3 _SURFACE_TYPE_TRANSPARENT | blob 23
void frag(
  float4 v0 : SV_POSITION0,
  float4 v1 : VDREG1,
  float4 v2 : VDREG2,
  float4 v3 : VDREG3,
  float4 v4 : VDREG4,
  float4 v5 : VDREG5,
  float4 v6 : VDREG6,
  out float4 o0 : SV_Target0)
{
  uint4 r0, r1, r2;
  r0 = 0u; r1 = 0u; r2 = 0u;

  r0.x = vd_asuint(saturate(vd_asfloat(v2.x)));
  r0.y = vd_asuint(saturate(vd_asfloat(_DissolveSharpness)));
  r0.z = vd_asuint(((-(vd_asfloat(r0.y))) * 0.5 + 1.0));
  r0.w = vd_asuint((vd_asfloat(r0.y) * 0.5 + -1.0));
  r1.x = vd_asuint(((-(vd_asfloat(r0.w))) + vd_asfloat(r0.z)));
  r0.z = vd_asuint(((-(vd_asfloat(r0.y))) * 0.5 + vd_asfloat(r0.z)));
  r0.z = vd_asuint((1.0 / vd_asfloat(r0.z)));
  r0.x = vd_asuint((vd_asfloat(r0.x) * vd_asfloat(r1.x) + vd_asfloat(r0.w)));
  r1.xy = vd_asuint((float2(1.0, 1.0) / vd_asfloat(_ScreenParams.xy)));
  r1.xy = vd_asuint((vd_asfloat(r1.xy) * vd_asfloat((_Pixelation).xx)));
  r1.zw = vd_asuint((vd_asfloat(v1.xy) / vd_asfloat(r1.xy)));
  r1.zw = vd_asuint(floor(vd_asfloat(r1.zw)));
  r2.xz = vd_asuint(v2.zw);
  r2.yw = uint2(0x00000000u, 0x3f800000u);
  r1.xy = vd_asuint((vd_asfloat(r1.zw) * vd_asfloat(r1.xy) + vd_asfloat(r2.xy)));
  r1.xy = vd_asuint((vd_asfloat(r2.zw) * vd_asfloat(r1.xy)));
  r1.xy = vd_asuint((vd_asfloat(r1.xy) * vd_asfloat(_MainTex_ST.xy) + vd_asfloat(_MainTex_ST.zw)));
  r1.xyzw = vd_asuint(_MainTex.SampleBias(sampler_MainTex, vd_asfloat(r1.xy), vd_asfloat(_GlobalMipBias.x)).xyzw);
  r2.xy = vd_asuint(((-(vd_asfloat(r1.zy))) + float2(1.0, 1.0)));
  r0.x = vd_asuint((vd_asfloat(r0.x) + vd_asfloat(r2.y)));
  r0.w = vd_asuint(((-(vd_asfloat(r1.z))) + vd_asfloat(r2.x)));
  r0.x = vd_asuint(((-(vd_asfloat(r0.y))) * 0.5 + vd_asfloat(r0.x)));
  r0.x = vd_asuint(saturate((vd_asfloat(r0.z) * vd_asfloat(r0.x))));
  r0.y = vd_asuint((vd_asfloat(r0.x) * -2.0 + 3.0));
  r0.x = vd_asuint((vd_asfloat(r0.x) * vd_asfloat(r0.x)));
  r0.x = vd_asuint(saturate(((-(vd_asfloat(r0.y))) * vd_asfloat(r0.x) + vd_asfloat(r1.w))));
  r0.x = vd_asuint((vd_asfloat(r0.x) * vd_asfloat(v4.w)));
  r0.yz = vd_asuint((vd_asfloat(v1.xy) * vd_asfloat(_MaskTex_ST.xy) + vd_asfloat(_MaskTex_ST.zw)));
  r2.xyzw = vd_asuint(_MaskTex.SampleBias(sampler_MaskTex, vd_asfloat(r0.yz), vd_asfloat(_GlobalMipBias.x)).xyzw);
  r0.y = vd_asuint((vd_asfloat(r2.w) * vd_asfloat(r2.x)));
  r0.z = vd_asuint((vd_asfloat(r0.x) * vd_asfloat(r0.y) + -0.5));
  r0.x = vd_asuint((vd_asfloat(r0.y) * vd_asfloat(r0.x)));
  r0.y = vd_asuint(ddx(vd_asfloat(r0.x)));
  r1.y = vd_asuint(ddy(vd_asfloat(r0.x)));
  r0.y = vd_asuint((abs(vd_asfloat(r0.y)) + abs(vd_asfloat(r1.y))));
  r1.y = vd_asuint(((-(vd_asfloat(r0.y))) * 0.5 + vd_asfloat(r0.z)));
  r0.y = vd_asuint(max(vd_asfloat(r0.y), 9.99999975e-05));
  r0.y = vd_asuint((vd_asfloat(r1.y) / vd_asfloat(r0.y)));
  r0.y = vd_asuint(saturate((vd_asfloat(r0.y) + 1.0)));
  r1.y = vd_asuint((vd_asfloat(r0.y) + -9.99999975e-05));
  r1.w = vd_asuint((-(int)(vd_asfloat(_AlphaToMaskAvailable) != 0.0)));
  r0.z = ((r1.w != 0u) ? r1.y : r0.z);
  o0.w = vd_asfloat(((r1.w != 0u) ? r0.y : r0.x));
  r0.x = vd_asuint((-(int)(vd_asfloat(r0.z) < 0.0)));
  if (r0.x != 0u) discard;
  r0.x = vd_asuint(saturate(vd_asfloat(_InvertB)));
  r0.x = vd_asuint((vd_asfloat(r0.x) * vd_asfloat(r0.w) + vd_asfloat(r1.z)));
  r0.x = vd_asuint((vd_asfloat(r0.x) * vd_asfloat(v3.w)));
  r0.yzw = vd_asuint((vd_asfloat(r1.xxx) * vd_asfloat(v4.xyz)));
  r1.xyz = vd_asuint(((-(vd_asfloat(v4.xyz))) * vd_asfloat(r1.xxx) + vd_asfloat(v3.xyz)));
  r0.xyz = vd_asuint((vd_asfloat(r0.xxx) * vd_asfloat(r1.xyz) + vd_asfloat(r0.yzw)));
  o0.xyz = (vd_asfloat(r0.xyz) * vd_asfloat(v2.yyy));
  return;
}
