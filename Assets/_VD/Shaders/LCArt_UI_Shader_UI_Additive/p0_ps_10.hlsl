// LCArt/UI/Shader_UI_Additive | pass 0 "Default" | progFragment | keywords: UNITY_UI_ALPHACLIP UNITY_UI_CLIP_RECT | blob 10
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

  r0.xy = cmp(v3.xy >= _ClipRect.xy);
  r0.zw = cmp(_ClipRect.zw >= v3.xy);
  r0.xyzw = r0.xyzw ? float4(1,1,1,1) : 0;
  r0.xy = r0.xy * r0.zw;
  r0.x = r0.x * r0.y;
  r1.xyzw = _MainTex.Sample(sampler_MainTex, v2.xy).xyzw;
  r1.xyzw = _TextureSampleAdd.xyzw + r1.xyzw;
  r1.xyzw = v1.xyzw * r1.xyzw;
  r0.y = r1.w * r0.x + -0.00100000005;
  r0.x = r1.w * r0.x;
  o0.xyz = r1.xyz;
  o0.w = r0.x;
  r0.x = cmp(r0.y < 0);
  if (r0.x != 0) discard;
  return;
}
