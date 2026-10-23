// LCArt/UI/Shader_UI_MinimapOuter | pass 0 "Default" | progFragment | keywords: UNITY_UI_ALPHACLIP UNITY_UI_CLIP_RECT | blob 9
void frag(
  float4 v0 : SV_POSITION0,
  float4 v1 : VDREG1,
  float4 v2 : VDREG2,
  float4 v3 : VDREG3,
  out float4 o0 : SV_Target0)
{
  uint4 r0, r1;
  r0 = 0u; r1 = 0u;

  r0.xy = vd_asuint((vd_asfloat(v2.xy) * vd_asfloat(_GlobalMinimapOuterNoiseStretch.xy)));
  r0.z = vd_asuint((vd_asfloat(_GlobalMinimapOuterNoiseSpeed) * vd_asfloat(_Time.y)));
  r1.xy = vd_asuint((vd_asfloat(v2.xy) * vd_asfloat(_GlobalMinimapOuterNoiseStretch.xy) + (-(vd_asfloat(r0.zz)))));
  r0.xy = vd_asuint((vd_asfloat(r0.xy) * float2(0.829999983, 0.829999983) + vd_asfloat(r0.zz)));
  r0.zw = vd_asuint((vd_asfloat(r1.xy) * vd_asfloat((_GlobalMinimapOuterNoiseScale).xx)));
  r1.xyzw = vd_asuint(_GlobalMinimapOuterNoiseTex.Sample(sampler_GlobalMinimapOuterNoiseTex, vd_asfloat(r0.zw)).xyzw);
  r0.xy = vd_asuint((vd_asfloat(r0.xy) * vd_asfloat((_GlobalMinimapOuterNoiseScale).xx)));
  r0.xyzw = vd_asuint(_GlobalMinimapOuterNoiseTex.Sample(sampler_GlobalMinimapOuterNoiseTex, vd_asfloat(r0.xy)).xyzw);
  r0.x = vd_asuint((vd_asfloat(r0.x) + vd_asfloat(r1.x)));
  r0.yz = vd_asuint((-(int2)(float2(9.99999975e-05, 9.99999975e-05) < vd_asfloat(_GlobalLayerGridSize.xy))));
  r0.y = (r0.z & r0.y);
  if (r0.y != 0u) {
    r0.yz = vd_asuint((vd_asfloat(v2.xy) + (-(vd_asfloat(_GlobalLayerGridOrigin.xy)))));
    r0.yz = vd_asuint((vd_asfloat(r0.yz) / vd_asfloat(_GlobalLayerGridSize.xy)));
    r1.xy = vd_asuint((-(int2)(vd_asfloat(r0.yz) >= float2(0.0, 0.0))));
    r1.zw = vd_asuint((-(int2)(float2(1.0, 1.0) >= vd_asfloat(r0.yz))));
    r0.w = (r1.z & r1.x);
    r0.w = (r1.y & r0.w);
    r0.w = (r1.w & r0.w);
    if (r0.w != 0u) {
      r1.xyzw = vd_asuint(_GlobalLayerSDFTex.Sample(sampler_GlobalLayerSDFTex, vd_asfloat(r0.yz)).xyzw);
      r0.y = vd_asuint(((-(vd_asfloat(_GlobalLayerOuterSDFMin))) + vd_asfloat(_GlobalLayerOuterSDFMax)));
      r0.z = vd_asuint((vd_asfloat(r1.x) + (-(vd_asfloat(_GlobalLayerOuterSDFMin)))));
      r0.y = vd_asuint((1.0 / vd_asfloat(r0.y)));
      r0.y = vd_asuint(saturate((vd_asfloat(r0.y) * vd_asfloat(r0.z))));
      r0.z = vd_asuint((vd_asfloat(r0.y) * -2.0 + 3.0));
      r0.y = vd_asuint((vd_asfloat(r0.y) * vd_asfloat(r0.y)));
      r0.y = vd_asuint((vd_asfloat(r0.y) * vd_asfloat(r0.z)));
    } else {
      r0.y = 0x3f800000u;
    }
  } else {
    r0.y = 0x00000000u;
  }
  r0.z = vd_asuint(((-(vd_asfloat(r0.y))) + 1.0));
  r0.y = vd_asuint((vd_asfloat(r0.y) * vd_asfloat(r0.z)));
  r0.y = vd_asuint((vd_asfloat(r0.y) * 4.0));
  r0.x = vd_asuint((vd_asfloat(r0.x) * 0.5 + -0.5));
  r0.x = vd_asuint(saturate((vd_asfloat(r0.x) * vd_asfloat(r0.y) + vd_asfloat(r0.z))));
  r0.x = vd_asuint(saturate((vd_asfloat(r0.x) * vd_asfloat(_GlobalMinimapOuterSmokeIntensity))));
  r0.x = vd_asuint(((-(vd_asfloat(r0.x))) + 1.0));
  r0.x = vd_asuint((vd_asfloat(r0.x) * vd_asfloat(_GlobalMinimapOuterColor.w)));
  o0.w = (vd_asfloat(r0.x) * vd_asfloat(_GlobalMinimapInStage));
  o0.xyz = vd_asfloat(vd_asuint(_GlobalMinimapOuterColor.xyz));
  return;
}
