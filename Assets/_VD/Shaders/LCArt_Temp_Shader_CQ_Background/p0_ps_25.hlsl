// LCArt/Temp/Shader_CQ_Background | pass 0 "ForwardLit" | progFragment | keywords: _ADDITIONAL_LIGHTS _CQ_EDITOR_CAMERA _LIGHT_LAYERS | blob 25
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
  r0.w = vd_asint(_MainLightLayerMask) & vd_asint(unity_RenderingLayer.x);
  r3.xyz = unity_LightData.zzz * _MainLightColor.xyz;
  r2.w = saturate(dot(r0.xyz, _MainLightPosition.xyz));
  r3.w = _ShadeSmoothEnd + -_ShadeSmoothStart;
  r2.w = -_ShadeSmoothStart + r2.w;
  r3.w = 1 / r3.w;
  r2.w = saturate(r3.w * r2.w);
  r4.x = r2.w * -2 + 3;
  r2.w = r2.w * r2.w;
  r4.y = r4.x * r2.w;
  r2.w = -r4.x * r2.w + 1;
  r2.w = _ShadeAlpha * r2.w + r4.y;
  r3.xyz = r3.xyz * r2.www;
  r3.xyz = r3.xyz * r1.yzw;
  r3.xyz = r0.www ? r3.xyz : float3(0,0,0);
  r0.w = min(unity_LightData.y, _AdditionalLightsCount.x);
  r0.w = (int)r0.w;
  r4.xyz = float3(0,0,0);
  r2.w = 0;
  while (true) {
    r4.w = cmp((uint)r2.w >= (uint)r0.w);
    if (r4.w != 0) break;
    r4.w = (uint)r2.w >> 2;
    r5.x = (int)r2.w & 3;
    r4.w = dot(unity_LightIndices[(int)(r4.w)].xyzw, icb[r5.x+0].xyzw);
    r4.w = (int)r4.w;
    r5.xyz = -v1.xyz * _AdditionalLightsPosition[(int)(r4.w)].www + _AdditionalLightsPosition[(int)(r4.w)].xyz;
    r5.w = dot(r5.xyz, r5.xyz);
    r5.w = max(6.10351563e-005, r5.w);
    r6.x = rsqrt(r5.w);
    r5.xyz = r6.xxx * r5.xyz;
    r6.x = 1 / r5.w;
    r5.w = _AdditionalLightsAttenuation[(int)(r4.w)].x * r5.w;
    r5.w = -r5.w * r5.w + 1;
    r5.w = max(0, r5.w);
    r5.w = r5.w * r5.w;
    r5.w = r6.x * r5.w;
    r6.x = dot(_AdditionalLightsSpotDir[(int)(r4.w)].xyz, r5.xyz);
    r6.x = saturate(r6.x * _AdditionalLightsAttenuation[(int)(r4.w)].z + _AdditionalLightsAttenuation[(int)(r4.w)].w);
    r6.x = r6.x * r6.x;
    r5.w = r6.x * r5.w;
    r6.x = vd_asint(unity_RenderingLayer.x) & vd_asint(_AdditionalLightsLayerMasks[(int)(r4.w)]);
    r6.yzw = _AdditionalLightsColor[(int)(r4.w)].xyz * r5.www;
    r4.w = saturate(dot(r0.xyz, r5.xyz));
    r4.w = -_ShadeSmoothStart + r4.w;
    r4.w = saturate(r4.w * r3.w);
    r5.x = r4.w * -2 + 3;
    r4.w = r4.w * r4.w;
    r5.y = r5.x * r4.w;
    r4.w = -r5.x * r4.w + 1;
    r4.w = _ShadeAlpha * r4.w + r5.y;
    r5.xyz = r6.yzw * r4.www;
    r5.xyz = r5.xyz * r1.yzw + r4.xyz;
    r4.xyz = r6.xxx ? r5.xyz : r4.xyz;
    r2.w = (int)r2.w + 1;
  }
  r0.xyz = r2.xyz * r1.yzw + r3.xyz;
  o0.xyz = r0.xyz + r4.xyz;
  r0.x = cmp(_Surface == 1.000000);
  o0.w = r0.x ? r1.x : 1;
  return;
}
