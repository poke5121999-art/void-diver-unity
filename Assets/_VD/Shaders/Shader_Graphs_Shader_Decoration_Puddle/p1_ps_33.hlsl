// Shader Graphs/Shader_Decoration_Puddle | pass 1 "DBufferMesh" | progFragment | keywords: _DBUFFER_MRT3 _DE_WEATHER_RAIN | blob 33
void frag(
  float4 v0 : SV_POSITION0,
  float4 v1 : INTERP0,
  float4 v2 : INTERP1,
  float3 v3 : INTERP2,
  float3 v4 : INTERP3,
  out float4 o0 : SV_Target0,
  out float4 o1 : SV_Target1,
  out float4 o2 : SV_Target2)
{
  float4 r0,r1,r2,r3,r4,r5;
  uint4 bitmask, uiDest;
  float4 fDest;

  r0.x = cmp(_ProjectionParams.x < 0);
  r0.y = _ScreenParams.y + -v0.y;
  r0.y = r0.x ? r0.y : v0.y;
  r0.x = v0.x;
  r0.xy = r0.xy / _ScreenParams.xy;
  r1.xyz = _EdgeColor.xyz + _EdgeColor.xyz;
  r0.z = 1 + -r0.y;
  r0.xyz = _PlaneReflectionTexture.SampleBias(sampler_PlaneReflectionTexture, r0.xz, _GlobalMipBias.x).xyz;
  r0.xyz = -_PlaneReflectionBackgroundColor.xyz + r0.xyz;
  r0.xyz = (_PlaneReflectionOpacity).xxx * r0.xyz + _PlaneReflectionBackgroundColor.xyz;
  r0.w = 0.100000001 * _TimeParameters.x;
  r2.xy = _TimeParameters.xx * float2(0.100000001,0.100000001) + v3.xy;
  r2.zw = floor(r2.xy);
  r2.xy = frac(r2.xy);
  r3.xy = (int2)r2.zw;
  r1.w = (int)r3.y ^ 0x41c64e6d;
  r3.x = (int)r1.w + (int)r3.x;
  r1.w = (int)r1.w * (int)r3.x;
  r3.x = (uint)r1.w >> 5;
  r1.w = (int)r1.w ^ (int)r3.x;
  r1.w = (int)r1.w * 0x27d4eb2d;
  r1.w = (uint)r1.w >> 8;
  r1.w = (uint)r1.w;
  r3.yz = r1.ww * float2(5.96046519e-008,5.96046519e-008) + float2(0.5,-0.5);
  r3.y = floor(r3.y);
  r3.x = r1.w * 5.96046519e-008 + -r3.y;
  r1.w = dot(r3.xz, r3.xz);
  r1.w = rsqrt(r1.w);
  r3.xy = r3.xz * r1.ww;
  r1.w = dot(r3.xy, r2.xy);
  r3.xyzw = float4(0,1,1,0) + r2.zwzw;
  r3.xyzw = (int4)r3.xyzw;
  r3.yw = (int2)r3.yw ^ int2(0,0);
  r3.xz = (int2)r3.yw + (int2)r3.xz;
  r3.xy = (int2)r3.yw * (int2)r3.xz;
  r3.zw = (uint2)r3.xy >> int2(5,5);
  r3.xy = (int2)r3.zw ^ (int2)r3.xy;
  r3.xy = (int2)r3.xy * int2(0x27d4eb2d,0x27d4eb2d);
  r3.xy = (uint2)r3.xy >> int2(8,8);
  r3.xy = (uint2)r3.xy;
  r4.xyzw = r3.xyxy * float4(5.96046519e-008,5.96046519e-008,5.96046519e-008,5.96046519e-008) + float4(0.5,0.5,-0.5,-0.5);
  r3.zw = floor(r4.xy);
  r4.xy = r3.xy * float2(5.96046519e-008,5.96046519e-008) + -r3.zw;
  r3.x = dot(r4.xz, r4.xz);
  r3.x = rsqrt(r3.x);
  r3.xy = r4.xz * r3.xx;
  r5.xyzw = float4(-0,-1,-1,-0) + r2.xyxy;
  r3.x = dot(r3.xy, r5.xy);
  r3.y = dot(r4.yw, r4.yw);
  r3.y = rsqrt(r3.y);
  r3.yz = r4.yw * r3.yy;
  r3.y = dot(r3.yz, r5.zw);
  r2.zw = float2(1,1) + r2.zw;
  r2.zw = (int2)r2.zw;
  r2.w = (int)r2.w ^ 0x41c64e6d;
  r2.z = (int)r2.w + (int)r2.z;
  r2.z = (int)r2.w * (int)r2.z;
  r2.w = (uint)r2.z >> 5;
  r2.z = (int)r2.w ^ (int)r2.z;
  r2.z = (int)r2.z * 0x27d4eb2d;
  r2.z = (uint)r2.z >> 8;
  r2.z = (uint)r2.z;
  r4.yz = r2.zz * float2(5.96046519e-008,5.96046519e-008) + float2(0.5,-0.5);
  r2.w = floor(r4.y);
  r4.x = r2.z * 5.96046519e-008 + -r2.w;
  r2.z = dot(r4.xz, r4.xz);
  r2.z = rsqrt(r2.z);
  r2.zw = r4.xz * r2.zz;
  r3.zw = float2(-1,-1) + r2.xy;
  r2.z = dot(r2.zw, r3.zw);
  r3.zw = r2.xy * r2.xy;
  r3.zw = r3.zw * r2.xy;
  r4.xy = r2.xy * float2(6,6) + float2(-15,-15);
  r2.xy = r2.xy * r4.xy + float2(10,10);
  r2.xy = r3.zw * r2.xy;
  r2.w = r3.x + -r1.w;
  r1.w = r2.y * r2.w + r1.w;
  r2.z = r2.z + -r3.y;
  r2.y = r2.y * r2.z + r3.y;
  r2.y = r2.y + -r1.w;
  r1.w = r2.x * r2.y + r1.w;
  r1.w = saturate(0.5 + r1.w);
  r1.w = 0.5 * r1.w;
  r2.x = _MainTex.SampleBias(sampler_MainTex, v2.xy, _GlobalMipBias.x).x;
  r2.y = 1 + -_Intensity;
  r2.x = -_Intensity + r2.x;
  r2.y = 1 / r2.y;
  r2.x = saturate(r2.x * r2.y);
  r2.y = r2.x * -2 + 3;
  r2.x = r2.x * r2.x;
  r2.z = r2.y * r2.x;
  r2.x = r2.y * r2.x + -0.5;
  r1.w = 1 / r1.w;
  r1.w = saturate(abs(r2.x) * r1.w);
  r2.x = r1.w * -2 + 3;
  r1.w = r1.w * r1.w;
  r1.w = r2.x * r1.w;
  r0.xyz = -_EdgeColor.xyz * float3(2,2,2) + r0.xyz;
  r0.xyz = r1.www * r0.xyz + r1.xyz;
  o0.xyz = _TintColor.xyz + r0.xyz;
  r0.x = _Opacity * r2.z;
  r0.yz = float2(5,5) * v2.xy;
  r1.xy = floor(r0.yz);
  r0.yz = frac(r0.yz);
  r1.zw = float2(0,8);
  r2.x = -1;
  while (true) {
    r2.y = cmp(1 < (int)r2.x);
    if (r2.y != 0) break;
    r3.y = (int)r2.x;
    r2.yz = r1.zw;
    r2.w = -1;
    while (true) {
      r3.z = cmp(1 < (int)r2.w);
      if (r3.z != 0) break;
      r3.x = (int)r2.w;
      r3.zw = r3.xy + r1.xy;
      r3.zw = (int2)r3.zw;
      r3.w = (int)r3.w ^ 0x41c64e6d;
      r3.z = (int)r3.w + (int)r3.z;
      r3.z = (int)r3.w * (int)r3.z;
      r4.x = (uint)r3.z >> 5;
      r3.z = (int)r3.z ^ (int)r4.x;
      r4.y = (int)r3.z * 0x27d4eb2d;
      r3.z = (uint)r4.y << 3;
      r4.x = (int)r3.z ^ (int)r3.w;
      r3.zw = (uint2)r4.xy >> int2(8,8);
      r3.zw = (uint2)r3.zw;
      r3.zw = r3.zw * r0.ww;
      r3.zw = float2(5.96046519e-008,5.96046519e-008) * r3.zw;
      r4.x = sin(r3.z);
      r4.y = cos(r3.w);
      r3.zw = r4.xy * float2(0.5,0.5) + float2(0.5,0.5);
      r3.xz = r3.xy + r3.zw;
      r3.xz = r3.xz + -r0.yz;
      r3.x = dot(r3.xz, r3.xz);
      r3.x = sqrt(r3.x);
      r3.z = cmp(r3.x < r2.z);
      r2.yz = r3.zz ? r3.xx : r2.yz;
      r2.w = (int)r2.w + 1;
    }
    r1.zw = r2.yz;
    r2.x = (int)r2.x + 1;
  }
  r0.y = r1.z * 0.25 + 0.75;
  r0.x = r0.x * r0.y;
  o0.w = _PlaneReflectionActive * r0.x;
  r0.x = dot(v4.xyz, v4.xyz);
  r0.x = rsqrt(r0.x);
  r0.xyz = v4.xyz * r0.xxx;
  o1.xyz = r0.xyz * float3(0.5,0.5,0.5) + float3(0.5,0.5,0.5);
  o1.w = 0;
  o2.xyzw = float4(0,1,0,0);
  return;
}
