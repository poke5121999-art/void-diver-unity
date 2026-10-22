// ArtTeam/VFX/VFX_Master_typeA_Trail | pass 2 "MotionVectors" | progFragment | keywords: _ALPHATEST_ON | blob 38
void frag(
  float3 v0 : CLIP_POSITION_NO_JITTER0,
  float3 v1 : PREVIOUS_CLIP_POSITION_NO_JITTER0,
  float4 v2 : SV_POSITION0,
  float4 v3 : INTERP0,
  float4 v4 : INTERP1,
  float3 v5 : INTERP2,
  float3 v6 : INTERP3,
  out float4 o0 : SV_Target0)
{
  float4 r0,r1,r2;
  uint4 bitmask, uiDest;
  float4 fDest;

  r0.x = cmp(_ProjectionParams.x < 0);
  r0.y = _ScaledScreenParams.y + -v2.y;
  r0.x = r0.x ? r0.y : v2.y;
  r0.x = r0.x / _ScaledScreenParams.y;
  r0.x = 1 + -r0.x;
  r0.y = 1 + _ScreenAlphaSmoothness;
  r0.y = _ScreenAlphaDissolve * r0.y + -_ScreenAlphaSmoothness;
  r0.x = r0.x + -r0.y;
  r0.y = 1 / _ScreenAlphaSmoothness;
  r0.x = saturate(r0.x * r0.y);
  r0.y = r0.x * -2 + 3;
  r0.x = r0.x * r0.x;
  r0.z = _DeformSpeed * _TimeParameters.x;
  r0.z = 0.0166666675 * r0.z;
  r0.w = cmp(r0.z >= -r0.z);
  r0.z = frac(abs(r0.z));
  r0.z = r0.w ? r0.z : -r0.z;
  r0.z = 60 * r0.z;
  r0.zw = r0.zz * _DeformDirection.xy + v3.xy;
  r0.zw = r0.zw * _DeformTex_ST.xy + _DeformTex_ST.zw;
  r1.xyzw = _DeformTex.SampleBias(sampler_DeformTex, r0.zw, _GlobalMipBias.x).xyzw;
  r1.x = r1.w * r1.x;
  r0.zw = r1.xy * float2(2,2) + float2(-1,-1);
  r1.x = _MainTexFlowSpeed * _TimeParameters.x;
  r1.y = cmp(r1.x >= -r1.x);
  r1.x = frac(abs(r1.x));
  r1.x = r1.y ? r1.x : -r1.x;
  r1.xy = r1.xx * float2(1,0) + v3.xy;
  r0.zw = (_DeformStrength).xx * r0.zw + r1.xy;
  r1.xy = r0.zw * _DissolveTex_ST.xy + _DissolveTex_ST.zw;
  r0.zw = r0.zw * _MainTex_ST.xy + _MainTex_ST.zw;
  r2.xyzw = _MainTex.SampleBias(sampler_MainTex, r0.zw, _GlobalMipBias.x).xyzw;
  r0.z = v4.w * r2.w;
  r1.xyzw = _DissolveTex.SampleBias(sampler_DissolveTex, r1.xy, _GlobalMipBias.x).xyzw;
  r0.x = r0.y * r0.x + -r1.x;
  r0.y = 1 + -r1.x;
  r0.y = 1 / r0.y;
  r0.x = saturate(r0.x * r0.y);
  r0.y = r0.x * -2 + 3;
  r0.x = r0.x * r0.x;
  r0.x = r0.y * r0.x;
  r0.y = saturate(_Dissolve);
  r0.w = saturate(v4.w);
  r0.y = r0.y * r0.w + r1.x;
  r0.y = max(0, r0.y);
  r0.y = log2(r0.y);
  r0.y = _DissovePower * r0.y;
  r0.y = exp2(r0.y);
  r0.xy = min(float2(1,1), r0.xy);
  r1.xy = v3.xy * _MaskTex_ST.xy + _MaskTex_ST.zw;
  r1.xyzw = _MaskTex.SampleBias(sampler_MaskTex, r1.xy, _GlobalMipBias.x).xyzw;
  r0.w = r1.x * r1.w;
  r0.z = r0.z * r0.w;
  r0.y = r0.y * r0.z;
  r0.x = r0.y * r0.x + -r0.y;
  r0.x = _ScreenAlphaStrength * r0.x + r0.y;
  r0.y = dot(v6.xyz, v6.xyz);
  r0.y = sqrt(r0.y);
  r0.y = 1 / r0.y;
  r0.yzw = v6.xyz * r0.yyy;
  r1.x = dot(r0.yzw, r0.yzw);
  r1.x = rsqrt(r1.x);
  r0.yzw = r1.xxx * r0.yzw;
  r1.xyz = _WorldSpaceCameraPos.xyz + -v5.xyz;
  r1.w = dot(r1.xyz, r1.xyz);
  r1.w = rsqrt(r1.w);
  r1.xyz = r1.xyz * r1.www;
  r1.w = cmp(unity_OrthoParams.w == 0.000000);
  r2.x = r1.w ? r1.x : (VD_COL(unity_MatrixV, 0).z);
  r2.y = r1.w ? r1.y : (VD_COL(unity_MatrixV, 1).z);
  r2.z = r1.w ? r1.z : (VD_COL(unity_MatrixV, 2).z);
  r0.y = saturate(dot(r0.yzw, r2.xyz));
  r0.y = 1 + -r0.y;
  r0.y = 1 + -r0.y;
  r0.z = 1 + _FresnelMask_Threshold;
  r0.z = _FresnelMask_Offset * r0.z + -_FresnelMask_Threshold;
  r0.y = r0.y + -r0.z;
  r0.z = _FresnelMask_Offset + -r0.z;
  r0.z = 1 / r0.z;
  r0.y = saturate(r0.y * r0.z);
  r0.z = r0.y * -2 + 3;
  r0.y = r0.y * r0.y;
  r0.y = r0.z * r0.y;
  r0.y = min(1, r0.y);
  r0.z = r0.y * -2 + 3;
  r0.y = r0.y * r0.y;
  r0.y = r0.z * r0.y;
  r0.x = r0.x * r0.y + -_AlphaThreshold;
  r0.x = cmp(r0.x < 0);
  if (r0.x != 0) discard;
  r0.x = 1 / v1.z;
  r0.xy = v1.xy * r0.xx;
  r0.z = 1 / v0.z;
  r0.xy = v0.xy * r0.zz + -r0.xy;
  r0.xy = float2(0.5,-0.5) * r0.xy;
  r0.z = cmp(unity_MotionVectorsParams.y != 0.000000);
  o0.xy = r0.zz ? r0.xy : 0;
  o0.zw = float2(0,0);
  return;
}
