Shader "LCArt/BGAnim/shader_DE_BGAnim_Normal"
{
Properties
{
    [MainTexture] [Header (Common Setting)] [Space (10)] _MainTex ("Texture", 2D) = "white" {}
    _EmissionTex ("Emission Texture", 2D) = "black" {}
    _EmissionIntensity ("Emission Intenstiy", Range(0, 10)) = 2
    [NoScaleOffset] [Normal] [Header (Normalmap Setting)] [Space (10)] _NormalTex ("NormalTex", 2D) = "bump" {}
    [HideInInspector] [Header (Color Setting)] [Space (10)] _ShadowColor ("Shadow Color", Color) = (0.20000000298023224, 0.20000000298023224, 0.30000001192092896, 1)
    [Header (Billboard Setting)] [Space (10)] [Toggle] _BillboardEnable ("Billboard Enable", Float) = 0
    [Header (Rim Light Setting)] [Space (10)] [Toggle] _RimLightEnable ("Rim Light Enable", Float) = 0
    [Header (Sight Setting)] [Space (10)] [Toggle] _SightEffectEnable ("Sight Effect Enable", Float) = 0
    [Header (Cutoff Setting)] [Space (10)] [Toggle] _CutoffEnable ("Cutoff Enable", Float) = 0
    [Header (Area Mask Setting)] [Space (10)] [Toggle] _CharacterAreaMaskOffEnable ("Character Area Mask Off Enable", Float) = 0
    [HideInInspector] _OutlineColor ("Outline Color", Color) = (0, 0, 0, 1)
    [HideInInspector] [Toggle] _OutlineEnable ("Outline Enable", Float) = 0
    [HideInInspector] _OutlineStroke ("Outline Storke", Float) = 0.125
    [HideInInspector] _DissolveColor ("Death Color", Color) = (0, 0, 0, 1)
    [HideInInspector] _DissolveProgress ("Death Progress", Range(0, 1)) = 0
    [Header (Vertex Color Setting)] [Space (10)] [Toggle] _IsVertexColorAlphaUsed ("Is Vertex Color Alpha Used", Float) = 1
    [Enum(UnityEngine.Rendering.CullMode)] _Cull ("Cull Mode", Float) = 2
    [Enum(UnityEngine.Rendering.CompareFunction)] _ZTest ("Z Test", Float) = 4
    [Toggle] _ZWrite ("Z Write", Float) = 1
    [HideInInspector] [Header (Ghost Effect Setting)] [Space (10)] [Toggle] _GhostEnable ("Ghost Enable", Float) = 0
    [HideInInspector] [Toggle] _IsLocalGhost ("Is Local Ghost", Float) = 1
    [HideInInspector] _OpticalRatioYThreshold ("Optical Ratio Y Threshold", Float) = 0
    [HideInInspector] _OpticalRatioYPositiveRatio ("Optical Ratio Y Positive Ratio", Float) = 0.10000000149011612
    [HideInInspector] _OpticalRatioYNegativeUnit ("Optical Ratio Y Negative Unit", Float) = 1.5
}
SubShader
{
    LOD 100
    Tags { "Queue" = "Transparent" "RenderPipeline" = "UniversalPipeline" }
    Pass
    {
        Name "ForwardLit"
        Tags { "LightMode" = "UniversalForward" "Queue" = "Transparent" "RenderPipeline" = "UniversalPipeline" }
        ZTest [_ZTest]
        ZWrite [_ZWrite]
        Cull [_Cull]
        Stencil {
          Ref 2
          Pass Replace
        }
        HLSLPROGRAM
        #pragma target 4.0
        #pragma vertex vert
        #pragma fragment frag
        #pragma multi_compile _ FOG_LINEAR
        #pragma multi_compile _ _ADDITIONAL_LIGHTS
        #pragma multi_compile _ _CLUSTER_LIGHT_LOOP
        #pragma multi_compile _ _DE_EDITOR_CAMERA

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
        float4 _ProjectionParams;
        float4 _ScreenParams;
        float4 unity_OrthoParams;
        float4 _ScaleBiasRt;
        float4 unity_FogParams;
        float4 unity_FogColor;
        float4x4 unity_MatrixV;
        float4x4 unity_MatrixVP;
        float4 _ScreenSize;
        float3 _PlaneBoundMinPos;
        float3 _PlaneBoundMaxPos;
        float _GlobalPlaneSystemEnable;
        float _GlobalPlanePointBufferEnable;
        float _GlobalBackgroundSeeThroughEnable;
        float _GlobalFogHeightEnable;
        float _GlobalFogHeightMax;
        float _GlobalFogHeightMin;
        float4 _GlobalBackgroundShadowColor;
        float _GlobalBoxAreaEnable;
        float4 _GlobalBoxAreaCenters[8];
        float4 _GlobalBoxAreaExtents[8];
        float _GlobalBoxAreaCoverCharacters[8];
        float _GlobalSeeThroughCircleAlpha;
        float _GlobalSeeThroughCircleRange;
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
            float4 unity_LightData : packoffset(c11);
            float4 unity_LightIndices[2] : packoffset(c12);
        }
        cbuffer UnityPerMaterial
        {
            float4 _MainTex_ST : packoffset(c0);
            float4 _NormalTex_ST : packoffset(c1);
            float _EmissionIntensity : packoffset(c3);
            float _SightEffectEnable : packoffset(c10.y);
            float _CharacterAreaMaskOffEnable : packoffset(c10.z);
            float _IsVertexColorAlphaUsed : packoffset(c12.y);
            float _BillboardEnable : packoffset(c12.z);
            float _OpticalRatioYThreshold : packoffset(c17.z);
            float _OpticalRatioYPositiveRatio : packoffset(c17.w);
            float _OpticalRatioYNegativeUnit : packoffset(c18);
        }
        cbuffer urp_TileBuffer
        {
            float4 urp_Tiles[4096] : packoffset(c0);
        }
        cbuffer urp_ZBinBuffer
        {
            float4 urp_ZBins[1024] : packoffset(c0);
        }

        Texture2D<float4> _CustomDepthBuffer;
        Texture2D<float4> _EmissionTex;
        Texture2D<float4> _MainTex;
        Texture2D<float4> _PlanePointBuffer;
        Texture2D<float4> _PlaneSightRenderTarget;
        SamplerState sampler_CustomDepthBuffer;
        SamplerState sampler_EmissionTex;
        SamplerState sampler_MainTex;
        SamplerState sampler_PlanePointBuffer;
        SamplerState sampler_PlaneSightRenderTarget;

        // ---- progVertex
        #if defined(FOG_LINEAR) && defined(_ADDITIONAL_LIGHTS) && defined(_DE_EDITOR_CAMERA)
        #include "LCArt_BGAnim_shader_DE_BGAnim_Normal/p0_vs_13.hlsl"
        #elif defined(FOG_LINEAR) && defined(_CLUSTER_LIGHT_LOOP) && defined(_DE_EDITOR_CAMERA)
        #include "LCArt_BGAnim_shader_DE_BGAnim_Normal/p0_vs_9.hlsl"
        #elif defined(FOG_LINEAR) && defined(_ADDITIONAL_LIGHTS)
        #include "LCArt_BGAnim_shader_DE_BGAnim_Normal/p0_vs_11.hlsl"
        #elif defined(FOG_LINEAR) && defined(_CLUSTER_LIGHT_LOOP)
        #include "LCArt_BGAnim_shader_DE_BGAnim_Normal/p0_vs_7.hlsl"
        #elif defined(FOG_LINEAR) && defined(_DE_EDITOR_CAMERA)
        #include "LCArt_BGAnim_shader_DE_BGAnim_Normal/p0_vs_5.hlsl"
        #elif defined(_ADDITIONAL_LIGHTS) && defined(_DE_EDITOR_CAMERA)
        #include "LCArt_BGAnim_shader_DE_BGAnim_Normal/p0_vs_12.hlsl"
        #elif defined(_CLUSTER_LIGHT_LOOP) && defined(_DE_EDITOR_CAMERA)
        #include "LCArt_BGAnim_shader_DE_BGAnim_Normal/p0_vs_8.hlsl"
        #elif defined(FOG_LINEAR)
        #include "LCArt_BGAnim_shader_DE_BGAnim_Normal/p0_vs_3.hlsl"
        #elif defined(_ADDITIONAL_LIGHTS)
        #include "LCArt_BGAnim_shader_DE_BGAnim_Normal/p0_vs_10.hlsl"
        #elif defined(_CLUSTER_LIGHT_LOOP)
        #include "LCArt_BGAnim_shader_DE_BGAnim_Normal/p0_vs_6.hlsl"
        #elif defined(_DE_EDITOR_CAMERA)
        #include "LCArt_BGAnim_shader_DE_BGAnim_Normal/p0_vs_4.hlsl"
        #elif 1
        #include "LCArt_BGAnim_shader_DE_BGAnim_Normal/p0_vs_2.hlsl"
        #endif
        // ---- progFragment
        #if defined(FOG_LINEAR) && defined(_ADDITIONAL_LIGHTS) && defined(_DE_EDITOR_CAMERA)
        #include "LCArt_BGAnim_shader_DE_BGAnim_Normal/p0_ps_34.hlsl"
        #elif defined(FOG_LINEAR) && defined(_CLUSTER_LIGHT_LOOP) && defined(_DE_EDITOR_CAMERA)
        #include "LCArt_BGAnim_shader_DE_BGAnim_Normal/p0_ps_30.hlsl"
        #elif defined(FOG_LINEAR) && defined(_ADDITIONAL_LIGHTS)
        #include "LCArt_BGAnim_shader_DE_BGAnim_Normal/p0_ps_32.hlsl"
        #elif defined(FOG_LINEAR) && defined(_CLUSTER_LIGHT_LOOP)
        #include "LCArt_BGAnim_shader_DE_BGAnim_Normal/p0_ps_28.hlsl"
        #elif defined(FOG_LINEAR) && defined(_DE_EDITOR_CAMERA)
        #include "LCArt_BGAnim_shader_DE_BGAnim_Normal/p0_ps_26.hlsl"
        #elif defined(_ADDITIONAL_LIGHTS) && defined(_DE_EDITOR_CAMERA)
        #include "LCArt_BGAnim_shader_DE_BGAnim_Normal/p0_ps_33.hlsl"
        #elif defined(_CLUSTER_LIGHT_LOOP) && defined(_DE_EDITOR_CAMERA)
        #include "LCArt_BGAnim_shader_DE_BGAnim_Normal/p0_ps_29.hlsl"
        #elif defined(FOG_LINEAR)
        #include "LCArt_BGAnim_shader_DE_BGAnim_Normal/p0_ps_24.hlsl"
        #elif defined(_ADDITIONAL_LIGHTS)
        #include "LCArt_BGAnim_shader_DE_BGAnim_Normal/p0_ps_31.hlsl"
        #elif defined(_CLUSTER_LIGHT_LOOP)
        #include "LCArt_BGAnim_shader_DE_BGAnim_Normal/p0_ps_27.hlsl"
        #elif defined(_DE_EDITOR_CAMERA)
        #include "LCArt_BGAnim_shader_DE_BGAnim_Normal/p0_ps_25.hlsl"
        #elif 1
        #include "LCArt_BGAnim_shader_DE_BGAnim_Normal/p0_ps_23.hlsl"
        #endif
        ENDHLSL
    }
    Pass
    {
        Name "Outline"
        Tags { "LightMode" = "Outline" "Queue" = "Transparent" "RenderPipeline" = "UniversalPipeline" }
        ZWrite Off
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
        float2 _GlobalMipBias;
        float3 _WorldSpaceCameraPos;
        float4 _ProjectionParams;
        float4x4 unity_MatrixV;
        float4x4 unity_MatrixVP;
        float _GlobalBoxAreaEnable;
        float4 _GlobalBoxAreaCenters[8];
        float4 _GlobalBoxAreaExtents[8];
        float _GlobalBoxAreaCoverOutlines[8];
        cbuffer UnityPerDraw
        {
            float4x4 unity_ObjectToWorld : packoffset(c0);
        }
        cbuffer UnityPerMaterial
        {
            float _DissolveProgress : packoffset(c12);
            float _BillboardEnable : packoffset(c12.z);
            float4 _OutlineColor : packoffset(c14);
            float _OutlineEnable : packoffset(c15);
            float _OutlineStroke : packoffset(c15.y);
            float _OpticalRatioYThreshold : packoffset(c17.z);
            float _OpticalRatioYPositiveRatio : packoffset(c17.w);
            float _OpticalRatioYNegativeUnit : packoffset(c18);
        }

        Texture2D<float4> _MainTex;
        SamplerState sampler_MainTex;

        // ---- progVertex
        #if 1
        #include "LCArt_BGAnim_shader_DE_BGAnim_Normal/p1_vs_36.hlsl"
        #endif
        // ---- progFragment
        #if 1
        #include "LCArt_BGAnim_shader_DE_BGAnim_Normal/p1_ps_37.hlsl"
        #endif
        ENDHLSL
    }
    Pass
    {
        Name "RimLight"
        Tags { "LightMode" = "RimLightTarget" "Queue" = "Transparent" "RenderPipeline" = "UniversalPipeline" }
        Blend One One
        Cull Off
        Stencil {
          Ref 2
          Comp NotEqual
          Pass Replace
        }
        HLSLPROGRAM
        #pragma target 4.0
        #pragma vertex vert
        #pragma fragment frag
        #pragma multi_compile _ _ADDITIONAL_LIGHTS
        #pragma multi_compile _ _CLUSTER_LIGHT_LOOP

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
        float4 _MainLightPosition;
        float4 _MainLightColor;
        float3 _WorldSpaceCameraPos;
        float4 _ProjectionParams;
        float4 _ScreenParams;
        float4 unity_OrthoParams;
        float4x4 unity_MatrixV;
        float4x4 unity_MatrixVP;
        float3 _PlaneBoundMinPos;
        float3 _PlaneBoundMaxPos;
        float _GlobalPlaneSystemEnable;
        float _GlobalPlanePointBufferEnable;
        int _RimEffectCount;
        float4 _RimEffectColors[8];
        float4 _RimEffectPositions[8];
        float _RimEffectRanges[8];
        float _GlobalBackgroundSeeThroughEnable;
        float _GlobalBoxAreaEnable;
        float4 _GlobalBoxAreaCenters[8];
        float4 _GlobalBoxAreaExtents[8];
        float _GlobalBoxAreaCoverCharacters[8];
        cbuffer AdditionalLights
        {
            float _AdditionalLightsLayerMasks[256] : packoffset(c1280);
        }
        cbuffer UnityPerDraw
        {
            float4x4 unity_ObjectToWorld : packoffset(c0);
            float4 unity_RenderingLayer : packoffset(c10);
            float4 unity_LightIndices[2] : packoffset(c12);
        }
        cbuffer UnityPerMaterial
        {
            float _SightEffectEnable : packoffset(c10.y);
            float _BillboardEnable : packoffset(c12.z);
            float _RimLightEnable : packoffset(c12.w);
            float _GhostEnable : packoffset(c13.z);
            float _OutlineEnable : packoffset(c15);
            float _OpticalRatioYThreshold : packoffset(c17.z);
            float _OpticalRatioYPositiveRatio : packoffset(c17.w);
            float _OpticalRatioYNegativeUnit : packoffset(c18);
        }

        Texture2D<float4> _MainTex;
        Texture2D<float4> _PlanePointBuffer;
        Texture2D<float4> _PlaneSightRenderTarget;
        SamplerState sampler_MainTex;
        SamplerState sampler_PlanePointBuffer;
        SamplerState sampler_PlaneSightRenderTarget;

        // ---- progVertex
        #if defined(_ADDITIONAL_LIGHTS)
        #include "LCArt_BGAnim_shader_DE_BGAnim_Normal/p2_vs_41.hlsl"
        #elif defined(_CLUSTER_LIGHT_LOOP)
        #include "LCArt_BGAnim_shader_DE_BGAnim_Normal/p2_vs_42.hlsl"
        #elif 1
        #include "LCArt_BGAnim_shader_DE_BGAnim_Normal/p2_vs_40.hlsl"
        #endif
        // ---- progFragment
        #if defined(_ADDITIONAL_LIGHTS)
        #include "LCArt_BGAnim_shader_DE_BGAnim_Normal/p2_ps_46.hlsl"
        #elif defined(_CLUSTER_LIGHT_LOOP)
        #include "LCArt_BGAnim_shader_DE_BGAnim_Normal/p2_ps_47.hlsl"
        #elif 1
        #include "LCArt_BGAnim_shader_DE_BGAnim_Normal/p2_ps_45.hlsl"
        #endif
        ENDHLSL
    }
}
}
