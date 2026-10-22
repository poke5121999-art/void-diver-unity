// Hidden/Shader_DE_PP_Snapshot_Stroke | pass 0 "DrawProcedural" | progFragment | keywords: (none) | blob 3
void frag(
  float4 v0 : SV_POSITION0,
  float4 v1 : INTERP0,
  float4 v2 : INTERP1,
  out float4 o0 : SV_TARGET0)
{
  float4 r0,r1,r2,r3,r4,r5;
  uint4 bitmask, uiDest;
  float4 fDest;

  r0.x = cmp(unity_OrthoParams.w == 1.000000);
  if (r0.x != 0) {
    r0.xy = -_CameraDepthTexture_TexelSize.xy * float2(0.5,0.5) + float2(1,1);
    r0.xy = min(v1.xy, r0.xy);
    r0.xy = _RTHandleScale.xy * r0.xy;
    r0.xyzw = _CameraDepthTexture.SampleBias(sampler_vd_point_clamp, r0.xy, _GlobalMipBias.x).xyzw;
    r0.yz = v1.xy * float2(2,2) + float2(-1,-1);
    r1.xyzw = (VD_COL(unity_MatrixInvVP, 1).xyzw) * -r0.zzzz;
    r1.xyzw = (VD_COL(unity_MatrixInvVP, 0).xyzw) * r0.yyyy + r1.xyzw;
    r0.xyzw = (VD_COL(unity_MatrixInvVP, 2).xyzw) * r0.xxxx + r1.xyzw;
    r0.xyzw = (VD_COL(unity_MatrixInvVP, 3).xyzw) + r0.xyzw;
    r0.xyz = r0.xyz / r0.www;
    r0.y = (VD_COL(unity_MatrixV, 1).z) * r0.y;
    r0.x = (VD_COL(unity_MatrixV, 0).z) * r0.x + r0.y;
    r0.x = (VD_COL(unity_MatrixV, 2).z) * r0.z + r0.x;
    r0.x = (VD_COL(unity_MatrixV, 3).z) + r0.x;
    r0.x = abs(r0.x);
  } else {
    r0.yz = -_CameraDepthTexture_TexelSize.xy * float2(0.5,0.5) + float2(1,1);
    r0.yz = min(v1.xy, r0.yz);
    r0.yz = _RTHandleScale.xy * r0.yz;
    r1.xyzw = _CameraDepthTexture.SampleBias(sampler_vd_point_clamp, r0.yz, _GlobalMipBias.x).xyzw;
    r0.y = _ZBufferParams.z * r1.x + _ZBufferParams.w;
    r0.x = 1 / r0.y;
  }
  r0.x = -(VD_COL(unity_MatrixV, 1).z) * r0.x + _WorldSpaceCameraPos.y;
  r0.x = cmp(r0.x >= 2);
  r0.x = r0.x ? 1.000000 : 0;
  r0.yzw = -_FloorColor.xyz + _CeilingColor.xyz;
  r0.xyz = r0.xxx * r0.yzw + _FloorColor.xyz;
  r1.xy = _ScreenSize.xy * v1.xy;
  r1.xy = (uint2)r1.xy;
  r1.zw = float2(0,0);
  r1.xyzw = _BlitTexture.Load(r1.xyz).xyzw;
  r1.xyz = r1.xyz * r1.www;
  r2.xyz = _WallColor.xyz + -r0.xyz;
  r0.xyz = r1.xyz * r2.xyz + r0.xyz;
  r2.xyz = r0.xyz * r1.www;
  r3.xy = (_Stroke).xx + v1.xy;
  r3.xy = _ScreenSize.xy * r3.xy;
  r3.xy = (uint2)r3.xy;
  r3.zw = float2(0,0);
  r3.xyzw = _BlitTexture.Load(r3.xyz).xyzw;
  r3.xyz = -r3.xyz * r3.www + r1.xyz;
  r4.xy = (_Stroke).xx * float2(1,-1) + v1.xy;
  r4.xy = _ScreenSize.xy * r4.xy;
  r4.xy = (uint2)r4.xy;
  r4.zw = float2(0,0);
  r4.xyzw = _BlitTexture.Load(r4.xyz).xyzw;
  r4.xyz = -r4.xyz * r4.www + r1.xyz;
  r3.xyz = abs(r4.xyz) + abs(r3.xyz);
  r5.xy = -(_Stroke).xx + v1.xy;
  r5.xy = _ScreenSize.xy * r5.xy;
  r5.xy = (uint2)r5.xy;
  r5.zw = float2(0,0);
  r5.xyzw = _BlitTexture.Load(r5.xyz).xyzw;
  r1.xyz = -r5.xyz * r5.www + r1.xyz;
  r1.xyz = abs(r4.xyz) + abs(r1.xyz);
  r1.xyz = r3.xyz + r1.xyz;
  r0.xyz = -r0.xyz * r1.www + _StrokeColor.xyz;
  o0.xyz = r1.xyz * r0.xyz + r2.xyz;
  o0.w = r1.w;
  return;
}
