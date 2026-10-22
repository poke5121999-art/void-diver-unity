// LCArt/Deco/shader_DE_Deco_StickyDecal | pass 0 "FORWARD" | progFragment | keywords: (none) | blob 2
void frag(
  float4 v0 : SV_POSITION0,
  float4 v1 : TEXCOORD0,
  float4 v2 : TEXCOORD1,
  float4 v3 : TEXCOORD2,
  float4 v4 : TEXCOORD3,
  float4 v5 : TEXCOORD4,
  float4 v6 : TEXCOORD5,
  float4 v7 : TEXCOORD6,
  out float4 o0 : SV_Target0)
{
  float4 r0,r1;
  uint4 bitmask, uiDest;
  float4 fDest;

  r0.x = dot(v6.xyz, v6.xyz);
  r0.x = rsqrt(r0.x);
  r0.xyz = v6.xyz * r0.xxx;
  r1.x = -(VD_COL(unity_MatrixV, 0).z);
  r1.y = -(VD_COL(unity_MatrixV, 1).z);
  r1.z = -(VD_COL(unity_MatrixV, 2).z);
  r0.w = dot(r0.xyz, r1.xyz);
  r0.xyz = r0.xyz / r0.www;
  r1.xy = v5.xy / v5.ww;
  r1.xyzw = _CameraDepthTexture.SampleBias(sampler_vd_point_clamp, r1.xy, _GlobalMipBias.x).xyzw;
  r0.w = _ZBufferParams.z * r1.x + _ZBufferParams.w;
  r0.w = 1 / r0.w;
  r0.xyz = r0.xyz * r0.www + _WorldSpaceCameraPos.xyz;
  r1.xyz = (VD_COL(unity_WorldToObject, 1).xyz) * r0.yyy;
  r0.xyw = (VD_COL(unity_WorldToObject, 0).xyz) * r0.xxx + r1.xyz;
  r0.xyz = (VD_COL(unity_WorldToObject, 2).xyz) * r0.zzz + r0.xyw;
  r0.xyz = (VD_COL(unity_WorldToObject, 3).xyz) + r0.xyz;
  r0.xyz = -_DecalOffset.xyz * r0.yyy + r0.xyz;
  r1.xyz = float3(0.5,0.5,0.5) + -abs(r0.xyz);
  r0.xyz = float3(0.5,0.5,0.5) + r0.yxz;
  r1.xyz = cmp(r1.xyz < float3(0,0,0));
  r0.w = (int)r1.y | (int)r1.x;
  r0.w = (int)r1.z | (int)r0.w;
  if (r0.w != 0) discard;
  r0.x = saturate(r0.x);
  r1.xyzw = _MainTex.SampleBias(sampler_MainTex, r0.yz, _GlobalMipBias.x).xyzw;
  r0.y = 1 + -r0.x;
  r0.x = max(r0.x, r0.y);
  r0.x = 1 + -r0.x;
  r0.x = dot(r0.xx, r1.ww);
  r0.y = 1 + -_DissolveProgress;
  r0.x = -r0.x * r0.y + 1;
  o0.xyz = saturate(r0.xxx);
  o0.w = 1;
  return;
}
