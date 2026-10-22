// Shader Graphs/shader_VertexColor | pass 0 "Universal Forward" | progFragment | keywords: FOG_LINEAR _ADDITIONAL_LIGHTS _LIGHT_LAYERS | blob 14
void frag(
  float4 v0 : SV_POSITION0,
  float3 v1 : INTERP2,
  float4 v2 : INTERP5,
  float4 v3 : INTERP6,
  float4 v4 : INTERP7,
  float3 v5 : INTERP8,
  float3 v6 : INTERP9,
  out float4 o0 : SV_Target0)
{
  const float4 icb[] = { { 1.000000, 0, 0, 0},
                              { 0, 1.000000, 0, 0},
                              { 0, 0, 1.000000, 0},
                              { 0, 0, 0, 1.000000} };
  float4 r0,r1,r2,r3,r4,r5,r6,r7,r8;
  uint4 bitmask, uiDest;
  float4 fDest;

  r0.x = dot(v6.xyz, v6.xyz);
  r0.x = rsqrt(r0.x);
  r0.xyz = v6.xyz * r0.xxx;
  r1.x = cmp(unity_OrthoParams.w == 0.000000);
  r1.yzw = _WorldSpaceCameraPos.xyz + -v5.xyz;
  r2.x = dot(r1.yzw, r1.yzw);
  r2.x = rsqrt(r2.x);
  r1.yzw = r2.xxx * r1.yzw;
  r2.x = (VD_COL(unity_MatrixV, 0).z);
  r2.y = (VD_COL(unity_MatrixV, 1).z);
  r2.z = (VD_COL(unity_MatrixV, 2).z);
  r1.xyz = r1.xxx ? r1.yzw : r2.xyz;
  r1.w = (VD_COL(unity_MatrixV, 1).z) * v5.y;
  r1.w = (VD_COL(unity_MatrixV, 0).z) * v5.x + r1.w;
  r1.w = (VD_COL(unity_MatrixV, 2).z) * v5.z + r1.w;
  r1.w = (VD_COL(unity_MatrixV, 3).z) + r1.w;
  r1.w = -_ProjectionParams.y + -r1.w;
  r1.w = max(0, r1.w);
  r1.w = saturate(r1.w * unity_FogParams.z + unity_FogParams.w);
  r0.w = 1;
  r2.x = dot(unity_SHAr.xyzw, r0.xyzw);
  r2.y = dot(unity_SHAg.xyzw, r0.xyzw);
  r2.z = dot(unity_SHAb.xyzw, r0.xyzw);
  r3.xyzw = r0.xyzz * r0.yzzx;
  r4.x = dot(unity_SHBr.xyzw, r3.xyzw);
  r4.y = dot(unity_SHBg.xyzw, r3.xyzw);
  r4.z = dot(unity_SHBb.xyzw, r3.xyzw);
  r0.w = r0.y * r0.y;
  r0.w = r0.x * r0.x + -r0.w;
  r3.xyz = unity_SHC.xyz * r0.www + r4.xyz;
  r2.xyz = r3.xyz + r2.xyz;
  r3.xyz = float3(0.959999979,0.959999979,0.959999979) * v3.xyz;
  r0.w = dot(-r1.xyz, r0.xyz);
  r0.w = r0.w + r0.w;
  r4.xyz = r0.xyz * -r0.www + -r1.xyz;
  r0.w = saturate(dot(r0.xyz, r1.xyz));
  r0.w = 1 + -r0.w;
  r0.w = r0.w * r0.w;
  r0.w = r0.w * r0.w;
  r4.xyzw = unity_SpecCube0.SampleLevel(samplerunity_SpecCube0, r4.xyz, 4.05000019).xyzw;
  r2.w = -1 + r4.w;
  r2.w = unity_SpecCube0_HDR.w * r2.w + 1;
  r2.w = max(0, r2.w);
  r2.w = log2(r2.w);
  r2.w = unity_SpecCube0_HDR.y * r2.w;
  r2.w = exp2(r2.w);
  r2.w = unity_SpecCube0_HDR.x * r2.w;
  r4.xyz = r2.www * r4.xyz;
  r0.w = r0.w * 0.5 + 0.0399999991;
  r0.w = 0.941176474 * r0.w;
  r4.xyz = r4.xyz * r0.www;
  r2.xyz = r2.xyz * r3.xyz + r4.xyz;
  r0.w = vd_asint(_MainLightLayerMask) & vd_asint(unity_RenderingLayer.x);
  r2.w = saturate(dot(r0.xyz, _MainLightPosition.xyz));
  r2.w = unity_LightData.z * r2.w;
  r4.xyz = _MainLightColor.xyz * r2.www;
  r5.xyz = _MainLightPosition.xyz + r1.xyz;
  r2.w = dot(r5.xyz, r5.xyz);
  r2.w = max(1.17549435e-038, r2.w);
  r2.w = rsqrt(r2.w);
  r5.xyz = r5.xyz * r2.www;
  r2.w = saturate(dot(r0.xyz, r5.xyz));
  r3.w = saturate(dot(_MainLightPosition.xyz, r5.xyz));
  r2.w = r2.w * r2.w;
  r2.w = r2.w * -0.9375 + 1.00001001;
  r3.w = r3.w * r3.w;
  r2.w = r2.w * r2.w;
  r3.w = max(0.100000001, r3.w);
  r2.w = r3.w * r2.w;
  r2.w = 3 * r2.w;
  r2.w = 0.0625 / r2.w;
  r5.xyz = r2.www * float3(0.0399999991,0.0399999991,0.0399999991) + r3.xyz;
  r4.xyz = r5.xyz * r4.xyz;
  r4.xyz = r0.www ? r4.xyz : float3(0,0,0);
  r0.w = min(unity_LightData.y, _AdditionalLightsCount.x);
  r0.w = (int)r0.w;
  r5.xyz = float3(0,0,0);
  r2.w = 0;
  while (true) {
    r3.w = cmp((uint)r2.w >= (uint)r0.w);
    if (r3.w != 0) break;
    r3.w = (uint)r2.w >> 2;
    r4.w = (int)r2.w & 3;
    r3.w = dot(unity_LightIndices[(int)(r3.w)].xyzw, icb[r4.w+0].xyzw);
    r3.w = (int)r3.w;
    r6.xyz = -v5.xyz * _AdditionalLightsPosition[(int)(r3.w)].www + _AdditionalLightsPosition[(int)(r3.w)].xyz;
    r4.w = dot(r6.xyz, r6.xyz);
    r4.w = max(6.10351563e-005, r4.w);
    r5.w = rsqrt(r4.w);
    r7.xyz = r6.xyz * r5.www;
    r6.w = 1 / r4.w;
    r4.w = _AdditionalLightsAttenuation[(int)(r3.w)].x * r4.w;
    r4.w = -r4.w * r4.w + 1;
    r4.w = max(0, r4.w);
    r4.w = r4.w * r4.w;
    r4.w = r6.w * r4.w;
    r6.w = dot(_AdditionalLightsSpotDir[(int)(r3.w)].xyz, r7.xyz);
    r6.w = saturate(r6.w * _AdditionalLightsAttenuation[(int)(r3.w)].z + _AdditionalLightsAttenuation[(int)(r3.w)].w);
    r6.w = r6.w * r6.w;
    r4.w = r6.w * r4.w;
    r6.w = vd_asint(unity_RenderingLayer.x) & vd_asint(_AdditionalLightsLayerMasks[(int)(r3.w)]);
    r7.w = saturate(dot(r0.xyz, r7.xyz));
    r4.w = r7.w * r4.w;
    r8.xyz = _AdditionalLightsColor[(int)(r3.w)].xyz * r4.www;
    r6.xyz = r6.xyz * r5.www + r1.xyz;
    r3.w = dot(r6.xyz, r6.xyz);
    r3.w = max(1.17549435e-038, r3.w);
    r3.w = rsqrt(r3.w);
    r6.xyz = r6.xyz * r3.www;
    r3.w = saturate(dot(r0.xyz, r6.xyz));
    r4.w = saturate(dot(r7.xyz, r6.xyz));
    r3.w = r3.w * r3.w;
    r3.w = r3.w * -0.9375 + 1.00001001;
    r4.w = r4.w * r4.w;
    r3.w = r3.w * r3.w;
    r4.w = max(0.100000001, r4.w);
    r3.w = r4.w * r3.w;
    r3.w = 3 * r3.w;
    r3.w = 0.0625 / r3.w;
    r6.xyz = r3.www * float3(0.0399999991,0.0399999991,0.0399999991) + r3.xyz;
    r6.xyz = r6.xyz * r8.xyz + r5.xyz;
    r5.xyz = r6.www ? r6.xyz : r5.xyz;
    r2.w = (int)r2.w + 1;
  }
  r0.xyz = r4.xyz + r2.xyz;
  r0.xyz = r0.xyz + r5.xyz;
  r0.w = 1 + -r1.w;
  r1.xyz = unity_FogColor.xyz * r0.www;
  o0.xyz = r0.xyz * r1.www + r1.xyz;
  o0.w = 1;
  return;
}
