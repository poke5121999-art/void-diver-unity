Shader "Shader Graphs/Shader_Decoration_Puddle"
{
Properties
{
    [NoScaleOffset] _MainTex ("MainTex", 2D) = "white" {}
    _TintColor ("TintColor", Color) = (0, 0.04952685162425041, 0.4716981053352356, 1)
    _EdgeColor ("EdgeColor", Color) = (1, 1, 1, 1)
    _Intensity ("Intensity", Range(0, 1)) = 0
    _Opacity ("Opacity", Range(0, 1)) = 0.5
    [HideInInspector] _DrawOrder ("Draw Order", Range(-50, 50)) = 0
    [HideInInspector] [Enum(Default, 0, Depth Bias, 0, View Bias, 1)] _DecalMeshBiasType ("DecalMesh BiasType", Float) = 0
    [HideInInspector] _DecalMeshDepthBias ("DecalMesh DepthBias", Float) = 0
    [HideInInspector] _DecalMeshViewBias ("DecalMesh ViewBias", Float) = 0
    [HideInInspector] [NoScaleOffset] unity_Lightmaps ("unity_Lightmaps", 2DArray) = "" {}
    [HideInInspector] [NoScaleOffset] unity_LightmapsInd ("unity_LightmapsInd", 2DArray) = "" {}
    [HideInInspector] [NoScaleOffset] unity_ShadowMasks ("unity_ShadowMasks", 2DArray) = "" {}
}
SubShader
{
    Tags { "DisableBatching" = "False" "PreviewType" = "Plane" "RenderPipeline" = "UniversalPipeline" "ShaderGraphShader" = "true" "ShaderGraphTargetId" = "UniversalDecalSubTarget" }
    Pass
    {
        Name "DBufferProjector"
        Tags { "DisableBatching" = "False" "LightMode" = "DBufferProjector" "PreviewType" = "Plane" "RenderPipeline" = "UniversalPipeline" "ShaderGraphShader" = "true" "ShaderGraphTargetId" = "UniversalDecalSubTarget" }
        Blend 0 SrcAlpha OneMinusSrcAlpha, Zero OneMinusSrcAlpha
        Blend 1 SrcAlpha OneMinusSrcAlpha, Zero OneMinusSrcAlpha
        ColorMask 1 0
        Blend 2 SrcAlpha OneMinusSrcAlpha, Zero OneMinusSrcAlpha
        ColorMask 2 0
        ZTest Greater
        ZWrite Off
        Cull Front
        HLSLPROGRAM
        #pragma target 5.0
        #pragma vertex vert
        #pragma fragment frag
        #pragma multi_compile _ FOG_LINEAR
        #pragma multi_compile _DBUFFER_MRT3
        #pragma multi_compile _ _DE_EDITOR_CAMERA
        #pragma multi_compile _ _DE_WEATHER_RAIN

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
        float4 _TimeParameters;
        float4 _ProjectionParams;
        float4 _ScreenParams;
        float4x4 unity_MatrixVP;
        float4x4 unity_MatrixInvVP;
        float4 _ScreenSize;
        float _PlaneReflectionActive;
        float _PlaneReflectionOpacity;
        float4 _PlaneReflectionBackgroundColor;
        cbuffer Decal
        {
            float4x4 _NormalToWorld : packoffset(c0);
        }
        cbuffer UnityPerDraw
        {
            float4x4 unity_ObjectToWorld : packoffset(c0);
            float4x4 unity_WorldToObject : packoffset(c4);
        }
        cbuffer UnityPerMaterial
        {
            float4 _EdgeColor : packoffset(c1);
            float4 _TintColor : packoffset(c2);
            float _Opacity : packoffset(c3);
            float _Intensity : packoffset(c3.y);
        }

        Texture2D<float4> _CameraDepthTexture;
        Texture2D<float4> _MainTex;
        Texture2D<float4> _PlaneReflectionTexture;
        SamplerState sampler_MainTex;
        SamplerState sampler_PlaneReflectionTexture;

        // ---- progVertex
        #if defined(FOG_LINEAR) && defined(_DE_EDITOR_CAMERA) && defined(_DE_WEATHER_RAIN)
        #include "Shader_Graphs_Shader_Decoration_Puddle/p0_vs_8.hlsl"
        #elif defined(FOG_LINEAR) && defined(_DE_EDITOR_CAMERA)
        #include "Shader_Graphs_Shader_Decoration_Puddle/p0_vs_4.hlsl"
        #elif defined(FOG_LINEAR) && defined(_DE_WEATHER_RAIN)
        #include "Shader_Graphs_Shader_Decoration_Puddle/p0_vs_6.hlsl"
        #elif defined(_DE_EDITOR_CAMERA) && defined(_DE_WEATHER_RAIN)
        #include "Shader_Graphs_Shader_Decoration_Puddle/p0_vs_7.hlsl"
        #elif defined(FOG_LINEAR)
        #include "Shader_Graphs_Shader_Decoration_Puddle/p0_vs_2.hlsl"
        #elif defined(_DE_EDITOR_CAMERA)
        #include "Shader_Graphs_Shader_Decoration_Puddle/p0_vs_3.hlsl"
        #elif defined(_DE_WEATHER_RAIN)
        #include "Shader_Graphs_Shader_Decoration_Puddle/p0_vs_5.hlsl"
        #elif 1
        #include "Shader_Graphs_Shader_Decoration_Puddle/p0_vs_1.hlsl"
        #endif
        // ---- progFragment
        #if defined(FOG_LINEAR) && defined(_DBUFFER_MRT3) && defined(_DE_EDITOR_CAMERA) && defined(_DE_WEATHER_RAIN)
        #include "Shader_Graphs_Shader_Decoration_Puddle/p0_ps_11.hlsl"
        #elif defined(FOG_LINEAR) && defined(_DBUFFER_MRT3) && defined(_DE_EDITOR_CAMERA)
        #include "Shader_Graphs_Shader_Decoration_Puddle/p0_ps_15.hlsl"
        #elif defined(FOG_LINEAR) && defined(_DBUFFER_MRT3) && defined(_DE_WEATHER_RAIN)
        #include "Shader_Graphs_Shader_Decoration_Puddle/p0_ps_13.hlsl"
        #elif defined(_DBUFFER_MRT3) && defined(_DE_EDITOR_CAMERA) && defined(_DE_WEATHER_RAIN)
        #include "Shader_Graphs_Shader_Decoration_Puddle/p0_ps_12.hlsl"
        #elif defined(FOG_LINEAR) && defined(_DBUFFER_MRT3)
        #include "Shader_Graphs_Shader_Decoration_Puddle/p0_ps_17.hlsl"
        #elif defined(_DBUFFER_MRT3) && defined(_DE_EDITOR_CAMERA)
        #include "Shader_Graphs_Shader_Decoration_Puddle/p0_ps_16.hlsl"
        #elif defined(_DBUFFER_MRT3) && defined(_DE_WEATHER_RAIN)
        #include "Shader_Graphs_Shader_Decoration_Puddle/p0_ps_14.hlsl"
        #elif defined(_DBUFFER_MRT3)
        #include "Shader_Graphs_Shader_Decoration_Puddle/p0_ps_18.hlsl"
        #else
        #include "Shader_Graphs_Shader_Decoration_Puddle/p0_ps_18.hlsl"
        #endif
        ENDHLSL
    }
    Pass
    {
        Name "DBufferMesh"
        Tags { "DisableBatching" = "False" "LightMode" = "DBufferMesh" "PreviewType" = "Plane" "RenderPipeline" = "UniversalPipeline" "ShaderGraphShader" = "true" "ShaderGraphTargetId" = "UniversalDecalSubTarget" }
        Blend 0 SrcAlpha OneMinusSrcAlpha, Zero OneMinusSrcAlpha
        Blend 1 SrcAlpha OneMinusSrcAlpha, Zero OneMinusSrcAlpha
        ColorMask 1 0
        Blend 2 SrcAlpha OneMinusSrcAlpha, Zero OneMinusSrcAlpha
        ColorMask 2 0
        ZWrite Off
        HLSLPROGRAM
        #pragma target 5.0
        #pragma vertex vert
        #pragma fragment frag
        #pragma multi_compile _ FOG_LINEAR
        #pragma multi_compile _DBUFFER_MRT3
        #pragma multi_compile _ _DE_EDITOR_CAMERA
        #pragma multi_compile _ _DE_WEATHER_RAIN

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
        float4 _TimeParameters;
        float3 _WorldSpaceCameraPos;
        float4 _ProjectionParams;
        float4 _ScreenParams;
        float4 unity_OrthoParams;
        float4x4 unity_MatrixV;
        float4x4 unity_MatrixVP;
        float _PlaneReflectionActive;
        float _PlaneReflectionOpacity;
        float4 _PlaneReflectionBackgroundColor;
        cbuffer UnityPerDraw
        {
            float4x4 unity_ObjectToWorld : packoffset(c0);
            float4x4 unity_WorldToObject : packoffset(c4);
        }
        cbuffer UnityPerMaterial
        {
            float4 _EdgeColor : packoffset(c1);
            float4 _TintColor : packoffset(c2);
            float _Opacity : packoffset(c3);
            float _Intensity : packoffset(c3.y);
            float _DecalMeshBiasType : packoffset(c3.w);
            float _DecalMeshDepthBias : packoffset(c4);
            float _DecalMeshViewBias : packoffset(c4.y);
        }

        Texture2D<float4> _MainTex;
        Texture2D<float4> _PlaneReflectionTexture;
        SamplerState sampler_MainTex;
        SamplerState sampler_PlaneReflectionTexture;

        // ---- progVertex
        #if defined(FOG_LINEAR) && defined(_DE_EDITOR_CAMERA) && defined(_DE_WEATHER_RAIN)
        #include "Shader_Graphs_Shader_Decoration_Puddle/p1_vs_27.hlsl"
        #elif defined(FOG_LINEAR) && defined(_DE_EDITOR_CAMERA)
        #include "Shader_Graphs_Shader_Decoration_Puddle/p1_vs_23.hlsl"
        #elif defined(FOG_LINEAR) && defined(_DE_WEATHER_RAIN)
        #include "Shader_Graphs_Shader_Decoration_Puddle/p1_vs_25.hlsl"
        #elif defined(_DE_EDITOR_CAMERA) && defined(_DE_WEATHER_RAIN)
        #include "Shader_Graphs_Shader_Decoration_Puddle/p1_vs_26.hlsl"
        #elif defined(FOG_LINEAR)
        #include "Shader_Graphs_Shader_Decoration_Puddle/p1_vs_21.hlsl"
        #elif defined(_DE_EDITOR_CAMERA)
        #include "Shader_Graphs_Shader_Decoration_Puddle/p1_vs_22.hlsl"
        #elif defined(_DE_WEATHER_RAIN)
        #include "Shader_Graphs_Shader_Decoration_Puddle/p1_vs_24.hlsl"
        #elif 1
        #include "Shader_Graphs_Shader_Decoration_Puddle/p1_vs_20.hlsl"
        #endif
        // ---- progFragment
        #if defined(FOG_LINEAR) && defined(_DBUFFER_MRT3) && defined(_DE_EDITOR_CAMERA) && defined(_DE_WEATHER_RAIN)
        #include "Shader_Graphs_Shader_Decoration_Puddle/p1_ps_30.hlsl"
        #elif defined(FOG_LINEAR) && defined(_DBUFFER_MRT3) && defined(_DE_EDITOR_CAMERA)
        #include "Shader_Graphs_Shader_Decoration_Puddle/p1_ps_34.hlsl"
        #elif defined(FOG_LINEAR) && defined(_DBUFFER_MRT3) && defined(_DE_WEATHER_RAIN)
        #include "Shader_Graphs_Shader_Decoration_Puddle/p1_ps_32.hlsl"
        #elif defined(_DBUFFER_MRT3) && defined(_DE_EDITOR_CAMERA) && defined(_DE_WEATHER_RAIN)
        #include "Shader_Graphs_Shader_Decoration_Puddle/p1_ps_31.hlsl"
        #elif defined(FOG_LINEAR) && defined(_DBUFFER_MRT3)
        #include "Shader_Graphs_Shader_Decoration_Puddle/p1_ps_36.hlsl"
        #elif defined(_DBUFFER_MRT3) && defined(_DE_EDITOR_CAMERA)
        #include "Shader_Graphs_Shader_Decoration_Puddle/p1_ps_35.hlsl"
        #elif defined(_DBUFFER_MRT3) && defined(_DE_WEATHER_RAIN)
        #include "Shader_Graphs_Shader_Decoration_Puddle/p1_ps_33.hlsl"
        #elif defined(_DBUFFER_MRT3)
        #include "Shader_Graphs_Shader_Decoration_Puddle/p1_ps_37.hlsl"
        #else
        #include "Shader_Graphs_Shader_Decoration_Puddle/p1_ps_37.hlsl"
        #endif
        ENDHLSL
    }
}
Fallback "Hidden/Shader Graph/FallbackError"
CustomEditor "UnityEditor.ShaderGraph.GenericShaderGraphMaterialGUI"
}
