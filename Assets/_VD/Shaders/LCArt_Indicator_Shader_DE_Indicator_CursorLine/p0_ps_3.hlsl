// LCArt/Indicator/Shader_DE_Indicator_CursorLine | pass 0 "ForwardUnlit" | progFragment | keywords: (none) | blob 3
void frag(
  float4 v0 : SV_POSITION0,
  float4 v1 : TEXCOORD0,
  float4 v2 : TEXCOORD1,
  out float4 o0 : SV_Target0)
{
  float4 r0,r1;
  uint4 bitmask, uiDest;
  float4 fDest;

  r0.xyz = v2.xyz / v2.www;
  r1.xyzw = _CustomDepthBuffer.SampleBias(sampler_CustomDepthBuffer, r0.xy, _GlobalMipBias.x).xyzw;
  r0.x = _ZBufferParams.z * r0.z + _ZBufferParams.w;
  r0.x = 1 / r0.x;
  r0.y = _ZBufferParams.z * r1.x + _ZBufferParams.w;
  r0.y = 1 / r0.y;
  r0.x = r0.x + -r0.y;
  r0.x = -_OcclusionFadeStart + r0.x;
  r0.y = _OcclusionFadeEnd + -_OcclusionFadeStart;
  r0.y = max(9.99999975e-005, r0.y);
  r0.x = saturate(r0.x / r0.y);
  r0.x = 1 + -r0.x;
  r0.y = 1 + -_OccludedAlpha;
  r0.x = r0.x * r0.y + _OccludedAlpha;
  r1.xyzw = _MainTex.SampleBias(sampler_MainTex, v1.xy, _GlobalMipBias.x).xyzw;
  r1.xyzw = _Color.xyzw * r1.xyzw;
  o0.w = r1.w * r0.x;
  o0.xyz = r1.xyz;
  return;
}
