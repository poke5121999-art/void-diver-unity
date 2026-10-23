// Hidden/ChocDino/UIFX/Blend-Extrude | pass 0 "Blend-Extrude" | progFragment | keywords: UNITY_UI_CLIP_RECT | blob 32
void frag(
  float4 v0 : SV_POSITION0,
  float4 v1 : VDREG1,
  float4 v2 : VDREG2,
  float4 v3 : VDREG3,
  out float4 o0 : SV_Target0)
{
  uint4 r0, r1;
  r0 = 0u; r1 = 0u;

  r0.xy = vd_asuint(((-(vd_asfloat(_ClipRect.xy))) + vd_asfloat(_ClipRect.zw)));
  r0.xy = vd_asuint((vd_asfloat(r0.xy) + (-(abs(vd_asfloat(v3.xy))))));
  r0.xy = vd_asuint(saturate((vd_asfloat(r0.xy) * vd_asfloat(v3.zw))));
  r0.x = vd_asuint((vd_asfloat(r0.y) * vd_asfloat(r0.x)));
  r1.xyzw = vd_asuint(_SourceTex.Sample(sampler_SourceTex, vd_asfloat(v1.xy)).xyzw);
  r1.xyzw = vd_asuint((vd_asfloat(r1.wxyz) * vd_asfloat((_SourceAlpha).xxxx)));
  r0.xyzw = vd_asuint((vd_asfloat(r0.xxxx) * vd_asfloat(r1.yzwx)));
  r0.xyz = vd_asuint((vd_asfloat(r0.xyz) * vd_asfloat(v2.www)));
  o0.xyzw = (vd_asfloat(r0.xyzw) * vd_asfloat(v2.xyzw));
  return;
}
