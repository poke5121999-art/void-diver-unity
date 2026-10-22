// ArtTeam/VFX/VFX_Master_typeA_Trail | pass 0 "Universal Forward" | progFragment | keywords: FOG_LINEAR _DBUFFER_MRT3 | blob 25
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
  r0.y = _DeformSpeed * _TimeParameters.x;
  r0.y = 0.0166666675 * r0.y;
  r0.z = cmp(r0.y >= -r0.y);
  r0.y = frac(abs(r0.y));
  r0.y = r0.z ? r0.y : -r0.y;
  r0.y = 60 * r0.y;
  r0.yz = r0.yy * _DeformDirection.xy + v1.xy;
  r0.yz = r0.yz * _DeformTex_ST.xy + _DeformTex_ST.zw;
  r1.xyzw = _DeformTex.SampleBias(sampler_DeformTex, r0.yz, _GlobalMipBias.x).xyzw;
  r1.x = r1.w * r1.x;
  r0.yz = r1.xy * float2(2,2) + float2(-1,-1);
  r0.w = _MainTexFlowSpeed * _TimeParameters.x;
  r1.x = cmp(r0.w >= -r0.w);
  r0.w = frac(abs(r0.w));
  r0.w = r1.x ? r0.w : -r0.w;
  r1.xy = r0.ww * float2(1,0) + v1.xy;
  r0.yz = (_DeformStrength).xx * r0.yz + r1.xy;
  r0.yz = r0.yz * _MainTex_ST.xy + _MainTex_ST.zw;
  r1.xyzw = _MainTex.SampleBias(sampler_MainTex, r0.yz, _GlobalMipBias.x).xyzw;
  r0.yzw = r1.xyz + -r1.xxx;
  r0.yzw = (_isColor).xxx * r0.yzw + r1.xxx;
  r0.yzw = v2.xyz * r0.yzw;
  r1.x = max(1, _Emmisive);
  r0.yzw = r1.xxx * r0.yzw;
  r1.xyz = _MainColor.xyz * r0.yzw;
  r0.yzw = -_MainColor.xyz * r0.yzw + unity_FogColor.xyz;
  r0.xyz = r0.xxx * r0.yzw + r1.xyz;
  r1.xy = (int2)v0.xy;
  r1.zw = float2(0,0);
  r1.xyzw = _DBufferTexture0.Load(r1.xyz).xyzw;
  o0.xyz = r0.xyz * r1.www + r1.xyz;
  o0.w = 1;
  return;
}
