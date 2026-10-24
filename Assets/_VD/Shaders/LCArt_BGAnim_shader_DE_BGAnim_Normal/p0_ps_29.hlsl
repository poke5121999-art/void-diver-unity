// LCArt/BGAnim/shader_DE_BGAnim_Normal | pass 0 "ForwardLit" | progFragment | keywords: _CLUSTER_LIGHT_LOOP _DE_EDITOR_CAMERA | blob 29
void frag(
  float4 v0 : SV_POSITION0,
  float4 v1 : VDREG1,
  float4 v2 : VDREG2,
  float4 v3 : VDREG3,
  float4 v4 : VDREG4,
  float4 v5 : VDREG5,
  float4 v6 : VDREG6,
  float4 v7 : VDREG7,
  float4 v8 : VDREG8,
  out float4 o0 : SV_Target0)
{
#if UNITY_UV_STARTS_AT_TOP
  uint4 r0, r1, r2, r3, r4, r5, r6, r7, r8, r9, r10;
  r0 = 0u; r1 = 0u; r2 = 0u; r3 = 0u; r4 = 0u; r5 = 0u; r6 = 0u; r7 = 0u; r8 = 0u; r9 = 0u; r10 = 0u;
#else
  uint4 r0, r1, r2, r3, r4, r5, r6, r7, r8, r9, r10, r11;
  r0 = 0u; r1 = 0u; r2 = 0u; r3 = 0u; r4 = 0u; r5 = 0u; r6 = 0u; r7 = 0u; r8 = 0u; r9 = 0u; r10 = 0u; r11 = 0u;
#endif
  uint4 x0[16];

#if UNITY_UV_STARTS_AT_TOP
#else
  r11.xyzw = vd_asuint(v0.xyzw);
  r11.z = vd_asuint(((-(vd_asfloat(v0.z))) + 1.0));
#endif
  x0[0].x = 0x3d70f16fu;
  x0[1].x = 0x3f07878bu;
  x0[2].x = 0x3e34b4d0u;
  x0[3].x = 0x3f25a5a9u;
  x0[4].x = 0x3f43c3c6u;
  x0[5].x = 0x3e9696a2u;
  x0[6].x = 0x3f61e1e3u;
  x0[7].x = 0x3ed2d2ddu;
  x0[8].x = 0x3e70f0e9u;
  x0[9].x = 0x3f34b4afu;
  x0[10].x = 0x3df0f0e9u;
  x0[11].x = 0x3f169692u;
  x0[12].x = 0x3f70f0e9u;
  x0[13].x = 0x3ef0f0e9u;
  x0[14].x = 0x3f52d2ccu;
  x0[15].x = 0x3eb4b4afu;
  r0.x = vd_asuint((-(int)(vd_asfloat(_GlobalBoxAreaEnable) >= 0.5)));
  if (r0.x != 0u) {
    r0.x = 0x00000000u;
    while (true) {
      r0.y = vd_asuint((-(int)(vd_asint(r0.x) >= 8)));
      if (r0.y != 0u) break;
      r0.y = vd_asuint((-(int)(vd_asfloat(_GlobalBoxAreaCoverCharacters[(int)(r0.x)]) < 0.5)));
      if (r0.y != 0u) {
        r0.y = vd_asuint((vd_asint(r0.x) + 1));
        r0.x = r0.y;
        continue;
      }
      r0.yzw = vd_asuint((vd_asfloat((VD_COL(unity_ObjectToWorld, 3).xyz)) + (-(vd_asfloat(_GlobalBoxAreaCenters[(int)(r0.x)].xyz)))));
      r0.yzw = vd_asuint((abs(vd_asfloat(r0.yzw)) + (-(vd_asfloat(_GlobalBoxAreaExtents[(int)(r0.x)].xyz)))));
      r0.y = vd_asuint(max(vd_asfloat(r0.z), vd_asfloat(r0.y)));
      r0.y = vd_asuint(max(vd_asfloat(r0.w), vd_asfloat(r0.y)));
      r0.y = vd_asuint((-(int)(0.0 >= vd_asfloat(r0.y))));
      if (r0.y != 0u) {
        if (0xffffffffu != 0u) discard;
        break;
      }
      r0.x = vd_asuint((vd_asint(r0.x) + 1));
    }
  }
  r0.xyzw = vd_asuint(_MainTex.SampleBias(sampler_MainTex, vd_asfloat(v1.xy), vd_asfloat(_GlobalMipBias.x)).xyzw);
  r1.x = vd_asuint((vd_asfloat(r0.w) + -0.100000001));
  r1.x = vd_asuint((-(int)(vd_asfloat(r1.x) < 0.0)));
  if (r1.x != 0u) discard;
  r1.xyz = vd_asuint((vd_asfloat(r0.xyz) * vd_asfloat(v8.xyz)));
  r1.w = vd_asuint(saturate(vd_asfloat(_IsVertexColorAlphaUsed)));
  r2.x = vd_asuint((vd_asfloat(v8.w) + -1.0));
  r1.w = vd_asuint((vd_asfloat(r1.w) * vd_asfloat(r2.x) + 1.0));
  r0.w = vd_asuint((vd_asfloat(r0.w) * vd_asfloat(r1.w)));
  r2.xy = vd_asuint((float2(1.0, 1.0) / vd_asfloat(_ScaledScreenParams.xy)));
#if UNITY_UV_STARTS_AT_TOP
  r2.xy = vd_asuint((vd_asfloat(r2.xy) * vd_asfloat(v0.xy)));
  r1.w = vd_asuint((vd_asfloat(r2.y) * vd_asfloat(_ScaleBiasRt.x) + vd_asfloat(_ScaleBiasRt.y)));
  r2.z = vd_asuint(((-(vd_asfloat(r1.w))) + 1.0));
#else
  r2.xy = vd_asuint((vd_asfloat(r2.xy) * vd_asfloat(r11.xy)));
  r1.w = r2.y;
  r2.z = r1.w;
#endif
  r2.yw = vd_asuint((vd_asfloat(v7.xy) / vd_asfloat(v7.ww)));
  r1.w = vd_asuint(dot(vd_asfloat(v2.xyz), vd_asfloat(_MainLightPosition.xyz)));
  r1.w = vd_asuint(saturate((vd_asfloat(r1.w) * 0.5 + 0.5)));
  r3.xyz = vd_asuint((vd_asfloat(_MainLightColor.xyz) + (-(vd_asfloat(_GlobalBackgroundShadowColor.xyz)))));
  r3.xyz = vd_asuint((vd_asfloat(r1.www) * vd_asfloat(r3.xyz) + vd_asfloat(_GlobalBackgroundShadowColor.xyz)));
  r2.xz = vd_asuint((vd_asfloat(r2.xz) * vd_asfloat(_FPParams1.xy)));
  r2.xz = ((uint2)vd_asfloat(r2.xz));
  r4.xy = ((uint2)vd_asfloat(_FPParams1.zw));
  r1.w = vd_asuint((vd_asint(r2.z) * vd_asint(r4.x) + vd_asint(r2.x)));
  r1.w = vd_asuint((vd_asint(r4.y) * vd_asint(r1.w)));
  r5.x = vd_asuint((-(vd_asfloat((VD_COL(unity_MatrixV, 0).z)))));
  r5.y = vd_asuint((-(vd_asfloat((VD_COL(unity_MatrixV, 1).z)))));
  r5.z = vd_asuint((-(vd_asfloat((VD_COL(unity_MatrixV, 2).z)))));
  r4.xzw = vd_asuint((vd_asfloat(v4.xyz) + (-(vd_asfloat(_WorldSpaceCameraPos.xyz)))));
  r2.x = vd_asuint(dot(vd_asfloat(r5.xyz), vd_asfloat(r4.xzw)));
  r2.z = vd_asuint((-(int)(vd_asfloat(unity_OrthoParams.w) == 0.0)));
  r3.w = vd_asuint(log2(vd_asfloat(r2.x)));
  r2.x = ((r2.z != 0u) ? r3.w : r2.x);
  r2.x = vd_asuint((vd_asfloat(r2.x) * vd_asfloat(_FPParams0.x) + vd_asfloat(_FPParams0.y)));
  r2.x = ((uint)vd_asfloat(r2.x));
  r2.z = ((uint)vd_asfloat(_FPParams2.x));
  r2.z = vd_asuint((vd_asint(r2.z) + -1));
  r3.w = vd_asuint((vd_asint(r4.y) + 2));
  r2.x = min(r2.z, r2.x);
  r2.z = vd_asuint((vd_asint(r3.w) * vd_asint(r2.x)));
  r2.x = vd_asuint((vd_asint(r2.x) * vd_asint(r3.w) + 2));
  r3.w = (r2.z >> (uint)2);
  r4.x = (r2.z << (uint)31);
  r4.y = (r2.z << (uint)30);
  r4.xy = vd_asuint((vd_asint(r4.xy) >> 31));
  r4.zw = (r4.xx & vd_asuint(urp_ZBins[(int)(r3.w)].wy));
  r5.xy = (~r4.xy);
  r5.xz = (r5.xx & vd_asuint(urp_ZBins[(int)(r3.w)].zx));
  r4.xz = (r4.zw | r5.xz);
  r2.z = (r4.y & r4.x);
  r3.w = (r5.y & r4.z);
  r2.z = (r2.z | r3.w);
  r3.w = ((uint)vd_asfloat(_FPParams0.w));
  r4.xy = vd_asuint((vd_asfloat(v4.xz) + (-(vd_asfloat(_PlaneBoundMaxPos.xz)))));
  r4.zw = vd_asuint((vd_asfloat(_PlaneBoundMinPos.xz) + (-(vd_asfloat(_PlaneBoundMaxPos.xz)))));
  r4.xy = vd_asuint(saturate((vd_asfloat(r4.xy) / vd_asfloat(r4.zw))));
  r4.xyzw = vd_asuint(_PlaneSightRenderTarget.SampleLevel(sampler_PlaneSightRenderTarget, vd_asfloat(r4.xy), 0.0).xyzw);
  r4.x = vd_asuint((vd_asfloat(_GlobalPlaneSystemEnable) * vd_asfloat(r4.w) + -1.0));
  r5.x = r1.w;
  r5.z = 0x00000000u;
  r5.yw = r2.xz;
  r4.yzw = uint3(0x00000000u, 0x00000000u, 0x00000000u);
  while (true) {
    r6.x = (r5.w >> (uint)16);
    r6.yz = r5.zw;
    while (true) {
      r6.w = vd_asuint((-(int)(vd_asint(r6.y) == 0)));
      r7.x = (r6.z & 0x0000ffffu);
      r7.x = vd_asuint((-(int)(r6.x >= r7.x)));
      r6.w = (r6.w & r7.x);
      if (r6.w == 0u) break;
      r6.w = (r6.z >> (uint)5);
      r6.w = (r6.w & 0x000007ffu);
      r7.xyzw = vd_asuint((vd_asint(r5.xxyy) + vd_asint(r6.wwww)));
      r8.xy = (r7.yw >> (uint2)2);
      r9.xz = (r7.xz << (uint2)31);
      r9.yw = (r7.yw << (uint2)30);
      r7.xyzw = vd_asuint((vd_asint(r9.xyzw) >> 31));
      r8.zw = (r7.xx & vd_asuint(urp_Tiles[(int)(r8.x)].wy));
      r9.xyzw = (~r7.xyzw);
      r10.xy = (r9.xx & vd_asuint(urp_Tiles[(int)(r8.x)].zx));
      r8.xz = (r8.zw | r10.xy);
      r7.x = (r7.y & r8.x);
      r7.y = (r9.y & r8.z);
      r7.x = (r7.y | r7.x);
      r7.yz = (r7.zz & vd_asuint(urp_ZBins[(int)(r8.y)].wy));
      r8.xy = (r9.zz & vd_asuint(urp_ZBins[(int)(r8.y)].zx));
      r7.yz = (r7.yz | r8.xy);
      r7.y = (r7.w & r7.y);
      r7.z = (r9.w & r7.z);
      r7.y = (r7.z | r7.y);
      r7.x = (r7.y & r7.x);
      r7.y = (0xffffffffu << (uint)(int)(r6.z & 31u));
      r7.x = (r7.y & r7.x);
      r6.w = (r6.w << (uint)5);
      r6.w = vd_asuint(((-(vd_asint(r6.w))) + vd_asint(r6.x)));
      r6.w = min(r6.w, 0x0000001fu);
      r6.w = vd_asuint(((-(vd_asint(r6.w))) + 31));
      r6.w = (0xffffffffu >> (uint)(int)(r6.w & 31u));
      r6.y = (r6.w & r7.x);
      r6.w = vd_asuint((vd_asint(r6.z) + 32));
      r6.z = (r6.w & 0xffffffe0u);
    }
    r5.w = r6.z;
    if (r6.y == 0u) break;
    r6.x = vd_asuint((-vd_asint(r6.y)));
    r6.x = (r6.x & r6.y);
    r6.x = vd_asuint(((float)r6.x));
    r6.x = (r6.x >> (uint)23);
    r6.x = vd_asuint((vd_asint(r6.x) + -127));
    r6.w = (0x00000001u << (uint)(int)(r6.x & 31u));
    r5.z = (r6.w ^ r6.y);
    r6.w = vd_asuint((vd_asint(r5.w) + -32));
    r6.w = (r6.w & 0x0000ffe0u);
    r6.x = vd_asuint((vd_asint(r6.x) + vd_asint(r6.w)));
    r6.x = vd_asuint((vd_asint(r3.w) + vd_asint(r6.x)));
    r7.xyz = vd_asuint(((-(vd_asfloat(v4.xyz))) * vd_asfloat(_AdditionalLightsPosition[(int)(r6.x)].www) + vd_asfloat(_AdditionalLightsPosition[(int)(r6.x)].xyz)));
    r6.w = vd_asuint(dot(vd_asfloat(r7.xyz), vd_asfloat(r7.xyz)));
    r6.w = vd_asuint(max(vd_asfloat(r6.w), 6.10351562e-05));
    r7.w = vd_asuint(rsqrt(vd_asfloat(r6.w)));
    r7.xyz = vd_asuint((vd_asfloat(r7.www) * vd_asfloat(r7.xyz)));
    r7.w = vd_asuint((1.0 / vd_asfloat(r6.w)));
    r6.w = vd_asuint((vd_asfloat(r6.w) * vd_asfloat(_AdditionalLightsAttenuation[(int)(r6.x)].x)));
    r6.w = vd_asuint(((-(vd_asfloat(r6.w))) * vd_asfloat(r6.w) + 1.0));
    r6.w = vd_asuint(max(vd_asfloat(r6.w), 0.0));
    r6.w = vd_asuint((vd_asfloat(r6.w) * vd_asfloat(r6.w)));
    r6.w = vd_asuint((vd_asfloat(r6.w) * vd_asfloat(r7.w)));
    r7.w = vd_asuint(dot(vd_asfloat(_AdditionalLightsSpotDir[(int)(r6.x)].xyz), vd_asfloat(r7.xyz)));
    r7.w = vd_asuint(saturate((vd_asfloat(r7.w) * vd_asfloat(_AdditionalLightsAttenuation[(int)(r6.x)].z) + vd_asfloat(_AdditionalLightsAttenuation[(int)(r6.x)].w))));
    r7.w = vd_asuint((vd_asfloat(r7.w) * vd_asfloat(r7.w)));
    r6.w = vd_asuint((vd_asfloat(r6.w) * vd_asfloat(r7.w)));
    r7.w = (0x00000002u & vd_asuint(_AdditionalLightsLayerMasks[(int)(r6.x)]));
    r7.w = ((r7.w != 0u) ? 0x3f800000u : 0x00000000u);
    r7.w = vd_asuint((vd_asfloat(r7.w) * vd_asfloat(r4.x) + 1.0));
    r6.w = vd_asuint(sqrt(vd_asfloat(r6.w)));
    r6.w = vd_asuint(min(vd_asfloat(r6.w), 1.0));
    r8.xyz = vd_asuint((vd_asfloat(r6.www) * vd_asfloat(_AdditionalLightsColor[(int)(r6.x)].xyz)));
    r6.x = vd_asuint(dot(vd_asfloat(v2.xyz), vd_asfloat(r7.xyz)));
    r6.x = vd_asuint(saturate((vd_asfloat(r6.x) * 0.5 + 0.5)));
    r7.xyz = vd_asuint((vd_asfloat(r6.xxx) * vd_asfloat(r8.xyz)));
    r7.xyz = vd_asuint((vd_asfloat(r1.xyz) * vd_asfloat(r7.xyz)));
    r4.yzw = vd_asuint((vd_asfloat(r7.xyz) * vd_asfloat(r7.www) + vd_asfloat(r4.yzw)));
  }
  r1.xyz = vd_asuint((vd_asfloat(r3.xyz) * vd_asfloat(r1.xyz) + vd_asfloat(r4.yzw)));
  r3.xyzw = vd_asuint(_EmissionTex.SampleBias(sampler_EmissionTex, vd_asfloat(v1.xy), vd_asfloat(_GlobalMipBias.x)).xyzw);
  r0.xyz = vd_asuint((vd_asfloat(r0.xyz) * vd_asfloat(r3.xxx)));
  o0.xyz = (vd_asfloat(r0.xyz) * vd_asfloat((_EmissionIntensity).xxx) + vd_asfloat(r1.xyz));
  r1.xyzw = vd_asuint(_CustomDepthBuffer.SampleBias(sampler_CustomDepthBuffer, vd_asfloat(r2.yw), vd_asfloat(_GlobalMipBias.x)).xyzw);
#if UNITY_UV_STARTS_AT_TOP
  r0.x = vd_asuint(((-(vd_asfloat(r1.z))) + vd_asfloat(v0.z)));
#else
  r0.x = vd_asuint(((-(vd_asfloat(r1.z))) + vd_asfloat(r11.z)));
#endif
  r0.x = vd_asuint((vd_asfloat(r0.x) + -7.50000036e-05));
  r0.x = vd_asuint(saturate((vd_asfloat(r0.x) * -6666.6665)));
  r0.y = vd_asuint((vd_asfloat(r0.x) * -2.0 + 3.0));
  r0.x = vd_asuint((vd_asfloat(r0.x) * vd_asfloat(r0.x)));
  r0.zw = vd_asuint((vd_asfloat(r0.xw) * vd_asfloat(r0.yw)));
  r1.xy = vd_asuint(((-(vd_asfloat(float2(_GlobalSeeThroughCircleRange, _GlobalSeeThroughCircleAlpha)))) + float2(1.0, 1.0)));
  r1.z = vd_asuint(((-(vd_asfloat(r1.x))) + 1.0));
  r1.x = vd_asuint(((-(vd_asfloat(r1.x))) + vd_asfloat(r1.w)));
  r1.z = vd_asuint((1.0 / vd_asfloat(r1.z)));
  r1.x = vd_asuint(saturate((vd_asfloat(r1.z) * vd_asfloat(r1.x))));
  r1.z = vd_asuint((vd_asfloat(r1.x) * -2.0 + 3.0));
  r1.x = vd_asuint((vd_asfloat(r1.x) * vd_asfloat(r1.x)));
  r1.x = vd_asuint((vd_asfloat(r1.x) * vd_asfloat(r1.z)));
  r0.x = vd_asuint(((-(vd_asfloat(r0.y))) * vd_asfloat(r0.x) + 1.0));
  r0.x = vd_asuint((vd_asfloat(r1.x) * vd_asfloat(r0.x) + vd_asfloat(r0.z)));
  r0.x = vd_asuint((vd_asfloat(r0.x) + -1.0));
  r0.x = vd_asuint((vd_asfloat(_GlobalBackgroundSeeThroughEnable) * vd_asfloat(r0.x) + 1.0));
  r0.x = vd_asuint((vd_asfloat(r0.x) * vd_asfloat(r1.y) + vd_asfloat(_GlobalSeeThroughCircleAlpha)));
  r0.y = vd_asuint((vd_asfloat(_GlobalPlaneSystemEnable) * vd_asfloat(_CharacterAreaMaskOffEnable)));
  r0.y = vd_asuint((vd_asfloat(r0.y) * vd_asfloat(_GlobalBackgroundSeeThroughEnable)));
  r0.x = vd_asuint((vd_asfloat(r0.x) + -1.0));
  r0.x = vd_asuint((vd_asfloat(r0.y) * vd_asfloat(r0.x) + 1.0));
  r0.x = vd_asuint(min(vd_asfloat(r0.x), vd_asfloat(v2.w)));
  r0.yz = vd_asuint((vd_asfloat(_ScreenSize.xy) / vd_asfloat(_ScreenParams.xy)));
  r1.xy = vd_asuint((vd_asfloat(r2.yw) * vd_asfloat(_ScreenParams.xy)));
  r0.yz = vd_asuint((vd_asfloat(r0.yz) * vd_asfloat(r1.xy)));
  r0.yz = vd_asuint((vd_asfloat(r0.yz) * float2(0.5, 0.5)));
  r0.yz = vd_asuint(floor(vd_asfloat(r0.yz)));
  r0.yz = ((uint2)vd_asfloat(r0.yz));
  r0.yz = (r0.yz & uint2(0x00000003u, 0x00000003u));
  r0.z = (r0.z << (uint)2);
  r0.y = vd_asuint((vd_asint(r0.z) + vd_asint(r0.y)));
  r0.y = x0[r0.y + 0].x;
  r0.x = vd_asuint((vd_asfloat(r0.w) * vd_asfloat(r0.x) + (-(vd_asfloat(r0.y)))));
  r0.x = vd_asuint((-(int)(vd_asfloat(r0.x) < 0.0)));
  if (r0.x != 0u) discard;
  o0.w = vd_asfloat(r0.w);
  return;
}
