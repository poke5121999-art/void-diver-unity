// LCArt/UI/Shader_DE_UI_DissolveFrame | pass 0 "Default" | progFragment | keywords: (none) | blob 3
void frag(
  float4 v0 : SV_POSITION0,
  float4 v1 : VDREG1,
  float4 v2 : VDREG2,
  float4 v3 : VDREG3,
  out float4 o0 : SV_Target0)
{
  uint4 r0, r1;
  r0 = 0u; r1 = 0u;

  r0.xy = vd_asuint((vd_asfloat(v2.xy) + float2(-0.5, -0.5)));
  r0.z = vd_asuint((vd_asfloat(r0.y) * -0.988031626));
  r1.x = vd_asuint((vd_asfloat(r0.x) * 0.154251456 + (-(vd_asfloat(r0.z)))));
  r1.y = vd_asuint(dot(vd_asfloat(r0.xy), float2(-0.988031626, 0.154251456)));
  r0.x = vd_asuint(max(abs(vd_asfloat(r1.y)), abs(vd_asfloat(r1.x))));
  r0.x = vd_asuint((1.0 / vd_asfloat(r0.x)));
  r0.y = vd_asuint(min(abs(vd_asfloat(r1.y)), abs(vd_asfloat(r1.x))));
  r0.x = vd_asuint((vd_asfloat(r0.x) * vd_asfloat(r0.y)));
  r0.y = vd_asuint((vd_asfloat(r0.x) * vd_asfloat(r0.x)));
  r0.z = vd_asuint((vd_asfloat(r0.y) * 0.0208350997 + -0.0851330012));
  r0.z = vd_asuint((vd_asfloat(r0.y) * vd_asfloat(r0.z) + 0.180141002));
  r0.z = vd_asuint((vd_asfloat(r0.y) * vd_asfloat(r0.z) + -0.330299497));
  r0.y = vd_asuint((vd_asfloat(r0.y) * vd_asfloat(r0.z) + 0.999866009));
  r0.z = vd_asuint((vd_asfloat(r0.y) * vd_asfloat(r0.x)));
  r0.z = vd_asuint((vd_asfloat(r0.z) * -2.0 + 1.57079637));
  r0.w = vd_asuint((-(int)(abs(vd_asfloat(r1.y)) < abs(vd_asfloat(r1.x)))));
  r0.z = (r0.w & r0.z);
  r0.x = vd_asuint((vd_asfloat(r0.x) * vd_asfloat(r0.y) + vd_asfloat(r0.z)));
  r0.y = vd_asuint((-(int)(vd_asfloat(r1.y) < (-(vd_asfloat(r1.y))))));
  r0.y = (r0.y & 0xc0490fdbu);
  r0.x = vd_asuint((vd_asfloat(r0.y) + vd_asfloat(r0.x)));
  r0.y = vd_asuint(min(vd_asfloat(r1.y), vd_asfloat(r1.x)));
  r0.y = vd_asuint((-(int)(vd_asfloat(r0.y) < (-(vd_asfloat(r0.y))))));
  r0.z = vd_asuint(max(vd_asfloat(r1.y), vd_asfloat(r1.x)));
  r0.w = vd_asuint(dot(vd_asfloat(r1.xy), vd_asfloat(r1.xy)));
  r0.w = vd_asuint(sqrt(vd_asfloat(r0.w)));
  r0.z = vd_asuint((-(int)(vd_asfloat(r0.z) >= (-(vd_asfloat(r0.z))))));
  r0.y = (r0.z & r0.y);
  r0.x = vd_asuint(((r0.y != 0u) ? (-(vd_asfloat(r0.x))) : vd_asfloat(r0.x)));
  r0.x = vd_asuint((vd_asfloat(r0.x) * 0.159235671));
  r0.y = vd_asuint(abs(vd_asfloat(r0.x)));
  r0.z = vd_asuint((vd_asfloat(r0.w) + vd_asfloat(r0.w)));
  r0.x = vd_asuint((vd_asfloat(_Time.y) * vd_asfloat(_DissolveNoiseSpeed) + vd_asfloat(r0.z)));
  r0.z = vd_asuint((vd_asfloat(r0.z) * vd_asfloat(r0.z)));
  r0.xy = vd_asuint((vd_asfloat(r0.xy) * float2(2.0, 0.25)));
  r0.xy = vd_asuint((vd_asfloat(r0.xy) * vd_asfloat((_DissolvePatternScale).xx)));
  r1.xyzw = vd_asuint(_NoiseTexture.Sample(sampler_NoiseTexture, vd_asfloat(r0.xy)).xyzw);
  r0.x = vd_asuint((vd_asfloat(r0.w) + vd_asfloat(r1.x)));
  r0.x = vd_asuint(saturate((vd_asfloat(r0.z) * vd_asfloat(r0.x))));
  r0.x = vd_asuint(((-(vd_asfloat(v1.w))) * 1.29999995 + vd_asfloat(r0.x)));
  r0.y = vd_asuint((vd_asfloat(v1.w) * 0.5));
  r0.y = vd_asuint((1.0 / vd_asfloat(r0.y)));
  r0.x = vd_asuint(saturate((vd_asfloat(r0.y) * vd_asfloat(r0.x))));
  r0.y = vd_asuint((vd_asfloat(r0.x) * -2.0 + 3.0));
  r0.x = vd_asuint((vd_asfloat(r0.x) * vd_asfloat(r0.x)));
  r0.x = vd_asuint(((-(vd_asfloat(r0.y))) * vd_asfloat(r0.x) + 1.0));
  r0.x = vd_asuint(max(vd_asfloat(r0.x), 0.0));
  r1.xyzw = vd_asuint(_MainTex.Sample(sampler_MainTex, vd_asfloat(v2.xy)).xyzw);
  r0.y = vd_asuint((vd_asfloat(r1.w) * vd_asfloat(_DissolveColor.w)));
  o0.w = (vd_asfloat(r0.x) * vd_asfloat(r0.y));
  r0.xyz = vd_asuint((vd_asfloat(v1.xyz) * vd_asfloat(_DissolveColor.xyz)));
  o0.xyz = (vd_asfloat(r1.xyz) * vd_asfloat(r0.xyz));
  return;
}
