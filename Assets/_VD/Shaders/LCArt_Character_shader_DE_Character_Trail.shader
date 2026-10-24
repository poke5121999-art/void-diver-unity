Shader "LCArt/Character/shader_DE_Character_Trail"
{
Properties
{
    [MainTexture] [Header (Common Setting)] [Space (10)] _MainTex ("Texture", 2D) = "white" {}
    [HideInInspector] [HDR] [Header (In Game Hit Effect Setting)] [Space (10)] _EffectColor ("(타격, 상태이상 용!) Effect Color", Color) = (1, 1, 1, 1)
    [HideInInspector] _Effect ("(타격, 상태이상 용!) Effect", Range(0, 1)) = 0
    [HideInInspector] [Header (Vertex Color Setting)] [Space (10)] [Toggle] _IsVertexColorAlphaUsed ("Is Vertex Color Alpha Used", Float) = 1
    [HideInInspector] [Header (Ghost Effect Setting)] [Space (10)] [Toggle] _GhostEnable ("Ghost Enable", Float) = 0
    [HideInInspector] [Toggle] _IsLocalGhost ("Is Local Ghost", Float) = 1
    [HideInInspector] [NoScaleOffset] [Normal] [Header (Normalmap Setting)] [Space (10)] _NormalTex ("Normal Texture", 2D) = "bump" {}
    [HideInInspector] [Header (Optical Numbers)] _OpticalRatioYThreshold ("Optical Ratio Y Threshold", Float) = 0
    [HideInInspector] _OpticalRatioYPositiveRatio ("Optical Ratio Y Negative Unit", Float) = 1
    [HideInInspector] _OpticalRatioYNegativeUnit ("Optical Ratio Y Negative Unit", Float) = 1.5
}
SubShader
{
    LOD 100
    Tags { "Queue" = "AlphaTest-10" "RenderPipeline" = "UniversalPipeline" }
    Pass
    {
        Name "ForwardLit"
        Tags { "IgnoreProjector" = "true" "LightMode" = "UniversalForward" "Queue" = "AlphaTest-10" "RenderPipeline" = "UniversalPipeline" "RenderType" = "Opaque" }
        ZWrite Off
        Cull Off
        HLSLPROGRAM
        #pragma target 4.0
        #pragma vertex vert
        #pragma fragment frag

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
        float4 _MainLightPosition;
        float4 _MainLightColor;
        int _MainLightLayerMask;
        float3 _WorldSpaceCameraPos;
        float4 _ProjectionParams;
        float4 _ScreenParams;
        float4 unity_FogColor;
        float4x4 unity_MatrixV;
        float4x4 unity_MatrixVP;
        float4 _ScreenSize;
        float _GlobalFogHeightEnable;
        float _GlobalFogHeightMin;
        float _GlobalFogHeightMax;
        float _GlobalIsRuntime;
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
            float _Effect : packoffset(c9);
            float _IsVertexColorAlphaUsed : packoffset(c11.y);
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
        #if 1
        #include "LCArt_Character_shader_DE_Character_Trail/p0_vs_1.hlsl"
        #endif
        // ---- progFragment
        #if 1
        #include "LCArt_Character_shader_DE_Character_Trail/p0_ps_2.hlsl"
        #endif
        ENDHLSL
    }
}
}
