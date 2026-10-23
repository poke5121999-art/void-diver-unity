// Shader Graphs/Shader_DE_CampaignBackground_Particle | pass 0 "Universal Forward" | progFragment | keywords: FOG_LINEAR | blob 6
void frag(
  float4 v0 : SV_POSITION0,
  float4 v1 : VDREG1,
  float4 v2 : VDREG2,
  out float4 o0 : SV_Target0)
{
  uint4 r0, r1, r2;
  r0 = 0u; r1 = 0u; r2 = 0u;

  r0.x = vd_asuint(dot(vd_asfloat(v2.xyz), vd_asfloat(v2.xyz)));
  r0.x = vd_asuint(sqrt(vd_asfloat(r0.x)));
  r0.x = vd_asuint((1.0 / vd_asfloat(r0.x)));
  r0.xyz = vd_asuint((vd_asfloat(r0.xxx) * vd_asfloat(v2.xyz)));
  r1.xyz = vd_asuint(((-(vd_asfloat(v1.xyz))) + vd_asfloat(_WorldSpaceCameraPos.xyz)));
  r0.w = vd_asuint(dot(vd_asfloat(r1.xyz), vd_asfloat(r1.xyz)));
  r0.w = vd_asuint(rsqrt(vd_asfloat(r0.w)));
  r1.xyz = vd_asuint((vd_asfloat(r0.www) * vd_asfloat(r1.xyz)));
  r0.w = vd_asuint((-(int)(vd_asfloat(unity_OrthoParams.w) == 0.0)));
  r2.x = ((r0.w != 0u) ? r1.x : vd_asuint((VD_COL(unity_MatrixV, 0).z)));
  r2.y = ((r0.w != 0u) ? r1.y : vd_asuint((VD_COL(unity_MatrixV, 1).z)));
  r2.z = ((r0.w != 0u) ? r1.z : vd_asuint((VD_COL(unity_MatrixV, 2).z)));
  r0.x = vd_asuint(dot(vd_asfloat(r0.xyz), vd_asfloat(r2.xyz)));
  r0.x = vd_asuint((vd_asfloat(r0.x) + (-(vd_asfloat(_SmoothEdge1)))));
  r0.y = vd_asuint(((-(vd_asfloat(_SmoothEdge1))) + vd_asfloat(_SmoothEdge2)));
  r0.y = vd_asuint((1.0 / vd_asfloat(r0.y)));
  r0.x = vd_asuint(saturate((vd_asfloat(r0.y) * vd_asfloat(r0.x))));
  r0.y = vd_asuint((vd_asfloat(r0.x) * -2.0 + 3.0));
  r0.x = vd_asuint((vd_asfloat(r0.x) * vd_asfloat(r0.x)));
  r0.x = vd_asuint((vd_asfloat(r0.x) * vd_asfloat(r0.y)));
  r0.y = vd_asuint((vd_asfloat(v1.y) + (-(vd_asfloat(_MinY)))));
  r0.z = vd_asuint(((-(vd_asfloat(_MinY))) + vd_asfloat(_MaxtY)));
  r0.y = vd_asuint(saturate((vd_asfloat(r0.y) / vd_asfloat(r0.z))));
  r1.xyz = vd_asuint((vd_asfloat(_MinColor.xyz) + (-(vd_asfloat(_MaxColor.xyz)))));
  r0.yzw = vd_asuint((vd_asfloat(r0.yyy) * vd_asfloat(r1.xyz) + vd_asfloat(_MaxColor.xyz)));
  o0.xyz = (vd_asfloat(r0.yzw) * vd_asfloat(r0.xxx));
  o0.w = vd_asfloat(0x3f800000u);
  return;
}
