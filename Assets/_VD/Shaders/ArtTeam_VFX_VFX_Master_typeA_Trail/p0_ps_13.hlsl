// ArtTeam/VFX/VFX_Master_typeA_Trail | pass 0 "Universal Forward" | progFragment | keywords: (none) | blob 13
void frag(
  float4 v0 : SV_POSITION0,
  float4 v1 : INTERP0,
  float4 v2 : INTERP1,
  float3 v3 : INTERP2,
  float3 v4 : INTERP3,
  out float4 o0 : SV_Target0)
{
  float4 r0;
  uint4 bitmask, uiDest;
  float4 fDest;

  r0.x = _DeformSpeed * _TimeParameters.x;
  r0.x = 0.0166666675 * r0.x;
  r0.y = cmp(r0.x >= -r0.x);
  r0.x = frac(abs(r0.x));
  r0.x = r0.y ? r0.x : -r0.x;
  r0.x = 60 * r0.x;
  r0.xy = r0.xx * _DeformDirection.xy + v1.xy;
  r0.xy = r0.xy * _DeformTex_ST.xy + _DeformTex_ST.zw;
  r0.xyzw = _DeformTex.SampleBias(sampler_DeformTex, r0.xy, _GlobalMipBias.x).xyzw;
  r0.x = r0.w * r0.x;
  r0.xy = r0.xy * float2(2,2) + float2(-1,-1);
  r0.z = _MainTexFlowSpeed * _TimeParameters.x;
  r0.w = cmp(r0.z >= -r0.z);
  r0.z = frac(abs(r0.z));
  r0.z = r0.w ? r0.z : -r0.z;
  r0.zw = r0.zz * float2(1,0) + v1.xy;
  r0.xy = (_DeformStrength).xx * r0.xy + r0.zw;
  r0.xy = r0.xy * _MainTex_ST.xy + _MainTex_ST.zw;
  r0.xyzw = _MainTex.SampleBias(sampler_MainTex, r0.xy, _GlobalMipBias.x).xyzw;
  r0.yzw = r0.xyz + -r0.xxx;
  r0.xyz = (_isColor).xxx * r0.yzw + r0.xxx;
  r0.xyz = v2.xyz * r0.xyz;
  r0.w = max(1, _Emmisive);
  r0.xyz = r0.xyz * r0.www;
  o0.xyz = _MainColor.xyz * r0.xyz;
  o0.w = 1;
  return;
}
