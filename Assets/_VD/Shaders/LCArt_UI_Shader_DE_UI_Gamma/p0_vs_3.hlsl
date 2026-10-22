// LCArt/UI/Shader_DE_UI_Gamma | pass 0 "Default" | progVertex | keywords: UNITY_UI_CLIP_RECT | blob 3
void vert(
  float4 v0 : POSITION0,
  float4 v1 : COLOR0,
  float2 v2 : TEXCOORD0,
  out float4 o0 : SV_POSITION0,
  out float4 o1 : COLOR0,
  out float4 o2 : TEXCOORD0,
  out float4 o3 : TEXCOORD1)
{
  float4 r0,r1;
  uint4 bitmask, uiDest;
  float4 fDest;

  r0.xyzw = (VD_COL(unity_ObjectToWorld, 1).xyzw) * v0.yyyy;
  r0.xyzw = (VD_COL(unity_ObjectToWorld, 0).xyzw) * v0.xxxx + r0.xyzw;
  r0.xyzw = (VD_COL(unity_ObjectToWorld, 2).xyzw) * v0.zzzz + r0.xyzw;
  r0.xyzw = (VD_COL(unity_ObjectToWorld, 3).xyzw) + r0.xyzw;
  r1.xyzw = (VD_COL(unity_MatrixVP, 1).xyzw) * r0.yyyy;
  r1.xyzw = (VD_COL(unity_MatrixVP, 0).xyzw) * r0.xxxx + r1.xyzw;
  r1.xyzw = (VD_COL(unity_MatrixVP, 2).xyzw) * r0.zzzz + r1.xyzw;
  o0.xyzw = (VD_COL(unity_MatrixVP, 3).xyzw) * r0.wwww + r1.xyzw;
  o1.xyzw = _Color.xyzw * v1.xyzw;
  o2.xy = v2.xy;
  o3.xyzw = v0.xyzw;
  return;
}
