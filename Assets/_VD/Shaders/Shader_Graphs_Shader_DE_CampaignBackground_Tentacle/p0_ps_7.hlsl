// Shader Graphs/Shader_DE_CampaignBackground_Tentacle | pass 0 "Universal Forward" | progFragment | keywords: _DBUFFER_MRT3 | blob 7
void frag(
  float4 v0 : SV_POSITION0,
  float4 v1 : VDREG1,
  float4 v2 : VDREG2,
  float4 v3 : VDREG3,
  out float4 o0 : SV_Target0)
{
  uint4 r0;
  r0 = 0u;

  r0.xyzw = vd_asuint(_Texture2D.SampleBias(sampler_Texture2D, vd_asfloat(v1.xy), vd_asfloat(_GlobalMipBias.x)).xyzw);
  r0.x = vd_asuint(ddx(vd_asfloat(r0.w)));
  r0.y = vd_asuint(ddy(vd_asfloat(r0.w)));
  r0.z = vd_asuint((vd_asfloat(r0.w) + -0.800000012));
  r0.x = vd_asuint((abs(vd_asfloat(r0.y)) + abs(vd_asfloat(r0.x))));
  r0.y = vd_asuint(((-(vd_asfloat(r0.x))) * 0.5 + vd_asfloat(r0.z)));
  r0.x = vd_asuint(max(vd_asfloat(r0.x), 9.99999975e-05));
  r0.x = vd_asuint((vd_asfloat(r0.y) / vd_asfloat(r0.x)));
  r0.x = vd_asuint(saturate((vd_asfloat(r0.x) + 1.0)));
  r0.y = vd_asuint((vd_asfloat(r0.x) + -9.99999975e-05));
  r0.w = vd_asuint((-(int)(vd_asfloat(_AlphaToMaskAvailable) != 0.0)));
  r0.y = ((r0.w != 0u) ? r0.y : r0.z);
  o0.w = vd_asfloat(((r0.w != 0u) ? r0.x : 0x3f800000u));
  r0.x = vd_asuint((-(int)(vd_asfloat(r0.y) < 0.0)));
  if (r0.x != 0u) discard;
  r0.xy = vd_asuint(((int2)vd_asfloat(v0.xy)));
  r0.zw = uint2(0x00000000u, 0x00000000u);
  r0.xyzw = vd_asuint(_DBufferTexture0.Load(vd_asint(r0.xyw)).xyzw);
  o0.xyz = (vd_asfloat(_Color.xyz) * vd_asfloat(r0.www) + vd_asfloat(r0.xyz));
  return;
}
