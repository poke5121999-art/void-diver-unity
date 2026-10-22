// ArtTeam/VFX/VFX_Master_typeB_forMesh | pass 0 "Universal Forward" | progFragment | keywords: _ALPHATEST_ON | blob 15
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

  r0.xy = saturate(float2(_DissolveSharpness, _Dissolve));
  r0.z = -r0.x * 0.5 + 1;
  r0.w = r0.x * 0.5 + -1;
  r1.x = r0.z + -r0.w;
  r0.z = -r0.x * 0.5 + r0.z;
  r0.z = 1 / r0.z;
  r0.y = r0.y * r1.x + r0.w;
  r1.xy = v1.xy * _MaskTex_G_Dissolve_B_2ndColor_A_alpha_ST.xy + _MaskTex_G_Dissolve_B_2ndColor_A_alpha_ST.zw;
  r1.xyzw = _MaskTex_G_Dissolve_B_2ndColor_A_alpha.SampleBias(sampler_MaskTex_G_Dissolve_B_2ndColor_A_alpha, r1.xy, _GlobalMipBias.x).xyzw;
  r1.xy = float2(1,1) + -r1.zy;
  r0.y = r1.y + r0.y;
  r0.w = r1.x + -r1.z;
  r0.x = -r0.x * 0.5 + r0.y;
  r0.x = saturate(r0.x * r0.z);
  r0.y = r0.x * -2 + 3;
  r0.x = r0.x * r0.x;
  r0.x = saturate(-r0.y * r0.x + r1.w);
  r0.y = _TintColor.w * r0.x;
  r0.x = _TintColor.w * r0.x + -0.5;
  r0.z = ddx(r0.y);
  r0.y = ddy(r0.y);
  r0.y = abs(r0.z) + abs(r0.y);
  r0.z = -r0.y * 0.5 + r0.x;
  r0.y = max(9.99999975e-005, r0.y);
  r0.y = r0.z / r0.y;
  r0.y = saturate(1 + r0.y);
  r0.z = -9.99999975e-005 + r0.y;
  r1.x = cmp(_AlphaToMaskAvailable != 0.000000);
  r0.x = r1.x ? r0.z : r0.x;
  o0.w = r1.x ? r0.y : 1;
  r0.x = cmp(r0.x < 0);
  if (r0.x != 0) discard;
  r0.x = saturate(_invertB);
  r0.x = r0.x * r0.w + r1.z;
  r0.x = _2ndColor.w * r0.x;
  r0.yz = v1.xy * _MainTex_ST.xy + _MainTex_ST.zw;
  r1.xyzw = _MainTex.SampleBias(sampler_MainTex, r0.yz, _GlobalMipBias.x).xyzw;
  r0.yzw = _TintColor.xyz * r1.xyz;
  r1.xyz = -_TintColor.xyz * r1.xyz + _2ndColor.xyz;
  r0.xyz = r0.xxx * r1.xyz + r0.yzw;
  r0.w = dot(v3.xyz, v3.xyz);
  r0.w = sqrt(r0.w);
  r0.w = 1 / r0.w;
  r1.xyz = v3.xyz * r0.www;
  r0.w = dot(r1.xyz, r1.xyz);
  r0.w = rsqrt(r0.w);
  r1.xyz = r1.xyz * r0.www;
  r2.xyz = _WorldSpaceCameraPos.xyz + -v2.xyz;
  r0.w = dot(r2.xyz, r2.xyz);
  r0.w = rsqrt(r0.w);
  r2.xyz = r2.xyz * r0.www;
  r0.w = cmp(unity_OrthoParams.w == 0.000000);
  r3.x = r0.w ? r2.x : (VD_COL(unity_MatrixV, 0).z);
  r3.y = r0.w ? r2.y : (VD_COL(unity_MatrixV, 1).z);
  r3.z = r0.w ? r2.z : (VD_COL(unity_MatrixV, 2).z);
  r0.w = saturate(dot(r1.xyz, r3.xyz));
  r0.w = 1 + -r0.w;
  r0.w = log2(r0.w);
  r0.w = _FresnelPower * r0.w;
  r0.w = exp2(r0.w);
  r0.xyz = r0.www * _FresnelColor.xyz + r0.xyz;
  o0.xyz = (_Emission).xxx * r0.xyz;
  return;
}
