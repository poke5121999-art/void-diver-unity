// LCArt/Character/shader_DE_Character_Additive | pass 0 "Additive" | progFragment | keywords: _DE_EDITOR_CAMERA _DE_PRERENDERING_CAMERA | blob 9
void frag(
  float4 v0 : SV_POSITION0,
  float2 v1 : TEXCOORD0,
  float w1 : TEXCOORD2,
  float4 v2 : TEXCOORD1,
  float4 v3 : TEXCOORD6,
  float4 v4 : TEXCOORD3,
  float3 v5 : TEXCOORD4,
  float4 v6 : COLOR0,
  out float4 o0 : SV_Target0)
{
  float4 r0,r1,r2;
  uint4 bitmask, uiDest;
  float4 fDest;

  r0.xyzw = _MainTex.SampleBias(sampler_MainTex, v1.xy, _GlobalMipBias.x).xyzw;
  r1.x = -0.00100000005 + r0.w;
  r1.x = cmp(r1.x < 0);
  if (r1.x != 0) discard;
  r1.x = cmp(0.5 < _UseLinearVertexColor);
  r1.yzw = v6.xyz * r0.xyz;
  r2.xyz = log2(abs(v6.xyz));
  r2.xyz = float3(2.20000005,2.20000005,2.20000005) * r2.xyz;
  r2.xyz = exp2(r2.xyz);
  r0.xyz = r2.xyz * r0.xyz;
  r0.xyz = r1.xxx ? r1.yzw : r0.xyz;
  r0.w = log2(abs(r0.w));
  r0.w = 1.5 * r0.w;
  r0.w = exp2(r0.w);
  r1.x = cmp(_GlobalBoxAreaEnable >= 0.5);
  if (r1.x != 0) {
    r1.x = 0;
    while (true) {
      r1.y = cmp((int)r1.x >= 8);
      if (r1.y != 0) break;
      r1.y = cmp(_GlobalBoxAreaCoverCharacters[(int)(r1.x)] < 0.5);
      if (r1.y != 0) {
        r1.y = (int)r1.x + 1;
        r1.x = r1.y;
        continue;
      }
      r1.yzw = -_GlobalBoxAreaCenters[(int)(r1.x)].xyz + (VD_COL(unity_ObjectToWorld, 3).xyz);
      r1.yzw = -_GlobalBoxAreaExtents[(int)(r1.x)].xyz + abs(r1.yzw);
      r1.y = max(r1.y, r1.z);
      r1.y = max(r1.y, r1.w);
      r1.y = cmp(0 >= r1.y);
      if (r1.y != 0) {
        if (-1 != 0) discard;
        break;
      }
      r1.x = (int)r1.x + 1;
    }
  }
  r1.x = -1 + w1.x;
  r1.x = _SightEffectEnable * r1.x + 1;
  r0.xyz = r1.xxx * r0.xyz;
  r0.xyz = float3(0.5,0.5,0.5) * r0.xyz;
  r0.xyz = r0.xyz * r0.www;
  r1.xyz = _AdditiveTint.xyz * r0.xyz;
  r0.w = saturate(_DissolveProgress + _DissolveProgress);
  r0.xyz = -r0.xyz * _AdditiveTint.xyz + _DissolveColor.xyz;
  r0.xyz = r0.www * r0.xyz + r1.xyz;
  r0.w = saturate(-0.5 + _DissolveProgress);
  r0.w = -r0.w * 2 + 1;
  r0.xyz = r0.xyz * r0.www;
  r0.xyz = log2(abs(r0.xyz));
  r0.xyz = (_AdditivePower).xxx * r0.xyz;
  r0.xyz = exp2(r0.xyz);
  o0.xyz = (_AdditiveIntensity).xxx * r0.xyz;
  o0.w = v6.w;
  return;
}
