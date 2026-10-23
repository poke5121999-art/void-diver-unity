// Hidden/ChocDino/UIFX/Blend-Fill-Gradient | pass 0 "Blend-Fill-Gradient" | progFragment | keywords: BLEND_BACKGROUND DITHER GRADIENT_LERP_LINEAR GRADIENT_SHAPE_RADIAL UNITY_UI_ALPHACLIP | blob 2696
void frag(
  float4 v0 : SV_POSITION0,
  float4 v1 : VDREG1,
  float4 v2 : VDREG2,
  out float4 o0 : SV_Target0)
{
  uint4 r0, r1, r2, r3;
  r0 = 0u; r1 = 0u; r2 = 0u; r3 = 0u;

  r0.xyzw = vd_asuint(_ResultTex.Sample(sampler_ResultTex, vd_asfloat(v1.xy)).xyzw);
  r1.x = vd_asuint((vd_asfloat(_ResultTex_TexelSize.y) / vd_asfloat(_ResultTex_TexelSize.x)));
  r2.xz = vd_asuint((vd_asfloat(v1.xy) + float2(-0.5, -0.5)));
  r2.y = vd_asuint((vd_asfloat(r1.x) * vd_asfloat(r2.x)));
  r1.zw = vd_asuint((vd_asfloat(r2.yz) + vd_asfloat(r2.yz)));
  r1.y = 0x3f800000u;
  r1.xy = vd_asuint(((-(vd_asfloat(_GradientRadial.xy))) * vd_asfloat(r1.xy) + vd_asfloat(r1.zw)));
  r1.x = vd_asuint(dot(vd_asfloat(r1.xy), vd_asfloat(r1.xy)));
  r1.x = vd_asuint(sqrt(vd_asfloat(r1.x)));
  r1.x = vd_asuint((vd_asfloat(r1.x) / vd_asfloat(_GradientRadial.z)));
  r1.y = vd_asuint((vd_asfloat(_GradientDither) / vd_asfloat(_GradientTransform.x)));
  r1.zw = vd_asuint((vd_asfloat(v1.xy) * vd_asfloat(_ResultTex_TexelSize.zw)));
  r1.z = vd_asuint(dot(vd_asfloat(r1.zw), float2(0.0671105608, 0.00583714992)));
  r1.z = vd_asuint(frac(vd_asfloat(r1.z)));
  r1.zw = vd_asuint((vd_asfloat(r1.zy) * float2(52.9829178, 0.5)));
  r1.z = vd_asuint(frac(vd_asfloat(r1.z)));
  r1.y = vd_asuint((vd_asfloat(r1.y) * vd_asfloat(r1.z) + (-(vd_asfloat(r1.w)))));
  r1.x = vd_asuint((vd_asfloat(r1.y) + vd_asfloat(r1.x)));
  r1.x = vd_asuint((vd_asfloat(r1.x) + (-(vd_asfloat(_GradientTransform.y)))));
  r1.x = vd_asuint((vd_asfloat(r1.x) * vd_asfloat(_GradientTransform.x) + vd_asfloat(_GradientTransform.y)));
  r1.x = vd_asuint((vd_asfloat(r1.x) + vd_asfloat(_GradientTransform.z)));
  r1.yz = vd_asuint((-(int2)(vd_asfloat(_GradientTransform.ww) >= float2(1.0, 2.0))));
  r1.w = vd_asuint((vd_asfloat(r1.x) * 0.5));
  r1.w = vd_asuint(floor(vd_asfloat(r1.w)));
  r1.w = vd_asuint(((-(vd_asfloat(r1.w))) * 2.0 + vd_asfloat(r1.x)));
  r1.w = vd_asuint(max(vd_asfloat(r1.w), 0.0));
  r1.w = vd_asuint(min(vd_asfloat(r1.w), 2.0));
  r1.w = vd_asuint((vd_asfloat(r1.w) + -1.0));
  r1.w = vd_asuint(((-(abs(vd_asfloat(r1.w)))) + 1.0));
  r2.x = vd_asuint(floor(vd_asfloat(r1.x)));
  r2.x = vd_asuint(saturate((vd_asfloat(r1.x) + (-(vd_asfloat(r2.x))))));
  r1.z = ((r1.z != 0u) ? r1.w : r2.x);
  r1.x = ((r1.y != 0u) ? r1.z : r1.x);
  r1.yzw = vd_asuint(_GradientColors[0].xyz);
  r2.x = 0x00000001u;
  while (true) {
    r2.y = vd_asuint((-(int)(vd_asint(r2.x) >= vd_asint(_GradientColorCount))));
    if (r2.y != 0u) break;
    r2.yz = vd_asuint((vd_asint(r2.xx) + int2(-1, 1)));
    r2.w = vd_asuint((vd_asfloat(r1.x) + (-(vd_asfloat(_GradientColors[(int)(r2.y)].w)))));
    r2.y = vd_asuint(((-(vd_asfloat(_GradientColors[(int)(r2.y)].w))) + vd_asfloat(_GradientColors[(int)(r2.x)].w)));
    r2.y = vd_asuint(saturate((vd_asfloat(r2.w) / vd_asfloat(r2.y))));
    r3.xyz = vd_asuint(((-(vd_asfloat(r1.yzw))) + vd_asfloat(_GradientColors[(int)(r2.x)].xyz)));
    r1.yzw = vd_asuint((vd_asfloat(r2.yyy) * vd_asfloat(r3.xyz) + vd_asfloat(r1.yzw)));
    r2.x = r2.z;
  }
  r2.x = vd_asuint(_GradientAlphas[0].x);
  r2.y = 0x00000001u;
  while (true) {
    r2.z = vd_asuint((-(int)(vd_asint(r2.y) >= vd_asint(_GradientAlphaCount))));
    if (r2.z != 0u) break;
    r2.zw = vd_asuint((vd_asint(r2.yy) + int2(-1, 1)));
    r3.x = vd_asuint((vd_asfloat(r1.x) + (-(vd_asfloat(_GradientAlphas[(int)(r2.z)].w)))));
    r2.z = vd_asuint(((-(vd_asfloat(_GradientAlphas[(int)(r2.z)].w))) + vd_asfloat(_GradientAlphas[(int)(r2.y)].w)));
    r2.z = vd_asuint(saturate((vd_asfloat(r3.x) / vd_asfloat(r2.z))));
    r3.x = vd_asuint(((-(vd_asfloat(r2.x))) + vd_asfloat(_GradientAlphas[(int)(r2.y)].x)));
    r2.x = vd_asuint((vd_asfloat(r2.z) * vd_asfloat(r3.x) + vd_asfloat(r2.x)));
    r2.xy = r2.xw;
  }
  r1.xyz = vd_asuint((vd_asfloat(r1.yzw) * vd_asfloat(r2.xxx)));
  r1.w = vd_asuint(((-(vd_asfloat(r0.w))) + 1.0));
  r2.w = vd_asuint((vd_asfloat(r1.w) * vd_asfloat(r2.x) + vd_asfloat(r0.w)));
  r2.xyz = vd_asuint((vd_asfloat(r1.www) * vd_asfloat(r1.xyz) + vd_asfloat(r0.xyz)));
  r1.xyzw = vd_asuint(((-(vd_asfloat(r0.xyzw))) + vd_asfloat(r2.xyzw)));
  r0.xyzw = vd_asuint((vd_asfloat((_Strength).xxxx) * vd_asfloat(r1.xyzw) + vd_asfloat(r0.xyzw)));
  r1.x = vd_asuint((vd_asfloat(r0.w) + -0.00100000005));
  r1.x = vd_asuint((-(int)(vd_asfloat(r1.x) < 0.0)));
  if (r1.x != 0u) discard;
  r0.xyz = vd_asuint((vd_asfloat(r0.xyz) * vd_asfloat(v2.www)));
  o0.xyzw = (vd_asfloat(r0.xyzw) * vd_asfloat(v2.xyzw));
  return;
}
