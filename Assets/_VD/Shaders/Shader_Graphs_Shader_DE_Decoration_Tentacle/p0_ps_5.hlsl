// Shader Graphs/Shader_DE_Decoration_Tentacle | pass 0 "Universal Forward" | progFragment | keywords: (none) | blob 5
void frag(
  float4 v0 : SV_POSITION0,
  float4 v1 : VDREG1,
  float4 v2 : VDREG2,
  float4 v3 : VDREG3,
  out float4 o0 : SV_Target0)
{
  uint4 r0, r1;
  r0 = 0u; r1 = 0u;

  r0.xyzw = vd_asuint(_Texture2D.SampleBias(sampler_Texture2D, vd_asfloat(v1.xy), vd_asfloat(_GlobalMipBias.x)).xyzw);
  r1.x = vd_asuint(ddx(vd_asfloat(r0.w)));
  r1.y = vd_asuint(ddy(vd_asfloat(r0.w)));
  r1.x = vd_asuint((abs(vd_asfloat(r1.y)) + abs(vd_asfloat(r1.x))));
  r0.w = vd_asuint((vd_asfloat(r0.w) + -0.800000012));
  o0.xyz = (vd_asfloat(r0.xyz) * vd_asfloat(_Color.xyz));
  r0.x = vd_asuint(((-(vd_asfloat(r1.x))) * 0.5 + vd_asfloat(r0.w)));
  r0.y = vd_asuint(max(vd_asfloat(r1.x), 9.99999975e-05));
  r0.x = vd_asuint((vd_asfloat(r0.x) / vd_asfloat(r0.y)));
  r0.x = vd_asuint(saturate((vd_asfloat(r0.x) + 1.0)));
  r0.y = vd_asuint((vd_asfloat(r0.x) + -9.99999975e-05));
  r0.z = vd_asuint((-(int)(vd_asfloat(_AlphaToMaskAvailable) != 0.0)));
  r0.y = ((r0.z != 0u) ? r0.y : r0.w);
  o0.w = vd_asfloat(((r0.z != 0u) ? r0.x : 0x3f800000u));
  r0.x = vd_asuint((-(int)(vd_asfloat(r0.y) < 0.0)));
  if (r0.x != 0u) discard;
  return;
}
