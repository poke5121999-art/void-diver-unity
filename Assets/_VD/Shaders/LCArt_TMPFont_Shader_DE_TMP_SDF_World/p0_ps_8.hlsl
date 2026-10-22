// LCArt/TMPFont/Shader_DE_TMP_SDF_World | pass 0 "TextMeshPro SDF" | progFragment | keywords: UNITY_UI_CLIP_RECT | blob 8
void frag(
  float4 v0 : SV_POSITION0,
  float4 v1 : COLOR0,
  float4 v2 : TEXCOORD0,
  float4 v3 : TEXCOORD1,
  float4 v4 : TEXCOORD2,
  float4 v5 : TEXCOORD3,
  float4 v6 : TEXCOORD5,
  out float4 o0 : SV_Target0)
{
  float4 r0,r1,r2,r3,r4;
  uint4 bitmask, uiDest;
  float4 fDest;

  r0.x = cmp(_GlobalBoxAreaEnable >= 0.5);
  if (r0.x != 0) {
    r0.x = 0;
    while (true) {
      r0.y = cmp((int)r0.x >= 8);
      if (r0.y != 0) break;
      r0.y = cmp(_GlobalBoxAreaCoverWorldUIs[(int)(r0.x)] < 0.5);
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
        o0.xyzw = float4(0,0,0,0);
        return;
      }
      r0.x = (int)r0.x + 1;
    }
  }
  r0.xyzw = _MainTex.SampleBias(sampler_MainTex, v2.xy, _GlobalMipBias.x).xyzw;
  r0.x = -v3.x + r0.w;
  r0.x = cmp(r0.x < 0);
  if (r0.x != 0) discard;
  r0.x = v3.z + -r0.w;
  r0.y = _ScaleRatioA * _OutlineWidth;
  r0.z = _ScaleRatioA * _OutlineSoftness;
  r0.yw = v3.yy * r0.yz;
  r1.xyz = _FaceColor.xyz * v1.xyz;
  r2.xy = float2(_FaceUVSpeedX, _FaceUVSpeedY) * _Time.yy + v6.xy;
  r2.xyzw = _FaceTex.SampleBias(sampler_FaceTex, r2.xy, _GlobalMipBias.x).xyzw;
  r1.xyz = r2.xyz * r1.xyz;
  r2.w = _FaceColor.w * r2.w;
  r3.xy = float2(_OutlineUVSpeedX, _OutlineUVSpeedY) * _Time.yy + v6.zw;
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
  r0.xyzw = r1.xyzw * r0.zzzz;
  r1.xy = _ClipRect.zw + -_ClipRect.xy;
  r1.xy = -abs(v4.xy) + r1.xy;
  r1.xy = saturate(v4.zw * r1.xy);
  r1.x = r1.x * r1.y;
  r0.xyzw = r1.xxxx * r0.xyzw;
  o0.xyzw = v1.wwww * r0.xyzw;
  return;
}
