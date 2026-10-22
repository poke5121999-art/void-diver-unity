// LCArt/PostProcessing/Shader_DE_PP_UnifiedShadow | pass 1 "FakeShadowCast" | progVertex | keywords: _DE_FAKE_SHADOW_PLANE_SIGHT _DE_PRERENDERING_CAMERA | blob 23
void vert(
  float4 v0 : POSITION0,
  float2 v1 : TEXCOORD0,
  out float4 o0 : SV_POSITION0,
  out float2 o1 : TEXCOORD0,
  out float p1 : TEXCOORD1,
  out float p1 : TEXCOORD2)
{
  float4 r0,r1,r2,r3,r4;
  uint4 bitmask, uiDest;
  float4 fDest;

  r0.x = cmp(_GlobalBoxAreaEnable >= 0.5);
  if (r0.x != 0) {
    r0.w = _FakeShadowVisibility;
    r1.x = 0;
    while (true) {
      r1.y = cmp((int)r1.x >= 8);
      if (r1.y != 0) break;
      r1.y = cmp(_GlobalBoxAreaCoverCharacters[(int)(r1.x)] < 0.5);
      if (r1.y != 0) {
        r1.y = (int)r1.x + 1;
        r1.x = r1.y;
        continue;
      }
      r1.yzw = -_GlobalBoxAreaCenters[(int)(r1.x)].xyz + (VD_COL(unity_ObjectToWorld, 3).xyz);
      r1.yzw = -_GlobalBoxAreaExtents[(int)(r1.x)].xyz + abs(r1.yzw);
      r1.y = max(r1.y, r1.z);
      r1.y = max(r1.y, r1.w);
      r1.y = cmp(0 >= r1.y);
      if (r1.y != 0) {
        o0.xyzw = float4(0,0,0,1);
        o1.xyzw = float4(0,0,0,0);
        return;
      }
      r1.x = (int)r1.x + 1;
    }
  } else {
    r0.w = _FakeShadowVisibility;
  }
  r1.x = cmp(0 >= _FakeShadowScale);
  if (r1.x != 0) {
    r1.x = dot((VD_COL(unity_ObjectToWorld, 0).xyz), (VD_COL(unity_ObjectToWorld, 0).xyz));
    r1.y = dot((VD_COL(unity_ObjectToWorld, 2).xyz), (VD_COL(unity_ObjectToWorld, 2).xyz));
    r1.xy = sqrt(r1.xy);
    r1.x = v0.x * r1.x + (VD_COL(unity_ObjectToWorld, 3).x);
    r1.y = v0.z * r1.y + (VD_COL(unity_ObjectToWorld, 3).z);
    r2.xyzw = (_FakeShadowGroundY).xxxx * (VD_COL(unity_MatrixVP, 1).xyzw);
    r2.xyzw = (VD_COL(unity_MatrixVP, 0).xyzw) * r1.xxxx + r2.xyzw;
    r1.xyzw = (VD_COL(unity_MatrixVP, 2).xyzw) * r1.yyyy + r2.xyzw;
    r1.xyzw = (VD_COL(unity_MatrixVP, 3).xyzw) + r1.xyzw;
    r2.x = cmp(r0.w >= 0.5);
    r2.x = r2.x ? 1.000000 : 0;
    r1.xyzw = float4(-0,-0,-0,-1) + r1.xyzw;
    o0.xyzw = r2.xxxx * r1.xyzw + float4(0,0,0,1);
    r0.xy = v1.xy;
    r0.z = 1;
    o1.xyzw = r0.xyzw;
    return;
  }
  r1.x = cmp(0.5 < _FakeShadowIsPoint);
  if (r1.x != 0) {
    r1.x = cmp(0 >= _FakeShadowPointRange);
    if (r1.x != 0) {
      o0.xyzw = float4(0,0,0,1);
      o1.xyz = float3(0,0,0);
      p1.x = r0.w;
      return;
    }
    r1.xyz = _FakeShadowLightData.xyz + -(VD_COL(unity_ObjectToWorld, 3).xyz);
    r1.w = dot(r1.xz, r1.xz);
    r1.w = sqrt(r1.w);
    r2.x = dot(r1.xyz, r1.xyz);
    r2.x = rsqrt(r2.x);
    r1.xyz = r2.xxx * r1.xyz;
    r2.x = dot(-r1.xz, -r1.xz);
    r2.x = rsqrt(r2.x);
    r2.yz = r2.xx * -r1.xz;
    r3.x = r2.z;
    r3.y = 0;
    r2.xw = -r2.xy * float2(0,1) + r3.xy;
    r3.x = dot(r2.xw, r2.xw);
    r3.x = rsqrt(r3.x);
    r2.xw = r3.xx * r2.xw;
    r3.x = dot(r1.xz, r1.xz);
    r3.x = sqrt(r3.x);
    r3.x = _FakeShadowScale * r3.x;
    r1.y = max(0.00999999978, abs(r1.y));
    r1.y = r3.x / r1.y;
    r2.yz = r2.yz * r1.yy;
    r3.x = (VD_COL(unity_MatrixV, 0).z);
    r3.y = (VD_COL(unity_MatrixV, 2).z);
    r1.x = dot(r3.xy, r1.xz);
    r1.x = cmp(r1.x >= 0);
    r1.x = r1.x ? 1 : -1;
    r1.xy = r2.xw * r1.xx;
    r1.z = max(0.00100000005, _FakeShadowPointRange);
    r1.z = r1.w / r1.z;
    r1.z = min(1, r1.z);
    r1.z = 1 + -r1.z;
    r1.z = r1.z * r1.z;
    r1.w = -0.5 + r1.w;
    r1.w = saturate(r1.w + r1.w);
    r0.x = r1.w * r1.z;
  } else {
    r1.z = dot(-_FakeShadowLightData.xz, -_FakeShadowLightData.xz);
    r3.x = rsqrt(r1.z);
    r3.yz = -_FakeShadowLightData.xz * r3.xx;
    r4.x = r3.z;
    r4.y = 0;
    r1.zw = -r3.xy * float2(0,1) + r4.xy;
    r2.x = dot(r1.zw, r1.zw);
    r2.x = rsqrt(r2.x);
    r1.zw = r2.xx * r1.zw;
    r2.x = dot(_FakeShadowLightData.xz, _FakeShadowLightData.xz);
    r2.x = sqrt(r2.x);
    r2.x = _FakeShadowScale * r2.x;
    r2.w = max(0.00999999978, abs(_FakeShadowLightData.y));
    r2.x = r2.x / r2.w;
    r2.yz = r3.yz * r2.xx;
    r3.x = (VD_COL(unity_MatrixV, 0).z);
    r3.y = (VD_COL(unity_MatrixV, 2).z);
    r2.x = dot(r3.xy, _FakeShadowLightData.xz);
    r2.x = cmp(r2.x >= 0);
    r2.x = r2.x ? 1 : -1;
    r1.xy = r2.xx * r1.zw;
    r0.x = 1;
  }
  r1.z = dot((VD_COL(unity_ObjectToWorld, 0).xyz), (VD_COL(unity_ObjectToWorld, 0).xyz));
  r1.w = dot((VD_COL(unity_ObjectToWorld, 1).xyz), (VD_COL(unity_ObjectToWorld, 1).xyz));
  r1.zw = sqrt(r1.zw);
  r1.zw = v0.xy * r1.zw;
  r2.xy = r2.yz * r1.ww;
  r1.xy = r1.xy * r1.zz + r2.xy;
  r1.xy = (VD_COL(unity_ObjectToWorld, 3).xz) + r1.xy;
  r2.xyzw = (_FakeShadowGroundY).xxxx * (VD_COL(unity_MatrixVP, 1).xyzw);
  r2.xyzw = (VD_COL(unity_MatrixVP, 0).xyzw) * r1.xxxx + r2.xyzw;
  r1.xyzw = (VD_COL(unity_MatrixVP, 2).xyzw) * r1.yyyy + r2.xyzw;
  r1.xyzw = (VD_COL(unity_MatrixVP, 3).xyzw) + r1.xyzw;
  r2.x = cmp(r0.w >= 0.5);
  r2.x = r2.x ? 1.000000 : 0;
  r1.xyzw = float4(-0,-0,-0,-1) + r1.xyzw;
  o0.xyzw = r2.xxxx * r1.xyzw + float4(0,0,0,1);
  r0.yz = v1.xy;
  o1.xyzw = r0.yzxw;
  return;
}
