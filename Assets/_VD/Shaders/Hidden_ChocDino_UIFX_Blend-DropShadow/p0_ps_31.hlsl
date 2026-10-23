// Hidden/ChocDino/UIFX/Blend-DropShadow | pass 0 "Blend-DropShadow" | progFragment | keywords: CUTOUT UNITY_UI_CLIP_RECT | blob 31
void frag(
  float4 v0 : SV_POSITION0,
  float4 v1 : VDREG1,
  float4 v2 : VDREG2,
  float4 v3 : VDREG3,
  out float4 o0 : SV_Target0)
{
  uint4 r0, r1, r2;
  r0 = 0u; r1 = 0u; r2 = 0u;

  r0.xy = vd_asuint((vd_asfloat(v1.xy) + vd_asfloat(_ShadowOffset.xy)));
  r0.zw = vd_asuint((-(int2)(vd_asfloat(r0.xy) >= float2(0.0, 0.0))));
  r0.z = (r0.w & r0.z);
  r1.xy = vd_asuint((-(int2)(float2(1.0, 1.0) >= vd_asfloat(r0.xy))));
  r0.z = (r0.z & r1.x);
  r0.z = (r1.y & r0.z);
  if (r0.z != 0u) {
    r1.xyzw = vd_asuint(_SourceTex.Sample(sampler_SourceTex, vd_asfloat(v1.xy)).xyzw);
    r0.xyzw = vd_asuint(_ResultTex.Sample(sampler_ResultTex, vd_asfloat(r0.xy)).xyzw);
    r0.xyzw = vd_asuint((vd_asfloat(r0.wwww) * vd_asfloat(_ShadowColor.wxyz)));
    r0.xyzw = vd_asuint(saturate((vd_asfloat(r0.xyzw) * vd_asfloat((_ShadowHardness).xxxx))));
    r1.x = vd_asuint(((-(vd_asfloat(_SourceAlpha))) * vd_asfloat(r1.w) + 1.0));
    r0.xyzw = vd_asuint((vd_asfloat(r0.yzwx) * vd_asfloat(r1.xxxx)));
  } else {
    r0.xyzw = uint4(0x00000000u, 0x00000000u, 0x00000000u, 0x00000000u);
  }
  r1.xy = vd_asuint(((-(vd_asfloat(_ClipRect.xy))) + vd_asfloat(_ClipRect.zw)));
  r1.xy = vd_asuint((vd_asfloat(r1.xy) + (-(abs(vd_asfloat(v3.xy))))));
  r1.xy = vd_asuint(saturate((vd_asfloat(r1.xy) * vd_asfloat(v3.zw))));
  r1.x = vd_asuint((vd_asfloat(r1.y) * vd_asfloat(r1.x)));
  r2.w = vd_asuint((vd_asfloat(r0.w) * vd_asfloat(r1.x)));
  r0.xyz = vd_asuint((vd_asfloat(r0.xyz) * vd_asfloat(r1.xxx)));
  r2.xyz = vd_asuint((vd_asfloat(r0.xyz) * vd_asfloat(v2.www)));
  o0.xyzw = (vd_asfloat(r2.xyzw) * vd_asfloat(v2.xyzw));
  return;
}
