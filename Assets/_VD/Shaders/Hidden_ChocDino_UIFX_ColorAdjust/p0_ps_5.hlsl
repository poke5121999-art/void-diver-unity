// Hidden/ChocDino/UIFX/ColorAdjust | pass 0 "ColorAdjust" | progFragment | keywords: (none) | blob 5
void frag(
  float4 v0 : SV_POSITION0,
  float4 v1 : VDREG1,
  out float4 o0 : SV_Target0)
{
  uint4 r0, r1, r2;
  r0 = 0u; r1 = 0u; r2 = 0u;

  r0.xyzw = vd_asuint(_MainTex.Sample(sampler_MainTex, vd_asfloat(v1.xy)).xyzw);
  r1.x = vd_asuint((-(int)(0.0 < vd_asfloat(r0.w))));
  r1.yzw = vd_asuint((vd_asfloat(r0.xyz) / vd_asfloat(r0.www)));
  r1.xyz = ((r1.xxx != 0u) ? r1.yzw : r0.xyz);
  r2.xyz = vd_asuint((vd_asfloat(r1.yyy) * vd_asfloat((VD_COL(_ColorMatrix, 1).xyz))));
  r1.xyw = vd_asuint((vd_asfloat((VD_COL(_ColorMatrix, 0).xyz)) * vd_asfloat(r1.xxx) + vd_asfloat(r2.xyz)));
  r1.xyz = vd_asuint((vd_asfloat((VD_COL(_ColorMatrix, 2).xyz)) * vd_asfloat(r1.zzz) + vd_asfloat(r1.xyw)));
  r1.xyz = vd_asuint(saturate((vd_asfloat(r1.xyz) + vd_asfloat((VD_COL(_ColorMatrix, 3).xyz)))));
  r1.xyz = vd_asuint(log2(vd_asfloat(r1.xyz)));
  r1.xyz = vd_asuint((vd_asfloat(r1.xyz) * float3(0.416666657, 0.416666657, 0.416666657)));
  r1.xyz = vd_asuint(exp2(vd_asfloat(r1.xyz)));
  r1.xyz = vd_asuint((vd_asfloat(r1.xyz) * float3(1.05499995, 1.05499995, 1.05499995) + float3(-0.0549999997, -0.0549999997, -0.0549999997)));
  r1.xyz = vd_asuint(max(vd_asfloat(r1.xyz), float3(0.0, 0.0, 0.0)));
  r1.xyz = vd_asuint((vd_asfloat(r1.xyz) + float3(-0.5, -0.5, -0.5)));
  r1.w = vd_asuint((vd_asfloat(_BCPO.y) + 1.0));
  r1.xyz = vd_asuint(saturate((vd_asfloat(r1.xyz) * vd_asfloat(r1.www) + float3(0.5, 0.5, 0.5))));
  r0.xyz = vd_asuint(saturate((vd_asfloat(r1.xyz) + vd_asfloat(_BCPO.xxx))));
  r0.xyzw = vd_asuint((vd_asfloat(r0.xyzw) + float4(-0.5, -0.5, -0.5, -0.5)));
  r1.xyzw = vd_asuint((vd_asfloat(_ContrastRGBA.xyzw) + float4(1.0, 1.0, 1.0, 1.0)));
  r0.xyzw = vd_asuint(saturate((vd_asfloat(r0.xyzw) * vd_asfloat(r1.xyzw) + float4(0.5, 0.5, 0.5, 0.5))));
  r0.xyzw = vd_asuint(saturate((vd_asfloat(r0.xyzw) + vd_asfloat(_BrightnessRGBA.xyzw))));
  r1.xyz = vd_asuint((vd_asfloat(r0.xyz) * float3(0.305306017, 0.305306017, 0.305306017) + float3(0.682171106, 0.682171106, 0.682171106)));
  r1.xyz = vd_asuint((vd_asfloat(r0.xyz) * vd_asfloat(r1.xyz) + float3(0.0125228781, 0.0125228781, 0.0125228781)));
  r1.xyz = vd_asuint((vd_asfloat(r0.xyz) * vd_asfloat(r1.xyz)));
  r0.xyz = vd_asuint((vd_asfloat(r0.www) * vd_asfloat(r1.xyz)));
  o0.xyzw = (vd_asfloat(r0.xyzw) * vd_asfloat(_BCPO.wwww));
  return;
}
