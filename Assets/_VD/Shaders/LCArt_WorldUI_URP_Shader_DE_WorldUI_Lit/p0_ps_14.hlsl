// LCArt/WorldUI/URP/Shader_DE_WorldUI_Lit | pass 0 "Forward" | progFragment | keywords: UNITY_UI_ALPHACLIP _ADDITIONAL_LIGHTS | blob 14
void frag(
  float4 v0 : SV_POSITION0,
  float4 v1 : COLOR0,
  float2 v2 : TEXCOORD0,
  float w2 : TEXCOORD3,
  float4 v3 : TEXCOORD1,
  float4 v4 : TEXCOORD2,
  float3 v5 : TEXCOORD7,
  out float4 o0 : SV_Target0)
{
  const float4 icb[] = { { 1.000000, 0, 0, 0},
                              { 0, 1.000000, 0, 0},
                              { 0, 0, 1.000000, 0},
                              { 0, 0, 0, 1.000000} };
  float4 r0,r1,r2,r3,r4,r5;
  uint4 bitmask, uiDest;
  float4 fDest;

  r0.xyzw = _MainTex.SampleBias(sampler_MainTex, v2.xy, _GlobalMipBias.x).xyzw;
  r1.x = -0.00100000005 + r0.w;
  r1.x = cmp(r1.x < 0);
  if (r1.x != 0) discard;
  r1.x = dot(v4.xyz, v4.xyz);
  r1.x = max(6.10351563e-005, r1.x);
  r1.x = rsqrt(r1.x);
  r1.xyz = v4.xyz * r1.xxx;
  r1.w = sqrt(unity_LightData.z);
  r1.w = min(1, r1.w);
  r2.xyz = _MainLightColor.xyz * r1.www;
  r1.w = dot(r1.xyz, _MainLightPosition.xyz);
  r1.w = saturate(r1.w * 0.5 + 0.5);
  r2.w = min(unity_LightData.y, _AdditionalLightsCount.x);
  r2.w = (int)r2.w;
  r3.xyzw = float4(0,0,0,0);
  while (true) {
    r4.x = cmp((uint)r3.w >= (uint)r2.w);
    if (r4.x != 0) break;
    r4.x = (uint)r3.w >> 2;
    r4.y = (int)r3.w & 3;
    r4.x = dot(unity_LightIndices[(int)(r4.x)].xyzw, icb[r4.y+0].xyzw);
    r4.x = (int)r4.x;
    r4.yzw = -v3.xyz * _AdditionalLightsPosition[(int)(r4.x)].www + _AdditionalLightsPosition[(int)(r4.x)].xyz;
    r5.x = dot(r4.yzw, r4.yzw);
    r5.x = max(6.10351563e-005, r5.x);
    r5.y = rsqrt(r5.x);
    r4.yzw = r5.yyy * r4.yzw;
    r5.y = 1 / r5.x;
    r5.x = _AdditionalLightsAttenuation[(int)(r4.x)].x * r5.x;
    r5.x = -r5.x * r5.x + 1;
    r5.x = max(0, r5.x);
    r5.x = r5.x * r5.x;
    r5.x = r5.y * r5.x;
    r5.y = dot(_AdditionalLightsSpotDir[(int)(r4.x)].xyz, r4.yzw);
    r5.y = saturate(r5.y * _AdditionalLightsAttenuation[(int)(r4.x)].z + _AdditionalLightsAttenuation[(int)(r4.x)].w);
    r5.y = r5.y * r5.y;
    r5.x = r5.x * r5.y;
    r5.x = sqrt(r5.x);
    r5.x = min(1, r5.x);
    r5.xyz = _AdditionalLightsColor[(int)(r4.x)].xyz * r5.xxx;
    r4.x = dot(r1.xyz, r4.yzw);
    r4.x = saturate(r4.x * 0.5 + 0.5);
    r3.xyz = r5.xyz * r4.xxx + r3.xyz;
    r3.w = (int)r3.w + 1;
  }
  r1.xyz = r2.xyz * r1.www + r3.xyz;
  o0.xyz = r1.xyz * r0.xyz;
  o0.w = v1.w * r0.w;
  return;
}
