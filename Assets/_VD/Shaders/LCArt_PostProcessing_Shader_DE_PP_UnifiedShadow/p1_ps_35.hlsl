// LCArt/PostProcessing/Shader_DE_PP_UnifiedShadow | pass 1 "FakeShadowCast" | progFragment | keywords: _DE_PRERENDERING_CAMERA | blob 35
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

  r0.xy = v1.xy * _MainTex_ST.xy + _MainTex_ST.zw;
  r0.xyzw = _MainTex.SampleBias(sampler_MainTex, r0.xy, _GlobalMipBias.x).xyzw;
  r0.x = -0.5 + r0.w;
  r0.x = cmp(r0.x < 0);
  r0.y = cmp(0 >= _FakeShadowScale);
  r0.z = ~(int)r0.y;
  r0.x = r0.x ? r0.z : 0;
  if (r0.x != 0) discard;
  r0.x = _FakeShadowAlpha * w1.x;
  r0.x = w1.x * r0.x;
  r0.z = r0.x * r0.w;
  o0.y = saturate(r0.y ? r0.z : r0.x);
  o0.xzw = float3(0,0,1);
  return;
}
