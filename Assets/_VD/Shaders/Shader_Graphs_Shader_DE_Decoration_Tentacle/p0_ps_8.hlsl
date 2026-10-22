// Shader Graphs/Shader_DE_Decoration_Tentacle | pass 0 "Universal Forward" | progFragment | keywords: FOG_LINEAR _DBUFFER_MRT3 | blob 8
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
  r0.xyz = _Color.xyz * r0.xyz;
  r1.y = -r1.x * 0.5 + r0.w;
  r1.x = max(9.99999975e-005, r1.x);
  r1.x = r1.y / r1.x;
  r1.x = saturate(1 + r1.x);
  r1.y = -9.99999975e-005 + r1.x;
  r1.z = cmp(_AlphaToMaskAvailable != 0.000000);
  r0.w = r1.z ? r1.y : r0.w;
  o0.w = r1.z ? r1.x : 1;
  r0.w = cmp(r0.w < 0);
  if (r0.w != 0) discard;
  r1.xy = (int2)v0.xy;
  r1.zw = float2(0,0);
  r1.xyzw = _DBufferTexture0.Load(r1.xyz).xyzw;
  o0.xyz = r0.xyz * r1.www + r1.xyz;
  return;
}
