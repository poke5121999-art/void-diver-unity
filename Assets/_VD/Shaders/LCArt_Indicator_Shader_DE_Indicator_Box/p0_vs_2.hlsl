// LCArt/Indicator/Shader_DE_Indicator_Box | pass 0 "" | progVertex | keywords: (none) | blob 2
void vert(
  float4 v0 : POSITION0,
  float2 v1 : TEXCOORD0,
  out float2 o0 : TEXCOORD0,
  out float p0 : TEXCOORD2,
  out float p0 : TEXCOORD3,
  out float3 o1 : TEXCOORD1,
  out float p1 : TEXCOORD4,
  out float4 o2 : TEXCOORD5,
  out float4 o3 : SV_POSITION0)
{
  float4 r0,r1;
  uint4 bitmask, uiDest;
  float4 fDest;

  r0.x = dot((VD_COL(unity_ObjectToWorld, 0).xyz), (VD_COL(unity_ObjectToWorld, 0).xyz));
  p0.x = sqrt(r0.x);
  r0.x = dot((VD_COL(unity_ObjectToWorld, 2).xyz), (VD_COL(unity_ObjectToWorld, 2).xyz));
  p0.x = sqrt(r0.x);
  o0.xy = v1.xy * _MainTex_ST.xy + _MainTex_ST.zw;
  p1.x = v0.x;
  r0.xyzw = (VD_COL(unity_ObjectToWorld, 1).xyzw) * v0.yyyy;
  r0.xyzw = (VD_COL(unity_ObjectToWorld, 0).xyzw) * v0.xxxx + r0.xyzw;
  r0.xyzw = (VD_COL(unity_ObjectToWorld, 2).xyzw) * v0.zzzz + r0.xyzw;
  r0.xyzw = (VD_COL(unity_ObjectToWorld, 3).xyzw) + r0.xyzw;
  o1.xyz = r0.xyz;
  r1.xyzw = (VD_COL(unity_MatrixVP, 1).xyzw) * r0.yyyy;
  r1.xyzw = (VD_COL(unity_MatrixVP, 0).xyzw) * r0.xxxx + r1.xyzw;
  r1.xyzw = (VD_COL(unity_MatrixVP, 2).xyzw) * r0.zzzz + r1.xyzw;
  r0.xyzw = (VD_COL(unity_MatrixVP, 3).xyzw) * r0.wwww + r1.xyzw;
  r1.xz = float2(0.5,0.5) * r0.xw;
  r1.y = _ProjectionParams.x * r0.y;
  r1.w = 0.5 * r1.y;
  o2.xy = r1.xw + r1.zz;
  o2.zw = r0.zw;
  o3.xyzw = r0.xyzw;
  return;
}
