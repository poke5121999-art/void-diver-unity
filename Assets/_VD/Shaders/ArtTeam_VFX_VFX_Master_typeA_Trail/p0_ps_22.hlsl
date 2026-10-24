// ArtTeam/VFX/VFX_Master_typeA_Trail | pass 0 "Universal Forward" | progFragment | keywords: _DBUFFER_MRT3 _SURFACE_TYPE_TRANSPARENT | blob 22
void frag(
  float4 v0 : SV_POSITION0,
  float4 v1 : VDREG1,
  float4 v2 : VDREG2,
  float4 v3 : VDREG3,
  float4 v4 : VDREG4,
  out float4 o0 : SV_Target0)
{
  uint4 r0, r1, r2;
  r0 = 0u; r1 = 0u; r2 = 0u;

  r0.x = vd_asuint(dot(vd_asfloat(v4.xyz), vd_asfloat(v4.xyz)));
  r0.x = vd_asuint(sqrt(vd_asfloat(r0.x)));
  r0.x = vd_asuint((1.0 / vd_asfloat(r0.x)));
  r0.xyz = vd_asuint((vd_asfloat(r0.xxx) * vd_asfloat(v4.xyz)));
  r0.w = vd_asuint(dot(vd_asfloat(r0.xyz), vd_asfloat(r0.xyz)));
  r0.w = vd_asuint(rsqrt(vd_asfloat(r0.w)));
  r0.xyz = vd_asuint((vd_asfloat(r0.www) * vd_asfloat(r0.xyz)));
  r1.xyz = vd_asuint(((-(vd_asfloat(v3.xyz))) + vd_asfloat(_WorldSpaceCameraPos.xyz)));
  r0.w = vd_asuint(dot(vd_asfloat(r1.xyz), vd_asfloat(r1.xyz)));
  r0.w = vd_asuint(rsqrt(vd_asfloat(r0.w)));
  r1.xyz = vd_asuint((vd_asfloat(r0.www) * vd_asfloat(r1.xyz)));
  r0.w = vd_asuint((-(int)(vd_asfloat(unity_OrthoParams.w) == 0.0)));
  r2.x = ((r0.w != 0u) ? r1.x : vd_asuint((VD_COL(unity_MatrixV, 0).z)));
  r2.y = ((r0.w != 0u) ? r1.y : vd_asuint((VD_COL(unity_MatrixV, 1).z)));
  r2.z = ((r0.w != 0u) ? r1.z : vd_asuint((VD_COL(unity_MatrixV, 2).z)));
  r0.x = vd_asuint(saturate(dot(vd_asfloat(r0.xyz), vd_asfloat(r2.xyz))));
  r0.x = vd_asuint(((-(vd_asfloat(r0.x))) + 1.0));
  r0.x = vd_asuint(((-(vd_asfloat(r0.x))) + 1.0));
  r0.y = vd_asuint((vd_asfloat(_FresnelMask_Threshold) + 1.0));
  r0.y = vd_asuint((vd_asfloat(_FresnelMask_Offset) * vd_asfloat(r0.y) + (-(vd_asfloat(_FresnelMask_Threshold)))));
  r0.x = vd_asuint(((-(vd_asfloat(r0.y))) + vd_asfloat(r0.x)));
  r0.y = vd_asuint(((-(vd_asfloat(r0.y))) + vd_asfloat(_FresnelMask_Offset)));
  r0.y = vd_asuint((1.0 / vd_asfloat(r0.y)));
  r0.x = vd_asuint(saturate((vd_asfloat(r0.y) * vd_asfloat(r0.x))));
  r0.y = vd_asuint((vd_asfloat(r0.x) * -2.0 + 3.0));
  r0.x = vd_asuint((vd_asfloat(r0.x) * vd_asfloat(r0.x)));
  r0.x = vd_asuint((vd_asfloat(r0.x) * vd_asfloat(r0.y)));
  r0.x = vd_asuint(min(vd_asfloat(r0.x), 1.0));
  r0.y = vd_asuint((vd_asfloat(r0.x) * -2.0 + 3.0));
  r0.x = vd_asuint((vd_asfloat(r0.x) * vd_asfloat(r0.x)));
  r0.x = vd_asuint((vd_asfloat(r0.x) * vd_asfloat(r0.y)));
#if UNITY_UV_STARTS_AT_TOP
  r0.y = vd_asuint((-(int)(vd_asfloat(_ProjectionParams.x) < 0.0)));
#else
  r0.y = vd_asuint((-(int)(0.0 < vd_asfloat(_ProjectionParams.x))));
#endif
  r0.z = vd_asuint(((-(vd_asfloat(v0.y))) + vd_asfloat(_ScaledScreenParams.y)));
  r0.y = ((r0.y != 0u) ? r0.z : vd_asuint(v0.y));
  r0.y = vd_asuint((vd_asfloat(r0.y) / vd_asfloat(_ScaledScreenParams.y)));
  r0.y = vd_asuint(((-(vd_asfloat(r0.y))) + 1.0));
  r0.z = vd_asuint((vd_asfloat(_ScreenAlphaSmoothness) + 1.0));
  r0.z = vd_asuint((vd_asfloat(_ScreenAlphaDissolve) * vd_asfloat(r0.z) + (-(vd_asfloat(_ScreenAlphaSmoothness)))));
  r0.y = vd_asuint(((-(vd_asfloat(r0.z))) + vd_asfloat(r0.y)));
  r0.z = vd_asuint((1.0 / vd_asfloat(_ScreenAlphaSmoothness)));
  r0.y = vd_asuint(saturate((vd_asfloat(r0.z) * vd_asfloat(r0.y))));
  r0.z = vd_asuint((vd_asfloat(r0.y) * -2.0 + 3.0));
  r0.y = vd_asuint((vd_asfloat(r0.y) * vd_asfloat(r0.y)));
  r0.w = vd_asuint((vd_asfloat(_TimeParameters.x) * vd_asfloat(_DeformSpeed)));
  r0.w = vd_asuint((vd_asfloat(r0.w) * 0.0166666675));
  r1.x = vd_asuint((-(int)(vd_asfloat(r0.w) >= (-(vd_asfloat(r0.w))))));
  r0.w = vd_asuint(frac(abs(vd_asfloat(r0.w))));
  r0.w = vd_asuint(((r1.x != 0u) ? vd_asfloat(r0.w) : (-(vd_asfloat(r0.w)))));
  r0.w = vd_asuint((vd_asfloat(r0.w) * 60.0));
  r1.xy = vd_asuint((vd_asfloat(r0.ww) * vd_asfloat(_DeformDirection.xy) + vd_asfloat(v1.xy)));
  r1.xy = vd_asuint((vd_asfloat(r1.xy) * vd_asfloat(_DeformTex_ST.xy) + vd_asfloat(_DeformTex_ST.zw)));
  r1.xyzw = vd_asuint(_DeformTex.SampleBias(sampler_DeformTex, vd_asfloat(r1.xy), vd_asfloat(_GlobalMipBias.x)).xyzw);
  r1.x = vd_asuint((vd_asfloat(r1.x) * vd_asfloat(r1.w)));
  r1.xy = vd_asuint((vd_asfloat(r1.xy) * float2(2.0, 2.0) + float2(-1.0, -1.0)));
  r0.w = vd_asuint((vd_asfloat(_TimeParameters.x) * vd_asfloat(_MainTexFlowSpeed)));
  r1.z = vd_asuint((-(int)(vd_asfloat(r0.w) >= (-(vd_asfloat(r0.w))))));
  r0.w = vd_asuint(frac(abs(vd_asfloat(r0.w))));
  r0.w = vd_asuint(((r1.z != 0u) ? vd_asfloat(r0.w) : (-(vd_asfloat(r0.w)))));
  r1.zw = vd_asuint((vd_asfloat(r0.ww) * float2(1.0, 0.0) + vd_asfloat(v1.xy)));
  r1.xy = vd_asuint((vd_asfloat((_DeformStrength).xx) * vd_asfloat(r1.xy) + vd_asfloat(r1.zw)));
  r1.zw = vd_asuint((vd_asfloat(r1.xy) * vd_asfloat(_DissolveTex_ST.xy) + vd_asfloat(_DissolveTex_ST.zw)));
  r1.xy = vd_asuint((vd_asfloat(r1.xy) * vd_asfloat(_MainTex_ST.xy) + vd_asfloat(_MainTex_ST.zw)));
  r2.xyzw = vd_asuint(_MainTex.SampleBias(sampler_MainTex, vd_asfloat(r1.xy), vd_asfloat(_GlobalMipBias.x)).xyzw);
  r1.xyzw = vd_asuint(_DissolveTex.SampleBias(sampler_DissolveTex, vd_asfloat(r1.zw), vd_asfloat(_GlobalMipBias.x)).xyzw);
  r0.y = vd_asuint((vd_asfloat(r0.z) * vd_asfloat(r0.y) + (-(vd_asfloat(r1.x)))));
  r0.z = vd_asuint(((-(vd_asfloat(r1.x))) + 1.0));
  r0.z = vd_asuint((1.0 / vd_asfloat(r0.z)));
  r0.y = vd_asuint(saturate((vd_asfloat(r0.z) * vd_asfloat(r0.y))));
  r0.z = vd_asuint((vd_asfloat(r0.y) * -2.0 + 3.0));
  r0.y = vd_asuint((vd_asfloat(r0.y) * vd_asfloat(r0.y)));
  r0.y = vd_asuint((vd_asfloat(r0.y) * vd_asfloat(r0.z)));
  r0.z = vd_asuint(saturate(vd_asfloat(_Dissolve)));
  r0.w = vd_asuint(saturate(vd_asfloat(v2.w)));
  r0.z = vd_asuint((vd_asfloat(r0.z) * vd_asfloat(r0.w) + vd_asfloat(r1.x)));
  r0.z = vd_asuint(max(vd_asfloat(r0.z), 0.0));
  r0.z = vd_asuint(log2(vd_asfloat(r0.z)));
  r0.z = vd_asuint((vd_asfloat(r0.z) * vd_asfloat(_DissovePower)));
  r0.z = vd_asuint(exp2(vd_asfloat(r0.z)));
  r0.yz = vd_asuint(min(vd_asfloat(r0.yz), float2(1.0, 1.0)));
  r0.w = vd_asuint((vd_asfloat(r2.w) * vd_asfloat(v2.w)));
  r1.xy = vd_asuint((vd_asfloat(v1.xy) * vd_asfloat(_MaskTex_ST.xy) + vd_asfloat(_MaskTex_ST.zw)));
  r1.xyzw = vd_asuint(_MaskTex.SampleBias(sampler_MaskTex, vd_asfloat(r1.xy), vd_asfloat(_GlobalMipBias.x)).xyzw);
  r1.x = vd_asuint((vd_asfloat(r1.w) * vd_asfloat(r1.x)));
  r0.w = vd_asuint((vd_asfloat(r0.w) * vd_asfloat(r1.x)));
  r0.z = vd_asuint((vd_asfloat(r0.w) * vd_asfloat(r0.z)));
  r0.y = vd_asuint((vd_asfloat(r0.z) * vd_asfloat(r0.y) + (-(vd_asfloat(r0.z)))));
  r0.y = vd_asuint((vd_asfloat(_ScreenAlphaStrength) * vd_asfloat(r0.y) + vd_asfloat(r0.z)));
  o0.w = (vd_asfloat(r0.x) * vd_asfloat(r0.y));
  r0.xyz = vd_asuint(((-(vd_asfloat(r2.xxx))) + vd_asfloat(r2.xyz)));
  r0.xyz = vd_asuint((vd_asfloat((_isColor).xxx) * vd_asfloat(r0.xyz) + vd_asfloat(r2.xxx)));
  r0.xyz = vd_asuint((vd_asfloat(r0.xyz) * vd_asfloat(v2.xyz)));
  r0.w = vd_asuint(max(vd_asfloat(_Emmisive), 1.0));
  r0.xyz = vd_asuint((vd_asfloat(r0.www) * vd_asfloat(r0.xyz)));
  o0.xyz = (vd_asfloat(r0.xyz) * vd_asfloat(_MainColor.xyz));
  return;
}
