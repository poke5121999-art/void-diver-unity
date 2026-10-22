// LCArt/VFX/VFX_Decoration_SurfaceCutterTarget | pass 0 "ForwardLit" | progFragment | keywords: (none) | blob 3
void frag(
  float4 v0 : SV_POSITION0,
  float4 v1 : TEXCOORD3,
  float2 v2 : TEXCOORD0,
  float4 v3 : COLOR0,
  float4 v4 : TEXCOORD1,
  float4 v5 : TEXCOORD2,
  out float4 o0 : SV_Target0)
{
  float4 r0;
  uint4 bitmask, uiDest;
  float4 fDest;

  r0.x = max(0.00100000005, v4.x);
  r0.x = saturate(-v1.y / r0.x);
  r0.x = log2(r0.x);
  r0.x = v4.y * r0.x;
  r0.x = exp2(r0.x);
  r0.yzw = v5.xyz + -v3.xyz;
  o0.xyz = r0.xxx * r0.yzw + v3.xyz;
  o0.w = 1;
  return;
}
