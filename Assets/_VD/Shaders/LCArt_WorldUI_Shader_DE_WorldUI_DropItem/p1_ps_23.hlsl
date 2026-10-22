// LCArt/WorldUI/Shader_DE_WorldUI_DropItem | pass 1 "Outline" | progFragment | keywords: (none) | blob 23
void frag(
  float4 v0 : SV_POSITION0,
  float2 v1 : TEXCOORD0,
  float w1 : TEXCOORD1,
  out float4 o0 : SV_Target0)
{
  float4 r0,r1,r2;
  uint4 bitmask, uiDest;
  float4 fDest;

  r0.xy = (_OutlineStroke).xx * _MainTex_TexelSize.xy;
  r0.xy = float2(5,5) * r0.xy;
  r0.z = 0;
  r1.xyzw = v1.xyxy + r0.zyxz;
  r0.xyzw = v1.xyxy + -r0.zyxz;
  r2.xyzw = _MainTex.SampleBias(sampler_MainTex, r1.xy, _GlobalMipBias.x).xyzw;
  r1.xyzw = _MainTex.SampleBias(sampler_MainTex, r1.zw, _GlobalMipBias.x).xyzw;
  r1.x = -0.100000001 + r1.w;
  r1.y = -0.100000001 + r2.w;
  r1.xy = saturate(float2(6.66666651,6.66666651) * r1.xy);
  r1.z = r1.y * -2 + 3;
  r1.y = r1.y * r1.y;
  r1.y = r1.z * r1.y;
  r2.xyzw = _MainTex.SampleBias(sampler_MainTex, r0.xy, _GlobalMipBias.x).xyzw;
  r0.xyzw = _MainTex.SampleBias(sampler_MainTex, r0.zw, _GlobalMipBias.x).xyzw;
  r0.x = -0.100000001 + r0.w;
  r0.y = -0.100000001 + r2.w;
  r0.xy = saturate(float2(6.66666651,6.66666651) * r0.xy);
  r0.z = r0.y * -2 + 3;
  r0.y = r0.y * r0.y;
  r0.y = r0.z * r0.y;
  r0.y = max(r1.y, r0.y);
  r0.z = r0.x * -2 + 3;
  r0.x = r0.x * r0.x;
  r0.x = r0.z * r0.x;
  r0.z = r1.x * -2 + 3;
  r0.w = r1.x * r1.x;
  r0.z = r0.z * r0.w;
  r0.x = max(r0.x, r0.z);
  r0.x = max(r0.y, r0.x);
  r1.xyzw = _MainTex.SampleBias(sampler_MainTex, v1.xy, _GlobalMipBias.x).xyzw;
  r0.y = r1.w * w1.x + -0.899999976;
  r0.y = saturate(9.99999809 * r0.y);
  r0.z = r0.y * -2 + 3;
  r0.y = r0.y * r0.y;
  r0.y = -r0.z * r0.y + 1;
  r0.x = r0.x * r0.y;
  r0.y = r0.x * _OutlineEnable + -0.00999999978;
  r0.x = _OutlineEnable * r0.x;
  o0.w = 0.375 * r0.x;
  r0.x = cmp(r0.y < 0);
  if (r0.x != 0) discard;
  o0.xyz = _OutlineColor.xyz;
  return;
}
