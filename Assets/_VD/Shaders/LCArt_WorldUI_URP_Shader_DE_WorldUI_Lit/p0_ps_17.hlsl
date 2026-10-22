// LCArt/WorldUI/URP/Shader_DE_WorldUI_Lit | pass 0 "Forward" | progFragment | keywords: UNITY_UI_ALPHACLIP _CLUSTER_LIGHT_LOOP | blob 17
void frag(
  float4 v0 : SV_POSITION0,
  float4 v1 : COLOR0,
  float2 v2 : TEXCOORD0,
  float w2 : TEXCOORD3,
  float4 v3 : TEXCOORD1,
  float4 v4 : TEXCOORD2,
  float3 v5 : TEXCOORD7,
  out float4 o0 : SV_Target0)
{
  float4 r0,r1,r2,r3,r4,r5,r6,r7,r8,r9;
  uint4 bitmask, uiDest;
  float4 fDest;

  r0.xyzw = _MainTex.SampleBias(sampler_MainTex, v2.xy, _GlobalMipBias.x).xyzw;
  r1.x = -0.00100000005 + r0.w;
  r1.x = cmp(r1.x < 0);
  if (r1.x != 0) discard;
  r1.x = dot(v4.xyz, v4.xyz);
  r1.x = max(6.10351563e-005, r1.x);
  r1.x = rsqrt(r1.x);
  r1.xyz = v4.xyz * r1.xxx;
  r2.xy = float2(1,1) / _ScaledScreenParams.xy;
  r2.xy = v0.xy * r2.xy;
  r1.w = r2.y * _ScaleBiasRt.x + _ScaleBiasRt.y;
  r2.z = 1 + -r1.w;
  r1.w = dot(r1.xyz, _MainLightPosition.xyz);
  r1.w = saturate(r1.w * 0.5 + 0.5);
  r2.xy = _FPParams1.xy * r2.xz;
  r2.xy = (uint2)r2.xy;
  r2.zw = (uint2)_FPParams1.zw;
  r2.x = mad((int)r2.y, (int)r2.z, (int)r2.x);
  r2.x = (int)r2.w * (int)r2.x;
  r3.x = -(VD_COL(unity_MatrixV, 0).z);
  r3.y = -(VD_COL(unity_MatrixV, 1).z);
  r3.z = -(VD_COL(unity_MatrixV, 2).z);
  r4.xyz = -_WorldSpaceCameraPos.xyz + v3.xyz;
  r2.y = dot(r3.xyz, r4.xyz);
  r2.z = cmp(unity_OrthoParams.w == 0.000000);
  r3.x = log2(r2.y);
  r2.y = r2.z ? r3.x : r2.y;
  r2.y = r2.y * _FPParams0.x + _FPParams0.y;
  r2.y = (uint)r2.y;
  r2.z = (uint)_FPParams2.x;
  r2.z = (int)r2.z + -1;
  r2.w = (int)r2.w + 2;
  r2.y = min((uint)r2.y, (uint)r2.z);
  r2.z = (int)r2.w * (int)r2.y;
  r2.y = mad((int)r2.y, (int)r2.w, 2);
  r2.w = (uint)r2.z >> 2;
  r3.x = (uint)r2.z << 31;
  r3.y = (uint)r2.z << 30;
  r3.xy = (uint2)r3.xy >> 31;
  r3.zw = (int2)r3.xx & vd_asint(urp_ZBins[(int)(r2.w)].wy);
  r4.xy = ~(int2)r3.xy;
  r2.zw = (int2)r4.xx & vd_asint(urp_ZBins[(int)(r2.w)].zx);
  r2.zw = (int2)r2.zw | (int2)r3.zw;
  r2.z = (int)r3.y & (int)r2.z;
  r2.w = (int)r4.y & (int)r2.w;
  r2.z = (int)r2.w | (int)r2.z;
  r2.w = (uint)_FPParams0.w;
  r3.z = 0;
  r3.xyw = r2.xyz;
  r4.xyz = float3(0,0,0);
  while (true) {
    r4.w = (uint)r3.w >> 16;
    r5.xy = r3.zw;
    while (true) {
      r5.z = cmp((int)r5.x == 0);
      r5.w = (int)r5.y & 0x0000ffff;
      r5.w = cmp((uint)r4.w >= (uint)r5.w);
      r5.z = r5.w ? r5.z : 0;
      if (r5.z == 0) break;
      r5.z = (uint)r5.y >> 5;
      r5.z = (int)r5.z & 2047;
      r6.xyzw = (int4)r3.xxyy + (int4)r5.zzzz;
      r7.xy = (uint2)r6.yw >> 2;
      r8.xz = (uint2)r6.xz << 31;
      r8.yw = (uint2)r6.yw << 30;
      r6.xyzw = (uint4)r8.xyzw >> 31;
      r7.zw = (int2)r6.xx & vd_asint(urp_Tiles[(int)(r7.x)].wy);
      r8.xyzw = ~(int4)r6.xyzw;
      r9.xy = (int2)r8.xx & vd_asint(urp_Tiles[(int)(r7.x)].zx);
      r7.xz = (int2)r7.zw | (int2)r9.xy;
      r5.w = (int)r6.y & (int)r7.x;
      r6.x = (int)r8.y & (int)r7.z;
      r5.w = (int)r5.w | (int)r6.x;
      r6.xy = (int2)r6.zz & vd_asint(urp_ZBins[(int)(r7.y)].wy);
      r7.xy = (int2)r8.zz & vd_asint(urp_ZBins[(int)(r7.y)].zx);
      r6.xy = (int2)r6.xy | (int2)r7.xy;
      r6.x = (int)r6.w & (int)r6.x;
      r6.y = (int)r8.w & (int)r6.y;
      r6.x = (int)r6.y | (int)r6.x;
      r5.w = r5.w ? r6.x : 0;
      r6.x = -1 << (int)r5.y;
      r5.w = r5.w ? r6.x : 0;
      r5.z = (uint)r5.z << 5;
      r5.z = (int)r4.w + (int)-r5.z;
      r5.z = min(31, (uint)r5.z);
      r5.z = (int)-r5.z + 31;
      r5.z = -1 >> (uint)r5.z;
      r5.x = r5.w ? r5.z : 0;
      r5.z = (int)r5.y + 32;
      r5.y = (int)r5.z & -32;
    }
    r3.w = r5.y;
    if (r5.x == 0) break;
    r4.w = -(int)r5.x;
    r4.w = (int)r4.w & (int)r5.x;
    r4.w = (uint)r4.w;
    r4.w = (uint)r4.w >> 23;
    r4.w = (int)r4.w + -127;
    r5.z = 1 << (int)r4.w;
    r3.z = (int)r5.z ^ (int)r5.x;
    r5.z = (int)r3.w + -32;
    r5.z = (int)r5.z & 0x0000ffe0;
    r4.w = (int)r4.w + (int)r5.z;
    r4.w = (int)r2.w + (int)r4.w;
    r6.xyz = -v3.xyz * _AdditionalLightsPosition[(int)(r4.w)].www + _AdditionalLightsPosition[(int)(r4.w)].xyz;
    r5.z = dot(r6.xyz, r6.xyz);
    r5.z = max(6.10351563e-005, r5.z);
    r5.w = rsqrt(r5.z);
    r6.xyz = r6.xyz * r5.www;
    r5.w = 1 / r5.z;
    r5.z = _AdditionalLightsAttenuation[(int)(r4.w)].x * r5.z;
    r5.z = -r5.z * r5.z + 1;
    r5.z = max(0, r5.z);
    r5.z = r5.z * r5.z;
    r5.z = r5.w * r5.z;
    r5.w = dot(_AdditionalLightsSpotDir[(int)(r4.w)].xyz, r6.xyz);
    r5.w = saturate(r5.w * _AdditionalLightsAttenuation[(int)(r4.w)].z + _AdditionalLightsAttenuation[(int)(r4.w)].w);
    r5.w = r5.w * r5.w;
    r5.z = r5.z * r5.w;
    r5.z = sqrt(r5.z);
    r5.z = min(1, r5.z);
    r7.xyz = _AdditionalLightsColor[(int)(r4.w)].xyz * r5.zzz;
    r4.w = dot(r1.xyz, r6.xyz);
    r4.w = saturate(r4.w * 0.5 + 0.5);
    r4.xyz = r7.xyz * r4.www + r4.xyz;
  }
  r1.xyz = _MainLightColor.xyz * r1.www + r4.xyz;
  o0.xyz = r1.xyz * r0.xyz;
  o0.w = v1.w * r0.w;
  return;
}
