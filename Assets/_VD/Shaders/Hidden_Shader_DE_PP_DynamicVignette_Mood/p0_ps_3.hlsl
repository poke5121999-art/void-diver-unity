// Hidden/Shader_DE_PP_DynamicVignette_Mood | pass 0 "DrawProcedural" | progFragment | keywords: (none) | blob 3
void frag(
  float4 v0 : SV_POSITION0,
  float4 v1 : VDREG1,
  float4 v2 : VDREG2,
  out float4 o0 : SV_TARGET0)
{
  uint4 r0, r1;
  r0 = 0u; r1 = 0u;

  r0.xy = vd_asuint((vd_asfloat(v1.xy) + float2(-0.5, -0.5)));
  r0.z = vd_asuint(max(abs(vd_asfloat(r0.y)), abs(vd_asfloat(r0.x))));
  r0.z = vd_asuint((1.0 / vd_asfloat(r0.z)));
  r0.w = vd_asuint(min(abs(vd_asfloat(r0.y)), abs(vd_asfloat(r0.x))));
  r0.z = vd_asuint((vd_asfloat(r0.z) * vd_asfloat(r0.w)));
  r0.w = vd_asuint((vd_asfloat(r0.z) * vd_asfloat(r0.z)));
  r1.x = vd_asuint((vd_asfloat(r0.w) * 0.0208350997 + -0.0851330012));
  r1.x = vd_asuint((vd_asfloat(r0.w) * vd_asfloat(r1.x) + 0.180141002));
  r1.x = vd_asuint((vd_asfloat(r0.w) * vd_asfloat(r1.x) + -0.330299497));
  r0.w = vd_asuint((vd_asfloat(r0.w) * vd_asfloat(r1.x) + 0.999866009));
  r1.x = vd_asuint((vd_asfloat(r0.w) * vd_asfloat(r0.z)));
  r1.x = vd_asuint((vd_asfloat(r1.x) * -2.0 + 1.57079637));
  r1.y = vd_asuint((-(int)(abs(vd_asfloat(r0.y)) < abs(vd_asfloat(r0.x)))));
  r1.x = (r1.y & r1.x);
  r0.z = vd_asuint((vd_asfloat(r0.z) * vd_asfloat(r0.w) + vd_asfloat(r1.x)));
  r0.w = vd_asuint((-(int)(vd_asfloat(r0.y) < (-(vd_asfloat(r0.y))))));
  r0.w = (r0.w & 0xc0490fdbu);
  r0.z = vd_asuint((vd_asfloat(r0.w) + vd_asfloat(r0.z)));
  r0.w = vd_asuint(min(vd_asfloat(r0.y), vd_asfloat(r0.x)));
  r0.w = vd_asuint((-(int)(vd_asfloat(r0.w) < (-(vd_asfloat(r0.w))))));
  r1.x = vd_asuint(max(vd_asfloat(r0.y), vd_asfloat(r0.x)));
  r0.x = vd_asuint(dot(vd_asfloat(r0.xy), vd_asfloat(r0.xy)));
  r0.x = vd_asuint(sqrt(vd_asfloat(r0.x)));
  r0.x = vd_asuint((vd_asfloat(r0.x) + vd_asfloat(r0.x)));
  r0.x = vd_asuint((vd_asfloat(_TimeParameters.x) * -0.0500000007 + vd_asfloat(r0.x)));
  r1.x = vd_asuint((-(int)(vd_asfloat(r1.x) >= (-(vd_asfloat(r1.x))))));
  r0.w = (r0.w & r1.x);
  r0.z = vd_asuint(((r0.w != 0u) ? (-(vd_asfloat(r0.z))) : vd_asfloat(r0.z)));
  r0.y = vd_asuint((vd_asfloat(r0.z) * 0.159235656));
  r0.xy = vd_asuint((vd_asfloat(r0.xy) * float2(0.25, 5.0)));
  r0.xyzw = vd_asuint(_NoiseTexture.SampleBias(sampler_NoiseTexture, vd_asfloat(r0.xy), vd_asfloat(_GlobalMipBias.x)).xyzw);
  r1.xyzw = vd_asuint(_DynamicTexture.SampleBias(sampler_DynamicTexture, vd_asfloat(v1.xy), vd_asfloat(_GlobalMipBias.x)).xyzw);
  r0.y = vd_asuint((vd_asfloat(r1.y) * vd_asfloat(r1.y)));
  r0.x = vd_asuint((vd_asfloat(r0.x) * vd_asfloat(r0.y)));
  r0.x = vd_asuint(rsqrt(vd_asfloat(r0.x)));
  r0.x = vd_asuint((1.0 / vd_asfloat(r0.x)));
  r0.x = vd_asuint(max(vd_asfloat(r0.x), 0.100000001));
  r0.x = vd_asuint((vd_asfloat(r0.x) + (-(vd_asfloat(_MoodSightVignetteIntensity)))));
  r0.x = vd_asuint(saturate((vd_asfloat(r0.x) * 10.0)));
  r0.y = vd_asuint((vd_asfloat(r0.x) * -2.0 + 3.0));
  r0.x = vd_asuint((vd_asfloat(r0.x) * vd_asfloat(r0.x)));
  r0.x = vd_asuint((vd_asfloat(r0.x) * vd_asfloat(r0.y)));
  r0.x = vd_asuint(saturate((vd_asfloat(_SightDissolveAlpha) * vd_asfloat(r0.x) + vd_asfloat(r1.y))));
  r0.x = vd_asuint((vd_asfloat(r0.x) * vd_asfloat(_SightColor.w)));
  r0.y = vd_asuint(saturate((vd_asfloat(_MoodLowHPVignetteIntensity) * vd_asfloat(_LowHpColor.w))));
  r0.y = vd_asuint((vd_asfloat(r0.y) * vd_asfloat(r1.x)));
  o0.w = max(vd_asfloat(r0.x), vd_asfloat(r0.y));
  r0.xzw = vd_asuint(((-(vd_asfloat(_SightColor.xyz))) + vd_asfloat(_LowHpColor.xyz)));
  o0.xyz = (vd_asfloat(r0.yyy) * vd_asfloat(r0.xzw) + vd_asfloat(_SightColor.xyz));
  return;
}
