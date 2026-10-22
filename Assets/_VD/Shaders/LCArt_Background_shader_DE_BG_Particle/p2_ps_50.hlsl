// LCArt/Background/shader_DE_BG_Particle | pass 2 "Glass" | progFragment | keywords: _LIGHT_LAYERS | blob 50
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
  float4 r0,r1,r2,r3,r4,r5;
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
  r0.yw = cmp(float2(1.5,0.5) < v1.wz);
  if (r0.y != 0) {
    r3.xy = float2(1,1) / _ScaledScreenParams.xy;
    r3.xy = v7.xy * r3.xy;
    r0.y = r3.y * _ScaleBiasRt.x + _ScaleBiasRt.y;
    r3.z = 1 + -r0.y;
    r0.y = cmp(1.5 < v1.z);
    if (r0.y != 0) {
      r4.xyzw = _CustomDepthBuffer.SampleBias(sampler_CustomDepthBuffer, r3.xz, _GlobalMipBias.x).xyzw;
      r0.y = v7.z + -r4.z;
      r0.y = -2.49999994e-005 + r0.y;
      r0.y = saturate(-20000 * r0.y);
      r1.w = r0.y * -2 + 3;
      r0.y = r0.y * r0.y;
      r2.w = r1.w * r0.y;
      r3.yw = float2(1,1) + -float2(_GlobalSeeThroughCircleRange, _GlobalSeeThroughCircleAlpha);
      r4.x = 1 + -r3.y;
      r3.y = r4.w + -r3.y;
      r4.x = 1 / r4.x;
      r3.y = saturate(r4.x * r3.y);
      r4.x = r3.y * -2 + 3;
      r3.y = r3.y * r3.y;
      r3.y = r4.x * r3.y;
      r0.y = -r1.w * r0.y + 1;
      r0.y = r3.y * r0.y + r2.w;
      r1.w = _GlobalPlaneSystemEnable * _GlobalBackgroundSeeThroughEnable;
      r0.y = -1 + r0.y;
      r0.y = r1.w * r0.y + 1;
      r0.y = r0.y * r3.w + _GlobalSeeThroughCircleAlpha;
      r3.yw = _ScreenSize.xy / _ScreenParams.xy;
      r4.xy = _ScreenParams.xy * r3.xz;
      r3.yw = r4.xy * r3.yw;
      r3.yw = floor(r3.yw);
      r3.yw = (uint2)r3.yw;
      r3.yw = (int2)r3.yw & int2(3,3);
      r2.w = (uint)r3.w << 2;
      r2.w = (int)r2.w + (int)r3.y;
      r2.w = x0[r2.w+0].x;
      r0.y = -r2.w + r0.y;
      r0.y = cmp(r0.y < 0);
      if (r0.y != 0) discard;
      r0.y = cmp(0.125 < v2.y);
      if (r0.y != 0) {
        r4.xyzw = _CharacterAreaMaskTexture.SampleLevel(sampler_CharacterAreaMaskTexture, r3.xz, 0).xyzw;
        r3.yw = round(_WorldSpaceCameraPos.xz);
        r3.yw = v2.xz + -r3.yw;
        r5.x = -(VD_COL(unity_MatrixV, 0).z);
        r5.y = -(VD_COL(unity_MatrixV, 2).z);
        r0.y = dot(r5.xy, r5.xy);
        r0.y = rsqrt(r0.y);
        r4.zw = r5.xy * r0.yy;
        r0.y = dot(r3.yw, r4.zw);
        r0.y = saturate(0.00999999978 * r0.y);
        r0.y = r4.y + -r0.y;
        r0.y = saturate(40 * r0.y);
        r2.w = r0.y * -2 + 3;
        r0.y = r0.y * r0.y;
        r0.y = r2.w * r0.y;
        r2.w = -1 + r4.x;
        r0.y = r2.w * r0.y;
        r0.y = _GlobalBackgroundSeeThroughEnable * r0.y + 1;
        r4.xyz = -_WorldSpaceCameraPos.xyz + v2.xyz;
        r2.w = -0.150000006 + -_WorldSpaceCameraPos.y;
        r2.w = r2.w / r4.y;
        r3.yw = r4.xz * r2.ww + _WorldSpaceCameraPos.xz;
        r3.yw = -_PlaneBoundMaxPos.xz + r3.yw;
        r4.xy = -_PlaneBoundMaxPos.xz + _PlaneBoundMinPos.xz;
        r3.yw = saturate(r3.yw / r4.xy);
        r4.xyzw = _PlaneSightRenderTarget.SampleLevel(sampler_PlaneSightRenderTarget, r3.yw, 0).xyzw;
        r2.w = -1 + r4.w;
        r2.w = _GlobalPlaneSystemEnable * r2.w + 1;
        r3.y = _SeeThroughDistanceMax + -_SeeThroughDistanceMin;
        r3.y = 1 / r3.y;
        r3.w = 1;
        r4.x = 0;
        while (true) {
          r4.y = cmp((int)r4.x >= 3);
          if (r4.y != 0) break;
          r4.yzw = -_SightPositions[(int)(r4.x)].xyz + v2.xyz;
          r4.y = dot(r4.yzw, r4.yzw);
          r4.y = sqrt(r4.y);
          r4.y = -_SeeThroughDistanceMin + r4.y;
          r4.y = saturate(r4.y * r3.y);
          r4.z = r4.y * -2 + 3;
          r4.y = r4.y * r4.y;
          r4.y = r4.z * r4.y;
          r3.w = min(r4.y, r3.w);
          r4.x = (int)r4.x + 1;
        }
        r2.w = r2.w * r2.w;
        r3.y = 1 + -r3.w;
        r2.w = -r2.w * r3.y + 1;
        r2.w = max(0, r2.w);
        r2.w = -1 + r2.w;
        r1.w = r1.w * r2.w + 1;
        r0.y = min(r1.w, r0.y);
      } else {
        r0.y = w0.x;
      }
      r1.w = _GlobalSeeThroughHeightMax + -_GlobalSeeThroughHeightMin;
      r2.w = -_GlobalSeeThroughHeightMin + v2.y;
      r1.w = 1 / r1.w;
      r1.w = saturate(r2.w * r1.w);
      r2.w = r1.w * -2 + 3;
      r1.w = r1.w * r1.w;
      r1.w = -r2.w * r1.w + 1;
      r0.y = max(r1.w, r0.y);
      r0.y = max(0.00100000005, r0.y);
    } else {
      r0.y = w0.x;
    }
    r1.w = 1 + -_GlobalSeeThroughAlpha;
    r0.y = r0.y * r1.w + _GlobalSeeThroughAlpha;
    r0.y = v8.w * r0.y;
    r3.yw = _ScreenSize.xy / _ScreenParams.xy;
    r3.xz = _ScreenParams.xy * r3.xz;
    r3.xy = r3.xz * r3.yw;
    r3.xy = floor(r3.xy);
    r3.xy = (uint2)r3.xy;
    r3.xy = (int2)r3.xy & int2(3,3);
    r1.w = (uint)r3.y << 2;
    r1.w = (int)r1.w + (int)r3.x;
    r1.w = x0[r1.w+0].x;
    r0.y = r0.y * _BaseColor.w + -r1.w;
    r0.y = cmp(r0.y < 0);
    if (r0.y != 0) discard;
  }
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
  r0.y = cmp(v1.z < 1.5);
  r0.y = r0.y ? r0.w : 0;
  r0.w = cmp(0.125 >= v11.y);
  r0.y = r0.w ? r0.y : 0;
  r3.xyz = r0.yyy ? v11.xyz : v2.xyz;
  r2.xyz = r0.yyy ? float3(0,1,0) : r2.xyz;
  r0.y = cmp(unity_OrthoParams.w == 0.000000);
  r4.xyz = _WorldSpaceCameraPos.xyz + -r3.xyz;
  r0.w = dot(r4.xyz, r4.xyz);
  r0.w = rsqrt(r0.w);
  r4.xyz = r4.xyz * r0.www;
  r5.x = r0.y ? r4.x : (VD_COL(unity_MatrixV, 0).z);
  r5.y = r0.y ? r4.y : (VD_COL(unity_MatrixV, 1).z);
  r5.z = r0.y ? r4.z : (VD_COL(unity_MatrixV, 2).z);
  r4.xyz = (VD_COL(unity_MatrixVP, 1).xyw) * r3.yyy;
  r3.xyw = (VD_COL(unity_MatrixVP, 0).xyw) * r3.xxx + r4.xyz;
  r3.xyz = (VD_COL(unity_MatrixVP, 2).xyw) * r3.zzz + r3.xyw;
  r3.xyz = (VD_COL(unity_MatrixVP, 3).xyw) + r3.xyz;
  r0.yw = r3.xy / -r3.zz;
  r3.xy = r0.yw * float2(0.5,0.5) + float2(0.5,0.5);
  r0.x = saturate(dot(r0.xx, r0.zz));
  r0.y = vd_asint(_MainLightLayerMask) & vd_asint(unity_RenderingLayer.x);
  r4.xyz = r1.xyz * r0.xxx;
  r4.xyz = _MainLightColor.xyz * r4.xyz;
  r5.xyz = _MainLightPosition.xyz + r5.xyz;
  r0.z = dot(r5.xyz, r5.xyz);
  r0.z = max(6.10351563e-005, r0.z);
  r0.z = rsqrt(r0.z);
  r5.xyz = r5.xyz * r0.zzz;
  r0.z = saturate(dot(r2.xyz, r5.xyz));
  r0.z = log2(r0.z);
  r0.z = 128 * r0.z;
  r0.z = exp2(r0.z);
  r2.xyz = _MainLightColor.xyz * r0.zzz;
  r2.xyz = r2.xyz * r0.xxx;
  r2.xyz = unity_LightData.zzz * r2.xyz;
  r2.xyz = r4.xyz * float3(0.600000024,0.600000024,0.600000024) + r2.xyz;
  r0.yzw = r0.yyy ? r2.xyz : float3(0,0,0);
  r0.x = 1 + -r0.x;
  r1.w = dot(r1.xyz, float3(0.298999995,0.587000012,0.114));
  r1.w = r1.w * r1.w;
  r2.y = r1.w * r1.w;
  r2.x = 0.0125000002;
  r2.xy = float2(0,-0.0125000002) * r2.xy;
  r3.z = 1 + -r3.x;
  r2.xy = r2.xy * r0.xx + r3.zy;
  r2.xyzw = _AfterRenderingGrabpassTexture.SampleBias(sampler_vd_linear_clamp, r2.xy, _GlobalMipBias.x).xyzw;
  o0.xyz = r2.xyz * r1.xyz + r0.yzw;
  o0.w = 1;
  return;
}
