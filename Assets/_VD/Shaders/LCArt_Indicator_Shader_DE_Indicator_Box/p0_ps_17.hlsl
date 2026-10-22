// LCArt/Indicator/Shader_DE_Indicator_Box | pass 0 "" | progFragment | keywords: _ADDITIONAL_LIGHTS _DE_EDITOR_CAMERA | blob 17
void frag(
  float2 v0 : TEXCOORD0,
  float w0 : TEXCOORD2,
  float w0 : TEXCOORD3,
  float3 v1 : TEXCOORD1,
  float w1 : TEXCOORD4,
  float4 v2 : TEXCOORD5,
  float4 v3 : SV_POSITION0,
  out float4 o0 : SV_Target0)
{
  float4 r0,r1,r2;
  uint4 bitmask, uiDest;
  float4 fDest;

  r0.x = _LineScaling * _MeshHalfLengthX;
  r0.x = max(9.99999975e-005, r0.x);
  r0.y = 0.0199999996 * r0.x;
  r0.z = saturate(_Progress);
  r0.w = r0.z * r0.x;
  r0.z = -0.125 + r0.z;
  r0.z = saturate(8 * r0.z);
  r0.y = r0.w * _FillRatio + r0.y;
  r0.x = r0.x * 0.0199999996 + r0.y;
  r0.x = -r0.w * _FillRatio + r0.x;
  r0.x = 1 / r0.x;
  r1.x = dot((VD_COL(unity_ObjectToWorld, 0).xyz), (VD_COL(unity_ObjectToWorld, 0).xyz));
  r1.x = rsqrt(r1.x);
  r1.xyz = (VD_COL(unity_ObjectToWorld, 0).xyz) * r1.xxx;
  r2.xyz = -(VD_COL(unity_ObjectToWorld, 3).xyz) + v1.xyz;
  r1.x = dot(r2.xyz, r1.xyz);
  r0.w = -r0.w * _FillRatio + abs(r1.x);
  r0.y = abs(r1.x) + -r0.y;
  r0.y = -_LineThickness + abs(r0.y);
  r0.x = saturate(r0.w * r0.x);
  r0.w = r0.x * -2 + 3;
  r0.x = r0.x * r0.x;
  r0.x = -r0.w * r0.x + 1;
  r0.x = cmp(r0.x >= 0.5);
  r0.x = r0.x ? 1.000000 : 0;
  r0.w = _FillColor.w * r0.x;
  r1.x = 1 / -_LineThickness;
  r0.y = saturate(r1.x * r0.y);
  r1.x = r0.y * -2 + 3;
  r0.y = r0.y * r0.y;
  r0.y = r1.x * r0.y;
  r0.y = _LineColor.w * r0.y;
  r0.w = max(r0.w, r0.y);
  r1.x = r0.z * -2 + 3;
  r0.z = r0.z * r0.z;
  r0.z = r1.x * r0.z;
  r0.w = r0.w * r0.z;
  r0.y = saturate(-r0.y * r0.z + 1);
  r0.x = r0.x * r0.y;
  r1.xyzw = _WeightTex.SampleBias(sampler_WeightTex, v0.xy, _GlobalMipBias.x).zwxy;
  r1.xy = saturate(r1.xy);
  r0.y = r1.y * r0.w;
  r0.z = cmp(r0.y >= 0.00999999978);
  r0.z = r0.z ? 1.000000 : 0;
  r0.z = r0.z * r1.y;
  r1.yzw = -_LineColor.xyz + _FillColor.xyz;
  r1.yzw = r0.xxx * r1.yzw + _LineColor.xyz;
  r2.xyzw = _MainTex.SampleBias(sampler_MainTex, v0.xy, _GlobalMipBias.x).xyzw;
  r1.yzw = -r2.xyz + r1.yzw;
  o0.xyz = r0.zzz * r1.yzw + r2.xyz;
  r0.x = r2.w * r2.w;
  r0.x = max(r0.x, r0.y);
  r0.y = max(9.99999975e-005, _LengthScaling);
  r0.y = _AlphaFadeWorld / r0.y;
  r0.y = max(9.99999975e-005, r0.y);
  r0.y = 1 / r0.y;
  r0.z = 1 + -v0.y;
  r0.z = saturate(r0.z * r0.y);
  r0.y = saturate(v0.y * r0.y);
  r0.w = r0.z * -2 + 3;
  r0.z = r0.z * r0.z;
  r0.z = r0.w * r0.z;
  r0.w = r0.y * -2 + 3;
  r0.y = r0.y * r0.y;
  r0.y = r0.w * r0.y;
  r0.y = min(r0.y, r0.z);
  r0.z = 1 + -r0.y;
  r0.y = r1.x * r0.z + r0.y;
  r0.z = -1.25 + _Progress;
  r0.z = saturate(4 * r0.z);
  r0.w = r0.z * -2 + 3;
  r0.z = r0.z * r0.z;
  r0.z = -r0.w * r0.z + 1;
  r0.x = r0.x * r0.z;
  r0.x = r0.x * r0.y;
  r0.yzw = v2.xyz / v2.www;
  r1.xyzw = _CustomDepthBuffer.SampleBias(sampler_CustomDepthBuffer, r0.yz, _GlobalMipBias.x).xyzw;
  r1.x = _ZBufferParams.z * r1.x + _ZBufferParams.w;
  r1.x = 1 / r1.x;
  r2.xyzw = _CameraDepthTexture.SampleBias(sampler_CameraDepthTexture, r0.yz, _GlobalMipBias.x).xyzw;
  r0.y = _ZBufferParams.z * r0.w + _ZBufferParams.w;
  r0.y = 1 / r0.y;
  r0.z = _ZBufferParams.z * r2.x + _ZBufferParams.w;
  r0.z = 1 / r0.z;
  r0.z = min(r1.x, r0.z);
  r0.y = r0.y + -r0.z;
  r0.y = -_OcclusionFadeStart + r0.y;
  r0.z = _OcclusionFadeEnd + -_OcclusionFadeStart;
  r0.z = max(9.99999975e-005, r0.z);
  r0.y = saturate(r0.y / r0.z);
  r0.y = 1 + -r0.y;
  r0.z = 1 + -_OccludedAlpha;
  r0.y = r0.y * r0.z + _OccludedAlpha;
  o0.w = r0.x * r0.y;
  return;
}
