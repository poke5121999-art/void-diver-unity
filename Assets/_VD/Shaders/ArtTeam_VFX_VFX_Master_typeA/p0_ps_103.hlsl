// ArtTeam/VFX/VFX_Master_typeA | pass 0 "Universal Forward" | progFragment | keywords: _DBUFFER_MRT3 _DEFORM_USE _MASK_USE _PLANESYSTEMCULLINGTYPE_ALWAYS | blob 103
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
  r2.xy = _Main_Offset_UV1xy.xy * _TimeParameters.xx + -r1.xy;
  r1.xy = (_Use_Main_TilingOffset).xx * r2.xy + r1.xy;
  r0.xy = r0.xy * r0.zw + r1.xy;
  r0.z = 1 + _Deform_Strength_UV2x;
  r0.w = v3.x * r0.z;
  r0.z = -v3.x * r0.z + _Deform_Strength_UV2x;
  r0.z = _Use_Deform_Str_Property * r0.z + r0.w;
  r0.w = cmp(abs(_Deform_Time_Offset_UV2y) >= 0.00100000005);
  r0.w = r0.w ? 1.000000 : 0;
  r1.x = _Deform_Time_Offset_UV2y * _TimeParameters.x + 1;
  r1.xz = v3.yy * r1.xz;
  r1.y = _Deform_Time_Offset_UV2y * _TimeParameters.x + -r1.x;
  r0.w = r0.w * r1.y + r1.x;
  r1.xy = _DeformDirection.xy * r0.ww;
  r2.xy = float2(1,1) / _ScreenParams.xy;
  r2.xy = (_Pixelation).xx * r2.xy;
  r2.zw = v1.xy / r2.xy;
  r2.zw = floor(r2.zw);
  r1.xy = r2.zw * r2.xy + r1.xy;
  r1.xy = r1.xy * _DeformTex_ST.xy + _DeformTex_ST.zw;
  r3.xyzw = _DeformTex.SampleBias(sampler_DeformTex, r1.xy, _GlobalMipBias.x).xyzw;
  r3.x = r3.w * r3.x;
  r1.xy = r3.xy * float2(2,2) + float2(-1,-1);
  r0.xy = r0.zz * r1.xy + r0.xy;
  r0.xy = r0.xy / r2.xy;
  r0.xy = floor(r0.xy);
  r0.zw = r0.xy * r2.xy;
  r0.xy = r0.xy * r2.xy + float2(-0.5,-0.5);
  r1.x = _Dissolve_Offset_UV2y * _TimeParameters.x + -r1.z;
  r1.y = cmp(abs(_Dissolve_Offset_UV2y) >= 0.00100000005);
  r1.y = r1.y ? 1.000000 : 0;
  r1.x = r1.y * r1.x + r1.z;
  r0.zw = r1.xx * _DissolveDirection.xy + r0.zw;
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
  r1.xy = (int2)v0.xy;
  r1.zw = float2(0,0);
  r1.xyzw = _DBufferTexture0.Load(r1.xyz).xyzw;
  o0.xyz = r0.xyz * r1.www + r1.xyz;
  o0.w = 1;
  return;
}
