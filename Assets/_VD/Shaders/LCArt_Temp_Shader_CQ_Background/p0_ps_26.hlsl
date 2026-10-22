// LCArt/Temp/Shader_CQ_Background | pass 0 "ForwardLit" | progFragment | keywords: FOG_LINEAR _ADDITIONAL_LIGHTS _LIGHT_LAYERS | blob 26
void frag(
  float2 v0 : TEXCOORD0,
  float w0 : TEXCOORD5,
  float4 v1 : TEXCOORD1,
  float4 v2 : TEXCOORD2,
  float3 v3 : TEXCOORD7,
  float4 v4 : SV_POSITION0,
  float4 v5 : COLOR0,
  out float4 o0 : SV_Target0)
{
  const float4 icb[] = { { 1.000000, 0, 0, 0},
                              { 0, 1.000000, 0, 0},
                              { 0, 0, 1.000000, 0},
                              { 0, 0, 0, 1.000000} };
  float4 r0,r1,r2,r3,r4,r5,r6;
  uint4 bitmask, uiDest;
  float4 fDest;

  r0.xyzw = _BaseMap.SampleBias(sampler_BaseMap, v0.xy, _GlobalMipBias.x).xyzw;
  r1.xyzw = _BaseColor.wxyz * r0.wxyz;
  r0.x = r0.w * _BaseColor.w + -0.5;
  r0.x = cmp(r0.x < 0);
  if (r0.x != 0) discard;
  r0.x = dot(v2.xyz, v2.xyz);
  r0.x = rsqrt(r0.x);
  r0.xyz = v2.xyz * r0.xxx;
  r2.x = (VD_COL(unity_MatrixV, 1).z) * v1.y;
  r2.x = (VD_COL(unity_MatrixV, 0).z) * v1.x + r2.x;
  r2.x = (VD_COL(unity_MatrixV, 2).z) * v1.z + r2.x;
  r2.x = (VD_COL(unity_MatrixV, 3).z) + r2.x;
  r2.x = -_ProjectionParams.y + -r2.x;
  r2.x = max(0, r2.x);
  r2.x = saturate(r2.x * unity_FogParams.z + unity_FogParams.w);
  r0.w = 1;
  r3.x = dot(unity_SHAr.xyzw, r0.xyzw);
  r3.y = dot(unity_SHAg.xyzw, r0.xyzw);
  r3.z = dot(unity_SHAb.xyzw, r0.xyzw);
  r4.xyzw = r0.xyzz * r0.yzzx;
  r5.x = dot(unity_SHBr.xyzw, r4.xyzw);
  r5.y = dot(unity_SHBg.xyzw, r4.xyzw);
  r5.z = dot(unity_SHBb.xyzw, r4.xyzw);
  r0.w = r0.y * r0.y;
  r0.w = r0.x * r0.x + -r0.w;
  r2.yzw = unity_SHC.xyz * r0.www + r5.xyz;
  r2.yzw = r3.xyz + r2.yzw;
  r0.w = vd_asint(_MainLightLayerMask) & vd_asint(unity_RenderingLayer.x);
  r3.xyz = unity_LightData.zzz * _MainLightColor.xyz;
  r3.w = saturate(dot(r0.xyz, _MainLightPosition.xyz));
  r4.x = _ShadeSmoothEnd + -_ShadeSmoothStart;
  r3.w = -_ShadeSmoothStart + r3.w;
  r4.x = 1 / r4.x;
  r3.w = saturate(r4.x * r3.w);
  r4.y = r3.w * -2 + 3;
  r3.w = r3.w * r3.w;
  r4.z = r4.y * r3.w;
  r3.w = -r4.y * r3.w + 1;
  r3.w = _ShadeAlpha * r3.w + r4.z;
  r3.xyz = r3.xyz * r3.www;
  r3.xyz = r3.xyz * r1.yzw;
  r3.xyz = r0.www ? r3.xyz : float3(0,0,0);
  r0.w = min(unity_LightData.y, _AdditionalLightsCount.x);
  r0.w = (int)r0.w;
  r4.yzw = float3(0,0,0);
  r3.w = 0;
  while (true) {
    r5.x = cmp((uint)r3.w >= (uint)r0.w);
    if (r5.x != 0) break;
    r5.x = (uint)r3.w >> 2;
    r5.y = (int)r3.w & 3;
    r5.x = dot(unity_LightIndices[(int)(r5.x)].xyzw, icb[r5.y+0].xyzw);
    r5.x = (int)r5.x;
    r5.yzw = -v1.xyz * _AdditionalLightsPosition[(int)(r5.x)].www + _AdditionalLightsPosition[(int)(r5.x)].xyz;
    r6.x = dot(r5.yzw, r5.yzw);
    r6.x = max(6.10351563e-005, r6.x);
    r6.y = rsqrt(r6.x);
    r5.yzw = r6.yyy * r5.yzw;
    r6.y = 1 / r6.x;
    r6.x = _AdditionalLightsAttenuation[(int)(r5.x)].x * r6.x;
    r6.x = -r6.x * r6.x + 1;
    r6.x = max(0, r6.x);
    r6.x = r6.x * r6.x;
    r6.x = r6.y * r6.x;
    r6.y = dot(_AdditionalLightsSpotDir[(int)(r5.x)].xyz, r5.yzw);
    r6.y = saturate(r6.y * _AdditionalLightsAttenuation[(int)(r5.x)].z + _AdditionalLightsAttenuation[(int)(r5.x)].w);
    r6.y = r6.y * r6.y;
    r6.x = r6.x * r6.y;
    r6.y = vd_asint(unity_RenderingLayer.x) & vd_asint(_AdditionalLightsLayerMasks[(int)(r5.x)]);
    r6.xzw = _AdditionalLightsColor[(int)(r5.x)].xyz * r6.xxx;
    r5.x = saturate(dot(r0.xyz, r5.yzw));
    r5.x = -_ShadeSmoothStart + r5.x;
    r5.x = saturate(r5.x * r4.x);
    r5.y = r5.x * -2 + 3;
    r5.x = r5.x * r5.x;
    r5.z = r5.y * r5.x;
    r5.x = -r5.y * r5.x + 1;
    r5.x = _ShadeAlpha * r5.x + r5.z;
    r5.xyz = r6.xzw * r5.xxx;
    r5.xyz = r5.xyz * r1.yzw + r4.yzw;
    r4.yzw = r6.yyy ? r5.xyz : r4.yzw;
    r3.w = (int)r3.w + 1;
  }
  r0.xyz = r2.yzw * r1.yzw + r3.xyz;
  r0.xyz = r0.xyz + r4.yzw;
  r0.w = 1 + -r2.x;
  r1.yzw = unity_FogColor.xyz * r0.www;
  o0.xyz = r0.xyz * r2.xxx + r1.yzw;
  r0.x = cmp(_Surface == 1.000000);
  o0.w = r0.x ? r1.x : 1;
  return;
}
