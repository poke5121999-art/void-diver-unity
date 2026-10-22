// Shader Graphs/Shader_DE_GroundNavigation | pass 0 "Universal Forward" | progFragment | keywords: _ADDITIONAL_LIGHTS _LIGHT_LAYERS | blob 13
void frag(
  float4 v0 : SV_POSITION0,
  float3 v1 : INTERP2,
  float4 v2 : INTERP5,
  float4 v3 : INTERP6,
  float4 v4 : INTERP7,
  float4 v5 : INTERP8,
  float3 v6 : INTERP9,
  float3 v7 : INTERP10,
  out float4 o0 : SV_Target0)
{
  const float4 icb[] = { { 1.000000, 0, 0, 0},
                              { 0, 1.000000, 0, 0},
                              { 0, 0, 1.000000, 0},
                              { 0, 0, 0, 1.000000} };
  float4 r0,r1,r2,r3,r4,r5,r6,r7,r8,r9,r10;
  uint4 bitmask, uiDest;
  float4 fDest;

  r0.x = (VD_COL(unity_MatrixVP, 1).w) * v6.y;
  r0.x = (VD_COL(unity_MatrixVP, 0).w) * v6.x + r0.x;
  r0.x = (VD_COL(unity_MatrixVP, 2).w) * v6.z + r0.x;
  r0.x = (VD_COL(unity_MatrixVP, 3).w) + r0.x;
  r0.y = cmp(_ProjectionParams.x < 0);
  r0.z = _ScaledScreenParams.y + -v0.y;
  r1.y = r0.y ? r0.z : v0.y;
  r1.x = v0.x;
  r1.xy = r1.xy / _ScaledScreenParams.xy;
  r2.y = _Speed * _TimeParameters.x;
  r2.x = 0;
  r0.yz = v3.xy + r2.xy;
  r2.xyzw = _MainTex.SampleBias(sampler_MainTex, r0.yz, _GlobalMipBias.x).xyzw;
  r2.xyzw = _Color.xyzw * r2.xyzw;
  r0.y = saturate(20 * v4.y);
  r0.z = r0.y * -2 + 3;
  r0.y = r0.y * r0.y;
  r0.y = r0.z * r0.y;
  r0.z = -1 + v4.y;
  r0.z = saturate(-19.9999962 * r0.z);
  r0.w = r0.z * -2 + 3;
  r0.z = r0.z * r0.z;
  r0.z = r0.w * r0.z;
  r0.y = r0.y * r0.z;
  r0.zw = cmp(unity_OrthoParams.ww == float2(1,0));
  if (r0.z != 0) {
    r3.xy = -_CameraDepthTexture_TexelSize.xy * float2(0.5,0.5) + float2(1,1);
    r1.z = 1 + -r1.y;
    r3.xy = min(r3.xy, r1.xz);
    r3.xy = _RTHandleScale.xy * r3.xy;
    r3.xyzw = _CameraDepthTexture.SampleBias(sampler_vd_point_clamp, r3.xy, _GlobalMipBias.x).xyzw;
    r3.yz = r1.xz * float2(2,2) + float2(-1,-1);
    r4.xyzw = (VD_COL(unity_MatrixInvVP, 1).xyzw) * -r3.zzzz;
    r4.xyzw = (VD_COL(unity_MatrixInvVP, 0).xyzw) * r3.yyyy + r4.xyzw;
    r3.xyzw = (VD_COL(unity_MatrixInvVP, 2).xyzw) * r3.xxxx + r4.xyzw;
    r3.xyzw = (VD_COL(unity_MatrixInvVP, 3).xyzw) + r3.xyzw;
    r3.xyz = r3.xyz / r3.www;
    r0.z = (VD_COL(unity_MatrixV, 1).z) * r3.y;
    r0.z = (VD_COL(unity_MatrixV, 0).z) * r3.x + r0.z;
    r0.z = (VD_COL(unity_MatrixV, 2).z) * r3.z + r0.z;
    r0.z = (VD_COL(unity_MatrixV, 3).z) + r0.z;
    r0.z = abs(r0.z);
  } else {
    r3.xy = -_CameraDepthTexture_TexelSize.xy * float2(0.5,0.5) + float2(1,1);
    r1.w = 1 + -r1.y;
    r1.xy = min(r3.xy, r1.xw);
    r1.xy = _RTHandleScale.xy * r1.xy;
    r1.xyzw = _CameraDepthTexture.SampleBias(sampler_vd_point_clamp, r1.xy, _GlobalMipBias.x).xyzw;
    r1.x = _ZBufferParams.z * r1.x + _ZBufferParams.w;
    r0.z = 1 / r1.x;
  }
  r0.x = r0.z + -r0.x;
  r0.x = abs(r0.x) + abs(r0.x);
  r0.x = min(1, r0.x);
  r0.z = r0.x * -2 + 3;
  r0.x = r0.x * r0.x;
  r0.x = r0.z * r0.x;
  r0.x = r0.y * r0.x;
  r0.y = r2.w * r0.x;
  r1.xyz = r2.xyz * r0.yyy;
  r0.z = cmp(_AlphaToMaskAvailable != 0.000000);
  r1.w = ddx(r0.y);
  r3.x = ddy(r0.y);
  r1.w = abs(r3.x) + abs(r1.w);
  r0.x = r2.w * r0.x + -0.00999999978;
  r2.w = -r1.w * 0.5 + r0.x;
  r1.w = max(9.99999975e-005, r1.w);
  r1.w = r2.w / r1.w;
  r1.w = saturate(1 + r1.w);
  r2.w = -9.99999975e-005 + r1.w;
  r0.x = r0.z ? r2.w : r0.x;
  r0.z = saturate(r0.z ? r1.w : r0.y);
  r0.x = cmp(r0.x < 0);
  if (r0.x != 0) discard;
  r0.x = dot(v7.xyz, v7.xyz);
  r0.x = rsqrt(r0.x);
  r3.xyz = v7.xyz * r0.xxx;
  r4.xyz = _WorldSpaceCameraPos.xyz + -v6.xyz;
  r0.x = dot(r4.xyz, r4.xyz);
  r0.x = rsqrt(r0.x);
  r4.xyz = r4.xyz * r0.xxx;
  r5.x = (VD_COL(unity_MatrixV, 0).z);
  r5.y = (VD_COL(unity_MatrixV, 1).z);
  r5.z = (VD_COL(unity_MatrixV, 2).z);
  r4.xyz = r0.www ? r4.xyz : r5.xyz;
  r3.w = 1;
  r5.x = dot(unity_SHAr.xyzw, r3.xyzw);
  r5.y = dot(unity_SHAg.xyzw, r3.xyzw);
  r5.z = dot(unity_SHAb.xyzw, r3.xyzw);
  r6.xyzw = r3.xyzz * r3.yzzx;
  r7.x = dot(unity_SHBr.xyzw, r6.xyzw);
  r7.y = dot(unity_SHBg.xyzw, r6.xyzw);
  r7.z = dot(unity_SHBb.xyzw, r6.xyzw);
  r0.x = r3.y * r3.y;
  r0.x = r3.x * r3.x + -r0.x;
  r6.xyz = unity_SHC.xyz * r0.xxx + r7.xyz;
  r5.xyz = r6.xyz + r5.xyz;
  r1.xyz = r1.xyz * r0.zzz;
  r1.xyz = float3(0.959999979,0.959999979,0.959999979) * r1.xyz;
  r0.x = dot(-r4.xyz, r3.xyz);
  r0.x = r0.x + r0.x;
  r6.xyz = r3.xyz * -r0.xxx + -r4.xyz;
  r0.x = saturate(dot(r3.xyz, r4.xyz));
  r0.x = 1 + -r0.x;
  r0.x = r0.x * r0.x;
  r0.x = r0.x * r0.x;
  r6.xyzw = unity_SpecCube0.SampleLevel(samplerunity_SpecCube0, r6.xyz, 4.05000019).xyzw;
  r0.w = -1 + r6.w;
  r0.w = unity_SpecCube0_HDR.w * r0.w + 1;
  r0.w = max(0, r0.w);
  r0.w = log2(r0.w);
  r0.w = unity_SpecCube0_HDR.y * r0.w;
  r0.w = exp2(r0.w);
  r0.w = unity_SpecCube0_HDR.x * r0.w;
  r6.xyz = r0.www * r6.xyz;
  r0.x = r0.x * 0.5 + 0.0399999991;
  r0.x = 0.941176474 * r0.x;
  r6.xyz = r6.xyz * r0.xxx;
  r5.xyz = r5.xyz * r1.xyz + r6.xyz;
  r0.x = vd_asint(_MainLightLayerMask) & vd_asint(unity_RenderingLayer.x);
  r0.w = saturate(dot(r3.xyz, _MainLightPosition.xyz));
  r0.w = unity_LightData.z * r0.w;
  r6.xyz = _MainLightColor.xyz * r0.www;
  r7.xyz = _MainLightPosition.xyz + r4.xyz;
  r0.w = dot(r7.xyz, r7.xyz);
  r0.w = max(1.17549435e-038, r0.w);
  r0.w = rsqrt(r0.w);
  r7.xyz = r7.xyz * r0.www;
  r0.w = saturate(dot(r3.xyz, r7.xyz));
  r1.w = saturate(dot(_MainLightPosition.xyz, r7.xyz));
  r0.w = r0.w * r0.w;
  r0.w = r0.w * -0.9375 + 1.00001001;
  r1.w = r1.w * r1.w;
  r0.w = r0.w * r0.w;
  r1.w = max(0.100000001, r1.w);
  r0.w = r1.w * r0.w;
  r0.w = 3 * r0.w;
  r0.w = 0.0625 / r0.w;
  r7.xyz = r0.www * float3(0.0399999991,0.0399999991,0.0399999991) + r1.xyz;
  r6.xyz = r7.xyz * r6.xyz;
  r6.xyz = r0.xxx ? r6.xyz : float3(0,0,0);
  r0.x = min(unity_LightData.y, _AdditionalLightsCount.x);
  r0.x = (int)r0.x;
  r7.xyz = float3(0,0,0);
  r0.w = 0;
  while (true) {
    r1.w = cmp((uint)r0.w >= (uint)r0.x);
    if (r1.w != 0) break;
    r1.w = (uint)r0.w >> 2;
    r2.w = (int)r0.w & 3;
    r1.w = dot(unity_LightIndices[(int)(r1.w)].xyzw, icb[r2.w+0].xyzw);
    r1.w = (int)r1.w;
    r8.xyz = -v6.xyz * _AdditionalLightsPosition[(int)(r1.w)].www + _AdditionalLightsPosition[(int)(r1.w)].xyz;
    r2.w = dot(r8.xyz, r8.xyz);
    r2.w = max(6.10351563e-005, r2.w);
    r3.w = rsqrt(r2.w);
    r9.xyz = r8.xyz * r3.www;
    r4.w = 1 / r2.w;
    r2.w = _AdditionalLightsAttenuation[(int)(r1.w)].x * r2.w;
    r2.w = -r2.w * r2.w + 1;
    r2.w = max(0, r2.w);
    r2.w = r2.w * r2.w;
    r2.w = r4.w * r2.w;
    r4.w = dot(_AdditionalLightsSpotDir[(int)(r1.w)].xyz, r9.xyz);
    r4.w = saturate(r4.w * _AdditionalLightsAttenuation[(int)(r1.w)].z + _AdditionalLightsAttenuation[(int)(r1.w)].w);
    r4.w = r4.w * r4.w;
    r2.w = r4.w * r2.w;
    r4.w = vd_asint(unity_RenderingLayer.x) & vd_asint(_AdditionalLightsLayerMasks[(int)(r1.w)]);
    r5.w = saturate(dot(r3.xyz, r9.xyz));
    r2.w = r5.w * r2.w;
    r10.xyz = _AdditionalLightsColor[(int)(r1.w)].xyz * r2.www;
    r8.xyz = r8.xyz * r3.www + r4.xyz;
    r1.w = dot(r8.xyz, r8.xyz);
    r1.w = max(1.17549435e-038, r1.w);
    r1.w = rsqrt(r1.w);
    r8.xyz = r8.xyz * r1.www;
    r1.w = saturate(dot(r3.xyz, r8.xyz));
    r2.w = saturate(dot(r9.xyz, r8.xyz));
    r1.w = r1.w * r1.w;
    r1.w = r1.w * -0.9375 + 1.00001001;
    r2.w = r2.w * r2.w;
    r1.w = r1.w * r1.w;
    r2.w = max(0.100000001, r2.w);
    r1.w = r2.w * r1.w;
    r1.w = 3 * r1.w;
    r1.w = 0.0625 / r1.w;
    r8.xyz = r1.www * float3(0.0399999991,0.0399999991,0.0399999991) + r1.xyz;
    r8.xyz = r8.xyz * r10.xyz + r7.xyz;
    r7.xyz = r4.www ? r8.xyz : r7.xyz;
    r0.w = (int)r0.w + 1;
  }
  r1.xyz = r6.xyz + r5.xyz;
  r1.xyz = r1.xyz + r7.xyz;
  o0.xyz = r2.xyz * r0.yyy + r1.xyz;
  o0.w = r0.z;
  return;
}
