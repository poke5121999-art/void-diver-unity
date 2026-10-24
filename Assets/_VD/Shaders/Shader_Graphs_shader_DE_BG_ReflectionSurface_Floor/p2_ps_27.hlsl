// Shader Graphs/shader_DE_BG_ReflectionSurface_Floor | pass 2 "DBufferMesh" | progFragment | keywords: _DBUFFER_MRT3 _DE_EDITOR_CAMERA | blob 27
void frag(
  float4 v0 : SV_POSITION0,
  float4 v1 : VDREG1,
  float4 v2 : VDREG2,
  float4 v3 : VDREG3,
  float4 v4 : VDREG4,
  out float4 o0 : SV_Target0,
  out float4 o1 : SV_Target1,
  out float4 o2 : SV_Target2)
{
  uint4 r0;
  r0 = 0u;

#if UNITY_UV_STARTS_AT_TOP
  r0.x = vd_asuint((-(int)(vd_asfloat(_ProjectionParams.x) < 0.0)));
#else
  r0.x = vd_asuint((-(int)(0.0 < vd_asfloat(_ProjectionParams.x))));
#endif
  r0.y = vd_asuint(((-(vd_asfloat(v0.y))) + vd_asfloat(_ScreenParams.y)));
  r0.y = ((r0.x != 0u) ? r0.y : vd_asuint(v0.y));
  r0.x = vd_asuint(v0.x);
  r0.xy = vd_asuint((vd_asfloat(r0.xy) / vd_asfloat(_ScreenParams.xy)));
  r0.z = vd_asuint(((-(vd_asfloat(r0.y))) + 1.0));
  r0.xyz = vd_asuint(_PlaneReflectionTexture.SampleBias(sampler_PlaneReflectionTexture, vd_asfloat(r0.xz), vd_asfloat(_GlobalMipBias.x)).xyz);
  o0.xyz = vd_asfloat(r0.xyz);
  o0.w = vd_asfloat(0x00000000u);
  r0.x = vd_asuint(dot(vd_asfloat(v4.xyz), vd_asfloat(v4.xyz)));
  r0.x = vd_asuint(rsqrt(vd_asfloat(r0.x)));
  r0.xyz = vd_asuint((vd_asfloat(r0.xxx) * vd_asfloat(v4.xyz)));
  o1.xyz = (vd_asfloat(r0.xyz) * float3(0.5, 0.5, 0.5) + float3(0.5, 0.5, 0.5));
  o1.w = vd_asfloat(0x3f800000u);
  o2.xyzw = vd_asfloat(uint4(0x00000000u, 0x3f800000u, 0x00000000u, 0x00000000u));
  return;
}
