// Universal Render Pipeline/Nature/SpeedTree8_PBRLit | pass 1 "MotionVectors" | progFragment | keywords: BACKFACE_NORMAL_MODE_MIRROR | blob 36
void frag(
  float4 v0 : VDREG0,
  float4 v1 : VDREG1,
  float4 v2 : SV_POSITION0,
  float4 v3 : VDREG3,
  float4 v4 : VDREG4,
  float4 v5 : VDREG5,
  float4 v6 : VDREG6,
  float4 v7 : VDREG7,
  float4 v8 : VDREG8,
  float4 v9 : VDREG9,
  uint v10 : SV_IsFrontFace0,
  out float4 o0 : SV_Target0)
{
  uint4 r0;
  r0 = 0u;

  r0.xyzw = vd_asuint(_MainTex.SampleBias(sampler_MainTex, vd_asfloat(v4.xy), vd_asfloat(_GlobalMipBias.x)).xyzw);
  r0.x = vd_asuint((vd_asfloat(v5.w) * vd_asfloat(r0.w) + (-(vd_asfloat(_AlphaClipThreshold)))));
  r0.x = vd_asuint((-(int)(vd_asfloat(r0.x) < 0.0)));
  if (r0.x != 0u) discard;
  r0.x = vd_asuint((1.0 / vd_asfloat(v1.z)));
  r0.xy = vd_asuint((vd_asfloat(r0.xx) * vd_asfloat(v1.xy)));
  r0.z = vd_asuint((1.0 / vd_asfloat(v0.z)));
  r0.xy = vd_asuint((vd_asfloat(v0.xy) * vd_asfloat(r0.zz) + (-(vd_asfloat(r0.xy)))));
  r0.xy = vd_asuint((vd_asfloat(r0.xy) * float2(0.5, -0.5)));
  r0.z = vd_asuint((-(int)(vd_asfloat(unity_MotionVectorsParams.y) != 0.0)));
  o0.xy = vd_asfloat((r0.xy & r0.zz));
  o0.zw = vd_asfloat(uint2(0x00000000u, 0x00000000u));
  return;
}
