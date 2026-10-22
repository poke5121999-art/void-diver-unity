Shader "LCArt/Character/shader_DE_Character_Normal"
{
Properties
{
    [MainTexture] [Header (Common Setting)] [Space (10)] _MainTex ("Texture", 2D) = "white" {}
    [NoScaleOffset] [Normal] [Header (Normalmap Setting)] [Space (10)] _NormalTex ("Normal Texture", 2D) = "bump" {}
    [HideInInspector] [Header (Color Setting)] [Space (10)] _ShadowColor ("Shadow Color", Color) = (0.20000000298023224, 0.20000000298023224, 0.30000001192092896, 1)
    [HideInInspector] _OutOfSightColor ("Out Of Sight Color", Color) = (0.10000000149011612, 0.10000000149011612, 0.10000000149011612, 1)
    [Header (Sight Setting)] [Space (10)] [Toggle] _SightEffectEnable ("Sight Effect Enable", Float) = 1
    [Header (Rim Light Setting)] [Space (10)] [Toggle] _RimLightEnable ("Rim Light Enable", Float) = 1
    [Toggle] _RimLightEnableWhenOutOfSight ("Rim Light Out Of Sight Enable", Float) = 0
    [Header (Outline Setting)] [Space (10)] [Toggle] _OutlineEnable ("Outline Enable", Float) = 0
    [HideInInspector] _OutlineColor ("Outline Color", Color) = (0, 0, 0, 1)
    [HideInInspector] _OutlineStroke ("Outline Storke", Float) = 0.125
    [Header (Mask Setting)] [Space (10)] _MaskTex ("Mask Texture", 2D) = "black" {}
    [HideInInspector] _MaskColor0 ("Mask Color 0", Color) = (1, 0, 0, 1)
    [HideInInspector] _MaskColor1 ("Mask Color 1", Color) = (0, 1, 0, 1)
    [HideInInspector] _MaskIntensity ("Mask Intensity", Range(0, 1)) = 0
    [Header (Ghost Effect Setting)] [Space (10)] [Toggle] _GhostEnable ("Ghost Enable", Float) = 0
    [Toggle] _IsLocalGhost ("Is Local Ghost", Float) = 1
    [Header (Fake Shadow Enable)] [Space (10)] [Toggle] _FakeShadowEnable ("Fake Shadow Enable", Float) = 1
    [HideInInspector] [HDR] [Header (In Game Hit Effect Setting)] [Space (10)] _EffectColor ("(타격, 상태이상 용!) Effect Color", Color) = (1, 1, 1, 1)
    [HideInInspector] _Effect ("(타격, 상태이상 용!) Effect", Range(0, 1)) = 0
    [HideInInspector] _LightEffectIntensity ("Monster Light Effect", Range(0, 1)) = 0
    [Header (In Game Death Effect Setting)] [Space (10)] _DissolveProgress ("Death Progress", Range(0, 1)) = 0
    _DissolveColor ("Death Color", Color) = (0, 0, 0, 1)
    [Header (Vertex Color Setting)] [Space (10)] [Toggle] _IsVertexColorAlphaUsed ("Is Vertex Color Alpha Used", Float) = 1
    [Toggle] _UseDarkColorMask ("Use Spine Tint Black", Float) = 0
    [Header (Optical Numbers)] _OpticalRatioYThreshold ("Optical Ratio Y Threshold", Float) = 0
    _OpticalRatioYPositiveRatio ("Optical Ratio Y Positive Ratio", Float) = 1
    _OpticalRatioYNegativeUnit ("Optical Ratio Y Negative Unit", Float) = 1.5
    [HideInInspector] _IsReflected ("Plane Reflection Flag", Float) = 1
    [HideInInspector] _IsEnemy ("Is Enemey", Float) = 0
    [HideInInspector] _RimLightOffset ("Rim Light Offset", Float) = 0.009999999776482582
}
SubShader
{
    LOD 100
    Tags { "Queue" = "AlphaTest" "RenderPipeline" = "UniversalPipeline" }
    Pass
    {
        Name "ForwardLit"
        Tags { "IgnoreProjector" = "true" "LightMode" = "CustomForward" "Queue" = "AlphaTest" "RenderPipeline" = "UniversalPipeline" "RenderType" = "Opaque" }
        Blend SrcAlpha OneMinusSrcAlpha
        Cull Off
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
        #pragma multi_compile _ _DE_PRERENDERING_CAMERA

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
        float4 _Time;
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
        float4 _GlobalCharacterShadowColor;
        float _GlobalFogHeightEnable;
        float _GlobalFogHeightMin;
        float _GlobalFogHeightMax;
        float _GlobalIsRuntime;
        int _GlobalWaterAreaEnable;
        float _GlobalWaterSurfaceLevel;
        float4 _GlobalWaterShallowColor;
        float4 _GlobalWaterDeepColor;
        float4 _GlobalWaterShadowColor;
        float4 _GlobalWaterShallowShadowColor;
        float4 _GlobalWaterShoreEdge;
        float4 _GlobalWaterDepthRange;
        float4 _GlobalWaterSurfaceWaveMapParams;
        float4 _GlobalOverallMapBoundMin;
        float4 _GlobalOverallMapBoundMax;
        float _GlobalBoxAreaEnable;
        float4 _GlobalBoxAreaCenters[8];
        float4 _GlobalBoxAreaExtents[8];
        float _GlobalBoxAreaCoverCharacters[8];
        float3 _PlaneBoundMinPos;
        float3 _PlaneBoundMaxPos;
        float _GlobalPlaneSystemEnable;
        float _GlobalPlanePointBufferEnable;
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
        }
        cbuffer UnityPerMaterial
        {
            float4 _MainTex_ST : packoffset(c0);
            float4 _NormalTex_ST : packoffset(c1);
            float4 _MaskColor0 : packoffset(c3);
            float4 _MaskColor1 : packoffset(c4);
            float _MaskIntensity : packoffset(c5);
            float4 _EffectColor : packoffset(c8);
            float _Effect : packoffset(c9);
            float _LightEffectIntensity : packoffset(c9.y);
            float _SightEffectEnable : packoffset(c9.z);
            float4 _DissolveColor : packoffset(c10);
            float _DissolveProgress : packoffset(c11);
            float _IsVertexColorAlphaUsed : packoffset(c11.y);
            float _GhostEnable : packoffset(c12.z);
            float _OpticalRatioYThreshold : packoffset(c18);
            float _OpticalRatioYPositiveRatio : packoffset(c18.y);
            float _OpticalRatioYNegativeUnit : packoffset(c18.z);
            float _UseDarkColorMask : packoffset(c18.w);
        }
        cbuffer urp_TileBuffer
        {
            float4 urp_Tiles[4096] : packoffset(c0);
        }
        cbuffer urp_ZBinBuffer
        {
            float4 urp_ZBins[1024] : packoffset(c0);
        }

        Texture2D<float4> _GlobalWaterAreaMap;
        Texture2D<float4> _GlobalWaterSurfaceWaveMap;
        Texture2D<float4> _MainTex;
        Texture2D<float4> _MaskTex;
        Texture2D<float4> _PlanePointBuffer;
        Texture2D<float4> _PlaneSightRenderTarget;
        SamplerState sampler_MainTex;
        SamplerState sampler_PlanePointBuffer;
        SamplerState sampler_PlaneSightRenderTarget;
        SamplerState sampler_vd_linear_clamp;
        SamplerState sampler_vd_point_repeat;

        // ---- progVertex
        #if defined(FOG_LINEAR) && defined(_ADDITIONAL_LIGHTS) && defined(_DE_EDITOR_CAMERA) && defined(_DE_PRERENDERING_CAMERA)
        #include "LCArt_Character_shader_DE_Character_Normal/p0_vs_19.hlsl"
        #elif defined(FOG_LINEAR) && defined(_CLUSTER_LIGHT_LOOP) && defined(_DE_EDITOR_CAMERA) && defined(_DE_PRERENDERING_CAMERA)
        #include "LCArt_Character_shader_DE_Character_Normal/p0_vs_27.hlsl"
        #elif defined(FOG_LINEAR) && defined(_ADDITIONAL_LIGHTS) && defined(_DE_EDITOR_CAMERA)
        #include "LCArt_Character_shader_DE_Character_Normal/p0_vs_17.hlsl"
        #elif defined(FOG_LINEAR) && defined(_ADDITIONAL_LIGHTS) && defined(_DE_PRERENDERING_CAMERA)
        #include "LCArt_Character_shader_DE_Character_Normal/p0_vs_15.hlsl"
        #elif defined(FOG_LINEAR) && defined(_CLUSTER_LIGHT_LOOP) && defined(_DE_EDITOR_CAMERA)
        #include "LCArt_Character_shader_DE_Character_Normal/p0_vs_25.hlsl"
        #elif defined(FOG_LINEAR) && defined(_CLUSTER_LIGHT_LOOP) && defined(_DE_PRERENDERING_CAMERA)
        #include "LCArt_Character_shader_DE_Character_Normal/p0_vs_23.hlsl"
        #elif defined(FOG_LINEAR) && defined(_DE_EDITOR_CAMERA) && defined(_DE_PRERENDERING_CAMERA)
        #include "LCArt_Character_shader_DE_Character_Normal/p0_vs_11.hlsl"
        #elif defined(_ADDITIONAL_LIGHTS) && defined(_DE_EDITOR_CAMERA) && defined(_DE_PRERENDERING_CAMERA)
        #include "LCArt_Character_shader_DE_Character_Normal/p0_vs_18.hlsl"
        #elif defined(_CLUSTER_LIGHT_LOOP) && defined(_DE_EDITOR_CAMERA) && defined(_DE_PRERENDERING_CAMERA)
        #include "LCArt_Character_shader_DE_Character_Normal/p0_vs_26.hlsl"
        #elif defined(FOG_LINEAR) && defined(_ADDITIONAL_LIGHTS)
        #include "LCArt_Character_shader_DE_Character_Normal/p0_vs_13.hlsl"
        #elif defined(FOG_LINEAR) && defined(_CLUSTER_LIGHT_LOOP)
        #include "LCArt_Character_shader_DE_Character_Normal/p0_vs_21.hlsl"
        #elif defined(FOG_LINEAR) && defined(_DE_EDITOR_CAMERA)
        #include "LCArt_Character_shader_DE_Character_Normal/p0_vs_9.hlsl"
        #elif defined(FOG_LINEAR) && defined(_DE_PRERENDERING_CAMERA)
        #include "LCArt_Character_shader_DE_Character_Normal/p0_vs_7.hlsl"
        #elif defined(_ADDITIONAL_LIGHTS) && defined(_DE_EDITOR_CAMERA)
        #include "LCArt_Character_shader_DE_Character_Normal/p0_vs_16.hlsl"
        #elif defined(_ADDITIONAL_LIGHTS) && defined(_DE_PRERENDERING_CAMERA)
        #include "LCArt_Character_shader_DE_Character_Normal/p0_vs_14.hlsl"
        #elif defined(_CLUSTER_LIGHT_LOOP) && defined(_DE_EDITOR_CAMERA)
        #include "LCArt_Character_shader_DE_Character_Normal/p0_vs_24.hlsl"
        #elif defined(_CLUSTER_LIGHT_LOOP) && defined(_DE_PRERENDERING_CAMERA)
        #include "LCArt_Character_shader_DE_Character_Normal/p0_vs_22.hlsl"
        #elif defined(_DE_EDITOR_CAMERA) && defined(_DE_PRERENDERING_CAMERA)
        #include "LCArt_Character_shader_DE_Character_Normal/p0_vs_10.hlsl"
        #elif defined(FOG_LINEAR)
        #include "LCArt_Character_shader_DE_Character_Normal/p0_vs_5.hlsl"
        #elif defined(_ADDITIONAL_LIGHTS)
        #include "LCArt_Character_shader_DE_Character_Normal/p0_vs_12.hlsl"
        #elif defined(_CLUSTER_LIGHT_LOOP)
        #include "LCArt_Character_shader_DE_Character_Normal/p0_vs_20.hlsl"
        #elif defined(_DE_EDITOR_CAMERA)
        #include "LCArt_Character_shader_DE_Character_Normal/p0_vs_8.hlsl"
        #elif defined(_DE_PRERENDERING_CAMERA)
        #include "LCArt_Character_shader_DE_Character_Normal/p0_vs_6.hlsl"
        #elif 1
        #include "LCArt_Character_shader_DE_Character_Normal/p0_vs_4.hlsl"
        #endif
        // ---- progFragment
        #if defined(FOG_LINEAR) && defined(_ADDITIONAL_LIGHTS) && defined(_DE_EDITOR_CAMERA) && defined(_DE_PRERENDERING_CAMERA)
        #include "LCArt_Character_shader_DE_Character_Normal/p0_ps_49.hlsl"
        #elif defined(FOG_LINEAR) && defined(_CLUSTER_LIGHT_LOOP) && defined(_DE_EDITOR_CAMERA) && defined(_DE_PRERENDERING_CAMERA)
        #include "LCArt_Character_shader_DE_Character_Normal/p0_ps_57.hlsl"
        #elif defined(FOG_LINEAR) && defined(_ADDITIONAL_LIGHTS) && defined(_DE_EDITOR_CAMERA)
        #include "LCArt_Character_shader_DE_Character_Normal/p0_ps_47.hlsl"
        #elif defined(FOG_LINEAR) && defined(_ADDITIONAL_LIGHTS) && defined(_DE_PRERENDERING_CAMERA)
        #include "LCArt_Character_shader_DE_Character_Normal/p0_ps_45.hlsl"
        #elif defined(FOG_LINEAR) && defined(_CLUSTER_LIGHT_LOOP) && defined(_DE_EDITOR_CAMERA)
        #include "LCArt_Character_shader_DE_Character_Normal/p0_ps_55.hlsl"
        #elif defined(FOG_LINEAR) && defined(_CLUSTER_LIGHT_LOOP) && defined(_DE_PRERENDERING_CAMERA)
        #include "LCArt_Character_shader_DE_Character_Normal/p0_ps_53.hlsl"
        #elif defined(FOG_LINEAR) && defined(_DE_EDITOR_CAMERA) && defined(_DE_PRERENDERING_CAMERA)
        #include "LCArt_Character_shader_DE_Character_Normal/p0_ps_41.hlsl"
        #elif defined(_ADDITIONAL_LIGHTS) && defined(_DE_EDITOR_CAMERA) && defined(_DE_PRERENDERING_CAMERA)
        #include "LCArt_Character_shader_DE_Character_Normal/p0_ps_48.hlsl"
        #elif defined(_CLUSTER_LIGHT_LOOP) && defined(_DE_EDITOR_CAMERA) && defined(_DE_PRERENDERING_CAMERA)
        #include "LCArt_Character_shader_DE_Character_Normal/p0_ps_56.hlsl"
        #elif defined(FOG_LINEAR) && defined(_ADDITIONAL_LIGHTS)
        #include "LCArt_Character_shader_DE_Character_Normal/p0_ps_43.hlsl"
        #elif defined(FOG_LINEAR) && defined(_CLUSTER_LIGHT_LOOP)
        #include "LCArt_Character_shader_DE_Character_Normal/p0_ps_51.hlsl"
        #elif defined(FOG_LINEAR) && defined(_DE_EDITOR_CAMERA)
        #include "LCArt_Character_shader_DE_Character_Normal/p0_ps_39.hlsl"
        #elif defined(FOG_LINEAR) && defined(_DE_PRERENDERING_CAMERA)
        #include "LCArt_Character_shader_DE_Character_Normal/p0_ps_37.hlsl"
        #elif defined(_ADDITIONAL_LIGHTS) && defined(_DE_EDITOR_CAMERA)
        #include "LCArt_Character_shader_DE_Character_Normal/p0_ps_46.hlsl"
        #elif defined(_ADDITIONAL_LIGHTS) && defined(_DE_PRERENDERING_CAMERA)
        #include "LCArt_Character_shader_DE_Character_Normal/p0_ps_44.hlsl"
        #elif defined(_CLUSTER_LIGHT_LOOP) && defined(_DE_EDITOR_CAMERA)
        #include "LCArt_Character_shader_DE_Character_Normal/p0_ps_54.hlsl"
        #elif defined(_CLUSTER_LIGHT_LOOP) && defined(_DE_PRERENDERING_CAMERA)
        #include "LCArt_Character_shader_DE_Character_Normal/p0_ps_52.hlsl"
        #elif defined(_DE_EDITOR_CAMERA) && defined(_DE_PRERENDERING_CAMERA)
        #include "LCArt_Character_shader_DE_Character_Normal/p0_ps_40.hlsl"
        #elif defined(FOG_LINEAR)
        #include "LCArt_Character_shader_DE_Character_Normal/p0_ps_35.hlsl"
        #elif defined(_ADDITIONAL_LIGHTS)
        #include "LCArt_Character_shader_DE_Character_Normal/p0_ps_42.hlsl"
        #elif defined(_CLUSTER_LIGHT_LOOP)
        #include "LCArt_Character_shader_DE_Character_Normal/p0_ps_50.hlsl"
        #elif defined(_DE_EDITOR_CAMERA)
        #include "LCArt_Character_shader_DE_Character_Normal/p0_ps_38.hlsl"
        #elif defined(_DE_PRERENDERING_CAMERA)
        #include "LCArt_Character_shader_DE_Character_Normal/p0_ps_36.hlsl"
        #elif 1
        #include "LCArt_Character_shader_DE_Character_Normal/p0_ps_34.hlsl"
        #endif
        ENDHLSL
    }
    Pass
    {
        Name "SpineGrabpass"
        Tags { "LightMode" = "SpineGrabpass" "Queue" = "AlphaTest" "RenderPipeline" = "UniversalPipeline" }
        Blend SrcAlpha OneMinusSrcAlpha
        Cull Off
        Stencil {
          Ref 2
          Pass Replace
        }
        HLSLPROGRAM
        #pragma target 4.0
        #pragma vertex vert
        #pragma fragment frag
        #pragma multi_compile _ _DE_EDITOR_CAMERA
        #pragma multi_compile _ _DE_PRERENDERING_CAMERA

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
        int _MainLightLayerMask;
        float4 _Time;
        float4 _SinTime;
        float4 _CosTime;
        float3 _WorldSpaceCameraPos;
        float4 _ProjectionParams;
        float4 _ScreenParams;
        float4 unity_FogColor;
        float4x4 unity_MatrixV;
        float4x4 unity_MatrixVP;
        float4 _ScreenSize;
        float4 _GlobalCharacterShadowColor;
        float4 _GlobalCharacterOutOfColor;
        float _GlobalFogHeightEnable;
        float _GlobalFogHeightMin;
        float _GlobalFogHeightMax;
        float _GlobalIsRuntime;
        int _GlobalWaterAreaEnable;
        float _GlobalWaterSurfaceLevel;
        float4 _GlobalWaterShallowColor;
        float4 _GlobalWaterDeepColor;
        float4 _GlobalWaterShadowColor;
        float4 _GlobalWaterShallowShadowColor;
        float4 _GlobalWaterShoreEdge;
        float4 _GlobalWaterDepthRange;
        float4 _GlobalWaterSurfaceWaveMapParams;
        float4 _GlobalOverallMapBoundMin;
        float4 _GlobalOverallMapBoundMax;
        float _GlobalBoxAreaEnable;
        float4 _GlobalBoxAreaCenters[8];
        float4 _GlobalBoxAreaExtents[8];
        float _GlobalBoxAreaCoverCharacters[8];
        float3 _PlaneBoundMinPos;
        float3 _PlaneBoundMaxPos;
        float _GlobalPlaneSystemEnable;
        float _GlobalPlanePointBufferEnable;
        cbuffer UnityPerDraw
        {
            float4x4 unity_ObjectToWorld : packoffset(c0);
            float4x4 unity_WorldToObject : packoffset(c4);
            float4 unity_RenderingLayer : packoffset(c10);
            float4 unity_LightData : packoffset(c11);
        }
        cbuffer UnityPerMaterial
        {
            float4 _MainTex_ST : packoffset(c0);
            float4 _NormalTex_ST : packoffset(c1);
            float4 _MaskColor0 : packoffset(c3);
            float4 _MaskColor1 : packoffset(c4);
            float _MaskIntensity : packoffset(c5);
            float4 _EffectColor : packoffset(c8);
            float _Effect : packoffset(c9);
            float _LightEffectIntensity : packoffset(c9.y);
            float4 _DissolveColor : packoffset(c10);
            float _DissolveProgress : packoffset(c11);
            float _IsVertexColorAlphaUsed : packoffset(c11.y);
            float _GhostEnable : packoffset(c12.z);
            float _IsLocalGhost : packoffset(c12.w);
            float _OpticalRatioYThreshold : packoffset(c18);
            float _OpticalRatioYPositiveRatio : packoffset(c18.y);
            float _OpticalRatioYNegativeUnit : packoffset(c18.z);
        }

        Texture2D<float4> _AfterRenderingGrabpassTexture;
        Texture2D<float4> _GlobalWaterAreaMap;
        Texture2D<float4> _GlobalWaterSurfaceWaveMap;
        Texture2D<float4> _MainTex;
        Texture2D<float4> _MaskTex;
        Texture2D<float4> _PlanePointBuffer;
        Texture2D<float4> _PlaneSightRenderTarget;
        SamplerState sampler_AfterRenderingGrabpassTexture;
        SamplerState sampler_MainTex;
        SamplerState sampler_PlanePointBuffer;
        SamplerState sampler_PlaneSightRenderTarget;
        SamplerState sampler_vd_linear_clamp;
        SamplerState sampler_vd_point_repeat;

        // ---- progVertex
        #if defined(_DE_EDITOR_CAMERA) && defined(_DE_PRERENDERING_CAMERA)
        #include "LCArt_Character_shader_DE_Character_Normal/p1_vs_63.hlsl"
        #elif defined(_DE_EDITOR_CAMERA)
        #include "LCArt_Character_shader_DE_Character_Normal/p1_vs_62.hlsl"
        #elif defined(_DE_PRERENDERING_CAMERA)
        #include "LCArt_Character_shader_DE_Character_Normal/p1_vs_61.hlsl"
        #elif 1
        #include "LCArt_Character_shader_DE_Character_Normal/p1_vs_60.hlsl"
        #endif
        // ---- progFragment
        #if defined(_DE_EDITOR_CAMERA) && defined(_DE_PRERENDERING_CAMERA)
        #include "LCArt_Character_shader_DE_Character_Normal/p1_ps_68.hlsl"
        #elif defined(_DE_EDITOR_CAMERA)
        #include "LCArt_Character_shader_DE_Character_Normal/p1_ps_67.hlsl"
        #elif defined(_DE_PRERENDERING_CAMERA)
        #include "LCArt_Character_shader_DE_Character_Normal/p1_ps_66.hlsl"
        #elif 1
        #include "LCArt_Character_shader_DE_Character_Normal/p1_ps_65.hlsl"
        #endif
        ENDHLSL
    }
    Pass
    {
        Name "DepthNormals"
        Tags { "LightMode" = "DepthNormals" "Queue" = "Geometry" "RenderPipeline" = "UniversalPipeline" }
        Cull Off
        HLSLPROGRAM
        #pragma target 4.0
        #pragma vertex vert
        #pragma fragment frag
        #pragma multi_compile _ _DE_REFLECTION_CAMERA

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
        float _GlobalIsRuntime;
        float _GlobalBoxAreaEnable;
        float4 _GlobalBoxAreaCenters[8];
        float4 _GlobalBoxAreaExtents[8];
        float _GlobalBoxAreaCoverCharacters[8];
        float3 _PlaneBoundMinPos;
        float3 _PlaneBoundMaxPos;
        float _GlobalPlaneSystemEnable;
        float _GlobalPlanePointBufferEnable;
        cbuffer UnityPerDraw
        {
            float4x4 unity_ObjectToWorld : packoffset(c0);
        }
        cbuffer UnityPerMaterial
        {
            float4 _MainTex_ST : packoffset(c0);
            float _DissolveProgress : packoffset(c11);
            float _OpticalRatioYThreshold : packoffset(c18);
            float _OpticalRatioYPositiveRatio : packoffset(c18.y);
            float _OpticalRatioYNegativeUnit : packoffset(c18.z);
        }

        Texture2D<float4> _MainTex;
        Texture2D<float4> _PlanePointBuffer;
        Texture2D<float4> _PlaneSightRenderTarget;
        SamplerState sampler_MainTex;
        SamplerState sampler_PlanePointBuffer;
        SamplerState sampler_PlaneSightRenderTarget;

        // ---- progVertex
        #if defined(_DE_REFLECTION_CAMERA)
        #include "LCArt_Character_shader_DE_Character_Normal/p2_vs_72.hlsl"
        #elif 1
        #include "LCArt_Character_shader_DE_Character_Normal/p2_vs_71.hlsl"
        #endif
        // ---- progFragment
        #if defined(_DE_REFLECTION_CAMERA)
        #include "LCArt_Character_shader_DE_Character_Normal/p2_ps_74.hlsl"
        #elif 1
        #include "LCArt_Character_shader_DE_Character_Normal/p2_ps_73.hlsl"
        #endif
        ENDHLSL
    }
    Pass
    {
        Name "RimLight"
        Tags { "LightMode" = "RimLightTarget" "Queue" = "AlphaTest" "RenderPipeline" = "UniversalPipeline" }
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
        #pragma multi_compile _ _DE_EDITOR_CAMERA
        #pragma multi_compile _ _DE_PRERENDERING_CAMERA

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
        int _RimEffectCount;
        float4 _RimEffectColors[8];
        float4 _RimEffectPositions[8];
        float _RimEffectRanges[8];
        float _GlobalIsRuntime;
        float _GlobalBoxAreaEnable;
        float4 _GlobalBoxAreaCenters[8];
        float4 _GlobalBoxAreaExtents[8];
        float _GlobalBoxAreaCoverCharacters[8];
        float3 _PlaneBoundMinPos;
        float3 _PlaneBoundMaxPos;
        float _GlobalPlaneSystemEnable;
        float _GlobalPlanePointBufferEnable;
        cbuffer UnityPerDraw
        {
            float4x4 unity_ObjectToWorld : packoffset(c0);
        }
        cbuffer UnityPerMaterial
        {
            float _DissolveProgress : packoffset(c11);
            float _GhostEnable : packoffset(c12.z);
            float _IsLocalGhost : packoffset(c12.w);
            float _OutlineEnable : packoffset(c13);
            float _OpticalRatioYThreshold : packoffset(c18);
            float _OpticalRatioYPositiveRatio : packoffset(c18.y);
        }

        Texture2D<float4> _MainTex;
        Texture2D<float4> _PlanePointBuffer;
        Texture2D<float4> _PlaneSightRenderTarget;
        SamplerState sampler_MainTex;
        SamplerState sampler_PlanePointBuffer;
        SamplerState sampler_PlaneSightRenderTarget;

        // ---- progVertex
        #if defined(_ADDITIONAL_LIGHTS) && defined(_DE_EDITOR_CAMERA) && defined(_DE_PRERENDERING_CAMERA)
        #include "LCArt_Character_shader_DE_Character_Normal/p3_vs_86.hlsl"
        #elif defined(_CLUSTER_LIGHT_LOOP) && defined(_DE_EDITOR_CAMERA) && defined(_DE_PRERENDERING_CAMERA)
        #include "LCArt_Character_shader_DE_Character_Normal/p3_vs_90.hlsl"
        #elif defined(_ADDITIONAL_LIGHTS) && defined(_DE_EDITOR_CAMERA)
        #include "LCArt_Character_shader_DE_Character_Normal/p3_vs_85.hlsl"
        #elif defined(_ADDITIONAL_LIGHTS) && defined(_DE_PRERENDERING_CAMERA)
        #include "LCArt_Character_shader_DE_Character_Normal/p3_vs_84.hlsl"
        #elif defined(_CLUSTER_LIGHT_LOOP) && defined(_DE_EDITOR_CAMERA)
        #include "LCArt_Character_shader_DE_Character_Normal/p3_vs_89.hlsl"
        #elif defined(_CLUSTER_LIGHT_LOOP) && defined(_DE_PRERENDERING_CAMERA)
        #include "LCArt_Character_shader_DE_Character_Normal/p3_vs_88.hlsl"
        #elif defined(_DE_EDITOR_CAMERA) && defined(_DE_PRERENDERING_CAMERA)
        #include "LCArt_Character_shader_DE_Character_Normal/p3_vs_82.hlsl"
        #elif defined(_ADDITIONAL_LIGHTS)
        #include "LCArt_Character_shader_DE_Character_Normal/p3_vs_83.hlsl"
        #elif defined(_CLUSTER_LIGHT_LOOP)
        #include "LCArt_Character_shader_DE_Character_Normal/p3_vs_87.hlsl"
        #elif defined(_DE_EDITOR_CAMERA)
        #include "LCArt_Character_shader_DE_Character_Normal/p3_vs_81.hlsl"
        #elif defined(_DE_PRERENDERING_CAMERA)
        #include "LCArt_Character_shader_DE_Character_Normal/p3_vs_80.hlsl"
        #elif 1
        #include "LCArt_Character_shader_DE_Character_Normal/p3_vs_79.hlsl"
        #endif
        // ---- progFragment
        #if defined(_ADDITIONAL_LIGHTS) && defined(_DE_EDITOR_CAMERA) && defined(_DE_PRERENDERING_CAMERA)
        #include "LCArt_Character_shader_DE_Character_Normal/p3_ps_100.hlsl"
        #elif defined(_CLUSTER_LIGHT_LOOP) && defined(_DE_EDITOR_CAMERA) && defined(_DE_PRERENDERING_CAMERA)
        #include "LCArt_Character_shader_DE_Character_Normal/p3_ps_104.hlsl"
        #elif defined(_ADDITIONAL_LIGHTS) && defined(_DE_EDITOR_CAMERA)
        #include "LCArt_Character_shader_DE_Character_Normal/p3_ps_99.hlsl"
        #elif defined(_ADDITIONAL_LIGHTS) && defined(_DE_PRERENDERING_CAMERA)
        #include "LCArt_Character_shader_DE_Character_Normal/p3_ps_98.hlsl"
        #elif defined(_CLUSTER_LIGHT_LOOP) && defined(_DE_EDITOR_CAMERA)
        #include "LCArt_Character_shader_DE_Character_Normal/p3_ps_103.hlsl"
        #elif defined(_CLUSTER_LIGHT_LOOP) && defined(_DE_PRERENDERING_CAMERA)
        #include "LCArt_Character_shader_DE_Character_Normal/p3_ps_102.hlsl"
        #elif defined(_DE_EDITOR_CAMERA) && defined(_DE_PRERENDERING_CAMERA)
        #include "LCArt_Character_shader_DE_Character_Normal/p3_ps_96.hlsl"
        #elif defined(_ADDITIONAL_LIGHTS)
        #include "LCArt_Character_shader_DE_Character_Normal/p3_ps_97.hlsl"
        #elif defined(_CLUSTER_LIGHT_LOOP)
        #include "LCArt_Character_shader_DE_Character_Normal/p3_ps_101.hlsl"
        #elif defined(_DE_EDITOR_CAMERA)
        #include "LCArt_Character_shader_DE_Character_Normal/p3_ps_95.hlsl"
        #elif defined(_DE_PRERENDERING_CAMERA)
        #include "LCArt_Character_shader_DE_Character_Normal/p3_ps_94.hlsl"
        #elif 1
        #include "LCArt_Character_shader_DE_Character_Normal/p3_ps_93.hlsl"
        #endif
        ENDHLSL
    }
    Pass
    {
        Name "StencilColor"
        Tags { "IgnoreProjector" = "true" "LightMode" = "StencilColor" "Queue" = "Transparent" "RenderPipeline" = "UniversalPipeline" }
        ZTest Off
        ZWrite Off
        Cull Off
        HLSLPROGRAM
        #pragma target 4.0
        #pragma vertex vert
        #pragma fragment frag
        #pragma multi_compile _ FOG_LINEAR
        #pragma multi_compile _ _DE_EDITOR_CAMERA
        #pragma multi_compile _ _DE_PRERENDERING_CAMERA

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
        float4 _ScreenParams;
        float4x4 unity_MatrixV;
        float4x4 unity_MatrixVP;
        float4 _ScreenSize;
        float4 _GlobalCharacterStencilColor;
        float4 _GlobalEnemyStencilColor;
        float _GlobalIsRuntime;
        float _GlobalBoxAreaEnable;
        float4 _GlobalBoxAreaCenters[8];
        float4 _GlobalBoxAreaExtents[8];
        float _GlobalBoxAreaCoverCharacters[8];
        float3 _PlaneBoundMinPos;
        float3 _PlaneBoundMaxPos;
        float _GlobalPlaneSystemEnable;
        float _GlobalPlanePointBufferEnable;
        cbuffer UnityPerDraw
        {
            float4x4 unity_ObjectToWorld : packoffset(c0);
        }
        cbuffer UnityPerMaterial
        {
            float4 _MainTex_ST : packoffset(c0);
            float4 _NormalTex_ST : packoffset(c1);
            float _DissolveProgress : packoffset(c11);
            float _GhostEnable : packoffset(c12.z);
            float _IsEnemy : packoffset(c17.w);
            float _OpticalRatioYThreshold : packoffset(c18);
            float _OpticalRatioYPositiveRatio : packoffset(c18.y);
            float _OpticalRatioYNegativeUnit : packoffset(c18.z);
        }

        Texture2D<float4> _CustomDepthBuffer;
        Texture2D<float4> _MainTex;
        Texture2D<float4> _PlanePointBuffer;
        Texture2D<float4> _PlaneSightRenderTarget;
        SamplerState sampler_CustomDepthBuffer;
        SamplerState sampler_MainTex;
        SamplerState sampler_PlanePointBuffer;
        SamplerState sampler_PlaneSightRenderTarget;

        // ---- progVertex
        #if defined(FOG_LINEAR) && defined(_DE_EDITOR_CAMERA) && defined(_DE_PRERENDERING_CAMERA)
        #include "LCArt_Character_shader_DE_Character_Normal/p4_vs_114.hlsl"
        #elif defined(FOG_LINEAR) && defined(_DE_EDITOR_CAMERA)
        #include "LCArt_Character_shader_DE_Character_Normal/p4_vs_112.hlsl"
        #elif defined(FOG_LINEAR) && defined(_DE_PRERENDERING_CAMERA)
        #include "LCArt_Character_shader_DE_Character_Normal/p4_vs_110.hlsl"
        #elif defined(_DE_EDITOR_CAMERA) && defined(_DE_PRERENDERING_CAMERA)
        #include "LCArt_Character_shader_DE_Character_Normal/p4_vs_113.hlsl"
        #elif defined(FOG_LINEAR)
        #include "LCArt_Character_shader_DE_Character_Normal/p4_vs_108.hlsl"
        #elif defined(_DE_EDITOR_CAMERA)
        #include "LCArt_Character_shader_DE_Character_Normal/p4_vs_111.hlsl"
        #elif defined(_DE_PRERENDERING_CAMERA)
        #include "LCArt_Character_shader_DE_Character_Normal/p4_vs_109.hlsl"
        #elif 1
        #include "LCArt_Character_shader_DE_Character_Normal/p4_vs_107.hlsl"
        #endif
        // ---- progFragment
        #if defined(FOG_LINEAR) && defined(_DE_EDITOR_CAMERA) && defined(_DE_PRERENDERING_CAMERA)
        #include "LCArt_Character_shader_DE_Character_Normal/p4_ps_122.hlsl"
        #elif defined(FOG_LINEAR) && defined(_DE_EDITOR_CAMERA)
        #include "LCArt_Character_shader_DE_Character_Normal/p4_ps_120.hlsl"
        #elif defined(FOG_LINEAR) && defined(_DE_PRERENDERING_CAMERA)
        #include "LCArt_Character_shader_DE_Character_Normal/p4_ps_118.hlsl"
        #elif defined(_DE_EDITOR_CAMERA) && defined(_DE_PRERENDERING_CAMERA)
        #include "LCArt_Character_shader_DE_Character_Normal/p4_ps_121.hlsl"
        #elif defined(FOG_LINEAR)
        #include "LCArt_Character_shader_DE_Character_Normal/p4_ps_116.hlsl"
        #elif defined(_DE_EDITOR_CAMERA)
        #include "LCArt_Character_shader_DE_Character_Normal/p4_ps_119.hlsl"
        #elif defined(_DE_PRERENDERING_CAMERA)
        #include "LCArt_Character_shader_DE_Character_Normal/p4_ps_117.hlsl"
        #elif 1
        #include "LCArt_Character_shader_DE_Character_Normal/p4_ps_115.hlsl"
        #endif
        ENDHLSL
    }
    Pass
    {
        Name "Reflection"
        Tags { "LightMode" = "Reflection" "Queue" = "AlphaTest" "RenderPipeline" = "UniversalPipeline" }
        ZWrite Off
        Cull Off
        HLSLPROGRAM
        #pragma target 4.0
        #pragma vertex vert
        #pragma fragment frag
        #pragma multi_compile _ _DE_EDITOR_CAMERA
        #pragma multi_compile _ _DE_PRERENDERING_CAMERA

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
        float4 _ScreenParams;
        float4 unity_FogColor;
        float4x4 unity_MatrixV;
        float4x4 unity_MatrixVP;
        float4 _ScreenSize;
        float4 _GlobalCharacterOutOfColor;
        float _PlaneReflectionPivotHeight;
        float4 _PlaneReflectionBackgroundColor;
        float _GlobalFogHeightEnable;
        float _GlobalFogHeightMin;
        float _GlobalFogHeightMax;
        float _GlobalIsRuntime;
        float _GlobalBoxAreaEnable;
        float4 _GlobalBoxAreaCenters[8];
        float4 _GlobalBoxAreaExtents[8];
        float _GlobalBoxAreaCoverCharacters[8];
        float3 _PlaneBoundMinPos;
        float3 _PlaneBoundMaxPos;
        float _GlobalPlaneSystemEnable;
        float _GlobalPlanePointBufferEnable;
        cbuffer UnityPerDraw
        {
            float4x4 unity_ObjectToWorld : packoffset(c0);
            float4x4 unity_WorldToObject : packoffset(c4);
        }
        cbuffer UnityPerMaterial
        {
            float4 _MainTex_ST : packoffset(c0);
            float4 _NormalTex_ST : packoffset(c1);
            float _Effect : packoffset(c9);
            float4 _DissolveColor : packoffset(c10);
            float _DissolveProgress : packoffset(c11);
            float _GhostEnable : packoffset(c12.z);
            float _IsReflected : packoffset(c15.y);
            float _OpticalRatioYThreshold : packoffset(c18);
            float _OpticalRatioYPositiveRatio : packoffset(c18.y);
            float _OpticalRatioYNegativeUnit : packoffset(c18.z);
        }

        Texture2D<float4> _MainTex;
        Texture2D<float4> _PlanePointBuffer;
        Texture2D<float4> _PlaneSightRenderTarget;
        SamplerState sampler_MainTex;
        SamplerState sampler_PlanePointBuffer;
        SamplerState sampler_PlaneSightRenderTarget;

        // ---- progVertex
        #if defined(_DE_EDITOR_CAMERA) && defined(_DE_PRERENDERING_CAMERA)
        #include "LCArt_Character_shader_DE_Character_Normal/p5_vs_126.hlsl"
        #elif defined(_DE_EDITOR_CAMERA)
        #include "LCArt_Character_shader_DE_Character_Normal/p5_vs_125.hlsl"
        #elif defined(_DE_PRERENDERING_CAMERA)
        #include "LCArt_Character_shader_DE_Character_Normal/p5_vs_124.hlsl"
        #elif 1
        #include "LCArt_Character_shader_DE_Character_Normal/p5_vs_123.hlsl"
        #endif
        // ---- progFragment
        #if defined(_DE_EDITOR_CAMERA) && defined(_DE_PRERENDERING_CAMERA)
        #include "LCArt_Character_shader_DE_Character_Normal/p5_ps_130.hlsl"
        #elif defined(_DE_EDITOR_CAMERA)
        #include "LCArt_Character_shader_DE_Character_Normal/p5_ps_129.hlsl"
        #elif defined(_DE_PRERENDERING_CAMERA)
        #include "LCArt_Character_shader_DE_Character_Normal/p5_ps_128.hlsl"
        #elif 1
        #include "LCArt_Character_shader_DE_Character_Normal/p5_ps_127.hlsl"
        #endif
        ENDHLSL
    }
}
}
