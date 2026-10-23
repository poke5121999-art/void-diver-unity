// LCArt/Character/shader_DE_Character_Normal | pass 3 "RimLight" | progFragment | keywords: _ADDITIONAL_LIGHTS _DE_PRERENDERING_CAMERA | blob 98
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

  r0.xyzw = vd_asuint(_MainTex.SampleBias(sampler_MainTex, vd_asfloat(v2.xy), vd_asfloat(_GlobalMipBias.x)).xyzw);
  r0.x = vd_asuint((vd_asfloat(r0.w) * vd_asfloat(v5.w) + -0.899999976));
  r0.x = vd_asuint((-(int)(vd_asfloat(r0.x) < 0.0)));
  if (r0.x != 0u) discard;
  r0.x = vd_asuint((-(int)(vd_asfloat(v2.z) >= 0.75)));
  if (r0.x == 0u) discard;
  r0.x = vd_asuint(saturate(((-(vd_asfloat(_DissolveProgress))) + 1.0)));
  r0.x = vd_asuint((-(int)(vd_asfloat(r0.x) >= 0.5)));
  if (r0.x == 0u) discard;
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
  r0.x = vd_asuint(saturate(((-(vd_asfloat(_IsLocalGhost))) + 1.0)));
  r0.x = vd_asuint((vd_asfloat(r0.x) * vd_asfloat(_GhostEnable)));
  r0.x = vd_asuint((-(int)(vd_asfloat(r0.x) >= 0.999000013)));
  o0.xyz = vd_asfloat(((r0.xxx != 0u) ? uint3(0x00000000u, 0x00000000u, 0x00000000u) : vd_asuint(v3.xyz)));
  o0.w = vd_asfloat(0x3f800000u);
  return;
}
