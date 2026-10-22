// LCArt/Character/shader_DE_Character_Additive | pass 1 "Reflection" | progFragment | keywords: _DE_EDITOR_CAMERA _DE_PRERENDERING_CAMERA | blob 19
void frag(
  float4 v0 : SV_POSITION0,
  float2 v1 : TEXCOORD0,
  float2 w1 : TEXCOORD2,
  float3 v2 : TEXCOORD1,
  float w2 : TEXCOOR5,
  float4 v3 : TEXCOORD3,
  float4 v4 : TEXCOORD4,
  float4 v5 : TEXCOORD6,
  float4 v6 : TEXCOORD7,
  float4 v7 : TEXCOORD8,
  float4 v8 : COLOR0,
  out float4 o0 : SV_Target0)
{
  float4 r0,r1,r2;
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
  r0.x = cmp(_GhostEnable >= 0.00100000005);
  if (r0.x != 0) discard;
  r0.x = cmp(_IsReflected >= 0.00100000005);
  if (r0.x == 0) discard;
  r0.xyzw = _MainTex.SampleBias(sampler_MainTex, v1.xy, _GlobalMipBias.x).xyzw;
  r1.x = -0.100000001 + r0.w;
  r1.x = cmp(r1.x < 0);
  if (r1.x != 0) discard;
  r0.xyz = -_OutOfSightColor.xyz + r0.xyz;
  r0.xyz = w2.xww * r0.xyz + _OutOfSightColor.xyz;
  r1.xy = v7.xy / v7.ww;
  r1.z = _Effect + w2.x;
  r2.xy = _ScreenSize.xy / _ScreenParams.xy;
  r1.xy = _ScreenParams.xy * r1.xy;
  r1.xy = r1.xy * r2.xy;
  r1.xy = float2(0.5,0.5) * r1.xy;
  r1.xy = floor(r1.xy);
  r1.xy = (uint2)r1.xy;
  r1.xy = (int2)r1.xy & int2(3,3);
  r1.y = (uint)r1.y << 2;
  r1.x = (int)r1.y + (int)r1.x;
  r1.y = x0[r1.x+0].x;
  r1.z = r1.z + -r1.y;
  r1.z = cmp(r1.z < 0);
  if (r1.z != 0) discard;
  r1.y = -r1.y + r0.w;
  r1.y = cmp(r1.y < 0);
  if (r1.y != 0) discard;
  r1.y = cmp(_GlobalBoxAreaEnable >= 0.5);
  if (r1.y != 0) {
    r1.y = 0;
    while (true) {
      r1.z = cmp((int)r1.y >= 8);
      if (r1.z != 0) break;
      r1.z = cmp(_GlobalBoxAreaCoverCharacters[(int)(r1.y)] < 0.5);
      if (r1.z != 0) {
        r1.z = (int)r1.y + 1;
        r1.y = r1.z;
        continue;
      }
      r2.xyz = -_GlobalBoxAreaCenters[(int)(r1.y)].xyz + (VD_COL(unity_ObjectToWorld, 3).xyz);
      r2.xyz = -_GlobalBoxAreaExtents[(int)(r1.y)].xyz + abs(r2.xyz);
      r1.z = max(r2.x, r2.y);
      r1.z = max(r1.z, r2.z);
      r1.z = cmp(0 >= r1.z);
      if (r1.z != 0) {
        if (-1 != 0) discard;
        break;
      }
      r1.y = (int)r1.y + 1;
    }
  }
  r1.y = -_GlobalFogHeightMin + v3.y;
  r1.z = _GlobalFogHeightMax + -_GlobalFogHeightMin;
  r1.y = r1.y / r1.z;
  r1.y = saturate(1 + -r1.y);
  r1.y = _GlobalFogHeightEnable * r1.y;
  r1.y = max(0, r1.y);
  r2.xyz = unity_FogColor.xyz + -r0.xyz;
  r0.xyz = r1.yyy * r2.xyz + r0.xyz;
  r1.y = saturate(_DissolveProgress + _DissolveProgress);
  r2.xyz = _DissolveColor.xyz + -r0.xyz;
  r0.xyz = r1.yyy * r2.xyz + r0.xyz;
  r1.y = saturate(-0.5 + _DissolveProgress);
  r1.y = -r1.y * 2 + 1;
  r1.x = x0[r1.x+0].x;
  r1.x = r0.w * r1.y + -r1.x;
  r1.x = cmp(r1.x < 0);
  if (r1.x != 0) discard;
  r1.x = saturate(-_PlaneReflectionPivotHeight + v3.y);
  r1.x = 1 + -r1.x;
  r1.y = -0.25 + _PlaneReflectionPivotHeight;
  r1.y = -(VD_COL(unity_ObjectToWorld, 3).y) + r1.y;
  r1.y = saturate(-4 * r1.y);
  r1.x = r1.x * r1.y;
  r0.xyz = r0.xyz * float3(0.75,0.75,0.75) + -_PlaneReflectionBackgroundColor.xyz;
  o0.xyz = r1.xxx * r0.xyz + _PlaneReflectionBackgroundColor.xyz;
  o0.w = r0.w;
  return;
}
