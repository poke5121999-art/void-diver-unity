// LCArt/VFX/VFX_Decoration_SurfaceCutterTarget | pass 0 "ForwardLit" | progVertex | keywords: (none) | blob 1
void vert(
  float4 v0 : POSITION0,
  float2 v1 : TEXCOORD0,
  float4 v2 : COLOR0,
  float4 v3 : TEXCOORD1,
  float4 v4 : TEXCOORD2,
  out float4 o0 : SV_POSITION0,
  out float4 o1 : TEXCOORD3,
  out float2 o2 : TEXCOORD0,
  out float4 o3 : COLOR0,
  out float4 o4 : TEXCOORD1,
  out float4 o5 : TEXCOORD2)
{
  float4 r0,r1;
  uint4 bitmask, uiDest;
  float4 fDest;

  r0.xyzw = (VD_COL(unity_ObjectToWorld, 1).xyzw) * v0.yyyy;
  r0.xyzw = (VD_COL(unity_ObjectToWorld, 0).xyzw) * v0.xxxx + r0.xyzw;
  r0.xyzw = (VD_COL(unity_ObjectToWorld, 2).xyzw) * v0.zzzz + r0.xyzw;
  r1.xyzw = (VD_COL(unity_ObjectToWorld, 3).xyzw) + r0.xyzw;
  o1.xyz = (VD_COL(unity_ObjectToWorld, 3).xyz) * v0.www + r0.xyz;
  r0.xyzw = (VD_COL(unity_MatrixVP, 1).xyzw) * r1.yyyy;
  r0.xyzw = (VD_COL(unity_MatrixVP, 0).xyzw) * r1.xxxx + r0.xyzw;
  r0.xyzw = (VD_COL(unity_MatrixVP, 2).xyzw) * r1.zzzz + r0.xyzw;
  o0.xyzw = (VD_COL(unity_MatrixVP, 3).xyzw) * r1.wwww + r0.xyzw;
  o2.xy = v1.xy;
  o3.xyzw = v2.xyzw;
  o4.xyzw = v3.xyzw;
  o5.xyzw = v4.xyzw;
  return;
}
