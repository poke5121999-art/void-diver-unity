// LCArt/Temp/Shader_CQ_Background | pass 0 "ForwardLit" | progFragment | keywords: _ADDITIONAL_LIGHTS _LIGHT_LAYERS | blob 27
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
  r0.w = 0x3f800000u;
  r2.x = vd_asuint(dot(vd_asfloat(unity_SHAr.xyzw), vd_asfloat(r0.xyzw)));
  r2.y = vd_asuint(dot(vd_asfloat(unity_SHAg.xyzw), vd_asfloat(r0.xyzw)));
  r2.z = vd_asuint(dot(vd_asfloat(unity_SHAb.xyzw), vd_asfloat(r0.xyzw)));
  r3.xyzw = vd_asuint((vd_asfloat(r0.yzzx) * vd_asfloat(r0.xyzz)));
  r4.x = vd_asuint(dot(vd_asfloat(unity_SHBr.xyzw), vd_asfloat(r3.xyzw)));
  r4.y = vd_asuint(dot(vd_asfloat(unity_SHBg.xyzw), vd_asfloat(r3.xyzw)));
  r4.z = vd_asuint(dot(vd_asfloat(unity_SHBb.xyzw), vd_asfloat(r3.xyzw)));
  r0.w = vd_asuint((vd_asfloat(r0.y) * vd_asfloat(r0.y)));
  r0.w = vd_asuint((vd_asfloat(r0.x) * vd_asfloat(r0.x) + (-(vd_asfloat(r0.w)))));
  r3.xyz = vd_asuint((vd_asfloat(unity_SHC.xyz) * vd_asfloat(r0.www) + vd_asfloat(r4.xyz)));
  r2.xyz = vd_asuint((vd_asfloat(r2.xyz) + vd_asfloat(r3.xyz)));
  r0.w = (vd_asuint(_MainLightLayerMask) & vd_asuint(unity_RenderingLayer.x));
  r3.xyz = vd_asuint((vd_asfloat(_MainLightColor.xyz) * vd_asfloat(unity_LightData.zzz)));
  r2.w = vd_asuint(saturate(dot(vd_asfloat(r0.xyz), vd_asfloat(_MainLightPosition.xyz))));
  r3.w = vd_asuint(((-(vd_asfloat(_ShadeSmoothStart))) + vd_asfloat(_ShadeSmoothEnd)));
  r2.w = vd_asuint((vd_asfloat(r2.w) + (-(vd_asfloat(_ShadeSmoothStart)))));
  r3.w = vd_asuint((1.0 / vd_asfloat(r3.w)));
  r2.w = vd_asuint(saturate((vd_asfloat(r2.w) * vd_asfloat(r3.w))));
  r4.x = vd_asuint((vd_asfloat(r2.w) * -2.0 + 3.0));
  r2.w = vd_asuint((vd_asfloat(r2.w) * vd_asfloat(r2.w)));
  r4.y = vd_asuint((vd_asfloat(r2.w) * vd_asfloat(r4.x)));
  r2.w = vd_asuint(((-(vd_asfloat(r4.x))) * vd_asfloat(r2.w) + 1.0));
  r2.w = vd_asuint((vd_asfloat(_ShadeAlpha) * vd_asfloat(r2.w) + vd_asfloat(r4.y)));
  r3.xyz = vd_asuint((vd_asfloat(r2.www) * vd_asfloat(r3.xyz)));
  r3.xyz = vd_asuint((vd_asfloat(r1.yzw) * vd_asfloat(r3.xyz)));
  r3.xyz = ((r0.www != 0u) ? r3.xyz : uint3(0x00000000u, 0x00000000u, 0x00000000u));
  r0.w = vd_asuint(min(vd_asfloat(_AdditionalLightsCount.x), vd_asfloat(unity_LightData.y)));
  r0.w = vd_asuint(((int)vd_asfloat(r0.w)));
  r4.xyz = uint3(0x00000000u, 0x00000000u, 0x00000000u);
  r2.w = 0x00000000u;
  while (true) {
    r4.w = vd_asuint((-(int)(r2.w >= r0.w)));
    if (r4.w != 0u) break;
    r4.w = (r2.w >> (uint)2);
    r5.x = (r2.w & 0x00000003u);
    r4.w = vd_asuint(dot(vd_asfloat(unity_LightIndices[(int)(r4.w)].xyzw), vd_asfloat(icb[r5.x + 0].xyzw)));
    r4.w = vd_asuint(((int)vd_asfloat(r4.w)));
    r5.xyz = vd_asuint(((-(vd_asfloat(v1.xyz))) * vd_asfloat(_AdditionalLightsPosition[(int)(r4.w)].www) + vd_asfloat(_AdditionalLightsPosition[(int)(r4.w)].xyz)));
    r5.w = vd_asuint(dot(vd_asfloat(r5.xyz), vd_asfloat(r5.xyz)));
    r5.w = vd_asuint(max(vd_asfloat(r5.w), 6.10351562e-05));
    r6.x = vd_asuint(rsqrt(vd_asfloat(r5.w)));
    r5.xyz = vd_asuint((vd_asfloat(r5.xyz) * vd_asfloat(r6.xxx)));
    r6.x = vd_asuint((1.0 / vd_asfloat(r5.w)));
    r5.w = vd_asuint((vd_asfloat(r5.w) * vd_asfloat(_AdditionalLightsAttenuation[(int)(r4.w)].x)));
    r5.w = vd_asuint(((-(vd_asfloat(r5.w))) * vd_asfloat(r5.w) + 1.0));
    r5.w = vd_asuint(max(vd_asfloat(r5.w), 0.0));
    r5.w = vd_asuint((vd_asfloat(r5.w) * vd_asfloat(r5.w)));
    r5.w = vd_asuint((vd_asfloat(r5.w) * vd_asfloat(r6.x)));
    r6.x = vd_asuint(dot(vd_asfloat(_AdditionalLightsSpotDir[(int)(r4.w)].xyz), vd_asfloat(r5.xyz)));
    r6.x = vd_asuint(saturate((vd_asfloat(r6.x) * vd_asfloat(_AdditionalLightsAttenuation[(int)(r4.w)].z) + vd_asfloat(_AdditionalLightsAttenuation[(int)(r4.w)].w))));
    r6.x = vd_asuint((vd_asfloat(r6.x) * vd_asfloat(r6.x)));
    r5.w = vd_asuint((vd_asfloat(r5.w) * vd_asfloat(r6.x)));
    r6.x = (vd_asuint(unity_RenderingLayer.x) & vd_asuint(_AdditionalLightsLayerMasks[(int)(r4.w)]));
    r6.yzw = vd_asuint((vd_asfloat(r5.www) * vd_asfloat(_AdditionalLightsColor[(int)(r4.w)].xyz)));
    r4.w = vd_asuint(saturate(dot(vd_asfloat(r0.xyz), vd_asfloat(r5.xyz))));
    r4.w = vd_asuint((vd_asfloat(r4.w) + (-(vd_asfloat(_ShadeSmoothStart)))));
    r4.w = vd_asuint(saturate((vd_asfloat(r3.w) * vd_asfloat(r4.w))));
    r5.x = vd_asuint((vd_asfloat(r4.w) * -2.0 + 3.0));
    r4.w = vd_asuint((vd_asfloat(r4.w) * vd_asfloat(r4.w)));
    r5.y = vd_asuint((vd_asfloat(r4.w) * vd_asfloat(r5.x)));
    r4.w = vd_asuint(((-(vd_asfloat(r5.x))) * vd_asfloat(r4.w) + 1.0));
    r4.w = vd_asuint((vd_asfloat(_ShadeAlpha) * vd_asfloat(r4.w) + vd_asfloat(r5.y)));
    r5.xyz = vd_asuint((vd_asfloat(r4.www) * vd_asfloat(r6.yzw)));
    r5.xyz = vd_asuint((vd_asfloat(r5.xyz) * vd_asfloat(r1.yzw) + vd_asfloat(r4.xyz)));
    r4.xyz = ((r6.xxx != 0u) ? r5.xyz : r4.xyz);
    r2.w = vd_asuint((vd_asint(r2.w) + 1));
  }
  r0.xyz = vd_asuint((vd_asfloat(r2.xyz) * vd_asfloat(r1.yzw) + vd_asfloat(r3.xyz)));
  o0.xyz = (vd_asfloat(r4.xyz) + vd_asfloat(r0.xyz));
  r0.x = vd_asuint((-(int)(vd_asfloat(_Surface) == 1.0)));
  o0.w = vd_asfloat(((r0.x != 0u) ? r1.x : 0x3f800000u));
  return;
}
