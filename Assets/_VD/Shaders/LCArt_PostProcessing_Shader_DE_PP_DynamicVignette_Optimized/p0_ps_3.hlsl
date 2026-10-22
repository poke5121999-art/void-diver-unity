// LCArt/PostProcessing/Shader_DE_PP_DynamicVignette_Optimized | pass 0 "DrawProcedural" | progFragment | keywords: (none) | blob 3
void frag(
  float4 v0 : SV_POSITION0,
  float4 v1 : INTERP0,
  float4 v2 : INTERP1,
  out float4 o0 : SV_TARGET0)
{
  float4 r0,r1,r2;
  uint4 bitmask, uiDest;
  float4 fDest;

  r0.xyzw = _DynamicTexture.SampleBias(sampler_DynamicTexture, v1.xy, _GlobalMipBias.x).xyzw;
  r0.w = saturate(_LowHpColor.w * _LowHPVignetteIntensity);
  r0.x = r0.x * r0.w;
  r1.xyz = _LowHpColor.xyz + -_SightColor.xyz;
  o0.xyz = r0.xxx * r1.xyz + _SightColor.xyz;
  r0.w = -1 + _SightVignetteIntensity;
  r0.w = _GlobalIsCurrentlyInStage * r0.w + 1;
  r1.x = r0.y + -r0.z;
  r0.z = r0.w * r1.x + r0.z;
  r0.w = cmp(0.00100000005 < _GlobalIsCurrentlyInStage);
  r1.x = cmp(0.00100000005 < _SightDissolveAlpha);
  r0.w = r0.w ? r1.x : 0;
  if (r0.w != 0) {
    r1.xy = float2(-0.5,-0.5) + v1.xy;
    r0.w = dot(r1.xy, r1.xy);
    r0.w = sqrt(r0.w);
    r0.w = r0.w + r0.w;
    r1.z = min(abs(r1.x), abs(r1.y));
    r1.w = max(abs(r1.x), abs(r1.y));
    r1.w = 1 / r1.w;
    r1.z = r1.z * r1.w;
    r1.w = r1.z * r1.z;
    r2.x = r1.w * 0.0208350997 + -0.0851330012;
    r2.x = r1.w * r2.x + 0.180141002;
    r2.x = r1.w * r2.x + -0.330299497;
    r1.w = r1.w * r2.x + 0.999866009;
    r2.x = r1.z * r1.w;
    r2.y = cmp(abs(r1.y) < abs(r1.x));
    r2.x = r2.x * -2 + 1.57079637;
    r2.x = r2.y ? r2.x : 0;
    r1.z = r1.z * r1.w + r2.x;
    r1.w = cmp(r1.y < -r1.y);
    r1.w = r1.w ? -3.141593 : 0;
    r1.z = r1.z + r1.w;
    r1.w = min(r1.x, r1.y);
    r1.x = max(r1.x, r1.y);
    r1.y = cmp(r1.w < -r1.w);
    r1.x = cmp(r1.x >= -r1.x);
    r1.x = r1.x ? r1.y : 0;
    r1.x = r1.x ? -r1.z : r1.z;
    r1.y = 0.159154937 * r1.x;
    r1.x = _TimeParameters.x * -0.0500000007 + r0.w;
    r1.xy = float2(0.25,5) * r1.xy;
    r1.xyzw = _NoiseTexture.SampleBias(sampler_DynamicTexture, r1.xy, _GlobalMipBias.x).xyzw;
    r0.y = r0.y * r0.y;
    r0.y = r0.y * r1.x;
    r0.y = sqrt(r0.y);
    r0.y = max(0.100000001, r0.y);
    r0.y = -_SightVignetteIntensity + r0.y;
    r0.y = saturate(10 * r0.y);
    r0.w = r0.y * -2 + 3;
    r0.y = r0.y * r0.y;
    r0.y = r0.w * r0.y;
    r0.y = _SightDissolveAlpha * r0.y;
    r0.y = _GlobalIsCurrentlyInStage * r0.y;
  } else {
    r0.y = 0;
  }
  r0.y = saturate(r0.z + r0.y);
  r0.y = _SightColor.w * r0.y;
  o0.w = max(r0.x, r0.y);
  return;
}
