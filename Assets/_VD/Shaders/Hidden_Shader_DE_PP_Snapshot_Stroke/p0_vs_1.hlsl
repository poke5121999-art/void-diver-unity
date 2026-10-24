// Hidden/Shader_DE_PP_Snapshot_Stroke | pass 0 "DrawProcedural" | progVertex | keywords: (none) | blob 1
void vert(
  uint v0 : SV_VertexID0,
  out float4 o0 : SV_POSITION0,
  out float4 o1 : VDREG1,
  out float4 o2 : VDREG2)
{
#if UNITY_UV_STARTS_AT_TOP
#else
  float4x4 vd_MatrixInvVPD3D = mul(unity_MatrixInvVP, float4x4(1, 0, 0, 0, 0, 1, 0, 0, 0, 0, -2, 1, 0, 0, 0, 1));
#endif
  uint4 r0, r1;
  r0 = 0u; r1 = 0u;

  r0.x = (vd_asuint(v0.x) << (uint)1);
  r0.x = (r0.x & 0x00000002u);
  r0.z = (vd_asuint(v0.x) & 0x00000002u);
  r0.xy = vd_asuint(((float2)r0.xz));
  r0.xy = vd_asuint((vd_asfloat(r0.xy) * float2(2.0, 2.0) + float2(-1.0, -1.0)));
  o0.xy = vd_asfloat(r0.xy);
#if UNITY_UV_STARTS_AT_TOP
  o0.zw = vd_asfloat(uint2(0x3f800000u, 0x3f800000u));
#else
  o0.zw = vd_asfloat(uint2(0xbf800000u, 0x3f800000u));
#endif
  r1.xy = vd_asuint((vd_asfloat(r0.xy) * float2(0.5, 0.5)));
  r1.z = 0x3f000000u;
  r1.xyz = vd_asuint((vd_asfloat(r1.xyz) + float3(0.5, 0.5, 0.5)));
  r0.z = vd_asuint(((-(vd_asfloat(r1.y))) + 1.0));
  r0.w = vd_asuint((-(int)(vd_asfloat(_FlipY) < 0.5)));
#if UNITY_UV_STARTS_AT_TOP
  r1.w = ((r0.w != 0u) ? r0.z : r1.y);
#else
  r1.w = ((r0.w != 0u) ? r1.y : r0.z);
#endif
  o1.xyzw = vd_asfloat(r1.xwzz);
#if UNITY_UV_STARTS_AT_TOP
  r1.xyzw = vd_asuint((vd_asfloat(r0.yyyy) * vd_asfloat((VD_COL(unity_MatrixInvVP, 1).xyzw))));
  r0.xyzw = vd_asuint((vd_asfloat((VD_COL(unity_MatrixInvVP, 0).xyzw)) * vd_asfloat(r0.xxxx) + vd_asfloat(r1.xyzw)));
  r0.xyzw = vd_asuint((vd_asfloat(r0.xyzw) + vd_asfloat((VD_COL(unity_MatrixInvVP, 2).xyzw))));
  r0.xyzw = vd_asuint((vd_asfloat(r0.xyzw) + vd_asfloat((VD_COL(unity_MatrixInvVP, 3).xyzw))));
#else
  r1.xyzw = vd_asuint((vd_asfloat(r0.yyyy) * vd_asfloat((VD_COL(vd_MatrixInvVPD3D, 1).xyzw))));
  r0.xyzw = vd_asuint((vd_asfloat((VD_COL(vd_MatrixInvVPD3D, 0).xyzw)) * vd_asfloat(r0.xxxx) + vd_asfloat(r1.xyzw)));
  r0.xyzw = vd_asuint((vd_asfloat(r0.xyzw) + vd_asfloat((VD_COL(vd_MatrixInvVPD3D, 2).xyzw))));
  r0.xyzw = vd_asuint((vd_asfloat(r0.xyzw) + vd_asfloat((VD_COL(vd_MatrixInvVPD3D, 3).xyzw))));
#endif
  r0.xyz = vd_asuint((vd_asfloat(r0.xyz) / vd_asfloat(r0.www)));
  r0.xyz = vd_asuint(((-(vd_asfloat(r0.xyz))) + vd_asfloat(_WorldSpaceCameraPos.xyz)));
  r0.w = vd_asuint((-(int)(vd_asfloat(unity_OrthoParams.w) == 0.0)));
  o2.x = vd_asfloat(((r0.w != 0u) ? r0.x : vd_asuint((VD_COL(unity_MatrixV, 0).z))));
  o2.y = vd_asfloat(((r0.w != 0u) ? r0.y : vd_asuint((VD_COL(unity_MatrixV, 1).z))));
  o2.z = vd_asfloat(((r0.w != 0u) ? r0.z : vd_asuint((VD_COL(unity_MatrixV, 2).z))));
  o2.w = vd_asfloat(0x00000000u);
  return;
}
