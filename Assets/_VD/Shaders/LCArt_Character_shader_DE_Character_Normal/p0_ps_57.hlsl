// LCArt/Character/shader_DE_Character_Normal | pass 0 "ForwardLit" | progFragment | keywords: FOG_LINEAR _CLUSTER_LIGHT_LOOP _DE_EDITOR_CAMERA _DE_PRERENDERING_CAMERA | blob 57
void frag(
  float4 v0 : SV_POSITION0,
  float2 v1 : TEXCOORD0,
  float2 w1 : TEXCOORD2,
  float3 v2 : TEXCOORD1,
  float w2 : TEXCOORD5,
  float3 v3 : TEXCOORD3,
  float w3 : TEXCOORD9,
  float4 v4 : TEXCOORD4,
  float4 v5 : TEXCOORD6,
  float4 v6 : TEXCOORD7,
  float4 v7 : TEXCOORD8,
  float4 v8 : COLOR0,
  float3 v9 : TEXCOORD10,
  out float4 o0 : SV_Target0)
{
  float4 r0,r1,r2,r3,r4,r5,r6,r7,r8,r9,r10,r11;
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
  r0.xyzw = _MainTex.SampleBias(sampler_MainTex, v1.xy, _GlobalMipBias.x).xyzw;
  r1.x = -0.100000001 + r0.w;
  r1.x = cmp(r1.x < 0);
  if (r1.x != 0) discard;
  r1.xyz = v8.xyz * r0.xyz;
  r1.w = saturate(_IsVertexColorAlphaUsed);
  r2.x = -1 + v8.w;
  r1.w = r1.w * r2.x + 1;
  o0.w = r1.w * r0.w;
  r2.xy = float2(1,1) / _ScaledScreenParams.xy;
  r2.xy = v0.xy * r2.xy;
  r1.w = r2.y * _ScaleBiasRt.x + _ScaleBiasRt.y;
  r2.z = 1 + -r1.w;
  r1.w = (VD_COL(unity_MatrixV, 1).z) * v4.y;
  r1.w = (VD_COL(unity_MatrixV, 0).z) * v4.x + r1.w;
  r1.w = (VD_COL(unity_MatrixV, 2).z) * v4.z + r1.w;
  r1.w = (VD_COL(unity_MatrixV, 3).z) + r1.w;
  r1.w = -_ProjectionParams.y + -r1.w;
  r1.w = max(0, r1.w);
  r1.w = saturate(r1.w * unity_FogParams.z + unity_FogParams.w);
  r2.y = vd_asint(_MainLightLayerMask) & vd_asint(unity_RenderingLayer.x);
  r2.w = saturate(_MainLightPosition.y * 0.5 + 0.5);
  r3.xyz = -_GlobalCharacterShadowColor.xyz + _MainLightColor.xyz;
  r3.xyz = r3.xyz * r2.www + _GlobalCharacterShadowColor.xyz;
  r3.xyz = r3.xyz * r1.xyz;
  r3.xyz = r2.yyy ? r3.xyz : float3(0,0,0);
  r2.yw = -_PlaneBoundMaxPos.xz + v4.xz;
  r4.xy = -_PlaneBoundMaxPos.xz + _PlaneBoundMinPos.xz;
  r2.yw = saturate(r2.yw / r4.xy);
  r5.xyzw = _PlaneSightRenderTarget.SampleLevel(sampler_PlaneSightRenderTarget, r2.yw, 0).xyzw;
  r2.y = _GlobalPlaneSystemEnable * r5.w;
  r4.zw = (VD_COL(unity_ObjectToWorld, 3).xz) + -_PlaneBoundMaxPos.xz;
  r4.xy = saturate(r4.zw / r4.xy);
  r4.xyzw = _PlanePointBuffer.SampleLevel(sampler_PlanePointBuffer, r4.xy, 0).xyzw;
  r2.w = saturate(4 * r4.w);
  r3.w = r2.w * -2 + 3;
  r2.w = r2.w * r2.w;
  r2.w = r3.w * r2.w;
  r2.w = _GlobalPlanePointBufferEnable * r2.w;
  r2.y = max(r2.y, r2.w);
  r4.xyz = r2.yyy * r1.xyz;
  r4.xyz = float3(0.25,0.25,0.25) * r4.xyz;
  r2.xy = _FPParams1.xy * r2.xz;
  r2.xy = (uint2)r2.xy;
  r2.zw = (uint2)_FPParams1.zw;
  r2.x = mad((int)r2.y, (int)r2.z, (int)r2.x);
  r2.x = (int)r2.w * (int)r2.x;
  r5.x = (VD_COL(unity_MatrixV, 0).z);
  r5.y = (VD_COL(unity_MatrixV, 1).z);
  r5.z = (VD_COL(unity_MatrixV, 2).z);
  r6.xyz = -_WorldSpaceCameraPos.xyz + v4.xyz;
  r2.y = dot(-r5.xyz, r6.xyz);
  r2.z = cmp(unity_OrthoParams.w == 0.000000);
  r3.w = log2(r2.y);
  r2.y = r2.z ? r3.w : r2.y;
  r2.y = r2.y * _FPParams0.x + _FPParams0.y;
  r2.y = (uint)r2.y;
  r2.z = (uint)_FPParams2.x;
  r2.z = (int)r2.z + -1;
  r2.w = (int)r2.w + 2;
  r2.y = min((uint)r2.y, (uint)r2.z);
  r2.z = (int)r2.w * (int)r2.y;
  r2.y = mad((int)r2.y, (int)r2.w, 2);
  r2.w = (uint)r2.z >> 2;
  r5.x = (uint)r2.z << 31;
  r5.y = (uint)r2.z << 30;
  r5.xy = (uint2)r5.xy >> 31;
  r5.zw = (int2)r5.xx & vd_asint(urp_ZBins[(int)(r2.w)].wy);
  r6.xy = ~(int2)r5.xy;
  r2.zw = (int2)r6.xx & vd_asint(urp_ZBins[(int)(r2.w)].zx);
  r2.zw = (int2)r2.zw | (int2)r5.zw;
  r2.z = (int)r5.y & (int)r2.z;
  r2.w = (int)r6.y & (int)r2.w;
  r2.z = (int)r2.w | (int)r2.z;
  r2.w = (uint)_FPParams0.w;
  r5.z = 0;
  r5.xyw = r2.xyz;
  r6.xyz = r4.xyz;
  while (true) {
    r3.w = (uint)r5.w >> 16;
    r4.w = r5.z;
    r7.w = r5.w;
    while (true) {
      r6.w = cmp((int)r4.w == 0);
      r8.x = (int)r7.w & 0x0000ffff;
      r8.x = cmp((uint)r3.w >= (uint)r8.x);
      r6.w = r6.w ? r8.x : 0;
      if (r6.w == 0) break;
      r6.w = (uint)r7.w >> 5;
      r6.w = (int)r6.w & 2047;
      r8.xyzw = (int4)r5.xxyy + (int4)r6.wwww;
      r9.xy = (uint2)r8.yw >> 2;
      r10.xz = (uint2)r8.xz << 31;
      r10.yw = (uint2)r8.yw << 30;
      r8.xyzw = (uint4)r10.xyzw >> 31;
      r9.zw = (int2)r8.xx & vd_asint(urp_Tiles[(int)(r9.x)].wy);
      r10.xyzw = ~(int4)r8.xyzw;
      r11.xy = (int2)r10.xx & vd_asint(urp_Tiles[(int)(r9.x)].zx);
      r9.xz = (int2)r9.zw | (int2)r11.xy;
      r8.x = (int)r8.y & (int)r9.x;
      r8.y = (int)r10.y & (int)r9.z;
      r8.x = (int)r8.y | (int)r8.x;
      r8.yz = (int2)r8.zz & vd_asint(urp_ZBins[(int)(r9.y)].wy);
      r9.xy = (int2)r10.zz & vd_asint(urp_ZBins[(int)(r9.y)].zx);
      r8.yz = (int2)r8.yz | (int2)r9.xy;
      r8.y = (int)r8.w & (int)r8.y;
      r8.z = (int)r10.w & (int)r8.z;
      r8.y = (int)r8.z | (int)r8.y;
      r8.x = (int)r8.y & (int)r8.x;
      r8.y = -1 << (int)r7.w;
      r8.x = (int)r8.y & (int)r8.x;
      r6.w = (uint)r6.w << 5;
      r6.w = (int)r3.w + (int)-r6.w;
      r6.w = min(31, (uint)r6.w);
      r6.w = (int)-r6.w + 31;
      r6.w = -1 >> (uint)r6.w;
      r4.w = (int)r6.w & (int)r8.x;
      r6.w = (int)r7.w + 32;
      r7.w = (int)r6.w & -32;
    }
    if (r4.w == 0) break;
    r3.w = -(int)r4.w;
    r3.w = (int)r3.w & (int)r4.w;
    r3.w = (uint)r3.w;
    r3.w = (uint)r3.w >> 23;
    r3.w = (int)r3.w + -127;
    r6.w = 1 << (int)r3.w;
    r7.z = (int)r4.w ^ (int)r6.w;
    r6.w = (int)r7.w + -32;
    r6.w = (int)r6.w & 0x0000ffe0;
    r3.w = (int)r3.w + (int)r6.w;
    r3.w = (int)r2.w + (int)r3.w;
    r6.w = vd_asint(unity_RenderingLayer.x) & vd_asint(_AdditionalLightsLayerMasks[(int)(r3.w)]);
    if (r6.w == 0) {
      r7.xy = r5.xy;
      r5.xyzw = r7.xyzw;
      continue;
    }
    r8.xyz = -v4.xyz * _AdditionalLightsPosition[(int)(r3.w)].www + _AdditionalLightsPosition[(int)(r3.w)].xyz;
    r6.w = dot(r8.xyz, r8.xyz);
    r6.w = max(6.10351563e-005, r6.w);
    r7.x = rsqrt(r6.w);
    r8.xyz = r8.xyz * r7.xxx;
    r7.x = 1 / r6.w;
    r6.w = _AdditionalLightsAttenuation[(int)(r3.w)].x * r6.w;
    r6.w = -r6.w * r6.w + 1;
    r6.w = max(0, r6.w);
    r6.w = r6.w * r6.w;
    r6.w = r7.x * r6.w;
    r7.x = dot(_AdditionalLightsSpotDir[(int)(r3.w)].xyz, r8.xyz);
    r7.x = saturate(r7.x * _AdditionalLightsAttenuation[(int)(r3.w)].z + _AdditionalLightsAttenuation[(int)(r3.w)].w);
    r7.x = r7.x * r7.x;
    r6.w = r7.x * r6.w;
    r8.xzw = _AdditionalLightsColor[(int)(r3.w)].xyz * r6.www;
    r3.w = saturate(r8.y * 0.5 + 0.5);
    r8.xyz = r8.xzw * r3.www;
    r6.xyz = r8.xyz * r1.xyz + r6.xyz;
    r5.z = r7.z;
    r5.w = r7.w;
  }
  r1.xyz = r6.xyz + r3.xyz;
  r2.x = cmp(0.5 < _UseDarkColorMask);
  if (r2.x != 0) {
    r0.xyz = -r0.xyz * v8.www + r0.www;
    r0.xyz = r0.xyz * v9.xyz + r1.xyz;
  } else {
    r0.xyz = v9.xyz + r1.xyz;
  }
  r2.xyzw = _MaskTex.SampleBias(sampler_vd_point_repeat, v1.xy, _GlobalMipBias.x).xyzw;
  r3.xyzw = _MaskColor1.xyzw + -_MaskColor0.xyzw;
  r2.xyzw = r2.xxxx * r3.xyzw + _MaskColor0.xyzw;
  r0.w = _MaskIntensity * r2.w;
  r1.xyz = r2.xyz + -r0.xyz;
  r0.xyz = r0.www * r1.xyz + r0.xyz;
  r1.xy = v7.xy / v7.ww;
  r0.w = -1 + w2.x;
  r0.w = _SightEffectEnable * r0.w + _Effect;
  r0.w = 1 + r0.w;
  r2.xy = _ScreenSize.xy / _ScreenParams.xy;
  r1.xy = _ScreenParams.xy * r1.xy;
  r1.xy = float2(0.5,0.5) * r1.xy;
  r1.xy = r1.xy * r2.xy;
  r1.xy = floor(r1.xy);
  r1.xy = (uint2)r1.xy;
  r1.xy = (int2)r1.xy & int2(3,3);
  r1.y = (uint)r1.y << 2;
  r1.x = (int)r1.y + (int)r1.x;
  r1.y = x0[r1.x+0].x;
  r0.w = -r1.y + r0.w;
  r0.w = cmp(r0.w < 0);
  if (r0.w != 0) discard;
  r0.w = cmp(_GlobalBoxAreaEnable >= 0.5);
  if (r0.w != 0) {
    r0.w = 0;
    while (true) {
      r1.y = cmp((int)r0.w >= 8);
      if (r1.y != 0) break;
      r1.y = cmp(_GlobalBoxAreaCoverCharacters[(int)(r0.w)] < 0.5);
      if (r1.y != 0) {
        r1.y = (int)r0.w + 1;
        r0.w = r1.y;
        continue;
      }
      r2.xyz = -_GlobalBoxAreaCenters[(int)(r0.w)].xyz + (VD_COL(unity_ObjectToWorld, 3).xyz);
      r2.xyz = -_GlobalBoxAreaExtents[(int)(r0.w)].xyz + abs(r2.xyz);
      r1.y = max(r2.x, r2.y);
      r1.y = max(r1.y, r2.z);
      r1.y = cmp(0 >= r1.y);
      if (r1.y != 0) {
        if (-1 != 0) discard;
        break;
      }
      r0.w = (int)r0.w + 1;
    }
  }
  r0.w = _Effect * _EffectColor.w;
  r2.xyz = _EffectColor.xyz + -r0.xyz;
  r0.xyz = r0.www * r2.xyz + r0.xyz;
  r2.xyz = float3(0.5,0.5,0.5) + -r0.xyz;
  r0.xyz = (_LightEffectIntensity).xxx * r2.xyz + r0.xyz;
  r0.w = saturate(_DissolveProgress + _DissolveProgress);
  r2.xyz = _DissolveColor.xyz + -r0.xyz;
  r0.xyz = r0.www * r2.xyz + r0.xyz;
  r0.w = saturate(-0.5 + _DissolveProgress);
  r0.w = -r0.w * 2 + 1;
  r1.x = x0[r1.x+0].x;
  r0.w = -r1.x + r0.w;
  r0.w = cmp(r0.w < 0);
  if (r0.w != 0) discard;
  if (_GlobalWaterAreaEnable != 0) {
    r1.xy = _GlobalOverallMapBoundMax.xz + -_GlobalOverallMapBoundMin.xz;
    r1.xy = max(float2(1,1), r1.xy);
    r2.xy = -_GlobalOverallMapBoundMin.xz + v4.xz;
    r1.xy = r2.xy / r1.xy;
    r2.xyzw = _GlobalWaterAreaMap.SampleBias(sampler_vd_linear_clamp, r1.xy, _GlobalMipBias.x).xyzw;
    r0.w = cmp(r2.z >= 0.449999988);
    if (r0.w != 0) {
      r0.w = _GlobalWaterShoreEdge.y + -_GlobalWaterShoreEdge.x;
      r1.x = -_GlobalWaterShoreEdge.x + r2.z;
      r0.w = 1 / r0.w;
      r0.w = saturate(r1.x * r0.w);
      r1.x = r0.w * -2 + 3;
      r0.w = r0.w * r0.w;
      r0.w = r1.x * r0.w;
      r1.x = _GlobalWaterDepthRange.y + -_GlobalWaterDepthRange.x;
      r1.y = -_GlobalWaterDepthRange.x + r2.z;
      r1.x = 1 / r1.x;
      r1.x = saturate(r1.y * r1.x);
      r1.y = r1.x * -2 + 3;
      r1.x = r1.x * r1.x;
      r1.x = r1.y * r1.x;
      r1.yz = cmp(float2(0.00999999978,0.00999999978) < _GlobalWaterSurfaceWaveMapParams.zw);
      r1.y = r1.z ? r1.y : 0;
      if (r1.y != 0) {
        r1.yz = -_GlobalWaterSurfaceWaveMapParams.xy + v4.xz;
        r1.yz = r1.yz / _GlobalWaterSurfaceWaveMapParams.zw;
        r1.yz = float2(0.5,0.5) + r1.yz;
        r2.xy = float2(1,1) + -r1.yz;
        r2.xy = min(r2.xy, r1.yz);
        r2.x = min(r2.x, r2.y);
        r2.x = saturate(_GlobalWaterSurfaceWaveMapParams.z * r2.x);
        r3.xyzw = _GlobalWaterSurfaceWaveMap.SampleBias(sampler_vd_linear_clamp, r1.yz, _GlobalMipBias.x).xyzw;
        r1.y = r3.x * r2.x;
      } else {
        r1.y = 0;
      }
      r1.z = dot((VD_COL(unity_ObjectToWorld, 1).xyz), (VD_COL(unity_ObjectToWorld, 1).xyz));
      r1.z = sqrt(r1.z);
      r1.z = w3.x * r1.z + (VD_COL(unity_ObjectToWorld, 3).y);
      r1.y = 0.125 * r1.y;
      r1.y = _GlobalWaterSurfaceLevel * r1.x + r1.y;
      r1.y = r1.z + -r1.y;
      r1.z = cmp(0.00999999978 >= r1.y);
      r2.x = saturate(-3 * r1.y);
      r1.x = r2.x * r1.x;
      r2.y = dot(r0.xyz, float3(0.298999995,0.587000012,0.114));
      r2.y = saturate(r2.y * 2 + 0.300000012);
      r3.xyzw = -_GlobalWaterShadowColor.xyzw + _GlobalWaterDeepColor.xyzw;
      r3.xyzw = r2.yyyy * r3.xyzw + _GlobalWaterShadowColor.xyzw;
      r4.xyzw = -_GlobalWaterShallowShadowColor.xyzw + _GlobalWaterShallowColor.xyzw;
      r4.xyzw = r2.yyyy * r4.xyzw + _GlobalWaterShallowShadowColor.xyzw;
      r3.xyzw = -r4.xyzw + r3.xyzw;
      r3.xyzw = r1.xxxx * r3.xyzw + r4.xyzw;
      r2.yzw = r3.xyz + -r0.xyz;
      r2.yzw = r3.www * r2.yzw + r0.xyz;
      r1.x = -r1.y * r1.y;
      r1.x = 721.347534 * r1.x;
      r1.x = exp2(r1.x);
      r1.x = 0.300000012 * r1.x;
      r3.xyz = r4.xyz + -r2.yzw;
      r2.yzw = r1.xxx * r3.xyz + r2.yzw;
      r0.w = r2.x * r0.w;
      r2.xyz = r2.yzw + -r0.xyz;
      r2.xyz = r0.www * r2.xyz + r0.xyz;
      r0.xyz = r1.zzz ? r2.xyz : r0.xyz;
    }
  }
  r0.w = 1 + -r1.w;
  r1.x = -_GlobalFogHeightMin + v3.y;
  r1.y = _GlobalFogHeightMax + -_GlobalFogHeightMin;
  r1.x = r1.x / r1.y;
  r1.x = saturate(1 + -r1.x);
  r1.x = _GlobalFogHeightEnable * r1.x;
  r0.w = max(r1.x, r0.w);
  r1.xyz = unity_FogColor.xyz + -r0.xyz;
  o0.xyz = r0.www * r1.xyz + r0.xyz;
  return;
}
