// Hidden/ChocDino/UIFX/Blend-Fill-Texture | pass 0 "Blend-Fill-Texture" | progFragment | keywords: BLEND_LIGHTEN WRAP_MIRROR | blob 146
void frag(
  float4 v0 : SV_POSITION0,
  float4 v1 : VDREG1,
  float4 v2 : VDREG2,
  out float4 o0 : SV_Target0)
{
  uint4 r0, r1;
  r0 = 0u; r1 = 0u;

  r0.xyz = vd_asuint((vd_asfloat(_Color.www) * vd_asfloat(_Color.xyz)));
  r0.w = vd_asuint(_Color.w);
  r1.xyzw = vd_asuint(_FillTex.Sample(sampler_vd_linear_mirror, vd_asfloat(v1.zw)).xyzw);
  r1.xyz = vd_asuint((vd_asfloat(r1.www) * vd_asfloat(r1.xyz)));
  r0.xyzw = vd_asuint((vd_asfloat(r0.xyzw) * vd_asfloat(r1.xyzw)));
  r1.xyzw = vd_asuint(_ResultTex.Sample(sampler_ResultTex, vd_asfloat(v1.xy)).xyzw);
  r0.xyzw = vd_asuint((vd_asfloat(r0.xyzw) * vd_asfloat(r1.wwww)));
  r0.xyzw = vd_asuint(max(vd_asfloat(r1.xyzw), vd_asfloat(r0.xyzw)));
  r0.xyzw = vd_asuint(((-(vd_asfloat(r1.xyzw))) + vd_asfloat(r0.xyzw)));
  r0.xyzw = vd_asuint((vd_asfloat((_Strength).xxxx) * vd_asfloat(r0.xyzw) + vd_asfloat(r1.xyzw)));
  r0.xyz = vd_asuint((vd_asfloat(r0.xyz) * vd_asfloat(v2.www)));
  o0.xyzw = (vd_asfloat(r0.xyzw) * vd_asfloat(v2.xyzw));
  return;
}
