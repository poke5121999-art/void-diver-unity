// LCArt/PostProcessing/CampaignTilt | pass 0 "CampaignTiltKawase" | progVertex | keywords: (none) | blob 1
void vert(
  uint v0 : SV_VertexID0,
  out float4 o0 : SV_POSITION0,
  out float2 o1 : TEXCOORD0)
{
// Needs manual fix for instruction:
// unknown dcl_: dcl_input_sgv v0.x, vertex_id
  float4 r0;
  uint4 bitmask, uiDest;
  float4 fDest;

  r0.x = (uint)v0.x << 1;
  r0.x = (int)r0.x & 2;
  r0.w = (int)v0.x & 2;
  r0.xy = (uint2)r0.xw;
  o0.xy = r0.xy * float2(2,2) + float2(-1,-1);
  r0.z = 1 + -r0.y;
  o1.xy = r0.xz * _BlitScaleBias.xy + _BlitScaleBias.zw;
  o0.zw = float2(1,1);
  return;
}
