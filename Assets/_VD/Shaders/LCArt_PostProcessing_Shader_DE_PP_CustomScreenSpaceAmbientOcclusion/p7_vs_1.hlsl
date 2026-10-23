// LCArt/PostProcessing/Shader_DE_PP_CustomScreenSpaceAmbientOcclusion | pass 7 "SSAO_Gaussian_VerticalBlur_AfterOpaque" | progVertex | keywords: (none) | blob 1
void vert(
  uint v0 : SV_VertexID0,
  out float4 o0 : SV_POSITION0,
  out float4 o1 : VDREG1)
{
  uint4 r0;
  r0 = 0u;

  r0.x = (vd_asuint(v0.x) << (uint)1);
  r0.x = (r0.x & 0x00000002u);
  r0.w = (vd_asuint(v0.x) & 0x00000002u);
  r0.xy = vd_asuint(((float2)r0.xw));
  o0.xy = (vd_asfloat(r0.xy) * float2(2.0, 2.0) + float2(-1.0, -1.0));
  r0.z = vd_asuint(((-(vd_asfloat(r0.y))) + 1.0));
  o1.xy = (vd_asfloat(r0.xz) * vd_asfloat(_BlitScaleBias.xy) + vd_asfloat(_BlitScaleBias.zw));
  o0.zw = vd_asfloat(uint2(0x3f800000u, 0x3f800000u));
  return;
}
