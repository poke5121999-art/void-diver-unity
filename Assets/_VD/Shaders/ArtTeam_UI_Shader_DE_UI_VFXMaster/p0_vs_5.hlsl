// ArtTeam/UI/Shader_DE_UI_VFXMaster | pass 0 "Default" | progVertex | keywords: _USE_MASK_ON | blob 5
void vert(
  float4 v0 : POSITION0,
  float4 v1 : COLOR0,
  float2 v2 : TEXCOORD0,
  out float4 o0 : SV_POSITION0,
  out float4 o1 : COLOR0,
  out float4 o2 : TEXCOORD0,
  out float4 o3 : TEXCOORD1,
  out float4 o4 : TEXCOORD2,
  out float4 o5 : TEXCOORD3)
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
  r0.xyzw = (VD_COL(unity_MatrixVP, 3).xyzw) * r0.wwww + r1.xyzw;
  o0.xyzw = r0.xyzw;
  o1.xyzw = _Color.xyzw * v1.xyzw;
  o2.xy = v2.xy;
  o3.xyzw = v0.xyzw;
  r1.z = (VD_COL(glstate_matrix_projection, 0).x) * _ScreenParams.x;
  r1.w = (VD_COL(glstate_matrix_projection, 1).y) * _ScreenParams.y;
  r0.xy = r0.ww / abs(r1.zw);
  r0.xy = float2(_UIMaskSoftnessX, _UIMaskSoftnessY) * float2(0.25,0.25) + abs(r0.xy);
  o4.zw = float2(0.25,0.25) / r0.xy;
  r0.xyzw = max(float4(-2e+010,-2e+010,-2e+010,-2e+010), _ClipRect.xyzw);
  r0.xyzw = min(float4(2e+010,2e+010,2e+010,2e+010), r0.xyzw);
  r0.xy = v0.xy * float2(2,2) + -r0.xy;
  o4.xy = r0.xy + -r0.zw;
  o5.xyzw = v0.xyzw;
  return;
}
