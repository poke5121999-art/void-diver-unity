// LCArt/Character/shader_DE_Character_Trail | pass 0 "ForwardLit" | progFragment | keywords: (none) | blob 2
void frag(
  float4 v0 : SV_POSITION0,
  float2 v1 : TEXCOORD0,
  float2 w1 : TEXCOORD2,
  float3 v2 : TEXCOORD1,
  float w2 : TEXCOORD5,
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
  r0.xyzw = _MainTex.SampleBias(sampler_MainTex, v1.xy, _GlobalMipBias.x).xyzw;
  r1.x = -0.100000001 + r0.w;
  r1.x = cmp(r1.x < 0);
  if (r1.x != 0) discard;
  r1.xy = v7.xy / v7.ww;
  r1.xy = _ScreenParams.xy * r1.xy;
  r1.zw = float2(0.5,0.5) * r1.xy;
  r2.xy = _ScreenSize.xy / _ScreenParams.xy;
  r1.xyzw = r2.xyxy * r1.xyzw;
  r1.xyzw = floor(r1.xyzw);
  r1.xyzw = (uint4)r1.xyzw;
  r1.xyzw = (int4)r1.xyzw & int4(3,3,3,3);
  r1.w = (uint)r1.w << 2;
  r1.z = (int)r1.w + (int)r1.z;
  r1.z = x0[r1.z+0].x;
  r1.w = _Effect + w2.x;
  r1.z = r1.w + -r1.z;
  r1.z = cmp(r1.z < 0);
  if (r1.z != 0) discard;
  r1.y = (uint)r1.y << 2;
  r1.x = (int)r1.y + (int)r1.x;
  r1.x = x0[r1.x+0].x;
  r1.y = saturate(_IsVertexColorAlphaUsed);
  r1.z = -1 + v8.w;
  r1.y = r1.y * r1.z + 1;
  r0.w = r1.y * r0.w;
  r0.w = r0.w * r0.w;
  r1.x = r0.w * v8.w + -r1.x;
  r0.xyzw = v8.xyzw * r0.xyzw;
  o0.w = r0.w;
  r0.w = cmp(r1.x < 0);
  if (r0.w != 0) discard;
  r1.xy = (VD_COL(unity_ObjectToWorld, 3).xz) + -_PlaneBoundMaxPos.xz;
  r1.zw = -_PlaneBoundMaxPos.xz + _PlaneBoundMinPos.xz;
  r1.xy = saturate(r1.xy / r1.zw);
  r2.xyzw = _PlanePointBuffer.SampleLevel(sampler_PlanePointBuffer, r1.xy, 0).xyzw;
  r0.w = saturate(4 * r2.w);
  r1.x = r0.w * -2 + 3;
  r0.w = r0.w * r0.w;
  r0.w = r1.x * r0.w;
  r0.w = _GlobalPlanePointBufferEnable * r0.w;
  r1.xy = -_PlaneBoundMaxPos.xz + v4.xz;
  r1.xy = saturate(r1.xy / r1.zw);
  r1.xyzw = _PlaneSightRenderTarget.SampleLevel(sampler_PlaneSightRenderTarget, r1.xy, 0).xyzw;
  r1.x = _GlobalPlaneSystemEnable * r1.w;
  r0.w = max(r1.x, r0.w);
  r1.xyz = r0.xyz * r0.www;
  r0.w = dot(v2.xyz, _MainLightPosition.xyz);
  r0.w = saturate(r0.w * 0.5 + 0.5);
  r2.xyz = _MainLightColor.xyz * unity_LightData.zzz + float3(-1,-1,-1);
  r2.xyz = r2.xyz * r0.www + float3(1,1,1);
  r0.xyz = r2.xyz * r0.xyz;
  r0.w = vd_asint(_MainLightLayerMask) & vd_asint(unity_RenderingLayer.x);
  r0.xyz = r0.www ? r0.xyz : float3(0,0,0);
  r0.xyz = r1.xyz * float3(0.25,0.25,0.25) + r0.xyz;
  r0.xyz = r0.xyz * v8.xyz + float3(-0.100000001,-0.100000001,-0.100000001);
  r0.xyz = w2.xww * r0.xyz + float3(0.100000001,0.100000001,0.100000001);
  r1.xyz = unity_FogColor.xyz + -r0.xyz;
  r0.w = -_GlobalFogHeightMin + v3.y;
  r1.w = _GlobalFogHeightMax + -_GlobalFogHeightMin;
  r0.w = r0.w / r1.w;
  r0.w = saturate(1 + -r0.w);
  r0.w = _GlobalFogHeightEnable * r0.w;
  r0.w = max(0, r0.w);
  o0.xyz = r0.www * r1.xyz + r0.xyz;
  return;
}
