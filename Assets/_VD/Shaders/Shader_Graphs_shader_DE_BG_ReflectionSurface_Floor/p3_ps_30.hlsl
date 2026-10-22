// Shader Graphs/shader_DE_BG_ReflectionSurface_Floor | pass 3 "DecalMeshForwardEmissive" | progFragment | keywords: (none) | blob 30
void frag(
  float4 v0 : SV_POSITION0,
  float4 v1 : INTERP0,
  float4 v2 : INTERP1,
  float3 v3 : INTERP2,
  float3 v4 : INTERP3,
  out float4 o0 : SV_Target0)
{
  float4 r0,r1;
  uint4 bitmask, uiDest;
  float4 fDest;

  r0.x = cmp(_ProjectionParams.x < 0);
  r0.y = _ScreenParams.y + -v0.y;
  r0.y = r0.x ? r0.y : v0.y;
  r0.x = v0.x;
  r0.xy = r0.xy / _ScreenParams.xy;
  r0.z = 1 + -r0.y;
  r0.xyzw = _PlaneReflectionTexture.SampleBias(sampler_PlaneReflectionTexture, r0.xz, _GlobalMipBias.x).xyzw;
  r1.x = 1 + -_GlobalBackgroundColorIntensity;
  r1.x = _PlaneReflectionActive * r1.x;
  r0.w = r1.x * r0.w;
  r0.xyz = cmp(r0.xyz >= float3(1.10000002,1.10000002,1.10000002));
  o0.xyz = r0.xyz ? float3(0.5,0.5,0.5) : 0;
  o0.w = _Opacity * r0.w;
  return;
}
