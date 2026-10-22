Shader "LCArt/UI/Shader_UI_MinimapSightMasker"
{
Properties
{
    [Header (Noise Setting)] [Space (10)] _NoiseTex ("Noise Texture", 2D) = "white" {}
    _NoiseScale ("Noise Scale", Range(0.009999999776482582, 8)) = 0.5
    _NoiseSpeed ("Noise Speed", Range(-1, 1)) = -0.05000000074505806
    _NoiseStretch ("Noise Stretch (polar UV scale)", Vector) = (1, 4, 0, 0)
    _DissolveOffset ("Dissolve Offset", Range(-1, 0)) = -0.5
    [PerRendererData] [Header (Defaut UI Setting)] [Space (10)] _MainTex ("Sprite Texture", 2D) = "white" {}
    _Color ("Image Tint Color", Color) = (1, 1, 1, 1)
    _StencilComp ("Stencil Comparison", Float) = 8
    _Stencil ("Stencil ID", Float) = 0
    _StencilOp ("Stencil Operation", Float) = 0
    _StencilWriteMask ("Stencil Write Mask", Float) = 255
    _StencilReadMask ("Stencil Read Mask", Float) = 255
    _ColorMask ("Color Mask", Float) = 15
    [Toggle(UNITY_UI_ALPHACLIP)] _UseUIAlphaClip ("Use Alpha Clip", Float) = 0
    [HideInInspector] _MinimapSightTexture ("Minimap Sight Texture", 2D) = "white" {}
    [HideInInspector] _PivotWorldPosition ("Pivot World Position", Vector) = (0, 0, 0, 0)
}
SubShader
{
    Tags { "CanUseSpriteAtlas" = "true" "IgnoreProjector" = "true" "PreviewType" = "Plane" "Queue" = "Transparent" "RenderType" = "Transparent" }
    Pass
    {
        Name "Default"
        Tags { "CanUseSpriteAtlas" = "true" "IgnoreProjector" = "true" "PreviewType" = "Plane" "Queue" = "Transparent" "RenderType" = "Transparent" }
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
        float _GlobalSectorWidth;
        float _GlobalSectorHeight;
        float4 _GlobalOverallMapBoundMax;
        float4 _GlobalOverallMapBoundMin;
        float _MinimapSightEnable;
        float _GlobalMinimapInStage;
        float4 _GlobalLayerPaddedSize;
        float _GlobalLayerInnerSDFMin;
        float _GlobalLayerInnerSDFMax;
        float4 _GlobalMinimapSightMaskColor;
        float4 _GlobalMinimapPassedAreaColor;
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
        cbuffer UnityPerMaterial
        {
            float4 _Color : packoffset(c0);
            float4 _MainTex_ST : packoffset(c1);
            float4 _PivotWorldPosition : packoffset(c4);
            float _NoiseScale : packoffset(c5);
            float _NoiseSpeed : packoffset(c5.y);
            float4 _NoiseStretch : packoffset(c6);
        }

        Texture2D<float4> _GlobalLayerSDFTex;
        Texture2D<float4> _MainTex;
        Texture2D<float4> _MinimapSightTexture;
        Texture2D<float4> _NoiseTex;
        SamplerState sampler_GlobalLayerSDFTex;
        SamplerState sampler_MainTex;
        SamplerState sampler_MinimapSightTexture;
        SamplerState sampler_NoiseTex;

        // ---- progVertex
        #if defined(UNITY_UI_ALPHACLIP) && defined(UNITY_UI_CLIP_RECT)
        #include "LCArt_UI_Shader_UI_MinimapSightMasker/p0_vs_4.hlsl"
        #elif defined(UNITY_UI_ALPHACLIP)
        #include "LCArt_UI_Shader_UI_MinimapSightMasker/p0_vs_2.hlsl"
        #elif defined(UNITY_UI_CLIP_RECT)
        #include "LCArt_UI_Shader_UI_MinimapSightMasker/p0_vs_3.hlsl"
        #elif 1
        #include "LCArt_UI_Shader_UI_MinimapSightMasker/p0_vs_1.hlsl"
        #endif
        // ---- progFragment
        #if defined(UNITY_UI_ALPHACLIP) && defined(UNITY_UI_CLIP_RECT)
        #include "LCArt_UI_Shader_UI_MinimapSightMasker/p0_ps_9.hlsl"
        #elif defined(UNITY_UI_ALPHACLIP)
        #include "LCArt_UI_Shader_UI_MinimapSightMasker/p0_ps_7.hlsl"
        #elif defined(UNITY_UI_CLIP_RECT)
        #include "LCArt_UI_Shader_UI_MinimapSightMasker/p0_ps_8.hlsl"
        #elif 1
        #include "LCArt_UI_Shader_UI_MinimapSightMasker/p0_ps_6.hlsl"
        #endif
        ENDHLSL
    }
}
}
