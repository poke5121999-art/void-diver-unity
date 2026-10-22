Shader "LCArt/Indicator/Shader_DE_Indicator_Box"
{
Properties
{
    [Header (Shader Texture Setting)] _MainTex ("Main Texture", 2D) = "white" {}
    _WeightTex ("Weight Texture", 2D) = "white" {}
    [HDR] [Header (Shader Color Setting)] _FillColor ("Fill Color", Color) = (0.550000011920929, 0.15000000596046448, 0.20000000298023224, 1)
    [HDR] _LineColor ("Line Color", Color) = (0.75, 0.10000000149011612, 0.05000000074505806, 1)
    [Header (Shader Thickness Setting)] _LineThickness ("Line Thickness (World)", Float) = 0.10000000149011612
    [Header (Shader In Game Setting)] _Progress ("Progress", Range(0, 2)) = 0.5
    _FillRatio ("Fill Ratio (Progress 1 시 외곽 도달 비율)", Range(0, 2)) = 0.949999988079071
    [Header (Shader Alpha Fade Setting)] _AlphaFadeWorld ("Alpha Fade Width (World, per edge)", Float) = 0.30000001192092896
    [Header (Occlusion Fade)] _OccludedAlpha ("Occluded Min Alpha", Range(0, 1)) = 0.30000001192092896
    _OcclusionFadeStart ("Fade Start Distance (m)", Range(0, 5)) = 0.05000000074505806
    _OcclusionFadeEnd ("Fade End Distance (m)", Range(0, 10)) = 1
    [Header (Shader Mesh Setting)] _MeshHalfLengthX ("Mesh Half Length X (Object-space)", Float) = 0.5
    _MeshCenterX ("Mesh Center X (Object-space)", Float) = 0
    [Header (Shader Fog Of War Setting)] [Toggle] _IsHiddenByPlaneSystem ("Is Hidden By Plane System (PlaneSight / PlaneLightBuffer)", Float) = 1
    [HideInInspector] _LineScaling ("Line Scaling (MPB, World 너비)", Float) = 1
    [HideInInspector] _LengthScaling ("Length Scaling (MPB, World 길이)", Float) = 1
    [HideInInspector] _GabDistance ("Gab Distance (MPB, World)", Float) = 0
}
SubShader
{
    LOD 100
    Tags { "Queue" = "Transparent" "RenderType" = "Transparent" }
    Pass
    {
        Tags { "Queue" = "Transparent" "RenderType" = "Transparent" }
        Blend SrcAlpha OneMinusSrcAlpha
        ZTest Always
        ZWrite Off
        Cull Off
        HLSLPROGRAM
        #pragma target 4.0
        #pragma vertex vert
        #pragma fragment frag
        #pragma multi_compile _ _ADDITIONAL_LIGHTS
        #pragma multi_compile _ _CLUSTER_LIGHT_LOOP
        #pragma multi_compile _ _DE_EDITOR_CAMERA

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
            float _LineThickness : packoffset(c4);
            float _Progress : packoffset(c4.y);
            float _FillRatio : packoffset(c4.z);
            float _AlphaFadeWorld : packoffset(c4.w);
            float _MeshHalfLengthX : packoffset(c5);
            float _OccludedAlpha : packoffset(c5.z);
            float _OcclusionFadeStart : packoffset(c5.w);
            float _OcclusionFadeEnd : packoffset(c6);
            float _IsHiddenByPlaneSystem : packoffset(c6.y);
            float _LineScaling : packoffset(c6.z);
            float _LengthScaling : packoffset(c6.w);
        }

        Texture2D<float4> _CameraDepthTexture;
        Texture2D<float4> _CustomDepthBuffer;
        Texture2D<float4> _MainTex;
        Texture2D<float4> _PlanePointBuffer;
        Texture2D<float4> _PlaneSightRenderTarget;
        Texture2D<float4> _WeightTex;
        SamplerState sampler_CameraDepthTexture;
        SamplerState sampler_CustomDepthBuffer;
        SamplerState sampler_MainTex;
        SamplerState sampler_PlanePointBuffer;
        SamplerState sampler_PlaneSightRenderTarget;
        SamplerState sampler_WeightTex;

        // ---- progVertex
        #if defined(_ADDITIONAL_LIGHTS) && defined(_DE_EDITOR_CAMERA)
        #include "LCArt_Indicator_Shader_DE_Indicator_Box/p0_vs_7.hlsl"
        #elif defined(_CLUSTER_LIGHT_LOOP) && defined(_DE_EDITOR_CAMERA)
        #include "LCArt_Indicator_Shader_DE_Indicator_Box/p0_vs_5.hlsl"
        #elif defined(_ADDITIONAL_LIGHTS)
        #include "LCArt_Indicator_Shader_DE_Indicator_Box/p0_vs_6.hlsl"
        #elif defined(_CLUSTER_LIGHT_LOOP)
        #include "LCArt_Indicator_Shader_DE_Indicator_Box/p0_vs_4.hlsl"
        #elif defined(_DE_EDITOR_CAMERA)
        #include "LCArt_Indicator_Shader_DE_Indicator_Box/p0_vs_3.hlsl"
        #elif 1
        #include "LCArt_Indicator_Shader_DE_Indicator_Box/p0_vs_2.hlsl"
        #endif
        // ---- progFragment
        #if defined(_ADDITIONAL_LIGHTS) && defined(_DE_EDITOR_CAMERA)
        #include "LCArt_Indicator_Shader_DE_Indicator_Box/p0_ps_17.hlsl"
        #elif defined(_CLUSTER_LIGHT_LOOP) && defined(_DE_EDITOR_CAMERA)
        #include "LCArt_Indicator_Shader_DE_Indicator_Box/p0_ps_15.hlsl"
        #elif defined(_ADDITIONAL_LIGHTS)
        #include "LCArt_Indicator_Shader_DE_Indicator_Box/p0_ps_16.hlsl"
        #elif defined(_CLUSTER_LIGHT_LOOP)
        #include "LCArt_Indicator_Shader_DE_Indicator_Box/p0_ps_14.hlsl"
        #elif defined(_DE_EDITOR_CAMERA)
        #include "LCArt_Indicator_Shader_DE_Indicator_Box/p0_ps_13.hlsl"
        #elif 1
        #include "LCArt_Indicator_Shader_DE_Indicator_Box/p0_ps_12.hlsl"
        #endif
        ENDHLSL
    }
}
}
