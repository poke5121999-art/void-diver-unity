// Shader Graphs/shader_DE_BG_ReflectionSurface_Floor | pass 3 "DecalMeshForwardEmissive" | progFragment | keywords: FOG_LINEAR | blob 31
void frag(
  float4 v0 : SV_POSITION0,
  float4 v1 : VDREG1,
  float4 v2 : VDREG2,
  float4 v3 : VDREG3,
  float4 v4 : VDREG4,
  out float4 o0 : SV_Target0)
{
  uint4 r0, r1;
  r0 = 0u; r1 = 0u;

  r0.xyz = vd_asuint((vd_asfloat(v3.yyy) * vd_asfloat((VD_COL(unity_WorldToObject, 1).xyz))));
  r0.xyz = vd_asuint((vd_asfloat((VD_COL(unity_WorldToObject, 0).xyz)) * vd_asfloat(v3.xxx) + vd_asfloat(r0.xyz)));
  r0.xyz = vd_asuint((vd_asfloat((VD_COL(unity_WorldToObject, 2).xyz)) * vd_asfloat(v3.zzz) + vd_asfloat(r0.xyz)));
  r0.xyz = vd_asuint((vd_asfloat(r0.xyz) + vd_asfloat((VD_COL(unity_WorldToObject, 3).xyz))));
  r1.xyz = vd_asuint((vd_asfloat(r0.yyy) * vd_asfloat((VD_COL(unity_ObjectToWorld, 1).xyz))));
  r0.xyw = vd_asuint((vd_asfloat((VD_COL(unity_ObjectToWorld, 0).xyz)) * vd_asfloat(r0.xxx) + vd_asfloat(r1.xyz)));
  r0.xyz = vd_asuint((vd_asfloat((VD_COL(unity_ObjectToWorld, 2).xyz)) * vd_asfloat(r0.zzz) + vd_asfloat(r0.xyw)));
  r0.xyz = vd_asuint((vd_asfloat(r0.xyz) + vd_asfloat((VD_COL(unity_ObjectToWorld, 3).xyz))));
  r0.y = vd_asuint((vd_asfloat(r0.y) * vd_asfloat((VD_COL(unity_MatrixV, 1).z))));
  r0.x = vd_asuint((vd_asfloat((VD_COL(unity_MatrixV, 0).z)) * vd_asfloat(r0.x) + vd_asfloat(r0.y)));
  r0.x = vd_asuint((vd_asfloat((VD_COL(unity_MatrixV, 2).z)) * vd_asfloat(r0.z) + vd_asfloat(r0.x)));
  r0.x = vd_asuint((vd_asfloat(r0.x) + vd_asfloat((VD_COL(unity_MatrixV, 3).z))));
  r0.x = vd_asuint(((-(vd_asfloat(r0.x))) + (-(vd_asfloat(_ProjectionParams.y)))));
  r0.x = vd_asuint(max(vd_asfloat(r0.x), 0.0));
  r0.x = vd_asuint(saturate((vd_asfloat(r0.x) * vd_asfloat(unity_FogParams.z) + vd_asfloat(unity_FogParams.w))));
  r0.x = vd_asuint(((-(vd_asfloat(r0.x))) + 1.0));
  r0.x = vd_asuint(((-(vd_asfloat(r0.x))) + 1.0));
  r0.y = vd_asuint((-(int)(vd_asfloat(_ProjectionParams.x) < 0.0)));
  r0.z = vd_asuint(((-(vd_asfloat(v0.y))) + vd_asfloat(_ScreenParams.y)));
  r1.y = ((r0.y != 0u) ? r0.z : vd_asuint(v0.y));
  r1.x = vd_asuint(v0.x);
  r1.xy = vd_asuint((vd_asfloat(r1.xy) / vd_asfloat(_ScreenParams.xy)));
  r1.z = vd_asuint(((-(vd_asfloat(r1.y))) + 1.0));
  r1.xyzw = vd_asuint(_PlaneReflectionTexture.SampleBias(sampler_PlaneReflectionTexture, vd_asfloat(r1.xz), vd_asfloat(_GlobalMipBias.x)).xyzw);
  r0.yzw = vd_asuint((vd_asfloat(r1.xyz) + (-(vd_asfloat(unity_FogColor.xyz)))));
  r0.xyz = vd_asuint((vd_asfloat(r0.xxx) * vd_asfloat(r0.yzw) + vd_asfloat(unity_FogColor.xyz)));
  r0.xyz = vd_asuint((-(int3)(vd_asfloat(r0.xyz) >= float3(1.10000002, 1.10000002, 1.10000002))));
  o0.xyz = vd_asfloat((r0.xyz & uint3(0x3f000000u, 0x3f000000u, 0x3f000000u)));
  r0.x = vd_asuint(((-(vd_asfloat(_GlobalBackgroundColorIntensity))) + 1.0));
  r0.x = vd_asuint((vd_asfloat(r0.x) * vd_asfloat(_PlaneReflectionActive)));
  r0.x = vd_asuint((vd_asfloat(r1.w) * vd_asfloat(r0.x)));
  o0.w = (vd_asfloat(r0.x) * vd_asfloat(_Opacity));
  return;
}
