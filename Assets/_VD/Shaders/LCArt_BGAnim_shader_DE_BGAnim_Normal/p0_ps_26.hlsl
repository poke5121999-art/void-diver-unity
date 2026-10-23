// LCArt/BGAnim/shader_DE_BGAnim_Normal | pass 0 "ForwardLit" | progFragment | keywords: FOG_LINEAR _DE_EDITOR_CAMERA | blob 26
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
  uint4 r0, r1, r2, r3, r4;
  r0 = 0u; r1 = 0u; r2 = 0u; r3 = 0u; r4 = 0u;
  uint4 x0[16];

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
  r2.xy = vd_asuint((vd_asfloat(v7.xy) / vd_asfloat(v7.ww)));
  r1.w = vd_asuint(sqrt(vd_asfloat(unity_LightData.z)));
  r1.w = vd_asuint(min(vd_asfloat(r1.w), 1.0));
  r2.z = vd_asuint(dot(vd_asfloat(v2.xyz), vd_asfloat(_MainLightPosition.xyz)));
  r2.z = vd_asuint(saturate((vd_asfloat(r2.z) * 0.5 + 0.5)));
  r3.xyz = vd_asuint((vd_asfloat(_MainLightColor.xyz) * vd_asfloat(r1.www) + (-(vd_asfloat(_GlobalBackgroundShadowColor.xyz)))));
  r3.xyz = vd_asuint((vd_asfloat(r2.zzz) * vd_asfloat(r3.xyz) + vd_asfloat(_GlobalBackgroundShadowColor.xyz)));
  r4.xyzw = vd_asuint(_EmissionTex.SampleBias(sampler_EmissionTex, vd_asfloat(v1.xy), vd_asfloat(_GlobalMipBias.x)).xyzw);
  r0.xyz = vd_asuint((vd_asfloat(r0.xyz) * vd_asfloat(r4.xxx)));
  r0.xyz = vd_asuint((vd_asfloat(r0.xyz) * vd_asfloat((_EmissionIntensity).xxx)));
  o0.xyz = (vd_asfloat(r3.xyz) * vd_asfloat(r1.xyz) + vd_asfloat(r0.xyz));
  r1.xyzw = vd_asuint(_CustomDepthBuffer.SampleBias(sampler_CustomDepthBuffer, vd_asfloat(r2.xy), vd_asfloat(_GlobalMipBias.x)).xyzw);
  r0.x = vd_asuint(((-(vd_asfloat(r1.z))) + vd_asfloat(v0.z)));
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
  r1.xy = vd_asuint((vd_asfloat(r2.xy) * vd_asfloat(_ScreenParams.xy)));
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
