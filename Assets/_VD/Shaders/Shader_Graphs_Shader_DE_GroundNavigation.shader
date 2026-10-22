Shader "Shader Graphs/Shader_DE_GroundNavigation"
{
Properties
{
    [NoScaleOffset] _MainTex ("_MainTex", 2D) = "white" {}
    _Color ("_Color", Color) = (1, 1, 1, 1)
    _Speed ("Speed", Float) = 0
    [HideInInspector] _QueueOffset ("_QueueOffset", Float) = 0
    [HideInInspector] _QueueControl ("_QueueControl", Float) = -1
    [HideInInspector] [NoScaleOffset] unity_Lightmaps ("unity_Lightmaps", 2DArray) = "" {}
    [HideInInspector] [NoScaleOffset] unity_LightmapsInd ("unity_LightmapsInd", 2DArray) = "" {}
    [HideInInspector] [NoScaleOffset] unity_ShadowMasks ("unity_ShadowMasks", 2DArray) = "" {}
}
SubShader
{
    Tags { "DisableBatching" = "False" "Queue" = "Transparent" "RenderPipeline" = "UniversalPipeline" "RenderType" = "Transparent" "ShaderGraphShader" = "true" "ShaderGraphTargetId" = "UniversalLitSubTarget" "UniversalMaterialType" = "Lit" }
    Pass
    {
        Name "Universal Forward"
        Tags { "DisableBatching" = "False" "LightMode" = "UniversalForward" "Queue" = "Transparent" "RenderPipeline" = "UniversalPipeline" "RenderType" = "Transparent" "ShaderGraphShader" = "true" "ShaderGraphTargetId" = "UniversalLitSubTarget" "UniversalMaterialType" = "Lit" }
        Blend One OneMinusSrcAlpha
        ZWrite Off
        HLSLPROGRAM
        #pragma target 4.0
        #pragma vertex vert
        #pragma fragment frag
        #pragma multi_compile _ FOG_LINEAR
        #pragma multi_compile _ _ADDITIONAL_LIGHTS
        #pragma multi_compile _ _CLUSTER_LIGHT_LOOP
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
        float _AlphaToMaskAvailable;
        float4 _MainLightPosition;
        float4 _MainLightColor;
        int _MainLightLayerMask;
        float4 _AdditionalLightsCount;
        float4 _FPParams0;
        float4 _FPParams1;
        float4 _FPParams2;
        float4 _TimeParameters;
        float3 _WorldSpaceCameraPos;
        float4 _ProjectionParams;
        float4 _ZBufferParams;
        float4 unity_OrthoParams;
        float4 _ScaleBiasRt;
        float4 _RTHandleScale;
        float4 unity_FogParams;
        float4 unity_FogColor;
        float4x4 unity_MatrixV;
        float4x4 unity_MatrixVP;
        float4x4 unity_MatrixInvVP;
        float4 _CameraDepthTexture_TexelSize;
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
            float4 unity_SpecCube0_HDR : packoffset(c15);
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
            float _Speed : packoffset(c1);
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

        Texture2D<float4> _CameraDepthTexture;
        Texture2D<float4> _MainTex;
        TextureCube<float4> unity_SpecCube0;
        SamplerState sampler_MainTex;
        SamplerState sampler_vd_point_clamp;
        SamplerState samplerunity_SpecCube0;

        // ---- progVertex
        #if defined(FOG_LINEAR) && defined(_ADDITIONAL_LIGHTS)
        #include "Shader_Graphs_Shader_DE_GroundNavigation/p0_vs_2.hlsl"
        #elif defined(FOG_LINEAR) && defined(_CLUSTER_LIGHT_LOOP)
        #include "Shader_Graphs_Shader_DE_GroundNavigation/p0_vs_4.hlsl"
        #elif defined(_ADDITIONAL_LIGHTS)
        #include "Shader_Graphs_Shader_DE_GroundNavigation/p0_vs_3.hlsl"
        #elif defined(_CLUSTER_LIGHT_LOOP)
        #include "Shader_Graphs_Shader_DE_GroundNavigation/p0_vs_5.hlsl"
        #else
        #include "Shader_Graphs_Shader_DE_GroundNavigation/p0_vs_5.hlsl"
        #endif
        // ---- progFragment
        #if defined(FOG_LINEAR) && defined(_CLUSTER_LIGHT_LOOP) && defined(_DBUFFER_MRT3) && defined(_LIGHT_LAYERS)
        #include "Shader_Graphs_Shader_DE_GroundNavigation/p0_ps_10.hlsl"
        #elif defined(FOG_LINEAR) && defined(_ADDITIONAL_LIGHTS) && defined(_LIGHT_LAYERS)
        #include "Shader_Graphs_Shader_DE_GroundNavigation/p0_ps_12.hlsl"
        #elif defined(FOG_LINEAR) && defined(_CLUSTER_LIGHT_LOOP) && defined(_LIGHT_LAYERS)
        #include "Shader_Graphs_Shader_DE_GroundNavigation/p0_ps_14.hlsl"
        #elif defined(_CLUSTER_LIGHT_LOOP) && defined(_DBUFFER_MRT3) && defined(_LIGHT_LAYERS)
        #include "Shader_Graphs_Shader_DE_GroundNavigation/p0_ps_11.hlsl"
        #elif defined(_ADDITIONAL_LIGHTS) && defined(_LIGHT_LAYERS)
        #include "Shader_Graphs_Shader_DE_GroundNavigation/p0_ps_13.hlsl"
        #elif defined(_CLUSTER_LIGHT_LOOP) && defined(_LIGHT_LAYERS)
        #include "Shader_Graphs_Shader_DE_GroundNavigation/p0_ps_15.hlsl"
        #else
        #include "Shader_Graphs_Shader_DE_GroundNavigation/p0_ps_15.hlsl"
        #endif
        ENDHLSL
    }
    Pass
    {
        Name "MotionVectors"
        Tags { "DisableBatching" = "False" "LightMode" = "MOTIONVECTORS" "Queue" = "Transparent" "RenderPipeline" = "UniversalPipeline" "RenderType" = "Transparent" "ShaderGraphShader" = "true" "ShaderGraphTargetId" = "UniversalLitSubTarget" "UniversalMaterialType" = "Lit" }
        ColorMask RG
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
        float4 _ScaledScreenParams;
        float2 _GlobalMipBias;
        float4 _TimeParameters;
        float4 _ProjectionParams;
        float4 _ZBufferParams;
        float4 unity_OrthoParams;
        float4 _RTHandleScale;
        float4x4 unity_MatrixV;
        float4x4 unity_MatrixVP;
        float4x4 unity_MatrixInvVP;
        float4x4 _PrevViewProjMatrix;
        float4x4 _NonJitteredViewProjMatrix;
        float4 _CameraDepthTexture_TexelSize;
        cbuffer UnityPerDraw
        {
            float4x4 unity_ObjectToWorld : packoffset(c0);
            float4x4 unity_MatrixPreviousM : packoffset(c36);
            float4 unity_MotionVectorsParams : packoffset(c44);
        }
        cbuffer UnityPerMaterial
        {
            float _Speed : packoffset(c1);
            float4 _Color : packoffset(c2);
        }

        Texture2D<float4> _CameraDepthTexture;
        Texture2D<float4> _MainTex;
        SamplerState sampler_MainTex;
        SamplerState sampler_vd_point_clamp;

        // ---- progVertex
        #if 1
        #include "Shader_Graphs_Shader_DE_GroundNavigation/p1_vs_17.hlsl"
        #endif
        // ---- progFragment
        #if 1
        #include "Shader_Graphs_Shader_DE_GroundNavigation/p1_ps_19.hlsl"
        #endif
        ENDHLSL
    }
    Pass
    {
        Name "DepthNormals"
        Tags { "DisableBatching" = "False" "LightMode" = "DepthNormals" "Queue" = "Transparent" "RenderPipeline" = "UniversalPipeline" "RenderType" = "Transparent" "ShaderGraphShader" = "true" "ShaderGraphTargetId" = "UniversalLitSubTarget" "UniversalMaterialType" = "Lit" }
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
        float4 _ScaledScreenParams;
        float2 _GlobalMipBias;
        float4 _TimeParameters;
        float4 _ProjectionParams;
        float4 _ZBufferParams;
        float4 unity_OrthoParams;
        float4 _RTHandleScale;
        float4x4 unity_MatrixV;
        float4x4 unity_MatrixVP;
        float4x4 unity_MatrixInvVP;
        float4 _CameraDepthTexture_TexelSize;
        cbuffer UnityPerDraw
        {
            float4x4 unity_ObjectToWorld : packoffset(c0);
            float4x4 unity_WorldToObject : packoffset(c4);
        }
        cbuffer UnityPerMaterial
        {
            float _Speed : packoffset(c1);
            float4 _Color : packoffset(c2);
        }

        Texture2D<float4> _CameraDepthTexture;
        Texture2D<float4> _MainTex;
        SamplerState sampler_MainTex;
        SamplerState sampler_vd_point_clamp;

        // ---- progVertex
        #if 1
        #include "Shader_Graphs_Shader_DE_GroundNavigation/p2_vs_21.hlsl"
        #endif
        // ---- progFragment
        #if 1
        #include "Shader_Graphs_Shader_DE_GroundNavigation/p2_ps_23.hlsl"
        #endif
        ENDHLSL
    }
    Pass
    {
        Name "Meta"
        Tags { "DisableBatching" = "False" "LightMode" = "META" "Queue" = "Transparent" "RenderPipeline" = "UniversalPipeline" "RenderType" = "Transparent" "ShaderGraphShader" = "true" "ShaderGraphTargetId" = "UniversalLitSubTarget" "UniversalMaterialType" = "Lit" }
        Cull Off
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
        float4 _ScaledScreenParams;
        float2 _GlobalMipBias;
        float4 _TimeParameters;
        float4 _ProjectionParams;
        float4 _ZBufferParams;
        float4 unity_OrthoParams;
        float4 _RTHandleScale;
        float4x4 unity_MatrixV;
        float4x4 unity_MatrixVP;
        float4x4 unity_MatrixInvVP;
        float4 _CameraDepthTexture_TexelSize;
        float unity_OneOverOutputBoost;
        float unity_MaxOutputValue;
        float unity_UseLinearSpace;
        cbuffer UnityMetaPass
        {
            int4 unity_MetaVertexControl : packoffset(c0);
            int4 unity_MetaFragmentControl : packoffset(c1);
        }
        cbuffer UnityPerDraw
        {
            float4x4 unity_ObjectToWorld : packoffset(c0);
            float4 unity_LightmapST : packoffset(c25);
            float4 unity_DynamicLightmapST : packoffset(c26);
        }
        cbuffer UnityPerMaterial
        {
            float _Speed : packoffset(c1);
            float4 _Color : packoffset(c2);
        }

        Texture2D<float4> _CameraDepthTexture;
        Texture2D<float4> _MainTex;
        SamplerState sampler_MainTex;
        SamplerState sampler_vd_point_clamp;

        // ---- progVertex
        #if 1
        #include "Shader_Graphs_Shader_DE_GroundNavigation/p3_vs_25.hlsl"
        #endif
        // ---- progFragment
        #if 1
        #include "Shader_Graphs_Shader_DE_GroundNavigation/p3_ps_27.hlsl"
        #endif
        ENDHLSL
    }
    Pass
    {
        Name "Universal 2D"
        Tags { "DisableBatching" = "False" "LightMode" = "Universal2D" "Queue" = "Transparent" "RenderPipeline" = "UniversalPipeline" "RenderType" = "Transparent" "ShaderGraphShader" = "true" "ShaderGraphTargetId" = "UniversalLitSubTarget" "UniversalMaterialType" = "Lit" }
        Blend SrcAlpha OneMinusSrcAlpha, One OneMinusSrcAlpha
        ZWrite Off
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
        float4 _ScaledScreenParams;
        float2 _GlobalMipBias;
        float4 _TimeParameters;
        float4 _ProjectionParams;
        float4 _ZBufferParams;
        float4 unity_OrthoParams;
        float4 _RTHandleScale;
        float4x4 unity_MatrixV;
        float4x4 unity_MatrixVP;
        float4x4 unity_MatrixInvVP;
        float4 _CameraDepthTexture_TexelSize;
        cbuffer UnityPerDraw
        {
            float4x4 unity_ObjectToWorld : packoffset(c0);
        }
        cbuffer UnityPerMaterial
        {
            float _Speed : packoffset(c1);
            float4 _Color : packoffset(c2);
        }

        Texture2D<float4> _CameraDepthTexture;
        Texture2D<float4> _MainTex;
        SamplerState sampler_MainTex;
        SamplerState sampler_vd_point_clamp;

        // ---- progVertex
        #if 1
        #include "Shader_Graphs_Shader_DE_GroundNavigation/p4_vs_28.hlsl"
        #endif
        // ---- progFragment
        #if 1
        #include "Shader_Graphs_Shader_DE_GroundNavigation/p4_ps_29.hlsl"
        #endif
        ENDHLSL
    }
}
Fallback "Hidden/Shader Graph/FallbackError"
CustomEditor "UnityEditor.ShaderGraph.GenericShaderGraphMaterialGUI"
}
