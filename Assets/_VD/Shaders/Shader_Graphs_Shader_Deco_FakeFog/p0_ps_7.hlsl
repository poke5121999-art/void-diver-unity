// Shader Graphs/Shader_Deco_FakeFog | pass 0 "Universal Forward" | progFragment | keywords: FOG_LINEAR _DBUFFER_MRT3 | blob 7
void frag(
  float4 v0 : SV_POSITION0,
  float3 v1 : INTERP0,
  float3 v2 : INTERP1,
  out float4 o0 : SV_Target0)
{
  float4 r0,r1,r2,r3;
  uint4 bitmask, uiDest;
  float4 fDest;

  r0.x = (VD_COL(unity_MatrixVP, 1).w) * v1.y;
  r0.x = (VD_COL(unity_MatrixVP, 0).w) * v1.x + r0.x;
  r0.x = (VD_COL(unity_MatrixVP, 2).w) * v1.z + r0.x;
  r0.x = (VD_COL(unity_MatrixVP, 3).w) + r0.x;
  r0.y = cmp(_ProjectionParams.x < 0);
  r0.z = _ScaledScreenParams.y + -v0.y;
  r1.y = r0.y ? r0.z : v0.y;
  r1.x = v0.x;
  r1.xy = r1.xy / _ScaledScreenParams.xy;
  r0.y = cmp(unity_OrthoParams.w == 1.000000);
  if (r0.y != 0) {
    r0.yz = -_CameraDepthTexture_TexelSize.xy * float2(0.5,0.5) + float2(1,1);
    r1.z = 1 + -r1.y;
    r0.yz = min(r1.xz, r0.yz);
    r0.yz = _RTHandleScale.xy * r0.yz;
    r2.xyzw = _CameraDepthTexture.SampleBias(sampler_vd_point_clamp, r0.yz, _GlobalMipBias.x).xyzw;
    r0.yz = r1.xz * float2(2,2) + float2(-1,-1);
    r3.xyzw = (VD_COL(unity_MatrixInvVP, 1).xyzw) * -r0.zzzz;
    r3.xyzw = (VD_COL(unity_MatrixInvVP, 0).xyzw) * r0.yyyy + r3.xyzw;
    r2.xyzw = (VD_COL(unity_MatrixInvVP, 2).xyzw) * r2.xxxx + r3.xyzw;
    r2.xyzw = (VD_COL(unity_MatrixInvVP, 3).xyzw) + r2.xyzw;
    r0.yzw = r2.xyz / r2.www;
    r0.z = (VD_COL(unity_MatrixV, 1).z) * r0.z;
    r0.y = (VD_COL(unity_MatrixV, 0).z) * r0.y + r0.z;
    r0.y = (VD_COL(unity_MatrixV, 2).z) * r0.w + r0.y;
    r0.y = (VD_COL(unity_MatrixV, 3).z) + r0.y;
    r0.y = abs(r0.y);
  } else {
    r0.zw = -_CameraDepthTexture_TexelSize.xy * float2(0.5,0.5) + float2(1,1);
    r1.w = 1 + -r1.y;
    r0.zw = min(r1.xw, r0.zw);
    r0.zw = _RTHandleScale.xy * r0.zw;
    r2.xyzw = _CameraDepthTexture.SampleBias(sampler_vd_point_clamp, r0.zw, _GlobalMipBias.x).xyzw;
    r0.z = _ZBufferParams.z * r2.x + _ZBufferParams.w;
    r0.y = 1 / r0.z;
  }
  r0.x = r0.y + -r0.x;
  r0.y = _DepthMax + -_DepthMin;
  r0.x = -_DepthMin + abs(r0.x);
  r0.y = 1 / r0.y;
  r0.x = saturate(r0.x * r0.y);
  r0.y = r0.x * -2 + 3;
  r0.x = r0.x * r0.x;
  r0.x = r0.y * r0.x;
  r0.yz = -_CameraDepthTexture_TexelSize.xy * float2(0.5,0.5) + float2(1,1);
  r1.xy = r1.xy * float2(1,-1) + float2(0,1);
  r0.yz = min(r1.xy, r0.yz);
  r0.yz = _RTHandleScale.xy * r0.yz;
  r1.xyzw = _CameraDepthTexture.SampleBias(sampler_vd_point_clamp, r0.yz, _GlobalMipBias.x).xyzw;
  r0.y = _ZBufferParams.x * r1.x + _ZBufferParams.y;
  r0.y = 1 / r0.y;
  r0.y = cmp(r0.y >= 0.99000001);
  o0.w = r0.y ? 0 : r0.x;
  o0.xyz = unity_FogColor.xyz;
  return;
}
