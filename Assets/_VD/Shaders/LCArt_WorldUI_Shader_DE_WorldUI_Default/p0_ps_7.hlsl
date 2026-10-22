// LCArt/WorldUI/Shader_DE_WorldUI_Default | pass 0 "Default" | progFragment | keywords: (none) | blob 7
void frag(
  float4 v0 : SV_POSITION0,
  float4 v1 : COLOR0,
  float2 v2 : TEXCOORD0,
  float w2 : TEXCOORD2,
  float4 v3 : TEXCOORD1,
  out float4 o0 : SV_Target0)
{
  float4 r0;
  uint4 bitmask, uiDest;
  float4 fDest;

  r0.x = cmp(_GlobalBoxAreaEnable >= 0.5);
  if (r0.x != 0) {
    r0.x = cmp(_GlobalBoxAreaCoverWorldUIs[0] >= 0.5);
    r0.yzw = (VD_COL(unity_ObjectToWorld, 3).xyz) + -_GlobalBoxAreaCenters[0].xyz;
    r0.yzw = -_GlobalBoxAreaExtents[0].xyz + abs(r0.yzw);
    r0.y = max(r0.y, r0.z);
    r0.y = max(r0.y, r0.w);
    r0.y = cmp(0 >= r0.y);
    r0.x = r0.y ? r0.x : 0;
    if (r0.x != 0) discard;
    r0.x = cmp(_GlobalBoxAreaCoverWorldUIs[1] >= 0.5);
    r0.yzw = (VD_COL(unity_ObjectToWorld, 3).xyz) + -_GlobalBoxAreaCenters[1].xyz;
    r0.yzw = -_GlobalBoxAreaExtents[1].xyz + abs(r0.yzw);
    r0.y = max(r0.y, r0.z);
    r0.y = max(r0.y, r0.w);
    r0.y = cmp(0 >= r0.y);
    r0.x = r0.y ? r0.x : 0;
    if (r0.x != 0) discard;
    r0.x = cmp(_GlobalBoxAreaCoverWorldUIs[2] >= 0.5);
    r0.yzw = (VD_COL(unity_ObjectToWorld, 3).xyz) + -_GlobalBoxAreaCenters[2].xyz;
    r0.yzw = -_GlobalBoxAreaExtents[2].xyz + abs(r0.yzw);
    r0.y = max(r0.y, r0.z);
    r0.y = max(r0.y, r0.w);
    r0.y = cmp(0 >= r0.y);
    r0.x = r0.y ? r0.x : 0;
    if (r0.x != 0) discard;
    r0.x = cmp(_GlobalBoxAreaCoverWorldUIs[3] >= 0.5);
    r0.yzw = (VD_COL(unity_ObjectToWorld, 3).xyz) + -_GlobalBoxAreaCenters[3].xyz;
    r0.yzw = -_GlobalBoxAreaExtents[3].xyz + abs(r0.yzw);
    r0.y = max(r0.y, r0.z);
    r0.y = max(r0.y, r0.w);
    r0.y = cmp(0 >= r0.y);
    r0.x = r0.y ? r0.x : 0;
    if (r0.x != 0) discard;
    r0.x = cmp(_GlobalBoxAreaCoverWorldUIs[4] >= 0.5);
    r0.yzw = (VD_COL(unity_ObjectToWorld, 3).xyz) + -_GlobalBoxAreaCenters[4].xyz;
    r0.yzw = -_GlobalBoxAreaExtents[4].xyz + abs(r0.yzw);
    r0.y = max(r0.y, r0.z);
    r0.y = max(r0.y, r0.w);
    r0.y = cmp(0 >= r0.y);
    r0.x = r0.y ? r0.x : 0;
    if (r0.x != 0) discard;
    r0.x = cmp(_GlobalBoxAreaCoverWorldUIs[5] >= 0.5);
    r0.yzw = (VD_COL(unity_ObjectToWorld, 3).xyz) + -_GlobalBoxAreaCenters[5].xyz;
    r0.yzw = -_GlobalBoxAreaExtents[5].xyz + abs(r0.yzw);
    r0.y = max(r0.y, r0.z);
    r0.y = max(r0.y, r0.w);
    r0.y = cmp(0 >= r0.y);
    r0.x = r0.y ? r0.x : 0;
    if (r0.x != 0) discard;
    r0.x = cmp(_GlobalBoxAreaCoverWorldUIs[6] >= 0.5);
    r0.yzw = (VD_COL(unity_ObjectToWorld, 3).xyz) + -_GlobalBoxAreaCenters[6].xyz;
    r0.yzw = -_GlobalBoxAreaExtents[6].xyz + abs(r0.yzw);
    r0.y = max(r0.y, r0.z);
    r0.y = max(r0.y, r0.w);
    r0.y = cmp(0 >= r0.y);
    r0.x = r0.y ? r0.x : 0;
    if (r0.x != 0) discard;
    r0.x = cmp(_GlobalBoxAreaCoverWorldUIs[7] >= 0.5);
    r0.yzw = (VD_COL(unity_ObjectToWorld, 3).xyz) + -_GlobalBoxAreaCenters[7].xyz;
    r0.yzw = -_GlobalBoxAreaExtents[7].xyz + abs(r0.yzw);
    r0.y = max(r0.y, r0.z);
    r0.y = max(r0.y, r0.w);
    r0.y = cmp(0 >= r0.y);
    r0.x = r0.y ? r0.x : 0;
    if (r0.x != 0) discard;
  }
  r0.xyzw = _MainTex.Sample(sampler_MainTex, v2.xy).xyzw;
  r0.xyzw = _TextureSampleAdd.xyzw + r0.xyzw;
  r0.xyzw = v1.xyzw * r0.xyzw;
  o0.xyz = r0.xyz * r0.www;
  o0.w = r0.w;
  return;
}
