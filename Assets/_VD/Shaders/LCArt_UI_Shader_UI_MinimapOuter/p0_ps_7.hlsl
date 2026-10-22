// LCArt/UI/Shader_UI_MinimapOuter | pass 0 "Default" | progFragment | keywords: UNITY_UI_ALPHACLIP | blob 7
void frag(
  float4 v0 : SV_POSITION0,
  float4 v1 : COLOR0,
  float4 v2 : TEXCOORD0,
  float4 v3 : TEXCOORD1,
  out float4 o0 : SV_Target0)
{
  float4 r0,r1;
  uint4 bitmask, uiDest;
  float4 fDest;

  r0.xy = _GlobalMinimapOuterNoiseStretch.xy * v2.xy;
  r0.z = _Time.y * _GlobalMinimapOuterNoiseSpeed;
  r1.xy = v2.xy * _GlobalMinimapOuterNoiseStretch.xy + -r0.zz;
  r0.xy = r0.xy * float2(0.829999983,0.829999983) + r0.zz;
  r0.zw = (_GlobalMinimapOuterNoiseScale).xx * r1.xy;
  r1.xyzw = _GlobalMinimapOuterNoiseTex.Sample(sampler_GlobalMinimapOuterNoiseTex, r0.zw).xyzw;
  r0.xy = (_GlobalMinimapOuterNoiseScale).xx * r0.xy;
  r0.xyzw = _GlobalMinimapOuterNoiseTex.Sample(sampler_GlobalMinimapOuterNoiseTex, r0.xy).xyzw;
  r0.x = r1.x + r0.x;
  r0.yz = cmp(float2(9.99999975e-005,9.99999975e-005) < _GlobalLayerGridSize.xy);
  r0.y = r0.z ? r0.y : 0;
  if (r0.y != 0) {
    r0.yz = -_GlobalLayerGridOrigin.xy + v2.xy;
    r0.yz = r0.yz / _GlobalLayerGridSize.xy;
    r1.xy = cmp(r0.yz >= float2(0,0));
    r1.zw = cmp(float2(1,1) >= r0.yz);
    r0.w = r1.z ? r1.x : 0;
    r0.w = r1.y ? r0.w : 0;
    r0.w = r1.w ? r0.w : 0;
    if (r0.w != 0) {
      r1.xyzw = _GlobalLayerSDFTex.Sample(sampler_GlobalLayerSDFTex, r0.yz).xyzw;
      r0.y = _GlobalLayerOuterSDFMax + -_GlobalLayerOuterSDFMin;
      r0.z = -_GlobalLayerOuterSDFMin + r1.x;
      r0.y = 1 / r0.y;
      r0.y = saturate(r0.z * r0.y);
      r0.z = r0.y * -2 + 3;
      r0.y = r0.y * r0.y;
      r0.y = r0.z * r0.y;
    } else {
      r0.y = 1;
    }
  } else {
    r0.y = 0;
  }
  r0.z = 1 + -r0.y;
  r0.y = r0.z * r0.y;
  r0.y = 4 * r0.y;
  r0.x = r0.x * 0.5 + -0.5;
  r0.x = saturate(r0.x * r0.y + r0.z);
  r0.x = saturate(_GlobalMinimapOuterSmokeIntensity * r0.x);
  r0.x = 1 + -r0.x;
  r0.x = _GlobalMinimapOuterColor.w * r0.x;
  o0.w = _GlobalMinimapInStage * r0.x;
  o0.xyz = _GlobalMinimapOuterColor.xyz;
  return;
}
