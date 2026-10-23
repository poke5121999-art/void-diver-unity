// LCArt/UI/Shader_DE_UI_Speaker | pass 0 "Default" | progFragment | keywords: (none) | blob 3
void frag(
  float4 v0 : SV_POSITION0,
  float4 v1 : VDREG1,
  float4 v2 : VDREG2,
  float4 v3 : VDREG3,
  float4 v4 : VDREG4,
  out float4 o0 : SV_Target0)
{
  uint4 r0, r1, r2;
  r0 = 0u; r1 = 0u; r2 = 0u;

  r0.xyzw = vd_asuint(_MainTex.Sample(sampler_MainTex, vd_asfloat(v2.xy)).xyzw);
  r0.xyzw = vd_asuint((vd_asfloat(r0.xyzw) + vd_asfloat(_TextureSampleAdd.xyzw)));
  r1.x = vd_asuint((vd_asfloat(r0.w) * vd_asfloat(v1.w) + -0.00100000005));
  r0.xyzw = vd_asuint((vd_asfloat(r0.xyzw) * vd_asfloat(v1.xyzw)));
  r1.x = vd_asuint((-(int)(vd_asfloat(r1.x) < 0.0)));
  if (r1.x != 0u) discard;
  r1.xy = vd_asuint((vd_asfloat(v2.zw) * vd_asfloat((_GlobalDialogRimLightOffset).xx) + vd_asfloat(v2.xy)));
  r1.xyzw = vd_asuint(_MainTex.Sample(sampler_MainTex, vd_asfloat(r1.xy)).xyzw);
  r1.x = vd_asuint(((-(vd_asfloat(r1.w))) + 1.0));
  r1.x = vd_asuint(saturate((vd_asfloat(r0.w) * vd_asfloat(r1.x))));
  r1.y = vd_asuint((-(int)(vd_asfloat(r0.w) >= vd_asfloat(_TransparentAlphaThreshold))));
  r1.y = (r1.y & 0x3f800000u);
  r1.x = vd_asuint((vd_asfloat(r1.x) * vd_asfloat(r1.y)));
  r1.yzw = vd_asuint(((-(vd_asfloat(_GlobalDialogBaseColor.xyz))) + vd_asfloat(_GlobalDialogRimLightColor.xyz)));
  r1.xyz = vd_asuint((vd_asfloat(r1.xxx) * vd_asfloat(r1.yzw) + vd_asfloat(_GlobalDialogBaseColor.xyz)));
  r2.xyz = vd_asuint((vd_asfloat(r0.xyz) * vd_asfloat(r1.xyz)));
  r0.xyz = vd_asuint(((-(vd_asfloat(r0.xyz))) * vd_asfloat(r1.xyz) + vd_asfloat(_GradientColor.xyz)));
  o0.w = vd_asfloat(r0.w);
  r0.w = vd_asuint((vd_asfloat(v4.y) / vd_asfloat(v4.w)));
  r0.w = vd_asuint((vd_asfloat(r0.w) + (-(vd_asfloat(_GradientHeightMax)))));
  r1.x = vd_asuint(((-(vd_asfloat(_GradientHeightMax))) + vd_asfloat(_GradientHeightMin)));
  r1.x = vd_asuint((1.0 / vd_asfloat(r1.x)));
  r0.w = vd_asuint(saturate((vd_asfloat(r0.w) * vd_asfloat(r1.x))));
  r1.x = vd_asuint((vd_asfloat(r0.w) * -2.0 + 3.0));
  r0.w = vd_asuint((vd_asfloat(r0.w) * vd_asfloat(r0.w)));
  r0.w = vd_asuint((vd_asfloat(r0.w) * vd_asfloat(r1.x)));
  r0.w = vd_asuint((vd_asfloat(r0.w) * vd_asfloat(_GradientColor.w)));
  o0.xyz = (vd_asfloat(r0.www) * vd_asfloat(r0.xyz) + vd_asfloat(r2.xyz));
  return;
}
