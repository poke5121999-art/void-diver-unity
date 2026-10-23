// ArtTeam/VFX/VFX_Master_typeA_Trail | pass 0 "Universal Forward" | progFragment | keywords: FOG_LINEAR _DBUFFER_MRT3 | blob 25
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
  r0.y = vd_asuint((vd_asfloat(_TimeParameters.x) * vd_asfloat(_DeformSpeed)));
  r0.y = vd_asuint((vd_asfloat(r0.y) * 0.0166666675));
  r0.z = vd_asuint((-(int)(vd_asfloat(r0.y) >= (-(vd_asfloat(r0.y))))));
  r0.y = vd_asuint(frac(abs(vd_asfloat(r0.y))));
  r0.y = vd_asuint(((r0.z != 0u) ? vd_asfloat(r0.y) : (-(vd_asfloat(r0.y)))));
  r0.y = vd_asuint((vd_asfloat(r0.y) * 60.0));
  r0.yz = vd_asuint((vd_asfloat(r0.yy) * vd_asfloat(_DeformDirection.xy) + vd_asfloat(v1.xy)));
  r0.yz = vd_asuint((vd_asfloat(r0.yz) * vd_asfloat(_DeformTex_ST.xy) + vd_asfloat(_DeformTex_ST.zw)));
  r1.xyzw = vd_asuint(_DeformTex.SampleBias(sampler_DeformTex, vd_asfloat(r0.yz), vd_asfloat(_GlobalMipBias.x)).xyzw);
  r1.x = vd_asuint((vd_asfloat(r1.x) * vd_asfloat(r1.w)));
  r0.yz = vd_asuint((vd_asfloat(r1.xy) * float2(2.0, 2.0) + float2(-1.0, -1.0)));
  r0.w = vd_asuint((vd_asfloat(_TimeParameters.x) * vd_asfloat(_MainTexFlowSpeed)));
  r1.x = vd_asuint((-(int)(vd_asfloat(r0.w) >= (-(vd_asfloat(r0.w))))));
  r0.w = vd_asuint(frac(abs(vd_asfloat(r0.w))));
  r0.w = vd_asuint(((r1.x != 0u) ? vd_asfloat(r0.w) : (-(vd_asfloat(r0.w)))));
  r1.xy = vd_asuint((vd_asfloat(r0.ww) * float2(1.0, 0.0) + vd_asfloat(v1.xy)));
  r0.yz = vd_asuint((vd_asfloat((_DeformStrength).xx) * vd_asfloat(r0.yz) + vd_asfloat(r1.xy)));
  r0.yz = vd_asuint((vd_asfloat(r0.yz) * vd_asfloat(_MainTex_ST.xy) + vd_asfloat(_MainTex_ST.zw)));
  r1.xyzw = vd_asuint(_MainTex.SampleBias(sampler_MainTex, vd_asfloat(r0.yz), vd_asfloat(_GlobalMipBias.x)).xyzw);
  r0.yzw = vd_asuint(((-(vd_asfloat(r1.xxx))) + vd_asfloat(r1.xyz)));
  r0.yzw = vd_asuint((vd_asfloat((_isColor).xxx) * vd_asfloat(r0.yzw) + vd_asfloat(r1.xxx)));
  r0.yzw = vd_asuint((vd_asfloat(r0.yzw) * vd_asfloat(v2.xyz)));
  r1.x = vd_asuint(max(vd_asfloat(_Emmisive), 1.0));
  r0.yzw = vd_asuint((vd_asfloat(r0.yzw) * vd_asfloat(r1.xxx)));
  r1.xyz = vd_asuint((vd_asfloat(r0.yzw) * vd_asfloat(_MainColor.xyz)));
  r0.yzw = vd_asuint(((-(vd_asfloat(_MainColor.xyz))) * vd_asfloat(r0.yzw) + vd_asfloat(unity_FogColor.xyz)));
  r0.xyz = vd_asuint((vd_asfloat(r0.xxx) * vd_asfloat(r0.yzw) + vd_asfloat(r1.xyz)));
  r1.xy = vd_asuint(((int2)vd_asfloat(v0.xy)));
  r1.zw = uint2(0x00000000u, 0x00000000u);
  r1.xyzw = vd_asuint(_DBufferTexture0.Load(vd_asint(r1.xyw)).xyzw);
  o0.xyz = (vd_asfloat(r0.xyz) * vd_asfloat(r1.www) + vd_asfloat(r1.xyz));
  o0.w = vd_asfloat(0x3f800000u);
  return;
}
