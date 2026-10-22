// LCArt/Indicator/Shader_DE_Indicator_Billboard | pass 0 "" | progFragment | keywords: _CLUSTER_LIGHT_LOOP | blob 14
void frag(
  float4 v0 : TEXCOORD0,
  float3 v1 : TEXCOORD1,
  float4 v2 : SV_POSITION0,
  float4 v3 : COLOR0,
  out float4 o0 : SV_Target0)
{
  float4 r0,r1;
  uint4 bitmask, uiDest;
  float4 fDest;

  r0.xy = -_PlaneBoundMaxPos.xz + v1.xz;
  r0.zw = -_PlaneBoundMaxPos.xz + _PlaneBoundMinPos.xz;
  r0.xy = saturate(r0.xy / r0.zw);
  r1.xyzw = _PlaneSightRenderTarget.SampleLevel(sampler_PlaneSightRenderTarget, r0.xy, 0).xyzw;
  r0.xyzw = _PlanePointBuffer.SampleLevel(sampler_PlanePointBuffer, r0.xy, 0).xyzw;
  r0.x = _GlobalPlanePointBufferEnable * r0.x;
  r0.y = -1 + r1.w;
  r0.y = _GlobalPlaneSystemEnable * r0.y + 1;
  r0.x = max(r0.y, r0.x);
  r0.x = -1 + r0.x;
  r0.x = _IsHiddenByPlaneSystem * r0.x + 1;
  r1.xyzw = _MainTex.SampleBias(sampler_MainTex, v0.xy, _GlobalMipBias.x).xyzw;
  r1.xyzw = saturate(r1.xyzw);
  r0.y = r1.w * r1.w;
  r0.y = r0.y * r0.y;
  o0.w = r0.y * r0.x;
  r0.x = saturate(_Progress);
  r0.yzw = _EndColor.xyz + -_StartColor.xyz;
  r0.xyz = r0.xxx * r0.yzw + _StartColor.xyz;
  o0.xyz = r1.xyz * r0.xyz;
  return;
}
