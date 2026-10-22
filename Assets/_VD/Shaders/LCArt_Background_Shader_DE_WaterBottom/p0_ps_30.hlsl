// LCArt/Background/Shader_DE_WaterBottom | pass 0 "ForwardLit" | progFragment | keywords: _LIGHT_LAYERS | blob 30
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
  float4 r0,r1,r2,r3;
  uint4 bitmask, uiDest;
  float4 fDest;

  r0.xy = _GlobalWaterWaveParams.yy * v1.xz;
  r0.z = _GlobalWaterWaveParams.x * _Time.y;
  r1.xyz = float3(0.707099974,-0.400000006,0.692799985) * r0.zzz;
  r0.zw = v1.xz * _GlobalWaterWaveParams.yy + r1.xx;
  r0.xy = r0.xy * float2(0.699999988,0.699999988) + r1.yz;
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
  r2.w = _GlobalWaterBottomColor.w;
  o0.xyzw = r2.xyzw;
  return;
}
