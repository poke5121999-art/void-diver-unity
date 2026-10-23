// LCArt/UI/Shader_DE_UI_DitheringScreen_Optimized | pass 0 "DrawProcedural" | progFragment | keywords: (none) | blob 3
void frag(
  float4 v0 : SV_POSITION0,
  float4 v1 : VDREG1,
  out float4 o0 : SV_Target0)
{
  uint4 r0, r1, r2;
  r0 = 0u; r1 = 0u; r2 = 0u;
  const uint4 icb[16] = { uint4(0x3d70f0f1u, 0x00000000u, 0x00000000u, 0x00000000u), uint4(0x3f078787u, 0x00000000u, 0x00000000u, 0x00000000u), uint4(0x3e34b4b5u, 0x00000000u, 0x00000000u, 0x00000000u), uint4(0x3f25a5a6u, 0x00000000u, 0x00000000u, 0x00000000u), uint4(0x3f43c3c4u, 0x00000000u, 0x00000000u, 0x00000000u), uint4(0x3e969697u, 0x00000000u, 0x00000000u, 0x00000000u), uint4(0x3f61e1e2u, 0x00000000u, 0x00000000u, 0x00000000u), uint4(0x3ed2d2d3u, 0x00000000u, 0x00000000u, 0x00000000u), uint4(0x3e70f0f1u, 0x00000000u, 0x00000000u, 0x00000000u), uint4(0x3f34b4b5u, 0x00000000u, 0x00000000u, 0x00000000u), uint4(0x3df0f0f1u, 0x00000000u, 0x00000000u, 0x00000000u), uint4(0x3f169697u, 0x00000000u, 0x00000000u, 0x00000000u), uint4(0x3f70f0f1u, 0x00000000u, 0x00000000u, 0x00000000u), uint4(0x3ef0f0f1u, 0x00000000u, 0x00000000u, 0x00000000u), uint4(0x3f52d2d3u, 0x00000000u, 0x00000000u, 0x00000000u), uint4(0x3eb4b4b5u, 0x00000000u, 0x00000000u, 0x00000000u) };

  r0.x = vd_asuint((vd_asfloat(_DitheringScale) + vd_asfloat(_DitheringScale)));
  r1.x = vd_asuint((vd_asfloat(_Time.y) * vd_asfloat(_DitheringSpeed)));
  r1.z = vd_asuint((vd_asfloat(r1.x) * -0.5));
  r1.y = 0x00000000u;
  r0.yz = vd_asuint((vd_asfloat(r1.zy) + vd_asfloat(v1.xy)));
  r1.xy = vd_asuint((vd_asfloat(r1.xy) + vd_asfloat(v1.xy)));
  r1.xy = vd_asuint((vd_asfloat(r1.xy) * vd_asfloat((_DitheringScale).xx)));
  r1.xy = vd_asuint((vd_asfloat(r1.xy) * float2(0.125, 0.125)));
  r1.xyzw = vd_asuint(_NoiseTex.SampleBias(sampler_vd_linear_repeat, vd_asfloat(r1.xy), vd_asfloat(_GlobalMipBias.x)).xyzw);
  r0.xy = vd_asuint((vd_asfloat(r0.xx) * vd_asfloat(r0.yz)));
  r0.xy = vd_asuint((vd_asfloat(r0.xy) * float2(0.125, 0.125)));
  r0.xyzw = vd_asuint(_NoiseTex.SampleBias(sampler_vd_linear_repeat, vd_asfloat(r0.xy), vd_asfloat(_GlobalMipBias.x)).xyzw);
  r0.x = vd_asuint((vd_asfloat(r0.x) * vd_asfloat(r1.x)));
  r0.yz = vd_asuint((vd_asfloat(v1.xy) * vd_asfloat(_ScreenParams.xy)));
  r0.yz = vd_asuint((vd_asfloat(r0.yz) * float2(0.5, 0.5)));
  r0.yz = ((uint2)vd_asfloat(r0.yz));
  r0.y = (r0.y << (uint)2);
  r0.yz = (r0.yz & uint2(0x0000000cu, 0x00000003u));
  r0.y = vd_asuint((vd_asint(r0.z) + vd_asint(r0.y)));
  r0.y = vd_asuint((0.75 + (-(vd_asfloat(icb[r0.y + 0].x)))));
  r0.x = vd_asuint((-(int)(vd_asfloat(r0.x) >= vd_asfloat(r0.y))));
  r0.x = (r0.x & 0x3f800000u);
  r0.yz = vd_asuint((vd_asfloat(v1.xy) * vd_asfloat(_ScreenSize.xy)));
  r1.xy = ((uint2)vd_asfloat(r0.yz));
  r1.zw = uint2(0x00000000u, 0x00000000u);
  r1.xyzw = vd_asuint(_BlitTexture.Load(vd_asint(r1.xyw)).xyzw);
  r0.yzw = vd_asuint((vd_asfloat(r1.xyz) * vd_asfloat(_Color.xyz)));
  r2.xyz = vd_asuint((vd_asfloat(r1.xyz) * vd_asfloat(_DitheringColor.xyz) + (-(vd_asfloat(r0.yzw)))));
  r0.xyz = vd_asuint((vd_asfloat(r0.xxx) * vd_asfloat(r2.xyz) + vd_asfloat(r0.yzw)));
  r0.xyz = vd_asuint(((-(vd_asfloat(r1.xyz))) + vd_asfloat(r0.xyz)));
  o0.xyz = (vd_asfloat((_ScreenBlurCustomShaderProgress).xxx) * vd_asfloat(r0.xyz) + vd_asfloat(r1.xyz));
  o0.w = vd_asfloat(0x3f800000u);
  return;
}
