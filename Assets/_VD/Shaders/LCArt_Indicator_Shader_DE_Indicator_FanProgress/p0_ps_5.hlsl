// LCArt/Indicator/Shader_DE_Indicator_FanProgress | pass 0 "ForwardUnlit" | progFragment | keywords: _DE_EDITOR_CAMERA | blob 5
void frag(
  float4 v0 : SV_POSITION0,
  float4 v1 : VDREG1,
  float4 v2 : VDREG2,
  float4 v3 : VDREG3,
  out float4 o0 : SV_Target0)
{
#if UNITY_UV_STARTS_AT_TOP
#else
  float4 vd_ZBufferParamsD3D = float4(-_ZBufferParams.x, _ZBufferParams.x + _ZBufferParams.y, -_ZBufferParams.z, _ZBufferParams.z + _ZBufferParams.w);
#endif
  uint4 r0, r1, r2, r3;
  r0 = 0u; r1 = 0u; r2 = 0u; r3 = 0u;

  r0.xyz = vd_asuint((vd_asfloat(v3.xyz) / vd_asfloat(v3.www)));
  r1.xyzw = vd_asuint(_CustomDepthBuffer.SampleBias(sampler_CustomDepthBuffer, vd_asfloat(r0.xy), vd_asfloat(_GlobalMipBias.x)).xyzw);
#if UNITY_UV_STARTS_AT_TOP
  r0.x = vd_asuint((vd_asfloat(_ZBufferParams.z) * vd_asfloat(r0.z) + vd_asfloat(_ZBufferParams.w)));
#else
  r0.x = vd_asuint((vd_asfloat(vd_ZBufferParamsD3D.z) * vd_asfloat(r0.z) + vd_asfloat(vd_ZBufferParamsD3D.w)));
#endif
  r0.x = vd_asuint((1.0 / vd_asfloat(r0.x)));
#if UNITY_UV_STARTS_AT_TOP
  r0.y = vd_asuint((vd_asfloat(_ZBufferParams.z) * vd_asfloat(r1.x) + vd_asfloat(_ZBufferParams.w)));
#else
  r0.y = vd_asuint((vd_asfloat(vd_ZBufferParamsD3D.z) * vd_asfloat(r1.x) + vd_asfloat(vd_ZBufferParamsD3D.w)));
#endif
  r0.y = vd_asuint((1.0 / vd_asfloat(r0.y)));
  r0.x = vd_asuint(((-(vd_asfloat(r0.y))) + vd_asfloat(r0.x)));
  r0.x = vd_asuint((vd_asfloat(r0.x) + (-(vd_asfloat(_OcclusionFadeStart)))));
  r0.y = vd_asuint(((-(vd_asfloat(_OcclusionFadeStart))) + vd_asfloat(_OcclusionFadeEnd)));
  r0.y = vd_asuint(max(vd_asfloat(r0.y), 9.99999975e-05));
  r0.x = vd_asuint(saturate((vd_asfloat(r0.x) / vd_asfloat(r0.y))));
  r0.x = vd_asuint(((-(vd_asfloat(r0.x))) + 1.0));
  r0.y = vd_asuint(((-(vd_asfloat(_OccludedAlpha))) + 1.0));
  r0.x = vd_asuint((vd_asfloat(r0.x) * vd_asfloat(r0.y) + vd_asfloat(_OccludedAlpha)));
  r0.y = vd_asuint(dot(vd_asfloat((VD_COL(unity_ObjectToWorld, 2).xz)), vd_asfloat((VD_COL(unity_ObjectToWorld, 2).xz))));
  r0.y = vd_asuint(rsqrt(vd_asfloat(r0.y)));
  r0.yz = vd_asuint((vd_asfloat(r0.yy) * vd_asfloat((VD_COL(unity_ObjectToWorld, 2).xz))));
  r1.xy = vd_asuint((vd_asfloat(v2.xz) + (-(vd_asfloat((VD_COL(unity_ObjectToWorld, 3).xz))))));
  r0.w = vd_asuint(dot(vd_asfloat(r1.xy), vd_asfloat(r1.xy)));
  r0.w = vd_asuint(sqrt(vd_asfloat(r0.w)));
  r1.z = vd_asuint(max(vd_asfloat(r0.w), 9.99999975e-05));
  r1.xy = vd_asuint((vd_asfloat(r1.xy) / vd_asfloat(r1.zz)));
  r0.y = vd_asuint(dot(vd_asfloat(r0.yz), vd_asfloat(r1.xy)));
  r0.z = vd_asuint(max(vd_asfloat(r0.y), -1.0));
  r0.z = vd_asuint(min(vd_asfloat(r0.z), 1.0));
  r1.x = vd_asuint((abs(vd_asfloat(r0.z)) * -0.0187292993 + 0.0742610022));
  r1.x = vd_asuint((vd_asfloat(r1.x) * abs(vd_asfloat(r0.z)) + -0.212114394));
  r1.x = vd_asuint((vd_asfloat(r1.x) * abs(vd_asfloat(r0.z)) + 1.57072878));
  r1.y = vd_asuint(((-(abs(vd_asfloat(r0.z)))) + 1.0));
  r0.z = vd_asuint((-(int)(vd_asfloat(r0.z) < (-(vd_asfloat(r0.z))))));
  r1.y = vd_asuint(sqrt(vd_asfloat(r1.y)));
  r1.z = vd_asuint((vd_asfloat(r1.y) * vd_asfloat(r1.x)));
  r1.z = vd_asuint((vd_asfloat(r1.z) * -2.0 + 3.14159274));
  r0.z = (r0.z & r1.z);
  r0.z = vd_asuint((vd_asfloat(r1.x) * vd_asfloat(r1.y) + vd_asfloat(r0.z)));
  r0.z = vd_asuint(((-(vd_asfloat(_ConeAngle))) * 0.00872664619 + vd_asfloat(r0.z)));
  r1.x = vd_asuint(sin(abs(vd_asfloat(r0.z))));
  r0.z = vd_asuint((-(int)(1.57079637 >= abs(vd_asfloat(r0.z)))));
  r0.z = (r0.z & 0x3f800000u);
  r1.x = vd_asuint((vd_asfloat(r0.w) * vd_asfloat(r1.x) + (-(vd_asfloat(_LineThickness)))));
  r1.y = vd_asuint((1.0 / (-(vd_asfloat(_LineThickness)))));
  r1.x = vd_asuint(saturate((vd_asfloat(r1.y) * vd_asfloat(r1.x))));
  r1.z = vd_asuint((vd_asfloat(r1.x) * -2.0 + 3.0));
  r1.x = vd_asuint((vd_asfloat(r1.x) * vd_asfloat(r1.x)));
  r1.x = vd_asuint((vd_asfloat(r1.x) * vd_asfloat(r1.z)));
  r0.z = vd_asuint((vd_asfloat(r0.z) * vd_asfloat(r1.x)));
  r1.x = vd_asuint((vd_asfloat(_ConeAngle) * 0.00872664619));
  r1.x = vd_asuint(cos(vd_asfloat(r1.x)));
  r1.z = vd_asuint((vd_asfloat(r1.x) + -9.99999975e-05));
  r1.z = vd_asuint(min(vd_asfloat(r1.z), vd_asfloat(r1.x)));
  r1.x = vd_asuint(((-(vd_asfloat(r1.z))) + vd_asfloat(r1.x)));
  r0.y = vd_asuint((vd_asfloat(r0.y) + (-(vd_asfloat(r1.z)))));
  r1.x = vd_asuint((1.0 / vd_asfloat(r1.x)));
  r0.y = vd_asuint(saturate((vd_asfloat(r0.y) * vd_asfloat(r1.x))));
  r1.x = vd_asuint((vd_asfloat(r0.y) * -2.0 + 3.0));
  r0.y = vd_asuint((vd_asfloat(r0.y) * vd_asfloat(r0.y)));
  r0.y = vd_asuint((vd_asfloat(r0.y) * vd_asfloat(r1.x)));
  r1.xz = vd_asuint((-(int2)(vd_asfloat((_ConeAngle).xx) >= float2(0.00100000005, 0.00999999978))));
  r1.xz = (r1.xz & uint2(0x3f800000u, 0x3f800000u));
  r0.y = vd_asuint((vd_asfloat(r0.y) * vd_asfloat(r1.x)));
  r1.x = vd_asuint(max(vd_asfloat(r0.z), vd_asfloat(r0.y)));
  r1.w = vd_asuint((vd_asfloat(_MeshRadius) * vd_asfloat(_LineScaling)));
  r2.xy = vd_asuint((vd_asfloat(r1.ww) * float2(0.5, 0.00999999978)));
  r1.w = vd_asuint(saturate(vd_asfloat(_Progress)));
  r2.x = vd_asuint((vd_asfloat(r2.x) * vd_asfloat(r1.w)));
  r1.w = vd_asuint((vd_asfloat(r1.w) + -0.125));
  r1.w = vd_asuint(saturate((vd_asfloat(r1.w) * 8.0)));
  r2.z = vd_asuint((vd_asfloat(r2.x) * vd_asfloat(_FillRatio) + vd_asfloat(r2.y)));
  r2.y = vd_asuint((vd_asfloat(r2.x) * vd_asfloat(_FillRatio) + (-(vd_asfloat(r2.y)))));
  r2.x = vd_asuint(((-(vd_asfloat(r2.x))) * vd_asfloat(_FillRatio) + vd_asfloat(r0.w)));
  r0.w = vd_asuint((vd_asfloat(r0.w) + (-(vd_asfloat(r2.y)))));
  r2.y = vd_asuint(((-(vd_asfloat(r2.y))) + vd_asfloat(r2.z)));
  r2.y = vd_asuint((1.0 / vd_asfloat(r2.y)));
  r0.w = vd_asuint(saturate((vd_asfloat(r0.w) * vd_asfloat(r2.y))));
  r2.x = vd_asuint((abs(vd_asfloat(r2.x)) + (-(vd_asfloat(_LineThickness)))));
  r1.y = vd_asuint(saturate((vd_asfloat(r1.y) * vd_asfloat(r2.x))));
  r2.x = vd_asuint((vd_asfloat(r0.w) * -2.0 + 3.0));
  r0.w = vd_asuint((vd_asfloat(r0.w) * vd_asfloat(r0.w)));
  r0.w = vd_asuint(((-(vd_asfloat(r2.x))) * vd_asfloat(r0.w) + 1.0));
  r0.w = vd_asuint((vd_asfloat(r0.w) * vd_asfloat(_FillColor.w)));
  r2.x = vd_asuint((vd_asfloat(r1.y) * -2.0 + 3.0));
  r1.y = vd_asuint((vd_asfloat(r1.y) * vd_asfloat(r1.y)));
  r1.y = vd_asuint((vd_asfloat(r1.y) * vd_asfloat(r2.x)));
  r1.y = vd_asuint((vd_asfloat(r1.y) * vd_asfloat(_LineColor.w)));
  r0.w = vd_asuint(max(vd_asfloat(r0.w), vd_asfloat(r1.y)));
  r2.x = vd_asuint((vd_asfloat(r1.w) * -2.0 + 3.0));
  r1.w = vd_asuint((vd_asfloat(r1.w) * vd_asfloat(r1.w)));
  r1.w = vd_asuint((vd_asfloat(r1.w) * vd_asfloat(r2.x)));
  r0.w = vd_asuint((vd_asfloat(r0.w) * vd_asfloat(r1.w)));
  r1.y = vd_asuint((vd_asfloat(r1.w) * vd_asfloat(r1.y)));
  r0.y = vd_asuint((vd_asfloat(r0.y) * vd_asfloat(r0.w)));
  r2.xyzw = vd_asuint(_WeightTex.SampleBias(sampler_WeightTex, vd_asfloat(v1.xy), vd_asfloat(_GlobalMipBias.x)).yzxw);
  r2.xy = vd_asuint(saturate(vd_asfloat(r2.xy)));
  r0.w = vd_asuint(((-(vd_asfloat(r2.y))) + 1.0));
  r0.w = vd_asuint((vd_asfloat(r0.w) * vd_asfloat(r0.y)));
  r0.y = vd_asuint((-(int)(vd_asfloat(r0.y) >= 0.00999999978)));
  r0.y = (r0.y & 0x3f800000u);
  r3.xyzw = vd_asuint(_MainTex.SampleBias(sampler_MainTex, vd_asfloat(v1.xy), vd_asfloat(_GlobalMipBias.x)).xyzw);
  r1.w = vd_asuint((vd_asfloat(r3.w) * vd_asfloat(r3.w)));
  r0.w = vd_asuint(max(vd_asfloat(r0.w), vd_asfloat(r1.w)));
  r0.w = vd_asuint((vd_asfloat(r1.x) * vd_asfloat(r0.w)));
  r0.z = vd_asuint((vd_asfloat(r0.z) * vd_asfloat(r2.x)));
  r1.x = vd_asuint((-(int)(359.5 >= vd_asfloat(_ConeAngle))));
  r1.x = (r1.x & 0x3f800000u);
  r1.x = vd_asuint((vd_asfloat(r1.x) * vd_asfloat(_StrokeColor.w)));
  r0.z = vd_asuint((vd_asfloat(r0.z) * vd_asfloat(r1.x)));
  r1.x = vd_asuint((vd_asfloat(r2.x) * vd_asfloat(r0.z)));
  r0.w = vd_asuint(max(vd_asfloat(r0.w), vd_asfloat(r1.x)));
  r0.w = vd_asuint((vd_asfloat(r1.z) * vd_asfloat(r0.w)));
  r1.x = vd_asuint((vd_asfloat(_Progress) + -1.25));
  r1.x = vd_asuint(saturate((vd_asfloat(r1.x) * 4.0)));
  r1.z = vd_asuint((vd_asfloat(r1.x) * -2.0 + 3.0));
  r1.x = vd_asuint((vd_asfloat(r1.x) * vd_asfloat(r1.x)));
  r1.x = vd_asuint(((-(vd_asfloat(r1.z))) * vd_asfloat(r1.x) + 1.0));
  r0.w = vd_asuint((vd_asfloat(r0.w) * vd_asfloat(r1.x)));
  o0.w = (vd_asfloat(r0.x) * vd_asfloat(r0.w));
  r1.xzw = vd_asuint(((-(vd_asfloat(_FillColor.xyz))) + vd_asfloat(_LineColor.xyz)));
  r1.xyz = vd_asuint((vd_asfloat(r1.yyy) * vd_asfloat(r1.xzw) + vd_asfloat(_FillColor.xyz)));
  r1.xyz = vd_asuint(((-(vd_asfloat(r3.xyz))) + vd_asfloat(r1.xyz)));
  r0.xyw = vd_asuint((vd_asfloat(r0.yyy) * vd_asfloat(r1.xyz) + vd_asfloat(r3.xyz)));
  r1.xyz = vd_asuint(((-(vd_asfloat(r0.xyw))) + vd_asfloat(_StrokeColor.xyz)));
  o0.xyz = (vd_asfloat(r0.zzz) * vd_asfloat(r1.xyz) + vd_asfloat(r0.xyw));
  return;
}
