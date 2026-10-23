// Shader Graphs/shader_BG_ReflectionSurface | pass 0 "Universal Forward" | progFragment | keywords: (none) | blob 4
void frag(
  float4 v0 : SV_POSITION0,
  float4 v1 : VDREG1,
  float4 v2 : VDREG2,
  out float4 o0 : SV_Target0)
{
  uint4 r0, r1;
  r0 = 0u; r1 = 0u;

  r0.x = vd_asuint((-(int)(vd_asfloat(_ProjectionParams.x) < 0.0)));
  r0.y = vd_asuint(((-(vd_asfloat(v0.y))) + vd_asfloat(_ScaledScreenParams.y)));
  r0.y = ((r0.x != 0u) ? r0.y : vd_asuint(v0.y));
  r0.x = vd_asuint(v0.x);
  r0.xy = vd_asuint((vd_asfloat(r0.xy) / vd_asfloat(_ScaledScreenParams.xy)));
  r0.z = vd_asuint(((-(vd_asfloat(r0.y))) + 1.0));
  r0.xyzw = vd_asuint(_PlaneReflectionTexture.SampleBias(sampler_PlaneReflectionTexture, vd_asfloat(r0.xz), vd_asfloat(_GlobalMipBias.x)).xyzw);
  r1.xyz = vd_asuint((vd_asfloat(r0.xyz) * vd_asfloat((_PlaneReflectionActive).xxx)));
  r0.xyz = vd_asuint(((-(vd_asfloat((_PlaneReflectionActive).xxx))) * vd_asfloat(r0.xyz) + float3(1.0, 1.0, 1.0)));
  o0.xyz = (vd_asfloat(r0.xyz) * float3(0.5, 0.5, 0.5) + vd_asfloat(r1.xyz));
  o0.w = vd_asfloat(0x3dcccccdu);
  return;
}
