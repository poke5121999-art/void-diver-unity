Shader "LCArt/UI/Shader_DE_UI_DefaultFrame"
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
    [Header (Frame Setting)] [Space (10)] _FrameInner ("Frame Inner (프레임의 안쪽 범위를 설정합니다.)", Float) = 0
    _FrameOutter ("Frame Outer (프레임의 밖 범위를 설정합니다.)", Float) = 1
    _FrameAlpha ("Frame Alpha (프레임의 투명도를 설정합니다.)", Range(0, 1)) = 1
    [Header (Dissolve Setting)] [Space (10)] _DissolveColor ("Dissolve Color (페이드 전환을 위한 디졸브 및 은은하게 깔리는 색상을 설정합니다.)", Color) = (0, 1, 0, 1)
    _DissolvePatternScale ("Dissolve Pattern Scale (디졸브 랜덤 패턴 역수 크기를 설정합니다.)", Float) = 1
    _DissolveScale ("Dissolve Scale (디졸브 고정 패턴 역수 크기를 설정합니다.)", Vector) = (1, 1, 0, 0)
    [Header (Smoke Setting)] [Space (10)] _SmokeColor ("Smoke Color (연기 색상을 설정합니다.)", Color) = (1, 0, 0, 1)
    _SmokeSpeed ("Smoke Speed (연기 속도를 설정합니다.)", Float) = 1
    _SmokeScale ("Smoke Scale (연기 패턴 역수 크기를 설정합니다. )", Float) = 1
    [Header (Dithering Setting)] [Space (10)] _DitheringColor ("Dithering Color (디졸브 색상을 설정합니다. 연기에 덮어집니다.)", Color) = (0, 0, 0, 1)
    _DitheringSpeed ("Dithering Speed (디졸브 속도를 설정합니다.)", Float) = 1
    _DitheringScale ("Dithering Scale (디졸브 패턴 역수 크기를 설정합니다.)", Float) = 1
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

        // The bytecode is D3D11, so it baked in D3D's screen conventions; lines that differ on OpenGL sit under
// UNITY_UV_STARTS_AT_TOP (uvflip.py). URP's API headers define it for every API but OpenGL Core / GLES3, the same set that
// has UNITY_NEAR_CLIP_VALUE 1.0 (reversed Z); these programs include none of them.
#if !defined(UNITY_UV_STARTS_AT_TOP) && !defined(SHADER_API_GLCORE) && !defined(SHADER_API_GLES3) && !defined(SHADER_API_GLES)
#define UNITY_UV_STARTS_AT_TOP 1
#endif
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
            float4 _DitheringColor : packoffset(c2);
            float _DitheringSpeed : packoffset(c3);
            float _DitheringScale : packoffset(c3.y);
            float4 _SmokeColor : packoffset(c4);
            float _SmokeSpeed : packoffset(c5);
            float _SmokeScale : packoffset(c5.y);
            float4 _DissolveColor : packoffset(c6);
            float _DissolvePatternScale : packoffset(c7);
            float2 _DissolveScale : packoffset(c7.y);
            float _FrameInner : packoffset(c7.w);
            float _FrameOutter : packoffset(c8);
            float _FrameAlpha : packoffset(c8.y);
        }

        Texture2D<float4> _MainTex;
        SamplerState sampler_MainTex;

        // ---- progVertex
        #if 1
        #include "LCArt_UI_Shader_DE_UI_DefaultFrame/p0_vs_1.hlsl"
        #endif
        // ---- progFragment
        #if 1
        #include "LCArt_UI_Shader_DE_UI_DefaultFrame/p0_ps_3.hlsl"
        #endif
        ENDHLSL
    }
}
}
