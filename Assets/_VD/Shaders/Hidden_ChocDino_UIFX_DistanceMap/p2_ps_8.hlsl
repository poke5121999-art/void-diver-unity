// Hidden/ChocDino/UIFX/DistanceMap | pass 2 "JumpFlood" | progFragment | keywords: DIST_SQUARE | blob 8
void frag(
  float4 v0 : SV_POSITION0,
  float4 v1 : VDREG1,
  out float2 o0 : SV_Target0)
{
  uint4 r0, r1, r2, r3;
  r0 = 0u; r1 = 0u; r2 = 0u; r3 = 0u;

  r0.xyzw = vd_asuint(_MainTex.Sample(sampler_MainTex, vd_asfloat(v1.xy)).xyzw);
  r0.w = vd_asuint((-(int)(-1.0 < vd_asfloat(r0.x))));
  if (r0.w != 0u) {
    r1.xy = vd_asuint((vd_asfloat(r0.xy) + (-(vd_asfloat(v1.xy)))));
    r1.xy = vd_asuint((vd_asfloat(r1.xy) * vd_asfloat(_MainTex_TexelSize.zw)));
    r0.w = vd_asuint(dot(vd_asfloat(r1.xy), vd_asfloat(r1.xy)));
    r0.w = vd_asuint((-(int)(vd_asfloat(r0.w) < 2.0)));
    if (r0.w != 0u) {
      o0.xy = vd_asfloat(r0.xy);
      return;
    }
    r1.xy = vd_asuint((vd_asfloat(v1.xy) * vd_asfloat(_MainTex_TexelSize.zw)));
    r1.xy = vd_asuint((vd_asfloat(r0.xy) * vd_asfloat(_MainTex_TexelSize.zw) + (-(vd_asfloat(r1.xy)))));
    r0.z = vd_asuint(max(abs(vd_asfloat(r1.y)), abs(vd_asfloat(r1.x))));
  } else {
    r0.xyz = uint3(0xbf800000u, 0xbf800000u, 0x4b800000u);
  }
  r1.xy = vd_asuint((vd_asfloat(_MainTex_TexelSize.xy) * vd_asfloat(_StepSize.xy)));
  r1.zw = vd_asuint(((-(vd_asfloat(_MainTex_TexelSize.xy))) * vd_asfloat(_StepSize.xy) + vd_asfloat(v1.xy)));
  r2.xyzw = vd_asuint(_MainTex.Sample(sampler_MainTex, vd_asfloat(r1.zw)).xyzw);
  r0.w = vd_asuint((-(int)(-1.0 < vd_asfloat(r2.x))));
  r1.zw = vd_asuint((vd_asfloat(v1.xy) * vd_asfloat(_MainTex_TexelSize.zw)));
  r3.xy = vd_asuint((vd_asfloat(r2.xy) * vd_asfloat(_MainTex_TexelSize.zw) + (-(vd_asfloat(r1.zw)))));
  r2.z = vd_asuint(max(abs(vd_asfloat(r3.y)), abs(vd_asfloat(r3.x))));
  r2.w = vd_asuint((-(int)(vd_asfloat(r2.z) < vd_asfloat(r0.z))));
  r2.xyz = ((r2.www != 0u) ? r2.xyz : r0.xyz);
  r0.xyz = ((r0.www != 0u) ? r2.xyz : r0.xyz);
  r2.xyzw = vd_asuint((vd_asfloat(r1.xyxy) * float4(0.0, -1.0, 1.0, -1.0) + vd_asfloat(v1.xyxy)));
  r3.xyzw = vd_asuint(_MainTex.Sample(sampler_MainTex, vd_asfloat(r2.xy)).xyzw);
  r0.w = vd_asuint((-(int)(-1.0 < vd_asfloat(r3.x))));
  r2.xy = vd_asuint((vd_asfloat(r3.xy) * vd_asfloat(_MainTex_TexelSize.zw) + (-(vd_asfloat(r1.zw)))));
  r3.z = vd_asuint(max(abs(vd_asfloat(r2.y)), abs(vd_asfloat(r2.x))));
  r2.x = vd_asuint((-(int)(vd_asfloat(r3.z) < vd_asfloat(r0.z))));
  r3.xyz = ((r2.xxx != 0u) ? r3.xyz : r0.xyz);
  r0.xyz = ((r0.www != 0u) ? r3.xyz : r0.xyz);
  r2.xyzw = vd_asuint(_MainTex.Sample(sampler_MainTex, vd_asfloat(r2.zw)).xyzw);
  r0.w = vd_asuint((-(int)(-1.0 < vd_asfloat(r2.x))));
  r3.xy = vd_asuint((vd_asfloat(r2.xy) * vd_asfloat(_MainTex_TexelSize.zw) + (-(vd_asfloat(r1.zw)))));
  r2.z = vd_asuint(max(abs(vd_asfloat(r3.y)), abs(vd_asfloat(r3.x))));
  r2.w = vd_asuint((-(int)(vd_asfloat(r2.z) < vd_asfloat(r0.z))));
  r2.xyz = ((r2.www != 0u) ? r2.xyz : r0.xyz);
  r0.xyz = ((r0.www != 0u) ? r2.xyz : r0.xyz);
  r2.xyzw = vd_asuint((vd_asfloat(r1.xyxy) * float4(-1.0, 0.0, 1.0, 0.0) + vd_asfloat(v1.xyxy)));
  r3.xyzw = vd_asuint(_MainTex.Sample(sampler_MainTex, vd_asfloat(r2.xy)).xyzw);
  r0.w = vd_asuint((-(int)(-1.0 < vd_asfloat(r3.x))));
  r2.xy = vd_asuint((vd_asfloat(r3.xy) * vd_asfloat(_MainTex_TexelSize.zw) + (-(vd_asfloat(r1.zw)))));
  r3.z = vd_asuint(max(abs(vd_asfloat(r2.y)), abs(vd_asfloat(r2.x))));
  r2.x = vd_asuint((-(int)(vd_asfloat(r3.z) < vd_asfloat(r0.z))));
  r3.xyz = ((r2.xxx != 0u) ? r3.xyz : r0.xyz);
  r0.xyz = ((r0.www != 0u) ? r3.xyz : r0.xyz);
  r2.xyzw = vd_asuint(_MainTex.Sample(sampler_MainTex, vd_asfloat(r2.zw)).xyzw);
  r0.w = vd_asuint((-(int)(-1.0 < vd_asfloat(r2.x))));
  r3.xy = vd_asuint((vd_asfloat(r2.xy) * vd_asfloat(_MainTex_TexelSize.zw) + (-(vd_asfloat(r1.zw)))));
  r2.z = vd_asuint(max(abs(vd_asfloat(r3.y)), abs(vd_asfloat(r3.x))));
  r2.w = vd_asuint((-(int)(vd_asfloat(r2.z) < vd_asfloat(r0.z))));
  r2.xyz = ((r2.www != 0u) ? r2.xyz : r0.xyz);
  r0.xyz = ((r0.www != 0u) ? r2.xyz : r0.xyz);
  r2.xyzw = vd_asuint((vd_asfloat(r1.xyxy) * float4(-1.0, 1.0, 0.0, 1.0) + vd_asfloat(v1.xyxy)));
  r3.xyzw = vd_asuint(_MainTex.Sample(sampler_MainTex, vd_asfloat(r2.xy)).xyzw);
  r0.w = vd_asuint((-(int)(-1.0 < vd_asfloat(r3.x))));
  r1.xy = vd_asuint((vd_asfloat(r3.xy) * vd_asfloat(_MainTex_TexelSize.zw) + (-(vd_asfloat(r1.zw)))));
  r3.z = vd_asuint(max(abs(vd_asfloat(r1.y)), abs(vd_asfloat(r1.x))));
  r1.x = vd_asuint((-(int)(vd_asfloat(r3.z) < vd_asfloat(r0.z))));
  r3.xyz = ((r1.xxx != 0u) ? r3.xyz : r0.xyz);
  r0.xyz = ((r0.www != 0u) ? r3.xyz : r0.xyz);
  r2.xyzw = vd_asuint(_MainTex.Sample(sampler_MainTex, vd_asfloat(r2.zw)).xyzw);
  r0.w = vd_asuint((-(int)(-1.0 < vd_asfloat(r2.x))));
  r1.xy = vd_asuint((vd_asfloat(r2.xy) * vd_asfloat(_MainTex_TexelSize.zw) + (-(vd_asfloat(r1.zw)))));
  r2.z = vd_asuint(max(abs(vd_asfloat(r1.y)), abs(vd_asfloat(r1.x))));
  r1.x = vd_asuint((-(int)(vd_asfloat(r2.z) < vd_asfloat(r0.z))));
  r2.xyz = ((r1.xxx != 0u) ? r2.xyz : r0.xyz);
  r0.xyz = ((r0.www != 0u) ? r2.xyz : r0.xyz);
  r1.xy = vd_asuint((vd_asfloat(_MainTex_TexelSize.xy) * vd_asfloat(_StepSize.xy) + vd_asfloat(v1.xy)));
  r2.xyzw = vd_asuint(_MainTex.Sample(sampler_MainTex, vd_asfloat(r1.xy)).xyzw);
  r0.w = vd_asuint((-(int)(-1.0 < vd_asfloat(r2.x))));
  r1.xy = vd_asuint((vd_asfloat(r2.xy) * vd_asfloat(_MainTex_TexelSize.zw) + (-(vd_asfloat(r1.zw)))));
  r1.x = vd_asuint(max(abs(vd_asfloat(r1.y)), abs(vd_asfloat(r1.x))));
  r0.z = vd_asuint((-(int)(vd_asfloat(r1.x) < vd_asfloat(r0.z))));
  r1.xy = ((r0.zz != 0u) ? r2.xy : r0.xy);
  o0.xy = vd_asfloat(((r0.ww != 0u) ? r1.xy : r0.xy));
  return;
}
