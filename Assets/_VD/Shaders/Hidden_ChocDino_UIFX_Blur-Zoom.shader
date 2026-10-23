Shader "Hidden/ChocDino/UIFX/Blur-Zoom"
{
Properties
{
    [PerRendererData] _MainTex ("Sprite Texture", 2D) = "white" {}
    _CenterInvScale ("Center InvScale", Vector) = (0, 0, 0, 0)
    _Dither ("Dither", Float) = 0
    _WeightsPower ("Weights Power", Float) = 1
}
SubShader
{
    Tags { "CanUseSpriteAtlas" = "true" "IgnoreProjector" = "true" "OutputsPremultipliedAlpha" = "true" "PreviewType" = "Plane" "Queue" = "Transparent" "RenderType" = "Transparent" }
    Pass
    {
        Name "Blur-Zoom"
        Tags { "CanUseSpriteAtlas" = "true" "IgnoreProjector" = "true" "OutputsPremultipliedAlpha" = "true" "PreviewType" = "Plane" "Queue" = "Transparent" "RenderType" = "Transparent" }
        ZTest Always
        ZWrite Off
        Cull Off
        HLSLPROGRAM
        #pragma target 4.0
        #pragma vertex vert
        #pragma fragment frag
        #pragma multi_compile_local _ DIR_BOTH
        #pragma multi_compile_local _ USE_DITHER

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
        float4 _MainTex_ST;
        float4 _MainTex_TexelSize;
        float3 _CenterInvScale;
        float _Dither;
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

        Texture2D<float4> _MainTex;
        SamplerState sampler_MainTex;

        // ---- progVertex
        #if defined(DIR_BOTH) && defined(USE_DITHER)
        #include "Hidden_ChocDino_UIFX_Blur-Zoom/p0_vs_4.hlsl"
        #elif defined(DIR_BOTH)
        #include "Hidden_ChocDino_UIFX_Blur-Zoom/p0_vs_2.hlsl"
        #elif defined(USE_DITHER)
        #include "Hidden_ChocDino_UIFX_Blur-Zoom/p0_vs_3.hlsl"
        #elif 1
        #include "Hidden_ChocDino_UIFX_Blur-Zoom/p0_vs_1.hlsl"
        #endif
        // ---- progFragment
        #if defined(DIR_BOTH) && defined(USE_DITHER)
        #include "Hidden_ChocDino_UIFX_Blur-Zoom/p0_ps_10.hlsl"
        #elif defined(DIR_BOTH)
        #include "Hidden_ChocDino_UIFX_Blur-Zoom/p0_ps_8.hlsl"
        #elif defined(USE_DITHER)
        #include "Hidden_ChocDino_UIFX_Blur-Zoom/p0_ps_9.hlsl"
        #elif 1
        #include "Hidden_ChocDino_UIFX_Blur-Zoom/p0_ps_7.hlsl"
        #endif
        ENDHLSL
    }
    Pass
    {
        Name "Blur-Zoom-Weighted"
        Tags { "CanUseSpriteAtlas" = "true" "IgnoreProjector" = "true" "OutputsPremultipliedAlpha" = "true" "PreviewType" = "Plane" "Queue" = "Transparent" "RenderType" = "Transparent" }
        ZTest Always
        ZWrite Off
        Cull Off
        HLSLPROGRAM
        #pragma target 4.0
        #pragma vertex vert
        #pragma fragment frag
        #pragma multi_compile_local _ DIR_BOTH
        #pragma multi_compile_local _ USE_DITHER

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
        float4 _MainTex_ST;
        float4 _MainTex_TexelSize;
        float3 _CenterInvScale;
        float _Dither;
        float _WeightsPower;
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

        Texture2D<float4> _MainTex;
        SamplerState sampler_MainTex;

        // ---- progVertex
        #if defined(DIR_BOTH) && defined(USE_DITHER)
        #include "Hidden_ChocDino_UIFX_Blur-Zoom/p1_vs_4.hlsl"
        #elif defined(DIR_BOTH)
        #include "Hidden_ChocDino_UIFX_Blur-Zoom/p1_vs_2.hlsl"
        #elif defined(USE_DITHER)
        #include "Hidden_ChocDino_UIFX_Blur-Zoom/p1_vs_3.hlsl"
        #elif 1
        #include "Hidden_ChocDino_UIFX_Blur-Zoom/p1_vs_1.hlsl"
        #endif
        // ---- progFragment
        #if defined(DIR_BOTH) && defined(USE_DITHER)
        #include "Hidden_ChocDino_UIFX_Blur-Zoom/p1_ps_14.hlsl"
        #elif defined(DIR_BOTH)
        #include "Hidden_ChocDino_UIFX_Blur-Zoom/p1_ps_12.hlsl"
        #elif defined(USE_DITHER)
        #include "Hidden_ChocDino_UIFX_Blur-Zoom/p1_ps_13.hlsl"
        #elif 1
        #include "Hidden_ChocDino_UIFX_Blur-Zoom/p1_ps_11.hlsl"
        #endif
        ENDHLSL
    }
}
}
