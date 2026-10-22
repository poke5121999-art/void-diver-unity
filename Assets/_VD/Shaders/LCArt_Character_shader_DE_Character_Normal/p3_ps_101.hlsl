// LCArt/Character/shader_DE_Character_Normal | pass 3 "RimLight" | progFragment | keywords: _CLUSTER_LIGHT_LOOP | blob 101
void frag(
  float4 v0 : SV_POSITION0,
  float3 v1 : TEXCOOR2,
  float2 v2 : TEXCOORD0,
  float w2 : TEXCOORD2,
  float4 v3 : TEXCOORD1,
  float4 v4 : TEXCOORD3,
  float4 v5 : COLOR0,
  out float4 o0 : SV_Target0)
{
  float4 r0;
  uint4 bitmask, uiDest;
  float4 fDest;

  r0.xyzw = _MainTex.SampleBias(sampler_MainTex, v2.xy, _GlobalMipBias.x).xyzw;
  r0.x = r0.w * v5.w + -0.899999976;
  r0.x = cmp(r0.x < 0);
  if (r0.x != 0) discard;
  r0.x = cmp(w2.x >= 0.75);
  if (r0.x == 0) discard;
  r0.x = saturate(1 + -_DissolveProgress);
  r0.x = cmp(r0.x >= 0.5);
  if (r0.x == 0) discard;
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
  r0.x = saturate(1 + -_IsLocalGhost);
  r0.x = _GhostEnable * r0.x;
  r0.x = cmp(r0.x >= 0.999000013);
  o0.xyz = r0.xxx ? float3(0,0,0) : v3.xyz;
  o0.w = 1;
  return;
}
