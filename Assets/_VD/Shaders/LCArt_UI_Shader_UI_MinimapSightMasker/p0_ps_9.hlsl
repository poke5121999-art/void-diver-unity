// LCArt/UI/Shader_UI_MinimapSightMasker | pass 0 "Default" | progFragment | keywords: UNITY_UI_ALPHACLIP UNITY_UI_CLIP_RECT | blob 9
void frag(
  float4 v0 : SV_POSITION0,
  float4 v1 : VDREG1,
  float4 v2 : VDREG2,
  float4 v3 : VDREG3,
  out float4 o0 : SV_Target0)
{
  uint4 r0, r1, r2, r3, r4, r5;
  r0 = 0u; r1 = 0u; r2 = 0u; r3 = 0u; r4 = 0u; r5 = 0u;

  r0.xy = vd_asuint((vd_asfloat(_GlobalOverallMapBoundMax.xz) + (-(vd_asfloat(_GlobalOverallMapBoundMin.xz)))));
  r0.xy = vd_asuint(max(vd_asfloat(r0.xy), float2(1.0, 1.0)));
  r0.zw = vd_asuint((vd_asfloat(_PivotWorldPosition.xz) / vd_asfloat(r0.xy)));
  r1.xy = vd_asuint((vd_asfloat(float2(_GlobalSectorWidth, _GlobalSectorHeight)) + vd_asfloat(_PivotWorldPosition.xz)));
  r0.xy = vd_asuint((vd_asfloat(r1.xy) / vd_asfloat(r0.xy)));
  r0.xy = vd_asuint(((-(vd_asfloat(r0.zw))) + vd_asfloat(r0.xy)));
  r0.xy = vd_asuint((vd_asfloat(v2.zw) * vd_asfloat(r0.xy) + vd_asfloat(r0.zw)));
  r1.xyzw = vd_asuint(_MainTex.Sample(sampler_MainTex, vd_asfloat(v2.xy)).xyzw);
  r2.xyzw = vd_asuint((vd_asfloat(r1.xyzw) * vd_asfloat(v1.xyzw)));
  r0.z = vd_asuint((vd_asfloat(r1.w) * vd_asfloat(v1.w) + -0.100000001));
  r0.z = vd_asuint((-(int)(vd_asfloat(r0.z) < 0.0)));
  if (r0.z != 0u) discard;
  r3.xyzw = vd_asuint(_MinimapSightTexture.Sample(sampler_MinimapSightTexture, vd_asfloat(r0.xy)).xyzw);
  r0.z = vd_asuint(((-(vd_asfloat(_MinimapSightEnable))) + 1.0));
  r0.zw = vd_asuint(max(vd_asfloat(r3.xy), vd_asfloat(r0.zz)));
  r3.xyz = vd_asuint((vd_asfloat(r2.xyz) * vd_asfloat(_GlobalMinimapPassedAreaColor.xyz) + (-(vd_asfloat(_GlobalMinimapSightMaskColor.xyz)))));
  r3.xyz = vd_asuint((vd_asfloat(r0.zzz) * vd_asfloat(r3.xyz) + vd_asfloat(_GlobalMinimapSightMaskColor.xyz)));
  r4.xyz = vd_asuint((vd_asfloat(r1.xyz) * vd_asfloat(v1.xyz) + (-(vd_asfloat(r3.xyz)))));
  r3.xyz = vd_asuint((vd_asfloat(r0.www) * vd_asfloat(r4.xyz) + vd_asfloat(r3.xyz)));
  r0.z = vd_asuint((-(int)(0.5 < vd_asfloat(_GlobalLayerPaddedSize.x))));
  if (r0.z != 0u) {
    r0.zw = vd_asuint((vd_asfloat(r0.xy) * vd_asfloat(_NoiseStretch.xy)));
    r3.w = vd_asuint((vd_asfloat(_Time.y) * vd_asfloat(_NoiseSpeed)));
    r4.xy = vd_asuint((vd_asfloat(r0.xy) * vd_asfloat(_NoiseStretch.xy) + (-(vd_asfloat(r3.ww)))));
    r0.zw = vd_asuint((vd_asfloat(r0.zw) * float2(0.829999983, 0.829999983) + vd_asfloat(r3.ww)));
    r4.xy = vd_asuint((vd_asfloat(r4.xy) * vd_asfloat((_NoiseScale).xx)));
    r4.xyzw = vd_asuint(_NoiseTex.Sample(sampler_NoiseTex, vd_asfloat(r4.xy)).xyzw);
    r0.zw = vd_asuint((vd_asfloat(r0.zw) * vd_asfloat((_NoiseScale).xx)));
    r5.xyzw = vd_asuint(_NoiseTex.Sample(sampler_NoiseTex, vd_asfloat(r0.zw)).xyzw);
    r0.z = vd_asuint((vd_asfloat(r4.x) + vd_asfloat(r5.x)));
    r0.z = vd_asuint((vd_asfloat(r0.z) * 0.5));
    r4.xy = vd_asuint(((-(vd_asfloat(_GlobalLayerPaddedSize.zz))) * float2(2.0, 2.0) + vd_asfloat(_GlobalLayerPaddedSize.xy)));
    r0.xy = vd_asuint((vd_asfloat(r0.xy) * vd_asfloat(r4.xy) + vd_asfloat(_GlobalLayerPaddedSize.zz)));
    r0.xy = vd_asuint((vd_asfloat(r0.xy) / vd_asfloat(_GlobalLayerPaddedSize.xy)));
    r4.xyzw = vd_asuint(_GlobalLayerSDFTex.Sample(sampler_GlobalLayerSDFTex, vd_asfloat(r0.xy)).xyzw);
    r0.x = vd_asuint(((-(vd_asfloat(_GlobalLayerInnerSDFMin))) + vd_asfloat(_GlobalLayerInnerSDFMax)));
    r0.y = vd_asuint((vd_asfloat(r4.x) + (-(vd_asfloat(_GlobalLayerInnerSDFMin)))));
    r0.x = vd_asuint((1.0 / vd_asfloat(r0.x)));
    r0.x = vd_asuint(saturate((vd_asfloat(r0.x) * vd_asfloat(r0.y))));
    r0.y = vd_asuint((vd_asfloat(r0.x) * -2.0 + 3.0));
    r0.x = vd_asuint((vd_asfloat(r0.x) * vd_asfloat(r0.x)));
    r0.w = vd_asuint((vd_asfloat(r0.x) * vd_asfloat(r0.y)));
    r0.x = vd_asuint(((-(vd_asfloat(r0.y))) * vd_asfloat(r0.x) + 1.0));
    r0.y = vd_asuint((vd_asfloat(r0.w) * vd_asfloat(r0.x)));
    r0.y = vd_asuint((vd_asfloat(r0.z) * vd_asfloat(r0.y)));
    r0.x = vd_asuint(saturate((vd_asfloat(r0.y) * 4.0 + vd_asfloat(r0.x))));
    r0.y = vd_asuint((vd_asfloat(r0.x) * vd_asfloat(_GlobalMinimapSightMaskColor.w)));
    r4.xyz = vd_asuint(((-(vd_asfloat(r3.xyz))) + vd_asfloat(_GlobalMinimapSightMaskColor.xyz)));
    r3.xyz = vd_asuint((vd_asfloat(r0.yyy) * vd_asfloat(r4.xyz) + vd_asfloat(r3.xyz)));
    r0.x = vd_asuint(((-(vd_asfloat(r0.x))) + 1.0));
  } else {
    r0.x = 0x3f800000u;
  }
  r0.yzw = vd_asuint(((-(vd_asfloat(r1.xyz))) * vd_asfloat(v1.xyz) + vd_asfloat(r3.xyz)));
  o0.xyz = (vd_asfloat((_GlobalMinimapInStage).xxx) * vd_asfloat(r0.yzw) + vd_asfloat(r2.xyz));
  r0.x = vd_asuint(((-(vd_asfloat(r1.w))) * vd_asfloat(v1.w) + vd_asfloat(r0.x)));
  o0.w = (vd_asfloat(_GlobalMinimapInStage) * vd_asfloat(r0.x) + vd_asfloat(r2.w));
  return;
}
