// LCArt/UI/Shader_UI_Overlay | pass 0 "OverlayBlend" | progFragment | keywords: (none) | blob 7
void frag(
  float4 v0 : SV_POSITION0,
  float4 v1 : VDREG1,
  float4 v2 : VDREG2,
  float4 v3 : VDREG3,
  out float4 o0 : SV_Target0)
{
  uint4 r0;
  r0 = 0u;

  r0.xyzw = vd_asuint(_MainTex.Sample(sampler_MainTex, vd_asfloat(v2.xy)).xyzw);
  r0.xyzw = vd_asuint((vd_asfloat(r0.xyzw) + vd_asfloat(_TextureSampleAdd.xyzw)));
  r0.xyz = vd_asuint(((-(vd_asfloat(r0.xyz))) * vd_asfloat(v1.xyz) + float3(1.0, 1.0, 1.0)));
  r0.w = vd_asuint((vd_asfloat(r0.w) * vd_asfloat(v1.w)));
  o0.w = vd_asfloat(r0.w);
  r0.xyz = vd_asuint(((-(vd_asfloat(r0.xyz))) + float3(0.5, 0.5, 0.5)));
  o0.xyz = (vd_asfloat((_OverlayStrength).xxx) * vd_asfloat(r0.xyz) + float3(0.5, 0.5, 0.5));
  return;
}
