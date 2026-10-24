// LCArt/PostProcessing/CampaignEdgeOutline | pass 0 "CampaignEdgeOutline" | progFragment | keywords: (none) | blob 4
void frag(
  float4 v0 : SV_POSITION0,
  float4 v1 : VDREG1,
  out float4 o0 : SV_Target0)
{
#if UNITY_UV_STARTS_AT_TOP
#else
  float4 vd_ZBufferParamsD3D = float4(-_ZBufferParams.x, _ZBufferParams.x + _ZBufferParams.y, -_ZBufferParams.z, _ZBufferParams.z + _ZBufferParams.w);
  float4x4 vd_MatrixInvVPD3D = mul(unity_MatrixInvVP, float4x4(1, 0, 0, 0, 0, 1, 0, 0, 0, 0, -2, 1, 0, 0, 0, 1));
#endif
  uint4 r0, r1, r2, r3, r4, r5, r6, r7, r8, r9, r10, r11;
  r0 = 0u; r1 = 0u; r2 = 0u; r3 = 0u; r4 = 0u; r5 = 0u; r6 = 0u; r7 = 0u; r8 = 0u; r9 = 0u; r10 = 0u; r11 = 0u;

  r0.xy = vd_asuint((vd_asfloat(_BlitTexture_TexelSize.xy) * vd_asfloat((_OutlineThickness).xx)));
  r1.xy = vd_asuint(((-(vd_asfloat(_CameraDepthTexture_TexelSize.xy))) * float2(0.5, 0.5) + float2(1.0, 1.0)));
  r1.zw = vd_asuint(min(vd_asfloat(r1.xy), vd_asfloat(v1.xy)));
  r1.zw = vd_asuint((vd_asfloat(r1.zw) * vd_asfloat(_RTHandleScale.xy)));
  r2.xyzw = vd_asuint(_CameraDepthTexture.SampleBias(sampler_vd_point_clamp, vd_asfloat(r1.zw), vd_asfloat(_GlobalMipBias.x)).xyzw);
#if UNITY_UV_STARTS_AT_TOP
#else
  r2.x = vd_asuint(((-(vd_asfloat(r2.x))) + 1.0));
#endif
  r1.z = vd_asuint((-(int)(1.00000001e-07 < vd_asfloat(r2.x))));
#if UNITY_UV_STARTS_AT_TOP
  r1.w = vd_asuint((vd_asfloat(_ZBufferParams.z) * vd_asfloat(r2.x) + vd_asfloat(_ZBufferParams.w)));
#else
  r1.w = vd_asuint((vd_asfloat(vd_ZBufferParamsD3D.z) * vd_asfloat(r2.x) + vd_asfloat(vd_ZBufferParamsD3D.w)));
#endif
  r1.w = vd_asuint((1.0 / vd_asfloat(r1.w)));
  r1.z = ((r1.z != 0u) ? r1.w : vd_asuint(_ProjectionParams.z));
  r2.yz = vd_asuint((vd_asfloat(v1.xy) * float2(2.0, 2.0) + float2(-1.0, -1.0)));
#if UNITY_UV_STARTS_AT_TOP
  r3.xyzw = vd_asuint(((-(vd_asfloat(r2.zzzz))) * vd_asfloat((VD_COL(unity_MatrixInvVP, 1).xyzw))));
  r3.xyzw = vd_asuint((vd_asfloat((VD_COL(unity_MatrixInvVP, 0).xyzw)) * vd_asfloat(r2.yyyy) + vd_asfloat(r3.xyzw)));
  r3.xyzw = vd_asuint((vd_asfloat((VD_COL(unity_MatrixInvVP, 2).xyzw)) * vd_asfloat(r2.xxxx) + vd_asfloat(r3.xyzw)));
  r3.xyzw = vd_asuint((vd_asfloat(r3.xyzw) + vd_asfloat((VD_COL(unity_MatrixInvVP, 3).xyzw))));
#else
  r3.xyzw = vd_asuint((vd_asfloat(r2.zzzz) * vd_asfloat((VD_COL(vd_MatrixInvVPD3D, 1).xyzw))));
  r3.xyzw = vd_asuint((vd_asfloat((VD_COL(vd_MatrixInvVPD3D, 0).xyzw)) * vd_asfloat(r2.yyyy) + vd_asfloat(r3.xyzw)));
  r3.xyzw = vd_asuint((vd_asfloat((VD_COL(vd_MatrixInvVPD3D, 2).xyzw)) * vd_asfloat(r2.xxxx) + vd_asfloat(r3.xyzw)));
  r3.xyzw = vd_asuint((vd_asfloat(r3.xyzw) + vd_asfloat((VD_COL(vd_MatrixInvVPD3D, 3).xyzw))));
#endif
  r2.yzw = vd_asuint((vd_asfloat(r3.yzx) / vd_asfloat(r3.www)));
  r3.xw = vd_asuint(_BlitTexture_TexelSize.xy);
  r3.yz = uint2(0x00000000u, 0x00000000u);
  r4.xyzw = vd_asuint((vd_asfloat(r3.xyzw) + vd_asfloat(v1.xyxy)));
  r5.xyzw = vd_asuint(min(vd_asfloat(r1.xyxy), vd_asfloat(r4.xyzw)));
  r5.xyzw = vd_asuint((vd_asfloat(r5.xyzw) * vd_asfloat(_RTHandleScale.xyxy)));
  r6.xyzw = vd_asuint(_CameraDepthTexture.SampleBias(sampler_vd_point_clamp, vd_asfloat(r5.xy), vd_asfloat(_GlobalMipBias.x)).xyzw);
#if UNITY_UV_STARTS_AT_TOP
#else
  r6.x = vd_asuint(((-(vd_asfloat(r6.x))) + 1.0));
#endif
  r4.xyzw = vd_asuint((vd_asfloat(r4.xyzw) * float4(2.0, 2.0, 2.0, 2.0) + float4(-1.0, -1.0, -1.0, -1.0)));
#if UNITY_UV_STARTS_AT_TOP
  r7.xyzw = vd_asuint(((-(vd_asfloat(r4.yyyy))) * vd_asfloat((VD_COL(unity_MatrixInvVP, 1).xyzw))));
  r7.xyzw = vd_asuint((vd_asfloat((VD_COL(unity_MatrixInvVP, 0).xyzw)) * vd_asfloat(r4.xxxx) + vd_asfloat(r7.xyzw)));
  r6.xyzw = vd_asuint((vd_asfloat((VD_COL(unity_MatrixInvVP, 2).xyzw)) * vd_asfloat(r6.xxxx) + vd_asfloat(r7.xyzw)));
  r6.xyzw = vd_asuint((vd_asfloat(r6.xyzw) + vd_asfloat((VD_COL(unity_MatrixInvVP, 3).xyzw))));
#else
  r7.xyzw = vd_asuint((vd_asfloat(r4.yyyy) * vd_asfloat((VD_COL(vd_MatrixInvVPD3D, 1).xyzw))));
  r7.xyzw = vd_asuint((vd_asfloat((VD_COL(vd_MatrixInvVPD3D, 0).xyzw)) * vd_asfloat(r4.xxxx) + vd_asfloat(r7.xyzw)));
  r6.xyzw = vd_asuint((vd_asfloat((VD_COL(vd_MatrixInvVPD3D, 2).xyzw)) * vd_asfloat(r6.xxxx) + vd_asfloat(r7.xyzw)));
  r6.xyzw = vd_asuint((vd_asfloat(r6.xyzw) + vd_asfloat((VD_COL(vd_MatrixInvVPD3D, 3).xyzw))));
#endif
  r6.xyz = vd_asuint((vd_asfloat(r6.yzx) / vd_asfloat(r6.www)));
  r5.xyzw = vd_asuint(_CameraDepthTexture.SampleBias(sampler_vd_point_clamp, vd_asfloat(r5.zw), vd_asfloat(_GlobalMipBias.x)).xyzw);
#if UNITY_UV_STARTS_AT_TOP
  r7.xyzw = vd_asuint(((-(vd_asfloat(r4.wwww))) * vd_asfloat((VD_COL(unity_MatrixInvVP, 1).xyzw))));
  r4.xyzw = vd_asuint((vd_asfloat((VD_COL(unity_MatrixInvVP, 0).xyzw)) * vd_asfloat(r4.zzzz) + vd_asfloat(r7.xyzw)));
  r4.xyzw = vd_asuint((vd_asfloat((VD_COL(unity_MatrixInvVP, 2).xyzw)) * vd_asfloat(r5.xxxx) + vd_asfloat(r4.xyzw)));
  r4.xyzw = vd_asuint((vd_asfloat(r4.xyzw) + vd_asfloat((VD_COL(unity_MatrixInvVP, 3).xyzw))));
#else
  r5.x = vd_asuint(((-(vd_asfloat(r5.x))) + 1.0));
  r7.xyzw = vd_asuint((vd_asfloat(r4.wwww) * vd_asfloat((VD_COL(vd_MatrixInvVPD3D, 1).xyzw))));
  r4.xyzw = vd_asuint((vd_asfloat((VD_COL(vd_MatrixInvVPD3D, 0).xyzw)) * vd_asfloat(r4.zzzz) + vd_asfloat(r7.xyzw)));
  r4.xyzw = vd_asuint((vd_asfloat((VD_COL(vd_MatrixInvVPD3D, 2).xyzw)) * vd_asfloat(r5.xxxx) + vd_asfloat(r4.xyzw)));
  r4.xyzw = vd_asuint((vd_asfloat(r4.xyzw) + vd_asfloat((VD_COL(vd_MatrixInvVPD3D, 3).xyzw))));
#endif
  r4.xyz = vd_asuint((vd_asfloat(r4.zxy) / vd_asfloat(r4.www)));
  r4.xyz = vd_asuint(((-(vd_asfloat(r2.zwy))) + vd_asfloat(r4.xyz)));
  r2.yzw = vd_asuint(((-(vd_asfloat(r2.yzw))) + vd_asfloat(r6.xyz)));
  r5.xyz = vd_asuint((vd_asfloat(r2.yzw) * vd_asfloat(r4.xyz)));
  r2.yzw = vd_asuint((vd_asfloat(r4.zxy) * vd_asfloat(r2.zwy) + (-(vd_asfloat(r5.xyz)))));
  r1.w = vd_asuint(dot(vd_asfloat(r2.yzw), vd_asfloat(r2.yzw)));
  r1.w = vd_asuint(rsqrt(vd_asfloat(r1.w)));
  r2.yzw = vd_asuint((vd_asfloat(r1.www) * vd_asfloat(r2.yzw)));
  r0.z = vd_asuint((-(vd_asfloat(r0.y))));
  r4.xy = vd_asuint((vd_asfloat(r0.xz) + vd_asfloat(v1.xy)));
  r4.zw = vd_asuint(min(vd_asfloat(r1.xy), vd_asfloat(r4.xy)));
  r4.zw = vd_asuint((vd_asfloat(r4.zw) * vd_asfloat(_RTHandleScale.xy)));
  r5.xyzw = vd_asuint(_CameraDepthTexture.SampleBias(sampler_vd_point_clamp, vd_asfloat(r4.zw), vd_asfloat(_GlobalMipBias.x)).xyzw);
#if UNITY_UV_STARTS_AT_TOP
#else
  r5.x = vd_asuint(((-(vd_asfloat(r5.x))) + 1.0));
#endif
  r0.z = vd_asuint((-(int)(1.00000001e-07 < vd_asfloat(r5.x))));
#if UNITY_UV_STARTS_AT_TOP
  r1.w = vd_asuint((vd_asfloat(_ZBufferParams.z) * vd_asfloat(r5.x) + vd_asfloat(_ZBufferParams.w)));
#else
  r1.w = vd_asuint((vd_asfloat(vd_ZBufferParamsD3D.z) * vd_asfloat(r5.x) + vd_asfloat(vd_ZBufferParamsD3D.w)));
#endif
  r1.w = vd_asuint((1.0 / vd_asfloat(r1.w)));
  r0.z = ((r0.z != 0u) ? r1.w : vd_asuint(_ProjectionParams.z));
  r0.w = 0x00000000u;
  r6.xy = vd_asuint((vd_asfloat(r0.xw) + vd_asfloat(v1.xy)));
  r4.zw = vd_asuint(min(vd_asfloat(r1.xy), vd_asfloat(r6.xy)));
  r4.zw = vd_asuint((vd_asfloat(r4.zw) * vd_asfloat(_RTHandleScale.xy)));
  r7.xyzw = vd_asuint(_CameraDepthTexture.SampleBias(sampler_vd_point_clamp, vd_asfloat(r4.zw), vd_asfloat(_GlobalMipBias.x)).xyzw);
#if UNITY_UV_STARTS_AT_TOP
#else
  r7.x = vd_asuint(((-(vd_asfloat(r7.x))) + 1.0));
#endif
  r0.x = vd_asuint((-(int)(1.00000001e-07 < vd_asfloat(r7.x))));
#if UNITY_UV_STARTS_AT_TOP
  r1.w = vd_asuint((vd_asfloat(_ZBufferParams.z) * vd_asfloat(r7.x) + vd_asfloat(_ZBufferParams.w)));
#else
  r1.w = vd_asuint((vd_asfloat(vd_ZBufferParamsD3D.z) * vd_asfloat(r7.x) + vd_asfloat(vd_ZBufferParamsD3D.w)));
#endif
  r1.w = vd_asuint((1.0 / vd_asfloat(r1.w)));
  r0.x = ((r0.x != 0u) ? r1.w : vd_asuint(_ProjectionParams.z));
  r8.xy = vd_asuint((vd_asfloat(r0.wy) * float2(1.0, -1.0) + vd_asfloat(v1.xy)));
  r0.yw = vd_asuint(min(vd_asfloat(r1.xy), vd_asfloat(r8.xy)));
  r0.yw = vd_asuint((vd_asfloat(r0.yw) * vd_asfloat(_RTHandleScale.xy)));
  r9.xyzw = vd_asuint(_CameraDepthTexture.SampleBias(sampler_vd_point_clamp, vd_asfloat(r0.yw), vd_asfloat(_GlobalMipBias.x)).xyzw);
#if UNITY_UV_STARTS_AT_TOP
#else
  r9.x = vd_asuint(((-(vd_asfloat(r9.x))) + 1.0));
#endif
  r0.y = vd_asuint((-(int)(1.00000001e-07 < vd_asfloat(r9.x))));
#if UNITY_UV_STARTS_AT_TOP
  r0.w = vd_asuint((vd_asfloat(_ZBufferParams.z) * vd_asfloat(r9.x) + vd_asfloat(_ZBufferParams.w)));
#else
  r0.w = vd_asuint((vd_asfloat(vd_ZBufferParamsD3D.z) * vd_asfloat(r9.x) + vd_asfloat(vd_ZBufferParamsD3D.w)));
#endif
  r0.w = vd_asuint((1.0 / vd_asfloat(r0.w)));
  r0.y = ((r0.y != 0u) ? r0.w : vd_asuint(_ProjectionParams.z));
  r0.z = vd_asuint(((-(vd_asfloat(r0.z))) + vd_asfloat(r1.z)));
  r0.x = vd_asuint(((-(vd_asfloat(r0.y))) + vd_asfloat(r0.x)));
  r0.x = vd_asuint((vd_asfloat(r0.x) * vd_asfloat(r0.x)));
  r0.x = vd_asuint((vd_asfloat(r0.z) * vd_asfloat(r0.z) + vd_asfloat(r0.x)));
  r0.yz = vd_asuint((vd_asfloat(r4.xy) * float2(2.0, 2.0) + float2(-1.0, -1.0)));
#if UNITY_UV_STARTS_AT_TOP
  r10.xyzw = vd_asuint(((-(vd_asfloat(r0.zzzz))) * vd_asfloat((VD_COL(unity_MatrixInvVP, 1).xyzw))));
  r10.xyzw = vd_asuint((vd_asfloat((VD_COL(unity_MatrixInvVP, 0).xyzw)) * vd_asfloat(r0.yyyy) + vd_asfloat(r10.xyzw)));
  r5.xyzw = vd_asuint((vd_asfloat((VD_COL(unity_MatrixInvVP, 2).xyzw)) * vd_asfloat(r5.xxxx) + vd_asfloat(r10.xyzw)));
  r5.xyzw = vd_asuint((vd_asfloat(r5.xyzw) + vd_asfloat((VD_COL(unity_MatrixInvVP, 3).xyzw))));
#else
  r10.xyzw = vd_asuint((vd_asfloat(r0.zzzz) * vd_asfloat((VD_COL(vd_MatrixInvVPD3D, 1).xyzw))));
  r10.xyzw = vd_asuint((vd_asfloat((VD_COL(vd_MatrixInvVPD3D, 0).xyzw)) * vd_asfloat(r0.yyyy) + vd_asfloat(r10.xyzw)));
  r5.xyzw = vd_asuint((vd_asfloat((VD_COL(vd_MatrixInvVPD3D, 2).xyzw)) * vd_asfloat(r5.xxxx) + vd_asfloat(r10.xyzw)));
  r5.xyzw = vd_asuint((vd_asfloat(r5.xyzw) + vd_asfloat((VD_COL(vd_MatrixInvVPD3D, 3).xyzw))));
#endif
  r0.yzw = vd_asuint((vd_asfloat(r5.yzx) / vd_asfloat(r5.www)));
  r4.xyzw = vd_asuint((vd_asfloat(r3.xyzw) + vd_asfloat(r4.xyxy)));
  r5.xyzw = vd_asuint(min(vd_asfloat(r1.xyxy), vd_asfloat(r4.xyzw)));
  r5.xyzw = vd_asuint((vd_asfloat(r5.xyzw) * vd_asfloat(_RTHandleScale.xyxy)));
  r10.xyzw = vd_asuint(_CameraDepthTexture.SampleBias(sampler_vd_point_clamp, vd_asfloat(r5.xy), vd_asfloat(_GlobalMipBias.x)).xyzw);
#if UNITY_UV_STARTS_AT_TOP
#else
  r10.x = vd_asuint(((-(vd_asfloat(r10.x))) + 1.0));
#endif
  r4.xyzw = vd_asuint((vd_asfloat(r4.xyzw) * float4(2.0, 2.0, 2.0, 2.0) + float4(-1.0, -1.0, -1.0, -1.0)));
#if UNITY_UV_STARTS_AT_TOP
  r11.xyzw = vd_asuint(((-(vd_asfloat(r4.yyyy))) * vd_asfloat((VD_COL(unity_MatrixInvVP, 1).xyzw))));
  r11.xyzw = vd_asuint((vd_asfloat((VD_COL(unity_MatrixInvVP, 0).xyzw)) * vd_asfloat(r4.xxxx) + vd_asfloat(r11.xyzw)));
  r10.xyzw = vd_asuint((vd_asfloat((VD_COL(unity_MatrixInvVP, 2).xyzw)) * vd_asfloat(r10.xxxx) + vd_asfloat(r11.xyzw)));
  r10.xyzw = vd_asuint((vd_asfloat(r10.xyzw) + vd_asfloat((VD_COL(unity_MatrixInvVP, 3).xyzw))));
#else
  r11.xyzw = vd_asuint((vd_asfloat(r4.yyyy) * vd_asfloat((VD_COL(vd_MatrixInvVPD3D, 1).xyzw))));
  r11.xyzw = vd_asuint((vd_asfloat((VD_COL(vd_MatrixInvVPD3D, 0).xyzw)) * vd_asfloat(r4.xxxx) + vd_asfloat(r11.xyzw)));
  r10.xyzw = vd_asuint((vd_asfloat((VD_COL(vd_MatrixInvVPD3D, 2).xyzw)) * vd_asfloat(r10.xxxx) + vd_asfloat(r11.xyzw)));
  r10.xyzw = vd_asuint((vd_asfloat(r10.xyzw) + vd_asfloat((VD_COL(vd_MatrixInvVPD3D, 3).xyzw))));
#endif
  r7.yzw = vd_asuint((vd_asfloat(r10.yzx) / vd_asfloat(r10.www)));
  r5.xyzw = vd_asuint(_CameraDepthTexture.SampleBias(sampler_vd_point_clamp, vd_asfloat(r5.zw), vd_asfloat(_GlobalMipBias.x)).xyzw);
#if UNITY_UV_STARTS_AT_TOP
  r10.xyzw = vd_asuint(((-(vd_asfloat(r4.wwww))) * vd_asfloat((VD_COL(unity_MatrixInvVP, 1).xyzw))));
  r4.xyzw = vd_asuint((vd_asfloat((VD_COL(unity_MatrixInvVP, 0).xyzw)) * vd_asfloat(r4.zzzz) + vd_asfloat(r10.xyzw)));
  r4.xyzw = vd_asuint((vd_asfloat((VD_COL(unity_MatrixInvVP, 2).xyzw)) * vd_asfloat(r5.xxxx) + vd_asfloat(r4.xyzw)));
  r4.xyzw = vd_asuint((vd_asfloat(r4.xyzw) + vd_asfloat((VD_COL(unity_MatrixInvVP, 3).xyzw))));
#else
  r5.x = vd_asuint(((-(vd_asfloat(r5.x))) + 1.0));
  r10.xyzw = vd_asuint((vd_asfloat(r4.wwww) * vd_asfloat((VD_COL(vd_MatrixInvVPD3D, 1).xyzw))));
  r4.xyzw = vd_asuint((vd_asfloat((VD_COL(vd_MatrixInvVPD3D, 0).xyzw)) * vd_asfloat(r4.zzzz) + vd_asfloat(r10.xyzw)));
  r4.xyzw = vd_asuint((vd_asfloat((VD_COL(vd_MatrixInvVPD3D, 2).xyzw)) * vd_asfloat(r5.xxxx) + vd_asfloat(r4.xyzw)));
  r4.xyzw = vd_asuint((vd_asfloat(r4.xyzw) + vd_asfloat((VD_COL(vd_MatrixInvVPD3D, 3).xyzw))));
#endif
  r4.xyz = vd_asuint((vd_asfloat(r4.zxy) / vd_asfloat(r4.www)));
  r4.xyz = vd_asuint(((-(vd_asfloat(r0.zwy))) + vd_asfloat(r4.xyz)));
  r0.yzw = vd_asuint(((-(vd_asfloat(r0.yzw))) + vd_asfloat(r7.yzw)));
  r5.xyz = vd_asuint((vd_asfloat(r0.yzw) * vd_asfloat(r4.xyz)));
  r0.yzw = vd_asuint((vd_asfloat(r4.zxy) * vd_asfloat(r0.zwy) + (-(vd_asfloat(r5.xyz)))));
  r1.w = vd_asuint(dot(vd_asfloat(r0.yzw), vd_asfloat(r0.yzw)));
  r1.w = vd_asuint(rsqrt(vd_asfloat(r1.w)));
  r4.xy = vd_asuint((vd_asfloat(r6.xy) * float2(2.0, 2.0) + float2(-1.0, -1.0)));
#if UNITY_UV_STARTS_AT_TOP
  r5.xyzw = vd_asuint(((-(vd_asfloat(r4.yyyy))) * vd_asfloat((VD_COL(unity_MatrixInvVP, 1).xyzw))));
  r4.xyzw = vd_asuint((vd_asfloat((VD_COL(unity_MatrixInvVP, 0).xyzw)) * vd_asfloat(r4.xxxx) + vd_asfloat(r5.xyzw)));
  r4.xyzw = vd_asuint((vd_asfloat((VD_COL(unity_MatrixInvVP, 2).xyzw)) * vd_asfloat(r7.xxxx) + vd_asfloat(r4.xyzw)));
  r4.xyzw = vd_asuint((vd_asfloat(r4.xyzw) + vd_asfloat((VD_COL(unity_MatrixInvVP, 3).xyzw))));
#else
  r5.xyzw = vd_asuint((vd_asfloat(r4.yyyy) * vd_asfloat((VD_COL(vd_MatrixInvVPD3D, 1).xyzw))));
  r4.xyzw = vd_asuint((vd_asfloat((VD_COL(vd_MatrixInvVPD3D, 0).xyzw)) * vd_asfloat(r4.xxxx) + vd_asfloat(r5.xyzw)));
  r4.xyzw = vd_asuint((vd_asfloat((VD_COL(vd_MatrixInvVPD3D, 2).xyzw)) * vd_asfloat(r7.xxxx) + vd_asfloat(r4.xyzw)));
  r4.xyzw = vd_asuint((vd_asfloat(r4.xyzw) + vd_asfloat((VD_COL(vd_MatrixInvVPD3D, 3).xyzw))));
#endif
  r4.xyz = vd_asuint((vd_asfloat(r4.yzx) / vd_asfloat(r4.www)));
  r6.z = vd_asuint(v1.y);
  r5.xyzw = vd_asuint((vd_asfloat(r3.xyzw) + vd_asfloat(r6.xzxy)));
  r6.xyzw = vd_asuint(min(vd_asfloat(r1.xyxy), vd_asfloat(r5.xyzw)));
  r6.xyzw = vd_asuint((vd_asfloat(r6.xyzw) * vd_asfloat(_RTHandleScale.xyxy)));
  r7.xyzw = vd_asuint(_CameraDepthTexture.SampleBias(sampler_vd_point_clamp, vd_asfloat(r6.xy), vd_asfloat(_GlobalMipBias.x)).xyzw);
#if UNITY_UV_STARTS_AT_TOP
#else
  r7.x = vd_asuint(((-(vd_asfloat(r7.x))) + 1.0));
#endif
  r5.xyzw = vd_asuint((vd_asfloat(r5.xyzw) * float4(2.0, 2.0, 2.0, 2.0) + float4(-1.0, -1.0, -1.0, -1.0)));
#if UNITY_UV_STARTS_AT_TOP
  r10.xyzw = vd_asuint(((-(vd_asfloat(r5.yyyy))) * vd_asfloat((VD_COL(unity_MatrixInvVP, 1).xyzw))));
  r10.xyzw = vd_asuint((vd_asfloat((VD_COL(unity_MatrixInvVP, 0).xyzw)) * vd_asfloat(r5.xxxx) + vd_asfloat(r10.xyzw)));
  r7.xyzw = vd_asuint((vd_asfloat((VD_COL(unity_MatrixInvVP, 2).xyzw)) * vd_asfloat(r7.xxxx) + vd_asfloat(r10.xyzw)));
  r7.xyzw = vd_asuint((vd_asfloat(r7.xyzw) + vd_asfloat((VD_COL(unity_MatrixInvVP, 3).xyzw))));
#else
  r10.xyzw = vd_asuint((vd_asfloat(r5.yyyy) * vd_asfloat((VD_COL(vd_MatrixInvVPD3D, 1).xyzw))));
  r10.xyzw = vd_asuint((vd_asfloat((VD_COL(vd_MatrixInvVPD3D, 0).xyzw)) * vd_asfloat(r5.xxxx) + vd_asfloat(r10.xyzw)));
  r7.xyzw = vd_asuint((vd_asfloat((VD_COL(vd_MatrixInvVPD3D, 2).xyzw)) * vd_asfloat(r7.xxxx) + vd_asfloat(r10.xyzw)));
  r7.xyzw = vd_asuint((vd_asfloat(r7.xyzw) + vd_asfloat((VD_COL(vd_MatrixInvVPD3D, 3).xyzw))));
#endif
  r7.xyz = vd_asuint((vd_asfloat(r7.yzx) / vd_asfloat(r7.www)));
  r6.xyzw = vd_asuint(_CameraDepthTexture.SampleBias(sampler_vd_point_clamp, vd_asfloat(r6.zw), vd_asfloat(_GlobalMipBias.x)).xyzw);
#if UNITY_UV_STARTS_AT_TOP
  r10.xyzw = vd_asuint(((-(vd_asfloat(r5.wwww))) * vd_asfloat((VD_COL(unity_MatrixInvVP, 1).xyzw))));
  r5.xyzw = vd_asuint((vd_asfloat((VD_COL(unity_MatrixInvVP, 0).xyzw)) * vd_asfloat(r5.zzzz) + vd_asfloat(r10.xyzw)));
  r5.xyzw = vd_asuint((vd_asfloat((VD_COL(unity_MatrixInvVP, 2).xyzw)) * vd_asfloat(r6.xxxx) + vd_asfloat(r5.xyzw)));
  r5.xyzw = vd_asuint((vd_asfloat(r5.xyzw) + vd_asfloat((VD_COL(unity_MatrixInvVP, 3).xyzw))));
#else
  r6.x = vd_asuint(((-(vd_asfloat(r6.x))) + 1.0));
  r10.xyzw = vd_asuint((vd_asfloat(r5.wwww) * vd_asfloat((VD_COL(vd_MatrixInvVPD3D, 1).xyzw))));
  r5.xyzw = vd_asuint((vd_asfloat((VD_COL(vd_MatrixInvVPD3D, 0).xyzw)) * vd_asfloat(r5.zzzz) + vd_asfloat(r10.xyzw)));
  r5.xyzw = vd_asuint((vd_asfloat((VD_COL(vd_MatrixInvVPD3D, 2).xyzw)) * vd_asfloat(r6.xxxx) + vd_asfloat(r5.xyzw)));
  r5.xyzw = vd_asuint((vd_asfloat(r5.xyzw) + vd_asfloat((VD_COL(vd_MatrixInvVPD3D, 3).xyzw))));
#endif
  r5.xyz = vd_asuint((vd_asfloat(r5.zxy) / vd_asfloat(r5.www)));
  r5.xyz = vd_asuint(((-(vd_asfloat(r4.yzx))) + vd_asfloat(r5.xyz)));
  r4.xyz = vd_asuint(((-(vd_asfloat(r4.xyz))) + vd_asfloat(r7.xyz)));
  r6.xyz = vd_asuint((vd_asfloat(r4.xyz) * vd_asfloat(r5.xyz)));
  r4.xyz = vd_asuint((vd_asfloat(r5.zxy) * vd_asfloat(r4.yzx) + (-(vd_asfloat(r6.xyz)))));
  r4.w = vd_asuint(dot(vd_asfloat(r4.xyz), vd_asfloat(r4.xyz)));
  r4.w = vd_asuint(rsqrt(vd_asfloat(r4.w)));
  r5.xy = vd_asuint((vd_asfloat(r8.xy) * float2(2.0, 2.0) + float2(-1.0, -1.0)));
#if UNITY_UV_STARTS_AT_TOP
  r6.xyzw = vd_asuint(((-(vd_asfloat(r5.yyyy))) * vd_asfloat((VD_COL(unity_MatrixInvVP, 1).xyzw))));
  r5.xyzw = vd_asuint((vd_asfloat((VD_COL(unity_MatrixInvVP, 0).xyzw)) * vd_asfloat(r5.xxxx) + vd_asfloat(r6.xyzw)));
  r5.xyzw = vd_asuint((vd_asfloat((VD_COL(unity_MatrixInvVP, 2).xyzw)) * vd_asfloat(r9.xxxx) + vd_asfloat(r5.xyzw)));
  r5.xyzw = vd_asuint((vd_asfloat(r5.xyzw) + vd_asfloat((VD_COL(unity_MatrixInvVP, 3).xyzw))));
#else
  r6.xyzw = vd_asuint((vd_asfloat(r5.yyyy) * vd_asfloat((VD_COL(vd_MatrixInvVPD3D, 1).xyzw))));
  r5.xyzw = vd_asuint((vd_asfloat((VD_COL(vd_MatrixInvVPD3D, 0).xyzw)) * vd_asfloat(r5.xxxx) + vd_asfloat(r6.xyzw)));
  r5.xyzw = vd_asuint((vd_asfloat((VD_COL(vd_MatrixInvVPD3D, 2).xyzw)) * vd_asfloat(r9.xxxx) + vd_asfloat(r5.xyzw)));
  r5.xyzw = vd_asuint((vd_asfloat(r5.xyzw) + vd_asfloat((VD_COL(vd_MatrixInvVPD3D, 3).xyzw))));
#endif
  r5.xyz = vd_asuint((vd_asfloat(r5.yzx) / vd_asfloat(r5.www)));
  r3.xy = vd_asuint((vd_asfloat(r3.xy) + vd_asfloat(r8.xy)));
  r6.xy = vd_asuint(min(vd_asfloat(r1.xy), vd_asfloat(r3.xy)));
  r6.xy = vd_asuint((vd_asfloat(r6.xy) * vd_asfloat(_RTHandleScale.xy)));
  r6.xyzw = vd_asuint(_CameraDepthTexture.SampleBias(sampler_vd_point_clamp, vd_asfloat(r6.xy), vd_asfloat(_GlobalMipBias.x)).xyzw);
#if UNITY_UV_STARTS_AT_TOP
#else
  r6.x = vd_asuint(((-(vd_asfloat(r6.x))) + 1.0));
#endif
  r3.xy = vd_asuint((vd_asfloat(r3.xy) * float2(2.0, 2.0) + float2(-1.0, -1.0)));
#if UNITY_UV_STARTS_AT_TOP
  r7.xyzw = vd_asuint(((-(vd_asfloat(r3.yyyy))) * vd_asfloat((VD_COL(unity_MatrixInvVP, 1).xyzw))));
  r7.xyzw = vd_asuint((vd_asfloat((VD_COL(unity_MatrixInvVP, 0).xyzw)) * vd_asfloat(r3.xxxx) + vd_asfloat(r7.xyzw)));
  r6.xyzw = vd_asuint((vd_asfloat((VD_COL(unity_MatrixInvVP, 2).xyzw)) * vd_asfloat(r6.xxxx) + vd_asfloat(r7.xyzw)));
  r6.xyzw = vd_asuint((vd_asfloat(r6.xyzw) + vd_asfloat((VD_COL(unity_MatrixInvVP, 3).xyzw))));
#else
  r7.xyzw = vd_asuint((vd_asfloat(r3.yyyy) * vd_asfloat((VD_COL(vd_MatrixInvVPD3D, 1).xyzw))));
  r7.xyzw = vd_asuint((vd_asfloat((VD_COL(vd_MatrixInvVPD3D, 0).xyzw)) * vd_asfloat(r3.xxxx) + vd_asfloat(r7.xyzw)));
  r6.xyzw = vd_asuint((vd_asfloat((VD_COL(vd_MatrixInvVPD3D, 2).xyzw)) * vd_asfloat(r6.xxxx) + vd_asfloat(r7.xyzw)));
  r6.xyzw = vd_asuint((vd_asfloat(r6.xyzw) + vd_asfloat((VD_COL(vd_MatrixInvVPD3D, 3).xyzw))));
#endif
  r6.xyz = vd_asuint((vd_asfloat(r6.yzx) / vd_asfloat(r6.www)));
  r8.z = vd_asuint(v1.x);
  r3.xy = vd_asuint((vd_asfloat(r3.zw) + vd_asfloat(r8.zy)));
  r1.xy = vd_asuint(min(vd_asfloat(r1.xy), vd_asfloat(r3.xy)));
  r1.xy = vd_asuint((vd_asfloat(r1.xy) * vd_asfloat(_RTHandleScale.xy)));
  r7.xyzw = vd_asuint(_CameraDepthTexture.SampleBias(sampler_vd_point_clamp, vd_asfloat(r1.xy), vd_asfloat(_GlobalMipBias.x)).xyzw);
#if UNITY_UV_STARTS_AT_TOP
#else
  r7.x = vd_asuint(((-(vd_asfloat(r7.x))) + 1.0));
#endif
  r1.xy = vd_asuint((vd_asfloat(r3.xy) * float2(2.0, 2.0) + float2(-1.0, -1.0)));
#if UNITY_UV_STARTS_AT_TOP
  r3.xyzw = vd_asuint(((-(vd_asfloat(r1.yyyy))) * vd_asfloat((VD_COL(unity_MatrixInvVP, 1).xyzw))));
  r3.xyzw = vd_asuint((vd_asfloat((VD_COL(unity_MatrixInvVP, 0).xyzw)) * vd_asfloat(r1.xxxx) + vd_asfloat(r3.xyzw)));
  r3.xyzw = vd_asuint((vd_asfloat((VD_COL(unity_MatrixInvVP, 2).xyzw)) * vd_asfloat(r7.xxxx) + vd_asfloat(r3.xyzw)));
  r3.xyzw = vd_asuint((vd_asfloat(r3.xyzw) + vd_asfloat((VD_COL(unity_MatrixInvVP, 3).xyzw))));
#else
  r3.xyzw = vd_asuint((vd_asfloat(r1.yyyy) * vd_asfloat((VD_COL(vd_MatrixInvVPD3D, 1).xyzw))));
  r3.xyzw = vd_asuint((vd_asfloat((VD_COL(vd_MatrixInvVPD3D, 0).xyzw)) * vd_asfloat(r1.xxxx) + vd_asfloat(r3.xyzw)));
  r3.xyzw = vd_asuint((vd_asfloat((VD_COL(vd_MatrixInvVPD3D, 2).xyzw)) * vd_asfloat(r7.xxxx) + vd_asfloat(r3.xyzw)));
  r3.xyzw = vd_asuint((vd_asfloat(r3.xyzw) + vd_asfloat((VD_COL(vd_MatrixInvVPD3D, 3).xyzw))));
#endif
  r3.xyz = vd_asuint((vd_asfloat(r3.zxy) / vd_asfloat(r3.www)));
  r3.xyz = vd_asuint(((-(vd_asfloat(r5.yzx))) + vd_asfloat(r3.xyz)));
  r5.xyz = vd_asuint(((-(vd_asfloat(r5.xyz))) + vd_asfloat(r6.xyz)));
  r6.xyz = vd_asuint((vd_asfloat(r3.xyz) * vd_asfloat(r5.xyz)));
  r3.xyz = vd_asuint((vd_asfloat(r3.zxy) * vd_asfloat(r5.yzx) + (-(vd_asfloat(r6.xyz)))));
  r1.x = vd_asuint(dot(vd_asfloat(r3.xyz), vd_asfloat(r3.xyz)));
  r1.x = vd_asuint(rsqrt(vd_asfloat(r1.x)));
  r3.xyz = vd_asuint((vd_asfloat(r1.xxx) * vd_asfloat(r3.xyz)));
  r0.yzw = vd_asuint(((-(vd_asfloat(r0.yzw))) * vd_asfloat(r1.www) + vd_asfloat(r2.yzw)));
  r1.xyw = vd_asuint((vd_asfloat(r4.xyz) * vd_asfloat(r4.www) + (-(vd_asfloat(r3.xyz)))));
  r0.y = vd_asuint(dot(vd_asfloat(r0.yzw), vd_asfloat(r0.yzw)));
  r0.z = vd_asuint(dot(vd_asfloat(r1.xyw), vd_asfloat(r1.xyw)));
  r0.y = vd_asuint((vd_asfloat(r0.z) + vd_asfloat(r0.y)));
  r0.xy = vd_asuint(sqrt(vd_asfloat(r0.xy)));
  r0.z = vd_asuint(max(vd_asfloat(r1.z), 0.00100000005));
  r0.z = vd_asuint((1.0 / vd_asfloat(r0.z)));
  r0.x = vd_asuint((vd_asfloat(r0.z) * vd_asfloat(r0.x)));
  r0.z = vd_asuint((vd_asfloat(r0.x) * vd_asfloat(_DepthSensitivity)));
  r0.w = vd_asuint((vd_asfloat(r0.y) * vd_asfloat(_NormalSensitivity)));
  r0.x = vd_asuint((vd_asfloat(r0.x) * vd_asfloat(_DepthSensitivity) + (-(vd_asfloat(_DepthThreshold)))));
  r1.xy = vd_asuint((float2(1.0, 1.0) / vd_asfloat(float2(_DepthEdgeSoftness, _NormalEdgeSoftness))));
  r0.x = vd_asuint(saturate((vd_asfloat(r0.x) * vd_asfloat(r1.x))));
  r1.x = vd_asuint((vd_asfloat(r0.x) * -2.0 + 3.0));
  r0.x = vd_asuint((vd_asfloat(r0.x) * vd_asfloat(r0.x)));
  r0.x = vd_asuint((vd_asfloat(r0.x) * vd_asfloat(r1.x)));
  r0.y = vd_asuint((vd_asfloat(r0.y) * vd_asfloat(_NormalSensitivity) + (-(vd_asfloat(_NormalThreshold)))));
  r0.y = vd_asuint(saturate((vd_asfloat(r1.y) * vd_asfloat(r0.y))));
  r1.x = vd_asuint((vd_asfloat(r0.y) * -2.0 + 3.0));
  r0.y = vd_asuint((vd_asfloat(r0.y) * vd_asfloat(r0.y)));
  r0.y = vd_asuint((vd_asfloat(r0.y) * vd_asfloat(r1.x)));
  r0.x = vd_asuint(max(vd_asfloat(r0.y), vd_asfloat(r0.x)));
  r0.y = vd_asuint((-(int)(0.5 < vd_asfloat(_DebugMode))));
  if (r0.y != 0u) {
    r0.y = vd_asuint((-(int)(vd_asfloat(_DebugMode) < 1.5)));
    if (r0.y != 0u) {
      o0.xyz = min(vd_asfloat(r0.xxx), float3(1.0, 1.0, 1.0));
      o0.w = vd_asfloat(0x3f800000u);
      return;
    }
    r0.y = vd_asuint((-(int)(vd_asfloat(_DebugMode) < 2.5)));
    if (r0.y != 0u) {
      r0.y = vd_asuint((vd_asfloat(r0.z) * vd_asfloat(_DebugGain)));
      o0.xyz = saturate(vd_asfloat(r0.yyy));
      o0.w = vd_asfloat(0x3f800000u);
      return;
    }
    r0.y = vd_asuint((-(int)(vd_asfloat(_DebugMode) < 3.5)));
    if (r0.y != 0u) {
      r0.y = vd_asuint((vd_asfloat(r0.w) * vd_asfloat(_DebugGain)));
      o0.xyz = saturate(vd_asfloat(r0.yyy));
      o0.w = vd_asfloat(0x3f800000u);
      return;
    }
    r0.y = vd_asuint((-(int)(vd_asfloat(_DebugMode) < 4.5)));
    if (r0.y != 0u) {
#if UNITY_UV_STARTS_AT_TOP
      r0.y = vd_asuint((vd_asfloat(_ZBufferParams.x) * vd_asfloat(r2.x) + vd_asfloat(_ZBufferParams.y)));
#else
      r0.y = vd_asuint((vd_asfloat(vd_ZBufferParamsD3D.x) * vd_asfloat(r2.x) + vd_asfloat(vd_ZBufferParamsD3D.y)));
#endif
      o0.xyz = (float3(1.0, 1.0, 1.0) / vd_asfloat(r0.yyy));
      o0.w = vd_asfloat(0x3f800000u);
      return;
    }
    r0.y = vd_asuint((-(int)(vd_asfloat(_DebugMode) < 5.5)));
    if (r0.y != 0u) {
      o0.xyz = (vd_asfloat(r2.yzw) * float3(0.5, 0.5, 0.5) + float3(0.5, 0.5, 0.5));
      o0.w = vd_asfloat(0x3f800000u);
      return;
    }
    o0.xyzw = vd_asfloat(uint4(0x00000000u, 0x00000000u, 0x00000000u, 0x3f800000u));
    return;
  }
  r1.xyzw = vd_asuint(_BlitTexture.SampleBias(sampler_vd_linear_clamp, vd_asfloat(v1.xy), vd_asfloat(_GlobalMipBias.x)).xyzw);
  r0.x = vd_asuint(min(vd_asfloat(r0.x), 1.0));
  r0.x = vd_asuint((vd_asfloat(r0.x) * vd_asfloat(_OutlineOpacity)));
  r0.yzw = vd_asuint(((-(vd_asfloat(r1.xyz))) + vd_asfloat(_OutlineColor.xyz)));
  o0.xyz = (vd_asfloat(r0.xxx) * vd_asfloat(r0.yzw) + vd_asfloat(r1.xyz));
  o0.w = vd_asfloat(r1.w);
  return;
}
