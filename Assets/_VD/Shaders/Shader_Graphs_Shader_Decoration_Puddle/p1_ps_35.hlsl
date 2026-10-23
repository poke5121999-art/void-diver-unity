// Shader Graphs/Shader_Decoration_Puddle | pass 1 "DBufferMesh" | progFragment | keywords: _DBUFFER_MRT3 _DE_EDITOR_CAMERA | blob 35
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
  uint4 r0, r1, r2, r3, r4;
  r0 = 0u; r1 = 0u; r2 = 0u; r3 = 0u; r4 = 0u;

  r0.xyz = vd_asuint((vd_asfloat(_EdgeColor.xyz) + vd_asfloat(_EdgeColor.xyz)));
  r0.w = vd_asuint((vd_asfloat(_TimeParameters.x) * 0.100000001));
  r1.xy = vd_asuint((vd_asfloat(_TimeParameters.xx) * float2(0.100000001, 0.100000001) + vd_asfloat(v3.xy)));
  r1.zw = vd_asuint(floor(vd_asfloat(r1.xy)));
  r1.xy = vd_asuint(frac(vd_asfloat(r1.xy)));
  r2.xy = vd_asuint(((int2)vd_asfloat(r1.zw)));
  r2.y = (r2.y ^ 0x41c64e6du);
  r2.x = vd_asuint((vd_asint(r2.y) + vd_asint(r2.x)));
  r2.x = vd_asuint((vd_asint(r2.y) * vd_asint(r2.x)));
  r2.y = (r2.x >> (uint)5);
  r2.x = (r2.y ^ r2.x);
  r2.x = vd_asuint((vd_asint(r2.x) * 668265261));
  r2.x = (r2.x >> (uint)8);
  r2.x = vd_asuint(((float)r2.x));
  r3.yz = vd_asuint((vd_asfloat(r2.xx) * float2(5.96046519e-08, 5.96046519e-08) + float2(0.5, -0.5)));
  r2.y = vd_asuint(floor(vd_asfloat(r3.y)));
  r3.x = vd_asuint((vd_asfloat(r2.x) * 5.96046519e-08 + (-(vd_asfloat(r2.y)))));
  r2.x = vd_asuint(dot(vd_asfloat(r3.xz), vd_asfloat(r3.xz)));
  r2.x = vd_asuint(rsqrt(vd_asfloat(r2.x)));
  r2.xy = vd_asuint((vd_asfloat(r2.xx) * vd_asfloat(r3.xz)));
  r2.x = vd_asuint(dot(vd_asfloat(r2.xy), vd_asfloat(r1.xy)));
  r3.xyzw = vd_asuint((vd_asfloat(r1.zwzw) + float4(0.0, 1.0, 1.0, 0.0)));
  r3.xyzw = vd_asuint(((int4)vd_asfloat(r3.xyzw)));
  r2.yz = (r3.yw ^ uint2(0x41c64e6du, 0x41c64e6du));
  r3.xy = vd_asuint((vd_asint(r2.yz) + vd_asint(r3.xz)));
  r2.yz = vd_asuint((vd_asint(r2.yz) * vd_asint(r3.xy)));
  r3.xy = (r2.yz >> (uint2)5);
  r2.yz = (r2.yz ^ r3.xy);
  r2.yz = vd_asuint((vd_asint(r2.yz) * int2(668265261, 668265261)));
  r2.yz = (r2.yz >> (uint2)8);
  r2.yz = vd_asuint(((float2)r2.yz));
  r3.xyzw = vd_asuint((vd_asfloat(r2.yzyz) * float4(5.96046519e-08, 5.96046519e-08, 5.96046519e-08, 5.96046519e-08) + float4(0.5, 0.5, -0.5, -0.5)));
  r4.xy = vd_asuint(floor(vd_asfloat(r3.xy)));
  r3.xy = vd_asuint((vd_asfloat(r2.yz) * float2(5.96046519e-08, 5.96046519e-08) + (-(vd_asfloat(r4.xy)))));
  r2.y = vd_asuint(dot(vd_asfloat(r3.xz), vd_asfloat(r3.xz)));
  r2.y = vd_asuint(rsqrt(vd_asfloat(r2.y)));
  r2.yz = vd_asuint((vd_asfloat(r2.yy) * vd_asfloat(r3.xz)));
  r4.xyzw = vd_asuint((vd_asfloat(r1.xyxy) + float4(-0.0, -1.0, -1.0, -0.0)));
  r2.y = vd_asuint(dot(vd_asfloat(r2.yz), vd_asfloat(r4.xy)));
  r2.z = vd_asuint(dot(vd_asfloat(r3.yw), vd_asfloat(r3.yw)));
  r2.z = vd_asuint(rsqrt(vd_asfloat(r2.z)));
  r2.zw = vd_asuint((vd_asfloat(r2.zz) * vd_asfloat(r3.yw)));
  r2.z = vd_asuint(dot(vd_asfloat(r2.zw), vd_asfloat(r4.zw)));
  r1.zw = vd_asuint((vd_asfloat(r1.zw) + float2(1.0, 1.0)));
  r1.zw = vd_asuint(((int2)vd_asfloat(r1.zw)));
  r1.w = (r1.w ^ 0x41c64e6du);
  r1.z = vd_asuint((vd_asint(r1.w) + vd_asint(r1.z)));
  r1.z = vd_asuint((vd_asint(r1.w) * vd_asint(r1.z)));
  r1.w = (r1.z >> (uint)5);
  r1.z = (r1.w ^ r1.z);
  r1.z = vd_asuint((vd_asint(r1.z) * 668265261));
  r1.z = (r1.z >> (uint)8);
  r1.z = vd_asuint(((float)r1.z));
  r3.yz = vd_asuint((vd_asfloat(r1.zz) * float2(5.96046519e-08, 5.96046519e-08) + float2(0.5, -0.5)));
  r1.w = vd_asuint(floor(vd_asfloat(r3.y)));
  r3.x = vd_asuint((vd_asfloat(r1.z) * 5.96046519e-08 + (-(vd_asfloat(r1.w)))));
  r1.z = vd_asuint(dot(vd_asfloat(r3.xz), vd_asfloat(r3.xz)));
  r1.z = vd_asuint(rsqrt(vd_asfloat(r1.z)));
  r1.zw = vd_asuint((vd_asfloat(r1.zz) * vd_asfloat(r3.xz)));
  r3.xy = vd_asuint((vd_asfloat(r1.xy) + float2(-1.0, -1.0)));
  r1.z = vd_asuint(dot(vd_asfloat(r1.zw), vd_asfloat(r3.xy)));
  r3.xy = vd_asuint((vd_asfloat(r1.xy) * vd_asfloat(r1.xy)));
  r3.xy = vd_asuint((vd_asfloat(r1.xy) * vd_asfloat(r3.xy)));
  r3.zw = vd_asuint((vd_asfloat(r1.xy) * float2(6.0, 6.0) + float2(-15.0, -15.0)));
  r1.xy = vd_asuint((vd_asfloat(r1.xy) * vd_asfloat(r3.zw) + float2(10.0, 10.0)));
  r1.xy = vd_asuint((vd_asfloat(r1.xy) * vd_asfloat(r3.xy)));
  r1.w = vd_asuint(((-(vd_asfloat(r2.x))) + vd_asfloat(r2.y)));
  r1.w = vd_asuint((vd_asfloat(r1.y) * vd_asfloat(r1.w) + vd_asfloat(r2.x)));
  r1.z = vd_asuint(((-(vd_asfloat(r2.z))) + vd_asfloat(r1.z)));
  r1.y = vd_asuint((vd_asfloat(r1.y) * vd_asfloat(r1.z) + vd_asfloat(r2.z)));
  r1.y = vd_asuint(((-(vd_asfloat(r1.w))) + vd_asfloat(r1.y)));
  r1.x = vd_asuint((vd_asfloat(r1.x) * vd_asfloat(r1.y) + vd_asfloat(r1.w)));
  r1.x = vd_asuint(saturate((vd_asfloat(r1.x) + 0.5)));
  r1.x = vd_asuint((vd_asfloat(r1.x) * 0.5));
  r1.y = vd_asuint(_MainTex.SampleBias(sampler_MainTex, vd_asfloat(v2.xy), vd_asfloat(_GlobalMipBias.x)).x);
  r1.z = vd_asuint(((-(vd_asfloat(_Intensity))) + 1.0));
  r1.y = vd_asuint((vd_asfloat(r1.y) + (-(vd_asfloat(_Intensity)))));
  r1.z = vd_asuint((1.0 / vd_asfloat(r1.z)));
  r1.y = vd_asuint(saturate((vd_asfloat(r1.z) * vd_asfloat(r1.y))));
  r1.z = vd_asuint((vd_asfloat(r1.y) * -2.0 + 3.0));
  r1.y = vd_asuint((vd_asfloat(r1.y) * vd_asfloat(r1.y)));
  r1.w = vd_asuint((vd_asfloat(r1.y) * vd_asfloat(r1.z)));
  r1.y = vd_asuint((vd_asfloat(r1.z) * vd_asfloat(r1.y) + -0.5));
  r1.x = vd_asuint((1.0 / vd_asfloat(r1.x)));
  r1.x = vd_asuint(saturate((vd_asfloat(r1.x) * abs(vd_asfloat(r1.y)))));
  r1.y = vd_asuint((vd_asfloat(r1.x) * -2.0 + 3.0));
  r1.x = vd_asuint((vd_asfloat(r1.x) * vd_asfloat(r1.x)));
  r1.x = vd_asuint((vd_asfloat(r1.x) * vd_asfloat(r1.y)));
  r2.xyz = vd_asuint(((-(vd_asfloat(_EdgeColor.xyz))) * float3(2.0, 2.0, 2.0) + vd_asfloat(_PlaneReflectionBackgroundColor.xyz)));
  r0.xyz = vd_asuint((vd_asfloat(r1.xxx) * vd_asfloat(r2.xyz) + vd_asfloat(r0.xyz)));
  o0.xyz = (vd_asfloat(r0.xyz) + vd_asfloat(_TintColor.xyz));
  r0.x = vd_asuint((vd_asfloat(r1.w) * vd_asfloat(_Opacity)));
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
