// ArtTeam/VFX/VFX_Master_typeA | pass 2 "MotionVectors" | progVertex | keywords: _MASK_USE _PLANESYSTEMCULLINGTYPE_ALWAYS _VERTEXDISPLACEMENT_USE | blob 185
void vert(
  float3 v0 : POSITION0,
  float3 v1 : NORMAL0,
  float4 v2 : TEXCOORD0,
  float4 v3 : TEXCOORD1,
  float4 v4 : TEXCOORD2,
  float4 v5 : COLOR0,
  float3 v6 : TEXCOORD4,
  out float3 o0 : CLIP_POSITION_NO_JITTER0,
  out float3 o1 : PREVIOUS_CLIP_POSITION_NO_JITTER0,
  out float4 o2 : SV_POSITION0,
  out float4 o3 : INTERP0,
  out float4 o4 : INTERP1,
  out float4 o5 : INTERP2,
  out float4 o6 : INTERP3,
  out float3 o7 : INTERP4,
  out float3 o8 : INTERP5)
{
  float4 r0,r1,r2,r3;
  uint4 bitmask, uiDest;
  float4 fDest;

  r0.xy = float2(1,1) / _ScreenParams.xy;
  r0.xy = (_Pixelation).xx * r0.xy;
  r0.zw = v2.xy / r0.xy;
  r0.zw = floor(r0.zw);
  r1.x = _Deform_Time_Offset_UV2y * _TimeParameters.x + 1;
  r1.x = v4.y * r1.x;
  r1.y = cmp(abs(_Deform_Time_Offset_UV2y) >= 0.00100000005);
  r1.y = r1.y ? 1.000000 : 0;
  r1.z = _Deform_Time_Offset_UV2y * _TimeParameters.x + -r1.x;
  r1.x = r1.y * r1.z + r1.x;
  r1.xy = _DeformDirection.xy * r1.xx;
  r0.xy = r0.zw * r0.xy + r1.xy;
  r0.xy = r0.xy * _Vertex_Tex_ST.xy + _Vertex_Tex_ST.zw;
  r0.xyzw = _Vertex_Tex.SampleLevel(sampler_vd_linear_repeat, r0.xy, 0).xyzw;
  r0.xyz = v1.xyz * r0.xxx;
  r0.w = 1 + v4.x;
  r1.x = _Vertex_Intensity_UV2x * r0.w;
  r1.y = cmp(abs(_Vertex_Intensity_UV2x) >= 0.00100000005);
  r1.y = r1.y ? 1.000000 : 0;
  r0.w = -_Vertex_Intensity_UV2x * r0.w + _Vertex_Intensity_UV2x;
  r0.w = r1.y * r0.w + r1.x;
  r1.xyz = r0.xyz * r0.www + v0.xyz;
  r2.xyz = (VD_COL(unity_ObjectToWorld, 1).xyz) * r1.yyy;
  r2.xyz = (VD_COL(unity_ObjectToWorld, 0).xyz) * r1.xxx + r2.xyz;
  r2.xyz = (VD_COL(unity_ObjectToWorld, 2).xyz) * r1.zzz + r2.xyz;
  r2.xyz = (VD_COL(unity_ObjectToWorld, 3).xyz) + r2.xyz;
  r3.x = dot(v1.xyz, (VD_COL(unity_WorldToObject, 0).xyz));
  r3.y = dot(v1.xyz, (VD_COL(unity_WorldToObject, 1).xyz));
  r3.z = dot(v1.xyz, (VD_COL(unity_WorldToObject, 2).xyz));
  r1.w = dot(r3.xyz, r3.xyz);
  r1.w = max(1.17549435e-038, r1.w);
  r1.w = rsqrt(r1.w);
  o8.xyz = r3.xyz * r1.www;
  r3.xyzw = (VD_COL(unity_MatrixVP, 1).xyzw) * r2.yyyy;
  r3.xyzw = (VD_COL(unity_MatrixVP, 0).xyzw) * r2.xxxx + r3.xyzw;
  r3.xyzw = (VD_COL(unity_MatrixVP, 2).xyzw) * r2.zzzz + r3.xyzw;
  o2.xyzw = (VD_COL(unity_MatrixVP, 3).xyzw) + r3.xyzw;
  r1.w = cmp(unity_MotionVectorsParams.y != 0.000000);
  if (r1.w != 0) {
    r1.w = cmp(unity_MotionVectorsParams.x == 1.000000);
    if (r1.w != 0) {
      r1.xyz = r0.xyz * r0.www + v6.xyz;
    }
    r0.xyz = (VD_COL(_NonJitteredViewProjMatrix, 1).xyw) * r2.yyy;
    r0.xyz = (VD_COL(_NonJitteredViewProjMatrix, 0).xyw) * r2.xxx + r0.xyz;
    r0.xyz = (VD_COL(_NonJitteredViewProjMatrix, 2).xyw) * r2.zzz + r0.xyz;
    o0.xyz = (VD_COL(_NonJitteredViewProjMatrix, 3).xyw) + r0.xyz;
    r0.xyzw = (VD_COL(unity_MatrixPreviousM, 1).xyzw) * r1.yyyy;
    r0.xyzw = (VD_COL(unity_MatrixPreviousM, 0).xyzw) * r1.xxxx + r0.xyzw;
    r0.xyzw = (VD_COL(unity_MatrixPreviousM, 2).xyzw) * r1.zzzz + r0.xyzw;
    r0.xyzw = (VD_COL(unity_MatrixPreviousM, 3).xyzw) + r0.xyzw;
    r1.xyz = (VD_COL(_PrevViewProjMatrix, 1).xyw) * r0.yyy;
    r1.xyz = (VD_COL(_PrevViewProjMatrix, 0).xyw) * r0.xxx + r1.xyz;
    r0.xyz = (VD_COL(_PrevViewProjMatrix, 2).xyw) * r0.zzz + r1.xyz;
    o1.xyz = (VD_COL(_PrevViewProjMatrix, 3).xyw) * r0.www + r0.xyz;
  } else {
    o0.xyz = float3(0,0,0);
    o1.xyz = float3(0,0,0);
  }
  o3.xyzw = v2.xyzw;
  o4.xyzw = v3.xyzw;
  o5.xyzw = v4.xyzw;
  o6.xyzw = v5.xyzw;
  o7.xyz = r2.xyz;
  return;
}
