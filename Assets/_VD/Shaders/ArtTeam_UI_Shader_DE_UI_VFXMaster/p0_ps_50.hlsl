// ArtTeam/UI/Shader_DE_UI_VFXMaster | pass 0 "Default" | progFragment | keywords: UNITY_UI_ALPHACLIP | blob 50
void frag(
  float4 v0 : SV_POSITION0,
  float4 v1 : COLOR0,
  float4 v2 : TEXCOORD0,
  float4 v3 : TEXCOORD1,
  float4 v4 : TEXCOORD2,
  float4 v5 : TEXCOORD3,
  out float4 o0 : SV_Target0)
{
  float4 r0,r1,r2,r3;
  uint4 bitmask, uiDest;
  float4 fDest;

  r0.xy = v2.xy * _Main_TileOffset.xy + _Main_TileOffset.zw;
  r0.zw = float2(-0.5,-0.5) + r0.xy;
  r1.x = max(abs(r0.z), abs(r0.w));
  r1.x = 1 / r1.x;
  r1.y = min(abs(r0.z), abs(r0.w));
  r1.x = r1.y * r1.x;
  r1.y = r1.x * r1.x;
  r1.z = r1.y * 0.0208350997 + -0.0851330012;
  r1.z = r1.y * r1.z + 0.180141002;
  r1.z = r1.y * r1.z + -0.330299497;
  r1.y = r1.y * r1.z + 0.999866009;
  r1.z = r1.x * r1.y;
  r1.z = r1.z * -2 + 1.57079637;
  r1.w = cmp(abs(r0.w) < abs(r0.z));
  r1.z = r1.w ? r1.z : 0;
  r1.x = r1.x * r1.y + r1.z;
  r1.y = cmp(r0.w < -r0.w);
  r1.y = r1.y ? -3.141593 : 0;
  r1.x = r1.x + r1.y;
  r1.y = min(r0.z, r0.w);
  r1.y = cmp(r1.y < -r1.y);
  r1.z = max(r0.z, r0.w);
  r0.z = dot(r0.zw, r0.zw);
  r0.z = sqrt(r0.z);
  r2.x = dot(r0.zz, _Main_Radial_Tilling.yy);
  r0.z = cmp(r1.z >= -r1.z);
  r0.z = r0.z ? r1.y : 0;
  r0.z = r0.z ? -r1.x : r1.x;
  r0.z = _Main_Radial_Tilling.x * r0.z;
  r2.y = 0.159154937 * r0.z;
  r0.zw = cmp(float2(0,0) != float2(_Main_Radial, _Use_Main_Panning));
  r0.xy = r0.zz ? r2.xy : r0.xy;
  r1.xy = _Time.yy * _Main_Pan_Speed.xy + r0.xy;
  r0.xy = r0.ww ? r1.xy : r0.xy;
  r0.xy = float2(-0.5,-0.5) + r0.xy;
  r0.z = 0.0174532942 * _Main_Rotate;
  sincos(r0.z, r1.x, r2.x);
  r3.z = r1.x;
  r3.y = r2.x;
  r3.x = -r1.x;
  r1.y = dot(r0.xy, r3.xy);
  r1.x = dot(r0.xy, r3.yz);
  r0.xy = float2(0.5,0.5) + r1.xy;
  r0.xyzw = _MainTex.Sample(sampler_MainTex, r0.xy).xyzw;
  r1.x = cmp(_Main_Alpha_Ch == 1.000000);
  r0.w = r1.x ? r0.x : r0.w;
  r0.xyz = log2(abs(r0.xyz));
  r0.xyz = (_MainTex_Contrast).xxx * r0.xyz;
  r0.xyz = exp2(r0.xyz);
  r0.xyz = _MainColor.xyz * r0.xyz;
  r0.w = log2(abs(r0.w));
  r0.w = _Main_Alpha_Contrast * r0.w;
  r0.w = exp2(r0.w);
  r1.x = _MainColor.w * r0.w;
  r1.y = 255 * v1.w;
  r1.y = round(r1.y);
  r1.y = 0.00392156886 * r1.y;
  r1.x = r1.x * r1.y;
  r1.y = cmp(0 != _DissolveAlphaControll);
  r0.w = r1.y ? r0.w : r1.x;
  r1.x = -_AlphaClip * 0.5 + r0.w;
  r1.y = 1 + -_AlphaClip;
  r1.y = 1 / r1.y;
  r1.x = saturate(r1.x * r1.y);
  r1.y = r1.x * -2 + 3;
  r1.x = r1.x * r1.x;
  r1.x = r1.y * r1.x + -r0.w;
  r1.y = cmp(0.00100000005 >= _AlphaClip);
  r1.y = r1.y ? 0 : 1;
  r0.w = r1.y * r1.x + r0.w;
  r1.x = -0.00100000005 + r0.w;
  r1.x = cmp(r1.x < 0);
  if (r1.x != 0) discard;
  r1.xyz = v1.xyz * float3(0.305306017,0.305306017,0.305306017) + float3(0.682171106,0.682171106,0.682171106);
  r1.xyz = v1.xyz * r1.xyz + float3(0.0125228781,0.0125228781,0.0125228781);
  r1.xyz = v1.xyz * r1.xyz + -v1.xyz;
  r1.w = vd_asint(_UIVertexColorAlwaysGammaSpace);
  r1.xyz = r1.www * r1.xyz + v1.xyz;
  r0.xyz = r1.xyz * r0.xyz;
  o0.xyz = r0.xyz * r0.www;
  o0.w = r0.w;
  return;
}
