// LCArt/BGAnim/shader_DE_BGAnim_Additive | pass 0 "ForwardLit" | progFragment | keywords: (none) | blob 2
void frag(
  float4 v0 : SV_POSITION0,
  float4 v1 : VDREG1,
  float4 v2 : VDREG2,
  float4 v3 : VDREG3,
  float4 v4 : VDREG4,
  float4 v5 : VDREG5,
  float4 v6 : VDREG6,
  out float4 o0 : SV_Target0)
{
#if UNITY_UV_STARTS_AT_TOP
  uint4 r0, r1, r2;
  r0 = 0u; r1 = 0u; r2 = 0u;
#else
  uint4 r0, r1, r2, r3;
  r0 = 0u; r1 = 0u; r2 = 0u; r3 = 0u;
#endif

#if UNITY_UV_STARTS_AT_TOP
#else
  r3.xyzw = vd_asuint(v0.xyzw);
  r3.z = vd_asuint(((-(vd_asfloat(v0.z))) + 1.0));
#endif
  r0.x = vd_asuint((-(int)(vd_asfloat(_GlobalBoxAreaEnable) >= 0.5)));
  if (r0.x != 0u) {
    r0.x = 0x00000000u;
    while (true) {
      r0.y = vd_asuint((-(int)(vd_asint(r0.x) >= 8)));
      if (r0.y != 0u) break;
      r0.y = vd_asuint((-(int)(vd_asfloat(_GlobalBoxAreaCoverCharacters[(int)(r0.x)]) < 0.5)));
      if (r0.y != 0u) {
        r0.y = vd_asuint((vd_asint(r0.x) + 1));
        r0.x = r0.y;
        continue;
      }
      r0.yzw = vd_asuint((vd_asfloat((VD_COL(unity_ObjectToWorld, 3).xyz)) + (-(vd_asfloat(_GlobalBoxAreaCenters[(int)(r0.x)].xyz)))));
      r0.yzw = vd_asuint((abs(vd_asfloat(r0.yzw)) + (-(vd_asfloat(_GlobalBoxAreaExtents[(int)(r0.x)].xyz)))));
      r0.y = vd_asuint(max(vd_asfloat(r0.z), vd_asfloat(r0.y)));
      r0.y = vd_asuint(max(vd_asfloat(r0.w), vd_asfloat(r0.y)));
      r0.y = vd_asuint((-(int)(0.0 >= vd_asfloat(r0.y))));
      if (r0.y != 0u) {
        if (0xffffffffu != 0u) discard;
        break;
      }
      r0.x = vd_asuint((vd_asint(r0.x) + 1));
    }
  }
  r0.xyzw = vd_asuint(_MainTex.SampleBias(sampler_MainTex, vd_asfloat(v1.xy), vd_asfloat(_GlobalMipBias.x)).xyzw);
  r1.x = vd_asuint((vd_asfloat(r0.w) + -0.100000001));
  r1.x = vd_asuint((-(int)(vd_asfloat(r1.x) < 0.0)));
  if (r1.x != 0u) discard;
  r1.xyz = vd_asuint(log2(abs(vd_asfloat(v5.xyz))));
  r1.xyz = vd_asuint((vd_asfloat(r1.xyz) * float3(2.20000005, 2.20000005, 2.20000005)));
  r1.xyz = vd_asuint(exp2(vd_asfloat(r1.xyz)));
  r0.xyz = vd_asuint((vd_asfloat(r0.xyz) * vd_asfloat(r1.xyz)));
  r0.w = vd_asuint(log2(abs(vd_asfloat(r0.w))));
  r0.w = vd_asuint((vd_asfloat(r0.w) * 1.5));
  o0.w = exp2(vd_asfloat(r0.w));
  r1.xy = vd_asuint((vd_asfloat(v6.xy) / vd_asfloat(v6.ww)));
  r1.xyzw = vd_asuint(_CustomDepthBuffer.SampleBias(sampler_CustomDepthBuffer, vd_asfloat(r1.xy), vd_asfloat(_GlobalMipBias.x)).xyzw);
#if UNITY_UV_STARTS_AT_TOP
  r0.w = vd_asuint(((-(vd_asfloat(r1.z))) + vd_asfloat(v0.z)));
#else
  r0.w = vd_asuint(((-(vd_asfloat(r1.z))) + vd_asfloat(r3.z)));
#endif
  r0.w = vd_asuint((vd_asfloat(r0.w) + -7.50000036e-05));
  r0.w = vd_asuint(saturate((vd_asfloat(r0.w) * -6666.6665)));
  r1.x = vd_asuint((vd_asfloat(r0.w) * -2.0 + 3.0));
  r0.w = vd_asuint((vd_asfloat(r0.w) * vd_asfloat(r0.w)));
  r1.y = vd_asuint((vd_asfloat(r0.w) * vd_asfloat(r1.x)));
  r2.xy = vd_asuint(((-(vd_asfloat(float2(_GlobalSeeThroughCircleRange, _GlobalSeeThroughCircleAlpha)))) + float2(1.0, 1.0)));
  r1.z = vd_asuint(((-(vd_asfloat(r2.x))) + 1.0));
  r1.w = vd_asuint((vd_asfloat(r1.w) + (-(vd_asfloat(r2.x)))));
  r1.z = vd_asuint((1.0 / vd_asfloat(r1.z)));
  r1.z = vd_asuint(saturate((vd_asfloat(r1.z) * vd_asfloat(r1.w))));
  r1.w = vd_asuint((vd_asfloat(r1.z) * -2.0 + 3.0));
  r1.z = vd_asuint((vd_asfloat(r1.z) * vd_asfloat(r1.z)));
  r1.z = vd_asuint((vd_asfloat(r1.z) * vd_asfloat(r1.w)));
  r0.w = vd_asuint(((-(vd_asfloat(r1.x))) * vd_asfloat(r0.w) + 1.0));
  r0.w = vd_asuint((vd_asfloat(r1.z) * vd_asfloat(r0.w) + vd_asfloat(r1.y)));
  r0.w = vd_asuint((vd_asfloat(r0.w) + -1.0));
  r0.w = vd_asuint((vd_asfloat(_GlobalBackgroundSeeThroughEnable) * vd_asfloat(r0.w) + 1.0));
  r0.w = vd_asuint((vd_asfloat(r0.w) * vd_asfloat(r2.y) + vd_asfloat(_GlobalSeeThroughCircleAlpha)));
  r1.x = vd_asuint((vd_asfloat(_GlobalPlaneSystemEnable) * vd_asfloat(_CharacterAreaMaskOffEnable)));
  r1.x = vd_asuint((vd_asfloat(r1.x) * vd_asfloat(_GlobalBackgroundSeeThroughEnable)));
  r0.w = vd_asuint((vd_asfloat(r0.w) + -1.0));
  r0.w = vd_asuint((vd_asfloat(r1.x) * vd_asfloat(r0.w) + 1.0));
  r0.w = vd_asuint(min(vd_asfloat(r0.w), vd_asfloat(v2.w)));
  r0.w = vd_asuint((vd_asfloat(r0.w) + -1.0));
  r0.w = vd_asuint((vd_asfloat(_SightEffectEnable) * vd_asfloat(r0.w) + 1.0));
  r0.xyz = vd_asuint((vd_asfloat(r0.www) * vd_asfloat(r0.xyz)));
  r1.xyz = vd_asuint((vd_asfloat(r0.xyz) * float3(0.5, 0.5, 0.5)));
  r0.w = vd_asuint(saturate((vd_asfloat(_DissolveProgress) + vd_asfloat(_DissolveProgress))));
  r0.xyz = vd_asuint(((-(vd_asfloat(r0.xyz))) * float3(0.5, 0.5, 0.5) + vd_asfloat(_DissolveColor.xyz)));
  r0.xyz = vd_asuint((vd_asfloat(r0.www) * vd_asfloat(r0.xyz) + vd_asfloat(r1.xyz)));
  r0.w = vd_asuint(saturate((vd_asfloat(_DissolveProgress) + -0.5)));
  r0.w = vd_asuint(((-(vd_asfloat(r0.w))) * 2.0 + 1.0));
  r0.xyz = vd_asuint((vd_asfloat(r0.www) * vd_asfloat(r0.xyz)));
  r0.xyz = vd_asuint(log2(abs(vd_asfloat(r0.xyz))));
  r0.xyz = vd_asuint((vd_asfloat(r0.xyz) * vd_asfloat((_AdditivePower).xxx)));
  r0.xyz = vd_asuint(exp2(vd_asfloat(r0.xyz)));
  o0.xyz = (vd_asfloat(r0.xyz) * vd_asfloat((_AdditiveIntensity).xxx));
  return;
}
