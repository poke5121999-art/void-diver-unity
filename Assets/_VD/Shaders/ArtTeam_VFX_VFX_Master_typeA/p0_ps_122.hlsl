// ArtTeam/VFX/VFX_Master_typeA | pass 0 "Universal Forward" | progFragment | keywords: FOG_LINEAR _DBUFFER_MRT3 _PLANESYSTEMCULLINGTYPE_ALWAYS | blob 122
void frag(
  float4 v0 : SV_POSITION0,
  float4 v1 : INTERP0,
  float4 v2 : INTERP1,
  float4 v3 : INTERP2,
  float4 v4 : INTERP3,
  float4 v5 : INTERP4,
  float3 v6 : INTERP5,
  float3 v7 : INTERP6,
  out float4 o0 : SV_Target0)
{
  float4 r0,r1,r2,r3;
  uint4 bitmask, uiDest;
  float4 fDest;

  r0.xy = float2(-0.5,-0.5) + v1.xy;
  r0.z = max(abs(r0.x), abs(r0.y));
  r0.z = 1 / r0.z;
  r0.w = min(abs(r0.x), abs(r0.y));
  r0.z = r0.w * r0.z;
  r0.w = r0.z * r0.z;
  r1.x = r0.w * 0.0208350997 + -0.0851330012;
  r1.x = r0.w * r1.x + 0.180141002;
  r1.x = r0.w * r1.x + -0.330299497;
  r0.w = r0.w * r1.x + 0.999866009;
  r1.x = r0.z * r0.w;
  r1.x = r1.x * -2 + 1.57079637;
  r1.y = cmp(abs(r0.y) < abs(r0.x));
  r1.x = r1.y ? r1.x : 0;
  r0.z = r0.z * r0.w + r1.x;
  r0.w = cmp(r0.y < -r0.y);
  r0.w = r0.w ? -3.141593 : 0;
  r0.z = r0.z + r0.w;
  r0.w = min(r0.x, r0.y);
  r0.w = cmp(r0.w < -r0.w);
  r1.x = max(r0.x, r0.y);
  r0.x = dot(r0.xy, r0.xy);
  r0.x = sqrt(r0.x);
  r0.x = r0.x + r0.x;
  r1.x = cmp(r1.x >= -r1.x);
  r0.w = r0.w ? r1.x : 0;
  r0.z = r0.w ? -r0.z : r0.z;
  r0.y = 0.159235656 * r0.z;
  r0.xy = -v1.xy + r0.xy;
  r0.xy = (_Main_Radial).xx * r0.xy + v1.xy;
  r0.zw = float2(1,1) + -v2.zw;
  r0.zw = (_Use_Main_TilingOffset).xx * r0.zw + v2.zw;
  r1.xyz = float3(_Main_Offset_UV1xy.x, _Main_Offset_UV1xy.y, _Dissolve_Offset_UV2y) * _TimeParameters.xxx + float3(1,1,1);
  r1.xy = v2.xy * r1.xy;
  r1.z = v3.y * r1.z;
  r2.xy = _Main_Offset_UV1xy.xy * _TimeParameters.xx + -r1.xy;
  r1.xy = (_Use_Main_TilingOffset).xx * r2.xy + r1.xy;
  r0.xy = r0.xy * r0.zw + r1.xy;
  r0.zw = float2(1,1) / _ScreenParams.xy;
  r0.zw = (_Pixelation).xx * r0.zw;
  r0.xy = r0.xy / r0.zw;
  r0.xy = floor(r0.xy);
  r1.xy = r0.xy * r0.zw;
  r0.xy = r0.xy * r0.zw + float2(-0.5,-0.5);
  r0.z = _Dissolve_Offset_UV2y * _TimeParameters.x + -r1.z;
  r0.w = cmp(abs(_Dissolve_Offset_UV2y) >= 0.00100000005);
  r0.w = r0.w ? 1.000000 : 0;
  r0.z = r0.w * r0.z + r1.z;
  r0.zw = r0.zz * _DissolveDirection.xy + r1.xy;
  r0.zw = float2(-0.5,-0.5) + r0.zw;
  r1.x = 0.0174444448 * _Dissolve_Rotate;
  sincos(r1.x, r1.x, r2.x);
  r3.x = -r1.x;
  r3.y = r2.x;
  r3.z = r1.x;
  r1.x = dot(r0.zw, r3.yz);
  r1.y = dot(r0.zw, r3.xy);
  r0.zw = float2(0.5,0.5) + r1.xy;
  r0.zw = r0.zw * _DissolveTex_ST.xy + _DissolveTex_ST.zw;
  r1.xyzw = _DissolveTex.SampleBias(sampler_DissolveTex, r0.zw, _GlobalMipBias.x).xyzw;
  r0.z = r1.y + -r1.x;
  r0.z = _Use_Dissolve_Green * r0.z + r1.x;
  r0.w = v5.w * 2 + -1;
  r1.x = _Dissolve_Progress_UV2w + -v3.w;
  r1.x = _Use_Dissolve_Progress_Property * r1.x + v3.w;
  r0.w = -r1.x + r0.w;
  r0.w = _Use_Dissolve_Alpha * r0.w + r1.x;
  r0.w = max(-1, r0.w);
  r0.w = min(1, r0.w);
  r0.w = -_DissolveEdge_Wide + r0.w;
  r0.z = r0.z + r0.w;
  r0.z = 1 + -r0.z;
  r0.w = -0.5 + _DissolveEdge_Sharp;
  r0.z = r0.z + -r0.w;
  r1.x = 1 + -r0.w;
  r0.w = r1.x + -r0.w;
  r0.w = 1 / r0.w;
  r0.z = saturate(r0.z * r0.w);
  r0.w = r0.z * -2 + 3;
  r0.z = r0.z * r0.z;
  r0.z = r0.w * r0.z;
  r0.w = 0.0174444448 * _Main_Rotate;
  sincos(r0.w, r1.x, r2.x);
  r3.x = -r1.x;
  r3.y = r2.x;
  r3.z = r1.x;
  r1.x = dot(r0.xy, r3.yz);
  r1.y = dot(r0.xy, r3.xy);
  r0.xy = float2(0.5,0.5) + r1.xy;
  r0.xy = r0.xy * _MainTex_ST.xy + _MainTex_ST.zw;
  r1.xyzw = _MainTex.SampleBias(sampler_MainTex, r0.xy, _GlobalMipBias.x).xyzw;
  r0.xyw = r1.xyz + -r1.xxx;
  r0.xyw = (_isColor).xxx * r0.xyw + r1.xxx;
  r0.xyw = v5.xyz * r0.xyw;
  r1.x = max(1, v3.z);
  r0.xyw = r1.xxx * r0.xyw;
  r1.xyz = -_MainColor.xyz * r0.xyw + _DissolveEdge_Color.xyz;
  r0.xyw = _MainColor.xyz * r0.xyw;
  r1.xyz = r1.xyz * r0.zzz;
  r0.xyz = (_DissolveEdge_Use).xxx * r1.xyz + r0.xyw;
  r1.xyz = unity_FogColor.xyz + -r0.xyz;
  r2.xyz = (VD_COL(unity_WorldToObject, 1).xyz) * v6.yyy;
  r2.xyz = (VD_COL(unity_WorldToObject, 0).xyz) * v6.xxx + r2.xyz;
  r2.xyz = (VD_COL(unity_WorldToObject, 2).xyz) * v6.zzz + r2.xyz;
  r2.xyz = (VD_COL(unity_WorldToObject, 3).xyz) + r2.xyz;
  r3.xyz = (VD_COL(unity_ObjectToWorld, 1).xyz) * r2.yyy;
  r2.xyw = (VD_COL(unity_ObjectToWorld, 0).xyz) * r2.xxx + r3.xyz;
  r2.xyz = (VD_COL(unity_ObjectToWorld, 2).xyz) * r2.zzz + r2.xyw;
  r2.xyz = (VD_COL(unity_ObjectToWorld, 3).xyz) + r2.xyz;
  r0.w = (VD_COL(unity_MatrixV, 1).z) * r2.y;
  r0.w = (VD_COL(unity_MatrixV, 0).z) * r2.x + r0.w;
  r0.w = (VD_COL(unity_MatrixV, 2).z) * r2.z + r0.w;
  r0.w = (VD_COL(unity_MatrixV, 3).z) + r0.w;
  r0.w = -_ProjectionParams.y + -r0.w;
  r0.w = max(0, r0.w);
  r0.w = saturate(r0.w * unity_FogParams.z + unity_FogParams.w);
  r0.w = 1 + -r0.w;
  r0.xyz = r0.www * r1.xyz + r0.xyz;
  r1.xy = (int2)v0.xy;
  r1.zw = float2(0,0);
  r1.xyzw = _DBufferTexture0.Load(r1.xyz).xyzw;
  o0.xyz = r0.xyz * r1.www + r1.xyz;
  o0.w = 1;
  return;
}
