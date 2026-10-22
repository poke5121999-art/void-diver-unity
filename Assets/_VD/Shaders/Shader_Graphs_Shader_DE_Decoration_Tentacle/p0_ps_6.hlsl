// Shader Graphs/Shader_DE_Decoration_Tentacle | pass 0 "Universal Forward" | progFragment | keywords: FOG_LINEAR | blob 6
void frag(
  float4 v0 : SV_POSITION0,
  float4 v1 : INTERP0,
  float3 v2 : INTERP1,
  float3 v3 : INTERP2,
  out float4 o0 : SV_Target0)
{
  float4 r0,r1;
  uint4 bitmask, uiDest;
  float4 fDest;

  r0.xyzw = _Texture2D.SampleBias(sampler_Texture2D, v1.xy, _GlobalMipBias.x).xyzw;
  r1.x = ddx(r0.w);
  r1.y = ddy(r0.w);
  r1.x = abs(r1.x) + abs(r1.y);
  r0.w = -0.800000012 + r0.w;
  o0.xyz = _Color.xyz * r0.xyz;
  r0.x = -r1.x * 0.5 + r0.w;
  r0.y = max(9.99999975e-005, r1.x);
  r0.x = r0.x / r0.y;
  r0.x = saturate(1 + r0.x);
  r0.y = -9.99999975e-005 + r0.x;
  r0.z = cmp(_AlphaToMaskAvailable != 0.000000);
  r0.y = r0.z ? r0.y : r0.w;
  o0.w = r0.z ? r0.x : 1;
  r0.x = cmp(r0.y < 0);
  if (r0.x != 0) discard;
  return;
}
