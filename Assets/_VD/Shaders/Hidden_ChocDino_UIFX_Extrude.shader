Shader "Hidden/ChocDino/UIFX/Extrude"
{
Properties
{
    [PerRendererData] _MainTex ("Sprite Texture", 2D) = "white" {}
    [PerRendererData] _GradientTex ("Gradient Texture", 2D) = "grey" {}
    _Length ("Length", Float) = 32
    _PixelStep ("Pixel Step", Vector) = (0.009999999776482582, 0.009999999776482582, 0, 0)
    _VanishingPoint ("Vanishing Point", Vector) = (0.5, 0.5, 0, 0)
    _Ratio ("Ratio", Vector) = (1, 1, 0, 0)
    _ColorFront ("Color Front", Vector) = (0.5, 0.5, 0.5, 1)
    _ColorBack ("Color Back", Vector) = (0, 0, 0, 0)
    _ReverseFill ("Reverse Fill", Float) = 0
    _Scroll ("Scroll", Float) = 0
}
SubShader
{
    Tags { "CanUseSpriteAtlas" = "true" "IgnoreProjector" = "true" "OutputsPremultipliedAlpha" = "true" "PreviewType" = "Plane" "Queue" = "Transparent" "RenderType" = "Transparent" }
    Pass
    {
        Name "Extrude-Perspective"
        Tags { "CanUseSpriteAtlas" = "true" "IgnoreProjector" = "true" "OutputsPremultipliedAlpha" = "true" "PreviewType" = "Plane" "Queue" = "Transparent" "RenderType" = "Transparent" }
        ZTest Always
        ZWrite Off
        Cull Off
        HLSLPROGRAM
        #pragma target 4.0
        #pragma vertex vert
        #pragma fragment frag
        #pragma multi_compile_local _ MULTIPLY_SOURCE_COLOR
        #pragma multi_compile_local _ USE_GRADIENT_TEXTURE

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
        float4 _MainTex_TexelSize;
        int _Length;
        float2 _VanishingPoint;
        float3 _Ratio;
        float4 _ColorFront;
        float4 _ColorBack;
        float _ReverseFill;
        float _Scroll;
        cbuffer UnityPerDraw
        {
            float4x4 unity_ObjectToWorld : packoffset(c0);
        }
        cbuffer UnityPerFrame
        {
            float4x4 unity_MatrixVP : packoffset(c17);
        }

        Texture2D<float4> _GradientTex;
        Texture2D<float4> _MainTex;
        SamplerState sampler_GradientTex;
        SamplerState sampler_MainTex;

        // ---- progVertex
        #if defined(MULTIPLY_SOURCE_COLOR) && defined(USE_GRADIENT_TEXTURE)
        #include "Hidden_ChocDino_UIFX_Extrude/p0_vs_4.hlsl"
        #elif defined(MULTIPLY_SOURCE_COLOR)
        #include "Hidden_ChocDino_UIFX_Extrude/p0_vs_2.hlsl"
        #elif defined(USE_GRADIENT_TEXTURE)
        #include "Hidden_ChocDino_UIFX_Extrude/p0_vs_3.hlsl"
        #elif 1
        #include "Hidden_ChocDino_UIFX_Extrude/p0_vs_1.hlsl"
        #endif
        // ---- progFragment
        #if defined(MULTIPLY_SOURCE_COLOR) && defined(USE_GRADIENT_TEXTURE)
        #include "Hidden_ChocDino_UIFX_Extrude/p0_ps_10.hlsl"
        #elif defined(MULTIPLY_SOURCE_COLOR)
        #include "Hidden_ChocDino_UIFX_Extrude/p0_ps_8.hlsl"
        #elif defined(USE_GRADIENT_TEXTURE)
        #include "Hidden_ChocDino_UIFX_Extrude/p0_ps_9.hlsl"
        #elif 1
        #include "Hidden_ChocDino_UIFX_Extrude/p0_ps_7.hlsl"
        #endif
        ENDHLSL
    }
    Pass
    {
        Name "Extrude-Orthographic"
        Tags { "CanUseSpriteAtlas" = "true" "IgnoreProjector" = "true" "OutputsPremultipliedAlpha" = "true" "PreviewType" = "Plane" "Queue" = "Transparent" "RenderType" = "Transparent" }
        ZTest Always
        ZWrite Off
        Cull Off
        HLSLPROGRAM
        #pragma target 4.0
        #pragma vertex vert
        #pragma fragment frag
        #pragma multi_compile_local _ MULTIPLY_SOURCE_COLOR
        #pragma multi_compile_local _ USE_GRADIENT_TEXTURE

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
        int _Length;
        float2 _PixelStep;
        float4 _ColorFront;
        float4 _ColorBack;
        float _ReverseFill;
        float _Scroll;
        cbuffer UnityPerDraw
        {
            float4x4 unity_ObjectToWorld : packoffset(c0);
        }
        cbuffer UnityPerFrame
        {
            float4x4 unity_MatrixVP : packoffset(c17);
        }

        Texture2D<float4> _GradientTex;
        Texture2D<float4> _MainTex;
        SamplerState sampler_GradientTex;
        SamplerState sampler_MainTex;

        // ---- progVertex
        #if defined(MULTIPLY_SOURCE_COLOR) && defined(USE_GRADIENT_TEXTURE)
        #include "Hidden_ChocDino_UIFX_Extrude/p1_vs_4.hlsl"
        #elif defined(MULTIPLY_SOURCE_COLOR)
        #include "Hidden_ChocDino_UIFX_Extrude/p1_vs_2.hlsl"
        #elif defined(USE_GRADIENT_TEXTURE)
        #include "Hidden_ChocDino_UIFX_Extrude/p1_vs_3.hlsl"
        #elif 1
        #include "Hidden_ChocDino_UIFX_Extrude/p1_vs_1.hlsl"
        #endif
        // ---- progFragment
        #if defined(MULTIPLY_SOURCE_COLOR) && defined(USE_GRADIENT_TEXTURE)
        #include "Hidden_ChocDino_UIFX_Extrude/p1_ps_14.hlsl"
        #elif defined(MULTIPLY_SOURCE_COLOR)
        #include "Hidden_ChocDino_UIFX_Extrude/p1_ps_12.hlsl"
        #elif defined(USE_GRADIENT_TEXTURE)
        #include "Hidden_ChocDino_UIFX_Extrude/p1_ps_13.hlsl"
        #elif 1
        #include "Hidden_ChocDino_UIFX_Extrude/p1_ps_11.hlsl"
        #endif
        ENDHLSL
    }
}
}
