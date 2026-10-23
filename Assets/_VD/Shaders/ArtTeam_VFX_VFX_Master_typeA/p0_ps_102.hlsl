// ArtTeam/VFX/VFX_Master_typeA | pass 0 "Universal Forward" | progFragment | keywords: _DBUFFER_MRT3 _PLANESYSTEMCULLINGTYPE_ALWAYS | blob 102
void frag(
  float4 v0 : SV_POSITION0,
  float4 v1 : VDREG1,
  float4 v2 : VDREG2,
  float4 v3 : VDREG3,
  float4 v4 : VDREG4,
  float4 v5 : VDREG5,
  float4 v6 : VDREG6,
  float4 v7 : VDREG7,
  out float4 o0 : SV_Target0)
{
  uint4 r0, r1, r2, r3;
  r0 = 0u; r1 = 0u; r2 = 0u; r3 = 0u;

  r0.xy = vd_asuint((vd_asfloat(v1.xy) + float2(-0.5, -0.5)));
  r0.z = vd_asuint(max(abs(vd_asfloat(r0.y)), abs(vd_asfloat(r0.x))));
  r0.z = vd_asuint((1.0 / vd_asfloat(r0.z)));
  r0.w = vd_asuint(min(abs(vd_asfloat(r0.y)), abs(vd_asfloat(r0.x))));
  r0.z = vd_asuint((vd_asfloat(r0.z) * vd_asfloat(r0.w)));
  r0.w = vd_asuint((vd_asfloat(r0.z) * vd_asfloat(r0.z)));
  r1.x = vd_asuint((vd_asfloat(r0.w) * 0.0208350997 + -0.0851330012));
  r1.x = vd_asuint((vd_asfloat(r0.w) * vd_asfloat(r1.x) + 0.180141002));
  r1.x = vd_asuint((vd_asfloat(r0.w) * vd_asfloat(r1.x) + -0.330299497));
  r0.w = vd_asuint((vd_asfloat(r0.w) * vd_asfloat(r1.x) + 0.999866009));
  r1.x = vd_asuint((vd_asfloat(r0.w) * vd_asfloat(r0.z)));
  r1.x = vd_asuint((vd_asfloat(r1.x) * -2.0 + 1.57079637));
  r1.y = vd_asuint((-(int)(abs(vd_asfloat(r0.y)) < abs(vd_asfloat(r0.x)))));
  r1.x = (r1.y & r1.x);
  r0.z = vd_asuint((vd_asfloat(r0.z) * vd_asfloat(r0.w) + vd_asfloat(r1.x)));
  r0.w = vd_asuint((-(int)(vd_asfloat(r0.y) < (-(vd_asfloat(r0.y))))));
  r0.w = (r0.w & 0xc0490fdbu);
  r0.z = vd_asuint((vd_asfloat(r0.w) + vd_asfloat(r0.z)));
  r0.w = vd_asuint(min(vd_asfloat(r0.y), vd_asfloat(r0.x)));
  r0.w = vd_asuint((-(int)(vd_asfloat(r0.w) < (-(vd_asfloat(r0.w))))));
  r1.x = vd_asuint(max(vd_asfloat(r0.y), vd_asfloat(r0.x)));
  r0.x = vd_asuint(dot(vd_asfloat(r0.xy), vd_asfloat(r0.xy)));
  r0.x = vd_asuint(sqrt(vd_asfloat(r0.x)));
  r0.x = vd_asuint((vd_asfloat(r0.x) + vd_asfloat(r0.x)));
  r1.x = vd_asuint((-(int)(vd_asfloat(r1.x) >= (-(vd_asfloat(r1.x))))));
  r0.w = (r0.w & r1.x);
  r0.z = vd_asuint(((r0.w != 0u) ? (-(vd_asfloat(r0.z))) : vd_asfloat(r0.z)));
  r0.y = vd_asuint((vd_asfloat(r0.z) * 0.159235656));
  r0.xy = vd_asuint((vd_asfloat(r0.xy) + (-(vd_asfloat(v1.xy)))));
  r0.xy = vd_asuint((vd_asfloat((_Main_Radial).xx) * vd_asfloat(r0.xy) + vd_asfloat(v1.xy)));
  r0.zw = vd_asuint(((-(vd_asfloat(v2.zw))) + float2(1.0, 1.0)));
  r0.zw = vd_asuint((vd_asfloat((_Use_Main_TilingOffset).xx) * vd_asfloat(r0.zw) + vd_asfloat(v2.zw)));
  r1.xyz = vd_asuint((vd_asfloat(float3(_Main_Offset_UV1xy.x, _Main_Offset_UV1xy.y, _Dissolve_Offset_UV2y)) * vd_asfloat(_TimeParameters.xxx) + float3(1.0, 1.0, 1.0)));
  r1.xy = vd_asuint((vd_asfloat(r1.xy) * vd_asfloat(v2.xy)));
  r1.z = vd_asuint((vd_asfloat(r1.z) * vd_asfloat(v3.y)));
  r2.xy = vd_asuint((vd_asfloat(_Main_Offset_UV1xy.xy) * vd_asfloat(_TimeParameters.xx) + (-(vd_asfloat(r1.xy)))));
  r1.xy = vd_asuint((vd_asfloat((_Use_Main_TilingOffset).xx) * vd_asfloat(r2.xy) + vd_asfloat(r1.xy)));
  r0.xy = vd_asuint((vd_asfloat(r0.xy) * vd_asfloat(r0.zw) + vd_asfloat(r1.xy)));
  r0.zw = vd_asuint((float2(1.0, 1.0) / vd_asfloat(_ScreenParams.xy)));
  r0.zw = vd_asuint((vd_asfloat(r0.zw) * vd_asfloat((_Pixelation).xx)));
  r0.xy = vd_asuint((vd_asfloat(r0.xy) / vd_asfloat(r0.zw)));
  r0.xy = vd_asuint(floor(vd_asfloat(r0.xy)));
  r1.xy = vd_asuint((vd_asfloat(r0.zw) * vd_asfloat(r0.xy)));
  r0.xy = vd_asuint((vd_asfloat(r0.xy) * vd_asfloat(r0.zw) + float2(-0.5, -0.5)));
  r0.z = vd_asuint((vd_asfloat(_Dissolve_Offset_UV2y) * vd_asfloat(_TimeParameters.x) + (-(vd_asfloat(r1.z)))));
  r0.w = vd_asuint((-(int)(abs(vd_asfloat(_Dissolve_Offset_UV2y)) >= 0.00100000005)));
  r0.w = (r0.w & 0x3f800000u);
  r0.z = vd_asuint((vd_asfloat(r0.w) * vd_asfloat(r0.z) + vd_asfloat(r1.z)));
  r0.zw = vd_asuint((vd_asfloat(r0.zz) * vd_asfloat(_DissolveDirection.xy) + vd_asfloat(r1.xy)));
  r0.zw = vd_asuint((vd_asfloat(r0.zw) + float2(-0.5, -0.5)));
  r1.x = vd_asuint((vd_asfloat(_Dissolve_Rotate) * 0.0174444448));
  r1.x = vd_asuint(sin(vd_asfloat(r1.x)));
  r2.x = vd_asuint(cos(vd_asfloat(r1.x)));
  r3.x = vd_asuint((-(vd_asfloat(r1.x))));
  r3.y = r2.x;
  r3.z = r1.x;
  r1.x = vd_asuint(dot(vd_asfloat(r0.zw), vd_asfloat(r3.yz)));
  r1.y = vd_asuint(dot(vd_asfloat(r0.zw), vd_asfloat(r3.xy)));
  r0.zw = vd_asuint((vd_asfloat(r1.xy) + float2(0.5, 0.5)));
  r0.zw = vd_asuint((vd_asfloat(r0.zw) * vd_asfloat(_DissolveTex_ST.xy) + vd_asfloat(_DissolveTex_ST.zw)));
  r1.xyzw = vd_asuint(_DissolveTex.SampleBias(sampler_DissolveTex, vd_asfloat(r0.zw), vd_asfloat(_GlobalMipBias.x)).xyzw);
  r0.z = vd_asuint(((-(vd_asfloat(r1.x))) + vd_asfloat(r1.y)));
  r0.z = vd_asuint((vd_asfloat(_Use_Dissolve_Green) * vd_asfloat(r0.z) + vd_asfloat(r1.x)));
  r0.w = vd_asuint((vd_asfloat(v5.w) * 2.0 + -1.0));
  r1.x = vd_asuint(((-(vd_asfloat(v3.w))) + vd_asfloat(_Dissolve_Progress_UV2w)));
  r1.x = vd_asuint((vd_asfloat(_Use_Dissolve_Progress_Property) * vd_asfloat(r1.x) + vd_asfloat(v3.w)));
  r0.w = vd_asuint((vd_asfloat(r0.w) + (-(vd_asfloat(r1.x)))));
  r0.w = vd_asuint((vd_asfloat(_Use_Dissolve_Alpha) * vd_asfloat(r0.w) + vd_asfloat(r1.x)));
  r0.w = vd_asuint(max(vd_asfloat(r0.w), -1.0));
  r0.w = vd_asuint(min(vd_asfloat(r0.w), 1.0));
  r0.w = vd_asuint((vd_asfloat(r0.w) + (-(vd_asfloat(_DissolveEdge_Wide)))));
  r0.z = vd_asuint((vd_asfloat(r0.w) + vd_asfloat(r0.z)));
  r0.z = vd_asuint(((-(vd_asfloat(r0.z))) + 1.0));
  r0.w = vd_asuint((vd_asfloat(_DissolveEdge_Sharp) + -0.5));
  r0.z = vd_asuint(((-(vd_asfloat(r0.w))) + vd_asfloat(r0.z)));
  r1.x = vd_asuint(((-(vd_asfloat(r0.w))) + 1.0));
  r0.w = vd_asuint(((-(vd_asfloat(r0.w))) + vd_asfloat(r1.x)));
  r0.w = vd_asuint((1.0 / vd_asfloat(r0.w)));
  r0.z = vd_asuint(saturate((vd_asfloat(r0.w) * vd_asfloat(r0.z))));
  r0.w = vd_asuint((vd_asfloat(r0.z) * -2.0 + 3.0));
  r0.z = vd_asuint((vd_asfloat(r0.z) * vd_asfloat(r0.z)));
  r0.z = vd_asuint((vd_asfloat(r0.z) * vd_asfloat(r0.w)));
  r0.w = vd_asuint((vd_asfloat(_Main_Rotate) * 0.0174444448));
  r1.x = vd_asuint(sin(vd_asfloat(r0.w)));
  r2.x = vd_asuint(cos(vd_asfloat(r0.w)));
  r3.x = vd_asuint((-(vd_asfloat(r1.x))));
  r3.y = r2.x;
  r3.z = r1.x;
  r1.x = vd_asuint(dot(vd_asfloat(r0.xy), vd_asfloat(r3.yz)));
  r1.y = vd_asuint(dot(vd_asfloat(r0.xy), vd_asfloat(r3.xy)));
  r0.xy = vd_asuint((vd_asfloat(r1.xy) + float2(0.5, 0.5)));
  r0.xy = vd_asuint((vd_asfloat(r0.xy) * vd_asfloat(_MainTex_ST.xy) + vd_asfloat(_MainTex_ST.zw)));
  r1.xyzw = vd_asuint(_MainTex.SampleBias(sampler_MainTex, vd_asfloat(r0.xy), vd_asfloat(_GlobalMipBias.x)).xyzw);
  r0.xyw = vd_asuint(((-(vd_asfloat(r1.xxx))) + vd_asfloat(r1.xyz)));
  r0.xyw = vd_asuint((vd_asfloat((_isColor).xxx) * vd_asfloat(r0.xyw) + vd_asfloat(r1.xxx)));
  r0.xyw = vd_asuint((vd_asfloat(r0.xyw) * vd_asfloat(v5.xyz)));
  r1.x = vd_asuint(max(vd_asfloat(v3.z), 1.0));
  r0.xyw = vd_asuint((vd_asfloat(r0.xyw) * vd_asfloat(r1.xxx)));
  r1.xyz = vd_asuint(((-(vd_asfloat(_MainColor.xyz))) * vd_asfloat(r0.xyw) + vd_asfloat(_DissolveEdge_Color.xyz)));
  r0.xyw = vd_asuint((vd_asfloat(r0.xyw) * vd_asfloat(_MainColor.xyz)));
  r1.xyz = vd_asuint((vd_asfloat(r0.zzz) * vd_asfloat(r1.xyz)));
  r0.xyz = vd_asuint((vd_asfloat((_DissolveEdge_Use).xxx) * vd_asfloat(r1.xyz) + vd_asfloat(r0.xyw)));
  r1.xy = vd_asuint(((int2)vd_asfloat(v0.xy)));
  r1.zw = uint2(0x00000000u, 0x00000000u);
  r1.xyzw = vd_asuint(_DBufferTexture0.Load(vd_asint(r1.xyw)).xyzw);
  o0.xyz = (vd_asfloat(r0.xyz) * vd_asfloat(r1.www) + vd_asfloat(r1.xyz));
  o0.w = vd_asfloat(0x3f800000u);
  return;
}
