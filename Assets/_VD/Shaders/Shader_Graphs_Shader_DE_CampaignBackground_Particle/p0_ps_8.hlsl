// Shader Graphs/Shader_DE_CampaignBackground_Particle | pass 0 "Universal Forward" | progFragment | keywords: FOG_LINEAR _DBUFFER_MRT3 | blob 8
void frag(
  float4 v0 : SV_POSITION0,
  float3 v1 : INTERP0,
  float3 v2 : INTERP1,
  out float4 o0 : SV_Target0)
{
  float4 r0,r1,r2;
  uint4 bitmask, uiDest;
  float4 fDest;

  r0.x = dot(v2.xyz, v2.xyz);
  r0.x = sqrt(r0.x);
  r0.x = 1 / r0.x;
  r0.xyz = v2.xyz * r0.xxx;
  r1.xyz = _WorldSpaceCameraPos.xyz + -v1.xyz;
  r0.w = dot(r1.xyz, r1.xyz);
  r0.w = rsqrt(r0.w);
  r1.xyz = r1.xyz * r0.www;
  r0.w = cmp(unity_OrthoParams.w == 0.000000);
  r2.x = r0.w ? r1.x : (VD_COL(unity_MatrixV, 0).z);
  r2.y = r0.w ? r1.y : (VD_COL(unity_MatrixV, 1).z);
  r2.z = r0.w ? r1.z : (VD_COL(unity_MatrixV, 2).z);
  r0.x = dot(r0.xyz, r2.xyz);
  r0.x = -_SmoothEdge1 + r0.x;
  r0.y = _SmoothEdge2 + -_SmoothEdge1;
  r0.y = 1 / r0.y;
  r0.x = saturate(r0.x * r0.y);
  r0.y = r0.x * -2 + 3;
  r0.x = r0.x * r0.x;
  r0.x = r0.y * r0.x;
  r0.y = -_MinY + v1.y;
  r0.z = _MaxtY + -_MinY;
  r0.y = saturate(r0.y / r0.z);
  r1.xyz = -_MaxColor.xyz + _MinColor.xyz;
  r0.yzw = r0.yyy * r1.xyz + _MaxColor.xyz;
  r0.xyz = r0.xxx * r0.yzw;
  r1.xy = (int2)v0.xy;
  r1.zw = float2(0,0);
  r1.xyzw = _DBufferTexture0.Load(r1.xyz).xyzw;
  o0.xyz = r0.xyz * r1.www + r1.xyz;
  o0.w = 1;
  return;
}
