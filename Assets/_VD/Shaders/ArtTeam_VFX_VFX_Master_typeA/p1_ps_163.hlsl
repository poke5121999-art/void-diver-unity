// ArtTeam/VFX/VFX_Master_typeA | pass 1 "DepthOnly" | progFragment | keywords: _DEFORM_USE _MASK_USE _PLANESYSTEMCULLINGTYPE_ALWAYS _USE_SECONDARYCOLOR | blob 163
void frag(
  float4 v0 : SV_POSITION0,
  float4 v1 : VDREG1,
  float4 v2 : VDREG2,
  float4 v3 : VDREG3,
  float4 v4 : VDREG4,
  float4 v5 : VDREG5,
  float4 v6 : VDREG6,
  out float4 o0 : SV_TARGET0)
{
#if UNITY_UV_STARTS_AT_TOP
#else
  uint4 r0;
  r0 = 0u;
#endif

#if UNITY_UV_STARTS_AT_TOP
  o0.xyzw = vd_asfloat(vd_asuint(v0.zzzz));
#else
  r0.xyzw = vd_asuint(v0.xyzw);
  r0.z = vd_asuint(((-(vd_asfloat(v0.z))) + 1.0));
  o0.xyzw = vd_asfloat(r0.zzzz);
#endif
  return;
}
