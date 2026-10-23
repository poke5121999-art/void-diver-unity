// LCArt/Background/shader_DE_BG_Base | pass 1 "DepthNormals" | progVertex | keywords: _DE_REFLECTION_CAMERA | blob 59
void vert(
  float4 v0 : POSITION0,
  float4 v1 : TANGENT0,
  float3 v2 : NORMAL0,
  float4 v3 : TEXCOORD0,
  float4 v4 : TEXCOORD1,
  float4 v5 : COLOR0,
  out float4 o0 : SV_POSITION0,
  out float4 o1 : VDREG1,
  out float4 o2 : VDREG2,
  out float4 o3 : VDREG3,
  out float4 o4 : VDREG4,
  out float4 o5 : VDREG5,
  out float4 o6 : VDREG6,
  out float4 o7 : VDREG7,
  out float4 o8 : VDREG8)
{

  o0.xyzw = vd_asfloat(uint4(0x00000000u, 0x00000000u, 0x00000000u, 0x3f800000u));
  o1.xyz = vd_asfloat(uint3(0x00000000u, 0x00000000u, 0x00000000u));
  o2.xyz = vd_asfloat(uint3(0x00000000u, 0x00000000u, 0x00000000u));
  o3.xyz = vd_asfloat(uint3(0x00000000u, 0x00000000u, 0x00000000u));
  o4.xyz = vd_asfloat(uint3(0x00000000u, 0x00000000u, 0x00000000u));
  o5.xyz = vd_asfloat(uint3(0x00000000u, 0x00000000u, 0x00000000u));
  o6.xyz = vd_asfloat(uint3(0x00000000u, 0x00000000u, 0x00000000u));
  o7.xyzw = vd_asfloat(vd_asuint(v5.xyzw));
  o8.xyzw = vd_asfloat(uint4(0x00000000u, 0x00000000u, 0x00000000u, 0x00000000u));
  return;
}
