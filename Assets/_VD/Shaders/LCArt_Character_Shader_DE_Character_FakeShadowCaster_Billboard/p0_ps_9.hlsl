// LCArt/Character/Shader_DE_Character_FakeShadowCaster_Billboard | pass 0 "FakeShadowCast" | progFragment | keywords: _DE_EDITOR_CAMERA | blob 9
void frag(
  float4 v0 : SV_POSITION0,
  float2 v1 : TEXCOORD0,
  float w1 : TEXCOORD1,
  float w1 : TEXCOORD2,
  out float4 o0 : SV_Target0)
{
  float4 r0;
  uint4 bitmask, uiDest;
  float4 fDest;

  r0.xyzw = _MainTex.SampleBias(sampler_MainTex, v1.xy, _GlobalMipBias.x).xyzw;
  r0.x = -0.5 + r0.w;
  r0.x = cmp(r0.x < 0);
  if (r0.x != 0) discard;
  r0.x = _FakeShadowAlpha * w1.x;
  o0.y = _FakeShadowVisibility * r0.x;
  o0.xzw = float3(0,0,1);
  return;
}
