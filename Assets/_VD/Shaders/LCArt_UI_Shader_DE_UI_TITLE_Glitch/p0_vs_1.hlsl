// LCArt/UI/Shader_DE_UI_TITLE_Glitch | pass 0 "Default" | progVertex | keywords: (none) | blob 1
void vert(
  float4 v0 : POSITION0,
  float4 v1 : COLOR0,
  float2 v2 : TEXCOORD0,
  out float4 o0 : SV_POSITION0,
  out float4 o1 : COLOR0,
  out float4 o2 : TEXCOORD0,
  out float4 o3 : TEXCOORD1)
{
  float4 r0,r1,r2;
  uint4 bitmask, uiDest;
  float4 fDest;

  r0.x = _JitterFlickerRate * _Time.y;
  r0.x = floor(r0.x);
  r0.x = r0.x * 12.9898005 + 31.7409992;
  r0.x = sin(r0.x);
  r0.x = 43758.5469 * r0.x;
  r0.x = frac(r0.x);
  r0.x = -_JitterFlickerThreshold + r0.x;
  r0.y = 1 + -_JitterFlickerThreshold;
  r0.y = 1 / r0.y;
  r0.x = saturate(r0.x * r0.y);
  r0.y = r0.x * -2 + 3;
  r0.x = r0.x * r0.x;
  r0.x = r0.y * r0.x;
  r0.y = _JitterSpeed * _Time.y;
  r0.y = floor(r0.y);
  r0.yz = float2(91.3450012,17.9130001) * r0.yy;
  r0.yz = sin(r0.yz);
  r0.yz = float2(43758.5469,43758.5469) * r0.yz;
  r0.yz = frac(r0.yz);
  r0.yz = float2(-0.5,-0.5) + r0.yz;
  r0.yz = (_VertexJitterStrength).xx * r0.yz;
  r1.xyzw = (VD_COL(unity_ObjectToWorld, 1).xyzw) * v0.yyyy;
  r1.xyzw = (VD_COL(unity_ObjectToWorld, 0).xyzw) * v0.xxxx + r1.xyzw;
  r1.xyzw = (VD_COL(unity_ObjectToWorld, 2).xyzw) * v0.zzzz + r1.xyzw;
  r1.xyzw = (VD_COL(unity_ObjectToWorld, 3).xyzw) + r1.xyzw;
  r2.xyzw = (VD_COL(unity_MatrixVP, 1).xyzw) * r1.yyyy;
  r2.xyzw = (VD_COL(unity_MatrixVP, 0).xyzw) * r1.xxxx + r2.xyzw;
  r2.xyzw = (VD_COL(unity_MatrixVP, 2).xyzw) * r1.zzzz + r2.xyzw;
  r1.xyzw = (VD_COL(unity_MatrixVP, 3).xyzw) * r1.wwww + r2.xyzw;
  r0.yz = r1.ww * r0.yz;
  o0.xy = r0.yz * r0.xx + r1.xy;
  o0.zw = r1.zw;
  o1.xyzw = v1.xyzw;
  o2.xy = v2.xy * _MainTex_ST.xy + _MainTex_ST.zw;
  o3.xyzw = v0.xyzw;
  return;
}
