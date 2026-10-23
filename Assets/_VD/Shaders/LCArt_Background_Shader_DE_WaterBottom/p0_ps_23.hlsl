// LCArt/Background/Shader_DE_WaterBottom | pass 0 "ForwardLit" | progFragment | keywords: _ADDITIONAL_LIGHTS _DE_EDITOR_CAMERA _LIGHT_LAYERS | blob 23
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
  uint4 r0, r1, r2, r3, r4, r5, r6;
  r0 = 0u; r1 = 0u; r2 = 0u; r3 = 0u; r4 = 0u; r5 = 0u; r6 = 0u;
  const uint4 icb[4] = { uint4(0x3f800000u, 0x00000000u, 0x00000000u, 0x00000000u), uint4(0x00000000u, 0x3f800000u, 0x00000000u, 0x00000000u), uint4(0x00000000u, 0x00000000u, 0x3f800000u, 0x00000000u), uint4(0x00000000u, 0x00000000u, 0x00000000u, 0x3f800000u) };

  r0.xy = vd_asuint((vd_asfloat(v1.xz) * vd_asfloat(_GlobalWaterWaveParams.yy)));
  r0.z = vd_asuint((vd_asfloat(_Time.y) * vd_asfloat(_GlobalWaterWaveParams.x)));
  r1.xyzw = vd_asuint((vd_asfloat(r0.zzzz) * float4(0.707099974, -0.400000006, 0.692799985, -0.707099974)));
  r0.zw = vd_asuint((vd_asfloat(v1.xz) * vd_asfloat(_GlobalWaterWaveParams.yy) + vd_asfloat(r1.xx)));
  r0.xy = vd_asuint((vd_asfloat(r0.xy) * float2(0.699999988, 0.699999988) + vd_asfloat(r1.yz)));
  r1.xyzw = vd_asuint((vd_asfloat(r0.zwzw) * vd_asfloat(_GlobalWaterWaveParams.yyyy) + vd_asfloat(r1.xxww)));
  r2.xyzw = vd_asuint(_BumpMap.SampleBias(sampler_vd_linear_repeat, vd_asfloat(r1.xy), vd_asfloat(_GlobalMipBias.x)).xyzw);
  r1.xyzw = vd_asuint(_BumpMap.SampleBias(sampler_vd_linear_repeat, vd_asfloat(r1.zw), vd_asfloat(_GlobalMipBias.x)).xyzw);
  r1.xyz = vd_asuint((vd_asfloat(r1.xyw) * float3(0.5, 0.5, 0.5)));
  r1.yzw = vd_asuint((vd_asfloat(r2.xyw) * float3(0.5, 0.5, 0.5) + vd_asfloat(r1.xyz)));
  r1.x = vd_asuint((vd_asfloat(r1.y) * vd_asfloat(r1.w)));
  r1.xy = vd_asuint((vd_asfloat(r1.xz) * float2(2.0, 2.0) + float2(-1.0, -1.0)));
  r1.z = vd_asuint(dot(vd_asfloat(r1.xy), vd_asfloat(r1.xy)));
  r1.z = vd_asuint(min(vd_asfloat(r1.z), 1.0));
  r1.z = vd_asuint(((-(vd_asfloat(r1.z))) + 1.0));
  r1.z = vd_asuint(sqrt(vd_asfloat(r1.z)));
  r1.z = vd_asuint(max(vd_asfloat(r1.z), 1.00000002e-16));
  r2.xyz = vd_asuint((vd_asfloat(r1.yyy) * vd_asfloat(v4.xyz)));
  r1.xyw = vd_asuint((vd_asfloat(r1.xxx) * vd_asfloat(v3.xyz) + vd_asfloat(r2.xyz)));
  r1.xyz = vd_asuint((vd_asfloat(r1.zzz) * vd_asfloat(v2.xyz) + vd_asfloat(r1.xyw)));
  r1.w = vd_asuint(dot(vd_asfloat(r1.xyz), vd_asfloat(r1.xyz)));
  r1.w = vd_asuint(rsqrt(vd_asfloat(r1.w)));
  r1.xyz = vd_asuint((vd_asfloat(r1.www) * vd_asfloat(r1.xyz)));
  r1.w = vd_asuint((-(int)(vd_asfloat(unity_OrthoParams.w) == 0.0)));
  r2.xyz = vd_asuint(((-(vd_asfloat(v1.xyz))) + vd_asfloat(_WorldSpaceCameraPos.xyz)));
  r2.w = vd_asuint(dot(vd_asfloat(r2.xyz), vd_asfloat(r2.xyz)));
  r2.w = vd_asuint(rsqrt(vd_asfloat(r2.w)));
  r2.xyz = vd_asuint((vd_asfloat(r2.www) * vd_asfloat(r2.xyz)));
  r3.x = ((r1.w != 0u) ? r2.x : vd_asuint((VD_COL(unity_MatrixV, 0).z)));
  r3.y = ((r1.w != 0u) ? r2.y : vd_asuint((VD_COL(unity_MatrixV, 1).z)));
  r3.z = ((r1.w != 0u) ? r2.z : vd_asuint((VD_COL(unity_MatrixV, 2).z)));
  r1.w = vd_asuint(min(vd_asfloat(_AdditionalLightsCount.x), vd_asfloat(unity_LightData.y)));
  r1.w = vd_asuint(((int)vd_asfloat(r1.w)));
  r4.xyz = uint3(0x00000000u, 0x00000000u, 0x00000000u);
  r2.xyzw = uint4(0x3f800000u, 0x3f800000u, 0x3f800000u, 0x00000000u);
  while (true) {
    r3.w = vd_asuint((-(int)(r2.w >= r1.w)));
    if (r3.w != 0u) break;
    r3.w = (r2.w >> (uint)2);
    r4.w = (r2.w & 0x00000003u);
    r3.w = vd_asuint(dot(vd_asfloat(unity_LightIndices[(int)(r3.w)].xyzw), vd_asfloat(icb[r4.w + 0].xyzw)));
    r3.w = vd_asuint(((int)vd_asfloat(r3.w)));
    r5.xyz = vd_asuint(((-(vd_asfloat(v1.xyz))) * vd_asfloat(_AdditionalLightsPosition[(int)(r3.w)].www) + vd_asfloat(_AdditionalLightsPosition[(int)(r3.w)].xyz)));
    r4.w = vd_asuint(dot(vd_asfloat(r5.xyz), vd_asfloat(r5.xyz)));
    r4.w = vd_asuint(max(vd_asfloat(r4.w), 6.10351562e-05));
    r5.w = vd_asuint(rsqrt(vd_asfloat(r4.w)));
    r6.xyz = vd_asuint((vd_asfloat(r5.www) * vd_asfloat(r5.xyz)));
    r6.w = vd_asuint((1.0 / vd_asfloat(r4.w)));
    r4.w = vd_asuint((vd_asfloat(r4.w) * vd_asfloat(_AdditionalLightsAttenuation[(int)(r3.w)].x)));
    r4.w = vd_asuint(((-(vd_asfloat(r4.w))) * vd_asfloat(r4.w) + 1.0));
    r4.w = vd_asuint(max(vd_asfloat(r4.w), 0.0));
    r4.w = vd_asuint((vd_asfloat(r4.w) * vd_asfloat(r4.w)));
    r4.w = vd_asuint((vd_asfloat(r4.w) * vd_asfloat(r6.w)));
    r6.x = vd_asuint(dot(vd_asfloat(_AdditionalLightsSpotDir[(int)(r3.w)].xyz), vd_asfloat(r6.xyz)));
    r6.x = vd_asuint(saturate((vd_asfloat(r6.x) * vd_asfloat(_AdditionalLightsAttenuation[(int)(r3.w)].z) + vd_asfloat(_AdditionalLightsAttenuation[(int)(r3.w)].w))));
    r6.x = vd_asuint((vd_asfloat(r6.x) * vd_asfloat(r6.x)));
    r4.w = vd_asuint((vd_asfloat(r4.w) * vd_asfloat(r6.x)));
    r6.x = (vd_asuint(unity_RenderingLayer.x) & vd_asuint(_AdditionalLightsLayerMasks[(int)(r3.w)]));
    r4.w = vd_asuint(sqrt(vd_asfloat(r4.w)));
    r5.xyz = vd_asuint((vd_asfloat(r5.xyz) * vd_asfloat(r5.www) + vd_asfloat(r3.xyz)));
    r5.w = vd_asuint(dot(vd_asfloat(r5.xyz), vd_asfloat(r5.xyz)));
    r5.w = vd_asuint(max(vd_asfloat(r5.w), 1.17549435e-38));
    r5.w = vd_asuint(rsqrt(vd_asfloat(r5.w)));
    r5.xyz = vd_asuint((vd_asfloat(r5.www) * vd_asfloat(r5.xyz)));
    r5.x = vd_asuint(saturate(dot(vd_asfloat(r1.xyz), vd_asfloat(r5.xyz))));
    r5.x = vd_asuint((vd_asfloat(r4.w) * vd_asfloat(r5.x) + -0.720000029));
    r5.x = vd_asuint(saturate((vd_asfloat(r5.x) * 100.000099)));
    r5.y = vd_asuint((vd_asfloat(r5.x) * -2.0 + 3.0));
    r5.x = vd_asuint((vd_asfloat(r5.x) * vd_asfloat(r5.x)));
    r5.x = vd_asuint((vd_asfloat(r5.x) * vd_asfloat(r5.y)));
    r5.yzw = vd_asuint((float3(-1.0, -1.0, -1.0) + vd_asfloat(_AdditionalLightsColor[(int)(r3.w)].xyz)));
    r5.yzw = vd_asuint((vd_asfloat(r4.www) * vd_asfloat(r5.yzw) + float3(1.0, 1.0, 1.0)));
    r4.w = vd_asuint((vd_asfloat(r5.x) * vd_asfloat(r5.x)));
    r5.xyz = vd_asuint((vd_asfloat(r2.xyz) * vd_asfloat(r5.yzw)));
    r6.yzw = vd_asuint((vd_asfloat(r4.www) * vd_asfloat(_AdditionalLightsColor[(int)(r3.w)].xyz) + vd_asfloat(r4.xyz)));
    r2.xyz = ((r6.xxx != 0u) ? r5.xyz : r2.xyz);
    r4.xyz = ((r6.xxx != 0u) ? r6.yzw : r4.xyz);
    r2.w = vd_asuint((vd_asint(r2.w) + 1));
  }
  r1.xy = vd_asuint((vd_asfloat(v7.xy) / vd_asfloat(v7.ww)));
  r1.zw = vd_asuint(((-(vd_asfloat(_CameraDepthTexture_TexelSize.xy))) * float2(0.5, 0.5) + float2(1.0, 1.0)));
  r1.zw = vd_asuint(min(vd_asfloat(r1.zw), vd_asfloat(r1.xy)));
  r1.zw = vd_asuint((vd_asfloat(r1.zw) * vd_asfloat(_RTHandleScale.xy)));
  r3.xyzw = vd_asuint(_CameraDepthTexture.SampleBias(sampler_vd_point_clamp, vd_asfloat(r1.zw), vd_asfloat(_GlobalMipBias.x)).xyzw);
  r1.z = vd_asuint((vd_asfloat(_ZBufferParams.z) * vd_asfloat(r3.x) + vd_asfloat(_ZBufferParams.w)));
  r1.z = vd_asuint((1.0 / vd_asfloat(r1.z)));
  r3.xyzw = vd_asuint(_GlobalWaterNoiseTex.SampleBias(sampler_vd_linear_repeat, vd_asfloat(r0.zw), vd_asfloat(_GlobalMipBias.x)).xyzw);
  r0.xyzw = vd_asuint(_GlobalWaterNoiseTex.SampleBias(sampler_vd_linear_repeat, vd_asfloat(r0.xy), vd_asfloat(_GlobalMipBias.x)).xyzw);
  r0.y = vd_asuint(min(vd_asfloat(r0.x), vd_asfloat(r3.x)));
  r0.y = vd_asuint(log2(abs(vd_asfloat(r0.y))));
  r0.y = vd_asuint((vd_asfloat(r0.y) * vd_asfloat(_GlobalWaterBottomEdgeParams.x)));
  r0.y = vd_asuint(exp2(vd_asfloat(r0.y)));
  r0.y = vd_asuint(min(vd_asfloat(r0.y), 1.0));
  r0.y = vd_asuint((vd_asfloat(r0.y) * vd_asfloat(_GlobalWaterBottomEdgeParams.y)));
  r0.x = vd_asuint((vd_asfloat(r0.x) + vd_asfloat(r3.x)));
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
  r3.xyz = vd_asuint((vd_asfloat(_GlobalWaterBottomColor.xyz) * float3(0.5, 0.5, 0.5)));
  r3.xyz = vd_asuint((vd_asfloat(r0.zzz) * vd_asfloat(r3.xyz) + vd_asfloat(r3.xyz)));
  r0.y = vd_asuint((vd_asfloat(r0.y) * vd_asfloat(_GlobalWaterBottomShallowColor.w)));
  r5.xyz = vd_asuint(((-(vd_asfloat(r3.xyz))) + vd_asfloat(_GlobalWaterBottomShallowColor.xyz)));
  r3.xyz = vd_asuint((vd_asfloat(r0.yyy) * vd_asfloat(r5.xyz) + vd_asfloat(r3.xyz)));
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
    r3.xyz = vd_asuint((vd_asfloat(r1.xyz) * vd_asfloat(r0.xxx) + vd_asfloat(r3.xyz)));
  }
  o0.xyz = (vd_asfloat(r3.xyz) * vd_asfloat(r2.xyz) + vd_asfloat(r4.xyz));
  o0.w = vd_asfloat(vd_asuint(_GlobalWaterBottomColor.w));
  return;
}
