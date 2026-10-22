// LCArt/Indicator/Shader_DE_Indicator_Billboard | pass 0 "" | progFragment | keywords: _ADDITIONAL_LIGHTS _DE_EDITOR_CAMERA | blob 17
void frag(
  float4 v0 : TEXCOORD0,
  float3 v1 : TEXCOORD1,
  float4 v2 : SV_POSITION0,
  float4 v3 : COLOR0,
  out float4 o0 : SV_Target0)
{
  float4 r0,r1;
  uint4 bitmask, uiDest;
  float4 fDest;

  r0.x = saturate(_Progress);
  r0.yzw = _EndColor.xyz + -_StartColor.xyz;
  r0.xyz = r0.xxx * r0.yzw + _StartColor.xyz;
  r1.xyzw = _MainTex.SampleBias(sampler_MainTex, v0.xy, _GlobalMipBias.x).xyzw;
  r1.xyzw = saturate(r1.xyzw);
  o0.xyz = r1.xyz * r0.xyz;
  r0.x = r1.w * r1.w;
  o0.w = r0.x * r0.x;
  return;
}
