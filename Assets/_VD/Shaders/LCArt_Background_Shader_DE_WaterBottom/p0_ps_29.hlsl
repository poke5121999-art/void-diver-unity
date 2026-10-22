// LCArt/Background/Shader_DE_WaterBottom | pass 0 "ForwardLit" | progFragment | keywords: _CLUSTER_LIGHT_LOOP _LIGHT_LAYERS | blob 29
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
  float4 r0,r1,r2,r3,r4,r5,r6,r7,r8,r9,r10;
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
  r3.x = (VD_COL(unity_MatrixV, 0).z);
  r3.y = (VD_COL(unity_MatrixV, 1).z);
  r3.z = (VD_COL(unity_MatrixV, 2).z);
  r2.xyz = r1.www ? r2.xyz : r3.xyz;
  r4.xy = float2(1,1) / _ScaledScreenParams.xy;
  r4.xy = v6.xy * r4.xy;
  r2.w = r4.y * _ScaleBiasRt.x + _ScaleBiasRt.y;
  r4.z = 1 + -r2.w;
  r4.xy = _FPParams1.xy * r4.xz;
  r4.xy = (uint2)r4.xy;
  r4.zw = (uint2)_FPParams1.zw;
  r2.w = mad((int)r4.y, (int)r4.z, (int)r4.x);
  r2.w = (int)r4.w * (int)r2.w;
  r4.xyz = -_WorldSpaceCameraPos.xyz + v1.xyz;
  r3.x = dot(-r3.xyz, r4.xyz);
  r3.y = log2(r3.x);
  r1.w = r1.w ? r3.y : r3.x;
  r1.w = r1.w * _FPParams0.x + _FPParams0.y;
  r1.w = (uint)r1.w;
  r3.x = (uint)_FPParams2.x;
  r3.x = (int)r3.x + -1;
  r3.y = (int)r4.w + 2;
  r1.w = min((uint)r3.x, (uint)r1.w);
  r3.x = (int)r3.y * (int)r1.w;
  r1.w = mad((int)r1.w, (int)r3.y, 2);
  r3.y = (uint)r3.x >> 2;
  r4.x = (uint)r3.x << 31;
  r4.y = (uint)r3.x << 30;
  r3.xz = (uint2)r4.xy >> 31;
  r4.xy = (int2)r3.xx & vd_asint(urp_ZBins[(int)(r3.y)].wy);
  r3.xw = ~(int2)r3.xz;
  r3.xy = (int2)r3.xx & vd_asint(urp_ZBins[(int)(r3.y)].zx);
  r3.xy = (int2)r3.xy | (int2)r4.xy;
  r3.xy = (int2)r3.zw & (int2)r3.xy;
  r3.x = (int)r3.y | (int)r3.x;
  r3.y = (uint)_FPParams0.w;
  r4.x = r2.w;
  r4.y = r1.w;
  r4.z = 0;
  r4.w = r3.x;
  r5.xyz = float3(1,1,1);
  r6.xyz = float3(0,0,0);
  while (true) {
    r3.z = (uint)r4.w >> 16;
    r3.w = r4.z;
    r5.w = r4.w;
    while (true) {
      r6.w = cmp((int)r3.w == 0);
      r7.x = (int)r5.w & 0x0000ffff;
      r7.x = cmp((uint)r3.z >= (uint)r7.x);
      r6.w = r6.w ? r7.x : 0;
      if (r6.w == 0) break;
      r6.w = (uint)r5.w >> 5;
      r6.w = (int)r6.w & 2047;
      r7.xyzw = (int4)r4.xxyy + (int4)r6.wwww;
      r8.xy = (uint2)r7.yw >> 2;
      r9.xz = (uint2)r7.xz << 31;
      r9.yw = (uint2)r7.yw << 30;
      r7.xyzw = (uint4)r9.xyzw >> 31;
      r8.zw = (int2)r7.xx & vd_asint(urp_Tiles[(int)(r8.x)].wy);
      r9.xyzw = ~(int4)r7.xyzw;
      r10.xy = (int2)r9.xx & vd_asint(urp_Tiles[(int)(r8.x)].zx);
      r8.xz = (int2)r8.zw | (int2)r10.xy;
      r7.x = (int)r7.y & (int)r8.x;
      r7.y = (int)r9.y & (int)r8.z;
      r7.x = (int)r7.y | (int)r7.x;
      r7.yz = (int2)r7.zz & vd_asint(urp_ZBins[(int)(r8.y)].wy);
      r8.xy = (int2)r9.zz & vd_asint(urp_ZBins[(int)(r8.y)].zx);
      r7.yz = (int2)r7.yz | (int2)r8.xy;
      r7.y = (int)r7.w & (int)r7.y;
      r7.z = (int)r9.w & (int)r7.z;
      r7.y = (int)r7.z | (int)r7.y;
      r7.x = (int)r7.y & (int)r7.x;
      r7.y = -1 << (int)r5.w;
      r7.x = (int)r7.y & (int)r7.x;
      r6.w = (uint)r6.w << 5;
      r6.w = (int)r3.z + (int)-r6.w;
      r6.w = min(31, (uint)r6.w);
      r6.w = (int)-r6.w + 31;
      r6.w = -1 >> (uint)r6.w;
      r3.w = (int)r6.w & (int)r7.x;
      r6.w = (int)r5.w + 32;
      r5.w = (int)r6.w & -32;
    }
    r4.w = r5.w;
    if (r3.w == 0) break;
    r3.z = -(int)r3.w;
    r3.z = (int)r3.z & (int)r3.w;
    r3.z = (uint)r3.z;
    r3.z = (uint)r3.z >> 23;
    r3.z = (int)r3.z + -127;
    r6.w = 1 << (int)r3.z;
    r4.z = (int)r3.w ^ (int)r6.w;
    r6.w = (int)r4.w + -32;
    r6.w = (int)r6.w & 0x0000ffe0;
    r3.z = (int)r3.z + (int)r6.w;
    r3.z = (int)r3.y + (int)r3.z;
    r7.xyz = -v1.xyz * _AdditionalLightsPosition[(int)(r3.z)].www + _AdditionalLightsPosition[(int)(r3.z)].xyz;
    r6.w = dot(r7.xyz, r7.xyz);
    r6.w = max(6.10351563e-005, r6.w);
    r7.w = rsqrt(r6.w);
    r8.xyz = r7.xyz * r7.www;
    r8.w = 1 / r6.w;
    r6.w = _AdditionalLightsAttenuation[(int)(r3.z)].x * r6.w;
    r6.w = -r6.w * r6.w + 1;
    r6.w = max(0, r6.w);
    r6.w = r6.w * r6.w;
    r6.w = r8.w * r6.w;
    r8.x = dot(_AdditionalLightsSpotDir[(int)(r3.z)].xyz, r8.xyz);
    r8.x = saturate(r8.x * _AdditionalLightsAttenuation[(int)(r3.z)].z + _AdditionalLightsAttenuation[(int)(r3.z)].w);
    r8.x = r8.x * r8.x;
    r6.w = r8.x * r6.w;
    r8.x = vd_asint(unity_RenderingLayer.x) & vd_asint(_AdditionalLightsLayerMasks[(int)(r3.z)]);
    r6.w = sqrt(r6.w);
    r7.xyz = r7.xyz * r7.www + r2.xyz;
    r7.w = dot(r7.xyz, r7.xyz);
    r7.w = max(1.17549435e-038, r7.w);
    r7.w = rsqrt(r7.w);
    r7.xyz = r7.xyz * r7.www;
    r7.x = saturate(dot(r1.xyz, r7.xyz));
    r7.x = r6.w * r7.x + -0.720000029;
    r7.x = saturate(100.000099 * r7.x);
    r7.y = r7.x * -2 + 3;
    r7.x = r7.x * r7.x;
    r7.x = r7.y * r7.x;
    r7.yzw = _AdditionalLightsColor[(int)(r3.z)].xyz + float3(-1,-1,-1);
    r7.yzw = r6.www * r7.yzw + float3(1,1,1);
    r6.w = r7.x * r7.x;
    r7.xyz = r7.yzw * r5.xyz;
    r8.yzw = r6.www * _AdditionalLightsColor[(int)(r3.z)].xyz + r6.xyz;
    r5.xyz = r8.xxx ? r7.xyz : r5.xyz;
    r6.xyz = r8.xxx ? r8.yzw : r6.xyz;
  }
  r1.xy = v7.xy / v7.ww;
  r1.zw = -_CameraDepthTexture_TexelSize.xy * float2(0.5,0.5) + float2(1,1);
  r1.zw = min(r1.xy, r1.zw);
  r1.zw = _RTHandleScale.xy * r1.zw;
  r2.xyzw = _CameraDepthTexture.SampleBias(sampler_vd_point_clamp, r1.zw, _GlobalMipBias.x).xyzw;
  r1.z = _ZBufferParams.z * r2.x + _ZBufferParams.w;
  r1.z = 1 / r1.z;
  r2.xyzw = _GlobalWaterNoiseTex.SampleBias(sampler_vd_linear_repeat, r0.zw, _GlobalMipBias.x).xyzw;
  r0.xyzw = _GlobalWaterNoiseTex.SampleBias(sampler_vd_linear_repeat, r0.xy, _GlobalMipBias.x).xyzw;
  r0.y = min(r2.x, r0.x);
  r0.y = log2(abs(r0.y));
  r0.y = _GlobalWaterBottomEdgeParams.x * r0.y;
  r0.y = exp2(r0.y);
  r0.y = min(1, r0.y);
  r0.y = _GlobalWaterBottomEdgeParams.y * r0.y;
  r0.x = r2.x + r0.x;
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
  r2.xyz = float3(0.5,0.5,0.5) * _GlobalWaterBottomColor.xyz;
  r2.xyz = r0.zzz * r2.xyz + r2.xyz;
  r0.y = _GlobalWaterBottomShallowColor.w * r0.y;
  r3.xyz = _GlobalWaterBottomShallowColor.xyz + -r2.xyz;
  r2.xyz = r0.yyy * r3.xyz + r2.xyz;
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
    r2.xyz = r1.xyz * r0.xxx + r2.xyz;
  }
  o0.xyz = r2.xyz * r5.xyz + r6.xyz;
  o0.w = _GlobalWaterBottomColor.w;
  return;
}
