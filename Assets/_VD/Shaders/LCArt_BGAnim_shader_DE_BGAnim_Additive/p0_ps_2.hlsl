// LCArt/BGAnim/shader_DE_BGAnim_Additive | pass 0 "ForwardLit" | progFragment | keywords: (none) | blob 2
void frag(
  float4 v0 : SV_POSITION0,
  float2 v1 : TEXCOORD0,
  float2 w1 : TEXCOORD7,
  float3 v2 : TEXCOORD1,
  float w2 : TEXCOORD8,
  float3 v3 : NORMAL0,
  float3 v4 : TEXCOORD4,
  float4 v5 : COLOR0,
  float4 v6 : TEXCOORD9,
  out float4 o0 : SV_Target0)
{
  float4 r0,r1,r2;
  uint4 bitmask, uiDest;
  float4 fDest;

  r0.x = cmp(_GlobalBoxAreaEnable >= 0.5);
  if (r0.x != 0) {
    r0.x = 0;
    while (true) {
      r0.y = cmp((int)r0.x >= 8);
      if (r0.y != 0) break;
      r0.y = cmp(_GlobalBoxAreaCoverCharacters[(int)(r0.x)] < 0.5);
      if (r0.y != 0) {
        r0.y = (int)r0.x + 1;
        r0.x = r0.y;
        continue;
      }
      r0.yzw = -_GlobalBoxAreaCenters[(int)(r0.x)].xyz + (VD_COL(unity_ObjectToWorld, 3).xyz);
      r0.yzw = -_GlobalBoxAreaExtents[(int)(r0.x)].xyz + abs(r0.yzw);
      r0.y = max(r0.y, r0.z);
      r0.y = max(r0.y, r0.w);
      r0.y = cmp(0 >= r0.y);
      if (r0.y != 0) {
        if (-1 != 0) discard;
        break;
      }
      r0.x = (int)r0.x + 1;
    }
  }
  r0.xyzw = _MainTex.SampleBias(sampler_MainTex, v1.xy, _GlobalMipBias.x).xyzw;
  r1.x = -0.100000001 + r0.w;
  r1.x = cmp(r1.x < 0);
  if (r1.x != 0) discard;
  r1.xyz = log2(abs(v5.xyz));
  r1.xyz = float3(2.20000005,2.20000005,2.20000005) * r1.xyz;
  r1.xyz = exp2(r1.xyz);
  r0.xyz = r1.xyz * r0.xyz;
  r0.w = log2(abs(r0.w));
  r0.w = 1.5 * r0.w;
  o0.w = exp2(r0.w);
  r1.xy = v6.xy / v6.ww;
  r1.xyzw = _CustomDepthBuffer.SampleBias(sampler_CustomDepthBuffer, r1.xy, _GlobalMipBias.x).xyzw;
  r0.w = v0.z + -r1.z;
  r0.w = -7.50000036e-005 + r0.w;
  r0.w = saturate(-6666.6665 * r0.w);
  r1.x = r0.w * -2 + 3;
  r0.w = r0.w * r0.w;
  r1.y = r1.x * r0.w;
  r2.xy = float2(1,1) + -float2(_GlobalSeeThroughCircleRange, _GlobalSeeThroughCircleAlpha);
  r1.z = 1 + -r2.x;
  r1.w = -r2.x + r1.w;
  r1.z = 1 / r1.z;
  r1.z = saturate(r1.w * r1.z);
  r1.w = r1.z * -2 + 3;
  r1.z = r1.z * r1.z;
  r1.z = r1.w * r1.z;
  r0.w = -r1.x * r0.w + 1;
  r0.w = r1.z * r0.w + r1.y;
  r0.w = -1 + r0.w;
  r0.w = _GlobalBackgroundSeeThroughEnable * r0.w + 1;
  r0.w = r0.w * r2.y + _GlobalSeeThroughCircleAlpha;
  r1.x = _CharacterAreaMaskOffEnable * _GlobalPlaneSystemEnable;
  r1.x = _GlobalBackgroundSeeThroughEnable * r1.x;
  r0.w = -1 + r0.w;
  r0.w = r1.x * r0.w + 1;
  r0.w = min(w2.x, r0.w);
  r0.w = -1 + r0.w;
  r0.w = _SightEffectEnable * r0.w + 1;
  r0.xyz = r0.xyz * r0.www;
  r1.xyz = float3(0.5,0.5,0.5) * r0.xyz;
  r0.w = saturate(_DissolveProgress + _DissolveProgress);
  r0.xyz = -r0.xyz * float3(0.5,0.5,0.5) + _DissolveColor.xyz;
  r0.xyz = r0.www * r0.xyz + r1.xyz;
  r0.w = saturate(-0.5 + _DissolveProgress);
  r0.w = -r0.w * 2 + 1;
  r0.xyz = r0.xyz * r0.www;
  r0.xyz = log2(abs(r0.xyz));
  r0.xyz = (_AdditivePower).xxx * r0.xyz;
  r0.xyz = exp2(r0.xyz);
  o0.xyz = (_AdditiveIntensity).xxx * r0.xyz;
  return;
}
