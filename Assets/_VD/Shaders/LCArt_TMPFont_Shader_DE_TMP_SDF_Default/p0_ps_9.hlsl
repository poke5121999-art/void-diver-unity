// LCArt/TMPFont/Shader_DE_TMP_SDF_Default | pass 0 "TextMeshPro SDF" | progFragment | keywords: UNITY_UI_CLIP_RECT | blob 9
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
  float4 r0,r1,r2,r3;
  uint4 bitmask, uiDest;
  float4 fDest;

  r0.xyzw = _MainTex.SampleBias(sampler_MainTex, v2.xy, _GlobalMipBias.x).xyzw;
  r0.x = -v3.x + r0.w;
  r0.y = v3.z + -r0.w;
  r0.x = cmp(r0.x < 0);
  if (r0.x != 0) discard;
  r0.x = _ScaleRatioA * _OutlineWidth;
  r0.x = v3.y * r0.x;
  r0.z = min(1, r0.x);
  r0.x = 0.5 * r0.x;
  r0.z = sqrt(r0.z);
  r0.w = saturate(r0.y * v3.y + r0.x);
  r0.x = r0.y * v3.y + -r0.x;
  r0.y = r0.w * r0.z;
  r0.zw = float2(_OutlineUVSpeedX, _OutlineUVSpeedY) * _Time.yy + v6.zw;
  r1.xyzw = _OutlineTex.SampleBias(sampler_OutlineTex, r0.zw, _GlobalMipBias.x).xyzw;
  r1.xyzw = _OutlineColor.xyzw * r1.xyzw;
  r1.xyz = r1.xyz * r1.www;
  r2.xyz = _FaceColor.xyz * v1.xyz;
  r0.zw = float2(_FaceUVSpeedX, _FaceUVSpeedY) * _Time.yy + v6.xy;
  r3.xyzw = _FaceTex.SampleBias(sampler_FaceTex, r0.zw, _GlobalMipBias.x).xyzw;
  r2.xyz = r3.xyz * r2.xyz;
  r3.w = _FaceColor.w * r3.w;
  r3.xyz = r3.www * r2.xyz;
  r1.xyzw = -r3.xyzw + r1.xyzw;
  r1.xyzw = r0.yyyy * r1.xyzw + r3.xyzw;
  r0.y = _ScaleRatioA * _OutlineSoftness;
  r0.z = v3.y * r0.y;
  r0.y = r0.y * v3.y + 1;
  r0.x = r0.z * 0.5 + r0.x;
  r0.x = saturate(r0.x / r0.y);
  r0.x = 1 + -r0.x;
  r0.xyzw = r1.xyzw * r0.xxxx;
  r1.xy = _ClipRect.zw + -_ClipRect.xy;
  r1.xy = -abs(v4.xy) + r1.xy;
  r1.xy = saturate(v4.zw * r1.xy);
  r1.x = r1.x * r1.y;
  r0.xyzw = r1.xxxx * r0.xyzw;
  o0.xyzw = v1.wwww * r0.xyzw;
  return;
}
