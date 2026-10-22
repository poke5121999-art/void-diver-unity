// LCArt/UI/Shader_UI_Additive | pass 0 "Default" | progFragment | keywords: UNITY_UI_ALPHACLIP | blob 8
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
  r0.xyzw = v1.xyzw * r0.xyzw;
  o0.xyzw = r0.xyzw;
  r0.x = cmp(r1.x < 0);
  if (r0.x != 0) discard;
  return;
}
