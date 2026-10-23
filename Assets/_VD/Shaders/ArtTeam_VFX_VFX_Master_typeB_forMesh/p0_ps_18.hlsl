// ArtTeam/VFX/VFX_Master_typeB_forMesh | pass 0 "Universal Forward" | progFragment | keywords: FOG_LINEAR _ALPHATEST_ON | blob 18
void frag(
  float4 v0 : SV_POSITION0,
  float4 v1 : VDREG1,
  float4 v2 : VDREG2,
  float4 v3 : VDREG3,
  out float4 o0 : SV_Target0)
{
  uint4 r0, r1, r2, r3;
  r0 = 0u; r1 = 0u; r2 = 0u; r3 = 0u;

  r0.xy = vd_asuint(saturate(vd_asfloat(float2(_DissolveSharpness, _Dissolve))));
  r0.z = vd_asuint(((-(vd_asfloat(r0.x))) * 0.5 + 1.0));
  r0.w = vd_asuint((vd_asfloat(r0.x) * 0.5 + -1.0));
  r1.x = vd_asuint(((-(vd_asfloat(r0.w))) + vd_asfloat(r0.z)));
  r0.z = vd_asuint(((-(vd_asfloat(r0.x))) * 0.5 + vd_asfloat(r0.z)));
  r0.z = vd_asuint((1.0 / vd_asfloat(r0.z)));
  r0.y = vd_asuint((vd_asfloat(r0.y) * vd_asfloat(r1.x) + vd_asfloat(r0.w)));
  r1.xy = vd_asuint((vd_asfloat(v1.xy) * vd_asfloat(_MaskTex_G_Dissolve_B_2ndColor_A_alpha_ST.xy) + vd_asfloat(_MaskTex_G_Dissolve_B_2ndColor_A_alpha_ST.zw)));
  r1.xyzw = vd_asuint(_MaskTex_G_Dissolve_B_2ndColor_A_alpha.SampleBias(sampler_MaskTex_G_Dissolve_B_2ndColor_A_alpha, vd_asfloat(r1.xy), vd_asfloat(_GlobalMipBias.x)).xyzw);
  r1.xy = vd_asuint(((-(vd_asfloat(r1.zy))) + float2(1.0, 1.0)));
  r0.y = vd_asuint((vd_asfloat(r0.y) + vd_asfloat(r1.y)));
  r0.w = vd_asuint(((-(vd_asfloat(r1.z))) + vd_asfloat(r1.x)));
  r0.x = vd_asuint(((-(vd_asfloat(r0.x))) * 0.5 + vd_asfloat(r0.y)));
  r0.x = vd_asuint(saturate((vd_asfloat(r0.z) * vd_asfloat(r0.x))));
  r0.y = vd_asuint((vd_asfloat(r0.x) * -2.0 + 3.0));
  r0.x = vd_asuint((vd_asfloat(r0.x) * vd_asfloat(r0.x)));
  r0.x = vd_asuint(saturate(((-(vd_asfloat(r0.y))) * vd_asfloat(r0.x) + vd_asfloat(r1.w))));
  r0.y = vd_asuint((vd_asfloat(r0.x) * vd_asfloat(_TintColor.w)));
  r0.x = vd_asuint((vd_asfloat(_TintColor.w) * vd_asfloat(r0.x) + -0.5));
  r0.z = vd_asuint(ddx(vd_asfloat(r0.y)));
  r0.y = vd_asuint(ddy(vd_asfloat(r0.y)));
  r0.y = vd_asuint((abs(vd_asfloat(r0.y)) + abs(vd_asfloat(r0.z))));
  r0.z = vd_asuint(((-(vd_asfloat(r0.y))) * 0.5 + vd_asfloat(r0.x)));
  r0.y = vd_asuint(max(vd_asfloat(r0.y), 9.99999975e-05));
  r0.y = vd_asuint((vd_asfloat(r0.z) / vd_asfloat(r0.y)));
  r0.y = vd_asuint(saturate((vd_asfloat(r0.y) + 1.0)));
  r0.z = vd_asuint((vd_asfloat(r0.y) + -9.99999975e-05));
  r1.x = vd_asuint((-(int)(vd_asfloat(_AlphaToMaskAvailable) != 0.0)));
  r0.x = ((r1.x != 0u) ? r0.z : r0.x);
  o0.w = vd_asfloat(((r1.x != 0u) ? r0.y : 0x3f800000u));
  r0.x = vd_asuint((-(int)(vd_asfloat(r0.x) < 0.0)));
  if (r0.x != 0u) discard;
  r0.x = vd_asuint(saturate(vd_asfloat(_invertB)));
  r0.x = vd_asuint((vd_asfloat(r0.x) * vd_asfloat(r0.w) + vd_asfloat(r1.z)));
  r0.x = vd_asuint((vd_asfloat(r0.x) * vd_asfloat(_2ndColor.w)));
  r0.yz = vd_asuint((vd_asfloat(v1.xy) * vd_asfloat(_MainTex_ST.xy) + vd_asfloat(_MainTex_ST.zw)));
  r1.xyzw = vd_asuint(_MainTex.SampleBias(sampler_MainTex, vd_asfloat(r0.yz), vd_asfloat(_GlobalMipBias.x)).xyzw);
  r0.yzw = vd_asuint((vd_asfloat(r1.xyz) * vd_asfloat(_TintColor.xyz)));
  r1.xyz = vd_asuint(((-(vd_asfloat(_TintColor.xyz))) * vd_asfloat(r1.xyz) + vd_asfloat(_2ndColor.xyz)));
  r0.xyz = vd_asuint((vd_asfloat(r0.xxx) * vd_asfloat(r1.xyz) + vd_asfloat(r0.yzw)));
  r1.xyz = vd_asuint(((-(vd_asfloat(v2.xyz))) + vd_asfloat(_WorldSpaceCameraPos.xyz)));
  r0.w = vd_asuint(dot(vd_asfloat(r1.xyz), vd_asfloat(r1.xyz)));
  r0.w = vd_asuint(rsqrt(vd_asfloat(r0.w)));
  r1.xyz = vd_asuint((vd_asfloat(r0.www) * vd_asfloat(r1.xyz)));
  r0.w = vd_asuint((-(int)(vd_asfloat(unity_OrthoParams.w) == 0.0)));
  r2.x = vd_asuint((VD_COL(unity_MatrixV, 0).z));
  r2.y = vd_asuint((VD_COL(unity_MatrixV, 1).z));
  r2.z = vd_asuint((VD_COL(unity_MatrixV, 2).z));
  r1.xyz = ((r0.www != 0u) ? r1.xyz : r2.xyz);
  r0.w = vd_asuint(dot(vd_asfloat(v3.xyz), vd_asfloat(v3.xyz)));
  r0.w = vd_asuint(sqrt(vd_asfloat(r0.w)));
  r0.w = vd_asuint((1.0 / vd_asfloat(r0.w)));
  r2.xyz = vd_asuint((vd_asfloat(r0.www) * vd_asfloat(v3.xyz)));
  r0.w = vd_asuint(dot(vd_asfloat(r2.xyz), vd_asfloat(r2.xyz)));
  r0.w = vd_asuint(rsqrt(vd_asfloat(r0.w)));
  r2.xyz = vd_asuint((vd_asfloat(r0.www) * vd_asfloat(r2.xyz)));
  r0.w = vd_asuint(saturate(dot(vd_asfloat(r2.xyz), vd_asfloat(r1.xyz))));
  r0.w = vd_asuint(((-(vd_asfloat(r0.w))) + 1.0));
  r0.w = vd_asuint(log2(vd_asfloat(r0.w)));
  r0.w = vd_asuint((vd_asfloat(r0.w) * vd_asfloat(_FresnelPower)));
  r0.w = vd_asuint(exp2(vd_asfloat(r0.w)));
  r0.xyz = vd_asuint((vd_asfloat(r0.www) * vd_asfloat(_FresnelColor.xyz) + vd_asfloat(r0.xyz)));
  r1.xyz = vd_asuint((vd_asfloat(r0.xyz) * vd_asfloat((_Emission).xxx)));
  r0.xyz = vd_asuint(((-(vd_asfloat(r0.xyz))) * vd_asfloat((_Emission).xxx) + vd_asfloat(unity_FogColor.xyz)));
  r2.xyz = vd_asuint((vd_asfloat(v2.yyy) * vd_asfloat((VD_COL(unity_WorldToObject, 1).xyz))));
  r2.xyz = vd_asuint((vd_asfloat((VD_COL(unity_WorldToObject, 0).xyz)) * vd_asfloat(v2.xxx) + vd_asfloat(r2.xyz)));
  r2.xyz = vd_asuint((vd_asfloat((VD_COL(unity_WorldToObject, 2).xyz)) * vd_asfloat(v2.zzz) + vd_asfloat(r2.xyz)));
  r2.xyz = vd_asuint((vd_asfloat(r2.xyz) + vd_asfloat((VD_COL(unity_WorldToObject, 3).xyz))));
  r3.xyz = vd_asuint((vd_asfloat(r2.yyy) * vd_asfloat((VD_COL(unity_ObjectToWorld, 1).xyz))));
  r2.xyw = vd_asuint((vd_asfloat((VD_COL(unity_ObjectToWorld, 0).xyz)) * vd_asfloat(r2.xxx) + vd_asfloat(r3.xyz)));
  r2.xyz = vd_asuint((vd_asfloat((VD_COL(unity_ObjectToWorld, 2).xyz)) * vd_asfloat(r2.zzz) + vd_asfloat(r2.xyw)));
  r2.xyz = vd_asuint((vd_asfloat(r2.xyz) + vd_asfloat((VD_COL(unity_ObjectToWorld, 3).xyz))));
  r0.w = vd_asuint((vd_asfloat(r2.y) * vd_asfloat((VD_COL(unity_MatrixV, 1).z))));
  r0.w = vd_asuint((vd_asfloat((VD_COL(unity_MatrixV, 0).z)) * vd_asfloat(r2.x) + vd_asfloat(r0.w)));
  r0.w = vd_asuint((vd_asfloat((VD_COL(unity_MatrixV, 2).z)) * vd_asfloat(r2.z) + vd_asfloat(r0.w)));
  r0.w = vd_asuint((vd_asfloat(r0.w) + vd_asfloat((VD_COL(unity_MatrixV, 3).z))));
  r0.w = vd_asuint(((-(vd_asfloat(r0.w))) + (-(vd_asfloat(_ProjectionParams.y)))));
  r0.w = vd_asuint(max(vd_asfloat(r0.w), 0.0));
  r0.w = vd_asuint(saturate((vd_asfloat(r0.w) * vd_asfloat(unity_FogParams.z) + vd_asfloat(unity_FogParams.w))));
  r0.w = vd_asuint(((-(vd_asfloat(r0.w))) + 1.0));
  o0.xyz = (vd_asfloat(r0.www) * vd_asfloat(r0.xyz) + vd_asfloat(r1.xyz));
  return;
}
