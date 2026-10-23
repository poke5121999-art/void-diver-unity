// LCArt/UI/Shader_DE_UI_SpineSkeletonGraphic | pass 0 "Normal" | progVertex | keywords: (none) | blob 1
void vert(
  float4 v0 : POSITION0,
  float4 v1 : COLOR0,
  float4 v2 : TEXCOORD0,
  float4 v3 : TEXCOORD1,
  float2 v4 : TEXCOORD2,
  out float4 o0 : SV_POSITION0,
  out float4 o1 : VDREG1,
  out float4 o2 : VDREG2,
  out float4 o3 : VDREG3,
  out float4 o4 : VDREG4)
{
  uint4 r0, r1, r2;
  r0 = 0u; r1 = 0u; r2 = 0u;

  r0.xyzw = vd_asuint((vd_asfloat(v0.yyyy) * vd_asfloat((VD_COL(unity_ObjectToWorld, 1).xyzw))));
  r0.xyzw = vd_asuint((vd_asfloat((VD_COL(unity_ObjectToWorld, 0).xyzw)) * vd_asfloat(v0.xxxx) + vd_asfloat(r0.xyzw)));
  r0.xyzw = vd_asuint((vd_asfloat((VD_COL(unity_ObjectToWorld, 2).xyzw)) * vd_asfloat(v0.zzzz) + vd_asfloat(r0.xyzw)));
  r0.xyzw = vd_asuint((vd_asfloat(r0.xyzw) + vd_asfloat((VD_COL(unity_ObjectToWorld, 3).xyzw))));
  r1.xyzw = vd_asuint((vd_asfloat(r0.yyyy) * vd_asfloat((VD_COL(unity_MatrixVP, 1).xyzw))));
  r1.xyzw = vd_asuint((vd_asfloat((VD_COL(unity_MatrixVP, 0).xyzw)) * vd_asfloat(r0.xxxx) + vd_asfloat(r1.xyzw)));
  r1.xyzw = vd_asuint((vd_asfloat((VD_COL(unity_MatrixVP, 2).xyzw)) * vd_asfloat(r0.zzzz) + vd_asfloat(r1.xyzw)));
  o0.xyzw = (vd_asfloat((VD_COL(unity_MatrixVP, 3).xyzw)) * vd_asfloat(r0.wwww) + vd_asfloat(r1.xyzw));
  o2.xy = (vd_asfloat(v2.xy) * vd_asfloat(_MainTex_ST.xy) + vd_asfloat(_MainTex_ST.zw));
  r0.x = vd_asuint((-(int)(0.5 < vd_asfloat(_PMAVertexColorInput))));
  if (r0.x != 0u) {
    r0.x = vd_asuint((-(int)(0.5 < vd_asfloat(_UIVertexColorAlwaysGammaSpace))));
    r0.yzw = vd_asuint((-(int3)(float3(0.00313080009, 0.00313080009, 0.00313080009) >= vd_asfloat(v1.xyz))));
    r1.xyz = vd_asuint((vd_asfloat(v1.xyz) * float3(12.9232101, 12.9232101, 12.9232101)));
    r2.xyz = vd_asuint(log2(abs(vd_asfloat(v1.xyz))));
    r2.xyz = vd_asuint((vd_asfloat(r2.xyz) * float3(0.416666657, 0.416666657, 0.416666657)));
    r2.xyz = vd_asuint(exp2(vd_asfloat(r2.xyz)));
    r2.xyz = vd_asuint((vd_asfloat(r2.xyz) * float3(1.05499995, 1.05499995, 1.05499995) + float3(-0.0549999997, -0.0549999997, -0.0549999997)));
    r0.yzw = ((r0.yzw != 0u) ? r1.xyz : r2.xyz);
    r0.xyz = ((r0.xxx != 0u) ? vd_asuint(v1.xyz) : r0.yzw);
    r0.w = vd_asuint((-(int)(0.00100000005 < vd_asfloat(v1.w))));
    r1.xyz = vd_asuint((vd_asfloat(r0.xyz) / vd_asfloat(v1.www)));
    r0.xyz = vd_asuint(saturate(((r0.www != 0u) ? vd_asfloat(r1.xyz) : vd_asfloat(r0.xyz))));
    r1.xyz = vd_asuint((vd_asfloat(r0.xyz) * float3(0.0773993805, 0.0773993805, 0.0773993805)));
    r2.xyz = vd_asuint((vd_asfloat(r0.xyz) + float3(0.0549999997, 0.0549999997, 0.0549999997)));
    r2.xyz = vd_asuint((vd_asfloat(r2.xyz) * float3(0.947867334, 0.947867334, 0.947867334)));
    r2.xyz = vd_asuint(log2(vd_asfloat(r2.xyz)));
    r2.xyz = vd_asuint((vd_asfloat(r2.xyz) * float3(2.4000001, 2.4000001, 2.4000001)));
    r2.xyz = vd_asuint(exp2(vd_asfloat(r2.xyz)));
    r0.xyz = vd_asuint((-(int3)(float3(0.0404499993, 0.0404499993, 0.0404499993) >= vd_asfloat(r0.xyz))));
    r0.xyz = ((r0.xyz != 0u) ? r1.xyz : r2.xyz);
  } else {
    r0.xyz = vd_asuint(v1.xyz);
  }
  r1.xy = vd_asuint((vd_asfloat(v3.xy) * float2(0.0773993805, 0.0773993805)));
  r1.zw = vd_asuint((vd_asfloat(v3.xy) + float2(0.0549999997, 0.0549999997)));
  r1.zw = vd_asuint((vd_asfloat(r1.zw) * float2(0.947867334, 0.947867334)));
  r1.zw = vd_asuint(log2(abs(vd_asfloat(r1.zw))));
  r1.zw = vd_asuint((vd_asfloat(r1.zw) * float2(2.4000001, 2.4000001)));
  r1.zw = vd_asuint(exp2(vd_asfloat(r1.zw)));
  r2.xy = vd_asuint((-(int2)(float2(0.0404499993, 0.0404499993) >= vd_asfloat(v3.xy))));
  o4.xy = vd_asfloat(((r2.xy != 0u) ? r1.xy : r1.zw));
  r1.x = vd_asuint((vd_asfloat(v4.x) * 0.0773993805));
  r1.y = vd_asuint((vd_asfloat(v4.x) + 0.0549999997));
  r1.y = vd_asuint((vd_asfloat(r1.y) * 0.947867334));
  r1.y = vd_asuint(log2(abs(vd_asfloat(r1.y))));
  r1.y = vd_asuint((vd_asfloat(r1.y) * 2.4000001));
  r1.y = vd_asuint(exp2(vd_asfloat(r1.y)));
  r1.z = vd_asuint((-(int)(0.0404499993 >= vd_asfloat(v4.x))));
  o4.z = vd_asfloat(((r1.z != 0u) ? r1.x : r1.y));
  r0.w = vd_asuint(v1.w);
  o1.xyzw = vd_asfloat(r0.xyzw);
  o3.xyzw = vd_asfloat(vd_asuint(v0.xyzw));
  return;
}
