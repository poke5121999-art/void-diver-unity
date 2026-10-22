// ArtTeam/ETC/shader_Rain | pass 0 "Universal Forward" | progFragment | keywords: _DBUFFER_MRT3 | blob 7
void frag(
  float4 v0 : SV_POSITION0,
  float4 v1 : INTERP0,
  float3 v2 : INTERP1,
  float3 v3 : INTERP2,
  out float4 o0 : SV_Target0)
{
  float4 r0;
  uint4 bitmask, uiDest;
  float4 fDest;

  r0.xy = (int2)v0.xy;
  r0.zw = float2(0,0);
  r0.xyzw = _DBufferTexture0.Load(r0.xyz).xyzw;
  o0.xyz = (_RainStrength).xxx * r0.www + r0.xyz;
  o0.w = 1;
  return;
}
