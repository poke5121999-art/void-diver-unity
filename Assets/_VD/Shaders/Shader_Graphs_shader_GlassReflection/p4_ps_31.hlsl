// Shader Graphs/shader_GlassReflection | pass 4 "Meta" | progFragment | keywords: (none) | blob 31
void frag(
  float4 v0 : SV_POSITION0,
  float4 v1 : INTERP0,
  float4 v2 : INTERP1,
  float4 v3 : INTERP2,
  float3 v4 : INTERP3,
  float3 v5 : INTERP4,
  out float4 o0 : SV_TARGET0)
{
  float4 r0,r1,r2;
  uint4 bitmask, uiDest;
  float4 fDest;

  r0.xyz = _WorldSpaceCameraPos.xyz + -v4.xyz;
  r0.w = dot(r0.xyz, r0.xyz);
  r0.w = rsqrt(r0.w);
  r0.xyz = r0.xyz * r0.www;
  r0.w = cmp(unity_OrthoParams.w == 0.000000);
  r0.y = r0.w ? r0.y : (VD_COL(unity_MatrixV, 1).z);
  r1.xyz = (VD_COL(unity_WorldToObject, 1).xyz) * r0.yyy;
  r0.x = r0.w ? r0.x : (VD_COL(unity_MatrixV, 0).z);
  r0.y = r0.w ? r0.z : (VD_COL(unity_MatrixV, 2).z);
  r0.xzw = (VD_COL(unity_WorldToObject, 0).xyz) * r0.xxx + r1.xyz;
  r0.xyz = (VD_COL(unity_WorldToObject, 2).xyz) * r0.yyy + r0.xzw;
  r0.w = dot(r0.xyz, r0.xyz);
  r0.w = rsqrt(r0.w);
  r0.xyz = r0.xyz * r0.www;
  r0.w = dot(v5.xyz, v5.xyz);
  r0.w = sqrt(r0.w);
  r0.w = 1 / r0.w;
  r1.xyz = v5.xyz * r0.www;
  r2.x = dot(r1.xyz, (VD_COL(unity_ObjectToWorld, 0).xyz));
  r2.y = dot(r1.xyz, (VD_COL(unity_ObjectToWorld, 1).xyz));
  r2.z = dot(r1.xyz, (VD_COL(unity_ObjectToWorld, 2).xyz));
  r0.w = dot(r2.xyz, r2.xyz);
  r0.w = rsqrt(r0.w);
  r1.xyz = r2.xyz * r0.www;
  r0.w = dot(-r0.xyz, r1.xyz);
  r0.w = r0.w + r0.w;
  r0.xyz = r1.xyz * -r0.www + -r0.xyz;
  r0.xyzw = unity_SpecCube0.SampleLevel(samplerunity_SpecCube0, r0.xyz, 0).xyzw;
  r0.w = -1 + r0.w;
  r0.w = unity_SpecCube0_HDR.w * r0.w + 1;
  r0.w = max(0, r0.w);
  r0.w = log2(r0.w);
  r0.w = unity_SpecCube0_HDR.y * r0.w;
  r0.w = exp2(r0.w);
  r0.w = unity_SpecCube0_HDR.x * r0.w;
  r0.xyz = r0.www * r0.xyz;
  r0.xyz = (_ReflMultiply).xxx * r0.xyz;
  r1.xyz = r0.xyz * r0.xyz;
  r0.w = cmp(0 != unity_UseLinearSpace);
  r0.xyz = r0.www ? r0.xyz : r1.xyz;
  r1.x = -_GradStartEnd.x + v4.y;
  r1.y = _GradStartEnd.y + -_GradStartEnd.x;
  r1.x = saturate(r1.x / r1.y);
  r1.yzw = _GradColor.xyz + -_Color.xyz;
  r1.xyz = r1.xxx * r1.yzw + _Color.xyz;
  r1.xyz = log2(abs(r1.xyz));
  r1.w = saturate(unity_OneOverOutputBoost);
  r1.xyz = r1.www * r1.xyz;
  r1.xyz = exp2(r1.xyz);
  r1.xyz = min((unity_MaxOutputValue).xxx, r1.xyz);
  r1.w = 1;
  r1.xyzw = unity_MetaFragmentControl.xxxx ? r1.xyzw : float4(0,0,0,0);
  r0.w = 1;
  o0.xyzw = unity_MetaFragmentControl.yyyy ? r0.xyzw : r1.xyzw;
  return;
}
