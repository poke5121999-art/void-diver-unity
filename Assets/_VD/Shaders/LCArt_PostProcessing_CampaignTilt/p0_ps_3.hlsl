// LCArt/PostProcessing/CampaignTilt | pass 0 "CampaignTiltKawase" | progFragment | keywords: (none) | blob 3
void frag(
  float4 v0 : SV_POSITION0,
  float4 v1 : VDREG1,
  out float4 o0 : SV_Target0)
{
  uint4 r0, r1, r2, r3, r4;
  r0 = 0u; r1 = 0u; r2 = 0u; r3 = 0u; r4 = 0u;

  r0.x = vd_asuint((vd_asfloat(v1.y) + -0.5));
  r0.x = vd_asuint((abs(vd_asfloat(r0.x)) * 2.0 + (-(vd_asfloat(_CenterRatio)))));
  r0.y = vd_asuint((1.0 / vd_asfloat(_EdgeSoftness)));
  r0.x = vd_asuint(saturate((vd_asfloat(r0.y) * vd_asfloat(r0.x))));
  r0.y = vd_asuint((vd_asfloat(r0.x) * -2.0 + 3.0));
  r0.x = vd_asuint((vd_asfloat(r0.x) * vd_asfloat(r0.x)));
  r0.x = vd_asuint((vd_asfloat(r0.x) * vd_asfloat(r0.y)));
  r0.x = vd_asuint((vd_asfloat(r0.x) * vd_asfloat(_BlurStrength)));
  r0.y = vd_asuint((vd_asfloat(_KawaseOffset) + 0.5));
  r1.xy = vd_asuint((vd_asfloat(r0.yy) * vd_asfloat(_BlitTexture_TexelSize.xy)));
  r1.zw = vd_asuint((-(vd_asfloat(r1.yx))));
  r1.xyzw = vd_asuint((vd_asfloat(r1.xzwy) + vd_asfloat(v1.xyxy)));
  r2.xyzw = vd_asuint(_BlitTexture.SampleBias(sampler_vd_linear_clamp, vd_asfloat(r1.xy), vd_asfloat(_GlobalMipBias.x)).xyzw);
  r1.xyzw = vd_asuint(_BlitTexture.SampleBias(sampler_vd_linear_clamp, vd_asfloat(r1.zw), vd_asfloat(_GlobalMipBias.x)).xyzw);
  r0.zw = vd_asuint(((-(vd_asfloat(_BlitTexture_TexelSize.xy))) * vd_asfloat(r0.yy) + vd_asfloat(v1.xy)));
  r3.xy = vd_asuint((vd_asfloat(_BlitTexture_TexelSize.xy) * vd_asfloat(r0.yy) + vd_asfloat(v1.xy)));
  r3.xyzw = vd_asuint(_BlitTexture.SampleBias(sampler_vd_linear_clamp, vd_asfloat(r3.xy), vd_asfloat(_GlobalMipBias.x)).xyzw);
  r4.xyzw = vd_asuint(_BlitTexture.SampleBias(sampler_vd_linear_clamp, vd_asfloat(r0.zw), vd_asfloat(_GlobalMipBias.x)).xyzw);
  r2.xyzw = vd_asuint((vd_asfloat(r2.xyzw) + vd_asfloat(r4.xyzw)));
  r1.xyzw = vd_asuint((vd_asfloat(r1.xyzw) + vd_asfloat(r2.xyzw)));
  r1.xyzw = vd_asuint((vd_asfloat(r3.xyzw) + vd_asfloat(r1.xyzw)));
  r2.xyzw = vd_asuint(_BlitTexture.SampleBias(sampler_vd_linear_clamp, vd_asfloat(v1.xy), vd_asfloat(_GlobalMipBias.x)).xyzw);
  r1.xyzw = vd_asuint((vd_asfloat(r1.xyzw) * float4(0.25, 0.25, 0.25, 0.25) + (-(vd_asfloat(r2.xyzw)))));
  o0.xyzw = (vd_asfloat(r0.xxxx) * vd_asfloat(r1.xyzw) + vd_asfloat(r2.xyzw));
  return;
}
