Shader "Shader Graphs/shader_DE_BG_ReflectionSurface_Floor"
{
Properties
{
    _Opacity ("Opacity", Float) = 0.5
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
        Blend 2 SrcAlpha OneMinusSrcAlpha, Zero OneMinusSrcAlpha
        ColorMask 0 2
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
        float4 _ScreenParams;
        float4 unity_FogParams;
        float4 unity_FogColor;
        float4x4 unity_MatrixV;
        float4x4 unity_MatrixVP;
        float4x4 unity_MatrixInvVP;
        float4 _ScreenSize;
        float _PlaneReflectionActive;
        float _GlobalBackgroundColorIntensity;
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
            float _Opacity : packoffset(c0);
        }

        Texture2D<float4> _CameraDepthTexture;
        Texture2D<float4> _PlaneReflectionTexture;
        SamplerState sampler_PlaneReflectionTexture;

        // ---- progVertex
        #if defined(FOG_LINEAR) && defined(_DE_EDITOR_CAMERA)
        #include "Shader_Graphs_shader_DE_BG_ReflectionSurface_Floor/p0_vs_4.hlsl"
        #elif defined(FOG_LINEAR)
        #include "Shader_Graphs_shader_DE_BG_ReflectionSurface_Floor/p0_vs_2.hlsl"
        #elif defined(_DE_EDITOR_CAMERA)
        #include "Shader_Graphs_shader_DE_BG_ReflectionSurface_Floor/p0_vs_3.hlsl"
        #elif 1
        #include "Shader_Graphs_shader_DE_BG_ReflectionSurface_Floor/p0_vs_1.hlsl"
        #endif
        // ---- progFragment
        #if defined(FOG_LINEAR) && defined(_DBUFFER_MRT3) && defined(_DE_EDITOR_CAMERA)
        #include "Shader_Graphs_shader_DE_BG_ReflectionSurface_Floor/p0_ps_9.hlsl"
        #elif defined(FOG_LINEAR) && defined(_DBUFFER_MRT3)
        #include "Shader_Graphs_shader_DE_BG_ReflectionSurface_Floor/p0_ps_11.hlsl"
        #elif defined(_DBUFFER_MRT3) && defined(_DE_EDITOR_CAMERA)
        #include "Shader_Graphs_shader_DE_BG_ReflectionSurface_Floor/p0_ps_10.hlsl"
        #elif defined(_DBUFFER_MRT3)
        #include "Shader_Graphs_shader_DE_BG_ReflectionSurface_Floor/p0_ps_12.hlsl"
        #else
        #include "Shader_Graphs_shader_DE_BG_ReflectionSurface_Floor/p0_ps_12.hlsl"
        #endif
        ENDHLSL
    }
    Pass
    {
        Name "DecalProjectorForwardEmissive"
        Tags { "DisableBatching" = "False" "LightMode" = "DecalProjectorForwardEmissive" "PreviewType" = "Plane" "RenderPipeline" = "UniversalPipeline" "ShaderGraphShader" = "true" "ShaderGraphTargetId" = "UniversalDecalSubTarget" }
        Blend 0 SrcAlpha One
        ZTest Greater
        ZWrite Off
        Cull Front
        HLSLPROGRAM
        #pragma target 5.0
        #pragma vertex vert
        #pragma fragment frag
        #pragma multi_compile _ FOG_LINEAR
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
        float4 _ScreenParams;
        float4 unity_FogParams;
        float4 unity_FogColor;
        float4x4 unity_MatrixV;
        float4x4 unity_MatrixVP;
        float4x4 unity_MatrixInvVP;
        float4 _ScreenSize;
        float _PlaneReflectionActive;
        float _GlobalBackgroundColorIntensity;
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
            float _Opacity : packoffset(c0);
        }

        Texture2D<float4> _CameraDepthTexture;
        Texture2D<float4> _PlaneReflectionTexture;
        SamplerState sampler_PlaneReflectionTexture;

        // ---- progVertex
        #if defined(FOG_LINEAR) && defined(_DE_EDITOR_CAMERA)
        #include "Shader_Graphs_shader_DE_BG_ReflectionSurface_Floor/p1_vs_4.hlsl"
        #elif defined(FOG_LINEAR)
        #include "Shader_Graphs_shader_DE_BG_ReflectionSurface_Floor/p1_vs_2.hlsl"
        #elif defined(_DE_EDITOR_CAMERA)
        #include "Shader_Graphs_shader_DE_BG_ReflectionSurface_Floor/p1_vs_3.hlsl"
        #elif 1
        #include "Shader_Graphs_shader_DE_BG_ReflectionSurface_Floor/p1_vs_1.hlsl"
        #endif
        // ---- progFragment
        #if defined(FOG_LINEAR) && defined(_DE_EDITOR_CAMERA)
        #include "Shader_Graphs_shader_DE_BG_ReflectionSurface_Floor/p1_ps_16.hlsl"
        #elif defined(FOG_LINEAR)
        #include "Shader_Graphs_shader_DE_BG_ReflectionSurface_Floor/p1_ps_14.hlsl"
        #elif defined(_DE_EDITOR_CAMERA)
        #include "Shader_Graphs_shader_DE_BG_ReflectionSurface_Floor/p1_ps_15.hlsl"
        #elif 1
        #include "Shader_Graphs_shader_DE_BG_ReflectionSurface_Floor/p1_ps_13.hlsl"
        #endif
        ENDHLSL
    }
    Pass
    {
        Name "DBufferMesh"
        Tags { "DisableBatching" = "False" "LightMode" = "DBufferMesh" "PreviewType" = "Plane" "RenderPipeline" = "UniversalPipeline" "ShaderGraphShader" = "true" "ShaderGraphTargetId" = "UniversalDecalSubTarget" }
        Blend 0 SrcAlpha OneMinusSrcAlpha, Zero OneMinusSrcAlpha
        Blend 1 SrcAlpha OneMinusSrcAlpha, Zero OneMinusSrcAlpha
        Blend 2 SrcAlpha OneMinusSrcAlpha, Zero OneMinusSrcAlpha
        ColorMask 0 2
        ZWrite Off
        HLSLPROGRAM
        #pragma target 5.0
        #pragma vertex vert
        #pragma fragment frag
        #pragma multi_compile _ FOG_LINEAR
        #pragma multi_compile _DBUFFER_MRT3
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
        float3 _WorldSpaceCameraPos;
        float4 _ProjectionParams;
        float4 _ScreenParams;
        float4 unity_OrthoParams;
        float4 unity_FogParams;
        float4 unity_FogColor;
        float4x4 unity_MatrixV;
        float4x4 unity_MatrixVP;
        float _PlaneReflectionActive;
        float _GlobalBackgroundColorIntensity;
        cbuffer UnityPerDraw
        {
            float4x4 unity_ObjectToWorld : packoffset(c0);
            float4x4 unity_WorldToObject : packoffset(c4);
        }
        cbuffer UnityPerMaterial
        {
            float _Opacity : packoffset(c0);
            float _DecalMeshBiasType : packoffset(c0.z);
            float _DecalMeshDepthBias : packoffset(c0.w);
            float _DecalMeshViewBias : packoffset(c1);
        }

        Texture2D<float4> _PlaneReflectionTexture;
        SamplerState sampler_PlaneReflectionTexture;

        // ---- progVertex
        #if defined(FOG_LINEAR) && defined(_DE_EDITOR_CAMERA)
        #include "Shader_Graphs_shader_DE_BG_ReflectionSurface_Floor/p2_vs_21.hlsl"
        #elif defined(FOG_LINEAR)
        #include "Shader_Graphs_shader_DE_BG_ReflectionSurface_Floor/p2_vs_19.hlsl"
        #elif defined(_DE_EDITOR_CAMERA)
        #include "Shader_Graphs_shader_DE_BG_ReflectionSurface_Floor/p2_vs_20.hlsl"
        #elif 1
        #include "Shader_Graphs_shader_DE_BG_ReflectionSurface_Floor/p2_vs_18.hlsl"
        #endif
        // ---- progFragment
        #if defined(FOG_LINEAR) && defined(_DBUFFER_MRT3) && defined(_DE_EDITOR_CAMERA)
        #include "Shader_Graphs_shader_DE_BG_ReflectionSurface_Floor/p2_ps_26.hlsl"
        #elif defined(FOG_LINEAR) && defined(_DBUFFER_MRT3)
        #include "Shader_Graphs_shader_DE_BG_ReflectionSurface_Floor/p2_ps_28.hlsl"
        #elif defined(_DBUFFER_MRT3) && defined(_DE_EDITOR_CAMERA)
        #include "Shader_Graphs_shader_DE_BG_ReflectionSurface_Floor/p2_ps_27.hlsl"
        #elif defined(_DBUFFER_MRT3)
        #include "Shader_Graphs_shader_DE_BG_ReflectionSurface_Floor/p2_ps_29.hlsl"
        #else
        #include "Shader_Graphs_shader_DE_BG_ReflectionSurface_Floor/p2_ps_29.hlsl"
        #endif
        ENDHLSL
    }
    Pass
    {
        Name "DecalMeshForwardEmissive"
        Tags { "DisableBatching" = "False" "LightMode" = "DecalMeshForwardEmissive" "PreviewType" = "Plane" "RenderPipeline" = "UniversalPipeline" "ShaderGraphShader" = "true" "ShaderGraphTargetId" = "UniversalDecalSubTarget" }
        Blend 0 SrcAlpha One
        ZWrite Off
        HLSLPROGRAM
        #pragma target 5.0
        #pragma vertex vert
        #pragma fragment frag
        #pragma multi_compile _ FOG_LINEAR
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
        float3 _WorldSpaceCameraPos;
        float4 _ProjectionParams;
        float4 _ScreenParams;
        float4 unity_OrthoParams;
        float4 unity_FogParams;
        float4 unity_FogColor;
        float4x4 unity_MatrixV;
        float4x4 unity_MatrixVP;
        float _PlaneReflectionActive;
        float _GlobalBackgroundColorIntensity;
        cbuffer UnityPerDraw
        {
            float4x4 unity_ObjectToWorld : packoffset(c0);
            float4x4 unity_WorldToObject : packoffset(c4);
        }
        cbuffer UnityPerMaterial
        {
            float _Opacity : packoffset(c0);
            float _DecalMeshBiasType : packoffset(c0.z);
            float _DecalMeshDepthBias : packoffset(c0.w);
            float _DecalMeshViewBias : packoffset(c1);
        }

        Texture2D<float4> _PlaneReflectionTexture;
        SamplerState sampler_PlaneReflectionTexture;

        // ---- progVertex
        #if defined(FOG_LINEAR) && defined(_DE_EDITOR_CAMERA)
        #include "Shader_Graphs_shader_DE_BG_ReflectionSurface_Floor/p3_vs_21.hlsl"
        #elif defined(FOG_LINEAR)
        #include "Shader_Graphs_shader_DE_BG_ReflectionSurface_Floor/p3_vs_19.hlsl"
        #elif defined(_DE_EDITOR_CAMERA)
        #include "Shader_Graphs_shader_DE_BG_ReflectionSurface_Floor/p3_vs_20.hlsl"
        #elif 1
        #include "Shader_Graphs_shader_DE_BG_ReflectionSurface_Floor/p3_vs_18.hlsl"
        #endif
        // ---- progFragment
        #if defined(FOG_LINEAR) && defined(_DE_EDITOR_CAMERA)
        #include "Shader_Graphs_shader_DE_BG_ReflectionSurface_Floor/p3_ps_33.hlsl"
        #elif defined(FOG_LINEAR)
        #include "Shader_Graphs_shader_DE_BG_ReflectionSurface_Floor/p3_ps_31.hlsl"
        #elif defined(_DE_EDITOR_CAMERA)
        #include "Shader_Graphs_shader_DE_BG_ReflectionSurface_Floor/p3_ps_32.hlsl"
        #elif 1
        #include "Shader_Graphs_shader_DE_BG_ReflectionSurface_Floor/p3_ps_30.hlsl"
        #endif
        ENDHLSL
    }
}
Fallback "Hidden/Shader Graph/FallbackError"
CustomEditor "UnityEditor.ShaderGraph.GenericShaderGraphMaterialGUI"
}
