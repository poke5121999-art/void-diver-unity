// LCArt/PostProcessing/CampaignTilt | pass 0 "CampaignTiltKawase" | progFragment | keywords: (none) | blob 3
void frag(
  float4 v0 : SV_POSITION0,
  float2 v1 : TEXCOORD0,
  out float4 o0 : SV_Target0)
{
  float4 r0,r1,r2,r3,r4;
  uint4 bitmask, uiDest;
  float4 fDest;

  r0.x = -0.5 + v1.y;
  r0.x = abs(r0.x) * 2 + -_CenterRatio;
  r0.y = 1 / _EdgeSoftness;
  r0.x = saturate(r0.x * r0.y);
  r0.y = r0.x * -2 + 3;
  r0.x = r0.x * r0.x;
  r0.x = r0.y * r0.x;
  r0.x = _BlurStrength * r0.x;
  r0.y = 0.5 + _KawaseOffset;
  r1.xy = _BlitTexture_TexelSize.xy * r0.yy;
  r1.zw = -r1.yx;
  r1.xyzw = v1.xyxy + r1.xzwy;
  r2.xyzw = _BlitTexture.SampleBias(sampler_vd_linear_clamp, r1.xy, _GlobalMipBias.x).xyzw;
  r1.xyzw = _BlitTexture.SampleBias(sampler_vd_linear_clamp, r1.zw, _GlobalMipBias.x).xyzw;
  r0.zw = -_BlitTexture_TexelSize.xy * r0.yy + v1.xy;
  r3.xy = _BlitTexture_TexelSize.xy * r0.yy + v1.xy;
  r3.xyzw = _BlitTexture.SampleBias(sampler_vd_linear_clamp, r3.xy, _GlobalMipBias.x).xyzw;
  r4.xyzw = _BlitTexture.SampleBias(sampler_vd_linear_clamp, r0.zw, _GlobalMipBias.x).xyzw;
  r2.xyzw = r4.xyzw + r2.xyzw;
  r1.xyzw = r2.xyzw + r1.xyzw;
  r1.xyzw = r1.xyzw + r3.xyzw;
  r2.xyzw = _BlitTexture.SampleBias(sampler_vd_linear_clamp, v1.xy, _GlobalMipBias.x).xyzw;
  r1.xyzw = r1.xyzw * float4(0.25,0.25,0.25,0.25) + -r2.xyzw;
  o0.xyzw = r0.xxxx * r1.xyzw + r2.xyzw;
  return;
}
