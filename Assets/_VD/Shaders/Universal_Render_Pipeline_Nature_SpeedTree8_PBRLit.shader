Shader "Universal Render Pipeline/Nature/SpeedTree8_PBRLit"
{
Properties
{
    [NoScaleOffset] _MainTex ("Base Map", 2D) = "white" {}
    _Color ("Color Tint", Color) = (1, 1, 1, 1)
    [ToggleUI] _HueVariationKwToggle ("Enable Hue Variation", Float) = 1
    [ToggleUI] _OldHueVarBehavior ("Use Old Hue Variation Behavior", Float) = 0
    _HueVariationColor ("Hue Variation Color", Color) = (1, 0.5, 0, 0.20000000298023224)
    [NoScaleOffset] [Normal] _BumpMap ("Normal Map", 2D) = "bump" {}
    [ToggleUI] EFFECT_EXTRA_TEX ("Enable Extra Map", Float) = 1
    [NoScaleOffset] _ExtraTex ("Smoothness (R), Metallic (G), AO (B)", 2D) = "white" {}
    _Glossiness ("Smoothness", Range(0, 1)) = 0.5
    _Metallic ("Metallic", Range(0, 1)) = 0
    [ToggleUI] _SubsurfaceKwToggle ("Enable Subsurface", Float) = 0
    _SubsurfaceIndirect ("Indirect Subsurface Intensity", Range(0, 1)) = 0.25
    _SubsurfaceColor ("Subsurface Color", Color) = (1, 1, 1, 1)
    [NoScaleOffset] _SubsurfaceTex ("Subsurface Map", 2D) = "black" {}
    _AlphaClipThreshold ("Alpha Clip Threshold", Range(0, 1)) = 0.33000001311302185
    [ToggleUI] _WindQuality ("Wind Enabled", Float) = 1
    [KeywordEnum(None, Fastest, Fast, Better, Best, Palm)] _WINDQUALITY ("Wind Quality", Float) = 4
    [Toggle(EFFECT_BILLBOARD)] EFFECT_BILLBOARD ("Is Billboard", Float) = 0
    [KeywordEnum(Flip, Mirror, None)] BACKFACE_NORMAL_MODE ("Backface Normal Mode", Float) = 1
    [HideInInspector] _QueueOffset ("_QueueOffset", Float) = 0
    [HideInInspector] _QueueControl ("_QueueControl", Float) = -1
    [HideInInspector] [NoScaleOffset] unity_Lightmaps ("unity_Lightmaps", 2DArray) = "" {}
    [HideInInspector] [NoScaleOffset] unity_LightmapsInd ("unity_LightmapsInd", 2DArray) = "" {}
    [HideInInspector] [NoScaleOffset] unity_ShadowMasks ("unity_ShadowMasks", 2DArray) = "" {}
}
SubShader
{
    Tags { "AlwaysRenderMotionVectors" = "true" "DisableBatching" = "LodFading" "Queue" = "AlphaTest" "RenderPipeline" = "UniversalPipeline" "RenderType" = "Opaque" "ShaderGraphShader" = "true" "ShaderGraphTargetId" = "UniversalLitSubTarget" "UniversalMaterialType" = "Lit" }
    Pass
    {
        Name "Universal Forward"
        Tags { "AlwaysRenderMotionVectors" = "true" "DisableBatching" = "LodFading" "LightMode" = "UniversalForward" "Queue" = "AlphaTest" "RenderPipeline" = "UniversalPipeline" "RenderType" = "Opaque" "ShaderGraphShader" = "true" "ShaderGraphTargetId" = "UniversalLitSubTarget" "UniversalMaterialType" = "Lit" }
        AlphaToMask On
        Cull Off
        HLSLPROGRAM
        #pragma target 4.0
        #pragma vertex vert
        #pragma fragment frag
        #pragma multi_compile_local BACKFACE_NORMAL_MODE_FLIP BACKFACE_NORMAL_MODE_MIRROR
        #pragma multi_compile _ FOG_LINEAR
        #pragma multi_compile _ _ADDITIONAL_LIGHTS
        #pragma multi_compile _ _CLUSTER_LIGHT_LOOP
        #pragma multi_compile _ _DBUFFER_MRT3
        #pragma multi_compile _LIGHT_LAYERS
        #pragma multi_compile_local _WINDQUALITY_NONE _WINDQUALITY_BEST

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
        float3 _WorldSpaceCameraPos;
        float4 _ProjectionParams;
        float4 unity_OrthoParams;
        float4 _ScaleBiasRt;
        float4 unity_FogParams;
        float4 unity_FogColor;
        float4x4 unity_MatrixInvV;
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
        cbuffer SpeedTreeWind
        {
            float4 _ST_WindVector : packoffset(c0);
            float4 _ST_WindGlobal : packoffset(c1);
            float4 _ST_WindBranch : packoffset(c2);
            float4 _ST_WindBranchTwitch : packoffset(c3);
            float4 _ST_WindBranchAdherences : packoffset(c6);
            float4 _ST_WindLeaf1Ripple : packoffset(c8);
            float4 _ST_WindLeaf1Tumble : packoffset(c9);
            float4 _ST_WindLeaf2Ripple : packoffset(c11);
            float4 _ST_WindLeaf2Tumble : packoffset(c12);
            float4 _ST_WindLeaf2Twitch : packoffset(c13);
        }
        cbuffer UnityPerDraw
        {
            float4x4 unity_ObjectToWorld : packoffset(c0);
            float4x4 unity_WorldToObject : packoffset(c4);
            float4 unity_LODFade : packoffset(c8);
            float4 unity_WorldTransformParams : packoffset(c9);
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
            float4 _Color : packoffset(c1);
            float _HueVariationKwToggle : packoffset(c2);
            float _OldHueVarBehavior : packoffset(c2.y);
            float4 _HueVariationColor : packoffset(c3);
            float EFFECT_EXTRA_TEX : packoffset(c5);
            float _Glossiness : packoffset(c7);
            float _Metallic : packoffset(c7.y);
            float _SubsurfaceKwToggle : packoffset(c7.z);
            float _SubsurfaceIndirect : packoffset(c7.w);
            float4 _SubsurfaceColor : packoffset(c8);
            float _AlphaClipThreshold : packoffset(c10);
            float _WindQuality : packoffset(c10.y);
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
        Texture2D<float4> _DBufferTexture0;
        Texture2D<float4> _DBufferTexture1;
        Texture2D<float4> _DBufferTexture2;
        Texture2D<float4> _ExtraTex;
        Texture2D<float4> _MainTex;
        Texture2D<float4> _SubsurfaceTex;
        TextureCube<float4> unity_SpecCube0;
        SamplerState sampler_BumpMap;
        SamplerState sampler_ExtraTex;
        SamplerState sampler_MainTex;
        SamplerState sampler_SubsurfaceTex;
        SamplerState samplerunity_SpecCube0;

        // ---- progVertex
        #if defined(FOG_LINEAR) && defined(_ADDITIONAL_LIGHTS) && defined(_WINDQUALITY_BEST)
        #include "Universal_Render_Pipeline_Nature_SpeedTree8_PBRLit/p0_vs_4.hlsl"
        #elif defined(FOG_LINEAR) && defined(_ADDITIONAL_LIGHTS) && defined(_WINDQUALITY_NONE)
        #include "Universal_Render_Pipeline_Nature_SpeedTree8_PBRLit/p0_vs_5.hlsl"
        #elif defined(FOG_LINEAR) && defined(_CLUSTER_LIGHT_LOOP) && defined(_WINDQUALITY_BEST)
        #include "Universal_Render_Pipeline_Nature_SpeedTree8_PBRLit/p0_vs_8.hlsl"
        #elif defined(FOG_LINEAR) && defined(_CLUSTER_LIGHT_LOOP) && defined(_WINDQUALITY_NONE)
        #include "Universal_Render_Pipeline_Nature_SpeedTree8_PBRLit/p0_vs_9.hlsl"
        #elif defined(_ADDITIONAL_LIGHTS) && defined(_WINDQUALITY_BEST)
        #include "Universal_Render_Pipeline_Nature_SpeedTree8_PBRLit/p0_vs_6.hlsl"
        #elif defined(_ADDITIONAL_LIGHTS) && defined(_WINDQUALITY_NONE)
        #include "Universal_Render_Pipeline_Nature_SpeedTree8_PBRLit/p0_vs_7.hlsl"
        #elif defined(_CLUSTER_LIGHT_LOOP) && defined(_WINDQUALITY_BEST)
        #include "Universal_Render_Pipeline_Nature_SpeedTree8_PBRLit/p0_vs_10.hlsl"
        #elif defined(_CLUSTER_LIGHT_LOOP) && defined(_WINDQUALITY_NONE)
        #include "Universal_Render_Pipeline_Nature_SpeedTree8_PBRLit/p0_vs_11.hlsl"
        #else
        #include "Universal_Render_Pipeline_Nature_SpeedTree8_PBRLit/p0_vs_11.hlsl"
        #endif
        // ---- progFragment
        #if defined(BACKFACE_NORMAL_MODE_FLIP) && defined(FOG_LINEAR) && defined(_CLUSTER_LIGHT_LOOP) && defined(_DBUFFER_MRT3) && defined(_LIGHT_LAYERS)
        #include "Universal_Render_Pipeline_Nature_SpeedTree8_PBRLit/p0_ps_19.hlsl"
        #elif defined(BACKFACE_NORMAL_MODE_MIRROR) && defined(FOG_LINEAR) && defined(_CLUSTER_LIGHT_LOOP) && defined(_DBUFFER_MRT3) && defined(_LIGHT_LAYERS)
        #include "Universal_Render_Pipeline_Nature_SpeedTree8_PBRLit/p0_ps_18.hlsl"
        #elif defined(BACKFACE_NORMAL_MODE_FLIP) && defined(FOG_LINEAR) && defined(_ADDITIONAL_LIGHTS) && defined(_LIGHT_LAYERS)
        #include "Universal_Render_Pipeline_Nature_SpeedTree8_PBRLit/p0_ps_23.hlsl"
        #elif defined(BACKFACE_NORMAL_MODE_FLIP) && defined(FOG_LINEAR) && defined(_CLUSTER_LIGHT_LOOP) && defined(_LIGHT_LAYERS)
        #include "Universal_Render_Pipeline_Nature_SpeedTree8_PBRLit/p0_ps_27.hlsl"
        #elif defined(BACKFACE_NORMAL_MODE_FLIP) && defined(_CLUSTER_LIGHT_LOOP) && defined(_DBUFFER_MRT3) && defined(_LIGHT_LAYERS)
        #include "Universal_Render_Pipeline_Nature_SpeedTree8_PBRLit/p0_ps_21.hlsl"
        #elif defined(BACKFACE_NORMAL_MODE_MIRROR) && defined(FOG_LINEAR) && defined(_ADDITIONAL_LIGHTS) && defined(_LIGHT_LAYERS)
        #include "Universal_Render_Pipeline_Nature_SpeedTree8_PBRLit/p0_ps_22.hlsl"
        #elif defined(BACKFACE_NORMAL_MODE_MIRROR) && defined(FOG_LINEAR) && defined(_CLUSTER_LIGHT_LOOP) && defined(_LIGHT_LAYERS)
        #include "Universal_Render_Pipeline_Nature_SpeedTree8_PBRLit/p0_ps_26.hlsl"
        #elif defined(BACKFACE_NORMAL_MODE_MIRROR) && defined(_CLUSTER_LIGHT_LOOP) && defined(_DBUFFER_MRT3) && defined(_LIGHT_LAYERS)
        #include "Universal_Render_Pipeline_Nature_SpeedTree8_PBRLit/p0_ps_20.hlsl"
        #elif defined(BACKFACE_NORMAL_MODE_FLIP) && defined(_ADDITIONAL_LIGHTS) && defined(_LIGHT_LAYERS)
        #include "Universal_Render_Pipeline_Nature_SpeedTree8_PBRLit/p0_ps_25.hlsl"
        #elif defined(BACKFACE_NORMAL_MODE_FLIP) && defined(_CLUSTER_LIGHT_LOOP) && defined(_LIGHT_LAYERS)
        #include "Universal_Render_Pipeline_Nature_SpeedTree8_PBRLit/p0_ps_29.hlsl"
        #elif defined(BACKFACE_NORMAL_MODE_MIRROR) && defined(_ADDITIONAL_LIGHTS) && defined(_LIGHT_LAYERS)
        #include "Universal_Render_Pipeline_Nature_SpeedTree8_PBRLit/p0_ps_24.hlsl"
        #elif defined(BACKFACE_NORMAL_MODE_MIRROR) && defined(_CLUSTER_LIGHT_LOOP) && defined(_LIGHT_LAYERS)
        #include "Universal_Render_Pipeline_Nature_SpeedTree8_PBRLit/p0_ps_28.hlsl"
        #else
        #include "Universal_Render_Pipeline_Nature_SpeedTree8_PBRLit/p0_ps_28.hlsl"
        #endif
        ENDHLSL
    }
    Pass
    {
        Name "MotionVectors"
        Tags { "AlwaysRenderMotionVectors" = "true" "DisableBatching" = "LodFading" "LightMode" = "MOTIONVECTORS" "Queue" = "AlphaTest" "RenderPipeline" = "UniversalPipeline" "RenderType" = "Opaque" "ShaderGraphShader" = "true" "ShaderGraphTargetId" = "UniversalLitSubTarget" "UniversalMaterialType" = "Lit" }
        ColorMask RG
        Cull Off
        HLSLPROGRAM
        #pragma target 4.0
        #pragma vertex vert
        #pragma fragment frag
        #pragma multi_compile_local BACKFACE_NORMAL_MODE_FLIP BACKFACE_NORMAL_MODE_MIRROR
        #pragma multi_compile_local _WINDQUALITY_NONE _WINDQUALITY_BEST

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
        float4x4 unity_MatrixInvV;
        float4x4 unity_MatrixVP;
        float4x4 _PrevViewProjMatrix;
        float4x4 _NonJitteredViewProjMatrix;
        cbuffer SpeedTreeWind
        {
            float4 _ST_WindVector : packoffset(c0);
            float4 _ST_WindGlobal : packoffset(c1);
            float4 _ST_WindBranch : packoffset(c2);
            float4 _ST_WindBranchTwitch : packoffset(c3);
            float4 _ST_WindBranchAdherences : packoffset(c6);
            float4 _ST_WindLeaf1Ripple : packoffset(c8);
            float4 _ST_WindLeaf1Tumble : packoffset(c9);
            float4 _ST_WindLeaf2Ripple : packoffset(c11);
            float4 _ST_WindLeaf2Tumble : packoffset(c12);
            float4 _ST_WindLeaf2Twitch : packoffset(c13);
        }
        cbuffer SpeedTreeWindHistory
        {
            float4 _ST_WindVectorHistory : packoffset(c0);
            float4 _ST_WindGlobalHistory : packoffset(c1);
            float4 _ST_WindBranchHistory : packoffset(c2);
            float4 _ST_WindBranchTwitchHistory : packoffset(c3);
            float4 _ST_WindBranchAdherencesHistory : packoffset(c6);
            float4 _ST_WindLeaf1RippleHistory : packoffset(c8);
            float4 _ST_WindLeaf1TumbleHistory : packoffset(c9);
            float4 _ST_WindLeaf2RippleHistory : packoffset(c11);
            float4 _ST_WindLeaf2TumbleHistory : packoffset(c12);
            float4 _ST_WindLeaf2TwitchHistory : packoffset(c13);
        }
        cbuffer UnityPerDraw
        {
            float4x4 unity_ObjectToWorld : packoffset(c0);
            float4x4 unity_WorldToObject : packoffset(c4);
            float4 unity_LODFade : packoffset(c8);
            float4 unity_WorldTransformParams : packoffset(c9);
            float4x4 unity_MatrixPreviousM : packoffset(c36);
            float4 unity_MotionVectorsParams : packoffset(c44);
        }
        cbuffer UnityPerMaterial
        {
            float _AlphaClipThreshold : packoffset(c10);
            float _WindQuality : packoffset(c10.y);
        }

        Texture2D<float4> _MainTex;
        SamplerState sampler_MainTex;

        // ---- progVertex
        #if defined(_WINDQUALITY_BEST)
        #include "Universal_Render_Pipeline_Nature_SpeedTree8_PBRLit/p1_vs_33.hlsl"
        #elif defined(_WINDQUALITY_NONE)
        #include "Universal_Render_Pipeline_Nature_SpeedTree8_PBRLit/p1_vs_32.hlsl"
        #else
        #include "Universal_Render_Pipeline_Nature_SpeedTree8_PBRLit/p1_vs_32.hlsl"
        #endif
        // ---- progFragment
        #if defined(BACKFACE_NORMAL_MODE_FLIP)
        #include "Universal_Render_Pipeline_Nature_SpeedTree8_PBRLit/p1_ps_35.hlsl"
        #elif defined(BACKFACE_NORMAL_MODE_MIRROR)
        #include "Universal_Render_Pipeline_Nature_SpeedTree8_PBRLit/p1_ps_36.hlsl"
        #else
        #include "Universal_Render_Pipeline_Nature_SpeedTree8_PBRLit/p1_ps_36.hlsl"
        #endif
        ENDHLSL
    }
    Pass
    {
        Name "DepthOnly"
        Tags { "AlwaysRenderMotionVectors" = "true" "DisableBatching" = "LodFading" "LightMode" = "DepthOnly" "Queue" = "AlphaTest" "RenderPipeline" = "UniversalPipeline" "RenderType" = "Opaque" "ShaderGraphShader" = "true" "ShaderGraphTargetId" = "UniversalLitSubTarget" "UniversalMaterialType" = "Lit" }
        ColorMask R
        Cull Off
        HLSLPROGRAM
        #pragma target 4.0
        #pragma vertex vert
        #pragma fragment frag
        #pragma multi_compile_local BACKFACE_NORMAL_MODE_FLIP BACKFACE_NORMAL_MODE_MIRROR
        #pragma multi_compile_local _WINDQUALITY_NONE _WINDQUALITY_BEST

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
        float4x4 unity_MatrixInvV;
        float4x4 unity_MatrixVP;
        cbuffer SpeedTreeWind
        {
            float4 _ST_WindVector : packoffset(c0);
            float4 _ST_WindGlobal : packoffset(c1);
            float4 _ST_WindBranch : packoffset(c2);
            float4 _ST_WindBranchTwitch : packoffset(c3);
            float4 _ST_WindBranchAdherences : packoffset(c6);
            float4 _ST_WindLeaf1Ripple : packoffset(c8);
            float4 _ST_WindLeaf1Tumble : packoffset(c9);
            float4 _ST_WindLeaf2Ripple : packoffset(c11);
            float4 _ST_WindLeaf2Tumble : packoffset(c12);
            float4 _ST_WindLeaf2Twitch : packoffset(c13);
        }
        cbuffer UnityPerDraw
        {
            float4x4 unity_ObjectToWorld : packoffset(c0);
            float4x4 unity_WorldToObject : packoffset(c4);
            float4 unity_LODFade : packoffset(c8);
            float4 unity_WorldTransformParams : packoffset(c9);
        }
        cbuffer UnityPerMaterial
        {
            float _AlphaClipThreshold : packoffset(c10);
            float _WindQuality : packoffset(c10.y);
        }

        Texture2D<float4> _MainTex;
        SamplerState sampler_MainTex;

        // ---- progVertex
        #if defined(_WINDQUALITY_BEST)
        #include "Universal_Render_Pipeline_Nature_SpeedTree8_PBRLit/p2_vs_39.hlsl"
        #elif defined(_WINDQUALITY_NONE)
        #include "Universal_Render_Pipeline_Nature_SpeedTree8_PBRLit/p2_vs_38.hlsl"
        #else
        #include "Universal_Render_Pipeline_Nature_SpeedTree8_PBRLit/p2_vs_38.hlsl"
        #endif
        // ---- progFragment
        #if defined(BACKFACE_NORMAL_MODE_FLIP)
        #include "Universal_Render_Pipeline_Nature_SpeedTree8_PBRLit/p2_ps_41.hlsl"
        #elif defined(BACKFACE_NORMAL_MODE_MIRROR)
        #include "Universal_Render_Pipeline_Nature_SpeedTree8_PBRLit/p2_ps_42.hlsl"
        #else
        #include "Universal_Render_Pipeline_Nature_SpeedTree8_PBRLit/p2_ps_42.hlsl"
        #endif
        ENDHLSL
    }
    Pass
    {
        Name "DepthNormals"
        Tags { "AlwaysRenderMotionVectors" = "true" "DisableBatching" = "LodFading" "LightMode" = "DepthNormals" "Queue" = "AlphaTest" "RenderPipeline" = "UniversalPipeline" "RenderType" = "Opaque" "ShaderGraphShader" = "true" "ShaderGraphTargetId" = "UniversalLitSubTarget" "UniversalMaterialType" = "Lit" }
        Cull Off
        HLSLPROGRAM
        #pragma target 4.0
        #pragma vertex vert
        #pragma fragment frag
        #pragma multi_compile_local BACKFACE_NORMAL_MODE_FLIP BACKFACE_NORMAL_MODE_MIRROR
        #pragma multi_compile_local _WINDQUALITY_NONE _WINDQUALITY_BEST

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
        float4x4 unity_MatrixInvV;
        float4x4 unity_MatrixVP;
        cbuffer SpeedTreeWind
        {
            float4 _ST_WindVector : packoffset(c0);
            float4 _ST_WindGlobal : packoffset(c1);
            float4 _ST_WindBranch : packoffset(c2);
            float4 _ST_WindBranchTwitch : packoffset(c3);
            float4 _ST_WindBranchAdherences : packoffset(c6);
            float4 _ST_WindLeaf1Ripple : packoffset(c8);
            float4 _ST_WindLeaf1Tumble : packoffset(c9);
            float4 _ST_WindLeaf2Ripple : packoffset(c11);
            float4 _ST_WindLeaf2Tumble : packoffset(c12);
            float4 _ST_WindLeaf2Twitch : packoffset(c13);
        }
        cbuffer UnityPerDraw
        {
            float4x4 unity_ObjectToWorld : packoffset(c0);
            float4x4 unity_WorldToObject : packoffset(c4);
            float4 unity_LODFade : packoffset(c8);
            float4 unity_WorldTransformParams : packoffset(c9);
        }
        cbuffer UnityPerMaterial
        {
            float _AlphaClipThreshold : packoffset(c10);
            float _WindQuality : packoffset(c10.y);
        }

        Texture2D<float4> _BumpMap;
        Texture2D<float4> _MainTex;
        SamplerState sampler_BumpMap;
        SamplerState sampler_MainTex;

        // ---- progVertex
        #if defined(_WINDQUALITY_BEST)
        #include "Universal_Render_Pipeline_Nature_SpeedTree8_PBRLit/p3_vs_39.hlsl"
        #elif defined(_WINDQUALITY_NONE)
        #include "Universal_Render_Pipeline_Nature_SpeedTree8_PBRLit/p3_vs_38.hlsl"
        #else
        #include "Universal_Render_Pipeline_Nature_SpeedTree8_PBRLit/p3_vs_38.hlsl"
        #endif
        // ---- progFragment
        #if defined(BACKFACE_NORMAL_MODE_FLIP)
        #include "Universal_Render_Pipeline_Nature_SpeedTree8_PBRLit/p3_ps_43.hlsl"
        #elif defined(BACKFACE_NORMAL_MODE_MIRROR)
        #include "Universal_Render_Pipeline_Nature_SpeedTree8_PBRLit/p3_ps_44.hlsl"
        #else
        #include "Universal_Render_Pipeline_Nature_SpeedTree8_PBRLit/p3_ps_44.hlsl"
        #endif
        ENDHLSL
    }
    Pass
    {
        Name "Meta"
        Tags { "AlwaysRenderMotionVectors" = "true" "DisableBatching" = "LodFading" "LightMode" = "META" "Queue" = "AlphaTest" "RenderPipeline" = "UniversalPipeline" "RenderType" = "Opaque" "ShaderGraphShader" = "true" "ShaderGraphTargetId" = "UniversalLitSubTarget" "UniversalMaterialType" = "Lit" }
        Cull Off
        HLSLPROGRAM
        #pragma target 4.0
        #pragma vertex vert
        #pragma fragment frag
        #pragma multi_compile_local BACKFACE_NORMAL_MODE_FLIP BACKFACE_NORMAL_MODE_MIRROR
        #pragma multi_compile_local _WINDQUALITY_NONE _WINDQUALITY_BEST

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
        float4 unity_OrthoParams;
        float4x4 unity_MatrixV;
        float4x4 unity_MatrixInvV;
        float4x4 unity_MatrixVP;
        float unity_OneOverOutputBoost;
        float unity_MaxOutputValue;
        float unity_UseLinearSpace;
        cbuffer SpeedTreeWind
        {
            float4 _ST_WindVector : packoffset(c0);
            float4 _ST_WindGlobal : packoffset(c1);
            float4 _ST_WindBranch : packoffset(c2);
            float4 _ST_WindBranchTwitch : packoffset(c3);
            float4 _ST_WindBranchAdherences : packoffset(c6);
            float4 _ST_WindLeaf1Ripple : packoffset(c8);
            float4 _ST_WindLeaf1Tumble : packoffset(c9);
            float4 _ST_WindLeaf2Ripple : packoffset(c11);
            float4 _ST_WindLeaf2Tumble : packoffset(c12);
            float4 _ST_WindLeaf2Twitch : packoffset(c13);
        }
        cbuffer UnityMetaPass
        {
            int4 unity_MetaVertexControl : packoffset(c0);
            int4 unity_MetaFragmentControl : packoffset(c1);
        }
        cbuffer UnityPerDraw
        {
            float4x4 unity_ObjectToWorld : packoffset(c0);
            float4x4 unity_WorldToObject : packoffset(c4);
            float4 unity_LODFade : packoffset(c8);
            float4 unity_WorldTransformParams : packoffset(c9);
            float4 unity_LightmapST : packoffset(c25);
            float4 unity_DynamicLightmapST : packoffset(c26);
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
            float4 _Color : packoffset(c1);
            float _HueVariationKwToggle : packoffset(c2);
            float _OldHueVarBehavior : packoffset(c2.y);
            float4 _HueVariationColor : packoffset(c3);
            float EFFECT_EXTRA_TEX : packoffset(c5);
            float _Glossiness : packoffset(c7);
            float _SubsurfaceKwToggle : packoffset(c7.z);
            float _SubsurfaceIndirect : packoffset(c7.w);
            float4 _SubsurfaceColor : packoffset(c8);
            float _AlphaClipThreshold : packoffset(c10);
            float _WindQuality : packoffset(c10.y);
        }

        Texture2D<float4> _ExtraTex;
        Texture2D<float4> _MainTex;
        Texture2D<float4> _SubsurfaceTex;
        SamplerState sampler_ExtraTex;
        SamplerState sampler_MainTex;
        SamplerState sampler_SubsurfaceTex;

        // ---- progVertex
        #if defined(_WINDQUALITY_BEST)
        #include "Universal_Render_Pipeline_Nature_SpeedTree8_PBRLit/p4_vs_48.hlsl"
        #elif defined(_WINDQUALITY_NONE)
        #include "Universal_Render_Pipeline_Nature_SpeedTree8_PBRLit/p4_vs_47.hlsl"
        #else
        #include "Universal_Render_Pipeline_Nature_SpeedTree8_PBRLit/p4_vs_47.hlsl"
        #endif
        // ---- progFragment
        #if defined(BACKFACE_NORMAL_MODE_FLIP)
        #include "Universal_Render_Pipeline_Nature_SpeedTree8_PBRLit/p4_ps_50.hlsl"
        #elif defined(BACKFACE_NORMAL_MODE_MIRROR)
        #include "Universal_Render_Pipeline_Nature_SpeedTree8_PBRLit/p4_ps_51.hlsl"
        #else
        #include "Universal_Render_Pipeline_Nature_SpeedTree8_PBRLit/p4_ps_51.hlsl"
        #endif
        ENDHLSL
    }
    Pass
    {
        Name "Universal 2D"
        Tags { "AlwaysRenderMotionVectors" = "true" "DisableBatching" = "LodFading" "LightMode" = "Universal2D" "Queue" = "AlphaTest" "RenderPipeline" = "UniversalPipeline" "RenderType" = "Opaque" "ShaderGraphShader" = "true" "ShaderGraphTargetId" = "UniversalLitSubTarget" "UniversalMaterialType" = "Lit" }
        Cull Off
        HLSLPROGRAM
        #pragma target 4.0
        #pragma vertex vert
        #pragma fragment frag
        #pragma multi_compile_local BACKFACE_NORMAL_MODE_FLIP BACKFACE_NORMAL_MODE_MIRROR
        #pragma multi_compile_local _WINDQUALITY_NONE _WINDQUALITY_BEST

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
        float4x4 unity_MatrixInvV;
        float4x4 unity_MatrixVP;
        cbuffer SpeedTreeWind
        {
            float4 _ST_WindVector : packoffset(c0);
            float4 _ST_WindGlobal : packoffset(c1);
            float4 _ST_WindBranch : packoffset(c2);
            float4 _ST_WindBranchTwitch : packoffset(c3);
            float4 _ST_WindBranchAdherences : packoffset(c6);
            float4 _ST_WindLeaf1Ripple : packoffset(c8);
            float4 _ST_WindLeaf1Tumble : packoffset(c9);
            float4 _ST_WindLeaf2Ripple : packoffset(c11);
            float4 _ST_WindLeaf2Tumble : packoffset(c12);
            float4 _ST_WindLeaf2Twitch : packoffset(c13);
        }
        cbuffer UnityPerDraw
        {
            float4x4 unity_ObjectToWorld : packoffset(c0);
            float4x4 unity_WorldToObject : packoffset(c4);
            float4 unity_LODFade : packoffset(c8);
            float4 unity_WorldTransformParams : packoffset(c9);
        }
        cbuffer UnityPerMaterial
        {
            float4 _Color : packoffset(c1);
            float _HueVariationKwToggle : packoffset(c2);
            float _OldHueVarBehavior : packoffset(c2.y);
            float4 _HueVariationColor : packoffset(c3);
            float _AlphaClipThreshold : packoffset(c10);
            float _WindQuality : packoffset(c10.y);
        }

        Texture2D<float4> _MainTex;
        SamplerState sampler_MainTex;

        // ---- progVertex
        #if defined(_WINDQUALITY_BEST)
        #include "Universal_Render_Pipeline_Nature_SpeedTree8_PBRLit/p5_vs_39.hlsl"
        #elif defined(_WINDQUALITY_NONE)
        #include "Universal_Render_Pipeline_Nature_SpeedTree8_PBRLit/p5_vs_38.hlsl"
        #else
        #include "Universal_Render_Pipeline_Nature_SpeedTree8_PBRLit/p5_vs_38.hlsl"
        #endif
        // ---- progFragment
        #if defined(BACKFACE_NORMAL_MODE_FLIP)
        #include "Universal_Render_Pipeline_Nature_SpeedTree8_PBRLit/p5_ps_55.hlsl"
        #elif defined(BACKFACE_NORMAL_MODE_MIRROR)
        #include "Universal_Render_Pipeline_Nature_SpeedTree8_PBRLit/p5_ps_56.hlsl"
        #else
        #include "Universal_Render_Pipeline_Nature_SpeedTree8_PBRLit/p5_ps_56.hlsl"
        #endif
        ENDHLSL
    }
}
Fallback "Hidden/Shader Graph/FallbackError"
CustomEditor "UnityEditor.ShaderGraph.GenericShaderGraphMaterialGUI"
}
