// Shader Graphs/shader_GlassReflection | pass 4 "Meta" | progFragment | keywords: (none) | blob 31
void frag(
  float4 v0 : SV_POSITION0,
  float4 v1 : VDREG1,
  float4 v2 : VDREG2,
  float4 v3 : VDREG3,
  float4 v4 : VDREG4,
  float4 v5 : VDREG5,
  out float4 o0 : SV_TARGET0)
{
  uint4 r0, r1, r2;
  r0 = 0u; r1 = 0u; r2 = 0u;

  r0.xyz = vd_asuint(((-(vd_asfloat(v4.xyz))) + vd_asfloat(_WorldSpaceCameraPos.xyz)));
  r0.w = vd_asuint(dot(vd_asfloat(r0.xyz), vd_asfloat(r0.xyz)));
  r0.w = vd_asuint(rsqrt(vd_asfloat(r0.w)));
  r0.xyz = vd_asuint((vd_asfloat(r0.www) * vd_asfloat(r0.xyz)));
  r0.w = vd_asuint((-(int)(vd_asfloat(unity_OrthoParams.w) == 0.0)));
  r0.y = ((r0.w != 0u) ? r0.y : vd_asuint((VD_COL(unity_MatrixV, 1).z)));
  r1.xyz = vd_asuint((vd_asfloat(r0.yyy) * vd_asfloat((VD_COL(unity_WorldToObject, 1).xyz))));
  r0.x = ((r0.w != 0u) ? r0.x : vd_asuint((VD_COL(unity_MatrixV, 0).z)));
  r0.y = ((r0.w != 0u) ? r0.z : vd_asuint((VD_COL(unity_MatrixV, 2).z)));
  r0.xzw = vd_asuint((vd_asfloat((VD_COL(unity_WorldToObject, 0).xyz)) * vd_asfloat(r0.xxx) + vd_asfloat(r1.xyz)));
  r0.xyz = vd_asuint((vd_asfloat((VD_COL(unity_WorldToObject, 2).xyz)) * vd_asfloat(r0.yyy) + vd_asfloat(r0.xzw)));
  r0.w = vd_asuint(dot(vd_asfloat(r0.xyz), vd_asfloat(r0.xyz)));
  r0.w = vd_asuint(rsqrt(vd_asfloat(r0.w)));
  r0.xyz = vd_asuint((vd_asfloat(r0.www) * vd_asfloat(r0.xyz)));
  r0.w = vd_asuint(dot(vd_asfloat(v5.xyz), vd_asfloat(v5.xyz)));
  r0.w = vd_asuint(sqrt(vd_asfloat(r0.w)));
  r0.w = vd_asuint((1.0 / vd_asfloat(r0.w)));
  r1.xyz = vd_asuint((vd_asfloat(r0.www) * vd_asfloat(v5.xyz)));
  r2.x = vd_asuint(dot(vd_asfloat(r1.xyz), vd_asfloat((VD_COL(unity_ObjectToWorld, 0).xyz))));
  r2.y = vd_asuint(dot(vd_asfloat(r1.xyz), vd_asfloat((VD_COL(unity_ObjectToWorld, 1).xyz))));
  r2.z = vd_asuint(dot(vd_asfloat(r1.xyz), vd_asfloat((VD_COL(unity_ObjectToWorld, 2).xyz))));
  r0.w = vd_asuint(dot(vd_asfloat(r2.xyz), vd_asfloat(r2.xyz)));
  r0.w = vd_asuint(rsqrt(vd_asfloat(r0.w)));
  r1.xyz = vd_asuint((vd_asfloat(r0.www) * vd_asfloat(r2.xyz)));
  r0.w = vd_asuint(dot((-(vd_asfloat(r0.xyz))), vd_asfloat(r1.xyz)));
  r0.w = vd_asuint((vd_asfloat(r0.w) + vd_asfloat(r0.w)));
  r0.xyz = vd_asuint((vd_asfloat(r1.xyz) * (-(vd_asfloat(r0.www))) + (-(vd_asfloat(r0.xyz)))));
  r0.xyzw = vd_asuint(unity_SpecCube0.SampleLevel(samplerunity_SpecCube0, vd_asfloat(r0.xyz), 0.0).xyzw);
  r0.w = vd_asuint((vd_asfloat(r0.w) + -1.0));
  r0.w = vd_asuint((vd_asfloat(unity_SpecCube0_HDR.w) * vd_asfloat(r0.w) + 1.0));
  r0.w = vd_asuint(max(vd_asfloat(r0.w), 0.0));
  r0.w = vd_asuint(log2(vd_asfloat(r0.w)));
  r0.w = vd_asuint((vd_asfloat(r0.w) * vd_asfloat(unity_SpecCube0_HDR.y)));
  r0.w = vd_asuint(exp2(vd_asfloat(r0.w)));
  r0.w = vd_asuint((vd_asfloat(r0.w) * vd_asfloat(unity_SpecCube0_HDR.x)));
  r0.xyz = vd_asuint((vd_asfloat(r0.xyz) * vd_asfloat(r0.www)));
  r0.xyz = vd_asuint((vd_asfloat(r0.xyz) * vd_asfloat((_ReflMultiply).xxx)));
  r1.xyz = vd_asuint((vd_asfloat(r0.xyz) * vd_asfloat(r0.xyz)));
  r0.w = vd_asuint((-(int)(0.0 != vd_asfloat(unity_UseLinearSpace))));
  r0.xyz = ((r0.www != 0u) ? r0.xyz : r1.xyz);
  r1.x = vd_asuint((vd_asfloat(v4.y) + (-(vd_asfloat(_GradStartEnd.x)))));
  r1.y = vd_asuint(((-(vd_asfloat(_GradStartEnd.x))) + vd_asfloat(_GradStartEnd.y)));
  r1.x = vd_asuint(saturate((vd_asfloat(r1.x) / vd_asfloat(r1.y))));
  r1.yzw = vd_asuint(((-(vd_asfloat(_Color.xyz))) + vd_asfloat(_GradColor.xyz)));
  r1.xyz = vd_asuint((vd_asfloat(r1.xxx) * vd_asfloat(r1.yzw) + vd_asfloat(_Color.xyz)));
  r1.xyz = vd_asuint(log2(abs(vd_asfloat(r1.xyz))));
  r1.w = vd_asuint(saturate(vd_asfloat(unity_OneOverOutputBoost)));
  r1.xyz = vd_asuint((vd_asfloat(r1.xyz) * vd_asfloat(r1.www)));
  r1.xyz = vd_asuint(exp2(vd_asfloat(r1.xyz)));
  r1.xyz = vd_asuint(min(vd_asfloat(r1.xyz), vd_asfloat((unity_MaxOutputValue).xxx)));
  r1.w = 0x3f800000u;
  r1.xyzw = ((vd_asuint(unity_MetaFragmentControl.xxxx) != 0u) ? r1.xyzw : uint4(0x00000000u, 0x00000000u, 0x00000000u, 0x00000000u));
  r0.w = 0x3f800000u;
  o0.xyzw = vd_asfloat(((vd_asuint(unity_MetaFragmentControl.yyyy) != 0u) ? r0.xyzw : r1.xyzw));
  return;
}
