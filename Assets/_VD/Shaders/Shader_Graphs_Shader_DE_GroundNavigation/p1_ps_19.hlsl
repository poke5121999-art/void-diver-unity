// Shader Graphs/Shader_DE_GroundNavigation | pass 1 "MotionVectors" | progFragment | keywords: (none) | blob 19
void frag(
  float3 v0 : CLIP_POSITION_NO_JITTER0,
  float3 v1 : PREVIOUS_CLIP_POSITION_NO_JITTER0,
  float4 v2 : SV_POSITION0,
  float4 v3 : INTERP0,
  float4 v4 : INTERP1,
  float3 v5 : INTERP2,
  out float4 o0 : SV_Target0)
{
  float4 r0,r1,r2,r3;
  uint4 bitmask, uiDest;
  float4 fDest;

  r0.x = (VD_COL(unity_MatrixVP, 1).w) * v5.y;
  r0.x = (VD_COL(unity_MatrixVP, 0).w) * v5.x + r0.x;
  r0.x = (VD_COL(unity_MatrixVP, 2).w) * v5.z + r0.x;
  r0.x = (VD_COL(unity_MatrixVP, 3).w) + r0.x;
  r0.y = cmp(_ProjectionParams.x < 0);
  r0.z = _ScaledScreenParams.y + -v2.y;
  r1.y = r0.y ? r0.z : v2.y;
  r1.x = v2.x;
  r1.xy = r1.xy / _ScaledScreenParams.xy;
  r2.y = _Speed * _TimeParameters.x;
  r2.x = 0;
  r0.yz = v3.xy + r2.xy;
  r2.xyzw = _MainTex.SampleBias(sampler_MainTex, r0.yz, _GlobalMipBias.x).xyzw;
  r0.y = _Color.w * r2.w;
  r0.z = saturate(20 * v4.y);
  r0.w = r0.z * -2 + 3;
  r0.z = r0.z * r0.z;
  r0.z = r0.w * r0.z;
  r0.w = -1 + v4.y;
  r0.w = saturate(-19.9999962 * r0.w);
  r2.x = r0.w * -2 + 3;
  r0.w = r0.w * r0.w;
  r0.w = r2.x * r0.w;
  r0.z = r0.z * r0.w;
  r0.w = cmp(unity_OrthoParams.w == 1.000000);
  if (r0.w != 0) {
    r2.xy = -_CameraDepthTexture_TexelSize.xy * float2(0.5,0.5) + float2(1,1);
    r1.z = 1 + -r1.y;
    r2.xy = min(r2.xy, r1.xz);
    r2.xy = _RTHandleScale.xy * r2.xy;
    r2.xyzw = _CameraDepthTexture.SampleBias(sampler_vd_point_clamp, r2.xy, _GlobalMipBias.x).xyzw;
    r2.yz = r1.xz * float2(2,2) + float2(-1,-1);
    r3.xyzw = (VD_COL(unity_MatrixInvVP, 1).xyzw) * -r2.zzzz;
    r3.xyzw = (VD_COL(unity_MatrixInvVP, 0).xyzw) * r2.yyyy + r3.xyzw;
    r2.xyzw = (VD_COL(unity_MatrixInvVP, 2).xyzw) * r2.xxxx + r3.xyzw;
    r2.xyzw = (VD_COL(unity_MatrixInvVP, 3).xyzw) + r2.xyzw;
    r2.xyz = r2.xyz / r2.www;
    r0.w = (VD_COL(unity_MatrixV, 1).z) * r2.y;
    r0.w = (VD_COL(unity_MatrixV, 0).z) * r2.x + r0.w;
    r0.w = (VD_COL(unity_MatrixV, 2).z) * r2.z + r0.w;
    r0.w = (VD_COL(unity_MatrixV, 3).z) + r0.w;
    r0.w = abs(r0.w);
  } else {
    r2.xy = -_CameraDepthTexture_TexelSize.xy * float2(0.5,0.5) + float2(1,1);
    r1.w = 1 + -r1.y;
    r1.xy = min(r2.xy, r1.xw);
    r1.xy = _RTHandleScale.xy * r1.xy;
    r1.xyzw = _CameraDepthTexture.SampleBias(sampler_vd_point_clamp, r1.xy, _GlobalMipBias.x).xyzw;
    r1.x = _ZBufferParams.z * r1.x + _ZBufferParams.w;
    r0.w = 1 / r1.x;
  }
  r0.x = r0.w + -r0.x;
  r0.x = abs(r0.x) + abs(r0.x);
  r0.x = min(1, r0.x);
  r0.w = r0.x * -2 + 3;
  r0.x = r0.x * r0.x;
  r0.x = r0.w * r0.x;
  r0.x = r0.z * r0.x;
  r0.x = r0.y * r0.x + -0.00999999978;
  r0.x = cmp(r0.x < 0);
  if (r0.x != 0) discard;
  r0.x = cmp(unity_MotionVectorsParams.y != 0.000000);
  r0.y = 1 / v0.z;
  r0.z = 1 / v1.z;
  r0.zw = v1.xy * r0.zz;
  r0.yz = v0.xy * r0.yy + -r0.zw;
  r0.yz = float2(0.5,-0.5) * r0.yz;
  o0.xy = r0.xx ? r0.yz : 0;
  o0.zw = float2(0,0);
  return;
}
