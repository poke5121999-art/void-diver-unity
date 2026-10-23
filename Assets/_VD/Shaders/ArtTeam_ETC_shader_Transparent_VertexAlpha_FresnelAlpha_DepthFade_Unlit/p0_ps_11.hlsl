// ArtTeam/ETC/shader_Transparent_VertexAlpha_FresnelAlpha_DepthFade_Unlit | pass 0 "Universal Forward" | progFragment | keywords: FOG_LINEAR | blob 11
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

  r0.xyz = vd_asuint((vd_asfloat(v3.yyy) * vd_asfloat((VD_COL(unity_WorldToObject, 1).xyz))));
  r0.xyz = vd_asuint((vd_asfloat((VD_COL(unity_WorldToObject, 0).xyz)) * vd_asfloat(v3.xxx) + vd_asfloat(r0.xyz)));
  r0.xyz = vd_asuint((vd_asfloat((VD_COL(unity_WorldToObject, 2).xyz)) * vd_asfloat(v3.zzz) + vd_asfloat(r0.xyz)));
  r0.xyz = vd_asuint((vd_asfloat(r0.xyz) + vd_asfloat((VD_COL(unity_WorldToObject, 3).xyz))));
  r1.xyz = vd_asuint((vd_asfloat(r0.yyy) * vd_asfloat((VD_COL(unity_ObjectToWorld, 1).xyz))));
  r0.xyw = vd_asuint((vd_asfloat((VD_COL(unity_ObjectToWorld, 0).xyz)) * vd_asfloat(r0.xxx) + vd_asfloat(r1.xyz)));
  r0.xyz = vd_asuint((vd_asfloat((VD_COL(unity_ObjectToWorld, 2).xyz)) * vd_asfloat(r0.zzz) + vd_asfloat(r0.xyw)));
  r0.xyz = vd_asuint((vd_asfloat(r0.xyz) + vd_asfloat((VD_COL(unity_ObjectToWorld, 3).xyz))));
  r0.y = vd_asuint((vd_asfloat(r0.y) * vd_asfloat((VD_COL(unity_MatrixV, 1).z))));
  r0.x = vd_asuint((vd_asfloat((VD_COL(unity_MatrixV, 0).z)) * vd_asfloat(r0.x) + vd_asfloat(r0.y)));
  r0.x = vd_asuint((vd_asfloat((VD_COL(unity_MatrixV, 2).z)) * vd_asfloat(r0.z) + vd_asfloat(r0.x)));
  r0.x = vd_asuint((vd_asfloat(r0.x) + vd_asfloat((VD_COL(unity_MatrixV, 3).z))));
  r0.x = vd_asuint(((-(vd_asfloat(r0.x))) + (-(vd_asfloat(_ProjectionParams.y)))));
  r0.x = vd_asuint(max(vd_asfloat(r0.x), 0.0));
  r0.x = vd_asuint(saturate((vd_asfloat(r0.x) * vd_asfloat(unity_FogParams.z) + vd_asfloat(unity_FogParams.w))));
  r0.x = vd_asuint(((-(vd_asfloat(r0.x))) + 1.0));
  r0.yz = vd_asuint((vd_asfloat(v1.xy) * vd_asfloat(_MainTex_ST.xy) + vd_asfloat(_MainTex_ST.zw)));
  r1.xyzw = vd_asuint(_MainTex.SampleBias(sampler_MainTex, vd_asfloat(r0.yz), vd_asfloat(_GlobalMipBias.x)).xyzw);
  r0.yzw = vd_asuint((vd_asfloat(r1.xyz) * vd_asfloat(_TintColor.xyz)));
  r1.xyz = vd_asuint(((-(vd_asfloat(_TintColor.xyz))) * vd_asfloat(r1.xyz) + vd_asfloat(unity_FogColor.xyz)));
  o0.xyz = (vd_asfloat(r0.xxx) * vd_asfloat(r1.xyz) + vd_asfloat(r0.yzw));
  o0.w = vd_asfloat(0x3f800000u);
  return;
}
