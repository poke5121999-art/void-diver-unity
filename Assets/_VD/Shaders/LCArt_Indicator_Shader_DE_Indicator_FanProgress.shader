Shader "LCArt/Indicator/Shader_DE_Indicator_FanProgress"
{
Properties
{
    [Header (Texture)] _MainTex ("Main Texture", 2D) = "white" {}
    _WeightTex ("Cone Weight Texture (R: Mask, G: 부채꼴 경계 선 패턴, B: 영향 받지 않는 영역)", 2D) = "white" {}
    [HDR] [Header (Color)] _FillColor ("Fill Color (Progress)", Color) = (1, 0.30000001192092896, 0.30000001192092896, 1)
    [HDR] _LineColor ("Line Color (Progress)", Color) = (1, 0.6000000238418579, 0.20000000298023224, 1)
    [HDR] _StrokeColor ("Stroke Color (Cone Outline)", Color) = (1, 0.6000000238418579, 0.20000000298023224, 1)
    [Header (Line)] _LineThickness ("Line Thickness (World)", Float) = 0.10000000149011612
    [Header (Progress)] _Progress ("Progress (0~2)", Range(0, 2)) = 0
    _ProgressCorrection ("Progress Correction", Float) = 2
    _FillRatio ("Fill Ratio (Progress 1 시 외곽 도달 비율)", Range(0, 2)) = 0.949999988079071
    [Header (Cone)] _ConeAngle ("Cone Angle (deg)", Range(0, 360)) = 360
    [Header (Occlusion Fade)] _OccludedAlpha ("Occluded Min Alpha", Range(0, 1)) = 0.30000001192092896
    _OcclusionFadeStart ("Fade Start Distance (m)", Range(0, 5)) = 0.05000000074505806
    _OcclusionFadeEnd ("Fade End Distance (m)", Range(0, 10)) = 1
    [Header (Fog of War)] [Toggle] _IsHiddenByPlaneSystem ("Hidden By Plane System (PlaneSight / PlaneLightBuffer)", Float) = 1
    [HideInInspector] _LineScaling ("Line Scaling (MPB)", Float) = 1
    [HideInInspector] _MeshRadius ("Mesh Radius (MPB)", Float) = 1
    [HideInInspector] _GabDistance ("Gab Distance (MPB)", Float) = 0.10000000149011612
}
SubShader
{
    LOD 100
    Tags { "Queue" = "Transparent" "RenderPipeline" = "UniversalPipeline" "RenderType" = "Transparent" }
    Pass
    {
        Name "ForwardUnlit"
        Tags { "Queue" = "Transparent" "RenderPipeline" = "UniversalPipeline" "RenderType" = "Transparent" }
        Blend SrcAlpha OneMinusSrcAlpha
        ZTest Always
        ZWrite Off
        Cull Off
        HLSLPROGRAM
        #pragma target 4.0
        #pragma vertex vert
        #pragma fragment frag
        #pragma multi_compile _ _DE_EDITOR_CAMERA

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
        float4x4 unity_MatrixVP;
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
            float4 _FillColor : packoffset(c2);
            float4 _LineColor : packoffset(c3);
            float4 _StrokeColor : packoffset(c4);
            float _LineThickness : packoffset(c5);
            float _Progress : packoffset(c5.y);
            float _FillRatio : packoffset(c5.z);
            float _ConeAngle : packoffset(c5.w);
            float _OccludedAlpha : packoffset(c6);
            float _OcclusionFadeStart : packoffset(c6.y);
            float _OcclusionFadeEnd : packoffset(c6.z);
            float _IsHiddenByPlaneSystem : packoffset(c6.w);
            float _LineScaling : packoffset(c7);
            float _MeshRadius : packoffset(c7.y);
        }

        Texture2D<float4> _CustomDepthBuffer;
        Texture2D<float4> _MainTex;
        Texture2D<float4> _PlanePointBuffer;
        Texture2D<float4> _PlaneSightRenderTarget;
        Texture2D<float4> _WeightTex;
        SamplerState sampler_CustomDepthBuffer;
        SamplerState sampler_MainTex;
        SamplerState sampler_PlanePointBuffer;
        SamplerState sampler_PlaneSightRenderTarget;
        SamplerState sampler_WeightTex;

        // ---- progVertex
        #if defined(_DE_EDITOR_CAMERA)
        #include "LCArt_Indicator_Shader_DE_Indicator_FanProgress/p0_vs_2.hlsl"
        #elif 1
        #include "LCArt_Indicator_Shader_DE_Indicator_FanProgress/p0_vs_1.hlsl"
        #endif
        // ---- progFragment
        #if defined(_DE_EDITOR_CAMERA)
        #include "LCArt_Indicator_Shader_DE_Indicator_FanProgress/p0_ps_5.hlsl"
        #elif 1
        #include "LCArt_Indicator_Shader_DE_Indicator_FanProgress/p0_ps_4.hlsl"
        #endif
        ENDHLSL
    }
}
}
