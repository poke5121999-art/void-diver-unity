// LCArt/Character/shader_DE_Character_Normal | pass 5 "Reflection" | progFragment | keywords: _DE_PRERENDERING_CAMERA | blob 128
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
  r0.x = vd_asuint((-(int)(vd_asfloat(_GhostEnable) >= 0.00100000005)));
  if (r0.x != 0u) discard;
  r0.x = vd_asuint((-(int)(vd_asfloat(_IsReflected) >= 0.00100000005)));
  if (r0.x == 0u) discard;
  r0.xyzw = vd_asuint(_MainTex.SampleBias(sampler_MainTex, vd_asfloat(v1.xy), vd_asfloat(_GlobalMipBias.x)).xyzw);
  r1.x = vd_asuint((vd_asfloat(r0.w) + -0.100000001));
  r1.x = vd_asuint((-(int)(vd_asfloat(r1.x) < 0.0)));
  if (r1.x != 0u) discard;
  r0.xyz = vd_asuint((vd_asfloat(r0.xyz) + (-(vd_asfloat(_GlobalCharacterOutOfColor.xyz)))));
  r0.xyz = vd_asuint((vd_asfloat(v2.www) * vd_asfloat(r0.xyz) + vd_asfloat(_GlobalCharacterOutOfColor.xyz)));
  r1.xy = vd_asuint((vd_asfloat(v7.xy) / vd_asfloat(v7.ww)));
  r1.z = vd_asuint((vd_asfloat(v2.w) + vd_asfloat(_Effect)));
  r2.xy = vd_asuint((vd_asfloat(_ScreenSize.xy) / vd_asfloat(_ScreenParams.xy)));
  r1.xy = vd_asuint((vd_asfloat(r1.xy) * vd_asfloat(_ScreenParams.xy)));
  r1.xy = vd_asuint((vd_asfloat(r2.xy) * vd_asfloat(r1.xy)));
  r1.xy = vd_asuint((vd_asfloat(r1.xy) * float2(0.5, 0.5)));
  r1.xy = vd_asuint(floor(vd_asfloat(r1.xy)));
  r1.xy = ((uint2)vd_asfloat(r1.xy));
  r1.xy = (r1.xy & uint2(0x00000003u, 0x00000003u));
  r1.y = (r1.y << (uint)2);
  r1.x = vd_asuint((vd_asint(r1.y) + vd_asint(r1.x)));
  r1.y = x0[r1.x + 0].x;
  r1.z = vd_asuint(((-(vd_asfloat(r1.y))) + vd_asfloat(r1.z)));
  r1.z = vd_asuint((-(int)(vd_asfloat(r1.z) < 0.0)));
  if (r1.z != 0u) discard;
  r1.y = vd_asuint((vd_asfloat(r0.w) + (-(vd_asfloat(r1.y)))));
  r1.y = vd_asuint((-(int)(vd_asfloat(r1.y) < 0.0)));
  if (r1.y != 0u) discard;
  r1.y = vd_asuint((-(int)(vd_asfloat(_GlobalBoxAreaEnable) >= 0.5)));
  if (r1.y != 0u) {
    r1.y = 0x00000000u;
    while (true) {
      r1.z = vd_asuint((-(int)(vd_asint(r1.y) >= 8)));
      if (r1.z != 0u) break;
      r1.z = vd_asuint((-(int)(vd_asfloat(_GlobalBoxAreaCoverCharacters[(int)(r1.y)]) < 0.5)));
      if (r1.z != 0u) {
        r1.z = vd_asuint((vd_asint(r1.y) + 1));
        r1.y = r1.z;
        continue;
      }
      r2.xyz = vd_asuint((vd_asfloat((VD_COL(unity_ObjectToWorld, 3).xyz)) + (-(vd_asfloat(_GlobalBoxAreaCenters[(int)(r1.y)].xyz)))));
      r2.xyz = vd_asuint((abs(vd_asfloat(r2.xyz)) + (-(vd_asfloat(_GlobalBoxAreaExtents[(int)(r1.y)].xyz)))));
      r1.z = vd_asuint(max(vd_asfloat(r2.y), vd_asfloat(r2.x)));
      r1.z = vd_asuint(max(vd_asfloat(r2.z), vd_asfloat(r1.z)));
      r1.z = vd_asuint((-(int)(0.0 >= vd_asfloat(r1.z))));
      if (r1.z != 0u) {
        if (0xffffffffu != 0u) discard;
        break;
      }
      r1.y = vd_asuint((vd_asint(r1.y) + 1));
    }
  }
  r1.y = vd_asuint((vd_asfloat(v3.y) + (-(vd_asfloat(_GlobalFogHeightMin)))));
  r1.z = vd_asuint(((-(vd_asfloat(_GlobalFogHeightMin))) + vd_asfloat(_GlobalFogHeightMax)));
  r1.y = vd_asuint((vd_asfloat(r1.y) / vd_asfloat(r1.z)));
  r1.y = vd_asuint(saturate(((-(vd_asfloat(r1.y))) + 1.0)));
  r1.y = vd_asuint((vd_asfloat(r1.y) * vd_asfloat(_GlobalFogHeightEnable)));
  r1.y = vd_asuint(max(vd_asfloat(r1.y), 0.0));
  r2.xyz = vd_asuint(((-(vd_asfloat(r0.xyz))) + vd_asfloat(unity_FogColor.xyz)));
  r0.xyz = vd_asuint((vd_asfloat(r1.yyy) * vd_asfloat(r2.xyz) + vd_asfloat(r0.xyz)));
  r1.y = vd_asuint(saturate((vd_asfloat(_DissolveProgress) + vd_asfloat(_DissolveProgress))));
  r2.xyz = vd_asuint(((-(vd_asfloat(r0.xyz))) + vd_asfloat(_DissolveColor.xyz)));
  r0.xyz = vd_asuint((vd_asfloat(r1.yyy) * vd_asfloat(r2.xyz) + vd_asfloat(r0.xyz)));
  r1.y = vd_asuint(saturate((vd_asfloat(_DissolveProgress) + -0.5)));
  r1.y = vd_asuint(((-(vd_asfloat(r1.y))) * 2.0 + 1.0));
  r1.x = x0[r1.x + 0].x;
  r1.x = vd_asuint((vd_asfloat(r0.w) * vd_asfloat(r1.y) + (-(vd_asfloat(r1.x)))));
  r1.x = vd_asuint((-(int)(vd_asfloat(r1.x) < 0.0)));
  if (r1.x != 0u) discard;
  r1.x = vd_asuint(saturate((vd_asfloat(v3.y) + (-(vd_asfloat(_PlaneReflectionPivotHeight))))));
  r1.x = vd_asuint(((-(vd_asfloat(r1.x))) + 1.0));
  r1.y = vd_asuint((vd_asfloat(_PlaneReflectionPivotHeight) + -0.25));
  r1.y = vd_asuint((vd_asfloat(r1.y) + (-(vd_asfloat((VD_COL(unity_ObjectToWorld, 3).y))))));
  r1.y = vd_asuint(saturate((vd_asfloat(r1.y) * -4.0)));
  r1.x = vd_asuint((vd_asfloat(r1.y) * vd_asfloat(r1.x)));
  r0.xyz = vd_asuint((vd_asfloat(r0.xyz) * float3(0.75, 0.75, 0.75) + (-(vd_asfloat(_PlaneReflectionBackgroundColor.xyz)))));
  o0.xyz = (vd_asfloat(r1.xxx) * vd_asfloat(r0.xyz) + vd_asfloat(_PlaneReflectionBackgroundColor.xyz));
  o0.w = vd_asfloat(r0.w);
  return;
}
