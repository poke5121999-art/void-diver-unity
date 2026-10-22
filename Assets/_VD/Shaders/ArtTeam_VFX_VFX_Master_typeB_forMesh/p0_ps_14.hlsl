// ArtTeam/VFX/VFX_Master_typeB_forMesh | pass 0 "Universal Forward" | progFragment | keywords: _SURFACE_TYPE_TRANSPARENT | blob 14
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

  r0.x = dot(v3.xyz, v3.xyz);
  r0.x = sqrt(r0.x);
  r0.x = 1 / r0.x;
  r0.xyz = v3.xyz * r0.xxx;
  r0.w = dot(r0.xyz, r0.xyz);
  r0.w = rsqrt(r0.w);
  r0.xyz = r0.xyz * r0.www;
  r1.xyz = _WorldSpaceCameraPos.xyz + -v2.xyz;
  r0.w = dot(r1.xyz, r1.xyz);
  r0.w = rsqrt(r0.w);
  r1.xyz = r1.xyz * r0.www;
  r0.w = cmp(unity_OrthoParams.w == 0.000000);
  r2.x = r0.w ? r1.x : (VD_COL(unity_MatrixV, 0).z);
  r2.y = r0.w ? r1.y : (VD_COL(unity_MatrixV, 1).z);
  r2.z = r0.w ? r1.z : (VD_COL(unity_MatrixV, 2).z);
  r0.x = saturate(dot(r0.xyz, r2.xyz));
  r0.x = 1 + -r0.x;
  r0.x = log2(r0.x);
  r0.x = _FresnelPower * r0.x;
  r0.x = exp2(r0.x);
  r0.y = saturate(_invertB);
  r0.zw = v1.xy * _MaskTex_G_Dissolve_B_2ndColor_A_alpha_ST.xy + _MaskTex_G_Dissolve_B_2ndColor_A_alpha_ST.zw;
  r1.xyzw = _MaskTex_G_Dissolve_B_2ndColor_A_alpha.SampleBias(sampler_MaskTex_G_Dissolve_B_2ndColor_A_alpha, r0.zw, _GlobalMipBias.x).xyzw;
  r0.z = r1.z * -2 + 1;
  r0.y = r0.y * r0.z + r1.z;
  r0.y = _2ndColor.w * r0.y;
  r0.zw = v1.xy * _MainTex_ST.xy + _MainTex_ST.zw;
  r2.xyzw = _MainTex.SampleBias(sampler_MainTex, r0.zw, _GlobalMipBias.x).xyzw;
  r3.xyz = _TintColor.xyz * r2.xyz;
  r2.xyz = -_TintColor.xyz * r2.xyz + _2ndColor.xyz;
  r0.yzw = r0.yyy * r2.xyz + r3.xyz;
  r0.xyz = r0.xxx * _FresnelColor.xyz + r0.yzw;
  o0.xyz = (_Emission).xxx * r0.xyz;
  r0.x = 1 + -r1.y;
  r0.yz = saturate(float2(_DissolveSharpness, _Dissolve));
  r0.w = -r0.y * 0.5 + 1;
  r1.x = r0.y * 0.5 + -1;
  r1.y = -r1.x + r0.w;
  r0.w = -r0.y * 0.5 + r0.w;
  r0.w = 1 / r0.w;
  r0.z = r0.z * r1.y + r1.x;
  r0.x = r0.x + r0.z;
  r0.x = -r0.y * 0.5 + r0.x;
  r0.x = saturate(r0.x * r0.w);
  r0.y = r0.x * -2 + 3;
  r0.x = r0.x * r0.x;
  r0.x = saturate(-r0.y * r0.x + r1.w);
  o0.w = _TintColor.w * r0.x;
  return;
}
