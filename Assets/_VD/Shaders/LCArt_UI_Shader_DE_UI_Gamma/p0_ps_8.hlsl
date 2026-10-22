// LCArt/UI/Shader_DE_UI_Gamma | pass 0 "Default" | progFragment | keywords: UNITY_UI_ALPHACLIP | blob 8
void frag(
  float4 v0 : SV_POSITION0,
  float4 v1 : COLOR0,
  float4 v2 : TEXCOORD0,
  float4 v3 : TEXCOORD1,
  out float4 o0 : SV_Target0)
{
  float4 r0,r1,r2;
  uint4 bitmask, uiDest;
  float4 fDest;

  r0.xyzw = _MainTex.SampleBias(sampler_MainTex, v2.xy, _GlobalMipBias.x).xyzw;
  r0.xyzw = _TextureSampleAdd.xyzw + r0.xyzw;
  r1.x = r0.w * v1.w + -0.00100000005;
  r0.xyzw = v1.xyzw * r0.xyzw;
  r1.x = cmp(r1.x < 0);
  if (r1.x != 0) discard;
  r1.xyz = cmp(float3(0,0,0) < r0.xyz);
  r2.xyz = cmp(r0.xyz < float3(0,0,0));
  r1.xyz = (int3)-r1.xyz + (int3)r2.xyz;
  r1.xyz = (int3)r1.xyz;
  r0.xyz = log2(abs(r0.xyz));
  o0.w = r0.w;
  r0.w = max(0.00100000005, _GammaDegree);
  r0.w = 1 / r0.w;
  r0.xyz = r0.www * r0.xyz;
  r0.xyz = exp2(r0.xyz);
  o0.xyz = r1.xyz * r0.xyz;
  return;
}
