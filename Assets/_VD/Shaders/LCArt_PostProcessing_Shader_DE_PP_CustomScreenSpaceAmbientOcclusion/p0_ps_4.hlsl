// LCArt/PostProcessing/Shader_DE_PP_CustomScreenSpaceAmbientOcclusion | pass 0 "SSAO_Occlusion" | progFragment | keywords: _INTERLEAVED_GRADIENT | blob 4
void frag(
  float4 v0 : SV_POSITION0,
  float4 v1 : VDREG1,
  out float4 o0 : SV_Target0)
{
#if UNITY_UV_STARTS_AT_TOP
#else
  float4 vd_ZBufferParamsD3D = float4(-_ZBufferParams.x, _ZBufferParams.x + _ZBufferParams.y, -_ZBufferParams.z, _ZBufferParams.z + _ZBufferParams.w);
  float4x4 vd_MatrixInvPD3D = mul(unity_MatrixInvP, float4x4(1, 0, 0, 0, 0, 1, 0, 0, 0, 0, -2, 1, 0, 0, 0, 1));
#endif
  uint4 r0, r1, r2, r3, r4, r5, r6, r7, r8, r9, r10;
  r0 = 0u; r1 = 0u; r2 = 0u; r3 = 0u; r4 = 0u; r5 = 0u; r6 = 0u; r7 = 0u; r8 = 0u; r9 = 0u; r10 = 0u;
  uint4 x0[40];

  x0[0].x = 0x00000000u;
  x0[1].x = 0x3eae0000u;
  x0[2].x = 0x3f410000u;
  x0[3].x = 0x3f110000u;
  x0[4].x = 0x3f7c0000u;
  x0[5].x = 0x3d980000u;
  x0[6].x = 0x3e740000u;
  x0[7].x = 0x3f240000u;
  x0[8].x = 0x3eb80000u;
  x0[9].x = 0x3f020000u;
  x0[10].x = 0x3ec40000u;
  x0[11].x = 0x3f7c0000u;
  x0[12].x = 0x3e340000u;
  x0[13].x = 0x3f0a0000u;
  x0[14].x = 0x3e920000u;
  x0[15].x = 0x3e6cecf0u;
  x0[16].x = 0x3eeaeaedu;
  x0[17].x = 0x3f0a8a8bu;
  x0[18].x = 0x3e048487u;
  x0[19].x = 0x3f24a4a5u;
  x0[20].x = 0x3f6e0000u;
  x0[21].x = 0x3f430000u;
  x0[22].x = 0x3e088886u;
  x0[23].x = 0x3c800000u;
  x0[24].x = 0x00000000u;
  x0[25].x = 0x3dd80000u;
  x0[26].x = 0x3f240000u;
  x0[27].x = 0x3f3f0000u;
  x0[28].x = 0x3f2e0000u;
  x0[29].x = 0x3eb40000u;
  x0[30].x = 0x3efc0000u;
  x0[31].x = 0x3e000000u;
  x0[32].x = 0x3e880000u;
  x0[33].x = 0x3f200000u;
  x0[34].x = 0x3ee40000u;
  x0[35].x = 0x3e34b4b5u;
  x0[36].x = 0x3ee4e4e7u;
  x0[37].x = 0x3f6eeef0u;
  x0[38].x = 0x3f5edee0u;
  x0[39].x = 0x3f119192u;
  r0.xy = vd_asuint((vd_asfloat(_CameraDepthTexture_TexelSize.xy) * float2(0.5, 0.5)));
  r0.z = vd_asuint((vd_asfloat(_SSAOParams.z) + -0.5));
  r0.z = vd_asuint(((-(vd_asfloat(r0.z))) * 2.0 + 1.0));
  r1.xy = vd_asuint((vd_asfloat(r0.xy) * vd_asfloat(r0.zz) + vd_asfloat(v1.xy)));
  r1.zw = vd_asuint(((-(vd_asfloat(_CameraDepthTexture_TexelSize.xy))) * float2(0.5, 0.5) + float2(1.0, 1.0)));
  r1.xy = vd_asuint(min(vd_asfloat(r1.zw), vd_asfloat(r1.xy)));
  r1.xy = vd_asuint((vd_asfloat(r1.xy) * vd_asfloat(_RTHandleScale.xy)));
  r2.xyzw = vd_asuint(_CameraDepthTexture.SampleBias(sampler_vd_point_clamp, vd_asfloat(r1.xy), vd_asfloat(_GlobalMipBias.x)).xyzw);
#if UNITY_UV_STARTS_AT_TOP
#else
  r2.x = vd_asuint(((-(vd_asfloat(r2.x))) + 1.0));
#endif
  r0.w = vd_asuint((-(int)(vd_asfloat(r2.x) < 9.99999975e-06)));
  if (r0.w != 0u) {
    o0.xyzw = vd_asfloat(uint4(0x3f800000u, 0x3f800000u, 0x3f800000u, 0x3f800000u));
    return;
  }
#if UNITY_UV_STARTS_AT_TOP
  r0.w = vd_asuint((vd_asfloat(_ZBufferParams.z) * vd_asfloat(r2.x) + vd_asfloat(_ZBufferParams.w)));
#else
  r0.w = vd_asuint((vd_asfloat(vd_ZBufferParamsD3D.z) * vd_asfloat(r2.x) + vd_asfloat(vd_ZBufferParamsD3D.w)));
#endif
  r0.w = vd_asuint((1.0 / vd_asfloat(r0.w)));
  r1.x = vd_asuint((-(int)(vd_asfloat(_SSAOParams.w) < vd_asfloat(r0.w))));
  if (r1.x != 0u) {
    o0.xyzw = vd_asfloat(uint4(0x00000000u, 0x3f000000u, 0x3f000000u, 0x3f000000u));
    return;
  }
  r1.x = vd_asuint((-(int)(50.0 < vd_asfloat(r0.w))));
  if (r1.x != 0u) {
    o0.xyzw = vd_asfloat(uint4(0x3f800000u, 0x3f800000u, 0x3f800000u, 0x3f800000u));
    return;
  }
  if (vd_asuint(_SSAOSourceMode) == 0u) {
    r1.xy = vd_asuint(((-(vd_asfloat(_CameraNormalsTexture_TexelSize.xy))) * float2(0.5, 0.5) + float2(1.0, 1.0)));
    r1.xy = vd_asuint(min(vd_asfloat(r1.xy), vd_asfloat(v1.xy)));
    r1.xy = vd_asuint((vd_asfloat(r1.xy) * vd_asfloat(_RTHandleScale.xy)));
    r2.xyzw = vd_asuint(_CameraNormalsTexture.SampleBias(sampler_vd_point_clamp, vd_asfloat(r1.xy), vd_asfloat(_GlobalMipBias.x)).xyzw);
  } else {
    r1.x = vd_asuint(((-(vd_asfloat(v1.y))) + 1.0));
    r1.y = vd_asuint((vd_asfloat(r0.w) * vd_asfloat(_ProjectionParams2.x)));
    r3.xyz = vd_asuint((vd_asfloat(_CameraViewXExtent[0].yzx) * vd_asfloat(v1.xxx) + vd_asfloat(_CameraViewTopLeftCorner[0].yzx)));
    r3.xyz = vd_asuint((vd_asfloat(_CameraViewYExtent[0].yzx) * vd_asfloat(r1.xxx) + vd_asfloat(r3.xyz)));
    r3.xyz = vd_asuint((vd_asfloat(r1.yyy) * vd_asfloat(r3.xyz)));
    r1.x = vd_asuint((-(int)(vd_asint(_SSAOSourceMode) == 1)));
    if (r1.x != 0u) {
      r4.xyz = vd_asuint(ddy(vd_asfloat(r3.yzx)));
      r5.xyz = vd_asuint(ddx(vd_asfloat(r3.xyz)));
      r6.xyz = vd_asuint((vd_asfloat(r4.xyz) * vd_asfloat(r5.xyz)));
      r4.xyz = vd_asuint((vd_asfloat(r4.zxy) * vd_asfloat(r5.yzx) + (-(vd_asfloat(r6.xyz)))));
      r1.x = vd_asuint(dot(vd_asfloat(r4.xyz), vd_asfloat(r4.xyz)));
      r1.x = vd_asuint(rsqrt(vd_asfloat(r1.x)));
      r2.xyz = vd_asuint((vd_asfloat(r1.xxx) * vd_asfloat(r4.xyz)));
    } else {
      r4.xz = vd_asuint((vd_asfloat(_SourceSize.zz) * float2(-2.0, 2.0)));
      r4.yw = uint2(0x00000000u, 0x00000000u);
      r5.xy = vd_asuint((vd_asfloat(r4.xy) + vd_asfloat(v1.xy)));
      r1.xy = vd_asuint((vd_asfloat(r0.xy) * vd_asfloat(r0.zz) + vd_asfloat(r5.xy)));
      r1.xy = vd_asuint(min(vd_asfloat(r1.zw), vd_asfloat(r1.xy)));
      r1.xy = vd_asuint((vd_asfloat(r1.xy) * vd_asfloat(_RTHandleScale.xy)));
      r6.xyzw = vd_asuint(_CameraDepthTexture.SampleBias(sampler_vd_point_clamp, vd_asfloat(r1.xy), vd_asfloat(_GlobalMipBias.x)).xyzw);
#if UNITY_UV_STARTS_AT_TOP
      r1.x = vd_asuint((vd_asfloat(_ZBufferParams.z) * vd_asfloat(r6.x) + vd_asfloat(_ZBufferParams.w)));
#else
      r6.x = vd_asuint(((-(vd_asfloat(r6.x))) + 1.0));
      r1.x = vd_asuint((vd_asfloat(vd_ZBufferParamsD3D.z) * vd_asfloat(r6.x) + vd_asfloat(vd_ZBufferParamsD3D.w)));
#endif
      r5.z = vd_asuint((1.0 / vd_asfloat(r1.x)));
      r4.xy = vd_asuint((vd_asfloat(r4.zw) + vd_asfloat(v1.xy)));
      r1.xy = vd_asuint((vd_asfloat(r0.xy) * vd_asfloat(r0.zz) + vd_asfloat(r4.xy)));
      r1.xy = vd_asuint(min(vd_asfloat(r1.zw), vd_asfloat(r1.xy)));
      r1.xy = vd_asuint((vd_asfloat(r1.xy) * vd_asfloat(_RTHandleScale.xy)));
      r6.xyzw = vd_asuint(_CameraDepthTexture.SampleBias(sampler_vd_point_clamp, vd_asfloat(r1.xy), vd_asfloat(_GlobalMipBias.x)).xyzw);
#if UNITY_UV_STARTS_AT_TOP
      r1.x = vd_asuint((vd_asfloat(_ZBufferParams.z) * vd_asfloat(r6.x) + vd_asfloat(_ZBufferParams.w)));
#else
      r6.x = vd_asuint(((-(vd_asfloat(r6.x))) + 1.0));
      r1.x = vd_asuint((vd_asfloat(vd_ZBufferParamsD3D.z) * vd_asfloat(r6.x) + vd_asfloat(vd_ZBufferParamsD3D.w)));
#endif
      r4.z = vd_asuint((1.0 / vd_asfloat(r1.x)));
      r6.xz = uint2(0x00000000u, 0x00000000u);
      r6.yw = vd_asuint((vd_asfloat(_SourceSize.ww) * float2(2.0, -2.0)));
      r7.xy = vd_asuint((vd_asfloat(r6.xy) + vd_asfloat(v1.xy)));
      r1.xy = vd_asuint((vd_asfloat(r0.xy) * vd_asfloat(r0.zz) + vd_asfloat(r7.xy)));
      r1.xy = vd_asuint(min(vd_asfloat(r1.zw), vd_asfloat(r1.xy)));
      r1.xy = vd_asuint((vd_asfloat(r1.xy) * vd_asfloat(_RTHandleScale.xy)));
      r8.xyzw = vd_asuint(_CameraDepthTexture.SampleBias(sampler_vd_point_clamp, vd_asfloat(r1.xy), vd_asfloat(_GlobalMipBias.x)).xyzw);
#if UNITY_UV_STARTS_AT_TOP
      r1.x = vd_asuint((vd_asfloat(_ZBufferParams.z) * vd_asfloat(r8.x) + vd_asfloat(_ZBufferParams.w)));
#else
      r8.x = vd_asuint(((-(vd_asfloat(r8.x))) + 1.0));
      r1.x = vd_asuint((vd_asfloat(vd_ZBufferParamsD3D.z) * vd_asfloat(r8.x) + vd_asfloat(vd_ZBufferParamsD3D.w)));
#endif
      r7.z = vd_asuint((1.0 / vd_asfloat(r1.x)));
      r6.xy = vd_asuint((vd_asfloat(r6.zw) + vd_asfloat(v1.xy)));
      r1.xy = vd_asuint((vd_asfloat(r0.xy) * vd_asfloat(r0.zz) + vd_asfloat(r6.xy)));
      r1.xy = vd_asuint(min(vd_asfloat(r1.zw), vd_asfloat(r1.xy)));
      r1.xy = vd_asuint((vd_asfloat(r1.xy) * vd_asfloat(_RTHandleScale.xy)));
      r8.xyzw = vd_asuint(_CameraDepthTexture.SampleBias(sampler_vd_point_clamp, vd_asfloat(r1.xy), vd_asfloat(_GlobalMipBias.x)).xyzw);
#if UNITY_UV_STARTS_AT_TOP
      r1.x = vd_asuint((vd_asfloat(_ZBufferParams.z) * vd_asfloat(r8.x) + vd_asfloat(_ZBufferParams.w)));
#else
      r8.x = vd_asuint(((-(vd_asfloat(r8.x))) + 1.0));
      r1.x = vd_asuint((vd_asfloat(vd_ZBufferParamsD3D.z) * vd_asfloat(r8.x) + vd_asfloat(vd_ZBufferParamsD3D.w)));
#endif
      r6.z = vd_asuint((1.0 / vd_asfloat(r1.x)));
      r1.x = vd_asuint((-(int)(vd_asint(_SSAOSourceMode) == 2)));
      if (r1.x != 0u) {
        r1.x = vd_asuint((-(int)(vd_asfloat(r4.z) < vd_asfloat(r5.z))));
        r1.y = vd_asuint((-(int)(vd_asfloat(r7.z) < vd_asfloat(r6.z))));
        r1.xy = ((r1.xy != 0u) ? uint2(0x00000000u, 0x00000000u) : uint2(0x00000001u, 0x00000001u));
      } else {
        r8.xw = vd_asuint(_SourceSize.zw);
        r8.yz = uint2(0x00000000u, 0x40000000u);
        r9.xyzw = vd_asuint((vd_asfloat(r8.xyxy) * float4(-4.0, 2.0, 4.0, 2.0) + vd_asfloat(v1.xyxy)));
        r9.xyzw = vd_asuint((vd_asfloat(r0.xyxy) * vd_asfloat(r0.zzzz) + vd_asfloat(r9.xyzw)));
        r9.xyzw = vd_asuint(min(vd_asfloat(r1.zwzw), vd_asfloat(r9.xyzw)));
        r9.xyzw = vd_asuint((vd_asfloat(r9.xyzw) * vd_asfloat(_RTHandleScale.xyxy)));
        r10.xyzw = vd_asuint(_CameraDepthTexture.SampleBias(sampler_vd_point_clamp, vd_asfloat(r9.xy), vd_asfloat(_GlobalMipBias.x)).xyzw);
#if UNITY_UV_STARTS_AT_TOP
        r2.w = vd_asuint((vd_asfloat(_ZBufferParams.z) * vd_asfloat(r10.x) + vd_asfloat(_ZBufferParams.w)));
#else
        r10.x = vd_asuint(((-(vd_asfloat(r10.x))) + 1.0));
        r2.w = vd_asuint((vd_asfloat(vd_ZBufferParamsD3D.z) * vd_asfloat(r10.x) + vd_asfloat(vd_ZBufferParamsD3D.w)));
#endif
        r2.w = vd_asuint((1.0 / vd_asfloat(r2.w)));
        r9.xyzw = vd_asuint(_CameraDepthTexture.SampleBias(sampler_vd_point_clamp, vd_asfloat(r9.zw), vd_asfloat(_GlobalMipBias.x)).xyzw);
#if UNITY_UV_STARTS_AT_TOP
        r3.w = vd_asuint((vd_asfloat(_ZBufferParams.z) * vd_asfloat(r9.x) + vd_asfloat(_ZBufferParams.w)));
#else
        r9.x = vd_asuint(((-(vd_asfloat(r9.x))) + 1.0));
        r3.w = vd_asuint((vd_asfloat(vd_ZBufferParamsD3D.z) * vd_asfloat(r9.x) + vd_asfloat(vd_ZBufferParamsD3D.w)));
#endif
        r3.w = vd_asuint((1.0 / vd_asfloat(r3.w)));
        r8.xyzw = vd_asuint((float4(0.0, 4.0, 0.0, -4.0) * vd_asfloat(r8.zwzw) + vd_asfloat(v1.xyxy)));
        r8.xyzw = vd_asuint((vd_asfloat(r0.xyxy) * vd_asfloat(r0.zzzz) + vd_asfloat(r8.xyzw)));
        r8.xyzw = vd_asuint(min(vd_asfloat(r1.zwzw), vd_asfloat(r8.xyzw)));
        r8.xyzw = vd_asuint((vd_asfloat(r8.xyzw) * vd_asfloat(_RTHandleScale.xyxy)));
        r9.xyzw = vd_asuint(_CameraDepthTexture.SampleBias(sampler_vd_point_clamp, vd_asfloat(r8.xy), vd_asfloat(_GlobalMipBias.x)).xyzw);
#if UNITY_UV_STARTS_AT_TOP
        r4.w = vd_asuint((vd_asfloat(_ZBufferParams.z) * vd_asfloat(r9.x) + vd_asfloat(_ZBufferParams.w)));
#else
        r9.x = vd_asuint(((-(vd_asfloat(r9.x))) + 1.0));
        r4.w = vd_asuint((vd_asfloat(vd_ZBufferParamsD3D.z) * vd_asfloat(r9.x) + vd_asfloat(vd_ZBufferParamsD3D.w)));
#endif
        r4.w = vd_asuint((1.0 / vd_asfloat(r4.w)));
        r8.xyzw = vd_asuint(_CameraDepthTexture.SampleBias(sampler_vd_point_clamp, vd_asfloat(r8.zw), vd_asfloat(_GlobalMipBias.x)).xyzw);
#if UNITY_UV_STARTS_AT_TOP
        r5.w = vd_asuint((vd_asfloat(_ZBufferParams.z) * vd_asfloat(r8.x) + vd_asfloat(_ZBufferParams.w)));
#else
        r8.x = vd_asuint(((-(vd_asfloat(r8.x))) + 1.0));
        r5.w = vd_asuint((vd_asfloat(vd_ZBufferParamsD3D.z) * vd_asfloat(r8.x) + vd_asfloat(vd_ZBufferParamsD3D.w)));
#endif
        r5.w = vd_asuint((1.0 / vd_asfloat(r5.w)));
        r2.w = vd_asuint((vd_asfloat(r5.z) * 2.0 + (-(vd_asfloat(r2.w)))));
        r2.w = vd_asuint(((-(vd_asfloat(r0.w))) + vd_asfloat(r2.w)));
        r3.w = vd_asuint((vd_asfloat(r4.z) * 2.0 + (-(vd_asfloat(r3.w)))));
        r3.w = vd_asuint(((-(vd_asfloat(r0.w))) + vd_asfloat(r3.w)));
        r2.w = vd_asuint((-(int)(abs(vd_asfloat(r2.w)) < abs(vd_asfloat(r3.w)))));
        r1.x = ((r2.w != 0u) ? 0x00000000u : 0x00000001u);
        r2.w = vd_asuint((vd_asfloat(r6.z) * 2.0 + (-(vd_asfloat(r5.w)))));
        r2.w = vd_asuint(((-(vd_asfloat(r0.w))) + vd_asfloat(r2.w)));
        r3.w = vd_asuint((vd_asfloat(r7.z) * 2.0 + (-(vd_asfloat(r4.w)))));
        r3.w = vd_asuint(((-(vd_asfloat(r0.w))) + vd_asfloat(r3.w)));
        r2.w = vd_asuint((-(int)(abs(vd_asfloat(r2.w)) < abs(vd_asfloat(r3.w)))));
        r1.y = ((r2.w != 0u) ? 0x00000000u : 0x00000001u);
      }
      if (r1.y == 0u) {
        r8.xyz = ((r1.xxx != 0u) ? r6.xyz : r5.xyz);
        r6.xyz = ((r1.xxx != 0u) ? r4.xyz : r6.xyz);
      } else {
        r8.xyz = ((r1.xxx != 0u) ? r4.xyz : r7.xyz);
        r6.xyz = ((r1.xxx != 0u) ? r7.xyz : r5.xyz);
      }
      r1.x = vd_asuint(((-(vd_asfloat(r6.y))) + 1.0));
      r1.y = vd_asuint((vd_asfloat(r6.z) * vd_asfloat(_ProjectionParams2.x)));
      r4.xyz = vd_asuint((vd_asfloat(_CameraViewXExtent[0].zxy) * vd_asfloat(r6.xxx) + vd_asfloat(_CameraViewTopLeftCorner[0].zxy)));
      r4.xyz = vd_asuint((vd_asfloat(_CameraViewYExtent[0].zxy) * vd_asfloat(r1.xxx) + vd_asfloat(r4.xyz)));
      r4.xyz = vd_asuint((vd_asfloat(r4.xyz) * vd_asfloat(r1.yyy) + (-(vd_asfloat(r3.yzx)))));
      r1.x = vd_asuint(((-(vd_asfloat(r8.y))) + 1.0));
      r1.y = vd_asuint((vd_asfloat(r8.z) * vd_asfloat(_ProjectionParams2.x)));
      r5.xyz = vd_asuint((vd_asfloat(_CameraViewXExtent[0].yzx) * vd_asfloat(r8.xxx) + vd_asfloat(_CameraViewTopLeftCorner[0].yzx)));
      r5.xyz = vd_asuint((vd_asfloat(_CameraViewYExtent[0].yzx) * vd_asfloat(r1.xxx) + vd_asfloat(r5.xyz)));
      r3.xyz = vd_asuint((vd_asfloat(r5.xyz) * vd_asfloat(r1.yyy) + (-(vd_asfloat(r3.xyz)))));
      r5.xyz = vd_asuint((vd_asfloat(r3.xyz) * vd_asfloat(r4.xyz)));
      r3.xyz = vd_asuint((vd_asfloat(r4.zxy) * vd_asfloat(r3.yzx) + (-(vd_asfloat(r5.xyz)))));
      r1.x = vd_asuint(dot(vd_asfloat(r3.xyz), vd_asfloat(r3.xyz)));
      r1.x = vd_asuint(rsqrt(vd_asfloat(r1.x)));
      r2.xyz = vd_asuint((vd_asfloat(r1.xxx) * vd_asfloat(r3.xyz)));
    }
  }
  r1.x = vd_asuint(((-(vd_asfloat(v1.y))) + 1.0));
  r1.y = vd_asuint((vd_asfloat(r0.w) * vd_asfloat(_ProjectionParams2.x)));
  r3.xyz = vd_asuint((vd_asfloat(_CameraViewXExtent[0].xyz) * vd_asfloat(v1.xxx) + vd_asfloat(_CameraViewTopLeftCorner[0].xyz)));
  r3.xyz = vd_asuint((vd_asfloat(_CameraViewYExtent[0].xyz) * vd_asfloat(r1.xxx) + vd_asfloat(r3.xyz)));
  r3.xyz = vd_asuint((vd_asfloat(r1.yyy) * vd_asfloat(r3.xyz)));
  r1.x = vd_asuint(((float)vd_asint(_SSAOSampleCount)));
  r1.x = vd_asuint((1.0 / vd_asfloat(r1.x)));
  r4.xy = vd_asuint((vd_asfloat(v1.xy) * vd_asfloat(_ScaledScreenParams.xy)));
  r4.zw = vd_asuint(((-(vd_asfloat(_SSAOParams.yx))) + vd_asfloat(float2(_CustomSSAOOverrideRadius, _CustomSSAOOverrideIntensity))));
  r4.zw = vd_asuint((vd_asfloat((_CustomSSAOEnableOverride).xx) * vd_asfloat(r4.zw) + vd_asfloat(_SSAOParams.yx)));
  r1.y = vd_asuint((vd_asfloat(r4.z) * vd_asfloat(_EditorViewConsts.y)));
  r5.x = vd_asuint((VD_COL(unity_MatrixV, 0).z));
  r5.y = vd_asuint((VD_COL(unity_MatrixV, 1).z));
  r5.z = vd_asuint((VD_COL(unity_MatrixV, 2).z));
  r2.w = 0x00000000u;
  r6.x = 0xbf800000u;
  r3.w = 0x00000000u;
  while (true) {
    r4.z = vd_asuint((-(int)(vd_asint(r3.w) >= vd_asint(_SSAOSampleCount))));
    if (r4.z != 0u) break;
    r4.z = vd_asuint(((float)vd_asint(r3.w)));
    r6.zw = vd_asuint((vd_asfloat(r4.zz) * float2(2.08299994, 4.8670001)));
    r6.zw = vd_asuint((vd_asfloat(r4.xy) * vd_asfloat(_SSAOParams.zz) + vd_asfloat(r6.zw)));
    r5.w = vd_asuint(dot(vd_asfloat(r6.zw), float2(0.0671105608, 0.00583714992)));
    r5.w = vd_asuint(frac(vd_asfloat(r5.w)));
    r5.w = vd_asuint((vd_asfloat(r5.w) * 52.9829178));
    r5.w = vd_asuint(frac(vd_asfloat(r5.w)));
    r6.z = ((uint)vd_asfloat(r4.z));
    r6.z = x0[r6.z + 0].x;
    r6.z = vd_asuint((vd_asfloat(r5.w) + vd_asfloat(r6.z)));
    r6.z = vd_asuint(frac(vd_asfloat(r6.z)));
    r7.z = vd_asuint((vd_asfloat(r6.z) * 2.0 + -1.0));
    r4.z = vd_asuint((vd_asfloat(r4.z) + 20.0));
    r4.z = ((uint)vd_asfloat(r4.z));
    r4.z = x0[r4.z + 0].x;
    r4.z = vd_asuint((vd_asfloat(r5.w) + vd_asfloat(r4.z)));
    r4.z = vd_asuint((vd_asfloat(r4.z) * 6.28318548));
    r5.w = vd_asuint(((-(vd_asfloat(r7.z))) * vd_asfloat(r7.z) + 1.0));
    r5.w = vd_asuint(sqrt(vd_asfloat(r5.w)));
    r8.x = vd_asuint(sin(vd_asfloat(r4.z)));
    r9.x = vd_asuint(cos(vd_asfloat(r4.z)));
    r7.x = vd_asuint((vd_asfloat(r5.w) * vd_asfloat(r9.x)));
    r7.y = vd_asuint((vd_asfloat(r5.w) * vd_asfloat(r8.x)));
    r6.xy = vd_asuint((vd_asfloat(r6.xx) + float2(1.0, 2.0)));
    r4.z = vd_asuint((vd_asfloat(r1.x) * vd_asfloat(r6.y)));
    r4.z = vd_asuint(sqrt(vd_asfloat(r4.z)));
    r6.yzw = vd_asuint((vd_asfloat(r4.zzz) * vd_asfloat(r7.xyz)));
    r4.z = vd_asuint(dot((-(vd_asfloat(r2.xyz))), vd_asfloat(r6.yzw)));
    r4.z = vd_asuint((-(int)(vd_asfloat(r4.z) >= 0.0)));
    r6.yzw = vd_asuint(((r4.zzz != 0u) ? (-(vd_asfloat(r6.yzw))) : vd_asfloat(r6.yzw)));
    r6.yzw = vd_asuint((vd_asfloat(r6.yzw) * vd_asfloat(r1.yyy) + vd_asfloat(r3.xyz)));
    r7.xy = vd_asuint((vd_asfloat(r6.zz) * vd_asfloat(float2(_CameraViewProjections[0][0][1], _CameraViewProjections[0][1][1]))));
    r7.xy = vd_asuint((vd_asfloat(float2(_CameraViewProjections[0][0][0], _CameraViewProjections[0][1][0])) * vd_asfloat(r6.yy) + vd_asfloat(r7.xy)));
    r7.xy = vd_asuint((vd_asfloat(float2(_CameraViewProjections[0][0][2], _CameraViewProjections[0][1][2])) * vd_asfloat(r6.ww) + vd_asfloat(r7.xy)));
    r4.z = vd_asuint(dot(vd_asfloat(r5.xyz), vd_asfloat(r6.yzw)));
    r5.w = vd_asuint((1.0 / (-(vd_asfloat(r4.z)))));
    r6.yz = vd_asuint((vd_asfloat(r7.xy) * vd_asfloat(r5.ww) + float2(1.0, 1.0)));
    r6.yz = vd_asuint(saturate((vd_asfloat(r6.yz) * float2(0.5, 0.5))));
    r7.xy = vd_asuint((vd_asfloat(r0.xy) * vd_asfloat(r0.zz) + vd_asfloat(r6.yz)));
    r7.xyzw = vd_asuint(_CameraDepthTexture.SampleLevel(sampler_vd_point_clamp, vd_asfloat(r7.xy), 0.0).xyzw);
#if UNITY_UV_STARTS_AT_TOP
    r5.w = vd_asuint((vd_asfloat(_ZBufferParams.z) * vd_asfloat(r7.x) + vd_asfloat(_ZBufferParams.w)));
#else
    r7.x = vd_asuint(((-(vd_asfloat(r7.x))) + 1.0));
    r5.w = vd_asuint((vd_asfloat(vd_ZBufferParamsD3D.z) * vd_asfloat(r7.x) + vd_asfloat(vd_ZBufferParamsD3D.w)));
#endif
    r5.w = vd_asuint((1.0 / vd_asfloat(r5.w)));
    r4.z = vd_asuint(((-(vd_asfloat(r4.z))) + (-(vd_asfloat(r5.w)))));
    r4.z = vd_asuint((-(int)(abs(vd_asfloat(r4.z)) < vd_asfloat(r1.y))));
    r4.z = (r4.z & 0x3f800000u);
    r6.w = vd_asuint((-(int)(9.99999975e-06 < vd_asfloat(r7.x))));
    r6.w = (r6.w & 0x3f800000u);
    r4.z = vd_asuint((vd_asfloat(r4.z) * vd_asfloat(r6.w)));
    r6.z = vd_asuint(((-(vd_asfloat(r6.z))) + 1.0));
    r5.w = vd_asuint((vd_asfloat(r5.w) * vd_asfloat(_ProjectionParams2.x)));
    r7.xyz = vd_asuint((vd_asfloat(_CameraViewXExtent[0].xyz) * vd_asfloat(r6.yyy) + vd_asfloat(_CameraViewTopLeftCorner[0].xyz)));
    r6.yzw = vd_asuint((vd_asfloat(_CameraViewYExtent[0].xyz) * vd_asfloat(r6.zzz) + vd_asfloat(r7.xyz)));
    r6.yzw = vd_asuint((vd_asfloat(r6.yzw) * vd_asfloat(r5.www) + (-(vd_asfloat(r3.xyz)))));
    r5.w = vd_asuint(dot(vd_asfloat(r6.yzw), vd_asfloat(r2.xyz)));
    r5.w = vd_asuint(((-(vd_asfloat(r0.w))) * 0.00400000019 + vd_asfloat(r5.w)));
    r5.w = vd_asuint(max(vd_asfloat(r5.w), 0.0));
    r6.y = vd_asuint(dot(vd_asfloat(r6.yzw), vd_asfloat(r6.yzw)));
    r6.y = vd_asuint((vd_asfloat(r6.y) + 9.99999975e-05));
    r6.y = vd_asuint((1.0 / vd_asfloat(r6.y)));
    r5.w = vd_asuint((vd_asfloat(r5.w) * vd_asfloat(r6.y)));
    r2.w = vd_asuint((vd_asfloat(r5.w) * vd_asfloat(r4.z) + vd_asfloat(r2.w)));
    r3.w = vd_asuint((vd_asint(r3.w) + 1));
  }
  r0.x = vd_asuint((vd_asfloat(r1.y) * vd_asfloat(r2.w)));
  r0.y = vd_asuint((1.0 / vd_asfloat(_SSAOParams.w)));
  r0.y = vd_asuint(((-(vd_asfloat(r0.w))) * vd_asfloat(r0.y) + 1.0));
  r0.y = vd_asuint((vd_asfloat(r0.y) * vd_asfloat(r0.y)));
  r0.x = vd_asuint((vd_asfloat(r4.w) * vd_asfloat(r0.x)));
  r0.x = vd_asuint((vd_asfloat(r0.x) * vd_asfloat(_EditorViewConsts.x)));
  r0.x = vd_asuint((vd_asfloat(r0.y) * vd_asfloat(r0.x)));
  r0.x = vd_asuint(saturate((vd_asfloat(r1.x) * vd_asfloat(r0.x))));
  r0.x = vd_asuint(log2(vd_asfloat(r0.x)));
  r0.x = vd_asuint((vd_asfloat(r0.x) * 0.600000024));
  r0.x = vd_asuint(exp2(vd_asfloat(r0.x)));
  r0.y = vd_asuint((vd_asfloat(v1.y) * vd_asfloat(v1.y)));
  r0.zw = vd_asuint(min(vd_asfloat(r1.zw), vd_asfloat(v1.xy)));
  r0.zw = vd_asuint((vd_asfloat(r0.zw) * vd_asfloat(_RTHandleScale.xy)));
  r1.xyzw = vd_asuint(_CameraDepthTexture.SampleBias(sampler_vd_point_clamp, vd_asfloat(r0.zw), vd_asfloat(_GlobalMipBias.x)).xyzw);
#if UNITY_UV_STARTS_AT_TOP
#else
  r1.x = vd_asuint(((-(vd_asfloat(r1.x))) + 1.0));
#endif
  r0.zw = vd_asuint((vd_asfloat(v1.xy) * float2(2.0, 2.0) + float2(-1.0, -1.0)));
#if UNITY_UV_STARTS_AT_TOP
  r3.xyzw = vd_asuint(((-(vd_asfloat(r0.wwww))) * vd_asfloat((VD_COL(unity_MatrixInvP, 1).xyzw))));
  r3.xyzw = vd_asuint((vd_asfloat((VD_COL(unity_MatrixInvP, 0).xyzw)) * vd_asfloat(r0.zzzz) + vd_asfloat(r3.xyzw)));
  r1.xyzw = vd_asuint((vd_asfloat((VD_COL(unity_MatrixInvP, 2).xyzw)) * vd_asfloat(r1.xxxx) + vd_asfloat(r3.xyzw)));
  r1.xyzw = vd_asuint((vd_asfloat(r1.xyzw) + vd_asfloat((VD_COL(unity_MatrixInvP, 3).xyzw))));
#else
  r3.xyzw = vd_asuint((vd_asfloat(r0.wwww) * vd_asfloat((VD_COL(vd_MatrixInvPD3D, 1).xyzw))));
  r3.xyzw = vd_asuint((vd_asfloat((VD_COL(vd_MatrixInvPD3D, 0).xyzw)) * vd_asfloat(r0.zzzz) + vd_asfloat(r3.xyzw)));
  r1.xyzw = vd_asuint((vd_asfloat((VD_COL(vd_MatrixInvPD3D, 2).xyzw)) * vd_asfloat(r1.xxxx) + vd_asfloat(r3.xyzw)));
  r1.xyzw = vd_asuint((vd_asfloat(r1.xyzw) + vd_asfloat((VD_COL(vd_MatrixInvPD3D, 3).xyzw))));
#endif
  r1.xyz = vd_asuint((vd_asfloat(r1.xyz) * float3(1.0, 1.0, -1.0)));
  r1.xyz = vd_asuint((vd_asfloat(r1.xyz) / vd_asfloat(r1.www)));
  r3.xyz = vd_asuint((vd_asfloat(r1.yyy) * vd_asfloat((VD_COL(unity_MatrixInvV, 1).xyz))));
  r1.xyw = vd_asuint((vd_asfloat((VD_COL(unity_MatrixInvV, 0).xyz)) * vd_asfloat(r1.xxx) + vd_asfloat(r3.xyz)));
  r1.xyz = vd_asuint((vd_asfloat((VD_COL(unity_MatrixInvV, 2).xyz)) * (-(vd_asfloat(r1.zzz))) + vd_asfloat(r1.xyw)));
  r1.xyz = vd_asuint((vd_asfloat(r1.xyz) + vd_asfloat((VD_COL(unity_MatrixInvV, 3).xyz))));
  r1.xyz = vd_asuint((vd_asfloat(r1.xyz) + (-(vd_asfloat(_WorldSpaceCameraPos.xyz)))));
  r0.z = vd_asuint(((-(vd_asfloat(_WorldSpaceCameraPos.y))) + -0.200000003));
  r0.z = vd_asuint((vd_asfloat(r0.z) / vd_asfloat(r1.y)));
  r0.zw = vd_asuint((vd_asfloat(r1.xz) * vd_asfloat(r0.zz) + vd_asfloat(_WorldSpaceCameraPos.xz)));
  r0.zw = vd_asuint((vd_asfloat(r0.zw) + (-(vd_asfloat(_PlaneBoundMaxPos.xz)))));
  r1.xy = vd_asuint((vd_asfloat(_PlaneBoundMinPos.xz) + (-(vd_asfloat(_PlaneBoundMaxPos.xz)))));
  r0.zw = vd_asuint(saturate((vd_asfloat(r0.zw) / vd_asfloat(r1.xy))));
  r1.xyzw = vd_asuint(_PlaneSightRenderTarget.SampleLevel(sampler_PlaneSightRenderTarget, vd_asfloat(r0.zw), 0.0).xyzw);
  r0.z = vd_asuint((vd_asfloat(r1.w) + -1.0));
  r0.z = vd_asuint((vd_asfloat(_GlobalPlaneSystemEnable) * vd_asfloat(r0.z) + 1.0));
  r0.z = vd_asuint(((-(vd_asfloat(r0.z))) * vd_asfloat(r0.z) + 1.0));
  r0.z = vd_asuint(max(vd_asfloat(r0.z), 0.0));
  r0.w = vd_asuint((vd_asfloat(_GlobalPlaneSystemEnable) * vd_asfloat(_GlobalBackgroundSeeThroughEnable)));
  r0.z = vd_asuint((vd_asfloat(r0.z) + -1.0));
  r0.z = vd_asuint((vd_asfloat(r0.w) * vd_asfloat(r0.z) + 1.0));
  r0.x = vd_asuint((vd_asfloat(r0.y) * (-(vd_asfloat(r0.x))) + vd_asfloat(r0.x)));
  r0.y = vd_asuint(saturate(((-(vd_asfloat(r0.z))) + 1.0)));
  r0.x = vd_asuint((vd_asfloat(r0.y) * (-(vd_asfloat(r0.x))) + vd_asfloat(r0.x)));
  o0.x = ((-(vd_asfloat(r0.x))) + 1.0);
  o0.yzw = (vd_asfloat(r2.xyz) * float3(0.5, 0.5, 0.5) + float3(0.5, 0.5, 0.5));
  return;
}
