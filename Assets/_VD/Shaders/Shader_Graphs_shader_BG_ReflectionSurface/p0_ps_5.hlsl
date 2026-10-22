// Shader Graphs/shader_BG_ReflectionSurface | pass 0 "Universal Forward" | progFragment | keywords: FOG_LINEAR | blob 5
void frag(
  float4 v0 : SV_POSITION0,
  float3 v1 : INTERP0,
  float3 v2 : INTERP1,
  out float4 o0 : SV_Target0)
{
  float4 r0,r1;
  uint4 bitmask, uiDest;
  float4 fDest;

  r0.x = cmp(_ProjectionParams.x < 0);
  r0.y = _ScaledScreenParams.y + -v0.y;
  r0.y = r0.x ? r0.y : v0.y;
  r0.x = v0.x;
  r0.xy = r0.xy / _ScaledScreenParams.xy;
  r0.z = 1 + -r0.y;
  r0.xyzw = _PlaneReflectionTexture.SampleBias(sampler_PlaneReflectionTexture, r0.xz, _GlobalMipBias.x).xyzw;
  r1.xyz = (_PlaneReflectionActive).xxx * r0.xyz;
  r0.xyz = -(_PlaneReflectionActive).xxx * r0.xyz + float3(1,1,1);
  o0.xyz = r0.xyz * float3(0.5,0.5,0.5) + r1.xyz;
  o0.w = 0.100000001;
  return;
}
