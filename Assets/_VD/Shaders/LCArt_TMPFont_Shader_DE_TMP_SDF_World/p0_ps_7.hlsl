// LCArt/TMPFont/Shader_DE_TMP_SDF_World | pass 0 "TextMeshPro SDF" | progFragment | keywords: UNITY_UI_ALPHACLIP | blob 7
void frag(
  float4 v0 : SV_POSITION0,
  float4 v1 : VDREG1,
  float4 v2 : VDREG2,
  float4 v3 : VDREG3,
  float4 v4 : VDREG4,
  float4 v5 : VDREG5,
  float4 v6 : VDREG6,
  out float4 o0 : SV_Target0)
{
  uint4 r0, r1, r2, r3, r4;
  r0 = 0u; r1 = 0u; r2 = 0u; r3 = 0u; r4 = 0u;

  r0.x = vd_asuint((-(int)(vd_asfloat(_GlobalBoxAreaEnable) >= 0.5)));
  if (r0.x != 0u) {
    r0.x = 0x00000000u;
    while (true) {
      r0.y = vd_asuint((-(int)(vd_asint(r0.x) >= 8)));
      if (r0.y != 0u) break;
      r0.y = vd_asuint((-(int)(vd_asfloat(_GlobalBoxAreaCoverWorldUIs[(int)(r0.x)]) < 0.5)));
      if (r0.y != 0u) {
        r0.y = vd_asuint((vd_asint(r0.x) + 1));
        r0.x = r0.y;
        continue;
      }
      r0.yzw = vd_asuint((vd_asfloat((VD_COL(unity_ObjectToWorld, 3).xyz)) + (-(vd_asfloat(_GlobalBoxAreaCenters[(int)(r0.x)].xyz)))));
      r0.yzw = vd_asuint((abs(vd_asfloat(r0.yzw)) + (-(vd_asfloat(_GlobalBoxAreaExtents[(int)(r0.x)].xyz)))));
      r0.y = vd_asuint(max(vd_asfloat(r0.z), vd_asfloat(r0.y)));
      r0.y = vd_asuint(max(vd_asfloat(r0.w), vd_asfloat(r0.y)));
      r0.y = vd_asuint((-(int)(0.0 >= vd_asfloat(r0.y))));
      if (r0.y != 0u) {
        if (0xffffffffu != 0u) discard;
        o0.xyzw = vd_asfloat(uint4(0x00000000u, 0x00000000u, 0x00000000u, 0x00000000u));
        return;
      }
      r0.x = vd_asuint((vd_asint(r0.x) + 1));
    }
  }
  r0.xyzw = vd_asuint(_MainTex.SampleBias(sampler_MainTex, vd_asfloat(v2.xy), vd_asfloat(_GlobalMipBias.x)).xyzw);
  r0.x = vd_asuint((vd_asfloat(r0.w) + (-(vd_asfloat(v3.x)))));
  r0.x = vd_asuint((-(int)(vd_asfloat(r0.x) < 0.0)));
  if (r0.x != 0u) discard;
  r0.x = vd_asuint(((-(vd_asfloat(r0.w))) + vd_asfloat(v3.z)));
  r0.y = vd_asuint((vd_asfloat(_OutlineWidth) * vd_asfloat(_ScaleRatioA)));
  r0.z = vd_asuint((vd_asfloat(_OutlineSoftness) * vd_asfloat(_ScaleRatioA)));
  r0.yw = vd_asuint((vd_asfloat(r0.yz) * vd_asfloat(v3.yy)));
  r1.xyz = vd_asuint((vd_asfloat(v1.xyz) * vd_asfloat(_FaceColor.xyz)));
  r2.xy = vd_asuint((vd_asfloat(float2(_FaceUVSpeedX, _FaceUVSpeedY)) * vd_asfloat(_Time.yy) + vd_asfloat(v6.xy)));
  r2.xyzw = vd_asuint(_FaceTex.SampleBias(sampler_FaceTex, vd_asfloat(r2.xy), vd_asfloat(_GlobalMipBias.x)).xyzw);
  r1.xyz = vd_asuint((vd_asfloat(r1.xyz) * vd_asfloat(r2.xyz)));
  r2.w = vd_asuint((vd_asfloat(r2.w) * vd_asfloat(_FaceColor.w)));
  r3.xy = vd_asuint((vd_asfloat(float2(_OutlineUVSpeedX, _OutlineUVSpeedY)) * vd_asfloat(_Time.yy) + vd_asfloat(v6.zw)));
  r3.xyzw = vd_asuint(_OutlineTex.SampleBias(sampler_OutlineTex, vd_asfloat(r3.xy), vd_asfloat(_GlobalMipBias.x)).xyzw);
  r3.xyzw = vd_asuint((vd_asfloat(r3.xyzw) * vd_asfloat(_OutlineColor.xyzw)));
  r1.w = vd_asuint((vd_asfloat(r0.y) * 0.5));
  r4.x = vd_asuint((vd_asfloat(r0.x) * vd_asfloat(v3.y) + (-(vd_asfloat(r1.w)))));
  r0.w = vd_asuint((vd_asfloat(r0.w) * 0.5 + vd_asfloat(r4.x)));
  r0.z = vd_asuint((vd_asfloat(r0.z) * vd_asfloat(v3.y) + 1.0));
  r0.z = vd_asuint(saturate((vd_asfloat(r0.w) / vd_asfloat(r0.z))));
  r0.z = vd_asuint(((-(vd_asfloat(r0.z))) + 1.0));
  r0.x = vd_asuint(saturate((vd_asfloat(r0.x) * vd_asfloat(v3.y) + vd_asfloat(r1.w))));
  r0.y = vd_asuint(min(vd_asfloat(r0.y), 1.0));
  r0.y = vd_asuint(sqrt(vd_asfloat(r0.y)));
  r0.x = vd_asuint((vd_asfloat(r0.y) * vd_asfloat(r0.x)));
  r2.xyz = vd_asuint((vd_asfloat(r1.xyz) * vd_asfloat(r2.www)));
  r3.xyz = vd_asuint((vd_asfloat(r3.www) * vd_asfloat(r3.xyz)));
  r1.xyzw = vd_asuint(((-(vd_asfloat(r2.xyzw))) + vd_asfloat(r3.xyzw)));
  r1.xyzw = vd_asuint((vd_asfloat(r0.xxxx) * vd_asfloat(r1.xyzw) + vd_asfloat(r2.xyzw)));
  r2.xyzw = vd_asuint((vd_asfloat(r0.zzzz) * vd_asfloat(r1.xyzw)));
  r0.x = vd_asuint((vd_asfloat(r1.w) * vd_asfloat(r0.z) + -0.00100000005));
  r0.x = vd_asuint((-(int)(vd_asfloat(r0.x) < 0.0)));
  if (r0.x != 0u) discard;
  o0.xyzw = (vd_asfloat(r2.xyzw) * vd_asfloat(v1.wwww));
  return;
}
