// Shader Graphs/Shader_DE_GroundNavigation | pass 4 "Universal 2D" | progFragment | keywords: (none) | blob 29
void frag(
  float4 v0 : SV_POSITION0,
  float4 v1 : VDREG1,
  float4 v2 : VDREG2,
  float4 v3 : VDREG3,
  out float4 o0 : SV_TARGET0)
{
  uint4 r0, r1, r2, r3, r4;
  r0 = 0u; r1 = 0u; r2 = 0u; r3 = 0u; r4 = 0u;

  r0.x = vd_asuint((vd_asfloat(v3.y) * vd_asfloat((VD_COL(unity_MatrixVP, 1).w))));
  r0.x = vd_asuint((vd_asfloat((VD_COL(unity_MatrixVP, 0).w)) * vd_asfloat(v3.x) + vd_asfloat(r0.x)));
  r0.x = vd_asuint((vd_asfloat((VD_COL(unity_MatrixVP, 2).w)) * vd_asfloat(v3.z) + vd_asfloat(r0.x)));
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
  r0.yz = vd_asuint((vd_asfloat(r2.xy) + vd_asfloat(v1.xy)));
  r2.xyzw = vd_asuint(_MainTex.SampleBias(sampler_MainTex, vd_asfloat(r0.yz), vd_asfloat(_GlobalMipBias.x)).xyzw);
  r2.xyzw = vd_asuint((vd_asfloat(r2.xyzw) * vd_asfloat(_Color.xyzw)));
  r0.y = vd_asuint(saturate((vd_asfloat(v2.y) * 20.0)));
  r0.z = vd_asuint((vd_asfloat(r0.y) * -2.0 + 3.0));
  r0.y = vd_asuint((vd_asfloat(r0.y) * vd_asfloat(r0.y)));
  r0.y = vd_asuint((vd_asfloat(r0.y) * vd_asfloat(r0.z)));
  r0.z = vd_asuint((vd_asfloat(v2.y) + -1.0));
  r0.z = vd_asuint(saturate((vd_asfloat(r0.z) * -19.9999962)));
  r0.w = vd_asuint((vd_asfloat(r0.z) * -2.0 + 3.0));
  r0.z = vd_asuint((vd_asfloat(r0.z) * vd_asfloat(r0.z)));
  r0.z = vd_asuint((vd_asfloat(r0.z) * vd_asfloat(r0.w)));
  r0.y = vd_asuint((vd_asfloat(r0.z) * vd_asfloat(r0.y)));
  r0.z = vd_asuint((-(int)(vd_asfloat(unity_OrthoParams.w) == 1.0)));
  if (r0.z != 0u) {
    r0.zw = vd_asuint(((-(vd_asfloat(_CameraDepthTexture_TexelSize.xy))) * float2(0.5, 0.5) + float2(1.0, 1.0)));
    r1.z = vd_asuint(((-(vd_asfloat(r1.y))) + 1.0));
    r0.zw = vd_asuint(min(vd_asfloat(r0.zw), vd_asfloat(r1.xz)));
    r0.zw = vd_asuint((vd_asfloat(r0.zw) * vd_asfloat(_RTHandleScale.xy)));
    r3.xyzw = vd_asuint(_CameraDepthTexture.SampleBias(sampler_vd_point_clamp, vd_asfloat(r0.zw), vd_asfloat(_GlobalMipBias.x)).xyzw);
    r0.zw = vd_asuint((vd_asfloat(r1.xz) * float2(2.0, 2.0) + float2(-1.0, -1.0)));
    r4.xyzw = vd_asuint(((-(vd_asfloat(r0.wwww))) * vd_asfloat((VD_COL(unity_MatrixInvVP, 1).xyzw))));
    r4.xyzw = vd_asuint((vd_asfloat((VD_COL(unity_MatrixInvVP, 0).xyzw)) * vd_asfloat(r0.zzzz) + vd_asfloat(r4.xyzw)));
    r3.xyzw = vd_asuint((vd_asfloat((VD_COL(unity_MatrixInvVP, 2).xyzw)) * vd_asfloat(r3.xxxx) + vd_asfloat(r4.xyzw)));
    r3.xyzw = vd_asuint((vd_asfloat(r3.xyzw) + vd_asfloat((VD_COL(unity_MatrixInvVP, 3).xyzw))));
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
    r0.w = vd_asuint((vd_asfloat(_ZBufferParams.z) * vd_asfloat(r1.x) + vd_asfloat(_ZBufferParams.w)));
    r0.z = vd_asuint((1.0 / vd_asfloat(r0.w)));
  }
  r0.x = vd_asuint(((-(vd_asfloat(r0.x))) + vd_asfloat(r0.z)));
  r0.x = vd_asuint((abs(vd_asfloat(r0.x)) + abs(vd_asfloat(r0.x))));
  r0.x = vd_asuint(min(vd_asfloat(r0.x), 1.0));
  r0.z = vd_asuint((vd_asfloat(r0.x) * -2.0 + 3.0));
  r0.x = vd_asuint((vd_asfloat(r0.x) * vd_asfloat(r0.x)));
  r0.x = vd_asuint((vd_asfloat(r0.x) * vd_asfloat(r0.z)));
  r0.x = vd_asuint((vd_asfloat(r0.x) * vd_asfloat(r0.y)));
  r0.y = vd_asuint((vd_asfloat(r0.x) * vd_asfloat(r2.w)));
  o0.xyz = (vd_asfloat(r0.yyy) * vd_asfloat(r2.xyz));
  r0.x = vd_asuint((vd_asfloat(r2.w) * vd_asfloat(r0.x) + -0.00999999978));
  r0.x = vd_asuint((-(int)(vd_asfloat(r0.x) < 0.0)));
  if (r0.x != 0u) discard;
  o0.w = vd_asfloat(r0.y);
  return;
}
