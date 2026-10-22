// LCArt/Character/shader_DE_Character_Normal | pass 4 "StencilColor" | progFragment | keywords: _DE_EDITOR_CAMERA _DE_PRERENDERING_CAMERA | blob 121
void frag(
  float4 v0 : SV_POSITION0,
  float2 v1 : TEXCOORD0,
  float2 w1 : TEXCOORD2,
  float3 v2 : TEXCOORD3,
  float w2 : TEXCOORD5,
  float4 v3 : TEXCOORD4,
  float4 v4 : TEXCOORD8,
  float4 v5 : COLOR0,
  out float4 o0 : SV_Target0)
{
  float4 r0,r1;
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
  r0.x = _GlobalPlaneSystemEnable * _GlobalIsRuntime;
  r0.y = _GlobalPlaneSystemEnable * _GlobalIsRuntime + -0.100000001;
  r0.y = cmp(r0.y < 0);
  if (r0.y != 0) discard;
  r0.y = -1 + w2.x;
  r0.x = r0.x * r0.y + 0.5;
  r0.x = cmp(r0.x < 0);
  if (r0.x != 0) discard;
  r0.xyzw = _MainTex.SampleBias(sampler_MainTex, v1.xy, _GlobalMipBias.x).xyzw;
  r0.x = -0.100000001 + r0.w;
  r0.x = cmp(r0.x < 0);
  if (r0.x != 0) discard;
  r0.x = saturate(1 + -_DissolveProgress);
  r0.x = cmp(r0.x >= 0.5);
  if (r0.x == 0) discard;
  r0.xyz = (VD_COL(unity_MatrixVP, 1).xyw) * v2.yyy;
  r0.xyz = (VD_COL(unity_MatrixVP, 0).xyw) * v2.xxx + r0.xyz;
  r0.xyz = (VD_COL(unity_MatrixVP, 2).xyw) * v2.zzz + r0.xyz;
  r0.xyz = (VD_COL(unity_MatrixVP, 3).xyw) + r0.xyz;
  r0.xy = r0.xy / r0.zz;
  r0.xy = r0.xy * float2(0.5,-0.5) + float2(0.5,0.5);
  r1.xyzw = _CustomDepthBuffer.SampleBias(sampler_CustomDepthBuffer, r0.xy, _GlobalMipBias.x).xyzw;
  r0.z = -9.99999975e-005 + r1.y;
  r0.z = cmp(r0.z < 0);
  if (r0.z != 0) discard;
  r0.z = -v0.z + r1.y;
  r0.z = cmp(r0.z < 0);
  if (r0.z != 0) discard;
  r0.zw = _ScreenSize.xy / _ScreenParams.xy;
  r0.xy = _ScreenParams.xy * r0.xy;
  r0.xy = r0.xy * r0.zw;
  r0.xy = floor(r0.xy);
  r0.xy = (uint2)r0.xy;
  r0.xy = (int2)r0.xy & int2(3,3);
  r0.y = (uint)r0.y << 2;
  r0.x = (int)r0.y + (int)r0.x;
  r0.x = x0[r0.x+0].x;
  r0.x = 0.25 + -r0.x;
  r0.x = cmp(r0.x < 0);
  if (r0.x != 0) discard;
  r0.x = cmp(_GlobalBoxAreaEnable >= 0.5);
  if (r0.x != 0) {
    r0.x = 0;
    while (true) {
      r0.y = cmp((int)r0.x >= 8);
      if (r0.y != 0) break;
      r0.y = cmp(_GlobalBoxAreaCoverCharacters[(int)(r0.x)] < 0.5);
      if (r0.y != 0) {
        r0.y = (int)r0.x + 1;
        r0.x = r0.y;
        continue;
      }
      r0.yzw = -_GlobalBoxAreaCenters[(int)(r0.x)].xyz + (VD_COL(unity_ObjectToWorld, 3).xyz);
      r0.yzw = -_GlobalBoxAreaExtents[(int)(r0.x)].xyz + abs(r0.yzw);
      r0.y = max(r0.y, r0.z);
      r0.y = max(r0.y, r0.w);
      r0.y = cmp(0 >= r0.y);
      if (r0.y != 0) {
        if (-1 != 0) discard;
        break;
      }
      r0.x = (int)r0.x + 1;
    }
  }
  r0.xyz = _GlobalEnemyStencilColor.xyz + -_GlobalCharacterStencilColor.xyz;
  o0.xyz = (_IsEnemy).xxx * r0.xyz + _GlobalCharacterStencilColor.xyz;
  o0.w = 1;
  return;
}
