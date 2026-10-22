// LCArt/UI/Shader_DE_UI_DitheringScreen_Optimized | pass 0 "DrawProcedural" | progFragment | keywords: (none) | blob 3
void frag(
  float4 v0 : SV_POSITION0,
  float2 v1 : TEXCOORD0,
  out float4 o0 : SV_Target0)
{
  const float4 icb[] = { { 0.058824, 0, 0, 0},
                              { 0.529412, 0, 0, 0},
                              { 0.176471, 0, 0, 0},
                              { 0.647059, 0, 0, 0},
                              { 0.764706, 0, 0, 0},
                              { 0.294118, 0, 0, 0},
                              { 0.882353, 0, 0, 0},
                              { 0.411765, 0, 0, 0},
                              { 0.235294, 0, 0, 0},
                              { 0.705882, 0, 0, 0},
                              { 0.117647, 0, 0, 0},
                              { 0.588235, 0, 0, 0},
                              { 0.941176, 0, 0, 0},
                              { 0.470588, 0, 0, 0},
                              { 0.823529, 0, 0, 0},
                              { 0.352941, 0, 0, 0} };
  float4 r0,r1,r2;
  uint4 bitmask, uiDest;
  float4 fDest;

  r0.x = _DitheringScale + _DitheringScale;
  r1.x = _DitheringSpeed * _Time.y;
  r1.z = -0.5 * r1.x;
  r1.y = 0;
  r0.yz = v1.xy + r1.zy;
  r1.xy = v1.xy + r1.xy;
  r1.xy = (_DitheringScale).xx * r1.xy;
  r1.xy = float2(0.125,0.125) * r1.xy;
  r1.xyzw = _NoiseTex.SampleBias(sampler_vd_linear_repeat, r1.xy, _GlobalMipBias.x).xyzw;
  r0.xy = r0.yz * r0.xx;
  r0.xy = float2(0.125,0.125) * r0.xy;
  r0.xyzw = _NoiseTex.SampleBias(sampler_vd_linear_repeat, r0.xy, _GlobalMipBias.x).xyzw;
  r0.x = r1.x * r0.x;
  r0.yz = _ScreenParams.xy * v1.xy;
  r0.yz = float2(0.5,0.5) * r0.yz;
  r0.yz = (uint2)r0.yz;
  r0.y = (uint)r0.y << 2;
  r0.yz = (int2)r0.yz & int2(12,3);
  r0.y = (int)r0.z + (int)r0.y;
  r0.y = -icb[r0.y+0].x + 0.75;
  r0.x = cmp(r0.x >= r0.y);
  r0.x = r0.x ? 1.000000 : 0;
  r0.yz = _ScreenSize.xy * v1.xy;
  r1.xy = (uint2)r0.yz;
  r1.zw = float2(0,0);
  r1.xyzw = _BlitTexture.Load(r1.xyz).xyzw;
  r0.yzw = _Color.xyz * r1.xyz;
  r2.xyz = r1.xyz * _DitheringColor.xyz + -r0.yzw;
  r0.xyz = r0.xxx * r2.xyz + r0.yzw;
  r0.xyz = r0.xyz + -r1.xyz;
  o0.xyz = (_ScreenBlurCustomShaderProgress).xxx * r0.xyz + r1.xyz;
  o0.w = 1;
  return;
}
