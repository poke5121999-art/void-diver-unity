// ArtTeam/VFX/VFX_Master_typeC_3CD | pass 0 "Universal Forward" | progFragment | keywords: FOG_LINEAR _SURFACE_TYPE_TRANSPARENT | blob 8
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
  uint4 r0, r1, r2, r3;
  r0 = 0u; r1 = 0u; r2 = 0u; r3 = 0u;

  r0.x = vd_asuint(((-(vd_asfloat(v3.w))) + 1.0));
  r0.yzw = vd_asuint(saturate(vd_asfloat(v2.wyx)));
  r1.x = vd_asuint(((-(vd_asfloat(r0.y))) + 1.0));
  r0.y = vd_asuint(((-(vd_asfloat(r0.y))) + vd_asfloat(r1.x)));
  r0.y = vd_asuint((vd_asfloat(r0.y) + 2.0));
  r1.y = vd_asuint((vd_asfloat(r0.y) * vd_asfloat(r0.x)));
  r1.x = vd_asuint((vd_asfloat(r1.x) * -2.0 + vd_asfloat(r1.y)));
  r0.x = vd_asuint((vd_asfloat(r0.x) * vd_asfloat(r0.y) + (-(vd_asfloat(r1.x)))));
  r0.x = vd_asuint((1.0 / vd_asfloat(r0.x)));
  r1.yz = vd_asuint((vd_asfloat(v1.xy) * vd_asfloat(_MainTex_ST.xy) + vd_asfloat(_MainTex_ST.zw)));
  r2.xyzw = vd_asuint(_MainTex.SampleBias(sampler_MainTex, vd_asfloat(r1.yz), vd_asfloat(_GlobalMipBias.x)).xyzw);
  r0.y = vd_asuint(((-(vd_asfloat(r1.x))) + vd_asfloat(r2.z)));
  r0.x = vd_asuint(saturate((vd_asfloat(r0.x) * vd_asfloat(r0.y))));
  r0.y = vd_asuint((vd_asfloat(r0.x) * -2.0 + 3.0));
  r0.x = vd_asuint((vd_asfloat(r0.x) * vd_asfloat(r0.x)));
  r0.x = vd_asuint((vd_asfloat(r0.x) * vd_asfloat(r0.y)));
  r0.y = vd_asuint(((-(vd_asfloat(r2.w))) + vd_asfloat(r2.x)));
  r0.y = vd_asuint((vd_asfloat(v4.w) * vd_asfloat(r0.y) + vd_asfloat(r2.w)));
  r1.xyz = vd_asuint((vd_asfloat(r0.yyy) * vd_asfloat(v4.xyz)));
  r3.xyz = vd_asuint(((-(vd_asfloat(v4.xyz))) * vd_asfloat(r0.yyy) + vd_asfloat(v3.xyz)));
  r1.xyz = vd_asuint((vd_asfloat(r0.xxx) * vd_asfloat(r3.xyz) + vd_asfloat(r1.xyz)));
  o0.xyz = (vd_asfloat(r1.xyz) * vd_asfloat(v2.zzz));
  r0.x = vd_asuint(((-(vd_asfloat(r0.z))) * 0.5 + 1.0));
  r0.y = vd_asuint((vd_asfloat(r0.z) * 0.5 + -1.0));
  r1.x = vd_asuint(((-(vd_asfloat(r0.y))) + vd_asfloat(r0.x)));
  r0.x = vd_asuint(((-(vd_asfloat(r0.z))) * 0.5 + vd_asfloat(r0.x)));
  r0.x = vd_asuint((1.0 / vd_asfloat(r0.x)));
  r0.y = vd_asuint((vd_asfloat(r0.w) * vd_asfloat(r1.x) + vd_asfloat(r0.y)));
  r0.w = vd_asuint(((-(vd_asfloat(r2.y))) + 1.0));
  r0.y = vd_asuint((vd_asfloat(r0.y) + vd_asfloat(r0.w)));
  r0.y = vd_asuint(((-(vd_asfloat(r0.z))) * 0.5 + vd_asfloat(r0.y)));
  r0.x = vd_asuint(saturate((vd_asfloat(r0.x) * vd_asfloat(r0.y))));
  r0.y = vd_asuint((vd_asfloat(r0.x) * -2.0 + 3.0));
  r0.x = vd_asuint((vd_asfloat(r0.x) * vd_asfloat(r0.x)));
  o0.w = saturate(((-(vd_asfloat(r0.y))) * vd_asfloat(r0.x) + vd_asfloat(r2.w)));
  return;
}
