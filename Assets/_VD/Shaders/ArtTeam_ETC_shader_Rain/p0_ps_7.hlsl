// ArtTeam/ETC/shader_Rain | pass 0 "Universal Forward" | progFragment | keywords: _DBUFFER_MRT3 | blob 7
void frag(
  float4 v0 : SV_POSITION0,
  float4 v1 : VDREG1,
  float4 v2 : VDREG2,
  float4 v3 : VDREG3,
  out float4 o0 : SV_Target0)
{
  uint4 r0;
  r0 = 0u;

  r0.xy = vd_asuint(((int2)vd_asfloat(v0.xy)));
  r0.zw = uint2(0x00000000u, 0x00000000u);
  r0.xyzw = vd_asuint(_DBufferTexture0.Load(vd_asint(r0.xyw)).xyzw);
  o0.xyz = (vd_asfloat((_RainStrength).xxx) * vd_asfloat(r0.www) + vd_asfloat(r0.xyz));
  o0.w = vd_asfloat(0x3f800000u);
  return;
}
