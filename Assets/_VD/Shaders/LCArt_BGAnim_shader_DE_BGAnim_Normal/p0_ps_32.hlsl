// LCArt/BGAnim/shader_DE_BGAnim_Normal | pass 0 "ForwardLit" | progFragment | keywords: FOG_LINEAR _ADDITIONAL_LIGHTS | blob 32
void frag(
  float4 v0 : SV_POSITION0,
  float2 v1 : TEXCOORD0,
  float2 w1 : TEXCOORD2,
  float3 v2 : NORMAL0,
  float w2 : TEXCOORD9,
  float4 v3 : TEXCOORD3,
  float4 v4 : TEXCOORD4,
  float4 v5 : TEXCOORD6,
  float4 v6 : TEXCOORD7,
  float4 v7 : TEXCOORD8,
  float4 v8 : COLOR0,
  out float4 o0 : SV_Target0)
{
  const float4 icb[] = { { 1.000000, 0, 0, 0},
                              { 0, 1.000000, 0, 0},
                              { 0, 0, 1.000000, 0},
                              { 0, 0, 0, 1.000000} };
  float4 r0,r1,r2,r3,r4,r5,r6;
  uint4 bitmask, uiDest;
  float4 fDest;

  float4 x0[16];
  x0[0].x = 0.058823999;
  x0[1].x = 0.529411972;
  x0[2].x = 0.176470995;
  x0[3].x = 0.647059023;
  x0[4].x = 0.764706016;
  x0[5].x = 0.294117987;
  x0[6].x = 0.882353008;
  x0[7].x = 0.411765009;
  x0[8].x = 0.235293999;
  x0[9].x = 0.705882013;
  x0[10].x = 0.117647;
  x0[11].x = 0.588235021;
  x0[12].x = 0.941175997;
  x0[13].x = 0.470587999;
  x0[14].x = 0.823529005;
  x0[15].x = 0.352941006;
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
  r1.xyz = v8.xyz * r0.xyz;
  r1.w = saturate(_IsVertexColorAlphaUsed);
  r2.x = -1 + v8.w;
  r1.w = r1.w * r2.x + 1;
  r0.w = r1.w * r0.w;
  r1.w = (VD_COL(unity_MatrixV, 1).z) * v4.y;
  r1.w = (VD_COL(unity_MatrixV, 0).z) * v4.x + r1.w;
  r1.w = (VD_COL(unity_MatrixV, 2).z) * v4.z + r1.w;
  r1.w = (VD_COL(unity_MatrixV, 3).z) + r1.w;
  r1.w = -_ProjectionParams.y + -r1.w;
  r1.w = max(0, r1.w);
  r1.w = saturate(r1.w * unity_FogParams.z + unity_FogParams.w);
  r2.xy = v7.xy / v7.ww;
  r2.z = sqrt(unity_LightData.z);
  r2.z = min(1, r2.z);
  r2.w = dot(v2.xyz, _MainLightPosition.xyz);
  r2.w = saturate(r2.w * 0.5 + 0.5);
  r3.xyz = _MainLightColor.xyz * r2.zzz + -_GlobalBackgroundShadowColor.xyz;
  r3.xyz = r2.www * r3.xyz + _GlobalBackgroundShadowColor.xyz;
  r2.z = min(unity_LightData.y, _AdditionalLightsCount.x);
  r2.z = (int)r2.z;
  r4.xy = -_PlaneBoundMaxPos.xz + v4.xz;
  r4.zw = -_PlaneBoundMaxPos.xz + _PlaneBoundMinPos.xz;
  r4.xy = saturate(r4.xy / r4.zw);
  r4.xyzw = _PlaneSightRenderTarget.SampleLevel(sampler_PlaneSightRenderTarget, r4.xy, 0).xyzw;
  r2.w = _GlobalPlaneSystemEnable * r4.w + -1;
  r4.xyz = float3(0,0,0);
  r3.w = 0;
  while (true) {
    r4.w = cmp((uint)r3.w >= (uint)r2.z);
    if (r4.w != 0) break;
    r4.w = (uint)r3.w >> 2;
    r5.x = (int)r3.w & 3;
    r4.w = dot(unity_LightIndices[(int)(r4.w)].xyzw, icb[r5.x+0].xyzw);
    r4.w = (int)r4.w;
    r5.xyz = -v4.xyz * _AdditionalLightsPosition[(int)(r4.w)].www + _AdditionalLightsPosition[(int)(r4.w)].xyz;
    r5.w = dot(r5.xyz, r5.xyz);
    r5.w = max(6.10351563e-005, r5.w);
    r6.x = rsqrt(r5.w);
    r5.xyz = r6.xxx * r5.xyz;
    r6.x = 1 / r5.w;
    r5.w = _AdditionalLightsAttenuation[(int)(r4.w)].x * r5.w;
    r5.w = -r5.w * r5.w + 1;
    r5.w = max(0, r5.w);
    r5.w = r5.w * r5.w;
    r5.w = r6.x * r5.w;
    r6.x = dot(_AdditionalLightsSpotDir[(int)(r4.w)].xyz, r5.xyz);
    r6.x = saturate(r6.x * _AdditionalLightsAttenuation[(int)(r4.w)].z + _AdditionalLightsAttenuation[(int)(r4.w)].w);
    r6.x = r6.x * r6.x;
    r5.w = r6.x * r5.w;
    r6.x = 2 & vd_asint(_AdditionalLightsLayerMasks[(int)(r4.w)]);
    r6.x = r6.x ? 1 : 0;
    r6.x = r6.x * r2.w + 1;
    r5.w = sqrt(r5.w);
    r5.w = min(1, r5.w);
    r6.yzw = _AdditionalLightsColor[(int)(r4.w)].xyz * r5.www;
    r4.w = dot(v2.xyz, r5.xyz);
    r4.w = saturate(r4.w * 0.5 + 0.5);
    r5.xyz = r6.yzw * r4.www;
    r5.xyz = r5.xyz * r1.xyz;
    r4.xyz = r5.xyz * r6.xxx + r4.xyz;
    r3.w = (int)r3.w + 1;
  }
  r1.xyz = r3.xyz * r1.xyz + r4.xyz;
  r3.xyzw = _EmissionTex.SampleBias(sampler_EmissionTex, v1.xy, _GlobalMipBias.x).xyzw;
  r0.xyz = r3.xxx * r0.xyz;
  r0.xyz = r0.xyz * (_EmissionIntensity).xxx + r1.xyz;
  r1.x = 1 + -r1.w;
  r1.y = -_GlobalFogHeightMin + v3.y;
  r1.z = -_GlobalFogHeightMin + _GlobalFogHeightMax;
  r1.y = r1.y / r1.z;
  r1.y = saturate(1 + -r1.y);
  r1.y = _GlobalFogHeightEnable * r1.y;
  r1.x = max(r1.x, r1.y);
  r1.yzw = unity_FogColor.xyz + -r0.xyz;
  o0.xyz = r1.xxx * r1.yzw + r0.xyz;
  r1.xyzw = _CustomDepthBuffer.SampleBias(sampler_CustomDepthBuffer, r2.xy, _GlobalMipBias.x).xyzw;
  r0.x = v0.z + -r1.z;
  r0.x = -7.50000036e-005 + r0.x;
  r0.x = saturate(-6666.6665 * r0.x);
  r0.y = r0.x * -2 + 3;
  r0.x = r0.x * r0.x;
  r0.zw = r0.yw * r0.xw;
  r1.xy = float2(1,1) + -float2(_GlobalSeeThroughCircleRange, _GlobalSeeThroughCircleAlpha);
  r1.z = 1 + -r1.x;
  r1.x = r1.w + -r1.x;
  r1.z = 1 / r1.z;
  r1.x = saturate(r1.x * r1.z);
  r1.z = r1.x * -2 + 3;
  r1.x = r1.x * r1.x;
  r1.x = r1.z * r1.x;
  r0.x = -r0.y * r0.x + 1;
  r0.x = r1.x * r0.x + r0.z;
  r0.x = -1 + r0.x;
  r0.x = _GlobalBackgroundSeeThroughEnable * r0.x + 1;
  r0.x = r0.x * r1.y + _GlobalSeeThroughCircleAlpha;
  r0.y = _CharacterAreaMaskOffEnable * _GlobalPlaneSystemEnable;
  r0.y = _GlobalBackgroundSeeThroughEnable * r0.y;
  r0.x = -1 + r0.x;
  r0.x = r0.y * r0.x + 1;
  r0.x = min(w2.x, r0.x);
  r0.yz = _ScreenSize.xy / _ScreenParams.xy;
  r1.xy = _ScreenParams.xy * r2.xy;
  r0.yz = r1.xy * r0.yz;
  r0.yz = float2(0.5,0.5) * r0.yz;
  r0.yz = floor(r0.yz);
  r0.yz = (uint2)r0.yz;
  r0.yz = (int2)r0.yz & int2(3,3);
  r0.z = (uint)r0.z << 2;
  r0.y = (int)r0.z + (int)r0.y;
  r0.y = x0[r0.y+0].x;
  r0.x = r0.w * r0.x + -r0.y;
  r0.x = cmp(r0.x < 0);
  if (r0.x != 0) discard;
  o0.w = r0.w;
  return;
}
