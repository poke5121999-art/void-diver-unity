// Universal Render Pipeline/Nature/SpeedTree8_PBRLit | pass 3 "DepthNormals" | progFragment | keywords: BACKFACE_NORMAL_MODE_MIRROR | blob 44
void frag(
  float4 v0 : SV_POSITION0,
  float4 v1 : VDREG1,
  float4 v2 : VDREG2,
  float4 v3 : VDREG3,
  float4 v4 : VDREG4,
  float4 v5 : VDREG5,
  float4 v6 : VDREG6,
  float4 v7 : VDREG7,
  uint v8 : SV_IsFrontFace0,
  out float4 o0 : SV_Target0)
{
  uint4 r0, r1, r2, r3, r4, r5;
  r0 = 0u; r1 = 0u; r2 = 0u; r3 = 0u; r4 = 0u; r5 = 0u;

  r0.xyzw = vd_asuint(_MainTex.SampleBias(sampler_MainTex, vd_asfloat(v2.xy), vd_asfloat(_GlobalMipBias.x)).xyzw);
  r0.x = vd_asuint((vd_asfloat(v3.w) * vd_asfloat(r0.w) + (-(vd_asfloat(_AlphaClipThreshold)))));
  r0.x = vd_asuint((-(int)(vd_asfloat(r0.x) < 0.0)));
  if (r0.x != 0u) discard;
  r0.xyzw = vd_asuint(_BumpMap.SampleBias(sampler_BumpMap, vd_asfloat(v2.xy), vd_asfloat(_GlobalMipBias.x)).xyzw);
  r0.x = vd_asuint((vd_asfloat(r0.x) * vd_asfloat(r0.w)));
  r0.xy = vd_asuint((vd_asfloat(r0.xy) * float2(2.0, 2.0) + float2(-1.0, -1.0)));
  r0.w = vd_asuint(dot(vd_asfloat(r0.xy), vd_asfloat(r0.xy)));
  r0.w = vd_asuint(min(vd_asfloat(r0.w), 1.0));
  r0.w = vd_asuint(((-(vd_asfloat(r0.w))) + 1.0));
  r0.w = vd_asuint(sqrt(vd_asfloat(r0.w)));
  r0.z = vd_asuint(max(vd_asfloat(r0.w), 1.00000002e-16));
  r1.xyz = ((vd_asuint(v8.xxx) != 0u) ? uint3(0x3f800000u, 0x3f800000u, 0x3f800000u) : uint3(0x3f800000u, 0x3f800000u, 0xbf800000u));
  r0.xyz = vd_asuint((vd_asfloat(r0.xyz) * vd_asfloat(r1.xyz)));
  r1.xyz = vd_asuint((vd_asfloat(r0.yyy) * vd_asfloat(v7.xyz)));
  r0.xyw = vd_asuint((vd_asfloat(r0.xxx) * vd_asfloat(v6.xyz) + vd_asfloat(r1.xyz)));
  r1.x = vd_asuint(v4.w);
  r1.y = vd_asuint(v5.w);
  r1.z = vd_asuint(v6.w);
  r0.xyz = vd_asuint((vd_asfloat(r0.zzz) * vd_asfloat(r1.xyz) + vd_asfloat(r0.xyw)));
  r0.w = vd_asuint((-(int)(0.0 < vd_asfloat(v1.w))));
  r0.w = ((r0.w != 0u) ? 0x3f800000u : 0xbf800000u);
  r1.x = vd_asuint((-(int)(vd_asfloat(unity_WorldTransformParams.w) >= 0.0)));
  r1.x = ((r1.x != 0u) ? 0x3f800000u : 0xbf800000u);
  r0.w = vd_asuint((vd_asfloat(r0.w) * vd_asfloat(r1.x)));
  r1.xyz = vd_asuint((vd_asfloat(v1.zxy) * vd_asfloat(v5.xyz)));
  r1.xyz = vd_asuint((vd_asfloat(v5.zxy) * vd_asfloat(v1.xyz) + (-(vd_asfloat(r1.xyz)))));
  r1.xyz = vd_asuint((vd_asfloat(r0.www) * vd_asfloat(r1.xyz)));
  r0.w = vd_asuint(dot(vd_asfloat(v5.xyz), vd_asfloat(v5.xyz)));
  r0.w = vd_asuint(sqrt(vd_asfloat(r0.w)));
  r0.w = vd_asuint((1.0 / vd_asfloat(r0.w)));
  r1.xyz = vd_asuint((vd_asfloat(r1.xyz) * vd_asfloat(r0.www)));
  r1.w = vd_asuint(dot(vd_asfloat(r1.zxy), vd_asfloat(r0.xyz)));
  r2.xyz = vd_asuint((vd_asfloat(r0.www) * vd_asfloat(v5.zxy)));
  r3.xyz = vd_asuint((vd_asfloat(r0.www) * vd_asfloat(v1.xyz)));
  r4.xyz = vd_asuint((vd_asfloat(r2.xyz) * vd_asfloat(r3.yzx)));
  r4.xyz = vd_asuint((vd_asfloat(r2.zxy) * vd_asfloat(r3.zxy) + (-(vd_asfloat(r4.xyz)))));
  r4.xyz = vd_asuint((vd_asfloat(r1.www) * vd_asfloat(r4.xyz)));
  r0.w = vd_asuint(dot(vd_asfloat(r3.xyz), vd_asfloat(r0.xyz)));
  r0.x = vd_asuint(dot(vd_asfloat(r2.yzx), vd_asfloat(r0.xyz)));
  r5.xyz = vd_asuint((vd_asfloat(r1.yzx) * vd_asfloat(r2.zxy)));
  r2.xyz = vd_asuint((vd_asfloat(r1.xyz) * vd_asfloat(r2.xyz) + (-(vd_asfloat(r5.xyz)))));
  r0.yzw = vd_asuint((vd_asfloat(r0.www) * vd_asfloat(r2.xyz) + vd_asfloat(r4.xyz)));
  r1.w = vd_asuint(dot(vd_asfloat(r3.xyz), vd_asfloat(r2.xyz)));
  r2.xyz = vd_asuint((vd_asfloat(r1.xyz) * vd_asfloat(r3.zxy)));
  r1.xyz = vd_asuint((vd_asfloat(r3.yzx) * vd_asfloat(r1.yzx) + (-(vd_asfloat(r2.xyz)))));
  r0.xyz = vd_asuint((vd_asfloat(r0.xxx) * vd_asfloat(r1.xyz) + vd_asfloat(r0.yzw)));
  r0.xyz = vd_asuint((vd_asfloat(r0.xyz) / vd_asfloat(r1.www)));
  r0.w = vd_asuint(dot(vd_asfloat(r0.xyz), vd_asfloat(r0.xyz)));
  r0.w = vd_asuint(rsqrt(vd_asfloat(r0.w)));
  o0.xyz = (vd_asfloat(r0.www) * vd_asfloat(r0.xyz));
  o0.w = vd_asfloat(0x00000000u);
  return;
}
