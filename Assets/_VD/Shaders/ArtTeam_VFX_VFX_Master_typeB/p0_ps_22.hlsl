// ArtTeam/VFX/VFX_Master_typeB | pass 0 "Universal Forward" | progFragment | keywords: _PLANESYSTEMCULLINGTYPE_ALWAYS _SURFACE_TYPE_TRANSPARENT | blob 22
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

  r0.x = vd_asuint(dot(vd_asfloat(v6.xyz), vd_asfloat(v6.xyz)));
  r0.x = vd_asuint(sqrt(vd_asfloat(r0.x)));
  r0.x = vd_asuint((1.0 / vd_asfloat(r0.x)));
  r0.xyz = vd_asuint((vd_asfloat(r0.xxx) * vd_asfloat(v6.xyz)));
  r0.w = vd_asuint(dot(vd_asfloat(r0.xyz), vd_asfloat(r0.xyz)));
  r0.w = vd_asuint(rsqrt(vd_asfloat(r0.w)));
  r0.xyz = vd_asuint((vd_asfloat(r0.www) * vd_asfloat(r0.xyz)));
  r1.xyz = vd_asuint(((-(vd_asfloat(v5.xyz))) + vd_asfloat(_WorldSpaceCameraPos.xyz)));
  r0.w = vd_asuint(dot(vd_asfloat(r1.xyz), vd_asfloat(r1.xyz)));
  r0.w = vd_asuint(rsqrt(vd_asfloat(r0.w)));
  r1.xyz = vd_asuint((vd_asfloat(r0.www) * vd_asfloat(r1.xyz)));
  r0.w = vd_asuint((-(int)(vd_asfloat(unity_OrthoParams.w) == 0.0)));
  r2.x = ((r0.w != 0u) ? r1.x : vd_asuint((VD_COL(unity_MatrixV, 0).z)));
  r2.y = ((r0.w != 0u) ? r1.y : vd_asuint((VD_COL(unity_MatrixV, 1).z)));
  r2.z = ((r0.w != 0u) ? r1.z : vd_asuint((VD_COL(unity_MatrixV, 2).z)));
  r0.x = vd_asuint(saturate(dot(vd_asfloat(r0.xyz), vd_asfloat(r2.xyz))));
  r0.x = vd_asuint(((-(vd_asfloat(r0.x))) + 1.0));
  r0.x = vd_asuint(log2(vd_asfloat(r0.x)));
  r0.x = vd_asuint((vd_asfloat(r0.x) * vd_asfloat(_FresnelPower)));
  r0.x = vd_asuint(exp2(vd_asfloat(r0.x)));
  r0.x = vd_asuint((vd_asfloat(r0.x) * vd_asfloat(_isFresnel)));
  r0.y = vd_asuint(max(vd_asfloat(v2.w), 1.0));
  r0.zw = vd_asuint((float2(1.0, 1.0) / vd_asfloat(_ScreenParams.xy)));
  r0.zw = vd_asuint((vd_asfloat(r0.zw) * vd_asfloat((_Pixelation).xx)));
  r1.xy = vd_asuint((vd_asfloat(v1.xy) / vd_asfloat(r0.zw)));
  r1.xy = vd_asuint(floor(vd_asfloat(r1.xy)));
  r0.zw = vd_asuint((vd_asfloat(r0.zw) * vd_asfloat(r1.xy)));
  r0.zw = vd_asuint((vd_asfloat(r0.zw) * vd_asfloat(_MainTex_ST.xy) + vd_asfloat(_MainTex_ST.zw)));
  r1.xyzw = vd_asuint(_MainTex.SampleBias(sampler_MainTex, vd_asfloat(r0.zw), vd_asfloat(_GlobalMipBias.x)).xyzw);
  r2.xyz = vd_asuint((vd_asfloat(r1.xxx) * vd_asfloat(v4.xyz)));
  r0.yzw = vd_asuint((vd_asfloat(r2.xyz) * vd_asfloat(r0.yyy) + (-(vd_asfloat(r2.xyz)))));
  r0.xyz = vd_asuint((vd_asfloat(r0.xxx) * vd_asfloat(r0.yzw) + vd_asfloat(r2.xyz)));
  r2.xyz = vd_asuint(((-(vd_asfloat(r0.xyz))) + vd_asfloat(v3.xyz)));
  r0.w = vd_asuint((vd_asfloat(r1.z) * -2.0 + 1.0));
  r1.x = vd_asuint(saturate(vd_asfloat(_invertB)));
  r0.w = vd_asuint((vd_asfloat(r1.x) * vd_asfloat(r0.w) + vd_asfloat(r1.z)));
  r0.w = vd_asuint((vd_asfloat(r0.w) * vd_asfloat(v3.w)));
  r0.xyz = vd_asuint((vd_asfloat(r0.www) * vd_asfloat(r2.xyz) + vd_asfloat(r0.xyz)));
  o0.xyz = (vd_asfloat(r0.xyz) * vd_asfloat(v2.zzz));
  r0.x = vd_asuint(((-(vd_asfloat(r1.y))) + 1.0));
  r0.yz = vd_asuint(saturate(vd_asfloat(v2.yx)));
  r0.w = vd_asuint(((-(vd_asfloat(r0.y))) * 0.5 + 1.0));
  r1.x = vd_asuint((vd_asfloat(r0.y) * 0.5 + -1.0));
  r1.y = vd_asuint((vd_asfloat(r0.w) + (-(vd_asfloat(r1.x)))));
  r0.w = vd_asuint(((-(vd_asfloat(r0.y))) * 0.5 + vd_asfloat(r0.w)));
  r0.w = vd_asuint((1.0 / vd_asfloat(r0.w)));
  r0.z = vd_asuint((vd_asfloat(r0.z) * vd_asfloat(r1.y) + vd_asfloat(r1.x)));
  r0.x = vd_asuint((vd_asfloat(r0.z) + vd_asfloat(r0.x)));
  r0.x = vd_asuint(((-(vd_asfloat(r0.y))) * 0.5 + vd_asfloat(r0.x)));
  r0.x = vd_asuint(saturate((vd_asfloat(r0.w) * vd_asfloat(r0.x))));
  r0.y = vd_asuint((vd_asfloat(r0.x) * -2.0 + 3.0));
  r0.x = vd_asuint((vd_asfloat(r0.x) * vd_asfloat(r0.x)));
  r0.x = vd_asuint(saturate(((-(vd_asfloat(r0.y))) * vd_asfloat(r0.x) + vd_asfloat(r1.w))));
  r0.x = vd_asuint((vd_asfloat(r0.x) * vd_asfloat(v4.w)));
  r0.yz = vd_asuint((vd_asfloat(v1.xy) * vd_asfloat(_MaskTex_ST.xy) + vd_asfloat(_MaskTex_ST.zw)));
  r1.xyzw = vd_asuint(_MaskTex.SampleBias(sampler_MaskTex, vd_asfloat(r0.yz), vd_asfloat(_GlobalMipBias.x)).xyzw);
  r0.y = vd_asuint((vd_asfloat(r1.w) * vd_asfloat(r1.x)));
  o0.w = saturate((vd_asfloat(r0.y) * vd_asfloat(r0.x)));
  return;
}
