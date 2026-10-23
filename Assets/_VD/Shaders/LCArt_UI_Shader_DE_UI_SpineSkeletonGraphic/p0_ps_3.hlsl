// LCArt/UI/Shader_DE_UI_SpineSkeletonGraphic | pass 0 "Normal" | progFragment | keywords: (none) | blob 3
void frag(
  float4 v0 : SV_POSITION0,
  float4 v1 : VDREG1,
  float4 v2 : VDREG2,
  float4 v3 : VDREG3,
  float4 v4 : VDREG4,
  out float4 o0 : SV_Target0)
{
  uint4 r0, r1, r2;
  r0 = 0u; r1 = 0u; r2 = 0u;

  r0.xyzw = vd_asuint(_MainTex.SampleBias(sampler_MainTex, vd_asfloat(v2.xy), vd_asfloat(_GlobalMipBias.x)).xyzw);
  r0.xyzw = vd_asuint((vd_asfloat(r0.xyzw) + vd_asfloat(_TextureSampleAdd.xyzw)));
  r1.xyz = vd_asuint((vd_asfloat(r0.xyz) * vd_asfloat(v1.xyz)));
  r1.w = vd_asuint(saturate(vd_asfloat(_IsVertexColorAlphaUsed)));
  r2.x = vd_asuint((vd_asfloat(v1.w) + -1.0));
  r1.w = vd_asuint((vd_asfloat(r1.w) * vd_asfloat(r2.x) + 1.0));
  r1.w = vd_asuint((vd_asfloat(r0.w) * vd_asfloat(r1.w)));
  r1.xyzw = vd_asuint((vd_asfloat(r1.xyzw) * vd_asfloat(_Color.xyzw)));
  r2.x = vd_asuint((-(int)(0.5 < vd_asfloat(_UseDarkColorMask))));
  if (r2.x != 0u) {
    r0.xyz = vd_asuint(((-(vd_asfloat(r0.xyz))) * vd_asfloat(v1.www) + vd_asfloat(r0.www)));
    o0.xyz = (vd_asfloat(r0.xyz) * vd_asfloat(v4.xyz) + vd_asfloat(r1.xyz));
  } else {
    o0.xyz = vd_asfloat(r1.xyz);
  }
  r0.xy = vd_asuint((-(int2)(vd_asfloat(v3.xy) >= vd_asfloat(_ClipRect.xy))));
  r0.zw = vd_asuint((-(int2)(vd_asfloat(_ClipRect.zw) >= vd_asfloat(v3.xy))));
  r0.xyzw = (r0.xyzw & uint4(0x3f800000u, 0x3f800000u, 0x3f800000u, 0x3f800000u));
  r0.xy = vd_asuint((vd_asfloat(r0.zw) * vd_asfloat(r0.xy)));
  r0.x = vd_asuint((vd_asfloat(r0.y) * vd_asfloat(r0.x)));
  o0.w = (vd_asfloat(r0.x) * vd_asfloat(r1.w));
  return;
}
