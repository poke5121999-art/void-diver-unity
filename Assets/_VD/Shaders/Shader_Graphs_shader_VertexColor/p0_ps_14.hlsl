// Shader Graphs/shader_VertexColor | pass 0 "Universal Forward" | progFragment | keywords: FOG_LINEAR _ADDITIONAL_LIGHTS _LIGHT_LAYERS | blob 14
void frag(
  float4 v0 : SV_POSITION0,
  float4 v1 : VDREG1,
  float4 v2 : VDREG2,
  float4 v3 : VDREG3,
  float4 v4 : VDREG4,
  float4 v5 : VDREG5,
  float4 v6 : VDREG6,
  out float4 o0 : SV_Target0)
{
  uint4 r0, r1, r2, r3, r4, r5, r6, r7, r8;
  r0 = 0u; r1 = 0u; r2 = 0u; r3 = 0u; r4 = 0u; r5 = 0u; r6 = 0u; r7 = 0u; r8 = 0u;
  const uint4 icb[4] = { uint4(0x3f800000u, 0x00000000u, 0x00000000u, 0x00000000u), uint4(0x00000000u, 0x3f800000u, 0x00000000u, 0x00000000u), uint4(0x00000000u, 0x00000000u, 0x3f800000u, 0x00000000u), uint4(0x00000000u, 0x00000000u, 0x00000000u, 0x3f800000u) };

  r0.x = vd_asuint(dot(vd_asfloat(v6.xyz), vd_asfloat(v6.xyz)));
  r0.x = vd_asuint(rsqrt(vd_asfloat(r0.x)));
  r0.xyz = vd_asuint((vd_asfloat(r0.xxx) * vd_asfloat(v6.xyz)));
  r1.x = vd_asuint((-(int)(vd_asfloat(unity_OrthoParams.w) == 0.0)));
  r1.yzw = vd_asuint(((-(vd_asfloat(v5.xyz))) + vd_asfloat(_WorldSpaceCameraPos.xyz)));
  r2.x = vd_asuint(dot(vd_asfloat(r1.yzw), vd_asfloat(r1.yzw)));
  r2.x = vd_asuint(rsqrt(vd_asfloat(r2.x)));
  r1.yzw = vd_asuint((vd_asfloat(r1.yzw) * vd_asfloat(r2.xxx)));
  r2.x = vd_asuint((VD_COL(unity_MatrixV, 0).z));
  r2.y = vd_asuint((VD_COL(unity_MatrixV, 1).z));
  r2.z = vd_asuint((VD_COL(unity_MatrixV, 2).z));
  r1.xyz = ((r1.xxx != 0u) ? r1.yzw : r2.xyz);
  r1.w = vd_asuint((vd_asfloat(v5.y) * vd_asfloat((VD_COL(unity_MatrixV, 1).z))));
  r1.w = vd_asuint((vd_asfloat((VD_COL(unity_MatrixV, 0).z)) * vd_asfloat(v5.x) + vd_asfloat(r1.w)));
  r1.w = vd_asuint((vd_asfloat((VD_COL(unity_MatrixV, 2).z)) * vd_asfloat(v5.z) + vd_asfloat(r1.w)));
  r1.w = vd_asuint((vd_asfloat(r1.w) + vd_asfloat((VD_COL(unity_MatrixV, 3).z))));
  r1.w = vd_asuint(((-(vd_asfloat(r1.w))) + (-(vd_asfloat(_ProjectionParams.y)))));
  r1.w = vd_asuint(max(vd_asfloat(r1.w), 0.0));
  r1.w = vd_asuint(saturate((vd_asfloat(r1.w) * vd_asfloat(unity_FogParams.z) + vd_asfloat(unity_FogParams.w))));
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
  r3.xyz = vd_asuint((vd_asfloat(v3.xyz) * float3(0.959999979, 0.959999979, 0.959999979)));
  r0.w = vd_asuint(dot((-(vd_asfloat(r1.xyz))), vd_asfloat(r0.xyz)));
  r0.w = vd_asuint((vd_asfloat(r0.w) + vd_asfloat(r0.w)));
  r4.xyz = vd_asuint((vd_asfloat(r0.xyz) * (-(vd_asfloat(r0.www))) + (-(vd_asfloat(r1.xyz)))));
  r0.w = vd_asuint(saturate(dot(vd_asfloat(r0.xyz), vd_asfloat(r1.xyz))));
  r0.w = vd_asuint(((-(vd_asfloat(r0.w))) + 1.0));
  r0.w = vd_asuint((vd_asfloat(r0.w) * vd_asfloat(r0.w)));
  r0.w = vd_asuint((vd_asfloat(r0.w) * vd_asfloat(r0.w)));
  r4.xyzw = vd_asuint(unity_SpecCube0.SampleLevel(samplerunity_SpecCube0, vd_asfloat(r4.xyz), 4.05000019).xyzw);
  r2.w = vd_asuint((vd_asfloat(r4.w) + -1.0));
  r2.w = vd_asuint((vd_asfloat(unity_SpecCube0_HDR.w) * vd_asfloat(r2.w) + 1.0));
  r2.w = vd_asuint(max(vd_asfloat(r2.w), 0.0));
  r2.w = vd_asuint(log2(vd_asfloat(r2.w)));
  r2.w = vd_asuint((vd_asfloat(r2.w) * vd_asfloat(unity_SpecCube0_HDR.y)));
  r2.w = vd_asuint(exp2(vd_asfloat(r2.w)));
  r2.w = vd_asuint((vd_asfloat(r2.w) * vd_asfloat(unity_SpecCube0_HDR.x)));
  r4.xyz = vd_asuint((vd_asfloat(r4.xyz) * vd_asfloat(r2.www)));
  r0.w = vd_asuint((vd_asfloat(r0.w) * 0.5 + 0.0399999991));
  r0.w = vd_asuint((vd_asfloat(r0.w) * 0.941176474));
  r4.xyz = vd_asuint((vd_asfloat(r0.www) * vd_asfloat(r4.xyz)));
  r2.xyz = vd_asuint((vd_asfloat(r2.xyz) * vd_asfloat(r3.xyz) + vd_asfloat(r4.xyz)));
  r0.w = (vd_asuint(_MainLightLayerMask) & vd_asuint(unity_RenderingLayer.x));
  r2.w = vd_asuint(saturate(dot(vd_asfloat(r0.xyz), vd_asfloat(_MainLightPosition.xyz))));
  r2.w = vd_asuint((vd_asfloat(r2.w) * vd_asfloat(unity_LightData.z)));
  r4.xyz = vd_asuint((vd_asfloat(r2.www) * vd_asfloat(_MainLightColor.xyz)));
  r5.xyz = vd_asuint((vd_asfloat(r1.xyz) + vd_asfloat(_MainLightPosition.xyz)));
  r2.w = vd_asuint(dot(vd_asfloat(r5.xyz), vd_asfloat(r5.xyz)));
  r2.w = vd_asuint(max(vd_asfloat(r2.w), 1.17549435e-38));
  r2.w = vd_asuint(rsqrt(vd_asfloat(r2.w)));
  r5.xyz = vd_asuint((vd_asfloat(r2.www) * vd_asfloat(r5.xyz)));
  r2.w = vd_asuint(saturate(dot(vd_asfloat(r0.xyz), vd_asfloat(r5.xyz))));
  r3.w = vd_asuint(saturate(dot(vd_asfloat(_MainLightPosition.xyz), vd_asfloat(r5.xyz))));
  r2.w = vd_asuint((vd_asfloat(r2.w) * vd_asfloat(r2.w)));
  r2.w = vd_asuint((vd_asfloat(r2.w) * -0.9375 + 1.00001001));
  r3.w = vd_asuint((vd_asfloat(r3.w) * vd_asfloat(r3.w)));
  r2.w = vd_asuint((vd_asfloat(r2.w) * vd_asfloat(r2.w)));
  r3.w = vd_asuint(max(vd_asfloat(r3.w), 0.100000001));
  r2.w = vd_asuint((vd_asfloat(r2.w) * vd_asfloat(r3.w)));
  r2.w = vd_asuint((vd_asfloat(r2.w) * 3.0));
  r2.w = vd_asuint((0.0625 / vd_asfloat(r2.w)));
  r5.xyz = vd_asuint((vd_asfloat(r2.www) * float3(0.0399999991, 0.0399999991, 0.0399999991) + vd_asfloat(r3.xyz)));
  r4.xyz = vd_asuint((vd_asfloat(r4.xyz) * vd_asfloat(r5.xyz)));
  r4.xyz = ((r0.www != 0u) ? r4.xyz : uint3(0x00000000u, 0x00000000u, 0x00000000u));
  r0.w = vd_asuint(min(vd_asfloat(_AdditionalLightsCount.x), vd_asfloat(unity_LightData.y)));
  r0.w = vd_asuint(((int)vd_asfloat(r0.w)));
  r5.xyz = uint3(0x00000000u, 0x00000000u, 0x00000000u);
  r2.w = 0x00000000u;
  while (true) {
    r3.w = vd_asuint((-(int)(r2.w >= r0.w)));
    if (r3.w != 0u) break;
    r3.w = (r2.w >> (uint)2);
    r4.w = (r2.w & 0x00000003u);
    r3.w = vd_asuint(dot(vd_asfloat(unity_LightIndices[(int)(r3.w)].xyzw), vd_asfloat(icb[r4.w + 0].xyzw)));
    r3.w = vd_asuint(((int)vd_asfloat(r3.w)));
    r6.xyz = vd_asuint(((-(vd_asfloat(v5.xyz))) * vd_asfloat(_AdditionalLightsPosition[(int)(r3.w)].www) + vd_asfloat(_AdditionalLightsPosition[(int)(r3.w)].xyz)));
    r4.w = vd_asuint(dot(vd_asfloat(r6.xyz), vd_asfloat(r6.xyz)));
    r4.w = vd_asuint(max(vd_asfloat(r4.w), 6.10351562e-05));
    r5.w = vd_asuint(rsqrt(vd_asfloat(r4.w)));
    r7.xyz = vd_asuint((vd_asfloat(r5.www) * vd_asfloat(r6.xyz)));
    r6.w = vd_asuint((1.0 / vd_asfloat(r4.w)));
    r4.w = vd_asuint((vd_asfloat(r4.w) * vd_asfloat(_AdditionalLightsAttenuation[(int)(r3.w)].x)));
    r4.w = vd_asuint(((-(vd_asfloat(r4.w))) * vd_asfloat(r4.w) + 1.0));
    r4.w = vd_asuint(max(vd_asfloat(r4.w), 0.0));
    r4.w = vd_asuint((vd_asfloat(r4.w) * vd_asfloat(r4.w)));
    r4.w = vd_asuint((vd_asfloat(r4.w) * vd_asfloat(r6.w)));
    r6.w = vd_asuint(dot(vd_asfloat(_AdditionalLightsSpotDir[(int)(r3.w)].xyz), vd_asfloat(r7.xyz)));
    r6.w = vd_asuint(saturate((vd_asfloat(r6.w) * vd_asfloat(_AdditionalLightsAttenuation[(int)(r3.w)].z) + vd_asfloat(_AdditionalLightsAttenuation[(int)(r3.w)].w))));
    r6.w = vd_asuint((vd_asfloat(r6.w) * vd_asfloat(r6.w)));
    r4.w = vd_asuint((vd_asfloat(r4.w) * vd_asfloat(r6.w)));
    r6.w = (vd_asuint(unity_RenderingLayer.x) & vd_asuint(_AdditionalLightsLayerMasks[(int)(r3.w)]));
    r7.w = vd_asuint(saturate(dot(vd_asfloat(r0.xyz), vd_asfloat(r7.xyz))));
    r4.w = vd_asuint((vd_asfloat(r4.w) * vd_asfloat(r7.w)));
    r8.xyz = vd_asuint((vd_asfloat(r4.www) * vd_asfloat(_AdditionalLightsColor[(int)(r3.w)].xyz)));
    r6.xyz = vd_asuint((vd_asfloat(r6.xyz) * vd_asfloat(r5.www) + vd_asfloat(r1.xyz)));
    r3.w = vd_asuint(dot(vd_asfloat(r6.xyz), vd_asfloat(r6.xyz)));
    r3.w = vd_asuint(max(vd_asfloat(r3.w), 1.17549435e-38));
    r3.w = vd_asuint(rsqrt(vd_asfloat(r3.w)));
    r6.xyz = vd_asuint((vd_asfloat(r3.www) * vd_asfloat(r6.xyz)));
    r3.w = vd_asuint(saturate(dot(vd_asfloat(r0.xyz), vd_asfloat(r6.xyz))));
    r4.w = vd_asuint(saturate(dot(vd_asfloat(r7.xyz), vd_asfloat(r6.xyz))));
    r3.w = vd_asuint((vd_asfloat(r3.w) * vd_asfloat(r3.w)));
    r3.w = vd_asuint((vd_asfloat(r3.w) * -0.9375 + 1.00001001));
    r4.w = vd_asuint((vd_asfloat(r4.w) * vd_asfloat(r4.w)));
    r3.w = vd_asuint((vd_asfloat(r3.w) * vd_asfloat(r3.w)));
    r4.w = vd_asuint(max(vd_asfloat(r4.w), 0.100000001));
    r3.w = vd_asuint((vd_asfloat(r3.w) * vd_asfloat(r4.w)));
    r3.w = vd_asuint((vd_asfloat(r3.w) * 3.0));
    r3.w = vd_asuint((0.0625 / vd_asfloat(r3.w)));
    r6.xyz = vd_asuint((vd_asfloat(r3.www) * float3(0.0399999991, 0.0399999991, 0.0399999991) + vd_asfloat(r3.xyz)));
    r6.xyz = vd_asuint((vd_asfloat(r6.xyz) * vd_asfloat(r8.xyz) + vd_asfloat(r5.xyz)));
    r5.xyz = ((r6.www != 0u) ? r6.xyz : r5.xyz);
    r2.w = vd_asuint((vd_asint(r2.w) + 1));
  }
  r0.xyz = vd_asuint((vd_asfloat(r2.xyz) + vd_asfloat(r4.xyz)));
  r0.xyz = vd_asuint((vd_asfloat(r5.xyz) + vd_asfloat(r0.xyz)));
  r0.w = vd_asuint(((-(vd_asfloat(r1.w))) + 1.0));
  r1.xyz = vd_asuint((vd_asfloat(r0.www) * vd_asfloat(unity_FogColor.xyz)));
  o0.xyz = (vd_asfloat(r0.xyz) * vd_asfloat(r1.www) + vd_asfloat(r1.xyz));
  o0.w = vd_asfloat(0x3f800000u);
  return;
}
