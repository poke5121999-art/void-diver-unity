// Hidden/LCArt/PostProcessing/Shader_PP_LateCustomScreenUI | pass 0 "DrawProcedural" | progFragment | keywords: (none) | blob 2
void frag(
  float4 v0 : SV_POSITION0,
  float4 v1 : VDREG1,
  float4 v2 : VDREG2,
  out float4 o0 : SV_TARGET0)
{
  uint4 r0, r1;
  r0 = 0u; r1 = 0u;

  r0.xy = vd_asuint((vd_asfloat(v1.xy) * vd_asfloat(_ScreenSize.xy)));
  r0.xy = ((uint2)vd_asfloat(r0.xy));
  r0.zw = uint2(0x00000000u, 0x00000000u);
  r0.xyzw = vd_asuint(_BlitTexture.Load(vd_asint(r0.xyw)).xyzw);
  r1.xyzw = vd_asuint(_CustomScreenUIBuffer.SampleBias(sampler_CustomScreenUIBuffer, vd_asfloat(v1.xy), vd_asfloat(_GlobalMipBias.x)).xyzw);
  r1.xyz = vd_asuint(((-(vd_asfloat(r0.xyz))) + vd_asfloat(r1.xyz)));
  o0.xyz = (vd_asfloat(r1.www) * vd_asfloat(r1.xyz) + vd_asfloat(r0.xyz));
  o0.w = vd_asfloat(0x3f800000u);
  return;
}
