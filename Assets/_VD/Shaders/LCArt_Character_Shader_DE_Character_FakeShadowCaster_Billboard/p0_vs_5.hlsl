// LCArt/Character/Shader_DE_Character_FakeShadowCaster_Billboard | pass 0 "FakeShadowCast" | progVertex | keywords: _DE_EDITOR_CAMERA _DE_PRERENDERING_CAMERA | blob 5
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

  r0.x = cmp(0 >= _FakeShadowScale);
  if (r0.x != 0) {
    r0.x = dot((VD_COL(unity_ObjectToWorld, 0).xyz), (VD_COL(unity_ObjectToWorld, 0).xyz));
    r0.y = dot((VD_COL(unity_ObjectToWorld, 2).xyz), (VD_COL(unity_ObjectToWorld, 2).xyz));
    r0.xy = sqrt(r0.xy);
    r0.x = v0.x * r0.x + (VD_COL(unity_ObjectToWorld, 3).x);
    r0.y = v0.z * r0.y + (VD_COL(unity_ObjectToWorld, 3).z);
    r1.xyzw = (_FakeShadowGroundY).xxxx * (VD_COL(unity_MatrixVP, 1).xyzw);
    r1.xyzw = (VD_COL(unity_MatrixVP, 0).xyzw) * r0.xxxx + r1.xyzw;
    r0.xyzw = (VD_COL(unity_MatrixVP, 2).xyzw) * r0.yyyy + r1.xyzw;
    r0.xyzw = (VD_COL(unity_MatrixVP, 3).xyzw) + r0.xyzw;
    r1.x = cmp(_FakeShadowVisibility >= 0.5);
    r1.x = r1.x ? 1.000000 : 0;
    r0.xyzw = float4(-0,-0,-0,-1) + r0.xyzw;
    o0.xyzw = r1.xxxx * r0.xyzw + float4(0,0,0,1);
    o1.xy = v1.xy;
    p1.x = 1;
    p1.x = _FakeShadowVisibility;
    return;
  }
  r0.x = dot((VD_COL(unity_ObjectToWorld, 0).xyz), (VD_COL(unity_ObjectToWorld, 0).xyz));
  r0.y = dot((VD_COL(unity_ObjectToWorld, 1).xyz), (VD_COL(unity_ObjectToWorld, 1).xyz));
  r0.xy = sqrt(r0.xy);
  r0.xy = v0.xy * r0.xy;
  r0.z = cmp(0.5 < _FakeShadowIsPoint);
  if (r0.z != 0) {
    r0.z = cmp(0 >= _FakeShadowPointRange);
    if (r0.z != 0) {
      o0.xyzw = float4(0,0,0,1);
      o1.xyz = float3(0,0,0);
      p1.x = _FakeShadowVisibility;
      return;
    }
    r1.xyz = _FakeShadowLightData.xyz + -(VD_COL(unity_ObjectToWorld, 3).xyz);
    r0.z = dot(r1.xz, r1.xz);
    r0.w = dot(r1.xyz, r1.xyz);
    r0.w = rsqrt(r0.w);
    r1.xyz = r1.xyz * r0.www;
    r0.w = dot(-r1.xz, -r1.xz);
    r2.x = rsqrt(r0.w);
    r2.yz = r2.xx * -r1.xz;
    r3.x = r2.z;
    r3.y = 0;
    r2.xw = -r2.xy * float2(0,1) + r3.xy;
    r0.w = dot(r2.xw, r2.xw);
    r0.w = rsqrt(r0.w);
    r2.xw = r2.xw * r0.ww;
    r0.w = dot(r1.xz, r1.xz);
    r0.zw = sqrt(r0.zw);
    r0.w = _FakeShadowScale * r0.w;
    r1.y = max(0.00999999978, abs(r1.y));
    r0.w = r0.w / r1.y;
    r1.yw = r2.yz * r0.ww;
    r3.x = (VD_COL(unity_MatrixV, 0).z);
    r3.y = (VD_COL(unity_MatrixV, 2).z);
    r0.w = dot(r3.xy, r1.xz);
    r0.w = cmp(r0.w >= 0);
    r0.w = r0.w ? 1 : -1;
    r1.xz = r2.xw * r0.ww;
    r1.yw = r1.yw * r0.yy;
    r1.xy = r1.xz * r0.xx + r1.yw;
    r1.xy = (VD_COL(unity_ObjectToWorld, 3).xz) + r1.xy;
    r0.w = max(0.00100000005, _FakeShadowPointRange);
    r0.w = r0.z / r0.w;
    r0.w = min(1, r0.w);
    r0.w = 1 + -r0.w;
    r0.w = r0.w * r0.w;
    r0.z = -0.5 + r0.z;
    r0.z = saturate(r0.z + r0.z);
    r2.x = r0.z * r0.w;
  } else {
    r0.z = dot(-_FakeShadowLightData.xz, -_FakeShadowLightData.xz);
    r3.x = rsqrt(r0.z);
    r3.yz = -_FakeShadowLightData.xz * r3.xx;
    r4.x = r3.z;
    r4.y = 0;
    r0.zw = -r3.xy * float2(0,1) + r4.xy;
    r1.z = dot(r0.zw, r0.zw);
    r1.z = rsqrt(r1.z);
    r0.zw = r1.zz * r0.zw;
    r1.z = dot(_FakeShadowLightData.xz, _FakeShadowLightData.xz);
    r1.z = sqrt(r1.z);
    r1.z = _FakeShadowScale * r1.z;
    r1.w = max(0.00999999978, abs(_FakeShadowLightData.y));
    r1.z = r1.z / r1.w;
    r1.zw = r3.yz * r1.zz;
    r3.x = (VD_COL(unity_MatrixV, 0).z);
    r3.y = (VD_COL(unity_MatrixV, 2).z);
    r3.x = dot(r3.xy, _FakeShadowLightData.xz);
    r3.x = cmp(r3.x >= 0);
    r3.x = r3.x ? 1 : -1;
    r0.zw = r3.xx * r0.zw;
    r1.zw = r1.zw * r0.yy;
    r0.xy = r0.zw * r0.xx + r1.zw;
    r1.xy = (VD_COL(unity_ObjectToWorld, 3).xz) + r0.xy;
    r2.x = 1;
  }
  r0.xyzw = (_FakeShadowGroundY).xxxx * (VD_COL(unity_MatrixVP, 1).xyzw);
  r0.xyzw = (VD_COL(unity_MatrixVP, 0).xyzw) * r1.xxxx + r0.xyzw;
  r0.xyzw = (VD_COL(unity_MatrixVP, 2).xyzw) * r1.yyyy + r0.xyzw;
  r0.xyzw = (VD_COL(unity_MatrixVP, 3).xyzw) + r0.xyzw;
  r1.x = cmp(_FakeShadowVisibility >= 0.5);
  r1.x = r1.x ? 1.000000 : 0;
  r0.xyzw = float4(-0,-0,-0,-1) + r0.xyzw;
  o0.xyzw = r1.xxxx * r0.xyzw + float4(0,0,0,1);
  r2.yz = v1.xy;
  r2.w = _FakeShadowVisibility;
  o1.xyzw = r2.yzxw;
  return;
}
