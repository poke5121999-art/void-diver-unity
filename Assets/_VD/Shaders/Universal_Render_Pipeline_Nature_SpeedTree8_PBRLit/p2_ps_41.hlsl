// Universal Render Pipeline/Nature/SpeedTree8_PBRLit | pass 2 "DepthOnly" | progFragment | keywords: BACKFACE_NORMAL_MODE_FLIP | blob 41
void frag(
  float4 v0 : SV_POSITION0,
  float4 v1 : VDREG1,
  float4 v2 : VDREG2,
  float4 v3 : VDREG3,
  float4 v4 : VDREG4,
  float4 v5 : VDREG5,
  float4 v6 : VDREG6,
  float4 v7 : VDREG7,
  uint v8 : SV_IsFrontFace0,
  out float4 o0 : SV_TARGET0)
{
  uint4 r0;
  r0 = 0u;

  r0.xyzw = vd_asuint(_MainTex.SampleBias(sampler_MainTex, vd_asfloat(v2.xy), vd_asfloat(_GlobalMipBias.x)).xyzw);
  r0.x = vd_asuint((vd_asfloat(v3.w) * vd_asfloat(r0.w) + (-(vd_asfloat(_AlphaClipThreshold)))));
  r0.x = vd_asuint((-(int)(vd_asfloat(r0.x) < 0.0)));
  if (r0.x != 0u) discard;
  o0.xyzw = vd_asfloat(vd_asuint(v0.zzzz));
  return;
}
