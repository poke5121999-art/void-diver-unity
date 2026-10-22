// Shader Graphs/Shader_DE_GroundNavigation | pass 4 "Universal 2D" | progFragment | keywords: (none) | blob 29
void frag(
  float4 v0 : SV_POSITION0,
  float4 v1 : INTERP0,
  float4 v2 : INTERP1,
  float3 v3 : INTERP2,
  out float4 o0 : SV_TARGET0)
{
  float4 r0,r1,r2,r3,r4;
  uint4 bitmask, uiDest;
  float4 fDest;

  r0.x = (VD_COL(unity_MatrixVP, 1).w) * v3.y;
  r0.x = (VD_COL(unity_MatrixVP, 0).w) * v3.x + r0.x;
  r0.x = (VD_COL(unity_MatrixVP, 2).w) * v3.z + r0.x;
  r0.x = (VD_COL(unity_MatrixVP, 3).w) + r0.x;
  r0.y = cmp(_ProjectionParams.x < 0);
  r0.z = _ScaledScreenParams.y + -v0.y;
  r1.y = r0.y ? r0.z : v0.y;
  r1.x = v0.x;
  r1.xy = r1.xy / _ScaledScreenParams.xy;
  r2.y = _Speed * _TimeParameters.x;
  r2.x = 0;
  r0.yz = v1.xy + r2.xy;
  r2.xyzw = _MainTex.SampleBias(sampler_MainTex, r0.yz, _GlobalMipBias.x).xyzw;
  r2.xyzw = _Color.xyzw * r2.xyzw;
  r0.y = saturate(20 * v2.y);
  r0.z = r0.y * -2 + 3;
  r0.y = r0.y * r0.y;
  r0.y = r0.z * r0.y;
  r0.z = -1 + v2.y;
  r0.z = saturate(-19.9999962 * r0.z);
  r0.w = r0.z * -2 + 3;
  r0.z = r0.z * r0.z;
  r0.z = r0.w * r0.z;
  r0.y = r0.y * r0.z;
  r0.z = cmp(unity_OrthoParams.w == 1.000000);
  if (r0.z != 0) {
    r0.zw = -_CameraDepthTexture_TexelSize.xy * float2(0.5,0.5) + float2(1,1);
    r1.z = 1 + -r1.y;
    r0.zw = min(r1.xz, r0.zw);
    r0.zw = _RTHandleScale.xy * r0.zw;
    r3.xyzw = _CameraDepthTexture.SampleBias(sampler_vd_point_clamp, r0.zw, _GlobalMipBias.x).xyzw;
    r0.zw = r1.xz * float2(2,2) + float2(-1,-1);
    r4.xyzw = (VD_COL(unity_MatrixInvVP, 1).xyzw) * -r0.wwww;
    r4.xyzw = (VD_COL(unity_MatrixInvVP, 0).xyzw) * r0.zzzz + r4.xyzw;
    r3.xyzw = (VD_COL(unity_MatrixInvVP, 2).xyzw) * r3.xxxx + r4.xyzw;
    r3.xyzw = (VD_COL(unity_MatrixInvVP, 3).xyzw) + r3.xyzw;
    r3.xyz = r3.xyz / r3.www;
    r0.z = (VD_COL(unity_MatrixV, 1).z) * r3.y;
    r0.z = (VD_COL(unity_MatrixV, 0).z) * r3.x + r0.z;
    r0.z = (VD_COL(unity_MatrixV, 2).z) * r3.z + r0.z;
    r0.z = (VD_COL(unity_MatrixV, 3).z) + r0.z;
    r0.z = abs(r0.z);
  } else {
    r3.xy = -_CameraDepthTexture_TexelSize.xy * float2(0.5,0.5) + float2(1,1);
    r1.w = 1 + -r1.y;
    r1.xy = min(r3.xy, r1.xw);
    r1.xy = _RTHandleScale.xy * r1.xy;
    r1.xyzw = _CameraDepthTexture.SampleBias(sampler_vd_point_clamp, r1.xy, _GlobalMipBias.x).xyzw;
    r0.w = _ZBufferParams.z * r1.x + _ZBufferParams.w;
    r0.z = 1 / r0.w;
  }
  r0.x = r0.z + -r0.x;
  r0.x = abs(r0.x) + abs(r0.x);
  r0.x = min(1, r0.x);
  r0.z = r0.x * -2 + 3;
  r0.x = r0.x * r0.x;
  r0.x = r0.z * r0.x;
  r0.x = r0.y * r0.x;
  r0.y = r2.w * r0.x;
  o0.xyz = r2.xyz * r0.yyy;
  r0.x = r2.w * r0.x + -0.00999999978;
  r0.x = cmp(r0.x < 0);
  if (r0.x != 0) discard;
  o0.w = r0.y;
  return;
}
