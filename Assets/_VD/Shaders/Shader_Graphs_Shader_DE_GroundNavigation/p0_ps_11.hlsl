// Shader Graphs/Shader_DE_GroundNavigation | pass 0 "Universal Forward" | progFragment | keywords: _CLUSTER_LIGHT_LOOP _DBUFFER_MRT3 _LIGHT_LAYERS | blob 11
void frag(
  float4 v0 : SV_POSITION0,
  float4 v1 : VDREG1,
  float4 v2 : VDREG2,
  float4 v3 : VDREG3,
  float4 v4 : VDREG4,
  float4 v5 : VDREG5,
  float4 v6 : VDREG6,
  float4 v7 : VDREG7,
  out float4 o0 : SV_Target0)
{
#if UNITY_UV_STARTS_AT_TOP
#else
  float4 vd_ZBufferParamsD3D = float4(-_ZBufferParams.x, _ZBufferParams.x + _ZBufferParams.y, -_ZBufferParams.z, _ZBufferParams.z + _ZBufferParams.w);
  float4x4 vd_MatrixInvVPD3D = mul(unity_MatrixInvVP, float4x4(1, 0, 0, 0, 0, 1, 0, 0, 0, 0, -2, 1, 0, 0, 0, 1));
#endif
  uint4 r0, r1, r2, r3, r4, r5, r6, r7, r8, r9, r10, r11, r12, r13;
  r0 = 0u; r1 = 0u; r2 = 0u; r3 = 0u; r4 = 0u; r5 = 0u; r6 = 0u; r7 = 0u; r8 = 0u; r9 = 0u; r10 = 0u; r11 = 0u; r12 = 0u; r13 = 0u;

  r0.x = vd_asuint((vd_asfloat(v6.y) * vd_asfloat((VD_COL(unity_MatrixVP, 1).w))));
  r0.x = vd_asuint((vd_asfloat((VD_COL(unity_MatrixVP, 0).w)) * vd_asfloat(v6.x) + vd_asfloat(r0.x)));
  r0.x = vd_asuint((vd_asfloat((VD_COL(unity_MatrixVP, 2).w)) * vd_asfloat(v6.z) + vd_asfloat(r0.x)));
  r0.x = vd_asuint((vd_asfloat(r0.x) + vd_asfloat((VD_COL(unity_MatrixVP, 3).w))));
#if UNITY_UV_STARTS_AT_TOP
  r0.y = vd_asuint((-(int)(vd_asfloat(_ProjectionParams.x) < 0.0)));
#else
  r0.y = vd_asuint((-(int)(0.0 < vd_asfloat(_ProjectionParams.x))));
#endif
  r0.z = vd_asuint(((-(vd_asfloat(v0.y))) + vd_asfloat(_ScaledScreenParams.y)));
  r1.y = ((r0.y != 0u) ? r0.z : vd_asuint(v0.y));
  r1.x = vd_asuint(v0.x);
  r1.xy = vd_asuint((vd_asfloat(r1.xy) / vd_asfloat(_ScaledScreenParams.xy)));
  r2.y = vd_asuint((vd_asfloat(_TimeParameters.x) * vd_asfloat(_Speed)));
  r2.x = 0x00000000u;
  r0.yz = vd_asuint((vd_asfloat(r2.xy) + vd_asfloat(v3.xy)));
  r2.xyzw = vd_asuint(_MainTex.SampleBias(sampler_MainTex, vd_asfloat(r0.yz), vd_asfloat(_GlobalMipBias.x)).xyzw);
  r2.xyzw = vd_asuint((vd_asfloat(r2.xyzw) * vd_asfloat(_Color.xyzw)));
  r0.y = vd_asuint(saturate((vd_asfloat(v4.y) * 20.0)));
  r0.z = vd_asuint((vd_asfloat(r0.y) * -2.0 + 3.0));
  r0.y = vd_asuint((vd_asfloat(r0.y) * vd_asfloat(r0.y)));
  r0.y = vd_asuint((vd_asfloat(r0.y) * vd_asfloat(r0.z)));
  r0.z = vd_asuint((vd_asfloat(v4.y) + -1.0));
  r0.z = vd_asuint(saturate((vd_asfloat(r0.z) * -19.9999962)));
  r0.w = vd_asuint((vd_asfloat(r0.z) * -2.0 + 3.0));
  r0.z = vd_asuint((vd_asfloat(r0.z) * vd_asfloat(r0.z)));
  r0.z = vd_asuint((vd_asfloat(r0.z) * vd_asfloat(r0.w)));
  r0.y = vd_asuint((vd_asfloat(r0.z) * vd_asfloat(r0.y)));
  r0.zw = vd_asuint((-(int2)(vd_asfloat(unity_OrthoParams.ww) == float2(1.0, 0.0))));
  if (r0.z != 0u) {
    r3.xy = vd_asuint(((-(vd_asfloat(_CameraDepthTexture_TexelSize.xy))) * float2(0.5, 0.5) + float2(1.0, 1.0)));
    r1.z = vd_asuint(((-(vd_asfloat(r1.y))) + 1.0));
    r3.xy = vd_asuint(min(vd_asfloat(r1.xz), vd_asfloat(r3.xy)));
    r3.xy = vd_asuint((vd_asfloat(r3.xy) * vd_asfloat(_RTHandleScale.xy)));
    r3.xyzw = vd_asuint(_CameraDepthTexture.SampleBias(sampler_vd_point_clamp, vd_asfloat(r3.xy), vd_asfloat(_GlobalMipBias.x)).xyzw);
#if UNITY_UV_STARTS_AT_TOP
#else
    r3.x = vd_asuint(((-(vd_asfloat(r3.x))) + 1.0));
#endif
    r3.yz = vd_asuint((vd_asfloat(r1.xz) * float2(2.0, 2.0) + float2(-1.0, -1.0)));
#if UNITY_UV_STARTS_AT_TOP
    r4.xyzw = vd_asuint(((-(vd_asfloat(r3.zzzz))) * vd_asfloat((VD_COL(unity_MatrixInvVP, 1).xyzw))));
    r4.xyzw = vd_asuint((vd_asfloat((VD_COL(unity_MatrixInvVP, 0).xyzw)) * vd_asfloat(r3.yyyy) + vd_asfloat(r4.xyzw)));
    r3.xyzw = vd_asuint((vd_asfloat((VD_COL(unity_MatrixInvVP, 2).xyzw)) * vd_asfloat(r3.xxxx) + vd_asfloat(r4.xyzw)));
    r3.xyzw = vd_asuint((vd_asfloat(r3.xyzw) + vd_asfloat((VD_COL(unity_MatrixInvVP, 3).xyzw))));
#else
    r4.xyzw = vd_asuint((vd_asfloat(r3.zzzz) * vd_asfloat((VD_COL(vd_MatrixInvVPD3D, 1).xyzw))));
    r4.xyzw = vd_asuint((vd_asfloat((VD_COL(vd_MatrixInvVPD3D, 0).xyzw)) * vd_asfloat(r3.yyyy) + vd_asfloat(r4.xyzw)));
    r3.xyzw = vd_asuint((vd_asfloat((VD_COL(vd_MatrixInvVPD3D, 2).xyzw)) * vd_asfloat(r3.xxxx) + vd_asfloat(r4.xyzw)));
    r3.xyzw = vd_asuint((vd_asfloat(r3.xyzw) + vd_asfloat((VD_COL(vd_MatrixInvVPD3D, 3).xyzw))));
#endif
    r3.xyz = vd_asuint((vd_asfloat(r3.xyz) / vd_asfloat(r3.www)));
    r0.z = vd_asuint((vd_asfloat(r3.y) * vd_asfloat((VD_COL(unity_MatrixV, 1).z))));
    r0.z = vd_asuint((vd_asfloat((VD_COL(unity_MatrixV, 0).z)) * vd_asfloat(r3.x) + vd_asfloat(r0.z)));
    r0.z = vd_asuint((vd_asfloat((VD_COL(unity_MatrixV, 2).z)) * vd_asfloat(r3.z) + vd_asfloat(r0.z)));
    r0.z = vd_asuint((vd_asfloat(r0.z) + vd_asfloat((VD_COL(unity_MatrixV, 3).z))));
    r0.z = vd_asuint(abs(vd_asfloat(r0.z)));
  } else {
    r3.xy = vd_asuint(((-(vd_asfloat(_CameraDepthTexture_TexelSize.xy))) * float2(0.5, 0.5) + float2(1.0, 1.0)));
    r1.w = vd_asuint(((-(vd_asfloat(r1.y))) + 1.0));
    r1.xy = vd_asuint(min(vd_asfloat(r1.xw), vd_asfloat(r3.xy)));
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
  r0.x = vd_asuint(((-(vd_asfloat(r0.x))) + vd_asfloat(r0.z)));
  r0.x = vd_asuint((abs(vd_asfloat(r0.x)) + abs(vd_asfloat(r0.x))));
  r0.x = vd_asuint(min(vd_asfloat(r0.x), 1.0));
  r0.z = vd_asuint((vd_asfloat(r0.x) * -2.0 + 3.0));
  r0.x = vd_asuint((vd_asfloat(r0.x) * vd_asfloat(r0.x)));
  r0.x = vd_asuint((vd_asfloat(r0.x) * vd_asfloat(r0.z)));
  r0.x = vd_asuint((vd_asfloat(r0.x) * vd_asfloat(r0.y)));
  r0.y = vd_asuint((vd_asfloat(r0.x) * vd_asfloat(r2.w)));
  r1.xyz = vd_asuint((vd_asfloat(r0.yyy) * vd_asfloat(r2.xyz)));
  r0.z = vd_asuint((-(int)(vd_asfloat(_AlphaToMaskAvailable) != 0.0)));
  r1.w = vd_asuint(ddx(vd_asfloat(r0.y)));
  r3.x = vd_asuint(ddy(vd_asfloat(r0.y)));
  r1.w = vd_asuint((abs(vd_asfloat(r1.w)) + abs(vd_asfloat(r3.x))));
  r0.x = vd_asuint((vd_asfloat(r2.w) * vd_asfloat(r0.x) + -0.00999999978));
  r2.w = vd_asuint(((-(vd_asfloat(r1.w))) * 0.5 + vd_asfloat(r0.x)));
  r1.w = vd_asuint(max(vd_asfloat(r1.w), 9.99999975e-05));
  r1.w = vd_asuint((vd_asfloat(r2.w) / vd_asfloat(r1.w)));
  r1.w = vd_asuint(saturate((vd_asfloat(r1.w) + 1.0)));
  r2.w = vd_asuint((vd_asfloat(r1.w) + -9.99999975e-05));
  r0.x = ((r0.z != 0u) ? r2.w : r0.x);
  r0.z = vd_asuint(saturate(((r0.z != 0u) ? vd_asfloat(r1.w) : vd_asfloat(r0.y))));
  r0.x = vd_asuint((-(int)(vd_asfloat(r0.x) < 0.0)));
  if (r0.x != 0u) discard;
  r0.x = vd_asuint(dot(vd_asfloat(v7.xyz), vd_asfloat(v7.xyz)));
  r0.x = vd_asuint(rsqrt(vd_asfloat(r0.x)));
  r3.xyz = vd_asuint((vd_asfloat(r0.xxx) * vd_asfloat(v7.xyz)));
  r4.xyz = vd_asuint(((-(vd_asfloat(v6.xyz))) + vd_asfloat(_WorldSpaceCameraPos.xyz)));
  r0.x = vd_asuint(dot(vd_asfloat(r4.xyz), vd_asfloat(r4.xyz)));
  r0.x = vd_asuint(rsqrt(vd_asfloat(r0.x)));
  r4.xyz = vd_asuint((vd_asfloat(r0.xxx) * vd_asfloat(r4.xyz)));
  r5.x = vd_asuint((VD_COL(unity_MatrixV, 0).z));
  r5.y = vd_asuint((VD_COL(unity_MatrixV, 1).z));
  r5.z = vd_asuint((VD_COL(unity_MatrixV, 2).z));
  r4.xyz = ((r0.www != 0u) ? r4.xyz : r5.xyz);
  r6.xy = vd_asuint((float2(1.0, 1.0) / vd_asfloat(_ScaledScreenParams.xy)));
  r6.xy = vd_asuint((vd_asfloat(r6.xy) * vd_asfloat(v0.xy)));
#if UNITY_UV_STARTS_AT_TOP
  r0.x = vd_asuint((vd_asfloat(r6.y) * vd_asfloat(_ScaleBiasRt.x) + vd_asfloat(_ScaleBiasRt.y)));
  r6.z = vd_asuint(((-(vd_asfloat(r0.x))) + 1.0));
#else
  r0.x = r6.y;
  r6.z = r0.x;
#endif
  r3.w = 0x3f800000u;
  r7.x = vd_asuint(dot(vd_asfloat(unity_SHAr.xyzw), vd_asfloat(r3.xyzw)));
  r7.y = vd_asuint(dot(vd_asfloat(unity_SHAg.xyzw), vd_asfloat(r3.xyzw)));
  r7.z = vd_asuint(dot(vd_asfloat(unity_SHAb.xyzw), vd_asfloat(r3.xyzw)));
  r8.xyzw = vd_asuint((vd_asfloat(r3.yzzx) * vd_asfloat(r3.xyzz)));
  r9.x = vd_asuint(dot(vd_asfloat(unity_SHBr.xyzw), vd_asfloat(r8.xyzw)));
  r9.y = vd_asuint(dot(vd_asfloat(unity_SHBg.xyzw), vd_asfloat(r8.xyzw)));
  r9.z = vd_asuint(dot(vd_asfloat(unity_SHBb.xyzw), vd_asfloat(r8.xyzw)));
  r0.x = vd_asuint((vd_asfloat(r3.y) * vd_asfloat(r3.y)));
  r0.x = vd_asuint((vd_asfloat(r3.x) * vd_asfloat(r3.x) + (-(vd_asfloat(r0.x)))));
  r8.xyz = vd_asuint((vd_asfloat(unity_SHC.xyz) * vd_asfloat(r0.xxx) + vd_asfloat(r9.xyz)));
  r7.xyz = vd_asuint((vd_asfloat(r7.xyz) + vd_asfloat(r8.xyz)));
  r1.xyz = vd_asuint((vd_asfloat(r0.zzz) * vd_asfloat(r1.xyz)));
  r1.xyz = vd_asuint((vd_asfloat(r1.xyz) * float3(0.959999979, 0.959999979, 0.959999979)));
  r0.x = vd_asuint(dot((-(vd_asfloat(r4.xyz))), vd_asfloat(r3.xyz)));
  r0.x = vd_asuint((vd_asfloat(r0.x) + vd_asfloat(r0.x)));
  r8.xyz = vd_asuint((vd_asfloat(r3.xyz) * (-(vd_asfloat(r0.xxx))) + (-(vd_asfloat(r4.xyz)))));
  r0.x = vd_asuint(saturate(dot(vd_asfloat(r3.xyz), vd_asfloat(r4.xyz))));
  r0.x = vd_asuint(((-(vd_asfloat(r0.x))) + 1.0));
  r0.x = vd_asuint((vd_asfloat(r0.x) * vd_asfloat(r0.x)));
  r0.x = vd_asuint((vd_asfloat(r0.x) * vd_asfloat(r0.x)));
  r8.xyzw = vd_asuint(unity_SpecCube0.SampleLevel(samplerunity_SpecCube0, vd_asfloat(r8.xyz), 4.05000019).xyzw);
  r1.w = vd_asuint((vd_asfloat(r8.w) + -1.0));
  r1.w = vd_asuint((vd_asfloat(unity_SpecCube0_HDR.w) * vd_asfloat(r1.w) + 1.0));
  r1.w = vd_asuint(max(vd_asfloat(r1.w), 0.0));
  r1.w = vd_asuint(log2(vd_asfloat(r1.w)));
  r1.w = vd_asuint((vd_asfloat(r1.w) * vd_asfloat(unity_SpecCube0_HDR.y)));
  r1.w = vd_asuint(exp2(vd_asfloat(r1.w)));
  r1.w = vd_asuint((vd_asfloat(r1.w) * vd_asfloat(unity_SpecCube0_HDR.x)));
  r8.xyz = vd_asuint((vd_asfloat(r8.xyz) * vd_asfloat(r1.www)));
  r0.x = vd_asuint((vd_asfloat(r0.x) * 0.5 + 0.0399999991));
  r0.x = vd_asuint((vd_asfloat(r0.x) * 0.941176474));
  r8.xyz = vd_asuint((vd_asfloat(r0.xxx) * vd_asfloat(r8.xyz)));
  r7.xyz = vd_asuint((vd_asfloat(r7.xyz) * vd_asfloat(r1.xyz) + vd_asfloat(r8.xyz)));
  r0.x = (vd_asuint(_MainLightLayerMask) & vd_asuint(unity_RenderingLayer.x));
  r1.w = vd_asuint(saturate(dot(vd_asfloat(r3.xyz), vd_asfloat(_MainLightPosition.xyz))));
  r8.xyz = vd_asuint((vd_asfloat(r1.www) * vd_asfloat(_MainLightColor.xyz)));
  r9.xyz = vd_asuint((vd_asfloat(r4.xyz) + vd_asfloat(_MainLightPosition.xyz)));
  r1.w = vd_asuint(dot(vd_asfloat(r9.xyz), vd_asfloat(r9.xyz)));
  r1.w = vd_asuint(max(vd_asfloat(r1.w), 1.17549435e-38));
  r1.w = vd_asuint(rsqrt(vd_asfloat(r1.w)));
  r9.xyz = vd_asuint((vd_asfloat(r1.www) * vd_asfloat(r9.xyz)));
  r1.w = vd_asuint(saturate(dot(vd_asfloat(r3.xyz), vd_asfloat(r9.xyz))));
  r2.w = vd_asuint(saturate(dot(vd_asfloat(_MainLightPosition.xyz), vd_asfloat(r9.xyz))));
  r1.w = vd_asuint((vd_asfloat(r1.w) * vd_asfloat(r1.w)));
  r1.w = vd_asuint((vd_asfloat(r1.w) * -0.9375 + 1.00001001));
  r2.w = vd_asuint((vd_asfloat(r2.w) * vd_asfloat(r2.w)));
  r1.w = vd_asuint((vd_asfloat(r1.w) * vd_asfloat(r1.w)));
  r2.w = vd_asuint(max(vd_asfloat(r2.w), 0.100000001));
  r1.w = vd_asuint((vd_asfloat(r1.w) * vd_asfloat(r2.w)));
  r1.w = vd_asuint((vd_asfloat(r1.w) * 3.0));
  r1.w = vd_asuint((0.0625 / vd_asfloat(r1.w)));
  r9.xyz = vd_asuint((vd_asfloat(r1.www) * float3(0.0399999991, 0.0399999991, 0.0399999991) + vd_asfloat(r1.xyz)));
  r8.xyz = vd_asuint((vd_asfloat(r8.xyz) * vd_asfloat(r9.xyz)));
  r8.xyz = ((r0.xxx != 0u) ? r8.xyz : uint3(0x00000000u, 0x00000000u, 0x00000000u));
  r0.x = ((uint)vd_asfloat(_FPParams0.w));
  r1.w = min(r0.x, 0x00000100u);
  r9.xyz = uint3(0x00000000u, 0x00000000u, 0x00000000u);
  r2.w = 0x00000000u;
  while (true) {
    r3.w = vd_asuint((-(int)(r2.w >= r1.w)));
    if (r3.w != 0u) break;
    r10.xyz = vd_asuint(((-(vd_asfloat(v6.xyz))) * vd_asfloat(_AdditionalLightsPosition[(int)(r2.w)].www) + vd_asfloat(_AdditionalLightsPosition[(int)(r2.w)].xyz)));
    r3.w = vd_asuint(dot(vd_asfloat(r10.xyz), vd_asfloat(r10.xyz)));
    r3.w = vd_asuint(max(vd_asfloat(r3.w), 6.10351562e-05));
    r4.w = vd_asuint(rsqrt(vd_asfloat(r3.w)));
    r11.xyz = vd_asuint((vd_asfloat(r4.www) * vd_asfloat(r10.xyz)));
    r5.w = vd_asuint((1.0 / vd_asfloat(r3.w)));
    r3.w = vd_asuint((vd_asfloat(r3.w) * vd_asfloat(_AdditionalLightsAttenuation[(int)(r2.w)].x)));
    r3.w = vd_asuint(((-(vd_asfloat(r3.w))) * vd_asfloat(r3.w) + 1.0));
    r3.w = vd_asuint(max(vd_asfloat(r3.w), 0.0));
    r3.w = vd_asuint((vd_asfloat(r3.w) * vd_asfloat(r3.w)));
    r3.w = vd_asuint((vd_asfloat(r3.w) * vd_asfloat(r5.w)));
    r5.w = vd_asuint(dot(vd_asfloat(_AdditionalLightsSpotDir[(int)(r2.w)].xyz), vd_asfloat(r11.xyz)));
    r5.w = vd_asuint(saturate((vd_asfloat(r5.w) * vd_asfloat(_AdditionalLightsAttenuation[(int)(r2.w)].z) + vd_asfloat(_AdditionalLightsAttenuation[(int)(r2.w)].w))));
    r5.w = vd_asuint((vd_asfloat(r5.w) * vd_asfloat(r5.w)));
    r3.w = vd_asuint((vd_asfloat(r3.w) * vd_asfloat(r5.w)));
    r5.w = (vd_asuint(unity_RenderingLayer.x) & vd_asuint(_AdditionalLightsLayerMasks[(int)(r2.w)]));
    r6.y = vd_asuint(saturate(dot(vd_asfloat(r3.xyz), vd_asfloat(r11.xyz))));
    r3.w = vd_asuint((vd_asfloat(r3.w) * vd_asfloat(r6.y)));
    r12.xyz = vd_asuint((vd_asfloat(r3.www) * vd_asfloat(_AdditionalLightsColor[(int)(r2.w)].xyz)));
    r10.xyz = vd_asuint((vd_asfloat(r10.xyz) * vd_asfloat(r4.www) + vd_asfloat(r4.xyz)));
    r3.w = vd_asuint(dot(vd_asfloat(r10.xyz), vd_asfloat(r10.xyz)));
    r3.w = vd_asuint(max(vd_asfloat(r3.w), 1.17549435e-38));
    r3.w = vd_asuint(rsqrt(vd_asfloat(r3.w)));
    r10.xyz = vd_asuint((vd_asfloat(r3.www) * vd_asfloat(r10.xyz)));
    r3.w = vd_asuint(saturate(dot(vd_asfloat(r3.xyz), vd_asfloat(r10.xyz))));
    r4.w = vd_asuint(saturate(dot(vd_asfloat(r11.xyz), vd_asfloat(r10.xyz))));
    r3.w = vd_asuint((vd_asfloat(r3.w) * vd_asfloat(r3.w)));
    r3.w = vd_asuint((vd_asfloat(r3.w) * -0.9375 + 1.00001001));
    r4.w = vd_asuint((vd_asfloat(r4.w) * vd_asfloat(r4.w)));
    r3.w = vd_asuint((vd_asfloat(r3.w) * vd_asfloat(r3.w)));
    r4.w = vd_asuint(max(vd_asfloat(r4.w), 0.100000001));
    r3.w = vd_asuint((vd_asfloat(r3.w) * vd_asfloat(r4.w)));
    r3.w = vd_asuint((vd_asfloat(r3.w) * 3.0));
    r3.w = vd_asuint((0.0625 / vd_asfloat(r3.w)));
    r10.xyz = vd_asuint((vd_asfloat(r3.www) * float3(0.0399999991, 0.0399999991, 0.0399999991) + vd_asfloat(r1.xyz)));
    r10.xyz = vd_asuint((vd_asfloat(r10.xyz) * vd_asfloat(r12.xyz) + vd_asfloat(r9.xyz)));
    r9.xyz = ((r5.www != 0u) ? r10.xyz : r9.xyz);
    r2.w = vd_asuint((vd_asint(r2.w) + 1));
  }
  r6.xy = vd_asuint((vd_asfloat(r6.xz) * vd_asfloat(_FPParams1.xy)));
  r6.xy = ((uint2)vd_asfloat(r6.xy));
  r6.zw = ((uint2)vd_asfloat(_FPParams1.zw));
  r1.w = vd_asuint((vd_asint(r6.y) * vd_asint(r6.z) + vd_asint(r6.x)));
  r1.w = vd_asuint((vd_asint(r6.w) * vd_asint(r1.w)));
  r6.xyz = vd_asuint((vd_asfloat(v6.xyz) + (-(vd_asfloat(_WorldSpaceCameraPos.xyz)))));
  r2.w = vd_asuint(dot((-(vd_asfloat(r5.xyz))), vd_asfloat(r6.xyz)));
  r3.w = vd_asuint(log2(vd_asfloat(r2.w)));
  r0.w = ((r0.w != 0u) ? r3.w : r2.w);
  r0.w = vd_asuint((vd_asfloat(r0.w) * vd_asfloat(_FPParams0.x) + vd_asfloat(_FPParams0.y)));
  r0.w = ((uint)vd_asfloat(r0.w));
  r2.w = ((uint)vd_asfloat(_FPParams2.x));
  r2.w = vd_asuint((vd_asint(r2.w) + -1));
  r3.w = vd_asuint((vd_asint(r6.w) + 2));
  r0.w = min(r0.w, r2.w);
  r2.w = vd_asuint((vd_asint(r3.w) * vd_asint(r0.w)));
  r0.w = vd_asuint((vd_asint(r0.w) * vd_asint(r3.w) + 2));
  r3.w = (r2.w >> (uint)2);
  r5.x = (r2.w << (uint)31);
  r5.y = (r2.w << (uint)30);
  r5.xy = vd_asuint((vd_asint(r5.xy) >> 31));
  r5.zw = (r5.xx & vd_asuint(urp_ZBins[(int)(r3.w)].wy));
  r6.xy = (~r5.xy);
  r6.xz = (r6.xx & vd_asuint(urp_ZBins[(int)(r3.w)].zx));
  r5.xz = (r5.zw | r6.xz);
  r2.w = (r5.y & r5.x);
  r3.w = (r6.y & r5.z);
  r2.w = (r2.w | r3.w);
  r5.x = r1.w;
  r5.y = r0.w;
  r5.z = 0x00000000u;
  r5.w = r2.w;
  r6.xyz = r9.xyz;
  while (true) {
    r3.w = (r5.w >> (uint)16);
    r4.w = r5.z;
    r6.w = r5.w;
    while (true) {
      r7.w = vd_asuint((-(int)(vd_asint(r4.w) == 0)));
      r8.w = (r6.w & 0x0000ffffu);
      r8.w = vd_asuint((-(int)(r3.w >= r8.w)));
      r7.w = (r7.w & r8.w);
      if (r7.w == 0u) break;
      r7.w = (r6.w >> (uint)5);
      r7.w = (r7.w & 0x000007ffu);
      r10.xyzw = vd_asuint((vd_asint(r5.xxyy) + vd_asint(r7.wwww)));
      r11.xy = (r10.yw >> (uint2)2);
      r12.xz = (r10.xz << (uint2)31);
      r12.yw = (r10.yw << (uint2)30);
      r10.xyzw = vd_asuint((vd_asint(r12.xyzw) >> 31));
      r11.zw = (r10.xx & vd_asuint(urp_Tiles[(int)(r11.x)].wy));
      r12.xyzw = (~r10.xyzw);
      r13.xy = (r12.xx & vd_asuint(urp_Tiles[(int)(r11.x)].zx));
      r11.xz = (r11.zw | r13.xy);
      r8.w = (r10.y & r11.x);
      r9.w = (r12.y & r11.z);
      r8.w = (r8.w | r9.w);
      r10.xy = (r10.zz & vd_asuint(urp_ZBins[(int)(r11.y)].wy));
      r11.xy = (r12.zz & vd_asuint(urp_ZBins[(int)(r11.y)].zx));
      r10.xy = (r10.xy | r11.xy);
      r9.w = (r10.w & r10.x);
      r10.x = (r12.w & r10.y);
      r9.w = (r9.w | r10.x);
      r8.w = (r8.w & r9.w);
      r9.w = (0xffffffffu << (uint)(int)(r6.w & 31u));
      r8.w = (r8.w & r9.w);
      r7.w = (r7.w << (uint)5);
      r7.w = vd_asuint((vd_asint(r3.w) + (-(vd_asint(r7.w)))));
      r7.w = min(r7.w, 0x0000001fu);
      r7.w = vd_asuint(((-(vd_asint(r7.w))) + 31));
      r7.w = (0xffffffffu >> (uint)(int)(r7.w & 31u));
      r4.w = (r7.w & r8.w);
      r7.w = vd_asuint((vd_asint(r6.w) + 32));
      r6.w = (r7.w & 0xffffffe0u);
    }
    r5.w = r6.w;
    if (r4.w == 0u) break;
    r3.w = vd_asuint((-vd_asint(r4.w)));
    r3.w = (r3.w & r4.w);
    r3.w = vd_asuint(((float)r3.w));
    r3.w = (r3.w >> (uint)23);
    r3.w = vd_asuint((vd_asint(r3.w) + -127));
    r7.w = (0x00000001u << (uint)(int)(r3.w & 31u));
    r5.z = (r4.w ^ r7.w);
    r7.w = vd_asuint((vd_asint(r5.w) + -32));
    r7.w = (r7.w & 0x0000ffe0u);
    r3.w = vd_asuint((vd_asint(r3.w) + vd_asint(r7.w)));
    r3.w = vd_asuint((vd_asint(r0.x) + vd_asint(r3.w)));
    r10.xyz = vd_asuint(((-(vd_asfloat(v6.xyz))) * vd_asfloat(_AdditionalLightsPosition[(int)(r3.w)].www) + vd_asfloat(_AdditionalLightsPosition[(int)(r3.w)].xyz)));
    r7.w = vd_asuint(dot(vd_asfloat(r10.xyz), vd_asfloat(r10.xyz)));
    r7.w = vd_asuint(max(vd_asfloat(r7.w), 6.10351562e-05));
    r8.w = vd_asuint(rsqrt(vd_asfloat(r7.w)));
    r11.xyz = vd_asuint((vd_asfloat(r8.www) * vd_asfloat(r10.xyz)));
    r9.w = vd_asuint((1.0 / vd_asfloat(r7.w)));
    r7.w = vd_asuint((vd_asfloat(r7.w) * vd_asfloat(_AdditionalLightsAttenuation[(int)(r3.w)].x)));
    r7.w = vd_asuint(((-(vd_asfloat(r7.w))) * vd_asfloat(r7.w) + 1.0));
    r7.w = vd_asuint(max(vd_asfloat(r7.w), 0.0));
    r7.w = vd_asuint((vd_asfloat(r7.w) * vd_asfloat(r7.w)));
    r7.w = vd_asuint((vd_asfloat(r7.w) * vd_asfloat(r9.w)));
    r9.w = vd_asuint(dot(vd_asfloat(_AdditionalLightsSpotDir[(int)(r3.w)].xyz), vd_asfloat(r11.xyz)));
    r9.w = vd_asuint(saturate((vd_asfloat(r9.w) * vd_asfloat(_AdditionalLightsAttenuation[(int)(r3.w)].z) + vd_asfloat(_AdditionalLightsAttenuation[(int)(r3.w)].w))));
    r9.w = vd_asuint((vd_asfloat(r9.w) * vd_asfloat(r9.w)));
    r7.w = vd_asuint((vd_asfloat(r7.w) * vd_asfloat(r9.w)));
    r9.w = (vd_asuint(unity_RenderingLayer.x) & vd_asuint(_AdditionalLightsLayerMasks[(int)(r3.w)]));
    r10.w = vd_asuint(saturate(dot(vd_asfloat(r3.xyz), vd_asfloat(r11.xyz))));
    r7.w = vd_asuint((vd_asfloat(r7.w) * vd_asfloat(r10.w)));
    r12.xyz = vd_asuint((vd_asfloat(r7.www) * vd_asfloat(_AdditionalLightsColor[(int)(r3.w)].xyz)));
    r10.xyz = vd_asuint((vd_asfloat(r10.xyz) * vd_asfloat(r8.www) + vd_asfloat(r4.xyz)));
    r3.w = vd_asuint(dot(vd_asfloat(r10.xyz), vd_asfloat(r10.xyz)));
    r3.w = vd_asuint(max(vd_asfloat(r3.w), 1.17549435e-38));
    r3.w = vd_asuint(rsqrt(vd_asfloat(r3.w)));
    r10.xyz = vd_asuint((vd_asfloat(r3.www) * vd_asfloat(r10.xyz)));
    r3.w = vd_asuint(saturate(dot(vd_asfloat(r3.xyz), vd_asfloat(r10.xyz))));
    r7.w = vd_asuint(saturate(dot(vd_asfloat(r11.xyz), vd_asfloat(r10.xyz))));
    r3.w = vd_asuint((vd_asfloat(r3.w) * vd_asfloat(r3.w)));
    r3.w = vd_asuint((vd_asfloat(r3.w) * -0.9375 + 1.00001001));
    r7.w = vd_asuint((vd_asfloat(r7.w) * vd_asfloat(r7.w)));
    r3.w = vd_asuint((vd_asfloat(r3.w) * vd_asfloat(r3.w)));
    r7.w = vd_asuint(max(vd_asfloat(r7.w), 0.100000001));
    r3.w = vd_asuint((vd_asfloat(r3.w) * vd_asfloat(r7.w)));
    r3.w = vd_asuint((vd_asfloat(r3.w) * 3.0));
    r3.w = vd_asuint((0.0625 / vd_asfloat(r3.w)));
    r10.xyz = vd_asuint((vd_asfloat(r3.www) * float3(0.0399999991, 0.0399999991, 0.0399999991) + vd_asfloat(r1.xyz)));
    r10.xyz = vd_asuint((vd_asfloat(r10.xyz) * vd_asfloat(r12.xyz) + vd_asfloat(r6.xyz)));
    r6.xyz = ((r9.www != 0u) ? r10.xyz : r6.xyz);
  }
  r1.xyz = vd_asuint((vd_asfloat(r7.xyz) + vd_asfloat(r8.xyz)));
  r1.xyz = vd_asuint((vd_asfloat(r6.xyz) + vd_asfloat(r1.xyz)));
  o0.xyz = (vd_asfloat(r2.xyz) * vd_asfloat(r0.yyy) + vd_asfloat(r1.xyz));
  o0.w = vd_asfloat(r0.z);
  return;
}
