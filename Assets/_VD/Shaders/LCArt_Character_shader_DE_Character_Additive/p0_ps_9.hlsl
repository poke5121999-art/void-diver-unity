// LCArt/Character/shader_DE_Character_Additive | pass 0 "Additive" | progFragment | keywords: _DE_EDITOR_CAMERA _DE_PRERENDERING_CAMERA | blob 9
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
  uint4 r0, r1, r2;
  r0 = 0u; r1 = 0u; r2 = 0u;

  r0.xyzw = vd_asuint(_MainTex.SampleBias(sampler_MainTex, vd_asfloat(v1.xy), vd_asfloat(_GlobalMipBias.x)).xyzw);
  r1.x = vd_asuint((vd_asfloat(r0.w) + -0.00100000005));
  r1.x = vd_asuint((-(int)(vd_asfloat(r1.x) < 0.0)));
  if (r1.x != 0u) discard;
  r1.x = vd_asuint((-(int)(0.5 < vd_asfloat(_UseLinearVertexColor))));
  r1.yzw = vd_asuint((vd_asfloat(r0.xyz) * vd_asfloat(v6.xyz)));
  r2.xyz = vd_asuint(log2(abs(vd_asfloat(v6.xyz))));
  r2.xyz = vd_asuint((vd_asfloat(r2.xyz) * float3(2.20000005, 2.20000005, 2.20000005)));
  r2.xyz = vd_asuint(exp2(vd_asfloat(r2.xyz)));
  r0.xyz = vd_asuint((vd_asfloat(r0.xyz) * vd_asfloat(r2.xyz)));
  r0.xyz = ((r1.xxx != 0u) ? r1.yzw : r0.xyz);
  r0.w = vd_asuint(log2(abs(vd_asfloat(r0.w))));
  r0.w = vd_asuint((vd_asfloat(r0.w) * 1.5));
  r0.w = vd_asuint(exp2(vd_asfloat(r0.w)));
  r1.x = vd_asuint((-(int)(vd_asfloat(_GlobalBoxAreaEnable) >= 0.5)));
  if (r1.x != 0u) {
    r1.x = 0x00000000u;
    while (true) {
      r1.y = vd_asuint((-(int)(vd_asint(r1.x) >= 8)));
      if (r1.y != 0u) break;
      r1.y = vd_asuint((-(int)(vd_asfloat(_GlobalBoxAreaCoverCharacters[(int)(r1.x)]) < 0.5)));
      if (r1.y != 0u) {
        r1.y = vd_asuint((vd_asint(r1.x) + 1));
        r1.x = r1.y;
        continue;
      }
      r1.yzw = vd_asuint((vd_asfloat((VD_COL(unity_ObjectToWorld, 3).xyz)) + (-(vd_asfloat(_GlobalBoxAreaCenters[(int)(r1.x)].xyz)))));
      r1.yzw = vd_asuint((abs(vd_asfloat(r1.yzw)) + (-(vd_asfloat(_GlobalBoxAreaExtents[(int)(r1.x)].xyz)))));
      r1.y = vd_asuint(max(vd_asfloat(r1.z), vd_asfloat(r1.y)));
      r1.y = vd_asuint(max(vd_asfloat(r1.w), vd_asfloat(r1.y)));
      r1.y = vd_asuint((-(int)(0.0 >= vd_asfloat(r1.y))));
      if (r1.y != 0u) {
        if (0xffffffffu != 0u) discard;
        break;
      }
      r1.x = vd_asuint((vd_asint(r1.x) + 1));
    }
  }
  r1.x = vd_asuint((vd_asfloat(v1.z) + -1.0));
  r1.x = vd_asuint((vd_asfloat(_SightEffectEnable) * vd_asfloat(r1.x) + 1.0));
  r0.xyz = vd_asuint((vd_asfloat(r0.xyz) * vd_asfloat(r1.xxx)));
  r0.xyz = vd_asuint((vd_asfloat(r0.xyz) * float3(0.5, 0.5, 0.5)));
  r0.xyz = vd_asuint((vd_asfloat(r0.www) * vd_asfloat(r0.xyz)));
  r1.xyz = vd_asuint((vd_asfloat(r0.xyz) * vd_asfloat(_AdditiveTint.xyz)));
  r0.w = vd_asuint(saturate((vd_asfloat(_DissolveProgress) + vd_asfloat(_DissolveProgress))));
  r0.xyz = vd_asuint(((-(vd_asfloat(r0.xyz))) * vd_asfloat(_AdditiveTint.xyz) + vd_asfloat(_DissolveColor.xyz)));
  r0.xyz = vd_asuint((vd_asfloat(r0.www) * vd_asfloat(r0.xyz) + vd_asfloat(r1.xyz)));
  r0.w = vd_asuint(saturate((vd_asfloat(_DissolveProgress) + -0.5)));
  r0.w = vd_asuint(((-(vd_asfloat(r0.w))) * 2.0 + 1.0));
  r0.xyz = vd_asuint((vd_asfloat(r0.www) * vd_asfloat(r0.xyz)));
  r0.xyz = vd_asuint(log2(abs(vd_asfloat(r0.xyz))));
  r0.xyz = vd_asuint((vd_asfloat(r0.xyz) * vd_asfloat((_AdditivePower).xxx)));
  r0.xyz = vd_asuint(exp2(vd_asfloat(r0.xyz)));
  o0.xyz = (vd_asfloat(r0.xyz) * vd_asfloat((_AdditiveIntensity).xxx));
  o0.w = vd_asfloat(vd_asuint(v6.w));
  return;
}
