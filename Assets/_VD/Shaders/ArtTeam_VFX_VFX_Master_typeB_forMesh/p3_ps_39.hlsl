// ArtTeam/VFX/VFX_Master_typeB_forMesh | pass 3 "DepthNormalsOnly" | progFragment | keywords: _ALPHATEST_ON | blob 39
void frag(
  float4 v0 : SV_POSITION0,
  float4 v1 : VDREG1,
  float4 v2 : VDREG2,
  out float4 o0 : SV_Target0)
{
  uint4 r0, r1;
  r0 = 0u; r1 = 0u;

  r0.xy = vd_asuint((vd_asfloat(v1.xy) * vd_asfloat(_MaskTex_G_Dissolve_B_2ndColor_A_alpha_ST.xy) + vd_asfloat(_MaskTex_G_Dissolve_B_2ndColor_A_alpha_ST.zw)));
  r0.xyzw = vd_asuint(_MaskTex_G_Dissolve_B_2ndColor_A_alpha.SampleBias(sampler_MaskTex_G_Dissolve_B_2ndColor_A_alpha, vd_asfloat(r0.xy), vd_asfloat(_GlobalMipBias.x)).xyzw);
  r0.x = vd_asuint(((-(vd_asfloat(r0.y))) + 1.0));
  r0.yz = vd_asuint(saturate(vd_asfloat(float2(_DissolveSharpness, _Dissolve))));
  r1.x = vd_asuint(((-(vd_asfloat(r0.y))) * 0.5 + 1.0));
  r1.y = vd_asuint((vd_asfloat(r0.y) * 0.5 + -1.0));
  r1.z = vd_asuint(((-(vd_asfloat(r1.y))) + vd_asfloat(r1.x)));
  r1.x = vd_asuint(((-(vd_asfloat(r0.y))) * 0.5 + vd_asfloat(r1.x)));
  r1.x = vd_asuint((1.0 / vd_asfloat(r1.x)));
  r0.z = vd_asuint((vd_asfloat(r0.z) * vd_asfloat(r1.z) + vd_asfloat(r1.y)));
  r0.x = vd_asuint((vd_asfloat(r0.z) + vd_asfloat(r0.x)));
  r0.x = vd_asuint(((-(vd_asfloat(r0.y))) * 0.5 + vd_asfloat(r0.x)));
  r0.x = vd_asuint(saturate((vd_asfloat(r1.x) * vd_asfloat(r0.x))));
  r0.y = vd_asuint((vd_asfloat(r0.x) * -2.0 + 3.0));
  r0.x = vd_asuint((vd_asfloat(r0.x) * vd_asfloat(r0.x)));
  r0.x = vd_asuint(saturate(((-(vd_asfloat(r0.y))) * vd_asfloat(r0.x) + vd_asfloat(r0.w))));
  r0.x = vd_asuint((vd_asfloat(_TintColor.w) * vd_asfloat(r0.x) + -0.5));
  r0.x = vd_asuint((-(int)(vd_asfloat(r0.x) < 0.0)));
  if (r0.x != 0u) discard;
  r0.x = vd_asuint(dot(vd_asfloat(v2.xyz), vd_asfloat(v2.xyz)));
  r0.x = vd_asuint(rsqrt(vd_asfloat(r0.x)));
  o0.xyz = (vd_asfloat(r0.xxx) * vd_asfloat(v2.xyz));
  o0.w = vd_asfloat(0x00000000u);
  return;
}
