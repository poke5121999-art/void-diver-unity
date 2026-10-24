// Hidden/Shader_DE_PP_Snapshot_Stroke | pass 1 "Blit" | progFragment | keywords: (none) | blob 3
void frag(
  float4 v0 : SV_POSITION0,
  float4 v1 : VDREG1,
  float4 v2 : VDREG2,
  out float4 o0 : SV_TARGET0)
{
#if UNITY_UV_STARTS_AT_TOP
#else
  float4 vd_ZBufferParamsD3D = float4(-_ZBufferParams.x, _ZBufferParams.x + _ZBufferParams.y, -_ZBufferParams.z, _ZBufferParams.z + _ZBufferParams.w);
  float4x4 vd_MatrixInvVPD3D = mul(unity_MatrixInvVP, float4x4(1, 0, 0, 0, 0, 1, 0, 0, 0, 0, -2, 1, 0, 0, 0, 1));
#endif
  uint4 r0, r1, r2, r3, r4, r5;
  r0 = 0u; r1 = 0u; r2 = 0u; r3 = 0u; r4 = 0u; r5 = 0u;

  r0.x = vd_asuint((-(int)(vd_asfloat(unity_OrthoParams.w) == 1.0)));
  if (r0.x != 0u) {
    r0.xy = vd_asuint(((-(vd_asfloat(_CameraDepthTexture_TexelSize.xy))) * float2(0.5, 0.5) + float2(1.0, 1.0)));
    r0.xy = vd_asuint(min(vd_asfloat(r0.xy), vd_asfloat(v1.xy)));
    r0.xy = vd_asuint((vd_asfloat(r0.xy) * vd_asfloat(_RTHandleScale.xy)));
    r0.xyzw = vd_asuint(_CameraDepthTexture.SampleBias(sampler_vd_point_clamp, vd_asfloat(r0.xy), vd_asfloat(_GlobalMipBias.x)).xyzw);
#if UNITY_UV_STARTS_AT_TOP
#else
    r0.x = vd_asuint(((-(vd_asfloat(r0.x))) + 1.0));
#endif
    r0.yz = vd_asuint((vd_asfloat(v1.xy) * float2(2.0, 2.0) + float2(-1.0, -1.0)));
#if UNITY_UV_STARTS_AT_TOP
    r1.xyzw = vd_asuint(((-(vd_asfloat(r0.zzzz))) * vd_asfloat((VD_COL(unity_MatrixInvVP, 1).xyzw))));
    r1.xyzw = vd_asuint((vd_asfloat((VD_COL(unity_MatrixInvVP, 0).xyzw)) * vd_asfloat(r0.yyyy) + vd_asfloat(r1.xyzw)));
    r0.xyzw = vd_asuint((vd_asfloat((VD_COL(unity_MatrixInvVP, 2).xyzw)) * vd_asfloat(r0.xxxx) + vd_asfloat(r1.xyzw)));
    r0.xyzw = vd_asuint((vd_asfloat(r0.xyzw) + vd_asfloat((VD_COL(unity_MatrixInvVP, 3).xyzw))));
#else
    r1.xyzw = vd_asuint((vd_asfloat(r0.zzzz) * vd_asfloat((VD_COL(vd_MatrixInvVPD3D, 1).xyzw))));
    r1.xyzw = vd_asuint((vd_asfloat((VD_COL(vd_MatrixInvVPD3D, 0).xyzw)) * vd_asfloat(r0.yyyy) + vd_asfloat(r1.xyzw)));
    r0.xyzw = vd_asuint((vd_asfloat((VD_COL(vd_MatrixInvVPD3D, 2).xyzw)) * vd_asfloat(r0.xxxx) + vd_asfloat(r1.xyzw)));
    r0.xyzw = vd_asuint((vd_asfloat(r0.xyzw) + vd_asfloat((VD_COL(vd_MatrixInvVPD3D, 3).xyzw))));
#endif
    r0.xyz = vd_asuint((vd_asfloat(r0.xyz) / vd_asfloat(r0.www)));
    r0.y = vd_asuint((vd_asfloat(r0.y) * vd_asfloat((VD_COL(unity_MatrixV, 1).z))));
    r0.x = vd_asuint((vd_asfloat((VD_COL(unity_MatrixV, 0).z)) * vd_asfloat(r0.x) + vd_asfloat(r0.y)));
    r0.x = vd_asuint((vd_asfloat((VD_COL(unity_MatrixV, 2).z)) * vd_asfloat(r0.z) + vd_asfloat(r0.x)));
    r0.x = vd_asuint((vd_asfloat(r0.x) + vd_asfloat((VD_COL(unity_MatrixV, 3).z))));
    r0.x = vd_asuint(abs(vd_asfloat(r0.x)));
  } else {
    r0.yz = vd_asuint(((-(vd_asfloat(_CameraDepthTexture_TexelSize.xy))) * float2(0.5, 0.5) + float2(1.0, 1.0)));
    r0.yz = vd_asuint(min(vd_asfloat(r0.yz), vd_asfloat(v1.xy)));
    r0.yz = vd_asuint((vd_asfloat(r0.yz) * vd_asfloat(_RTHandleScale.xy)));
    r1.xyzw = vd_asuint(_CameraDepthTexture.SampleBias(sampler_vd_point_clamp, vd_asfloat(r0.yz), vd_asfloat(_GlobalMipBias.x)).xyzw);
#if UNITY_UV_STARTS_AT_TOP
    r0.y = vd_asuint((vd_asfloat(_ZBufferParams.z) * vd_asfloat(r1.x) + vd_asfloat(_ZBufferParams.w)));
#else
    r1.x = vd_asuint(((-(vd_asfloat(r1.x))) + 1.0));
    r0.y = vd_asuint((vd_asfloat(vd_ZBufferParamsD3D.z) * vd_asfloat(r1.x) + vd_asfloat(vd_ZBufferParamsD3D.w)));
#endif
    r0.x = vd_asuint((1.0 / vd_asfloat(r0.y)));
  }
  r0.x = vd_asuint(((-(vd_asfloat((VD_COL(unity_MatrixV, 1).z)))) * vd_asfloat(r0.x) + vd_asfloat(_WorldSpaceCameraPos.y)));
  r0.x = vd_asuint((-(int)(vd_asfloat(r0.x) >= 2.0)));
  r0.x = (r0.x & 0x3f800000u);
  r0.yzw = vd_asuint((vd_asfloat(_CeilingColor.xyz) + (-(vd_asfloat(_FloorColor.xyz)))));
  r0.xyz = vd_asuint((vd_asfloat(r0.xxx) * vd_asfloat(r0.yzw) + vd_asfloat(_FloorColor.xyz)));
  r1.xy = vd_asuint((vd_asfloat(v1.xy) * vd_asfloat(_ScreenSize.xy)));
  r1.xy = ((uint2)vd_asfloat(r1.xy));
  r1.zw = uint2(0x00000000u, 0x00000000u);
  r1.xyzw = vd_asuint(_BlitTexture.Load(vd_asint(r1.xyw)).xyzw);
  r1.xyz = vd_asuint((vd_asfloat(r1.www) * vd_asfloat(r1.xyz)));
  r2.xyz = vd_asuint(((-(vd_asfloat(r0.xyz))) + vd_asfloat(_WallColor.xyz)));
  r0.xyz = vd_asuint((vd_asfloat(r1.xyz) * vd_asfloat(r2.xyz) + vd_asfloat(r0.xyz)));
  r2.xyz = vd_asuint((vd_asfloat(r1.www) * vd_asfloat(r0.xyz)));
  r3.xy = vd_asuint((vd_asfloat(v1.xy) + vd_asfloat((_Stroke).xx)));
  r3.xy = vd_asuint((vd_asfloat(r3.xy) * vd_asfloat(_ScreenSize.xy)));
  r3.xy = ((uint2)vd_asfloat(r3.xy));
  r3.zw = uint2(0x00000000u, 0x00000000u);
  r3.xyzw = vd_asuint(_BlitTexture.Load(vd_asint(r3.xyw)).xyzw);
  r3.xyz = vd_asuint(((-(vd_asfloat(r3.xyz))) * vd_asfloat(r3.www) + vd_asfloat(r1.xyz)));
  r4.xy = vd_asuint((vd_asfloat((_Stroke).xx) * float2(1.0, -1.0) + vd_asfloat(v1.xy)));
  r4.xy = vd_asuint((vd_asfloat(r4.xy) * vd_asfloat(_ScreenSize.xy)));
  r4.xy = ((uint2)vd_asfloat(r4.xy));
  r4.zw = uint2(0x00000000u, 0x00000000u);
  r4.xyzw = vd_asuint(_BlitTexture.Load(vd_asint(r4.xyw)).xyzw);
  r4.xyz = vd_asuint(((-(vd_asfloat(r4.xyz))) * vd_asfloat(r4.www) + vd_asfloat(r1.xyz)));
  r3.xyz = vd_asuint((abs(vd_asfloat(r3.xyz)) + abs(vd_asfloat(r4.xyz))));
  r5.xy = vd_asuint((vd_asfloat(v1.xy) + (-(vd_asfloat((_Stroke).xx)))));
  r5.xy = vd_asuint((vd_asfloat(r5.xy) * vd_asfloat(_ScreenSize.xy)));
  r5.xy = ((uint2)vd_asfloat(r5.xy));
  r5.zw = uint2(0x00000000u, 0x00000000u);
  r5.xyzw = vd_asuint(_BlitTexture.Load(vd_asint(r5.xyw)).xyzw);
  r1.xyz = vd_asuint(((-(vd_asfloat(r5.xyz))) * vd_asfloat(r5.www) + vd_asfloat(r1.xyz)));
  r1.xyz = vd_asuint((abs(vd_asfloat(r1.xyz)) + abs(vd_asfloat(r4.xyz))));
  r1.xyz = vd_asuint((vd_asfloat(r1.xyz) + vd_asfloat(r3.xyz)));
  r0.xyz = vd_asuint(((-(vd_asfloat(r0.xyz))) * vd_asfloat(r1.www) + vd_asfloat(_StrokeColor.xyz)));
  o0.xyz = (vd_asfloat(r1.xyz) * vd_asfloat(r0.xyz) + vd_asfloat(r2.xyz));
  o0.w = vd_asfloat(r1.w);
  return;
}
