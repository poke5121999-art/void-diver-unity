// LCArt/UI/Shader_DE_UI_Speaker | pass 0 "Default" | progFragment | keywords: (none) | blob 3
void frag(
  float4 v0 : SV_POSITION0,
  float4 v1 : COLOR0,
  float2 v2 : TEXCOORD0,
  float2 w2 : TEXCOORD3,
  float4 v3 : TEXCOORD1,
  float4 v4 : TEXCOORD2,
  out float4 o0 : SV_Target0)
{
  float4 r0,r1,r2;
  uint4 bitmask, uiDest;
  float4 fDest;

  r0.xyzw = _MainTex.Sample(sampler_MainTex, v2.xy).xyzw;
  r0.xyzw = _TextureSampleAdd.xyzw + r0.xyzw;
  r1.x = r0.w * v1.w + -0.00100000005;
  r0.xyzw = v1.xyzw * r0.xyzw;
  r1.x = cmp(r1.x < 0);
  if (r1.x != 0) discard;
  r1.xy = w2.xy * (_GlobalDialogRimLightOffset).xx + v2.xy;
  r1.xyzw = _MainTex.Sample(sampler_MainTex, r1.xy).xyzw;
  r1.x = 1 + -r1.w;
  r1.x = saturate(r1.x * r0.w);
  r1.y = cmp(r0.w >= _TransparentAlphaThreshold);
  r1.y = r1.y ? 1.000000 : 0;
  r1.x = r1.y * r1.x;
  r1.yzw = _GlobalDialogRimLightColor.xyz + -_GlobalDialogBaseColor.xyz;
  r1.xyz = r1.xxx * r1.yzw + _GlobalDialogBaseColor.xyz;
  r2.xyz = r1.xyz * r0.xyz;
  r0.xyz = -r0.xyz * r1.xyz + _GradientColor.xyz;
  o0.w = r0.w;
  r0.w = v4.y / v4.w;
  r0.w = -_GradientHeightMax + r0.w;
  r1.x = _GradientHeightMin + -_GradientHeightMax;
  r1.x = 1 / r1.x;
  r0.w = saturate(r1.x * r0.w);
  r1.x = r0.w * -2 + 3;
  r0.w = r0.w * r0.w;
  r0.w = r1.x * r0.w;
  r0.w = _GradientColor.w * r0.w;
  o0.xyz = r0.www * r0.xyz + r2.xyz;
  return;
}
