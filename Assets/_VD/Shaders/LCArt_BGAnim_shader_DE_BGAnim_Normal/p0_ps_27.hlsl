// LCArt/BGAnim/shader_DE_BGAnim_Normal | pass 0 "ForwardLit" | progFragment | keywords: _CLUSTER_LIGHT_LOOP | blob 27
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
  float4 r0,r1,r2,r3,r4,r5,r6,r7,r8,r9,r10;
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
  r2.xy = float2(1,1) / _ScaledScreenParams.xy;
  r2.xy = v0.xy * r2.xy;
  r1.w = r2.y * _ScaleBiasRt.x + _ScaleBiasRt.y;
  r2.z = 1 + -r1.w;
  r2.yw = v7.xy / v7.ww;
  r1.w = dot(v2.xyz, _MainLightPosition.xyz);
  r1.w = saturate(r1.w * 0.5 + 0.5);
  r3.xyz = -_GlobalBackgroundShadowColor.xyz + _MainLightColor.xyz;
  r3.xyz = r1.www * r3.xyz + _GlobalBackgroundShadowColor.xyz;
  r2.xz = _FPParams1.xy * r2.xz;
  r2.xz = (uint2)r2.xz;
  r4.xy = (uint2)_FPParams1.zw;
  r1.w = mad((int)r2.z, (int)r4.x, (int)r2.x);
  r1.w = (int)r4.y * (int)r1.w;
  r5.x = -(VD_COL(unity_MatrixV, 0).z);
  r5.y = -(VD_COL(unity_MatrixV, 1).z);
  r5.z = -(VD_COL(unity_MatrixV, 2).z);
  r4.xzw = -_WorldSpaceCameraPos.xyz + v4.xyz;
  r2.x = dot(r5.xyz, r4.xzw);
  r2.z = cmp(unity_OrthoParams.w == 0.000000);
  r3.w = log2(r2.x);
  r2.x = r2.z ? r3.w : r2.x;
  r2.x = r2.x * _FPParams0.x + _FPParams0.y;
  r2.x = (uint)r2.x;
  r2.z = (uint)_FPParams2.x;
  r2.z = (int)r2.z + -1;
  r3.w = (int)r4.y + 2;
  r2.x = min((uint)r2.x, (uint)r2.z);
  r2.z = (int)r3.w * (int)r2.x;
  r2.x = mad((int)r2.x, (int)r3.w, 2);
  r3.w = (uint)r2.z >> 2;
  r4.x = (uint)r2.z << 31;
  r4.y = (uint)r2.z << 30;
  r4.xy = (uint2)r4.xy >> 31;
  r4.zw = (int2)r4.xx & vd_asint(urp_ZBins[(int)(r3.w)].wy);
  r5.xy = ~(int2)r4.xy;
  r5.xz = (int2)r5.xx & vd_asint(urp_ZBins[(int)(r3.w)].zx);
  r4.xz = (int2)r4.zw | (int2)r5.xz;
  r2.z = (int)r4.y & (int)r4.x;
  r3.w = (int)r5.y & (int)r4.z;
  r2.z = (int)r2.z | (int)r3.w;
  r3.w = (uint)_FPParams0.w;
  r4.xy = -_PlaneBoundMaxPos.xz + v4.xz;
  r4.zw = -_PlaneBoundMaxPos.xz + _PlaneBoundMinPos.xz;
  r4.xy = saturate(r4.xy / r4.zw);
  r4.xyzw = _PlaneSightRenderTarget.SampleLevel(sampler_PlaneSightRenderTarget, r4.xy, 0).xyzw;
  r4.x = _GlobalPlaneSystemEnable * r4.w + -1;
  r5.x = r1.w;
  r5.z = 0;
  r5.yw = r2.xz;
  r4.yzw = float3(0,0,0);
  while (true) {
    r6.x = (uint)r5.w >> 16;
    r6.yz = r5.zw;
    while (true) {
      r6.w = cmp((int)r6.y == 0);
      r7.x = (int)r6.z & 0x0000ffff;
      r7.x = cmp((uint)r6.x >= (uint)r7.x);
      r6.w = r6.w ? r7.x : 0;
      if (r6.w == 0) break;
      r6.w = (uint)r6.z >> 5;
      r6.w = (int)r6.w & 2047;
      r7.xyzw = (int4)r5.xxyy + (int4)r6.wwww;
      r8.xy = (uint2)r7.yw >> 2;
      r9.xz = (uint2)r7.xz << 31;
      r9.yw = (uint2)r7.yw << 30;
      r7.xyzw = (uint4)r9.xyzw >> 31;
      r8.zw = (int2)r7.xx & vd_asint(urp_Tiles[(int)(r8.x)].wy);
      r9.xyzw = ~(int4)r7.xyzw;
      r10.xy = (int2)r9.xx & vd_asint(urp_Tiles[(int)(r8.x)].zx);
      r8.xz = (int2)r8.zw | (int2)r10.xy;
      r7.x = (int)r7.y & (int)r8.x;
      r7.y = (int)r9.y & (int)r8.z;
      r7.x = (int)r7.y | (int)r7.x;
      r7.yz = (int2)r7.zz & vd_asint(urp_ZBins[(int)(r8.y)].wy);
      r8.xy = (int2)r9.zz & vd_asint(urp_ZBins[(int)(r8.y)].zx);
      r7.yz = (int2)r7.yz | (int2)r8.xy;
      r7.y = (int)r7.w & (int)r7.y;
      r7.z = (int)r9.w & (int)r7.z;
      r7.y = (int)r7.z | (int)r7.y;
      r7.x = (int)r7.y & (int)r7.x;
      r7.y = -1 << (int)r6.z;
      r7.x = (int)r7.y & (int)r7.x;
      r6.w = (uint)r6.w << 5;
      r6.w = (int)-r6.w + (int)r6.x;
      r6.w = min(31, (uint)r6.w);
      r6.w = (int)-r6.w + 31;
      r6.w = -1 >> (uint)r6.w;
      r6.y = (int)r6.w & (int)r7.x;
      r6.w = (int)r6.z + 32;
      r6.z = (int)r6.w & -32;
    }
    r5.w = r6.z;
    if (r6.y == 0) break;
    r6.x = -(int)r6.y;
    r6.x = (int)r6.x & (int)r6.y;
    r6.x = (uint)r6.x;
    r6.x = (uint)r6.x >> 23;
    r6.x = (int)r6.x + -127;
    r6.w = 1 << (int)r6.x;
    r5.z = (int)r6.w ^ (int)r6.y;
    r6.w = (int)r5.w + -32;
    r6.w = (int)r6.w & 0x0000ffe0;
    r6.x = (int)r6.x + (int)r6.w;
    r6.x = (int)r3.w + (int)r6.x;
    r7.xyz = -v4.xyz * _AdditionalLightsPosition[(int)(r6.x)].www + _AdditionalLightsPosition[(int)(r6.x)].xyz;
    r6.w = dot(r7.xyz, r7.xyz);
    r6.w = max(6.10351563e-005, r6.w);
    r7.w = rsqrt(r6.w);
    r7.xyz = r7.xyz * r7.www;
    r7.w = 1 / r6.w;
    r6.w = _AdditionalLightsAttenuation[(int)(r6.x)].x * r6.w;
    r6.w = -r6.w * r6.w + 1;
    r6.w = max(0, r6.w);
    r6.w = r6.w * r6.w;
    r6.w = r7.w * r6.w;
    r7.w = dot(_AdditionalLightsSpotDir[(int)(r6.x)].xyz, r7.xyz);
    r7.w = saturate(r7.w * _AdditionalLightsAttenuation[(int)(r6.x)].z + _AdditionalLightsAttenuation[(int)(r6.x)].w);
    r7.w = r7.w * r7.w;
    r6.w = r7.w * r6.w;
    r7.w = 2 & vd_asint(_AdditionalLightsLayerMasks[(int)(r6.x)]);
    r7.w = r7.w ? 1 : 0;
    r7.w = r7.w * r4.x + 1;
    r6.w = sqrt(r6.w);
    r6.w = min(1, r6.w);
    r8.xyz = _AdditionalLightsColor[(int)(r6.x)].xyz * r6.www;
    r6.x = dot(v2.xyz, r7.xyz);
    r6.x = saturate(r6.x * 0.5 + 0.5);
    r7.xyz = r8.xyz * r6.xxx;
    r7.xyz = r7.xyz * r1.xyz;
    r4.yzw = r7.xyz * r7.www + r4.yzw;
  }
  r1.xyz = r3.xyz * r1.xyz + r4.yzw;
  r3.xyzw = _EmissionTex.SampleBias(sampler_EmissionTex, v1.xy, _GlobalMipBias.x).xyzw;
  r0.xyz = r3.xxx * r0.xyz;
  r0.xyz = r0.xyz * (_EmissionIntensity).xxx + r1.xyz;
  r1.x = -_GlobalFogHeightMin + v3.y;
  r1.y = -_GlobalFogHeightMin + _GlobalFogHeightMax;
  r1.x = r1.x / r1.y;
  r1.x = saturate(1 + -r1.x);
  r1.x = _GlobalFogHeightEnable * r1.x;
  r1.x = max(0, r1.x);
  r1.yzw = unity_FogColor.xyz + -r0.xyz;
  o0.xyz = r1.xxx * r1.yzw + r0.xyz;
  r1.xyzw = _CustomDepthBuffer.SampleBias(sampler_CustomDepthBuffer, r2.yw, _GlobalMipBias.x).xyzw;
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
  r1.xy = _ScreenParams.xy * r2.yw;
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
