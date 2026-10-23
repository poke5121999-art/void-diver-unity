// LCArt/Indicator/Shader_DE_Indicator_Billboard | pass 0 "" | progFragment | keywords: _CLUSTER_LIGHT_LOOP | blob 14
void frag(
  float4 v0 : VDREG0,
  float4 v1 : VDREG1,
  float4 v2 : SV_POSITION0,
  float4 v3 : VDREG3,
  out float4 o0 : SV_Target0)
{
  uint4 r0, r1;
  r0 = 0u; r1 = 0u;

  r0.xy = vd_asuint((vd_asfloat(v1.xz) + (-(vd_asfloat(_PlaneBoundMaxPos.xz)))));
  r0.zw = vd_asuint((vd_asfloat(_PlaneBoundMinPos.xz) + (-(vd_asfloat(_PlaneBoundMaxPos.xz)))));
  r0.xy = vd_asuint(saturate((vd_asfloat(r0.xy) / vd_asfloat(r0.zw))));
  r1.xyzw = vd_asuint(_PlaneSightRenderTarget.SampleLevel(sampler_PlaneSightRenderTarget, vd_asfloat(r0.xy), 0.0).xyzw);
  r0.xyzw = vd_asuint(_PlanePointBuffer.SampleLevel(sampler_PlanePointBuffer, vd_asfloat(r0.xy), 0.0).xyzw);
  r0.x = vd_asuint((vd_asfloat(r0.x) * vd_asfloat(_GlobalPlanePointBufferEnable)));
  r0.y = vd_asuint((vd_asfloat(r1.w) + -1.0));
  r0.y = vd_asuint((vd_asfloat(_GlobalPlaneSystemEnable) * vd_asfloat(r0.y) + 1.0));
  r0.x = vd_asuint(max(vd_asfloat(r0.x), vd_asfloat(r0.y)));
  r0.x = vd_asuint((vd_asfloat(r0.x) + -1.0));
  r0.x = vd_asuint((vd_asfloat(_IsHiddenByPlaneSystem) * vd_asfloat(r0.x) + 1.0));
  r1.xyzw = vd_asuint(_MainTex.SampleBias(sampler_MainTex, vd_asfloat(v0.xy), vd_asfloat(_GlobalMipBias.x)).xyzw);
  r1.xyzw = vd_asuint(saturate(vd_asfloat(r1.xyzw)));
  r0.y = vd_asuint((vd_asfloat(r1.w) * vd_asfloat(r1.w)));
  r0.y = vd_asuint((vd_asfloat(r0.y) * vd_asfloat(r0.y)));
  o0.w = (vd_asfloat(r0.x) * vd_asfloat(r0.y));
  r0.x = vd_asuint(saturate(vd_asfloat(_Progress)));
  r0.yzw = vd_asuint(((-(vd_asfloat(_StartColor.xyz))) + vd_asfloat(_EndColor.xyz)));
  r0.xyz = vd_asuint((vd_asfloat(r0.xxx) * vd_asfloat(r0.yzw) + vd_asfloat(_StartColor.xyz)));
  o0.xyz = (vd_asfloat(r0.xyz) * vd_asfloat(r1.xyz));
  return;
}
