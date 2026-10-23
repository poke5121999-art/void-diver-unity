// LCArt/TMPFont/Shader_DE_TMP_SDF_Lit | pass 0 "TextMeshPro SDF" | progFragment | keywords: UNDERLAY_ON UNITY_UI_ALPHACLIP UNITY_UI_CLIP_RECT _ADDITIONAL_LIGHTS | blob 24
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
  float4 v9 : VDREG9,
  float4 v10 : VDREG10,
  out float4 o0 : SV_Target0)
{
  uint4 r0, r1, r2, r3, r4, r5;
  r0 = 0u; r1 = 0u; r2 = 0u; r3 = 0u; r4 = 0u; r5 = 0u;
  const uint4 icb[4] = { uint4(0x3f800000u, 0x00000000u, 0x00000000u, 0x00000000u), uint4(0x00000000u, 0x3f800000u, 0x00000000u, 0x00000000u), uint4(0x00000000u, 0x00000000u, 0x3f800000u, 0x00000000u), uint4(0x00000000u, 0x00000000u, 0x00000000u, 0x3f800000u) };

  r0.xyzw = vd_asuint(_MainTex.SampleBias(sampler_MainTex, vd_asfloat(v2.xy), vd_asfloat(_GlobalMipBias.x)).xyzw);
  r0.x = vd_asuint(((-(vd_asfloat(r0.w))) + vd_asfloat(v3.z)));
  r0.y = vd_asuint((vd_asfloat(_OutlineWidth) * vd_asfloat(_ScaleRatioA)));
  r0.z = vd_asuint((vd_asfloat(_OutlineSoftness) * vd_asfloat(_ScaleRatioA)));
  r0.yw = vd_asuint((vd_asfloat(r0.yz) * vd_asfloat(v3.yy)));
  r1.xyz = vd_asuint((vd_asfloat(v1.xyz) * vd_asfloat(_FaceColor.xyz)));
  r2.xy = vd_asuint((vd_asfloat(float2(_FaceUVSpeedX, _FaceUVSpeedY)) * vd_asfloat(_Time.yy) + vd_asfloat(v8.xy)));
  r2.xyzw = vd_asuint(_FaceTex.SampleBias(sampler_FaceTex, vd_asfloat(r2.xy), vd_asfloat(_GlobalMipBias.x)).xyzw);
  r1.xyz = vd_asuint((vd_asfloat(r1.xyz) * vd_asfloat(r2.xyz)));
  r2.w = vd_asuint((vd_asfloat(r2.w) * vd_asfloat(_FaceColor.w)));
  r3.xy = vd_asuint((vd_asfloat(float2(_OutlineUVSpeedX, _OutlineUVSpeedY)) * vd_asfloat(_Time.yy) + vd_asfloat(v8.zw)));
  r3.xyzw = vd_asuint(_OutlineTex.SampleBias(sampler_OutlineTex, vd_asfloat(r3.xy), vd_asfloat(_GlobalMipBias.x)).xyzw);
  r3.xyzw = vd_asuint((vd_asfloat(r3.xyzw) * vd_asfloat(_OutlineColor.xyzw)));
  r1.w = vd_asuint((vd_asfloat(r0.y) * 0.5));
  r4.x = vd_asuint((vd_asfloat(r0.x) * vd_asfloat(v3.y) + (-(vd_asfloat(r1.w)))));
  r0.w = vd_asuint((vd_asfloat(r0.w) * 0.5 + vd_asfloat(r4.x)));
  r0.z = vd_asuint((vd_asfloat(r0.z) * vd_asfloat(v3.y) + 1.0));
  r0.z = vd_asuint(saturate((vd_asfloat(r0.w) / vd_asfloat(r0.z))));
  r0.z = vd_asuint(((-(vd_asfloat(r0.z))) + 1.0));
  r0.x = vd_asuint(saturate((vd_asfloat(r0.x) * vd_asfloat(v3.y) + vd_asfloat(r1.w))));
  r0.y = vd_asuint(min(vd_asfloat(r0.y), 1.0));
  r0.y = vd_asuint(sqrt(vd_asfloat(r0.y)));
  r0.x = vd_asuint((vd_asfloat(r0.y) * vd_asfloat(r0.x)));
  r2.xyz = vd_asuint((vd_asfloat(r1.xyz) * vd_asfloat(r2.www)));
  r3.xyz = vd_asuint((vd_asfloat(r3.www) * vd_asfloat(r3.xyz)));
  r1.xyzw = vd_asuint(((-(vd_asfloat(r2.xyzw))) + vd_asfloat(r3.xyzw)));
  r1.xyzw = vd_asuint((vd_asfloat(r0.xxxx) * vd_asfloat(r1.xyzw) + vd_asfloat(r2.xyzw)));
  r2.xyzw = vd_asuint((vd_asfloat(r0.zzzz) * vd_asfloat(r1.xyzw)));
  r3.xyzw = vd_asuint(_MainTex.SampleBias(sampler_MainTex, vd_asfloat(v6.xy), vd_asfloat(_GlobalMipBias.x)).xyzw);
  r0.x = vd_asuint(saturate((vd_asfloat(r3.w) * vd_asfloat(v6.z) + (-(vd_asfloat(v6.w))))));
  r3.xyzw = vd_asuint((vd_asfloat(r0.xxxx) * vd_asfloat(v7.xyzw)));
  r0.x = vd_asuint(((-(vd_asfloat(r1.w))) * vd_asfloat(r0.z) + 1.0));
  r0.xyzw = vd_asuint((vd_asfloat(r3.xyzw) * vd_asfloat(r0.xxxx) + vd_asfloat(r2.xyzw)));
  r1.xy = vd_asuint(((-(vd_asfloat(_ClipRect.xy))) + vd_asfloat(_ClipRect.zw)));
  r1.xy = vd_asuint((vd_asfloat(r1.xy) + (-(abs(vd_asfloat(v4.xy))))));
  r1.xy = vd_asuint(saturate((vd_asfloat(r1.xy) * vd_asfloat(v4.zw))));
  r1.x = vd_asuint((vd_asfloat(r1.y) * vd_asfloat(r1.x)));
  r2.xyzw = vd_asuint((vd_asfloat(r0.xyzw) * vd_asfloat(r1.xxxx)));
  r0.x = vd_asuint((vd_asfloat(r0.w) * vd_asfloat(r1.x) + -0.00100000005));
  r0.x = vd_asuint((-(int)(vd_asfloat(r0.x) < 0.0)));
  if (r0.x != 0u) discard;
  r0.x = vd_asuint(dot(vd_asfloat(v9.xyz), vd_asfloat(v9.xyz)));
  r0.x = vd_asuint(max(vd_asfloat(r0.x), 1.17549435e-38));
  r0.x = vd_asuint(rsqrt(vd_asfloat(r0.x)));
  r0.xyz = vd_asuint((vd_asfloat(r0.xxx) * vd_asfloat(v9.xyz)));
  r0.w = vd_asuint(sqrt(vd_asfloat(unity_LightData.z)));
  r0.w = vd_asuint(min(vd_asfloat(r0.w), 1.0));
  r1.xyz = vd_asuint((vd_asfloat(r0.www) * vd_asfloat(_MainLightColor.xyz)));
  r0.w = vd_asuint(dot(vd_asfloat(r0.xyz), vd_asfloat(_MainLightPosition.xyz)));
  r0.w = vd_asuint(saturate((vd_asfloat(r0.w) * 0.5 + 0.5)));
  r1.w = vd_asuint(min(vd_asfloat(_AdditionalLightsCount.x), vd_asfloat(unity_LightData.y)));
  r1.w = vd_asuint(((int)vd_asfloat(r1.w)));
  r3.xyzw = uint4(0x00000000u, 0x00000000u, 0x00000000u, 0x00000000u);
  while (true) {
    r4.x = vd_asuint((-(int)(r3.w >= r1.w)));
    if (r4.x != 0u) break;
    r4.x = (r3.w >> (uint)2);
    r4.y = (r3.w & 0x00000003u);
    r4.x = vd_asuint(dot(vd_asfloat(unity_LightIndices[(int)(r4.x)].xyzw), vd_asfloat(icb[r4.y + 0].xyzw)));
    r4.x = vd_asuint(((int)vd_asfloat(r4.x)));
    r4.yzw = vd_asuint(((-(vd_asfloat(v10.xyz))) * vd_asfloat(_AdditionalLightsPosition[(int)(r4.x)].www) + vd_asfloat(_AdditionalLightsPosition[(int)(r4.x)].xyz)));
    r5.x = vd_asuint(dot(vd_asfloat(r4.yzw), vd_asfloat(r4.yzw)));
    r5.x = vd_asuint(max(vd_asfloat(r5.x), 6.10351562e-05));
    r5.y = vd_asuint(rsqrt(vd_asfloat(r5.x)));
    r4.yzw = vd_asuint((vd_asfloat(r4.yzw) * vd_asfloat(r5.yyy)));
    r5.y = vd_asuint((1.0 / vd_asfloat(r5.x)));
    r5.x = vd_asuint((vd_asfloat(r5.x) * vd_asfloat(_AdditionalLightsAttenuation[(int)(r4.x)].x)));
    r5.x = vd_asuint(((-(vd_asfloat(r5.x))) * vd_asfloat(r5.x) + 1.0));
    r5.x = vd_asuint(max(vd_asfloat(r5.x), 0.0));
    r5.x = vd_asuint((vd_asfloat(r5.x) * vd_asfloat(r5.x)));
    r5.x = vd_asuint((vd_asfloat(r5.x) * vd_asfloat(r5.y)));
    r5.y = vd_asuint(dot(vd_asfloat(_AdditionalLightsSpotDir[(int)(r4.x)].xyz), vd_asfloat(r4.yzw)));
    r5.y = vd_asuint(saturate((vd_asfloat(r5.y) * vd_asfloat(_AdditionalLightsAttenuation[(int)(r4.x)].z) + vd_asfloat(_AdditionalLightsAttenuation[(int)(r4.x)].w))));
    r5.y = vd_asuint((vd_asfloat(r5.y) * vd_asfloat(r5.y)));
    r5.x = vd_asuint((vd_asfloat(r5.y) * vd_asfloat(r5.x)));
    r5.x = vd_asuint(sqrt(vd_asfloat(r5.x)));
    r5.x = vd_asuint(min(vd_asfloat(r5.x), 1.0));
    r5.xyz = vd_asuint((vd_asfloat(r5.xxx) * vd_asfloat(_AdditionalLightsColor[(int)(r4.x)].xyz)));
    r4.x = vd_asuint(dot(vd_asfloat(r0.xyz), vd_asfloat(r4.yzw)));
    r4.x = vd_asuint(saturate((vd_asfloat(r4.x) * 0.5 + 0.5)));
    r3.xyz = vd_asuint((vd_asfloat(r5.xyz) * vd_asfloat(r4.xxx) + vd_asfloat(r3.xyz)));
    r3.w = vd_asuint((vd_asint(r3.w) + 1));
  }
  r0.xyz = vd_asuint((vd_asfloat(r1.xyz) * vd_asfloat(r0.www) + vd_asfloat(r3.xyz)));
  r2.xyz = vd_asuint((vd_asfloat(r0.xyz) * vd_asfloat(r2.xyz)));
  o0.xyzw = (vd_asfloat(r2.xyzw) * vd_asfloat(v1.wwww));
  return;
}
