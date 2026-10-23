// Universal Render Pipeline/Nature/SpeedTree8_PBRLit | pass 5 "Universal 2D" | progFragment | keywords: BACKFACE_NORMAL_MODE_MIRROR | blob 56
void frag(
  float4 v0 : SV_POSITION0,
  float4 v1 : VDREG1,
  float4 v2 : VDREG2,
  float4 v3 : VDREG3,
  float4 v4 : VDREG4,
  float4 v5 : VDREG5,
  float4 v6 : VDREG6,
  float4 v7 : VDREG7,
  uint v8 : SV_IsFrontFace0,
  out float4 o0 : SV_TARGET0)
{
  uint4 r0, r1, r2, r3, r4;
  r0 = 0u; r1 = 0u; r2 = 0u; r3 = 0u; r4 = 0u;

  r0.xyzw = vd_asuint(_MainTex.SampleBias(sampler_MainTex, vd_asfloat(v2.xy), vd_asfloat(_GlobalMipBias.x)).xyzw);
  r1.x = vd_asuint((vd_asfloat(v3.w) * vd_asfloat(r0.w) + (-(vd_asfloat(_AlphaClipThreshold)))));
  r1.x = vd_asuint((-(int)(vd_asfloat(r1.x) < 0.0)));
  if (r1.x != 0u) discard;
  r1.xyz = vd_asuint(((-(vd_asfloat(_HueVariationColor.xyz))) + float3(1.0, 1.0, 1.0)));
  r2.xyz = vd_asuint(((-(vd_asfloat(_Color.xyz))) * vd_asfloat(r0.xyz) + float3(1.0, 1.0, 1.0)));
  r2.xyz = vd_asuint((vd_asfloat(r2.xyz) + vd_asfloat(r2.xyz)));
  r1.xyz = vd_asuint(((-(vd_asfloat(r2.xyz))) * vd_asfloat(r1.xyz) + float3(1.0, 1.0, 1.0)));
  r2.xyz = vd_asuint((vd_asfloat(r0.xyz) * vd_asfloat(_Color.xyz)));
  r3.xyz = vd_asuint((-(int3)(float3(0.5, 0.5, 0.5) >= vd_asfloat(r2.xyz))));
  r4.xyz = ((r3.xyz != 0u) ? uint3(0x00000000u, 0x00000000u, 0x00000000u) : uint3(0x3f800000u, 0x3f800000u, 0x3f800000u));
  r3.xyz = (r3.xyz & uint3(0x3f800000u, 0x3f800000u, 0x3f800000u));
  r1.xyz = vd_asuint((vd_asfloat(r1.xyz) * vd_asfloat(r4.xyz)));
  r4.xyz = vd_asuint((vd_asfloat(r2.xyz) * vd_asfloat(_HueVariationColor.xyz)));
  r4.xyz = vd_asuint((vd_asfloat(r4.xyz) + vd_asfloat(r4.xyz)));
  r1.xyz = vd_asuint((vd_asfloat(r4.xyz) * vd_asfloat(r3.xyz) + vd_asfloat(r1.xyz)));
  r1.xyz = vd_asuint(((-(vd_asfloat(_Color.xyz))) * vd_asfloat(r0.xyz) + vd_asfloat(r1.xyz)));
  r1.w = vd_asuint((vd_asfloat((VD_COL(unity_ObjectToWorld, 3).y)) + vd_asfloat((VD_COL(unity_ObjectToWorld, 3).x))));
  r1.w = vd_asuint((vd_asfloat(r1.w) + vd_asfloat((VD_COL(unity_ObjectToWorld, 3).z))));
  r1.w = vd_asuint(frac(vd_asfloat(r1.w)));
  r1.w = vd_asuint(saturate((vd_asfloat(r1.w) * vd_asfloat(_HueVariationColor.w))));
  r1.xyz = vd_asuint((vd_asfloat(r1.www) * vd_asfloat(r1.xyz) + vd_asfloat(r2.xyz)));
  r0.xyz = vd_asuint(((-(vd_asfloat(_Color.xyz))) * vd_asfloat(r0.xyz) + vd_asfloat(_HueVariationColor.xyz)));
  r0.w = vd_asuint((vd_asfloat(r0.w) * vd_asfloat(v3.w)));
  o0.w = vd_asfloat(r0.w);
  r0.xyz = vd_asuint((vd_asfloat(r1.www) * vd_asfloat(r0.xyz) + vd_asfloat(r2.xyz)));
  r3.xy = vd_asuint((-(int2)(float2(0.0, 0.0) != vd_asfloat(float2(_OldHueVarBehavior, _HueVariationKwToggle)))));
  r0.xyz = ((r3.xxx != 0u) ? r0.xyz : r1.xyz);
  r0.w = vd_asuint(max(vd_asfloat(r0.y), vd_asfloat(r0.x)));
  r0.w = vd_asuint(max(vd_asfloat(r0.z), vd_asfloat(r0.w)));
  r1.x = vd_asuint(max(vd_asfloat(r2.y), vd_asfloat(r2.x)));
  r1.x = vd_asuint(max(vd_asfloat(r2.z), vd_asfloat(r1.x)));
  r0.w = vd_asuint((vd_asfloat(r1.x) / vd_asfloat(r0.w)));
  r0.w = vd_asuint((vd_asfloat(r0.w) * 0.5 + 0.5));
  r0.xyz = vd_asuint(saturate((vd_asfloat(r0.www) * vd_asfloat(r0.xyz))));
  o0.xyz = vd_asfloat(((r3.yyy != 0u) ? r0.xyz : r2.xyz));
  return;
}
