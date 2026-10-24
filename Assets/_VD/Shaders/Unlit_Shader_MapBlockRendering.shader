Shader "Unlit/Shader_MapBlockRendering"
{
Properties
{
    [MainTexture] _BaseMap ("Texture", 2D) = "white" {}
    [MainColor] _BaseColor ("Color", Color) = (1, 1, 1, 1)
    _Cutoff ("AlphaCutout", Range(0, 1)) = 0.5
    _Surface ("__surface", Float) = 0
    _Blend ("__mode", Float) = 0
    _Cull ("__cull", Float) = 2
    [ToggleUI] _AlphaClip ("__clip", Float) = 0
    [HideInInspector] _BlendOp ("__blendop", Float) = 0
    [HideInInspector] _SrcBlend ("__src", Float) = 1
    [HideInInspector] _DstBlend ("__dst", Float) = 0
    [HideInInspector] _SrcBlendAlpha ("__srcA", Float) = 1
    [HideInInspector] _DstBlendAlpha ("__dstA", Float) = 0
    [HideInInspector] _ZWrite ("__zw", Float) = 1
    [HideInInspector] _AlphaToMask ("__alphaToMask", Float) = 0
    _QueueOffset ("Queue offset", Float) = 0
    [HideInInspector] _MainTex ("BaseMap", 2D) = "white" {}
    [HideInInspector] _Color ("Base Color", Color) = (0.5, 0.5, 0.5, 1)
    [HideInInspector] _SampleGI ("SampleGI", Float) = 0
}
SubShader
{
    LOD 100
    Tags { "IgnoreProjector" = "true" "RenderPipeline" = "UniversalPipeline" "RenderType" = "Opaque" "UniversalMaterialType" = "Unlit" }
    Pass
    {
        Name "Unlit"
        Tags { "IgnoreProjector" = "true" "RenderPipeline" = "UniversalPipeline" "RenderType" = "Opaque" "UniversalMaterialType" = "Unlit" }
        Blend One One
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

        // $Globals: bound per variable by name
        float4x4 unity_MatrixVP;
        cbuffer UnityPerDraw
        {
            float4x4 unity_ObjectToWorld : packoffset(c0);
        }


        // ---- progVertex
        #if 1
        #include "Unlit_Shader_MapBlockRendering/p0_vs_1.hlsl"
        #endif
        // ---- progFragment
        #if 1
        #include "Unlit_Shader_MapBlockRendering/p0_ps_3.hlsl"
        #endif
        ENDHLSL
    }
}
}
