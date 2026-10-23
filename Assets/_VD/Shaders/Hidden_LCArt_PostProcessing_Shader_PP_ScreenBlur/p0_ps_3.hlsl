// Hidden/LCArt/PostProcessing/Shader_PP_ScreenBlur | pass 0 "BlurPassVertical" | progFragment | keywords: (none) | blob 3
void frag(
  float4 v0 : SV_POSITION0,
  float4 v1 : VDREG1,
  out float4 o0 : SV_Target0)
{
  uint4 r0, r1, r2, r3, r4;
  r0 = 0u; r1 = 0u; r2 = 0u; r3 = 0u; r4 = 0u;
  const uint4 icb[24] = { uint4(0x00000001u, 0x00000000u, 0x3f800000u, 0x00000000u), uint4(0x00000003u, 0x3aa3d70au, 0x3ee75a96u, 0x00000000u), uint4(0x00000005u, 0x3b23d70au, 0x3e94cd75u, 0x00000000u), uint4(0x00000007u, 0x3ba3d70au, 0x3e405e1cu, 0x3f800000u), uint4(0x00000009u, 0x3bf5c28fu, 0x3e1dcbdeu, 0x00000000u), uint4(0x0000000bu, 0x3c23d70au, 0x3e2b302fu, 0x00000000u), uint4(0x00000000u, 0x00000000u, 0x00000000u, 0x3fababa8u), uint4(0x00000000u, 0x00000000u, 0x00000000u, 0x00000000u), uint4(0x00000000u, 0x00000000u, 0x00000000u, 0x00000000u), uint4(0x3e8c52c6u, 0x00000000u, 0x00000000u, 0x3fb9d356u), uint4(0x00000000u, 0x00000000u, 0x00000000u, 0x40400000u), uint4(0x00000000u, 0x00000000u, 0x00000000u, 0x00000000u), uint4(0x3eb59946u, 0x00000000u, 0x00000000u, 0x3fbb6e83u), uint4(0x00000000u, 0x00000000u, 0x00000000u, 0x405708d5u), uint4(0x00000000u, 0x00000000u, 0x00000000u, 0x00000000u), uint4(0x3ea68555u, 0x00000000u, 0x00000000u, 0x3fb57c1cu), uint4(0x3da58cd2u, 0x00000000u, 0x00000000u, 0x40597e80u), uint4(0x00000000u, 0x00000000u, 0x00000000u, 0x40a00000u), uint4(0x3e8e379bu, 0x00000000u, 0x00000000u, 0x00000000u), uint4(0x3e14aadau, 0x00000000u, 0x00000000u, 0x00000000u), uint4(0x00000000u, 0x00000000u, 0x00000000u, 0x00000000u), uint4(0x3e855433u, 0x00000000u, 0x00000000u, 0x00000000u), uint4(0x3dfda37fu, 0x00000000u, 0x00000000u, 0x00000000u), uint4(0x3d03b70fu, 0x00000000u, 0x00000000u, 0x00000000u) };

  r0.x = vd_asuint(max(vd_asint(_ScreenBlurDegree), 0));
  r0.x = vd_asuint(min(vd_asint(r0.x), 5));
  r0.y = vd_asuint(((float)vd_asint(icb[r0.x + 0].x)));
  r0.y = vd_asuint((vd_asfloat(r0.y) * 0.5));
  r0.z = vd_asuint((vd_asfloat(_ScreenParams.x) / vd_asfloat(_ScreenParams.y)));
  r0.z = vd_asuint((vd_asfloat(r0.z) * vd_asfloat(icb[r0.x + 0].y)));
  r0.w = vd_asuint(saturate(vd_asfloat(_ScreenBlurProgress)));
  r0.z = vd_asuint((vd_asfloat(r0.w) * vd_asfloat(r0.z)));
  r0.y = vd_asuint((vd_asfloat(r0.z) / vd_asfloat(r0.y)));
  r1.xyzw = vd_asuint(_BlitTexture.SampleBias(sampler_vd_linear_clamp, vd_asfloat(v1.xy), vd_asfloat(_GlobalMipBias.x)).xyzw);
  r1.xyz = vd_asuint((vd_asfloat(r1.xyz) * vd_asfloat(icb[r0.x + 0].zzz)));
  r2.xyz = vd_asuint((-(int3)(vd_asint(r0.xxx) >= int3(1, 3, 5))));
  if (r2.x != 0u) {
    r0.z = vd_asuint((vd_asint(r0.x) * 3));
    r3.y = vd_asuint((vd_asfloat(r0.y) * vd_asfloat(icb[r0.z + 0].w)));
    r3.x = 0x00000000u;
    r2.xw = vd_asuint((vd_asfloat(r3.xy) + vd_asfloat(v1.xy)));
    r4.xyzw = vd_asuint(_BlitTexture.SampleBias(sampler_vd_linear_clamp, vd_asfloat(r2.xw), vd_asfloat(_GlobalMipBias.x)).xyzw);
    r4.xyz = vd_asuint((vd_asfloat(r4.xyz) * vd_asfloat(icb[r0.z + 6].xxx) + vd_asfloat(r1.xyz)));
    r2.xw = vd_asuint(((-(vd_asfloat(r3.xy))) + vd_asfloat(v1.xy)));
    r3.xyzw = vd_asuint(_BlitTexture.SampleBias(sampler_vd_linear_clamp, vd_asfloat(r2.xw), vd_asfloat(_GlobalMipBias.x)).xyzw);
    r1.xyz = vd_asuint((vd_asfloat(r3.xyz) * vd_asfloat(icb[r0.z + 6].xxx) + vd_asfloat(r4.xyz)));
  }
  if (r2.y != 0u) {
    r0.x = vd_asuint((vd_asint(r0.x) * 3));
    r2.y = vd_asuint((vd_asfloat(r0.y) * vd_asfloat(icb[r0.x + 1].w)));
    r2.x = 0x00000000u;
    r0.zw = vd_asuint((vd_asfloat(r2.xy) + vd_asfloat(v1.xy)));
    r3.xyzw = vd_asuint(_BlitTexture.SampleBias(sampler_vd_linear_clamp, vd_asfloat(r0.zw), vd_asfloat(_GlobalMipBias.x)).xyzw);
    r3.xyz = vd_asuint((vd_asfloat(r3.xyz) * vd_asfloat(icb[r0.x + 7].xxx) + vd_asfloat(r1.xyz)));
    r0.zw = vd_asuint(((-(vd_asfloat(r2.xy))) + vd_asfloat(v1.xy)));
    r4.xyzw = vd_asuint(_BlitTexture.SampleBias(sampler_vd_linear_clamp, vd_asfloat(r0.zw), vd_asfloat(_GlobalMipBias.x)).xyzw);
    r1.xyz = vd_asuint((vd_asfloat(r4.xyz) * vd_asfloat(icb[r0.x + 7].xxx) + vd_asfloat(r3.xyz)));
  }
  if (r2.z != 0u) {
    r0.y = vd_asuint((vd_asfloat(r0.y) * 5.0));
    r0.x = 0x00000000u;
    r0.zw = vd_asuint((vd_asfloat(r0.xy) + vd_asfloat(v1.xy)));
    r2.xyzw = vd_asuint(_BlitTexture.SampleBias(sampler_vd_linear_clamp, vd_asfloat(r0.zw), vd_asfloat(_GlobalMipBias.x)).xyzw);
    r2.xyz = vd_asuint((vd_asfloat(r2.xyz) * float3(0.0321570002, 0.0321570002, 0.0321570002) + vd_asfloat(r1.xyz)));
    r0.xy = vd_asuint(((-(vd_asfloat(r0.xy))) + vd_asfloat(v1.xy)));
    r0.xyzw = vd_asuint(_BlitTexture.SampleBias(sampler_vd_linear_clamp, vd_asfloat(r0.xy), vd_asfloat(_GlobalMipBias.x)).xyzw);
    r1.xyz = vd_asuint((vd_asfloat(r0.xyz) * float3(0.0321570002, 0.0321570002, 0.0321570002) + vd_asfloat(r2.xyz)));
  }
  o0.xyz = vd_asfloat(r1.xyz);
  o0.w = vd_asfloat(0x3f800000u);
  return;
}
