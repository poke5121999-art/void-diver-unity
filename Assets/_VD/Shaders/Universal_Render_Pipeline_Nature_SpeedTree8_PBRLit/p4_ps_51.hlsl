// Universal Render Pipeline/Nature/SpeedTree8_PBRLit | pass 4 "Meta" | progFragment | keywords: BACKFACE_NORMAL_MODE_MIRROR | blob 51
void frag(
  float4 v0 : SV_POSITION0,
  float4 v1 : VDREG1,
  float4 v2 : VDREG2,
  float4 v3 : VDREG3,
  float4 v4 : VDREG4,
  float4 v5 : VDREG5,
  float4 v6 : VDREG6,
  float4 v7 : VDREG7,
  float4 v8 : VDREG8,
  float4 v9 : VDREG9,
  uint v10 : SV_IsFrontFace0,
  out float4 o0 : SV_TARGET0)
{
  uint4 r0, r1, r2, r3, r4, r5;
  r0 = 0u; r1 = 0u; r2 = 0u; r3 = 0u; r4 = 0u; r5 = 0u;

  r0.xyzw = vd_asuint(_MainTex.SampleBias(sampler_MainTex, vd_asfloat(v2.xy), vd_asfloat(_GlobalMipBias.x)).xyzw);
  r0.w = vd_asuint((vd_asfloat(v5.w) * vd_asfloat(r0.w) + (-(vd_asfloat(_AlphaClipThreshold)))));
  r0.w = vd_asuint((-(int)(vd_asfloat(r0.w) < 0.0)));
  if (r0.w != 0u) discard;
  r1.xyz = vd_asuint(((-(vd_asfloat(_HueVariationColor.xyz))) + float3(1.0, 1.0, 1.0)));
  r2.xyz = vd_asuint(((-(vd_asfloat(_Color.xyz))) * vd_asfloat(r0.xyz) + float3(1.0, 1.0, 1.0)));
  r2.xyz = vd_asuint((vd_asfloat(r2.xyz) + vd_asfloat(r2.xyz)));
  r1.xyz = vd_asuint(((-(vd_asfloat(r2.xyz))) * vd_asfloat(r1.xyz) + float3(1.0, 1.0, 1.0)));
  r2.xyz = vd_asuint((vd_asfloat(r0.xyz) * vd_asfloat(_Color.xyz)));
  r3.xyz = vd_asuint((-(int3)(float3(0.5, 0.5, 0.5) >= vd_asfloat(r2.xyz))));
  r4.xyz = ((r3.xyz != 0u) ? uint3(0x00000000u, 0x00000000u, 0x00000000u) : uint3(0x3f800000u, 0x3f800000u, 0x3f800000u));
  r3.xyz = (r3.xyz & uint3(0x3f800000u, 0x3f800000u, 0x3f800000u));
  r1.xyz = vd_asuint((vd_asfloat(r1.xyz) * vd_asfloat(r4.xyz)));
  r4.xyz = vd_asuint((vd_asfloat(r2.xyz) * vd_asfloat(_HueVariationColor.xyz)));
  r4.xyz = vd_asuint((vd_asfloat(r4.xyz) + vd_asfloat(r4.xyz)));
  r1.xyz = vd_asuint((vd_asfloat(r4.xyz) * vd_asfloat(r3.xyz) + vd_asfloat(r1.xyz)));
  r1.xyz = vd_asuint(((-(vd_asfloat(_Color.xyz))) * vd_asfloat(r0.xyz) + vd_asfloat(r1.xyz)));
  r0.xyz = vd_asuint(((-(vd_asfloat(_Color.xyz))) * vd_asfloat(r0.xyz) + vd_asfloat(_HueVariationColor.xyz)));
  r0.w = vd_asuint((vd_asfloat((VD_COL(unity_ObjectToWorld, 3).y)) + vd_asfloat((VD_COL(unity_ObjectToWorld, 3).x))));
  r0.w = vd_asuint((vd_asfloat(r0.w) + vd_asfloat((VD_COL(unity_ObjectToWorld, 3).z))));
  r0.w = vd_asuint(frac(vd_asfloat(r0.w)));
  r0.w = vd_asuint(saturate((vd_asfloat(r0.w) * vd_asfloat(_HueVariationColor.w))));
  r1.xyz = vd_asuint((vd_asfloat(r0.www) * vd_asfloat(r1.xyz) + vd_asfloat(r2.xyz)));
  r0.xyz = vd_asuint((vd_asfloat(r0.www) * vd_asfloat(r0.xyz) + vd_asfloat(r2.xyz)));
  r3.xy = vd_asuint((-(int2)(float2(0.0, 0.0) != vd_asfloat(float2(_OldHueVarBehavior, _HueVariationKwToggle)))));
  r0.xyz = ((r3.xxx != 0u) ? r0.xyz : r1.xyz);
  r0.w = vd_asuint(max(vd_asfloat(r0.y), vd_asfloat(r0.x)));
  r0.w = vd_asuint(max(vd_asfloat(r0.z), vd_asfloat(r0.w)));
  r1.x = vd_asuint(max(vd_asfloat(r2.y), vd_asfloat(r2.x)));
  r1.x = vd_asuint(max(vd_asfloat(r2.z), vd_asfloat(r1.x)));
  r0.w = vd_asuint((vd_asfloat(r1.x) / vd_asfloat(r0.w)));
  r0.w = vd_asuint((vd_asfloat(r0.w) * 0.5 + 0.5));
  r0.xyz = vd_asuint(saturate((vd_asfloat(r0.www) * vd_asfloat(r0.xyz))));
  r0.xyz = ((r3.yyy != 0u) ? r0.xyz : r2.xyz);
  r0.xyz = vd_asuint(log2(abs(vd_asfloat(r0.xyz))));
  r0.w = vd_asuint(saturate(vd_asfloat(unity_OneOverOutputBoost)));
  r0.xyz = vd_asuint((vd_asfloat(r0.xyz) * vd_asfloat(r0.www)));
  r0.xyz = vd_asuint(exp2(vd_asfloat(r0.xyz)));
  r0.xyz = vd_asuint(min(vd_asfloat(r0.xyz), vd_asfloat((unity_MaxOutputValue).xxx)));
  r0.w = 0x3f800000u;
  r0.xyzw = ((vd_asuint(unity_MetaFragmentControl.xxxx) != 0u) ? r0.xyzw : uint4(0x00000000u, 0x00000000u, 0x00000000u, 0x00000000u));
  r1.xyz = vd_asuint(((-(vd_asfloat(v6.xyz))) + vd_asfloat(_WorldSpaceCameraPos.xyz)));
  r1.w = vd_asuint(dot(vd_asfloat(r1.xyz), vd_asfloat(r1.xyz)));
  r1.w = vd_asuint(rsqrt(vd_asfloat(r1.w)));
  r1.xyz = vd_asuint((vd_asfloat(r1.www) * vd_asfloat(r1.xyz)));
  r1.w = vd_asuint((-(int)(vd_asfloat(unity_OrthoParams.w) == 0.0)));
  r2.x = ((r1.w != 0u) ? r1.x : vd_asuint((VD_COL(unity_MatrixV, 0).z)));
  r2.y = ((r1.w != 0u) ? r1.y : vd_asuint((VD_COL(unity_MatrixV, 1).z)));
  r2.z = ((r1.w != 0u) ? r1.z : vd_asuint((VD_COL(unity_MatrixV, 2).z)));
  r1.x = vd_asuint(dot(vd_asfloat(_MainLightPosition.xyz), vd_asfloat(r2.xyz)));
  r1.x = vd_asuint(saturate((-(vd_asfloat(r1.x)))));
  r2.xyzw = vd_asuint(_ExtraTex.SampleBias(sampler_ExtraTex, vd_asfloat(v2.xy), vd_asfloat(_GlobalMipBias.x)).xyzw);
  r1.y = vd_asuint((vd_asfloat(r2.x) * vd_asfloat(_Glossiness)));
  r1.z = vd_asuint((-(int)(0.0 != vd_asfloat(EFFECT_EXTRA_TEX))));
  r1.y = ((r1.z != 0u) ? r1.y : vd_asuint(_Glossiness));
  r1.y = vd_asuint(((-(vd_asfloat(r1.y))) * 0.5 + 0.699999988));
  r1.y = vd_asuint((vd_asfloat(r1.y) * vd_asfloat(r1.y)));
  r1.z = vd_asuint((vd_asfloat(r1.x) * vd_asfloat(r1.y) + (-(vd_asfloat(r1.x)))));
  r1.x = vd_asuint((vd_asfloat(r1.z) * vd_asfloat(r1.x) + 1.0));
  r1.x = vd_asuint((vd_asfloat(r1.x) * vd_asfloat(r1.x) + 1.00000001e-07));
  r1.y = vd_asuint((vd_asfloat(r1.y) * 0.318309873));
  r1.x = vd_asuint((vd_asfloat(r1.y) / vd_asfloat(r1.x)));
  r1.xyz = vd_asuint((vd_asfloat(r1.xxx) * vd_asfloat(_MainLightColor.xyz)));
  r2.xyzw = vd_asuint(_SubsurfaceTex.SampleBias(sampler_SubsurfaceTex, vd_asfloat(v2.xy), vd_asfloat(_GlobalMipBias.x)).xyzw);
  r2.xyz = vd_asuint((vd_asfloat(r2.xyz) * vd_asfloat(_SubsurfaceColor.xyz)));
  r1.xyz = vd_asuint((vd_asfloat(r1.xyz) * vd_asfloat(r2.xyz)));
  r1.w = vd_asuint(dot(vd_asfloat(v7.xyz), vd_asfloat(v7.xyz)));
  r1.w = vd_asuint(sqrt(vd_asfloat(r1.w)));
  r1.w = vd_asuint((1.0 / vd_asfloat(r1.w)));
  r3.xyz = vd_asuint((vd_asfloat(r1.www) * vd_asfloat(v7.xyz)));
  r1.w = vd_asuint((vd_asfloat(r3.y) * vd_asfloat(r3.y)));
  r1.w = vd_asuint((vd_asfloat(r3.x) * vd_asfloat(r3.x) + (-(vd_asfloat(r1.w)))));
  r4.xyzw = vd_asuint((vd_asfloat(r3.yzzx) * vd_asfloat(r3.xyzz)));
  r5.x = vd_asuint(dot(vd_asfloat(unity_SHBr.xyzw), vd_asfloat(r4.xyzw)));
  r5.y = vd_asuint(dot(vd_asfloat(unity_SHBg.xyzw), vd_asfloat(r4.xyzw)));
  r5.z = vd_asuint(dot(vd_asfloat(unity_SHBb.xyzw), vd_asfloat(r4.xyzw)));
  r4.xyz = vd_asuint((vd_asfloat(unity_SHC.xyz) * vd_asfloat(r1.www) + vd_asfloat(r5.xyz)));
  r3.w = 0x3f800000u;
  r5.x = vd_asuint(dot(vd_asfloat(unity_SHAr.xyzw), vd_asfloat(r3.xyzw)));
  r5.y = vd_asuint(dot(vd_asfloat(unity_SHAg.xyzw), vd_asfloat(r3.xyzw)));
  r5.z = vd_asuint(dot(vd_asfloat(unity_SHAb.xyzw), vd_asfloat(r3.xyzw)));
  r3.xyz = vd_asuint((vd_asfloat(r4.xyz) + vd_asfloat(r5.xyz)));
  r2.xyz = vd_asuint((vd_asfloat(r2.xyz) * vd_asfloat(r3.xyz)));
  r1.xyz = vd_asuint((vd_asfloat((_SubsurfaceIndirect).xxx) * vd_asfloat(r2.xyz) + vd_asfloat(r1.xyz)));
  r1.w = vd_asuint((-(int)(0.0 != vd_asfloat(_SubsurfaceKwToggle))));
  r1.xyz = (r1.xyz & r1.www);
  r2.xyz = vd_asuint((vd_asfloat(r1.xyz) * vd_asfloat(r1.xyz)));
  r1.w = vd_asuint((-(int)(0.0 != vd_asfloat(unity_UseLinearSpace))));
  r1.xyz = ((r1.www != 0u) ? r1.xyz : r2.xyz);
  r1.w = 0x3f800000u;
  o0.xyzw = vd_asfloat(((vd_asuint(unity_MetaFragmentControl.yyyy) != 0u) ? r1.xyzw : r0.xyzw));
  return;
}
