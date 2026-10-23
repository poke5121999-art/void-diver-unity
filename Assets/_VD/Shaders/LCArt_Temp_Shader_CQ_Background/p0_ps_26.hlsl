// LCArt/Temp/Shader_CQ_Background | pass 0 "ForwardLit" | progFragment | keywords: FOG_LINEAR _ADDITIONAL_LIGHTS _LIGHT_LAYERS | blob 26
void frag(
  float4 v0 : VDREG0,
  float4 v1 : VDREG1,
  float4 v2 : VDREG2,
  float4 v3 : VDREG3,
  float4 v4 : SV_POSITION0,
  float4 v5 : VDREG5,
  out float4 o0 : SV_Target0)
{
  uint4 r0, r1, r2, r3, r4, r5, r6;
  r0 = 0u; r1 = 0u; r2 = 0u; r3 = 0u; r4 = 0u; r5 = 0u; r6 = 0u;
  const uint4 icb[4] = { uint4(0x3f800000u, 0x00000000u, 0x00000000u, 0x00000000u), uint4(0x00000000u, 0x3f800000u, 0x00000000u, 0x00000000u), uint4(0x00000000u, 0x00000000u, 0x3f800000u, 0x00000000u), uint4(0x00000000u, 0x00000000u, 0x00000000u, 0x3f800000u) };

  r0.xyzw = vd_asuint(_BaseMap.SampleBias(sampler_BaseMap, vd_asfloat(v0.xy), vd_asfloat(_GlobalMipBias.x)).xyzw);
  r1.xyzw = vd_asuint((vd_asfloat(r0.wxyz) * vd_asfloat(_BaseColor.wxyz)));
  r0.x = vd_asuint((vd_asfloat(r0.w) * vd_asfloat(_BaseColor.w) + -0.5));
  r0.x = vd_asuint((-(int)(vd_asfloat(r0.x) < 0.0)));
  if (r0.x != 0u) discard;
  r0.x = vd_asuint(dot(vd_asfloat(v2.xyz), vd_asfloat(v2.xyz)));
  r0.x = vd_asuint(rsqrt(vd_asfloat(r0.x)));
  r0.xyz = vd_asuint((vd_asfloat(r0.xxx) * vd_asfloat(v2.xyz)));
  r2.x = vd_asuint((vd_asfloat(v1.y) * vd_asfloat((VD_COL(unity_MatrixV, 1).z))));
  r2.x = vd_asuint((vd_asfloat((VD_COL(unity_MatrixV, 0).z)) * vd_asfloat(v1.x) + vd_asfloat(r2.x)));
  r2.x = vd_asuint((vd_asfloat((VD_COL(unity_MatrixV, 2).z)) * vd_asfloat(v1.z) + vd_asfloat(r2.x)));
  r2.x = vd_asuint((vd_asfloat(r2.x) + vd_asfloat((VD_COL(unity_MatrixV, 3).z))));
  r2.x = vd_asuint(((-(vd_asfloat(r2.x))) + (-(vd_asfloat(_ProjectionParams.y)))));
  r2.x = vd_asuint(max(vd_asfloat(r2.x), 0.0));
  r2.x = vd_asuint(saturate((vd_asfloat(r2.x) * vd_asfloat(unity_FogParams.z) + vd_asfloat(unity_FogParams.w))));
  r0.w = 0x3f800000u;
  r3.x = vd_asuint(dot(vd_asfloat(unity_SHAr.xyzw), vd_asfloat(r0.xyzw)));
  r3.y = vd_asuint(dot(vd_asfloat(unity_SHAg.xyzw), vd_asfloat(r0.xyzw)));
  r3.z = vd_asuint(dot(vd_asfloat(unity_SHAb.xyzw), vd_asfloat(r0.xyzw)));
  r4.xyzw = vd_asuint((vd_asfloat(r0.yzzx) * vd_asfloat(r0.xyzz)));
  r5.x = vd_asuint(dot(vd_asfloat(unity_SHBr.xyzw), vd_asfloat(r4.xyzw)));
  r5.y = vd_asuint(dot(vd_asfloat(unity_SHBg.xyzw), vd_asfloat(r4.xyzw)));
  r5.z = vd_asuint(dot(vd_asfloat(unity_SHBb.xyzw), vd_asfloat(r4.xyzw)));
  r0.w = vd_asuint((vd_asfloat(r0.y) * vd_asfloat(r0.y)));
  r0.w = vd_asuint((vd_asfloat(r0.x) * vd_asfloat(r0.x) + (-(vd_asfloat(r0.w)))));
  r2.yzw = vd_asuint((vd_asfloat(unity_SHC.xyz) * vd_asfloat(r0.www) + vd_asfloat(r5.xyz)));
  r2.yzw = vd_asuint((vd_asfloat(r2.yzw) + vd_asfloat(r3.xyz)));
  r0.w = (vd_asuint(_MainLightLayerMask) & vd_asuint(unity_RenderingLayer.x));
  r3.xyz = vd_asuint((vd_asfloat(_MainLightColor.xyz) * vd_asfloat(unity_LightData.zzz)));
  r3.w = vd_asuint(saturate(dot(vd_asfloat(r0.xyz), vd_asfloat(_MainLightPosition.xyz))));
  r4.x = vd_asuint(((-(vd_asfloat(_ShadeSmoothStart))) + vd_asfloat(_ShadeSmoothEnd)));
  r3.w = vd_asuint((vd_asfloat(r3.w) + (-(vd_asfloat(_ShadeSmoothStart)))));
  r4.x = vd_asuint((1.0 / vd_asfloat(r4.x)));
  r3.w = vd_asuint(saturate((vd_asfloat(r3.w) * vd_asfloat(r4.x))));
  r4.y = vd_asuint((vd_asfloat(r3.w) * -2.0 + 3.0));
  r3.w = vd_asuint((vd_asfloat(r3.w) * vd_asfloat(r3.w)));
  r4.z = vd_asuint((vd_asfloat(r3.w) * vd_asfloat(r4.y)));
  r3.w = vd_asuint(((-(vd_asfloat(r4.y))) * vd_asfloat(r3.w) + 1.0));
  r3.w = vd_asuint((vd_asfloat(_ShadeAlpha) * vd_asfloat(r3.w) + vd_asfloat(r4.z)));
  r3.xyz = vd_asuint((vd_asfloat(r3.www) * vd_asfloat(r3.xyz)));
  r3.xyz = vd_asuint((vd_asfloat(r1.yzw) * vd_asfloat(r3.xyz)));
  r3.xyz = ((r0.www != 0u) ? r3.xyz : uint3(0x00000000u, 0x00000000u, 0x00000000u));
  r0.w = vd_asuint(min(vd_asfloat(_AdditionalLightsCount.x), vd_asfloat(unity_LightData.y)));
  r0.w = vd_asuint(((int)vd_asfloat(r0.w)));
  r4.yzw = uint3(0x00000000u, 0x00000000u, 0x00000000u);
  r3.w = 0x00000000u;
  while (true) {
    r5.x = vd_asuint((-(int)(r3.w >= r0.w)));
    if (r5.x != 0u) break;
    r5.x = (r3.w >> (uint)2);
    r5.y = (r3.w & 0x00000003u);
    r5.x = vd_asuint(dot(vd_asfloat(unity_LightIndices[(int)(r5.x)].xyzw), vd_asfloat(icb[r5.y + 0].xyzw)));
    r5.x = vd_asuint(((int)vd_asfloat(r5.x)));
    r5.yzw = vd_asuint(((-(vd_asfloat(v1.xyz))) * vd_asfloat(_AdditionalLightsPosition[(int)(r5.x)].www) + vd_asfloat(_AdditionalLightsPosition[(int)(r5.x)].xyz)));
    r6.x = vd_asuint(dot(vd_asfloat(r5.yzw), vd_asfloat(r5.yzw)));
    r6.x = vd_asuint(max(vd_asfloat(r6.x), 6.10351562e-05));
    r6.y = vd_asuint(rsqrt(vd_asfloat(r6.x)));
    r5.yzw = vd_asuint((vd_asfloat(r5.yzw) * vd_asfloat(r6.yyy)));
    r6.y = vd_asuint((1.0 / vd_asfloat(r6.x)));
    r6.x = vd_asuint((vd_asfloat(r6.x) * vd_asfloat(_AdditionalLightsAttenuation[(int)(r5.x)].x)));
    r6.x = vd_asuint(((-(vd_asfloat(r6.x))) * vd_asfloat(r6.x) + 1.0));
    r6.x = vd_asuint(max(vd_asfloat(r6.x), 0.0));
    r6.x = vd_asuint((vd_asfloat(r6.x) * vd_asfloat(r6.x)));
    r6.x = vd_asuint((vd_asfloat(r6.x) * vd_asfloat(r6.y)));
    r6.y = vd_asuint(dot(vd_asfloat(_AdditionalLightsSpotDir[(int)(r5.x)].xyz), vd_asfloat(r5.yzw)));
    r6.y = vd_asuint(saturate((vd_asfloat(r6.y) * vd_asfloat(_AdditionalLightsAttenuation[(int)(r5.x)].z) + vd_asfloat(_AdditionalLightsAttenuation[(int)(r5.x)].w))));
    r6.y = vd_asuint((vd_asfloat(r6.y) * vd_asfloat(r6.y)));
    r6.x = vd_asuint((vd_asfloat(r6.y) * vd_asfloat(r6.x)));
    r6.y = (vd_asuint(unity_RenderingLayer.x) & vd_asuint(_AdditionalLightsLayerMasks[(int)(r5.x)]));
    r6.xzw = vd_asuint((vd_asfloat(r6.xxx) * vd_asfloat(_AdditionalLightsColor[(int)(r5.x)].xyz)));
    r5.x = vd_asuint(saturate(dot(vd_asfloat(r0.xyz), vd_asfloat(r5.yzw))));
    r5.x = vd_asuint((vd_asfloat(r5.x) + (-(vd_asfloat(_ShadeSmoothStart)))));
    r5.x = vd_asuint(saturate((vd_asfloat(r4.x) * vd_asfloat(r5.x))));
    r5.y = vd_asuint((vd_asfloat(r5.x) * -2.0 + 3.0));
    r5.x = vd_asuint((vd_asfloat(r5.x) * vd_asfloat(r5.x)));
    r5.z = vd_asuint((vd_asfloat(r5.x) * vd_asfloat(r5.y)));
    r5.x = vd_asuint(((-(vd_asfloat(r5.y))) * vd_asfloat(r5.x) + 1.0));
    r5.x = vd_asuint((vd_asfloat(_ShadeAlpha) * vd_asfloat(r5.x) + vd_asfloat(r5.z)));
    r5.xyz = vd_asuint((vd_asfloat(r5.xxx) * vd_asfloat(r6.xzw)));
    r5.xyz = vd_asuint((vd_asfloat(r5.xyz) * vd_asfloat(r1.yzw) + vd_asfloat(r4.yzw)));
    r4.yzw = ((r6.yyy != 0u) ? r5.xyz : r4.yzw);
    r3.w = vd_asuint((vd_asint(r3.w) + 1));
  }
  r0.xyz = vd_asuint((vd_asfloat(r2.yzw) * vd_asfloat(r1.yzw) + vd_asfloat(r3.xyz)));
  r0.xyz = vd_asuint((vd_asfloat(r4.yzw) + vd_asfloat(r0.xyz)));
  r0.w = vd_asuint(((-(vd_asfloat(r2.x))) + 1.0));
  r1.yzw = vd_asuint((vd_asfloat(r0.www) * vd_asfloat(unity_FogColor.xyz)));
  o0.xyz = (vd_asfloat(r0.xyz) * vd_asfloat(r2.xxx) + vd_asfloat(r1.yzw));
  r0.x = vd_asuint((-(int)(vd_asfloat(_Surface) == 1.0)));
  o0.w = vd_asfloat(((r0.x != 0u) ? r1.x : 0x3f800000u));
  return;
}
