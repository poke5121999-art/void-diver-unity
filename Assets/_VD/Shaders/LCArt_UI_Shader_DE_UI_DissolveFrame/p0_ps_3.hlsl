// LCArt/UI/Shader_DE_UI_DissolveFrame | pass 0 "Default" | progFragment | keywords: (none) | blob 3
void frag(
  float4 v0 : SV_POSITION0,
  float4 v1 : COLOR0,
  float4 v2 : TEXCOORD0,
  float4 v3 : TEXCOORD1,
  out float4 o0 : SV_Target0)
{
  float4 r0,r1;
  uint4 bitmask, uiDest;
  float4 fDest;

  r0.xy = float2(-0.5,-0.5) + v2.xy;
  r0.z = -0.988031626 * r0.y;
  r1.x = r0.x * 0.154251456 + -r0.z;
  r1.y = dot(r0.xy, float2(-0.988031626,0.154251456));
  r0.x = max(abs(r1.x), abs(r1.y));
  r0.x = 1 / r0.x;
  r0.y = min(abs(r1.x), abs(r1.y));
  r0.x = r0.y * r0.x;
  r0.y = r0.x * r0.x;
  r0.z = r0.y * 0.0208350997 + -0.0851330012;
  r0.z = r0.y * r0.z + 0.180141002;
  r0.z = r0.y * r0.z + -0.330299497;
  r0.y = r0.y * r0.z + 0.999866009;
  r0.z = r0.x * r0.y;
  r0.z = r0.z * -2 + 1.57079637;
  r0.w = cmp(abs(r1.y) < abs(r1.x));
  r0.z = r0.w ? r0.z : 0;
  r0.x = r0.x * r0.y + r0.z;
  r0.y = cmp(r1.y < -r1.y);
  r0.y = r0.y ? -3.141593 : 0;
  r0.x = r0.x + r0.y;
  r0.y = min(r1.x, r1.y);
  r0.y = cmp(r0.y < -r0.y);
  r0.z = max(r1.x, r1.y);
  r0.w = dot(r1.xy, r1.xy);
  r0.w = sqrt(r0.w);
  r0.z = cmp(r0.z >= -r0.z);
  r0.y = r0.z ? r0.y : 0;
  r0.x = r0.y ? -r0.x : r0.x;
  r0.x = 0.159235671 * r0.x;
  r0.y = abs(r0.x);
  r0.z = r0.w + r0.w;
  r0.x = _Time.y * _DissolveNoiseSpeed + r0.z;
  r0.z = r0.z * r0.z;
  r0.xy = float2(2,0.25) * r0.xy;
  r0.xy = (_DissolvePatternScale).xx * r0.xy;
  r1.xyzw = _NoiseTexture.Sample(sampler_NoiseTexture, r0.xy).xyzw;
  r0.x = r1.x + r0.w;
  r0.x = saturate(r0.x * r0.z);
  r0.x = -v1.w * 1.29999995 + r0.x;
  r0.y = 0.5 * v1.w;
  r0.y = 1 / r0.y;
  r0.x = saturate(r0.x * r0.y);
  r0.y = r0.x * -2 + 3;
  r0.x = r0.x * r0.x;
  r0.x = -r0.y * r0.x + 1;
  r0.x = max(0, r0.x);
  r1.xyzw = _MainTex.Sample(sampler_MainTex, v2.xy).xyzw;
  r0.y = _DissolveColor.w * r1.w;
  o0.w = r0.y * r0.x;
  r0.xyz = _DissolveColor.xyz * v1.xyz;
  o0.xyz = r0.xyz * r1.xyz;
  return;
}
