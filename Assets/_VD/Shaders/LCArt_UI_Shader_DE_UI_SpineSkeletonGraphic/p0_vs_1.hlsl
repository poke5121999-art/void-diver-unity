// LCArt/UI/Shader_DE_UI_SpineSkeletonGraphic | pass 0 "Normal" | progVertex | keywords: (none) | blob 1
void vert(
  float4 v0 : POSITION0,
  float4 v1 : COLOR0,
  float4 v2 : TEXCOORD0,
  float4 v3 : TEXCOORD1,
  float2 v4 : TEXCOORD2,
  out float4 o0 : SV_POSITION0,
  out float4 o1 : COLOR0,
  out float4 o2 : TEXCOORD0,
  out float4 o3 : TEXCOORD1,
  out float3 o4 : TEXCOORD2)
{
  float4 r0,r1,r2;
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
  o2.xy = v2.xy * _MainTex_ST.xy + _MainTex_ST.zw;
  r0.x = cmp(0.5 < _PMAVertexColorInput);
  if (r0.x != 0) {
    r0.x = cmp(0.5 < _UIVertexColorAlwaysGammaSpace);
    r0.yzw = cmp(float3(0.00313080009,0.00313080009,0.00313080009) >= v1.xyz);
    r1.xyz = float3(12.9232101,12.9232101,12.9232101) * v1.xyz;
    r2.xyz = log2(abs(v1.xyz));
    r2.xyz = float3(0.416666657,0.416666657,0.416666657) * r2.xyz;
    r2.xyz = exp2(r2.xyz);
    r2.xyz = r2.xyz * float3(1.05499995,1.05499995,1.05499995) + float3(-0.0549999997,-0.0549999997,-0.0549999997);
    r0.yzw = r0.yzw ? r1.xyz : r2.xyz;
    r0.xyz = r0.xxx ? v1.xyz : r0.yzw;
    r0.w = cmp(0.00100000005 < v1.w);
    r1.xyz = r0.xyz / v1.www;
    r0.xyz = saturate(r0.www ? r1.xyz : r0.xyz);
    r1.xyz = float3(0.0773993805,0.0773993805,0.0773993805) * r0.xyz;
    r2.xyz = float3(0.0549999997,0.0549999997,0.0549999997) + r0.xyz;
    r2.xyz = float3(0.947867334,0.947867334,0.947867334) * r2.xyz;
    r2.xyz = log2(r2.xyz);
    r2.xyz = float3(2.4000001,2.4000001,2.4000001) * r2.xyz;
    r2.xyz = exp2(r2.xyz);
    r0.xyz = cmp(float3(0.0404499993,0.0404499993,0.0404499993) >= r0.xyz);
    r0.xyz = r0.xyz ? r1.xyz : r2.xyz;
  } else {
    r0.xyz = v1.xyz;
  }
  r1.xy = float2(0.0773993805,0.0773993805) * v3.xy;
  r1.zw = float2(0.0549999997,0.0549999997) + v3.xy;
  r1.zw = float2(0.947867334,0.947867334) * r1.zw;
  r1.zw = log2(abs(r1.zw));
  r1.zw = float2(2.4000001,2.4000001) * r1.zw;
  r1.zw = exp2(r1.zw);
  r2.xy = cmp(float2(0.0404499993,0.0404499993) >= v3.xy);
  o4.xy = r2.xy ? r1.xy : r1.zw;
  r1.x = 0.0773993805 * v4.x;
  r1.y = 0.0549999997 + v4.x;
  r1.y = 0.947867334 * r1.y;
  r1.y = log2(abs(r1.y));
  r1.y = 2.4000001 * r1.y;
  r1.y = exp2(r1.y);
  r1.z = cmp(0.0404499993 >= v4.x);
  o4.z = r1.z ? r1.x : r1.y;
  r0.w = v1.w;
  o1.xyzw = r0.xyzw;
  o3.xyzw = v0.xyzw;
  return;
}
