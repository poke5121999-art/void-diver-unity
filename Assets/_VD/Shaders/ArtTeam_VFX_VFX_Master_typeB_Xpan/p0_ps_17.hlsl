// ArtTeam/VFX/VFX_Master_typeB_Xpan | pass 0 "Universal Forward" | progFragment | keywords: _DBUFFER_MRT3 _SURFACE_TYPE_TRANSPARENT | blob 17
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

  r0.x = vd_asuint(saturate(vd_asfloat(_InvertB)));
  r0.yz = vd_asuint((float2(1.0, 1.0) / vd_asfloat(_ScreenParams.xy)));
  r0.yz = vd_asuint((vd_asfloat(r0.yz) * vd_asfloat((_Pixelation).xx)));
  r1.xy = vd_asuint((vd_asfloat(v1.xy) / vd_asfloat(r0.yz)));
  r1.xy = vd_asuint(floor(vd_asfloat(r1.xy)));
  r2.xz = vd_asuint(v2.zw);
  r2.yw = uint2(0x00000000u, 0x3f800000u);
  r0.yz = vd_asuint((vd_asfloat(r1.xy) * vd_asfloat(r0.yz) + vd_asfloat(r2.xy)));
  r0.yz = vd_asuint((vd_asfloat(r2.zw) * vd_asfloat(r0.yz)));
  r0.yz = vd_asuint((vd_asfloat(r0.yz) * vd_asfloat(_MainTex_ST.xy) + vd_asfloat(_MainTex_ST.zw)));
  r1.xyzw = vd_asuint(_MainTex.SampleBias(sampler_MainTex, vd_asfloat(r0.yz), vd_asfloat(_GlobalMipBias.x)).xyzw);
  r0.y = vd_asuint((vd_asfloat(r1.z) * -2.0 + 1.0));
  r0.x = vd_asuint((vd_asfloat(r0.x) * vd_asfloat(r0.y) + vd_asfloat(r1.z)));
  r0.x = vd_asuint((vd_asfloat(r0.x) * vd_asfloat(v3.w)));
  r0.yzw = vd_asuint((vd_asfloat(r1.xxx) * vd_asfloat(v4.xyz)));
  r2.xyz = vd_asuint(((-(vd_asfloat(v4.xyz))) * vd_asfloat(r1.xxx) + vd_asfloat(v3.xyz)));
  r0.xyz = vd_asuint((vd_asfloat(r0.xxx) * vd_asfloat(r2.xyz) + vd_asfloat(r0.yzw)));
  o0.xyz = (vd_asfloat(r0.xyz) * vd_asfloat(v2.yyy));
  r0.x = vd_asuint(((-(vd_asfloat(r1.y))) + 1.0));
  r0.y = vd_asuint(saturate(vd_asfloat(v2.x)));
  r0.z = vd_asuint(saturate(vd_asfloat(_DissolveSharpness)));
  r0.w = vd_asuint(((-(vd_asfloat(r0.z))) * 0.5 + 1.0));
  r1.x = vd_asuint((vd_asfloat(r0.z) * 0.5 + -1.0));
  r1.y = vd_asuint((vd_asfloat(r0.w) + (-(vd_asfloat(r1.x)))));
  r0.w = vd_asuint(((-(vd_asfloat(r0.z))) * 0.5 + vd_asfloat(r0.w)));
  r0.w = vd_asuint((1.0 / vd_asfloat(r0.w)));
  r0.y = vd_asuint((vd_asfloat(r0.y) * vd_asfloat(r1.y) + vd_asfloat(r1.x)));
  r0.x = vd_asuint((vd_asfloat(r0.y) + vd_asfloat(r0.x)));
  r0.x = vd_asuint(((-(vd_asfloat(r0.z))) * 0.5 + vd_asfloat(r0.x)));
  r0.x = vd_asuint(saturate((vd_asfloat(r0.w) * vd_asfloat(r0.x))));
  r0.y = vd_asuint((vd_asfloat(r0.x) * -2.0 + 3.0));
  r0.x = vd_asuint((vd_asfloat(r0.x) * vd_asfloat(r0.x)));
  r0.x = vd_asuint(saturate(((-(vd_asfloat(r0.y))) * vd_asfloat(r0.x) + vd_asfloat(r1.w))));
  r0.x = vd_asuint((vd_asfloat(r0.x) * vd_asfloat(v4.w)));
  r0.yz = vd_asuint((vd_asfloat(v1.xy) * vd_asfloat(_MaskTex_ST.xy) + vd_asfloat(_MaskTex_ST.zw)));
  r1.xyzw = vd_asuint(_MaskTex.SampleBias(sampler_MaskTex, vd_asfloat(r0.yz), vd_asfloat(_GlobalMipBias.x)).xyzw);
  r0.y = vd_asuint((vd_asfloat(r1.w) * vd_asfloat(r1.x)));
  o0.w = (vd_asfloat(r0.y) * vd_asfloat(r0.x));
  return;
}
