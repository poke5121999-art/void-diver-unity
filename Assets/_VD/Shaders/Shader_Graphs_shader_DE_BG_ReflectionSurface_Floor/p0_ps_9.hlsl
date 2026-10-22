// Shader Graphs/shader_DE_BG_ReflectionSurface_Floor | pass 0 "DBufferProjector" | progFragment | keywords: FOG_LINEAR _DBUFFER_MRT3 _DE_EDITOR_CAMERA | blob 9
void frag(
  float4 v0 : SV_POSITION0,
  float3 v1 : INTERP0,
  out float4 o0 : SV_Target0,
  out float4 o1 : SV_Target1,
  out float4 o2 : SV_Target2)
{
  float4 r0,r1;
  uint4 bitmask, uiDest;
  float4 fDest;

  r0.xy = _ScreenSize.zw * v0.xy;
  r0.xy = r0.xy * float2(2,2) + float2(-1,-1);
  r1.xyzw = (VD_COL(unity_MatrixInvVP, 1).xyzw) * -r0.yyyy;
  r0.xyzw = (VD_COL(unity_MatrixInvVP, 0).xyzw) * r0.xxxx + r1.xyzw;
  r1.xy = (uint2)v0.xy;
  r1.zw = float2(0,0);
  r1.x = _CameraDepthTexture.Load(r1.xyz).x;
  r0.xyzw = (VD_COL(unity_MatrixInvVP, 2).xyzw) * r1.xxxx + r0.xyzw;
  r0.xyzw = (VD_COL(unity_MatrixInvVP, 3).xyzw) + r0.xyzw;
  r0.xyz = r0.xyz / r0.www;
  r1.xyz = (VD_COL(unity_WorldToObject, 1).xyz) * r0.yyy;
  r0.xyw = (VD_COL(unity_WorldToObject, 0).xyz) * r0.xxx + r1.xyz;
  r0.xyz = (VD_COL(unity_WorldToObject, 2).xyz) * r0.zzz + r0.xyw;
  r0.xyz = (VD_COL(unity_WorldToObject, 3).xyz) + r0.xyz;
  r1.xyz = float3(1,-1,1) * r0.xyz;
  r0.w = max(abs(r1.x), abs(r1.y));
  r0.w = max(r0.w, abs(r1.z));
  r0.w = 0.5 + -r0.w;
  r0.w = cmp(r0.w < 0);
  if (r0.w != 0) discard;
  r1.xyz = (VD_COL(unity_ObjectToWorld, 1).xyz) * r0.yyy;
  r0.xyw = (VD_COL(unity_ObjectToWorld, 0).xyz) * r0.xxx + r1.xyz;
  r0.xyz = (VD_COL(unity_ObjectToWorld, 2).xyz) * r0.zzz + r0.xyw;
  r0.xyz = (VD_COL(unity_ObjectToWorld, 3).xyz) + r0.xyz;
  r0.y = (VD_COL(unity_MatrixV, 1).z) * r0.y;
  r0.x = (VD_COL(unity_MatrixV, 0).z) * r0.x + r0.y;
  r0.x = (VD_COL(unity_MatrixV, 2).z) * r0.z + r0.x;
  r0.x = (VD_COL(unity_MatrixV, 3).z) + r0.x;
  r0.x = -_ProjectionParams.y + -r0.x;
  r0.x = max(0, r0.x);
  r0.x = saturate(r0.x * unity_FogParams.z + unity_FogParams.w);
  r0.x = 1 + -r0.x;
  r0.x = 1 + -r0.x;
  r0.y = cmp(_ProjectionParams.x < 0);
  r0.z = _ScreenParams.y + -v0.y;
  r1.y = r0.y ? r0.z : v0.y;
  r1.x = v0.x;
  r1.xy = r1.xy / _ScreenParams.xy;
  r1.z = 1 + -r1.y;
  r0.yzw = _PlaneReflectionTexture.SampleBias(sampler_PlaneReflectionTexture, r1.xz, _GlobalMipBias.x).xyz;
  r0.yzw = -unity_FogColor.xyz + r0.yzw;
  o0.xyz = r0.xxx * r0.yzw + unity_FogColor.xyz;
  o0.w = 0;
  r0.x = dot((VD_COL(_NormalToWorld, 2).xyz), (VD_COL(_NormalToWorld, 2).xyz));
  r0.x = rsqrt(r0.x);
  r0.xyz = (VD_COL(_NormalToWorld, 2).xyz) * r0.xxx;
  o1.xyz = r0.xyz * float3(0.5,0.5,0.5) + float3(0.5,0.5,0.5);
  o1.w = saturate((VD_COL(_NormalToWorld, 3).x));
  o2.xyzw = float4(0,1,0,0);
  return;
}
