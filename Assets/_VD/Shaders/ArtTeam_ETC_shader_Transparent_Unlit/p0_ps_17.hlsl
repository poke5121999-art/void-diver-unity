// ArtTeam/ETC/shader_Transparent_Unlit | pass 0 "Universal Forward" | progFragment | keywords: FOG_LINEAR _ALPHATEST_ON _DBUFFER_MRT3 _SURFACE_TYPE_TRANSPARENT | blob 17
void frag(
  float4 v0 : SV_POSITION0,
  float4 v1 : VDREG1,
  float4 v2 : VDREG2,
  float4 v3 : VDREG3,
  out float4 o0 : SV_Target0)
{
  uint4 r0, r1, r2;
  r0 = 0u; r1 = 0u; r2 = 0u;

  r0.xy = vd_asuint((vd_asfloat(v1.xy) * vd_asfloat(_Albedo_ST.xy) + vd_asfloat(_Albedo_ST.zw)));
  r0.xyzw = vd_asuint(_Albedo.SampleBias(sampler_Albedo, vd_asfloat(r0.xy), vd_asfloat(_GlobalMipBias.x)).xyzw);
  r1.x = vd_asuint(ddx(vd_asfloat(r0.w)));
  r1.y = vd_asuint(ddy(vd_asfloat(r0.w)));
  r1.x = vd_asuint((abs(vd_asfloat(r1.y)) + abs(vd_asfloat(r1.x))));
  r1.y = vd_asuint((vd_asfloat(r0.w) + (-(vd_asfloat(_AlphaClipThreshold)))));
  r1.z = vd_asuint(((-(vd_asfloat(r1.x))) * 0.5 + vd_asfloat(r1.y)));
  r1.x = vd_asuint(max(vd_asfloat(r1.x), 9.99999975e-05));
  r1.x = vd_asuint((vd_asfloat(r1.z) / vd_asfloat(r1.x)));
  r1.x = vd_asuint(saturate((vd_asfloat(r1.x) + 1.0)));
  r1.z = vd_asuint((-(int)(0.0 >= vd_asfloat(_AlphaClipThreshold))));
  r1.x = ((r1.z != 0u) ? 0x3f800000u : r1.x);
  r1.z = (~r1.z);
  r1.w = vd_asuint((vd_asfloat(r1.x) + -9.99999975e-05));
  r2.x = vd_asuint((-(int)(vd_asfloat(_AlphaToMaskAvailable) != 0.0)));
  r1.z = (r1.z & r2.x);
  o0.w = vd_asfloat(((r2.x != 0u) ? r1.x : r0.w));
  r0.w = ((r1.z != 0u) ? r1.w : r1.y);
  r0.w = vd_asuint((-(int)(vd_asfloat(r0.w) < 0.0)));
  if (r0.w != 0u) discard;
  r0.yzw = vd_asuint(((-(vd_asfloat(r0.xxx))) + vd_asfloat(r0.xyz)));
  r0.xyz = vd_asuint((vd_asfloat((_isColor).xxx) * vd_asfloat(r0.yzw) + vd_asfloat(r0.xxx)));
  o0.xyz = (vd_asfloat(r0.xyz) * vd_asfloat((_AlbedoStr).xxx));
  return;
}
