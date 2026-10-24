// LCArt/Indicator/Shader_DE_Indicator_CursorLine | pass 0 "ForwardUnlit" | progFragment | keywords: (none) | blob 3
void frag(
  float4 v0 : SV_POSITION0,
  float4 v1 : VDREG1,
  float4 v2 : VDREG2,
  out float4 o0 : SV_Target0)
{
#if UNITY_UV_STARTS_AT_TOP
#else
  float4 vd_ZBufferParamsD3D = float4(-_ZBufferParams.x, _ZBufferParams.x + _ZBufferParams.y, -_ZBufferParams.z, _ZBufferParams.z + _ZBufferParams.w);
#endif
  uint4 r0, r1;
  r0 = 0u; r1 = 0u;

  r0.xyz = vd_asuint((vd_asfloat(v2.xyz) / vd_asfloat(v2.www)));
  r1.xyzw = vd_asuint(_CustomDepthBuffer.SampleBias(sampler_CustomDepthBuffer, vd_asfloat(r0.xy), vd_asfloat(_GlobalMipBias.x)).xyzw);
#if UNITY_UV_STARTS_AT_TOP
  r0.x = vd_asuint((vd_asfloat(_ZBufferParams.z) * vd_asfloat(r0.z) + vd_asfloat(_ZBufferParams.w)));
#else
  r0.x = vd_asuint((vd_asfloat(vd_ZBufferParamsD3D.z) * vd_asfloat(r0.z) + vd_asfloat(vd_ZBufferParamsD3D.w)));
#endif
  r0.x = vd_asuint((1.0 / vd_asfloat(r0.x)));
#if UNITY_UV_STARTS_AT_TOP
  r0.y = vd_asuint((vd_asfloat(_ZBufferParams.z) * vd_asfloat(r1.x) + vd_asfloat(_ZBufferParams.w)));
#else
  r0.y = vd_asuint((vd_asfloat(vd_ZBufferParamsD3D.z) * vd_asfloat(r1.x) + vd_asfloat(vd_ZBufferParamsD3D.w)));
#endif
  r0.y = vd_asuint((1.0 / vd_asfloat(r0.y)));
  r0.x = vd_asuint(((-(vd_asfloat(r0.y))) + vd_asfloat(r0.x)));
  r0.x = vd_asuint((vd_asfloat(r0.x) + (-(vd_asfloat(_OcclusionFadeStart)))));
  r0.y = vd_asuint(((-(vd_asfloat(_OcclusionFadeStart))) + vd_asfloat(_OcclusionFadeEnd)));
  r0.y = vd_asuint(max(vd_asfloat(r0.y), 9.99999975e-05));
  r0.x = vd_asuint(saturate((vd_asfloat(r0.x) / vd_asfloat(r0.y))));
  r0.x = vd_asuint(((-(vd_asfloat(r0.x))) + 1.0));
  r0.y = vd_asuint(((-(vd_asfloat(_OccludedAlpha))) + 1.0));
  r0.x = vd_asuint((vd_asfloat(r0.x) * vd_asfloat(r0.y) + vd_asfloat(_OccludedAlpha)));
  r1.xyzw = vd_asuint(_MainTex.SampleBias(sampler_MainTex, vd_asfloat(v1.xy), vd_asfloat(_GlobalMipBias.x)).xyzw);
  r1.xyzw = vd_asuint((vd_asfloat(r1.xyzw) * vd_asfloat(_Color.xyzw)));
  o0.w = (vd_asfloat(r0.x) * vd_asfloat(r1.w));
  o0.xyz = vd_asfloat(r1.xyz);
  return;
}
