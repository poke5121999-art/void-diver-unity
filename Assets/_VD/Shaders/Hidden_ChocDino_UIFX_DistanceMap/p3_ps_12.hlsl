// Hidden/ChocDino/UIFX/DistanceMap | pass 3 "JumpFloodSingleAxis" | progFragment | keywords: DIST_DIAMOND | blob 12
void frag(
  float4 v0 : SV_POSITION0,
  float4 v1 : VDREG1,
  out float2 o0 : SV_Target0)
{
  uint4 r0, r1, r2;
  r0 = 0u; r1 = 0u; r2 = 0u;

  r0.xyzw = vd_asuint(_MainTex.Sample(sampler_MainTex, vd_asfloat(v1.xy)).xyzw);
  r0.w = vd_asuint((-(int)(-1.0 < vd_asfloat(r0.x))));
  if (r0.w != 0u) {
    r1.xy = vd_asuint((vd_asfloat(r0.xy) + (-(vd_asfloat(v1.xy)))));
    r1.xy = vd_asuint((vd_asfloat(r1.xy) * vd_asfloat(_MainTex_TexelSize.zw)));
    r0.w = vd_asuint(dot(vd_asfloat(r1.xy), vd_asfloat(r1.xy)));
    r0.w = vd_asuint((-(int)(vd_asfloat(r0.w) < 2.0)));
    if (r0.w != 0u) {
      o0.xy = vd_asfloat(r0.xy);
      return;
    }
    r1.xy = vd_asuint((vd_asfloat(v1.xy) * vd_asfloat(_MainTex_TexelSize.zw)));
    r1.xy = vd_asuint((vd_asfloat(r0.xy) * vd_asfloat(_MainTex_TexelSize.zw) + (-(vd_asfloat(r1.xy)))));
    r0.z = vd_asuint((abs(vd_asfloat(r1.y)) + abs(vd_asfloat(r1.x))));
  } else {
    r0.xyz = uint3(0xbf800000u, 0xbf800000u, 0x4b800000u);
  }
  r1.xy = vd_asuint(((-(vd_asfloat(_MainTex_TexelSize.xy))) * vd_asfloat(_StepSize.xy) + vd_asfloat(v1.xy)));
  r1.xyzw = vd_asuint(_MainTex.Sample(sampler_MainTex, vd_asfloat(r1.xy)).xyzw);
  r0.w = vd_asuint((-(int)(-1.0 < vd_asfloat(r1.x))));
  r2.xy = vd_asuint((vd_asfloat(v1.xy) * vd_asfloat(_MainTex_TexelSize.zw)));
  r2.zw = vd_asuint((vd_asfloat(r1.xy) * vd_asfloat(_MainTex_TexelSize.zw) + (-(vd_asfloat(r2.xy)))));
  r1.z = vd_asuint((abs(vd_asfloat(r2.w)) + abs(vd_asfloat(r2.z))));
  r1.w = vd_asuint((-(int)(vd_asfloat(r1.z) < vd_asfloat(r0.z))));
  r1.xyz = ((r1.www != 0u) ? r1.xyz : r0.xyz);
  r0.xyz = ((r0.www != 0u) ? r1.xyz : r0.xyz);
  r1.xy = vd_asuint((vd_asfloat(_MainTex_TexelSize.xy) * vd_asfloat(_StepSize.xy) + vd_asfloat(v1.xy)));
  r1.xyzw = vd_asuint(_MainTex.Sample(sampler_MainTex, vd_asfloat(r1.xy)).xyzw);
  r0.w = vd_asuint((-(int)(-1.0 < vd_asfloat(r1.x))));
  r1.zw = vd_asuint((vd_asfloat(r1.xy) * vd_asfloat(_MainTex_TexelSize.zw) + (-(vd_asfloat(r2.xy)))));
  r1.z = vd_asuint((abs(vd_asfloat(r1.w)) + abs(vd_asfloat(r1.z))));
  r0.z = vd_asuint((-(int)(vd_asfloat(r1.z) < vd_asfloat(r0.z))));
  r1.xy = ((r0.zz != 0u) ? r1.xy : r0.xy);
  o0.xy = vd_asfloat(((r0.ww != 0u) ? r1.xy : r0.xy));
  return;
}
