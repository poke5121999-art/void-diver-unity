// Hidden/ChocDino/UIFX/Blend-DropShadow | pass 0 "Blend-DropShadow" | progFragment | keywords: UNITY_UI_ALPHACLIP | blob 24
void frag(
  float4 v0 : SV_POSITION0,
  float4 v1 : VDREG1,
  float4 v2 : VDREG2,
  out float4 o0 : SV_Target0)
{
  uint4 r0, r1, r2;
  r0 = 0u; r1 = 0u; r2 = 0u;

  r0.xyzw = vd_asuint(_SourceTex.Sample(sampler_SourceTex, vd_asfloat(v1.xy)).xyzw);
  r1.xyzw = vd_asuint((vd_asfloat(r0.xyzw) * vd_asfloat((_SourceAlpha).xxxx)));
  r0.xy = vd_asuint((vd_asfloat(v1.xy) + vd_asfloat(_ShadowOffset.xy)));
  r2.xy = vd_asuint((-(int2)(vd_asfloat(r0.xy) >= float2(0.0, 0.0))));
  r0.z = (r2.y & r2.x);
  r2.xy = vd_asuint((-(int2)(float2(1.0, 1.0) >= vd_asfloat(r0.xy))));
  r0.z = (r0.z & r2.x);
  r0.z = (r2.y & r0.z);
  if (r0.z != 0u) {
    r2.xyzw = vd_asuint(_ResultTex.Sample(sampler_ResultTex, vd_asfloat(r0.xy)).xyzw);
    r2.xyzw = vd_asuint((vd_asfloat(r2.wwww) * vd_asfloat(_ShadowColor.wxyz)));
    r2.xyzw = vd_asuint(saturate((vd_asfloat(r2.xyzw) * vd_asfloat((_ShadowHardness).xxxx))));
    r0.x = vd_asuint(((-(vd_asfloat(_SourceAlpha))) * vd_asfloat(r0.w) + 1.0));
    r1.xyzw = vd_asuint((vd_asfloat(r0.xxxx) * vd_asfloat(r2.yzwx) + vd_asfloat(r1.xyzw)));
  }
  r0.x = vd_asuint((vd_asfloat(r1.w) + -0.00100000005));
  r0.x = vd_asuint((-(int)(vd_asfloat(r0.x) < 0.0)));
  if (r0.x != 0u) discard;
  r1.xyz = vd_asuint((vd_asfloat(r1.xyz) * vd_asfloat(v2.www)));
  o0.xyzw = (vd_asfloat(r1.xyzw) * vd_asfloat(v2.xyzw));
  return;
}
