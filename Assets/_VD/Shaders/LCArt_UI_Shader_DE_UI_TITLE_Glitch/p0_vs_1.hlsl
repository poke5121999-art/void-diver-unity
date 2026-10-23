// LCArt/UI/Shader_DE_UI_TITLE_Glitch | pass 0 "Default" | progVertex | keywords: (none) | blob 1
void vert(
  float4 v0 : POSITION0,
  float4 v1 : COLOR0,
  float2 v2 : TEXCOORD0,
  out float4 o0 : SV_POSITION0,
  out float4 o1 : VDREG1,
  out float4 o2 : VDREG2,
  out float4 o3 : VDREG3)
{
  uint4 r0, r1, r2;
  r0 = 0u; r1 = 0u; r2 = 0u;

  r0.x = vd_asuint((vd_asfloat(_Time.y) * vd_asfloat(_JitterFlickerRate)));
  r0.x = vd_asuint(floor(vd_asfloat(r0.x)));
  r0.x = vd_asuint((vd_asfloat(r0.x) * 12.9898005 + 31.7409992));
  r0.x = vd_asuint(sin(vd_asfloat(r0.x)));
  r0.x = vd_asuint((vd_asfloat(r0.x) * 43758.5469));
  r0.x = vd_asuint(frac(vd_asfloat(r0.x)));
  r0.x = vd_asuint((vd_asfloat(r0.x) + (-(vd_asfloat(_JitterFlickerThreshold)))));
  r0.y = vd_asuint(((-(vd_asfloat(_JitterFlickerThreshold))) + 1.0));
  r0.y = vd_asuint((1.0 / vd_asfloat(r0.y)));
  r0.x = vd_asuint(saturate((vd_asfloat(r0.y) * vd_asfloat(r0.x))));
  r0.y = vd_asuint((vd_asfloat(r0.x) * -2.0 + 3.0));
  r0.x = vd_asuint((vd_asfloat(r0.x) * vd_asfloat(r0.x)));
  r0.x = vd_asuint((vd_asfloat(r0.x) * vd_asfloat(r0.y)));
  r0.y = vd_asuint((vd_asfloat(_Time.y) * vd_asfloat(_JitterSpeed)));
  r0.y = vd_asuint(floor(vd_asfloat(r0.y)));
  r0.yz = vd_asuint((vd_asfloat(r0.yy) * float2(91.3450012, 17.9130001)));
  r0.yz = vd_asuint(sin(vd_asfloat(r0.yz)));
  r0.yz = vd_asuint((vd_asfloat(r0.yz) * float2(43758.5469, 43758.5469)));
  r0.yz = vd_asuint(frac(vd_asfloat(r0.yz)));
  r0.yz = vd_asuint((vd_asfloat(r0.yz) + float2(-0.5, -0.5)));
  r0.yz = vd_asuint((vd_asfloat(r0.yz) * vd_asfloat((_VertexJitterStrength).xx)));
  r1.xyzw = vd_asuint((vd_asfloat(v0.yyyy) * vd_asfloat((VD_COL(unity_ObjectToWorld, 1).xyzw))));
  r1.xyzw = vd_asuint((vd_asfloat((VD_COL(unity_ObjectToWorld, 0).xyzw)) * vd_asfloat(v0.xxxx) + vd_asfloat(r1.xyzw)));
  r1.xyzw = vd_asuint((vd_asfloat((VD_COL(unity_ObjectToWorld, 2).xyzw)) * vd_asfloat(v0.zzzz) + vd_asfloat(r1.xyzw)));
  r1.xyzw = vd_asuint((vd_asfloat(r1.xyzw) + vd_asfloat((VD_COL(unity_ObjectToWorld, 3).xyzw))));
  r2.xyzw = vd_asuint((vd_asfloat(r1.yyyy) * vd_asfloat((VD_COL(unity_MatrixVP, 1).xyzw))));
  r2.xyzw = vd_asuint((vd_asfloat((VD_COL(unity_MatrixVP, 0).xyzw)) * vd_asfloat(r1.xxxx) + vd_asfloat(r2.xyzw)));
  r2.xyzw = vd_asuint((vd_asfloat((VD_COL(unity_MatrixVP, 2).xyzw)) * vd_asfloat(r1.zzzz) + vd_asfloat(r2.xyzw)));
  r1.xyzw = vd_asuint((vd_asfloat((VD_COL(unity_MatrixVP, 3).xyzw)) * vd_asfloat(r1.wwww) + vd_asfloat(r2.xyzw)));
  r0.yz = vd_asuint((vd_asfloat(r0.yz) * vd_asfloat(r1.ww)));
  o0.xy = (vd_asfloat(r0.yz) * vd_asfloat(r0.xx) + vd_asfloat(r1.xy));
  o0.zw = vd_asfloat(r1.zw);
  o1.xyzw = vd_asfloat(vd_asuint(v1.xyzw));
  o2.xy = (vd_asfloat(v2.xy) * vd_asfloat(_MainTex_ST.xy) + vd_asfloat(_MainTex_ST.zw));
  o3.xyzw = vd_asfloat(vd_asuint(v0.xyzw));
  return;
}
