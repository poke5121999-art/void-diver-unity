// LCArt/WorldUI/Shader_DE_WorldUI_DropItem | pass 1 "Outline" | progFragment | keywords: (none) | blob 23
void frag(
  float4 v0 : SV_POSITION0,
  float4 v1 : VDREG1,
  out float4 o0 : SV_Target0)
{
  uint4 r0, r1, r2;
  r0 = 0u; r1 = 0u; r2 = 0u;

  r0.xy = vd_asuint((vd_asfloat(_MainTex_TexelSize.xy) * vd_asfloat((_OutlineStroke).xx)));
  r0.xy = vd_asuint((vd_asfloat(r0.xy) * float2(5.0, 5.0)));
  r0.z = 0x00000000u;
  r1.xyzw = vd_asuint((vd_asfloat(r0.zyxz) + vd_asfloat(v1.xyxy)));
  r0.xyzw = vd_asuint(((-(vd_asfloat(r0.zyxz))) + vd_asfloat(v1.xyxy)));
  r2.xyzw = vd_asuint(_MainTex.SampleBias(sampler_MainTex, vd_asfloat(r1.xy), vd_asfloat(_GlobalMipBias.x)).xyzw);
  r1.xyzw = vd_asuint(_MainTex.SampleBias(sampler_MainTex, vd_asfloat(r1.zw), vd_asfloat(_GlobalMipBias.x)).xyzw);
  r1.x = vd_asuint((vd_asfloat(r1.w) + -0.100000001));
  r1.y = vd_asuint((vd_asfloat(r2.w) + -0.100000001));
  r1.xy = vd_asuint(saturate((vd_asfloat(r1.xy) * float2(6.66666651, 6.66666651))));
  r1.z = vd_asuint((vd_asfloat(r1.y) * -2.0 + 3.0));
  r1.y = vd_asuint((vd_asfloat(r1.y) * vd_asfloat(r1.y)));
  r1.y = vd_asuint((vd_asfloat(r1.y) * vd_asfloat(r1.z)));
  r2.xyzw = vd_asuint(_MainTex.SampleBias(sampler_MainTex, vd_asfloat(r0.xy), vd_asfloat(_GlobalMipBias.x)).xyzw);
  r0.xyzw = vd_asuint(_MainTex.SampleBias(sampler_MainTex, vd_asfloat(r0.zw), vd_asfloat(_GlobalMipBias.x)).xyzw);
  r0.x = vd_asuint((vd_asfloat(r0.w) + -0.100000001));
  r0.y = vd_asuint((vd_asfloat(r2.w) + -0.100000001));
  r0.xy = vd_asuint(saturate((vd_asfloat(r0.xy) * float2(6.66666651, 6.66666651))));
  r0.z = vd_asuint((vd_asfloat(r0.y) * -2.0 + 3.0));
  r0.y = vd_asuint((vd_asfloat(r0.y) * vd_asfloat(r0.y)));
  r0.y = vd_asuint((vd_asfloat(r0.y) * vd_asfloat(r0.z)));
  r0.y = vd_asuint(max(vd_asfloat(r0.y), vd_asfloat(r1.y)));
  r0.z = vd_asuint((vd_asfloat(r0.x) * -2.0 + 3.0));
  r0.x = vd_asuint((vd_asfloat(r0.x) * vd_asfloat(r0.x)));
  r0.x = vd_asuint((vd_asfloat(r0.x) * vd_asfloat(r0.z)));
  r0.z = vd_asuint((vd_asfloat(r1.x) * -2.0 + 3.0));
  r0.w = vd_asuint((vd_asfloat(r1.x) * vd_asfloat(r1.x)));
  r0.z = vd_asuint((vd_asfloat(r0.w) * vd_asfloat(r0.z)));
  r0.x = vd_asuint(max(vd_asfloat(r0.z), vd_asfloat(r0.x)));
  r0.x = vd_asuint(max(vd_asfloat(r0.x), vd_asfloat(r0.y)));
  r1.xyzw = vd_asuint(_MainTex.SampleBias(sampler_MainTex, vd_asfloat(v1.xy), vd_asfloat(_GlobalMipBias.x)).xyzw);
  r0.y = vd_asuint((vd_asfloat(r1.w) * vd_asfloat(v1.z) + -0.899999976));
  r0.y = vd_asuint(saturate((vd_asfloat(r0.y) * 9.99999809)));
  r0.z = vd_asuint((vd_asfloat(r0.y) * -2.0 + 3.0));
  r0.y = vd_asuint((vd_asfloat(r0.y) * vd_asfloat(r0.y)));
  r0.y = vd_asuint(((-(vd_asfloat(r0.z))) * vd_asfloat(r0.y) + 1.0));
  r0.x = vd_asuint((vd_asfloat(r0.y) * vd_asfloat(r0.x)));
  r0.y = vd_asuint((vd_asfloat(r0.x) * vd_asfloat(_OutlineEnable) + -0.00999999978));
  r0.x = vd_asuint((vd_asfloat(r0.x) * vd_asfloat(_OutlineEnable)));
  o0.w = (vd_asfloat(r0.x) * 0.375);
  r0.x = vd_asuint((-(int)(vd_asfloat(r0.y) < 0.0)));
  if (r0.x != 0u) discard;
  o0.xyz = vd_asfloat(vd_asuint(_OutlineColor.xyz));
  return;
}
