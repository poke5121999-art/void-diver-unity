// ArtTeam/VFX/VFX_Master_typeA_Trail | pass 0 "Universal Forward" | progFragment | keywords: _DBUFFER_MRT3 | blob 21
void frag(
  float4 v0 : SV_POSITION0,
  float4 v1 : VDREG1,
  float4 v2 : VDREG2,
  float4 v3 : VDREG3,
  float4 v4 : VDREG4,
  out float4 o0 : SV_Target0)
{
  uint4 r0, r1;
  r0 = 0u; r1 = 0u;

  r0.x = vd_asuint((vd_asfloat(_TimeParameters.x) * vd_asfloat(_DeformSpeed)));
  r0.x = vd_asuint((vd_asfloat(r0.x) * 0.0166666675));
  r0.y = vd_asuint((-(int)(vd_asfloat(r0.x) >= (-(vd_asfloat(r0.x))))));
  r0.x = vd_asuint(frac(abs(vd_asfloat(r0.x))));
  r0.x = vd_asuint(((r0.y != 0u) ? vd_asfloat(r0.x) : (-(vd_asfloat(r0.x)))));
  r0.x = vd_asuint((vd_asfloat(r0.x) * 60.0));
  r0.xy = vd_asuint((vd_asfloat(r0.xx) * vd_asfloat(_DeformDirection.xy) + vd_asfloat(v1.xy)));
  r0.xy = vd_asuint((vd_asfloat(r0.xy) * vd_asfloat(_DeformTex_ST.xy) + vd_asfloat(_DeformTex_ST.zw)));
  r0.xyzw = vd_asuint(_DeformTex.SampleBias(sampler_DeformTex, vd_asfloat(r0.xy), vd_asfloat(_GlobalMipBias.x)).xyzw);
  r0.x = vd_asuint((vd_asfloat(r0.x) * vd_asfloat(r0.w)));
  r0.xy = vd_asuint((vd_asfloat(r0.xy) * float2(2.0, 2.0) + float2(-1.0, -1.0)));
  r0.z = vd_asuint((vd_asfloat(_TimeParameters.x) * vd_asfloat(_MainTexFlowSpeed)));
  r0.w = vd_asuint((-(int)(vd_asfloat(r0.z) >= (-(vd_asfloat(r0.z))))));
  r0.z = vd_asuint(frac(abs(vd_asfloat(r0.z))));
  r0.z = vd_asuint(((r0.w != 0u) ? vd_asfloat(r0.z) : (-(vd_asfloat(r0.z)))));
  r0.zw = vd_asuint((vd_asfloat(r0.zz) * float2(1.0, 0.0) + vd_asfloat(v1.xy)));
  r0.xy = vd_asuint((vd_asfloat((_DeformStrength).xx) * vd_asfloat(r0.xy) + vd_asfloat(r0.zw)));
  r0.xy = vd_asuint((vd_asfloat(r0.xy) * vd_asfloat(_MainTex_ST.xy) + vd_asfloat(_MainTex_ST.zw)));
  r0.xyzw = vd_asuint(_MainTex.SampleBias(sampler_MainTex, vd_asfloat(r0.xy), vd_asfloat(_GlobalMipBias.x)).xyzw);
  r0.yzw = vd_asuint(((-(vd_asfloat(r0.xxx))) + vd_asfloat(r0.xyz)));
  r0.xyz = vd_asuint((vd_asfloat((_isColor).xxx) * vd_asfloat(r0.yzw) + vd_asfloat(r0.xxx)));
  r0.xyz = vd_asuint((vd_asfloat(r0.xyz) * vd_asfloat(v2.xyz)));
  r0.w = vd_asuint(max(vd_asfloat(_Emmisive), 1.0));
  r0.xyz = vd_asuint((vd_asfloat(r0.www) * vd_asfloat(r0.xyz)));
  r0.xyz = vd_asuint((vd_asfloat(r0.xyz) * vd_asfloat(_MainColor.xyz)));
  r1.xy = vd_asuint(((int2)vd_asfloat(v0.xy)));
  r1.zw = uint2(0x00000000u, 0x00000000u);
  r1.xyzw = vd_asuint(_DBufferTexture0.Load(vd_asint(r1.xyw)).xyzw);
  o0.xyz = (vd_asfloat(r0.xyz) * vd_asfloat(r1.www) + vd_asfloat(r1.xyz));
  o0.w = vd_asfloat(0x3f800000u);
  return;
}
