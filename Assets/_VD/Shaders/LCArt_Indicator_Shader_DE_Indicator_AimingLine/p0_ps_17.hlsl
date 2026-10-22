// LCArt/Indicator/Shader_DE_Indicator_AimingLine | pass 0 "" | progFragment | keywords: _ADDITIONAL_LIGHTS _DE_EDITOR_CAMERA | blob 17
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

  r0.x = -1.125 + _Progress;
  r0.x = saturate(8 * r0.x);
  r0.y = r0.x * -2 + 3;
  r0.x = r0.x * r0.x;
  r0.x = -r0.y * r0.x + 1;
  r0.y = saturate(_Progress);
  r0.z = 8 * r0.y;
  r0.z = min(1, r0.z);
  r0.w = r0.z * -2 + 3;
  r0.z = r0.z * r0.z;
  r0.z = r0.w * r0.z;
  r1.xyzw = _WeightTex.SampleBias(sampler_WeightTex, v0.xy, _GlobalMipBias.x).xyzw;
  r1.xyzw = saturate(r1.xyzw);
  r0.z = r1.w * r0.z;
  o0.w = r0.z * r0.x;
  r0.xzw = _EndColor.xyz + -_StartColor.xyz;
  r0.xyz = r0.yyy * r0.xzw + _StartColor.xyz;
  o0.xyz = r1.xyz * r0.xyz;
  return;
}
