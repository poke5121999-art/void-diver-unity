// LCArt/Character/shader_DE_Character_Trail | pass 0 "ForwardLit" | progFragment | keywords: (none) | blob 2
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
  uint4 r0, r1, r2;
  r0 = 0u; r1 = 0u; r2 = 0u;
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
  r0.xyzw = vd_asuint(_MainTex.SampleBias(sampler_MainTex, vd_asfloat(v1.xy), vd_asfloat(_GlobalMipBias.x)).xyzw);
  r1.x = vd_asuint((vd_asfloat(r0.w) + -0.100000001));
  r1.x = vd_asuint((-(int)(vd_asfloat(r1.x) < 0.0)));
  if (r1.x != 0u) discard;
  r1.xy = vd_asuint((vd_asfloat(v7.xy) / vd_asfloat(v7.ww)));
  r1.xy = vd_asuint((vd_asfloat(r1.xy) * vd_asfloat(_ScreenParams.xy)));
  r1.zw = vd_asuint((vd_asfloat(r1.xy) * float2(0.5, 0.5)));
  r2.xy = vd_asuint((vd_asfloat(_ScreenSize.xy) / vd_asfloat(_ScreenParams.xy)));
  r1.xyzw = vd_asuint((vd_asfloat(r1.xyzw) * vd_asfloat(r2.xyxy)));
  r1.xyzw = vd_asuint(floor(vd_asfloat(r1.xyzw)));
  r1.xyzw = ((uint4)vd_asfloat(r1.xyzw));
  r1.xyzw = (r1.xyzw & uint4(0x00000003u, 0x00000003u, 0x00000003u, 0x00000003u));
  r1.w = (r1.w << (uint)2);
  r1.z = vd_asuint((vd_asint(r1.w) + vd_asint(r1.z)));
  r1.z = x0[r1.z + 0].x;
  r1.w = vd_asuint((vd_asfloat(v2.w) + vd_asfloat(_Effect)));
  r1.z = vd_asuint(((-(vd_asfloat(r1.z))) + vd_asfloat(r1.w)));
  r1.z = vd_asuint((-(int)(vd_asfloat(r1.z) < 0.0)));
  if (r1.z != 0u) discard;
  r1.y = (r1.y << (uint)2);
  r1.x = vd_asuint((vd_asint(r1.y) + vd_asint(r1.x)));
  r1.x = x0[r1.x + 0].x;
  r1.y = vd_asuint(saturate(vd_asfloat(_IsVertexColorAlphaUsed)));
  r1.z = vd_asuint((vd_asfloat(v8.w) + -1.0));
  r1.y = vd_asuint((vd_asfloat(r1.y) * vd_asfloat(r1.z) + 1.0));
  r0.w = vd_asuint((vd_asfloat(r0.w) * vd_asfloat(r1.y)));
  r0.w = vd_asuint((vd_asfloat(r0.w) * vd_asfloat(r0.w)));
  r1.x = vd_asuint((vd_asfloat(r0.w) * vd_asfloat(v8.w) + (-(vd_asfloat(r1.x)))));
  r0.xyzw = vd_asuint((vd_asfloat(r0.xyzw) * vd_asfloat(v8.xyzw)));
  o0.w = vd_asfloat(r0.w);
  r0.w = vd_asuint((-(int)(vd_asfloat(r1.x) < 0.0)));
  if (r0.w != 0u) discard;
  r1.xy = vd_asuint(((-(vd_asfloat(_PlaneBoundMaxPos.xz))) + vd_asfloat((VD_COL(unity_ObjectToWorld, 3).xz))));
  r1.zw = vd_asuint((vd_asfloat(_PlaneBoundMinPos.xz) + (-(vd_asfloat(_PlaneBoundMaxPos.xz)))));
  r1.xy = vd_asuint(saturate((vd_asfloat(r1.xy) / vd_asfloat(r1.zw))));
  r2.xyzw = vd_asuint(_PlanePointBuffer.SampleLevel(sampler_PlanePointBuffer, vd_asfloat(r1.xy), 0.0).xyzw);
  r0.w = vd_asuint(saturate((vd_asfloat(r2.w) * 4.0)));
  r1.x = vd_asuint((vd_asfloat(r0.w) * -2.0 + 3.0));
  r0.w = vd_asuint((vd_asfloat(r0.w) * vd_asfloat(r0.w)));
  r0.w = vd_asuint((vd_asfloat(r0.w) * vd_asfloat(r1.x)));
  r0.w = vd_asuint((vd_asfloat(r0.w) * vd_asfloat(_GlobalPlanePointBufferEnable)));
  r1.xy = vd_asuint((vd_asfloat(v4.xz) + (-(vd_asfloat(_PlaneBoundMaxPos.xz)))));
  r1.xy = vd_asuint(saturate((vd_asfloat(r1.xy) / vd_asfloat(r1.zw))));
  r1.xyzw = vd_asuint(_PlaneSightRenderTarget.SampleLevel(sampler_PlaneSightRenderTarget, vd_asfloat(r1.xy), 0.0).xyzw);
  r1.x = vd_asuint((vd_asfloat(r1.w) * vd_asfloat(_GlobalPlaneSystemEnable)));
  r0.w = vd_asuint(max(vd_asfloat(r0.w), vd_asfloat(r1.x)));
  r1.xyz = vd_asuint((vd_asfloat(r0.www) * vd_asfloat(r0.xyz)));
  r0.w = vd_asuint(dot(vd_asfloat(v2.xyz), vd_asfloat(_MainLightPosition.xyz)));
  r0.w = vd_asuint(saturate((vd_asfloat(r0.w) * 0.5 + 0.5)));
  r2.xyz = vd_asuint((vd_asfloat(_MainLightColor.xyz) * vd_asfloat(unity_LightData.zzz) + float3(-1.0, -1.0, -1.0)));
  r2.xyz = vd_asuint((vd_asfloat(r2.xyz) * vd_asfloat(r0.www) + float3(1.0, 1.0, 1.0)));
  r0.xyz = vd_asuint((vd_asfloat(r0.xyz) * vd_asfloat(r2.xyz)));
  r0.w = (vd_asuint(_MainLightLayerMask) & vd_asuint(unity_RenderingLayer.x));
  r0.xyz = ((r0.www != 0u) ? r0.xyz : uint3(0x00000000u, 0x00000000u, 0x00000000u));
  r0.xyz = vd_asuint((vd_asfloat(r1.xyz) * float3(0.25, 0.25, 0.25) + vd_asfloat(r0.xyz)));
  r0.xyz = vd_asuint((vd_asfloat(r0.xyz) * vd_asfloat(v8.xyz) + float3(-0.100000001, -0.100000001, -0.100000001)));
  r0.xyz = vd_asuint((vd_asfloat(v2.www) * vd_asfloat(r0.xyz) + float3(0.100000001, 0.100000001, 0.100000001)));
  r1.xyz = vd_asuint(((-(vd_asfloat(r0.xyz))) + vd_asfloat(unity_FogColor.xyz)));
  r0.w = vd_asuint((vd_asfloat(v3.y) + (-(vd_asfloat(_GlobalFogHeightMin)))));
  r1.w = vd_asuint(((-(vd_asfloat(_GlobalFogHeightMin))) + vd_asfloat(_GlobalFogHeightMax)));
  r0.w = vd_asuint((vd_asfloat(r0.w) / vd_asfloat(r1.w)));
  r0.w = vd_asuint(saturate(((-(vd_asfloat(r0.w))) + 1.0)));
  r0.w = vd_asuint((vd_asfloat(r0.w) * vd_asfloat(_GlobalFogHeightEnable)));
  r0.w = vd_asuint(max(vd_asfloat(r0.w), 0.0));
  o0.xyz = (vd_asfloat(r0.www) * vd_asfloat(r1.xyz) + vd_asfloat(r0.xyz));
  return;
}
