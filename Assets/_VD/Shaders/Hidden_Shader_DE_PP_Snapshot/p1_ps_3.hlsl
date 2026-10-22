// Hidden/Shader_DE_PP_Snapshot | pass 1 "Blit" | progFragment | keywords: (none) | blob 3
void frag(
  float4 v0 : SV_POSITION0,
  float4 v1 : INTERP0,
  float4 v2 : INTERP1,
  out float4 o0 : SV_TARGET0)
{
  float4 r0,r1,r2,r3,r4,r5;
  uint4 bitmask, uiDest;
  float4 fDest;

  r0.xy = saturate((_Stroke).xx + v1.xy);
  r0.xy = _ScreenSize.xy * r0.xy;
  r0.xy = (uint2)r0.xy;
  r0.zw = float2(0,0);
  r0.xyzw = _BlitTexture.Load(r0.xyz).xyzw;
  r0.xy = _ScreenSize.xy * v1.xy;
  r1.xy = (uint2)r0.xy;
  r1.zw = float2(0,0);
  r1.xyzw = _BlitTexture.Load(r1.xyz).xyzw;
  r0.xy = r1.yw * r1.ww;
  r0.z = -r0.w * r0.w + r0.y;
  r2.xyzw = saturate((_Stroke).xxxx * float4(1,-1,-1,1) + v1.xyxy);
  r2.xyzw = _ScreenSize.xyxy * r2.xyzw;
  r3.xy = (uint2)r2.xy;
  r2.xy = (uint2)r2.zw;
  r3.zw = float2(0,0);
  r3.xyzw = _BlitTexture.Load(r3.xyz).xyzw;
  r0.w = -r3.w * r3.w + r0.y;
  r0.z = abs(r0.z) + abs(r0.w);
  r2.zw = float2(0,0);
  r2.xyzw = _BlitTexture.Load(r2.xyz).xyzw;
  r0.w = -r2.w * r2.w + r0.y;
  r1.xy = saturate(-(_Stroke).xx + v1.xy);
  r1.xy = _ScreenSize.xy * r1.xy;
  r2.xy = (uint2)r1.xy;
  r2.zw = float2(0,0);
  r2.xyzw = _BlitTexture.Load(r2.xyz).xyzw;
  r0.y = -r2.w * r2.w + r0.y;
  r0.y = abs(r0.w) + abs(r0.y);
  r0.y = r0.z + r0.y;
  o0.w = saturate(r0.y + r1.w);
  r1.z = -_NavMeshStroke;
  r0.y = _ScreenParams.x / _ScreenParams.y;
  r2.xw = -(_NavMeshStroke).xx;
  r1.x = r2.x * r0.y;
  r2.y = _NavMeshStroke * r0.y;
  r0.yz = v1.xy + r2.wy;
  r3.xyzw = _MapBlockMeshTexture.SampleBias(sampler_vd_point_clamp, r0.yz, _GlobalMipBias.x).xyzw;
  r0.yz = v1.xy + r1.zx;
  r4.xyzw = _MapBlockMeshTexture.SampleBias(sampler_vd_point_clamp, r0.yz, _GlobalMipBias.x).xyzw;
  r0.y = cmp(r4.y >= 0.00100000005);
  r5.xyzw = _MapBlockMeshTexture.SampleBias(sampler_vd_point_clamp, v1.xy, _GlobalMipBias.x).xyzw;
  r0.z = cmp(r5.y >= 0.00100000005);
  r0.yz = r0.yz ? float2(0,0) : float2(1,1);
  r0.z = r5.x * r0.z;
  r0.y = -r4.x * r0.y + r0.z;
  r0.w = cmp(r3.y >= 0.00100000005);
  r0.w = r0.w ? 0 : 1;
  r0.w = -r3.x * r0.w + r0.z;
  r0.y = max(abs(r0.w), abs(r0.y));
  r1.y = _NavMeshStroke;
  r1.xy = v1.xy + r1.yx;
  r1.xyzw = _MapBlockMeshTexture.SampleBias(sampler_vd_point_clamp, r1.xy, _GlobalMipBias.x).xyzw;
  r0.w = cmp(r1.y >= 0.00100000005);
  r0.w = r0.w ? 0 : 1;
  r0.w = -r1.x * r0.w + r0.z;
  r2.z = _NavMeshStroke;
  r1.xy = v1.xy + r2.zy;
  r1.xyzw = _MapBlockMeshTexture.SampleBias(sampler_vd_point_clamp, r1.xy, _GlobalMipBias.x).xyzw;
  r1.y = cmp(r1.y >= 0.00100000005);
  r1.y = r1.y ? 0 : 1;
  r1.x = -r1.x * r1.y + r0.z;
  r0.w = max(abs(r1.x), abs(r0.w));
  r0.y = max(r0.w, r0.y);
  r0.y = r0.z * r0.y;
  r1.xyzw = _MapBlockMeshTexture.SampleBias(sampler_MapBlockMeshTexture, v1.xy, _GlobalMipBias.x).xyzw;
  r0.z = cmp(r1.y >= 0.00100000005);
  r0.z = r0.z ? 0 : 1;
  r0.z = -r1.x * r0.z + 1;
  r1.xyz = _NavMeshColor.xyz + -_FloorColor.xyz;
  r1.xyz = r0.zzz * r1.xyz + _FloorColor.xyz;
  r2.xyz = _NavMeshStrokeColor.xyz + -r1.xyz;
  r0.yzw = r0.yyy * r2.xyz + r1.xyz;
  r1.xyz = _WallColor.xyz + -r0.yzw;
  o0.xyz = r0.xxx * r1.xyz + r0.yzw;
  return;
}
