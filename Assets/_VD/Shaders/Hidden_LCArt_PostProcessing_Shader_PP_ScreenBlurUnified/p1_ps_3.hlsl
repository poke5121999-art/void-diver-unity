// Hidden/LCArt/PostProcessing/Shader_PP_ScreenBlurUnified | pass 1 "CompositePass" | progFragment | keywords: (none) | blob 3
void frag(
  float4 v0 : SV_POSITION0,
  float4 v1 : VDREG1,
  out float4 o0 : SV_Target0)
{
#if UNITY_UV_STARTS_AT_TOP
#else
  float4x4 vd_MatrixInvPD3D = mul(unity_MatrixInvP, float4x4(1, 0, 0, 0, 0, 1, 0, 0, 0, 0, -2, 1, 0, 0, 0, 1));
#endif
  uint4 r0, r1, r2;
  r0 = 0u; r1 = 0u; r2 = 0u;

  r0.xy = vd_asuint((vd_asfloat(v1.xy) * float2(2.0, 2.0) + float2(-1.0, -1.0)));
#if UNITY_UV_STARTS_AT_TOP
  r1.xyzw = vd_asuint(((-(vd_asfloat(r0.yyyy))) * vd_asfloat((VD_COL(unity_MatrixInvP, 1).xyzw))));
  r0.xyzw = vd_asuint((vd_asfloat((VD_COL(unity_MatrixInvP, 0).xyzw)) * vd_asfloat(r0.xxxx) + vd_asfloat(r1.xyzw)));
#else
  r1.xyzw = vd_asuint((vd_asfloat(r0.yyyy) * vd_asfloat((VD_COL(vd_MatrixInvPD3D, 1).xyzw))));
  r0.xyzw = vd_asuint((vd_asfloat((VD_COL(vd_MatrixInvPD3D, 0).xyzw)) * vd_asfloat(r0.xxxx) + vd_asfloat(r1.xyzw)));
#endif
  r1.xyzw = vd_asuint(_UnifiedBlurDepthTexture.SampleBias(sampler_UnifiedBlurDepthTexture, vd_asfloat(v1.xy), vd_asfloat(_GlobalMipBias.x)).xyzw);
#if UNITY_UV_STARTS_AT_TOP
  r0.xyzw = vd_asuint((vd_asfloat((VD_COL(unity_MatrixInvP, 2).xyzw)) * vd_asfloat(r1.xxxx) + vd_asfloat(r0.xyzw)));
  r0.xyzw = vd_asuint((vd_asfloat(r0.xyzw) + vd_asfloat((VD_COL(unity_MatrixInvP, 3).xyzw))));
#else
  r1.x = vd_asuint(((-(vd_asfloat(r1.x))) + 1.0));
  r0.xyzw = vd_asuint((vd_asfloat((VD_COL(vd_MatrixInvPD3D, 2).xyzw)) * vd_asfloat(r1.xxxx) + vd_asfloat(r0.xyzw)));
  r0.xyzw = vd_asuint((vd_asfloat(r0.xyzw) + vd_asfloat((VD_COL(vd_MatrixInvPD3D, 3).xyzw))));
#endif
  r0.xyz = vd_asuint((vd_asfloat(r0.xyz) * float3(1.0, 1.0, -1.0)));
  r0.xyz = vd_asuint((vd_asfloat(r0.xyz) / vd_asfloat(r0.www)));
  r0.y = vd_asuint((vd_asfloat(r0.y) * vd_asfloat((VD_COL(unity_MatrixInvV, 1).y))));
  r0.x = vd_asuint((vd_asfloat((VD_COL(unity_MatrixInvV, 0).y)) * vd_asfloat(r0.x) + vd_asfloat(r0.y)));
  r0.x = vd_asuint((vd_asfloat((VD_COL(unity_MatrixInvV, 2).y)) * (-(vd_asfloat(r0.z))) + vd_asfloat(r0.x)));
  r0.x = vd_asuint((vd_asfloat(r0.x) + vd_asfloat((VD_COL(unity_MatrixInvV, 3).y))));
  r0.x = vd_asuint((vd_asfloat(r0.x) + (-(vd_asfloat(_UnifiedDepthBlurMaxDistance)))));
  r0.y = vd_asuint(((-(vd_asfloat(_UnifiedDepthBlurMaxDistance))) + vd_asfloat(_UnifiedDepthBlurMinDistance)));
  r0.y = vd_asuint((1.0 / vd_asfloat(r0.y)));
  r0.x = vd_asuint(saturate((vd_asfloat(r0.y) * vd_asfloat(r0.x))));
  r0.y = vd_asuint((vd_asfloat(r0.x) * -2.0 + 3.0));
  r0.x = vd_asuint((vd_asfloat(r0.x) * vd_asfloat(r0.x)));
  r0.x = vd_asuint((vd_asfloat(r0.x) * vd_asfloat(r0.y)));
  r0.y = vd_asuint((vd_asfloat(v1.y) + (-(vd_asfloat(_UnifiedHeightBlurMaxRatio)))));
  r0.z = vd_asuint(((-(vd_asfloat(_UnifiedHeightBlurMaxRatio))) + 1.0));
  r0.y = vd_asuint((vd_asfloat(r0.y) / vd_asfloat(r0.z)));
  r0.y = vd_asuint(max(vd_asfloat(r0.y), 0.0));
  r0.z = vd_asuint(((-(vd_asfloat(v1.y))) + vd_asfloat(_UnifiedHeightBlurMinRatio)));
  r0.z = vd_asuint((vd_asfloat(r0.z) / vd_asfloat(_UnifiedHeightBlurMinRatio)));
  r0.y = vd_asuint(max(vd_asfloat(r0.z), vd_asfloat(r0.y)));
  r0.x = vd_asuint(max(vd_asfloat(r0.x), vd_asfloat(r0.y)));
  r0.y = vd_asuint(max(vd_asint(_UnifiedDepthBlurDegree), vd_asint(_UnifiedHeightBlurDegree)));
  r0.y = vd_asuint(max(vd_asint(r0.y), 0));
  r0.y = vd_asuint(min(vd_asint(r0.y), 6));
  r0.x = ((r0.y != 0u) ? r0.x : 0x00000000u);
  r1.xyzw = vd_asuint(_BlitTexture.SampleBias(sampler_vd_linear_clamp, vd_asfloat(v1.xy), vd_asfloat(_GlobalMipBias.x)).xyzw);
  r2.xyzw = vd_asuint(_OriginalCameraColor.SampleBias(sampler_OriginalCameraColor, vd_asfloat(v1.xy), vd_asfloat(_GlobalMipBias.x)).xyzw);
  r0.yzw = vd_asuint((vd_asfloat(r1.xyz) + (-(vd_asfloat(r2.xyz)))));
  o0.xyz = (vd_asfloat(r0.xxx) * vd_asfloat(r0.yzw) + vd_asfloat(r2.xyz));
  o0.w = vd_asfloat(0x3f800000u);
  return;
}
