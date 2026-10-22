// LCArt/TMPFont/Shader_DE_TMP_SDF_Lit | pass 0 "TextMeshPro SDF" | progFragment | keywords: UNDERLAY_ON UNITY_UI_ALPHACLIP UNITY_UI_CLIP_RECT _CLUSTER_LIGHT_LOOP | blob 39
void frag(
  float4 v0 : SV_POSITION0,
  float4 v1 : COLOR0,
  float2 v2 : TEXCOORD0,
  float w2 : TEXCOORD8,
  float4 v3 : TEXCOORD1,
  float4 v4 : TEXCOORD2,
  float4 v5 : TEXCOORD3,
  float4 v6 : TEXCOORD4,
  float4 v7 : COLOR1,
  float4 v8 : TEXCOORD5,
  float4 v9 : TEXCOORD6,
  float3 v10 : TEXCOORD7,
  out float4 o0 : SV_Target0)
{
  float4 r0,r1,r2,r3,r4,r5,r6,r7,r8,r9;
  uint4 bitmask, uiDest;
  float4 fDest;

  r0.xyzw = _MainTex.SampleBias(sampler_MainTex, v2.xy, _GlobalMipBias.x).xyzw;
  r0.x = v3.z + -r0.w;
  r0.y = _ScaleRatioA * _OutlineWidth;
  r0.z = _ScaleRatioA * _OutlineSoftness;
  r0.yw = v3.yy * r0.yz;
  r1.xyz = _FaceColor.xyz * v1.xyz;
  r2.xy = float2(_FaceUVSpeedX, _FaceUVSpeedY) * _Time.yy + v8.xy;
  r2.xyzw = _FaceTex.SampleBias(sampler_FaceTex, r2.xy, _GlobalMipBias.x).xyzw;
  r1.xyz = r2.xyz * r1.xyz;
  r2.w = _FaceColor.w * r2.w;
  r3.xy = float2(_OutlineUVSpeedX, _OutlineUVSpeedY) * _Time.yy + v8.zw;
  r3.xyzw = _OutlineTex.SampleBias(sampler_OutlineTex, r3.xy, _GlobalMipBias.x).xyzw;
  r3.xyzw = _OutlineColor.xyzw * r3.xyzw;
  r1.w = 0.5 * r0.y;
  r4.x = r0.x * v3.y + -r1.w;
  r0.w = r0.w * 0.5 + r4.x;
  r0.z = r0.z * v3.y + 1;
  r0.z = saturate(r0.w / r0.z);
  r0.z = 1 + -r0.z;
  r0.x = saturate(r0.x * v3.y + r1.w);
  r0.y = min(1, r0.y);
  r0.y = sqrt(r0.y);
  r0.x = r0.x * r0.y;
  r2.xyz = r2.www * r1.xyz;
  r3.xyz = r3.xyz * r3.www;
  r1.xyzw = r3.xyzw + -r2.xyzw;
  r1.xyzw = r0.xxxx * r1.xyzw + r2.xyzw;
  r2.xyzw = r1.xyzw * r0.zzzz;
  r3.xyzw = _MainTex.SampleBias(sampler_MainTex, v6.xy, _GlobalMipBias.x).xyzw;
  r0.x = saturate(r3.w * v6.z + -v6.w);
  r3.xyzw = v7.xyzw * r0.xxxx;
  r0.x = -r1.w * r0.z + 1;
  r0.xyzw = r3.xyzw * r0.xxxx + r2.xyzw;
  r1.xy = _ClipRect.zw + -_ClipRect.xy;
  r1.xy = -abs(v4.xy) + r1.xy;
  r1.xy = saturate(v4.zw * r1.xy);
  r1.x = r1.x * r1.y;
  r2.xyzw = r1.xxxx * r0.xyzw;
  r0.x = r0.w * r1.x + -0.00100000005;
  r0.x = cmp(r0.x < 0);
  if (r0.x != 0) discard;
  r0.x = dot(v9.xyz, v9.xyz);
  r0.x = max(1.17549435e-038, r0.x);
  r0.x = rsqrt(r0.x);
  r0.xyz = v9.xyz * r0.xxx;
  r1.xy = float2(1,1) / _ScaledScreenParams.xy;
  r1.xy = v0.xy * r1.xy;
  r0.w = r1.y * _ScaleBiasRt.x + _ScaleBiasRt.y;
  r1.z = 1 + -r0.w;
  r0.w = dot(r0.xyz, _MainLightPosition.xyz);
  r0.w = saturate(r0.w * 0.5 + 0.5);
  r1.xy = _FPParams1.xy * r1.xz;
  r1.xy = (uint2)r1.xy;
  r1.zw = (uint2)_FPParams1.zw;
  r1.x = mad((int)r1.y, (int)r1.z, (int)r1.x);
  r1.x = (int)r1.w * (int)r1.x;
  r3.x = -(VD_COL(unity_MatrixV, 0).z);
  r3.y = -(VD_COL(unity_MatrixV, 1).z);
  r3.z = -(VD_COL(unity_MatrixV, 2).z);
  r4.xyz = -_WorldSpaceCameraPos.xyz + v10.xyz;
  r1.y = dot(r3.xyz, r4.xyz);
  r1.z = cmp(unity_OrthoParams.w == 0.000000);
  r3.x = log2(r1.y);
  r1.y = r1.z ? r3.x : r1.y;
  r1.y = r1.y * _FPParams0.x + _FPParams0.y;
  r1.y = (uint)r1.y;
  r1.z = (uint)_FPParams2.x;
  r1.z = (int)r1.z + -1;
  r1.w = (int)r1.w + 2;
  r1.y = min((uint)r1.y, (uint)r1.z);
  r1.z = (int)r1.w * (int)r1.y;
  r1.y = mad((int)r1.y, (int)r1.w, 2);
  r1.w = (uint)r1.z >> 2;
  r3.x = (uint)r1.z << 31;
  r3.y = (uint)r1.z << 30;
  r3.xy = (uint2)r3.xy >> 31;
  r3.zw = (int2)r3.xx & vd_asint(urp_ZBins[(int)(r1.w)].wy);
  r4.xy = ~(int2)r3.xy;
  r1.zw = (int2)r4.xx & vd_asint(urp_ZBins[(int)(r1.w)].zx);
  r1.zw = (int2)r1.zw | (int2)r3.zw;
  r1.z = (int)r3.y & (int)r1.z;
  r1.w = (int)r4.y & (int)r1.w;
  r1.z = (int)r1.w | (int)r1.z;
  r1.w = (uint)_FPParams0.w;
  r3.z = 0;
  r3.xyw = r1.xyz;
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
    r4.w = (int)r1.w + (int)r4.w;
    r6.xyz = -v10.xyz * _AdditionalLightsPosition[(int)(r4.w)].www + _AdditionalLightsPosition[(int)(r4.w)].xyz;
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
    r4.w = dot(r0.xyz, r6.xyz);
    r4.w = saturate(r4.w * 0.5 + 0.5);
    r4.xyz = r7.xyz * r4.www + r4.xyz;
  }
  r0.xyz = _MainLightColor.xyz * r0.www + r4.xyz;
  r2.xyz = r2.xyz * r0.xyz;
  o0.xyzw = v1.wwww * r2.xyzw;
  return;
}
