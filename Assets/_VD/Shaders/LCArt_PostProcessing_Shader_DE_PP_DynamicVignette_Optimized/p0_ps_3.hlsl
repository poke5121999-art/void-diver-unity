// LCArt/PostProcessing/Shader_DE_PP_DynamicVignette_Optimized | pass 0 "DrawProcedural" | progFragment | keywords: (none) | blob 3
void frag(
  float4 v0 : SV_POSITION0,
  float4 v1 : VDREG1,
  float4 v2 : VDREG2,
  out float4 o0 : SV_TARGET0)
{
  uint4 r0, r1, r2;
  r0 = 0u; r1 = 0u; r2 = 0u;

  r0.xyzw = vd_asuint(_DynamicTexture.SampleBias(sampler_DynamicTexture, vd_asfloat(v1.xy), vd_asfloat(_GlobalMipBias.x)).xyzw);
  r0.w = vd_asuint(saturate((vd_asfloat(_LowHPVignetteIntensity) * vd_asfloat(_LowHpColor.w))));
  r0.x = vd_asuint((vd_asfloat(r0.w) * vd_asfloat(r0.x)));
  r1.xyz = vd_asuint(((-(vd_asfloat(_SightColor.xyz))) + vd_asfloat(_LowHpColor.xyz)));
  o0.xyz = (vd_asfloat(r0.xxx) * vd_asfloat(r1.xyz) + vd_asfloat(_SightColor.xyz));
  r0.w = vd_asuint((vd_asfloat(_SightVignetteIntensity) + -1.0));
  r0.w = vd_asuint((vd_asfloat(_GlobalIsCurrentlyInStage) * vd_asfloat(r0.w) + 1.0));
  r1.x = vd_asuint(((-(vd_asfloat(r0.z))) + vd_asfloat(r0.y)));
  r0.z = vd_asuint((vd_asfloat(r0.w) * vd_asfloat(r1.x) + vd_asfloat(r0.z)));
  r0.w = vd_asuint((-(int)(0.00100000005 < vd_asfloat(_GlobalIsCurrentlyInStage))));
  r1.x = vd_asuint((-(int)(0.00100000005 < vd_asfloat(_SightDissolveAlpha))));
  r0.w = (r0.w & r1.x);
  if (r0.w != 0u) {
    r1.xy = vd_asuint((vd_asfloat(v1.xy) + float2(-0.5, -0.5)));
    r0.w = vd_asuint(dot(vd_asfloat(r1.xy), vd_asfloat(r1.xy)));
    r0.w = vd_asuint(sqrt(vd_asfloat(r0.w)));
    r0.w = vd_asuint((vd_asfloat(r0.w) + vd_asfloat(r0.w)));
    r1.z = vd_asuint(min(abs(vd_asfloat(r1.y)), abs(vd_asfloat(r1.x))));
    r1.w = vd_asuint(max(abs(vd_asfloat(r1.y)), abs(vd_asfloat(r1.x))));
    r1.w = vd_asuint((1.0 / vd_asfloat(r1.w)));
    r1.z = vd_asuint((vd_asfloat(r1.w) * vd_asfloat(r1.z)));
    r1.w = vd_asuint((vd_asfloat(r1.z) * vd_asfloat(r1.z)));
    r2.x = vd_asuint((vd_asfloat(r1.w) * 0.0208350997 + -0.0851330012));
    r2.x = vd_asuint((vd_asfloat(r1.w) * vd_asfloat(r2.x) + 0.180141002));
    r2.x = vd_asuint((vd_asfloat(r1.w) * vd_asfloat(r2.x) + -0.330299497));
    r1.w = vd_asuint((vd_asfloat(r1.w) * vd_asfloat(r2.x) + 0.999866009));
    r2.x = vd_asuint((vd_asfloat(r1.w) * vd_asfloat(r1.z)));
    r2.y = vd_asuint((-(int)(abs(vd_asfloat(r1.y)) < abs(vd_asfloat(r1.x)))));
    r2.x = vd_asuint((vd_asfloat(r2.x) * -2.0 + 1.57079637));
    r2.x = (r2.y & r2.x);
    r1.z = vd_asuint((vd_asfloat(r1.z) * vd_asfloat(r1.w) + vd_asfloat(r2.x)));
    r1.w = vd_asuint((-(int)(vd_asfloat(r1.y) < (-(vd_asfloat(r1.y))))));
    r1.w = (r1.w & 0xc0490fdbu);
    r1.z = vd_asuint((vd_asfloat(r1.w) + vd_asfloat(r1.z)));
    r1.w = vd_asuint(min(vd_asfloat(r1.y), vd_asfloat(r1.x)));
    r1.x = vd_asuint(max(vd_asfloat(r1.y), vd_asfloat(r1.x)));
    r1.y = vd_asuint((-(int)(vd_asfloat(r1.w) < (-(vd_asfloat(r1.w))))));
    r1.x = vd_asuint((-(int)(vd_asfloat(r1.x) >= (-(vd_asfloat(r1.x))))));
    r1.x = (r1.x & r1.y);
    r1.x = vd_asuint(((r1.x != 0u) ? (-(vd_asfloat(r1.z))) : vd_asfloat(r1.z)));
    r1.y = vd_asuint((vd_asfloat(r1.x) * 0.159154937));
    r1.x = vd_asuint((vd_asfloat(_TimeParameters.x) * -0.0500000007 + vd_asfloat(r0.w)));
    r1.xy = vd_asuint((vd_asfloat(r1.xy) * float2(0.25, 5.0)));
    r1.xyzw = vd_asuint(_NoiseTexture.SampleBias(sampler_DynamicTexture, vd_asfloat(r1.xy), vd_asfloat(_GlobalMipBias.x)).xyzw);
    r0.y = vd_asuint((vd_asfloat(r0.y) * vd_asfloat(r0.y)));
    r0.y = vd_asuint((vd_asfloat(r1.x) * vd_asfloat(r0.y)));
    r0.y = vd_asuint(sqrt(vd_asfloat(r0.y)));
    r0.y = vd_asuint(max(vd_asfloat(r0.y), 0.100000001));
    r0.y = vd_asuint((vd_asfloat(r0.y) + (-(vd_asfloat(_SightVignetteIntensity)))));
    r0.y = vd_asuint(saturate((vd_asfloat(r0.y) * 10.0)));
    r0.w = vd_asuint((vd_asfloat(r0.y) * -2.0 + 3.0));
    r0.y = vd_asuint((vd_asfloat(r0.y) * vd_asfloat(r0.y)));
    r0.y = vd_asuint((vd_asfloat(r0.y) * vd_asfloat(r0.w)));
    r0.y = vd_asuint((vd_asfloat(r0.y) * vd_asfloat(_SightDissolveAlpha)));
    r0.y = vd_asuint((vd_asfloat(r0.y) * vd_asfloat(_GlobalIsCurrentlyInStage)));
  } else {
    r0.y = 0x00000000u;
  }
  r0.y = vd_asuint(saturate((vd_asfloat(r0.y) + vd_asfloat(r0.z))));
  r0.y = vd_asuint((vd_asfloat(r0.y) * vd_asfloat(_SightColor.w)));
  o0.w = max(vd_asfloat(r0.y), vd_asfloat(r0.x));
  return;
}
