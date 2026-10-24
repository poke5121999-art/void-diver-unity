// ArtTeam/ETC/shader_Transparent_VertexAlpha_FresnelAlpha_DepthFade_Unlit | pass 0 "Universal Forward" | progFragment | keywords: _DBUFFER_MRT3 _SURFACE_TYPE_TRANSPARENT | blob 14
void frag(
  float4 v0 : SV_POSITION0,
  float4 v1 : VDREG1,
  float4 v2 : VDREG2,
  float4 v3 : VDREG3,
  float4 v4 : VDREG4,
  out float4 o0 : SV_Target0)
{
#if UNITY_UV_STARTS_AT_TOP
#else
  float4 vd_ZBufferParamsD3D = float4(-_ZBufferParams.x, _ZBufferParams.x + _ZBufferParams.y, -_ZBufferParams.z, _ZBufferParams.z + _ZBufferParams.w);
  float4x4 vd_MatrixInvVPD3D = mul(unity_MatrixInvVP, float4x4(1, 0, 0, 0, 0, 1, 0, 0, 0, 0, -2, 1, 0, 0, 0, 1));
#endif
  uint4 r0, r1, r2, r3, r4, r5;
  r0 = 0u; r1 = 0u; r2 = 0u; r3 = 0u; r4 = 0u; r5 = 0u;

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
  r0.w = vd_asuint((vd_asfloat(v3.y) * vd_asfloat((VD_COL(unity_MatrixVP, 1).w))));
  r0.w = vd_asuint((vd_asfloat((VD_COL(unity_MatrixVP, 0).w)) * vd_asfloat(v3.x) + vd_asfloat(r0.w)));
  r0.w = vd_asuint((vd_asfloat((VD_COL(unity_MatrixVP, 2).w)) * vd_asfloat(v3.z) + vd_asfloat(r0.w)));
  r0.w = vd_asuint((vd_asfloat(r0.w) + vd_asfloat((VD_COL(unity_MatrixVP, 3).w))));
#if UNITY_UV_STARTS_AT_TOP
  r2.x = vd_asuint((-(int)(vd_asfloat(_ProjectionParams.x) < 0.0)));
#else
  r2.x = vd_asuint((-(int)(0.0 < vd_asfloat(_ProjectionParams.x))));
#endif
  r2.y = vd_asuint(((-(vd_asfloat(v0.y))) + vd_asfloat(_ScaledScreenParams.y)));
  r2.y = ((r2.x != 0u) ? r2.y : vd_asuint(v0.y));
  r2.x = vd_asuint(v0.x);
  r2.xy = vd_asuint((vd_asfloat(r2.xy) / vd_asfloat(_ScaledScreenParams.xy)));
  r3.xy = vd_asuint((vd_asfloat(v1.xy) * vd_asfloat(_MainTex_ST.xy) + vd_asfloat(_MainTex_ST.zw)));
  r3.xyzw = vd_asuint(_MainTex.SampleBias(sampler_MainTex, vd_asfloat(r3.xy), vd_asfloat(_GlobalMipBias.x)).xyzw);
  o0.xyz = (vd_asfloat(r3.xyz) * vd_asfloat(_TintColor.xyz));
  if (r1.y != 0u) {
    r3.xy = vd_asuint(((-(vd_asfloat(_CameraDepthTexture_TexelSize.xy))) * float2(0.5, 0.5) + float2(1.0, 1.0)));
    r2.z = vd_asuint(((-(vd_asfloat(r2.y))) + 1.0));
    r3.xy = vd_asuint(min(vd_asfloat(r2.xz), vd_asfloat(r3.xy)));
    r3.xy = vd_asuint((vd_asfloat(r3.xy) * vd_asfloat(_RTHandleScale.xy)));
    r4.xyzw = vd_asuint(_CameraDepthTexture.SampleBias(sampler_vd_point_clamp, vd_asfloat(r3.xy), vd_asfloat(_GlobalMipBias.x)).xyzw);
#if UNITY_UV_STARTS_AT_TOP
#else
    r4.x = vd_asuint(((-(vd_asfloat(r4.x))) + 1.0));
#endif
    r3.xy = vd_asuint((vd_asfloat(r2.xz) * float2(2.0, 2.0) + float2(-1.0, -1.0)));
#if UNITY_UV_STARTS_AT_TOP
    r5.xyzw = vd_asuint(((-(vd_asfloat(r3.yyyy))) * vd_asfloat((VD_COL(unity_MatrixInvVP, 1).xyzw))));
    r5.xyzw = vd_asuint((vd_asfloat((VD_COL(unity_MatrixInvVP, 0).xyzw)) * vd_asfloat(r3.xxxx) + vd_asfloat(r5.xyzw)));
    r4.xyzw = vd_asuint((vd_asfloat((VD_COL(unity_MatrixInvVP, 2).xyzw)) * vd_asfloat(r4.xxxx) + vd_asfloat(r5.xyzw)));
    r4.xyzw = vd_asuint((vd_asfloat(r4.xyzw) + vd_asfloat((VD_COL(unity_MatrixInvVP, 3).xyzw))));
#else
    r5.xyzw = vd_asuint((vd_asfloat(r3.yyyy) * vd_asfloat((VD_COL(vd_MatrixInvVPD3D, 1).xyzw))));
    r5.xyzw = vd_asuint((vd_asfloat((VD_COL(vd_MatrixInvVPD3D, 0).xyzw)) * vd_asfloat(r3.xxxx) + vd_asfloat(r5.xyzw)));
    r4.xyzw = vd_asuint((vd_asfloat((VD_COL(vd_MatrixInvVPD3D, 2).xyzw)) * vd_asfloat(r4.xxxx) + vd_asfloat(r5.xyzw)));
    r4.xyzw = vd_asuint((vd_asfloat(r4.xyzw) + vd_asfloat((VD_COL(vd_MatrixInvVPD3D, 3).xyzw))));
#endif
    r3.xyz = vd_asuint((vd_asfloat(r4.xyz) / vd_asfloat(r4.www)));
    r1.y = vd_asuint((vd_asfloat(r3.y) * vd_asfloat((VD_COL(unity_MatrixV, 1).z))));
    r1.y = vd_asuint((vd_asfloat((VD_COL(unity_MatrixV, 0).z)) * vd_asfloat(r3.x) + vd_asfloat(r1.y)));
    r1.y = vd_asuint((vd_asfloat((VD_COL(unity_MatrixV, 2).z)) * vd_asfloat(r3.z) + vd_asfloat(r1.y)));
    r1.y = vd_asuint((vd_asfloat(r1.y) + vd_asfloat((VD_COL(unity_MatrixV, 3).z))));
    r1.y = vd_asuint(abs(vd_asfloat(r1.y)));
  } else {
    r3.xy = vd_asuint(((-(vd_asfloat(_CameraDepthTexture_TexelSize.xy))) * float2(0.5, 0.5) + float2(1.0, 1.0)));
    r2.w = vd_asuint(((-(vd_asfloat(r2.y))) + 1.0));
    r2.xy = vd_asuint(min(vd_asfloat(r2.xw), vd_asfloat(r3.xy)));
    r2.xy = vd_asuint((vd_asfloat(r2.xy) * vd_asfloat(_RTHandleScale.xy)));
    r2.xyzw = vd_asuint(_CameraDepthTexture.SampleBias(sampler_vd_point_clamp, vd_asfloat(r2.xy), vd_asfloat(_GlobalMipBias.x)).xyzw);
#if UNITY_UV_STARTS_AT_TOP
    r2.x = vd_asuint((vd_asfloat(_ZBufferParams.z) * vd_asfloat(r2.x) + vd_asfloat(_ZBufferParams.w)));
#else
    r2.x = vd_asuint(((-(vd_asfloat(r2.x))) + 1.0));
    r2.x = vd_asuint((vd_asfloat(vd_ZBufferParamsD3D.z) * vd_asfloat(r2.x) + vd_asfloat(vd_ZBufferParamsD3D.w)));
#endif
    r1.y = vd_asuint((1.0 / vd_asfloat(r2.x)));
  }
  r0.w = vd_asuint(((-(vd_asfloat(r0.w))) + vd_asfloat(r1.y)));
  r0.w = vd_asuint(saturate((vd_asfloat(r0.w) / vd_asfloat(_DepthFadeDistance))));
  r0.w = vd_asuint((vd_asfloat(r0.w) * vd_asfloat(r3.w)));
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
