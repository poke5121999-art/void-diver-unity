// ArtTeam/ETC/shader_Transparent_Unlit | pass 0 "Universal Forward" | progFragment | keywords: FOG_LINEAR _DBUFFER_MRT3 | blob 15
void frag(
  float4 v0 : SV_POSITION0,
  float4 v1 : VDREG1,
  float4 v2 : VDREG2,
  float4 v3 : VDREG3,
  out float4 o0 : SV_Target0)
{
  uint4 r0, r1;
  r0 = 0u; r1 = 0u;

  r0.xy = vd_asuint((vd_asfloat(v1.xy) * vd_asfloat(_Albedo_ST.xy) + vd_asfloat(_Albedo_ST.zw)));
  r0.xyzw = vd_asuint(_Albedo.SampleBias(sampler_Albedo, vd_asfloat(r0.xy), vd_asfloat(_GlobalMipBias.x)).xyzw);
  r0.yzw = vd_asuint(((-(vd_asfloat(r0.xxx))) + vd_asfloat(r0.xyz)));
  r0.xyz = vd_asuint((vd_asfloat((_isColor).xxx) * vd_asfloat(r0.yzw) + vd_asfloat(r0.xxx)));
  r0.xyz = vd_asuint((vd_asfloat(r0.xyz) * vd_asfloat((_AlbedoStr).xxx)));
  r1.xy = vd_asuint(((int2)vd_asfloat(v0.xy)));
  r1.zw = uint2(0x00000000u, 0x00000000u);
  r1.xyzw = vd_asuint(_DBufferTexture0.Load(vd_asint(r1.xyw)).xyzw);
  o0.xyz = (vd_asfloat(r0.xyz) * vd_asfloat(r1.www) + vd_asfloat(r1.xyz));
  o0.w = vd_asfloat(0x3f800000u);
  return;
}
