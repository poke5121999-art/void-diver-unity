// ArtTeam/VFX/VFX_Master_typeB_Xpan | pass 0 "Universal Forward" | progFragment | keywords: FOG_LINEAR | blob 12
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
  uint4 r0, r1;
  r0 = 0u; r1 = 0u;

  r0.xy = vd_asuint((float2(1.0, 1.0) / vd_asfloat(_ScreenParams.xy)));
  r0.xy = vd_asuint((vd_asfloat(r0.xy) * vd_asfloat((_Pixelation).xx)));
  r0.zw = vd_asuint((vd_asfloat(v1.xy) / vd_asfloat(r0.xy)));
  r0.zw = vd_asuint(floor(vd_asfloat(r0.zw)));
  r1.xz = vd_asuint(v2.zw);
  r1.yw = uint2(0x00000000u, 0x3f800000u);
  r0.xy = vd_asuint((vd_asfloat(r0.zw) * vd_asfloat(r0.xy) + vd_asfloat(r1.xy)));
  r0.xy = vd_asuint((vd_asfloat(r1.zw) * vd_asfloat(r0.xy)));
  r0.xy = vd_asuint((vd_asfloat(r0.xy) * vd_asfloat(_MainTex_ST.xy) + vd_asfloat(_MainTex_ST.zw)));
  r0.xyzw = vd_asuint(_MainTex.SampleBias(sampler_MainTex, vd_asfloat(r0.xy), vd_asfloat(_GlobalMipBias.x)).xyzw);
  r0.y = vd_asuint(((-(vd_asfloat(r0.z))) + 1.0));
  r0.y = vd_asuint(((-(vd_asfloat(r0.z))) + vd_asfloat(r0.y)));
  r0.w = vd_asuint(saturate(vd_asfloat(_InvertB)));
  r0.y = vd_asuint((vd_asfloat(r0.w) * vd_asfloat(r0.y) + vd_asfloat(r0.z)));
  r0.y = vd_asuint((vd_asfloat(r0.y) * vd_asfloat(v3.w)));
  r1.xyz = vd_asuint((vd_asfloat(r0.xxx) * vd_asfloat(v4.xyz)));
  r0.xzw = vd_asuint(((-(vd_asfloat(v4.xyz))) * vd_asfloat(r0.xxx) + vd_asfloat(v3.xyz)));
  r0.xyz = vd_asuint((vd_asfloat(r0.yyy) * vd_asfloat(r0.xzw) + vd_asfloat(r1.xyz)));
  o0.xyz = (vd_asfloat(r0.xyz) * vd_asfloat(v2.yyy));
  o0.w = vd_asfloat(0x3f800000u);
  return;
}
