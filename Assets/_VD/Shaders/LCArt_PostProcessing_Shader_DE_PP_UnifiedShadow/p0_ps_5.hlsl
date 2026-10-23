// LCArt/PostProcessing/Shader_DE_PP_UnifiedShadow | pass 0 "ScreenSpaceShadowCompute" | progFragment | keywords: (none) | blob 5
void frag(
  float4 v0 : SV_POSITION0,
  float4 v1 : VDREG1,
  out float4 o0 : SV_Target0)
{
  uint4 r0, r1, r2, r3, r4, r5, r6, r7, r8, r9;
  r0 = 0u; r1 = 0u; r2 = 0u; r3 = 0u; r4 = 0u; r5 = 0u; r6 = 0u; r7 = 0u; r8 = 0u; r9 = 0u;

  r0.x = vd_asuint((-(int)(vd_asfloat(_ScreenSpaceShadowPointCount) == 0.0)));
  if (r0.x != 0u) {
    o0.xyzw = vd_asfloat(uint4(0x3f800000u, 0x00000000u, 0x00000000u, 0x3f800000u));
    return;
  }
  r0.xyzw = vd_asuint(_CustomDepthBuffer.SampleBias(sampler_CustomDepthBuffer, vd_asfloat(v1.xy), vd_asfloat(_GlobalMipBias.x)).xyzw);
  r0.yz = vd_asuint((vd_asfloat(v1.xy) * float2(2.0, 2.0) + float2(-1.0, -1.0)));
  r1.xyzw = vd_asuint(((-(vd_asfloat(r0.zzzz))) * vd_asfloat((VD_COL(unity_MatrixInvP, 1).xyzw))));
  r1.xyzw = vd_asuint((vd_asfloat((VD_COL(unity_MatrixInvP, 0).xyzw)) * vd_asfloat(r0.yyyy) + vd_asfloat(r1.xyzw)));
  r1.xyzw = vd_asuint((vd_asfloat((VD_COL(unity_MatrixInvP, 2).xyzw)) * vd_asfloat(r0.xxxx) + vd_asfloat(r1.xyzw)));
  r1.xyzw = vd_asuint((vd_asfloat(r1.xyzw) + vd_asfloat((VD_COL(unity_MatrixInvP, 3).xyzw))));
  r0.xyz = vd_asuint((vd_asfloat(r1.xyz) * float3(1.0, 1.0, -1.0)));
  r1.xyz = vd_asuint((vd_asfloat(r0.xyz) / vd_asfloat(r1.www)));
  r0.xyz = vd_asuint((vd_asfloat(r1.yyy) * vd_asfloat((VD_COL(unity_MatrixInvV, 1).xyz))));
  r0.xyz = vd_asuint((vd_asfloat((VD_COL(unity_MatrixInvV, 0).xyz)) * vd_asfloat(r1.xxx) + vd_asfloat(r0.xyz)));
  r0.xyz = vd_asuint((vd_asfloat((VD_COL(unity_MatrixInvV, 2).xyz)) * (-(vd_asfloat(r1.zzz))) + vd_asfloat(r0.xyz)));
  r0.xyz = vd_asuint((vd_asfloat(r0.xyz) + vd_asfloat((VD_COL(unity_MatrixInvV, 3).xyz))));
  r2.x = vd_asuint((-(int)(vd_asfloat(_SSShadowIsFirstFrame) < 0.5)));
  if (r2.x != 0u) {
    r2.yz = vd_asuint((vd_asfloat(v1.xy) * vd_asfloat(_ShadowRTSize.xy)));
    r2.yz = ((uint2)vd_asfloat(r2.yz));
    r2.yz = (r2.yz >> (uint2)1);
    r2.y = vd_asuint((vd_asint(r2.z) + vd_asint(r2.y)));
    r2.y = (r2.y & 0x00000001u);
    r2.z = ((uint)vd_asfloat(_SSShadowFrameIndex));
    r2.y = vd_asuint((-(int)(vd_asint(r2.z) != vd_asint(r2.y))));
    if (r2.y != 0u) {
      r2.yzw = vd_asuint((vd_asfloat(r0.yyy) * vd_asfloat((VD_COL(_SSShadowPrevVPMatrix, 1).xyw))));
      r2.yzw = vd_asuint((vd_asfloat((VD_COL(_SSShadowPrevVPMatrix, 0).xyw)) * vd_asfloat(r0.xxx) + vd_asfloat(r2.yzw)));
      r2.yzw = vd_asuint((vd_asfloat((VD_COL(_SSShadowPrevVPMatrix, 2).xyw)) * vd_asfloat(r0.zzz) + vd_asfloat(r2.yzw)));
      r2.yzw = vd_asuint((vd_asfloat(r2.yzw) + vd_asfloat((VD_COL(_SSShadowPrevVPMatrix, 3).xyw))));
      r2.yz = vd_asuint((vd_asfloat(r2.yz) / vd_asfloat(r2.ww)));
      r3.xy = vd_asuint((vd_asfloat(r2.yz) * float2(0.5, 0.5) + float2(0.5, 0.5)));
      r3.z = vd_asuint(((-(vd_asfloat(r3.y))) + 1.0));
      r2.yz = vd_asuint((-(int2)(vd_asfloat(r3.xz) >= float2(0.0, 0.0))));
      r2.y = (r2.z & r2.y);
      r2.zw = vd_asuint((-(int2)(float2(1.0, 1.0) >= vd_asfloat(r3.xz))));
      r2.z = (r2.w & r2.z);
      r2.y = (r2.z & r2.y);
      if (r2.y != 0u) {
        o0.xyzw = _PrevShadowTexture.SampleBias(sampler_vd_linear_clamp, vd_asfloat(r3.xz), vd_asfloat(_GlobalMipBias.x)).xyzw;
        return;
      }
    }
  }
  r2.y = vd_asuint((vd_asfloat(_GlobalShadowStandardY) + 1.5));
  r2.z = vd_asuint(((-(vd_asfloat(r2.y))) + vd_asfloat(_GlobalShadowStandardY)));
  r2.y = vd_asuint((vd_asfloat(r0.y) + (-(vd_asfloat(r2.y)))));
  r2.z = vd_asuint((1.0 / vd_asfloat(r2.z)));
  r2.y = vd_asuint(saturate((vd_asfloat(r2.z) * vd_asfloat(r2.y))));
  r2.z = vd_asuint((vd_asfloat(r2.y) * -2.0 + 3.0));
  r2.y = vd_asuint((vd_asfloat(r2.y) * vd_asfloat(r2.y)));
  r2.y = vd_asuint((vd_asfloat(r2.y) * vd_asfloat(r2.z)));
  r2.z = vd_asuint(max(vd_asfloat(_SSShadowStepCount), 1.0));
  r2.z = vd_asuint((1.0 / vd_asfloat(r2.z)));
  r1.w = vd_asuint((-(vd_asfloat(r1.z))));
  r3.x = vd_asuint((VD_COL(glstate_matrix_projection, 0).x));
  r3.y = vd_asuint((VD_COL(glstate_matrix_projection, 2).x));
  r3.z = vd_asuint((VD_COL(glstate_matrix_projection, 3).x));
  r4.w = 0x3f800000u;
  r5.x = vd_asuint((VD_COL(glstate_matrix_projection, 1).y));
  r5.y = vd_asuint((VD_COL(glstate_matrix_projection, 2).y));
  r5.z = vd_asuint((VD_COL(glstate_matrix_projection, 3).y));
  r6.x = vd_asuint((VD_COL(glstate_matrix_projection, 2).w));
  r6.y = vd_asuint((VD_COL(glstate_matrix_projection, 3).w));
  r1.z = 0x3f800000u;
  r2.w = 0x00000000u;
  while (true) {
    r3.w = vd_asuint((-(int)(vd_asint(r2.w) >= 8)));
    if (r3.w != 0u) break;
    r3.w = vd_asuint(((float)vd_asint(r2.w)));
    r3.w = vd_asuint((-(int)(vd_asfloat(r3.w) >= vd_asfloat(_ScreenSpaceShadowPointCount))));
    if (r3.w != 0u) {
      r3.w = vd_asuint((vd_asint(r2.w) + 1));
      r2.w = r3.w;
      continue;
    }
    r7.xyz = vd_asuint((vd_asfloat((VD_COL(unity_MatrixV, 1).xyz)) * vd_asfloat(_ScreenSpaceShadowPointPositions[(int)(r2.w)].yyy)));
    r7.xyz = vd_asuint((vd_asfloat((VD_COL(unity_MatrixV, 0).xyz)) * vd_asfloat(_ScreenSpaceShadowPointPositions[(int)(r2.w)].xxx) + vd_asfloat(r7.xyz)));
    r7.xyz = vd_asuint((vd_asfloat((VD_COL(unity_MatrixV, 2).xyz)) * vd_asfloat(_ScreenSpaceShadowPointPositions[(int)(r2.w)].zzz) + vd_asfloat(r7.xyz)));
    r7.xyz = vd_asuint((vd_asfloat(r7.xyz) + vd_asfloat((VD_COL(unity_MatrixV, 3).xyz))));
    r8.xyz = vd_asuint((vd_asfloat(r1.xyw) + (-(vd_asfloat(r7.xyz)))));
    r3.w = vd_asuint(dot(vd_asfloat(r8.xyz), vd_asfloat(r8.xyz)));
    r3.w = vd_asuint(sqrt(vd_asfloat(r3.w)));
    r5.w = vd_asuint((-(int)(vd_asfloat(_ScreenSpaceShadowPointRanges[(int)(r2.w)]) < vd_asfloat(r3.w))));
    if (r5.w != 0u) {
      r5.w = vd_asuint((vd_asint(r2.w) + 1));
      r2.w = r5.w;
      continue;
    }
    r3.w = vd_asuint(saturate((vd_asfloat(r3.w) / vd_asfloat(_ScreenSpaceShadowPointRanges[(int)(r2.w)]))));
    r3.w = vd_asuint(((-(vd_asfloat(r3.w))) + 1.0));
    r5.w = vd_asuint((-(int)(vd_asfloat(r3.w) < 0.00999999978)));
    if (r5.w != 0u) {
      r5.w = vd_asuint((vd_asint(r2.w) + 1));
      r2.w = r5.w;
      continue;
    }
    r7.xyz = vd_asuint(((-(vd_asfloat(r1.xyw))) + vd_asfloat(r7.xyz)));
    r6.zw = uint2(0x3f800000u, 0x00000000u);
    while (true) {
      r5.w = vd_asuint((-(int)(64 < vd_asint(r6.w))));
      if (r5.w != 0u) break;
      r5.w = vd_asuint(((float)vd_asint(r6.w)));
      r7.w = vd_asuint((-(int)(vd_asfloat(_SSShadowStepCount) < vd_asfloat(r5.w))));
      if (r7.w != 0u) {
        break;
      }
      r5.w = vd_asuint((vd_asfloat(r5.w) + 0.5));
      r5.w = vd_asuint(saturate((vd_asfloat(r2.z) * vd_asfloat(r5.w))));
      r4.xyz = vd_asuint((vd_asfloat(r5.www) * vd_asfloat(r7.xyz) + vd_asfloat(r1.xyw)));
      r8.x = vd_asuint(dot(vd_asfloat(r3.xyz), vd_asfloat(r4.xzw)));
      r8.y = vd_asuint(dot(vd_asfloat(r5.xyz), vd_asfloat(r4.yzw)));
      r4.x = vd_asuint(dot(vd_asfloat(r6.xy), vd_asfloat(r4.zw)));
      r4.xy = vd_asuint((vd_asfloat(r8.xy) / vd_asfloat(r4.xx)));
      r8.xy = vd_asuint((vd_asfloat(r4.xy) * float2(0.5, 0.5) + float2(0.5, 0.5)));
      r8.z = vd_asuint(((-(vd_asfloat(r8.y))) + 1.0));
      r9.xyzw = vd_asuint(_CustomDepthBuffer.SampleLevel(sampler_CustomDepthBuffer, vd_asfloat(r8.xz), 0.0).xyzw);
      r4.xy = vd_asuint((vd_asfloat(r8.xz) * float2(2.0, 2.0) + float2(-1.0, -1.0)));
      r8.xy = vd_asuint(((-(vd_asfloat(r4.yy))) * vd_asfloat((VD_COL(unity_MatrixInvP, 1).zw))));
      r4.xy = vd_asuint((vd_asfloat((VD_COL(unity_MatrixInvP, 0).zw)) * vd_asfloat(r4.xx) + vd_asfloat(r8.xy)));
      r4.xy = vd_asuint((vd_asfloat((VD_COL(unity_MatrixInvP, 2).zw)) * vd_asfloat(r9.xx) + vd_asfloat(r4.xy)));
      r4.xy = vd_asuint((vd_asfloat(r4.xy) + vd_asfloat((VD_COL(unity_MatrixInvP, 3).zw))));
      r4.x = vd_asuint(((-(vd_asfloat(r4.x))) / vd_asfloat(r4.y)));
      r4.x = vd_asuint(((-(vd_asfloat(r4.x))) + -0.100000001));
      r4.x = vd_asuint(((-(vd_asfloat(r4.x))) + vd_asfloat(r4.z)));
      r4.x = vd_asuint(saturate((vd_asfloat(r4.x) * 20.0)));
      r4.y = vd_asuint((vd_asfloat(r4.x) * -2.0 + 3.0));
      r4.x = vd_asuint((vd_asfloat(r4.x) * vd_asfloat(r4.x)));
      r4.z = vd_asuint((vd_asfloat(r4.x) * vd_asfloat(r4.y)));
      r4.z = vd_asuint((-(int)(vd_asfloat(r4.z) < 0.00999999978)));
      if (r4.z != 0u) {
        r6.z = 0x00000000u;
        break;
      }
      r4.x = vd_asuint((vd_asfloat(r4.y) * vd_asfloat(r4.x) + -1.0));
      r4.x = vd_asuint((vd_asfloat(r2.y) * vd_asfloat(r4.x) + 1.0));
      r6.z = vd_asuint((vd_asfloat(r4.x) * vd_asfloat(r6.z)));
      r6.w = vd_asuint((vd_asint(r6.w) + 1));
    }
    r4.x = vd_asuint(((-(vd_asfloat(r1.z))) + vd_asfloat(r6.z)));
    r3.w = vd_asuint((vd_asfloat(r3.w) * vd_asfloat(r4.x) + vd_asfloat(r1.z)));
    r1.z = vd_asuint(min(vd_asfloat(r1.z), vd_asfloat(r3.w)));
    r2.w = vd_asuint((vd_asint(r2.w) + 1));
  }
  r1.x = vd_asuint((vd_asfloat(r1.z) + -1.0));
  r1.x = vd_asuint((vd_asfloat(r1.x) * vd_asfloat(_GlobalScreenSpaceShadowAlpha)));
  r3.xyzw = vd_asuint(_ScreenSpaceShadowMask.SampleBias(sampler_vd_linear_clamp, vd_asfloat(v1.xy), vd_asfloat(_GlobalMipBias.x)).xyzw);
  r1.x = vd_asuint((vd_asfloat(r1.x) * vd_asfloat(r3.x)));
  r0.w = vd_asuint((vd_asfloat(r0.w) + -1.0));
  r0.w = vd_asuint((vd_asfloat(_GlobalIsCurrentlyInStage) * vd_asfloat(r0.w) + 1.0));
  r0.w = vd_asuint((vd_asfloat(r0.w) * vd_asfloat(r1.x) + 1.0));
  if (r2.x != 0u) {
    r1.xyz = vd_asuint((vd_asfloat(r0.yyy) * vd_asfloat((VD_COL(_SSShadowPrevVPMatrix, 1).xyw))));
    r1.xyz = vd_asuint((vd_asfloat((VD_COL(_SSShadowPrevVPMatrix, 0).xyw)) * vd_asfloat(r0.xxx) + vd_asfloat(r1.xyz)));
    r0.xyz = vd_asuint((vd_asfloat((VD_COL(_SSShadowPrevVPMatrix, 2).xyw)) * vd_asfloat(r0.zzz) + vd_asfloat(r1.xyz)));
    r0.xyz = vd_asuint((vd_asfloat(r0.xyz) + vd_asfloat((VD_COL(_SSShadowPrevVPMatrix, 3).xyw))));
    r0.xy = vd_asuint((vd_asfloat(r0.xy) / vd_asfloat(r0.zz)));
    r0.xy = vd_asuint((vd_asfloat(r0.xy) * float2(0.5, 0.5) + float2(0.5, 0.5)));
    r0.z = vd_asuint(((-(vd_asfloat(r0.y))) + 1.0));
    r1.xy = vd_asuint((-(int2)(vd_asfloat(r0.xz) >= float2(0.0, 0.0))));
    r0.y = (r1.y & r1.x);
    r1.xy = vd_asuint((-(int2)(float2(1.0, 1.0) >= vd_asfloat(r0.xz))));
    r1.x = (r1.y & r1.x);
    r0.y = (r0.y & r1.x);
    if (r0.y != 0u) {
      r1.xyzw = vd_asuint(_PrevShadowTexture.SampleBias(sampler_vd_linear_clamp, vd_asfloat(r0.xz), vd_asfloat(_GlobalMipBias.x)).xyzw);
      r0.x = vd_asuint((vd_asfloat(unity_DeltaTime.x) * -14.4269505));
      r0.x = vd_asuint(exp2(vd_asfloat(r0.x)));
      r0.x = vd_asuint(((-(vd_asfloat(r0.x))) + 1.0));
      r0.y = vd_asuint((vd_asfloat(r0.w) + (-(vd_asfloat(r1.x)))));
      r0.w = vd_asuint((vd_asfloat(r0.x) * vd_asfloat(r0.y) + vd_asfloat(r1.x)));
    }
  }
  o0.x = vd_asfloat(r0.w);
  o0.yzw = vd_asfloat(uint3(0x00000000u, 0x00000000u, 0x3f800000u));
  return;
}
