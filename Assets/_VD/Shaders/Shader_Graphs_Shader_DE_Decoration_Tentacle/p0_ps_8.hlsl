// Shader Graphs/Shader_DE_Decoration_Tentacle | pass 0 "Universal Forward" | progFragment | keywords: FOG_LINEAR _DBUFFER_MRT3 | blob 8
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
  r0.xyz = vd_asuint((vd_asfloat(r0.xyz) * vd_asfloat(_Color.xyz)));
  r1.y = vd_asuint(((-(vd_asfloat(r1.x))) * 0.5 + vd_asfloat(r0.w)));
  r1.x = vd_asuint(max(vd_asfloat(r1.x), 9.99999975e-05));
  r1.x = vd_asuint((vd_asfloat(r1.y) / vd_asfloat(r1.x)));
  r1.x = vd_asuint(saturate((vd_asfloat(r1.x) + 1.0)));
  r1.y = vd_asuint((vd_asfloat(r1.x) + -9.99999975e-05));
  r1.z = vd_asuint((-(int)(vd_asfloat(_AlphaToMaskAvailable) != 0.0)));
  r0.w = ((r1.z != 0u) ? r1.y : r0.w);
  o0.w = vd_asfloat(((r1.z != 0u) ? r1.x : 0x3f800000u));
  r0.w = vd_asuint((-(int)(vd_asfloat(r0.w) < 0.0)));
  if (r0.w != 0u) discard;
  r1.xy = vd_asuint(((int2)vd_asfloat(v0.xy)));
  r1.zw = uint2(0x00000000u, 0x00000000u);
  r1.xyzw = vd_asuint(_DBufferTexture0.Load(vd_asint(r1.xyw)).xyzw);
  o0.xyz = (vd_asfloat(r0.xyz) * vd_asfloat(r1.www) + vd_asfloat(r1.xyz));
  return;
}
