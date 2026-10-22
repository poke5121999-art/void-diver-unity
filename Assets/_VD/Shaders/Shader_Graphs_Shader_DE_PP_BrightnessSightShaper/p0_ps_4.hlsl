// Shader Graphs/Shader_DE_PP_BrightnessSightShaper | pass 0 "Universal Forward" | progFragment | keywords: (none) | blob 4
void frag(
  float4 v0 : SV_POSITION0,
  float4 v1 : INTERP0,
  float3 v2 : INTERP1,
  float3 v3 : INTERP2,
  out float4 o0 : SV_Target0)
{
  float4 r0,r1,r2,r3;
  uint4 bitmask, uiDest;
  float4 fDest;

  r0.x = (VD_COL(unity_MatrixVP, 1).w) * v2.y;
  r0.x = (VD_COL(unity_MatrixVP, 0).w) * v2.x + r0.x;
  r0.x = (VD_COL(unity_MatrixVP, 2).w) * v2.z + r0.x;
  r0.x = (VD_COL(unity_MatrixVP, 3).w) + r0.x;
  r0.y = cmp(_ProjectionParams.x < 0);
  r0.z = _ScaledScreenParams.y + -v0.y;
  r1.y = r0.y ? r0.z : v0.y;
  r1.x = v0.x;
  r1.xy = r1.xy / _ScaledScreenParams.xy;
  r0.yzw = (_Intensity).xxx * _Color.xyz;
  r0.yzw = float3(0.0500000007,0.0500000007,0.0500000007) * r0.yzw;
  r2.xyzw = _Texture.SampleBias(sampler_Texture, v1.xy, _GlobalMipBias.x).xyzw;
  o0.xyz = r2.xxx * r0.yzw;
  r0.yz = _PlaneBoundMaxPos.xz + -v2.xz;
  r2.xy = _PlaneBoundMaxPos.xz + -_PlaneBoundMinPos.xz;
  r0.yz = r0.yz / r2.xy;
  r2.xyzw = _PlaneSightRenderTarget.SampleBias(sampler_PlaneSightRenderTarget, r0.yz, _GlobalMipBias.x).xyzw;
  r0.y = -1 + r2.w;
  r0.y = _GlobalPlaneSystemEnable * r0.y + 1;
  r0.z = cmp(unity_OrthoParams.w == 1.000000);
  if (r0.z != 0) {
    r0.zw = -_CameraDepthTexture_TexelSize.xy * float2(0.5,0.5) + float2(1,1);
    r1.z = 1 + -r1.y;
    r0.zw = min(r1.xz, r0.zw);
    r0.zw = _RTHandleScale.xy * r0.zw;
    r2.xyzw = _CameraDepthTexture.SampleBias(sampler_vd_point_clamp, r0.zw, _GlobalMipBias.x).xyzw;
    r0.zw = r1.xz * float2(2,2) + float2(-1,-1);
    r3.xyzw = (VD_COL(unity_MatrixInvVP, 1).xyzw) * -r0.wwww;
    r3.xyzw = (VD_COL(unity_MatrixInvVP, 0).xyzw) * r0.zzzz + r3.xyzw;
    r2.xyzw = (VD_COL(unity_MatrixInvVP, 2).xyzw) * r2.xxxx + r3.xyzw;
    r2.xyzw = (VD_COL(unity_MatrixInvVP, 3).xyzw) + r2.xyzw;
    r2.xyz = r2.xyz / r2.www;
    r0.z = (VD_COL(unity_MatrixV, 1).z) * r2.y;
    r0.z = (VD_COL(unity_MatrixV, 0).z) * r2.x + r0.z;
    r0.z = (VD_COL(unity_MatrixV, 2).z) * r2.z + r0.z;
    r0.z = (VD_COL(unity_MatrixV, 3).z) + r0.z;
    r0.z = abs(r0.z);
  } else {
    r2.xy = -_CameraDepthTexture_TexelSize.xy * float2(0.5,0.5) + float2(1,1);
    r1.w = 1 + -r1.y;
    r1.xy = min(r2.xy, r1.xw);
    r1.xy = _RTHandleScale.xy * r1.xy;
    r1.xyzw = _CameraDepthTexture.SampleBias(sampler_vd_point_clamp, r1.xy, _GlobalMipBias.x).xyzw;
    r0.w = _ZBufferParams.z * r1.x + _ZBufferParams.w;
    r0.z = 1 / r0.w;
  }
  r0.x = r0.z + -r0.x;
  r0.x = abs(r0.x) + abs(r0.x);
  r0.x = min(1, r0.x);
  r0.x = r0.y * r0.x;
  o0.w = _Color.w * r0.x;
  return;
}
