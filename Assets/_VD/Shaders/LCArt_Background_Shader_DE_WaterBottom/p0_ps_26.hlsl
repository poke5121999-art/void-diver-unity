// LCArt/Background/Shader_DE_WaterBottom | pass 0 "ForwardLit" | progFragment | keywords: FOG_LINEAR _CLUSTER_LIGHT_LOOP _DE_EDITOR_CAMERA _LIGHT_LAYERS | blob 26
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
  uint4 r0, r1, r2, r3, r4, r5, r6, r7, r8, r9, r10;
  r0 = 0u; r1 = 0u; r2 = 0u; r3 = 0u; r4 = 0u; r5 = 0u; r6 = 0u; r7 = 0u; r8 = 0u; r9 = 0u; r10 = 0u;

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
  r3.x = vd_asuint((VD_COL(unity_MatrixV, 0).z));
  r3.y = vd_asuint((VD_COL(unity_MatrixV, 1).z));
  r3.z = vd_asuint((VD_COL(unity_MatrixV, 2).z));
  r2.xyz = ((r1.www != 0u) ? r2.xyz : r3.xyz);
  r4.xy = vd_asuint((float2(1.0, 1.0) / vd_asfloat(_ScaledScreenParams.xy)));
  r4.xy = vd_asuint((vd_asfloat(r4.xy) * vd_asfloat(v6.xy)));
#if UNITY_UV_STARTS_AT_TOP
  r2.w = vd_asuint((vd_asfloat(r4.y) * vd_asfloat(_ScaleBiasRt.x) + vd_asfloat(_ScaleBiasRt.y)));
  r4.z = vd_asuint(((-(vd_asfloat(r2.w))) + 1.0));
#else
  r2.w = r4.y;
  r4.z = r2.w;
#endif
  r4.xy = vd_asuint((vd_asfloat(r4.xz) * vd_asfloat(_FPParams1.xy)));
  r4.xy = ((uint2)vd_asfloat(r4.xy));
  r4.zw = ((uint2)vd_asfloat(_FPParams1.zw));
  r2.w = vd_asuint((vd_asint(r4.y) * vd_asint(r4.z) + vd_asint(r4.x)));
  r2.w = vd_asuint((vd_asint(r4.w) * vd_asint(r2.w)));
  r4.xyz = vd_asuint((vd_asfloat(v1.xyz) + (-(vd_asfloat(_WorldSpaceCameraPos.xyz)))));
  r3.x = vd_asuint(dot((-(vd_asfloat(r3.xyz))), vd_asfloat(r4.xyz)));
  r3.y = vd_asuint(log2(vd_asfloat(r3.x)));
  r1.w = ((r1.w != 0u) ? r3.y : r3.x);
  r1.w = vd_asuint((vd_asfloat(r1.w) * vd_asfloat(_FPParams0.x) + vd_asfloat(_FPParams0.y)));
  r1.w = ((uint)vd_asfloat(r1.w));
  r3.x = ((uint)vd_asfloat(_FPParams2.x));
  r3.x = vd_asuint((vd_asint(r3.x) + -1));
  r3.y = vd_asuint((vd_asint(r4.w) + 2));
  r1.w = min(r1.w, r3.x);
  r3.x = vd_asuint((vd_asint(r3.y) * vd_asint(r1.w)));
  r1.w = vd_asuint((vd_asint(r1.w) * vd_asint(r3.y) + 2));
  r3.y = (r3.x >> (uint)2);
  r4.x = (r3.x << (uint)31);
  r4.y = (r3.x << (uint)30);
  r3.xz = vd_asuint((vd_asint(r4.xy) >> 31));
  r4.xy = (r3.xx & vd_asuint(urp_ZBins[(int)(r3.y)].wy));
  r3.xw = (~r3.xz);
  r3.xy = (r3.xx & vd_asuint(urp_ZBins[(int)(r3.y)].zx));
  r3.xy = (r3.xy | r4.xy);
  r3.xy = (r3.zw & r3.xy);
  r3.x = (r3.y | r3.x);
  r3.y = ((uint)vd_asfloat(_FPParams0.w));
  r4.x = r2.w;
  r4.y = r1.w;
  r4.z = 0x00000000u;
  r4.w = r3.x;
  r5.xyz = uint3(0x3f800000u, 0x3f800000u, 0x3f800000u);
  r6.xyz = uint3(0x00000000u, 0x00000000u, 0x00000000u);
  while (true) {
    r3.z = (r4.w >> (uint)16);
    r3.w = r4.z;
    r5.w = r4.w;
    while (true) {
      r6.w = vd_asuint((-(int)(vd_asint(r3.w) == 0)));
      r7.x = (r5.w & 0x0000ffffu);
      r7.x = vd_asuint((-(int)(r3.z >= r7.x)));
      r6.w = (r6.w & r7.x);
      if (r6.w == 0u) break;
      r6.w = (r5.w >> (uint)5);
      r6.w = (r6.w & 0x000007ffu);
      r7.xyzw = vd_asuint((vd_asint(r4.xxyy) + vd_asint(r6.wwww)));
      r8.xy = (r7.yw >> (uint2)2);
      r9.xz = (r7.xz << (uint2)31);
      r9.yw = (r7.yw << (uint2)30);
      r7.xyzw = vd_asuint((vd_asint(r9.xyzw) >> 31));
      r8.zw = (r7.xx & vd_asuint(urp_Tiles[(int)(r8.x)].wy));
      r9.xyzw = (~r7.xyzw);
      r10.xy = (r9.xx & vd_asuint(urp_Tiles[(int)(r8.x)].zx));
      r8.xz = (r8.zw | r10.xy);
      r7.x = (r7.y & r8.x);
      r7.y = (r9.y & r8.z);
      r7.x = (r7.y | r7.x);
      r7.yz = (r7.zz & vd_asuint(urp_ZBins[(int)(r8.y)].wy));
      r8.xy = (r9.zz & vd_asuint(urp_ZBins[(int)(r8.y)].zx));
      r7.yz = (r7.yz | r8.xy);
      r7.y = (r7.w & r7.y);
      r7.z = (r9.w & r7.z);
      r7.y = (r7.z | r7.y);
      r7.x = (r7.y & r7.x);
      r7.y = (0xffffffffu << (uint)(int)(r5.w & 31u));
      r7.x = (r7.y & r7.x);
      r6.w = (r6.w << (uint)5);
      r6.w = vd_asuint((vd_asint(r3.z) + (-(vd_asint(r6.w)))));
      r6.w = min(r6.w, 0x0000001fu);
      r6.w = vd_asuint(((-(vd_asint(r6.w))) + 31));
      r6.w = (0xffffffffu >> (uint)(int)(r6.w & 31u));
      r3.w = (r6.w & r7.x);
      r6.w = vd_asuint((vd_asint(r5.w) + 32));
      r5.w = (r6.w & 0xffffffe0u);
    }
    r4.w = r5.w;
    if (r3.w == 0u) break;
    r3.z = vd_asuint((-vd_asint(r3.w)));
    r3.z = (r3.z & r3.w);
    r3.z = vd_asuint(((float)r3.z));
    r3.z = (r3.z >> (uint)23);
    r3.z = vd_asuint((vd_asint(r3.z) + -127));
    r6.w = (0x00000001u << (uint)(int)(r3.z & 31u));
    r4.z = (r3.w ^ r6.w);
    r6.w = vd_asuint((vd_asint(r4.w) + -32));
    r6.w = (r6.w & 0x0000ffe0u);
    r3.z = vd_asuint((vd_asint(r3.z) + vd_asint(r6.w)));
    r3.z = vd_asuint((vd_asint(r3.y) + vd_asint(r3.z)));
    r7.xyz = vd_asuint(((-(vd_asfloat(v1.xyz))) * vd_asfloat(_AdditionalLightsPosition[(int)(r3.z)].www) + vd_asfloat(_AdditionalLightsPosition[(int)(r3.z)].xyz)));
    r6.w = vd_asuint(dot(vd_asfloat(r7.xyz), vd_asfloat(r7.xyz)));
    r6.w = vd_asuint(max(vd_asfloat(r6.w), 6.10351562e-05));
    r7.w = vd_asuint(rsqrt(vd_asfloat(r6.w)));
    r8.xyz = vd_asuint((vd_asfloat(r7.www) * vd_asfloat(r7.xyz)));
    r8.w = vd_asuint((1.0 / vd_asfloat(r6.w)));
    r6.w = vd_asuint((vd_asfloat(r6.w) * vd_asfloat(_AdditionalLightsAttenuation[(int)(r3.z)].x)));
    r6.w = vd_asuint(((-(vd_asfloat(r6.w))) * vd_asfloat(r6.w) + 1.0));
    r6.w = vd_asuint(max(vd_asfloat(r6.w), 0.0));
    r6.w = vd_asuint((vd_asfloat(r6.w) * vd_asfloat(r6.w)));
    r6.w = vd_asuint((vd_asfloat(r6.w) * vd_asfloat(r8.w)));
    r8.x = vd_asuint(dot(vd_asfloat(_AdditionalLightsSpotDir[(int)(r3.z)].xyz), vd_asfloat(r8.xyz)));
    r8.x = vd_asuint(saturate((vd_asfloat(r8.x) * vd_asfloat(_AdditionalLightsAttenuation[(int)(r3.z)].z) + vd_asfloat(_AdditionalLightsAttenuation[(int)(r3.z)].w))));
    r8.x = vd_asuint((vd_asfloat(r8.x) * vd_asfloat(r8.x)));
    r6.w = vd_asuint((vd_asfloat(r6.w) * vd_asfloat(r8.x)));
    r8.x = (vd_asuint(unity_RenderingLayer.x) & vd_asuint(_AdditionalLightsLayerMasks[(int)(r3.z)]));
    r6.w = vd_asuint(sqrt(vd_asfloat(r6.w)));
    r7.xyz = vd_asuint((vd_asfloat(r7.xyz) * vd_asfloat(r7.www) + vd_asfloat(r2.xyz)));
    r7.w = vd_asuint(dot(vd_asfloat(r7.xyz), vd_asfloat(r7.xyz)));
    r7.w = vd_asuint(max(vd_asfloat(r7.w), 1.17549435e-38));
    r7.w = vd_asuint(rsqrt(vd_asfloat(r7.w)));
    r7.xyz = vd_asuint((vd_asfloat(r7.www) * vd_asfloat(r7.xyz)));
    r7.x = vd_asuint(saturate(dot(vd_asfloat(r1.xyz), vd_asfloat(r7.xyz))));
    r7.x = vd_asuint((vd_asfloat(r6.w) * vd_asfloat(r7.x) + -0.720000029));
    r7.x = vd_asuint(saturate((vd_asfloat(r7.x) * 100.000099)));
    r7.y = vd_asuint((vd_asfloat(r7.x) * -2.0 + 3.0));
    r7.x = vd_asuint((vd_asfloat(r7.x) * vd_asfloat(r7.x)));
    r7.x = vd_asuint((vd_asfloat(r7.x) * vd_asfloat(r7.y)));
    r7.yzw = vd_asuint((float3(-1.0, -1.0, -1.0) + vd_asfloat(_AdditionalLightsColor[(int)(r3.z)].xyz)));
    r7.yzw = vd_asuint((vd_asfloat(r6.www) * vd_asfloat(r7.yzw) + float3(1.0, 1.0, 1.0)));
    r6.w = vd_asuint((vd_asfloat(r7.x) * vd_asfloat(r7.x)));
    r7.xyz = vd_asuint((vd_asfloat(r5.xyz) * vd_asfloat(r7.yzw)));
    r8.yzw = vd_asuint((vd_asfloat(r6.www) * vd_asfloat(_AdditionalLightsColor[(int)(r3.z)].xyz) + vd_asfloat(r6.xyz)));
    r5.xyz = ((r8.xxx != 0u) ? r7.xyz : r5.xyz);
    r6.xyz = ((r8.xxx != 0u) ? r8.yzw : r6.xyz);
  }
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
  r0.xyz = vd_asuint((vd_asfloat(r2.xyz) * vd_asfloat(r5.xyz) + vd_asfloat(r6.xyz)));
  r0.xyz = vd_asuint((vd_asfloat(r0.xyz) + (-(vd_asfloat(unity_FogColor.xyz)))));
  o0.xyz = (vd_asfloat(v0.www) * vd_asfloat(r0.xyz) + vd_asfloat(unity_FogColor.xyz));
  o0.w = vd_asfloat(vd_asuint(_GlobalWaterBottomColor.w));
  return;
}
