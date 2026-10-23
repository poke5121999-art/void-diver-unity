// LCArt/Indicator/Shader_DE_Indicator_Box | pass 0 "" | progFragment | keywords: _CLUSTER_LIGHT_LOOP _DE_EDITOR_CAMERA | blob 15
void frag(
  float4 v0 : VDREG0,
  float4 v1 : VDREG1,
  float4 v2 : VDREG2,
  float4 v3 : SV_POSITION0,
  out float4 o0 : SV_Target0)
{
  uint4 r0, r1, r2;
  r0 = 0u; r1 = 0u; r2 = 0u;

  r0.x = vd_asuint((vd_asfloat(_MeshHalfLengthX) * vd_asfloat(_LineScaling)));
  r0.x = vd_asuint(max(vd_asfloat(r0.x), 9.99999975e-05));
  r0.y = vd_asuint((vd_asfloat(r0.x) * 0.0199999996));
  r0.z = vd_asuint(saturate(vd_asfloat(_Progress)));
  r0.w = vd_asuint((vd_asfloat(r0.x) * vd_asfloat(r0.z)));
  r0.z = vd_asuint((vd_asfloat(r0.z) + -0.125));
  r0.z = vd_asuint(saturate((vd_asfloat(r0.z) * 8.0)));
  r0.y = vd_asuint((vd_asfloat(r0.w) * vd_asfloat(_FillRatio) + vd_asfloat(r0.y)));
  r0.x = vd_asuint((vd_asfloat(r0.x) * 0.0199999996 + vd_asfloat(r0.y)));
  r0.x = vd_asuint(((-(vd_asfloat(r0.w))) * vd_asfloat(_FillRatio) + vd_asfloat(r0.x)));
  r0.x = vd_asuint((1.0 / vd_asfloat(r0.x)));
  r1.x = vd_asuint(dot(vd_asfloat((VD_COL(unity_ObjectToWorld, 0).xyz)), vd_asfloat((VD_COL(unity_ObjectToWorld, 0).xyz))));
  r1.x = vd_asuint(rsqrt(vd_asfloat(r1.x)));
  r1.xyz = vd_asuint((vd_asfloat(r1.xxx) * vd_asfloat((VD_COL(unity_ObjectToWorld, 0).xyz))));
  r2.xyz = vd_asuint((vd_asfloat(v1.xyz) + (-(vd_asfloat((VD_COL(unity_ObjectToWorld, 3).xyz))))));
  r1.x = vd_asuint(dot(vd_asfloat(r2.xyz), vd_asfloat(r1.xyz)));
  r0.w = vd_asuint(((-(vd_asfloat(r0.w))) * vd_asfloat(_FillRatio) + abs(vd_asfloat(r1.x))));
  r0.y = vd_asuint(((-(vd_asfloat(r0.y))) + abs(vd_asfloat(r1.x))));
  r0.y = vd_asuint((abs(vd_asfloat(r0.y)) + (-(vd_asfloat(_LineThickness)))));
  r0.x = vd_asuint(saturate((vd_asfloat(r0.x) * vd_asfloat(r0.w))));
  r0.w = vd_asuint((vd_asfloat(r0.x) * -2.0 + 3.0));
  r0.x = vd_asuint((vd_asfloat(r0.x) * vd_asfloat(r0.x)));
  r0.x = vd_asuint(((-(vd_asfloat(r0.w))) * vd_asfloat(r0.x) + 1.0));
  r0.x = vd_asuint((-(int)(vd_asfloat(r0.x) >= 0.5)));
  r0.x = (r0.x & 0x3f800000u);
  r0.w = vd_asuint((vd_asfloat(r0.x) * vd_asfloat(_FillColor.w)));
  r1.x = vd_asuint((1.0 / (-(vd_asfloat(_LineThickness)))));
  r0.y = vd_asuint(saturate((vd_asfloat(r0.y) * vd_asfloat(r1.x))));
  r1.x = vd_asuint((vd_asfloat(r0.y) * -2.0 + 3.0));
  r0.y = vd_asuint((vd_asfloat(r0.y) * vd_asfloat(r0.y)));
  r0.y = vd_asuint((vd_asfloat(r0.y) * vd_asfloat(r1.x)));
  r0.y = vd_asuint((vd_asfloat(r0.y) * vd_asfloat(_LineColor.w)));
  r0.w = vd_asuint(max(vd_asfloat(r0.y), vd_asfloat(r0.w)));
  r1.x = vd_asuint((vd_asfloat(r0.z) * -2.0 + 3.0));
  r0.z = vd_asuint((vd_asfloat(r0.z) * vd_asfloat(r0.z)));
  r0.z = vd_asuint((vd_asfloat(r0.z) * vd_asfloat(r1.x)));
  r0.w = vd_asuint((vd_asfloat(r0.z) * vd_asfloat(r0.w)));
  r0.y = vd_asuint(saturate(((-(vd_asfloat(r0.y))) * vd_asfloat(r0.z) + 1.0)));
  r0.x = vd_asuint((vd_asfloat(r0.y) * vd_asfloat(r0.x)));
  r1.xyzw = vd_asuint(_WeightTex.SampleBias(sampler_WeightTex, vd_asfloat(v0.xy), vd_asfloat(_GlobalMipBias.x)).zwxy);
  r1.xy = vd_asuint(saturate(vd_asfloat(r1.xy)));
  r0.y = vd_asuint((vd_asfloat(r0.w) * vd_asfloat(r1.y)));
  r0.z = vd_asuint((-(int)(vd_asfloat(r0.y) >= 0.00999999978)));
  r0.z = (r0.z & 0x3f800000u);
  r0.z = vd_asuint((vd_asfloat(r1.y) * vd_asfloat(r0.z)));
  r1.yzw = vd_asuint((vd_asfloat(_FillColor.xyz) + (-(vd_asfloat(_LineColor.xyz)))));
  r1.yzw = vd_asuint((vd_asfloat(r0.xxx) * vd_asfloat(r1.yzw) + vd_asfloat(_LineColor.xyz)));
  r2.xyzw = vd_asuint(_MainTex.SampleBias(sampler_MainTex, vd_asfloat(v0.xy), vd_asfloat(_GlobalMipBias.x)).xyzw);
  r1.yzw = vd_asuint((vd_asfloat(r1.yzw) + (-(vd_asfloat(r2.xyz)))));
  o0.xyz = (vd_asfloat(r0.zzz) * vd_asfloat(r1.yzw) + vd_asfloat(r2.xyz));
  r0.x = vd_asuint((vd_asfloat(r2.w) * vd_asfloat(r2.w)));
  r0.x = vd_asuint(max(vd_asfloat(r0.y), vd_asfloat(r0.x)));
  r0.y = vd_asuint(max(vd_asfloat(_LengthScaling), 9.99999975e-05));
  r0.y = vd_asuint((vd_asfloat(_AlphaFadeWorld) / vd_asfloat(r0.y)));
  r0.y = vd_asuint(max(vd_asfloat(r0.y), 9.99999975e-05));
  r0.y = vd_asuint((1.0 / vd_asfloat(r0.y)));
  r0.z = vd_asuint(((-(vd_asfloat(v0.y))) + 1.0));
  r0.z = vd_asuint(saturate((vd_asfloat(r0.y) * vd_asfloat(r0.z))));
  r0.y = vd_asuint(saturate((vd_asfloat(r0.y) * vd_asfloat(v0.y))));
  r0.w = vd_asuint((vd_asfloat(r0.z) * -2.0 + 3.0));
  r0.z = vd_asuint((vd_asfloat(r0.z) * vd_asfloat(r0.z)));
  r0.z = vd_asuint((vd_asfloat(r0.z) * vd_asfloat(r0.w)));
  r0.w = vd_asuint((vd_asfloat(r0.y) * -2.0 + 3.0));
  r0.y = vd_asuint((vd_asfloat(r0.y) * vd_asfloat(r0.y)));
  r0.y = vd_asuint((vd_asfloat(r0.y) * vd_asfloat(r0.w)));
  r0.y = vd_asuint(min(vd_asfloat(r0.z), vd_asfloat(r0.y)));
  r0.z = vd_asuint(((-(vd_asfloat(r0.y))) + 1.0));
  r0.y = vd_asuint((vd_asfloat(r1.x) * vd_asfloat(r0.z) + vd_asfloat(r0.y)));
  r0.z = vd_asuint((vd_asfloat(_Progress) + -1.25));
  r0.z = vd_asuint(saturate((vd_asfloat(r0.z) * 4.0)));
  r0.w = vd_asuint((vd_asfloat(r0.z) * -2.0 + 3.0));
  r0.z = vd_asuint((vd_asfloat(r0.z) * vd_asfloat(r0.z)));
  r0.z = vd_asuint(((-(vd_asfloat(r0.w))) * vd_asfloat(r0.z) + 1.0));
  r0.x = vd_asuint((vd_asfloat(r0.z) * vd_asfloat(r0.x)));
  r0.x = vd_asuint((vd_asfloat(r0.y) * vd_asfloat(r0.x)));
  r0.yzw = vd_asuint((vd_asfloat(v2.xyz) / vd_asfloat(v2.www)));
  r1.xyzw = vd_asuint(_CustomDepthBuffer.SampleBias(sampler_CustomDepthBuffer, vd_asfloat(r0.yz), vd_asfloat(_GlobalMipBias.x)).xyzw);
  r1.x = vd_asuint((vd_asfloat(_ZBufferParams.z) * vd_asfloat(r1.x) + vd_asfloat(_ZBufferParams.w)));
  r1.x = vd_asuint((1.0 / vd_asfloat(r1.x)));
  r2.xyzw = vd_asuint(_CameraDepthTexture.SampleBias(sampler_CameraDepthTexture, vd_asfloat(r0.yz), vd_asfloat(_GlobalMipBias.x)).xyzw);
  r0.y = vd_asuint((vd_asfloat(_ZBufferParams.z) * vd_asfloat(r0.w) + vd_asfloat(_ZBufferParams.w)));
  r0.y = vd_asuint((1.0 / vd_asfloat(r0.y)));
  r0.z = vd_asuint((vd_asfloat(_ZBufferParams.z) * vd_asfloat(r2.x) + vd_asfloat(_ZBufferParams.w)));
  r0.z = vd_asuint((1.0 / vd_asfloat(r0.z)));
  r0.z = vd_asuint(min(vd_asfloat(r0.z), vd_asfloat(r1.x)));
  r0.y = vd_asuint(((-(vd_asfloat(r0.z))) + vd_asfloat(r0.y)));
  r0.y = vd_asuint((vd_asfloat(r0.y) + (-(vd_asfloat(_OcclusionFadeStart)))));
  r0.z = vd_asuint(((-(vd_asfloat(_OcclusionFadeStart))) + vd_asfloat(_OcclusionFadeEnd)));
  r0.z = vd_asuint(max(vd_asfloat(r0.z), 9.99999975e-05));
  r0.y = vd_asuint(saturate((vd_asfloat(r0.y) / vd_asfloat(r0.z))));
  r0.y = vd_asuint(((-(vd_asfloat(r0.y))) + 1.0));
  r0.z = vd_asuint(((-(vd_asfloat(_OccludedAlpha))) + 1.0));
  r0.y = vd_asuint((vd_asfloat(r0.y) * vd_asfloat(r0.z) + vd_asfloat(_OccludedAlpha)));
  o0.w = (vd_asfloat(r0.y) * vd_asfloat(r0.x));
  return;
}
