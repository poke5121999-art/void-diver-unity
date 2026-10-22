// LCArt/VFX/Shader_VFX_Grabpass_Distortion | pass 0 "" | progFragment | keywords: (none) | blob 3
void frag(
  float4 v0 : SV_POSITION0,
  float4 v1 : COLOR0,
  float3 v2 : NORMAL0,
  float2 v3 : TEXCOORD0,
  float2 w3 : TEXCOORD2,
  float4 v4 : TEXCOORD1,
  float4 v5 : TEXCOORD3,
  float4 v6 : TEXCOORD4,
  out float4 o0 : SV_Target0)
{
  float4 r0,r1;
  uint4 bitmask, uiDest;
  float4 fDest;

  r0.xyzw = _MaskMap.SampleBias(sampler_MaskMap, v3.xy, _GlobalMipBias.x).xyzw;
  r0.x = saturate(r0.x * r0.w);
  r0.y = -0.00100000005 + r0.x;
  r0.y = cmp(r0.y < 0);
  if (r0.y != 0) discard;
  r0.yz = v3.xy * v5.zw + v5.xy;
  r1.xyzw = _NormalMap.SampleBias(sampler_NormalMap, r0.yz, _GlobalMipBias.x).xyzw;
  r1.x = r1.w * r1.x;
  r0.yz = r1.xy * float2(2,2) + float2(-1,-1);
  r0.w = dot(r0.yz, r0.yz);
  r0.w = min(1, r0.w);
  r0.w = 1 + -r0.w;
  r0.w = sqrt(r0.w);
  r0.w = max(1.00000002e-016, r0.w);
  r0.w = 1 + -r0.w;
  r1.x = saturate(v6.x);
  r0.w = r1.x * r0.w;
  r0.yz = r0.yz * r0.ww;
  o0.w = r0.w;
  r0.xy = r0.xx * r0.yz + w3.xy;
  r0.xyzw = _AfterRenderingGrabpassTexture.SampleBias(sampler_AfterRenderingGrabpassTexture, r0.xy, _GlobalMipBias.x).xyzw;
  o0.xyz = v1.xyz * r0.xyz;
  return;
}
