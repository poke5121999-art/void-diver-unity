// LCArt/Background/Shader_DE_WaterBottom | pass 0 "ForwardLit" | progFragment | keywords: FOG_LINEAR _LIGHT_LAYERS | blob 31
void frag(
  float4 v0 : VDREG0,
  float4 v1 : VDREG1,
  float4 v2 : VDREG2,
  float4 v3 : VDREG3,
  float4 v4 : VDREG4,
  float4 v5 : VDREG5,
  float4 v6 : SV_POSITION0,
  float4 v7 : VDREG7,
  float4 v8 : VDREG8,
  out float4 o0 : SV_Target0)
{
  uint4 r0, r1, r2, r3;
  r0 = 0u; r1 = 0u; r2 = 0u; r3 = 0u;

  r0.xy = vd_asuint((vd_asfloat(v1.xz) * vd_asfloat(_GlobalWaterWaveParams.yy)));
  r0.z = vd_asuint((vd_asfloat(_Time.y) * vd_asfloat(_GlobalWaterWaveParams.x)));
  r1.xyz = vd_asuint((vd_asfloat(r0.zzz) * float3(0.707099974, -0.400000006, 0.692799985)));
  r0.zw = vd_asuint((vd_asfloat(v1.xz) * vd_asfloat(_GlobalWaterWaveParams.yy) + vd_asfloat(r1.xx)));
  r0.xy = vd_asuint((vd_asfloat(r0.xy) * float2(0.699999988, 0.699999988) + vd_asfloat(r1.yz)));
  r1.xy = vd_asuint((vd_asfloat(v7.xy) / vd_asfloat(v7.ww)));
  r1.zw = vd_asuint(((-(vd_asfloat(_CameraDepthTexture_TexelSize.xy))) * float2(0.5, 0.5) + float2(1.0, 1.0)));
  r1.zw = vd_asuint(min(vd_asfloat(r1.zw), vd_asfloat(r1.xy)));
  r1.zw = vd_asuint((vd_asfloat(r1.zw) * vd_asfloat(_RTHandleScale.xy)));
  r2.xyzw = vd_asuint(_CameraDepthTexture.SampleBias(sampler_vd_point_clamp, vd_asfloat(r1.zw), vd_asfloat(_GlobalMipBias.x)).xyzw);
  r1.z = vd_asuint((vd_asfloat(_ZBufferParams.z) * vd_asfloat(r2.x) + vd_asfloat(_ZBufferParams.w)));
  r1.z = vd_asuint((1.0 / vd_asfloat(r1.z)));
  r2.xyzw = vd_asuint(_GlobalWaterNoiseTex.SampleBias(sampler_vd_linear_repeat, vd_asfloat(r0.zw), vd_asfloat(_GlobalMipBias.x)).xyzw);
  r0.xyzw = vd_asuint(_GlobalWaterNoiseTex.SampleBias(sampler_vd_linear_repeat, vd_asfloat(r0.xy), vd_asfloat(_GlobalMipBias.x)).xyzw);
  r0.y = vd_asuint(min(vd_asfloat(r0.x), vd_asfloat(r2.x)));
  r0.y = vd_asuint(log2(abs(vd_asfloat(r0.y))));
  r0.y = vd_asuint((vd_asfloat(r0.y) * vd_asfloat(_GlobalWaterBottomEdgeParams.x)));
  r0.y = vd_asuint(exp2(vd_asfloat(r0.y)));
  r0.y = vd_asuint(min(vd_asfloat(r0.y), 1.0));
  r0.y = vd_asuint((vd_asfloat(r0.y) * vd_asfloat(_GlobalWaterBottomEdgeParams.y)));
  r0.x = vd_asuint((vd_asfloat(r0.x) + vd_asfloat(r2.x)));
  r0.z = vd_asuint((vd_asfloat(r1.z) + (-(vd_asfloat(v7.w)))));
  r0.z = vd_asuint((vd_asfloat(r0.x) * 0.0500000007 + vd_asfloat(r0.z)));
  r0.w = vd_asuint((vd_asfloat(r0.z) + (-(vd_asfloat(_GlobalWaterBottomEdgeParams.w)))));
  r1.z = vd_asuint((1.0 / (-(vd_asfloat(_GlobalWaterBottomEdgeParams.w)))));
  r0.w = vd_asuint(saturate((vd_asfloat(r0.w) * vd_asfloat(r1.z))));
  r1.z = vd_asuint((vd_asfloat(r0.w) * -2.0 + 3.0));
  r1.w = vd_asuint((1.0 / vd_asfloat(_GlobalWaterBottomEdgeParams.z)));
  r0.z = vd_asuint(saturate((vd_asfloat(r0.z) * vd_asfloat(r1.w))));
  r1.w = vd_asuint((vd_asfloat(r0.z) * -2.0 + 3.0));
  r0.zw = vd_asuint((vd_asfloat(r0.zw) * vd_asfloat(r0.zw)));
  r0.z = vd_asuint((vd_asfloat(r0.z) * vd_asfloat(r1.w)));
  r0.y = vd_asuint((vd_asfloat(r1.z) * vd_asfloat(r0.w) + vd_asfloat(r0.y)));
  r2.xyz = vd_asuint((vd_asfloat(_GlobalWaterBottomColor.xyz) * float3(0.5, 0.5, 0.5)));
  r2.xyz = vd_asuint((vd_asfloat(r0.zzz) * vd_asfloat(r2.xyz) + vd_asfloat(r2.xyz)));
  r0.y = vd_asuint((vd_asfloat(r0.y) * vd_asfloat(_GlobalWaterBottomShallowColor.w)));
  r3.xyz = vd_asuint(((-(vd_asfloat(r2.xyz))) + vd_asfloat(_GlobalWaterBottomShallowColor.xyz)));
  r2.xyz = vd_asuint((vd_asfloat(r0.yyy) * vd_asfloat(r3.xyz) + vd_asfloat(r2.xyz)));
  r0.y = vd_asuint((-(int)(vd_asint(_PlaneReflectionActive) != 0)));
  r0.w = vd_asuint((-(int)(0.00999999978 < vd_asfloat(_GlobalWaterReflectionIntensity))));
  r0.y = (r0.w & r0.y);
  if (r0.y != 0u) {
    r0.x = vd_asuint((vd_asfloat(r0.x) * 0.5));
    r0.xy = vd_asuint(((-(vd_asfloat(r0.xx))) * vd_asfloat(_GlobalWaterWaveParams2.xx) + vd_asfloat(r1.xy)));
    r0.y = vd_asuint((vd_asfloat(r0.y) + vd_asfloat(_GlobalWaterBottomParams.y)));
    r1.xyzw = vd_asuint(_PlaneReflectionTexture.SampleBias(sampler_vd_linear_clamp, vd_asfloat(r0.xy), vd_asfloat(_GlobalMipBias.x)).xyzw);
    r0.x = vd_asuint((vd_asfloat(r0.z) * 0.699999988 + 0.300000012));
    r0.x = vd_asuint((vd_asfloat(r0.x) * vd_asfloat(_GlobalWaterBottomParams.z)));
    r2.xyz = vd_asuint((vd_asfloat(r1.xyz) * vd_asfloat(r0.xxx) + vd_asfloat(r2.xyz)));
  }
  r0.xyz = vd_asuint((vd_asfloat(r2.xyz) + (-(vd_asfloat(unity_FogColor.xyz)))));
  o0.xyz = (vd_asfloat(v0.www) * vd_asfloat(r0.xyz) + vd_asfloat(unity_FogColor.xyz));
  o0.w = vd_asfloat(vd_asuint(_GlobalWaterBottomColor.w));
  return;
}
