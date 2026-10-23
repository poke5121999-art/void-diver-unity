// LCArt/WorldUI/Shader_DE_WorldUI_Default | pass 0 "Default" | progFragment | keywords: UNITY_UI_ALPHACLIP | blob 8
void frag(
  float4 v0 : SV_POSITION0,
  float4 v1 : VDREG1,
  float4 v2 : VDREG2,
  float4 v3 : VDREG3,
  out float4 o0 : SV_Target0)
{
  uint4 r0, r1;
  r0 = 0u; r1 = 0u;

  r0.x = vd_asuint((-(int)(vd_asfloat(_GlobalBoxAreaEnable) >= 0.5)));
  if (r0.x != 0u) {
    r0.x = vd_asuint((-(int)(vd_asfloat(_GlobalBoxAreaCoverWorldUIs[0]) >= 0.5)));
    r0.yzw = vd_asuint(((-(vd_asfloat(_GlobalBoxAreaCenters[0].xyz))) + vd_asfloat((VD_COL(unity_ObjectToWorld, 3).xyz))));
    r0.yzw = vd_asuint((abs(vd_asfloat(r0.yzw)) + (-(vd_asfloat(_GlobalBoxAreaExtents[0].xyz)))));
    r0.y = vd_asuint(max(vd_asfloat(r0.z), vd_asfloat(r0.y)));
    r0.y = vd_asuint(max(vd_asfloat(r0.w), vd_asfloat(r0.y)));
    r0.y = vd_asuint((-(int)(0.0 >= vd_asfloat(r0.y))));
    r0.x = (r0.y & r0.x);
    if (r0.x != 0u) discard;
    r0.x = vd_asuint((-(int)(vd_asfloat(_GlobalBoxAreaCoverWorldUIs[1]) >= 0.5)));
    r0.yzw = vd_asuint(((-(vd_asfloat(_GlobalBoxAreaCenters[1].xyz))) + vd_asfloat((VD_COL(unity_ObjectToWorld, 3).xyz))));
    r0.yzw = vd_asuint((abs(vd_asfloat(r0.yzw)) + (-(vd_asfloat(_GlobalBoxAreaExtents[1].xyz)))));
    r0.y = vd_asuint(max(vd_asfloat(r0.z), vd_asfloat(r0.y)));
    r0.y = vd_asuint(max(vd_asfloat(r0.w), vd_asfloat(r0.y)));
    r0.y = vd_asuint((-(int)(0.0 >= vd_asfloat(r0.y))));
    r0.x = (r0.y & r0.x);
    if (r0.x != 0u) discard;
    r0.x = vd_asuint((-(int)(vd_asfloat(_GlobalBoxAreaCoverWorldUIs[2]) >= 0.5)));
    r0.yzw = vd_asuint(((-(vd_asfloat(_GlobalBoxAreaCenters[2].xyz))) + vd_asfloat((VD_COL(unity_ObjectToWorld, 3).xyz))));
    r0.yzw = vd_asuint((abs(vd_asfloat(r0.yzw)) + (-(vd_asfloat(_GlobalBoxAreaExtents[2].xyz)))));
    r0.y = vd_asuint(max(vd_asfloat(r0.z), vd_asfloat(r0.y)));
    r0.y = vd_asuint(max(vd_asfloat(r0.w), vd_asfloat(r0.y)));
    r0.y = vd_asuint((-(int)(0.0 >= vd_asfloat(r0.y))));
    r0.x = (r0.y & r0.x);
    if (r0.x != 0u) discard;
    r0.x = vd_asuint((-(int)(vd_asfloat(_GlobalBoxAreaCoverWorldUIs[3]) >= 0.5)));
    r0.yzw = vd_asuint(((-(vd_asfloat(_GlobalBoxAreaCenters[3].xyz))) + vd_asfloat((VD_COL(unity_ObjectToWorld, 3).xyz))));
    r0.yzw = vd_asuint((abs(vd_asfloat(r0.yzw)) + (-(vd_asfloat(_GlobalBoxAreaExtents[3].xyz)))));
    r0.y = vd_asuint(max(vd_asfloat(r0.z), vd_asfloat(r0.y)));
    r0.y = vd_asuint(max(vd_asfloat(r0.w), vd_asfloat(r0.y)));
    r0.y = vd_asuint((-(int)(0.0 >= vd_asfloat(r0.y))));
    r0.x = (r0.y & r0.x);
    if (r0.x != 0u) discard;
    r0.x = vd_asuint((-(int)(vd_asfloat(_GlobalBoxAreaCoverWorldUIs[4]) >= 0.5)));
    r0.yzw = vd_asuint(((-(vd_asfloat(_GlobalBoxAreaCenters[4].xyz))) + vd_asfloat((VD_COL(unity_ObjectToWorld, 3).xyz))));
    r0.yzw = vd_asuint((abs(vd_asfloat(r0.yzw)) + (-(vd_asfloat(_GlobalBoxAreaExtents[4].xyz)))));
    r0.y = vd_asuint(max(vd_asfloat(r0.z), vd_asfloat(r0.y)));
    r0.y = vd_asuint(max(vd_asfloat(r0.w), vd_asfloat(r0.y)));
    r0.y = vd_asuint((-(int)(0.0 >= vd_asfloat(r0.y))));
    r0.x = (r0.y & r0.x);
    if (r0.x != 0u) discard;
    r0.x = vd_asuint((-(int)(vd_asfloat(_GlobalBoxAreaCoverWorldUIs[5]) >= 0.5)));
    r0.yzw = vd_asuint(((-(vd_asfloat(_GlobalBoxAreaCenters[5].xyz))) + vd_asfloat((VD_COL(unity_ObjectToWorld, 3).xyz))));
    r0.yzw = vd_asuint((abs(vd_asfloat(r0.yzw)) + (-(vd_asfloat(_GlobalBoxAreaExtents[5].xyz)))));
    r0.y = vd_asuint(max(vd_asfloat(r0.z), vd_asfloat(r0.y)));
    r0.y = vd_asuint(max(vd_asfloat(r0.w), vd_asfloat(r0.y)));
    r0.y = vd_asuint((-(int)(0.0 >= vd_asfloat(r0.y))));
    r0.x = (r0.y & r0.x);
    if (r0.x != 0u) discard;
    r0.x = vd_asuint((-(int)(vd_asfloat(_GlobalBoxAreaCoverWorldUIs[6]) >= 0.5)));
    r0.yzw = vd_asuint(((-(vd_asfloat(_GlobalBoxAreaCenters[6].xyz))) + vd_asfloat((VD_COL(unity_ObjectToWorld, 3).xyz))));
    r0.yzw = vd_asuint((abs(vd_asfloat(r0.yzw)) + (-(vd_asfloat(_GlobalBoxAreaExtents[6].xyz)))));
    r0.y = vd_asuint(max(vd_asfloat(r0.z), vd_asfloat(r0.y)));
    r0.y = vd_asuint(max(vd_asfloat(r0.w), vd_asfloat(r0.y)));
    r0.y = vd_asuint((-(int)(0.0 >= vd_asfloat(r0.y))));
    r0.x = (r0.y & r0.x);
    if (r0.x != 0u) discard;
    r0.x = vd_asuint((-(int)(vd_asfloat(_GlobalBoxAreaCoverWorldUIs[7]) >= 0.5)));
    r0.yzw = vd_asuint(((-(vd_asfloat(_GlobalBoxAreaCenters[7].xyz))) + vd_asfloat((VD_COL(unity_ObjectToWorld, 3).xyz))));
    r0.yzw = vd_asuint((abs(vd_asfloat(r0.yzw)) + (-(vd_asfloat(_GlobalBoxAreaExtents[7].xyz)))));
    r0.y = vd_asuint(max(vd_asfloat(r0.z), vd_asfloat(r0.y)));
    r0.y = vd_asuint(max(vd_asfloat(r0.w), vd_asfloat(r0.y)));
    r0.y = vd_asuint((-(int)(0.0 >= vd_asfloat(r0.y))));
    r0.x = (r0.y & r0.x);
    if (r0.x != 0u) discard;
  }
  r0.xyzw = vd_asuint(_MainTex.Sample(sampler_MainTex, vd_asfloat(v2.xy)).xyzw);
  r0.xyzw = vd_asuint((vd_asfloat(r0.xyzw) + vd_asfloat(_TextureSampleAdd.xyzw)));
  r1.xyzw = vd_asuint((vd_asfloat(r0.xyzw) * vd_asfloat(v1.xyzw)));
  r0.x = vd_asuint((vd_asfloat(v1.w) * vd_asfloat(r0.w) + -0.00100000005));
  r0.x = vd_asuint((-(int)(vd_asfloat(r0.x) < 0.0)));
  if (r0.x != 0u) discard;
  o0.xyz = (vd_asfloat(r1.www) * vd_asfloat(r1.xyz));
  o0.w = vd_asfloat(r1.w);
  return;
}
