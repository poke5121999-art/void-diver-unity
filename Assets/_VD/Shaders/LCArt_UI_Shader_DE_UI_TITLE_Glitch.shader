Shader "LCArt/UI/Shader_DE_UI_TITLE_Glitch"
{
Properties
{
    [HideInInspector] [PerRendererData] _MainTex ("Sprite Texture", 2D) = "white" {}
    [HideInInspector] _Color ("Tint", Color) = (1, 1, 1, 1)
    [HideInInspector] _StencilComp ("Stencil Comparison", Float) = 8
    [HideInInspector] _Stencil ("Stencil ID", Float) = 0
    [HideInInspector] _StencilOp ("Stencil Operation", Float) = 0
    [HideInInspector] _StencilWriteMask ("Stencil Write Mask", Float) = 255
    [HideInInspector] _StencilReadMask ("Stencil Read Mask", Float) = 255
    [HideInInspector] _ColorMask ("Color Mask", Float) = 15
    [Header (Effect 1 Slice and Chroma)] [Space (5)] _SliceFlickerRate ("Flicker Rate (초당 깜빡임 체크 횟수)", Float) = 8
    _SliceFlickerThreshold ("Flicker Threshold (활성 확률 0~1)", Range(0, 1)) = 0.15000000596046448
    _GlitchStrength ("Slice Strength (가로 왜곡 강도)", Range(0, 0.5)) = 0.05000000074505806
    _GlitchBandScale ("Slice Band Scale (세로 밴드 분할 수)", Float) = 10
    _GlitchStepRate ("Noise Flow Speed (노이즈 흐름 속도)", Float) = 4
    _ChromaStrength ("Chroma Strength (RGB 분리 UV 오프셋)", Range(0, 0.10000000149011612)) = 0.07999999821186066
    [Header (Effect 2 Vertex Jitter)] [Space (5)] _JitterFlickerRate ("Flicker Rate (초당 깜빡임 체크 횟수)", Float) = 12
    _JitterFlickerThreshold ("Flicker Threshold (활성 확률 0~1)", Range(0, 1)) = 0.20000000298023224
    _JitterSpeed ("Jitter Speed (활성 중 진동 갱신 속도)", Float) = 12
    _VertexJitterStrength ("Jitter Strength (메쉬 진동 NDC 오프셋)", Range(0, 0.10000000149011612)) = 0.004999999888241291
    [Header (Effect 3 Warp)] [Space (5)] _WarpFlickerRate ("Flicker Rate (초당 깜빡임 체크 횟수)", Float) = 4
    _WarpFlickerThreshold ("Flicker Threshold (활성 확률 0~1)", Range(0, 1)) = 0.10000000149011612
    _WarpSpeed ("Warp Speed (활성 중 노이즈 흐름 속도)", Float) = 1.5
    _WarpStrength ("Warp Strength (UV 왜곡 강도)", Range(0, 0.30000001192092896)) = 0.029999999329447746
    _WarpScale ("Warp Scale (왜곡 패턴 밀도)", Float) = 5
    [Header (Effect 4 Block Offset)] [Space (5)] _BlockFlickerRate ("Flicker Rate (초당 깜빡임 체크 횟수)", Float) = 6
    _BlockFlickerThreshold ("Flicker Threshold (활성 확률 0~1)", Range(0, 1)) = 0.11999999731779099
    _BlockSpeed ("Block Speed (활성 중 격자 해시 갱신 속도)", Float) = 6
    _BlockStrength ("Block Strength (격자 셀 UV 오프셋)", Range(0, 0.30000001192092896)) = 0.05000000074505806
    _BlockScale ("Block Scale (격자 셀 X Y 개수)", Vector) = (8, 4, 0, 0)
}
SubShader
{
    Tags { "CanUseSpriteAtlas" = "true" "IgnoreProjector" = "true" "PreviewType" = "Plane" "Queue" = "Transparent" "RenderType" = "Transparent" }
    Pass
    {
        Name "Default"
        Tags { "CanUseSpriteAtlas" = "true" "IgnoreProjector" = "true" "PreviewType" = "Plane" "Queue" = "Transparent" "RenderType" = "Transparent" }
        Blend SrcAlpha OneMinusSrcAlpha
        ColorMask [_ColorMask]
        ZTest [unity_GUIZTestMode]
        ZWrite Off
        Cull Off
        Stencil {
          Ref [_Stencil]
          ReadMask [_StencilReadMask]
          WriteMask [_StencilWriteMask]
          Comp [_StencilComp]
          Pass [_StencilOp]
        }
        HLSLPROGRAM
        #pragma target 4.0
        #pragma vertex vert
        #pragma fragment frag

        // 3Dmigoto decompiler conventions used by the per-variant code
#define cmp -
#define VD_COL(M, c) (float4((M)[0][c], (M)[1][c], (M)[2][c], (M)[3][c]))
int vd_asint(float v) { return asint(v); } int2 vd_asint(float2 v) { return asint(v); } int3 vd_asint(float3 v) { return asint(v); } int4 vd_asint(float4 v) { return asint(v); }
int vd_asint(uint v) { return asint(v); } int2 vd_asint(uint2 v) { return asint(v); } int3 vd_asint(uint3 v) { return asint(v); } int4 vd_asint(uint4 v) { return asint(v); }
int vd_asint(int v) { return v; } int2 vd_asint(int2 v) { return v; } int3 vd_asint(int3 v) { return v; } int4 vd_asint(int4 v) { return v; }
uint vd_asuint(float v) { return asuint(v); } uint2 vd_asuint(float2 v) { return asuint(v); } uint3 vd_asuint(float3 v) { return asuint(v); } uint4 vd_asuint(float4 v) { return asuint(v); }
uint vd_asuint(int v) { return asuint(v); } uint2 vd_asuint(int2 v) { return asuint(v); } uint3 vd_asuint(int3 v) { return asuint(v); } uint4 vd_asuint(int4 v) { return asuint(v); }
uint vd_asuint(uint v) { return v; } uint2 vd_asuint(uint2 v) { return v; } uint3 vd_asuint(uint3 v) { return v; } uint4 vd_asuint(uint4 v) { return v; }
float vd_asfloat(int v) { return asfloat(v); } float2 vd_asfloat(int2 v) { return asfloat(v); } float3 vd_asfloat(int3 v) { return asfloat(v); } float4 vd_asfloat(int4 v) { return asfloat(v); }
float vd_asfloat(uint v) { return asfloat(v); } float2 vd_asfloat(uint2 v) { return asfloat(v); } float3 vd_asfloat(uint3 v) { return asfloat(v); } float4 vd_asfloat(uint4 v) { return asfloat(v); }
float vd_asfloat(float v) { return v; } float2 vd_asfloat(float2 v) { return v; } float3 vd_asfloat(float3 v) { return v; } float4 vd_asfloat(float4 v) { return v; }

        cbuffer UnityPerCamera
        {
            float4 _Time : packoffset(c0);
        }
        cbuffer UnityPerDraw
        {
            float4x4 unity_ObjectToWorld : packoffset(c0);
        }
        cbuffer UnityPerFrame
        {
            float4x4 unity_MatrixVP : packoffset(c17);
        }
        cbuffer UnityPerMaterial
        {
            float4 _MainTex_ST : packoffset(c1);
            float _SliceFlickerRate : packoffset(c2);
            float _SliceFlickerThreshold : packoffset(c2.y);
            float _GlitchStrength : packoffset(c2.z);
            float _GlitchBandScale : packoffset(c2.w);
            float _GlitchStepRate : packoffset(c3);
            float _ChromaStrength : packoffset(c3.y);
            float _JitterFlickerRate : packoffset(c3.z);
            float _JitterFlickerThreshold : packoffset(c3.w);
            float _JitterSpeed : packoffset(c4);
            float _VertexJitterStrength : packoffset(c4.y);
            float _WarpFlickerRate : packoffset(c4.z);
            float _WarpFlickerThreshold : packoffset(c4.w);
            float _WarpSpeed : packoffset(c5);
            float _WarpStrength : packoffset(c5.y);
            float _WarpScale : packoffset(c5.z);
            float _BlockFlickerRate : packoffset(c5.w);
            float _BlockFlickerThreshold : packoffset(c6);
            float _BlockSpeed : packoffset(c6.y);
            float _BlockStrength : packoffset(c6.z);
            float4 _BlockScale : packoffset(c7);
        }

        Texture2D<float4> _MainTex;
        SamplerState sampler_MainTex;

        // ---- progVertex
        #if 1
        #include "LCArt_UI_Shader_DE_UI_TITLE_Glitch/p0_vs_1.hlsl"
        #endif
        // ---- progFragment
        #if 1
        #include "LCArt_UI_Shader_DE_UI_TITLE_Glitch/p0_ps_3.hlsl"
        #endif
        ENDHLSL
    }
}
}
