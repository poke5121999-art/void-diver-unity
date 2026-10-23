// LCArt/Character/Shader_DE_Character_FakeShadowCaster_Billboard | pass 0 "FakeShadowCast" | progVertex | keywords: _DE_EDITOR_CAMERA | blob 4
void vert(
  float4 v0 : POSITION0,
  float2 v1 : TEXCOORD0,
  out float4 o0 : SV_POSITION0,
  out float4 o1 : VDREG1)
{
  uint4 r0, r1, r2, r3, r4;
  r0 = 0u; r1 = 0u; r2 = 0u; r3 = 0u; r4 = 0u;

  r0.xy = vd_asuint(((-(vd_asfloat(_PlaneBoundMaxPos.xz))) + vd_asfloat((VD_COL(unity_ObjectToWorld, 3).xz))));
  r0.zw = vd_asuint((vd_asfloat(_PlaneBoundMinPos.xz) + (-(vd_asfloat(_PlaneBoundMaxPos.xz)))));
  r0.xy = vd_asuint(saturate((vd_asfloat(r0.xy) / vd_asfloat(r0.zw))));
  r1.xyzw = vd_asuint(_PlaneSightRenderTarget.SampleLevel(sampler_PlaneSightRenderTarget, vd_asfloat(r0.xy), 0.0).xyzw);
  r0.xyzw = vd_asuint(_PlanePointBuffer.SampleLevel(sampler_PlanePointBuffer, vd_asfloat(r0.xy), 0.0).xyzw);
  r0.x = vd_asuint(saturate((vd_asfloat(r0.x) * 4.0)));
  r0.y = vd_asuint((vd_asfloat(r0.x) * -2.0 + 3.0));
  r0.x = vd_asuint((vd_asfloat(r0.x) * vd_asfloat(r0.x)));
  r0.x = vd_asuint((vd_asfloat(r0.x) * vd_asfloat(r0.y)));
  r0.x = vd_asuint((vd_asfloat(r0.x) * vd_asfloat(_GlobalPlanePointBufferEnable)));
  r0.x = vd_asuint(max(vd_asfloat(r0.x), vd_asfloat(r1.w)));
  r0.y = vd_asuint((vd_asfloat(_GlobalPlaneSystemEnable) * vd_asfloat(_GlobalIsRuntime)));
  r0.x = vd_asuint((vd_asfloat(r0.x) + -1.0));
  r0.x = vd_asuint((vd_asfloat(r0.y) * vd_asfloat(r0.x) + 1.0));
  r0.x = vd_asuint(max(vd_asfloat(r0.x), 0.5));
  r0.w = vd_asuint((vd_asfloat(r0.x) * vd_asfloat(_FakeShadowVisibility)));
  r1.x = vd_asuint((-(int)(0.0 >= vd_asfloat(_FakeShadowScale))));
  if (r1.x != 0u) {
    r1.x = vd_asuint(dot(vd_asfloat((VD_COL(unity_ObjectToWorld, 0).xyz)), vd_asfloat((VD_COL(unity_ObjectToWorld, 0).xyz))));
    r1.y = vd_asuint(dot(vd_asfloat((VD_COL(unity_ObjectToWorld, 2).xyz)), vd_asfloat((VD_COL(unity_ObjectToWorld, 2).xyz))));
    r1.xy = vd_asuint(sqrt(vd_asfloat(r1.xy)));
    r1.x = vd_asuint((vd_asfloat(v0.x) * vd_asfloat(r1.x) + vd_asfloat((VD_COL(unity_ObjectToWorld, 3).x))));
    r1.y = vd_asuint((vd_asfloat(v0.z) * vd_asfloat(r1.y) + vd_asfloat((VD_COL(unity_ObjectToWorld, 3).z))));
    r2.xyzw = vd_asuint((vd_asfloat((VD_COL(unity_MatrixVP, 1).xyzw)) * vd_asfloat((_FakeShadowGroundY).xxxx)));
    r2.xyzw = vd_asuint((vd_asfloat((VD_COL(unity_MatrixVP, 0).xyzw)) * vd_asfloat(r1.xxxx) + vd_asfloat(r2.xyzw)));
    r1.xyzw = vd_asuint((vd_asfloat((VD_COL(unity_MatrixVP, 2).xyzw)) * vd_asfloat(r1.yyyy) + vd_asfloat(r2.xyzw)));
    r1.xyzw = vd_asuint((vd_asfloat(r1.xyzw) + vd_asfloat((VD_COL(unity_MatrixVP, 3).xyzw))));
    r2.x = vd_asuint((-(int)(vd_asfloat(r0.w) >= 0.5)));
    r2.x = (r2.x & 0x3f800000u);
    r1.xyzw = vd_asuint((vd_asfloat(r1.xyzw) + float4(-0.0, -0.0, -0.0, -1.0)));
    o0.xyzw = (vd_asfloat(r2.xxxx) * vd_asfloat(r1.xyzw) + float4(0.0, 0.0, 0.0, 1.0));
    o1.xy = vd_asfloat(vd_asuint(v1.xy));
    o1.z = vd_asfloat(0x3f800000u);
    o1.w = vd_asfloat(r0.w);
    return;
  }
  r1.x = vd_asuint(dot(vd_asfloat((VD_COL(unity_ObjectToWorld, 0).xyz)), vd_asfloat((VD_COL(unity_ObjectToWorld, 0).xyz))));
  r1.y = vd_asuint(dot(vd_asfloat((VD_COL(unity_ObjectToWorld, 1).xyz)), vd_asfloat((VD_COL(unity_ObjectToWorld, 1).xyz))));
  r1.xy = vd_asuint(sqrt(vd_asfloat(r1.xy)));
  r1.xy = vd_asuint((vd_asfloat(r1.xy) * vd_asfloat(v0.xy)));
  r1.z = vd_asuint((-(int)(0.5 < vd_asfloat(_FakeShadowIsPoint))));
  if (r1.z != 0u) {
    r1.z = vd_asuint((-(int)(0.0 >= vd_asfloat(_FakeShadowPointRange))));
    if (r1.z != 0u) {
      o0.xyzw = vd_asfloat(uint4(0x00000000u, 0x00000000u, 0x00000000u, 0x3f800000u));
      o1.xyz = vd_asfloat(uint3(0x00000000u, 0x00000000u, 0x00000000u));
      o1.w = vd_asfloat(r0.w);
      return;
    }
    r2.xyz = vd_asuint(((-(vd_asfloat((VD_COL(unity_ObjectToWorld, 3).xyz)))) + vd_asfloat(_FakeShadowLightData.xyz)));
    r1.z = vd_asuint(dot(vd_asfloat(r2.xz), vd_asfloat(r2.xz)));
    r1.w = vd_asuint(dot(vd_asfloat(r2.xyz), vd_asfloat(r2.xyz)));
    r1.w = vd_asuint(rsqrt(vd_asfloat(r1.w)));
    r2.xyz = vd_asuint((vd_asfloat(r1.www) * vd_asfloat(r2.xyz)));
    r1.w = vd_asuint(dot((-(vd_asfloat(r2.xz))), (-(vd_asfloat(r2.xz)))));
    r3.x = vd_asuint(rsqrt(vd_asfloat(r1.w)));
    r3.yz = vd_asuint(((-(vd_asfloat(r2.xz))) * vd_asfloat(r3.xx)));
    r4.x = r3.z;
    r4.y = 0x00000000u;
    r3.xw = vd_asuint(((-(vd_asfloat(r3.xy))) * float2(0.0, 1.0) + vd_asfloat(r4.xy)));
    r1.w = vd_asuint(dot(vd_asfloat(r3.xw), vd_asfloat(r3.xw)));
    r1.w = vd_asuint(rsqrt(vd_asfloat(r1.w)));
    r3.xw = vd_asuint((vd_asfloat(r1.ww) * vd_asfloat(r3.xw)));
    r1.w = vd_asuint(dot(vd_asfloat(r2.xz), vd_asfloat(r2.xz)));
    r1.zw = vd_asuint(sqrt(vd_asfloat(r1.zw)));
    r1.w = vd_asuint((vd_asfloat(r1.w) * vd_asfloat(_FakeShadowScale)));
    r2.y = vd_asuint(max(abs(vd_asfloat(r2.y)), 0.00999999978));
    r1.w = vd_asuint((vd_asfloat(r1.w) / vd_asfloat(r2.y)));
    r2.yw = vd_asuint((vd_asfloat(r1.ww) * vd_asfloat(r3.yz)));
    r4.x = vd_asuint((VD_COL(unity_MatrixV, 0).z));
    r4.y = vd_asuint((VD_COL(unity_MatrixV, 2).z));
    r1.w = vd_asuint(dot(vd_asfloat(r4.xy), vd_asfloat(r2.xz)));
    r1.w = vd_asuint((-(int)(vd_asfloat(r1.w) >= 0.0)));
    r1.w = ((r1.w != 0u) ? 0x3f800000u : 0xbf800000u);
    r2.xz = vd_asuint((vd_asfloat(r1.ww) * vd_asfloat(r3.xw)));
    r2.yw = vd_asuint((vd_asfloat(r1.yy) * vd_asfloat(r2.yw)));
    r2.xy = vd_asuint((vd_asfloat(r2.xz) * vd_asfloat(r1.xx) + vd_asfloat(r2.yw)));
    r2.xy = vd_asuint((vd_asfloat(r2.xy) + vd_asfloat((VD_COL(unity_ObjectToWorld, 3).xz))));
    r1.w = vd_asuint(max(vd_asfloat(_FakeShadowPointRange), 0.00100000005));
    r1.w = vd_asuint((vd_asfloat(r1.z) / vd_asfloat(r1.w)));
    r1.w = vd_asuint(min(vd_asfloat(r1.w), 1.0));
    r1.w = vd_asuint(((-(vd_asfloat(r1.w))) + 1.0));
    r1.w = vd_asuint((vd_asfloat(r1.w) * vd_asfloat(r1.w)));
    r1.z = vd_asuint((vd_asfloat(r1.z) + -0.5));
    r1.z = vd_asuint(saturate((vd_asfloat(r1.z) + vd_asfloat(r1.z))));
    r0.x = vd_asuint((vd_asfloat(r1.w) * vd_asfloat(r1.z)));
  } else {
    r1.z = vd_asuint(dot((-(vd_asfloat(_FakeShadowLightData.xz))), (-(vd_asfloat(_FakeShadowLightData.xz)))));
    r3.x = vd_asuint(rsqrt(vd_asfloat(r1.z)));
    r3.yz = vd_asuint((vd_asfloat(r3.xx) * (-(vd_asfloat(_FakeShadowLightData.xz)))));
    r4.x = r3.z;
    r4.y = 0x00000000u;
    r1.zw = vd_asuint(((-(vd_asfloat(r3.xy))) * float2(0.0, 1.0) + vd_asfloat(r4.xy)));
    r2.z = vd_asuint(dot(vd_asfloat(r1.zw), vd_asfloat(r1.zw)));
    r2.z = vd_asuint(rsqrt(vd_asfloat(r2.z)));
    r1.zw = vd_asuint((vd_asfloat(r1.zw) * vd_asfloat(r2.zz)));
    r2.z = vd_asuint(dot(vd_asfloat(_FakeShadowLightData.xz), vd_asfloat(_FakeShadowLightData.xz)));
    r2.z = vd_asuint(sqrt(vd_asfloat(r2.z)));
    r2.z = vd_asuint((vd_asfloat(r2.z) * vd_asfloat(_FakeShadowScale)));
    r2.w = vd_asuint(max(abs(vd_asfloat(_FakeShadowLightData.y)), 0.00999999978));
    r2.z = vd_asuint((vd_asfloat(r2.z) / vd_asfloat(r2.w)));
    r2.zw = vd_asuint((vd_asfloat(r2.zz) * vd_asfloat(r3.yz)));
    r3.x = vd_asuint((VD_COL(unity_MatrixV, 0).z));
    r3.y = vd_asuint((VD_COL(unity_MatrixV, 2).z));
    r3.x = vd_asuint(dot(vd_asfloat(r3.xy), vd_asfloat(_FakeShadowLightData.xz)));
    r3.x = vd_asuint((-(int)(vd_asfloat(r3.x) >= 0.0)));
    r3.x = ((r3.x != 0u) ? 0x3f800000u : 0xbf800000u);
    r1.zw = vd_asuint((vd_asfloat(r1.zw) * vd_asfloat(r3.xx)));
    r2.zw = vd_asuint((vd_asfloat(r1.yy) * vd_asfloat(r2.zw)));
    r1.xy = vd_asuint((vd_asfloat(r1.zw) * vd_asfloat(r1.xx) + vd_asfloat(r2.zw)));
    r2.xy = vd_asuint((vd_asfloat(r1.xy) + vd_asfloat((VD_COL(unity_ObjectToWorld, 3).xz))));
    r0.x = 0x3f800000u;
  }
  r1.xyzw = vd_asuint((vd_asfloat((VD_COL(unity_MatrixVP, 1).xyzw)) * vd_asfloat((_FakeShadowGroundY).xxxx)));
  r1.xyzw = vd_asuint((vd_asfloat((VD_COL(unity_MatrixVP, 0).xyzw)) * vd_asfloat(r2.xxxx) + vd_asfloat(r1.xyzw)));
  r1.xyzw = vd_asuint((vd_asfloat((VD_COL(unity_MatrixVP, 2).xyzw)) * vd_asfloat(r2.yyyy) + vd_asfloat(r1.xyzw)));
  r1.xyzw = vd_asuint((vd_asfloat(r1.xyzw) + vd_asfloat((VD_COL(unity_MatrixVP, 3).xyzw))));
  r2.x = vd_asuint((-(int)(vd_asfloat(r0.w) >= 0.5)));
  r2.x = (r2.x & 0x3f800000u);
  r1.xyzw = vd_asuint((vd_asfloat(r1.xyzw) + float4(-0.0, -0.0, -0.0, -1.0)));
  o0.xyzw = (vd_asfloat(r2.xxxx) * vd_asfloat(r1.xyzw) + float4(0.0, 0.0, 0.0, 1.0));
  r0.yz = vd_asuint(v1.xy);
  o1.xyzw = vd_asfloat(r0.yzxw);
  return;
}
