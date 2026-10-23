// LCArt/Character/shader_DE_Character_Normal | pass 2 "DepthNormals" | progVertex | keywords: _DE_REFLECTION_CAMERA | blob 72
void vert(
  float4 v0 : POSITION0,
  float2 v1 : TEXCOORD0,
  float4 v2 : COLOR0,
  out float4 o0 : SV_POSITION0,
  out float4 o1 : VDREG1,
  out float4 o2 : VDREG2,
  out float4 o3 : VDREG3)
{

  o0.xyzw = vd_asfloat(uint4(0x00000000u, 0x00000000u, 0x00000000u, 0x3f800000u));
  o1.xyz = vd_asfloat(uint3(0x00000000u, 0x00000000u, 0x00000000u));
  o2.xyzw = vd_asfloat(uint4(0x00000000u, 0x00000000u, 0x00000000u, 0x00000000u));
  o3.xyzw = vd_asfloat(uint4(0x00000000u, 0x00000000u, 0x00000000u, 0x00000000u));
  return;
}
