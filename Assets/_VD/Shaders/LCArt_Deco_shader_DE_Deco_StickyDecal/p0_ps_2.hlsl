// LCArt/Deco/shader_DE_Deco_StickyDecal | pass 0 "FORWARD" | progFragment | keywords: (none) | blob 2
void frag(
  float4 v0 : SV_POSITION0,
  float4 v1 : VDREG1,
  float4 v2 : VDREG2,
  float4 v3 : VDREG3,
  float4 v4 : VDREG4,
  float4 v5 : VDREG5,
  float4 v6 : VDREG6,
  float4 v7 : VDREG7,
  out float4 o0 : SV_Target0)
{
  uint4 r0, r1;
  r0 = 0u; r1 = 0u;

  r0.x = vd_asuint(dot(vd_asfloat(v6.xyz), vd_asfloat(v6.xyz)));
  r0.x = vd_asuint(rsqrt(vd_asfloat(r0.x)));
  r0.xyz = vd_asuint((vd_asfloat(r0.xxx) * vd_asfloat(v6.xyz)));
  r1.x = vd_asuint((-(vd_asfloat((VD_COL(unity_MatrixV, 0).z)))));
  r1.y = vd_asuint((-(vd_asfloat((VD_COL(unity_MatrixV, 1).z)))));
  r1.z = vd_asuint((-(vd_asfloat((VD_COL(unity_MatrixV, 2).z)))));
  r0.w = vd_asuint(dot(vd_asfloat(r0.xyz), vd_asfloat(r1.xyz)));
  r0.xyz = vd_asuint((vd_asfloat(r0.xyz) / vd_asfloat(r0.www)));
  r1.xy = vd_asuint((vd_asfloat(v5.xy) / vd_asfloat(v5.ww)));
  r1.xyzw = vd_asuint(_CameraDepthTexture.SampleBias(sampler_vd_point_clamp, vd_asfloat(r1.xy), vd_asfloat(_GlobalMipBias.x)).xyzw);
  r0.w = vd_asuint((vd_asfloat(_ZBufferParams.z) * vd_asfloat(r1.x) + vd_asfloat(_ZBufferParams.w)));
  r0.w = vd_asuint((1.0 / vd_asfloat(r0.w)));
  r0.xyz = vd_asuint((vd_asfloat(r0.xyz) * vd_asfloat(r0.www) + vd_asfloat(_WorldSpaceCameraPos.xyz)));
  r1.xyz = vd_asuint((vd_asfloat(r0.yyy) * vd_asfloat((VD_COL(unity_WorldToObject, 1).xyz))));
  r0.xyw = vd_asuint((vd_asfloat((VD_COL(unity_WorldToObject, 0).xyz)) * vd_asfloat(r0.xxx) + vd_asfloat(r1.xyz)));
  r0.xyz = vd_asuint((vd_asfloat((VD_COL(unity_WorldToObject, 2).xyz)) * vd_asfloat(r0.zzz) + vd_asfloat(r0.xyw)));
  r0.xyz = vd_asuint((vd_asfloat(r0.xyz) + vd_asfloat((VD_COL(unity_WorldToObject, 3).xyz))));
  r0.xyz = vd_asuint(((-(vd_asfloat(_DecalOffset.xyz))) * vd_asfloat(r0.yyy) + vd_asfloat(r0.xyz)));
  r1.xyz = vd_asuint(((-(abs(vd_asfloat(r0.xyz)))) + float3(0.5, 0.5, 0.5)));
  r0.xyz = vd_asuint((vd_asfloat(r0.yxz) + float3(0.5, 0.5, 0.5)));
  r1.xyz = vd_asuint((-(int3)(vd_asfloat(r1.xyz) < float3(0.0, 0.0, 0.0))));
  r0.w = (r1.y | r1.x);
  r0.w = (r1.z | r0.w);
  if (r0.w != 0u) discard;
  r0.x = vd_asuint(saturate(vd_asfloat(r0.x)));
  r1.xyzw = vd_asuint(_MainTex.SampleBias(sampler_MainTex, vd_asfloat(r0.yz), vd_asfloat(_GlobalMipBias.x)).xyzw);
  r0.y = vd_asuint(((-(vd_asfloat(r0.x))) + 1.0));
  r0.x = vd_asuint(max(vd_asfloat(r0.y), vd_asfloat(r0.x)));
  r0.x = vd_asuint(((-(vd_asfloat(r0.x))) + 1.0));
  r0.x = vd_asuint(dot(vd_asfloat(r0.xx), vd_asfloat(r1.ww)));
  r0.y = vd_asuint(((-(vd_asfloat(_DissolveProgress))) + 1.0));
  r0.x = vd_asuint(((-(vd_asfloat(r0.x))) * vd_asfloat(r0.y) + 1.0));
  o0.xyz = saturate(vd_asfloat(r0.xxx));
  o0.w = vd_asfloat(0x3f800000u);
  return;
}
