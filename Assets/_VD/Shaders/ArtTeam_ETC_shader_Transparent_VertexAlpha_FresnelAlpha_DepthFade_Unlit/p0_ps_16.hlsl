// ArtTeam/ETC/shader_Transparent_VertexAlpha_FresnelAlpha_DepthFade_Unlit | pass 0 "Universal Forward" | progFragment | keywords: FOG_LINEAR _DBUFFER_MRT3 _SURFACE_TYPE_TRANSPARENT | blob 16
void frag(
  float4 v0 : SV_POSITION0,
  float4 v1 : VDREG1,
  float4 v2 : VDREG2,
  float4 v3 : VDREG3,
  float4 v4 : VDREG4,
  out float4 o0 : SV_Target0)
{
  uint4 r0, r1, r2, r3, r4, r5, r6;
  r0 = 0u; r1 = 0u; r2 = 0u; r3 = 0u; r4 = 0u; r5 = 0u; r6 = 0u;

  r0.x = vd_asuint(dot(vd_asfloat(v4.xyz), vd_asfloat(v4.xyz)));
  r0.x = vd_asuint(sqrt(vd_asfloat(r0.x)));
  r0.x = vd_asuint((1.0 / vd_asfloat(r0.x)));
  r0.xyz = vd_asuint((vd_asfloat(r0.xxx) * vd_asfloat(v4.xyz)));
  r1.xy = vd_asuint((-(int2)(vd_asfloat(unity_OrthoParams.ww) == float2(0.0, 1.0))));
  r2.xyz = vd_asuint(((-(vd_asfloat(v3.xyz))) + vd_asfloat(_WorldSpaceCameraPos.xyz)));
  r0.w = vd_asuint(dot(vd_asfloat(r2.xyz), vd_asfloat(r2.xyz)));
  r0.w = vd_asuint(rsqrt(vd_asfloat(r0.w)));
  r2.xyz = vd_asuint((vd_asfloat(r0.www) * vd_asfloat(r2.xyz)));
  r3.x = vd_asuint((VD_COL(unity_MatrixV, 0).z));
  r3.y = vd_asuint((VD_COL(unity_MatrixV, 1).z));
  r3.z = vd_asuint((VD_COL(unity_MatrixV, 2).z));
  r1.xzw = ((r1.xxx != 0u) ? r2.xyz : r3.xyz);
  r2.xyz = vd_asuint((vd_asfloat(v3.yyy) * vd_asfloat((VD_COL(unity_WorldToObject, 1).xyz))));
  r2.xyz = vd_asuint((vd_asfloat((VD_COL(unity_WorldToObject, 0).xyz)) * vd_asfloat(v3.xxx) + vd_asfloat(r2.xyz)));
  r2.xyz = vd_asuint((vd_asfloat((VD_COL(unity_WorldToObject, 2).xyz)) * vd_asfloat(v3.zzz) + vd_asfloat(r2.xyz)));
  r2.xyz = vd_asuint((vd_asfloat(r2.xyz) + vd_asfloat((VD_COL(unity_WorldToObject, 3).xyz))));
  r0.w = vd_asuint((vd_asfloat(v3.y) * vd_asfloat((VD_COL(unity_MatrixVP, 1).w))));
  r0.w = vd_asuint((vd_asfloat((VD_COL(unity_MatrixVP, 0).w)) * vd_asfloat(v3.x) + vd_asfloat(r0.w)));
  r0.w = vd_asuint((vd_asfloat((VD_COL(unity_MatrixVP, 2).w)) * vd_asfloat(v3.z) + vd_asfloat(r0.w)));
  r0.w = vd_asuint((vd_asfloat(r0.w) + vd_asfloat((VD_COL(unity_MatrixVP, 3).w))));
#if UNITY_UV_STARTS_AT_TOP
  r2.w = vd_asuint((-(int)(vd_asfloat(_ProjectionParams.x) < 0.0)));
#else
  r2.w = vd_asuint((-(int)(0.0 < vd_asfloat(_ProjectionParams.x))));
#endif
  r3.x = vd_asuint(((-(vd_asfloat(v0.y))) + vd_asfloat(_ScaledScreenParams.y)));
  r3.y = ((r2.w != 0u) ? r3.x : vd_asuint(v0.y));
  r3.x = vd_asuint(v0.x);
  r3.xy = vd_asuint((vd_asfloat(r3.xy) / vd_asfloat(_ScaledScreenParams.xy)));
  r4.xy = vd_asuint((vd_asfloat(v1.xy) * vd_asfloat(_MainTex_ST.xy) + vd_asfloat(_MainTex_ST.zw)));
  r4.xyzw = vd_asuint(_MainTex.SampleBias(sampler_MainTex, vd_asfloat(r4.xy), vd_asfloat(_GlobalMipBias.x)).xyzw);
  r5.xyz = vd_asuint((vd_asfloat(r4.xyz) * vd_asfloat(_TintColor.xyz)));
  r6.xyz = vd_asuint((vd_asfloat(r2.yyy) * vd_asfloat((VD_COL(unity_ObjectToWorld, 1).xyz))));
  r2.xyw = vd_asuint((vd_asfloat((VD_COL(unity_ObjectToWorld, 0).xyz)) * vd_asfloat(r2.xxx) + vd_asfloat(r6.xyz)));
  r2.xyz = vd_asuint((vd_asfloat((VD_COL(unity_ObjectToWorld, 2).xyz)) * vd_asfloat(r2.zzz) + vd_asfloat(r2.xyw)));
  r2.xyz = vd_asuint((vd_asfloat(r2.xyz) + vd_asfloat((VD_COL(unity_ObjectToWorld, 3).xyz))));
  r2.y = vd_asuint((vd_asfloat(r2.y) * vd_asfloat((VD_COL(unity_MatrixV, 1).z))));
  r2.x = vd_asuint((vd_asfloat((VD_COL(unity_MatrixV, 0).z)) * vd_asfloat(r2.x) + vd_asfloat(r2.y)));
  r2.x = vd_asuint((vd_asfloat((VD_COL(unity_MatrixV, 2).z)) * vd_asfloat(r2.z) + vd_asfloat(r2.x)));
  r2.x = vd_asuint((vd_asfloat(r2.x) + vd_asfloat((VD_COL(unity_MatrixV, 3).z))));
  r2.x = vd_asuint(((-(vd_asfloat(r2.x))) + (-(vd_asfloat(_ProjectionParams.y)))));
  r2.x = vd_asuint(max(vd_asfloat(r2.x), 0.0));
  r2.x = vd_asuint(saturate((vd_asfloat(r2.x) * vd_asfloat(unity_FogParams.z) + vd_asfloat(unity_FogParams.w))));
  r2.x = vd_asuint(((-(vd_asfloat(r2.x))) + 1.0));
  r2.yzw = vd_asuint(((-(vd_asfloat(_TintColor.xyz))) * vd_asfloat(r4.xyz) + vd_asfloat(unity_FogColor.xyz)));
  o0.xyz = (vd_asfloat(r2.xxx) * vd_asfloat(r2.yzw) + vd_asfloat(r5.xyz));
  if (r1.y != 0u) {
    r2.xy = vd_asuint(((-(vd_asfloat(_CameraDepthTexture_TexelSize.xy))) * float2(0.5, 0.5) + float2(1.0, 1.0)));
    r3.z = vd_asuint(((-(vd_asfloat(r3.y))) + 1.0));
    r2.xy = vd_asuint(min(vd_asfloat(r2.xy), vd_asfloat(r3.xz)));
    r2.xy = vd_asuint((vd_asfloat(r2.xy) * vd_asfloat(_RTHandleScale.xy)));
    r2.xyzw = vd_asuint(_CameraDepthTexture.SampleBias(sampler_vd_point_clamp, vd_asfloat(r2.xy), vd_asfloat(_GlobalMipBias.x)).xyzw);
    r2.yz = vd_asuint((vd_asfloat(r3.xz) * float2(2.0, 2.0) + float2(-1.0, -1.0)));
    r5.xyzw = vd_asuint(((-(vd_asfloat(r2.zzzz))) * vd_asfloat((VD_COL(unity_MatrixInvVP, 1).xyzw))));
    r5.xyzw = vd_asuint((vd_asfloat((VD_COL(unity_MatrixInvVP, 0).xyzw)) * vd_asfloat(r2.yyyy) + vd_asfloat(r5.xyzw)));
    r2.xyzw = vd_asuint((vd_asfloat((VD_COL(unity_MatrixInvVP, 2).xyzw)) * vd_asfloat(r2.xxxx) + vd_asfloat(r5.xyzw)));
    r2.xyzw = vd_asuint((vd_asfloat(r2.xyzw) + vd_asfloat((VD_COL(unity_MatrixInvVP, 3).xyzw))));
    r2.xyz = vd_asuint((vd_asfloat(r2.xyz) / vd_asfloat(r2.www)));
    r1.y = vd_asuint((vd_asfloat(r2.y) * vd_asfloat((VD_COL(unity_MatrixV, 1).z))));
    r1.y = vd_asuint((vd_asfloat((VD_COL(unity_MatrixV, 0).z)) * vd_asfloat(r2.x) + vd_asfloat(r1.y)));
    r1.y = vd_asuint((vd_asfloat((VD_COL(unity_MatrixV, 2).z)) * vd_asfloat(r2.z) + vd_asfloat(r1.y)));
    r1.y = vd_asuint((vd_asfloat(r1.y) + vd_asfloat((VD_COL(unity_MatrixV, 3).z))));
    r1.y = vd_asuint(abs(vd_asfloat(r1.y)));
  } else {
    r2.xy = vd_asuint(((-(vd_asfloat(_CameraDepthTexture_TexelSize.xy))) * float2(0.5, 0.5) + float2(1.0, 1.0)));
    r3.w = vd_asuint(((-(vd_asfloat(r3.y))) + 1.0));
    r2.xy = vd_asuint(min(vd_asfloat(r2.xy), vd_asfloat(r3.xw)));
    r2.xy = vd_asuint((vd_asfloat(r2.xy) * vd_asfloat(_RTHandleScale.xy)));
    r2.xyzw = vd_asuint(_CameraDepthTexture.SampleBias(sampler_vd_point_clamp, vd_asfloat(r2.xy), vd_asfloat(_GlobalMipBias.x)).xyzw);
    r2.x = vd_asuint((vd_asfloat(_ZBufferParams.z) * vd_asfloat(r2.x) + vd_asfloat(_ZBufferParams.w)));
    r1.y = vd_asuint((1.0 / vd_asfloat(r2.x)));
  }
  r0.w = vd_asuint(((-(vd_asfloat(r0.w))) + vd_asfloat(r1.y)));
  r0.w = vd_asuint(saturate((vd_asfloat(r0.w) / vd_asfloat(_DepthFadeDistance))));
  r0.w = vd_asuint((vd_asfloat(r0.w) * vd_asfloat(r4.w)));
  r0.w = vd_asuint((vd_asfloat(r0.w) * vd_asfloat(_TintColor.w)));
  r1.y = vd_asuint(dot(vd_asfloat(r0.xyz), vd_asfloat(r0.xyz)));
  r1.y = vd_asuint(rsqrt(vd_asfloat(r1.y)));
  r0.xyz = vd_asuint((vd_asfloat(r0.xyz) * vd_asfloat(r1.yyy)));
  r0.x = vd_asuint(saturate(dot(vd_asfloat(r0.xyz), vd_asfloat(r1.xzw))));
  r0.x = vd_asuint(((-(vd_asfloat(r0.x))) + 1.0));
  r0.x = vd_asuint(log2(vd_asfloat(r0.x)));
  r0.x = vd_asuint((vd_asfloat(r0.x) * vd_asfloat(_FresnelPower)));
  r0.x = vd_asuint(exp2(vd_asfloat(r0.x)));
  r0.x = vd_asuint(((-(vd_asfloat(r0.x))) + 1.0));
  r0.x = vd_asuint((vd_asfloat(r0.w) * vd_asfloat(r0.x) + (-(vd_asfloat(r0.w)))));
  r0.x = vd_asuint((vd_asfloat(_FresnelStrength) * vd_asfloat(r0.x) + vd_asfloat(r0.w)));
  o0.w = (vd_asfloat(r0.x) * vd_asfloat(v2.w));
  return;
}
