// ArtTeam/VFX/VFX_Master_typeB | pass 0 "Universal Forward" | progFragment | keywords: FOG_LINEAR _DBUFFER_MRT3 _PLANESYSTEMCULLINGTYPE_ALWAYS | blob 39
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

  r0.xyz = vd_asuint(((-(vd_asfloat(v5.xyz))) + vd_asfloat(_WorldSpaceCameraPos.xyz)));
  r0.w = vd_asuint(dot(vd_asfloat(r0.xyz), vd_asfloat(r0.xyz)));
  r0.w = vd_asuint(rsqrt(vd_asfloat(r0.w)));
  r0.xyz = vd_asuint((vd_asfloat(r0.www) * vd_asfloat(r0.xyz)));
  r0.w = vd_asuint((-(int)(vd_asfloat(unity_OrthoParams.w) == 0.0)));
  r1.x = vd_asuint((VD_COL(unity_MatrixV, 0).z));
  r1.y = vd_asuint((VD_COL(unity_MatrixV, 1).z));
  r1.z = vd_asuint((VD_COL(unity_MatrixV, 2).z));
  r0.xyz = ((r0.www != 0u) ? r0.xyz : r1.xyz);
  r0.w = vd_asuint(dot(vd_asfloat(v6.xyz), vd_asfloat(v6.xyz)));
  r0.w = vd_asuint(sqrt(vd_asfloat(r0.w)));
  r0.w = vd_asuint((1.0 / vd_asfloat(r0.w)));
  r1.xyz = vd_asuint((vd_asfloat(r0.www) * vd_asfloat(v6.xyz)));
  r0.w = vd_asuint(dot(vd_asfloat(r1.xyz), vd_asfloat(r1.xyz)));
  r0.w = vd_asuint(rsqrt(vd_asfloat(r0.w)));
  r1.xyz = vd_asuint((vd_asfloat(r0.www) * vd_asfloat(r1.xyz)));
  r0.x = vd_asuint(saturate(dot(vd_asfloat(r1.xyz), vd_asfloat(r0.xyz))));
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
  r1.xyw = vd_asuint((vd_asfloat(r1.xxx) * vd_asfloat(v4.xyz)));
  r0.yzw = vd_asuint((vd_asfloat(r1.xyw) * vd_asfloat(r0.yyy) + (-(vd_asfloat(r1.xyw)))));
  r0.xyz = vd_asuint((vd_asfloat(r0.xxx) * vd_asfloat(r0.yzw) + vd_asfloat(r1.xyw)));
  r1.xyw = vd_asuint(((-(vd_asfloat(r0.xyz))) + vd_asfloat(v3.xyz)));
  r0.w = vd_asuint(((-(vd_asfloat(r1.z))) + 1.0));
  r0.w = vd_asuint(((-(vd_asfloat(r1.z))) + vd_asfloat(r0.w)));
  r2.x = vd_asuint(saturate(vd_asfloat(_invertB)));
  r0.w = vd_asuint((vd_asfloat(r2.x) * vd_asfloat(r0.w) + vd_asfloat(r1.z)));
  r0.w = vd_asuint((vd_asfloat(r0.w) * vd_asfloat(v3.w)));
  r0.xyz = vd_asuint((vd_asfloat(r0.www) * vd_asfloat(r1.xyw) + vd_asfloat(r0.xyz)));
  r1.xyz = vd_asuint((vd_asfloat(r0.xyz) * vd_asfloat(v2.zzz)));
  r0.xyz = vd_asuint(((-(vd_asfloat(r0.xyz))) * vd_asfloat(v2.zzz) + vd_asfloat(unity_FogColor.xyz)));
  r2.xyz = vd_asuint((vd_asfloat(v5.yyy) * vd_asfloat((VD_COL(unity_WorldToObject, 1).xyz))));
  r2.xyz = vd_asuint((vd_asfloat((VD_COL(unity_WorldToObject, 0).xyz)) * vd_asfloat(v5.xxx) + vd_asfloat(r2.xyz)));
  r2.xyz = vd_asuint((vd_asfloat((VD_COL(unity_WorldToObject, 2).xyz)) * vd_asfloat(v5.zzz) + vd_asfloat(r2.xyz)));
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
  r0.xyz = vd_asuint((vd_asfloat(r0.www) * vd_asfloat(r0.xyz) + vd_asfloat(r1.xyz)));
  r1.xy = vd_asuint(((int2)vd_asfloat(v0.xy)));
  r1.zw = uint2(0x00000000u, 0x00000000u);
  r1.xyzw = vd_asuint(_DBufferTexture0.Load(vd_asint(r1.xyw)).xyzw);
  o0.xyz = (vd_asfloat(r0.xyz) * vd_asfloat(r1.www) + vd_asfloat(r1.xyz));
  o0.w = vd_asfloat(0x3f800000u);
  return;
}
