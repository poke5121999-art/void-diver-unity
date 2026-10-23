// LCArt/UI/Shader_UI_Additive | pass 0 "Default" | progFragment | keywords: UNITY_UI_CLIP_RECT | blob 9
void frag(
  float4 v0 : SV_POSITION0,
  float4 v1 : VDREG1,
  float4 v2 : VDREG2,
  float4 v3 : VDREG3,
  out float4 o0 : SV_Target0)
{
  uint4 r0, r1;
  r0 = 0u; r1 = 0u;

  r0.xy = vd_asuint((-(int2)(vd_asfloat(v3.xy) >= vd_asfloat(_ClipRect.xy))));
  r0.zw = vd_asuint((-(int2)(vd_asfloat(_ClipRect.zw) >= vd_asfloat(v3.xy))));
  r0.xyzw = (r0.xyzw & uint4(0x3f800000u, 0x3f800000u, 0x3f800000u, 0x3f800000u));
  r0.xy = vd_asuint((vd_asfloat(r0.zw) * vd_asfloat(r0.xy)));
  r0.x = vd_asuint((vd_asfloat(r0.y) * vd_asfloat(r0.x)));
  r1.xyzw = vd_asuint(_MainTex.Sample(sampler_MainTex, vd_asfloat(v2.xy)).xyzw);
  r1.xyzw = vd_asuint((vd_asfloat(r1.xyzw) + vd_asfloat(_TextureSampleAdd.xyzw)));
  r1.xyzw = vd_asuint((vd_asfloat(r1.xyzw) * vd_asfloat(v1.xyzw)));
  o0.w = (vd_asfloat(r0.x) * vd_asfloat(r1.w));
  o0.xyz = vd_asfloat(r1.xyz);
  return;
}
