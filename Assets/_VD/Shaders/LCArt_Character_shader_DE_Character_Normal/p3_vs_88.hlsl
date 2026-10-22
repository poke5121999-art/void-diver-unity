// LCArt/Character/shader_DE_Character_Normal | pass 3 "RimLight" | progVertex | keywords: _CLUSTER_LIGHT_LOOP _DE_PRERENDERING_CAMERA | blob 88
void vert(
  float4 v0 : POSITION0,
  float3 v1 : NORMAL0,
  float2 v2 : TEXCOORD0,
  float4 v3 : COLOR0,
  out float4 o0 : SV_POSITION0,
  out float3 o1 : TEXCOOR2,
  out float2 o2 : TEXCOORD0,
  out float p2 : TEXCOORD2,
  out float4 o3 : TEXCOORD1,
  out float4 o4 : TEXCOORD3,
  out float4 o5 : COLOR0)
{
  float4 r0,r1,r2,r3,r4;
  uint4 bitmask, uiDest;
  float4 fDest;

  r0.x = 1 / (VD_COL(unity_MatrixV, 1).y);
  r0.y = min(1.5, abs(r0.x));
  r1.xyz = (VD_COL(unity_ObjectToWorld, 3).xyz) + -_WorldSpaceCameraPos.xyz;
  r0.w = dot(r1.xyz, r1.xyz);
  r0.w = rsqrt(r0.w);
  r1.xyz = r1.xyz * r0.www;
  r1.w = -r1.x;
  r2.xy = v0.xx * r1.zw;
  r0.w = cmp(v0.y >= 0);
  r0.w = r0.w ? 1.000000 : 0;
  r3.x = (VD_COL(unity_MatrixV, 0).y);
  r3.y = (VD_COL(unity_MatrixV, 1).y);
  r3.z = (VD_COL(unity_MatrixV, 2).y);
  r0.xz = float2(0,0);
  r0.xyz = -r3.xyz + r0.xyz;
  r0.xyz = r0.www * r0.xyz + r3.xyz;
  r0.w = cmp(v0.y >= _OpticalRatioYThreshold);
  r0.w = r0.w ? 1.000000 : 0;
  r1.w = -1 + _OpticalRatioYPositiveRatio;
  r0.w = r0.w * r1.w + 1;
  r2.xz = float2(1.17499995,1.17499995) * r2.xy;
  r2.y = 0;
  r0.xyz = r0.xyz * v0.yyy + r2.xyz;
  r0.w = v0.z * r0.w + 0.00999999978;
  r0.xyz = r1.xyz * r0.www + r0.xyz;
  r1.xyz = (VD_COL(unity_ObjectToWorld, 1).xyz) * r0.yyy;
  r0.xyw = (VD_COL(unity_ObjectToWorld, 0).xyz) * r0.xxx + r1.xyz;
  r0.xyz = (VD_COL(unity_ObjectToWorld, 2).xyz) * r0.zzz + r0.xyw;
  r0.xyz = (VD_COL(unity_ObjectToWorld, 3).xyz) + r0.xyz;
  r1.xyzw = (VD_COL(unity_MatrixVP, 1).xyzw) * r0.yyyy;
  r1.xyzw = (VD_COL(unity_MatrixVP, 0).xyzw) * r0.xxxx + r1.xyzw;
  r1.xyzw = (VD_COL(unity_MatrixVP, 2).xyzw) * r0.zzzz + r1.xyzw;
  r1.xyzw = (VD_COL(unity_MatrixVP, 3).xyzw) + r1.xyzw;
  r2.xz = float2(0.5,0.5) * r1.xw;
  r0.w = _ProjectionParams.x * r1.y;
  r2.w = 0.5 * r0.w;
  o4.xy = r2.xw + r2.zz;
  r0.w = dot(_MainLightPosition.xyz, _MainLightPosition.xyz);
  r0.w = rsqrt(r0.w);
  r2.xyz = _MainLightPosition.xyz * r0.www;
  r3.xyz = float3(0,0,0);
  r0.w = 0;
  while (true) {
    r2.w = cmp((int)r0.w >= vd_asint(_RimEffectCount));
    if (r2.w != 0) break;
    r4.xyz = -_RimEffectPositions[(int)(r0.w)].xyz + r0.xyz;
    r2.w = dot(r4.xyz, r4.xyz);
    r3.w = rsqrt(r2.w);
    r4.xyz = r4.xyz * r3.www;
    r2.w = sqrt(r2.w);
    r2.w = r2.w / _RimEffectRanges[(int)(r0.w)];
    r2.w = saturate(1 + -r2.w);
    r3.xyz = r4.xyz * r2.www + r3.xyz;
    r0.w = (int)r0.w + 1;
  }
  r2.xyz = r2.xyz * float3(-0.5,-0.5,-0.5) + r3.xyz;
  r0.w = dot(r2.xyz, r2.xyz);
  r0.w = rsqrt(r0.w);
  r2.xyz = r2.xyz * r0.www;
  r0.w = cmp(unity_OrthoParams.w == 0.000000);
  r3.xyz = _WorldSpaceCameraPos.xyz + -r0.xyz;
  r4.x = r0.w ? r3.x : (VD_COL(unity_MatrixV, 0).z);
  r4.y = r0.w ? r3.y : (VD_COL(unity_MatrixV, 1).z);
  r4.z = r0.w ? r3.z : (VD_COL(unity_MatrixV, 2).z);
  r0.w = dot(r4.xyz, r4.xyz);
  r0.w = rsqrt(r0.w);
  r3.xyz = r4.xyz * r0.www;
  r0.w = dot(r2.xyz, r3.xyz);
  r2.xyz = -r0.www * r3.xyz + r2.xyz;
  r0.w = dot(-r2.xyz, -r2.xyz);
  r0.w = rsqrt(r0.w);
  r2.xyz = -r2.xyz * r0.www;
  r3.x = _ScreenParams.x / _ScreenParams.y;
  r2.w = 0.0125000002 * r2.x;
  r3.yz = float2(0.0125000002,0.0125000002);
  r2.xyz = r2.wyz * r3.xyz + r0.xyz;
  r2.yw = (VD_COL(unity_MatrixVP, 1).xy) * r2.yy;
  r2.xy = (VD_COL(unity_MatrixVP, 0).xy) * r2.xx + r2.yw;
  r2.xy = (VD_COL(unity_MatrixVP, 2).xy) * r2.zz + r2.xy;
  r1.xy = (VD_COL(unity_MatrixVP, 3).xy) + r2.xy;
  r2.xyz = (VD_COL(unity_ObjectToWorld, 1).xyz) * v1.yyy;
  r2.xyz = (VD_COL(unity_ObjectToWorld, 0).xyz) * v1.xxx + r2.xyz;
  r2.xyz = (VD_COL(unity_ObjectToWorld, 2).xyz) * v1.zzz + r2.xyz;
  o1.xyz = (VD_COL(unity_ObjectToWorld, 3).xyz) + r2.xyz;
  r2.xyz = float3(0,0,0);
  r0.w = 0;
  while (true) {
    r2.w = cmp((int)r0.w >= vd_asint(_RimEffectCount));
    if (r2.w != 0) break;
    r3.xyz = -_RimEffectPositions[(int)(r0.w)].xyz + r0.xyz;
    r2.w = dot(r3.xyz, r3.xyz);
    r2.w = sqrt(r2.w);
    r2.w = saturate(r2.w / _RimEffectRanges[(int)(r0.w)]);
    r2.w = 1 + -r2.w;
    r3.xyz = _RimEffectColors[(int)(r0.w)].xyz * r2.www;
    r2.xyz = max(r3.xyz, r2.xyz);
    r0.w = (int)r0.w + 1;
  }
  r0.xyz = float3(0.5,0.5,0.5) * r2.xyz;
  o3.xyz = max(_MainLightColor.xyz, r0.xyz);
  r0.xyzw = float4(0,0,0,1) + -r1.xyzw;
  o0.xyzw = (_OutlineEnable).xxxx * r0.xyzw + r1.xyzw;
  o4.zw = r1.zw;
  o5.xyzw = v3.xyzw;
  o2.xy = v2.xy;
  p2.x = 1;
  return;
}
