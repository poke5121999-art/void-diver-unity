// Shader Graphs/shader_DE_BG_ReflectionSurface_Floor | pass 0 "DBufferProjector" | progFragment | keywords: _DBUFFER_MRT3 | blob 12
void frag(
  float4 v0 : SV_POSITION0,
  float4 v1 : VDREG1,
  out float4 o0 : SV_Target0,
  out float4 o1 : SV_Target1,
  out float4 o2 : SV_Target2)
{
#if UNITY_UV_STARTS_AT_TOP
#else
  float4x4 vd_MatrixInvVPD3D = mul(unity_MatrixInvVP, float4x4(1, 0, 0, 0, 0, 1, 0, 0, 0, 0, -2, 1, 0, 0, 0, 1));
#endif
  uint4 r0, r1;
  r0 = 0u; r1 = 0u;

  r0.xy = vd_asuint((vd_asfloat(v0.xy) * vd_asfloat(_ScreenSize.zw)));
  r0.xy = vd_asuint((vd_asfloat(r0.xy) * float2(2.0, 2.0) + float2(-1.0, -1.0)));
#if UNITY_UV_STARTS_AT_TOP
  r1.xyzw = vd_asuint(((-(vd_asfloat(r0.yyyy))) * vd_asfloat((VD_COL(unity_MatrixInvVP, 1).xyzw))));
  r0.xyzw = vd_asuint((vd_asfloat((VD_COL(unity_MatrixInvVP, 0).xyzw)) * vd_asfloat(r0.xxxx) + vd_asfloat(r1.xyzw)));
#else
  r1.xyzw = vd_asuint((vd_asfloat(r0.yyyy) * vd_asfloat((VD_COL(vd_MatrixInvVPD3D, 1).xyzw))));
  r0.xyzw = vd_asuint((vd_asfloat((VD_COL(vd_MatrixInvVPD3D, 0).xyzw)) * vd_asfloat(r0.xxxx) + vd_asfloat(r1.xyzw)));
#endif
  r1.xy = ((uint2)vd_asfloat(v0.xy));
  r1.zw = uint2(0x00000000u, 0x00000000u);
  r1.x = vd_asuint(_CameraDepthTexture.Load(vd_asint(r1.xyw)).x);
#if UNITY_UV_STARTS_AT_TOP
  r0.xyzw = vd_asuint((vd_asfloat((VD_COL(unity_MatrixInvVP, 2).xyzw)) * vd_asfloat(r1.xxxx) + vd_asfloat(r0.xyzw)));
  r0.xyzw = vd_asuint((vd_asfloat(r0.xyzw) + vd_asfloat((VD_COL(unity_MatrixInvVP, 3).xyzw))));
#else
  r1.x = vd_asuint(((-(vd_asfloat(r1.x))) + 1.0));
  r0.xyzw = vd_asuint((vd_asfloat((VD_COL(vd_MatrixInvVPD3D, 2).xyzw)) * vd_asfloat(r1.xxxx) + vd_asfloat(r0.xyzw)));
  r0.xyzw = vd_asuint((vd_asfloat(r0.xyzw) + vd_asfloat((VD_COL(vd_MatrixInvVPD3D, 3).xyzw))));
#endif
  r0.xyz = vd_asuint((vd_asfloat(r0.xyz) / vd_asfloat(r0.www)));
  r1.xyz = vd_asuint((vd_asfloat(r0.yyy) * vd_asfloat((VD_COL(unity_WorldToObject, 1).xyz))));
  r0.xyw = vd_asuint((vd_asfloat((VD_COL(unity_WorldToObject, 0).xyz)) * vd_asfloat(r0.xxx) + vd_asfloat(r1.xyz)));
  r0.xyz = vd_asuint((vd_asfloat((VD_COL(unity_WorldToObject, 2).xyz)) * vd_asfloat(r0.zzz) + vd_asfloat(r0.xyw)));
  r0.xyz = vd_asuint((vd_asfloat(r0.xyz) + vd_asfloat((VD_COL(unity_WorldToObject, 3).xyz))));
  r0.xyz = vd_asuint((vd_asfloat(r0.xyz) * float3(1.0, -1.0, 1.0)));
  r0.x = vd_asuint(max(abs(vd_asfloat(r0.y)), abs(vd_asfloat(r0.x))));
  r0.x = vd_asuint(max(abs(vd_asfloat(r0.z)), vd_asfloat(r0.x)));
  r0.x = vd_asuint(((-(vd_asfloat(r0.x))) + 0.5));
  r0.x = vd_asuint((-(int)(vd_asfloat(r0.x) < 0.0)));
  if (r0.x != 0u) discard;
#if UNITY_UV_STARTS_AT_TOP
  r0.x = vd_asuint((-(int)(vd_asfloat(_ProjectionParams.x) < 0.0)));
#else
  r0.x = vd_asuint((-(int)(0.0 < vd_asfloat(_ProjectionParams.x))));
#endif
  r0.y = vd_asuint(((-(vd_asfloat(v0.y))) + vd_asfloat(_ScreenParams.y)));
  r0.y = ((r0.x != 0u) ? r0.y : vd_asuint(v0.y));
  r0.x = vd_asuint(v0.x);
  r0.xy = vd_asuint((vd_asfloat(r0.xy) / vd_asfloat(_ScreenParams.xy)));
  r0.z = vd_asuint(((-(vd_asfloat(r0.y))) + 1.0));
  r0.xyzw = vd_asuint(_PlaneReflectionTexture.SampleBias(sampler_PlaneReflectionTexture, vd_asfloat(r0.xz), vd_asfloat(_GlobalMipBias.x)).xyzw);
  r1.x = vd_asuint(((-(vd_asfloat(_GlobalBackgroundColorIntensity))) + 1.0));
  r1.x = vd_asuint((vd_asfloat(r1.x) * vd_asfloat(_PlaneReflectionActive)));
  r0.w = vd_asuint((vd_asfloat(r0.w) * vd_asfloat(r1.x)));
  o0.xyz = vd_asfloat(r0.xyz);
  r0.x = vd_asuint((vd_asfloat(r0.w) * vd_asfloat(_Opacity)));
  r0.y = vd_asuint(saturate(vd_asfloat((VD_COL(_NormalToWorld, 3).x))));
  o0.w = (vd_asfloat(r0.y) * vd_asfloat(r0.x));
  o1.w = vd_asfloat(r0.y);
  r0.x = vd_asuint(dot(vd_asfloat((VD_COL(_NormalToWorld, 2).xyz)), vd_asfloat((VD_COL(_NormalToWorld, 2).xyz))));
  r0.x = vd_asuint(rsqrt(vd_asfloat(r0.x)));
  r0.xyz = vd_asuint((vd_asfloat(r0.xxx) * vd_asfloat((VD_COL(_NormalToWorld, 2).xyz))));
  o1.xyz = (vd_asfloat(r0.xyz) * float3(0.5, 0.5, 0.5) + float3(0.5, 0.5, 0.5));
  o2.xyzw = vd_asfloat(uint4(0x00000000u, 0x3f800000u, 0x00000000u, 0x00000000u));
  return;
}
