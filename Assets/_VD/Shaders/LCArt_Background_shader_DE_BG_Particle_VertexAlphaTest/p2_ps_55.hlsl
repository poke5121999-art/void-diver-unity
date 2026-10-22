// LCArt/Background/shader_DE_BG_Particle_VertexAlphaTest | pass 2 "Glass" | progFragment | keywords: _ADDITIONAL_LIGHTS _DE_EDITOR_CAMERA _LIGHT_LAYERS | blob 55
void frag(
  float2 v0 : TEXCOORD0,
  float w0 : TEXCOORD8,
  float4 v1 : TEXCOORD11,
  float4 v2 : TEXCOORD1,
  float4 v3 : TEXCOORD2,
  float4 v4 : TEXCOORD3,
  float4 v5 : TEXCOORD4,
  float3 v6 : TEXCOORD7,
  float4 v7 : SV_POSITION0,
  float4 v8 : COLOR0,
  float4 v9 : TEXCOORD9,
  float4 v10 : TEXCOORD5,
  float3 v11 : TEXCOORD10,
  out float4 o0 : SV_Target0)
{
  const float4 icb[] = { { 1.000000, 0, 0, 0},
                              { 0, 1.000000, 0, 0},
                              { 0, 0, 1.000000, 0},
                              { 0, 0, 0, 1.000000} };
  float4 r0,r1,r2,r3,r4,r5,r6,r7,r8,r9,r10;
  uint4 bitmask, uiDest;
  float4 fDest;

  float4 x0[16];
  x0[0].x = 0.058823999;
  x0[1].x = 0.529411972;
  x0[2].x = 0.176470995;
  x0[3].x = 0.647059023;
  x0[4].x = 0.764706016;
  x0[5].x = 0.294117987;
  x0[6].x = 0.882353008;
  x0[7].x = 0.411765009;
  x0[8].x = 0.235293999;
  x0[9].x = 0.705882013;
  x0[10].x = 0.117647;
  x0[11].x = 0.588235021;
  x0[12].x = 0.941175997;
  x0[13].x = 0.470587999;
  x0[14].x = 0.823529005;
  x0[15].x = 0.352941006;
  r0.xyzw = _EmissionMap.SampleBias(sampler_vd_point_repeat, v0.xy, _GlobalMipBias.x).xyzw;
  r0.y = cmp(r0.z < 0.5);
  if (r0.y != 0) discard;
  r1.xyzw = _BaseMap.SampleBias(sampler_BaseMap, v0.xy, _GlobalMipBias.x).xyzw;
  r1.xyz = _BaseColor.xyz * r1.xyz;
  r2.xyzw = _BumpMap.SampleBias(sampler_vd_point_repeat, v0.xy, _GlobalMipBias.x).xyzw;
  r2.x = r2.w * r2.x;
  r3.xy = r2.xy + r2.xy;
  r0.yw = r2.xy * float2(2,2) + float2(-1,-1);
  r0.y = dot(r0.yw, r0.yw);
  r0.y = min(1, r0.y);
  r0.y = 1 + -r0.y;
  r0.y = sqrt(r0.y);
  r3.z = max(1.00000002e-016, r0.y);
  r2.xyz = float3(-1,-1,-1) + r3.xyz;
  r2.xyz = (_BumpScale).xxx * r2.xyz + float3(0,0,1);
  r0.yw = float2(1,1) / _ScaledScreenParams.xy;
  r3.xy = v7.xy * r0.yw;
  r0.y = r3.y * _ScaleBiasRt.x + _ScaleBiasRt.y;
  r3.z = 1 + -r0.y;
  r0.y = 1 + -_GlobalSeeThroughAlpha;
  r0.y = w0.x * r0.y + _GlobalSeeThroughAlpha;
  r0.w = saturate(dot(v8.xyz, float3(0.298999995,0.587000012,0.114)));
  r0.y = r0.y * r0.w;
  r3.yw = _ScreenSize.xy / _ScreenParams.xy;
  r3.xz = _ScreenParams.xy * r3.xz;
  r3.xy = r3.xz * r3.yw;
  r3.xy = floor(r3.xy);
  r3.xy = (uint2)r3.xy;
  r3.xy = (int2)r3.xy & int2(3,3);
  r0.w = (uint)r3.y << 2;
  r0.w = (int)r0.w + (int)r3.x;
  r0.w = x0[r0.w+0].x;
  r0.y = r0.y * _BaseColor.w + -r0.w;
  r0.y = cmp(r0.y < 0);
  if (r0.y != 0) discard;
  r0.y = cmp(0.5 < _UseWorldNormalMap);
  if (r0.y != 0) {
    r3.xyzw = _WorldNormalMap.SampleBias(sampler_vd_point_repeat, v0.xy, _GlobalMipBias.x).xyzw;
    r3.xyz = r3.xyz * float3(2,2,2) + float3(-1,-1,-1);
    r0.y = dot(r3.xyz, r3.xyz);
    r0.y = rsqrt(r0.y);
    r3.xyz = r3.xyz * r0.yyy;
    r0.y = dot(v4.xyz, r3.xyz);
    r4.xyz = -r3.xyz * r0.yyy + v4.xyz;
    r0.y = dot(r4.xyz, r4.xyz);
    r0.y = rsqrt(r0.y);
    r4.xyz = r4.xyz * r0.yyy;
    r0.y = dot(v5.xyz, r3.xyz);
    r5.xyz = -r3.xyz * r0.yyy + v5.xyz;
    r0.y = dot(r5.xyz, r5.xyz);
    r0.y = rsqrt(r0.y);
    r5.xyz = r5.xyz * r0.yyy;
    r5.xyz = r5.xyz * r2.yyy;
    r4.xyz = r2.xxx * r4.xyz + r5.xyz;
    r3.xyz = r2.zzz * r3.xyz + r4.xyz;
  } else {
    r4.xyz = v5.xyz * r2.yyy;
    r2.xyw = r2.xxx * v4.xyz + r4.xyz;
    r3.xyz = r2.zzz * v3.xyz + r2.xyw;
  }
  r0.y = dot(r3.xyz, r3.xyz);
  r0.y = rsqrt(r0.y);
  r2.xyz = r3.xyz * r0.yyy;
  r0.y = cmp(0.5 < v1.z);
  r0.w = cmp(v1.z < 1.5);
  r0.y = r0.w ? r0.y : 0;
  r0.w = cmp(0.125 >= v11.y);
  r0.y = r0.w ? r0.y : 0;
  r3.xyz = v11.xyz;
  r3.w = 0;
  r4.xyz = v2.xyz;
  r4.w = r2.x;
  r3.xyzw = r0.yyyy ? r3.xyzw : r4.xyzw;
  r2.yz = r0.yy ? float2(1,0) : r2.yz;
  r0.y = cmp(unity_OrthoParams.w == 0.000000);
  r4.xyz = _WorldSpaceCameraPos.xyz + -r3.xyz;
  r0.w = dot(r4.xyz, r4.xyz);
  r0.w = rsqrt(r0.w);
  r4.xyz = r4.xyz * r0.www;
  r5.x = r0.y ? r4.x : (VD_COL(unity_MatrixV, 0).z);
  r5.y = r0.y ? r4.y : (VD_COL(unity_MatrixV, 1).z);
  r5.z = r0.y ? r4.z : (VD_COL(unity_MatrixV, 2).z);
  r4.xyz = (VD_COL(unity_MatrixVP, 1).xyw) * r3.yyy;
  r4.xyz = (VD_COL(unity_MatrixVP, 0).xyw) * r3.xxx + r4.xyz;
  r4.xyz = (VD_COL(unity_MatrixVP, 2).xyw) * r3.zzz + r4.xyz;
  r4.xyz = (VD_COL(unity_MatrixVP, 3).xyw) + r4.xyz;
  r0.yw = r4.xy / -r4.zz;
  r4.xy = r0.yw * float2(0.5,0.5) + float2(0.5,0.5);
  r0.x = saturate(dot(r0.xx, r0.zz));
  r0.y = vd_asint(_MainLightLayerMask) & vd_asint(unity_RenderingLayer.x);
  r6.xyz = r1.xyz * r0.xxx;
  r6.xyz = _MainLightColor.xyz * r6.xyz;
  r7.xyz = _MainLightPosition.xyz + r5.xyz;
  r0.z = dot(r7.xyz, r7.xyz);
  r0.z = max(6.10351563e-005, r0.z);
  r0.z = rsqrt(r0.z);
  r7.xyz = r7.xyz * r0.zzz;
  r2.x = r3.w;
  r0.z = saturate(dot(r2.xyz, r7.xyz));
  r0.z = log2(r0.z);
  r0.z = 128 * r0.z;
  r0.z = exp2(r0.z);
  r7.xyz = _MainLightColor.xyz * r0.zzz;
  r7.xyz = r7.xyz * r0.xxx;
  r7.xyz = unity_LightData.zzz * r7.xyz;
  r6.xyz = r6.xyz * float3(0.600000024,0.600000024,0.600000024) + r7.xyz;
  r0.yzw = r0.yyy ? r6.xyz : float3(0,0,0);
  r1.w = min(unity_LightData.y, _AdditionalLightsCount.x);
  r1.w = (int)r1.w;
  r6.xy = -_PlaneBoundMaxPos.xz + r3.xz;
  r6.zw = -_PlaneBoundMaxPos.xz + _PlaneBoundMinPos.xz;
  r6.xy = saturate(r6.xy / r6.zw);
  r6.xyzw = _PlaneSightRenderTarget.SampleLevel(sampler_PlaneSightRenderTarget, r6.xy, 0).xyzw;
  r2.w = -1 + r6.w;
  r2.w = _GlobalPlaneSystemEnable * r2.w;
  r6.xyz = float3(32,32,32) * r3.xyz;
  r6.xyz = floor(r6.xyz);
  r3.w = r0.x * 0.5 + 0.5;
  r7.xyz = float3(0,0,0);
  r4.w = 0;
  while (true) {
    r5.w = cmp((uint)r4.w >= (uint)r1.w);
    if (r5.w != 0) break;
    r5.w = (uint)r4.w >> 2;
    r6.w = (int)r4.w & 3;
    r5.w = dot(unity_LightIndices[(int)(r5.w)].xyzw, icb[r6.w+0].xyzw);
    r5.w = (int)r5.w;
    r8.xyz = -r3.xyz * _AdditionalLightsPosition[(int)(r5.w)].www + _AdditionalLightsPosition[(int)(r5.w)].xyz;
    r6.w = dot(r8.xyz, r8.xyz);
    r6.w = max(6.10351563e-005, r6.w);
    r7.w = rsqrt(r6.w);
    r8.xyz = r8.xyz * r7.www;
    r7.w = 1 / r6.w;
    r6.w = _AdditionalLightsAttenuation[(int)(r5.w)].x * r6.w;
    r6.w = -r6.w * r6.w + 1;
    r6.w = max(0, r6.w);
    r6.w = r6.w * r6.w;
    r6.w = r7.w * r6.w;
    r7.w = dot(_AdditionalLightsSpotDir[(int)(r5.w)].xyz, r8.xyz);
    r7.w = saturate(r7.w * _AdditionalLightsAttenuation[(int)(r5.w)].z + _AdditionalLightsAttenuation[(int)(r5.w)].w);
    r7.w = r7.w * r7.w;
    r6.w = r7.w * r6.w;
    r7.w = cmp(2 == vd_asint(_AdditionalLightsLayerMasks[(int)(r5.w)]));
    r7.w = r7.w ? 1.000000 : 0;
    r7.w = r7.w * r2.w + 1;
    r8.xyz = r6.xyz * float3(0.03125,0.03125,0.03125) + -_AdditionalLightsPosition[(int)(r4.w)].xyz;
    r8.w = dot(r8.xyz, r8.xyz);
    r8.w = rsqrt(r8.w);
    r9.xyz = r8.xyz * r8.www;
    r9.w = dot(r9.xyz, r2.xyz);
    r10.xyz = -r9.www * r2.xyz + r9.xyz;
    r9.w = dot(r10.xyz, r10.xyz);
    r9.w = rsqrt(r9.w);
    r10.xyz = r10.xyz * r9.www;
    r9.w = dot(r9.xyz, r10.xyz);
    r9.w = r9.w + r9.w;
    r9.xyz = r10.xyz * -r9.www + r9.xyz;
    r8.xyz = r8.xyz * r8.www + r5.xyz;
    r8.w = dot(r8.xyz, r8.xyz);
    r8.w = max(1.17549435e-038, r8.w);
    r8.w = rsqrt(r8.w);
    r8.xyz = r8.xyz * r8.www;
    r8.x = saturate(dot(r2.xyz, r8.xyz));
    r8.yzw = -r9.xyz + r5.xyz;
    r9.x = dot(r8.yzw, r8.yzw);
    r9.x = max(1.17549435e-038, r9.x);
    r9.x = rsqrt(r9.x);
    r8.yzw = r9.xxx * r8.yzw;
    r8.y = saturate(dot(r2.xyz, r8.yzw));
    r8.x = max(r8.x, r8.y);
    r8.x = log2(r8.x);
    r8.x = 256 * r8.x;
    r8.x = exp2(r8.x);
    r8.yzw = _AdditionalLightsColor[(int)(r5.w)].xyz * _AdditionalLightsColor[(int)(r5.w)].xyz;
    r8.xyz = r8.yzw * r8.xxx;
    r8.xyz = r8.xyz * r6.www;
    r8.xyz = r8.xyz * r3.www;
    r8.xyz = r8.xyz * r1.xyz;
    r8.xyz = float3(0.25,0.25,0.25) * r8.xyz;
    r7.xyz = r8.xyz * r7.www + r7.xyz;
    r4.w = (int)r4.w + 1;
  }
  r0.yzw = r7.xyz + r0.yzw;
  r0.x = 1 + -r0.x;
  r1.w = dot(r1.xyz, float3(0.298999995,0.587000012,0.114));
  r1.w = r1.w * r1.w;
  r2.y = r1.w * r1.w;
  r2.x = 0.0125000002;
  r2.xy = float2(0,-0.0125000002) * r2.xy;
  r4.z = 1 + -r4.x;
  r2.xy = r2.xy * r0.xx + r4.zy;
  r2.xyzw = _AfterRenderingGrabpassTexture.SampleBias(sampler_vd_linear_clamp, r2.xy, _GlobalMipBias.x).xyzw;
  o0.xyz = r2.xyz * r1.xyz + r0.yzw;
  o0.w = 1;
  return;
}
