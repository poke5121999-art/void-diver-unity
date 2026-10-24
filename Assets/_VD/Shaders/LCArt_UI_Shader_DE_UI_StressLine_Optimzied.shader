Shader "LCArt/UI/Shader_DE_UI_StressLine_Optimzied"
{
Properties
{
    _Intensity ("Intensity", Range(0, 1)) = 0
    _HitIntensity ("HitIntensity", Range(0, 1)) = 0
    [ToggleUI] _Blackout ("Blackout", Float) = 0
    _Padding ("Padding", Float) = 0.5
    _Glow ("Glow", Float) = 0.009999999776482582
    _Stroke ("Stroke", Float) = 0
    _SpeedHigh ("SpeedHigh", Float) = 1
    _SpeedLow ("SpeedLow", Float) = 1
    _ScaleHigh ("ScaleHigh", Float) = 0.5
    _ScaleLow ("ScaleLow", Float) = 0
    _UnitHigh ("UnitHigh", Float) = 80
    _UnitLow ("UnitLow", Float) = 50
    _StartColor ("StartColor", Color) = (1, 1, 1, 1)
    _EndColor ("EndColor", Color) = (1, 0, 0, 1)
    _BlackoutColor ("BlackoutColor", Color) = (1, 0.7028301954269409, 0.7028301954269409, 1)
    _HitPadding ("HitPadding", Float) = 0.75
    _HitScale ("HitScale", Float) = 5
    _HitSpeed ("HitSpeed", Float) = 0.5
    [HideInInspector] [PerRendererData] _MainTex ("Main Texture", 2D) = "white" {}
    [HideInInspector] _QueueOffset ("_QueueOffset", Float) = 0
    [HideInInspector] _QueueControl ("_QueueControl", Float) = -1
}
SubShader
{
    Tags { "DisableBatching" = "False" "Queue" = "Transparent" "RenderPipeline" = "UniversalPipeline" "RenderType" = "Transparent" "UniversalMaterialType" = "Unlit" }
    Pass
    {
        Name "Universal Forward"
        Tags { "DisableBatching" = "False" "Queue" = "Transparent" "RenderPipeline" = "UniversalPipeline" "RenderType" = "Transparent" "UniversalMaterialType" = "Unlit" }
        Blend SrcAlpha OneMinusSrcAlpha, One OneMinusSrcAlpha
        ZWrite Off
        HLSLPROGRAM
        #pragma target 4.0
        #pragma vertex vert
        #pragma fragment frag
        #pragma multi_compile _ FOG_LINEAR

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
        float4 _Time;
        float4x4 unity_MatrixVP;
        cbuffer UnityPerDraw
        {
            float4x4 unity_ObjectToWorld : packoffset(c0);
        }
        cbuffer UnityPerMaterial
        {
            float _HitPadding : packoffset(c0);
            float _UnitLow : packoffset(c0.y);
            float _ScaleLow : packoffset(c0.z);
            float _UnitHigh : packoffset(c0.w);
            float _Stroke : packoffset(c1);
            float _ScaleHigh : packoffset(c1.y);
            float _Intensity : packoffset(c1.z);
            float _Padding : packoffset(c1.w);
            float _SpeedLow : packoffset(c2);
            float4 _StartColor : packoffset(c3);
            float4 _EndColor : packoffset(c4);
            float _SpeedHigh : packoffset(c5);
            float _Glow : packoffset(c5.y);
            float _HitIntensity : packoffset(c5.z);
            float _HitScale : packoffset(c5.w);
            float _HitSpeed : packoffset(c6);
            float _Blackout : packoffset(c6.y);
            float4 _BlackoutColor : packoffset(c7);
        }


        // ---- progVertex
        #if defined(FOG_LINEAR)
        #include "LCArt_UI_Shader_DE_UI_StressLine_Optimzied/p0_vs_2.hlsl"
        #elif 1
        #include "LCArt_UI_Shader_DE_UI_StressLine_Optimzied/p0_vs_1.hlsl"
        #endif
        // ---- progFragment
        #if defined(FOG_LINEAR)
        #include "LCArt_UI_Shader_DE_UI_StressLine_Optimzied/p0_ps_5.hlsl"
        #elif 1
        #include "LCArt_UI_Shader_DE_UI_StressLine_Optimzied/p0_ps_4.hlsl"
        #endif
        ENDHLSL
    }
}
}
