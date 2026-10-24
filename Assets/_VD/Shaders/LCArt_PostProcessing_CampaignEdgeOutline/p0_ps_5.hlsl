// LCArt/PostProcessing/CampaignEdgeOutline | pass 0 "CampaignEdgeOutline" | progFragment | keywords: _DE_CAVITY _DE_EDGE_SOBEL | blob 5
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
  uint4 r0, r1, r2, r3, r4, r5, r6, r7, r8, r9, r10, r11, r12, r13, r14, r15, r16, r17, r18, r19, r20;
  r0 = 0u; r1 = 0u; r2 = 0u; r3 = 0u; r4 = 0u; r5 = 0u; r6 = 0u; r7 = 0u; r8 = 0u; r9 = 0u; r10 = 0u; r11 = 0u; r12 = 0u; r13 = 0u; r14 = 0u; r15 = 0u; r16 = 0u; r17 = 0u; r18 = 0u; r19 = 0u; r20 = 0u;

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
  r0.z = vd_asuint((-(vd_asfloat(r0.x))));
  r3.xyzw = vd_asuint((vd_asfloat(r0.zyxy) + vd_asfloat(v1.xyxy)));
  r4.xyzw = vd_asuint(min(vd_asfloat(r1.xyxy), vd_asfloat(r3.xyzw)));
  r4.xyzw = vd_asuint((vd_asfloat(r4.xyzw) * vd_asfloat(_RTHandleScale.xyxy)));
  r5.xyzw = vd_asuint(_CameraDepthTexture.SampleBias(sampler_vd_point_clamp, vd_asfloat(r4.xy), vd_asfloat(_GlobalMipBias.x)).xyzw);
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
  r6.xy = vd_asuint((vd_asfloat(r0.wy) + vd_asfloat(v1.xy)));
  r2.yz = vd_asuint(min(vd_asfloat(r1.xy), vd_asfloat(r6.xy)));
  r2.yz = vd_asuint((vd_asfloat(r2.yz) * vd_asfloat(_RTHandleScale.xy)));
  r7.xyzw = vd_asuint(_CameraDepthTexture.SampleBias(sampler_vd_point_clamp, vd_asfloat(r2.yz), vd_asfloat(_GlobalMipBias.x)).xyzw);
#if UNITY_UV_STARTS_AT_TOP
#else
  r7.x = vd_asuint(((-(vd_asfloat(r7.x))) + 1.0));
#endif
  r1.w = vd_asuint((-(int)(1.00000001e-07 < vd_asfloat(r7.x))));
#if UNITY_UV_STARTS_AT_TOP
  r2.y = vd_asuint((vd_asfloat(_ZBufferParams.z) * vd_asfloat(r7.x) + vd_asfloat(_ZBufferParams.w)));
#else
  r2.y = vd_asuint((vd_asfloat(vd_ZBufferParamsD3D.z) * vd_asfloat(r7.x) + vd_asfloat(vd_ZBufferParamsD3D.w)));
#endif
  r2.y = vd_asuint((1.0 / vd_asfloat(r2.y)));
  r1.w = ((r1.w != 0u) ? r2.y : vd_asuint(_ProjectionParams.z));
  r4.xyzw = vd_asuint(_CameraDepthTexture.SampleBias(sampler_vd_point_clamp, vd_asfloat(r4.zw), vd_asfloat(_GlobalMipBias.x)).xyzw);
#if UNITY_UV_STARTS_AT_TOP
#else
  r4.x = vd_asuint(((-(vd_asfloat(r4.x))) + 1.0));
#endif
  r2.y = vd_asuint((-(int)(1.00000001e-07 < vd_asfloat(r4.x))));
#if UNITY_UV_STARTS_AT_TOP
  r2.z = vd_asuint((vd_asfloat(_ZBufferParams.z) * vd_asfloat(r4.x) + vd_asfloat(_ZBufferParams.w)));
#else
  r2.z = vd_asuint((vd_asfloat(vd_ZBufferParamsD3D.z) * vd_asfloat(r4.x) + vd_asfloat(vd_ZBufferParamsD3D.w)));
#endif
  r2.z = vd_asuint((1.0 / vd_asfloat(r2.z)));
  r2.y = ((r2.y != 0u) ? r2.z : vd_asuint(_ProjectionParams.z));
  r8.xy = vd_asuint((vd_asfloat(r0.xw) * float2(-1.0, 1.0) + vd_asfloat(v1.xy)));
  r2.zw = vd_asuint(min(vd_asfloat(r1.xy), vd_asfloat(r8.xy)));
  r2.zw = vd_asuint((vd_asfloat(r2.zw) * vd_asfloat(_RTHandleScale.xy)));
  r9.xyzw = vd_asuint(_CameraDepthTexture.SampleBias(sampler_vd_point_clamp, vd_asfloat(r2.zw), vd_asfloat(_GlobalMipBias.x)).xyzw);
#if UNITY_UV_STARTS_AT_TOP
#else
  r9.x = vd_asuint(((-(vd_asfloat(r9.x))) + 1.0));
#endif
  r2.z = vd_asuint((-(int)(1.00000001e-07 < vd_asfloat(r9.x))));
#if UNITY_UV_STARTS_AT_TOP
  r2.w = vd_asuint((vd_asfloat(_ZBufferParams.z) * vd_asfloat(r9.x) + vd_asfloat(_ZBufferParams.w)));
#else
  r2.w = vd_asuint((vd_asfloat(vd_ZBufferParamsD3D.z) * vd_asfloat(r9.x) + vd_asfloat(vd_ZBufferParamsD3D.w)));
#endif
  r2.w = vd_asuint((1.0 / vd_asfloat(r2.w)));
  r2.z = ((r2.z != 0u) ? r2.w : vd_asuint(_ProjectionParams.z));
  r10.xy = vd_asuint((vd_asfloat(r0.xw) + vd_asfloat(v1.xy)));
  r4.yz = vd_asuint(min(vd_asfloat(r1.xy), vd_asfloat(r10.xy)));
  r4.yz = vd_asuint((vd_asfloat(r4.yz) * vd_asfloat(_RTHandleScale.xy)));
  r11.xyzw = vd_asuint(_CameraDepthTexture.SampleBias(sampler_vd_point_clamp, vd_asfloat(r4.yz), vd_asfloat(_GlobalMipBias.x)).xyzw);
#if UNITY_UV_STARTS_AT_TOP
#else
  r11.x = vd_asuint(((-(vd_asfloat(r11.x))) + 1.0));
#endif
  r2.w = vd_asuint((-(int)(1.00000001e-07 < vd_asfloat(r11.x))));
#if UNITY_UV_STARTS_AT_TOP
  r4.y = vd_asuint((vd_asfloat(_ZBufferParams.z) * vd_asfloat(r11.x) + vd_asfloat(_ZBufferParams.w)));
#else
  r4.y = vd_asuint((vd_asfloat(vd_ZBufferParamsD3D.z) * vd_asfloat(r11.x) + vd_asfloat(vd_ZBufferParamsD3D.w)));
#endif
  r4.y = vd_asuint((1.0 / vd_asfloat(r4.y)));
  r2.w = ((r2.w != 0u) ? r4.y : vd_asuint(_ProjectionParams.z));
  r4.yz = vd_asuint(((-(vd_asfloat(_BlitTexture_TexelSize.xy))) * vd_asfloat((_OutlineThickness).xx) + vd_asfloat(v1.xy)));
  r5.yz = vd_asuint(min(vd_asfloat(r1.xy), vd_asfloat(r4.yz)));
  r5.yz = vd_asuint((vd_asfloat(r5.yz) * vd_asfloat(_RTHandleScale.xy)));
  r12.xyzw = vd_asuint(_CameraDepthTexture.SampleBias(sampler_vd_point_clamp, vd_asfloat(r5.yz), vd_asfloat(_GlobalMipBias.x)).xyzw);
#if UNITY_UV_STARTS_AT_TOP
#else
  r12.x = vd_asuint(((-(vd_asfloat(r12.x))) + 1.0));
#endif
  r4.w = vd_asuint((-(int)(1.00000001e-07 < vd_asfloat(r12.x))));
#if UNITY_UV_STARTS_AT_TOP
  r5.y = vd_asuint((vd_asfloat(_ZBufferParams.z) * vd_asfloat(r12.x) + vd_asfloat(_ZBufferParams.w)));
#else
  r5.y = vd_asuint((vd_asfloat(vd_ZBufferParamsD3D.z) * vd_asfloat(r12.x) + vd_asfloat(vd_ZBufferParamsD3D.w)));
#endif
  r5.y = vd_asuint((1.0 / vd_asfloat(r5.y)));
  r4.w = ((r4.w != 0u) ? r5.y : vd_asuint(_ProjectionParams.z));
  r13.xy = vd_asuint((vd_asfloat(r0.wy) * float2(1.0, -1.0) + vd_asfloat(v1.xy)));
  r5.yz = vd_asuint(min(vd_asfloat(r1.xy), vd_asfloat(r13.xy)));
  r5.yz = vd_asuint((vd_asfloat(r5.yz) * vd_asfloat(_RTHandleScale.xy)));
  r14.xyzw = vd_asuint(_CameraDepthTexture.SampleBias(sampler_vd_point_clamp, vd_asfloat(r5.yz), vd_asfloat(_GlobalMipBias.x)).xyzw);
#if UNITY_UV_STARTS_AT_TOP
#else
  r14.x = vd_asuint(((-(vd_asfloat(r14.x))) + 1.0));
#endif
  r0.w = vd_asuint((-(int)(1.00000001e-07 < vd_asfloat(r14.x))));
#if UNITY_UV_STARTS_AT_TOP
  r5.y = vd_asuint((vd_asfloat(_ZBufferParams.z) * vd_asfloat(r14.x) + vd_asfloat(_ZBufferParams.w)));
#else
  r5.y = vd_asuint((vd_asfloat(vd_ZBufferParamsD3D.z) * vd_asfloat(r14.x) + vd_asfloat(vd_ZBufferParamsD3D.w)));
#endif
  r5.y = vd_asuint((1.0 / vd_asfloat(r5.y)));
  r0.w = ((r0.w != 0u) ? r5.y : vd_asuint(_ProjectionParams.z));
  r0.xy = vd_asuint((vd_asfloat(r0.xy) * float2(1.0, -1.0) + vd_asfloat(v1.xy)));
  r5.yz = vd_asuint(min(vd_asfloat(r1.xy), vd_asfloat(r0.xy)));
  r5.yz = vd_asuint((vd_asfloat(r5.yz) * vd_asfloat(_RTHandleScale.xy)));
  r15.xyzw = vd_asuint(_CameraDepthTexture.SampleBias(sampler_vd_point_clamp, vd_asfloat(r5.yz), vd_asfloat(_GlobalMipBias.x)).xyzw);
#if UNITY_UV_STARTS_AT_TOP
#else
  r15.x = vd_asuint(((-(vd_asfloat(r15.x))) + 1.0));
#endif
  r5.y = vd_asuint((-(int)(1.00000001e-07 < vd_asfloat(r15.x))));
#if UNITY_UV_STARTS_AT_TOP
  r5.z = vd_asuint((vd_asfloat(_ZBufferParams.z) * vd_asfloat(r15.x) + vd_asfloat(_ZBufferParams.w)));
#else
  r5.z = vd_asuint((vd_asfloat(vd_ZBufferParamsD3D.z) * vd_asfloat(r15.x) + vd_asfloat(vd_ZBufferParamsD3D.w)));
#endif
  r5.z = vd_asuint((1.0 / vd_asfloat(r5.z)));
  r5.y = ((r5.y != 0u) ? r5.z : vd_asuint(_ProjectionParams.z));
  r2.w = vd_asuint((vd_asfloat(r2.w) * 2.0 + vd_asfloat(r2.y)));
  r2.w = vd_asuint((vd_asfloat(r5.y) + vd_asfloat(r2.w)));
  r2.z = vd_asuint((vd_asfloat(r2.z) * 2.0 + vd_asfloat(r0.z)));
  r2.z = vd_asuint((vd_asfloat(r4.w) + vd_asfloat(r2.z)));
  r2.z = vd_asuint(((-(vd_asfloat(r2.z))) + vd_asfloat(r2.w)));
  r0.z = vd_asuint((vd_asfloat(r1.w) * 2.0 + vd_asfloat(r0.z)));
  r0.z = vd_asuint((vd_asfloat(r2.y) + vd_asfloat(r0.z)));
  r0.w = vd_asuint((vd_asfloat(r0.w) * 2.0 + vd_asfloat(r4.w)));
  r0.w = vd_asuint((vd_asfloat(r5.y) + vd_asfloat(r0.w)));
  r0.z = vd_asuint(((-(vd_asfloat(r0.w))) + vd_asfloat(r0.z)));
  r0.z = vd_asuint((vd_asfloat(r0.z) * vd_asfloat(r0.z)));
  r0.z = vd_asuint((vd_asfloat(r2.z) * vd_asfloat(r2.z) + vd_asfloat(r0.z)));
  r16.xyzw = vd_asuint((vd_asfloat(r3.xyzw) * float4(2.0, 2.0, 2.0, 2.0) + float4(-1.0, -1.0, -1.0, -1.0)));
#if UNITY_UV_STARTS_AT_TOP
  r17.xyzw = vd_asuint(((-(vd_asfloat(r16.yyyy))) * vd_asfloat((VD_COL(unity_MatrixInvVP, 1).xyzw))));
  r17.xyzw = vd_asuint((vd_asfloat((VD_COL(unity_MatrixInvVP, 0).xyzw)) * vd_asfloat(r16.xxxx) + vd_asfloat(r17.xyzw)));
  r5.xyzw = vd_asuint((vd_asfloat((VD_COL(unity_MatrixInvVP, 2).xyzw)) * vd_asfloat(r5.xxxx) + vd_asfloat(r17.xyzw)));
  r5.xyzw = vd_asuint((vd_asfloat(r5.xyzw) + vd_asfloat((VD_COL(unity_MatrixInvVP, 3).xyzw))));
#else
  r17.xyzw = vd_asuint((vd_asfloat(r16.yyyy) * vd_asfloat((VD_COL(vd_MatrixInvVPD3D, 1).xyzw))));
  r17.xyzw = vd_asuint((vd_asfloat((VD_COL(vd_MatrixInvVPD3D, 0).xyzw)) * vd_asfloat(r16.xxxx) + vd_asfloat(r17.xyzw)));
  r5.xyzw = vd_asuint((vd_asfloat((VD_COL(vd_MatrixInvVPD3D, 2).xyzw)) * vd_asfloat(r5.xxxx) + vd_asfloat(r17.xyzw)));
  r5.xyzw = vd_asuint((vd_asfloat(r5.xyzw) + vd_asfloat((VD_COL(vd_MatrixInvVPD3D, 3).xyzw))));
#endif
  r2.yzw = vd_asuint((vd_asfloat(r5.yzx) / vd_asfloat(r5.www)));
  r5.xw = vd_asuint(_BlitTexture_TexelSize.xy);
  r5.yz = uint2(0x00000000u, 0x00000000u);
  r17.xyzw = vd_asuint((vd_asfloat(r3.xyxy) + vd_asfloat(r5.xyzw)));
  r18.xyzw = vd_asuint(min(vd_asfloat(r1.xyxy), vd_asfloat(r17.xyzw)));
  r18.xyzw = vd_asuint((vd_asfloat(r18.xyzw) * vd_asfloat(_RTHandleScale.xyxy)));
  r19.xyzw = vd_asuint(_CameraDepthTexture.SampleBias(sampler_vd_point_clamp, vd_asfloat(r18.xy), vd_asfloat(_GlobalMipBias.x)).xyzw);
#if UNITY_UV_STARTS_AT_TOP
#else
  r19.x = vd_asuint(((-(vd_asfloat(r19.x))) + 1.0));
#endif
  r17.xyzw = vd_asuint((vd_asfloat(r17.xyzw) * float4(2.0, 2.0, 2.0, 2.0) + float4(-1.0, -1.0, -1.0, -1.0)));
#if UNITY_UV_STARTS_AT_TOP
  r20.xyzw = vd_asuint(((-(vd_asfloat(r17.yyyy))) * vd_asfloat((VD_COL(unity_MatrixInvVP, 1).xyzw))));
  r20.xyzw = vd_asuint((vd_asfloat((VD_COL(unity_MatrixInvVP, 0).xyzw)) * vd_asfloat(r17.xxxx) + vd_asfloat(r20.xyzw)));
  r19.xyzw = vd_asuint((vd_asfloat((VD_COL(unity_MatrixInvVP, 2).xyzw)) * vd_asfloat(r19.xxxx) + vd_asfloat(r20.xyzw)));
  r19.xyzw = vd_asuint((vd_asfloat(r19.xyzw) + vd_asfloat((VD_COL(unity_MatrixInvVP, 3).xyzw))));
#else
  r20.xyzw = vd_asuint((vd_asfloat(r17.yyyy) * vd_asfloat((VD_COL(vd_MatrixInvVPD3D, 1).xyzw))));
  r20.xyzw = vd_asuint((vd_asfloat((VD_COL(vd_MatrixInvVPD3D, 0).xyzw)) * vd_asfloat(r17.xxxx) + vd_asfloat(r20.xyzw)));
  r19.xyzw = vd_asuint((vd_asfloat((VD_COL(vd_MatrixInvVPD3D, 2).xyzw)) * vd_asfloat(r19.xxxx) + vd_asfloat(r20.xyzw)));
  r19.xyzw = vd_asuint((vd_asfloat(r19.xyzw) + vd_asfloat((VD_COL(vd_MatrixInvVPD3D, 3).xyzw))));
#endif
  r7.yzw = vd_asuint((vd_asfloat(r19.yzx) / vd_asfloat(r19.www)));
  r18.xyzw = vd_asuint(_CameraDepthTexture.SampleBias(sampler_vd_point_clamp, vd_asfloat(r18.zw), vd_asfloat(_GlobalMipBias.x)).xyzw);
#if UNITY_UV_STARTS_AT_TOP
  r19.xyzw = vd_asuint(((-(vd_asfloat(r17.wwww))) * vd_asfloat((VD_COL(unity_MatrixInvVP, 1).xyzw))));
  r17.xyzw = vd_asuint((vd_asfloat((VD_COL(unity_MatrixInvVP, 0).xyzw)) * vd_asfloat(r17.zzzz) + vd_asfloat(r19.xyzw)));
  r17.xyzw = vd_asuint((vd_asfloat((VD_COL(unity_MatrixInvVP, 2).xyzw)) * vd_asfloat(r18.xxxx) + vd_asfloat(r17.xyzw)));
  r17.xyzw = vd_asuint((vd_asfloat(r17.xyzw) + vd_asfloat((VD_COL(unity_MatrixInvVP, 3).xyzw))));
#else
  r18.x = vd_asuint(((-(vd_asfloat(r18.x))) + 1.0));
  r19.xyzw = vd_asuint((vd_asfloat(r17.wwww) * vd_asfloat((VD_COL(vd_MatrixInvVPD3D, 1).xyzw))));
  r17.xyzw = vd_asuint((vd_asfloat((VD_COL(vd_MatrixInvVPD3D, 0).xyzw)) * vd_asfloat(r17.zzzz) + vd_asfloat(r19.xyzw)));
  r17.xyzw = vd_asuint((vd_asfloat((VD_COL(vd_MatrixInvVPD3D, 2).xyzw)) * vd_asfloat(r18.xxxx) + vd_asfloat(r17.xyzw)));
  r17.xyzw = vd_asuint((vd_asfloat(r17.xyzw) + vd_asfloat((VD_COL(vd_MatrixInvVPD3D, 3).xyzw))));
#endif
  r9.yzw = vd_asuint((vd_asfloat(r17.zxy) / vd_asfloat(r17.www)));
  r9.yzw = vd_asuint(((-(vd_asfloat(r2.zwy))) + vd_asfloat(r9.yzw)));
  r2.yzw = vd_asuint(((-(vd_asfloat(r2.yzw))) + vd_asfloat(r7.yzw)));
  r7.yzw = vd_asuint((vd_asfloat(r2.yzw) * vd_asfloat(r9.yzw)));
  r2.yzw = vd_asuint((vd_asfloat(r9.wyz) * vd_asfloat(r2.zwy) + (-(vd_asfloat(r7.yzw)))));
  r0.w = vd_asuint(dot(vd_asfloat(r2.yzw), vd_asfloat(r2.yzw)));
  r0.w = vd_asuint(rsqrt(vd_asfloat(r0.w)));
  r2.yzw = vd_asuint((vd_asfloat(r0.www) * vd_asfloat(r2.yzw)));
  r3.xy = vd_asuint((vd_asfloat(r6.xy) * float2(2.0, 2.0) + float2(-1.0, -1.0)));
#if UNITY_UV_STARTS_AT_TOP
  r17.xyzw = vd_asuint(((-(vd_asfloat(r3.yyyy))) * vd_asfloat((VD_COL(unity_MatrixInvVP, 1).xyzw))));
  r17.xyzw = vd_asuint((vd_asfloat((VD_COL(unity_MatrixInvVP, 0).xyzw)) * vd_asfloat(r3.xxxx) + vd_asfloat(r17.xyzw)));
  r7.xyzw = vd_asuint((vd_asfloat((VD_COL(unity_MatrixInvVP, 2).xyzw)) * vd_asfloat(r7.xxxx) + vd_asfloat(r17.xyzw)));
  r7.xyzw = vd_asuint((vd_asfloat(r7.xyzw) + vd_asfloat((VD_COL(unity_MatrixInvVP, 3).xyzw))));
#else
  r17.xyzw = vd_asuint((vd_asfloat(r3.yyyy) * vd_asfloat((VD_COL(vd_MatrixInvVPD3D, 1).xyzw))));
  r17.xyzw = vd_asuint((vd_asfloat((VD_COL(vd_MatrixInvVPD3D, 0).xyzw)) * vd_asfloat(r3.xxxx) + vd_asfloat(r17.xyzw)));
  r7.xyzw = vd_asuint((vd_asfloat((VD_COL(vd_MatrixInvVPD3D, 2).xyzw)) * vd_asfloat(r7.xxxx) + vd_asfloat(r17.xyzw)));
  r7.xyzw = vd_asuint((vd_asfloat(r7.xyzw) + vd_asfloat((VD_COL(vd_MatrixInvVPD3D, 3).xyzw))));
#endif
  r7.xyz = vd_asuint((vd_asfloat(r7.yzx) / vd_asfloat(r7.www)));
  r3.xy = vd_asuint((vd_asfloat(r5.xy) + vd_asfloat(r6.xy)));
  r6.xw = vd_asuint(min(vd_asfloat(r1.xy), vd_asfloat(r3.xy)));
  r6.xw = vd_asuint((vd_asfloat(r6.xw) * vd_asfloat(_RTHandleScale.xy)));
  r17.xyzw = vd_asuint(_CameraDepthTexture.SampleBias(sampler_vd_point_clamp, vd_asfloat(r6.xw), vd_asfloat(_GlobalMipBias.x)).xyzw);
#if UNITY_UV_STARTS_AT_TOP
#else
  r17.x = vd_asuint(((-(vd_asfloat(r17.x))) + 1.0));
#endif
  r3.xy = vd_asuint((vd_asfloat(r3.xy) * float2(2.0, 2.0) + float2(-1.0, -1.0)));
#if UNITY_UV_STARTS_AT_TOP
  r18.xyzw = vd_asuint(((-(vd_asfloat(r3.yyyy))) * vd_asfloat((VD_COL(unity_MatrixInvVP, 1).xyzw))));
  r18.xyzw = vd_asuint((vd_asfloat((VD_COL(unity_MatrixInvVP, 0).xyzw)) * vd_asfloat(r3.xxxx) + vd_asfloat(r18.xyzw)));
  r17.xyzw = vd_asuint((vd_asfloat((VD_COL(unity_MatrixInvVP, 2).xyzw)) * vd_asfloat(r17.xxxx) + vd_asfloat(r18.xyzw)));
  r17.xyzw = vd_asuint((vd_asfloat(r17.xyzw) + vd_asfloat((VD_COL(unity_MatrixInvVP, 3).xyzw))));
#else
  r18.xyzw = vd_asuint((vd_asfloat(r3.yyyy) * vd_asfloat((VD_COL(vd_MatrixInvVPD3D, 1).xyzw))));
  r18.xyzw = vd_asuint((vd_asfloat((VD_COL(vd_MatrixInvVPD3D, 0).xyzw)) * vd_asfloat(r3.xxxx) + vd_asfloat(r18.xyzw)));
  r17.xyzw = vd_asuint((vd_asfloat((VD_COL(vd_MatrixInvVPD3D, 2).xyzw)) * vd_asfloat(r17.xxxx) + vd_asfloat(r18.xyzw)));
  r17.xyzw = vd_asuint((vd_asfloat(r17.xyzw) + vd_asfloat((VD_COL(vd_MatrixInvVPD3D, 3).xyzw))));
#endif
  r9.yzw = vd_asuint((vd_asfloat(r17.yzx) / vd_asfloat(r17.www)));
  r6.z = vd_asuint(v1.x);
  r3.xy = vd_asuint((vd_asfloat(r5.zw) + vd_asfloat(r6.zy)));
  r6.xy = vd_asuint(min(vd_asfloat(r1.xy), vd_asfloat(r3.xy)));
  r6.xy = vd_asuint((vd_asfloat(r6.xy) * vd_asfloat(_RTHandleScale.xy)));
  r6.xyzw = vd_asuint(_CameraDepthTexture.SampleBias(sampler_vd_point_clamp, vd_asfloat(r6.xy), vd_asfloat(_GlobalMipBias.x)).xyzw);
#if UNITY_UV_STARTS_AT_TOP
#else
  r6.x = vd_asuint(((-(vd_asfloat(r6.x))) + 1.0));
#endif
  r3.xy = vd_asuint((vd_asfloat(r3.xy) * float2(2.0, 2.0) + float2(-1.0, -1.0)));
#if UNITY_UV_STARTS_AT_TOP
  r17.xyzw = vd_asuint(((-(vd_asfloat(r3.yyyy))) * vd_asfloat((VD_COL(unity_MatrixInvVP, 1).xyzw))));
  r17.xyzw = vd_asuint((vd_asfloat((VD_COL(unity_MatrixInvVP, 0).xyzw)) * vd_asfloat(r3.xxxx) + vd_asfloat(r17.xyzw)));
  r6.xyzw = vd_asuint((vd_asfloat((VD_COL(unity_MatrixInvVP, 2).xyzw)) * vd_asfloat(r6.xxxx) + vd_asfloat(r17.xyzw)));
  r6.xyzw = vd_asuint((vd_asfloat(r6.xyzw) + vd_asfloat((VD_COL(unity_MatrixInvVP, 3).xyzw))));
#else
  r17.xyzw = vd_asuint((vd_asfloat(r3.yyyy) * vd_asfloat((VD_COL(vd_MatrixInvVPD3D, 1).xyzw))));
  r17.xyzw = vd_asuint((vd_asfloat((VD_COL(vd_MatrixInvVPD3D, 0).xyzw)) * vd_asfloat(r3.xxxx) + vd_asfloat(r17.xyzw)));
  r6.xyzw = vd_asuint((vd_asfloat((VD_COL(vd_MatrixInvVPD3D, 2).xyzw)) * vd_asfloat(r6.xxxx) + vd_asfloat(r17.xyzw)));
  r6.xyzw = vd_asuint((vd_asfloat(r6.xyzw) + vd_asfloat((VD_COL(vd_MatrixInvVPD3D, 3).xyzw))));
#endif
  r6.xyz = vd_asuint((vd_asfloat(r6.zxy) / vd_asfloat(r6.www)));
  r6.xyz = vd_asuint(((-(vd_asfloat(r7.yzx))) + vd_asfloat(r6.xyz)));
  r7.xyz = vd_asuint(((-(vd_asfloat(r7.xyz))) + vd_asfloat(r9.yzw)));
  r9.yzw = vd_asuint((vd_asfloat(r6.xyz) * vd_asfloat(r7.xyz)));
  r6.xyz = vd_asuint((vd_asfloat(r6.zxy) * vd_asfloat(r7.yzx) + (-(vd_asfloat(r9.yzw)))));
  r0.w = vd_asuint(dot(vd_asfloat(r6.xyz), vd_asfloat(r6.xyz)));
  r0.w = vd_asuint(rsqrt(vd_asfloat(r0.w)));
  r6.xyz = vd_asuint((vd_asfloat(r0.www) * vd_asfloat(r6.xyz)));
#if UNITY_UV_STARTS_AT_TOP
  r7.xyzw = vd_asuint(((-(vd_asfloat(r16.wwww))) * vd_asfloat((VD_COL(unity_MatrixInvVP, 1).xyzw))));
  r7.xyzw = vd_asuint((vd_asfloat((VD_COL(unity_MatrixInvVP, 0).xyzw)) * vd_asfloat(r16.zzzz) + vd_asfloat(r7.xyzw)));
  r7.xyzw = vd_asuint((vd_asfloat((VD_COL(unity_MatrixInvVP, 2).xyzw)) * vd_asfloat(r4.xxxx) + vd_asfloat(r7.xyzw)));
  r7.xyzw = vd_asuint((vd_asfloat(r7.xyzw) + vd_asfloat((VD_COL(unity_MatrixInvVP, 3).xyzw))));
#else
  r7.xyzw = vd_asuint((vd_asfloat(r16.wwww) * vd_asfloat((VD_COL(vd_MatrixInvVPD3D, 1).xyzw))));
  r7.xyzw = vd_asuint((vd_asfloat((VD_COL(vd_MatrixInvVPD3D, 0).xyzw)) * vd_asfloat(r16.zzzz) + vd_asfloat(r7.xyzw)));
  r7.xyzw = vd_asuint((vd_asfloat((VD_COL(vd_MatrixInvVPD3D, 2).xyzw)) * vd_asfloat(r4.xxxx) + vd_asfloat(r7.xyzw)));
  r7.xyzw = vd_asuint((vd_asfloat(r7.xyzw) + vd_asfloat((VD_COL(vd_MatrixInvVPD3D, 3).xyzw))));
#endif
  r7.xyz = vd_asuint((vd_asfloat(r7.yzx) / vd_asfloat(r7.www)));
  r3.xyzw = vd_asuint((vd_asfloat(r3.zwzw) + vd_asfloat(r5.xyzw)));
  r16.xyzw = vd_asuint(min(vd_asfloat(r1.xyxy), vd_asfloat(r3.xyzw)));
  r16.xyzw = vd_asuint((vd_asfloat(r16.xyzw) * vd_asfloat(_RTHandleScale.xyxy)));
  r17.xyzw = vd_asuint(_CameraDepthTexture.SampleBias(sampler_vd_point_clamp, vd_asfloat(r16.xy), vd_asfloat(_GlobalMipBias.x)).xyzw);
#if UNITY_UV_STARTS_AT_TOP
#else
  r17.x = vd_asuint(((-(vd_asfloat(r17.x))) + 1.0));
#endif
  r3.xyzw = vd_asuint((vd_asfloat(r3.xyzw) * float4(2.0, 2.0, 2.0, 2.0) + float4(-1.0, -1.0, -1.0, -1.0)));
#if UNITY_UV_STARTS_AT_TOP
  r18.xyzw = vd_asuint(((-(vd_asfloat(r3.yyyy))) * vd_asfloat((VD_COL(unity_MatrixInvVP, 1).xyzw))));
  r18.xyzw = vd_asuint((vd_asfloat((VD_COL(unity_MatrixInvVP, 0).xyzw)) * vd_asfloat(r3.xxxx) + vd_asfloat(r18.xyzw)));
  r17.xyzw = vd_asuint((vd_asfloat((VD_COL(unity_MatrixInvVP, 2).xyzw)) * vd_asfloat(r17.xxxx) + vd_asfloat(r18.xyzw)));
  r17.xyzw = vd_asuint((vd_asfloat(r17.xyzw) + vd_asfloat((VD_COL(unity_MatrixInvVP, 3).xyzw))));
#else
  r18.xyzw = vd_asuint((vd_asfloat(r3.yyyy) * vd_asfloat((VD_COL(vd_MatrixInvVPD3D, 1).xyzw))));
  r18.xyzw = vd_asuint((vd_asfloat((VD_COL(vd_MatrixInvVPD3D, 0).xyzw)) * vd_asfloat(r3.xxxx) + vd_asfloat(r18.xyzw)));
  r17.xyzw = vd_asuint((vd_asfloat((VD_COL(vd_MatrixInvVPD3D, 2).xyzw)) * vd_asfloat(r17.xxxx) + vd_asfloat(r18.xyzw)));
  r17.xyzw = vd_asuint((vd_asfloat(r17.xyzw) + vd_asfloat((VD_COL(vd_MatrixInvVPD3D, 3).xyzw))));
#endif
  r9.yzw = vd_asuint((vd_asfloat(r17.yzx) / vd_asfloat(r17.www)));
  r16.xyzw = vd_asuint(_CameraDepthTexture.SampleBias(sampler_vd_point_clamp, vd_asfloat(r16.zw), vd_asfloat(_GlobalMipBias.x)).xyzw);
#if UNITY_UV_STARTS_AT_TOP
  r17.xyzw = vd_asuint(((-(vd_asfloat(r3.wwww))) * vd_asfloat((VD_COL(unity_MatrixInvVP, 1).xyzw))));
  r3.xyzw = vd_asuint((vd_asfloat((VD_COL(unity_MatrixInvVP, 0).xyzw)) * vd_asfloat(r3.zzzz) + vd_asfloat(r17.xyzw)));
  r3.xyzw = vd_asuint((vd_asfloat((VD_COL(unity_MatrixInvVP, 2).xyzw)) * vd_asfloat(r16.xxxx) + vd_asfloat(r3.xyzw)));
  r3.xyzw = vd_asuint((vd_asfloat(r3.xyzw) + vd_asfloat((VD_COL(unity_MatrixInvVP, 3).xyzw))));
#else
  r16.x = vd_asuint(((-(vd_asfloat(r16.x))) + 1.0));
  r17.xyzw = vd_asuint((vd_asfloat(r3.wwww) * vd_asfloat((VD_COL(vd_MatrixInvVPD3D, 1).xyzw))));
  r3.xyzw = vd_asuint((vd_asfloat((VD_COL(vd_MatrixInvVPD3D, 0).xyzw)) * vd_asfloat(r3.zzzz) + vd_asfloat(r17.xyzw)));
  r3.xyzw = vd_asuint((vd_asfloat((VD_COL(vd_MatrixInvVPD3D, 2).xyzw)) * vd_asfloat(r16.xxxx) + vd_asfloat(r3.xyzw)));
  r3.xyzw = vd_asuint((vd_asfloat(r3.xyzw) + vd_asfloat((VD_COL(vd_MatrixInvVPD3D, 3).xyzw))));
#endif
  r3.xyz = vd_asuint((vd_asfloat(r3.zxy) / vd_asfloat(r3.www)));
  r3.xyz = vd_asuint(((-(vd_asfloat(r7.yzx))) + vd_asfloat(r3.xyz)));
  r7.xyz = vd_asuint(((-(vd_asfloat(r7.xyz))) + vd_asfloat(r9.yzw)));
  r9.yzw = vd_asuint((vd_asfloat(r3.xyz) * vd_asfloat(r7.xyz)));
  r3.xyz = vd_asuint((vd_asfloat(r3.zxy) * vd_asfloat(r7.yzx) + (-(vd_asfloat(r9.yzw)))));
  r0.w = vd_asuint(dot(vd_asfloat(r3.xyz), vd_asfloat(r3.xyz)));
  r0.w = vd_asuint(rsqrt(vd_asfloat(r0.w)));
  r7.xyz = vd_asuint((vd_asfloat(r0.www) * vd_asfloat(r3.xyz)));
  r4.xw = vd_asuint((vd_asfloat(r8.xy) * float2(2.0, 2.0) + float2(-1.0, -1.0)));
#if UNITY_UV_STARTS_AT_TOP
  r16.xyzw = vd_asuint(((-(vd_asfloat(r4.wwww))) * vd_asfloat((VD_COL(unity_MatrixInvVP, 1).xyzw))));
  r16.xyzw = vd_asuint((vd_asfloat((VD_COL(unity_MatrixInvVP, 0).xyzw)) * vd_asfloat(r4.xxxx) + vd_asfloat(r16.xyzw)));
  r9.xyzw = vd_asuint((vd_asfloat((VD_COL(unity_MatrixInvVP, 2).xyzw)) * vd_asfloat(r9.xxxx) + vd_asfloat(r16.xyzw)));
  r9.xyzw = vd_asuint((vd_asfloat(r9.xyzw) + vd_asfloat((VD_COL(unity_MatrixInvVP, 3).xyzw))));
#else
  r16.xyzw = vd_asuint((vd_asfloat(r4.wwww) * vd_asfloat((VD_COL(vd_MatrixInvVPD3D, 1).xyzw))));
  r16.xyzw = vd_asuint((vd_asfloat((VD_COL(vd_MatrixInvVPD3D, 0).xyzw)) * vd_asfloat(r4.xxxx) + vd_asfloat(r16.xyzw)));
  r9.xyzw = vd_asuint((vd_asfloat((VD_COL(vd_MatrixInvVPD3D, 2).xyzw)) * vd_asfloat(r9.xxxx) + vd_asfloat(r16.xyzw)));
  r9.xyzw = vd_asuint((vd_asfloat(r9.xyzw) + vd_asfloat((VD_COL(vd_MatrixInvVPD3D, 3).xyzw))));
#endif
  r9.xyz = vd_asuint((vd_asfloat(r9.yzx) / vd_asfloat(r9.www)));
  r8.z = vd_asuint(v1.y);
  r8.xyzw = vd_asuint((vd_asfloat(r5.xyzw) + vd_asfloat(r8.xzxy)));
  r16.xyzw = vd_asuint(min(vd_asfloat(r1.xyxy), vd_asfloat(r8.xyzw)));
  r16.xyzw = vd_asuint((vd_asfloat(r16.xyzw) * vd_asfloat(_RTHandleScale.xyxy)));
  r17.xyzw = vd_asuint(_CameraDepthTexture.SampleBias(sampler_vd_point_clamp, vd_asfloat(r16.xy), vd_asfloat(_GlobalMipBias.x)).xyzw);
#if UNITY_UV_STARTS_AT_TOP
#else
  r17.x = vd_asuint(((-(vd_asfloat(r17.x))) + 1.0));
#endif
  r8.xyzw = vd_asuint((vd_asfloat(r8.xyzw) * float4(2.0, 2.0, 2.0, 2.0) + float4(-1.0, -1.0, -1.0, -1.0)));
#if UNITY_UV_STARTS_AT_TOP
  r18.xyzw = vd_asuint(((-(vd_asfloat(r8.yyyy))) * vd_asfloat((VD_COL(unity_MatrixInvVP, 1).xyzw))));
  r18.xyzw = vd_asuint((vd_asfloat((VD_COL(unity_MatrixInvVP, 0).xyzw)) * vd_asfloat(r8.xxxx) + vd_asfloat(r18.xyzw)));
  r17.xyzw = vd_asuint((vd_asfloat((VD_COL(unity_MatrixInvVP, 2).xyzw)) * vd_asfloat(r17.xxxx) + vd_asfloat(r18.xyzw)));
  r17.xyzw = vd_asuint((vd_asfloat(r17.xyzw) + vd_asfloat((VD_COL(unity_MatrixInvVP, 3).xyzw))));
#else
  r18.xyzw = vd_asuint((vd_asfloat(r8.yyyy) * vd_asfloat((VD_COL(vd_MatrixInvVPD3D, 1).xyzw))));
  r18.xyzw = vd_asuint((vd_asfloat((VD_COL(vd_MatrixInvVPD3D, 0).xyzw)) * vd_asfloat(r8.xxxx) + vd_asfloat(r18.xyzw)));
  r17.xyzw = vd_asuint((vd_asfloat((VD_COL(vd_MatrixInvVPD3D, 2).xyzw)) * vd_asfloat(r17.xxxx) + vd_asfloat(r18.xyzw)));
  r17.xyzw = vd_asuint((vd_asfloat(r17.xyzw) + vd_asfloat((VD_COL(vd_MatrixInvVPD3D, 3).xyzw))));
#endif
  r11.yzw = vd_asuint((vd_asfloat(r17.yzx) / vd_asfloat(r17.www)));
  r16.xyzw = vd_asuint(_CameraDepthTexture.SampleBias(sampler_vd_point_clamp, vd_asfloat(r16.zw), vd_asfloat(_GlobalMipBias.x)).xyzw);
#if UNITY_UV_STARTS_AT_TOP
  r17.xyzw = vd_asuint(((-(vd_asfloat(r8.wwww))) * vd_asfloat((VD_COL(unity_MatrixInvVP, 1).xyzw))));
  r8.xyzw = vd_asuint((vd_asfloat((VD_COL(unity_MatrixInvVP, 0).xyzw)) * vd_asfloat(r8.zzzz) + vd_asfloat(r17.xyzw)));
  r8.xyzw = vd_asuint((vd_asfloat((VD_COL(unity_MatrixInvVP, 2).xyzw)) * vd_asfloat(r16.xxxx) + vd_asfloat(r8.xyzw)));
  r8.xyzw = vd_asuint((vd_asfloat(r8.xyzw) + vd_asfloat((VD_COL(unity_MatrixInvVP, 3).xyzw))));
#else
  r16.x = vd_asuint(((-(vd_asfloat(r16.x))) + 1.0));
  r17.xyzw = vd_asuint((vd_asfloat(r8.wwww) * vd_asfloat((VD_COL(vd_MatrixInvVPD3D, 1).xyzw))));
  r8.xyzw = vd_asuint((vd_asfloat((VD_COL(vd_MatrixInvVPD3D, 0).xyzw)) * vd_asfloat(r8.zzzz) + vd_asfloat(r17.xyzw)));
  r8.xyzw = vd_asuint((vd_asfloat((VD_COL(vd_MatrixInvVPD3D, 2).xyzw)) * vd_asfloat(r16.xxxx) + vd_asfloat(r8.xyzw)));
  r8.xyzw = vd_asuint((vd_asfloat(r8.xyzw) + vd_asfloat((VD_COL(vd_MatrixInvVPD3D, 3).xyzw))));
#endif
  r8.xyz = vd_asuint((vd_asfloat(r8.zxy) / vd_asfloat(r8.www)));
  r8.xyz = vd_asuint(((-(vd_asfloat(r9.yzx))) + vd_asfloat(r8.xyz)));
  r9.xyz = vd_asuint(((-(vd_asfloat(r9.xyz))) + vd_asfloat(r11.yzw)));
  r11.yzw = vd_asuint((vd_asfloat(r8.xyz) * vd_asfloat(r9.xyz)));
  r8.xyz = vd_asuint((vd_asfloat(r8.zxy) * vd_asfloat(r9.yzx) + (-(vd_asfloat(r11.yzw)))));
  r1.w = vd_asuint(dot(vd_asfloat(r8.xyz), vd_asfloat(r8.xyz)));
  r1.w = vd_asuint(rsqrt(vd_asfloat(r1.w)));
  r8.xyz = vd_asuint((vd_asfloat(r1.www) * vd_asfloat(r8.xyz)));
  r4.xw = vd_asuint((vd_asfloat(r10.xy) * float2(2.0, 2.0) + float2(-1.0, -1.0)));
#if UNITY_UV_STARTS_AT_TOP
  r9.xyzw = vd_asuint(((-(vd_asfloat(r4.wwww))) * vd_asfloat((VD_COL(unity_MatrixInvVP, 1).xyzw))));
  r9.xyzw = vd_asuint((vd_asfloat((VD_COL(unity_MatrixInvVP, 0).xyzw)) * vd_asfloat(r4.xxxx) + vd_asfloat(r9.xyzw)));
  r9.xyzw = vd_asuint((vd_asfloat((VD_COL(unity_MatrixInvVP, 2).xyzw)) * vd_asfloat(r11.xxxx) + vd_asfloat(r9.xyzw)));
  r9.xyzw = vd_asuint((vd_asfloat(r9.xyzw) + vd_asfloat((VD_COL(unity_MatrixInvVP, 3).xyzw))));
#else
  r9.xyzw = vd_asuint((vd_asfloat(r4.wwww) * vd_asfloat((VD_COL(vd_MatrixInvVPD3D, 1).xyzw))));
  r9.xyzw = vd_asuint((vd_asfloat((VD_COL(vd_MatrixInvVPD3D, 0).xyzw)) * vd_asfloat(r4.xxxx) + vd_asfloat(r9.xyzw)));
  r9.xyzw = vd_asuint((vd_asfloat((VD_COL(vd_MatrixInvVPD3D, 2).xyzw)) * vd_asfloat(r11.xxxx) + vd_asfloat(r9.xyzw)));
  r9.xyzw = vd_asuint((vd_asfloat(r9.xyzw) + vd_asfloat((VD_COL(vd_MatrixInvVPD3D, 3).xyzw))));
#endif
  r9.xyz = vd_asuint((vd_asfloat(r9.yzx) / vd_asfloat(r9.www)));
  r10.z = vd_asuint(v1.y);
  r10.xyzw = vd_asuint((vd_asfloat(r5.xyzw) + vd_asfloat(r10.xzxy)));
  r11.xyzw = vd_asuint(min(vd_asfloat(r1.xyxy), vd_asfloat(r10.xyzw)));
  r11.xyzw = vd_asuint((vd_asfloat(r11.xyzw) * vd_asfloat(_RTHandleScale.xyxy)));
  r16.xyzw = vd_asuint(_CameraDepthTexture.SampleBias(sampler_vd_point_clamp, vd_asfloat(r11.xy), vd_asfloat(_GlobalMipBias.x)).xyzw);
#if UNITY_UV_STARTS_AT_TOP
#else
  r16.x = vd_asuint(((-(vd_asfloat(r16.x))) + 1.0));
#endif
  r10.xyzw = vd_asuint((vd_asfloat(r10.xyzw) * float4(2.0, 2.0, 2.0, 2.0) + float4(-1.0, -1.0, -1.0, -1.0)));
#if UNITY_UV_STARTS_AT_TOP
  r17.xyzw = vd_asuint(((-(vd_asfloat(r10.yyyy))) * vd_asfloat((VD_COL(unity_MatrixInvVP, 1).xyzw))));
  r17.xyzw = vd_asuint((vd_asfloat((VD_COL(unity_MatrixInvVP, 0).xyzw)) * vd_asfloat(r10.xxxx) + vd_asfloat(r17.xyzw)));
  r16.xyzw = vd_asuint((vd_asfloat((VD_COL(unity_MatrixInvVP, 2).xyzw)) * vd_asfloat(r16.xxxx) + vd_asfloat(r17.xyzw)));
  r16.xyzw = vd_asuint((vd_asfloat(r16.xyzw) + vd_asfloat((VD_COL(unity_MatrixInvVP, 3).xyzw))));
#else
  r17.xyzw = vd_asuint((vd_asfloat(r10.yyyy) * vd_asfloat((VD_COL(vd_MatrixInvVPD3D, 1).xyzw))));
  r17.xyzw = vd_asuint((vd_asfloat((VD_COL(vd_MatrixInvVPD3D, 0).xyzw)) * vd_asfloat(r10.xxxx) + vd_asfloat(r17.xyzw)));
  r16.xyzw = vd_asuint((vd_asfloat((VD_COL(vd_MatrixInvVPD3D, 2).xyzw)) * vd_asfloat(r16.xxxx) + vd_asfloat(r17.xyzw)));
  r16.xyzw = vd_asuint((vd_asfloat(r16.xyzw) + vd_asfloat((VD_COL(vd_MatrixInvVPD3D, 3).xyzw))));
#endif
  r12.yzw = vd_asuint((vd_asfloat(r16.yzx) / vd_asfloat(r16.www)));
  r11.xyzw = vd_asuint(_CameraDepthTexture.SampleBias(sampler_vd_point_clamp, vd_asfloat(r11.zw), vd_asfloat(_GlobalMipBias.x)).xyzw);
#if UNITY_UV_STARTS_AT_TOP
  r16.xyzw = vd_asuint(((-(vd_asfloat(r10.wwww))) * vd_asfloat((VD_COL(unity_MatrixInvVP, 1).xyzw))));
  r10.xyzw = vd_asuint((vd_asfloat((VD_COL(unity_MatrixInvVP, 0).xyzw)) * vd_asfloat(r10.zzzz) + vd_asfloat(r16.xyzw)));
  r10.xyzw = vd_asuint((vd_asfloat((VD_COL(unity_MatrixInvVP, 2).xyzw)) * vd_asfloat(r11.xxxx) + vd_asfloat(r10.xyzw)));
  r10.xyzw = vd_asuint((vd_asfloat(r10.xyzw) + vd_asfloat((VD_COL(unity_MatrixInvVP, 3).xyzw))));
#else
  r11.x = vd_asuint(((-(vd_asfloat(r11.x))) + 1.0));
  r16.xyzw = vd_asuint((vd_asfloat(r10.wwww) * vd_asfloat((VD_COL(vd_MatrixInvVPD3D, 1).xyzw))));
  r10.xyzw = vd_asuint((vd_asfloat((VD_COL(vd_MatrixInvVPD3D, 0).xyzw)) * vd_asfloat(r10.zzzz) + vd_asfloat(r16.xyzw)));
  r10.xyzw = vd_asuint((vd_asfloat((VD_COL(vd_MatrixInvVPD3D, 2).xyzw)) * vd_asfloat(r11.xxxx) + vd_asfloat(r10.xyzw)));
  r10.xyzw = vd_asuint((vd_asfloat(r10.xyzw) + vd_asfloat((VD_COL(vd_MatrixInvVPD3D, 3).xyzw))));
#endif
  r10.xyz = vd_asuint((vd_asfloat(r10.zxy) / vd_asfloat(r10.www)));
  r10.xyz = vd_asuint(((-(vd_asfloat(r9.yzx))) + vd_asfloat(r10.xyz)));
  r9.xyz = vd_asuint(((-(vd_asfloat(r9.xyz))) + vd_asfloat(r12.yzw)));
  r11.xyz = vd_asuint((vd_asfloat(r9.xyz) * vd_asfloat(r10.xyz)));
  r9.xyz = vd_asuint((vd_asfloat(r10.zxy) * vd_asfloat(r9.yzx) + (-(vd_asfloat(r11.xyz)))));
  r1.w = vd_asuint(dot(vd_asfloat(r9.xyz), vd_asfloat(r9.xyz)));
  r1.w = vd_asuint(rsqrt(vd_asfloat(r1.w)));
  r9.xyz = vd_asuint((vd_asfloat(r1.www) * vd_asfloat(r9.xyz)));
  r4.xw = vd_asuint((vd_asfloat(r4.yz) * float2(2.0, 2.0) + float2(-1.0, -1.0)));
#if UNITY_UV_STARTS_AT_TOP
  r10.xyzw = vd_asuint(((-(vd_asfloat(r4.wwww))) * vd_asfloat((VD_COL(unity_MatrixInvVP, 1).xyzw))));
  r10.xyzw = vd_asuint((vd_asfloat((VD_COL(unity_MatrixInvVP, 0).xyzw)) * vd_asfloat(r4.xxxx) + vd_asfloat(r10.xyzw)));
  r10.xyzw = vd_asuint((vd_asfloat((VD_COL(unity_MatrixInvVP, 2).xyzw)) * vd_asfloat(r12.xxxx) + vd_asfloat(r10.xyzw)));
  r10.xyzw = vd_asuint((vd_asfloat(r10.xyzw) + vd_asfloat((VD_COL(unity_MatrixInvVP, 3).xyzw))));
#else
  r10.xyzw = vd_asuint((vd_asfloat(r4.wwww) * vd_asfloat((VD_COL(vd_MatrixInvVPD3D, 1).xyzw))));
  r10.xyzw = vd_asuint((vd_asfloat((VD_COL(vd_MatrixInvVPD3D, 0).xyzw)) * vd_asfloat(r4.xxxx) + vd_asfloat(r10.xyzw)));
  r10.xyzw = vd_asuint((vd_asfloat((VD_COL(vd_MatrixInvVPD3D, 2).xyzw)) * vd_asfloat(r12.xxxx) + vd_asfloat(r10.xyzw)));
  r10.xyzw = vd_asuint((vd_asfloat(r10.xyzw) + vd_asfloat((VD_COL(vd_MatrixInvVPD3D, 3).xyzw))));
#endif
  r10.xyz = vd_asuint((vd_asfloat(r10.yzx) / vd_asfloat(r10.www)));
  r4.xyzw = vd_asuint((vd_asfloat(r4.yzyz) + vd_asfloat(r5.xyzw)));
  r11.xyzw = vd_asuint(min(vd_asfloat(r1.xyxy), vd_asfloat(r4.xyzw)));
  r11.xyzw = vd_asuint((vd_asfloat(r11.xyzw) * vd_asfloat(_RTHandleScale.xyxy)));
  r12.xyzw = vd_asuint(_CameraDepthTexture.SampleBias(sampler_vd_point_clamp, vd_asfloat(r11.xy), vd_asfloat(_GlobalMipBias.x)).xyzw);
#if UNITY_UV_STARTS_AT_TOP
#else
  r12.x = vd_asuint(((-(vd_asfloat(r12.x))) + 1.0));
#endif
  r4.xyzw = vd_asuint((vd_asfloat(r4.xyzw) * float4(2.0, 2.0, 2.0, 2.0) + float4(-1.0, -1.0, -1.0, -1.0)));
#if UNITY_UV_STARTS_AT_TOP
  r16.xyzw = vd_asuint(((-(vd_asfloat(r4.yyyy))) * vd_asfloat((VD_COL(unity_MatrixInvVP, 1).xyzw))));
  r16.xyzw = vd_asuint((vd_asfloat((VD_COL(unity_MatrixInvVP, 0).xyzw)) * vd_asfloat(r4.xxxx) + vd_asfloat(r16.xyzw)));
  r12.xyzw = vd_asuint((vd_asfloat((VD_COL(unity_MatrixInvVP, 2).xyzw)) * vd_asfloat(r12.xxxx) + vd_asfloat(r16.xyzw)));
  r12.xyzw = vd_asuint((vd_asfloat(r12.xyzw) + vd_asfloat((VD_COL(unity_MatrixInvVP, 3).xyzw))));
#else
  r16.xyzw = vd_asuint((vd_asfloat(r4.yyyy) * vd_asfloat((VD_COL(vd_MatrixInvVPD3D, 1).xyzw))));
  r16.xyzw = vd_asuint((vd_asfloat((VD_COL(vd_MatrixInvVPD3D, 0).xyzw)) * vd_asfloat(r4.xxxx) + vd_asfloat(r16.xyzw)));
  r12.xyzw = vd_asuint((vd_asfloat((VD_COL(vd_MatrixInvVPD3D, 2).xyzw)) * vd_asfloat(r12.xxxx) + vd_asfloat(r16.xyzw)));
  r12.xyzw = vd_asuint((vd_asfloat(r12.xyzw) + vd_asfloat((VD_COL(vd_MatrixInvVPD3D, 3).xyzw))));
#endif
  r12.xyz = vd_asuint((vd_asfloat(r12.yzx) / vd_asfloat(r12.www)));
  r11.xyzw = vd_asuint(_CameraDepthTexture.SampleBias(sampler_vd_point_clamp, vd_asfloat(r11.zw), vd_asfloat(_GlobalMipBias.x)).xyzw);
#if UNITY_UV_STARTS_AT_TOP
  r16.xyzw = vd_asuint(((-(vd_asfloat(r4.wwww))) * vd_asfloat((VD_COL(unity_MatrixInvVP, 1).xyzw))));
  r4.xyzw = vd_asuint((vd_asfloat((VD_COL(unity_MatrixInvVP, 0).xyzw)) * vd_asfloat(r4.zzzz) + vd_asfloat(r16.xyzw)));
  r4.xyzw = vd_asuint((vd_asfloat((VD_COL(unity_MatrixInvVP, 2).xyzw)) * vd_asfloat(r11.xxxx) + vd_asfloat(r4.xyzw)));
  r4.xyzw = vd_asuint((vd_asfloat(r4.xyzw) + vd_asfloat((VD_COL(unity_MatrixInvVP, 3).xyzw))));
#else
  r11.x = vd_asuint(((-(vd_asfloat(r11.x))) + 1.0));
  r16.xyzw = vd_asuint((vd_asfloat(r4.wwww) * vd_asfloat((VD_COL(vd_MatrixInvVPD3D, 1).xyzw))));
  r4.xyzw = vd_asuint((vd_asfloat((VD_COL(vd_MatrixInvVPD3D, 0).xyzw)) * vd_asfloat(r4.zzzz) + vd_asfloat(r16.xyzw)));
  r4.xyzw = vd_asuint((vd_asfloat((VD_COL(vd_MatrixInvVPD3D, 2).xyzw)) * vd_asfloat(r11.xxxx) + vd_asfloat(r4.xyzw)));
  r4.xyzw = vd_asuint((vd_asfloat(r4.xyzw) + vd_asfloat((VD_COL(vd_MatrixInvVPD3D, 3).xyzw))));
#endif
  r4.xyz = vd_asuint((vd_asfloat(r4.zxy) / vd_asfloat(r4.www)));
  r4.xyz = vd_asuint(((-(vd_asfloat(r10.yzx))) + vd_asfloat(r4.xyz)));
  r10.xyz = vd_asuint(((-(vd_asfloat(r10.xyz))) + vd_asfloat(r12.xyz)));
  r11.xyz = vd_asuint((vd_asfloat(r4.xyz) * vd_asfloat(r10.xyz)));
  r4.xyz = vd_asuint((vd_asfloat(r4.zxy) * vd_asfloat(r10.yzx) + (-(vd_asfloat(r11.xyz)))));
  r1.w = vd_asuint(dot(vd_asfloat(r4.xyz), vd_asfloat(r4.xyz)));
  r1.w = vd_asuint(rsqrt(vd_asfloat(r1.w)));
  r10.xyz = vd_asuint((vd_asfloat(r1.www) * vd_asfloat(r4.xyz)));
  r11.xy = vd_asuint((vd_asfloat(r13.xy) * float2(2.0, 2.0) + float2(-1.0, -1.0)));
#if UNITY_UV_STARTS_AT_TOP
  r12.xyzw = vd_asuint(((-(vd_asfloat(r11.yyyy))) * vd_asfloat((VD_COL(unity_MatrixInvVP, 1).xyzw))));
  r11.xyzw = vd_asuint((vd_asfloat((VD_COL(unity_MatrixInvVP, 0).xyzw)) * vd_asfloat(r11.xxxx) + vd_asfloat(r12.xyzw)));
  r11.xyzw = vd_asuint((vd_asfloat((VD_COL(unity_MatrixInvVP, 2).xyzw)) * vd_asfloat(r14.xxxx) + vd_asfloat(r11.xyzw)));
  r11.xyzw = vd_asuint((vd_asfloat(r11.xyzw) + vd_asfloat((VD_COL(unity_MatrixInvVP, 3).xyzw))));
#else
  r12.xyzw = vd_asuint((vd_asfloat(r11.yyyy) * vd_asfloat((VD_COL(vd_MatrixInvVPD3D, 1).xyzw))));
  r11.xyzw = vd_asuint((vd_asfloat((VD_COL(vd_MatrixInvVPD3D, 0).xyzw)) * vd_asfloat(r11.xxxx) + vd_asfloat(r12.xyzw)));
  r11.xyzw = vd_asuint((vd_asfloat((VD_COL(vd_MatrixInvVPD3D, 2).xyzw)) * vd_asfloat(r14.xxxx) + vd_asfloat(r11.xyzw)));
  r11.xyzw = vd_asuint((vd_asfloat(r11.xyzw) + vd_asfloat((VD_COL(vd_MatrixInvVPD3D, 3).xyzw))));
#endif
  r11.xyz = vd_asuint((vd_asfloat(r11.yzx) / vd_asfloat(r11.www)));
  r12.xy = vd_asuint((vd_asfloat(r5.xy) + vd_asfloat(r13.xy)));
  r12.zw = vd_asuint(min(vd_asfloat(r1.xy), vd_asfloat(r12.xy)));
  r12.zw = vd_asuint((vd_asfloat(r12.zw) * vd_asfloat(_RTHandleScale.xy)));
  r14.xyzw = vd_asuint(_CameraDepthTexture.SampleBias(sampler_vd_point_clamp, vd_asfloat(r12.zw), vd_asfloat(_GlobalMipBias.x)).xyzw);
#if UNITY_UV_STARTS_AT_TOP
#else
  r14.x = vd_asuint(((-(vd_asfloat(r14.x))) + 1.0));
#endif
  r12.xy = vd_asuint((vd_asfloat(r12.xy) * float2(2.0, 2.0) + float2(-1.0, -1.0)));
#if UNITY_UV_STARTS_AT_TOP
  r16.xyzw = vd_asuint(((-(vd_asfloat(r12.yyyy))) * vd_asfloat((VD_COL(unity_MatrixInvVP, 1).xyzw))));
  r12.xyzw = vd_asuint((vd_asfloat((VD_COL(unity_MatrixInvVP, 0).xyzw)) * vd_asfloat(r12.xxxx) + vd_asfloat(r16.xyzw)));
  r12.xyzw = vd_asuint((vd_asfloat((VD_COL(unity_MatrixInvVP, 2).xyzw)) * vd_asfloat(r14.xxxx) + vd_asfloat(r12.xyzw)));
  r12.xyzw = vd_asuint((vd_asfloat(r12.xyzw) + vd_asfloat((VD_COL(unity_MatrixInvVP, 3).xyzw))));
#else
  r16.xyzw = vd_asuint((vd_asfloat(r12.yyyy) * vd_asfloat((VD_COL(vd_MatrixInvVPD3D, 1).xyzw))));
  r12.xyzw = vd_asuint((vd_asfloat((VD_COL(vd_MatrixInvVPD3D, 0).xyzw)) * vd_asfloat(r12.xxxx) + vd_asfloat(r16.xyzw)));
  r12.xyzw = vd_asuint((vd_asfloat((VD_COL(vd_MatrixInvVPD3D, 2).xyzw)) * vd_asfloat(r14.xxxx) + vd_asfloat(r12.xyzw)));
  r12.xyzw = vd_asuint((vd_asfloat(r12.xyzw) + vd_asfloat((VD_COL(vd_MatrixInvVPD3D, 3).xyzw))));
#endif
  r12.xyz = vd_asuint((vd_asfloat(r12.yzx) / vd_asfloat(r12.www)));
  r13.z = vd_asuint(v1.x);
  r13.xy = vd_asuint((vd_asfloat(r5.zw) + vd_asfloat(r13.zy)));
  r13.zw = vd_asuint(min(vd_asfloat(r1.xy), vd_asfloat(r13.xy)));
  r13.zw = vd_asuint((vd_asfloat(r13.zw) * vd_asfloat(_RTHandleScale.xy)));
  r14.xyzw = vd_asuint(_CameraDepthTexture.SampleBias(sampler_vd_point_clamp, vd_asfloat(r13.zw), vd_asfloat(_GlobalMipBias.x)).xyzw);
#if UNITY_UV_STARTS_AT_TOP
#else
  r14.x = vd_asuint(((-(vd_asfloat(r14.x))) + 1.0));
#endif
  r13.xy = vd_asuint((vd_asfloat(r13.xy) * float2(2.0, 2.0) + float2(-1.0, -1.0)));
#if UNITY_UV_STARTS_AT_TOP
  r16.xyzw = vd_asuint(((-(vd_asfloat(r13.yyyy))) * vd_asfloat((VD_COL(unity_MatrixInvVP, 1).xyzw))));
  r13.xyzw = vd_asuint((vd_asfloat((VD_COL(unity_MatrixInvVP, 0).xyzw)) * vd_asfloat(r13.xxxx) + vd_asfloat(r16.xyzw)));
  r13.xyzw = vd_asuint((vd_asfloat((VD_COL(unity_MatrixInvVP, 2).xyzw)) * vd_asfloat(r14.xxxx) + vd_asfloat(r13.xyzw)));
  r13.xyzw = vd_asuint((vd_asfloat(r13.xyzw) + vd_asfloat((VD_COL(unity_MatrixInvVP, 3).xyzw))));
#else
  r16.xyzw = vd_asuint((vd_asfloat(r13.yyyy) * vd_asfloat((VD_COL(vd_MatrixInvVPD3D, 1).xyzw))));
  r13.xyzw = vd_asuint((vd_asfloat((VD_COL(vd_MatrixInvVPD3D, 0).xyzw)) * vd_asfloat(r13.xxxx) + vd_asfloat(r16.xyzw)));
  r13.xyzw = vd_asuint((vd_asfloat((VD_COL(vd_MatrixInvVPD3D, 2).xyzw)) * vd_asfloat(r14.xxxx) + vd_asfloat(r13.xyzw)));
  r13.xyzw = vd_asuint((vd_asfloat(r13.xyzw) + vd_asfloat((VD_COL(vd_MatrixInvVPD3D, 3).xyzw))));
#endif
  r13.xyz = vd_asuint((vd_asfloat(r13.zxy) / vd_asfloat(r13.www)));
  r13.xyz = vd_asuint(((-(vd_asfloat(r11.yzx))) + vd_asfloat(r13.xyz)));
  r11.xyz = vd_asuint(((-(vd_asfloat(r11.xyz))) + vd_asfloat(r12.xyz)));
  r12.xyz = vd_asuint((vd_asfloat(r11.xyz) * vd_asfloat(r13.xyz)));
  r11.xyz = vd_asuint((vd_asfloat(r13.zxy) * vd_asfloat(r11.yzx) + (-(vd_asfloat(r12.xyz)))));
  r3.w = vd_asuint(dot(vd_asfloat(r11.xyz), vd_asfloat(r11.xyz)));
  r3.w = vd_asuint(rsqrt(vd_asfloat(r3.w)));
  r11.xyz = vd_asuint((vd_asfloat(r3.www) * vd_asfloat(r11.xyz)));
  r12.xy = vd_asuint((vd_asfloat(r0.xy) * float2(2.0, 2.0) + float2(-1.0, -1.0)));
#if UNITY_UV_STARTS_AT_TOP
  r13.xyzw = vd_asuint(((-(vd_asfloat(r12.yyyy))) * vd_asfloat((VD_COL(unity_MatrixInvVP, 1).xyzw))));
  r12.xyzw = vd_asuint((vd_asfloat((VD_COL(unity_MatrixInvVP, 0).xyzw)) * vd_asfloat(r12.xxxx) + vd_asfloat(r13.xyzw)));
  r12.xyzw = vd_asuint((vd_asfloat((VD_COL(unity_MatrixInvVP, 2).xyzw)) * vd_asfloat(r15.xxxx) + vd_asfloat(r12.xyzw)));
  r12.xyzw = vd_asuint((vd_asfloat(r12.xyzw) + vd_asfloat((VD_COL(unity_MatrixInvVP, 3).xyzw))));
#else
  r13.xyzw = vd_asuint((vd_asfloat(r12.yyyy) * vd_asfloat((VD_COL(vd_MatrixInvVPD3D, 1).xyzw))));
  r12.xyzw = vd_asuint((vd_asfloat((VD_COL(vd_MatrixInvVPD3D, 0).xyzw)) * vd_asfloat(r12.xxxx) + vd_asfloat(r13.xyzw)));
  r12.xyzw = vd_asuint((vd_asfloat((VD_COL(vd_MatrixInvVPD3D, 2).xyzw)) * vd_asfloat(r15.xxxx) + vd_asfloat(r12.xyzw)));
  r12.xyzw = vd_asuint((vd_asfloat(r12.xyzw) + vd_asfloat((VD_COL(vd_MatrixInvVPD3D, 3).xyzw))));
#endif
  r12.xyz = vd_asuint((vd_asfloat(r12.yzx) / vd_asfloat(r12.www)));
  r13.xyzw = vd_asuint((vd_asfloat(r0.xyxy) + vd_asfloat(r5.xyzw)));
  r14.xyzw = vd_asuint(min(vd_asfloat(r1.xyxy), vd_asfloat(r13.xyzw)));
  r14.xyzw = vd_asuint((vd_asfloat(r14.xyzw) * vd_asfloat(_RTHandleScale.xyxy)));
  r15.xyzw = vd_asuint(_CameraDepthTexture.SampleBias(sampler_vd_point_clamp, vd_asfloat(r14.xy), vd_asfloat(_GlobalMipBias.x)).xyzw);
#if UNITY_UV_STARTS_AT_TOP
#else
  r15.x = vd_asuint(((-(vd_asfloat(r15.x))) + 1.0));
#endif
  r13.xyzw = vd_asuint((vd_asfloat(r13.xyzw) * float4(2.0, 2.0, 2.0, 2.0) + float4(-1.0, -1.0, -1.0, -1.0)));
#if UNITY_UV_STARTS_AT_TOP
  r16.xyzw = vd_asuint(((-(vd_asfloat(r13.yyyy))) * vd_asfloat((VD_COL(unity_MatrixInvVP, 1).xyzw))));
  r16.xyzw = vd_asuint((vd_asfloat((VD_COL(unity_MatrixInvVP, 0).xyzw)) * vd_asfloat(r13.xxxx) + vd_asfloat(r16.xyzw)));
  r15.xyzw = vd_asuint((vd_asfloat((VD_COL(unity_MatrixInvVP, 2).xyzw)) * vd_asfloat(r15.xxxx) + vd_asfloat(r16.xyzw)));
  r15.xyzw = vd_asuint((vd_asfloat(r15.xyzw) + vd_asfloat((VD_COL(unity_MatrixInvVP, 3).xyzw))));
#else
  r16.xyzw = vd_asuint((vd_asfloat(r13.yyyy) * vd_asfloat((VD_COL(vd_MatrixInvVPD3D, 1).xyzw))));
  r16.xyzw = vd_asuint((vd_asfloat((VD_COL(vd_MatrixInvVPD3D, 0).xyzw)) * vd_asfloat(r13.xxxx) + vd_asfloat(r16.xyzw)));
  r15.xyzw = vd_asuint((vd_asfloat((VD_COL(vd_MatrixInvVPD3D, 2).xyzw)) * vd_asfloat(r15.xxxx) + vd_asfloat(r16.xyzw)));
  r15.xyzw = vd_asuint((vd_asfloat(r15.xyzw) + vd_asfloat((VD_COL(vd_MatrixInvVPD3D, 3).xyzw))));
#endif
  r15.xyz = vd_asuint((vd_asfloat(r15.yzx) / vd_asfloat(r15.www)));
  r14.xyzw = vd_asuint(_CameraDepthTexture.SampleBias(sampler_vd_point_clamp, vd_asfloat(r14.zw), vd_asfloat(_GlobalMipBias.x)).xyzw);
#if UNITY_UV_STARTS_AT_TOP
  r16.xyzw = vd_asuint(((-(vd_asfloat(r13.wwww))) * vd_asfloat((VD_COL(unity_MatrixInvVP, 1).xyzw))));
  r13.xyzw = vd_asuint((vd_asfloat((VD_COL(unity_MatrixInvVP, 0).xyzw)) * vd_asfloat(r13.zzzz) + vd_asfloat(r16.xyzw)));
  r13.xyzw = vd_asuint((vd_asfloat((VD_COL(unity_MatrixInvVP, 2).xyzw)) * vd_asfloat(r14.xxxx) + vd_asfloat(r13.xyzw)));
  r13.xyzw = vd_asuint((vd_asfloat(r13.xyzw) + vd_asfloat((VD_COL(unity_MatrixInvVP, 3).xyzw))));
#else
  r14.x = vd_asuint(((-(vd_asfloat(r14.x))) + 1.0));
  r16.xyzw = vd_asuint((vd_asfloat(r13.wwww) * vd_asfloat((VD_COL(vd_MatrixInvVPD3D, 1).xyzw))));
  r13.xyzw = vd_asuint((vd_asfloat((VD_COL(vd_MatrixInvVPD3D, 0).xyzw)) * vd_asfloat(r13.zzzz) + vd_asfloat(r16.xyzw)));
  r13.xyzw = vd_asuint((vd_asfloat((VD_COL(vd_MatrixInvVPD3D, 2).xyzw)) * vd_asfloat(r14.xxxx) + vd_asfloat(r13.xyzw)));
  r13.xyzw = vd_asuint((vd_asfloat(r13.xyzw) + vd_asfloat((VD_COL(vd_MatrixInvVPD3D, 3).xyzw))));
#endif
  r13.xyz = vd_asuint((vd_asfloat(r13.zxy) / vd_asfloat(r13.www)));
  r13.xyz = vd_asuint(((-(vd_asfloat(r12.yzx))) + vd_asfloat(r13.xyz)));
  r12.xyz = vd_asuint(((-(vd_asfloat(r12.xyz))) + vd_asfloat(r15.xyz)));
  r14.xyz = vd_asuint((vd_asfloat(r12.xyz) * vd_asfloat(r13.xyz)));
  r12.xyz = vd_asuint((vd_asfloat(r13.zxy) * vd_asfloat(r12.yzx) + (-(vd_asfloat(r14.xyz)))));
  r0.x = vd_asuint(dot(vd_asfloat(r12.xyz), vd_asfloat(r12.xyz)));
  r0.x = vd_asuint(rsqrt(vd_asfloat(r0.x)));
  r7.xyz = vd_asuint((vd_asfloat(r9.xyz) * float3(2.0, 2.0, 2.0) + vd_asfloat(r7.xyz)));
  r7.xyz = vd_asuint((vd_asfloat(r12.xyz) * vd_asfloat(r0.xxx) + vd_asfloat(r7.xyz)));
  r8.xyz = vd_asuint((vd_asfloat(r8.xyz) * float3(2.0, 2.0, 2.0) + vd_asfloat(r2.yzw)));
  r4.xyz = vd_asuint((vd_asfloat(r4.xyz) * vd_asfloat(r1.www) + vd_asfloat(r8.xyz)));
  r4.xyz = vd_asuint(((-(vd_asfloat(r4.xyz))) + vd_asfloat(r7.xyz)));
  r2.yzw = vd_asuint((vd_asfloat(r6.xyz) * float3(2.0, 2.0, 2.0) + vd_asfloat(r2.yzw)));
  r2.yzw = vd_asuint((vd_asfloat(r3.xyz) * vd_asfloat(r0.www) + vd_asfloat(r2.yzw)));
  r3.xyz = vd_asuint((vd_asfloat(r11.xyz) * float3(2.0, 2.0, 2.0) + vd_asfloat(r10.xyz)));
  r0.xyw = vd_asuint((vd_asfloat(r12.xyz) * vd_asfloat(r0.xxx) + vd_asfloat(r3.xyz)));
  r0.xyw = vd_asuint(((-(vd_asfloat(r0.xyw))) + vd_asfloat(r2.yzw)));
  r1.w = vd_asuint(dot(vd_asfloat(r4.xyz), vd_asfloat(r4.xyz)));
  r0.x = vd_asuint(dot(vd_asfloat(r0.xyw), vd_asfloat(r0.xyw)));
  r0.x = vd_asuint((vd_asfloat(r0.x) + vd_asfloat(r1.w)));
  r0.xz = vd_asuint(sqrt(vd_asfloat(r0.xz)));
  r0.y = vd_asuint(max(vd_asfloat(r1.z), 0.00100000005));
  r0.y = vd_asuint((1.0 / vd_asfloat(r0.y)));
  r0.y = vd_asuint((vd_asfloat(r0.y) * vd_asfloat(r0.z)));
  r0.z = vd_asuint((vd_asfloat(r0.y) * vd_asfloat(_DepthSensitivity)));
  r0.w = vd_asuint((vd_asfloat(r0.x) * vd_asfloat(_NormalSensitivity)));
  r0.y = vd_asuint((vd_asfloat(r0.y) * vd_asfloat(_DepthSensitivity) + (-(vd_asfloat(_DepthThreshold)))));
  r1.zw = vd_asuint((float2(1.0, 1.0) / vd_asfloat(float2(_DepthEdgeSoftness, _NormalEdgeSoftness))));
  r0.y = vd_asuint(saturate((vd_asfloat(r0.y) * vd_asfloat(r1.z))));
  r1.z = vd_asuint((vd_asfloat(r0.y) * -2.0 + 3.0));
  r0.y = vd_asuint((vd_asfloat(r0.y) * vd_asfloat(r0.y)));
  r0.y = vd_asuint((vd_asfloat(r0.y) * vd_asfloat(r1.z)));
  r0.x = vd_asuint((vd_asfloat(r0.x) * vd_asfloat(_NormalSensitivity) + (-(vd_asfloat(_NormalThreshold)))));
  r0.x = vd_asuint(saturate((vd_asfloat(r1.w) * vd_asfloat(r0.x))));
  r1.z = vd_asuint((vd_asfloat(r0.x) * -2.0 + 3.0));
  r0.x = vd_asuint((vd_asfloat(r0.x) * vd_asfloat(r0.x)));
  r0.x = vd_asuint((vd_asfloat(r0.x) * vd_asfloat(r1.z)));
  r0.x = vd_asuint(max(vd_asfloat(r0.x), vd_asfloat(r0.y)));
  r3.xy = vd_asuint((vd_asfloat(_BlitTexture_TexelSize.xy) * vd_asfloat((_CavityThickness).xx)));
  r3.z = 0x00000000u;
  r4.xyzw = vd_asuint(((-(vd_asfloat(r3.xzzy))) + vd_asfloat(v1.xyxy)));
  r6.xyzw = vd_asuint(min(vd_asfloat(r1.xyxy), vd_asfloat(r4.xyzw)));
  r6.xyzw = vd_asuint((vd_asfloat(r6.xyzw) * vd_asfloat(_RTHandleScale.xyxy)));
  r7.xyzw = vd_asuint(_CameraDepthTexture.SampleBias(sampler_vd_point_clamp, vd_asfloat(r6.xy), vd_asfloat(_GlobalMipBias.x)).xyzw);
#if UNITY_UV_STARTS_AT_TOP
#else
  r7.x = vd_asuint(((-(vd_asfloat(r7.x))) + 1.0));
#endif
  r8.xyzw = vd_asuint((vd_asfloat(r4.xyzw) * float4(2.0, 2.0, 2.0, 2.0) + float4(-1.0, -1.0, -1.0, -1.0)));
#if UNITY_UV_STARTS_AT_TOP
  r9.xyzw = vd_asuint(((-(vd_asfloat(r8.yyyy))) * vd_asfloat((VD_COL(unity_MatrixInvVP, 1).xyzw))));
  r9.xyzw = vd_asuint((vd_asfloat((VD_COL(unity_MatrixInvVP, 0).xyzw)) * vd_asfloat(r8.xxxx) + vd_asfloat(r9.xyzw)));
  r7.xyzw = vd_asuint((vd_asfloat((VD_COL(unity_MatrixInvVP, 2).xyzw)) * vd_asfloat(r7.xxxx) + vd_asfloat(r9.xyzw)));
  r7.xyzw = vd_asuint((vd_asfloat(r7.xyzw) + vd_asfloat((VD_COL(unity_MatrixInvVP, 3).xyzw))));
#else
  r9.xyzw = vd_asuint((vd_asfloat(r8.yyyy) * vd_asfloat((VD_COL(vd_MatrixInvVPD3D, 1).xyzw))));
  r9.xyzw = vd_asuint((vd_asfloat((VD_COL(vd_MatrixInvVPD3D, 0).xyzw)) * vd_asfloat(r8.xxxx) + vd_asfloat(r9.xyzw)));
  r7.xyzw = vd_asuint((vd_asfloat((VD_COL(vd_MatrixInvVPD3D, 2).xyzw)) * vd_asfloat(r7.xxxx) + vd_asfloat(r9.xyzw)));
  r7.xyzw = vd_asuint((vd_asfloat(r7.xyzw) + vd_asfloat((VD_COL(vd_MatrixInvVPD3D, 3).xyzw))));
#endif
  r2.yzw = vd_asuint((vd_asfloat(r7.yzx) / vd_asfloat(r7.www)));
  r7.xyzw = vd_asuint((vd_asfloat(r5.xyzw) + vd_asfloat(r4.xyxy)));
  r9.xyzw = vd_asuint(min(vd_asfloat(r1.xyxy), vd_asfloat(r7.xyzw)));
  r9.xyzw = vd_asuint((vd_asfloat(r9.xyzw) * vd_asfloat(_RTHandleScale.xyxy)));
  r10.xyzw = vd_asuint(_CameraDepthTexture.SampleBias(sampler_vd_point_clamp, vd_asfloat(r9.xy), vd_asfloat(_GlobalMipBias.x)).xyzw);
#if UNITY_UV_STARTS_AT_TOP
#else
  r10.x = vd_asuint(((-(vd_asfloat(r10.x))) + 1.0));
#endif
  r7.xyzw = vd_asuint((vd_asfloat(r7.xyzw) * float4(2.0, 2.0, 2.0, 2.0) + float4(-1.0, -1.0, -1.0, -1.0)));
#if UNITY_UV_STARTS_AT_TOP
  r11.xyzw = vd_asuint(((-(vd_asfloat(r7.yyyy))) * vd_asfloat((VD_COL(unity_MatrixInvVP, 1).xyzw))));
  r11.xyzw = vd_asuint((vd_asfloat((VD_COL(unity_MatrixInvVP, 0).xyzw)) * vd_asfloat(r7.xxxx) + vd_asfloat(r11.xyzw)));
  r10.xyzw = vd_asuint((vd_asfloat((VD_COL(unity_MatrixInvVP, 2).xyzw)) * vd_asfloat(r10.xxxx) + vd_asfloat(r11.xyzw)));
  r10.xyzw = vd_asuint((vd_asfloat(r10.xyzw) + vd_asfloat((VD_COL(unity_MatrixInvVP, 3).xyzw))));
#else
  r11.xyzw = vd_asuint((vd_asfloat(r7.yyyy) * vd_asfloat((VD_COL(vd_MatrixInvVPD3D, 1).xyzw))));
  r11.xyzw = vd_asuint((vd_asfloat((VD_COL(vd_MatrixInvVPD3D, 0).xyzw)) * vd_asfloat(r7.xxxx) + vd_asfloat(r11.xyzw)));
  r10.xyzw = vd_asuint((vd_asfloat((VD_COL(vd_MatrixInvVPD3D, 2).xyzw)) * vd_asfloat(r10.xxxx) + vd_asfloat(r11.xyzw)));
  r10.xyzw = vd_asuint((vd_asfloat(r10.xyzw) + vd_asfloat((VD_COL(vd_MatrixInvVPD3D, 3).xyzw))));
#endif
  r10.xyz = vd_asuint((vd_asfloat(r10.yzx) / vd_asfloat(r10.www)));
  r9.xyzw = vd_asuint(_CameraDepthTexture.SampleBias(sampler_vd_point_clamp, vd_asfloat(r9.zw), vd_asfloat(_GlobalMipBias.x)).xyzw);
#if UNITY_UV_STARTS_AT_TOP
  r11.xyzw = vd_asuint(((-(vd_asfloat(r7.wwww))) * vd_asfloat((VD_COL(unity_MatrixInvVP, 1).xyzw))));
  r7.xyzw = vd_asuint((vd_asfloat((VD_COL(unity_MatrixInvVP, 0).xyzw)) * vd_asfloat(r7.zzzz) + vd_asfloat(r11.xyzw)));
  r7.xyzw = vd_asuint((vd_asfloat((VD_COL(unity_MatrixInvVP, 2).xyzw)) * vd_asfloat(r9.xxxx) + vd_asfloat(r7.xyzw)));
  r7.xyzw = vd_asuint((vd_asfloat(r7.xyzw) + vd_asfloat((VD_COL(unity_MatrixInvVP, 3).xyzw))));
#else
  r9.x = vd_asuint(((-(vd_asfloat(r9.x))) + 1.0));
  r11.xyzw = vd_asuint((vd_asfloat(r7.wwww) * vd_asfloat((VD_COL(vd_MatrixInvVPD3D, 1).xyzw))));
  r7.xyzw = vd_asuint((vd_asfloat((VD_COL(vd_MatrixInvVPD3D, 0).xyzw)) * vd_asfloat(r7.zzzz) + vd_asfloat(r11.xyzw)));
  r7.xyzw = vd_asuint((vd_asfloat((VD_COL(vd_MatrixInvVPD3D, 2).xyzw)) * vd_asfloat(r9.xxxx) + vd_asfloat(r7.xyzw)));
  r7.xyzw = vd_asuint((vd_asfloat(r7.xyzw) + vd_asfloat((VD_COL(vd_MatrixInvVPD3D, 3).xyzw))));
#endif
  r7.xyz = vd_asuint((vd_asfloat(r7.zxy) / vd_asfloat(r7.www)));
  r7.xyz = vd_asuint(((-(vd_asfloat(r2.zwy))) + vd_asfloat(r7.xyz)));
  r2.yzw = vd_asuint(((-(vd_asfloat(r2.yzw))) + vd_asfloat(r10.xyz)));
  r9.xyz = vd_asuint((vd_asfloat(r2.yzw) * vd_asfloat(r7.xyz)));
  r2.yzw = vd_asuint((vd_asfloat(r7.zxy) * vd_asfloat(r2.zwy) + (-(vd_asfloat(r9.xyz)))));
  r0.y = vd_asuint(dot(vd_asfloat(r2.yzw), vd_asfloat(r2.yzw)));
  r0.y = vd_asuint(rsqrt(vd_asfloat(r0.y)));
  r0.y = vd_asuint((vd_asfloat(r0.y) * vd_asfloat(r2.y)));
  r7.xy = vd_asuint((vd_asfloat(r3.xz) + vd_asfloat(v1.xy)));
  r1.zw = vd_asuint(min(vd_asfloat(r1.xy), vd_asfloat(r7.xy)));
  r1.zw = vd_asuint((vd_asfloat(r1.zw) * vd_asfloat(_RTHandleScale.xy)));
  r9.xyzw = vd_asuint(_CameraDepthTexture.SampleBias(sampler_vd_point_clamp, vd_asfloat(r1.zw), vd_asfloat(_GlobalMipBias.x)).xyzw);
#if UNITY_UV_STARTS_AT_TOP
#else
  r9.x = vd_asuint(((-(vd_asfloat(r9.x))) + 1.0));
#endif
  r1.zw = vd_asuint((vd_asfloat(r7.xy) * float2(2.0, 2.0) + float2(-1.0, -1.0)));
#if UNITY_UV_STARTS_AT_TOP
  r10.xyzw = vd_asuint(((-(vd_asfloat(r1.wwww))) * vd_asfloat((VD_COL(unity_MatrixInvVP, 1).xyzw))));
  r10.xyzw = vd_asuint((vd_asfloat((VD_COL(unity_MatrixInvVP, 0).xyzw)) * vd_asfloat(r1.zzzz) + vd_asfloat(r10.xyzw)));
  r9.xyzw = vd_asuint((vd_asfloat((VD_COL(unity_MatrixInvVP, 2).xyzw)) * vd_asfloat(r9.xxxx) + vd_asfloat(r10.xyzw)));
  r9.xyzw = vd_asuint((vd_asfloat(r9.xyzw) + vd_asfloat((VD_COL(unity_MatrixInvVP, 3).xyzw))));
#else
  r10.xyzw = vd_asuint((vd_asfloat(r1.wwww) * vd_asfloat((VD_COL(vd_MatrixInvVPD3D, 1).xyzw))));
  r10.xyzw = vd_asuint((vd_asfloat((VD_COL(vd_MatrixInvVPD3D, 0).xyzw)) * vd_asfloat(r1.zzzz) + vd_asfloat(r10.xyzw)));
  r9.xyzw = vd_asuint((vd_asfloat((VD_COL(vd_MatrixInvVPD3D, 2).xyzw)) * vd_asfloat(r9.xxxx) + vd_asfloat(r10.xyzw)));
  r9.xyzw = vd_asuint((vd_asfloat(r9.xyzw) + vd_asfloat((VD_COL(vd_MatrixInvVPD3D, 3).xyzw))));
#endif
  r2.yzw = vd_asuint((vd_asfloat(r9.yzx) / vd_asfloat(r9.www)));
  r7.z = vd_asuint(v1.y);
  r7.xyzw = vd_asuint((vd_asfloat(r5.xyzw) + vd_asfloat(r7.xzxy)));
  r9.xyzw = vd_asuint(min(vd_asfloat(r1.xyxy), vd_asfloat(r7.xyzw)));
  r9.xyzw = vd_asuint((vd_asfloat(r9.xyzw) * vd_asfloat(_RTHandleScale.xyxy)));
  r10.xyzw = vd_asuint(_CameraDepthTexture.SampleBias(sampler_vd_point_clamp, vd_asfloat(r9.xy), vd_asfloat(_GlobalMipBias.x)).xyzw);
#if UNITY_UV_STARTS_AT_TOP
#else
  r10.x = vd_asuint(((-(vd_asfloat(r10.x))) + 1.0));
#endif
  r7.xyzw = vd_asuint((vd_asfloat(r7.xyzw) * float4(2.0, 2.0, 2.0, 2.0) + float4(-1.0, -1.0, -1.0, -1.0)));
#if UNITY_UV_STARTS_AT_TOP
  r11.xyzw = vd_asuint(((-(vd_asfloat(r7.yyyy))) * vd_asfloat((VD_COL(unity_MatrixInvVP, 1).xyzw))));
  r11.xyzw = vd_asuint((vd_asfloat((VD_COL(unity_MatrixInvVP, 0).xyzw)) * vd_asfloat(r7.xxxx) + vd_asfloat(r11.xyzw)));
  r10.xyzw = vd_asuint((vd_asfloat((VD_COL(unity_MatrixInvVP, 2).xyzw)) * vd_asfloat(r10.xxxx) + vd_asfloat(r11.xyzw)));
  r10.xyzw = vd_asuint((vd_asfloat(r10.xyzw) + vd_asfloat((VD_COL(unity_MatrixInvVP, 3).xyzw))));
#else
  r11.xyzw = vd_asuint((vd_asfloat(r7.yyyy) * vd_asfloat((VD_COL(vd_MatrixInvVPD3D, 1).xyzw))));
  r11.xyzw = vd_asuint((vd_asfloat((VD_COL(vd_MatrixInvVPD3D, 0).xyzw)) * vd_asfloat(r7.xxxx) + vd_asfloat(r11.xyzw)));
  r10.xyzw = vd_asuint((vd_asfloat((VD_COL(vd_MatrixInvVPD3D, 2).xyzw)) * vd_asfloat(r10.xxxx) + vd_asfloat(r11.xyzw)));
  r10.xyzw = vd_asuint((vd_asfloat(r10.xyzw) + vd_asfloat((VD_COL(vd_MatrixInvVPD3D, 3).xyzw))));
#endif
  r10.xyz = vd_asuint((vd_asfloat(r10.yzx) / vd_asfloat(r10.www)));
  r9.xyzw = vd_asuint(_CameraDepthTexture.SampleBias(sampler_vd_point_clamp, vd_asfloat(r9.zw), vd_asfloat(_GlobalMipBias.x)).xyzw);
#if UNITY_UV_STARTS_AT_TOP
  r11.xyzw = vd_asuint(((-(vd_asfloat(r7.wwww))) * vd_asfloat((VD_COL(unity_MatrixInvVP, 1).xyzw))));
  r7.xyzw = vd_asuint((vd_asfloat((VD_COL(unity_MatrixInvVP, 0).xyzw)) * vd_asfloat(r7.zzzz) + vd_asfloat(r11.xyzw)));
  r7.xyzw = vd_asuint((vd_asfloat((VD_COL(unity_MatrixInvVP, 2).xyzw)) * vd_asfloat(r9.xxxx) + vd_asfloat(r7.xyzw)));
  r7.xyzw = vd_asuint((vd_asfloat(r7.xyzw) + vd_asfloat((VD_COL(unity_MatrixInvVP, 3).xyzw))));
#else
  r9.x = vd_asuint(((-(vd_asfloat(r9.x))) + 1.0));
  r11.xyzw = vd_asuint((vd_asfloat(r7.wwww) * vd_asfloat((VD_COL(vd_MatrixInvVPD3D, 1).xyzw))));
  r7.xyzw = vd_asuint((vd_asfloat((VD_COL(vd_MatrixInvVPD3D, 0).xyzw)) * vd_asfloat(r7.zzzz) + vd_asfloat(r11.xyzw)));
  r7.xyzw = vd_asuint((vd_asfloat((VD_COL(vd_MatrixInvVPD3D, 2).xyzw)) * vd_asfloat(r9.xxxx) + vd_asfloat(r7.xyzw)));
  r7.xyzw = vd_asuint((vd_asfloat(r7.xyzw) + vd_asfloat((VD_COL(vd_MatrixInvVPD3D, 3).xyzw))));
#endif
  r7.xyz = vd_asuint((vd_asfloat(r7.zxy) / vd_asfloat(r7.www)));
  r7.xyz = vd_asuint(((-(vd_asfloat(r2.zwy))) + vd_asfloat(r7.xyz)));
  r2.yzw = vd_asuint(((-(vd_asfloat(r2.yzw))) + vd_asfloat(r10.xyz)));
  r9.xyz = vd_asuint((vd_asfloat(r2.yzw) * vd_asfloat(r7.xyz)));
  r2.yzw = vd_asuint((vd_asfloat(r7.zxy) * vd_asfloat(r2.zwy) + (-(vd_asfloat(r9.xyz)))));
  r1.z = vd_asuint(dot(vd_asfloat(r2.yzw), vd_asfloat(r2.yzw)));
  r1.z = vd_asuint(rsqrt(vd_asfloat(r1.z)));
  r6.xyzw = vd_asuint(_CameraDepthTexture.SampleBias(sampler_vd_point_clamp, vd_asfloat(r6.zw), vd_asfloat(_GlobalMipBias.x)).xyzw);
#if UNITY_UV_STARTS_AT_TOP
  r7.xyzw = vd_asuint(((-(vd_asfloat(r8.wwww))) * vd_asfloat((VD_COL(unity_MatrixInvVP, 1).xyzw))));
  r7.xyzw = vd_asuint((vd_asfloat((VD_COL(unity_MatrixInvVP, 0).xyzw)) * vd_asfloat(r8.zzzz) + vd_asfloat(r7.xyzw)));
  r6.xyzw = vd_asuint((vd_asfloat((VD_COL(unity_MatrixInvVP, 2).xyzw)) * vd_asfloat(r6.xxxx) + vd_asfloat(r7.xyzw)));
  r6.xyzw = vd_asuint((vd_asfloat(r6.xyzw) + vd_asfloat((VD_COL(unity_MatrixInvVP, 3).xyzw))));
#else
  r6.x = vd_asuint(((-(vd_asfloat(r6.x))) + 1.0));
  r7.xyzw = vd_asuint((vd_asfloat(r8.wwww) * vd_asfloat((VD_COL(vd_MatrixInvVPD3D, 1).xyzw))));
  r7.xyzw = vd_asuint((vd_asfloat((VD_COL(vd_MatrixInvVPD3D, 0).xyzw)) * vd_asfloat(r8.zzzz) + vd_asfloat(r7.xyzw)));
  r6.xyzw = vd_asuint((vd_asfloat((VD_COL(vd_MatrixInvVPD3D, 2).xyzw)) * vd_asfloat(r6.xxxx) + vd_asfloat(r7.xyzw)));
  r6.xyzw = vd_asuint((vd_asfloat(r6.xyzw) + vd_asfloat((VD_COL(vd_MatrixInvVPD3D, 3).xyzw))));
#endif
  r6.xyz = vd_asuint((vd_asfloat(r6.yzx) / vd_asfloat(r6.www)));
  r4.xyzw = vd_asuint((vd_asfloat(r5.xyzw) + vd_asfloat(r4.zwzw)));
  r7.xyzw = vd_asuint(min(vd_asfloat(r1.xyxy), vd_asfloat(r4.xyzw)));
  r7.xyzw = vd_asuint((vd_asfloat(r7.xyzw) * vd_asfloat(_RTHandleScale.xyxy)));
  r8.xyzw = vd_asuint(_CameraDepthTexture.SampleBias(sampler_vd_point_clamp, vd_asfloat(r7.xy), vd_asfloat(_GlobalMipBias.x)).xyzw);
#if UNITY_UV_STARTS_AT_TOP
#else
  r8.x = vd_asuint(((-(vd_asfloat(r8.x))) + 1.0));
#endif
  r4.xyzw = vd_asuint((vd_asfloat(r4.xyzw) * float4(2.0, 2.0, 2.0, 2.0) + float4(-1.0, -1.0, -1.0, -1.0)));
#if UNITY_UV_STARTS_AT_TOP
  r9.xyzw = vd_asuint(((-(vd_asfloat(r4.yyyy))) * vd_asfloat((VD_COL(unity_MatrixInvVP, 1).xyzw))));
  r9.xyzw = vd_asuint((vd_asfloat((VD_COL(unity_MatrixInvVP, 0).xyzw)) * vd_asfloat(r4.xxxx) + vd_asfloat(r9.xyzw)));
  r8.xyzw = vd_asuint((vd_asfloat((VD_COL(unity_MatrixInvVP, 2).xyzw)) * vd_asfloat(r8.xxxx) + vd_asfloat(r9.xyzw)));
  r8.xyzw = vd_asuint((vd_asfloat(r8.xyzw) + vd_asfloat((VD_COL(unity_MatrixInvVP, 3).xyzw))));
#else
  r9.xyzw = vd_asuint((vd_asfloat(r4.yyyy) * vd_asfloat((VD_COL(vd_MatrixInvVPD3D, 1).xyzw))));
  r9.xyzw = vd_asuint((vd_asfloat((VD_COL(vd_MatrixInvVPD3D, 0).xyzw)) * vd_asfloat(r4.xxxx) + vd_asfloat(r9.xyzw)));
  r8.xyzw = vd_asuint((vd_asfloat((VD_COL(vd_MatrixInvVPD3D, 2).xyzw)) * vd_asfloat(r8.xxxx) + vd_asfloat(r9.xyzw)));
  r8.xyzw = vd_asuint((vd_asfloat(r8.xyzw) + vd_asfloat((VD_COL(vd_MatrixInvVPD3D, 3).xyzw))));
#endif
  r8.xyz = vd_asuint((vd_asfloat(r8.yzx) / vd_asfloat(r8.www)));
  r7.xyzw = vd_asuint(_CameraDepthTexture.SampleBias(sampler_vd_point_clamp, vd_asfloat(r7.zw), vd_asfloat(_GlobalMipBias.x)).xyzw);
#if UNITY_UV_STARTS_AT_TOP
  r9.xyzw = vd_asuint(((-(vd_asfloat(r4.wwww))) * vd_asfloat((VD_COL(unity_MatrixInvVP, 1).xyzw))));
  r4.xyzw = vd_asuint((vd_asfloat((VD_COL(unity_MatrixInvVP, 0).xyzw)) * vd_asfloat(r4.zzzz) + vd_asfloat(r9.xyzw)));
  r4.xyzw = vd_asuint((vd_asfloat((VD_COL(unity_MatrixInvVP, 2).xyzw)) * vd_asfloat(r7.xxxx) + vd_asfloat(r4.xyzw)));
  r4.xyzw = vd_asuint((vd_asfloat(r4.xyzw) + vd_asfloat((VD_COL(unity_MatrixInvVP, 3).xyzw))));
#else
  r7.x = vd_asuint(((-(vd_asfloat(r7.x))) + 1.0));
  r9.xyzw = vd_asuint((vd_asfloat(r4.wwww) * vd_asfloat((VD_COL(vd_MatrixInvVPD3D, 1).xyzw))));
  r4.xyzw = vd_asuint((vd_asfloat((VD_COL(vd_MatrixInvVPD3D, 0).xyzw)) * vd_asfloat(r4.zzzz) + vd_asfloat(r9.xyzw)));
  r4.xyzw = vd_asuint((vd_asfloat((VD_COL(vd_MatrixInvVPD3D, 2).xyzw)) * vd_asfloat(r7.xxxx) + vd_asfloat(r4.xyzw)));
  r4.xyzw = vd_asuint((vd_asfloat(r4.xyzw) + vd_asfloat((VD_COL(vd_MatrixInvVPD3D, 3).xyzw))));
#endif
  r4.xyz = vd_asuint((vd_asfloat(r4.zxy) / vd_asfloat(r4.www)));
  r4.xyz = vd_asuint(((-(vd_asfloat(r6.yzx))) + vd_asfloat(r4.xyz)));
  r6.xyz = vd_asuint(((-(vd_asfloat(r6.xyz))) + vd_asfloat(r8.xyz)));
  r7.xyz = vd_asuint((vd_asfloat(r4.xyz) * vd_asfloat(r6.xyz)));
  r4.xyz = vd_asuint((vd_asfloat(r4.zxy) * vd_asfloat(r6.yzx) + (-(vd_asfloat(r7.xyz)))));
  r1.w = vd_asuint(dot(vd_asfloat(r4.xyz), vd_asfloat(r4.xyz)));
  r1.w = vd_asuint(rsqrt(vd_asfloat(r1.w)));
  r1.w = vd_asuint((vd_asfloat(r1.w) * vd_asfloat(r4.y)));
  r3.xy = vd_asuint((vd_asfloat(r3.zy) + vd_asfloat(v1.xy)));
  r2.zw = vd_asuint(min(vd_asfloat(r1.xy), vd_asfloat(r3.xy)));
  r2.zw = vd_asuint((vd_asfloat(r2.zw) * vd_asfloat(_RTHandleScale.xy)));
  r4.xyzw = vd_asuint(_CameraDepthTexture.SampleBias(sampler_vd_point_clamp, vd_asfloat(r2.zw), vd_asfloat(_GlobalMipBias.x)).xyzw);
#if UNITY_UV_STARTS_AT_TOP
#else
  r4.x = vd_asuint(((-(vd_asfloat(r4.x))) + 1.0));
#endif
  r2.zw = vd_asuint((vd_asfloat(r3.xy) * float2(2.0, 2.0) + float2(-1.0, -1.0)));
#if UNITY_UV_STARTS_AT_TOP
  r6.xyzw = vd_asuint(((-(vd_asfloat(r2.wwww))) * vd_asfloat((VD_COL(unity_MatrixInvVP, 1).xyzw))));
  r6.xyzw = vd_asuint((vd_asfloat((VD_COL(unity_MatrixInvVP, 0).xyzw)) * vd_asfloat(r2.zzzz) + vd_asfloat(r6.xyzw)));
  r4.xyzw = vd_asuint((vd_asfloat((VD_COL(unity_MatrixInvVP, 2).xyzw)) * vd_asfloat(r4.xxxx) + vd_asfloat(r6.xyzw)));
  r4.xyzw = vd_asuint((vd_asfloat(r4.xyzw) + vd_asfloat((VD_COL(unity_MatrixInvVP, 3).xyzw))));
#else
  r6.xyzw = vd_asuint((vd_asfloat(r2.wwww) * vd_asfloat((VD_COL(vd_MatrixInvVPD3D, 1).xyzw))));
  r6.xyzw = vd_asuint((vd_asfloat((VD_COL(vd_MatrixInvVPD3D, 0).xyzw)) * vd_asfloat(r2.zzzz) + vd_asfloat(r6.xyzw)));
  r4.xyzw = vd_asuint((vd_asfloat((VD_COL(vd_MatrixInvVPD3D, 2).xyzw)) * vd_asfloat(r4.xxxx) + vd_asfloat(r6.xyzw)));
  r4.xyzw = vd_asuint((vd_asfloat(r4.xyzw) + vd_asfloat((VD_COL(vd_MatrixInvVPD3D, 3).xyzw))));
#endif
  r4.xyz = vd_asuint((vd_asfloat(r4.yzx) / vd_asfloat(r4.www)));
  r2.zw = vd_asuint((vd_asfloat(r5.xy) + vd_asfloat(r3.xy)));
  r3.xw = vd_asuint(min(vd_asfloat(r1.xy), vd_asfloat(r2.zw)));
  r3.xw = vd_asuint((vd_asfloat(r3.xw) * vd_asfloat(_RTHandleScale.xy)));
  r6.xyzw = vd_asuint(_CameraDepthTexture.SampleBias(sampler_vd_point_clamp, vd_asfloat(r3.xw), vd_asfloat(_GlobalMipBias.x)).xyzw);
#if UNITY_UV_STARTS_AT_TOP
#else
  r6.x = vd_asuint(((-(vd_asfloat(r6.x))) + 1.0));
#endif
  r2.zw = vd_asuint((vd_asfloat(r2.zw) * float2(2.0, 2.0) + float2(-1.0, -1.0)));
#if UNITY_UV_STARTS_AT_TOP
  r7.xyzw = vd_asuint(((-(vd_asfloat(r2.wwww))) * vd_asfloat((VD_COL(unity_MatrixInvVP, 1).xyzw))));
  r7.xyzw = vd_asuint((vd_asfloat((VD_COL(unity_MatrixInvVP, 0).xyzw)) * vd_asfloat(r2.zzzz) + vd_asfloat(r7.xyzw)));
  r6.xyzw = vd_asuint((vd_asfloat((VD_COL(unity_MatrixInvVP, 2).xyzw)) * vd_asfloat(r6.xxxx) + vd_asfloat(r7.xyzw)));
  r6.xyzw = vd_asuint((vd_asfloat(r6.xyzw) + vd_asfloat((VD_COL(unity_MatrixInvVP, 3).xyzw))));
#else
  r7.xyzw = vd_asuint((vd_asfloat(r2.wwww) * vd_asfloat((VD_COL(vd_MatrixInvVPD3D, 1).xyzw))));
  r7.xyzw = vd_asuint((vd_asfloat((VD_COL(vd_MatrixInvVPD3D, 0).xyzw)) * vd_asfloat(r2.zzzz) + vd_asfloat(r7.xyzw)));
  r6.xyzw = vd_asuint((vd_asfloat((VD_COL(vd_MatrixInvVPD3D, 2).xyzw)) * vd_asfloat(r6.xxxx) + vd_asfloat(r7.xyzw)));
  r6.xyzw = vd_asuint((vd_asfloat(r6.xyzw) + vd_asfloat((VD_COL(vd_MatrixInvVPD3D, 3).xyzw))));
#endif
  r6.xyz = vd_asuint((vd_asfloat(r6.yzx) / vd_asfloat(r6.www)));
  r3.z = vd_asuint(v1.x);
  r2.zw = vd_asuint((vd_asfloat(r5.zw) + vd_asfloat(r3.zy)));
  r3.xy = vd_asuint(min(vd_asfloat(r1.xy), vd_asfloat(r2.zw)));
  r3.xy = vd_asuint((vd_asfloat(r3.xy) * vd_asfloat(_RTHandleScale.xy)));
  r3.xyzw = vd_asuint(_CameraDepthTexture.SampleBias(sampler_vd_point_clamp, vd_asfloat(r3.xy), vd_asfloat(_GlobalMipBias.x)).xyzw);
#if UNITY_UV_STARTS_AT_TOP
#else
  r3.x = vd_asuint(((-(vd_asfloat(r3.x))) + 1.0));
#endif
  r2.zw = vd_asuint((vd_asfloat(r2.zw) * float2(2.0, 2.0) + float2(-1.0, -1.0)));
#if UNITY_UV_STARTS_AT_TOP
  r7.xyzw = vd_asuint(((-(vd_asfloat(r2.wwww))) * vd_asfloat((VD_COL(unity_MatrixInvVP, 1).xyzw))));
  r7.xyzw = vd_asuint((vd_asfloat((VD_COL(unity_MatrixInvVP, 0).xyzw)) * vd_asfloat(r2.zzzz) + vd_asfloat(r7.xyzw)));
  r3.xyzw = vd_asuint((vd_asfloat((VD_COL(unity_MatrixInvVP, 2).xyzw)) * vd_asfloat(r3.xxxx) + vd_asfloat(r7.xyzw)));
  r3.xyzw = vd_asuint((vd_asfloat(r3.xyzw) + vd_asfloat((VD_COL(unity_MatrixInvVP, 3).xyzw))));
#else
  r7.xyzw = vd_asuint((vd_asfloat(r2.wwww) * vd_asfloat((VD_COL(vd_MatrixInvVPD3D, 1).xyzw))));
  r7.xyzw = vd_asuint((vd_asfloat((VD_COL(vd_MatrixInvVPD3D, 0).xyzw)) * vd_asfloat(r2.zzzz) + vd_asfloat(r7.xyzw)));
  r3.xyzw = vd_asuint((vd_asfloat((VD_COL(vd_MatrixInvVPD3D, 2).xyzw)) * vd_asfloat(r3.xxxx) + vd_asfloat(r7.xyzw)));
  r3.xyzw = vd_asuint((vd_asfloat(r3.xyzw) + vd_asfloat((VD_COL(vd_MatrixInvVPD3D, 3).xyzw))));
#endif
  r3.xyz = vd_asuint((vd_asfloat(r3.zxy) / vd_asfloat(r3.www)));
  r3.xyz = vd_asuint(((-(vd_asfloat(r4.yzx))) + vd_asfloat(r3.xyz)));
  r4.xyz = vd_asuint(((-(vd_asfloat(r4.xyz))) + vd_asfloat(r6.xyz)));
  r6.xyz = vd_asuint((vd_asfloat(r3.xyz) * vd_asfloat(r4.xyz)));
  r3.xyz = vd_asuint((vd_asfloat(r3.zxy) * vd_asfloat(r4.yzx) + (-(vd_asfloat(r6.xyz)))));
  r2.z = vd_asuint(dot(vd_asfloat(r3.xyz), vd_asfloat(r3.xyz)));
  r2.z = vd_asuint(rsqrt(vd_asfloat(r2.z)));
  r0.y = vd_asuint((vd_asfloat(r2.y) * vd_asfloat(r1.z) + (-(vd_asfloat(r0.y)))));
  r1.z = vd_asuint((vd_asfloat(r3.y) * vd_asfloat(r2.z) + (-(vd_asfloat(r1.w)))));
  r0.y = vd_asuint((vd_asfloat(r0.y) + vd_asfloat(r1.z)));
  r0.y = vd_asuint((vd_asfloat(r0.y) * vd_asfloat(_CavityStrength)));
  r1.z = vd_asuint(saturate(vd_asfloat(r0.y)));
  r1.z = vd_asuint((vd_asfloat(r1.z) * vd_asfloat(_RidgeIntensity)));
  r0.y = vd_asuint(saturate((-(vd_asfloat(r0.y)))));
  r0.y = vd_asuint((vd_asfloat(r0.y) * vd_asfloat(_ValleyIntensity)));
  r1.w = vd_asuint((-(int)(0.5 < vd_asfloat(_DebugMode))));
  if (r1.w != 0u) {
    r1.w = vd_asuint((-(int)(vd_asfloat(_DebugMode) < 1.5)));
    if (r1.w != 0u) {
      o0.xyz = min(vd_asfloat(r0.xxx), float3(1.0, 1.0, 1.0));
      o0.w = vd_asfloat(0x3f800000u);
      return;
    }
    r1.w = vd_asuint((-(int)(vd_asfloat(_DebugMode) < 2.5)));
    if (r1.w != 0u) {
      r0.z = vd_asuint((vd_asfloat(r0.z) * vd_asfloat(_DebugGain)));
      o0.xyz = saturate(vd_asfloat(r0.zzz));
      o0.w = vd_asfloat(0x3f800000u);
      return;
    }
    r0.z = vd_asuint((-(int)(vd_asfloat(_DebugMode) < 3.5)));
    if (r0.z != 0u) {
      r0.z = vd_asuint((vd_asfloat(r0.w) * vd_asfloat(_DebugGain)));
      o0.xyz = saturate(vd_asfloat(r0.zzz));
      o0.w = vd_asfloat(0x3f800000u);
      return;
    }
    r0.z = vd_asuint((-(int)(vd_asfloat(_DebugMode) < 4.5)));
    if (r0.z != 0u) {
#if UNITY_UV_STARTS_AT_TOP
      r0.z = vd_asuint((vd_asfloat(_ZBufferParams.x) * vd_asfloat(r2.x) + vd_asfloat(_ZBufferParams.y)));
#else
      r0.z = vd_asuint((vd_asfloat(vd_ZBufferParamsD3D.x) * vd_asfloat(r2.x) + vd_asfloat(vd_ZBufferParamsD3D.y)));
#endif
      o0.xyz = (float3(1.0, 1.0, 1.0) / vd_asfloat(r0.zzz));
      o0.w = vd_asfloat(0x3f800000u);
      return;
    }
    r0.z = vd_asuint((-(int)(vd_asfloat(_DebugMode) < 5.5)));
    if (r0.z != 0u) {
      r0.zw = vd_asuint((vd_asfloat(v1.xy) * float2(2.0, 2.0) + float2(-1.0, -1.0)));
#if UNITY_UV_STARTS_AT_TOP
      r3.xyzw = vd_asuint(((-(vd_asfloat(r0.wwww))) * vd_asfloat((VD_COL(unity_MatrixInvVP, 1).xyzw))));
      r3.xyzw = vd_asuint((vd_asfloat((VD_COL(unity_MatrixInvVP, 0).xyzw)) * vd_asfloat(r0.zzzz) + vd_asfloat(r3.xyzw)));
      r2.xyzw = vd_asuint((vd_asfloat((VD_COL(unity_MatrixInvVP, 2).xyzw)) * vd_asfloat(r2.xxxx) + vd_asfloat(r3.xyzw)));
      r2.xyzw = vd_asuint((vd_asfloat(r2.xyzw) + vd_asfloat((VD_COL(unity_MatrixInvVP, 3).xyzw))));
#else
      r3.xyzw = vd_asuint((vd_asfloat(r0.wwww) * vd_asfloat((VD_COL(vd_MatrixInvVPD3D, 1).xyzw))));
      r3.xyzw = vd_asuint((vd_asfloat((VD_COL(vd_MatrixInvVPD3D, 0).xyzw)) * vd_asfloat(r0.zzzz) + vd_asfloat(r3.xyzw)));
      r2.xyzw = vd_asuint((vd_asfloat((VD_COL(vd_MatrixInvVPD3D, 2).xyzw)) * vd_asfloat(r2.xxxx) + vd_asfloat(r3.xyzw)));
      r2.xyzw = vd_asuint((vd_asfloat(r2.xyzw) + vd_asfloat((VD_COL(vd_MatrixInvVPD3D, 3).xyzw))));
#endif
      r2.xyz = vd_asuint((vd_asfloat(r2.yzx) / vd_asfloat(r2.www)));
      r3.xyzw = vd_asuint((vd_asfloat(r5.xyzw) + vd_asfloat(v1.xyxy)));
      r4.xyzw = vd_asuint(min(vd_asfloat(r1.xyxy), vd_asfloat(r3.xyzw)));
      r4.xyzw = vd_asuint((vd_asfloat(r4.xyzw) * vd_asfloat(_RTHandleScale.xyxy)));
      r5.xyzw = vd_asuint(_CameraDepthTexture.SampleBias(sampler_vd_point_clamp, vd_asfloat(r4.xy), vd_asfloat(_GlobalMipBias.x)).xyzw);
#if UNITY_UV_STARTS_AT_TOP
#else
      r5.x = vd_asuint(((-(vd_asfloat(r5.x))) + 1.0));
#endif
      r3.xyzw = vd_asuint((vd_asfloat(r3.xyzw) * float4(2.0, 2.0, 2.0, 2.0) + float4(-1.0, -1.0, -1.0, -1.0)));
#if UNITY_UV_STARTS_AT_TOP
      r6.xyzw = vd_asuint(((-(vd_asfloat(r3.yyyy))) * vd_asfloat((VD_COL(unity_MatrixInvVP, 1).xyzw))));
      r6.xyzw = vd_asuint((vd_asfloat((VD_COL(unity_MatrixInvVP, 0).xyzw)) * vd_asfloat(r3.xxxx) + vd_asfloat(r6.xyzw)));
      r5.xyzw = vd_asuint((vd_asfloat((VD_COL(unity_MatrixInvVP, 2).xyzw)) * vd_asfloat(r5.xxxx) + vd_asfloat(r6.xyzw)));
      r5.xyzw = vd_asuint((vd_asfloat(r5.xyzw) + vd_asfloat((VD_COL(unity_MatrixInvVP, 3).xyzw))));
#else
      r6.xyzw = vd_asuint((vd_asfloat(r3.yyyy) * vd_asfloat((VD_COL(vd_MatrixInvVPD3D, 1).xyzw))));
      r6.xyzw = vd_asuint((vd_asfloat((VD_COL(vd_MatrixInvVPD3D, 0).xyzw)) * vd_asfloat(r3.xxxx) + vd_asfloat(r6.xyzw)));
      r5.xyzw = vd_asuint((vd_asfloat((VD_COL(vd_MatrixInvVPD3D, 2).xyzw)) * vd_asfloat(r5.xxxx) + vd_asfloat(r6.xyzw)));
      r5.xyzw = vd_asuint((vd_asfloat(r5.xyzw) + vd_asfloat((VD_COL(vd_MatrixInvVPD3D, 3).xyzw))));
#endif
      r1.xyw = vd_asuint((vd_asfloat(r5.yzx) / vd_asfloat(r5.www)));
      r4.xyzw = vd_asuint(_CameraDepthTexture.SampleBias(sampler_vd_point_clamp, vd_asfloat(r4.zw), vd_asfloat(_GlobalMipBias.x)).xyzw);
#if UNITY_UV_STARTS_AT_TOP
      r5.xyzw = vd_asuint(((-(vd_asfloat(r3.wwww))) * vd_asfloat((VD_COL(unity_MatrixInvVP, 1).xyzw))));
      r3.xyzw = vd_asuint((vd_asfloat((VD_COL(unity_MatrixInvVP, 0).xyzw)) * vd_asfloat(r3.zzzz) + vd_asfloat(r5.xyzw)));
      r3.xyzw = vd_asuint((vd_asfloat((VD_COL(unity_MatrixInvVP, 2).xyzw)) * vd_asfloat(r4.xxxx) + vd_asfloat(r3.xyzw)));
      r3.xyzw = vd_asuint((vd_asfloat(r3.xyzw) + vd_asfloat((VD_COL(unity_MatrixInvVP, 3).xyzw))));
#else
      r4.x = vd_asuint(((-(vd_asfloat(r4.x))) + 1.0));
      r5.xyzw = vd_asuint((vd_asfloat(r3.wwww) * vd_asfloat((VD_COL(vd_MatrixInvVPD3D, 1).xyzw))));
      r3.xyzw = vd_asuint((vd_asfloat((VD_COL(vd_MatrixInvVPD3D, 0).xyzw)) * vd_asfloat(r3.zzzz) + vd_asfloat(r5.xyzw)));
      r3.xyzw = vd_asuint((vd_asfloat((VD_COL(vd_MatrixInvVPD3D, 2).xyzw)) * vd_asfloat(r4.xxxx) + vd_asfloat(r3.xyzw)));
      r3.xyzw = vd_asuint((vd_asfloat(r3.xyzw) + vd_asfloat((VD_COL(vd_MatrixInvVPD3D, 3).xyzw))));
#endif
      r3.xyz = vd_asuint((vd_asfloat(r3.zxy) / vd_asfloat(r3.www)));
      r3.xyz = vd_asuint(((-(vd_asfloat(r2.yzx))) + vd_asfloat(r3.xyz)));
      r1.xyw = vd_asuint(((-(vd_asfloat(r2.xyz))) + vd_asfloat(r1.xyw)));
      r2.xyz = vd_asuint((vd_asfloat(r1.xyw) * vd_asfloat(r3.xyz)));
      r1.xyw = vd_asuint((vd_asfloat(r3.zxy) * vd_asfloat(r1.ywx) + (-(vd_asfloat(r2.xyz)))));
      r0.z = vd_asuint(dot(vd_asfloat(r1.xyw), vd_asfloat(r1.xyw)));
      r0.z = vd_asuint(rsqrt(vd_asfloat(r0.z)));
      r1.xyw = vd_asuint((vd_asfloat(r0.zzz) * vd_asfloat(r1.xyw)));
      o0.xyz = (vd_asfloat(r1.xyw) * float3(0.5, 0.5, 0.5) + float3(0.5, 0.5, 0.5));
      o0.w = vd_asfloat(0x3f800000u);
      return;
    }
    o0.x = saturate(vd_asfloat(r1.z));
    o0.z = saturate(vd_asfloat(r0.y));
    o0.yw = vd_asfloat(uint2(0x00000000u, 0x3f800000u));
    return;
  }
  r2.xyzw = vd_asuint(_BlitTexture.SampleBias(sampler_vd_linear_clamp, vd_asfloat(v1.xy), vd_asfloat(_GlobalMipBias.x)).xyzw);
  r0.y = vd_asuint(saturate(vd_asfloat(r0.y)));
  r0.y = vd_asuint((vd_asfloat(r0.y) * vd_asfloat(_ValleyColor.w)));
  r1.xyw = vd_asuint(((-(vd_asfloat(r2.xyz))) + vd_asfloat(_ValleyColor.xyz)));
  r0.yzw = vd_asuint((vd_asfloat(r0.yyy) * vd_asfloat(r1.xyw) + vd_asfloat(r2.xyz)));
  r1.z = vd_asuint(saturate(vd_asfloat(r1.z)));
  r1.x = vd_asuint((vd_asfloat(r1.z) * vd_asfloat(_RidgeColor.w)));
  r1.yzw = vd_asuint(((-(vd_asfloat(r0.yzw))) + vd_asfloat(_RidgeColor.xyz)));
  r0.yzw = vd_asuint((vd_asfloat(r1.xxx) * vd_asfloat(r1.yzw) + vd_asfloat(r0.yzw)));
  r0.x = vd_asuint(min(vd_asfloat(r0.x), 1.0));
  r0.x = vd_asuint((vd_asfloat(r0.x) * vd_asfloat(_OutlineOpacity)));
  r1.xyz = vd_asuint(((-(vd_asfloat(r0.yzw))) + vd_asfloat(_OutlineColor.xyz)));
  o0.xyz = (vd_asfloat(r0.xxx) * vd_asfloat(r1.xyz) + vd_asfloat(r0.yzw));
  o0.w = vd_asfloat(r2.w);
  return;
}
