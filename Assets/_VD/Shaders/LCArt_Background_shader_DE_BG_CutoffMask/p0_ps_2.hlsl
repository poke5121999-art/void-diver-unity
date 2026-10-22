// LCArt/Background/shader_DE_BG_CutoffMask | pass 0 "CustomDepth" | progFragment | keywords: (none) | blob 2
void frag(
  float4 v0 : SV_POSITION0,
  float2 v1 : TEXCOORD1,
  float w1 : TEXCOORD6,
  float4 v2 : TEXCOORD8,
  float4 v3 : TEXCOORD2,
  float4 v4 : TEXCOORD3,
  float4 v5 : TEXCOORD4,
  float3 v6 : TEXCOORD5,
  float4 v7 : COLOR0,
  float4 v8 : TEXCOORD7,
  out float4 o0 : SV_Target0)
{
  float4 r0,r1,r2;
  uint4 bitmask, uiDest;
  float4 fDest;

  r0.xyzw = _BaseMap.SampleBias(sampler_BaseMap, v1.xy, _GlobalMipBias.x).xyzw;
  r0.x = -0.100000001 + r0.w;
  r0.x = cmp(r0.x < 0);
  if (r0.x != 0) discard;
  r0.x = cmp(0.5 < _DissolveProgress);
  if (r0.x != 0) {
    r0.x = saturate(_DissolveProgress * 2 + -1);
    r0.x = 0.999000013 + -r0.x;
    r0.x = cmp(r0.x < 0);
    if (r0.x != 0) discard;
  }
  r0.x = cmp(_GlobalBoxAreaEnable >= 0.5);
  if (r0.x != 0) {
    r0.xy = float2(0.5,0.5) * v6.xz;
    r0.zw = floor(r0.xy);
    r0.xy = frac(r0.xy);
    r1.xy = r0.xy * r0.xy;
    r0.xy = -r0.xy * float2(2,2) + float2(3,3);
    r0.xy = r1.xy * r0.xy;
    r1.x = dot(r0.zw, float2(127.099998,311.700012));
    r1.x = sin(r1.x);
    r1.x = 43758.5469 * r1.x;
    r2.xyzw = float4(1,0,0,1) + r0.zwzw;
    r1.y = dot(r2.xy, float2(127.099998,311.700012));
    r1.y = sin(r1.y);
    r1.y = 43758.5469 * r1.y;
    r1.z = dot(r2.zw, float2(127.099998,311.700012));
    r1.z = sin(r1.z);
    r1.z = 43758.5469 * r1.z;
    r1.xyz = frac(r1.xyz);
    r0.zw = float2(1,1) + r0.zw;
    r0.z = dot(r0.zw, float2(127.099998,311.700012));
    r0.z = sin(r0.z);
    r0.z = 43758.5469 * r0.z;
    r0.z = frac(r0.z);
    r0.w = r1.y + -r1.x;
    r0.w = r0.x * r0.w + r1.x;
    r0.z = r0.z + -r1.z;
    r0.x = r0.x * r0.z + r1.z;
    r0.x = r0.x + -r0.w;
    r0.x = r0.y * r0.x + r0.w;
    r0.y = (int)_GlobalBoxAreaCenters[0].w;
    r0.y = cmp((int)r0.y == 4);
    if (r0.y != 0) {
      r0.yzw = -_GlobalBoxAreaCenters[0].xyz + v6.xyz;
      r0.yzw = _GlobalBoxAreaExtents[0].xyz + -abs(r0.yzw);
      r0.z = min(r0.z, r0.w);
      r0.y = min(r0.y, r0.z);
      r0.z = max(0.00100000005, _GlobalBoxAreaExtents[0].w);
      r0.y = saturate(r0.y / r0.z);
      r0.z = cmp(0 < r0.y);
      if (r0.z != 0) {
        r0.z = 1.125 + -_GlobalBoxAreaIntensities[0];
        r0.z = r0.x + -r0.z;
        r0.z = saturate(-4 * r0.z);
        r0.w = r0.z * -2 + 3;
        r0.z = r0.z * r0.z;
        r0.z = r0.w * r0.z + -0.5;
        r0.y = r0.z + -r0.y;
        r0.y = 1 + r0.y;
        r0.y = cmp(r0.y < 0);
        if (r0.y != 0) discard;
      }
    }
    r0.y = (int)_GlobalBoxAreaCenters[1].w;
    r0.y = cmp((int)r0.y == 4);
    if (r0.y != 0) {
      r0.yzw = -_GlobalBoxAreaCenters[1].xyz + v6.xyz;
      r0.yzw = _GlobalBoxAreaExtents[1].xyz + -abs(r0.yzw);
      r0.z = min(r0.z, r0.w);
      r0.y = min(r0.y, r0.z);
      r0.z = max(0.00100000005, _GlobalBoxAreaExtents[1].w);
      r0.y = saturate(r0.y / r0.z);
      r0.z = cmp(0 < r0.y);
      if (r0.z != 0) {
        r0.z = 1.125 + -_GlobalBoxAreaIntensities[1];
        r0.z = r0.x + -r0.z;
        r0.z = saturate(-4 * r0.z);
        r0.w = r0.z * -2 + 3;
        r0.z = r0.z * r0.z;
        r0.z = r0.w * r0.z + -0.5;
        r0.y = r0.z + -r0.y;
        r0.y = 1 + r0.y;
        r0.y = cmp(r0.y < 0);
        if (r0.y != 0) discard;
      }
    }
    r0.y = (int)_GlobalBoxAreaCenters[2].w;
    r0.y = cmp((int)r0.y == 4);
    if (r0.y != 0) {
      r0.yzw = -_GlobalBoxAreaCenters[2].xyz + v6.xyz;
      r0.yzw = _GlobalBoxAreaExtents[2].xyz + -abs(r0.yzw);
      r0.z = min(r0.z, r0.w);
      r0.y = min(r0.y, r0.z);
      r0.z = max(0.00100000005, _GlobalBoxAreaExtents[2].w);
      r0.y = saturate(r0.y / r0.z);
      r0.z = cmp(0 < r0.y);
      if (r0.z != 0) {
        r0.z = 1.125 + -_GlobalBoxAreaIntensities[2];
        r0.z = r0.x + -r0.z;
        r0.z = saturate(-4 * r0.z);
        r0.w = r0.z * -2 + 3;
        r0.z = r0.z * r0.z;
        r0.z = r0.w * r0.z + -0.5;
        r0.y = r0.z + -r0.y;
        r0.y = 1 + r0.y;
        r0.y = cmp(r0.y < 0);
        if (r0.y != 0) discard;
      }
    }
    r0.y = (int)_GlobalBoxAreaCenters[3].w;
    r0.y = cmp((int)r0.y == 4);
    if (r0.y != 0) {
      r0.yzw = -_GlobalBoxAreaCenters[3].xyz + v6.xyz;
      r0.yzw = _GlobalBoxAreaExtents[3].xyz + -abs(r0.yzw);
      r0.z = min(r0.z, r0.w);
      r0.y = min(r0.y, r0.z);
      r0.z = max(0.00100000005, _GlobalBoxAreaExtents[3].w);
      r0.y = saturate(r0.y / r0.z);
      r0.z = cmp(0 < r0.y);
      if (r0.z != 0) {
        r0.z = 1.125 + -_GlobalBoxAreaIntensities[3];
        r0.z = r0.x + -r0.z;
        r0.z = saturate(-4 * r0.z);
        r0.w = r0.z * -2 + 3;
        r0.z = r0.z * r0.z;
        r0.z = r0.w * r0.z + -0.5;
        r0.y = r0.z + -r0.y;
        r0.y = 1 + r0.y;
        r0.y = cmp(r0.y < 0);
        if (r0.y != 0) discard;
      }
    }
    r0.y = (int)_GlobalBoxAreaCenters[4].w;
    r0.y = cmp((int)r0.y == 4);
    if (r0.y != 0) {
      r0.yzw = -_GlobalBoxAreaCenters[4].xyz + v6.xyz;
      r0.yzw = _GlobalBoxAreaExtents[4].xyz + -abs(r0.yzw);
      r0.z = min(r0.z, r0.w);
      r0.y = min(r0.y, r0.z);
      r0.z = max(0.00100000005, _GlobalBoxAreaExtents[4].w);
      r0.y = saturate(r0.y / r0.z);
      r0.z = cmp(0 < r0.y);
      if (r0.z != 0) {
        r0.z = 1.125 + -_GlobalBoxAreaIntensities[4];
        r0.z = r0.x + -r0.z;
        r0.z = saturate(-4 * r0.z);
        r0.w = r0.z * -2 + 3;
        r0.z = r0.z * r0.z;
        r0.z = r0.w * r0.z + -0.5;
        r0.y = r0.z + -r0.y;
        r0.y = 1 + r0.y;
        r0.y = cmp(r0.y < 0);
        if (r0.y != 0) discard;
      }
    }
    r0.y = (int)_GlobalBoxAreaCenters[5].w;
    r0.y = cmp((int)r0.y == 4);
    if (r0.y != 0) {
      r0.yzw = -_GlobalBoxAreaCenters[5].xyz + v6.xyz;
      r0.yzw = _GlobalBoxAreaExtents[5].xyz + -abs(r0.yzw);
      r0.z = min(r0.z, r0.w);
      r0.y = min(r0.y, r0.z);
      r0.z = max(0.00100000005, _GlobalBoxAreaExtents[5].w);
      r0.y = saturate(r0.y / r0.z);
      r0.z = cmp(0 < r0.y);
      if (r0.z != 0) {
        r0.z = 1.125 + -_GlobalBoxAreaIntensities[5];
        r0.z = r0.x + -r0.z;
        r0.z = saturate(-4 * r0.z);
        r0.w = r0.z * -2 + 3;
        r0.z = r0.z * r0.z;
        r0.z = r0.w * r0.z + -0.5;
        r0.y = r0.z + -r0.y;
        r0.y = 1 + r0.y;
        r0.y = cmp(r0.y < 0);
        if (r0.y != 0) discard;
      }
    }
    r0.y = (int)_GlobalBoxAreaCenters[6].w;
    r0.y = cmp((int)r0.y == 4);
    if (r0.y != 0) {
      r0.yzw = -_GlobalBoxAreaCenters[6].xyz + v6.xyz;
      r0.yzw = _GlobalBoxAreaExtents[6].xyz + -abs(r0.yzw);
      r0.z = min(r0.z, r0.w);
      r0.y = min(r0.y, r0.z);
      r0.z = max(0.00100000005, _GlobalBoxAreaExtents[6].w);
      r0.y = saturate(r0.y / r0.z);
      r0.z = cmp(0 < r0.y);
      if (r0.z != 0) {
        r0.z = 1.125 + -_GlobalBoxAreaIntensities[6];
        r0.z = r0.x + -r0.z;
        r0.z = saturate(-4 * r0.z);
        r0.w = r0.z * -2 + 3;
        r0.z = r0.z * r0.z;
        r0.z = r0.w * r0.z + -0.5;
        r0.y = r0.z + -r0.y;
        r0.y = 1 + r0.y;
        r0.y = cmp(r0.y < 0);
        if (r0.y != 0) discard;
      }
    }
    r0.y = (int)_GlobalBoxAreaCenters[7].w;
    r0.y = cmp((int)r0.y == 4);
    r1.xyz = -_GlobalBoxAreaCenters[7].xyz + v6.xyz;
    r1.xyz = _GlobalBoxAreaExtents[7].xyz + -abs(r1.xyz);
    r0.z = min(r1.y, r1.z);
    r0.z = min(r1.x, r0.z);
    r0.w = max(0.00100000005, _GlobalBoxAreaExtents[7].w);
    r0.z = saturate(r0.z / r0.w);
    r0.w = cmp(0 < r0.z);
    r0.y = r0.w ? r0.y : 0;
    r0.w = 1.125 + -_GlobalBoxAreaIntensities[7];
    r0.x = r0.x + -r0.w;
    r0.x = saturate(-4 * r0.x);
    r0.w = r0.x * -2 + 3;
    r0.x = r0.x * r0.x;
    r0.x = r0.w * r0.x + -0.5;
    r0.x = r0.x + -r0.z;
    r0.x = 1 + r0.x;
    r0.x = cmp(r0.x < 0);
    r0.x = r0.y ? r0.x : 0;
    if (r0.x != 0) discard;
  }
  r0.xyz = cmp(float3(1.5,0.5,1.5) < v8.wzz);
  r0.w = -0.5 + v7.y;
  r0.w = r0.w * 100 + (VD_COL(unity_ObjectToWorld, 3).y);
  r1.x = (VD_COL(unity_ObjectToWorld, 3).y) + -r0.w;
  r1.y = _IsSingleObject * r1.x + r0.w;
  r0.w = 100 * v8.x;
  r1.z = -v8.x * 100 + _ObjectBoundSize.y;
  r0.w = _IsSingleObject * r1.z + r0.w;
  r1.z = cmp(r1.y >= 1.5);
  r1.z = r1.z ? 1.000000 : 0;
  r0.w = cmp(r0.w >= 1.5);
  r0.w = r0.w ? 1.000000 : 0;
  r0.w = max(r1.z, r0.w);
  r1.x = 1 + -r0.w;
  r0.xw = r0.xx ? r1.xy : float2(1,0);
  r0.w = cmp(r0.w < 0.5);
  r0.yw = r0.yw ? float2(0,0) : 0;
  r0.y = (int)r0.y * (int)r0.w;
  r0.y = (int)r0.y;
  r0.y = 1 + -r0.y;
  r0.x = r0.x * r0.y;
  r0.x = r0.z ? 0 : r0.x;
  r0.y = 1 + -r0.x;
  r0.x = _GlobalAlwaysStencil * r0.y + r0.x;
  o0.y = v0.z * r0.x;
  o0.x = v0.z;
  o0.zw = float2(0,0);
  return;
}
