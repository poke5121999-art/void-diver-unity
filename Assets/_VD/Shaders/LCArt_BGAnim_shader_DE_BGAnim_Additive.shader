Shader "LCArt/BGAnim/shader_DE_BGAnim_Additive"
{
Properties
{
    [Header (Common Setting)] [Space (10)] _MainTex ("Texture", 2D) = "white" {}
    [Header (Additive Setting)] [Space (10)] _AdditiveTint ("Additive Tint Color", Color) = (1, 1, 1, 1)
    _AdditiveIntensity ("Additive Intensity", Range(0, 10)) = 2.5
    _AdditivePower ("Additive Power", Range(0.0010000000474974513, 10)) = 1
    [Header (Billboard Setting)] [Space (10)] [Toggle] _BillboardEnable ("Billboard Enable", Float) = 0
    [Header (Sight Setting)] [Space (10)] [Toggle] _SightEffectEnable ("Sight Effect Enable", Float) = 0
    [Header (Cutoff Setting)] [Space (10)] [Toggle] _CutoffEnable ("Cutoff Enable", Float) = 0
    [Header (Area Mask Setting)] [Space (10)] [Toggle] _CharacterAreaMaskOffEnable ("Character Area Mask Off Enable", Float) = 0
    [HideInInspector] _DissolveColor ("Death Color", Color) = (0, 0, 0, 1)
    [HideInInspector] _DissolveProgress ("Death Progress", Range(0, 1)) = 0
    [HideInInspector] [NoScaleOffset] [Normal] [Header (Normalmap Setting)] [Space (10)] _NormalTex ("NormalTex", 2D) = "bump" {}
    [HideInInspector] _OpticalRatioYThreshold ("Optical Ratio Y Threshold", Float) = 0
    [HideInInspector] _OpticalRatioYPositiveRatio ("Optical Ratio Y Positive Ratio", Float) = 0.10000000149011612
    [HideInInspector] _OpticalRatioYNegativeUnit ("Optical Ratio Y Negative Unit", Float) = 1.5
}
SubShader
{
    LOD 100
    Tags { "IgnoreProjector" = "true" "Queue" = "Transparent" "RenderPipeline" = "UniversalPipeline" "RenderType" = "Transparent" }
    Pass
    {
        Name "ForwardLit"
        Tags { "IgnoreProjector" = "true" "LightMode" = "CustomAdditive" "Queue" = "Transparent" "RenderPipeline" = "UniversalPipeline" "RenderType" = "Transparent" }
        Blend SrcAlpha One
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
        float _GlobalBackgroundSeeThroughEnable;
        float _GlobalBoxAreaEnable;
        float4 _GlobalBoxAreaCenters[8];
        float4 _GlobalBoxAreaExtents[8];
        float _GlobalBoxAreaCoverCharacters[8];
        float3 _PlaneBoundMinPos;
        float3 _PlaneBoundMaxPos;
        float _GlobalPlaneSystemEnable;
        float _GlobalPlanePointBufferEnable;
        float _GlobalSeeThroughCircleAlpha;
        float _GlobalSeeThroughCircleRange;
        cbuffer UnityPerDraw
        {
            float4x4 unity_ObjectToWorld : packoffset(c0);
            float4x4 unity_WorldToObject : packoffset(c4);
        }
        cbuffer UnityPerMaterial
        {
            float4 _MainTex_ST : packoffset(c0);
            float4 _NormalTex_ST : packoffset(c1);
            float _SightEffectEnable : packoffset(c10.y);
            float _CharacterAreaMaskOffEnable : packoffset(c10.z);
            float4 _DissolveColor : packoffset(c11);
            float _DissolveProgress : packoffset(c12);
            float _BillboardEnable : packoffset(c12.z);
            float _AdditiveIntensity : packoffset(c17);
            float _AdditivePower : packoffset(c17.y);
            float _OpticalRatioYThreshold : packoffset(c17.z);
            float _OpticalRatioYPositiveRatio : packoffset(c17.w);
            float _OpticalRatioYNegativeUnit : packoffset(c18);
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
        #if 1
        #include "LCArt_BGAnim_shader_DE_BGAnim_Additive/p0_vs_1.hlsl"
        #endif
        // ---- progFragment
        #if 1
        #include "LCArt_BGAnim_shader_DE_BGAnim_Additive/p0_ps_2.hlsl"
        #endif
        ENDHLSL
    }
}
}
