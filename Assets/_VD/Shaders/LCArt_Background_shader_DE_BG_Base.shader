Shader "LCArt/Background/shader_DE_BG_Base"
{
Properties
{
    [MainTexture] [Header (______________________________________________________________________________________________________________________)] [Header (Common)] [Space (10)] _BaseMap ("Base Map (RGB)", 2D) = "white" {}
    [MainColor] _BaseColor ("Base Color", Color) = (1, 1, 1, 1)
    _Cutoff ("Alpha Clipping", Range(0, 1)) = 0.5
    [NoScaleOffset] _BumpMap ("Normal Map", 2D) = "bump" {}
    _BumpScale ("Normal Map Intensity", Range(0, 10)) = 1
    [NoScaleOffset] _WorldNormalMap ("World Normal Map", 2D) = "bump" {}
    [Toggle] _UseWorldNormalMap ("Use World Normal Map", Float) = 0
    [NoScaleOffset] _EmissionMap ("Multi Map (R: Emission, G: Dissolve Mask, B: Glass, RB: Fresnel)", 2D) = "black" {}
    _EmissionIntensity ("Emission Intensity", Range(0, 100)) = 0
    _EmissionFlickRate ("Emission Flick Rate", Range(0, 1)) = 0
    [HideInInspector] _Surface ("__surface", Float) = 0
    [HideInInspector] _Blend ("__blend", Float) = 0
    [HideInInspector] _Cull ("__cull", Float) = 2
    [HideInInspector] [ToggleUI] _AlphaClip ("__clip", Float) = 0
    [HideInInspector] _SrcBlend ("__src", Float) = 1
    [HideInInspector] _DstBlend ("__dst", Float) = 0
    [HideInInspector] _SrcBlendAlpha ("__srcA", Float) = 1
    [HideInInspector] _DstBlendAlpha ("__dstA", Float) = 0
    [HideInInspector] _ZWrite ("__zw", Float) = 1
    [HideInInspector] _BlendModePreserveSpecular ("_BlendModePreserveSpecular", Float) = 1
    [HideInInspector] _AlphaToMask ("__alphaToMask", Float) = 0
    [HideInInspector] _QueueOffset ("Queue offset", Float) = 0
    [HideInInspector] _MainTex ("BaseMap", 2D) = "white" {}
    [HideInInspector] _Color ("Base Color", Color) = (1, 1, 1, 1)
    [HideInInspector] _Shininess ("Smoothness", Float) = 0
    [HideInInspector] _GlossinessSource ("GlossinessSource", Float) = 0
    [HideInInspector] _SpecSource ("SpecularHighlights", Float) = 0
    [HideInInspector] [NoScaleOffset] unity_Lightmaps ("unity_Lightmaps", 2DArray) = "" {}
    [HideInInspector] [NoScaleOffset] unity_LightmapsInd ("unity_LightmapsInd", 2DArray) = "" {}
    [HideInInspector] [NoScaleOffset] unity_ShadowMasks ("unity_ShadowMasks", 2DArray) = "" {}
    [HideInInspector] [Space (20)] [Header (______________________________________________________________________________________________________________________)] [Header (Shadow)] [Space (10)] _ShadowColor ("Shadow Color", Color) = (0.5, 0.5, 0.6000000238418579, 1)
    [Space (20)] [Header (______________________________________________________________________________________________________________________)] [Header (Dirt)] [Space (10)] [Toggle] _UseDirtNoise ("Use Dirt Noise", Float) = 0
    [NoScaleOffset] _DirtNoiseTexture ("Dirt Noise Texture", 2D) = "white" {}
    _DirtNoiseColor ("Dirt Color", Color) = (0, 0, 0, 0.5)
    _DirtNoiseTiling ("Dirt Pattern Tiling", Float) = 1
    [HideInInspector] _OutlineColor ("Outline Color", Color) = (0, 0, 0, 1)
    [HideInInspector] [Toggle] _OutlineEnable ("Outline Enable", Float) = 0
    [HideInInspector] _OutlineStroke ("Outline Storke", Float) = 0.125
    [Space (20)] [Header (______________________________________________________________________________________________________________________)] [Header (Dissolve)] [Space (10)] _DissolveProgress ("Dissolve Progress", Range(0, 1)) = 0
    _DissolveEdgeColor ("Dissolve Edge Color", Color) = (1, 1, 1, 1)
    [HideInInspector] _AOEnable ("AO Enable", Float) = 1
    [HideInInspector] _Smoothness ("Smoothness", Float) = 0.5
    [HideInInspector] _SpecIntensity ("Spec Intensity", Float) = 1
    [HideInInspector] _SanityRandomSeed ("Sanity Random Seed", Float) = 0
    [HideInInspector] _SanityDefaultDegree ("Sanity Default Degree", Float) = 0
    [HideInInspector] _CutoffMaskPivotOffset ("Cutoff Mask Pivot Offset", Vector) = (0, 0, 0, 0)
    [HideInInspector] _CutoffMaskBump ("Cutoff Mask Bump", Float) = 0
    [Space (20)] [Header (______________________________________________________________________________________________________________________)] [Header (Snapshot)] [Space (10)] _SnapshotType ("Snapshot Type", Float) = 0
    [HideInInspector] [Space (20)] [Header (______________________________________________________________________________________________________________________)] [Header (Stencil)] [Space (10)] _StencilId ("Stencil Id", Float) = 15
    _BackgroundType ("Background Type", Float) = 0
}
SubShader
{
    LOD 300
    Tags { "IgnoreProjector" = "true" "RenderPipeline" = "UniversalPipeline" "RenderType" = "Opaque" "UniversalMaterialType" = "SimpleLit" }
    Pass
    {
        Name "ForwardLit"
        Tags { "IgnoreProjector" = "true" "LightMode" = "UniversalForward" "RenderPipeline" = "UniversalPipeline" "RenderType" = "Opaque" "UniversalMaterialType" = "SimpleLit" }
        AlphaToMask [_AlphaToMask]
        Stencil {
          Ref [_StencilId]
          Comp NotEqual
        }
        HLSLPROGRAM
        #pragma target 4.0
        #pragma vertex vert
        #pragma fragment frag
        #pragma multi_compile _ FOG_LINEAR
        #pragma multi_compile _ _ADDITIONAL_LIGHTS
        #pragma multi_compile _ _CLUSTER_LIGHT_LOOP
        #pragma multi_compile _ _CURSOR_LINE_ENABLE
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
        float _GlobalCoverSeeThroughFactor;
        float _GlobalIsRuntime;
        float4 _GlobalBackgroundColor;
        float _GlobalBackgroundColorIntensity;
        float _GlobalWindEnable;
        float _GlobalWindIntensity;
        float _GlobalBackgroundSeeThroughEnable;
        float _GlobalSeeThroughAlpha;
        float _GlobalSeeThroughCircleAlpha;
        float _GlobalSeeThroughCircleRange;
        float _GlobalSeeThroughHeightMax;
        float _GlobalSeeThroughHeightMin;
        float _GlobalSeeThroughBuildingAlpha;
        float _GlobalSeeThroughBuildingHeightMax;
        float _GlobalSeeThroughBuildingHeightMin;
        float4 _GlobalBackgroundShadowColor;
        float _IsSingleObject;
        float3 _ObjectBoundSize;
        float _GlobalFogHeightEnable;
        float _GlobalFogHeightMax;
        float _GlobalFogHeightMin;
        int _GlobalWaterAreaEnable;
        float _GlobalWaterSurfaceLevel;
        float4 _GlobalWaterShallowColor;
        float4 _GlobalWaterDeepColor;
        float4 _GlobalWaterShoreEdge;
        float _GlobalWaterShoreStroke;
        float _GlobalWaterShoreIntensity;
        float _GlobalWaterShoreHeightDrop;
        float _GlobalWaterSurfaceEdgeWidth;
        float _GlobalWaterDepthOffset;
        float4 _GlobalWaterWaveParams;
        float4 _GlobalWaterWaveParams2;
        float4 _GlobalWaterDepthRange;
        float _GlobalWaterReflectionIntensity;
        float _GlobalWaterReflectionYOffset;
        int _PlaneReflectionActive;
        float _PlaneReflectionOpacity;
        float4 _GlobalWaterSurfaceWaveMapParams;
        float4 _GlobalOverallMapBoundMin;
        float4 _GlobalOverallMapBoundMax;
        float _WeatherIntensity;
        float _WeatherIndoorMask;
        float _WeatherWetSurfaceIntensity;
        float4 _WeatherBgTintColor;
        float _WeatherBgTintIntensity;
        float _WeatherBgDesaturation;
        float3 _GlobalPlayerPosition;
        float3 _GlobalCursorPosition;
        float4 _GlobalCursorLineColor;
        float _GlobalCursorLineThickness;
        float _GlobalCursorLineSmoothness;
        float _GlobalCursorPlayerRadius;
        float _GlobalCursorPlayerLineThickness;
        float _GlobalCursorPlayerLineSmoothness;
        float _GlobalCursorRadius;
        float4 _GlobalBufferPoints_Character[8];
        float3 _PlaneBoundMinPos;
        float3 _PlaneBoundMaxPos;
        float _GlobalPlaneSystemEnable;
        float4 _SightPositions[3];
        float _SeeThroughDistanceMin;
        float _SeeThroughDistanceMax;
        float3 _SanityPointPositions[8];
        float _SanityPointRadiuses[8];
        float _SanityPointIntensities[8];
        float _GlobalStressFloatingEnableThreshold;
        float4 _GlobalStressFloatingPosition;
        float4 _GlobalStressFloatingAdditionalPosition;
        float _GlobalStressFloatingRotation;
        float _GlobalStressFloatingPositionPeriodMin;
        float _GlobalStressFloatingPositionPeriodMax;
        float _GlobalStressFloatingRotationPeriodMin;
        float _GlobalStressFloatingRotationPeriodMax;
        float _GlobalBoxAreaEnable;
        float4 _GlobalBoxAreaCenters[8];
        float4 _GlobalBoxAreaExtents[8];
        float _GlobalBoxAreaIntensities[8];
        float4 _GlobalBoxAreaColors[8];
        float4 _GlobalBoxAreaShimmerParams[8];
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
            float4 _BaseColor : packoffset(c1);
            float _EmissionIntensity : packoffset(c2);
            float _EmissionFlickRate : packoffset(c2.y);
            float _Surface : packoffset(c2.w);
            float _BumpScale : packoffset(c3);
            float _UseDirtNoise : packoffset(c6.z);
            float4 _DirtNoiseColor : packoffset(c7);
            float _SanityRandomSeed : packoffset(c8.y);
            float _SanityDefaultDegree : packoffset(c8.z);
            float _UseWorldNormalMap : packoffset(c10.y);
            int _BackgroundType : packoffset(c11);
            float _DissolveProgress : packoffset(c11.y);
            float4 _DissolveEdgeColor : packoffset(c12);
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
        Texture2D<float4> _BumpMap;
        Texture2D<float4> _CharacterAreaMaskTexture;
        Texture2D<float4> _CustomDepthBuffer;
        Texture2D<float4> _DirtNoiseTexture;
        Texture2D<float4> _EmissionMap;
        Texture2D<float4> _GlobalWaterAreaMap;
        Texture2D<float4> _GlobalWaterNoiseTex;
        Texture2D<float4> _GlobalWaterNormalMap;
        Texture2D<float4> _GlobalWaterSurfaceWaveMap;
        Texture2D<float4> _PlaneReflectionTexture;
        Texture2D<float4> _PlaneSightRenderTarget;
        Texture2D<float4> _WorldNormalMap;
        SamplerState sampler_BaseMap;
        SamplerState sampler_CharacterAreaMaskTexture;
        SamplerState sampler_CustomDepthBuffer;
        SamplerState sampler_PlaneSightRenderTarget;
        SamplerState sampler_vd_linear_clamp;
        SamplerState sampler_vd_linear_repeat;
        SamplerState sampler_vd_point_repeat;

        // ---- progVertex
        #if defined(FOG_LINEAR) && defined(_ADDITIONAL_LIGHTS) && defined(_DE_EDITOR_CAMERA) && defined(_LIGHT_LAYERS)
        #include "LCArt_Background_shader_DE_BG_Base/p0_vs_13.hlsl"
        #elif defined(FOG_LINEAR) && defined(_CLUSTER_LIGHT_LOOP) && defined(_DE_EDITOR_CAMERA) && defined(_LIGHT_LAYERS)
        #include "LCArt_Background_shader_DE_BG_Base/p0_vs_9.hlsl"
        #elif defined(FOG_LINEAR) && defined(_ADDITIONAL_LIGHTS) && defined(_LIGHT_LAYERS)
        #include "LCArt_Background_shader_DE_BG_Base/p0_vs_11.hlsl"
        #elif defined(FOG_LINEAR) && defined(_CLUSTER_LIGHT_LOOP) && defined(_LIGHT_LAYERS)
        #include "LCArt_Background_shader_DE_BG_Base/p0_vs_7.hlsl"
        #elif defined(FOG_LINEAR) && defined(_DE_EDITOR_CAMERA) && defined(_LIGHT_LAYERS)
        #include "LCArt_Background_shader_DE_BG_Base/p0_vs_5.hlsl"
        #elif defined(_ADDITIONAL_LIGHTS) && defined(_DE_EDITOR_CAMERA) && defined(_LIGHT_LAYERS)
        #include "LCArt_Background_shader_DE_BG_Base/p0_vs_12.hlsl"
        #elif defined(_CLUSTER_LIGHT_LOOP) && defined(_DE_EDITOR_CAMERA) && defined(_LIGHT_LAYERS)
        #include "LCArt_Background_shader_DE_BG_Base/p0_vs_8.hlsl"
        #elif defined(FOG_LINEAR) && defined(_LIGHT_LAYERS)
        #include "LCArt_Background_shader_DE_BG_Base/p0_vs_3.hlsl"
        #elif defined(_ADDITIONAL_LIGHTS) && defined(_LIGHT_LAYERS)
        #include "LCArt_Background_shader_DE_BG_Base/p0_vs_10.hlsl"
        #elif defined(_CLUSTER_LIGHT_LOOP) && defined(_LIGHT_LAYERS)
        #include "LCArt_Background_shader_DE_BG_Base/p0_vs_6.hlsl"
        #elif defined(_DE_EDITOR_CAMERA) && defined(_LIGHT_LAYERS)
        #include "LCArt_Background_shader_DE_BG_Base/p0_vs_4.hlsl"
        #elif defined(_LIGHT_LAYERS)
        #include "LCArt_Background_shader_DE_BG_Base/p0_vs_2.hlsl"
        #else
        #include "LCArt_Background_shader_DE_BG_Base/p0_vs_2.hlsl"
        #endif
        // ---- progFragment
        #if defined(FOG_LINEAR) && defined(_ADDITIONAL_LIGHTS) && defined(_CURSOR_LINE_ENABLE) && defined(_DE_EDITOR_CAMERA) && defined(_LIGHT_LAYERS)
        #include "LCArt_Background_shader_DE_BG_Base/p0_ps_44.hlsl"
        #elif defined(FOG_LINEAR) && defined(_CLUSTER_LIGHT_LOOP) && defined(_CURSOR_LINE_ENABLE) && defined(_DE_EDITOR_CAMERA) && defined(_LIGHT_LAYERS)
        #include "LCArt_Background_shader_DE_BG_Base/p0_ps_55.hlsl"
        #elif defined(FOG_LINEAR) && defined(_ADDITIONAL_LIGHTS) && defined(_CURSOR_LINE_ENABLE) && defined(_LIGHT_LAYERS)
        #include "LCArt_Background_shader_DE_BG_Base/p0_ps_46.hlsl"
        #elif defined(FOG_LINEAR) && defined(_ADDITIONAL_LIGHTS) && defined(_DE_EDITOR_CAMERA) && defined(_LIGHT_LAYERS)
        #include "LCArt_Background_shader_DE_BG_Base/p0_ps_43.hlsl"
        #elif defined(FOG_LINEAR) && defined(_CLUSTER_LIGHT_LOOP) && defined(_CURSOR_LINE_ENABLE) && defined(_LIGHT_LAYERS)
        #include "LCArt_Background_shader_DE_BG_Base/p0_ps_53.hlsl"
        #elif defined(FOG_LINEAR) && defined(_CLUSTER_LIGHT_LOOP) && defined(_DE_EDITOR_CAMERA) && defined(_LIGHT_LAYERS)
        #include "LCArt_Background_shader_DE_BG_Base/p0_ps_39.hlsl"
        #elif defined(FOG_LINEAR) && defined(_CURSOR_LINE_ENABLE) && defined(_DE_EDITOR_CAMERA) && defined(_LIGHT_LAYERS)
        #include "LCArt_Background_shader_DE_BG_Base/p0_ps_51.hlsl"
        #elif defined(_ADDITIONAL_LIGHTS) && defined(_CURSOR_LINE_ENABLE) && defined(_DE_EDITOR_CAMERA) && defined(_LIGHT_LAYERS)
        #include "LCArt_Background_shader_DE_BG_Base/p0_ps_45.hlsl"
        #elif defined(_CLUSTER_LIGHT_LOOP) && defined(_CURSOR_LINE_ENABLE) && defined(_DE_EDITOR_CAMERA) && defined(_LIGHT_LAYERS)
        #include "LCArt_Background_shader_DE_BG_Base/p0_ps_54.hlsl"
        #elif defined(FOG_LINEAR) && defined(_ADDITIONAL_LIGHTS) && defined(_LIGHT_LAYERS)
        #include "LCArt_Background_shader_DE_BG_Base/p0_ps_41.hlsl"
        #elif defined(FOG_LINEAR) && defined(_CLUSTER_LIGHT_LOOP) && defined(_LIGHT_LAYERS)
        #include "LCArt_Background_shader_DE_BG_Base/p0_ps_37.hlsl"
        #elif defined(FOG_LINEAR) && defined(_CURSOR_LINE_ENABLE) && defined(_LIGHT_LAYERS)
        #include "LCArt_Background_shader_DE_BG_Base/p0_ps_49.hlsl"
        #elif defined(FOG_LINEAR) && defined(_DE_EDITOR_CAMERA) && defined(_LIGHT_LAYERS)
        #include "LCArt_Background_shader_DE_BG_Base/p0_ps_35.hlsl"
        #elif defined(_ADDITIONAL_LIGHTS) && defined(_CURSOR_LINE_ENABLE) && defined(_LIGHT_LAYERS)
        #include "LCArt_Background_shader_DE_BG_Base/p0_ps_47.hlsl"
        #elif defined(_ADDITIONAL_LIGHTS) && defined(_DE_EDITOR_CAMERA) && defined(_LIGHT_LAYERS)
        #include "LCArt_Background_shader_DE_BG_Base/p0_ps_42.hlsl"
        #elif defined(_CLUSTER_LIGHT_LOOP) && defined(_CURSOR_LINE_ENABLE) && defined(_LIGHT_LAYERS)
        #include "LCArt_Background_shader_DE_BG_Base/p0_ps_52.hlsl"
        #elif defined(_CLUSTER_LIGHT_LOOP) && defined(_DE_EDITOR_CAMERA) && defined(_LIGHT_LAYERS)
        #include "LCArt_Background_shader_DE_BG_Base/p0_ps_38.hlsl"
        #elif defined(_CURSOR_LINE_ENABLE) && defined(_DE_EDITOR_CAMERA) && defined(_LIGHT_LAYERS)
        #include "LCArt_Background_shader_DE_BG_Base/p0_ps_50.hlsl"
        #elif defined(FOG_LINEAR) && defined(_LIGHT_LAYERS)
        #include "LCArt_Background_shader_DE_BG_Base/p0_ps_33.hlsl"
        #elif defined(_ADDITIONAL_LIGHTS) && defined(_LIGHT_LAYERS)
        #include "LCArt_Background_shader_DE_BG_Base/p0_ps_40.hlsl"
        #elif defined(_CLUSTER_LIGHT_LOOP) && defined(_LIGHT_LAYERS)
        #include "LCArt_Background_shader_DE_BG_Base/p0_ps_36.hlsl"
        #elif defined(_CURSOR_LINE_ENABLE) && defined(_LIGHT_LAYERS)
        #include "LCArt_Background_shader_DE_BG_Base/p0_ps_48.hlsl"
        #elif defined(_DE_EDITOR_CAMERA) && defined(_LIGHT_LAYERS)
        #include "LCArt_Background_shader_DE_BG_Base/p0_ps_34.hlsl"
        #elif defined(_LIGHT_LAYERS)
        #include "LCArt_Background_shader_DE_BG_Base/p0_ps_32.hlsl"
        #else
        #include "LCArt_Background_shader_DE_BG_Base/p0_ps_32.hlsl"
        #endif
        ENDHLSL
    }
    Pass
    {
        Name "DepthNormals"
        Tags { "IgnoreProjector" = "true" "LightMode" = "DepthNormals" "RenderPipeline" = "UniversalPipeline" "RenderType" = "Opaque" "UniversalMaterialType" = "SimpleLit" }
        Cull Off
        Stencil {
          Ref [_StencilId]
          Comp NotEqual
        }
        HLSLPROGRAM
        #pragma target 4.0
        #pragma vertex vert
        #pragma fragment frag
        #pragma multi_compile _ _DE_EDITOR_CAMERA
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
        float4 _ScaledScreenParams;
        float2 _GlobalMipBias;
        float4 _Time;
        float3 _WorldSpaceCameraPos;
        float4 _ScreenParams;
        float4 _ScaleBiasRt;
        float4x4 unity_MatrixV;
        float4x4 unity_MatrixVP;
        float4 _ScreenSize;
        float _GlobalCoverSeeThroughFactor;
        float _GlobalIsRuntime;
        float _GlobalWindEnable;
        float _GlobalWindIntensity;
        float _GlobalBackgroundSeeThroughEnable;
        float _GlobalSeeThroughAlpha;
        float _GlobalSeeThroughCircleAlpha;
        float _GlobalSeeThroughCircleRange;
        float _GlobalSeeThroughHeightMax;
        float _GlobalSeeThroughHeightMin;
        float _GlobalSeeThroughBuildingAlpha;
        float _GlobalSeeThroughBuildingHeightMax;
        float _GlobalSeeThroughBuildingHeightMin;
        float _IsSingleObject;
        float3 _ObjectBoundSize;
        float4 _GlobalBufferPoints_Character[8];
        float3 _SanityPointPositions[8];
        float _SanityPointRadiuses[8];
        float _SanityPointIntensities[8];
        float _GlobalStressFloatingEnableThreshold;
        float4 _GlobalStressFloatingPosition;
        float4 _GlobalStressFloatingAdditionalPosition;
        float _GlobalStressFloatingRotation;
        float _GlobalStressFloatingPositionPeriodMin;
        float _GlobalStressFloatingPositionPeriodMax;
        float _GlobalStressFloatingRotationPeriodMin;
        float _GlobalStressFloatingRotationPeriodMax;
        float _GlobalBoxAreaEnable;
        float4 _GlobalBoxAreaCenters[8];
        float4 _GlobalBoxAreaExtents[8];
        float _GlobalBoxAreaIntensities[8];
        float3 _PlaneBoundMinPos;
        float3 _PlaneBoundMaxPos;
        float _GlobalPlaneSystemEnable;
        float4 _SightPositions[3];
        float _SeeThroughDistanceMin;
        float _SeeThroughDistanceMax;
        cbuffer UnityPerDraw
        {
            float4x4 unity_ObjectToWorld : packoffset(c0);
            float4x4 unity_WorldToObject : packoffset(c4);
            float4 unity_WorldTransformParams : packoffset(c9);
        }
        cbuffer UnityPerMaterial
        {
            float4 _BaseMap_ST : packoffset(c0);
            float4 _BaseColor : packoffset(c1);
            float _BumpScale : packoffset(c3);
            float _SanityRandomSeed : packoffset(c8.y);
            float _SanityDefaultDegree : packoffset(c8.z);
            float _UseWorldNormalMap : packoffset(c10.y);
            int _BackgroundType : packoffset(c11);
            float _DissolveProgress : packoffset(c11.y);
        }

        Texture2D<float4> _BaseMap;
        Texture2D<float4> _BumpMap;
        Texture2D<float4> _CharacterAreaMaskTexture;
        Texture2D<float4> _CustomDepthBuffer;
        Texture2D<float4> _EmissionMap;
        Texture2D<float4> _PlaneSightRenderTarget;
        Texture2D<float4> _WorldNormalMap;
        SamplerState sampler_BaseMap;
        SamplerState sampler_CharacterAreaMaskTexture;
        SamplerState sampler_CustomDepthBuffer;
        SamplerState sampler_EmissionMap;
        SamplerState sampler_PlaneSightRenderTarget;
        SamplerState sampler_vd_point_repeat;

        // ---- progVertex
        #if defined(_DE_EDITOR_CAMERA) && defined(_DE_REFLECTION_CAMERA)
        #include "LCArt_Background_shader_DE_BG_Base/p1_vs_61.hlsl"
        #elif defined(_DE_EDITOR_CAMERA)
        #include "LCArt_Background_shader_DE_BG_Base/p1_vs_60.hlsl"
        #elif defined(_DE_REFLECTION_CAMERA)
        #include "LCArt_Background_shader_DE_BG_Base/p1_vs_59.hlsl"
        #elif 1
        #include "LCArt_Background_shader_DE_BG_Base/p1_vs_58.hlsl"
        #endif
        // ---- progFragment
        #if defined(_DE_EDITOR_CAMERA) && defined(_DE_REFLECTION_CAMERA)
        #include "LCArt_Background_shader_DE_BG_Base/p1_ps_67.hlsl"
        #elif defined(_DE_EDITOR_CAMERA)
        #include "LCArt_Background_shader_DE_BG_Base/p1_ps_66.hlsl"
        #elif defined(_DE_REFLECTION_CAMERA)
        #include "LCArt_Background_shader_DE_BG_Base/p1_ps_65.hlsl"
        #elif 1
        #include "LCArt_Background_shader_DE_BG_Base/p1_ps_64.hlsl"
        #endif
        ENDHLSL
    }
    Pass
    {
        Name "CustomDepth"
        Tags { "IgnoreProjector" = "true" "LightMode" = "CustomDepth" "RenderPipeline" = "UniversalPipeline" "RenderType" = "Opaque" "UniversalMaterialType" = "SimpleLit" }
        BlendOp Max
        ColorMask RG
        Stencil {
          Ref [_StencilId]
          Comp NotEqual
        }
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
        float4 _ScaledScreenParams;
        float2 _GlobalMipBias;
        float4 _Time;
        float3 _WorldSpaceCameraPos;
        float4 _ScaleBiasRt;
        float4x4 unity_MatrixV;
        float4x4 unity_MatrixVP;
        float _GlobalCoverSeeThroughFactor;
        float _GlobalIsRuntime;
        float _GlobalWindEnable;
        float _GlobalWindIntensity;
        float _GlobalBackgroundSeeThroughEnable;
        float _GlobalAlwaysStencil;
        float _GlobalSeeThroughBuildingAlpha;
        float _GlobalSeeThroughBuildingHeightMax;
        float _GlobalSeeThroughBuildingHeightMin;
        float _IsSingleObject;
        float3 _ObjectBoundSize;
        float4 _GlobalBufferPoints_Character[8];
        float3 _SanityPointPositions[8];
        float _SanityPointRadiuses[8];
        float _SanityPointIntensities[8];
        float _GlobalStressFloatingEnableThreshold;
        float4 _GlobalStressFloatingPosition;
        float4 _GlobalStressFloatingAdditionalPosition;
        float _GlobalStressFloatingRotation;
        float _GlobalStressFloatingPositionPeriodMin;
        float _GlobalStressFloatingPositionPeriodMax;
        float _GlobalStressFloatingRotationPeriodMin;
        float _GlobalStressFloatingRotationPeriodMax;
        float _GlobalBoxAreaEnable;
        float4 _GlobalBoxAreaCenters[8];
        float4 _GlobalBoxAreaExtents[8];
        float _GlobalBoxAreaIntensities[8];
        cbuffer UnityPerDraw
        {
            float4x4 unity_ObjectToWorld : packoffset(c0);
            float4x4 unity_WorldToObject : packoffset(c4);
            float4 unity_WorldTransformParams : packoffset(c9);
        }
        cbuffer UnityPerMaterial
        {
            float4 _BaseMap_ST : packoffset(c0);
            float _SanityRandomSeed : packoffset(c8.y);
            float _SanityDefaultDegree : packoffset(c8.z);
            int _BackgroundType : packoffset(c11);
            float _DissolveProgress : packoffset(c11.y);
        }

        Texture2D<float4> _BaseMap;
        Texture2D<float4> _CharacterAreaMaskTexture;
        SamplerState sampler_BaseMap;
        SamplerState sampler_CharacterAreaMaskTexture;

        // ---- progVertex
        #if defined(_DE_REFLECTION_CAMERA)
        #include "LCArt_Background_shader_DE_BG_Base/p2_vs_59.hlsl"
        #elif 1
        #include "LCArt_Background_shader_DE_BG_Base/p2_vs_58.hlsl"
        #endif
        // ---- progFragment
        #if defined(_DE_REFLECTION_CAMERA)
        #include "LCArt_Background_shader_DE_BG_Base/p2_ps_70.hlsl"
        #elif 1
        #include "LCArt_Background_shader_DE_BG_Base/p2_ps_69.hlsl"
        #endif
        ENDHLSL
    }
    Pass
    {
        Name "Reflection"
        Tags { "IgnoreProjector" = "true" "LightMode" = "Reflection" "RenderPipeline" = "UniversalPipeline" "RenderType" = "Opaque" "UniversalMaterialType" = "SimpleLit" }
        Stencil {
          Ref [_StencilId]
          Comp NotEqual
        }
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
        float4x4 unity_MatrixVP;
        float _PlaneReflectionPivotHeight;
        float4 _PlaneReflectionBackgroundColor;
        cbuffer UnityPerDraw
        {
            float4x4 unity_ObjectToWorld : packoffset(c0);
            float4x4 unity_WorldToObject : packoffset(c4);
        }
        cbuffer UnityPerMaterial
        {
            float4 _BaseColor : packoffset(c1);
            float _EmissionIntensity : packoffset(c2);
        }

        Texture2D<float4> _BaseMap;
        Texture2D<float4> _EmissionMap;
        SamplerState sampler_BaseMap;
        SamplerState sampler_vd_point_repeat;

        // ---- progVertex
        #if 1
        #include "LCArt_Background_shader_DE_BG_Base/p3_vs_72.hlsl"
        #endif
        // ---- progFragment
        #if 1
        #include "LCArt_Background_shader_DE_BG_Base/p3_ps_74.hlsl"
        #endif
        ENDHLSL
    }
    Pass
    {
        Name "Glass"
        Tags { "IgnoreProjector" = "true" "LightMode" = "Glass" "RenderPipeline" = "UniversalPipeline" "RenderType" = "Opaque" "UniversalMaterialType" = "SimpleLit" }
        BlendOp Max
        ZWrite Off
        Stencil {
          Ref [_StencilId]
          Comp NotEqual
        }
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
        float4 _MainLightPosition;
        float4 _MainLightColor;
        int _MainLightLayerMask;
        float4 _AdditionalLightsCount;
        float4 _Time;
        float4 _FPParams0;
        float4 _FPParams1;
        float4 _FPParams2;
        float3 _WorldSpaceCameraPos;
        float4 _ScreenParams;
        float4 unity_OrthoParams;
        float4 _ScaleBiasRt;
        float4x4 unity_MatrixV;
        float4x4 unity_MatrixVP;
        float4 _ScreenSize;
        float _GlobalCoverSeeThroughFactor;
        float _GlobalIsRuntime;
        float _GlobalWindEnable;
        float _GlobalWindIntensity;
        float _GlobalBackgroundSeeThroughEnable;
        float _GlobalSeeThroughAlpha;
        float _GlobalSeeThroughCircleAlpha;
        float _GlobalSeeThroughCircleRange;
        float _GlobalSeeThroughHeightMax;
        float _GlobalSeeThroughHeightMin;
        float _GlobalSeeThroughBuildingAlpha;
        float _GlobalSeeThroughBuildingHeightMax;
        float _GlobalSeeThroughBuildingHeightMin;
        float _IsSingleObject;
        float3 _ObjectBoundSize;
        float4 _GlobalBufferPoints_Character[8];
        float3 _PlaneBoundMinPos;
        float3 _PlaneBoundMaxPos;
        float _GlobalPlaneSystemEnable;
        float4 _SightPositions[3];
        float _SeeThroughDistanceMin;
        float _SeeThroughDistanceMax;
        float3 _SanityPointPositions[8];
        float _SanityPointRadiuses[8];
        float _SanityPointIntensities[8];
        float _GlobalStressFloatingEnableThreshold;
        float4 _GlobalStressFloatingPosition;
        float4 _GlobalStressFloatingAdditionalPosition;
        float _GlobalStressFloatingRotation;
        float _GlobalStressFloatingPositionPeriodMin;
        float _GlobalStressFloatingPositionPeriodMax;
        float _GlobalStressFloatingRotationPeriodMin;
        float _GlobalStressFloatingRotationPeriodMax;
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
            float4 _BaseColor : packoffset(c1);
            float _BumpScale : packoffset(c3);
            float _SanityRandomSeed : packoffset(c8.y);
            float _SanityDefaultDegree : packoffset(c8.z);
            float _UseWorldNormalMap : packoffset(c10.y);
            int _BackgroundType : packoffset(c11);
        }
        cbuffer urp_TileBuffer
        {
            float4 urp_Tiles[4096] : packoffset(c0);
        }
        cbuffer urp_ZBinBuffer
        {
            float4 urp_ZBins[1024] : packoffset(c0);
        }

        Texture2D<float4> _AfterRenderingGrabpassTexture;
        Texture2D<float4> _BaseMap;
        Texture2D<float4> _BumpMap;
        Texture2D<float4> _CharacterAreaMaskTexture;
        Texture2D<float4> _CustomDepthBuffer;
        Texture2D<float4> _EmissionMap;
        Texture2D<float4> _PlaneSightRenderTarget;
        Texture2D<float4> _WorldNormalMap;
        SamplerState sampler_BaseMap;
        SamplerState sampler_CharacterAreaMaskTexture;
        SamplerState sampler_CustomDepthBuffer;
        SamplerState sampler_PlaneSightRenderTarget;
        SamplerState sampler_vd_linear_clamp;
        SamplerState sampler_vd_point_repeat;

        // ---- progVertex
        #if defined(FOG_LINEAR) && defined(_ADDITIONAL_LIGHTS) && defined(_DE_EDITOR_CAMERA) && defined(_LIGHT_LAYERS)
        #include "LCArt_Background_shader_DE_BG_Base/p4_vs_13.hlsl"
        #elif defined(FOG_LINEAR) && defined(_CLUSTER_LIGHT_LOOP) && defined(_DE_EDITOR_CAMERA) && defined(_LIGHT_LAYERS)
        #include "LCArt_Background_shader_DE_BG_Base/p4_vs_9.hlsl"
        #elif defined(FOG_LINEAR) && defined(_ADDITIONAL_LIGHTS) && defined(_LIGHT_LAYERS)
        #include "LCArt_Background_shader_DE_BG_Base/p4_vs_11.hlsl"
        #elif defined(FOG_LINEAR) && defined(_CLUSTER_LIGHT_LOOP) && defined(_LIGHT_LAYERS)
        #include "LCArt_Background_shader_DE_BG_Base/p4_vs_7.hlsl"
        #elif defined(FOG_LINEAR) && defined(_DE_EDITOR_CAMERA) && defined(_LIGHT_LAYERS)
        #include "LCArt_Background_shader_DE_BG_Base/p4_vs_5.hlsl"
        #elif defined(_ADDITIONAL_LIGHTS) && defined(_DE_EDITOR_CAMERA) && defined(_LIGHT_LAYERS)
        #include "LCArt_Background_shader_DE_BG_Base/p4_vs_12.hlsl"
        #elif defined(_CLUSTER_LIGHT_LOOP) && defined(_DE_EDITOR_CAMERA) && defined(_LIGHT_LAYERS)
        #include "LCArt_Background_shader_DE_BG_Base/p4_vs_8.hlsl"
        #elif defined(FOG_LINEAR) && defined(_LIGHT_LAYERS)
        #include "LCArt_Background_shader_DE_BG_Base/p4_vs_3.hlsl"
        #elif defined(_ADDITIONAL_LIGHTS) && defined(_LIGHT_LAYERS)
        #include "LCArt_Background_shader_DE_BG_Base/p4_vs_10.hlsl"
        #elif defined(_CLUSTER_LIGHT_LOOP) && defined(_LIGHT_LAYERS)
        #include "LCArt_Background_shader_DE_BG_Base/p4_vs_6.hlsl"
        #elif defined(_DE_EDITOR_CAMERA) && defined(_LIGHT_LAYERS)
        #include "LCArt_Background_shader_DE_BG_Base/p4_vs_4.hlsl"
        #elif defined(_LIGHT_LAYERS)
        #include "LCArt_Background_shader_DE_BG_Base/p4_vs_2.hlsl"
        #else
        #include "LCArt_Background_shader_DE_BG_Base/p4_vs_2.hlsl"
        #endif
        // ---- progFragment
        #if defined(FOG_LINEAR) && defined(_ADDITIONAL_LIGHTS) && defined(_DE_EDITOR_CAMERA) && defined(_LIGHT_LAYERS)
        #include "LCArt_Background_shader_DE_BG_Base/p4_ps_92.hlsl"
        #elif defined(FOG_LINEAR) && defined(_CLUSTER_LIGHT_LOOP) && defined(_DE_EDITOR_CAMERA) && defined(_LIGHT_LAYERS)
        #include "LCArt_Background_shader_DE_BG_Base/p4_ps_88.hlsl"
        #elif defined(FOG_LINEAR) && defined(_ADDITIONAL_LIGHTS) && defined(_LIGHT_LAYERS)
        #include "LCArt_Background_shader_DE_BG_Base/p4_ps_90.hlsl"
        #elif defined(FOG_LINEAR) && defined(_CLUSTER_LIGHT_LOOP) && defined(_LIGHT_LAYERS)
        #include "LCArt_Background_shader_DE_BG_Base/p4_ps_86.hlsl"
        #elif defined(FOG_LINEAR) && defined(_DE_EDITOR_CAMERA) && defined(_LIGHT_LAYERS)
        #include "LCArt_Background_shader_DE_BG_Base/p4_ps_84.hlsl"
        #elif defined(_ADDITIONAL_LIGHTS) && defined(_DE_EDITOR_CAMERA) && defined(_LIGHT_LAYERS)
        #include "LCArt_Background_shader_DE_BG_Base/p4_ps_91.hlsl"
        #elif defined(_CLUSTER_LIGHT_LOOP) && defined(_DE_EDITOR_CAMERA) && defined(_LIGHT_LAYERS)
        #include "LCArt_Background_shader_DE_BG_Base/p4_ps_87.hlsl"
        #elif defined(FOG_LINEAR) && defined(_LIGHT_LAYERS)
        #include "LCArt_Background_shader_DE_BG_Base/p4_ps_82.hlsl"
        #elif defined(_ADDITIONAL_LIGHTS) && defined(_LIGHT_LAYERS)
        #include "LCArt_Background_shader_DE_BG_Base/p4_ps_89.hlsl"
        #elif defined(_CLUSTER_LIGHT_LOOP) && defined(_LIGHT_LAYERS)
        #include "LCArt_Background_shader_DE_BG_Base/p4_ps_85.hlsl"
        #elif defined(_DE_EDITOR_CAMERA) && defined(_LIGHT_LAYERS)
        #include "LCArt_Background_shader_DE_BG_Base/p4_ps_83.hlsl"
        #elif defined(_LIGHT_LAYERS)
        #include "LCArt_Background_shader_DE_BG_Base/p4_ps_81.hlsl"
        #else
        #include "LCArt_Background_shader_DE_BG_Base/p4_ps_81.hlsl"
        #endif
        ENDHLSL
    }
}
Fallback "Hidden/Universal Render Pipeline/FallbackError"
}
