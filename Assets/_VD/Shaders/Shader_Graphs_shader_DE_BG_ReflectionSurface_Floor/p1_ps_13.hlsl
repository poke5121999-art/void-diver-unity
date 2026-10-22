// Shader Graphs/shader_DE_BG_ReflectionSurface_Floor | pass 1 "DecalProjectorForwardEmissive" | progFragment | keywords: (none) | blob 13
void frag(
  float4 v0 : SV_POSITION0,
  float3 v1 : INTERP0,
  out float4 o0 : SV_Target0)
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
  r0.xyz = float3(1,-1,1) * r0.xyz;
  r0.x = max(abs(r0.x), abs(r0.y));
  r0.x = max(r0.x, abs(r0.z));
  r0.x = 0.5 + -r0.x;
  r0.x = cmp(r0.x < 0);
  if (r0.x != 0) discard;
  r0.x = cmp(_ProjectionParams.x < 0);
  r0.y = _ScreenParams.y + -v0.y;
  r0.y = r0.x ? r0.y : v0.y;
  r0.x = v0.x;
  r0.xy = r0.xy / _ScreenParams.xy;
  r0.z = 1 + -r0.y;
  r0.xyzw = _PlaneReflectionTexture.SampleBias(sampler_PlaneReflectionTexture, r0.xz, _GlobalMipBias.x).xyzw;
  r1.x = 1 + -_GlobalBackgroundColorIntensity;
  r1.x = _PlaneReflectionActive * r1.x;
  r0.w = r1.x * r0.w;
  r0.xyz = cmp(r0.xyz >= float3(1.10000002,1.10000002,1.10000002));
  r0.xyz = r0.xyz ? float3(0.5,0.5,0.5) : 0;
  r0.w = _Opacity * r0.w;
  r1.x = saturate((VD_COL(_NormalToWorld, 3).x));
  o0.xyzw = r1.xxxx * r0.xyzw;
  return;
}
