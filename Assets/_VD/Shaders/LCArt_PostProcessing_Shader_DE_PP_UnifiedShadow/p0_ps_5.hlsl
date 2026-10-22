// LCArt/PostProcessing/Shader_DE_PP_UnifiedShadow | pass 0 "ScreenSpaceShadowCompute" | progFragment | keywords: (none) | blob 5
void frag(
  float4 v0 : SV_POSITION0,
  float2 v1 : TEXCOORD0,
  out float4 o0 : SV_Target0)
{
  float4 r0,r1,r2,r3,r4,r5,r6,r7,r8,r9;
  uint4 bitmask, uiDest;
  float4 fDest;

  r0.x = cmp(_ScreenSpaceShadowPointCount == 0.000000);
  if (r0.x != 0) {
    o0.xyzw = float4(1,0,0,1);
    return;
  }
  r0.xyzw = _CustomDepthBuffer.SampleBias(sampler_CustomDepthBuffer, v1.xy, _GlobalMipBias.x).xyzw;
  r0.yz = v1.xy * float2(2,2) + float2(-1,-1);
  r1.xyzw = (VD_COL(unity_MatrixInvP, 1).xyzw) * -r0.zzzz;
  r1.xyzw = (VD_COL(unity_MatrixInvP, 0).xyzw) * r0.yyyy + r1.xyzw;
  r1.xyzw = (VD_COL(unity_MatrixInvP, 2).xyzw) * r0.xxxx + r1.xyzw;
  r1.xyzw = (VD_COL(unity_MatrixInvP, 3).xyzw) + r1.xyzw;
  r0.xyz = float3(1,1,-1) * r1.xyz;
  r1.xyz = r0.xyz / r1.www;
  r0.xyz = (VD_COL(unity_MatrixInvV, 1).xyz) * r1.yyy;
  r0.xyz = (VD_COL(unity_MatrixInvV, 0).xyz) * r1.xxx + r0.xyz;
  r0.xyz = (VD_COL(unity_MatrixInvV, 2).xyz) * -r1.zzz + r0.xyz;
  r0.xyz = (VD_COL(unity_MatrixInvV, 3).xyz) + r0.xyz;
  r2.x = cmp(_SSShadowIsFirstFrame < 0.5);
  if (r2.x != 0) {
    r2.yz = _ShadowRTSize.xy * v1.xy;
    r2.yz = (uint2)r2.yz;
    r2.yz = (uint2)r2.yz >> 1;
    r2.y = (int)r2.z + (int)r2.y;
    r2.y = (int)r2.y & 1;
    r2.z = (uint)_SSShadowFrameIndex;
    r2.y = cmp((int)r2.z != (int)r2.y);
    if (r2.y != 0) {
      r2.yzw = (VD_COL(_SSShadowPrevVPMatrix, 1).xyw) * r0.yyy;
      r2.yzw = (VD_COL(_SSShadowPrevVPMatrix, 0).xyw) * r0.xxx + r2.yzw;
      r2.yzw = (VD_COL(_SSShadowPrevVPMatrix, 2).xyw) * r0.zzz + r2.yzw;
      r2.yzw = (VD_COL(_SSShadowPrevVPMatrix, 3).xyw) + r2.yzw;
      r2.yz = r2.yz / r2.ww;
      r3.xy = r2.yz * float2(0.5,0.5) + float2(0.5,0.5);
      r3.z = 1 + -r3.y;
      r2.yz = cmp(r3.xz >= float2(0,0));
      r2.y = r2.z ? r2.y : 0;
      r2.zw = cmp(float2(1,1) >= r3.xz);
      r2.z = r2.w ? r2.z : 0;
      r2.y = r2.z ? r2.y : 0;
      if (r2.y != 0) {
        o0.xyzw = _PrevShadowTexture.SampleBias(sampler_vd_linear_clamp, r3.xz, _GlobalMipBias.x).xyzw;
        return;
      }
    }
  }
  r2.y = 1.5 + _GlobalShadowStandardY;
  r2.z = _GlobalShadowStandardY + -r2.y;
  r2.y = -r2.y + r0.y;
  r2.z = 1 / r2.z;
  r2.y = saturate(r2.y * r2.z);
  r2.z = r2.y * -2 + 3;
  r2.y = r2.y * r2.y;
  r2.y = r2.z * r2.y;
  r2.z = max(1, _SSShadowStepCount);
  r2.z = 1 / r2.z;
  r1.w = -r1.z;
  r3.x = (VD_COL(glstate_matrix_projection, 0).x);
  r3.y = (VD_COL(glstate_matrix_projection, 2).x);
  r3.z = (VD_COL(glstate_matrix_projection, 3).x);
  r4.w = 1;
  r5.x = (VD_COL(glstate_matrix_projection, 1).y);
  r5.y = (VD_COL(glstate_matrix_projection, 2).y);
  r5.z = (VD_COL(glstate_matrix_projection, 3).y);
  r6.x = (VD_COL(glstate_matrix_projection, 2).w);
  r6.y = (VD_COL(glstate_matrix_projection, 3).w);
  r1.z = 1;
  r2.w = 0;
  while (true) {
    r3.w = cmp((int)r2.w >= 8);
    if (r3.w != 0) break;
    r3.w = (int)r2.w;
    r3.w = cmp(r3.w >= _ScreenSpaceShadowPointCount);
    if (r3.w != 0) {
      r3.w = (int)r2.w + 1;
      r2.w = r3.w;
      continue;
    }
    r7.xyz = _ScreenSpaceShadowPointPositions[(int)(r2.w)].yyy * (VD_COL(unity_MatrixV, 1).xyz);
    r7.xyz = (VD_COL(unity_MatrixV, 0).xyz) * _ScreenSpaceShadowPointPositions[(int)(r2.w)].xxx + r7.xyz;
    r7.xyz = (VD_COL(unity_MatrixV, 2).xyz) * _ScreenSpaceShadowPointPositions[(int)(r2.w)].zzz + r7.xyz;
    r7.xyz = (VD_COL(unity_MatrixV, 3).xyz) + r7.xyz;
    r8.xyz = -r7.xyz + r1.xyw;
    r3.w = dot(r8.xyz, r8.xyz);
    r3.w = sqrt(r3.w);
    r5.w = cmp(_ScreenSpaceShadowPointRanges[(int)(r2.w)] < r3.w);
    if (r5.w != 0) {
      r5.w = (int)r2.w + 1;
      r2.w = r5.w;
      continue;
    }
    r3.w = saturate(r3.w / _ScreenSpaceShadowPointRanges[(int)(r2.w)]);
    r3.w = 1 + -r3.w;
    r5.w = cmp(r3.w < 0.00999999978);
    if (r5.w != 0) {
      r5.w = (int)r2.w + 1;
      r2.w = r5.w;
      continue;
    }
    r7.xyz = r7.xyz + -r1.xyw;
    r6.zw = float2(1,0);
    while (true) {
      r5.w = cmp(64 < (int)r6.w);
      if (r5.w != 0) break;
      r5.w = (int)r6.w;
      r7.w = cmp(_SSShadowStepCount < r5.w);
      if (r7.w != 0) {
        break;
      }
      r5.w = 0.5 + r5.w;
      r5.w = saturate(r5.w * r2.z);
      r4.xyz = r5.www * r7.xyz + r1.xyw;
      r8.x = dot(r3.xyz, r4.xzw);
      r8.y = dot(r5.xyz, r4.yzw);
      r4.x = dot(r6.xy, r4.zw);
      r4.xy = r8.xy / r4.xx;
      r8.xy = r4.xy * float2(0.5,0.5) + float2(0.5,0.5);
      r8.z = 1 + -r8.y;
      r9.xyzw = _CustomDepthBuffer.SampleLevel(sampler_CustomDepthBuffer, r8.xz, 0).xyzw;
      r4.xy = r8.xz * float2(2,2) + float2(-1,-1);
      r8.xy = (VD_COL(unity_MatrixInvP, 1).zw) * -r4.yy;
      r4.xy = (VD_COL(unity_MatrixInvP, 0).zw) * r4.xx + r8.xy;
      r4.xy = (VD_COL(unity_MatrixInvP, 2).zw) * r9.xx + r4.xy;
      r4.xy = (VD_COL(unity_MatrixInvP, 3).zw) + r4.xy;
      r4.x = -r4.x / r4.y;
      r4.x = -0.100000001 + -r4.x;
      r4.x = r4.z + -r4.x;
      r4.x = saturate(20 * r4.x);
      r4.y = r4.x * -2 + 3;
      r4.x = r4.x * r4.x;
      r4.z = r4.y * r4.x;
      r4.z = cmp(r4.z < 0.00999999978);
      if (r4.z != 0) {
        r6.z = 0;
        break;
      }
      r4.x = r4.y * r4.x + -1;
      r4.x = r2.y * r4.x + 1;
      r6.z = r6.z * r4.x;
      r6.w = (int)r6.w + 1;
    }
    r4.x = r6.z + -r1.z;
    r3.w = r3.w * r4.x + r1.z;
    r1.z = min(r3.w, r1.z);
    r2.w = (int)r2.w + 1;
  }
  r1.x = -1 + r1.z;
  r1.x = _GlobalScreenSpaceShadowAlpha * r1.x;
  r3.xyzw = _ScreenSpaceShadowMask.SampleBias(sampler_vd_linear_clamp, v1.xy, _GlobalMipBias.x).xyzw;
  r1.x = r3.x * r1.x;
  r0.w = -1 + r0.w;
  r0.w = _GlobalIsCurrentlyInStage * r0.w + 1;
  r0.w = r0.w * r1.x + 1;
  if (r2.x != 0) {
    r1.xyz = (VD_COL(_SSShadowPrevVPMatrix, 1).xyw) * r0.yyy;
    r1.xyz = (VD_COL(_SSShadowPrevVPMatrix, 0).xyw) * r0.xxx + r1.xyz;
    r0.xyz = (VD_COL(_SSShadowPrevVPMatrix, 2).xyw) * r0.zzz + r1.xyz;
    r0.xyz = (VD_COL(_SSShadowPrevVPMatrix, 3).xyw) + r0.xyz;
    r0.xy = r0.xy / r0.zz;
    r0.xy = r0.xy * float2(0.5,0.5) + float2(0.5,0.5);
    r0.z = 1 + -r0.y;
    r1.xy = cmp(r0.xz >= float2(0,0));
    r0.y = r1.y ? r1.x : 0;
    r1.xy = cmp(float2(1,1) >= r0.xz);
    r1.x = r1.y ? r1.x : 0;
    r0.y = r1.x ? r0.y : 0;
    if (r0.y != 0) {
      r1.xyzw = _PrevShadowTexture.SampleBias(sampler_vd_linear_clamp, r0.xz, _GlobalMipBias.x).xyzw;
      r0.x = -14.4269505 * unity_DeltaTime.x;
      r0.x = exp2(r0.x);
      r0.x = 1 + -r0.x;
      r0.y = -r1.x + r0.w;
      r0.w = r0.x * r0.y + r1.x;
    }
  }
  o0.x = r0.w;
  o0.yzw = float3(0,0,1);
  return;
}
