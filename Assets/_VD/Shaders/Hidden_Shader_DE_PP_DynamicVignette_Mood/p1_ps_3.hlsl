// Hidden/Shader_DE_PP_DynamicVignette_Mood | pass 1 "Blit" | progFragment | keywords: (none) | blob 3
void frag(
  float4 v0 : SV_POSITION0,
  float4 v1 : INTERP0,
  float4 v2 : INTERP1,
  out float4 o0 : SV_TARGET0)
{
  float4 r0,r1;
  uint4 bitmask, uiDest;
  float4 fDest;

  r0.xy = float2(-0.5,-0.5) + v1.xy;
  r0.z = max(abs(r0.x), abs(r0.y));
  r0.z = 1 / r0.z;
  r0.w = min(abs(r0.x), abs(r0.y));
  r0.z = r0.w * r0.z;
  r0.w = r0.z * r0.z;
  r1.x = r0.w * 0.0208350997 + -0.0851330012;
  r1.x = r0.w * r1.x + 0.180141002;
  r1.x = r0.w * r1.x + -0.330299497;
  r0.w = r0.w * r1.x + 0.999866009;
  r1.x = r0.z * r0.w;
  r1.x = r1.x * -2 + 1.57079637;
  r1.y = cmp(abs(r0.y) < abs(r0.x));
  r1.x = r1.y ? r1.x : 0;
  r0.z = r0.z * r0.w + r1.x;
  r0.w = cmp(r0.y < -r0.y);
  r0.w = r0.w ? -3.141593 : 0;
  r0.z = r0.z + r0.w;
  r0.w = min(r0.x, r0.y);
  r0.w = cmp(r0.w < -r0.w);
  r1.x = max(r0.x, r0.y);
  r0.x = dot(r0.xy, r0.xy);
  r0.x = sqrt(r0.x);
  r0.x = r0.x + r0.x;
  r0.x = _TimeParameters.x * -0.0500000007 + r0.x;
  r1.x = cmp(r1.x >= -r1.x);
  r0.w = r0.w ? r1.x : 0;
  r0.z = r0.w ? -r0.z : r0.z;
  r0.y = 0.159235656 * r0.z;
  r0.xy = float2(0.25,5) * r0.xy;
  r0.xyzw = _NoiseTexture.SampleBias(sampler_NoiseTexture, r0.xy, _GlobalMipBias.x).xyzw;
  r1.xyzw = _DynamicTexture.SampleBias(sampler_DynamicTexture, v1.xy, _GlobalMipBias.x).xyzw;
  r0.y = r1.y * r1.y;
  r0.x = r0.y * r0.x;
  r0.x = rsqrt(r0.x);
  r0.x = 1 / r0.x;
  r0.x = max(0.100000001, r0.x);
  r0.x = -_MoodSightVignetteIntensity + r0.x;
  r0.x = saturate(10 * r0.x);
  r0.y = r0.x * -2 + 3;
  r0.x = r0.x * r0.x;
  r0.x = r0.y * r0.x;
  r0.x = saturate(_SightDissolveAlpha * r0.x + r1.y);
  r0.x = _SightColor.w * r0.x;
  r0.y = saturate(_LowHpColor.w * _MoodLowHPVignetteIntensity);
  r0.y = r1.x * r0.y;
  o0.w = max(r0.y, r0.x);
  r0.xzw = _LowHpColor.xyz + -_SightColor.xyz;
  o0.xyz = r0.yyy * r0.xzw + _SightColor.xyz;
  return;
}
