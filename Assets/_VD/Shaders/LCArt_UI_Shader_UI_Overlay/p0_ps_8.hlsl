// LCArt/UI/Shader_UI_Overlay | pass 0 "OverlayBlend" | progFragment | keywords: UNITY_UI_ALPHACLIP | blob 8
void frag(
  float4 v0 : SV_POSITION0,
  float4 v1 : COLOR0,
  float4 v2 : TEXCOORD0,
  float4 v3 : TEXCOORD1,
  out float4 o0 : SV_Target0)
{
  float4 r0,r1;
  uint4 bitmask, uiDest;
  float4 fDest;

  r0.xyzw = _MainTex.Sample(sampler_MainTex, v2.xy).xyzw;
  r0.xyzw = _TextureSampleAdd.xyzw + r0.xyzw;
  r1.x = r0.w * v1.w + -0.00100000005;
  r1.x = cmp(r1.x < 0);
  if (r1.x != 0) discard;
  r0.xyz = -r0.xyz * v1.xyz + float3(1,1,1);
  r0.w = v1.w * r0.w;
  o0.w = r0.w;
  r0.xyz = float3(0.5,0.5,0.5) + -r0.xyz;
  o0.xyz = (_OverlayStrength).xxx * r0.xyz + float3(0.5,0.5,0.5);
  return;
}
