// LCArt/Background/Shader_DE_WaterBottom | pass 0 "ForwardLit" | progFragment | keywords: FOG_LINEAR _ADDITIONAL_LIGHTS _DE_EDITOR_CAMERA _LIGHT_LAYERS | blob 22
void frag(
  float2 v0 : TEXCOORD0,
  float w0 : TEXCOORD9,
  float w0 : TEXCOORD10,
  float4 v1 : TEXCOORD1,
  float4 v2 : TEXCOORD2,
  float4 v3 : TEXCOORD3,
  float4 v4 : TEXCOORD4,
  float3 v5 : TEXCOORD7,
  float4 v6 : SV_POSITION0,
  float4 v7 : TEXCOORD8,
  float4 v8 : COLOR0,
  out float4 o0 : SV_Target0)
{
  const float4 icb[] = { { 1.000000, 0, 0, 0},
                              { 0, 1.000000, 0, 0},
                              { 0, 0, 1.000000, 0},
                              { 0, 0, 0, 1.000000} };
  float4 r0,r1,r2,r3,r4,r5,r6;
  uint4 bitmask, uiDest;
  float4 fDest;

  r0.xy = _GlobalWaterWaveParams.yy * v1.xz;
  r0.z = _GlobalWaterWaveParams.x * _Time.y;
  r1.xyzw = float4(0.707099974,-0.400000006,0.692799985,-0.707099974) * r0.zzzz;
  r0.zw = v1.xz * _GlobalWaterWaveParams.yy + r1.xx;
  r0.xy = r0.xy * float2(0.699999988,0.699999988) + r1.yz;
  r1.xyzw = r0.zwzw * _GlobalWaterWaveParams.yyyy + r1.xxww;
  r2.xyzw = _BumpMap.SampleBias(sampler_vd_linear_repeat, r1.xy, _GlobalMipBias.x).xyzw;
  r1.xyzw = _BumpMap.SampleBias(sampler_vd_linear_repeat, r1.zw, _GlobalMipBias.x).xyzw;
  r1.xyz = float3(0.5,0.5,0.5) * r1.xyw;
  r1.yzw = r2.xyw * float3(0.5,0.5,0.5) + r1.xyz;
  r1.x = r1.w * r1.y;
  r1.xy = r1.xz * float2(2,2) + float2(-1,-1);
  r1.z = dot(r1.xy, r1.xy);
  r1.z = min(1, r1.z);
  r1.z = 1 + -r1.z;
  r1.z = sqrt(r1.z);
  r1.z = max(1.00000002e-016, r1.z);
  r2.xyz = v4.xyz * r1.yyy;
  r1.xyw = r1.xxx * v3.xyz + r2.xyz;
  r1.xyz = r1.zzz * v2.xyz + r1.xyw;
  r1.w = dot(r1.xyz, r1.xyz);
  r1.w = rsqrt(r1.w);
  r1.xyz = r1.xyz * r1.www;
  r1.w = cmp(unity_OrthoParams.w == 0.000000);
  r2.xyz = _WorldSpaceCameraPos.xyz + -v1.xyz;
  r2.w = dot(r2.xyz, r2.xyz);
  r2.w = rsqrt(r2.w);
  r2.xyz = r2.xyz * r2.www;
  r3.x = r1.w ? r2.x : (VD_COL(unity_MatrixV, 0).z);
  r3.y = r1.w ? r2.y : (VD_COL(unity_MatrixV, 1).z);
  r3.z = r1.w ? r2.z : (VD_COL(unity_MatrixV, 2).z);
  r1.w = min(unity_LightData.y, _AdditionalLightsCount.x);
  r1.w = (int)r1.w;
  r4.xyz = float3(0,0,0);
  r2.xyzw = float4(1,1,1,0);
  while (true) {
    r3.w = cmp((uint)r2.w >= (uint)r1.w);
    if (r3.w != 0) break;
    r3.w = (uint)r2.w >> 2;
    r4.w = (int)r2.w & 3;
    r3.w = dot(unity_LightIndices[(int)(r3.w)].xyzw, icb[r4.w+0].xyzw);
    r3.w = (int)r3.w;
    r5.xyz = -v1.xyz * _AdditionalLightsPosition[(int)(r3.w)].www + _AdditionalLightsPosition[(int)(r3.w)].xyz;
    r4.w = dot(r5.xyz, r5.xyz);
    r4.w = max(6.10351563e-005, r4.w);
    r5.w = rsqrt(r4.w);
    r6.xyz = r5.xyz * r5.www;
    r6.w = 1 / r4.w;
    r4.w = _AdditionalLightsAttenuation[(int)(r3.w)].x * r4.w;
    r4.w = -r4.w * r4.w + 1;
    r4.w = max(0, r4.w);
    r4.w = r4.w * r4.w;
    r4.w = r6.w * r4.w;
    r6.x = dot(_AdditionalLightsSpotDir[(int)(r3.w)].xyz, r6.xyz);
    r6.x = saturate(r6.x * _AdditionalLightsAttenuation[(int)(r3.w)].z + _AdditionalLightsAttenuation[(int)(r3.w)].w);
    r6.x = r6.x * r6.x;
    r4.w = r6.x * r4.w;
    r6.x = vd_asint(unity_RenderingLayer.x) & vd_asint(_AdditionalLightsLayerMasks[(int)(r3.w)]);
    r4.w = sqrt(r4.w);
    r5.xyz = r5.xyz * r5.www + r3.xyz;
    r5.w = dot(r5.xyz, r5.xyz);
    r5.w = max(1.17549435e-038, r5.w);
    r5.w = rsqrt(r5.w);
    r5.xyz = r5.xyz * r5.www;
    r5.x = saturate(dot(r1.xyz, r5.xyz));
    r5.x = r4.w * r5.x + -0.720000029;
    r5.x = saturate(100.000099 * r5.x);
    r5.y = r5.x * -2 + 3;
    r5.x = r5.x * r5.x;
    r5.x = r5.y * r5.x;
    r5.yzw = _AdditionalLightsColor[(int)(r3.w)].xyz + float3(-1,-1,-1);
    r5.yzw = r4.www * r5.yzw + float3(1,1,1);
    r4.w = r5.x * r5.x;
    r5.xyz = r5.yzw * r2.xyz;
    r6.yzw = r4.www * _AdditionalLightsColor[(int)(r3.w)].xyz + r4.xyz;
    r2.xyz = r6.xxx ? r5.xyz : r2.xyz;
    r4.xyz = r6.xxx ? r6.yzw : r4.xyz;
    r2.w = (int)r2.w + 1;
  }
  r1.xy = v7.xy / v7.ww;
  r1.zw = -_CameraDepthTexture_TexelSize.xy * float2(0.5,0.5) + float2(1,1);
  r1.zw = min(r1.xy, r1.zw);
  r1.zw = _RTHandleScale.xy * r1.zw;
  r3.xyzw = _CameraDepthTexture.SampleBias(sampler_vd_point_clamp, r1.zw, _GlobalMipBias.x).xyzw;
  r1.z = _ZBufferParams.z * r3.x + _ZBufferParams.w;
  r1.z = 1 / r1.z;
  r3.xyzw = _GlobalWaterNoiseTex.SampleBias(sampler_vd_linear_repeat, r0.zw, _GlobalMipBias.x).xyzw;
  r0.xyzw = _GlobalWaterNoiseTex.SampleBias(sampler_vd_linear_repeat, r0.xy, _GlobalMipBias.x).xyzw;
  r0.y = min(r3.x, r0.x);
  r0.y = log2(abs(r0.y));
  r0.y = _GlobalWaterBottomEdgeParams.x * r0.y;
  r0.y = exp2(r0.y);
  r0.y = min(1, r0.y);
  r0.y = _GlobalWaterBottomEdgeParams.y * r0.y;
  r0.x = r3.x + r0.x;
  r0.z = -v7.w + r1.z;
  r0.z = r0.x * 0.0500000007 + r0.z;
  r0.w = -_GlobalWaterBottomEdgeParams.w + r0.z;
  r1.z = 1 / -_GlobalWaterBottomEdgeParams.w;
  r0.w = saturate(r1.z * r0.w);
  r1.z = r0.w * -2 + 3;
  r1.w = 1 / _GlobalWaterBottomEdgeParams.z;
  r0.z = saturate(r1.w * r0.z);
  r1.w = r0.z * -2 + 3;
  r0.zw = r0.zw * r0.zw;
  r0.z = r1.w * r0.z;
  r0.y = r1.z * r0.w + r0.y;
  r3.xyz = float3(0.5,0.5,0.5) * _GlobalWaterBottomColor.xyz;
  r3.xyz = r0.zzz * r3.xyz + r3.xyz;
  r0.y = _GlobalWaterBottomShallowColor.w * r0.y;
  r5.xyz = _GlobalWaterBottomShallowColor.xyz + -r3.xyz;
  r3.xyz = r0.yyy * r5.xyz + r3.xyz;
  r0.y = cmp(vd_asint(_PlaneReflectionActive) != 0);
  r0.w = cmp(0.00999999978 < _GlobalWaterReflectionIntensity);
  r0.y = r0.w ? r0.y : 0;
  if (r0.y != 0) {
    r0.x = 0.5 * r0.x;
    r0.xy = -r0.xx * _GlobalWaterWaveParams2.xx + r1.xy;
    r0.y = _GlobalWaterBottomParams.y + r0.y;
    r1.xyzw = _PlaneReflectionTexture.SampleBias(sampler_vd_linear_clamp, r0.xy, _GlobalMipBias.x).xyzw;
    r0.x = r0.z * 0.699999988 + 0.300000012;
    r0.x = _GlobalWaterBottomParams.z * r0.x;
    r3.xyz = r1.xyz * r0.xxx + r3.xyz;
  }
  r0.xyz = r3.xyz * r2.xyz + r4.xyz;
  r0.xyz = -unity_FogColor.xyz + r0.xyz;
  o0.xyz = w0.xww * r0.xyz + unity_FogColor.xyz;
  o0.w = _GlobalWaterBottomColor.w;
  return;
}
