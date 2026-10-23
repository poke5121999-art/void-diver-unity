// LCArt/VFX/VFX_Decoration_SurfaceCutterTarget | pass 0 "ForwardLit" | progFragment | keywords: (none) | blob 3
void frag(
  float4 v0 : SV_POSITION0,
  float4 v1 : VDREG1,
  float4 v2 : VDREG2,
  float4 v3 : VDREG3,
  float4 v4 : VDREG4,
  float4 v5 : VDREG5,
  out float4 o0 : SV_Target0)
{
  uint4 r0;
  r0 = 0u;

  r0.x = vd_asuint(max(vd_asfloat(v4.x), 0.00100000005));
  r0.x = vd_asuint(saturate(((-(vd_asfloat(v1.y))) / vd_asfloat(r0.x))));
  r0.x = vd_asuint(log2(vd_asfloat(r0.x)));
  r0.x = vd_asuint((vd_asfloat(r0.x) * vd_asfloat(v4.y)));
  r0.x = vd_asuint(exp2(vd_asfloat(r0.x)));
  r0.yzw = vd_asuint(((-(vd_asfloat(v3.xyz))) + vd_asfloat(v5.xyz)));
  o0.xyz = (vd_asfloat(r0.xxx) * vd_asfloat(r0.yzw) + vd_asfloat(v3.xyz));
  o0.w = vd_asfloat(0x3f800000u);
  return;
}
