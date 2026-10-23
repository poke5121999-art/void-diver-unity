// Hidden/ChocDino/UIFX/Blend-Pixelate | pass 0 "Blend-Pixelate" | progFragment | keywords: (none) | blob 8
void frag(
  float4 v0 : SV_POSITION0,
  float4 v1 : VDREG1,
  float4 v2 : VDREG2,
  out float4 o0 : SV_Target0)
{
  uint4 r0;
  r0 = 0u;

  r0.xy = vd_asuint((vd_asfloat(v1.xy) + float2(-0.5, -0.5)));
  r0.xy = vd_asuint((vd_asfloat(r0.xy) * vd_asfloat(_ResultTex_TexelSize.zw)));
  r0.xy = vd_asuint((vd_asfloat(r0.xy) / vd_asfloat((_Size).xx)));
  r0.xy = vd_asuint(round(vd_asfloat(r0.xy)));
  r0.xy = vd_asuint((vd_asfloat(r0.xy) * vd_asfloat((_Size).xx)));
  r0.xy = vd_asuint((vd_asfloat(r0.xy) * vd_asfloat(_ResultTex_TexelSize.xy) + float2(0.5, 0.5)));
  r0.xyzw = vd_asuint(_SourceTex.Sample(sampler_SourceTex, vd_asfloat(r0.xy)).xyzw);
  r0.xyz = vd_asuint((vd_asfloat(r0.xyz) * vd_asfloat(v2.www)));
  o0.xyzw = (vd_asfloat(r0.xyzw) * vd_asfloat(v2.xyzw));
  return;
}
