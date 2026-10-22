Shader "LCArt/Background/Shader_DE_WaterBottom"
{
Properties
{
    [MainTexture] [Header(______________________________________________________________________________________________________________________)] [Header(Common)] [Space(10)] _BaseMap ("Base Map (RGB)", 2D) = "white" {}
    [MainColor] _BaseColor ("Base Color", Color) = (1, 1, 1, 1)
    _BumpMap ("Normal Map", 2D) = "bump" {}
    [Header(Vertex Displacement)] _SurfaceWaveHeight ("Wave Height", Float) = 0.10000000149011612
    [Header(Depth Edge)] _SurfaceWaveDepthStart ("Depth Start", Float) = 0
    _SurfaceWaveDepthEnd ("Depth End", Float) = 0.10000000149011612
    _SurfaceWaveDepthInner ("Depth Inner", Float) = 0.5
}
SubShader
{
    LOD 300
    Tags { "Queue" = "Transparent" "RenderType" = "Transparent" }
    Pass
    {
        Name "ForwardLit"
        Tags { "LightMode" = "UniversalForward" "Queue" = "Transparent" "RenderType" = "Transparent" }
        Blend SrcAlpha OneMinusSrcAlpha
        ZWrite Off
        HLSLPROGRAM
        #pragma target 4.0
        #pragma vertex vert
        #pragma fragment frag
        #pragma multi_compile _ FOG_LINEAR
        #pragma multi_compile _ _ADDITIONAL_LIGHTS
        #pragma multi_compile _ _CLUSTER_LIGHT_LOOP
        #pragma multi_compile _ _DE_EDITOR_CAMERA
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
        float4 _AdditionalLightsCount;
        float4 _Time;
        float4 _FPParams0;
        float4 _FPParams1;
        float4 _FPParams2;
        float3 _WorldSpaceCameraPos;
        float4 _ProjectionParams;
        float4 _ZBufferParams;
        float4 unity_OrthoParams;
        float4 _RTHandleScale;
        float4 _ScaleBiasRt;
        float4 unity_FogParams;
        float4 unity_FogColor;
        float4x4 unity_MatrixV;
        float4x4 unity_MatrixVP;
        float4 _GlobalWaterBottomColor;
        float4 _GlobalWaterBottomShallowColor;
        float4 _GlobalWaterBottomEdgeParams;
        float4 _GlobalWaterBottomParams;
        float4 _GlobalWaterWaveParams;
        float4 _GlobalWaterWaveParams2;
        float _GlobalWaterReflectionIntensity;
        int _PlaneReflectionActive;
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
            float4 unity_WorldTransformParams : packoffset(c9);
            float4 unity_RenderingLayer : packoffset(c10);
            float4 unity_LightData : packoffset(c11);
            float4 unity_LightIndices[2] : packoffset(c12);
        }
        cbuffer UnityPerMaterial
        {
            float4 _BaseMap_ST : packoffset(c0);
            float _SurfaceWaveHeight : packoffset(c2);
        }
        cbuffer urp_TileBuffer
        {
            float4 urp_Tiles[4096] : packoffset(c0);
        }
        cbuffer urp_ZBinBuffer
        {
            float4 urp_ZBins[1024] : packoffset(c0);
        }

        Texture2D<float4> _BumpMap;
        Texture2D<float4> _CameraDepthTexture;
        Texture2D<float4> _GlobalWaterNoiseTex;
        Texture2D<float4> _PlaneReflectionTexture;
        SamplerState sampler_vd_linear_clamp;
        SamplerState sampler_vd_linear_repeat;
        SamplerState sampler_vd_point_clamp;

        // ---- progVertex
        #if defined(FOG_LINEAR) && defined(_ADDITIONAL_LIGHTS) && defined(_DE_EDITOR_CAMERA) && defined(_LIGHT_LAYERS)
        #include "LCArt_Background_Shader_DE_WaterBottom/p0_vs_4.hlsl"
        #elif defined(FOG_LINEAR) && defined(_CLUSTER_LIGHT_LOOP) && defined(_DE_EDITOR_CAMERA) && defined(_LIGHT_LAYERS)
        #include "LCArt_Background_Shader_DE_WaterBottom/p0_vs_8.hlsl"
        #elif defined(FOG_LINEAR) && defined(_ADDITIONAL_LIGHTS) && defined(_LIGHT_LAYERS)
        #include "LCArt_Background_Shader_DE_WaterBottom/p0_vs_6.hlsl"
        #elif defined(FOG_LINEAR) && defined(_CLUSTER_LIGHT_LOOP) && defined(_LIGHT_LAYERS)
        #include "LCArt_Background_Shader_DE_WaterBottom/p0_vs_10.hlsl"
        #elif defined(FOG_LINEAR) && defined(_DE_EDITOR_CAMERA) && defined(_LIGHT_LAYERS)
        #include "LCArt_Background_Shader_DE_WaterBottom/p0_vs_15.hlsl"
        #elif defined(_ADDITIONAL_LIGHTS) && defined(_DE_EDITOR_CAMERA) && defined(_LIGHT_LAYERS)
        #include "LCArt_Background_Shader_DE_WaterBottom/p0_vs_5.hlsl"
        #elif defined(_CLUSTER_LIGHT_LOOP) && defined(_DE_EDITOR_CAMERA) && defined(_LIGHT_LAYERS)
        #include "LCArt_Background_Shader_DE_WaterBottom/p0_vs_9.hlsl"
        #elif defined(FOG_LINEAR) && defined(_LIGHT_LAYERS)
        #include "LCArt_Background_Shader_DE_WaterBottom/p0_vs_13.hlsl"
        #elif defined(_ADDITIONAL_LIGHTS) && defined(_LIGHT_LAYERS)
        #include "LCArt_Background_Shader_DE_WaterBottom/p0_vs_7.hlsl"
        #elif defined(_CLUSTER_LIGHT_LOOP) && defined(_LIGHT_LAYERS)
        #include "LCArt_Background_Shader_DE_WaterBottom/p0_vs_11.hlsl"
        #elif defined(_DE_EDITOR_CAMERA) && defined(_LIGHT_LAYERS)
        #include "LCArt_Background_Shader_DE_WaterBottom/p0_vs_14.hlsl"
        #elif defined(_LIGHT_LAYERS)
        #include "LCArt_Background_Shader_DE_WaterBottom/p0_vs_12.hlsl"
        #else
        #include "LCArt_Background_Shader_DE_WaterBottom/p0_vs_12.hlsl"
        #endif
        // ---- progFragment
        #if defined(FOG_LINEAR) && defined(_ADDITIONAL_LIGHTS) && defined(_DE_EDITOR_CAMERA) && defined(_LIGHT_LAYERS)
        #include "LCArt_Background_Shader_DE_WaterBottom/p0_ps_22.hlsl"
        #elif defined(FOG_LINEAR) && defined(_CLUSTER_LIGHT_LOOP) && defined(_DE_EDITOR_CAMERA) && defined(_LIGHT_LAYERS)
        #include "LCArt_Background_Shader_DE_WaterBottom/p0_ps_26.hlsl"
        #elif defined(FOG_LINEAR) && defined(_ADDITIONAL_LIGHTS) && defined(_LIGHT_LAYERS)
        #include "LCArt_Background_Shader_DE_WaterBottom/p0_ps_24.hlsl"
        #elif defined(FOG_LINEAR) && defined(_CLUSTER_LIGHT_LOOP) && defined(_LIGHT_LAYERS)
        #include "LCArt_Background_Shader_DE_WaterBottom/p0_ps_28.hlsl"
        #elif defined(FOG_LINEAR) && defined(_DE_EDITOR_CAMERA) && defined(_LIGHT_LAYERS)
        #include "LCArt_Background_Shader_DE_WaterBottom/p0_ps_33.hlsl"
        #elif defined(_ADDITIONAL_LIGHTS) && defined(_DE_EDITOR_CAMERA) && defined(_LIGHT_LAYERS)
        #include "LCArt_Background_Shader_DE_WaterBottom/p0_ps_23.hlsl"
        #elif defined(_CLUSTER_LIGHT_LOOP) && defined(_DE_EDITOR_CAMERA) && defined(_LIGHT_LAYERS)
        #include "LCArt_Background_Shader_DE_WaterBottom/p0_ps_27.hlsl"
        #elif defined(FOG_LINEAR) && defined(_LIGHT_LAYERS)
        #include "LCArt_Background_Shader_DE_WaterBottom/p0_ps_31.hlsl"
        #elif defined(_ADDITIONAL_LIGHTS) && defined(_LIGHT_LAYERS)
        #include "LCArt_Background_Shader_DE_WaterBottom/p0_ps_25.hlsl"
        #elif defined(_CLUSTER_LIGHT_LOOP) && defined(_LIGHT_LAYERS)
        #include "LCArt_Background_Shader_DE_WaterBottom/p0_ps_29.hlsl"
        #elif defined(_DE_EDITOR_CAMERA) && defined(_LIGHT_LAYERS)
        #include "LCArt_Background_Shader_DE_WaterBottom/p0_ps_32.hlsl"
        #elif defined(_LIGHT_LAYERS)
        #include "LCArt_Background_Shader_DE_WaterBottom/p0_ps_30.hlsl"
        #else
        #include "LCArt_Background_Shader_DE_WaterBottom/p0_ps_30.hlsl"
        #endif
        ENDHLSL
    }
}
Fallback "Hidden/Universal Render Pipeline/FallbackError"
}
