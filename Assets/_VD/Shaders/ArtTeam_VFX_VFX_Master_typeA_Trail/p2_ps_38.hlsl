// ArtTeam/VFX/VFX_Master_typeA_Trail | pass 2 "MotionVectors" | progFragment | keywords: _ALPHATEST_ON | blob 38
void frag(
  float4 v0 : VDREG0,
  float4 v1 : VDREG1,
  float4 v2 : SV_POSITION0,
  float4 v3 : VDREG3,
  float4 v4 : VDREG4,
  float4 v5 : VDREG5,
  float4 v6 : VDREG6,
  out float4 o0 : SV_Target0)
{
  uint4 r0, r1, r2;
  r0 = 0u; r1 = 0u; r2 = 0u;

#if UNITY_UV_STARTS_AT_TOP
  r0.x = vd_asuint((-(int)(vd_asfloat(_ProjectionParams.x) < 0.0)));
#else
  r0.x = vd_asuint((-(int)(0.0 < vd_asfloat(_ProjectionParams.x))));
#endif
  r0.y = vd_asuint(((-(vd_asfloat(v2.y))) + vd_asfloat(_ScaledScreenParams.y)));
  r0.x = ((r0.x != 0u) ? r0.y : vd_asuint(v2.y));
  r0.x = vd_asuint((vd_asfloat(r0.x) / vd_asfloat(_ScaledScreenParams.y)));
  r0.x = vd_asuint(((-(vd_asfloat(r0.x))) + 1.0));
  r0.y = vd_asuint((vd_asfloat(_ScreenAlphaSmoothness) + 1.0));
  r0.y = vd_asuint((vd_asfloat(_ScreenAlphaDissolve) * vd_asfloat(r0.y) + (-(vd_asfloat(_ScreenAlphaSmoothness)))));
  r0.x = vd_asuint(((-(vd_asfloat(r0.y))) + vd_asfloat(r0.x)));
  r0.y = vd_asuint((1.0 / vd_asfloat(_ScreenAlphaSmoothness)));
  r0.x = vd_asuint(saturate((vd_asfloat(r0.y) * vd_asfloat(r0.x))));
  r0.y = vd_asuint((vd_asfloat(r0.x) * -2.0 + 3.0));
  r0.x = vd_asuint((vd_asfloat(r0.x) * vd_asfloat(r0.x)));
  r0.z = vd_asuint((vd_asfloat(_TimeParameters.x) * vd_asfloat(_DeformSpeed)));
  r0.z = vd_asuint((vd_asfloat(r0.z) * 0.0166666675));
  r0.w = vd_asuint((-(int)(vd_asfloat(r0.z) >= (-(vd_asfloat(r0.z))))));
  r0.z = vd_asuint(frac(abs(vd_asfloat(r0.z))));
  r0.z = vd_asuint(((r0.w != 0u) ? vd_asfloat(r0.z) : (-(vd_asfloat(r0.z)))));
  r0.z = vd_asuint((vd_asfloat(r0.z) * 60.0));
  r0.zw = vd_asuint((vd_asfloat(r0.zz) * vd_asfloat(_DeformDirection.xy) + vd_asfloat(v3.xy)));
  r0.zw = vd_asuint((vd_asfloat(r0.zw) * vd_asfloat(_DeformTex_ST.xy) + vd_asfloat(_DeformTex_ST.zw)));
  r1.xyzw = vd_asuint(_DeformTex.SampleBias(sampler_DeformTex, vd_asfloat(r0.zw), vd_asfloat(_GlobalMipBias.x)).xyzw);
  r1.x = vd_asuint((vd_asfloat(r1.x) * vd_asfloat(r1.w)));
  r0.zw = vd_asuint((vd_asfloat(r1.xy) * float2(2.0, 2.0) + float2(-1.0, -1.0)));
  r1.x = vd_asuint((vd_asfloat(_TimeParameters.x) * vd_asfloat(_MainTexFlowSpeed)));
  r1.y = vd_asuint((-(int)(vd_asfloat(r1.x) >= (-(vd_asfloat(r1.x))))));
  r1.x = vd_asuint(frac(abs(vd_asfloat(r1.x))));
  r1.x = vd_asuint(((r1.y != 0u) ? vd_asfloat(r1.x) : (-(vd_asfloat(r1.x)))));
  r1.xy = vd_asuint((vd_asfloat(r1.xx) * float2(1.0, 0.0) + vd_asfloat(v3.xy)));
  r0.zw = vd_asuint((vd_asfloat((_DeformStrength).xx) * vd_asfloat(r0.zw) + vd_asfloat(r1.xy)));
  r1.xy = vd_asuint((vd_asfloat(r0.zw) * vd_asfloat(_DissolveTex_ST.xy) + vd_asfloat(_DissolveTex_ST.zw)));
  r0.zw = vd_asuint((vd_asfloat(r0.zw) * vd_asfloat(_MainTex_ST.xy) + vd_asfloat(_MainTex_ST.zw)));
  r2.xyzw = vd_asuint(_MainTex.SampleBias(sampler_MainTex, vd_asfloat(r0.zw), vd_asfloat(_GlobalMipBias.x)).xyzw);
  r0.z = vd_asuint((vd_asfloat(r2.w) * vd_asfloat(v4.w)));
  r1.xyzw = vd_asuint(_DissolveTex.SampleBias(sampler_DissolveTex, vd_asfloat(r1.xy), vd_asfloat(_GlobalMipBias.x)).xyzw);
  r0.x = vd_asuint((vd_asfloat(r0.y) * vd_asfloat(r0.x) + (-(vd_asfloat(r1.x)))));
  r0.y = vd_asuint(((-(vd_asfloat(r1.x))) + 1.0));
  r0.y = vd_asuint((1.0 / vd_asfloat(r0.y)));
  r0.x = vd_asuint(saturate((vd_asfloat(r0.y) * vd_asfloat(r0.x))));
  r0.y = vd_asuint((vd_asfloat(r0.x) * -2.0 + 3.0));
  r0.x = vd_asuint((vd_asfloat(r0.x) * vd_asfloat(r0.x)));
  r0.x = vd_asuint((vd_asfloat(r0.x) * vd_asfloat(r0.y)));
  r0.y = vd_asuint(saturate(vd_asfloat(_Dissolve)));
  r0.w = vd_asuint(saturate(vd_asfloat(v4.w)));
  r0.y = vd_asuint((vd_asfloat(r0.y) * vd_asfloat(r0.w) + vd_asfloat(r1.x)));
  r0.y = vd_asuint(max(vd_asfloat(r0.y), 0.0));
  r0.y = vd_asuint(log2(vd_asfloat(r0.y)));
  r0.y = vd_asuint((vd_asfloat(r0.y) * vd_asfloat(_DissovePower)));
  r0.y = vd_asuint(exp2(vd_asfloat(r0.y)));
  r0.xy = vd_asuint(min(vd_asfloat(r0.xy), float2(1.0, 1.0)));
  r1.xy = vd_asuint((vd_asfloat(v3.xy) * vd_asfloat(_MaskTex_ST.xy) + vd_asfloat(_MaskTex_ST.zw)));
  r1.xyzw = vd_asuint(_MaskTex.SampleBias(sampler_MaskTex, vd_asfloat(r1.xy), vd_asfloat(_GlobalMipBias.x)).xyzw);
  r0.w = vd_asuint((vd_asfloat(r1.w) * vd_asfloat(r1.x)));
  r0.z = vd_asuint((vd_asfloat(r0.w) * vd_asfloat(r0.z)));
  r0.y = vd_asuint((vd_asfloat(r0.z) * vd_asfloat(r0.y)));
  r0.x = vd_asuint((vd_asfloat(r0.y) * vd_asfloat(r0.x) + (-(vd_asfloat(r0.y)))));
  r0.x = vd_asuint((vd_asfloat(_ScreenAlphaStrength) * vd_asfloat(r0.x) + vd_asfloat(r0.y)));
  r0.y = vd_asuint(dot(vd_asfloat(v6.xyz), vd_asfloat(v6.xyz)));
  r0.y = vd_asuint(sqrt(vd_asfloat(r0.y)));
  r0.y = vd_asuint((1.0 / vd_asfloat(r0.y)));
  r0.yzw = vd_asuint((vd_asfloat(r0.yyy) * vd_asfloat(v6.xyz)));
  r1.x = vd_asuint(dot(vd_asfloat(r0.yzw), vd_asfloat(r0.yzw)));
  r1.x = vd_asuint(rsqrt(vd_asfloat(r1.x)));
  r0.yzw = vd_asuint((vd_asfloat(r0.yzw) * vd_asfloat(r1.xxx)));
  r1.xyz = vd_asuint(((-(vd_asfloat(v5.xyz))) + vd_asfloat(_WorldSpaceCameraPos.xyz)));
  r1.w = vd_asuint(dot(vd_asfloat(r1.xyz), vd_asfloat(r1.xyz)));
  r1.w = vd_asuint(rsqrt(vd_asfloat(r1.w)));
  r1.xyz = vd_asuint((vd_asfloat(r1.www) * vd_asfloat(r1.xyz)));
  r1.w = vd_asuint((-(int)(vd_asfloat(unity_OrthoParams.w) == 0.0)));
  r2.x = ((r1.w != 0u) ? r1.x : vd_asuint((VD_COL(unity_MatrixV, 0).z)));
  r2.y = ((r1.w != 0u) ? r1.y : vd_asuint((VD_COL(unity_MatrixV, 1).z)));
  r2.z = ((r1.w != 0u) ? r1.z : vd_asuint((VD_COL(unity_MatrixV, 2).z)));
  r0.y = vd_asuint(saturate(dot(vd_asfloat(r0.yzw), vd_asfloat(r2.xyz))));
  r0.y = vd_asuint(((-(vd_asfloat(r0.y))) + 1.0));
  r0.y = vd_asuint(((-(vd_asfloat(r0.y))) + 1.0));
  r0.z = vd_asuint((vd_asfloat(_FresnelMask_Threshold) + 1.0));
  r0.z = vd_asuint((vd_asfloat(_FresnelMask_Offset) * vd_asfloat(r0.z) + (-(vd_asfloat(_FresnelMask_Threshold)))));
  r0.y = vd_asuint(((-(vd_asfloat(r0.z))) + vd_asfloat(r0.y)));
  r0.z = vd_asuint(((-(vd_asfloat(r0.z))) + vd_asfloat(_FresnelMask_Offset)));
  r0.z = vd_asuint((1.0 / vd_asfloat(r0.z)));
  r0.y = vd_asuint(saturate((vd_asfloat(r0.z) * vd_asfloat(r0.y))));
  r0.z = vd_asuint((vd_asfloat(r0.y) * -2.0 + 3.0));
  r0.y = vd_asuint((vd_asfloat(r0.y) * vd_asfloat(r0.y)));
  r0.y = vd_asuint((vd_asfloat(r0.y) * vd_asfloat(r0.z)));
  r0.y = vd_asuint(min(vd_asfloat(r0.y), 1.0));
  r0.z = vd_asuint((vd_asfloat(r0.y) * -2.0 + 3.0));
  r0.y = vd_asuint((vd_asfloat(r0.y) * vd_asfloat(r0.y)));
  r0.y = vd_asuint((vd_asfloat(r0.y) * vd_asfloat(r0.z)));
  r0.x = vd_asuint((vd_asfloat(r0.x) * vd_asfloat(r0.y) + (-(vd_asfloat(_AlphaThreshold)))));
  r0.x = vd_asuint((-(int)(vd_asfloat(r0.x) < 0.0)));
  if (r0.x != 0u) discard;
  r0.x = vd_asuint((1.0 / vd_asfloat(v1.z)));
  r0.xy = vd_asuint((vd_asfloat(r0.xx) * vd_asfloat(v1.xy)));
  r0.z = vd_asuint((1.0 / vd_asfloat(v0.z)));
  r0.xy = vd_asuint((vd_asfloat(v0.xy) * vd_asfloat(r0.zz) + (-(vd_asfloat(r0.xy)))));
  r0.xy = vd_asuint((vd_asfloat(r0.xy) * float2(0.5, -0.5)));
  r0.z = vd_asuint((-(int)(vd_asfloat(unity_MotionVectorsParams.y) != 0.0)));
  o0.xy = vd_asfloat((r0.xy & r0.zz));
  o0.zw = vd_asfloat(uint2(0x00000000u, 0x00000000u));
  return;
}
