Shader "LCArt/Temp/Shader_CQ_Background"
{
Properties
{
    [MainTexture] _BaseMap ("Base Map (RGB) Smoothness / Alpha (A)", 2D) = "white" {}
    [MainColor] _BaseColor ("Base Color", Color) = (1, 1, 1, 1)
    _Cutoff ("Alpha Clipping", Range(0, 1)) = 0.5
    _Smoothness ("Smoothness", Range(0, 1)) = 0.5
    _SpecColor ("Specular Color", Color) = (0.5, 0.5, 0.5, 0.5)
    _SpecGlossMap ("Specular Map", 2D) = "white" {}
    _SmoothnessSource ("Smoothness Source", Float) = 0
    _SpecularHighlights ("Specular Highlights", Float) = 1
    [HideInInspector] _BumpScale ("Scale", Float) = 1
    [NoScaleOffset] _BumpMap ("Normal Map", 2D) = "bump" {}
    [HDR] _EmissionColor ("Emission Color", Color) = (0, 0, 0, 1)
    [NoScaleOffset] _EmissionMap ("Emission Map", 2D) = "white" {}
    _Surface ("__surface", Float) = 0
    _Blend ("__blend", Float) = 0
    _Cull ("__cull", Float) = 2
    [ToggleUI] _AlphaClip ("__clip", Float) = 0
    [HideInInspector] _SrcBlend ("__src", Float) = 1
    [HideInInspector] _DstBlend ("__dst", Float) = 0
    [HideInInspector] _SrcBlendAlpha ("__srcA", Float) = 1
    [HideInInspector] _DstBlendAlpha ("__dstA", Float) = 0
    [HideInInspector] _ZWrite ("__zw", Float) = 1
    [HideInInspector] _BlendModePreserveSpecular ("_BlendModePreserveSpecular", Float) = 1
    [HideInInspector] _AlphaToMask ("__alphaToMask", Float) = 0
    [HideInInspector] _AddPrecomputedVelocity ("_AddPrecomputedVelocity", Float) = 0
    [HideInInspector] _XRMotionVectorsPass ("_XRMotionVectorsPass", Float) = 1
    [ToggleUI] _ReceiveShadows ("Receive Shadows", Float) = 1
    _QueueOffset ("Queue offset", Float) = 0
    [HideInInspector] _MainTex ("BaseMap", 2D) = "white" {}
    [HideInInspector] _Color ("Base Color", Color) = (1, 1, 1, 1)
    [HideInInspector] _Shininess ("Smoothness", Float) = 0
    [HideInInspector] _GlossinessSource ("GlossinessSource", Float) = 0
    [HideInInspector] _SpecSource ("SpecularHighlights", Float) = 0
    [HideInInspector] [NoScaleOffset] unity_Lightmaps ("unity_Lightmaps", 2DArray) = "" {}
    [HideInInspector] [NoScaleOffset] unity_LightmapsInd ("unity_LightmapsInd", 2DArray) = "" {}
    [HideInInspector] [NoScaleOffset] unity_ShadowMasks ("unity_ShadowMasks", 2DArray) = "" {}
    [Header (Shade Setting)] _ShadeSmoothStart ("Shade Smooth Step Start", Range(-1, 1)) = 0.10000000149011612
    _ShadeSmoothEnd ("Shade Smooth Step End", Range(-1, 1)) = 0.15000000596046448
    _ShadeAlpha ("Shade Alpha", Range(0, 1)) = 0.25
}
SubShader
{
    LOD 300
    Tags { "IgnoreProjector" = "true" "RenderPipeline" = "UniversalPipeline" "RenderType" = "Opaque" "UniversalMaterialType" = "SimpleLit" }
    Pass
    {
        Name "ForwardLit"
        Tags { "IgnoreProjector" = "true" "LightMode" = "UniversalForward" "RenderPipeline" = "UniversalPipeline" "RenderType" = "Opaque" "UniversalMaterialType" = "SimpleLit" }
        Blend [_SrcBlend] [_DstBlend], [_SrcBlendAlpha] [_DstBlendAlpha]
        AlphaToMask [_AlphaToMask]
        Cull [_Cull]
        HLSLPROGRAM
        #pragma target 4.0
        #pragma vertex vert
        #pragma fragment frag
        #pragma multi_compile _ FOG_LINEAR
        #pragma multi_compile _ _ADDITIONAL_LIGHTS
        #pragma multi_compile _ _CLUSTER_LIGHT_LOOP
        #pragma multi_compile _ _CQ_EDITOR_CAMERA
        #pragma multi_compile _ _DBUFFER_MRT3
        #pragma multi_compile _LIGHT_LAYERS

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
        int _MainLightLayerMask;
        float4 _AdditionalLightsCount;
        float4 _FPParams0;
        float4 _FPParams1;
        float4 _FPParams2;
        float3 _WorldSpaceCameraPos;
        float4 _ProjectionParams;
        float4 unity_OrthoParams;
        float4 _ScaleBiasRt;
        float4 unity_FogParams;
        float4 unity_FogColor;
        float4x4 unity_MatrixV;
        float4x4 unity_MatrixVP;
        cbuffer AdditionalLights
        {
            float4 _AdditionalLightsPosition[256] : packoffset(c0);
            float4 _AdditionalLightsColor[256] : packoffset(c256);
            float4 _AdditionalLightsAttenuation[256] : packoffset(c512);
            float4 _AdditionalLightsSpotDir[256] : packoffset(c768);
            float _AdditionalLightsLayerMasks[256] : packoffset(c1280);
        }
        cbuffer UnityPerDraw
        {
            float4x4 unity_ObjectToWorld : packoffset(c0);
            float4x4 unity_WorldToObject : packoffset(c4);
            float4 unity_RenderingLayer : packoffset(c10);
            float4 unity_LightData : packoffset(c11);
            float4 unity_LightIndices[2] : packoffset(c12);
            float4 unity_SHAr : packoffset(c27);
            float4 unity_SHAg : packoffset(c28);
            float4 unity_SHAb : packoffset(c29);
            float4 unity_SHBr : packoffset(c30);
            float4 unity_SHBg : packoffset(c31);
            float4 unity_SHBb : packoffset(c32);
            float4 unity_SHC : packoffset(c33);
        }
        cbuffer UnityPerMaterial
        {
            float4 _BaseMap_ST : packoffset(c0);
            float4 _BaseColor : packoffset(c2);
            float _Surface : packoffset(c5.y);
            float _ShadeSmoothStart : packoffset(c10);
            float _ShadeSmoothEnd : packoffset(c10.y);
            float _ShadeAlpha : packoffset(c10.z);
        }
        cbuffer urp_TileBuffer
        {
            float4 urp_Tiles[4096] : packoffset(c0);
        }
        cbuffer urp_ZBinBuffer
        {
            float4 urp_ZBins[1024] : packoffset(c0);
        }

        Texture2D<float4> _BaseMap;
        Texture2D<float4> _DBufferTexture0;
        Texture2D<float4> _DBufferTexture1;
        Texture2D<float4> _DBufferTexture2;
        SamplerState sampler_BaseMap;

        // ---- progVertex
        #if defined(FOG_LINEAR) && defined(_ADDITIONAL_LIGHTS) && defined(_CQ_EDITOR_CAMERA) && defined(_LIGHT_LAYERS)
        #include "LCArt_Temp_Shader_CQ_Background/p0_vs_2.hlsl"
        #elif defined(FOG_LINEAR) && defined(_CLUSTER_LIGHT_LOOP) && defined(_CQ_EDITOR_CAMERA) && defined(_LIGHT_LAYERS)
        #include "LCArt_Temp_Shader_CQ_Background/p0_vs_9.hlsl"
        #elif defined(FOG_LINEAR) && defined(_ADDITIONAL_LIGHTS) && defined(_LIGHT_LAYERS)
        #include "LCArt_Temp_Shader_CQ_Background/p0_vs_4.hlsl"
        #elif defined(FOG_LINEAR) && defined(_CLUSTER_LIGHT_LOOP) && defined(_LIGHT_LAYERS)
        #include "LCArt_Temp_Shader_CQ_Background/p0_vs_7.hlsl"
        #elif defined(_ADDITIONAL_LIGHTS) && defined(_CQ_EDITOR_CAMERA) && defined(_LIGHT_LAYERS)
        #include "LCArt_Temp_Shader_CQ_Background/p0_vs_3.hlsl"
        #elif defined(_CLUSTER_LIGHT_LOOP) && defined(_CQ_EDITOR_CAMERA) && defined(_LIGHT_LAYERS)
        #include "LCArt_Temp_Shader_CQ_Background/p0_vs_8.hlsl"
        #elif defined(_ADDITIONAL_LIGHTS) && defined(_LIGHT_LAYERS)
        #include "LCArt_Temp_Shader_CQ_Background/p0_vs_5.hlsl"
        #elif defined(_CLUSTER_LIGHT_LOOP) && defined(_LIGHT_LAYERS)
        #include "LCArt_Temp_Shader_CQ_Background/p0_vs_6.hlsl"
        #else
        #include "LCArt_Temp_Shader_CQ_Background/p0_vs_6.hlsl"
        #endif
        // ---- progFragment
        #if defined(FOG_LINEAR) && defined(_CLUSTER_LIGHT_LOOP) && defined(_CQ_EDITOR_CAMERA) && defined(_DBUFFER_MRT3) && defined(_LIGHT_LAYERS)
        #include "LCArt_Temp_Shader_CQ_Background/p0_ps_16.hlsl"
        #elif defined(FOG_LINEAR) && defined(_ADDITIONAL_LIGHTS) && defined(_CQ_EDITOR_CAMERA) && defined(_LIGHT_LAYERS)
        #include "LCArt_Temp_Shader_CQ_Background/p0_ps_24.hlsl"
        #elif defined(FOG_LINEAR) && defined(_CLUSTER_LIGHT_LOOP) && defined(_CQ_EDITOR_CAMERA) && defined(_LIGHT_LAYERS)
        #include "LCArt_Temp_Shader_CQ_Background/p0_ps_23.hlsl"
        #elif defined(FOG_LINEAR) && defined(_CLUSTER_LIGHT_LOOP) && defined(_DBUFFER_MRT3) && defined(_LIGHT_LAYERS)
        #include "LCArt_Temp_Shader_CQ_Background/p0_ps_18.hlsl"
        #elif defined(_CLUSTER_LIGHT_LOOP) && defined(_CQ_EDITOR_CAMERA) && defined(_DBUFFER_MRT3) && defined(_LIGHT_LAYERS)
        #include "LCArt_Temp_Shader_CQ_Background/p0_ps_17.hlsl"
        #elif defined(FOG_LINEAR) && defined(_ADDITIONAL_LIGHTS) && defined(_LIGHT_LAYERS)
        #include "LCArt_Temp_Shader_CQ_Background/p0_ps_26.hlsl"
        #elif defined(FOG_LINEAR) && defined(_CLUSTER_LIGHT_LOOP) && defined(_LIGHT_LAYERS)
        #include "LCArt_Temp_Shader_CQ_Background/p0_ps_21.hlsl"
        #elif defined(_ADDITIONAL_LIGHTS) && defined(_CQ_EDITOR_CAMERA) && defined(_LIGHT_LAYERS)
        #include "LCArt_Temp_Shader_CQ_Background/p0_ps_25.hlsl"
        #elif defined(_CLUSTER_LIGHT_LOOP) && defined(_CQ_EDITOR_CAMERA) && defined(_LIGHT_LAYERS)
        #include "LCArt_Temp_Shader_CQ_Background/p0_ps_22.hlsl"
        #elif defined(_CLUSTER_LIGHT_LOOP) && defined(_DBUFFER_MRT3) && defined(_LIGHT_LAYERS)
        #include "LCArt_Temp_Shader_CQ_Background/p0_ps_19.hlsl"
        #elif defined(_ADDITIONAL_LIGHTS) && defined(_LIGHT_LAYERS)
        #include "LCArt_Temp_Shader_CQ_Background/p0_ps_27.hlsl"
        #elif defined(_CLUSTER_LIGHT_LOOP) && defined(_LIGHT_LAYERS)
        #include "LCArt_Temp_Shader_CQ_Background/p0_ps_20.hlsl"
        #else
        #include "LCArt_Temp_Shader_CQ_Background/p0_ps_20.hlsl"
        #endif
        ENDHLSL
    }
    Pass
    {
        Name "DepthNormals"
        Tags { "IgnoreProjector" = "true" "LightMode" = "DepthNormals" "RenderPipeline" = "UniversalPipeline" "RenderType" = "Opaque" "UniversalMaterialType" = "SimpleLit" }
        Cull [_Cull]
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

        // $Globals: bound per variable by name
        float4x4 unity_MatrixVP;
        cbuffer UnityPerDraw
        {
            float4x4 unity_ObjectToWorld : packoffset(c0);
            float4x4 unity_WorldToObject : packoffset(c4);
        }


        // ---- progVertex
        #if 1
        #include "LCArt_Temp_Shader_CQ_Background/p1_vs_29.hlsl"
        #endif
        // ---- progFragment
        #if 1
        #include "LCArt_Temp_Shader_CQ_Background/p1_ps_31.hlsl"
        #endif
        ENDHLSL
    }
}
Fallback "Hidden/Universal Render Pipeline/FallbackError"
}
