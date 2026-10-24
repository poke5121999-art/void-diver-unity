// ArtTeam/VFX/VFX_Master_typeA | pass 1 "DepthOnly" | progFragment | keywords: _ALPHATEST_ON _PLANESYSTEMCULLINGTYPE_ALWAYS _USE_SECONDARYCOLOR | blob 167
void frag(
  float4 v0 : SV_POSITION0,
  float4 v1 : VDREG1,
  float4 v2 : VDREG2,
  float4 v3 : VDREG3,
  float4 v4 : VDREG4,
  float4 v5 : VDREG5,
  float4 v6 : VDREG6,
  out float4 o0 : SV_TARGET0)
{
#if UNITY_UV_STARTS_AT_TOP
  uint4 r0, r1, r2, r3, r4, r5, r6, r7;
  r0 = 0u; r1 = 0u; r2 = 0u; r3 = 0u; r4 = 0u; r5 = 0u; r6 = 0u; r7 = 0u;
#else
  float4 vd_ZBufferParamsD3D = float4(-_ZBufferParams.x, _ZBufferParams.x + _ZBufferParams.y, -_ZBufferParams.z, _ZBufferParams.z + _ZBufferParams.w);
  float4x4 vd_MatrixInvVPD3D = mul(unity_MatrixInvVP, float4x4(1, 0, 0, 0, 0, 1, 0, 0, 0, 0, -2, 1, 0, 0, 0, 1));
  uint4 r0, r1, r2, r3, r4, r5, r6, r7, r8;
  r0 = 0u; r1 = 0u; r2 = 0u; r3 = 0u; r4 = 0u; r5 = 0u; r6 = 0u; r7 = 0u; r8 = 0u;
#endif

#if UNITY_UV_STARTS_AT_TOP
#else
  r8.xyzw = vd_asuint(v0.xyzw);
  r8.z = vd_asuint(((-(vd_asfloat(v0.z))) + 1.0));
#endif
  r0.x = vd_asuint(dot(vd_asfloat(v6.xyz), vd_asfloat(v6.xyz)));
  r0.x = vd_asuint(sqrt(vd_asfloat(r0.x)));
  r0.x = vd_asuint((1.0 / vd_asfloat(r0.x)));
  r0.xyz = vd_asuint((vd_asfloat(r0.xxx) * vd_asfloat(v6.xyz)));
  r1.xy = vd_asuint((-(int2)(vd_asfloat(unity_OrthoParams.ww) == float2(0.0, 1.0))));
  r2.xyz = vd_asuint(((-(vd_asfloat(v5.xyz))) + vd_asfloat(_WorldSpaceCameraPos.xyz)));
  r0.w = vd_asuint(dot(vd_asfloat(r2.xyz), vd_asfloat(r2.xyz)));
  r0.w = vd_asuint(rsqrt(vd_asfloat(r0.w)));
  r2.xyz = vd_asuint((vd_asfloat(r0.www) * vd_asfloat(r2.xyz)));
  r3.x = vd_asuint((VD_COL(unity_MatrixV, 0).z));
  r3.y = vd_asuint((VD_COL(unity_MatrixV, 1).z));
  r3.z = vd_asuint((VD_COL(unity_MatrixV, 2).z));
  r1.xzw = ((r1.xxx != 0u) ? r2.xyz : r3.xyz);
  r0.w = vd_asuint((vd_asfloat(v5.y) * vd_asfloat((VD_COL(unity_MatrixVP, 1).w))));
  r0.w = vd_asuint((vd_asfloat((VD_COL(unity_MatrixVP, 0).w)) * vd_asfloat(v5.x) + vd_asfloat(r0.w)));
  r0.w = vd_asuint((vd_asfloat((VD_COL(unity_MatrixVP, 2).w)) * vd_asfloat(v5.z) + vd_asfloat(r0.w)));
  r0.w = vd_asuint((vd_asfloat(r0.w) + vd_asfloat((VD_COL(unity_MatrixVP, 3).w))));
#if UNITY_UV_STARTS_AT_TOP
  r2.x = vd_asuint((-(int)(vd_asfloat(_ProjectionParams.x) < 0.0)));
  r2.y = vd_asuint(((-(vd_asfloat(v0.y))) + vd_asfloat(_ScaledScreenParams.y)));
  r2.y = ((r2.x != 0u) ? r2.y : vd_asuint(v0.y));
  r2.x = vd_asuint(v0.x);
#else
  r2.x = vd_asuint((-(int)(0.0 < vd_asfloat(_ProjectionParams.x))));
  r2.y = vd_asuint(((-(vd_asfloat(r8.y))) + vd_asfloat(_ScaledScreenParams.y)));
  r2.y = ((r2.x != 0u) ? r2.y : r8.y);
  r2.x = r8.x;
#endif
  r2.xy = vd_asuint((vd_asfloat(r2.xy) / vd_asfloat(_ScaledScreenParams.xy)));
  r3.xy = vd_asuint((vd_asfloat(v1.xy) + float2(-0.5, -0.5)));
  r3.z = vd_asuint(dot(vd_asfloat(r3.xy), vd_asfloat(r3.xy)));
  r3.z = vd_asuint(sqrt(vd_asfloat(r3.z)));
  r4.x = vd_asuint((vd_asfloat(r3.z) + vd_asfloat(r3.z)));
  r3.z = vd_asuint(min(abs(vd_asfloat(r3.y)), abs(vd_asfloat(r3.x))));
  r3.w = vd_asuint(max(abs(vd_asfloat(r3.y)), abs(vd_asfloat(r3.x))));
  r3.w = vd_asuint((1.0 / vd_asfloat(r3.w)));
  r3.z = vd_asuint((vd_asfloat(r3.w) * vd_asfloat(r3.z)));
  r3.w = vd_asuint((vd_asfloat(r3.z) * vd_asfloat(r3.z)));
  r4.z = vd_asuint((vd_asfloat(r3.w) * 0.0208350997 + -0.0851330012));
  r4.z = vd_asuint((vd_asfloat(r3.w) * vd_asfloat(r4.z) + 0.180141002));
  r4.z = vd_asuint((vd_asfloat(r3.w) * vd_asfloat(r4.z) + -0.330299497));
  r3.w = vd_asuint((vd_asfloat(r3.w) * vd_asfloat(r4.z) + 0.999866009));
  r4.z = vd_asuint((vd_asfloat(r3.w) * vd_asfloat(r3.z)));
  r4.w = vd_asuint((-(int)(abs(vd_asfloat(r3.y)) < abs(vd_asfloat(r3.x)))));
  r4.z = vd_asuint((vd_asfloat(r4.z) * -2.0 + 1.57079637));
  r4.z = (r4.w & r4.z);
  r3.z = vd_asuint((vd_asfloat(r3.z) * vd_asfloat(r3.w) + vd_asfloat(r4.z)));
  r3.w = vd_asuint((-(int)(vd_asfloat(r3.y) < (-(vd_asfloat(r3.y))))));
  r3.w = (r3.w & 0xc0490fdbu);
  r3.z = vd_asuint((vd_asfloat(r3.w) + vd_asfloat(r3.z)));
  r3.w = vd_asuint(min(vd_asfloat(r3.y), vd_asfloat(r3.x)));
  r3.x = vd_asuint(max(vd_asfloat(r3.y), vd_asfloat(r3.x)));
  r3.y = vd_asuint((-(int)(vd_asfloat(r3.w) < (-(vd_asfloat(r3.w))))));
  r3.x = vd_asuint((-(int)(vd_asfloat(r3.x) >= (-(vd_asfloat(r3.x))))));
  r3.x = (r3.x & r3.y);
  r3.x = vd_asuint(((r3.x != 0u) ? (-(vd_asfloat(r3.z))) : vd_asfloat(r3.z)));
  r4.y = vd_asuint((vd_asfloat(r3.x) * 0.159235656));
  r3.xy = vd_asuint((vd_asfloat(r4.xy) + (-(vd_asfloat(v1.xy)))));
  r3.xy = vd_asuint((vd_asfloat((_Main_Radial).xx) * vd_asfloat(r3.xy) + vd_asfloat(v1.xy)));
  r3.zw = vd_asuint(((-(vd_asfloat(v2.zw))) + float2(1.0, 1.0)));
  r3.zw = vd_asuint((vd_asfloat((_Use_Main_TilingOffset).xx) * vd_asfloat(r3.zw) + vd_asfloat(v2.zw)));
  r4.xyz = vd_asuint((vd_asfloat(float3(_Main_Offset_UV1xy.x, _Main_Offset_UV1xy.y, _Dissolve_Offset_UV2y)) * vd_asfloat(_TimeParameters.xxx) + float3(1.0, 1.0, 1.0)));
  r4.xy = vd_asuint((vd_asfloat(r4.xy) * vd_asfloat(v2.xy)));
  r5.xy = vd_asuint((vd_asfloat(_Main_Offset_UV1xy.xy) * vd_asfloat(_TimeParameters.xx) + (-(vd_asfloat(r4.xy)))));
  r4.xy = vd_asuint((vd_asfloat((_Use_Main_TilingOffset).xx) * vd_asfloat(r5.xy) + vd_asfloat(r4.xy)));
  r3.xy = vd_asuint((vd_asfloat(r3.xy) * vd_asfloat(r3.zw) + vd_asfloat(r4.xy)));
  r3.zw = vd_asuint((float2(1.0, 1.0) / vd_asfloat(_ScreenParams.xy)));
  r3.zw = vd_asuint((vd_asfloat(r3.zw) * vd_asfloat((_Pixelation).xx)));
  r3.xy = vd_asuint((vd_asfloat(r3.xy) / vd_asfloat(r3.zw)));
  r3.xy = vd_asuint(floor(vd_asfloat(r3.xy)));
  r4.xy = vd_asuint((vd_asfloat(r3.zw) * vd_asfloat(r3.xy)));
  r4.z = vd_asuint((vd_asfloat(r4.z) * vd_asfloat(v3.y)));
  r4.w = vd_asuint((-(int)(abs(vd_asfloat(_Dissolve_Offset_UV2y)) >= 0.00100000005)));
  r4.w = (r4.w & 0x3f800000u);
  r5.x = vd_asuint((vd_asfloat(_Dissolve_Offset_UV2y) * vd_asfloat(_TimeParameters.x) + (-(vd_asfloat(r4.z)))));
  r4.z = vd_asuint((vd_asfloat(r4.w) * vd_asfloat(r5.x) + vd_asfloat(r4.z)));
  r4.xy = vd_asuint((vd_asfloat(r4.zz) * vd_asfloat(_DissolveDirection.xy) + vd_asfloat(r4.xy)));
  r4.z = vd_asuint((vd_asfloat(_Dissolve_Rotate) * 0.0174444448));
  r4.xy = vd_asuint((vd_asfloat(r4.xy) + float2(-0.5, -0.5)));
  r5.x = vd_asuint(sin(vd_asfloat(r4.z)));
  r6.x = vd_asuint(cos(vd_asfloat(r4.z)));
  r7.x = vd_asuint((-(vd_asfloat(r5.x))));
  r7.y = r6.x;
  r6.y = vd_asuint(dot(vd_asfloat(r4.xy), vd_asfloat(r7.xy)));
  r7.z = r5.x;
  r6.x = vd_asuint(dot(vd_asfloat(r4.xy), vd_asfloat(r7.yz)));
  r4.xy = vd_asuint((vd_asfloat(r6.xy) + float2(0.5, 0.5)));
  r4.xy = vd_asuint((vd_asfloat(r4.xy) * vd_asfloat(_DissolveTex_ST.xy) + vd_asfloat(_DissolveTex_ST.zw)));
  r4.xyzw = vd_asuint(_DissolveTex.SampleBias(sampler_DissolveTex, vd_asfloat(r4.xy), vd_asfloat(_GlobalMipBias.x)).xyzw);
  r4.y = vd_asuint(((-(vd_asfloat(r4.x))) + vd_asfloat(r4.y)));
  r4.x = vd_asuint((vd_asfloat(_Use_Dissolve_Green) * vd_asfloat(r4.y) + vd_asfloat(r4.x)));
  r4.y = vd_asuint(((-(vd_asfloat(v3.w))) + vd_asfloat(_Dissolve_Progress_UV2w)));
  r4.y = vd_asuint((vd_asfloat(_Use_Dissolve_Progress_Property) * vd_asfloat(r4.y) + vd_asfloat(v3.w)));
  r4.z = vd_asuint((vd_asfloat(v4.w) * 2.0 + -1.0));
  r4.z = vd_asuint(((-(vd_asfloat(r4.y))) + vd_asfloat(r4.z)));
  r4.y = vd_asuint((vd_asfloat(_Use_Dissolve_Alpha) * vd_asfloat(r4.z) + vd_asfloat(r4.y)));
  r4.y = vd_asuint(max(vd_asfloat(r4.y), -1.0));
  r4.y = vd_asuint(min(vd_asfloat(r4.y), 1.0));
  r4.x = vd_asuint((vd_asfloat(r4.y) + vd_asfloat(r4.x)));
  r4.y = vd_asuint(((-(vd_asfloat(_Dissolve_Sharp))) + 1.0));
  r4.x = vd_asuint(((-(vd_asfloat(_Dissolve_Sharp))) * 0.5 + vd_asfloat(r4.x)));
  r4.y = vd_asuint((1.0 / vd_asfloat(r4.y)));
  r4.x = vd_asuint(saturate((vd_asfloat(r4.y) * vd_asfloat(r4.x))));
  r4.y = vd_asuint((vd_asfloat(r4.x) * -2.0 + 3.0));
  r4.x = vd_asuint((vd_asfloat(r4.x) * vd_asfloat(r4.x)));
  r4.x = vd_asuint((vd_asfloat(r4.x) * vd_asfloat(r4.y)));
  r4.x = vd_asuint(log2(vd_asfloat(r4.x)));
  r4.x = vd_asuint((vd_asfloat(r4.x) * vd_asfloat(_DissovePower)));
  r4.x = vd_asuint(exp2(vd_asfloat(r4.x)));
  r4.x = vd_asuint(min(vd_asfloat(r4.x), 1.0));
  r4.y = vd_asuint((vd_asfloat(_Main_Rotate) * 0.0174444448));
  r3.xy = vd_asuint((vd_asfloat(r3.xy) * vd_asfloat(r3.zw) + float2(-0.5, -0.5)));
  r5.x = vd_asuint(sin(vd_asfloat(r4.y)));
  r6.x = vd_asuint(cos(vd_asfloat(r4.y)));
  r7.x = vd_asuint((-(vd_asfloat(r5.x))));
  r7.y = r6.x;
  r6.y = vd_asuint(dot(vd_asfloat(r3.xy), vd_asfloat(r7.xy)));
  r7.z = r5.x;
  r6.x = vd_asuint(dot(vd_asfloat(r3.xy), vd_asfloat(r7.yz)));
  r3.xy = vd_asuint((vd_asfloat(r6.xy) + float2(0.5, 0.5)));
  r3.xy = vd_asuint((vd_asfloat(r3.xy) * vd_asfloat(_MainTex_ST.xy) + vd_asfloat(_MainTex_ST.zw)));
  r3.xyzw = vd_asuint(_MainTex.SampleBias(sampler_MainTex, vd_asfloat(r3.xy), vd_asfloat(_GlobalMipBias.x)).xyzw);
  r3.x = vd_asuint(((-(vd_asfloat(v4.w))) + 1.0));
  r3.x = vd_asuint((vd_asfloat(_Use_Dissolve_Alpha) * vd_asfloat(r3.x) + vd_asfloat(v4.w)));
  r3.x = vd_asuint((vd_asfloat(r3.x) * vd_asfloat(r3.w)));
  r3.x = vd_asuint((vd_asfloat(r3.x) * vd_asfloat(r4.x)));
  r3.y = vd_asuint((vd_asfloat(_FresnelMask_Threshold) + 1.0));
  r3.y = vd_asuint((vd_asfloat(r3.y) * vd_asfloat(_FresnelMask_Offset) + (-(vd_asfloat(_FresnelMask_Threshold)))));
  r0.x = vd_asuint(dot(vd_asfloat(r0.xyz), vd_asfloat(r1.xzw)));
  r0.x = vd_asuint(min(abs(vd_asfloat(r0.x)), 1.0));
  r0.y = vd_asuint((vd_asfloat(r0.x) * -2.0 + 1.0));
  r0.x = vd_asuint((vd_asfloat(_Fresnel_Reverse) * vd_asfloat(r0.y) + vd_asfloat(r0.x)));
  r0.y = vd_asuint(((-(vd_asfloat(r3.y))) + vd_asfloat(_FresnelMask_Offset)));
  r0.x = vd_asuint(((-(vd_asfloat(r3.y))) + vd_asfloat(r0.x)));
  r0.y = vd_asuint((1.0 / vd_asfloat(r0.y)));
  r0.x = vd_asuint(saturate((vd_asfloat(r0.y) * vd_asfloat(r0.x))));
  r0.y = vd_asuint((vd_asfloat(r0.x) * -2.0 + 3.0));
  r0.x = vd_asuint((vd_asfloat(r0.x) * vd_asfloat(r0.x)));
  r0.x = vd_asuint((vd_asfloat(r0.x) * vd_asfloat(r0.y)));
  r0.x = vd_asuint((vd_asfloat(r0.x) * vd_asfloat(r3.x)));
  r0.y = vd_asuint((vd_asfloat(_Depth_Sharp) + -0.5));
  if (r1.y != 0u) {
    r1.xy = vd_asuint(((-(vd_asfloat(_CameraDepthTexture_TexelSize.xy))) * float2(0.5, 0.5) + float2(1.0, 1.0)));
    r2.z = vd_asuint(((-(vd_asfloat(r2.y))) + 1.0));
    r1.xy = vd_asuint(min(vd_asfloat(r1.xy), vd_asfloat(r2.xz)));
    r1.xy = vd_asuint((vd_asfloat(r1.xy) * vd_asfloat(_RTHandleScale.xy)));
    r1.xyzw = vd_asuint(_CameraDepthTexture.SampleBias(sampler_vd_point_clamp, vd_asfloat(r1.xy), vd_asfloat(_GlobalMipBias.x)).xyzw);
#if UNITY_UV_STARTS_AT_TOP
#else
    r1.x = vd_asuint(((-(vd_asfloat(r1.x))) + 1.0));
#endif
    r1.yz = vd_asuint((vd_asfloat(r2.xz) * float2(2.0, 2.0) + float2(-1.0, -1.0)));
#if UNITY_UV_STARTS_AT_TOP
    r3.xyzw = vd_asuint(((-(vd_asfloat(r1.zzzz))) * vd_asfloat((VD_COL(unity_MatrixInvVP, 1).xyzw))));
    r3.xyzw = vd_asuint((vd_asfloat((VD_COL(unity_MatrixInvVP, 0).xyzw)) * vd_asfloat(r1.yyyy) + vd_asfloat(r3.xyzw)));
    r1.xyzw = vd_asuint((vd_asfloat((VD_COL(unity_MatrixInvVP, 2).xyzw)) * vd_asfloat(r1.xxxx) + vd_asfloat(r3.xyzw)));
    r1.xyzw = vd_asuint((vd_asfloat(r1.xyzw) + vd_asfloat((VD_COL(unity_MatrixInvVP, 3).xyzw))));
#else
    r3.xyzw = vd_asuint((vd_asfloat(r1.zzzz) * vd_asfloat((VD_COL(vd_MatrixInvVPD3D, 1).xyzw))));
    r3.xyzw = vd_asuint((vd_asfloat((VD_COL(vd_MatrixInvVPD3D, 0).xyzw)) * vd_asfloat(r1.yyyy) + vd_asfloat(r3.xyzw)));
    r1.xyzw = vd_asuint((vd_asfloat((VD_COL(vd_MatrixInvVPD3D, 2).xyzw)) * vd_asfloat(r1.xxxx) + vd_asfloat(r3.xyzw)));
    r1.xyzw = vd_asuint((vd_asfloat(r1.xyzw) + vd_asfloat((VD_COL(vd_MatrixInvVPD3D, 3).xyzw))));
#endif
    r1.xyz = vd_asuint((vd_asfloat(r1.xyz) / vd_asfloat(r1.www)));
    r0.z = vd_asuint((vd_asfloat(r1.y) * vd_asfloat((VD_COL(unity_MatrixV, 1).z))));
    r0.z = vd_asuint((vd_asfloat((VD_COL(unity_MatrixV, 0).z)) * vd_asfloat(r1.x) + vd_asfloat(r0.z)));
    r0.z = vd_asuint((vd_asfloat((VD_COL(unity_MatrixV, 2).z)) * vd_asfloat(r1.z) + vd_asfloat(r0.z)));
    r0.z = vd_asuint((vd_asfloat(r0.z) + vd_asfloat((VD_COL(unity_MatrixV, 3).z))));
    r0.z = vd_asuint(abs(vd_asfloat(r0.z)));
  } else {
    r1.xy = vd_asuint(((-(vd_asfloat(_CameraDepthTexture_TexelSize.xy))) * float2(0.5, 0.5) + float2(1.0, 1.0)));
    r2.w = vd_asuint(((-(vd_asfloat(r2.y))) + 1.0));
    r1.xy = vd_asuint(min(vd_asfloat(r1.xy), vd_asfloat(r2.xw)));
    r1.xy = vd_asuint((vd_asfloat(r1.xy) * vd_asfloat(_RTHandleScale.xy)));
    r1.xyzw = vd_asuint(_CameraDepthTexture.SampleBias(sampler_vd_point_clamp, vd_asfloat(r1.xy), vd_asfloat(_GlobalMipBias.x)).xyzw);
#if UNITY_UV_STARTS_AT_TOP
    r1.x = vd_asuint((vd_asfloat(_ZBufferParams.z) * vd_asfloat(r1.x) + vd_asfloat(_ZBufferParams.w)));
#else
    r1.x = vd_asuint(((-(vd_asfloat(r1.x))) + 1.0));
    r1.x = vd_asuint((vd_asfloat(vd_ZBufferParamsD3D.z) * vd_asfloat(r1.x) + vd_asfloat(vd_ZBufferParamsD3D.w)));
#endif
    r0.z = vd_asuint((1.0 / vd_asfloat(r1.x)));
  }
  r0.w = vd_asuint((vd_asfloat(r0.w) + vd_asfloat(_Depth_Offset)));
  r0.z = vd_asuint(((-(vd_asfloat(r0.w))) + vd_asfloat(r0.z)));
  r0.z = vd_asuint(saturate(((-(vd_asfloat(r0.z))) + 1.0)));
  r0.w = vd_asuint((vd_asfloat(r0.z) * -2.0 + 1.0));
  r0.z = vd_asuint((vd_asfloat(_Depth_Reverse) * vd_asfloat(r0.w) + vd_asfloat(r0.z)));
  r0.w = vd_asuint((vd_asfloat(r0.y) * -2.0 + 1.0));
  r0.y = vd_asuint(((-(vd_asfloat(r0.y))) + vd_asfloat(r0.z)));
  r0.z = vd_asuint((1.0 / vd_asfloat(r0.w)));
  r0.y = vd_asuint(saturate((vd_asfloat(r0.z) * vd_asfloat(r0.y))));
  r0.z = vd_asuint((vd_asfloat(r0.y) * -2.0 + 3.0));
  r0.y = vd_asuint((vd_asfloat(r0.y) * vd_asfloat(r0.y)));
  r0.y = vd_asuint((vd_asfloat(r0.y) * vd_asfloat(r0.z)));
  r0.y = vd_asuint(min(vd_asfloat(r0.y), 1.0));
  r0.y = vd_asuint((vd_asfloat(r0.y) + -1.0));
  r0.y = vd_asuint((vd_asfloat(_Depth_Use) * vd_asfloat(r0.y) + 1.0));
  r0.x = vd_asuint((vd_asfloat(r0.y) * vd_asfloat(r0.x)));
  r0.x = vd_asuint(saturate((vd_asfloat(r0.x) * vd_asfloat(_Main_Alpha))));
  r0.x = vd_asuint((vd_asfloat(r0.x) + (-(vd_asfloat(_AlphaThreshold)))));
  r0.x = vd_asuint((-(int)(vd_asfloat(r0.x) < 0.0)));
  if (r0.x != 0u) discard;
#if UNITY_UV_STARTS_AT_TOP
  o0.xyzw = vd_asfloat(vd_asuint(v0.zzzz));
#else
  o0.xyzw = vd_asfloat(r8.zzzz);
#endif
  return;
}
