// LCArt/Background/shader_DE_BG_Base | pass 3 "Reflection" | progFragment | keywords: (none) | blob 74
void frag(
  float4 v0 : SV_POSITION0,
  float4 v1 : VDREG1,
  float4 v2 : VDREG2,
  float4 v3 : VDREG3,
  out float4 o0 : SV_Target0)
{
  uint4 r0, r1;
  r0 = 0u; r1 = 0u;

  r0.x = vd_asuint((vd_asfloat(_PlaneReflectionPivotHeight) + 0.075000003));
  r0.x = vd_asuint((-(int)(vd_asfloat(v1.y) < vd_asfloat(r0.x))));
  if (r0.x != 0u) {
    if (0xffffffffu != 0u) discard;
  }
  r0.xyzw = vd_asuint(_BaseMap.SampleBias(sampler_BaseMap, vd_asfloat(v3.xy), vd_asfloat(_GlobalMipBias.x)).xyzw);
  r1.x = vd_asuint((vd_asfloat(r0.w) + -0.100000001));
  r1.x = vd_asuint((-(int)(vd_asfloat(r1.x) < 0.0)));
  if (r1.x != 0u) discard;
  r1.xyzw = vd_asuint(_EmissionMap.SampleBias(sampler_vd_point_repeat, vd_asfloat(v3.xy), vd_asfloat(_GlobalMipBias.x)).xyzw);
  r1.y = vd_asuint(((-(vd_asfloat(r1.z))) + 0.5));
  r1.y = vd_asuint((-(int)(vd_asfloat(r1.y) < 0.0)));
  if (r1.y != 0u) discard;
  r1.y = vd_asuint((-(int)(0.5 >= vd_asfloat(r1.z))));
  r1.y = (r1.y & 0x3f800000u);
  r1.x = vd_asuint((vd_asfloat(r1.y) * vd_asfloat(r1.x)));
  r1.xyz = vd_asuint((vd_asfloat(r0.xyz) * vd_asfloat(r1.xxx)));
  r1.xyz = vd_asuint((vd_asfloat(r1.xyz) * vd_asfloat((_EmissionIntensity).xxx)));
  r0.xyz = vd_asuint((vd_asfloat(r1.xyz) * float3(2.0, 2.0, 2.0) + vd_asfloat(r0.xyz)));
  r1.xyzw = vd_asuint((vd_asfloat(r0.xyzw) * vd_asfloat(_BaseColor.xyzw)));
  r0.w = vd_asuint((vd_asfloat(v1.y) + (-(vd_asfloat(_PlaneReflectionPivotHeight)))));
  r0.w = vd_asuint(saturate((vd_asfloat(r0.w) * 0.5)));
  r0.xyz = vd_asuint(((-(vd_asfloat(r0.xyz))) * vd_asfloat(_BaseColor.xyz) + vd_asfloat(_PlaneReflectionBackgroundColor.xyz)));
  o0.xyz = (vd_asfloat(r0.www) * vd_asfloat(r0.xyz) + vd_asfloat(r1.xyz));
  o0.w = vd_asfloat(r1.w);
  return;
}
