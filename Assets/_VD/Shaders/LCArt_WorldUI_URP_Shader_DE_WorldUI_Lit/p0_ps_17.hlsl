// LCArt/WorldUI/URP/Shader_DE_WorldUI_Lit | pass 0 "Forward" | progFragment | keywords: UNITY_UI_ALPHACLIP _CLUSTER_LIGHT_LOOP | blob 17
void frag(
  float4 v0 : SV_POSITION0,
  float4 v1 : VDREG1,
  float4 v2 : VDREG2,
  float4 v3 : VDREG3,
  float4 v4 : VDREG4,
  float4 v5 : VDREG5,
  out float4 o0 : SV_Target0)
{
  uint4 r0, r1, r2, r3, r4, r5, r6, r7, r8, r9;
  r0 = 0u; r1 = 0u; r2 = 0u; r3 = 0u; r4 = 0u; r5 = 0u; r6 = 0u; r7 = 0u; r8 = 0u; r9 = 0u;

  r0.xyzw = vd_asuint(_MainTex.SampleBias(sampler_MainTex, vd_asfloat(v2.xy), vd_asfloat(_GlobalMipBias.x)).xyzw);
  r1.x = vd_asuint((vd_asfloat(r0.w) + -0.00100000005));
  r1.x = vd_asuint((-(int)(vd_asfloat(r1.x) < 0.0)));
  if (r1.x != 0u) discard;
  r1.x = vd_asuint(dot(vd_asfloat(v4.xyz), vd_asfloat(v4.xyz)));
  r1.x = vd_asuint(max(vd_asfloat(r1.x), 6.10351562e-05));
  r1.x = vd_asuint(rsqrt(vd_asfloat(r1.x)));
  r1.xyz = vd_asuint((vd_asfloat(r1.xxx) * vd_asfloat(v4.xyz)));
  r2.xy = vd_asuint((float2(1.0, 1.0) / vd_asfloat(_ScaledScreenParams.xy)));
  r2.xy = vd_asuint((vd_asfloat(r2.xy) * vd_asfloat(v0.xy)));
  r1.w = vd_asuint((vd_asfloat(r2.y) * vd_asfloat(_ScaleBiasRt.x) + vd_asfloat(_ScaleBiasRt.y)));
  r2.z = vd_asuint(((-(vd_asfloat(r1.w))) + 1.0));
  r1.w = vd_asuint(dot(vd_asfloat(r1.xyz), vd_asfloat(_MainLightPosition.xyz)));
  r1.w = vd_asuint(saturate((vd_asfloat(r1.w) * 0.5 + 0.5)));
  r2.xy = vd_asuint((vd_asfloat(r2.xz) * vd_asfloat(_FPParams1.xy)));
  r2.xy = ((uint2)vd_asfloat(r2.xy));
  r2.zw = ((uint2)vd_asfloat(_FPParams1.zw));
  r2.x = vd_asuint((vd_asint(r2.y) * vd_asint(r2.z) + vd_asint(r2.x)));
  r2.x = vd_asuint((vd_asint(r2.w) * vd_asint(r2.x)));
  r3.x = vd_asuint((-(vd_asfloat((VD_COL(unity_MatrixV, 0).z)))));
  r3.y = vd_asuint((-(vd_asfloat((VD_COL(unity_MatrixV, 1).z)))));
  r3.z = vd_asuint((-(vd_asfloat((VD_COL(unity_MatrixV, 2).z)))));
  r4.xyz = vd_asuint((vd_asfloat(v3.xyz) + (-(vd_asfloat(_WorldSpaceCameraPos.xyz)))));
  r2.y = vd_asuint(dot(vd_asfloat(r3.xyz), vd_asfloat(r4.xyz)));
  r2.z = vd_asuint((-(int)(vd_asfloat(unity_OrthoParams.w) == 0.0)));
  r3.x = vd_asuint(log2(vd_asfloat(r2.y)));
  r2.y = ((r2.z != 0u) ? r3.x : r2.y);
  r2.y = vd_asuint((vd_asfloat(r2.y) * vd_asfloat(_FPParams0.x) + vd_asfloat(_FPParams0.y)));
  r2.y = ((uint)vd_asfloat(r2.y));
  r2.z = ((uint)vd_asfloat(_FPParams2.x));
  r2.z = vd_asuint((vd_asint(r2.z) + -1));
  r2.w = vd_asuint((vd_asint(r2.w) + 2));
  r2.y = min(r2.z, r2.y);
  r2.z = vd_asuint((vd_asint(r2.w) * vd_asint(r2.y)));
  r2.y = vd_asuint((vd_asint(r2.y) * vd_asint(r2.w) + 2));
  r2.w = (r2.z >> (uint)2);
  r3.x = (r2.z << (uint)31);
  r3.y = (r2.z << (uint)30);
  r3.xy = vd_asuint((vd_asint(r3.xy) >> 31));
  r3.zw = (r3.xx & vd_asuint(urp_ZBins[(int)(r2.w)].wy));
  r4.xy = (~r3.xy);
  r2.zw = (r4.xx & vd_asuint(urp_ZBins[(int)(r2.w)].zx));
  r2.zw = (r2.zw | r3.zw);
  r2.z = (r3.y & r2.z);
  r2.w = (r4.y & r2.w);
  r2.z = (r2.w | r2.z);
  r2.w = ((uint)vd_asfloat(_FPParams0.w));
  r3.z = 0x00000000u;
  r3.xyw = r2.xyz;
  r4.xyz = uint3(0x00000000u, 0x00000000u, 0x00000000u);
  while (true) {
    r4.w = (r3.w >> (uint)16);
    r5.xy = r3.zw;
    while (true) {
      r5.z = vd_asuint((-(int)(vd_asint(r5.x) == 0)));
      r5.w = (r5.y & 0x0000ffffu);
      r5.w = vd_asuint((-(int)(r4.w >= r5.w)));
      r5.z = (r5.w & r5.z);
      if (r5.z == 0u) break;
      r5.z = (r5.y >> (uint)5);
      r5.z = (r5.z & 0x000007ffu);
      r6.xyzw = vd_asuint((vd_asint(r3.xxyy) + vd_asint(r5.zzzz)));
      r7.xy = (r6.yw >> (uint2)2);
      r8.xz = (r6.xz << (uint2)31);
      r8.yw = (r6.yw << (uint2)30);
      r6.xyzw = vd_asuint((vd_asint(r8.xyzw) >> 31));
      r7.zw = (r6.xx & vd_asuint(urp_Tiles[(int)(r7.x)].wy));
      r8.xyzw = (~r6.xyzw);
      r9.xy = (r8.xx & vd_asuint(urp_Tiles[(int)(r7.x)].zx));
      r7.xz = (r7.zw | r9.xy);
      r5.w = (r6.y & r7.x);
      r6.x = (r8.y & r7.z);
      r5.w = (r5.w | r6.x);
      r6.xy = (r6.zz & vd_asuint(urp_ZBins[(int)(r7.y)].wy));
      r7.xy = (r8.zz & vd_asuint(urp_ZBins[(int)(r7.y)].zx));
      r6.xy = (r6.xy | r7.xy);
      r6.x = (r6.w & r6.x);
      r6.y = (r8.w & r6.y);
      r6.x = (r6.y | r6.x);
      r5.w = (r5.w & r6.x);
      r6.x = (0xffffffffu << (uint)(int)(r5.y & 31u));
      r5.w = (r5.w & r6.x);
      r5.z = (r5.z << (uint)5);
      r5.z = vd_asuint((vd_asint(r4.w) + (-(vd_asint(r5.z)))));
      r5.z = min(r5.z, 0x0000001fu);
      r5.z = vd_asuint(((-(vd_asint(r5.z))) + 31));
      r5.z = (0xffffffffu >> (uint)(int)(r5.z & 31u));
      r5.x = (r5.z & r5.w);
      r5.z = vd_asuint((vd_asint(r5.y) + 32));
      r5.y = (r5.z & 0xffffffe0u);
    }
    r3.w = r5.y;
    if (r5.x == 0u) break;
    r4.w = vd_asuint((-vd_asint(r5.x)));
    r4.w = (r4.w & r5.x);
    r4.w = vd_asuint(((float)r4.w));
    r4.w = (r4.w >> (uint)23);
    r4.w = vd_asuint((vd_asint(r4.w) + -127));
    r5.z = (0x00000001u << (uint)(int)(r4.w & 31u));
    r3.z = (r5.z ^ r5.x);
    r5.z = vd_asuint((vd_asint(r3.w) + -32));
    r5.z = (r5.z & 0x0000ffe0u);
    r4.w = vd_asuint((vd_asint(r4.w) + vd_asint(r5.z)));
    r4.w = vd_asuint((vd_asint(r2.w) + vd_asint(r4.w)));
    r6.xyz = vd_asuint(((-(vd_asfloat(v3.xyz))) * vd_asfloat(_AdditionalLightsPosition[(int)(r4.w)].www) + vd_asfloat(_AdditionalLightsPosition[(int)(r4.w)].xyz)));
    r5.z = vd_asuint(dot(vd_asfloat(r6.xyz), vd_asfloat(r6.xyz)));
    r5.z = vd_asuint(max(vd_asfloat(r5.z), 6.10351562e-05));
    r5.w = vd_asuint(rsqrt(vd_asfloat(r5.z)));
    r6.xyz = vd_asuint((vd_asfloat(r5.www) * vd_asfloat(r6.xyz)));
    r5.w = vd_asuint((1.0 / vd_asfloat(r5.z)));
    r5.z = vd_asuint((vd_asfloat(r5.z) * vd_asfloat(_AdditionalLightsAttenuation[(int)(r4.w)].x)));
    r5.z = vd_asuint(((-(vd_asfloat(r5.z))) * vd_asfloat(r5.z) + 1.0));
    r5.z = vd_asuint(max(vd_asfloat(r5.z), 0.0));
    r5.z = vd_asuint((vd_asfloat(r5.z) * vd_asfloat(r5.z)));
    r5.z = vd_asuint((vd_asfloat(r5.z) * vd_asfloat(r5.w)));
    r5.w = vd_asuint(dot(vd_asfloat(_AdditionalLightsSpotDir[(int)(r4.w)].xyz), vd_asfloat(r6.xyz)));
    r5.w = vd_asuint(saturate((vd_asfloat(r5.w) * vd_asfloat(_AdditionalLightsAttenuation[(int)(r4.w)].z) + vd_asfloat(_AdditionalLightsAttenuation[(int)(r4.w)].w))));
    r5.w = vd_asuint((vd_asfloat(r5.w) * vd_asfloat(r5.w)));
    r5.z = vd_asuint((vd_asfloat(r5.w) * vd_asfloat(r5.z)));
    r5.z = vd_asuint(sqrt(vd_asfloat(r5.z)));
    r5.z = vd_asuint(min(vd_asfloat(r5.z), 1.0));
    r7.xyz = vd_asuint((vd_asfloat(r5.zzz) * vd_asfloat(_AdditionalLightsColor[(int)(r4.w)].xyz)));
    r4.w = vd_asuint(dot(vd_asfloat(r1.xyz), vd_asfloat(r6.xyz)));
    r4.w = vd_asuint(saturate((vd_asfloat(r4.w) * 0.5 + 0.5)));
    r4.xyz = vd_asuint((vd_asfloat(r7.xyz) * vd_asfloat(r4.www) + vd_asfloat(r4.xyz)));
  }
  r1.xyz = vd_asuint((vd_asfloat(_MainLightColor.xyz) * vd_asfloat(r1.www) + vd_asfloat(r4.xyz)));
  o0.xyz = (vd_asfloat(r0.xyz) * vd_asfloat(r1.xyz));
  o0.w = (vd_asfloat(r0.w) * vd_asfloat(v1.w));
  return;
}
