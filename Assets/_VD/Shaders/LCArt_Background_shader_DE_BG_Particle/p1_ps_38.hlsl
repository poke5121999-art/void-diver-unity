// LCArt/Background/shader_DE_BG_Particle | pass 1 "Reflection" | progFragment | keywords: (none) | blob 38
void frag(
  float4 v0 : SV_POSITION0,
  float3 v1 : TEXCOORD1,
  float3 v2 : NORMAL0,
  float2 v3 : TEXCOORD0,
  out float4 o0 : SV_Target0)
{
  float4 r0,r1;
  uint4 bitmask, uiDest;
  float4 fDest;

  r0.x = 0.075000003 + _PlaneReflectionPivotHeight;
  r0.x = cmp(v1.y < r0.x);
  if (r0.x != 0) {
    if (-1 != 0) discard;
  }
  r0.xyzw = _BaseMap.SampleBias(sampler_BaseMap, v3.xy, _GlobalMipBias.x).xyzw;
  r1.x = -0.100000001 + r0.w;
  r1.x = cmp(r1.x < 0);
  if (r1.x != 0) discard;
  r1.xyzw = _EmissionMap.SampleBias(sampler_vd_point_repeat, v3.xy, _GlobalMipBias.x).xyzw;
  r1.y = 0.5 + -r1.z;
  r1.y = cmp(r1.y < 0);
  if (r1.y != 0) discard;
  r1.y = cmp(0.5 >= r1.z);
  r1.y = r1.y ? 1.000000 : 0;
  r1.x = r1.x * r1.y;
  r1.xyz = r1.xxx * r0.xyz;
  r1.xyz = (_EmissionIntensity).xxx * r1.xyz;
  r0.xyz = r1.xyz * float3(2,2,2) + r0.xyz;
  r1.xyzw = _BaseColor.xyzw * r0.xyzw;
  r0.w = -_PlaneReflectionPivotHeight + v1.y;
  r0.w = saturate(0.5 * r0.w);
  r0.xyz = -r0.xyz * _BaseColor.xyz + _PlaneReflectionBackgroundColor.xyz;
  o0.xyz = r0.www * r0.xyz + r1.xyz;
  o0.w = r1.w;
  return;
}
