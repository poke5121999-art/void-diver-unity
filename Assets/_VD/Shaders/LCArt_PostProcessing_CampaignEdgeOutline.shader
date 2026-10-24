Shader "LCArt/PostProcessing/CampaignEdgeOutline"
{
Properties
{
    [HDR] [Header(Outline)] _OutlineColor ("Outline Color", Color) = (0, 0, 0, 1)
    _OutlineThickness ("Outline Thickness (px)", Range(0.5, 4)) = 1
    _OutlineOpacity ("Outline Opacity", Range(0, 1)) = 1
    [Header(Depth Edge)] _DepthSensitivity ("Depth Sensitivity", Range(0, 200)) = 40
    _DepthThreshold ("Depth Threshold", Range(0, 1)) = 0.019999999552965164
    _DepthEdgeSoftness ("Depth Edge Softness", Range(0.0010000000474974513, 1)) = 0.05000000074505806
    [Header(Normal Edge)] _NormalSensitivity ("Normal Sensitivity", Range(0, 16)) = 4
    _NormalThreshold ("Normal Threshold", Range(0, 1)) = 0.10000000149011612
    _NormalEdgeSoftness ("Normal Edge Softness", Range(0.0010000000474974513, 1)) = 0.10000000149011612
    [Header(Cavity)] [Toggle(_DE_CAVITY)] _UseCavity ("Enable Cavity", Float) = 1
    _CavityThickness ("Cavity Sample (px)", Range(0.5, 6)) = 1.5
    _CavityStrength ("Cavity Strength", Range(0, 80)) = 20
    [HDR] _RidgeColor ("Ridge Color (convex)", Color) = (1, 1, 1, 1)
    _RidgeIntensity ("Ridge Intensity", Range(0, 2)) = 0.6000000238418579
    [HDR] _ValleyColor ("Valley Color (concave)", Color) = (0, 0, 0, 1)
    _ValleyIntensity ("Valley Intensity", Range(0, 2)) = 0.800000011920929
    [Header(Operator)] [Toggle(_DE_EDGE_SOBEL)] _UseSobel ("Use Sobel (off = Roberts Cross)", Float) = 0
    [Header(Debug)] [Enum(Off,0,Edge,1,DepthEdge,2,NormalEdge,3,LinearDepth,4,ReconNormal,5,Cavity,6)] _DebugMode ("Debug View", Float) = 0
    _DebugGain ("Debug Raw Gain", Range(0.009999999776482582, 50)) = 1
}
SubShader
{
    Tags { "RenderPipeline" = "UniversalPipeline" "RenderType" = "Opaque" }
    Pass
    {
        Name "CampaignEdgeOutline"
        Tags { "RenderPipeline" = "UniversalPipeline" "RenderType" = "Opaque" }
        ZTest Always
        ZWrite Off
        Cull Off
        HLSLPROGRAM
        #pragma target 4.0
        #pragma vertex vert
        #pragma fragment frag
        #pragma multi_compile_local _ _DE_CAVITY
        #pragma multi_compile_local _ _DE_EDGE_SOBEL

        // The bytecode is D3D11, so it baked in D3D's screen conventions; lines that differ on OpenGL sit under
// UNITY_UV_STARTS_AT_TOP (uvflip.py). URP's API headers define it for every API but OpenGL Core / GLES3, the same set that
// has UNITY_NEAR_CLIP_VALUE 1.0 (reversed Z); these programs include none of them.
#if !defined(UNITY_UV_STARTS_AT_TOP) && !defined(SHADER_API_GLCORE) && !defined(SHADER_API_GLES3) && !defined(SHADER_API_GLES)
#define UNITY_UV_STARTS_AT_TOP 1
#endif
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
        float4 _ProjectionParams;
        float4 _ZBufferParams;
        float4 _RTHandleScale;
        float4x4 unity_MatrixInvVP;
        float4 _BlitScaleBias;
        float4 _BlitTexture_TexelSize;
        float4 _CameraDepthTexture_TexelSize;
        cbuffer UnityPerMaterial
        {
            float4 _OutlineColor : packoffset(c0);
            float _OutlineThickness : packoffset(c1);
            float _OutlineOpacity : packoffset(c1.y);
            float _DepthSensitivity : packoffset(c1.z);
            float _DepthThreshold : packoffset(c1.w);
            float _DepthEdgeSoftness : packoffset(c2);
            float _NormalSensitivity : packoffset(c2.y);
            float _NormalThreshold : packoffset(c2.z);
            float _NormalEdgeSoftness : packoffset(c2.w);
            float _CavityThickness : packoffset(c3);
            float _CavityStrength : packoffset(c3.y);
            float4 _RidgeColor : packoffset(c4);
            float _RidgeIntensity : packoffset(c5);
            float4 _ValleyColor : packoffset(c6);
            float _ValleyIntensity : packoffset(c7);
            float _DebugMode : packoffset(c7.y);
            float _DebugGain : packoffset(c7.z);
        }

        Texture2D<float4> _BlitTexture;
        Texture2D<float4> _CameraDepthTexture;
        SamplerState sampler_vd_linear_clamp;
        SamplerState sampler_vd_point_clamp;

        // ---- progVertex
        #if 1
        #include "LCArt_PostProcessing_CampaignEdgeOutline/p0_vs_1.hlsl"
        #endif
        // ---- progFragment
        #if defined(_DE_CAVITY) && defined(_DE_EDGE_SOBEL)
        #include "LCArt_PostProcessing_CampaignEdgeOutline/p0_ps_5.hlsl"
        #elif 1
        #include "LCArt_PostProcessing_CampaignEdgeOutline/p0_ps_4.hlsl"
        #endif
        ENDHLSL
    }
}
}
