// LCArt/Background/shader_DE_BG_Particle | pass 2 "Glass" | progFragment | keywords: FOG_LINEAR _DE_EDITOR_CAMERA _LIGHT_LAYERS | blob 53
void frag(
  float4 v0 : VDREG0,
  float4 v1 : VDREG1,
  float4 v2 : VDREG2,
  float4 v3 : VDREG3,
  float4 v4 : VDREG4,
  float4 v5 : VDREG5,
  float4 v6 : VDREG6,
  float4 v7 : SV_POSITION0,
  float4 v8 : VDREG8,
  float4 v9 : VDREG9,
  float4 v10 : VDREG10,
  float4 v11 : VDREG11,
  out float4 o0 : SV_Target0)
{
  uint4 r0, r1, r2, r3, r4, r5;
  r0 = 0u; r1 = 0u; r2 = 0u; r3 = 0u; r4 = 0u; r5 = 0u;
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
  r0.xyzw = vd_asuint(_EmissionMap.SampleBias(sampler_vd_point_repeat, vd_asfloat(v0.xy), vd_asfloat(_GlobalMipBias.x)).xyzw);
  r0.y = vd_asuint((-(int)(vd_asfloat(r0.z) < 0.5)));
  if (r0.y != 0u) discard;
  r1.xyzw = vd_asuint(_BaseMap.SampleBias(sampler_BaseMap, vd_asfloat(v0.xy), vd_asfloat(_GlobalMipBias.x)).xyzw);
  r1.xyz = vd_asuint((vd_asfloat(r1.xyz) * vd_asfloat(_BaseColor.xyz)));
  r2.xyzw = vd_asuint(_BumpMap.SampleBias(sampler_vd_point_repeat, vd_asfloat(v0.xy), vd_asfloat(_GlobalMipBias.x)).xyzw);
  r2.x = vd_asuint((vd_asfloat(r2.x) * vd_asfloat(r2.w)));
  r3.xy = vd_asuint((vd_asfloat(r2.xy) + vd_asfloat(r2.xy)));
  r0.yw = vd_asuint((vd_asfloat(r2.xy) * float2(2.0, 2.0) + float2(-1.0, -1.0)));
  r0.y = vd_asuint(dot(vd_asfloat(r0.yw), vd_asfloat(r0.yw)));
  r0.y = vd_asuint(min(vd_asfloat(r0.y), 1.0));
  r0.y = vd_asuint(((-(vd_asfloat(r0.y))) + 1.0));
  r0.y = vd_asuint(sqrt(vd_asfloat(r0.y)));
  r3.z = vd_asuint(max(vd_asfloat(r0.y), 1.00000002e-16));
  r2.xyz = vd_asuint((vd_asfloat(r3.xyz) + float3(-1.0, -1.0, -1.0)));
  r2.xyz = vd_asuint((vd_asfloat((_BumpScale).xxx) * vd_asfloat(r2.xyz) + float3(0.0, 0.0, 1.0)));
  r0.yw = vd_asuint((-(int2)(float2(1.5, 0.5) < vd_asfloat(v1.wz))));
  if (r0.y != 0u) {
    r3.xy = vd_asuint((float2(1.0, 1.0) / vd_asfloat(_ScaledScreenParams.xy)));
    r3.xy = vd_asuint((vd_asfloat(r3.xy) * vd_asfloat(v7.xy)));
    r0.y = vd_asuint((vd_asfloat(r3.y) * vd_asfloat(_ScaleBiasRt.x) + vd_asfloat(_ScaleBiasRt.y)));
    r3.z = vd_asuint(((-(vd_asfloat(r0.y))) + 1.0));
    r0.y = vd_asuint(((-(vd_asfloat(_GlobalSeeThroughAlpha))) + 1.0));
    r0.y = vd_asuint((vd_asfloat(v0.z) * vd_asfloat(r0.y) + vd_asfloat(_GlobalSeeThroughAlpha)));
    r0.y = vd_asuint((vd_asfloat(r0.y) * vd_asfloat(v8.w)));
    r3.yw = vd_asuint((vd_asfloat(_ScreenSize.xy) / vd_asfloat(_ScreenParams.xy)));
    r3.xz = vd_asuint((vd_asfloat(r3.xz) * vd_asfloat(_ScreenParams.xy)));
    r3.xy = vd_asuint((vd_asfloat(r3.yw) * vd_asfloat(r3.xz)));
    r3.xy = vd_asuint(floor(vd_asfloat(r3.xy)));
    r3.xy = ((uint2)vd_asfloat(r3.xy));
    r3.xy = (r3.xy & uint2(0x00000003u, 0x00000003u));
    r1.w = (r3.y << (uint)2);
    r1.w = vd_asuint((vd_asint(r1.w) + vd_asint(r3.x)));
    r1.w = x0[r1.w + 0].x;
    r0.y = vd_asuint((vd_asfloat(r0.y) * vd_asfloat(_BaseColor.w) + (-(vd_asfloat(r1.w)))));
    r0.y = vd_asuint((-(int)(vd_asfloat(r0.y) < 0.0)));
    if (r0.y != 0u) discard;
  }
  r0.y = vd_asuint((-(int)(0.5 < vd_asfloat(_UseWorldNormalMap))));
  if (r0.y != 0u) {
    r3.xyzw = vd_asuint(_WorldNormalMap.SampleBias(sampler_vd_point_repeat, vd_asfloat(v0.xy), vd_asfloat(_GlobalMipBias.x)).xyzw);
    r3.xyz = vd_asuint((vd_asfloat(r3.xyz) * float3(2.0, 2.0, 2.0) + float3(-1.0, -1.0, -1.0)));
    r0.y = vd_asuint(dot(vd_asfloat(r3.xyz), vd_asfloat(r3.xyz)));
    r0.y = vd_asuint(rsqrt(vd_asfloat(r0.y)));
    r3.xyz = vd_asuint((vd_asfloat(r0.yyy) * vd_asfloat(r3.xyz)));
    r0.y = vd_asuint(dot(vd_asfloat(v4.xyz), vd_asfloat(r3.xyz)));
    r4.xyz = vd_asuint(((-(vd_asfloat(r3.xyz))) * vd_asfloat(r0.yyy) + vd_asfloat(v4.xyz)));
    r0.y = vd_asuint(dot(vd_asfloat(r4.xyz), vd_asfloat(r4.xyz)));
    r0.y = vd_asuint(rsqrt(vd_asfloat(r0.y)));
    r4.xyz = vd_asuint((vd_asfloat(r0.yyy) * vd_asfloat(r4.xyz)));
    r0.y = vd_asuint(dot(vd_asfloat(v5.xyz), vd_asfloat(r3.xyz)));
    r5.xyz = vd_asuint(((-(vd_asfloat(r3.xyz))) * vd_asfloat(r0.yyy) + vd_asfloat(v5.xyz)));
    r0.y = vd_asuint(dot(vd_asfloat(r5.xyz), vd_asfloat(r5.xyz)));
    r0.y = vd_asuint(rsqrt(vd_asfloat(r0.y)));
    r5.xyz = vd_asuint((vd_asfloat(r0.yyy) * vd_asfloat(r5.xyz)));
    r5.xyz = vd_asuint((vd_asfloat(r2.yyy) * vd_asfloat(r5.xyz)));
    r4.xyz = vd_asuint((vd_asfloat(r2.xxx) * vd_asfloat(r4.xyz) + vd_asfloat(r5.xyz)));
    r3.xyz = vd_asuint((vd_asfloat(r2.zzz) * vd_asfloat(r3.xyz) + vd_asfloat(r4.xyz)));
  } else {
    r4.xyz = vd_asuint((vd_asfloat(r2.yyy) * vd_asfloat(v5.xyz)));
    r2.xyw = vd_asuint((vd_asfloat(r2.xxx) * vd_asfloat(v4.xyz) + vd_asfloat(r4.xyz)));
    r3.xyz = vd_asuint((vd_asfloat(r2.zzz) * vd_asfloat(v3.xyz) + vd_asfloat(r2.xyw)));
  }
  r0.y = vd_asuint(dot(vd_asfloat(r3.xyz), vd_asfloat(r3.xyz)));
  r0.y = vd_asuint(rsqrt(vd_asfloat(r0.y)));
  r2.xyz = vd_asuint((vd_asfloat(r0.yyy) * vd_asfloat(r3.xyz)));
  r0.y = vd_asuint((-(int)(vd_asfloat(v1.z) < 1.5)));
  r0.y = (r0.y & r0.w);
  r0.w = vd_asuint((-(int)(0.125 >= vd_asfloat(v11.y))));
  r0.y = (r0.w & r0.y);
  r3.xyz = ((r0.yyy != 0u) ? vd_asuint(v11.xyz) : vd_asuint(v2.xyz));
  r2.xyz = ((r0.yyy != 0u) ? uint3(0x00000000u, 0x3f800000u, 0x00000000u) : r2.xyz);
  r0.y = vd_asuint((-(int)(vd_asfloat(unity_OrthoParams.w) == 0.0)));
  r4.xyz = vd_asuint(((-(vd_asfloat(r3.xyz))) + vd_asfloat(_WorldSpaceCameraPos.xyz)));
  r0.w = vd_asuint(dot(vd_asfloat(r4.xyz), vd_asfloat(r4.xyz)));
  r0.w = vd_asuint(rsqrt(vd_asfloat(r0.w)));
  r4.xyz = vd_asuint((vd_asfloat(r0.www) * vd_asfloat(r4.xyz)));
  r5.x = ((r0.y != 0u) ? r4.x : vd_asuint((VD_COL(unity_MatrixV, 0).z)));
  r5.y = ((r0.y != 0u) ? r4.y : vd_asuint((VD_COL(unity_MatrixV, 1).z)));
  r5.z = ((r0.y != 0u) ? r4.z : vd_asuint((VD_COL(unity_MatrixV, 2).z)));
  r4.xyz = vd_asuint((vd_asfloat(r3.yyy) * vd_asfloat((VD_COL(unity_MatrixVP, 1).xyw))));
  r3.xyw = vd_asuint((vd_asfloat((VD_COL(unity_MatrixVP, 0).xyw)) * vd_asfloat(r3.xxx) + vd_asfloat(r4.xyz)));
  r3.xyz = vd_asuint((vd_asfloat((VD_COL(unity_MatrixVP, 2).xyw)) * vd_asfloat(r3.zzz) + vd_asfloat(r3.xyw)));
  r3.xyz = vd_asuint((vd_asfloat(r3.xyz) + vd_asfloat((VD_COL(unity_MatrixVP, 3).xyw))));
  r0.yw = vd_asuint((vd_asfloat(r3.xy) / (-(vd_asfloat(r3.zz)))));
  r3.xy = vd_asuint((vd_asfloat(r0.yw) * float2(0.5, 0.5) + float2(0.5, 0.5)));
  r0.x = vd_asuint(saturate(dot(vd_asfloat(r0.xx), vd_asfloat(r0.zz))));
  r0.y = (vd_asuint(_MainLightLayerMask) & vd_asuint(unity_RenderingLayer.x));
  r4.xyz = vd_asuint((vd_asfloat(r0.xxx) * vd_asfloat(r1.xyz)));
  r4.xyz = vd_asuint((vd_asfloat(r4.xyz) * vd_asfloat(_MainLightColor.xyz)));
  r5.xyz = vd_asuint((vd_asfloat(r5.xyz) + vd_asfloat(_MainLightPosition.xyz)));
  r0.z = vd_asuint(dot(vd_asfloat(r5.xyz), vd_asfloat(r5.xyz)));
  r0.z = vd_asuint(max(vd_asfloat(r0.z), 6.10351562e-05));
  r0.z = vd_asuint(rsqrt(vd_asfloat(r0.z)));
  r5.xyz = vd_asuint((vd_asfloat(r0.zzz) * vd_asfloat(r5.xyz)));
  r0.z = vd_asuint(saturate(dot(vd_asfloat(r2.xyz), vd_asfloat(r5.xyz))));
  r0.z = vd_asuint(log2(vd_asfloat(r0.z)));
  r0.z = vd_asuint((vd_asfloat(r0.z) * 128.0));
  r0.z = vd_asuint(exp2(vd_asfloat(r0.z)));
  r2.xyz = vd_asuint((vd_asfloat(r0.zzz) * vd_asfloat(_MainLightColor.xyz)));
  r2.xyz = vd_asuint((vd_asfloat(r0.xxx) * vd_asfloat(r2.xyz)));
  r2.xyz = vd_asuint((vd_asfloat(r2.xyz) * vd_asfloat(unity_LightData.zzz)));
  r2.xyz = vd_asuint((vd_asfloat(r4.xyz) * float3(0.600000024, 0.600000024, 0.600000024) + vd_asfloat(r2.xyz)));
  r0.yzw = ((r0.yyy != 0u) ? r2.xyz : uint3(0x00000000u, 0x00000000u, 0x00000000u));
  r0.x = vd_asuint(((-(vd_asfloat(r0.x))) + 1.0));
  r1.w = vd_asuint(dot(vd_asfloat(r1.xyz), float3(0.298999995, 0.587000012, 0.114)));
  r1.w = vd_asuint((vd_asfloat(r1.w) * vd_asfloat(r1.w)));
  r2.y = vd_asuint((vd_asfloat(r1.w) * vd_asfloat(r1.w)));
  r2.x = 0x3c4ccccdu;
  r2.xy = vd_asuint((vd_asfloat(r2.xy) * float2(0.0, -0.0125000002)));
  r3.z = vd_asuint(((-(vd_asfloat(r3.x))) + 1.0));
  r2.xy = vd_asuint((vd_asfloat(r2.xy) * vd_asfloat(r0.xx) + vd_asfloat(r3.zy)));
  r2.xyzw = vd_asuint(_AfterRenderingGrabpassTexture.SampleBias(sampler_vd_linear_clamp, vd_asfloat(r2.xy), vd_asfloat(_GlobalMipBias.x)).xyzw);
  o0.xyz = (vd_asfloat(r2.xyz) * vd_asfloat(r1.xyz) + vd_asfloat(r0.yzw));
  o0.w = vd_asfloat(0x3f800000u);
  return;
}
