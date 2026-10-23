// LCArt/PlaneReflection/Shader_PlaneReflectionOverrider | pass 0 "ForwardLit" | progFragment | keywords: (none) | blob 6
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
  if (r0.x != 0u) discard;
  r0.xyzw = vd_asuint(_BaseMap.SampleBias(sampler_BaseMap, vd_asfloat(v3.xy), vd_asfloat(_GlobalMipBias.x)).xyzw);
  r1.xyzw = vd_asuint((vd_asfloat(r0.xyzw) * vd_asfloat(_BaseColor.xyzw)));
  r0.x = vd_asuint((vd_asfloat(r0.w) * vd_asfloat(_BaseColor.w) + -0.100000001));
  r0.x = vd_asuint((-(int)(vd_asfloat(r0.x) < 0.0)));
  if (r0.x != 0u) discard;
  r0.xyzw = vd_asuint(_MultiMap.SampleBias(sampler_MultiMap, vd_asfloat(v3.xy), vd_asfloat(_GlobalMipBias.x)).xyzw);
  r0.xyz = vd_asuint((vd_asfloat(r1.xyz) * vd_asfloat(r0.xxx)));
  r0.xyz = vd_asuint((vd_asfloat(r0.xyz) * vd_asfloat((_EmissionIntensity).xxx)));
  r0.xyz = vd_asuint((vd_asfloat(r0.xyz) * float3(2.0, 2.0, 2.0) + vd_asfloat(r1.xyz)));
  r0.w = vd_asuint((vd_asfloat(v1.y) + (-(vd_asfloat(_PlaneReflectionPivotHeight)))));
  r0.w = vd_asuint(saturate((vd_asfloat(r0.w) * 0.5)));
  r0.w = vd_asuint(((-(vd_asfloat(r0.w))) + 1.0));
  r0.xyz = vd_asuint((vd_asfloat(r0.xyz) + (-(vd_asfloat(_PlaneReflectionBackgroundColor.xyz)))));
  o0.xyz = (vd_asfloat(r0.www) * vd_asfloat(r0.xyz) + vd_asfloat(_PlaneReflectionBackgroundColor.xyz));
  o0.w = vd_asfloat(r1.w);
  return;
}
