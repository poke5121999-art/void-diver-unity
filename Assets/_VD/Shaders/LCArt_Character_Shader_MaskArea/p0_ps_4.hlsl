// LCArt/Character/Shader_MaskArea | pass 0 "" | progFragment | keywords: _WRITE_RENDERING_LAYERS | blob 4
void frag(
  float4 v0 : SV_POSITION0,
  float2 v1 : TEXCOORD0,
  float w1 : TEXCOORD2,
  float3 v2 : TEXCOORD1,
  out float4 o0 : SV_Target0)
{
  float4 r0;
  uint4 bitmask, uiDest;
  float4 fDest;

  r0.x = _MainTex.SampleBias(sampler_MainTex, v1.xy, _GlobalMipBias.x).x;
  o0.w = r0.x;
  o0.xy = float2(0,0);
  o0.z = w1.x;
  return;
}
