// Hidden/LCArt/PostProcessing/Shader_PP_ScreenBlurUnified | pass 0 "KawaseBlurPass" | progFragment | keywords: (none) | blob 2
void frag(
  float4 v0 : SV_POSITION0,
  float4 v1 : VDREG1,
  out float4 o0 : SV_Target0)
{
  uint4 r0, r1, r2;
  r0 = 0u; r1 = 0u; r2 = 0u;
  const uint4 icb[7] = { uint4(0x00000000u, 0x00000000u, 0x00000000u, 0x00000000u), uint4(0x3e000000u, 0x00000000u, 0x00000000u, 0x00000000u), uint4(0x3e800000u, 0x00000000u, 0x00000000u, 0x00000000u), uint4(0x3ec00000u, 0x00000000u, 0x00000000u, 0x00000000u), uint4(0x3f000000u, 0x00000000u, 0x00000000u, 0x00000000u), uint4(0x3f200000u, 0x00000000u, 0x00000000u, 0x00000000u), uint4(0x3f400000u, 0x00000000u, 0x00000000u, 0x00000000u) };

  r0.x = vd_asuint(max(vd_asint(_UnifiedDepthBlurDegree), vd_asint(_UnifiedHeightBlurDegree)));
  r0.x = vd_asuint(max(vd_asint(r0.x), 0));
  r0.x = vd_asuint(min(vd_asint(r0.x), 6));
  if (r0.x == 0u) {
    r1.xyzw = vd_asuint(_BlitTexture.SampleBias(sampler_vd_linear_clamp, vd_asfloat(v1.xy), vd_asfloat(_GlobalMipBias.x)).xyzw);
    o0.xyz = vd_asfloat(r1.xyz);
    o0.w = vd_asfloat(0x3f800000u);
    return;
  }
  r0.y = vd_asuint((vd_asfloat(v1.y) + (-(vd_asfloat(_UnifiedHeightBlurMaxRatio)))));
  r0.z = vd_asuint(((-(vd_asfloat(_UnifiedHeightBlurMaxRatio))) + 1.0));
  r0.y = vd_asuint((vd_asfloat(r0.y) / vd_asfloat(r0.z)));
  r0.y = vd_asuint(max(vd_asfloat(r0.y), 0.0));
  r0.z = vd_asuint(((-(vd_asfloat(v1.y))) + vd_asfloat(_UnifiedHeightBlurMinRatio)));
  r0.z = vd_asuint((vd_asfloat(r0.z) / vd_asfloat(_UnifiedHeightBlurMinRatio)));
  r0.y = vd_asuint(max(vd_asfloat(r0.z), vd_asfloat(r0.y)));
  r1.xyzw = vd_asuint(_UnifiedBlurDepthTexture.SampleBias(sampler_UnifiedBlurDepthTexture, vd_asfloat(v1.xy), vd_asfloat(_GlobalMipBias.x)).xyzw);
  r0.zw = vd_asuint((vd_asfloat(v1.xy) * float2(2.0, 2.0) + float2(-1.0, -1.0)));
  r2.xyzw = vd_asuint(((-(vd_asfloat(r0.wwww))) * vd_asfloat((VD_COL(unity_MatrixInvP, 1).xyzw))));
  r2.xyzw = vd_asuint((vd_asfloat((VD_COL(unity_MatrixInvP, 0).xyzw)) * vd_asfloat(r0.zzzz) + vd_asfloat(r2.xyzw)));
  r1.xyzw = vd_asuint((vd_asfloat((VD_COL(unity_MatrixInvP, 2).xyzw)) * vd_asfloat(r1.xxxx) + vd_asfloat(r2.xyzw)));
  r1.xyzw = vd_asuint((vd_asfloat(r1.xyzw) + vd_asfloat((VD_COL(unity_MatrixInvP, 3).xyzw))));
  r1.xyz = vd_asuint((vd_asfloat(r1.xyz) * float3(1.0, 1.0, -1.0)));
  r1.xyz = vd_asuint((vd_asfloat(r1.xyz) / vd_asfloat(r1.www)));
  r0.z = vd_asuint((vd_asfloat(r1.y) * vd_asfloat((VD_COL(unity_MatrixInvV, 1).y))));
  r0.z = vd_asuint((vd_asfloat((VD_COL(unity_MatrixInvV, 0).y)) * vd_asfloat(r1.x) + vd_asfloat(r0.z)));
  r0.z = vd_asuint((vd_asfloat((VD_COL(unity_MatrixInvV, 2).y)) * (-(vd_asfloat(r1.z))) + vd_asfloat(r0.z)));
  r0.z = vd_asuint((vd_asfloat(r0.z) + vd_asfloat((VD_COL(unity_MatrixInvV, 3).y))));
  r0.w = vd_asuint(((-(vd_asfloat(_UnifiedDepthBlurMaxDistance))) + vd_asfloat(_UnifiedDepthBlurMinDistance)));
  r0.z = vd_asuint((vd_asfloat(r0.z) + (-(vd_asfloat(_UnifiedDepthBlurMaxDistance)))));
  r0.w = vd_asuint((1.0 / vd_asfloat(r0.w)));
  r0.z = vd_asuint(saturate((vd_asfloat(r0.w) * vd_asfloat(r0.z))));
  r0.w = vd_asuint((vd_asfloat(r0.z) * -2.0 + 3.0));
  r0.z = vd_asuint((vd_asfloat(r0.z) * vd_asfloat(r0.z)));
  r0.z = vd_asuint((vd_asfloat(r0.z) * vd_asfloat(r0.w)));
  r0.y = vd_asuint(max(vd_asfloat(r0.z), vd_asfloat(r0.y)));
  r0.z = vd_asuint((vd_asfloat(_KawaseOffset) + 0.5));
  r0.x = vd_asuint((vd_asfloat(r0.z) * vd_asfloat(icb[r0.x + 0].x)));
  r0.x = vd_asuint((vd_asfloat(r0.y) * vd_asfloat(r0.x)));
  r0.y = vd_asuint((vd_asfloat(r0.x) * 0.00092592591));
  r0.xzw = vd_asuint((-(vd_asfloat(r0.yyy))));
  r1.xyzw = vd_asuint((vd_asfloat(r0.xxyz) + vd_asfloat(v1.xyxy)));
  r2.xyzw = vd_asuint(_BlitTexture.SampleBias(sampler_vd_linear_clamp, vd_asfloat(r1.xy), vd_asfloat(_GlobalMipBias.x)).xyzw);
  r1.xyzw = vd_asuint(_BlitTexture.SampleBias(sampler_vd_linear_clamp, vd_asfloat(r1.zw), vd_asfloat(_GlobalMipBias.x)).xyzw);
  r1.xyz = vd_asuint((vd_asfloat(r1.xyz) + vd_asfloat(r2.xyz)));
  r0.xyzw = vd_asuint((vd_asfloat(r0.wyyy) + vd_asfloat(v1.xyxy)));
  r2.xyzw = vd_asuint(_BlitTexture.SampleBias(sampler_vd_linear_clamp, vd_asfloat(r0.xy), vd_asfloat(_GlobalMipBias.x)).xyzw);
  r1.xyz = vd_asuint((vd_asfloat(r1.xyz) + vd_asfloat(r2.xyz)));
  r0.xyzw = vd_asuint(_BlitTexture.SampleBias(sampler_vd_linear_clamp, vd_asfloat(r0.zw), vd_asfloat(_GlobalMipBias.x)).xyzw);
  r0.xyz = vd_asuint((vd_asfloat(r0.xyz) + vd_asfloat(r1.xyz)));
  o0.xyz = (vd_asfloat(r0.xyz) * float3(0.25, 0.25, 0.25));
  o0.w = vd_asfloat(0x3f800000u);
  return;
}
