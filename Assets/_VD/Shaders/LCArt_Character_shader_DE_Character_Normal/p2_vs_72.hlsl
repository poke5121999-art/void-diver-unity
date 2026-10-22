// LCArt/Character/shader_DE_Character_Normal | pass 2 "DepthNormals" | progVertex | keywords: _DE_REFLECTION_CAMERA | blob 72
void vert(
  float4 v0 : POSITION0,
  float2 v1 : TEXCOORD0,
  float4 v2 : COLOR0,
  out float4 o0 : SV_POSITION0,
  out float2 o1 : TEXCOORD0,
  out float p1 : TEXCOORD1,
  out float4 o2 : TEXCOORD2,
  out float4 o3 : COLOR0)
{
  o0.xyzw = float4(0,0,0,1);
  o1.xyz = float3(0,0,0);
  o2.xyzw = float4(0,0,0,0);
  o3.xyzw = float4(0,0,0,0);
  return;
}
