// Shader Graphs/Shader_Decoration_Puddle | pass 1 "DBufferMesh" | progFragment | keywords: _DBUFFER_MRT3 _DE_WEATHER_RAIN | blob 33
void frag(
  float4 v0 : SV_POSITION0,
  float4 v1 : VDREG1,
  float4 v2 : VDREG2,
  float4 v3 : VDREG3,
  float4 v4 : VDREG4,
  out float4 o0 : SV_Target0,
  out float4 o1 : SV_Target1,
  out float4 o2 : SV_Target2)
{
  uint4 r0, r1, r2, r3, r4, r5;
  r0 = 0u; r1 = 0u; r2 = 0u; r3 = 0u; r4 = 0u; r5 = 0u;

#if UNITY_UV_STARTS_AT_TOP
  r0.x = vd_asuint((-(int)(vd_asfloat(_ProjectionParams.x) < 0.0)));
#else
  r0.x = vd_asuint((-(int)(0.0 < vd_asfloat(_ProjectionParams.x))));
#endif
  r0.y = vd_asuint(((-(vd_asfloat(v0.y))) + vd_asfloat(_ScreenParams.y)));
  r0.y = ((r0.x != 0u) ? r0.y : vd_asuint(v0.y));
  r0.x = vd_asuint(v0.x);
  r0.xy = vd_asuint((vd_asfloat(r0.xy) / vd_asfloat(_ScreenParams.xy)));
  r1.xyz = vd_asuint((vd_asfloat(_EdgeColor.xyz) + vd_asfloat(_EdgeColor.xyz)));
  r0.z = vd_asuint(((-(vd_asfloat(r0.y))) + 1.0));
  r0.xyz = vd_asuint(_PlaneReflectionTexture.SampleBias(sampler_PlaneReflectionTexture, vd_asfloat(r0.xz), vd_asfloat(_GlobalMipBias.x)).xyz);
  r0.xyz = vd_asuint((vd_asfloat(r0.xyz) + (-(vd_asfloat(_PlaneReflectionBackgroundColor.xyz)))));
  r0.xyz = vd_asuint((vd_asfloat((_PlaneReflectionOpacity).xxx) * vd_asfloat(r0.xyz) + vd_asfloat(_PlaneReflectionBackgroundColor.xyz)));
  r0.w = vd_asuint((vd_asfloat(_TimeParameters.x) * 0.100000001));
  r2.xy = vd_asuint((vd_asfloat(_TimeParameters.xx) * float2(0.100000001, 0.100000001) + vd_asfloat(v3.xy)));
  r2.zw = vd_asuint(floor(vd_asfloat(r2.xy)));
  r2.xy = vd_asuint(frac(vd_asfloat(r2.xy)));
  r3.xy = vd_asuint(((int2)vd_asfloat(r2.zw)));
  r1.w = (r3.y ^ 0x41c64e6du);
  r3.x = vd_asuint((vd_asint(r1.w) + vd_asint(r3.x)));
  r1.w = vd_asuint((vd_asint(r1.w) * vd_asint(r3.x)));
  r3.x = (r1.w >> (uint)5);
  r1.w = (r1.w ^ r3.x);
  r1.w = vd_asuint((vd_asint(r1.w) * 668265261));
  r1.w = (r1.w >> (uint)8);
  r1.w = vd_asuint(((float)r1.w));
  r3.yz = vd_asuint((vd_asfloat(r1.ww) * float2(5.96046519e-08, 5.96046519e-08) + float2(0.5, -0.5)));
  r3.y = vd_asuint(floor(vd_asfloat(r3.y)));
  r3.x = vd_asuint((vd_asfloat(r1.w) * 5.96046519e-08 + (-(vd_asfloat(r3.y)))));
  r1.w = vd_asuint(dot(vd_asfloat(r3.xz), vd_asfloat(r3.xz)));
  r1.w = vd_asuint(rsqrt(vd_asfloat(r1.w)));
  r3.xy = vd_asuint((vd_asfloat(r1.ww) * vd_asfloat(r3.xz)));
  r1.w = vd_asuint(dot(vd_asfloat(r3.xy), vd_asfloat(r2.xy)));
  r3.xyzw = vd_asuint((vd_asfloat(r2.zwzw) + float4(0.0, 1.0, 1.0, 0.0)));
  r3.xyzw = vd_asuint(((int4)vd_asfloat(r3.xyzw)));
  r3.yw = (r3.yw ^ uint2(0x41c64e6du, 0x41c64e6du));
  r3.xz = vd_asuint((vd_asint(r3.yw) + vd_asint(r3.xz)));
  r3.xy = vd_asuint((vd_asint(r3.yw) * vd_asint(r3.xz)));
  r3.zw = (r3.xy >> (uint2)5);
  r3.xy = (r3.zw ^ r3.xy);
  r3.xy = vd_asuint((vd_asint(r3.xy) * int2(668265261, 668265261)));
  r3.xy = (r3.xy >> (uint2)8);
  r3.xy = vd_asuint(((float2)r3.xy));
  r4.xyzw = vd_asuint((vd_asfloat(r3.xyxy) * float4(5.96046519e-08, 5.96046519e-08, 5.96046519e-08, 5.96046519e-08) + float4(0.5, 0.5, -0.5, -0.5)));
  r3.zw = vd_asuint(floor(vd_asfloat(r4.xy)));
  r4.xy = vd_asuint((vd_asfloat(r3.xy) * float2(5.96046519e-08, 5.96046519e-08) + (-(vd_asfloat(r3.zw)))));
  r3.x = vd_asuint(dot(vd_asfloat(r4.xz), vd_asfloat(r4.xz)));
  r3.x = vd_asuint(rsqrt(vd_asfloat(r3.x)));
  r3.xy = vd_asuint((vd_asfloat(r3.xx) * vd_asfloat(r4.xz)));
  r5.xyzw = vd_asuint((vd_asfloat(r2.xyxy) + float4(-0.0, -1.0, -1.0, -0.0)));
  r3.x = vd_asuint(dot(vd_asfloat(r3.xy), vd_asfloat(r5.xy)));
  r3.y = vd_asuint(dot(vd_asfloat(r4.yw), vd_asfloat(r4.yw)));
  r3.y = vd_asuint(rsqrt(vd_asfloat(r3.y)));
  r3.yz = vd_asuint((vd_asfloat(r3.yy) * vd_asfloat(r4.yw)));
  r3.y = vd_asuint(dot(vd_asfloat(r3.yz), vd_asfloat(r5.zw)));
  r2.zw = vd_asuint((vd_asfloat(r2.zw) + float2(1.0, 1.0)));
  r2.zw = vd_asuint(((int2)vd_asfloat(r2.zw)));
  r2.w = (r2.w ^ 0x41c64e6du);
  r2.z = vd_asuint((vd_asint(r2.w) + vd_asint(r2.z)));
  r2.z = vd_asuint((vd_asint(r2.w) * vd_asint(r2.z)));
  r2.w = (r2.z >> (uint)5);
  r2.z = (r2.w ^ r2.z);
  r2.z = vd_asuint((vd_asint(r2.z) * 668265261));
  r2.z = (r2.z >> (uint)8);
  r2.z = vd_asuint(((float)r2.z));
  r4.yz = vd_asuint((vd_asfloat(r2.zz) * float2(5.96046519e-08, 5.96046519e-08) + float2(0.5, -0.5)));
  r2.w = vd_asuint(floor(vd_asfloat(r4.y)));
  r4.x = vd_asuint((vd_asfloat(r2.z) * 5.96046519e-08 + (-(vd_asfloat(r2.w)))));
  r2.z = vd_asuint(dot(vd_asfloat(r4.xz), vd_asfloat(r4.xz)));
  r2.z = vd_asuint(rsqrt(vd_asfloat(r2.z)));
  r2.zw = vd_asuint((vd_asfloat(r2.zz) * vd_asfloat(r4.xz)));
  r3.zw = vd_asuint((vd_asfloat(r2.xy) + float2(-1.0, -1.0)));
  r2.z = vd_asuint(dot(vd_asfloat(r2.zw), vd_asfloat(r3.zw)));
  r3.zw = vd_asuint((vd_asfloat(r2.xy) * vd_asfloat(r2.xy)));
  r3.zw = vd_asuint((vd_asfloat(r2.xy) * vd_asfloat(r3.zw)));
  r4.xy = vd_asuint((vd_asfloat(r2.xy) * float2(6.0, 6.0) + float2(-15.0, -15.0)));
  r2.xy = vd_asuint((vd_asfloat(r2.xy) * vd_asfloat(r4.xy) + float2(10.0, 10.0)));
  r2.xy = vd_asuint((vd_asfloat(r2.xy) * vd_asfloat(r3.zw)));
  r2.w = vd_asuint(((-(vd_asfloat(r1.w))) + vd_asfloat(r3.x)));
  r1.w = vd_asuint((vd_asfloat(r2.y) * vd_asfloat(r2.w) + vd_asfloat(r1.w)));
  r2.z = vd_asuint(((-(vd_asfloat(r3.y))) + vd_asfloat(r2.z)));
  r2.y = vd_asuint((vd_asfloat(r2.y) * vd_asfloat(r2.z) + vd_asfloat(r3.y)));
  r2.y = vd_asuint(((-(vd_asfloat(r1.w))) + vd_asfloat(r2.y)));
  r1.w = vd_asuint((vd_asfloat(r2.x) * vd_asfloat(r2.y) + vd_asfloat(r1.w)));
  r1.w = vd_asuint(saturate((vd_asfloat(r1.w) + 0.5)));
  r1.w = vd_asuint((vd_asfloat(r1.w) * 0.5));
  r2.x = vd_asuint(_MainTex.SampleBias(sampler_MainTex, vd_asfloat(v2.xy), vd_asfloat(_GlobalMipBias.x)).x);
  r2.y = vd_asuint(((-(vd_asfloat(_Intensity))) + 1.0));
  r2.x = vd_asuint((vd_asfloat(r2.x) + (-(vd_asfloat(_Intensity)))));
  r2.y = vd_asuint((1.0 / vd_asfloat(r2.y)));
  r2.x = vd_asuint(saturate((vd_asfloat(r2.y) * vd_asfloat(r2.x))));
  r2.y = vd_asuint((vd_asfloat(r2.x) * -2.0 + 3.0));
  r2.x = vd_asuint((vd_asfloat(r2.x) * vd_asfloat(r2.x)));
  r2.z = vd_asuint((vd_asfloat(r2.x) * vd_asfloat(r2.y)));
  r2.x = vd_asuint((vd_asfloat(r2.y) * vd_asfloat(r2.x) + -0.5));
  r1.w = vd_asuint((1.0 / vd_asfloat(r1.w)));
  r1.w = vd_asuint(saturate((vd_asfloat(r1.w) * abs(vd_asfloat(r2.x)))));
  r2.x = vd_asuint((vd_asfloat(r1.w) * -2.0 + 3.0));
  r1.w = vd_asuint((vd_asfloat(r1.w) * vd_asfloat(r1.w)));
  r1.w = vd_asuint((vd_asfloat(r1.w) * vd_asfloat(r2.x)));
  r0.xyz = vd_asuint(((-(vd_asfloat(_EdgeColor.xyz))) * float3(2.0, 2.0, 2.0) + vd_asfloat(r0.xyz)));
  r0.xyz = vd_asuint((vd_asfloat(r1.www) * vd_asfloat(r0.xyz) + vd_asfloat(r1.xyz)));
  o0.xyz = (vd_asfloat(r0.xyz) + vd_asfloat(_TintColor.xyz));
  r0.x = vd_asuint((vd_asfloat(r2.z) * vd_asfloat(_Opacity)));
  r0.yz = vd_asuint((vd_asfloat(v2.xy) * float2(5.0, 5.0)));
  r1.xy = vd_asuint(floor(vd_asfloat(r0.yz)));
  r0.yz = vd_asuint(frac(vd_asfloat(r0.yz)));
  r1.zw = uint2(0x00000000u, 0x41000000u);
  r2.x = 0xffffffffu;
  while (true) {
    r2.y = vd_asuint((-(int)(1 < vd_asint(r2.x))));
    if (r2.y != 0u) break;
    r3.y = vd_asuint(((float)vd_asint(r2.x)));
    r2.yz = r1.zw;
    r2.w = 0xffffffffu;
    while (true) {
      r3.z = vd_asuint((-(int)(1 < vd_asint(r2.w))));
      if (r3.z != 0u) break;
      r3.x = vd_asuint(((float)vd_asint(r2.w)));
      r3.zw = vd_asuint((vd_asfloat(r1.xy) + vd_asfloat(r3.xy)));
      r3.zw = vd_asuint(((int2)vd_asfloat(r3.zw)));
      r3.w = (r3.w ^ 0x41c64e6du);
      r3.z = vd_asuint((vd_asint(r3.w) + vd_asint(r3.z)));
      r3.z = vd_asuint((vd_asint(r3.w) * vd_asint(r3.z)));
      r4.x = (r3.z >> (uint)5);
      r3.z = (r3.z ^ r4.x);
      r4.y = vd_asuint((vd_asint(r3.z) * 668265261));
      r3.z = (r4.y << (uint)3);
      r4.x = (r3.z ^ r3.w);
      r3.zw = (r4.xy >> (uint2)8);
      r3.zw = vd_asuint(((float2)r3.zw));
      r3.zw = vd_asuint((vd_asfloat(r0.ww) * vd_asfloat(r3.zw)));
      r3.zw = vd_asuint((vd_asfloat(r3.zw) * float2(5.96046519e-08, 5.96046519e-08)));
      r4.x = vd_asuint(sin(vd_asfloat(r3.z)));
      r4.y = vd_asuint(cos(vd_asfloat(r3.w)));
      r3.zw = vd_asuint((vd_asfloat(r4.xy) * float2(0.5, 0.5) + float2(0.5, 0.5)));
      r3.xz = vd_asuint((vd_asfloat(r3.zw) + vd_asfloat(r3.xy)));
      r3.xz = vd_asuint(((-(vd_asfloat(r0.yz))) + vd_asfloat(r3.xz)));
      r3.x = vd_asuint(dot(vd_asfloat(r3.xz), vd_asfloat(r3.xz)));
      r3.x = vd_asuint(sqrt(vd_asfloat(r3.x)));
      r3.z = vd_asuint((-(int)(vd_asfloat(r3.x) < vd_asfloat(r2.z))));
      r2.yz = ((r3.zz != 0u) ? r3.xx : r2.yz);
      r2.w = vd_asuint((vd_asint(r2.w) + 1));
    }
    r1.zw = r2.yz;
    r2.x = vd_asuint((vd_asint(r2.x) + 1));
  }
  r0.y = vd_asuint((vd_asfloat(r1.z) * 0.25 + 0.75));
  r0.x = vd_asuint((vd_asfloat(r0.y) * vd_asfloat(r0.x)));
  o0.w = (vd_asfloat(r0.x) * vd_asfloat(_PlaneReflectionActive));
  r0.x = vd_asuint(dot(vd_asfloat(v4.xyz), vd_asfloat(v4.xyz)));
  r0.x = vd_asuint(rsqrt(vd_asfloat(r0.x)));
  r0.xyz = vd_asuint((vd_asfloat(r0.xxx) * vd_asfloat(v4.xyz)));
  o1.xyz = (vd_asfloat(r0.xyz) * float3(0.5, 0.5, 0.5) + float3(0.5, 0.5, 0.5));
  o1.w = vd_asfloat(0x00000000u);
  o2.xyzw = vd_asfloat(uint4(0x00000000u, 0x3f800000u, 0x00000000u, 0x00000000u));
  return;
}
