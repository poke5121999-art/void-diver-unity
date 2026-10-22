// LCArt/TMPFont/Shader_DE_TMP_SDF_Lit | pass 0 "TextMeshPro SDF" | progVertex | keywords: UNDERLAY_ON UNITY_UI_CLIP_RECT _CLUSTER_LIGHT_LOOP | blob 17
void vert(
  float4 v0 : POSITION0,
  float3 v1 : NORMAL0,
  float4 v2 : COLOR0,
  float4 v3 : TEXCOORD0,
  float2 v4 : TEXCOORD1,
  out float4 o0 : SV_POSITION0,
  out float4 o1 : COLOR0,
  out float2 o2 : TEXCOORD0,
  out float p2 : TEXCOORD8,
  out float4 o3 : TEXCOORD1,
  out float4 o4 : TEXCOORD2,
  out float4 o5 : TEXCOORD3,
  out float4 o6 : TEXCOORD4,
  out float4 o7 : COLOR1,
  out float4 o8 : TEXCOORD5,
  out float4 o9 : TEXCOORD6,
  out float3 o10 : TEXCOORD7)
{
  float4 r0,r1,r2,r3;
  uint4 bitmask, uiDest;
  float4 fDest;

  r0.xy = float2(_VertexOffsetX, _VertexOffsetY) + v0.xy;
  r1.xyzw = (VD_COL(unity_ObjectToWorld, 1).xyzw) * r0.yyyy;
  r1.xyzw = (VD_COL(unity_ObjectToWorld, 0).xyzw) * r0.xxxx + r1.xyzw;
  r1.xyzw = (VD_COL(unity_ObjectToWorld, 2).xyzw) * v0.zzzz + r1.xyzw;
  r1.xyzw = (VD_COL(unity_ObjectToWorld, 3).xyzw) + r1.xyzw;
  r2.xyzw = (VD_COL(unity_MatrixVP, 1).xyzw) * r1.yyyy;
  r2.xyzw = (VD_COL(unity_MatrixVP, 0).xyzw) * r1.xxxx + r2.xyzw;
  r2.xyzw = (VD_COL(unity_MatrixVP, 2).xyzw) * r1.zzzz + r2.xyzw;
  r2.xyzw = (VD_COL(unity_MatrixVP, 3).xyzw) * r1.wwww + r2.xyzw;
  r1.xyz = _WorldSpaceCameraPos.xyz + -r1.xyz;
  o0.xyzw = r2.xyzw;
  o1.xyzw = v2.xyzw;
  o2.xy = v3.xy;
  p2.x = 0;
  r2.x = dot(v1.xyz, (VD_COL(unity_WorldToObject, 0).xyz));
  r2.y = dot(v1.xyz, (VD_COL(unity_WorldToObject, 1).xyz));
  r2.z = dot(v1.xyz, (VD_COL(unity_WorldToObject, 2).xyz));
  r0.z = dot(r2.xyz, r2.xyz);
  r0.z = max(1.17549435e-038, r0.z);
  r0.z = rsqrt(r0.z);
  r2.xyz = r2.xyz * r0.zzz;
  r0.z = dot(r1.xyz, r1.xyz);
  r0.z = rsqrt(r0.z);
  r3.xyz = r1.xyz * r0.zzz;
  r0.z = dot(r2.xyz, r3.xyz);
  r3.z = (VD_COL(glstate_matrix_projection, 0).x) * _ScreenParams.x;
  r3.w = (VD_COL(glstate_matrix_projection, 1).y) * _ScreenParams.y;
  r2.xy = float2(_ScaleX, _ScaleY) * abs(r3.zw);
  r2.xy = r2.ww / r2.xy;
  r0.w = dot(r2.xy, r2.xy);
  r2.xy = float2(_MaskSoftnessX, _MaskSoftnessY) * float2(0.25,0.25) + r2.xy;
  o4.zw = float2(0.25,0.25) / r2.xy;
  r0.w = rsqrt(r0.w);
  r1.w = _GradientScale * abs(v3.w);
  r2.x = 1 + _Sharpness;
  r1.w = r2.x * r1.w;
  r2.x = r1.w * r0.w;
  r2.y = 1 + -_PerspectiveFilter;
  r2.y = abs(r2.x) * r2.y;
  r0.w = r0.w * r1.w + -r2.y;
  r0.z = abs(r0.z) * r0.w + r2.y;
  r0.w = cmp((VD_COL(glstate_matrix_projection, 3).w) == 0.000000);
  r2.y = r0.w ? r0.z : r2.x;
  r0.z = cmp(0 >= v3.w);
  r0.z = r0.z ? 1.000000 : 0;
  r0.w = _WeightBold + -_WeightNormal;
  r0.z = r0.z * r0.w + _WeightNormal;
  r0.z = r0.z * 0.25 + _FaceDilate;
  r0.z = _ScaleRatioA * r0.z;
  r2.w = 0.5 * r0.z;
  o3.yw = r2.yw;
  r0.w = 0.5 / r2.y;
  r1.w = -_OutlineWidth * _ScaleRatioA + 1;
  r1.w = -_OutlineSoftness * _ScaleRatioA + r1.w;
  r1.w = r1.w * 0.5 + -r0.w;
  o3.x = -r0.z * 0.5 + r1.w;
  r0.z = -r0.z * 0.5 + 0.5;
  o3.z = r0.z + r0.w;
  r3.xyzw = max(float4(-2e+010,-2e+010,-2e+010,-2e+010), _ClipRect.xyzw);
  r3.xyzw = min(float4(2e+010,2e+010,2e+010,2e+010), r3.xyzw);
  r0.xy = r0.xy * float2(2,2) + -r3.xy;
  o4.xy = r0.xy + -r3.zw;
  r0.xyw = (VD_COL(_EnvMatrix, 1).xyz) * r1.yyy;
  r0.xyw = (VD_COL(_EnvMatrix, 0).xyz) * r1.xxx + r0.xyw;
  o5.xyz = (VD_COL(_EnvMatrix, 2).xyz) * r1.zzz + r0.xyw;
  r1.xyzw = (_ScaleRatioC).xxxx * float4(_UnderlaySoftness, _UnderlayDilate, _UnderlayOffsetX, _UnderlayOffsetY);
  r0.x = r1.x * r2.y + 1;
  r0.x = r2.y / r0.x;
  r0.y = r0.z * r0.x + -0.5;
  r0.z = r1.y * r0.x;
  r1.xy = (_GradientScale).xx * -r1.zw;
  r1.xy = r1.xy / float2(_TextureWidth, _TextureHeight);
  o6.xy = v3.xy + r1.xy;
  o6.z = r0.x;
  o6.w = -r0.z * 0.5 + r0.y;
  o7.xyz = _UnderlayColor.xyz * _UnderlayColor.www;
  o7.w = _UnderlayColor.w;
  o8.xy = v4.xy * _FaceTex_ST.xy + _FaceTex_ST.zw;
  o8.zw = v4.xy * _OutlineTex_ST.xy + _OutlineTex_ST.zw;
  o9.xyz = -(VD_COL(unity_ObjectToWorld, 2).xyz);
  r0.xyz = (VD_COL(unity_ObjectToWorld, 1).xyz) * v0.yyy;
  r0.xyz = (VD_COL(unity_ObjectToWorld, 0).xyz) * v0.xxx + r0.xyz;
  r0.xyz = (VD_COL(unity_ObjectToWorld, 2).xyz) * v0.zzz + r0.xyz;
  o10.xyz = (VD_COL(unity_ObjectToWorld, 3).xyz) * v0.www + r0.xyz;
  return;
}
