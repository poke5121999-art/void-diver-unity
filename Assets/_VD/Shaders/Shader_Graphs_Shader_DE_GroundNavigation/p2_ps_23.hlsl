// Shader Graphs/Shader_DE_GroundNavigation | pass 2 "DepthNormals" | progFragment | keywords: (none) | blob 23
void frag(
  float4 v0 : SV_POSITION0,
  float4 v1 : INTERP0,
  float4 v2 : INTERP1,
  float4 v3 : INTERP2,
  float3 v4 : INTERP3,
  float3 v5 : INTERP4,
  out float4 o0 : SV_Target0)
{
  float4 r0,r1,r2,r3;
  uint4 bitmask, uiDest;
  float4 fDest;

  r0.x = (VD_COL(unity_MatrixVP, 1).w) * v4.y;
  r0.x = (VD_COL(unity_MatrixVP, 0).w) * v4.x + r0.x;
  r0.x = (VD_COL(unity_MatrixVP, 2).w) * v4.z + r0.x;
  r0.x = (VD_COL(unity_MatrixVP, 3).w) + r0.x;
  r0.y = cmp(_ProjectionParams.x < 0);
  r0.z = _ScaledScreenParams.y + -v0.y;
  r1.y = r0.y ? r0.z : v0.y;
  r1.x = v0.x;
  r1.xy = r1.xy / _ScaledScreenParams.xy;
  r2.y = _Speed * _TimeParameters.x;
  r2.x = 0;
  r0.yz = v2.xy + r2.xy;
  r2.xyzw = _MainTex.SampleBias(sampler_MainTex, r0.yz, _GlobalMipBias.x).xyzw;
  r0.y = _Color.w * r2.w;
  r0.z = saturate(20 * v3.y);
  r0.w = r0.z * -2 + 3;
  r0.z = r0.z * r0.z;
  r0.z = r0.w * r0.z;
  r0.w = -1 + v3.y;
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
  r0.x = dot(v5.xyz, v5.xyz);
  r0.x = rsqrt(r0.x);
  o0.xyz = v5.xyz * r0.xxx;
  o0.w = 0;
  return;
}
