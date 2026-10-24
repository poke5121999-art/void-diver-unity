Shader "LCArt/WorldUI/Shader_DE_WorldUI_DropItem"
{
Properties
{
    [PerRendererData] _MainTex ("Sprite Texture", 2D) = "white" {}
    _Color ("Tint", Color) = (1, 1, 1, 1)
    _StencilComp ("Stencil Comparison", Float) = 8
    _Stencil ("Stencil ID", Float) = 0
    _StencilOp ("Stencil Operation", Float) = 0
    _StencilWriteMask ("Stencil Write Mask", Float) = 255
    _StencilReadMask ("Stencil Read Mask", Float) = 255
    _ColorMask ("Color Mask", Float) = 15
    [Toggle(UNITY_UI_ALPHACLIP)] _UseUIAlphaClip ("Use Alpha Clip", Float) = 0
    [HideInInspector] _OutlineColor ("Outline Color", Color) = (1, 1, 1, 1)
    [HideInInspector] _OutlineEnable ("Outline Enable", Float) = 0
    [HideInInspector] _OutlineStroke ("Outline Stroke", Float) = 0.125
}
SubShader
{
    Tags { "CanUseSpriteAtlas" = "true" "IgnoreProjector" = "true" "PreviewType" = "Plane" "Queue" = "Transparent" "RenderPipeline" = "UniversalPipeline" "RenderType" = "Transparent" }
    Pass
    {
        Name "Forward"
        Tags { "CanUseSpriteAtlas" = "true" "IgnoreProjector" = "true" "LightMode" = "UniversalForward" "PreviewType" = "Plane" "Queue" = "Transparent" "RenderPipeline" = "UniversalPipeline" "RenderType" = "Transparent" }
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
        #pragma multi_compile _ _ADDITIONAL_LIGHTS
        #pragma multi_compile _ _CLUSTER_LIGHT_LOOP

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
        float4 _ScaledScreenParams;
        float2 _GlobalMipBias;
        float4 _MainLightPosition;
        float4 _MainLightColor;
        float4 _AdditionalLightsCount;
        float4 _FPParams0;
        float4 _FPParams1;
        float4 _FPParams2;
        float3 _WorldSpaceCameraPos;
        float4 unity_OrthoParams;
        float4 _ScaleBiasRt;
        float4x4 unity_MatrixV;
        float4x4 unity_MatrixVP;
        cbuffer AdditionalLights
        {
            float4 _AdditionalLightsPosition[256] : packoffset(c0);
            float4 _AdditionalLightsColor[256] : packoffset(c256);
            float4 _AdditionalLightsAttenuation[256] : packoffset(c512);
            float4 _AdditionalLightsSpotDir[256] : packoffset(c768);
        }
        cbuffer UnityPerDraw
        {
            float4x4 unity_ObjectToWorld : packoffset(c0);
            float4 unity_LightData : packoffset(c11);
            float4 unity_LightIndices[2] : packoffset(c12);
        }
        cbuffer UnityPerMaterial
        {
            float4 _MainTex_ST : packoffset(c0);
            float4 _Color : packoffset(c2);
        }
        cbuffer urp_TileBuffer
        {
            float4 urp_Tiles[4096] : packoffset(c0);
        }
        cbuffer urp_ZBinBuffer
        {
            float4 urp_ZBins[1024] : packoffset(c0);
        }

        Texture2D<float4> _MainTex;
        SamplerState sampler_MainTex;

        // ---- progVertex
        #if defined(UNITY_UI_ALPHACLIP) && defined(UNITY_UI_CLIP_RECT) && defined(_ADDITIONAL_LIGHTS)
        #include "LCArt_WorldUI_Shader_DE_WorldUI_DropItem/p0_vs_2.hlsl"
        #elif defined(UNITY_UI_ALPHACLIP) && defined(UNITY_UI_CLIP_RECT) && defined(_CLUSTER_LIGHT_LOOP)
        #include "LCArt_WorldUI_Shader_DE_WorldUI_DropItem/p0_vs_9.hlsl"
        #elif defined(UNITY_UI_ALPHACLIP) && defined(_ADDITIONAL_LIGHTS)
        #include "LCArt_WorldUI_Shader_DE_WorldUI_DropItem/p0_vs_4.hlsl"
        #elif defined(UNITY_UI_ALPHACLIP) && defined(_CLUSTER_LIGHT_LOOP)
        #include "LCArt_WorldUI_Shader_DE_WorldUI_DropItem/p0_vs_7.hlsl"
        #elif defined(UNITY_UI_CLIP_RECT) && defined(_ADDITIONAL_LIGHTS)
        #include "LCArt_WorldUI_Shader_DE_WorldUI_DropItem/p0_vs_3.hlsl"
        #elif defined(UNITY_UI_CLIP_RECT) && defined(_CLUSTER_LIGHT_LOOP)
        #include "LCArt_WorldUI_Shader_DE_WorldUI_DropItem/p0_vs_8.hlsl"
        #elif defined(_ADDITIONAL_LIGHTS)
        #include "LCArt_WorldUI_Shader_DE_WorldUI_DropItem/p0_vs_5.hlsl"
        #elif defined(_CLUSTER_LIGHT_LOOP)
        #include "LCArt_WorldUI_Shader_DE_WorldUI_DropItem/p0_vs_6.hlsl"
        #else
        #include "LCArt_WorldUI_Shader_DE_WorldUI_DropItem/p0_vs_6.hlsl"
        #endif
        // ---- progFragment
        #if defined(UNITY_UI_ALPHACLIP) && defined(UNITY_UI_CLIP_RECT) && defined(_ADDITIONAL_LIGHTS)
        #include "LCArt_WorldUI_Shader_DE_WorldUI_DropItem/p0_ps_12.hlsl"
        #elif defined(UNITY_UI_ALPHACLIP) && defined(UNITY_UI_CLIP_RECT) && defined(_CLUSTER_LIGHT_LOOP)
        #include "LCArt_WorldUI_Shader_DE_WorldUI_DropItem/p0_ps_19.hlsl"
        #elif defined(UNITY_UI_ALPHACLIP) && defined(_ADDITIONAL_LIGHTS)
        #include "LCArt_WorldUI_Shader_DE_WorldUI_DropItem/p0_ps_14.hlsl"
        #elif defined(UNITY_UI_ALPHACLIP) && defined(_CLUSTER_LIGHT_LOOP)
        #include "LCArt_WorldUI_Shader_DE_WorldUI_DropItem/p0_ps_17.hlsl"
        #elif defined(UNITY_UI_CLIP_RECT) && defined(_ADDITIONAL_LIGHTS)
        #include "LCArt_WorldUI_Shader_DE_WorldUI_DropItem/p0_ps_13.hlsl"
        #elif defined(UNITY_UI_CLIP_RECT) && defined(_CLUSTER_LIGHT_LOOP)
        #include "LCArt_WorldUI_Shader_DE_WorldUI_DropItem/p0_ps_18.hlsl"
        #elif defined(_ADDITIONAL_LIGHTS)
        #include "LCArt_WorldUI_Shader_DE_WorldUI_DropItem/p0_ps_15.hlsl"
        #elif defined(_CLUSTER_LIGHT_LOOP)
        #include "LCArt_WorldUI_Shader_DE_WorldUI_DropItem/p0_ps_16.hlsl"
        #else
        #include "LCArt_WorldUI_Shader_DE_WorldUI_DropItem/p0_ps_16.hlsl"
        #endif
        ENDHLSL
    }
    Pass
    {
        Name "Outline"
        Tags { "CanUseSpriteAtlas" = "true" "IgnoreProjector" = "true" "LightMode" = "ItemOutline" "PreviewType" = "Plane" "Queue" = "Transparent" "RenderPipeline" = "UniversalPipeline" "RenderType" = "Transparent" }
        Blend SrcAlpha OneMinusSrcAlpha
        ColorMask [_ColorMask]
        ZTest Always
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

        // $Globals: bound per variable by name
        float2 _GlobalMipBias;
        float4x4 unity_MatrixVP;
        cbuffer UnityPerDraw
        {
            float4x4 unity_ObjectToWorld : packoffset(c0);
        }
        cbuffer UnityPerMaterial
        {
            float4 _MainTex_ST : packoffset(c0);
            float4 _MainTex_TexelSize : packoffset(c1);
            float4 _OutlineColor : packoffset(c3);
            float _OutlineEnable : packoffset(c4);
            float _OutlineStroke : packoffset(c4.y);
        }

        Texture2D<float4> _MainTex;
        SamplerState sampler_MainTex;

        // ---- progVertex
        #if 1
        #include "LCArt_WorldUI_Shader_DE_WorldUI_DropItem/p1_vs_21.hlsl"
        #endif
        // ---- progFragment
        #if 1
        #include "LCArt_WorldUI_Shader_DE_WorldUI_DropItem/p1_ps_23.hlsl"
        #endif
        ENDHLSL
    }
}
}
