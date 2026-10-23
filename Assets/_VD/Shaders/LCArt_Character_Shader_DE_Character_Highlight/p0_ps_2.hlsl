// LCArt/Character/Shader_DE_Character_Highlight | pass 0 "Glowline" | progFragment | keywords: (none) | blob 2
void frag(
  float4 v0 : SV_POSITION0,
  float4 v1 : VDREG1,
  float4 v2 : VDREG2,
  out float4 o0 : SV_Target0)
{
  uint4 r0, r1;
  r0 = 0u; r1 = 0u;
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
  r0.x = vd_asuint((vd_asfloat(r0.w) * vd_asfloat(v1.w) + -0.100000001));
  r0.x = vd_asuint((-(int)(vd_asfloat(r0.x) < 0.0)));
  if (r0.x != 0u) discard;
  r0.xy = vd_asuint((vd_asfloat(v2.xy) / vd_asfloat(v2.ww)));
  r0.zw = vd_asuint((vd_asfloat(_ScreenSize.xy) / vd_asfloat(_ScreenParams.xy)));
  r0.xy = vd_asuint((vd_asfloat(r0.xy) * vd_asfloat(_ScreenParams.xy)));
  r0.xy = vd_asuint((vd_asfloat(r0.zw) * vd_asfloat(r0.xy)));
  r0.xy = vd_asuint((vd_asfloat(r0.xy) * float2(0.5, 0.5)));
  r0.xy = vd_asuint(floor(vd_asfloat(r0.xy)));
  r0.xy = ((uint2)vd_asfloat(r0.xy));
  r0.xy = (r0.xy & uint2(0x00000003u, 0x00000003u));
  r0.y = (r0.y << (uint)2);
  r0.x = vd_asuint((vd_asint(r0.y) + vd_asint(r0.x)));
  r0.x = x0[r0.x + 0].x;
  r0.x = vd_asuint(((-(vd_asfloat(r0.x))) + vd_asfloat(v1.z)));
  r0.x = vd_asuint((-(int)(vd_asfloat(r0.x) < 0.0)));
  if (r0.x != 0u) discard;
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
  r0.x = vd_asuint((vd_asfloat(_DissolveProgress) + -0.00100000005));
  r0.x = vd_asuint((-(int)(0.0 >= vd_asfloat(r0.x))));
  r0.x = (r0.x & 0x3f800000u);
  r0.y = vd_asuint((vd_asfloat(_Time.y) * 30.0));
  r0.y = vd_asuint(cos(vd_asfloat(r0.y)));
  r0.y = vd_asuint((vd_asfloat(r0.y) * 0.5 + 0.5));
  r1.xyz = vd_asuint(((-(vd_asfloat(_HighlightLineColor.xyz))) + vd_asfloat(_HighlightLineColor1.xyz)));
  r0.yzw = vd_asuint((vd_asfloat(r0.yyy) * vd_asfloat(r1.xyz) + vd_asfloat(_HighlightLineColor.xyz)));
  r0.yzw = vd_asuint((vd_asfloat(r0.yzw) * float3(5.0, 5.0, 5.0)));
  o0.xyz = (vd_asfloat(r0.xxx) * vd_asfloat(r0.yzw));
  o0.w = vd_asfloat(0x3f800000u);
  return;
}
