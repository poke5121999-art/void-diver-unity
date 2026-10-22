// Shader Graphs/shader_DE_BG_ReflectionSurface_Floor | pass 2 "DBufferMesh" | progFragment | keywords: _DBUFFER_MRT3 _DE_EDITOR_CAMERA | blob 27
void frag(
  float4 v0 : SV_POSITION0,
  float4 v1 : INTERP0,
  float4 v2 : INTERP1,
  float3 v3 : INTERP2,
  float3 v4 : INTERP3,
  out float4 o0 : SV_Target0,
  out float4 o1 : SV_Target1,
  out float4 o2 : SV_Target2)
{
  float4 r0;
  uint4 bitmask, uiDest;
  float4 fDest;

  r0.x = cmp(_ProjectionParams.x < 0);
  r0.y = _ScreenParams.y + -v0.y;
  r0.y = r0.x ? r0.y : v0.y;
  r0.x = v0.x;
  r0.xy = r0.xy / _ScreenParams.xy;
  r0.z = 1 + -r0.y;
  r0.xyz = _PlaneReflectionTexture.SampleBias(sampler_PlaneReflectionTexture, r0.xz, _GlobalMipBias.x).xyz;
  o0.xyz = r0.xyz;
  o0.w = 0;
  r0.x = dot(v4.xyz, v4.xyz);
  r0.x = rsqrt(r0.x);
  r0.xyz = v4.xyz * r0.xxx;
  o1.xyz = r0.xyz * float3(0.5,0.5,0.5) + float3(0.5,0.5,0.5);
  o1.w = 1;
  o2.xyzw = float4(0,1,0,0);
  return;
}
