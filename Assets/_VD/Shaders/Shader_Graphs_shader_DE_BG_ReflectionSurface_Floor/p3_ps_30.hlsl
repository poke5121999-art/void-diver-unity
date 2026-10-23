// Shader Graphs/shader_DE_BG_ReflectionSurface_Floor | pass 3 "DecalMeshForwardEmissive" | progFragment | keywords: (none) | blob 30
void frag(
  float4 v0 : SV_POSITION0,
  float4 v1 : VDREG1,
  float4 v2 : VDREG2,
  float4 v3 : VDREG3,
  float4 v4 : VDREG4,
  out float4 o0 : SV_Target0)
{
  uint4 r0, r1;
  r0 = 0u; r1 = 0u;

  r0.x = vd_asuint((-(int)(vd_asfloat(_ProjectionParams.x) < 0.0)));
  r0.y = vd_asuint(((-(vd_asfloat(v0.y))) + vd_asfloat(_ScreenParams.y)));
  r0.y = ((r0.x != 0u) ? r0.y : vd_asuint(v0.y));
  r0.x = vd_asuint(v0.x);
  r0.xy = vd_asuint((vd_asfloat(r0.xy) / vd_asfloat(_ScreenParams.xy)));
  r0.z = vd_asuint(((-(vd_asfloat(r0.y))) + 1.0));
  r0.xyzw = vd_asuint(_PlaneReflectionTexture.SampleBias(sampler_PlaneReflectionTexture, vd_asfloat(r0.xz), vd_asfloat(_GlobalMipBias.x)).xyzw);
  r1.x = vd_asuint(((-(vd_asfloat(_GlobalBackgroundColorIntensity))) + 1.0));
  r1.x = vd_asuint((vd_asfloat(r1.x) * vd_asfloat(_PlaneReflectionActive)));
  r0.w = vd_asuint((vd_asfloat(r0.w) * vd_asfloat(r1.x)));
  r0.xyz = vd_asuint((-(int3)(vd_asfloat(r0.xyz) >= float3(1.10000002, 1.10000002, 1.10000002))));
  o0.xyz = vd_asfloat((r0.xyz & uint3(0x3f000000u, 0x3f000000u, 0x3f000000u)));
  o0.w = (vd_asfloat(r0.w) * vd_asfloat(_Opacity));
  return;
}
