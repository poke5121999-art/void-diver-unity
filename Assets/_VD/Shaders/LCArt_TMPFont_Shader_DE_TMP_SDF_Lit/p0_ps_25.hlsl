// LCArt/TMPFont/Shader_DE_TMP_SDF_Lit | pass 0 "TextMeshPro SDF" | progFragment | keywords: UNITY_UI_ALPHACLIP UNITY_UI_CLIP_RECT _ADDITIONAL_LIGHTS | blob 25
void frag(
  float4 v0 : SV_POSITION0,
  float4 v1 : COLOR0,
  float2 v2 : TEXCOORD0,
  float w2 : TEXCOORD8,
  float4 v3 : TEXCOORD1,
  float4 v4 : TEXCOORD2,
  float4 v5 : TEXCOORD3,
  float4 v6 : TEXCOORD5,
  float4 v7 : TEXCOORD6,
  float3 v8 : TEXCOORD7,
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
  r0.x = -v3.x + r0.w;
  r0.x = cmp(r0.x < 0);
  if (r0.x != 0) discard;
  r0.x = v3.z + -r0.w;
  r0.y = _ScaleRatioA * _OutlineWidth;
  r0.z = _ScaleRatioA * _OutlineSoftness;
  r0.yw = v3.yy * r0.yz;
  r1.xyz = _FaceColor.xyz * v1.xyz;
  r2.xy = float2(_FaceUVSpeedX, _FaceUVSpeedY) * _Time.yy + v6.xy;
  r2.xyzw = _FaceTex.SampleBias(sampler_FaceTex, r2.xy, _GlobalMipBias.x).xyzw;
  r1.xyz = r2.xyz * r1.xyz;
  r2.w = _FaceColor.w * r2.w;
  r3.xy = float2(_OutlineUVSpeedX, _OutlineUVSpeedY) * _Time.yy + v6.zw;
  r3.xyzw = _OutlineTex.SampleBias(sampler_OutlineTex, r3.xy, _GlobalMipBias.x).xyzw;
  r3.xyzw = _OutlineColor.xyzw * r3.xyzw;
  r1.w = 0.5 * r0.y;
  r4.x = r0.x * v3.y + -r1.w;
  r0.w = r0.w * 0.5 + r4.x;
  r0.z = r0.z * v3.y + 1;
  r0.z = saturate(r0.w / r0.z);
  r0.z = 1 + -r0.z;
  r0.x = saturate(r0.x * v3.y + r1.w);
  r0.y = min(1, r0.y);
  r0.y = sqrt(r0.y);
  r0.x = r0.x * r0.y;
  r2.xyz = r2.www * r1.xyz;
  r3.xyz = r3.xyz * r3.www;
  r1.xyzw = r3.xyzw + -r2.xyzw;
  r1.xyzw = r0.xxxx * r1.xyzw + r2.xyzw;
  r0.xyzw = r1.xyzw * r0.zzzz;
  r1.xy = _ClipRect.zw + -_ClipRect.xy;
  r1.xy = -abs(v4.xy) + r1.xy;
  r1.xy = saturate(v4.zw * r1.xy);
  r1.x = r1.x * r1.y;
  r2.xyzw = r1.xxxx * r0.xyzw;
  r0.x = r0.w * r1.x + -0.00100000005;
  r0.x = cmp(r0.x < 0);
  if (r0.x != 0) discard;
  r0.x = dot(v7.xyz, v7.xyz);
  r0.x = max(1.17549435e-038, r0.x);
  r0.x = rsqrt(r0.x);
  r0.xyz = v7.xyz * r0.xxx;
  r0.w = sqrt(unity_LightData.z);
  r0.w = min(1, r0.w);
  r1.xyz = _MainLightColor.xyz * r0.www;
  r0.w = dot(r0.xyz, _MainLightPosition.xyz);
  r0.w = saturate(r0.w * 0.5 + 0.5);
  r1.w = min(unity_LightData.y, _AdditionalLightsCount.x);
  r1.w = (int)r1.w;
  r3.xyzw = float4(0,0,0,0);
  while (true) {
    r4.x = cmp((uint)r3.w >= (uint)r1.w);
    if (r4.x != 0) break;
    r4.x = (uint)r3.w >> 2;
    r4.y = (int)r3.w & 3;
    r4.x = dot(unity_LightIndices[(int)(r4.x)].xyzw, icb[r4.y+0].xyzw);
    r4.x = (int)r4.x;
    r4.yzw = -v8.xyz * _AdditionalLightsPosition[(int)(r4.x)].www + _AdditionalLightsPosition[(int)(r4.x)].xyz;
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
    r4.x = dot(r0.xyz, r4.yzw);
    r4.x = saturate(r4.x * 0.5 + 0.5);
    r3.xyz = r5.xyz * r4.xxx + r3.xyz;
    r3.w = (int)r3.w + 1;
  }
  r0.xyz = r1.xyz * r0.www + r3.xyz;
  r2.xyz = r2.xyz * r0.xyz;
  o0.xyzw = v1.wwww * r2.xyzw;
  return;
}
