Shader "LCArt/UI/Shader_DE_UI_Gamma"
{
Properties
{
    [PerRendererData] _MainTex ("Sprite Texture", 2D) = "white" {}
    _Color ("Tint", Color) = (1, 1, 1, 1)
    _GammaDegree ("Gamma Degree", Float) = 0
    [HideInInspector] _StencilComp ("Stencil Comparison", Float) = 8
    [HideInInspector] _Stencil ("Stencil ID", Float) = 0
    [HideInInspector] _StencilOp ("Stencil Operation", Float) = 0
    [HideInInspector] _StencilWriteMask ("Stencil Write Mask", Float) = 255
    [HideInInspector] _StencilReadMask ("Stencil Read Mask", Float) = 255
    [HideInInspector] _ColorMask ("Color Mask", Float) = 15
    [HideInInspector] [Toggle(UNITY_UI_ALPHACLIP)] _UseUIAlphaClip ("Use Alpha Clip", Float) = 0
}
SubShader
{
    Tags { "CanUseSpriteAtlas" = "true" "IgnoreProjector" = "true" "PreviewType" = "Plane" "Queue" = "Transparent" "RenderPipeline" = "UniversalPipeline" "RenderType" = "Transparent" }
    Pass
    {
        Name "Default"
        Tags { "CanUseSpriteAtlas" = "true" "IgnoreProjector" = "true" "PreviewType" = "Plane" "Queue" = "Transparent" "RenderPipeline" = "UniversalPipeline" "RenderType" = "Transparent" }
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
        #pragma multi_compile_local _ UNITY_UI_ALPHACLIP
        #pragma multi_compile_local _ UNITY_UI_CLIP_RECT

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

        // $Globals: bound per variable by name
        float2 _GlobalMipBias;
        float4x4 unity_MatrixVP;
        float4 _TextureSampleAdd;
        float4 _ClipRect;
        cbuffer UnityPerDraw
        {
            float4x4 unity_ObjectToWorld : packoffset(c0);
        }
        cbuffer UnityPerMaterial
        {
            float4 _Color : packoffset(c0);
            float _GammaDegree : packoffset(c1);
        }

        Texture2D<float4> _MainTex;
        SamplerState sampler_MainTex;

        // ---- progVertex
        #if defined(UNITY_UI_ALPHACLIP) && defined(UNITY_UI_CLIP_RECT)
        #include "LCArt_UI_Shader_DE_UI_Gamma/p0_vs_4.hlsl"
        #elif defined(UNITY_UI_ALPHACLIP)
        #include "LCArt_UI_Shader_DE_UI_Gamma/p0_vs_2.hlsl"
        #elif defined(UNITY_UI_CLIP_RECT)
        #include "LCArt_UI_Shader_DE_UI_Gamma/p0_vs_3.hlsl"
        #elif 1
        #include "LCArt_UI_Shader_DE_UI_Gamma/p0_vs_1.hlsl"
        #endif
        // ---- progFragment
        #if defined(UNITY_UI_ALPHACLIP) && defined(UNITY_UI_CLIP_RECT)
        #include "LCArt_UI_Shader_DE_UI_Gamma/p0_ps_10.hlsl"
        #elif defined(UNITY_UI_ALPHACLIP)
        #include "LCArt_UI_Shader_DE_UI_Gamma/p0_ps_8.hlsl"
        #elif defined(UNITY_UI_CLIP_RECT)
        #include "LCArt_UI_Shader_DE_UI_Gamma/p0_ps_9.hlsl"
        #elif 1
        #include "LCArt_UI_Shader_DE_UI_Gamma/p0_ps_7.hlsl"
        #endif
        ENDHLSL
    }
}
}
