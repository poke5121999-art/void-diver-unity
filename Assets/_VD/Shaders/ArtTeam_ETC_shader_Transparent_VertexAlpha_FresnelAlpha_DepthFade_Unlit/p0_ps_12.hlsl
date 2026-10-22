// ArtTeam/ETC/shader_Transparent_VertexAlpha_FresnelAlpha_DepthFade_Unlit | pass 0 "Universal Forward" | progFragment | keywords: FOG_LINEAR _SURFACE_TYPE_TRANSPARENT | blob 12
void frag(
  float4 v0 : SV_POSITION0,
  float4 v1 : INTERP0,
  float4 v2 : INTERP1,
  float3 v3 : INTERP2,
  float3 v4 : INTERP3,
  out float4 o0 : SV_Target0)
{
  float4 r0,r1,r2,r3,r4,r5,r6;
  uint4 bitmask, uiDest;
  float4 fDest;

  r0.x = dot(v4.xyz, v4.xyz);
  r0.x = sqrt(r0.x);
  r0.x = 1 / r0.x;
  r0.xyz = v4.xyz * r0.xxx;
  r1.xy = cmp(unity_OrthoParams.ww == float2(0,1));
  r2.xyz = _WorldSpaceCameraPos.xyz + -v3.xyz;
  r0.w = dot(r2.xyz, r2.xyz);
  r0.w = rsqrt(r0.w);
  r2.xyz = r2.xyz * r0.www;
  r3.x = (VD_COL(unity_MatrixV, 0).z);
  r3.y = (VD_COL(unity_MatrixV, 1).z);
  r3.z = (VD_COL(unity_MatrixV, 2).z);
  r1.xzw = r1.xxx ? r2.xyz : r3.xyz;
  r2.xyz = (VD_COL(unity_WorldToObject, 1).xyz) * v3.yyy;
  r2.xyz = (VD_COL(unity_WorldToObject, 0).xyz) * v3.xxx + r2.xyz;
  r2.xyz = (VD_COL(unity_WorldToObject, 2).xyz) * v3.zzz + r2.xyz;
  r2.xyz = (VD_COL(unity_WorldToObject, 3).xyz) + r2.xyz;
  r0.w = (VD_COL(unity_MatrixVP, 1).w) * v3.y;
  r0.w = (VD_COL(unity_MatrixVP, 0).w) * v3.x + r0.w;
  r0.w = (VD_COL(unity_MatrixVP, 2).w) * v3.z + r0.w;
  r0.w = (VD_COL(unity_MatrixVP, 3).w) + r0.w;
  r2.w = cmp(_ProjectionParams.x < 0);
  r3.x = _ScaledScreenParams.y + -v0.y;
  r3.y = r2.w ? r3.x : v0.y;
  r3.x = v0.x;
  r3.xy = r3.xy / _ScaledScreenParams.xy;
  r4.xy = v1.xy * _MainTex_ST.xy + _MainTex_ST.zw;
  r4.xyzw = _MainTex.SampleBias(sampler_MainTex, r4.xy, _GlobalMipBias.x).xyzw;
  r5.xyz = _TintColor.xyz * r4.xyz;
  r6.xyz = (VD_COL(unity_ObjectToWorld, 1).xyz) * r2.yyy;
  r2.xyw = (VD_COL(unity_ObjectToWorld, 0).xyz) * r2.xxx + r6.xyz;
  r2.xyz = (VD_COL(unity_ObjectToWorld, 2).xyz) * r2.zzz + r2.xyw;
  r2.xyz = (VD_COL(unity_ObjectToWorld, 3).xyz) + r2.xyz;
  r2.y = (VD_COL(unity_MatrixV, 1).z) * r2.y;
  r2.x = (VD_COL(unity_MatrixV, 0).z) * r2.x + r2.y;
  r2.x = (VD_COL(unity_MatrixV, 2).z) * r2.z + r2.x;
  r2.x = (VD_COL(unity_MatrixV, 3).z) + r2.x;
  r2.x = -_ProjectionParams.y + -r2.x;
  r2.x = max(0, r2.x);
  r2.x = saturate(r2.x * unity_FogParams.z + unity_FogParams.w);
  r2.x = 1 + -r2.x;
  r2.yzw = -_TintColor.xyz * r4.xyz + unity_FogColor.xyz;
  o0.xyz = r2.xxx * r2.yzw + r5.xyz;
  if (r1.y != 0) {
    r2.xy = -_CameraDepthTexture_TexelSize.xy * float2(0.5,0.5) + float2(1,1);
    r3.z = 1 + -r3.y;
    r2.xy = min(r3.xz, r2.xy);
    r2.xy = _RTHandleScale.xy * r2.xy;
    r2.xyzw = _CameraDepthTexture.SampleBias(sampler_vd_point_clamp, r2.xy, _GlobalMipBias.x).xyzw;
    r2.yz = r3.xz * float2(2,2) + float2(-1,-1);
    r5.xyzw = (VD_COL(unity_MatrixInvVP, 1).xyzw) * -r2.zzzz;
    r5.xyzw = (VD_COL(unity_MatrixInvVP, 0).xyzw) * r2.yyyy + r5.xyzw;
    r2.xyzw = (VD_COL(unity_MatrixInvVP, 2).xyzw) * r2.xxxx + r5.xyzw;
    r2.xyzw = (VD_COL(unity_MatrixInvVP, 3).xyzw) + r2.xyzw;
    r2.xyz = r2.xyz / r2.www;
    r1.y = (VD_COL(unity_MatrixV, 1).z) * r2.y;
    r1.y = (VD_COL(unity_MatrixV, 0).z) * r2.x + r1.y;
    r1.y = (VD_COL(unity_MatrixV, 2).z) * r2.z + r1.y;
    r1.y = (VD_COL(unity_MatrixV, 3).z) + r1.y;
    r1.y = abs(r1.y);
  } else {
    r2.xy = -_CameraDepthTexture_TexelSize.xy * float2(0.5,0.5) + float2(1,1);
    r3.w = 1 + -r3.y;
    r2.xy = min(r3.xw, r2.xy);
    r2.xy = _RTHandleScale.xy * r2.xy;
    r2.xyzw = _CameraDepthTexture.SampleBias(sampler_vd_point_clamp, r2.xy, _GlobalMipBias.x).xyzw;
    r2.x = _ZBufferParams.z * r2.x + _ZBufferParams.w;
    r1.y = 1 / r2.x;
  }
  r0.w = r1.y + -r0.w;
  r0.w = saturate(r0.w / _DepthFadeDistance);
  r0.w = r4.w * r0.w;
  r0.w = _TintColor.w * r0.w;
  r1.y = dot(r0.xyz, r0.xyz);
  r1.y = rsqrt(r1.y);
  r0.xyz = r1.yyy * r0.xyz;
  r0.x = saturate(dot(r0.xyz, r1.xzw));
  r0.x = 1 + -r0.x;
  r0.x = log2(r0.x);
  r0.x = _FresnelPower * r0.x;
  r0.x = exp2(r0.x);
  r0.x = 1 + -r0.x;
  r0.x = r0.w * r0.x + -r0.w;
  r0.x = _FresnelStrength * r0.x + r0.w;
  o0.w = v2.w * r0.x;
  return;
}
