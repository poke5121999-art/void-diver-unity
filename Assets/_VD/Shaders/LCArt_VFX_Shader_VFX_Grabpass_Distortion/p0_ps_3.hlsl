// LCArt/VFX/Shader_VFX_Grabpass_Distortion | pass 0 "" | progFragment | keywords: (none) | blob 3
void frag(
  float4 v0 : SV_POSITION0,
  float4 v1 : VDREG1,
  float4 v2 : VDREG2,
  float4 v3 : VDREG3,
  float4 v4 : VDREG4,
  float4 v5 : VDREG5,
  float4 v6 : VDREG6,
  out float4 o0 : SV_Target0)
{
  uint4 r0, r1;
  r0 = 0u; r1 = 0u;

  r0.xyzw = vd_asuint(_MaskMap.SampleBias(sampler_MaskMap, vd_asfloat(v3.xy), vd_asfloat(_GlobalMipBias.x)).xyzw);
  r0.x = vd_asuint(saturate((vd_asfloat(r0.w) * vd_asfloat(r0.x))));
  r0.y = vd_asuint((vd_asfloat(r0.x) + -0.00100000005));
  r0.y = vd_asuint((-(int)(vd_asfloat(r0.y) < 0.0)));
  if (r0.y != 0u) discard;
  r0.yz = vd_asuint((vd_asfloat(v3.xy) * vd_asfloat(v5.zw) + vd_asfloat(v5.xy)));
  r1.xyzw = vd_asuint(_NormalMap.SampleBias(sampler_NormalMap, vd_asfloat(r0.yz), vd_asfloat(_GlobalMipBias.x)).xyzw);
  r1.x = vd_asuint((vd_asfloat(r1.x) * vd_asfloat(r1.w)));
  r0.yz = vd_asuint((vd_asfloat(r1.xy) * float2(2.0, 2.0) + float2(-1.0, -1.0)));
  r0.w = vd_asuint(dot(vd_asfloat(r0.yz), vd_asfloat(r0.yz)));
  r0.w = vd_asuint(min(vd_asfloat(r0.w), 1.0));
  r0.w = vd_asuint(((-(vd_asfloat(r0.w))) + 1.0));
  r0.w = vd_asuint(sqrt(vd_asfloat(r0.w)));
  r0.w = vd_asuint(max(vd_asfloat(r0.w), 1.00000002e-16));
  r0.w = vd_asuint(((-(vd_asfloat(r0.w))) + 1.0));
  r1.x = vd_asuint(saturate(vd_asfloat(v6.x)));
  r0.w = vd_asuint((vd_asfloat(r0.w) * vd_asfloat(r1.x)));
  r0.yz = vd_asuint((vd_asfloat(r0.ww) * vd_asfloat(r0.yz)));
  o0.w = vd_asfloat(r0.w);
  r0.xy = vd_asuint((vd_asfloat(r0.xx) * vd_asfloat(r0.yz) + vd_asfloat(v3.zw)));
  r0.xyzw = vd_asuint(_AfterRenderingGrabpassTexture.SampleBias(sampler_AfterRenderingGrabpassTexture, vd_asfloat(r0.xy), vd_asfloat(_GlobalMipBias.x)).xyzw);
  o0.xyz = (vd_asfloat(r0.xyz) * vd_asfloat(v1.xyz));
  return;
}
