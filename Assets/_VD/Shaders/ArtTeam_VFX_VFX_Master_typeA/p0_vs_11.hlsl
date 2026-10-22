// ArtTeam/VFX/VFX_Master_typeA | pass 0 "Universal Forward" | progVertex | keywords: _MASK_USE _PLANESYSTEMCULLINGTYPE_ALWAYS _VERTEXDISPLACEMENT_USE | blob 11
void vert(
  float3 v0 : POSITION0,
  float3 v1 : NORMAL0,
  float4 v2 : TANGENT0,
  float4 v3 : TEXCOORD0,
  float4 v4 : TEXCOORD1,
  float4 v5 : TEXCOORD2,
  float4 v6 : TEXCOORD3,
  float4 v7 : COLOR0,
  out float4 o0 : SV_POSITION0,
  out float4 o1 : INTERP0,
  out float4 o2 : INTERP1,
  out float4 o3 : INTERP2,
  out float4 o4 : INTERP3,
  out float4 o5 : INTERP4,
  out float3 o6 : INTERP5,
  out float3 o7 : INTERP6)
{
  float4 r0,r1;
  uint4 bitmask, uiDest;
  float4 fDest;

  r0.x = cmp(abs(_Deform_Time_Offset_UV2y) >= 0.00100000005);
  r0.x = r0.x ? 1.000000 : 0;
  r0.y = _Deform_Time_Offset_UV2y * _TimeParameters.x + 1;
  r0.y = v5.y * r0.y;
  r0.z = _Deform_Time_Offset_UV2y * _TimeParameters.x + -r0.y;
  r0.x = r0.x * r0.z + r0.y;
  r0.xy = _DeformDirection.xy * r0.xx;
  r0.zw = float2(1,1) / _ScreenParams.xy;
  r0.zw = (_Pixelation).xx * r0.zw;
  r1.xy = v3.xy / r0.zw;
  r1.xy = floor(r1.xy);
  r0.xy = r1.xy * r0.zw + r0.xy;
  r0.xy = r0.xy * _Vertex_Tex_ST.xy + _Vertex_Tex_ST.zw;
  r0.xyzw = _Vertex_Tex.SampleLevel(sampler_vd_linear_repeat, r0.xy, 0).xyzw;
  r0.xyz = v1.xyz * r0.xxx;
  r0.w = cmp(abs(_Vertex_Intensity_UV2x) >= 0.00100000005);
  r0.w = r0.w ? 1.000000 : 0;
  r1.x = 1 + v5.x;
  r1.y = _Vertex_Intensity_UV2x * r1.x;
  r1.x = -_Vertex_Intensity_UV2x * r1.x + _Vertex_Intensity_UV2x;
  r0.w = r0.w * r1.x + r1.y;
  r0.xyz = r0.xyz * r0.www + v0.xyz;
  r1.xyz = (VD_COL(unity_ObjectToWorld, 1).xyz) * r0.yyy;
  r0.xyw = (VD_COL(unity_ObjectToWorld, 0).xyz) * r0.xxx + r1.xyz;
  r0.xyz = (VD_COL(unity_ObjectToWorld, 2).xyz) * r0.zzz + r0.xyw;
  r0.xyz = (VD_COL(unity_ObjectToWorld, 3).xyz) + r0.xyz;
  r1.xyzw = (VD_COL(unity_MatrixVP, 1).xyzw) * r0.yyyy;
  r1.xyzw = (VD_COL(unity_MatrixVP, 0).xyzw) * r0.xxxx + r1.xyzw;
  r1.xyzw = (VD_COL(unity_MatrixVP, 2).xyzw) * r0.zzzz + r1.xyzw;
  o6.xyz = r0.xyz;
  o0.xyzw = (VD_COL(unity_MatrixVP, 3).xyzw) + r1.xyzw;
  o1.xyzw = v3.xyzw;
  o2.xyzw = v4.xyzw;
  o3.xyzw = v5.xyzw;
  o4.xyzw = v6.xyzw;
  o5.xyzw = v7.xyzw;
  r0.x = dot(v1.xyz, (VD_COL(unity_WorldToObject, 0).xyz));
  r0.y = dot(v1.xyz, (VD_COL(unity_WorldToObject, 1).xyz));
  r0.z = dot(v1.xyz, (VD_COL(unity_WorldToObject, 2).xyz));
  r0.w = dot(r0.xyz, r0.xyz);
  r0.w = max(1.17549435e-038, r0.w);
  r0.w = rsqrt(r0.w);
  o7.xyz = r0.xyz * r0.www;
  return;
}
