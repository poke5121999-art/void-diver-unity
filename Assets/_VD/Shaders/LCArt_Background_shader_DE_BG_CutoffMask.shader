Shader "LCArt/Background/shader_DE_BG_CutoffMask"
{
Properties
{
    [HideInInspector] [MainTexture] [Header (______________________________________________________________________________________________________________________)] [Header (Common)] [Space (10)] _BaseMap ("Base Map (RGB)", 2D) = "white" {}
    [HideInInspector] [MainColor] _BaseColor ("Base Color", Color) = (1, 1, 1, 1)
    [HideInInspector] _Cutoff ("Alpha Clipping", Range(0, 1)) = 0.5
    [HideInInspector] _SpecGlossMap ("Specular Map", 2D) = "black" {}
    [HideInInspector] _SpecIntensity ("Specular Intensity", Range(0, 2)) = 1
    [HideInInspector] _Smoothness ("Smoothness", Range(0, 1)) = 0.5
    [HideInInspector] _SmoothnessSource ("Smoothness Source", Float) = 0
    [HideInInspector] _SpecularHighlights ("Specular Highlights", Float) = 1
    [HideInInspector] [NoScaleOffset] _BumpMap ("Normal Map", 2D) = "bump" {}
    [HideInInspector] _BumpScale ("Normal Map Intensity", Range(0, 10)) = 1
    [HideInInspector] [NoScaleOffset] _WorldNormalMap ("World Normal Map", 2D) = "bump" {}
    [HideInInspector] [Toggle] _UseWorldNormalMap ("Use World Normal Map", Float) = 0
    [HideInInspector] [NoScaleOffset] _EmissionMap ("Multi Map (R: Emission, G: -, B: Glass, RB: Fresnel)", 2D) = "black" {}
    [HideInInspector] _EmissionIntensity ("Emission Intensity", Range(0, 100)) = 0
    [HideInInspector] _EmissionFlickRate ("Emission Flick Rate", Range(0, 1)) = 0
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
    [HideInInspector] [Space (20)] [Header (______________________________________________________________________________________________________________________)] [Header (Dirt)] [Space (10)] [Toggle] _UseDirtNoise ("Use Dirt Noise", Float) = 0
    [HideInInspector] _DirtNoiseColor ("Dirt Color", Color) = (0, 0, 0, 0.5)
    [HideInInspector] _DirtNoiseTiling ("Dirt Pattern Tiling", Float) = 1
    [HideInInspector] _OutlineColor ("Outline Color", Color) = (0, 0, 0, 1)
    [HideInInspector] [Toggle] _OutlineEnable ("Outline Enable", Float) = 0
    [HideInInspector] _OutlineStroke ("Outline Storke", Float) = 0.125
    [Space (20)] [Header (______________________________________________________________________________________________________________________)] [Header (Dissolve)] [Space (10)] _DissolveProgress ("Dissolve Progress", Range(0, 1)) = 0
    _DissolveEdgeColor ("Dissolve Edge Color", Color) = (1, 1, 1, 1)
    [HideInInspector] [Space (20)] [Header (______________________________________________________________________________________________________________________)] [Header (Snapshot)] [Space (10)] _SnapshotType ("Snapshot Type", Float) = 0
    [Space (20)] [Header (______________________________________________________________________________________________________________________)] [Header (Cutoff Mask Setting)] [Space (10)] _CutoffMaskPivotOffset ("Cutoff Mask Pivot Offset", Vector) = (-1, 0, 1, 0)
    _CutoffMaskBump ("Cutoff Mask Bump", Float) = 3
    [HideInInspector] [Space (20)] [Header (______________________________________________________________________________________________________________________)] [Header (Stencil)] [Space (10)] _StencilId ("Stencil Id", Float) = 15
}
SubShader
{
    LOD 300
    Tags { "IgnoreProjector" = "true" "RenderType" = "Opaque" }
    Pass
    {
        Name "CustomDepth"
        Tags { "IgnoreProjector" = "true" "LightMode" = "None" "RenderType" = "Opaque" }
        BlendOp Max
        ColorMask RG
        ZTest Always
        ZWrite Off
        Cull Front
        HLSLPROGRAM
        #pragma target 5.0
        #pragma vertex vert
        #pragma fragment frag
        #pragma multi_compile _ _WRITE_RENDERING_LAYERS

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
        float4 _Time;
        float4x4 unity_MatrixVP;
        float _GlobalWindEnable;
        float _GlobalWindIntensity;
        float _GlobalAlwaysStencil;
        float _IsSingleObject;
        float3 _ObjectBoundSize;
        float4 _GlobalBufferPoints_Character[8];
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
            float _DissolveProgress : packoffset(c11.y);
        }

        Texture2D<float4> _BaseMap;
        SamplerState sampler_BaseMap;

        // ---- progVertex
        #if 1
        #include "LCArt_Background_shader_DE_BG_CutoffMask/p0_vs_1.hlsl"
        #endif
        // ---- progFragment
        #if defined(_WRITE_RENDERING_LAYERS)
        #include "LCArt_Background_shader_DE_BG_CutoffMask/p0_ps_3.hlsl"
        #elif 1
        #include "LCArt_Background_shader_DE_BG_CutoffMask/p0_ps_2.hlsl"
        #endif
        ENDHLSL
    }
}
Fallback "Hidden/Universal Render Pipeline/FallbackError"
}
