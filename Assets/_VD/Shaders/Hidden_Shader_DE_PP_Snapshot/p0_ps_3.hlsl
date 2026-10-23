// Hidden/Shader_DE_PP_Snapshot | pass 0 "DrawProcedural" | progFragment | keywords: (none) | blob 3
void frag(
  float4 v0 : SV_POSITION0,
  float4 v1 : VDREG1,
  float4 v2 : VDREG2,
  out float4 o0 : SV_TARGET0)
{
  uint4 r0, r1, r2, r3, r4, r5;
  r0 = 0u; r1 = 0u; r2 = 0u; r3 = 0u; r4 = 0u; r5 = 0u;

  r0.xy = vd_asuint(saturate((vd_asfloat(v1.xy) + vd_asfloat((_Stroke).xx))));
  r0.xy = vd_asuint((vd_asfloat(r0.xy) * vd_asfloat(_ScreenSize.xy)));
  r0.xy = ((uint2)vd_asfloat(r0.xy));
  r0.zw = uint2(0x00000000u, 0x00000000u);
  r0.xyzw = vd_asuint(_BlitTexture.Load(vd_asint(r0.xyw)).xyzw);
  r0.xy = vd_asuint((vd_asfloat(v1.xy) * vd_asfloat(_ScreenSize.xy)));
  r1.xy = ((uint2)vd_asfloat(r0.xy));
  r1.zw = uint2(0x00000000u, 0x00000000u);
  r1.xyzw = vd_asuint(_BlitTexture.Load(vd_asint(r1.xyw)).xyzw);
  r0.xy = vd_asuint((vd_asfloat(r1.ww) * vd_asfloat(r1.yw)));
  r0.z = vd_asuint(((-(vd_asfloat(r0.w))) * vd_asfloat(r0.w) + vd_asfloat(r0.y)));
  r2.xyzw = vd_asuint(saturate((vd_asfloat((_Stroke).xxxx) * float4(1.0, -1.0, -1.0, 1.0) + vd_asfloat(v1.xyxy))));
  r2.xyzw = vd_asuint((vd_asfloat(r2.xyzw) * vd_asfloat(_ScreenSize.xyxy)));
  r3.xy = ((uint2)vd_asfloat(r2.xy));
  r2.xy = ((uint2)vd_asfloat(r2.zw));
  r3.zw = uint2(0x00000000u, 0x00000000u);
  r3.xyzw = vd_asuint(_BlitTexture.Load(vd_asint(r3.xyw)).xyzw);
  r0.w = vd_asuint(((-(vd_asfloat(r3.w))) * vd_asfloat(r3.w) + vd_asfloat(r0.y)));
  r0.z = vd_asuint((abs(vd_asfloat(r0.w)) + abs(vd_asfloat(r0.z))));
  r2.zw = uint2(0x00000000u, 0x00000000u);
  r2.xyzw = vd_asuint(_BlitTexture.Load(vd_asint(r2.xyw)).xyzw);
  r0.w = vd_asuint(((-(vd_asfloat(r2.w))) * vd_asfloat(r2.w) + vd_asfloat(r0.y)));
  r1.xy = vd_asuint(saturate((vd_asfloat(v1.xy) + (-(vd_asfloat((_Stroke).xx))))));
  r1.xy = vd_asuint((vd_asfloat(r1.xy) * vd_asfloat(_ScreenSize.xy)));
  r2.xy = ((uint2)vd_asfloat(r1.xy));
  r2.zw = uint2(0x00000000u, 0x00000000u);
  r2.xyzw = vd_asuint(_BlitTexture.Load(vd_asint(r2.xyw)).xyzw);
  r0.y = vd_asuint(((-(vd_asfloat(r2.w))) * vd_asfloat(r2.w) + vd_asfloat(r0.y)));
  r0.y = vd_asuint((abs(vd_asfloat(r0.y)) + abs(vd_asfloat(r0.w))));
  r0.y = vd_asuint((vd_asfloat(r0.y) + vd_asfloat(r0.z)));
  o0.w = saturate((vd_asfloat(r1.w) + vd_asfloat(r0.y)));
  r1.z = vd_asuint((-(vd_asfloat(_NavMeshStroke))));
  r0.y = vd_asuint((vd_asfloat(_ScreenParams.x) / vd_asfloat(_ScreenParams.y)));
  r2.xw = vd_asuint((-(vd_asfloat((_NavMeshStroke).xx))));
  r1.x = vd_asuint((vd_asfloat(r0.y) * vd_asfloat(r2.x)));
  r2.y = vd_asuint((vd_asfloat(r0.y) * vd_asfloat(_NavMeshStroke)));
  r0.yz = vd_asuint((vd_asfloat(r2.wy) + vd_asfloat(v1.xy)));
  r3.xyzw = vd_asuint(_MapBlockMeshTexture.SampleBias(sampler_vd_point_clamp, vd_asfloat(r0.yz), vd_asfloat(_GlobalMipBias.x)).xyzw);
  r0.yz = vd_asuint((vd_asfloat(r1.zx) + vd_asfloat(v1.xy)));
  r4.xyzw = vd_asuint(_MapBlockMeshTexture.SampleBias(sampler_vd_point_clamp, vd_asfloat(r0.yz), vd_asfloat(_GlobalMipBias.x)).xyzw);
  r0.y = vd_asuint((-(int)(vd_asfloat(r4.y) >= 0.00100000005)));
  r5.xyzw = vd_asuint(_MapBlockMeshTexture.SampleBias(sampler_vd_point_clamp, vd_asfloat(v1.xy), vd_asfloat(_GlobalMipBias.x)).xyzw);
  r0.z = vd_asuint((-(int)(vd_asfloat(r5.y) >= 0.00100000005)));
  r0.yz = ((r0.yz != 0u) ? uint2(0x00000000u, 0x00000000u) : uint2(0x3f800000u, 0x3f800000u));
  r0.z = vd_asuint((vd_asfloat(r0.z) * vd_asfloat(r5.x)));
  r0.y = vd_asuint(((-(vd_asfloat(r4.x))) * vd_asfloat(r0.y) + vd_asfloat(r0.z)));
  r0.w = vd_asuint((-(int)(vd_asfloat(r3.y) >= 0.00100000005)));
  r0.w = ((r0.w != 0u) ? 0x00000000u : 0x3f800000u);
  r0.w = vd_asuint(((-(vd_asfloat(r3.x))) * vd_asfloat(r0.w) + vd_asfloat(r0.z)));
  r0.y = vd_asuint(max(abs(vd_asfloat(r0.y)), abs(vd_asfloat(r0.w))));
  r1.y = vd_asuint(_NavMeshStroke);
  r1.xy = vd_asuint((vd_asfloat(r1.yx) + vd_asfloat(v1.xy)));
  r1.xyzw = vd_asuint(_MapBlockMeshTexture.SampleBias(sampler_vd_point_clamp, vd_asfloat(r1.xy), vd_asfloat(_GlobalMipBias.x)).xyzw);
  r0.w = vd_asuint((-(int)(vd_asfloat(r1.y) >= 0.00100000005)));
  r0.w = ((r0.w != 0u) ? 0x00000000u : 0x3f800000u);
  r0.w = vd_asuint(((-(vd_asfloat(r1.x))) * vd_asfloat(r0.w) + vd_asfloat(r0.z)));
  r2.z = vd_asuint(_NavMeshStroke);
  r1.xy = vd_asuint((vd_asfloat(r2.zy) + vd_asfloat(v1.xy)));
  r1.xyzw = vd_asuint(_MapBlockMeshTexture.SampleBias(sampler_vd_point_clamp, vd_asfloat(r1.xy), vd_asfloat(_GlobalMipBias.x)).xyzw);
  r1.y = vd_asuint((-(int)(vd_asfloat(r1.y) >= 0.00100000005)));
  r1.y = ((r1.y != 0u) ? 0x00000000u : 0x3f800000u);
  r1.x = vd_asuint(((-(vd_asfloat(r1.x))) * vd_asfloat(r1.y) + vd_asfloat(r0.z)));
  r0.w = vd_asuint(max(abs(vd_asfloat(r0.w)), abs(vd_asfloat(r1.x))));
  r0.y = vd_asuint(max(vd_asfloat(r0.y), vd_asfloat(r0.w)));
  r0.y = vd_asuint((vd_asfloat(r0.y) * vd_asfloat(r0.z)));
  r1.xyzw = vd_asuint(_MapBlockMeshTexture.SampleBias(sampler_MapBlockMeshTexture, vd_asfloat(v1.xy), vd_asfloat(_GlobalMipBias.x)).xyzw);
  r0.z = vd_asuint((-(int)(vd_asfloat(r1.y) >= 0.00100000005)));
  r0.z = ((r0.z != 0u) ? 0x00000000u : 0x3f800000u);
  r0.z = vd_asuint(((-(vd_asfloat(r1.x))) * vd_asfloat(r0.z) + 1.0));
  r1.xyz = vd_asuint(((-(vd_asfloat(_FloorColor.xyz))) + vd_asfloat(_NavMeshColor.xyz)));
  r1.xyz = vd_asuint((vd_asfloat(r0.zzz) * vd_asfloat(r1.xyz) + vd_asfloat(_FloorColor.xyz)));
  r2.xyz = vd_asuint(((-(vd_asfloat(r1.xyz))) + vd_asfloat(_NavMeshStrokeColor.xyz)));
  r0.yzw = vd_asuint((vd_asfloat(r0.yyy) * vd_asfloat(r2.xyz) + vd_asfloat(r1.xyz)));
  r1.xyz = vd_asuint(((-(vd_asfloat(r0.yzw))) + vd_asfloat(_WallColor.xyz)));
  o0.xyz = (vd_asfloat(r0.xxx) * vd_asfloat(r1.xyz) + vd_asfloat(r0.yzw));
  return;
}
