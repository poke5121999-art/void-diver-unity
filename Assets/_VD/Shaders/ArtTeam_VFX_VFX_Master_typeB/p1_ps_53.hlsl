// ArtTeam/VFX/VFX_Master_typeB | pass 1 "DepthOnly" | progFragment | keywords: _ALPHATEST_ON _PLANESYSTEMCULLINGTYPE_EQUAL | blob 53
void frag(
  float4 v0 : SV_POSITION0,
  float4 v1 : VDREG1,
  float4 v2 : VDREG2,
  float4 v3 : VDREG3,
  float4 v4 : VDREG4,
  out float4 o0 : SV_TARGET0)
{
  uint4 r0, r1, r2;
  r0 = 0u; r1 = 0u; r2 = 0u;

  r0.xy = vd_asuint((float2(1.0, 1.0) / vd_asfloat(_ScreenParams.xy)));
  r0.xy = vd_asuint((vd_asfloat(r0.xy) * vd_asfloat((_Pixelation).xx)));
  r0.zw = vd_asuint((vd_asfloat(v1.xy) / vd_asfloat(r0.xy)));
  r0.zw = vd_asuint(floor(vd_asfloat(r0.zw)));
  r0.xy = vd_asuint((vd_asfloat(r0.xy) * vd_asfloat(r0.zw)));
  r0.xy = vd_asuint((vd_asfloat(r0.xy) * vd_asfloat(_MainTex_ST.xy) + vd_asfloat(_MainTex_ST.zw)));
  r0.xyzw = vd_asuint(_MainTex.SampleBias(sampler_MainTex, vd_asfloat(r0.xy), vd_asfloat(_GlobalMipBias.x)).xyzw);
  r0.x = vd_asuint(((-(vd_asfloat(r0.y))) + 1.0));
  r0.yz = vd_asuint(saturate(vd_asfloat(v2.yx)));
  r1.x = vd_asuint(((-(vd_asfloat(r0.y))) * 0.5 + 1.0));
  r1.y = vd_asuint((vd_asfloat(r0.y) * 0.5 + -1.0));
  r1.z = vd_asuint(((-(vd_asfloat(r1.y))) + vd_asfloat(r1.x)));
  r1.x = vd_asuint(((-(vd_asfloat(r0.y))) * 0.5 + vd_asfloat(r1.x)));
  r1.x = vd_asuint((1.0 / vd_asfloat(r1.x)));
  r0.z = vd_asuint((vd_asfloat(r0.z) * vd_asfloat(r1.z) + vd_asfloat(r1.y)));
  r0.x = vd_asuint((vd_asfloat(r0.z) + vd_asfloat(r0.x)));
  r0.x = vd_asuint(((-(vd_asfloat(r0.y))) * 0.5 + vd_asfloat(r0.x)));
  r0.x = vd_asuint(saturate((vd_asfloat(r1.x) * vd_asfloat(r0.x))));
  r0.y = vd_asuint((vd_asfloat(r0.x) * -2.0 + 3.0));
  r0.x = vd_asuint((vd_asfloat(r0.x) * vd_asfloat(r0.x)));
  r0.x = vd_asuint(saturate(((-(vd_asfloat(r0.y))) * vd_asfloat(r0.x) + vd_asfloat(r0.w))));
  r0.x = vd_asuint((vd_asfloat(r0.x) * vd_asfloat(v3.w)));
  r0.yz = vd_asuint((vd_asfloat(v1.xy) * vd_asfloat(_MaskTex_ST.xy) + vd_asfloat(_MaskTex_ST.zw)));
  r1.xyzw = vd_asuint(_MaskTex.SampleBias(sampler_MaskTex, vd_asfloat(r0.yz), vd_asfloat(_GlobalMipBias.x)).xyzw);
  r0.y = vd_asuint((vd_asfloat(r1.w) * vd_asfloat(r1.x)));
  r0.x = vd_asuint((vd_asfloat(r0.y) * vd_asfloat(r0.x)));
  r0.y = vd_asuint(((-(vd_asfloat(_WorldSpaceCameraPos.y))) + -0.200000003));
  r1.xyz = vd_asuint((vd_asfloat(v4.xyz) + (-(vd_asfloat(_WorldSpaceCameraPos.xyz)))));
  r0.y = vd_asuint((vd_asfloat(r0.y) / vd_asfloat(r1.y)));
  r0.yz = vd_asuint((vd_asfloat(r1.xz) * vd_asfloat(r0.yy) + vd_asfloat(_WorldSpaceCameraPos.xz)));
  r0.yz = vd_asuint(((-(vd_asfloat(r0.yz))) + vd_asfloat(_PlaneBoundMaxPos.xz)));
  r1.xy = vd_asuint(((-(vd_asfloat(_PlaneBoundMinPos.xz))) + vd_asfloat(_PlaneBoundMaxPos.xz)));
  r0.yz = vd_asuint((vd_asfloat(r0.yz) / vd_asfloat(r1.xy)));
  r1.xyzw = vd_asuint(_PlanePointBuffer.SampleBias(sampler_PlanePointBuffer, vd_asfloat(r0.yz), vd_asfloat(_GlobalMipBias.x)).xyzw);
  r2.xyzw = vd_asuint(_PlaneSightRenderTarget.SampleBias(sampler_PlaneSightRenderTarget, vd_asfloat(r0.yz), vd_asfloat(_GlobalMipBias.x)).xyzw);
  r0.y = vd_asuint((vd_asfloat(_GlobalPlanePointBufferEnable) * vd_asfloat(_PlaneSystemUsingLightBuffer)));
  r0.y = vd_asuint((vd_asfloat(r0.y) * vd_asfloat(r1.x)));
  r0.y = vd_asuint(max(vd_asfloat(r0.y), vd_asfloat(r2.w)));
  r0.y = vd_asuint((vd_asfloat(r0.y) + -1.0));
  r0.y = vd_asuint((vd_asfloat(r0.y) * vd_asfloat(_GlobalPlaneSystemEnable)));
  r0.y = vd_asuint((vd_asfloat(_GlobalIsRuntime) * vd_asfloat(r0.y) + 1.0));
  r0.x = vd_asuint(saturate((vd_asfloat(r0.y) * vd_asfloat(r0.x))));
  r0.x = vd_asuint((vd_asfloat(r0.x) + (-(vd_asfloat(_AlphaClipThreshold)))));
  r0.x = vd_asuint((-(int)(vd_asfloat(r0.x) < 0.0)));
  if (r0.x != 0u) discard;
  o0.xyzw = vd_asfloat(vd_asuint(v0.zzzz));
  return;
}
