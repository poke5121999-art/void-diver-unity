// Shader Graphs/shader_DE_BG_ReflectionSurface_Floor | pass 3 "DecalMeshForwardEmissive" | progFragment | keywords: FOG_LINEAR | blob 31
void frag(
  float4 v0 : SV_POSITION0,
  float4 v1 : INTERP0,
  float4 v2 : INTERP1,
  float3 v3 : INTERP2,
  float3 v4 : INTERP3,
  out float4 o0 : SV_Target0)
{
  float4 r0,r1;
  uint4 bitmask, uiDest;
  float4 fDest;

  r0.xyz = (VD_COL(unity_WorldToObject, 1).xyz) * v3.yyy;
  r0.xyz = (VD_COL(unity_WorldToObject, 0).xyz) * v3.xxx + r0.xyz;
  r0.xyz = (VD_COL(unity_WorldToObject, 2).xyz) * v3.zzz + r0.xyz;
  r0.xyz = (VD_COL(unity_WorldToObject, 3).xyz) + r0.xyz;
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
  r1.xyzw = _PlaneReflectionTexture.SampleBias(sampler_PlaneReflectionTexture, r1.xz, _GlobalMipBias.x).xyzw;
  r0.yzw = -unity_FogColor.xyz + r1.xyz;
  r0.xyz = r0.xxx * r0.yzw + unity_FogColor.xyz;
  r0.xyz = cmp(r0.xyz >= float3(1.10000002,1.10000002,1.10000002));
  o0.xyz = r0.xyz ? float3(0.5,0.5,0.5) : 0;
  r0.x = 1 + -_GlobalBackgroundColorIntensity;
  r0.x = _PlaneReflectionActive * r0.x;
  r0.x = r0.x * r1.w;
  o0.w = _Opacity * r0.x;
  return;
}
