// Hidden/ChocDino/UIFX/Blend-Fill-Texture | pass 0 "Blend-Fill-Texture" | progFragment | keywords: BLEND_ALPHABLEND UNITY_UI_CLIP_RECT WRAP_CLAMP | blob 186
void frag(
  float4 v0 : SV_POSITION0,
  float4 v1 : VDREG1,
  float4 v2 : VDREG2,
  float4 v3 : VDREG3,
  out float4 o0 : SV_Target0)
{
  uint4 r0, r1, r2;
  r0 = 0u; r1 = 0u; r2 = 0u;

  r0.xyz = vd_asuint((vd_asfloat(_Color.www) * vd_asfloat(_Color.xyz)));
  r1.xyzw = vd_asuint(_FillTex.Sample(sampler_vd_linear_clamp, vd_asfloat(v1.zw)).xyzw);
  r1.xyz = vd_asuint((vd_asfloat(r1.www) * vd_asfloat(r1.xyz)));
  r0.xyz = vd_asuint((vd_asfloat(r0.xyz) * vd_asfloat(r1.xyz)));
  r0.w = vd_asuint(_Color.w);
  r0.w = vd_asuint(((-(vd_asfloat(r1.w))) * vd_asfloat(r0.w) + 1.0));
  r1.xyzw = vd_asuint(_ResultTex.Sample(sampler_ResultTex, vd_asfloat(v1.xy)).xyzw);
  r2.xyz = vd_asuint((vd_asfloat(r0.www) * vd_asfloat(r1.xyz)));
  r0.xyz = vd_asuint((vd_asfloat(r1.www) * vd_asfloat(r0.xyz) + vd_asfloat(r2.xyz)));
  r0.yzw = vd_asuint(((-(vd_asfloat(r1.xyz))) + vd_asfloat(r0.xyz)));
  r0.x = 0x00000000u;
  r0.xyzw = vd_asuint((vd_asfloat((_Strength).xxxx) * vd_asfloat(r0.xyzw) + vd_asfloat(r1.wxyz)));
  r1.xy = vd_asuint(((-(vd_asfloat(_ClipRect.xy))) + vd_asfloat(_ClipRect.zw)));
  r1.xy = vd_asuint((vd_asfloat(r1.xy) + (-(abs(vd_asfloat(v3.xy))))));
  r1.xy = vd_asuint(saturate((vd_asfloat(r1.xy) * vd_asfloat(v3.zw))));
  r1.x = vd_asuint((vd_asfloat(r1.y) * vd_asfloat(r1.x)));
  r0.xyzw = vd_asuint((vd_asfloat(r0.yzwx) * vd_asfloat(r1.xxxx)));
  r0.xyz = vd_asuint((vd_asfloat(r0.xyz) * vd_asfloat(v2.www)));
  o0.xyzw = (vd_asfloat(r0.xyzw) * vd_asfloat(v2.xyzw));
  return;
}
