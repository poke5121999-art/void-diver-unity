// LCArt/UI/Shader_UI_MinimapSightMasker | pass 0 "Default" | progFragment | keywords: UNITY_UI_ALPHACLIP UNITY_UI_CLIP_RECT | blob 9
void frag(
  float4 v0 : SV_POSITION0,
  float4 v1 : COLOR0,
  float2 v2 : TEXCOORD0,
  float2 w2 : TEXCOORD1,
  float4 v3 : TEXCOORD2,
  out float4 o0 : SV_Target0)
{
  float4 r0,r1,r2,r3,r4,r5;
  uint4 bitmask, uiDest;
  float4 fDest;

  r0.xy = -_GlobalOverallMapBoundMin.xz + _GlobalOverallMapBoundMax.xz;
  r0.xy = max(float2(1,1), r0.xy);
  r0.zw = _PivotWorldPosition.xz / r0.xy;
  r1.xy = _PivotWorldPosition.xz + float2(_GlobalSectorWidth, _GlobalSectorHeight);
  r0.xy = r1.xy / r0.xy;
  r0.xy = r0.xy + -r0.zw;
  r0.xy = w2.xy * r0.xy + r0.zw;
  r1.xyzw = _MainTex.Sample(sampler_MainTex, v2.xy).xyzw;
  r2.xyzw = v1.xyzw * r1.xyzw;
  r0.z = r1.w * v1.w + -0.100000001;
  r0.z = cmp(r0.z < 0);
  if (r0.z != 0) discard;
  r3.xyzw = _MinimapSightTexture.Sample(sampler_MinimapSightTexture, r0.xy).xyzw;
  r0.z = 1 + -_MinimapSightEnable;
  r0.zw = max(r0.zz, r3.xy);
  r3.xyz = r2.xyz * _GlobalMinimapPassedAreaColor.xyz + -_GlobalMinimapSightMaskColor.xyz;
  r3.xyz = r0.zzz * r3.xyz + _GlobalMinimapSightMaskColor.xyz;
  r4.xyz = r1.xyz * v1.xyz + -r3.xyz;
  r3.xyz = r0.www * r4.xyz + r3.xyz;
  r0.z = cmp(0.5 < _GlobalLayerPaddedSize.x);
  if (r0.z != 0) {
    r0.zw = _NoiseStretch.xy * r0.xy;
    r3.w = _NoiseSpeed * _Time.y;
    r4.xy = r0.xy * _NoiseStretch.xy + -r3.ww;
    r0.zw = r0.zw * float2(0.829999983,0.829999983) + r3.ww;
    r4.xy = (_NoiseScale).xx * r4.xy;
    r4.xyzw = _NoiseTex.Sample(sampler_NoiseTex, r4.xy).xyzw;
    r0.zw = (_NoiseScale).xx * r0.zw;
    r5.xyzw = _NoiseTex.Sample(sampler_NoiseTex, r0.zw).xyzw;
    r0.z = r5.x + r4.x;
    r0.z = 0.5 * r0.z;
    r4.xy = -_GlobalLayerPaddedSize.zz * float2(2,2) + _GlobalLayerPaddedSize.xy;
    r0.xy = r0.xy * r4.xy + _GlobalLayerPaddedSize.zz;
    r0.xy = r0.xy / _GlobalLayerPaddedSize.xy;
    r4.xyzw = _GlobalLayerSDFTex.Sample(sampler_GlobalLayerSDFTex, r0.xy).xyzw;
    r0.x = _GlobalLayerInnerSDFMax + -_GlobalLayerInnerSDFMin;
    r0.y = -_GlobalLayerInnerSDFMin + r4.x;
    r0.x = 1 / r0.x;
    r0.x = saturate(r0.y * r0.x);
    r0.y = r0.x * -2 + 3;
    r0.x = r0.x * r0.x;
    r0.w = r0.y * r0.x;
    r0.x = -r0.y * r0.x + 1;
    r0.y = r0.x * r0.w;
    r0.y = r0.y * r0.z;
    r0.x = saturate(r0.y * 4 + r0.x);
    r0.y = _GlobalMinimapSightMaskColor.w * r0.x;
    r4.xyz = _GlobalMinimapSightMaskColor.xyz + -r3.xyz;
    r3.xyz = r0.yyy * r4.xyz + r3.xyz;
    r0.x = 1 + -r0.x;
  } else {
    r0.x = 1;
  }
  r0.yzw = -r1.xyz * v1.xyz + r3.xyz;
  o0.xyz = (_GlobalMinimapInStage).xxx * r0.yzw + r2.xyz;
  r0.x = -r1.w * v1.w + r0.x;
  o0.w = _GlobalMinimapInStage * r0.x + r2.w;
  return;
}
