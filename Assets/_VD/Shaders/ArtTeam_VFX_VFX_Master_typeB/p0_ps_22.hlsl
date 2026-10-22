// ArtTeam/VFX/VFX_Master_typeB | pass 0 "Universal Forward" | progFragment | keywords: _PLANESYSTEMCULLINGTYPE_ALWAYS _SURFACE_TYPE_TRANSPARENT | blob 22
void frag(
  float4 v0 : SV_POSITION0,
  float4 v1 : INTERP0,
  float4 v2 : INTERP1,
  float4 v3 : INTERP2,
  float4 v4 : INTERP3,
  float3 v5 : INTERP4,
  float3 v6 : INTERP5,
  out float4 o0 : SV_Target0)
{
  float4 r0,r1,r2;
  uint4 bitmask, uiDest;
  float4 fDest;

  r0.x = dot(v6.xyz, v6.xyz);
  r0.x = sqrt(r0.x);
  r0.x = 1 / r0.x;
  r0.xyz = v6.xyz * r0.xxx;
  r0.w = dot(r0.xyz, r0.xyz);
  r0.w = rsqrt(r0.w);
  r0.xyz = r0.xyz * r0.www;
  r1.xyz = _WorldSpaceCameraPos.xyz + -v5.xyz;
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
  r0.x = _isFresnel * r0.x;
  r0.y = max(1, v2.w);
  r0.zw = float2(1,1) / _ScreenParams.xy;
  r0.zw = (_Pixelation).xx * r0.zw;
  r1.xy = v1.xy / r0.zw;
  r1.xy = floor(r1.xy);
  r0.zw = r1.xy * r0.zw;
  r0.zw = r0.zw * _MainTex_ST.xy + _MainTex_ST.zw;
  r1.xyzw = _MainTex.SampleBias(sampler_MainTex, r0.zw, _GlobalMipBias.x).xyzw;
  r2.xyz = v4.xyz * r1.xxx;
  r0.yzw = r2.xyz * r0.yyy + -r2.xyz;
  r0.xyz = r0.xxx * r0.yzw + r2.xyz;
  r2.xyz = v3.xyz + -r0.xyz;
  r0.w = r1.z * -2 + 1;
  r1.x = saturate(_invertB);
  r0.w = r1.x * r0.w + r1.z;
  r0.w = v3.w * r0.w;
  r0.xyz = r0.www * r2.xyz + r0.xyz;
  o0.xyz = v2.zzz * r0.xyz;
  r0.x = 1 + -r1.y;
  r0.yz = saturate(v2.yx);
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
  r0.x = v4.w * r0.x;
  r0.yz = v1.xy * _MaskTex_ST.xy + _MaskTex_ST.zw;
  r1.xyzw = _MaskTex.SampleBias(sampler_MaskTex, r0.yz, _GlobalMipBias.x).xyzw;
  r0.y = r1.x * r1.w;
  o0.w = saturate(r0.x * r0.y);
  return;
}
