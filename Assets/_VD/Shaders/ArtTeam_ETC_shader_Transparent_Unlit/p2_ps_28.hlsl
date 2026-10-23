// ArtTeam/ETC/shader_Transparent_Unlit | pass 2 "MotionVectors" | progFragment | keywords: _ALPHATEST_ON | blob 28
void frag(
  float4 v0 : VDREG0,
  float4 v1 : VDREG1,
  float4 v2 : SV_POSITION0,
  float4 v3 : VDREG3,
  out float4 o0 : SV_Target0)
{
  uint4 r0;
  r0 = 0u;

  r0.xy = vd_asuint((vd_asfloat(v3.xy) * vd_asfloat(_Albedo_ST.xy) + vd_asfloat(_Albedo_ST.zw)));
  r0.xyzw = vd_asuint(_Albedo.SampleBias(sampler_Albedo, vd_asfloat(r0.xy), vd_asfloat(_GlobalMipBias.x)).xyzw);
  r0.x = vd_asuint((vd_asfloat(r0.w) + (-(vd_asfloat(_AlphaClipThreshold)))));
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
