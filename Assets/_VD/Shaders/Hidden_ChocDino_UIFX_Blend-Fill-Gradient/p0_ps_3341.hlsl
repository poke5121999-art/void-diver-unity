// Hidden/ChocDino/UIFX/Blend-Fill-Gradient | pass 0 "Blend-Fill-Gradient" | progFragment | keywords: BLEND_REPLACE_ALPHA GRADIENT_LERP_STEP GRADIENT_SHAPE_CONIC UNITY_UI_ALPHACLIP UNITY_UI_CLIP_RECT | blob 3341
void frag(
  float4 v0 : SV_POSITION0,
  float4 v1 : VDREG1,
  float4 v2 : VDREG2,
  float4 v3 : VDREG3,
  out float4 o0 : SV_Target0)
{
  uint4 r0, r1, r2;
  r0 = 0u; r1 = 0u; r2 = 0u;

  r0.xyzw = vd_asuint(_ResultTex.Sample(sampler_ResultTex, vd_asfloat(v1.xy)).xyzw);
  r1.xy = vd_asuint(((-(vd_asfloat(v1.xy))) + float2(0.5, 0.5)));
  r1.xy = vd_asuint((vd_asfloat(r1.xy) * float2(2.0, 2.0) + vd_asfloat(_GradientRadial.xy)));
  r1.z = vd_asuint(min(abs(vd_asfloat(r1.y)), abs(vd_asfloat(r1.x))));
  r1.w = vd_asuint(max(abs(vd_asfloat(r1.y)), abs(vd_asfloat(r1.x))));
  r1.w = vd_asuint((1.0 / vd_asfloat(r1.w)));
  r1.z = vd_asuint((vd_asfloat(r1.w) * vd_asfloat(r1.z)));
  r1.w = vd_asuint((vd_asfloat(r1.z) * vd_asfloat(r1.z)));
  r2.x = vd_asuint((vd_asfloat(r1.w) * 0.0208350997 + -0.0851330012));
  r2.x = vd_asuint((vd_asfloat(r1.w) * vd_asfloat(r2.x) + 0.180141002));
  r2.x = vd_asuint((vd_asfloat(r1.w) * vd_asfloat(r2.x) + -0.330299497));
  r1.w = vd_asuint((vd_asfloat(r1.w) * vd_asfloat(r2.x) + 0.999866009));
  r2.x = vd_asuint((vd_asfloat(r1.w) * vd_asfloat(r1.z)));
  r2.y = vd_asuint((-(int)(abs(vd_asfloat(r1.y)) < abs(vd_asfloat(r1.x)))));
  r2.x = vd_asuint((vd_asfloat(r2.x) * -2.0 + 1.57079637));
  r2.x = (r2.y & r2.x);
  r1.z = vd_asuint((vd_asfloat(r1.z) * vd_asfloat(r1.w) + vd_asfloat(r2.x)));
  r1.w = vd_asuint((-(int)(vd_asfloat(r1.y) < (-(vd_asfloat(r1.y))))));
  r1.w = (r1.w & 0xc0490fdbu);
  r1.z = vd_asuint((vd_asfloat(r1.w) + vd_asfloat(r1.z)));
  r1.w = vd_asuint(min(vd_asfloat(r1.y), vd_asfloat(r1.x)));
  r1.x = vd_asuint(max(vd_asfloat(r1.y), vd_asfloat(r1.x)));
  r1.y = vd_asuint((-(int)(vd_asfloat(r1.w) < (-(vd_asfloat(r1.w))))));
  r1.x = vd_asuint((-(int)(vd_asfloat(r1.x) >= (-(vd_asfloat(r1.x))))));
  r1.x = (r1.x & r1.y);
  r1.x = vd_asuint(((r1.x != 0u) ? (-(vd_asfloat(r1.z))) : vd_asfloat(r1.z)));
  r1.x = vd_asuint((vd_asfloat(r1.x) + 3.14159274));
  r1.x = vd_asuint((vd_asfloat(r1.x) * 0.159154937 + (-(vd_asfloat(_GradientTransform.y)))));
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
  r1.y = vd_asuint(_GradientAlphas[0].x);
  r1.z = 0x00000001u;
  while (true) {
    r1.w = vd_asuint((-(int)(vd_asint(r1.z) >= vd_asint(_GradientAlphaCount))));
    if (r1.w != 0u) break;
    r2.xy = vd_asuint((vd_asint(r1.zz) + int2(-1, 1)));
    r1.w = vd_asuint((-(int)(vd_asfloat(r1.x) >= vd_asfloat(_GradientAlphas[(int)(r2.x)].w))));
    r1.w = (r1.w & 0x3f800000u);
    r2.z = vd_asuint(((-(vd_asfloat(_GradientAlphas[(int)(r2.x)].w))) + vd_asfloat(_GradientAlphas[(int)(r1.z)].w)));
    r1.w = vd_asuint((vd_asfloat(r1.w) + (-(vd_asfloat(_GradientAlphas[(int)(r2.x)].w)))));
    r2.x = vd_asuint((1.0 / vd_asfloat(r2.z)));
    r1.w = vd_asuint(saturate((vd_asfloat(r1.w) * vd_asfloat(r2.x))));
    r2.x = vd_asuint((vd_asfloat(r1.w) * -2.0 + 3.0));
    r1.w = vd_asuint((vd_asfloat(r1.w) * vd_asfloat(r1.w)));
    r1.w = vd_asuint((vd_asfloat(r1.w) * vd_asfloat(r2.x)));
    r2.x = vd_asuint(((-(vd_asfloat(r1.y))) + vd_asfloat(_GradientAlphas[(int)(r1.z)].x)));
    r1.y = vd_asuint((vd_asfloat(r1.w) * vd_asfloat(r2.x) + vd_asfloat(r1.y)));
    r1.z = r2.y;
  }
  r1.xyzw = vd_asuint((vd_asfloat(r0.wxyz) * vd_asfloat(r1.yyyy) + (-(vd_asfloat(r0.wxyz)))));
  r0.xyzw = vd_asuint((vd_asfloat((_Strength).xxxx) * vd_asfloat(r1.xyzw) + vd_asfloat(r0.wxyz)));
  r1.xy = vd_asuint(((-(vd_asfloat(_ClipRect.xy))) + vd_asfloat(_ClipRect.zw)));
  r1.xy = vd_asuint((vd_asfloat(r1.xy) + (-(abs(vd_asfloat(v3.xy))))));
  r1.xy = vd_asuint(saturate((vd_asfloat(r1.xy) * vd_asfloat(v3.zw))));
  r1.x = vd_asuint((vd_asfloat(r1.y) * vd_asfloat(r1.x)));
  r2.xyzw = vd_asuint((vd_asfloat(r0.yzwx) * vd_asfloat(r1.xxxx)));
  r0.x = vd_asuint((vd_asfloat(r0.x) * vd_asfloat(r1.x) + -0.00100000005));
  r0.x = vd_asuint((-(int)(vd_asfloat(r0.x) < 0.0)));
  if (r0.x != 0u) discard;
  r2.xyz = vd_asuint((vd_asfloat(r2.xyz) * vd_asfloat(v2.www)));
  o0.xyzw = (vd_asfloat(r2.xyzw) * vd_asfloat(v2.xyzw));
  return;
}
