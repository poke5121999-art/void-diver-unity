// ArtTeam/VFX/VFX_Master_typeB | pass 0 "Universal Forward" | progFragment | keywords: _ALPHATEST_ON _PLANESYSTEMCULLINGTYPE_ALWAYS | blob 24
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
  uint4 r0, r1, r2, r3;
  r0 = 0u; r1 = 0u; r2 = 0u; r3 = 0u;

  r0.xy = vd_asuint((float2(1.0, 1.0) / vd_asfloat(_ScreenParams.xy)));
  r0.xy = vd_asuint((vd_asfloat(r0.xy) * vd_asfloat((_Pixelation).xx)));
  r0.zw = vd_asuint((vd_asfloat(v1.xy) / vd_asfloat(r0.xy)));
  r0.zw = vd_asuint(floor(vd_asfloat(r0.zw)));
  r0.xy = vd_asuint((vd_asfloat(r0.xy) * vd_asfloat(r0.zw)));
  r0.xy = vd_asuint((vd_asfloat(r0.xy) * vd_asfloat(_MainTex_ST.xy) + vd_asfloat(_MainTex_ST.zw)));
  r0.xyzw = vd_asuint(_MainTex.SampleBias(sampler_MainTex, vd_asfloat(r0.xy), vd_asfloat(_GlobalMipBias.x)).xyzw);
  r1.xy = vd_asuint(((-(vd_asfloat(r0.zy))) + float2(1.0, 1.0)));
  r1.zw = vd_asuint(saturate(vd_asfloat(v2.yx)));
  r0.y = vd_asuint(((-(vd_asfloat(r1.z))) * 0.5 + 1.0));
  r2.x = vd_asuint((vd_asfloat(r1.z) * 0.5 + -1.0));
  r2.y = vd_asuint((vd_asfloat(r0.y) + (-(vd_asfloat(r2.x)))));
  r0.y = vd_asuint(((-(vd_asfloat(r1.z))) * 0.5 + vd_asfloat(r0.y)));
  r0.y = vd_asuint((1.0 / vd_asfloat(r0.y)));
  r1.w = vd_asuint((vd_asfloat(r1.w) * vd_asfloat(r2.y) + vd_asfloat(r2.x)));
  r1.y = vd_asuint((vd_asfloat(r1.w) + vd_asfloat(r1.y)));
  r1.x = vd_asuint(((-(vd_asfloat(r0.z))) + vd_asfloat(r1.x)));
  r1.y = vd_asuint(((-(vd_asfloat(r1.z))) * 0.5 + vd_asfloat(r1.y)));
  r0.y = vd_asuint(saturate((vd_asfloat(r0.y) * vd_asfloat(r1.y))));
  r1.y = vd_asuint((vd_asfloat(r0.y) * -2.0 + 3.0));
  r0.y = vd_asuint((vd_asfloat(r0.y) * vd_asfloat(r0.y)));
  r0.y = vd_asuint(saturate(((-(vd_asfloat(r1.y))) * vd_asfloat(r0.y) + vd_asfloat(r0.w))));
  r0.y = vd_asuint((vd_asfloat(r0.y) * vd_asfloat(v4.w)));
  r1.yz = vd_asuint((vd_asfloat(v1.xy) * vd_asfloat(_MaskTex_ST.xy) + vd_asfloat(_MaskTex_ST.zw)));
  r2.xyzw = vd_asuint(_MaskTex.SampleBias(sampler_MaskTex, vd_asfloat(r1.yz), vd_asfloat(_GlobalMipBias.x)).xyzw);
  r0.w = vd_asuint((vd_asfloat(r2.w) * vd_asfloat(r2.x)));
  r0.y = vd_asuint(saturate((vd_asfloat(r0.w) * vd_asfloat(r0.y))));
  r0.w = vd_asuint(ddx(vd_asfloat(r0.y)));
  r1.y = vd_asuint(ddy(vd_asfloat(r0.y)));
  r0.y = vd_asuint((vd_asfloat(r0.y) + (-(vd_asfloat(_AlphaClipThreshold)))));
  r0.w = vd_asuint((abs(vd_asfloat(r0.w)) + abs(vd_asfloat(r1.y))));
  r1.y = vd_asuint(((-(vd_asfloat(r0.w))) * 0.5 + vd_asfloat(r0.y)));
  r0.w = vd_asuint(max(vd_asfloat(r0.w), 9.99999975e-05));
  r0.w = vd_asuint((vd_asfloat(r1.y) / vd_asfloat(r0.w)));
  r0.w = vd_asuint(saturate((vd_asfloat(r0.w) + 1.0)));
  r1.y = vd_asuint((-(int)(0.0 >= vd_asfloat(_AlphaClipThreshold))));
  r0.w = ((r1.y != 0u) ? 0x3f800000u : r0.w);
  r1.y = (~r1.y);
  r1.z = vd_asuint((vd_asfloat(r0.w) + -9.99999975e-05));
  r1.w = vd_asuint((-(int)(vd_asfloat(_AlphaToMaskAvailable) != 0.0)));
  r1.y = (r1.y & r1.w);
  o0.w = vd_asfloat(((r1.w != 0u) ? r0.w : 0x3f800000u));
  r0.y = ((r1.y != 0u) ? r1.z : r0.y);
  r0.y = vd_asuint((-(int)(vd_asfloat(r0.y) < 0.0)));
  if (r0.y != 0u) discard;
  r0.y = vd_asuint(saturate(vd_asfloat(_invertB)));
  r0.y = vd_asuint((vd_asfloat(r0.y) * vd_asfloat(r1.x) + vd_asfloat(r0.z)));
  r0.xzw = vd_asuint((vd_asfloat(r0.xxx) * vd_asfloat(v4.xyz)));
  r0.y = vd_asuint((vd_asfloat(r0.y) * vd_asfloat(v3.w)));
  r1.x = vd_asuint(dot(vd_asfloat(v6.xyz), vd_asfloat(v6.xyz)));
  r1.x = vd_asuint(sqrt(vd_asfloat(r1.x)));
  r1.x = vd_asuint((1.0 / vd_asfloat(r1.x)));
  r1.xyz = vd_asuint((vd_asfloat(r1.xxx) * vd_asfloat(v6.xyz)));
  r1.w = vd_asuint(dot(vd_asfloat(r1.xyz), vd_asfloat(r1.xyz)));
  r1.w = vd_asuint(rsqrt(vd_asfloat(r1.w)));
  r1.xyz = vd_asuint((vd_asfloat(r1.www) * vd_asfloat(r1.xyz)));
  r2.xyz = vd_asuint(((-(vd_asfloat(v5.xyz))) + vd_asfloat(_WorldSpaceCameraPos.xyz)));
  r1.w = vd_asuint(dot(vd_asfloat(r2.xyz), vd_asfloat(r2.xyz)));
  r1.w = vd_asuint(rsqrt(vd_asfloat(r1.w)));
  r2.xyz = vd_asuint((vd_asfloat(r1.www) * vd_asfloat(r2.xyz)));
  r1.w = vd_asuint((-(int)(vd_asfloat(unity_OrthoParams.w) == 0.0)));
  r3.x = ((r1.w != 0u) ? r2.x : vd_asuint((VD_COL(unity_MatrixV, 0).z)));
  r3.y = ((r1.w != 0u) ? r2.y : vd_asuint((VD_COL(unity_MatrixV, 1).z)));
  r3.z = ((r1.w != 0u) ? r2.z : vd_asuint((VD_COL(unity_MatrixV, 2).z)));
  r1.x = vd_asuint(saturate(dot(vd_asfloat(r1.xyz), vd_asfloat(r3.xyz))));
  r1.x = vd_asuint(((-(vd_asfloat(r1.x))) + 1.0));
  r1.x = vd_asuint(log2(vd_asfloat(r1.x)));
  r1.x = vd_asuint((vd_asfloat(r1.x) * vd_asfloat(_FresnelPower)));
  r1.x = vd_asuint(exp2(vd_asfloat(r1.x)));
  r1.x = vd_asuint((vd_asfloat(r1.x) * vd_asfloat(_isFresnel)));
  r1.y = vd_asuint(max(vd_asfloat(v2.w), 1.0));
  r1.yzw = vd_asuint((vd_asfloat(r0.xzw) * vd_asfloat(r1.yyy) + (-(vd_asfloat(r0.xzw)))));
  r0.xzw = vd_asuint((vd_asfloat(r1.xxx) * vd_asfloat(r1.yzw) + vd_asfloat(r0.xzw)));
  r1.xyz = vd_asuint(((-(vd_asfloat(r0.xzw))) + vd_asfloat(v3.xyz)));
  r0.xyz = vd_asuint((vd_asfloat(r0.yyy) * vd_asfloat(r1.xyz) + vd_asfloat(r0.xzw)));
  o0.xyz = (vd_asfloat(r0.xyz) * vd_asfloat(v2.zzz));
  return;
}
