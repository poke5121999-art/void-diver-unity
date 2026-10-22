// LCArt/Background/shader_DE_BG_Base | pass 1 "DepthNormals" | progVertex | keywords: _DE_REFLECTION_CAMERA | blob 59
void vert(
  float4 v0 : POSITION0,
  float4 v1 : TANGENT0,
  float3 v2 : NORMAL0,
  float4 v3 : TEXCOORD0,
  float4 v4 : TEXCOORD1,
  float4 v5 : COLOR0,
  out float4 o0 : SV_POSITION0,
  out float2 o1 : TEXCOORD1,
  out float p1 : TEXCOORD6,
  out float4 o2 : TEXCOORD8,
  out float4 o3 : TEXCOORD2,
  out float4 o4 : TEXCOORD3,
  out float4 o5 : TEXCOORD4,
  out float3 o6 : TEXCOORD5,
  out float4 o7 : COLOR0,
  out float4 o8 : TEXCOORD7)
{
  o0.xyzw = float4(0,0,0,1);
  o1.xyz = float3(0,0,0);
  o2.xyz = float3(0,0,0);
  o3.xyz = float3(0,0,0);
  o4.xyz = float3(0,0,0);
  o5.xyz = float3(0,0,0);
  o6.xyz = float3(0,0,0);
  o7.xyzw = v5.xyzw;
  o8.xyzw = float4(0,0,0,0);
  return;
}
